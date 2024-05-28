// Decompiled with JetBrains decompiler
// Type: Unit0046ConfirmAutoOrganization
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit0046ConfirmAutoOrganization : BackButtonMenuBase
{
  [SerializeField]
  private UILabel txtCommon_;
  [SerializeField]
  private UILabel txtLimited_;
  [SerializeField]
  private UITexture[] selectElementButtons;
  private CommonElement selectElement = CommonElement.none;
  private Action<CommonElement> eventYes_;

  public static IEnumerator doPopup(string modeLimitedDescription, Action<CommonElement> eventYes)
  {
    bool isSea = Singleton<NGGameDataManager>.GetInstance().IsSea;
    string path = !string.IsNullOrEmpty(modeLimitedDescription) ? "Prefabs/popup/popup_002_quest_automatic_team_edit__anim_popup02" : (!isSea ? "Prefabs/popup/popup_002_quest_automatic_team_edit__anim_popup01" : "Prefabs/popup/popup_002_quest_automatic_team_edit_Sea__anim_popup01");
    Future<GameObject> prefab = Singleton<ResourceManager>.GetInstance().Load<GameObject>(path);
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(prefab.Result).GetComponent<Unit0046ConfirmAutoOrganization>().Initialize(modeLimitedDescription, eventYes);
    while (Singleton<PopupManager>.GetInstance().isOpen)
      yield return (object) null;
  }

  private void Initialize(string modeLimitedDescription, Action<CommonElement> eventYes)
  {
    if (string.IsNullOrEmpty(modeLimitedDescription))
    {
      this.txtCommon_.SetTextLocalize(Consts.GetInstance().UNIT_0046_CONFIRM_NORMAL_AUTODECK);
      modeLimitedDescription = Consts.GetInstance().UNIT_0046_CONFIRM_NORMAL_RULE_AUTODECK;
      this.SelectElementButtonsDisable(Unit0046ConfirmAutoOrganization.ButtonElement.all);
      this.selectElement = CommonElement.none;
    }
    else
      this.txtLimited_.SetTextLocalize(modeLimitedDescription);
    this.eventYes_ = eventYes;
  }

  private void SelectElementButtonsDisable(
    Unit0046ConfirmAutoOrganization.ButtonElement buttonElement)
  {
    for (int index = 0; index < this.selectElementButtons.Length; ++index)
    {
      if ((Unit0046ConfirmAutoOrganization.ButtonElement) index == buttonElement)
        ((UIWidget) this.selectElementButtons[index]).color = Color.white;
      else
        ((UIWidget) this.selectElementButtons[index]).color = new Color(0.5019608f, 0.5019608f, 0.5019608f);
    }
  }

  public void OnAllElement()
  {
    this.SelectElementButtonsDisable(Unit0046ConfirmAutoOrganization.ButtonElement.all);
    this.selectElement = CommonElement.none;
  }

  public void OnFireElement()
  {
    this.SelectElementButtonsDisable(Unit0046ConfirmAutoOrganization.ButtonElement.fire);
    this.selectElement = CommonElement.fire;
  }

  public void OnWindElement()
  {
    this.SelectElementButtonsDisable(Unit0046ConfirmAutoOrganization.ButtonElement.wind);
    this.selectElement = CommonElement.wind;
  }

  public void OnThunderElement()
  {
    this.SelectElementButtonsDisable(Unit0046ConfirmAutoOrganization.ButtonElement.thunder);
    this.selectElement = CommonElement.thunder;
  }

  public void OnIceElement()
  {
    this.SelectElementButtonsDisable(Unit0046ConfirmAutoOrganization.ButtonElement.ice);
    this.selectElement = CommonElement.ice;
  }

  public void OnLightElement()
  {
    this.SelectElementButtonsDisable(Unit0046ConfirmAutoOrganization.ButtonElement.light);
    this.selectElement = CommonElement.light;
  }

  public void OnDarkElement()
  {
    this.SelectElementButtonsDisable(Unit0046ConfirmAutoOrganization.ButtonElement.dark);
    this.selectElement = CommonElement.dark;
  }

  public override void onBackButton() => this.onClickedNO();

  public void onClickedNO()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public void onClickedYES()
  {
    if (this.IsPushAndSet())
      return;
    this.eventYes_(this.selectElement);
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  private enum ButtonElement
  {
    all,
    fire,
    wind,
    thunder,
    ice,
    light,
    dark,
  }
}
