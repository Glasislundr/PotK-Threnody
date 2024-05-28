// Decompiled with JetBrains decompiler
// Type: Guild028OptionEditPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Guild028OptionEditPopup : BackButtonMenuBase
{
  private Guild0283Menu guildMenu;
  private GuildRegistration guildSetting;
  private string prevRequirementPulldownValue;
  [SerializeField]
  private UILabel popupTitle;
  [SerializeField]
  private UILabel guildNameLabel;
  [SerializeField]
  private UIInput guildNameInput;
  [SerializeField]
  private UILabel guildName;
  [SerializeField]
  private UILabel atmosphereLabel;
  [SerializeField]
  private UILabel atmosphere;
  [SerializeField]
  private UILabel conditionLabel;
  [SerializeField]
  private UILabel condition;
  [SerializeField]
  private UILabel approvalLabel;
  [SerializeField]
  private UILabel approval;
  [SerializeField]
  private UILabel autokickLabel;
  [SerializeField]
  private UILabel autokick;
  [SerializeField]
  private UIPopupList atmospherePulldown;
  [SerializeField]
  private UIPopupList conditionPulldown;
  [SerializeField]
  private UIPopupList approvalPulldown;
  [SerializeField]
  private UIPopupList autokickPulldown;
  [SerializeField]
  private SpreadColorButton decideButton;

  public void Initialize(Guild0283Menu menu)
  {
    if (Object.op_Inequality((Object) ((Component) this).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) this).GetComponent<UIWidget>()).alpha = 0.0f;
    this.guildMenu = menu;
    this.guildSetting = PlayerAffiliation.Current.guild;
    this.popupTitle.SetTextLocalize(Consts.GetInstance().POPUP_GUILD_OPTION_EDIT_TITLE);
    this.guildNameLabel.SetTextLocalize(Consts.GetInstance().POPUP_GUILD_OPTION_EDIT_GUILD_NAME);
    this.atmosphereLabel.SetTextLocalize(Consts.GetInstance().POPUP_GUILD_OPTION_EDIT_ATMOSPHERE);
    this.conditionLabel.SetTextLocalize(Consts.GetInstance().POPUP_GUILD_OPTION_EDIT_REQUIREMENT);
    this.approvalLabel.SetTextLocalize(Consts.GetInstance().POPUP_GUILD_OPTION_EDIT_APPROVAL);
    this.autokickLabel.SetTextLocalize(Consts.GetInstance().POPUP_GUILD_OPTION_EDIT_AUTO_KICK);
    this.atmospherePulldown.items.Clear();
    this.conditionPulldown.items.Clear();
    this.approvalPulldown.items.Clear();
    this.autokickPulldown.items.Clear();
    this.atmospherePulldown.onChange.Add(new EventDelegate((EventDelegate.Callback) (() => Singleton<NGSoundManager>.GetInstance().playSE("SE_1002"))));
    this.conditionPulldown.onChange.Add(new EventDelegate((EventDelegate.Callback) (() => Singleton<NGSoundManager>.GetInstance().playSE("SE_1002"))));
    this.approvalPulldown.onChange.Add(new EventDelegate((EventDelegate.Callback) (() => Singleton<NGSoundManager>.GetInstance().playSE("SE_1002"))));
    this.autokickPulldown.onChange.Add(new EventDelegate((EventDelegate.Callback) (() => Singleton<NGSoundManager>.GetInstance().playSE("SE_1002"))));
    ((IEnumerable<GuildAtmosphere>) MasterData.GuildAtmosphereList).ForEach<GuildAtmosphere>((Action<GuildAtmosphere>) (x => this.atmospherePulldown.items.Add(x.name)));
    ((IEnumerable<GuildApprovalPolicy>) MasterData.GuildApprovalPolicyList).ForEach<GuildApprovalPolicy>((Action<GuildApprovalPolicy>) (x => this.conditionPulldown.items.Add(x.name)));
    ((IEnumerable<GuildAutoApproval>) MasterData.GuildAutoApprovalList).ForEach<GuildAutoApproval>((Action<GuildAutoApproval>) (x => this.approvalPulldown.items.Add(x.name)));
    ((IEnumerable<GuildAutokick>) MasterData.GuildAutokickList).ForEach<GuildAutokick>((Action<GuildAutokick>) (x => this.autokickPulldown.items.Add(x.name)));
    int? nullable1 = ((IEnumerable<GuildAtmosphere>) MasterData.GuildAtmosphereList).FirstIndexOrNull<GuildAtmosphere>((Func<GuildAtmosphere, bool>) (x => x.ID == this.guildSetting.atmosphere.ID));
    if (nullable1.HasValue)
    {
      this.atmosphere.SetTextLocalize(MasterData.GuildAtmosphereList[nullable1.Value].name);
      this.atmospherePulldown.value = this.atmosphere.text;
    }
    int? nullable2 = ((IEnumerable<GuildApprovalPolicy>) MasterData.GuildApprovalPolicyList).FirstIndexOrNull<GuildApprovalPolicy>((Func<GuildApprovalPolicy, bool>) (x => x.ID == this.guildSetting.approval_policy.ID));
    if (nullable2.HasValue)
    {
      this.condition.SetTextLocalize(MasterData.GuildApprovalPolicyList[nullable2.Value].name);
      this.conditionPulldown.value = this.condition.text;
    }
    int? nullable3 = ((IEnumerable<GuildAutoApproval>) MasterData.GuildAutoApprovalList).FirstIndexOrNull<GuildAutoApproval>((Func<GuildAutoApproval, bool>) (x => x.ID == this.guildSetting.auto_approval.ID));
    if (nullable3.HasValue)
    {
      this.approval.SetTextLocalize(MasterData.GuildAutoApprovalList[nullable3.Value].name);
      this.approvalPulldown.value = this.approval.text;
    }
    int? nullable4 = ((IEnumerable<GuildAutokick>) MasterData.GuildAutokickList).FirstIndexOrNull<GuildAutokick>((Func<GuildAutokick, bool>) (x => x.ID == this.guildSetting.auto_kick.ID));
    if (nullable4.HasValue)
    {
      this.autokick.SetTextLocalize(MasterData.GuildAutokickList[nullable4.Value].name);
      this.autokickPulldown.value = this.autokick.text;
    }
    this.guildName.text = this.guildSetting.guild_name;
    this.guildNameInput.value = this.guildName.text;
    // ISSUE: method pointer
    this.guildNameInput.onValidate = new UIInput.OnValidate((object) this, __methodptr(onValidateGuildName));
    this.guildNameInput.caretColor = Color.black;
  }

  public void SetPulldownEventCallback()
  {
    if (this.conditionPulldown.onChange == null)
      this.conditionPulldown.onChange = new List<EventDelegate>();
    this.conditionPulldown.onChange.Add(new EventDelegate(new EventDelegate.Callback(this.onRequirementPulldownValueChanged)));
  }

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().dismiss();

  public void onChangeGuildName()
  {
    ((UIButtonColor) this.decideButton).isEnabled = !this.guildNameInput.value.isEmptyOrWhitespace();
  }

  public char onValidateGuildName(string text, int charIndex, char addedChar)
  {
    bool flag = char.IsControl(addedChar) || addedChar >= '\uE000' && addedChar <= '\uF8FF';
    Debug.Log((object) (((int) addedChar).ToString() + ":" + flag.ToString()));
    return flag ? char.MinValue : addedChar;
  }

  public void onDecideButton()
  {
    int num1 = -1;
    int num2 = -1;
    int num3 = -1;
    int num4 = -1;
    int? nullable = ((IEnumerable<GuildApprovalPolicy>) MasterData.GuildApprovalPolicyList).FirstIndexOrNull<GuildApprovalPolicy>((Func<GuildApprovalPolicy, bool>) (x => x.name == this.condition.text));
    if (nullable.HasValue)
      num1 = MasterData.GuildApprovalPolicyList[nullable.Value].ID;
    nullable = ((IEnumerable<GuildAtmosphere>) MasterData.GuildAtmosphereList).FirstIndexOrNull<GuildAtmosphere>((Func<GuildAtmosphere, bool>) (x => x.name == this.atmosphere.text));
    if (nullable.HasValue)
      num2 = MasterData.GuildAtmosphereList[nullable.Value].ID;
    nullable = ((IEnumerable<GuildAutoApproval>) MasterData.GuildAutoApprovalList).FirstIndexOrNull<GuildAutoApproval>((Func<GuildAutoApproval, bool>) (x => x.name == this.approval.text));
    if (nullable.HasValue)
      num3 = MasterData.GuildAutoApprovalList[nullable.Value].ID;
    nullable = ((IEnumerable<GuildAutokick>) MasterData.GuildAutokickList).FirstIndexOrNull<GuildAutokick>((Func<GuildAutokick, bool>) (x => x.name == this.autokick.text));
    if (nullable.HasValue)
      num4 = MasterData.GuildAutokickList[nullable.Value].ID;
    if (num1 < 0 || num2 < 0 || num3 < 0 || num4 < 0)
      return;
    GameObject prefab = this.guildMenu.GuildSettingConfirmPopup.Clone();
    Guild028OptionEditConfirmPopup component = prefab.GetComponent<Guild028OptionEditConfirmPopup>();
    prefab.SetActive(false);
    this.guildName.SetTextLocalize(this.guildNameInput.value);
    Guild0283Menu guildMenu = this.guildMenu;
    int approval_policy = num1;
    int atmosphere = num2;
    int auto_approval = num3;
    int auto_kick = num4;
    string text = this.guildName.text;
    component.Initialize(guildMenu, approval_policy, atmosphere, auto_approval, auto_kick, text);
    prefab.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }

  public void onRequirementPulldownValueChanged()
  {
    if (this.prevRequirementPulldownValue != null && this.prevRequirementPulldownValue.Equals(this.conditionPulldown.value))
      return;
    this.prevRequirementPulldownValue = this.conditionPulldown.value;
    int? nullable1 = ((IEnumerable<GuildApprovalPolicy>) MasterData.GuildApprovalPolicyList).FirstIndexOrNull<GuildApprovalPolicy>((Func<GuildApprovalPolicy, bool>) (x => x.name == this.conditionPulldown.value));
    if (!nullable1.HasValue || !MasterData.GuildApprovalPolicyList[nullable1.Value].default_manual)
      return;
    int? nullable2 = ((IEnumerable<GuildAutoApproval>) MasterData.GuildAutoApprovalList).FirstIndexOrNull<GuildAutoApproval>((Func<GuildAutoApproval, bool>) (x => !x.auto_approval));
    if (!nullable2.HasValue)
      return;
    this.approval.SetTextLocalize(MasterData.GuildAutoApprovalList[nullable2.Value].name);
    this.approvalPulldown.value = this.approval.text;
  }
}
