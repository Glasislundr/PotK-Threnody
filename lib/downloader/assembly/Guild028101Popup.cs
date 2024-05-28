// Decompiled with JetBrains decompiler
// Type: Guild028101Popup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Guild028101Popup : BackButtonMenuBase
{
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
  private UILabel availability;
  [SerializeField]
  private UIPopupList availabilityList;
  [SerializeField]
  private UILabel autokick;
  [SerializeField]
  private UIPopupList autokickList;
  private Guild02811Menu menu2811;
  private Guild02812Menu menu2812;
  private GuildUtil.MenuType aMenu;

  public void Initialize(Guild02811Menu guild02811menu, GuildSetting guildsetting)
  {
    this.aMenu = GuildUtil.MenuType.menu2811;
    this.menu2811 = guild02811menu;
    this.Init(guildsetting);
  }

  public void Initialize(Guild02812Menu guild02812menu, GuildSetting guildsetting)
  {
    this.aMenu = GuildUtil.MenuType.menu2812;
    this.menu2812 = guild02812menu;
    this.Init(guildsetting);
  }

  private void Init(GuildSetting guildsetting)
  {
    this.setting = guildsetting;
    // ISSUE: method pointer
    this.guildNameInput.onValidate = new UIInput.OnValidate((object) this, __methodptr(onValidate));
    this.guildName.SetTextLocalize(this.setting.guildName);
    this.guildNameInput.value = this.setting.guildName;
    this.guildNameInput.defaultText = string.Empty;
    this.atmosphere.SetTextLocalize(this.setting.atmosphere);
    this.approval.SetTextLocalize(this.setting.approval);
    this.autoApproval.SetTextLocalize(this.setting.autoApproval);
    this.availability.SetTextLocalize(this.setting.availability);
    this.autokick.SetTextLocalize(this.setting.autokick);
    this.atmosphereList.items.Clear();
    this.atmosphereList.items.Add(Consts.GetInstance().GUILD_SETTING_CONDITIONS_NULL);
    ((IEnumerable<GuildAtmosphere>) MasterData.GuildAtmosphereList).ForEach<GuildAtmosphere>((Action<GuildAtmosphere>) (x => this.atmosphereList.items.Add(x.name)));
    this.atmosphereList.onChange.Add(new EventDelegate((EventDelegate.Callback) (() => Singleton<NGSoundManager>.GetInstance().playSE("SE_1002"))));
    this.atmosphereList.value = this.setting.atmosphere;
    this.approvalList.items.Clear();
    this.approvalList.items.Add(Consts.GetInstance().GUILD_SETTING_CONDITIONS_NULL);
    ((IEnumerable<GuildApprovalPolicy>) MasterData.GuildApprovalPolicyList).ForEach<GuildApprovalPolicy>((Action<GuildApprovalPolicy>) (x => this.approvalList.items.Add(x.name)));
    this.approvalList.onChange.Add(new EventDelegate((EventDelegate.Callback) (() => Singleton<NGSoundManager>.GetInstance().playSE("SE_1002"))));
    this.approvalList.value = this.setting.approval;
    this.autoApprovalList.items.Clear();
    this.autoApprovalList.items.Add(Consts.GetInstance().GUILD_SETTING_CONDITIONS_NULL);
    ((IEnumerable<GuildAutoApproval>) MasterData.GuildAutoApprovalList).ForEach<GuildAutoApproval>((Action<GuildAutoApproval>) (x => this.autoApprovalList.items.Add(x.name)));
    this.autoApprovalList.onChange.Add(new EventDelegate((EventDelegate.Callback) (() => Singleton<NGSoundManager>.GetInstance().playSE("SE_1002"))));
    this.autoApprovalList.value = this.setting.autoApproval;
    this.availabilityList.items.Clear();
    this.availabilityList.items.Add(Consts.GetInstance().GUILD_SETTING_CONDITIONS_NULL);
    ((IEnumerable<GuildAvailability>) MasterData.GuildAvailabilityList).ForEach<GuildAvailability>((Action<GuildAvailability>) (x => this.availabilityList.items.Add(x.name)));
    this.availabilityList.onChange.Add(new EventDelegate((EventDelegate.Callback) (() => Singleton<NGSoundManager>.GetInstance().playSE("SE_1002"))));
    this.availabilityList.value = this.setting.availability;
    this.autokickList.items.Clear();
    this.autokickList.items.Add(Consts.GetInstance().GUILD_SETTING_CONDITIONS_NULL);
    ((IEnumerable<GuildAutokick>) MasterData.GuildAutokickList).ForEach<GuildAutokick>((Action<GuildAutokick>) (x => this.autokickList.items.Add(x.name)));
    this.autokickList.onChange.Add(new EventDelegate((EventDelegate.Callback) (() => Singleton<NGSoundManager>.GetInstance().playSE("SE_1002"))));
    this.autokickList.value = this.setting.autokick;
  }

  private char onValidate(string text, int charIndex, char addedChar)
  {
    bool flag = char.IsControl(addedChar) || addedChar >= '\uE000' && addedChar <= '\uF8FF';
    Debug.Log((object) (((int) addedChar).ToString() + ":" + flag.ToString()));
    return flag ? char.MinValue : addedChar;
  }

  public void onChangeInput()
  {
    int num = this.IsPush ? 1 : 0;
  }

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().dismiss();

  public void onButtonSerch()
  {
    GuildSetting data = new GuildSetting();
    this.guildName.SetTextLocalize(this.guildNameInput.value);
    data.guildName = this.guildName.text;
    data.atmosphere = this.atmosphere.text;
    data.approval = this.approval.text;
    data.autoApproval = this.autoApproval.text;
    data.availability = this.availability.text;
    data.autokick = this.autokick.text;
    switch (this.aMenu)
    {
      case GuildUtil.MenuType.menu2811:
        this.menu2811.Setting(data);
        Singleton<PopupManager>.GetInstance().dismiss();
        this.StartCoroutine(this.menu2811.SearchGuild(new Action(this.menu2811.DrawGuildList)));
        break;
      case GuildUtil.MenuType.menu2812:
        this.menu2812.Setting(data);
        Singleton<PopupManager>.GetInstance().dismiss();
        this.StartCoroutine(this.menu2812.SearchGuild(new Action(this.menu2812.DrawGuildList)));
        break;
    }
  }

  public void onButtonBest()
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    switch (this.aMenu)
    {
      case GuildUtil.MenuType.menu2811:
        this.StartCoroutine(this.menu2811.SearchBestGuild(new Action(this.menu2811.DrawGuildList)));
        break;
      case GuildUtil.MenuType.menu2812:
        this.StartCoroutine(this.menu2812.SearchBestGuild(new Action(this.menu2812.DrawGuildList)));
        break;
    }
  }

  public void onButtonFriend()
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    switch (this.aMenu)
    {
      case GuildUtil.MenuType.menu2811:
        this.StartCoroutine(this.menu2811.FriendList());
        break;
      case GuildUtil.MenuType.menu2812:
        this.StartCoroutine(this.menu2812.FriendList());
        break;
    }
  }
}
