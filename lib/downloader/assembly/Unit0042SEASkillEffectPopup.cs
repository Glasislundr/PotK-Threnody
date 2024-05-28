// Decompiled with JetBrains decompiler
// Type: Unit0042SEASkillEffectPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Unit0042SEASkillEffectPopup : MonoBehaviour
{
  [SerializeField]
  private TweenAlpha tweenAlpha;
  [SerializeField]
  private GameObject clearEffect;
  [SerializeField]
  private Animator animator;
  private bool tapButton;
  private TweenAlpha bgTweenAlpha;

  public void InitPopup(TweenAlpha bgAlpha) => this.bgTweenAlpha = bgAlpha;

  public void onOkButton()
  {
    if (this.tapButton)
      return;
    this.tapButton = true;
    this.tweenAlpha.value = 1f;
    ((UITweener) this.tweenAlpha).tweenFactor = 0.0f;
    ((UITweener) this.tweenAlpha).PlayForward();
    this.bgTweenAlpha.value = 1f;
    ((UITweener) this.bgTweenAlpha).tweenFactor = 0.0f;
    ((UITweener) this.bgTweenAlpha).delay = 0.2f;
    ((UITweener) this.bgTweenAlpha).PlayForward();
    ((Behaviour) this.animator).enabled = false;
    this.clearEffect.SetActive(false);
    ((UITweener) this.tweenAlpha).AddOnFinished(new EventDelegate.Callback(this.AnimationEnd));
  }

  public void TweenFinished()
  {
    ((UITweener) this.tweenAlpha).RemoveOnFinished(new EventDelegate(new EventDelegate.Callback(this.TweenFinished)));
    this.tweenAlpha.from = 1f;
    this.tweenAlpha.to = 0.0f;
    this.bgTweenAlpha.from = 1f;
    this.bgTweenAlpha.to = 0.0f;
  }

  public void AnimationEnd()
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    Singleton<NGSceneManager>.GetInstance().backScene();
  }
}
