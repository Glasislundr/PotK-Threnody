// Decompiled with JetBrains decompiler
// Type: CommonFooterMenuAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class CommonFooterMenuAnimation : MonoBehaviour
{
  [SerializeField]
  private Animator[] menuAnimator;
  [SerializeField]
  private TweenAlpha menuTweenAlpha;
  [SerializeField]
  private TweenPosition menuTweenPosition;
  [Header("inアニメのTrigger名")]
  [SerializeField]
  private string inTrigger;
  [Header("outアニメのTrigger名")]
  [SerializeField]
  private string outTrigger;
  [Header("TweenAlphaの設定")]
  [SerializeField]
  private CommonFooterMenuAnimation.tweenAnimaParam TweenAlphaData;
  [Header("TweenPositionの設定")]
  [SerializeField]
  private CommonFooterMenuAnimation.tweenAnimaParam TweenPositionData;

  public void OpenMenu()
  {
    ((Component) this).gameObject.SetActive(true);
    UIWidget component = ((Component) this).gameObject.GetComponent<UIWidget>();
    if (Object.op_Inequality((Object) this.menuTweenAlpha, (Object) null))
    {
      ((UITweener) this.menuTweenAlpha).onFinished.Clear();
      ((UITweener) this.menuTweenAlpha).tweenFactor = 0.0f;
      ((UITweener) this.menuTweenAlpha).delay = this.TweenAlphaData.inDelayTime;
      ((UITweener) this.menuTweenAlpha).duration = this.TweenAlphaData.inDurationTime;
      ((UIRect) component).alpha = this.menuTweenAlpha.from;
      ((UITweener) this.menuTweenAlpha).PlayForward();
    }
    if (!Object.op_Inequality((Object) this.menuTweenPosition, (Object) null))
      return;
    ((UITweener) this.menuTweenPosition).onFinished.Clear();
    ((UITweener) this.menuTweenPosition).tweenFactor = 0.0f;
    ((UITweener) this.menuTweenPosition).delay = this.TweenPositionData.inDelayTime;
    ((UITweener) this.menuTweenPosition).duration = this.TweenPositionData.inDurationTime;
    ((Component) this).gameObject.transform.localPosition = this.menuTweenPosition.from;
    ((UITweener) this.menuTweenPosition).PlayForward();
  }

  public void CloseMenu()
  {
    if (Object.op_Inequality((Object) this.menuTweenAlpha, (Object) null))
    {
      ((UITweener) this.menuTweenAlpha).tweenFactor = 1f;
      ((UITweener) this.menuTweenAlpha).delay = this.TweenAlphaData.outDelayTime;
      ((UITweener) this.menuTweenAlpha).duration = this.TweenAlphaData.outDurationTime;
      ((UITweener) this.menuTweenAlpha).PlayReverse();
      ((UITweener) this.menuTweenAlpha).AddOnFinished(new EventDelegate.Callback(this.CloseAnimeFinished));
    }
    if (Object.op_Inequality((Object) this.menuTweenPosition, (Object) null))
    {
      ((UITweener) this.menuTweenPosition).tweenFactor = 1f;
      ((UITweener) this.menuTweenPosition).delay = this.TweenPositionData.outDelayTime;
      ((UITweener) this.menuTweenPosition).duration = this.TweenPositionData.outDurationTime;
      ((UITweener) this.menuTweenPosition).PlayReverse();
      if (Object.op_Equality((Object) this.menuTweenAlpha, (Object) null))
        ((UITweener) this.menuTweenPosition).AddOnFinished(new EventDelegate.Callback(this.CloseAnimeFinished));
    }
    for (int index = 0; index < this.menuAnimator.Length; ++index)
      this.menuAnimator[index].SetTrigger(this.outTrigger);
  }

  public void CloseAnimeFinished()
  {
    if (Object.op_Inequality((Object) this.menuTweenAlpha, (Object) null))
    {
      ((UITweener) this.menuTweenAlpha).tweenFactor = 0.0f;
      ((UITweener) this.menuTweenAlpha).RemoveOnFinished(new EventDelegate(new EventDelegate.Callback(this.CloseAnimeFinished)));
      ((UITweener) this.menuTweenAlpha).ResetToBeginning();
    }
    if (Object.op_Inequality((Object) this.menuTweenPosition, (Object) null))
    {
      ((UITweener) this.menuTweenPosition).tweenFactor = 0.0f;
      if (Object.op_Equality((Object) this.menuTweenAlpha, (Object) null))
        ((UITweener) this.menuTweenPosition).RemoveOnFinished(new EventDelegate(new EventDelegate.Callback(this.CloseAnimeFinished)));
      ((UITweener) this.menuTweenPosition).ResetToBeginning();
    }
    for (int index = 0; index < this.menuAnimator.Length; ++index)
    {
      this.menuAnimator[index].ResetTrigger(this.outTrigger);
      this.menuAnimator[index].SetTrigger(this.inTrigger);
    }
    ((Component) this).gameObject.SetActive(false);
  }

  [Serializable]
  public class tweenAnimaParam
  {
    [Header("In時のStartDelay")]
    public float inDelayTime;
    [Header("Out時のStartDelay")]
    public float outDelayTime;
    [Header("In時のDuration")]
    public float inDurationTime;
    [Header("Out時のDuration")]
    public float outDurationTime;
  }
}
