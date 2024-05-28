// Decompiled with JetBrains decompiler
// Type: TweenController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("Utility/Tween/TweenController")]
public class TweenController : MonoBehaviour
{
  public bool includeTweenChildren = true;
  public bool autoSetOnTweenFinished;
  private UITweener[] tweens_;
  private bool? isActive_;
  private bool isRunning_;
  private bool isDestroyed_;

  public bool isActive
  {
    get
    {
      return !this.isActive_.HasValue ? (this.isActive_ = new bool?(((Component) this).gameObject.activeSelf)).Value : this.isActive_.Value;
    }
    set
    {
      if (this.isActive == value)
        return;
      this.forceActive(value);
    }
  }

  public void resetActive(bool v)
  {
    if (((Component) this).gameObject.activeSelf != v)
      ((Component) this).gameObject.SetActive(v);
    this.isActive_ = new bool?(v);
  }

  public void forceActive(bool v)
  {
    this.isActive_ = new bool?(v);
    this.isRunning_ = true;
    ((Component) this).gameObject.SetActive(true);
    this.playTweens(v);
  }

  private void playTweens(bool v)
  {
    if (this.tweens_ == null)
    {
      this.tweens_ = NGTween.findTweeners(((Component) this).gameObject, this.includeTweenChildren);
      if (this.autoSetOnTweenFinished)
        NGTween.setOnTweenFinished(this.tweens_, (MonoBehaviour) this);
    }
    bool flag1 = NGTween.playTweensNoRestart(this.tweens_, NGTween.Kind.START_END, !v);
    bool isTweensError = NGTween.isTweensError;
    bool flag2;
    bool flag3;
    if (v)
    {
      flag2 = flag1 | NGTween.playTweensNoRestart(this.tweens_, NGTween.Kind.START) | flag1;
      flag3 = isTweensError | NGTween.isTweensError;
    }
    else
    {
      flag2 = flag1 | NGTween.playTweensNoRestart(this.tweens_, NGTween.Kind.END) | flag1;
      flag3 = isTweensError | NGTween.isTweensError;
    }
    if (!flag2)
    {
      if (this.autoSetOnTweenFinished)
        this.onTweenFinished();
      else if (!this.isActive_.Value)
        ((Component) this).gameObject.SetActive(false);
      this.isRunning_ = false;
    }
    if (!flag3)
      return;
    this.tweens_ = (UITweener[]) null;
  }

  public void onTweenFinished()
  {
    if (this.isDestroyed_)
      return;
    this.isRunning_ = false;
    ((Component) this).gameObject.SetActive(this.isActive_.Value);
  }

  private void OnDestroy() => this.isDestroyed_ = true;
}
