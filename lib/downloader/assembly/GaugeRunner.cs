// Decompiled with JetBrains decompiler
// Type: GaugeRunner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class GaugeRunner
{
  private static HashSet<string> seQueue = new HashSet<string>();
  private static int mSeChannel;
  private static bool mIsPlayingSE = false;
  public GameObject obj;
  public float before;
  public float after;
  public int loopNum;
  public int count;
  public bool isLow;
  public float duration = 0.6f;
  public bool isRunning;
  public Func<GameObject, int, IEnumerator> levelupCallback;
  public Action onFinishCallback;
  private bool skipDirty;

  public static void PlaySE()
  {
    if (GaugeRunner.mIsPlayingSE)
      return;
    GaugeRunner.mSeChannel = Singleton<NGSoundManager>.GetInstance().playSE("SE_1014", true);
    GaugeRunner.mIsPlayingSE = true;
  }

  public static void StopSE()
  {
    if (!GaugeRunner.mIsPlayingSE)
      return;
    Singleton<NGSoundManager>.GetInstance().stopSE(GaugeRunner.mSeChannel);
    GaugeRunner.mIsPlayingSE = false;
  }

  public static void PauseSE()
  {
    if (!GaugeRunner.mIsPlayingSE)
      return;
    Singleton<NGSoundManager>.GetInstance().pauseSE(GaugeRunner.mSeChannel);
  }

  public static void ResumeSE()
  {
    if (!GaugeRunner.mIsPlayingSE)
      return;
    Singleton<NGSoundManager>.GetInstance().resumeSE(GaugeRunner.mSeChannel);
  }

  public static void AddSEQueue(string seName) => GaugeRunner.seQueue.Add(seName);

  private static void playQueuedSE()
  {
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    foreach (string se in GaugeRunner.seQueue)
      instance.playSE(se);
    GaugeRunner.seQueue.Clear();
  }

  public static IEnumerator Run(GaugeRunner runner)
  {
    return GaugeRunner.Run(new GaugeRunner[1]{ runner });
  }

  public static IEnumerator Run(GaugeRunner[] runners)
  {
    if (runners.Length != 0)
    {
      List<IEnumerator> ee = ((IEnumerable<GaugeRunner>) runners).Select<GaugeRunner, IEnumerator>((Func<GaugeRunner, IEnumerator>) (x => x.Run())).ToList<IEnumerator>();
      List<IEnumerator> finishedList = new List<IEnumerator>();
      GaugeRunner.PlaySE();
      while (ee.Count > 0)
      {
        foreach (IEnumerator enumerator in ee)
        {
          if (!enumerator.MoveNext())
            finishedList.Add(enumerator);
        }
        foreach (IEnumerator enumerator in finishedList)
          ee.Remove(enumerator);
        finishedList.Clear();
        GaugeRunner.playQueuedSE();
        yield return (object) null;
      }
      ee.Clear();
      ee = (List<IEnumerator>) null;
      finishedList.Clear();
      finishedList = (List<IEnumerator>) null;
      yield return (object) new WaitForSeconds(0.1f);
      GaugeRunner.StopSE();
    }
  }

  public GaugeRunner(
    GameObject obj,
    float after,
    int loopNum,
    Func<GameObject, int, IEnumerator> levelupCallback = null,
    bool isLow = false,
    float duration = 0.6f)
  {
    this.obj = obj;
    this.before = this.from();
    this.after = after;
    this.loopNum = loopNum;
    this.levelupCallback = levelupCallback;
    this.isLow = isLow;
    this.duration = duration;
    this.count = 0;
    Object.Destroy((Object) obj.GetComponent<TweenScale>());
  }

  public GaugeRunner(
    GameObject obj,
    float before,
    float after,
    int loopNum,
    Func<GameObject, int, IEnumerator> levelupCallback = null,
    bool isLow = false,
    float duration = 0.6f)
  {
    this.obj = obj;
    this.before = float.IsNaN(before) ? 0.0f : before;
    this.after = float.IsNaN(after) ? 0.0f : after;
    this.loopNum = loopNum;
    this.levelupCallback = levelupCallback;
    this.isLow = isLow;
    this.duration = duration;
    this.count = 0;
    obj.transform.localScale = new Vector3()
    {
      x = this.before,
      y = 1f
    };
  }

  public bool IsLast() => this.count >= this.loopNum - 1;

  public void Skip() => this.skipDirty = true;

  private IEnumerator Run()
  {
    this.isRunning = true;
    this.skipDirty = false;
    List<IEnumerator> runners = new List<IEnumerator>();
    if (this.loopNum == 0)
    {
      runners.Add(this.Run(this.before, this.after));
    }
    else
    {
      float from = this.isLow ? 1f : 0.0f;
      float to = this.isLow ? 0.0f : 1f;
      runners.Add(this.Run(this.before, to));
      if (this.levelupCallback != null)
        runners.Add(this.levelupCallback(this.obj, 0));
      runners.Add(this.AddCount());
      for (int index = 1; index < this.loopNum; ++index)
      {
        runners.Add(this.Run(from, to));
        if (this.levelupCallback != null)
          runners.Add(this.levelupCallback(this.obj, index));
        runners.Add(this.AddCount());
      }
      runners.Add(this.Run(from, this.after));
    }
    bool didSkip = false;
    IEnumerator e;
    foreach (IEnumerator enumerator in runners)
    {
      e = enumerator;
      if (this.skipDirty)
      {
        didSkip = true;
        break;
      }
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    runners.Clear();
    if (didSkip)
    {
      Vector3 localScale = this.obj.transform.localScale;
      this.obj.transform.localScale = new Vector3(this.after, localScale.y, localScale.z);
      if (!this.IsLast() && this.levelupCallback != null)
      {
        e = this.levelupCallback(this.obj, this.loopNum - 1);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      this.count = this.loopNum - 1;
    }
    if (this.onFinishCallback != null)
      this.onFinishCallback();
    this.isRunning = false;
  }

  private float from() => this.obj.transform.localScale.x;

  private float to()
  {
    TweenScale component = this.obj.GetComponent<TweenScale>();
    return Object.op_Equality((Object) component, (Object) null) ? 0.0f : component.to.x;
  }

  private IEnumerator Run(float from, float to)
  {
    TweenScale tween = this.obj.AddComponent<TweenScale>();
    TweenScale tweenScale1 = tween;
    Vector3 vector3_1 = new Vector3();
    vector3_1.x = from;
    vector3_1.y = 1f;
    Vector3 vector3_2 = vector3_1;
    tweenScale1.from = vector3_2;
    TweenScale tweenScale2 = tween;
    vector3_1 = new Vector3();
    vector3_1.x = to;
    vector3_1.y = 1f;
    Vector3 vector3_3 = vector3_1;
    tweenScale2.to = vector3_3;
    ((UITweener) tween).delay = 0.0f;
    ((UITweener) tween).duration = Mathf.Abs(tween.to.x - tween.from.x) * this.duration;
    ((UITweener) tween).PlayForward();
    while (((Behaviour) tween).enabled)
    {
      if (this.skipDirty)
      {
        ((Behaviour) tween).enabled = false;
        break;
      }
      yield return (object) null;
    }
    Object.Destroy((Object) tween);
  }

  private IEnumerator AddCount()
  {
    ++this.count;
    yield break;
  }
}
