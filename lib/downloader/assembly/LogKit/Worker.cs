// Decompiled with JetBrains decompiler
// Type: LogKit.Worker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

#nullable disable
namespace LogKit
{
  public class Worker : MonoBehaviour
  {
    private Thread workerThread;
    private List<Logger> loggers;
    private volatile bool destoried;

    public static void LaunchWorker(List<Logger> loggers, GameObject node = null)
    {
      if (Object.op_Equality((Object) node, (Object) null))
      {
        node = new GameObject("LogKit.Worker");
        Object.DontDestroyOnLoad((Object) node);
      }
      if (!Object.op_Equality((Object) node.GetComponent<Worker>(), (Object) null))
        return;
      node.AddComponent<Worker>().loggers = loggers;
    }

    private void Awake()
    {
      this.workerThread = new Thread(new ThreadStart(this.WorkingThreadStart));
    }

    private void OnDestroy() => this.destoried = true;

    private IEnumerator Start()
    {
      this.workerThread.Start();
      WaitForSeconds wait = new WaitForSeconds(60f);
label_1:
      yield return (object) wait;
      for (int index = 0; index < this.loggers.Count; ++index)
        this.loggers[index].Emit();
      goto label_1;
    }

    private void OnApplicationFocus(bool focusState)
    {
      if (focusState)
        return;
      for (int index = 0; index < this.loggers.Count; ++index)
      {
        Logger logger = this.loggers[index];
        logger.Emit();
        logger.Flush();
      }
    }

    private void WorkingThreadStart()
    {
      ushort num = 0;
      do
      {
        Logger logger = this.loggers[(int) num++ % this.loggers.Count];
        logger.Flush();
        logger.Send();
        Thread.Sleep(5000);
      }
      while (!this.destoried);
    }
  }
}
