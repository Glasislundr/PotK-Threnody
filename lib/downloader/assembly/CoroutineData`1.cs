// Decompiled with JetBrains decompiler
// Type: CoroutineData`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Reflection;
using System.Threading;
using UnityEngine;

#nullable disable
public class CoroutineData<T>
{
  private Coroutine m_Coroutine;
  private T m_Value;
  private Exception m_Exception;
  private volatile bool m_Running;
  private volatile bool m_Completed;
  private volatile bool m_Stopped;
  private volatile bool m_ShouldStop;
  private Thread m_Thread;
  private Object m_ValueLock = new Object();
  private Object m_ExceptionLock = new Object();

  public Coroutine Coroutine
  {
    get
    {
      lock (this.m_Coroutine)
        return this.m_Coroutine;
    }
  }

  public T Value
  {
    get
    {
      lock (this.m_ValueLock)
      {
        Exception exception = this.Exception;
        if (exception != null)
          throw exception;
        if (this.Stopped)
          throw new CoroutineStoppedException();
        return this.m_Value;
      }
    }
    protected set
    {
      lock (this.m_ValueLock)
        this.m_Value = value;
    }
  }

  public Exception Exception
  {
    get
    {
      lock (this.m_ExceptionLock)
        return this.m_Exception;
    }
    protected set
    {
      lock (this.m_ExceptionLock)
        this.m_Exception = value;
    }
  }

  public bool Running
  {
    get => this.m_Running;
    protected set => this.m_Running = value;
  }

  public bool Completed
  {
    get => this.m_Completed;
    protected set => this.m_Completed = value;
  }

  public bool Stopped
  {
    get => this.m_Stopped;
    protected set => this.m_Stopped = value;
  }

  public bool ShouldStop
  {
    get => this.m_ShouldStop;
    protected set => this.m_ShouldStop = value;
  }

  public bool RunningOnMainThread => this.m_Thread == null;

  private Thread Thread
  {
    get => this.m_Thread;
    set
    {
      if (this.m_Thread != null && value == null)
        this.m_Thread.Abort();
      this.m_Thread = value;
    }
  }

  private CoroutineData()
  {
    this.Running = true;
    this.Completed = false;
  }

  internal static CoroutineData<T> Start(MonoBehaviour behaviour, IEnumerator coroutine)
  {
    CoroutineData<T> coroutineData = new CoroutineData<T>();
    coroutineData.m_Coroutine = behaviour.StartCoroutine(coroutineData.Wrap(coroutine));
    return coroutineData;
  }

  internal static CoroutineData<T> Start(MonoBehaviour behaviour, MonitorCoroutine<T> coroutine)
  {
    CoroutineData<T> data = new CoroutineData<T>();
    data.m_Coroutine = behaviour.StartCoroutine(data.Wrap(coroutine(data)));
    return data;
  }

  private IEnumerator Wrap(IEnumerator coroutine)
  {
    CoroutineData<T> coroutineData = this;
    if (coroutine == null)
    {
      coroutineData.Exception = (Exception) new ArgumentException("Coroutine reference is null");
      coroutineData.Running = false;
    }
    else
    {
      while (!coroutineData.Stopped && coroutineData.Running)
      {
        if (coroutineData.Thread != null)
        {
          if (coroutineData.Thread.IsAlive)
          {
            yield return (object) null;
            continue;
          }
          coroutineData.Thread = (Thread) null;
        }
        try
        {
          if (!coroutine.MoveNext())
          {
            coroutineData.Running = false;
            yield break;
          }
        }
        catch (Exception ex)
        {
          coroutineData.Exception = ex;
          coroutineData.Running = false;
          yield break;
        }
        object current = coroutine.Current;
        switch (current)
        {
          case WaitForWorkerThread _:
            coroutineData.Thread = new Thread(new ParameterizedThreadStart(coroutineData.WorkerThread));
            coroutineData.Thread.Start((object) coroutine);
            yield return (object) null;
            continue;
          case WaitForMainThread _:
            Debug.LogWarning((object) "Received WaitForMainThread while already on main thread");
            continue;
          case T obj:
            coroutineData.Value = obj;
            coroutineData.Running = false;
            coroutineData.Completed = true;
            yield break;
          case WaitIfFrameTime _:
            if ((double) Time.realtimeSinceStartup - (double) Time.time > (double) ((WaitIfFrameTime) current).MaxFrameTime)
            {
              yield return (object) null;
              continue;
            }
            continue;
          default:
            yield return coroutine.Current;
            continue;
        }
      }
      coroutineData.Running = false;
      coroutineData.Thread = (Thread) null;
    }
  }

  private void WorkerThread(object coroutineObject)
  {
    if (!(coroutineObject is IEnumerator enumerator))
    {
      this.Exception = (Exception) new ArgumentException("Coroutine object passed to thread is null");
      this.Running = false;
    }
    else
    {
      while (!this.Stopped)
      {
        if (this.Running)
        {
          try
          {
            if (!enumerator.MoveNext())
            {
              this.Running = false;
              return;
            }
          }
          catch (Exception ex)
          {
            this.Exception = ex;
            this.Running = false;
            return;
          }
          object current = enumerator.Current;
          switch (current)
          {
            case null:
              Thread.Sleep(1);
              continue;
            case WaitForWorkerThread _:
              Debug.LogWarning((object) "Received WaitForWorkerThread while already on worker thread");
              continue;
            case WaitForMainThread _:
              return;
            case WaitForSeconds _:
              Thread.Sleep(TimeSpan.FromSeconds((double) (float) typeof (WaitForSeconds).GetField("m_Seconds", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(current)));
              continue;
            case T obj:
              this.Value = obj;
              this.Running = false;
              this.Completed = true;
              return;
            default:
              Debug.LogWarning((object) string.Format("Unsupported worker thread yield instruction: {0}", (object) current.GetType().Name));
              goto case null;
          }
        }
        else
          break;
      }
      this.Running = false;
    }
  }

  public void Stop() => this.Stopped = true;

  public void RequestStop() => this.ShouldStop = true;
}
