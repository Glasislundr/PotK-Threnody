// Decompiled with JetBrains decompiler
// Type: Guild028401Popup
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
public class Guild028401Popup : BackButtonMenuBase
{
  private Guild02811Menu menu2811;
  private Guild02812Menu menu2812;
  private GuildUtil.MenuType aMenu;
  [SerializeField]
  private GuildSetting setting;
  [SerializeField]
  private UILabel guildName;
  [SerializeField]
  private UILabel atmosphere;
  [SerializeField]
  private UILabel approval;
  [SerializeField]
  private UILabel autoApproval;
  [SerializeField]
  private UILabel autoKick;

  public void Initialize(Guild02811Menu guild02811menu, GuildSetting data)
  {
    this.aMenu = GuildUtil.MenuType.menu2811;
    this.menu2811 = guild02811menu;
    this.Init(data);
  }

  public void Initialize(Guild02812Menu guild02812menu, GuildSetting data)
  {
    this.aMenu = GuildUtil.MenuType.menu2812;
    this.menu2812 = guild02812menu;
    this.Init(data);
  }

  public void Init(GuildSetting data)
  {
    this.setting = data;
    this.guildName.SetTextLocalize(this.setting.guildName);
    this.atmosphere.SetTextLocalize(this.setting.atmosphere);
    this.approval.SetTextLocalize(this.setting.approval);
    this.autoApproval.SetTextLocalize(this.setting.autoApproval);
    this.autoKick.SetTextLocalize(this.setting.autokick);
  }

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().dismiss();

  public void onButtonDecision()
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    this.StartCoroutine(this.BuildGuild());
  }

  private IEnumerator BuildGuild()
  {
    Guild028401Popup guild028401Popup = this;
    // ISSUE: reference to a compiler-generated method
    int id1 = ((IEnumerable<GuildApprovalPolicy>) MasterData.GuildApprovalPolicyList).Where<GuildApprovalPolicy>(new Func<GuildApprovalPolicy, bool>(guild028401Popup.\u003CBuildGuild\u003Eb__14_0)).First<GuildApprovalPolicy>().ID;
    // ISSUE: reference to a compiler-generated method
    int id2 = ((IEnumerable<GuildAtmosphere>) MasterData.GuildAtmosphereList).Where<GuildAtmosphere>(new Func<GuildAtmosphere, bool>(guild028401Popup.\u003CBuildGuild\u003Eb__14_1)).First<GuildAtmosphere>().ID;
    // ISSUE: reference to a compiler-generated method
    int id3 = ((IEnumerable<GuildAutoApproval>) MasterData.GuildAutoApprovalList).Where<GuildAutoApproval>(new Func<GuildAutoApproval, bool>(guild028401Popup.\u003CBuildGuild\u003Eb__14_2)).First<GuildAutoApproval>().ID;
    // ISSUE: reference to a compiler-generated method
    int id4 = ((IEnumerable<GuildAutokick>) MasterData.GuildAutokickList).Where<GuildAutokick>(new Func<GuildAutokick, bool>(guild028401Popup.\u003CBuildGuild\u003Eb__14_3)).First<GuildAutokick>().ID;
    Persist.guildRaidProgress.Data.setDefault();
    Persist.guildRaidProgress.Flush();
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.GuildEstablish> establish = WebAPI.GuildEstablish(id1, id2, id3, id4, guild028401Popup.guildName.text, false, new Action<WebAPI.Response.UserError>(guild028401Popup.ErrorCallback));
    IEnumerator e = establish.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    if (establish.Result != null)
    {
      switch (guild028401Popup.aMenu)
      {
        case GuildUtil.MenuType.menu2811:
          Singleton<PopupManager>.GetInstance().open(guild028401Popup.menu2811.BuildEffectPopup);
          break;
        case GuildUtil.MenuType.menu2812:
          Singleton<PopupManager>.GetInstance().open(guild028401Popup.menu2812.BuildEffectPopup);
          break;
      }
    }
  }

  private void ErrorCallback(WebAPI.Response.UserError error)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    if (error.Code.Equals("GLD011"))
    {
      switch (this.aMenu)
      {
        case GuildUtil.MenuType.menu2811:
          Singleton<PopupManager>.GetInstance().open(this.menu2811.GuildNgWordPopup).GetComponent<Guild028NgWordPopup>().Initialize((Action) (() => this.menu2811.BuildGuildPopupOpen(this.setting)));
          break;
        case GuildUtil.MenuType.menu2812:
          Singleton<PopupManager>.GetInstance().open(this.menu2812.GuildNgWordPopup).GetComponent<Guild028NgWordPopup>().Initialize((Action) (() => this.menu2812.BuildGuildPopupOpen(this.setting)));
          break;
      }
    }
    else
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
    }
  }
}
