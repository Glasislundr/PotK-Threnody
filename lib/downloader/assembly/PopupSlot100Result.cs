// Decompiled with JetBrains decompiler
// Type: PopupSlot100Result
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PopupSlot100Result : MonoBehaviour
{
  [SerializeField]
  public UIGrid grid;
  [SerializeField]
  public UIScrollView scrollView;
  [SerializeField]
  private TweenAlpha tweenAlpha;
  private SlotDebug slotScript;
  private Shop00720Menu shopMenuScript;
  private bool click;

  public void Initialized(SlotDebug Script, Shop00720Menu shopMenu)
  {
    this.slotScript = Script;
    this.shopMenuScript = shopMenu;
    this.click = false;
    ((UITweener) this.tweenAlpha).PlayForward();
    Singleton<NGSoundManager>.GetInstance().StopSe(time: 0.0f);
    Singleton<NGSoundManager>.GetInstance().playSE("SE_0506");
  }

  public void OnClick()
  {
    if (this.click)
      return;
    this.click = true;
    this.slotScript.SlotEnd();
    this.slotScript.SlotInit();
    this.shopMenuScript.Ready();
    ((UITweener) this.tweenAlpha).PlayReverse();
    EventDelegate.Set(((UITweener) this.tweenAlpha).onFinished, new EventDelegate.Callback(this.Close));
  }

  public void Close() => Singleton<PopupManager>.GetInstance().dismissWithoutAnim();
}
