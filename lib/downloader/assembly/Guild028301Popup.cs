// Decompiled with JetBrains decompiler
// Type: Guild028301Popup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Guild028301Popup : BackButtonMenuBase
{
  private Guild02811Menu menu2811;
  private Guild02812Menu menu2812;
  private GuildUtil.MenuType aMenu;
  [SerializeField]
  private GuildSetting setting;
  [SerializeField]
  private UILabel guildName;
  [SerializeField]
  private UIInput guildNameInput;
  [SerializeField]
  private UILabel atmosphere;
  [SerializeField]
  private UIPopupList atmosphereList;
  [SerializeField]
  private UILabel approval;
  [SerializeField]
  private UIPopupList approvalList;
  [SerializeField]
  private UILabel autoApproval;
  [SerializeField]
  private UIPopupList autoApprovalList;
  [SerializeField]
  private UILabel autoKick;
  [SerializeField]
  private UIPopupList autoKickList;
  [SerializeField]
  private SpreadColorButton OKButton;

  public void Initialize(Guild02811Menu guild02811menu)
  {
    this.aMenu = GuildUtil.MenuType.menu2811;
    this.menu2811 = guild02811menu;
    this.Init();
  }

  public void Initialize(Guild02812Menu guild02812menu)
  {
    this.aMenu = GuildUtil.MenuType.menu2812;
    this.menu2812 = guild02812menu;
    this.Init();
  }

  public void Init()
  {
    // ISSUE: method pointer
    ((Component) this.guildName).GetComponent<UIInput>().onValidate = new UIInput.OnValidate((object) this, __methodptr(onValidate));
    ((UIButtonColor) this.OKButton).isEnabled = false;
    this.atmosphereList.items.Clear();
    this.approvalList.items.Clear();
    this.autoApprovalList.items.Clear();
    this.autoKickList.items.Clear();
    this.atmosphereList.onChange.Add(new EventDelegate((EventDelegate.Callback) (() => Singleton<NGSoundManager>.GetInstance().playSE("SE_1002"))));
    this.approvalList.onChange.Add(new EventDelegate((EventDelegate.Callback) (() => Singleton<NGSoundManager>.GetInstance().playSE("SE_1002"))));
    this.autoApprovalList.onChange.Add(new EventDelegate((EventDelegate.Callback) (() => Singleton<NGSoundManager>.GetInstance().playSE("SE_1002"))));
    this.autoKickList.onChange.Add(new EventDelegate((EventDelegate.Callback) (() => Singleton<NGSoundManager>.GetInstance().playSE("SE_1002"))));
    this.approvalList.onChange.Add(new EventDelegate((EventDelegate.Callback) (() =>
    {
      int? nullable1 = ((IEnumerable<GuildApprovalPolicy>) MasterData.GuildApprovalPolicyList).FirstIndexOrNull<GuildApprovalPolicy>((Func<GuildApprovalPolicy, bool>) (x => x.name == this.approval.text));
      if (!nullable1.HasValue || !MasterData.GuildApprovalPolicyList[nullable1.Value].default_manual)
        return;
      int? nullable2 = ((IEnumerable<GuildAutoApproval>) MasterData.GuildAutoApprovalList).FirstIndexOrNull<GuildAutoApproval>((Func<GuildAutoApproval, bool>) (x => !x.auto_approval));
      if (!nullable2.HasValue)
        return;
      this.autoApprovalList.value = MasterData.GuildAutoApprovalList[nullable2.Value].name;
      this.autoApproval.SetTextLocalize(MasterData.GuildAutoApprovalList[nullable2.Value].name);
    })));
    ((IEnumerable<GuildAtmosphere>) MasterData.GuildAtmosphereList).ForEach<GuildAtmosphere>((Action<GuildAtmosphere>) (x => this.atmosphereList.items.Add(x.name)));
    ((IEnumerable<GuildApprovalPolicy>) MasterData.GuildApprovalPolicyList).ForEach<GuildApprovalPolicy>((Action<GuildApprovalPolicy>) (x => this.approvalList.items.Add(x.name)));
    ((IEnumerable<GuildAutoApproval>) MasterData.GuildAutoApprovalList).ForEach<GuildAutoApproval>((Action<GuildAutoApproval>) (x => this.autoApprovalList.items.Add(x.name)));
    ((IEnumerable<GuildAutokick>) MasterData.GuildAutokickList).ForEach<GuildAutokick>((Action<GuildAutokick>) (x => this.autoKickList.items.Add(x.name)));
    this.guildName.SetTextLocalize(string.Empty);
    this.atmosphere.SetTextLocalize(((IEnumerable<GuildAtmosphere>) MasterData.GuildAtmosphereList).First<GuildAtmosphere>().name);
    this.atmosphereList.value = ((IEnumerable<GuildAtmosphere>) MasterData.GuildAtmosphereList).First<GuildAtmosphere>().name;
    this.approval.SetTextLocalize(((IEnumerable<GuildApprovalPolicy>) MasterData.GuildApprovalPolicyList).First<GuildApprovalPolicy>().name);
    this.approvalList.value = ((IEnumerable<GuildApprovalPolicy>) MasterData.GuildApprovalPolicyList).First<GuildApprovalPolicy>().name;
    this.autoApproval.SetTextLocalize(((IEnumerable<GuildAutoApproval>) MasterData.GuildAutoApprovalList).First<GuildAutoApproval>().name);
    this.autoApprovalList.value = ((IEnumerable<GuildAutoApproval>) MasterData.GuildAutoApprovalList).First<GuildAutoApproval>().name;
    this.autoKick.SetTextLocalize(((IEnumerable<GuildAutokick>) MasterData.GuildAutokickList).First<GuildAutokick>().name);
    this.autoKickList.value = ((IEnumerable<GuildAutokick>) MasterData.GuildAutokickList).First<GuildAutokick>().name;
    this.guildNameInput.caretColor = Color.black;
  }

  public IEnumerator Initialize(Guild02811Menu guild02811menu, GuildSetting setting)
  {
    this.Initialize(guild02811menu);
    yield return (object) null;
    this.Init(setting);
  }

  public IEnumerator Initialize(Guild02812Menu guild02812menu, GuildSetting setting)
  {
    this.Initialize(guild02812menu);
    yield return (object) null;
    this.Init(setting);
  }

  public IEnumerator Init(GuildSetting setting)
  {
    yield return (object) null;
    this.guildName.SetTextLocalize(setting.guildName);
    this.guildNameInput.value = setting.guildName;
    this.guildNameInput.defaultText = string.Empty;
    this.atmosphere.SetTextLocalize(setting.atmosphere);
    this.atmosphereList.value = setting.atmosphere;
    this.approval.SetTextLocalize(setting.approval);
    this.approvalList.value = setting.approval;
    this.autoApproval.SetTextLocalize(setting.autoApproval);
    this.autoApprovalList.value = setting.autoApproval;
    this.autoKick.SetTextLocalize(setting.autokick);
    this.autoKickList.value = setting.autokick;
  }

  private char onValidate(string text, int charIndex, char addedChar)
  {
    bool flag = char.IsControl(addedChar) || addedChar >= '\uE000' && addedChar <= '\uF8FF';
    Debug.Log((object) (((int) addedChar).ToString() + ":" + flag.ToString()));
    return flag ? char.MinValue : addedChar;
  }

  public void onChangeInput()
  {
    if (this.IsPush)
      return;
    ((UIButtonColor) this.OKButton).isEnabled = !this.guildName.text.isEmptyOrWhitespace();
  }

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().dismiss();

  public void onButtonDecision()
  {
    GuildSetting data = new GuildSetting();
    this.guildName.SetTextLocalize(this.guildNameInput.value);
    data.guildName = this.guildName.text;
    data.atmosphere = this.atmosphere.text;
    data.approval = this.approval.text;
    data.autoApproval = this.autoApproval.text;
    data.autokick = this.autoKick.text;
    switch (this.aMenu)
    {
      case GuildUtil.MenuType.menu2811:
        Singleton<PopupManager>.GetInstance().open(this.menu2811.BuildSettingCheckPopup).GetComponent<Guild028401Popup>().Initialize(this.menu2811, data);
        break;
      case GuildUtil.MenuType.menu2812:
        Singleton<PopupManager>.GetInstance().open(this.menu2812.BuildSettingCheckPopup).GetComponent<Guild028401Popup>().Initialize(this.menu2812, data);
        break;
    }
  }
}
