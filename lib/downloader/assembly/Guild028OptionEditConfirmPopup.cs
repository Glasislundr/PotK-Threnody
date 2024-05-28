// Decompiled with JetBrains decompiler
// Type: Guild028OptionEditConfirmPopup
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
public class Guild028OptionEditConfirmPopup : BackButtonMenuBase
{
  private Guild0283Menu guildMenu;
  private int approval_policy_id;
  private int atmosphere_id;
  private int auto_approval_id;
  private int auto_kick_id;
  [SerializeField]
  private UILabel popupTitle;
  [SerializeField]
  private UILabel guildNameLabel;
  [SerializeField]
  private UILabel guildName;
  [SerializeField]
  private UILabel approvalPolicyLabel;
  [SerializeField]
  private UILabel approvalPolicy;
  [SerializeField]
  private UILabel atmosphereLabel;
  [SerializeField]
  private UILabel atmosphere;
  [SerializeField]
  private UILabel autoApprovalLabel;
  [SerializeField]
  private UILabel autoApproval;
  [SerializeField]
  private UILabel autoKickLabel;
  [SerializeField]
  private UILabel autoKick;

  public void Initialize(
    Guild0283Menu menu,
    int approval_policy,
    int atmosphere,
    int auto_approval,
    int auto_kick,
    string guild_name)
  {
    if (Object.op_Inequality((Object) ((Component) this).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) this).GetComponent<UIWidget>()).alpha = 0.0f;
    this.guildMenu = menu;
    this.approval_policy_id = approval_policy;
    this.atmosphere_id = atmosphere;
    this.auto_approval_id = auto_approval;
    this.auto_kick_id = auto_kick;
    this.popupTitle.SetTextLocalize(Consts.GetInstance().POPUP_GUILD_OPTION_EDIT_TITLE2);
    this.guildNameLabel.SetTextLocalize(Consts.GetInstance().POPUP_GUILD_OPTION_EDIT_GUILD_NAME);
    this.atmosphereLabel.SetTextLocalize(Consts.GetInstance().POPUP_GUILD_OPTION_EDIT_ATMOSPHERE);
    this.approvalPolicyLabel.SetTextLocalize(Consts.GetInstance().POPUP_GUILD_OPTION_EDIT_REQUIREMENT);
    this.autoApprovalLabel.SetTextLocalize(Consts.GetInstance().POPUP_GUILD_OPTION_EDIT_APPROVAL);
    this.autoKickLabel.SetTextLocalize(Consts.GetInstance().POPUP_GUILD_OPTION_EDIT_AUTO_KICK);
    this.guildName.SetTextLocalize(guild_name);
    this.atmosphere.SetTextLocalize(((IEnumerable<GuildAtmosphere>) MasterData.GuildAtmosphereList).Where<GuildAtmosphere>((Func<GuildAtmosphere, bool>) (x => x.ID == atmosphere)).First<GuildAtmosphere>().name);
    this.approvalPolicy.SetTextLocalize(((IEnumerable<GuildApprovalPolicy>) MasterData.GuildApprovalPolicyList).Where<GuildApprovalPolicy>((Func<GuildApprovalPolicy, bool>) (x => x.ID == approval_policy)).First<GuildApprovalPolicy>().name);
    this.autoApproval.SetTextLocalize(((IEnumerable<GuildAutoApproval>) MasterData.GuildAutoApprovalList).Where<GuildAutoApproval>((Func<GuildAutoApproval, bool>) (x => x.ID == auto_approval)).First<GuildAutoApproval>().name);
    this.autoKick.SetTextLocalize(((IEnumerable<GuildAutokick>) MasterData.GuildAutokickList).Where<GuildAutokick>((Func<GuildAutokick, bool>) (x => x.ID == auto_kick)).First<GuildAutokick>().name);
  }

  private void ErrorCallback(WebAPI.Response.UserError error)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    if (error.Code.Equals("GLD011"))
      Singleton<PopupManager>.GetInstance().open(this.guildMenu.GuildNgWordPopup).GetComponent<Guild028NgWordPopup>().Initialize((Action) (() => { }));
    else if (error.Code.Equals("GLD014"))
    {
      WebAPI.DefaultUserErrorCallback(error);
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      MypageScene.ChangeSceneOnError();
    }
    else
      WebAPI.DefaultUserErrorCallback(error);
  }

  private IEnumerator SendGuildSetting()
  {
    Guild028OptionEditConfirmPopup editConfirmPopup = this;
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    while (Singleton<PopupManager>.GetInstance().isOpenNoFinish)
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.GuildSettings> ft = WebAPI.GuildSettings(editConfirmPopup.approval_policy_id, editConfirmPopup.atmosphere_id, editConfirmPopup.auto_approval_id, editConfirmPopup.auto_kick_id, editConfirmPopup.guildName.text, false, new Action<WebAPI.Response.UserError>(editConfirmPopup.ErrorCallback));
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (ft.Result != null)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
  }

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().dismiss();

  public void onYesButton() => this.StartCoroutine(this.SendGuildSetting());
}
