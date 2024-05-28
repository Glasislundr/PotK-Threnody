// Decompiled with JetBrains decompiler
// Type: Net.UnityTcpClient
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

#nullable disable
namespace Net
{
  public class UnityTcpClient : IDisposable
  {
    private TcpClient client;

    public UnityTcpClient()
    {
      this.client = new TcpClient();
      this.ConnectTimeout = -1;
      this.ReceiveTimeout = -1;
      this.SendTimeout = -1;
    }

    public void Close() => this.Dispose();

    public bool Connected => this.client != null && this.client.Connected;

    public int ConnectTimeout { get; set; }

    public int ReceiveTimeout { get; set; }

    public int SendTimeout { get; set; }

    private NetworkStream GetStreamByWorker()
    {
      lock (this)
        return this.client.GetStream();
    }

    private IEnumerator RunWithTimeout<T>(
      T result,
      int timeout,
      Promise<T> promise,
      Action<object, T> action,
      Action timeoutAction = null)
      where T : class
    {
      object sync = new object();
      bool isDone = false;
      Stopwatch stopwatch = timeout < 0 ? (Stopwatch) null : Stopwatch.StartNew();
      Exception exception = (Exception) null;
      Func<bool> getDone = (Func<bool>) (() =>
      {
        lock (sync)
          return isDone;
      });
      ThreadPool.QueueUserWorkItem((WaitCallback) (t =>
      {
        try
        {
          action(sync, result);
        }
        catch (Exception ex)
        {
          lock (sync)
            exception = ex;
        }
        finally
        {
          lock (sync)
            isDone = true;
        }
      }));
      while (!getDone())
      {
        if (stopwatch != null && stopwatch.ElapsedMilliseconds > (long) timeout)
        {
          if (timeoutAction != null)
            timeoutAction();
          lock (sync)
          {
            exception = (Exception) new IOException("timeout: timeout={0}".F((object) timeout));
            break;
          }
        }
        else
          yield return (object) null;
      }
      if (exception != null)
        promise.Exception = exception;
      else
        promise.Result = result;
    }

    public Future<None> Connect(string hostname, int port)
    {
      return new Future<None>((Func<Promise<None>, IEnumerator>) (promise => this.ConnectE(hostname, port, promise)));
    }

    private IEnumerator ConnectE(string hostname, int port, Promise<None> promise)
    {
      return this.RunWithTimeout<None>(None.Value, this.ConnectTimeout, promise, (Action<object, None>) ((sync, result) =>
      {
        try
        {
          this.client.Connect(hostname, port);
        }
        catch
        {
          this.Dispose();
          IPHostEntry hostEntry = Dns.GetHostEntry(hostname);
          if (((IEnumerable<IPAddress>) hostEntry.AddressList).Count<IPAddress>() == 0)
            throw new Exception(string.Format("Name Resolution Failed. hostname: {0}", (object) hostname));
          Dictionary<IPAddress, Exception> dictionary = new Dictionary<IPAddress, Exception>();
          bool flag = false;
          foreach (IPAddress address in hostEntry.AddressList)
          {
            try
            {
              this.client = address.AddressFamily == AddressFamily.InterNetwork || address.AddressFamily == AddressFamily.InterNetworkV6 ? new TcpClient(address.AddressFamily) : throw new Exception(string.Format("Not Support Address Family. Address Family: {0}", (object) address.AddressFamily.ToString()));
              this.client.Connect(address, port);
              flag = true;
              break;
            }
            catch (Exception ex)
            {
              dictionary[address] = ex;
              this.Dispose();
            }
          }
          if (!flag)
          {
            Exception innerException = (Exception) null;
            foreach (KeyValuePair<IPAddress, Exception> keyValuePair in dictionary)
            {
              IPAddress key = keyValuePair.Key;
              Exception exception = keyValuePair.Value;
              innerException = new Exception(string.Format("{0}**************************************************{0}Detail. Address: {1}, Family: {2}{0}{3}{0}", (object) Environment.NewLine, (object) key.ToString(), (object) key.AddressFamily.ToString(), (object) exception.ToString()), innerException);
            }
            throw new Exception(string.Format("Connect Failed!!! HostName: {0}, Port: {1}", (object) hostname, (object) port), innerException);
          }
        }
      }), (Action) (() => this.Dispose()));
    }

    public Future<UnityTcpClient.ReadResult> Read(int bytes)
    {
      return new Future<UnityTcpClient.ReadResult>((Func<Promise<UnityTcpClient.ReadResult>, IEnumerator>) (promise => this.ReadE(bytes, promise)));
    }

    private IEnumerator ReadE(int bytes, Promise<UnityTcpClient.ReadResult> promise)
    {
      return this.RunWithTimeout<UnityTcpClient.ReadResult>(new UnityTcpClient.ReadResult(), this.ReceiveTimeout, promise, (Action<object, UnityTcpClient.ReadResult>) ((sync, result) =>
      {
        byte[] buffer = new byte[bytes];
        NetworkStream streamByWorker = this.GetStreamByWorker();
        int offset = 0;
        while (offset != bytes)
          offset += streamByWorker.Read(buffer, offset, bytes - offset);
        lock (sync)
          result.Buffer = buffer;
      }));
    }

    public Future<None> Write(byte[] buf)
    {
      return new Future<None>((Func<Promise<None>, IEnumerator>) (promise => this.WriteE(buf, promise)));
    }

    private IEnumerator WriteE(byte[] buf, Promise<None> promise)
    {
      return this.RunWithTimeout<None>(None.Value, this.SendTimeout, promise, (Action<object, None>) ((sync, result) => this.GetStreamByWorker().Write(buf, 0, buf.Length)));
    }

    public void Dispose()
    {
      try
      {
        this.client?.GetStream()?.Close();
      }
      catch
      {
      }
      try
      {
        this.client?.Close();
      }
      catch
      {
      }
      this.client = (TcpClient) null;
    }

    public class ReadResult
    {
      public byte[] Buffer;
    }
  }
}
