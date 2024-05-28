// Decompiled with JetBrains decompiler
// Type: BlinkSync
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Utility/Behaviour/BlinkSync")]
public class BlinkSync : MonoBehaviour
{
  [SerializeField]
  [Tooltip("同じ番号間で同タイミングを取る")]
  private int syncID_;
  [Header("演出期間調整(表示～切り替え)")]
  [SerializeField]
  [Tooltip("切り替え速度")]
  private AnimationCurve animationCurve_;
  [SerializeField]
  [Tooltip("表示期間(秒)")]
  private float wait_;
  [SerializeField]
  [Tooltip("切り替え期間(秒)")]
  private float duration_;
  [SerializeField]
  [Tooltip("制御対象")]
  private UIRect[] uiBlinks_;
  private bool isDestroyed_;
  private BlinkSync.SyncControl syncController_;
  private bool isInitialized_;
  private bool isTicksError_;
  private int ticksWait_;
  private int ticksDuration_;
  private float[] stackAlpha_;
  private static Dictionary<int, BlinkSync.ReferenceSyncControl> syncManager_;

  private BlinkSync.SyncControl syncController
  {
    get
    {
      if (this.syncController_ != null || this.isDestroyed_)
        return this.syncController_;
      this.firstInit();
      this.syncController_ = BlinkSync.getSyncControl(this.syncID_, this.animationCurve_);
      return this.syncController_;
    }
  }

  private void Awake() => this.firstInit();

  private void firstInit()
  {
    if (this.isInitialized_)
      return;
    int ticks = (int) new TimeSpan(0, 0, 1).Ticks;
    this.ticksWait_ = Mathf.CeilToInt((float) ticks * this.wait_);
    this.ticksDuration_ = Mathf.CeilToInt((float) ticks * this.duration_);
    this.isTicksError_ = this.ticksWait_ + this.ticksDuration_ <= 0;
    if (this.uiBlinks_ != null && this.uiBlinks_.Length != 0)
      this.stackAlpha();
    this.isInitialized_ = true;
  }

  private void stackAlpha()
  {
    if (this.uiBlinks_ == null || this.uiBlinks_.Length == 0)
      return;
    this.stackAlpha_ = new float[this.uiBlinks_.Length];
    for (int index = 0; index < this.uiBlinks_.Length; ++index)
      this.stackAlpha_[index] = this.uiBlinks_[index].alpha;
  }

  private void revertAlpha()
  {
    if (this.stackAlpha_ == null)
      return;
    for (int index = 0; index < this.stackAlpha_.Length; ++index)
      this.uiBlinks_[index].alpha = this.stackAlpha_[index];
    this.stackAlpha_ = (float[]) null;
  }

  private void Update() => this.updateBlinks();

  private void updateBlinks()
  {
    if (this.isTicksError_ || this.uiBlinks_ == null || this.uiBlinks_.Length == 0)
      ((Behaviour) this).enabled = false;
    else if (this.uiBlinks_.Length == 1)
    {
      this.uiBlinks_[0].alpha = 1f;
      ((Behaviour) this).enabled = false;
    }
    else
      this.syncController.updateBlink(this.uiBlinks_, this.ticksWait_, this.ticksDuration_);
  }

  private void OnDestroy()
  {
    if (this.isDestroyed_)
      return;
    if (this.syncController_ != null)
      BlinkSync.releaseSyncControl(this.syncID_);
    this.syncController_ = (BlinkSync.SyncControl) null;
    this.isDestroyed_ = true;
  }

  public void restartBlink()
  {
    if (!((Behaviour) this).enabled)
      return;
    this.syncController.restart();
    this.updateBlinks();
  }

  public void resetBlinks(IEnumerable<GameObject> blinks, bool bRestart = false)
  {
    if (blinks == null)
      this.resetBlinks(bRestart: bRestart);
    else
      this.resetBlinks(blinks.Select<GameObject, UIRect>((Func<GameObject, UIRect>) (x => x.GetComponent<UIRect>())).Where<UIRect>((Func<UIRect, bool>) (y => Object.op_Inequality((Object) y, (Object) null))).ToArray<UIRect>(), bRestart);
  }

  public void resetBlinks(UIRect[] blinks = null, bool bRestart = false)
  {
    if (this.stackAlpha_ != null)
      this.revertAlpha();
    this.uiBlinks_ = blinks == null || blinks.Length == 0 ? (UIRect[]) null : blinks;
    if (this.uiBlinks_ != null)
    {
      this.stackAlpha();
      ((Behaviour) this).enabled = true;
      if (bRestart)
        this.syncController.restart();
      this.updateBlinks();
    }
    else
      ((Behaviour) this).enabled = false;
  }

  private static BlinkSync.SyncControl getSyncControl(int id, AnimationCurve aCurve)
  {
    if (BlinkSync.syncManager_ == null)
      BlinkSync.syncManager_ = new Dictionary<int, BlinkSync.ReferenceSyncControl>();
    BlinkSync.ReferenceSyncControl referenceSyncControl;
    if (!BlinkSync.syncManager_.TryGetValue(id, out referenceSyncControl))
    {
      referenceSyncControl = new BlinkSync.ReferenceSyncControl(new BlinkSync.SyncControl(aCurve));
      BlinkSync.syncManager_.Add(id, referenceSyncControl);
    }
    ++referenceSyncControl.refCount_;
    return referenceSyncControl.control;
  }

  private static void releaseSyncControl(int id)
  {
    BlinkSync.ReferenceSyncControl referenceSyncControl;
    if (BlinkSync.syncManager_ == null || !BlinkSync.syncManager_.TryGetValue(id, out referenceSyncControl) || --referenceSyncControl.refCount_ != 0)
      return;
    BlinkSync.syncManager_.Remove(id);
    if (BlinkSync.syncManager_.Count != 0)
      return;
    BlinkSync.syncManager_ = (Dictionary<int, BlinkSync.ReferenceSyncControl>) null;
  }

  private class SyncControl
  {
    private int frameCount_;
    private DateTime start_;
    private long ticks_;
    private AnimationCurve animationCurve_;

    public SyncControl(AnimationCurve aCurve)
    {
      this.restart();
      this.animationCurve_ = aCurve;
    }

    public void restart()
    {
      int frameCount = Time.frameCount;
      if (this.frameCount_ == frameCount)
        return;
      this.frameCount_ = frameCount;
      this.start_ = DateTime.Now;
      this.ticks_ = 0L;
    }

    public long ticks
    {
      get
      {
        if (this.frameCount_ == Time.frameCount)
          return this.ticks_;
        this.ticks_ = (DateTime.Now - this.start_).Ticks;
        this.frameCount_ = Time.frameCount;
        return this.ticks_;
      }
    }

    public void updateBlink(UIRect[] rects, int nWait, int nDuration)
    {
      long num1 = (long) (nWait + nDuration);
      if (rects == null || rects.Length == 0 || num1 <= 0L)
        return;
      long num2 = this.ticks % (num1 * (long) rects.Length);
      int num3 = (int) (num2 / num1);
      int num4 = (int) (num2 % num1);
      if (num4 < nWait)
      {
        for (int index = 0; index < rects.Length; ++index)
          rects[index].alpha = index == num3 ? 1f : 0.0f;
      }
      else
      {
        int num5 = num4 - nWait;
        int num6 = num3 + 1;
        if (rects.Length <= num6)
          num6 = 0;
        float num7 = Mathf.Clamp01(this.animationCurve_.Evaluate((float) num5 / (float) nDuration));
        float num8 = 1f - num7;
        for (int index = 0; index < rects.Length; ++index)
          rects[index].alpha = index == num3 ? num8 : (index == num6 ? num7 : 0.0f);
      }
    }
  }

  private class ReferenceSyncControl
  {
    public int refCount_;

    public BlinkSync.SyncControl control { get; private set; }

    public ReferenceSyncControl(BlinkSync.SyncControl c)
    {
      this.refCount_ = 0;
      this.control = c;
    }
  }
}
