// Decompiled with JetBrains decompiler
// Type: Gsc.Tasks.ParallelWorker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace Gsc.Tasks
{
  public class ParallelWorker : MonoBehaviour
  {
    private List<IEnumerator> tasks = new List<IEnumerator>();

    public int TaskCount => this.tasks.Count;

    public void AddTask(ITask task) => this.AddTask(ParallelWorker._AddTask(task));

    public void AddTask(IEnumerator task) => this.tasks.Add(task);

    private static IEnumerator _AddTask(ITask task)
    {
      task.OnStart();
      yield return (object) task.Run();
      task.OnFinish();
    }

    private void Start() => this.StartCoroutine(this.Run());

    private IEnumerator Run()
    {
      while (true)
      {
        yield return (object) null;
        this.tasks = this.tasks.Where<IEnumerator>((Func<IEnumerator, bool>) (x => x.MoveNext())).ToList<IEnumerator>();
      }
    }
  }
}
