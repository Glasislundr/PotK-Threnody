// Decompiled with JetBrains decompiler
// Type: CorpsQuestEntryPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CorpsQuestEntryPopup : BackButtonMenuBase
{
  [SerializeField]
  private UILabel[] Messages = new UILabel[2];
  [SerializeField]
  private UIButton[] BottomBtns = new UIButton[2];

  public CorpsQuestEntryPopup.Select Selected { get; private set; }

  public void InitializeEntry()
  {
    ((Component) this.Messages[0]).gameObject.SetActive(true);
    ((Component) this.Messages[1]).gameObject.SetActive(false);
    ((Component) this.BottomBtns[0]).gameObject.SetActive(true);
    ((Component) this.BottomBtns[1]).gameObject.SetActive(false);
  }

  public void InitializeRetry()
  {
    ((Component) this.Messages[0]).gameObject.SetActive(false);
    ((Component) this.Messages[1]).gameObject.SetActive(true);
    ((Component) this.BottomBtns[0]).gameObject.SetActive(false);
    ((Component) this.BottomBtns[1]).gameObject.SetActive(true);
  }

  public void onAutoButton()
  {
    if (this.IsPushAndSet())
      return;
    this.Selected = CorpsQuestEntryPopup.Select.Auto;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public void onManualButton()
  {
    if (this.IsPushAndSet())
      return;
    this.Selected = CorpsQuestEntryPopup.Select.Manual;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public void onYesButton()
  {
    if (this.IsPushAndSet())
      return;
    this.Selected = CorpsQuestEntryPopup.Select.Continue;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public void onCancelButton()
  {
    if (this.IsPushAndSet())
      return;
    this.Selected = CorpsQuestEntryPopup.Select.Cancel;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public override void onBackButton() => this.onCancelButton();

  public enum Select
  {
    None,
    Manual,
    Auto,
    Continue,
    Cancel,
  }
}
