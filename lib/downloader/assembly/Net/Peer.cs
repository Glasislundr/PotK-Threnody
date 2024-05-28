// Decompiled with JetBrains decompiler
// Type: Net.Peer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using GameCore.Serialization;
using System;
using System.Collections;
using System.IO;

#nullable disable
namespace Net
{
  public class Peer
  {
    private UnityTcpClient client;
    private int requestId;
    private const int ERROR_TYPE = 255;

    public static CrossSerializer MakeDefaultSerializer()
    {
      CrossSerializer crossSerializer = BinaryFormatter.MakeSerializer();
      Action<TypeInfo> modifyTypeInfo = (Action<TypeInfo>) null;
      modifyTypeInfo = (Action<TypeInfo>) (ti =>
      {
        if (ti.assembly.name == "GameCore")
          ti.assembly.name = "Assembly-CSharp";
        foreach (TypeInfo typeArgument in ti.type_arguments)
          modifyTypeInfo(typeArgument);
      });
      crossSerializer.TypeBinder = (Func<TypeInfo, System.Type>) (typeInfo =>
      {
        TypeInfo typeInfo1 = typeInfo.Clone();
        modifyTypeInfo(typeInfo1);
        return typeInfo1.Type;
      });
      return crossSerializer;
    }

    public Peer()
    {
      this.Serializer = Peer.MakeDefaultSerializer();
      this.client = new UnityTcpClient();
    }

    public CrossSerializer Serializer { get; set; }

    public Future<None> Connect(string hostname, int port) => this.client.Connect(hostname, port);

    public bool Connected => this.client.Connected;

    public void Close() => this.client.Close();

    private IEnumerator ReceiveE(
      int timeoutMilliseconds,
      Promise<Tuple<Peer.CommonHeader, byte[]>> promise)
    {
      this.client.ReceiveTimeout = timeoutMilliseconds;
      Peer.CommonHeader header = new Peer.CommonHeader();
      Future<None> headerF = header.Read(this.client);
      IEnumerator e = headerF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (headerF.Exception != null)
      {
        promise.Exception = headerF.Exception;
      }
      else
      {
        Future<UnityTcpClient.ReadResult> resultF = this.client.Read(header.Length);
        e = resultF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (resultF.Exception != null)
        {
          promise.Exception = resultF.Exception;
        }
        else
        {
          UnityTcpClient.ReadResult result = resultF.Result;
          if (header.Type == (int) byte.MaxValue)
            promise.Exception = (Exception) new UserException((AssocList<string, object>) this.Serializer.DeserializeFromMemory(result.Buffer));
          else
            promise.Result = Tuple.Create<Peer.CommonHeader, byte[]>(header, result.Buffer);
        }
      }
    }

    public Future<Tuple<Peer.CommonHeader, byte[]>> Receive(int timeoutMilliseconds = -1)
    {
      return new Future<Tuple<Peer.CommonHeader, byte[]>>((Func<Promise<Tuple<Peer.CommonHeader, byte[]>>, IEnumerator>) (promise => this.ReceiveE(timeoutMilliseconds, promise)));
    }

    private byte[] MakeBytes(int type, int requestId, object obj)
    {
      using (MemoryStream ms = new MemoryStream())
      {
        ms.Seek(8L, SeekOrigin.Begin);
        this.Serializer.Serialize(obj, (Stream) ms);
        Peer.CommonHeader commonHeader = new Peer.CommonHeader();
        commonHeader.Type = type;
        commonHeader.Length = (int) (ms.Position - 8L);
        commonHeader.RequestId = requestId;
        ms.Position = 0L;
        commonHeader.Write(ms);
        return ms.ToArray();
      }
    }

    private IEnumerator SendResponseE(
      int requestId,
      int type,
      AssocList<string, object> data,
      int timeout,
      Promise<int> promise)
    {
      byte[] buf = this.MakeBytes(type, requestId, (object) data);
      this.client.SendTimeout = timeout;
      IEnumerator e = this.client.Write(buf).Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      promise.Result = requestId;
    }

    private Future<int> SendResponse(
      int requestId,
      int type,
      AssocList<string, object> data,
      int timeout)
    {
      return new Future<int>((Func<Promise<int>, IEnumerator>) (promise => this.SendResponseE(requestId, type, data, timeout, promise)));
    }

    private IEnumerator SendE(
      int type,
      AssocList<string, object> data,
      int timeout,
      Promise<int> promise)
    {
      Peer peer1 = this;
      Peer peer2 = peer1;
      int requestId1 = peer1.requestId;
      int num = requestId1 + 1;
      peer2.requestId = num;
      int requestId = requestId1;
      byte[] buf = peer1.MakeBytes(type, requestId, (object) data);
      peer1.client.SendTimeout = timeout;
      IEnumerator e = peer1.client.Write(buf).Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      promise.Result = requestId;
    }

    public Future<int> Send(int type, AssocList<string, object> data, int timeout)
    {
      return new Future<int>((Func<Promise<int>, IEnumerator>) (promise => this.SendE(type, data, timeout, promise)));
    }

    public Future<int> SendError(string message, int timeout)
    {
      return new Future<int>((Func<Promise<int>, IEnumerator>) (promise => this.SendE((int) byte.MaxValue, new AssocList<string, object>()
      {
        [nameof (message)] = (object) message
      }, timeout, promise)));
    }

    public class CommonHeader
    {
      public int Type;
      public int Length;
      public int RequestId;
      public const int HEADER_BYTES = 8;

      public IEnumerator ReadE(UnityTcpClient client, Promise<None> promise)
      {
        Future<UnityTcpClient.ReadResult> resultF = client.Read(8);
        IEnumerator e = resultF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (resultF.Exception != null)
        {
          promise.Exception = resultF.Exception;
        }
        else
        {
          UnityTcpClient.ReadResult result = resultF.Result;
          int int32 = BitConverter.ToInt32(result.Buffer, 0);
          this.Type = int32 >> 24 & (int) byte.MaxValue;
          this.Length = int32 & 16777215;
          this.RequestId = BitConverter.ToInt32(result.Buffer, 4);
          promise.Result = None.Value;
        }
      }

      public Future<None> Read(UnityTcpClient client)
      {
        return new Future<None>((Func<Promise<None>, IEnumerator>) (promise => this.ReadE(client, promise)));
      }

      public void Write(MemoryStream ms)
      {
        int num = this.Type << 24 | this.Length;
        ms.Write(BitConverter.GetBytes(num), 0, 4);
        ms.Write(BitConverter.GetBytes(this.RequestId), 0, 4);
      }
    }
  }
}
