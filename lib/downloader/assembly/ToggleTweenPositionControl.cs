// Decompiled with JetBrains decompiler
// Type: ToggleTweenPositionControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("NGFramework/UI/ToggleTweenPositionControl")]
public class ToggleTweenPositionControl : MonoBehaviour
{
  [SerializeField]
  [Tooltip("トグルアニメーション用")]
  private TweenPosition tween_;
  [SerializeField]
  [Tooltip("トグルアニメーションの順方向での表現がOFF->ON")]
  private bool forwardOFF_ON_ = true;
  [SerializeField]
  private bool isSwitch_;
  [SerializeField]
  [Tooltip("0:OFF/1:ON 表示切替")]
  private GameObject[] objSwitches_;
  private Func<bool> checkCancel_;
  private bool isToggleControl_;
  private bool initialize_ = true;
  private bool isPlaying_;
  private const int INDEX_OFF = 0;
  private const int INDEX_ON = 1;

  public bool isSwitch => this.isSwitch_;

  private void Awake()
  {
    this.isToggleControl_ = this.objSwitches_ != null && this.objSwitches_.Length == 2;
    if (this.isToggleControl_)
      return;
    ((Behaviour) this.tween_).enabled = false;
    EventDelegate.Set(((UITweener) this.tween_).onFinished, new EventDelegate.Callback(this.onAnimeFinished));
  }

  private void Start()
  {
    if (!this.initialize_)
      return;
    this.resetSwitch(this.isSwitch_);
  }

  public void setCheckCancel(Func<bool> checkCancel) => this.checkCancel_ = checkCancel;

  public void resetSwitch(bool flag)
  {
    this.isSwitch_ = flag;
    if (this.isToggleControl_)
    {
      ((IEnumerable<GameObject>) this.objSwitches_).ToggleOnce(this.isSwitch_ ? 1 : 0);
    }
    else
    {
      Vector3 vector3_1;
      Vector3 vector3_2;
      float num;
      if (this.forwardOFF_ON_)
      {
        vector3_1 = this.tween_.to;
        vector3_2 = this.tween_.from;
        num = flag ? 1f : 0.0f;
      }
      else
      {
        vector3_1 = this.tween_.from;
        vector3_2 = this.tween_.to;
        num = flag ? 0.0f : 1f;
      }
      this.tween_.value = flag ? vector3_1 : vector3_2;
      ((UITweener) this.tween_).tweenFactor = num;
    }
    this.initialize_ = false;
    this.isPlaying_ = false;
  }

  public void setSwitch(bool flag)
  {
    if (this.isSwitch_ == flag)
      return;
    this.isSwitch_ = flag;
    if (this.isToggleControl_)
    {
      ((IEnumerable<GameObject>) this.objSwitches_).ToggleOnce(this.isSwitch_ ? 1 : 0);
    }
    else
    {
      this.isPlaying_ = true;
      NGTween.playTween((UITweener) this.tween_, flag ? !this.forwardOFF_ON_ : this.forwardOFF_ON_);
    }
  }

  private void onAnimeFinished() => this.isPlaying_ = false;

  public void onClickedToggle()
  {
    if (this.isPlaying_ || this.checkCancel_ != null && this.checkCancel_())
      return;
    this.setSwitch(!this.isSwitch_);
  }
}
