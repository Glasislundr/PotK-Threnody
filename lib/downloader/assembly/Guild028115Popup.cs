// Decompiled with JetBrains decompiler
// Type: Guild028115Popup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Guild028115Popup : BackButtonMenuBase
{
  private GameObject commonDlgObj;
  private GuildInfoPopup guildPopupInfo;
  private GuildRegistration guildRegistration;
  private GuildDirectory guildDirectory;
  [SerializeField]
  private UILabel popupTitle;
  [SerializeField]
  private UILabel guildName;
  [SerializeField]
  private UI2DSprite guildTitleImage;
  [SerializeField]
  private UILabel popupDesc;
  private bool fromChangeGuild;

  public void Initialize(GuildRegistration guild, GuildInfoPopup popup, bool changeGuild)
  {
    this.guildDirectory = (GuildDirectory) null;
    this.guildRegistration = guild;
    this.StartCoroutine(this.Initialize(this.guildRegistration.guild_name, popup, this.guildRegistration.appearance, changeGuild));
  }

  public void Initialize(GuildDirectory guild, GuildInfoPopup popup, bool changeGuild)
  {
    this.guildRegistration = (GuildRegistration) null;
    this.guildDirectory = guild;
    this.StartCoroutine(this.Initialize(this.guildDirectory.guild_name, popup, guild.appearance, changeGuild));
  }

  private IEnumerator Initialize(
    string name,
    GuildInfoPopup popup,
    GuildAppearance appearance,
    bool changeGuild)
  {
    this.guildPopupInfo = popup;
    this.guildName.SetTextLocalize(name);
    this.fromChangeGuild = changeGuild;
    this.popupTitle.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_1_1_5_TITLE));
    if (!this.fromChangeGuild)
      this.popupDesc.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_1_1_5_DESC));
    else
      this.popupDesc.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_1_1_5_DESC_CHANGE_GUILD));
    IEnumerator e;
    if (Object.op_Equality((Object) this.commonDlgObj, (Object) null))
    {
      Future<GameObject> fObj = Res.Prefabs.popup.popup_028_guild_common_ok__anim_popup01.Load<GameObject>();
      e = fObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.commonDlgObj = fObj.Result;
      fObj = (Future<GameObject>) null;
    }
    e = this.SetGuildData(appearance);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator SetGuildData(GuildAppearance data)
  {
    Future<Sprite> futureGuildTitleImage = EmblemUtility.LoadGuildEmblemSprite(data._current_emblem);
    IEnumerator e = futureGuildTitleImage.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.guildTitleImage.sprite2D = futureGuildTitleImage.Result;
  }

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().dismiss();

  public void onButtonYes() => this.StartCoroutine(this.SendRequest());

  private IEnumerator SendRequest()
  {
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    while (Singleton<PopupManager>.GetInstance().isOpenNoFinish)
      yield return (object) null;
    Persist.guildRaidProgress.Data.setDefault();
    Persist.guildRaidProgress.Flush();
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    string target_guild_id = this.guildRegistration != null ? this.guildRegistration.guild_id : this.guildDirectory.guild_id;
    bool applied = false;
    bool maintenance = false;
    Future<WebAPI.Response.GuildApplicantsSend> ft = WebAPI.GuildApplicantsSend(false, target_guild_id, (Action<WebAPI.Response.UserError>) (e =>
    {
      if (e.Code.Equals("GLD001"))
        applied = true;
      else if (e.Code.Equals("GLD020"))
      {
        this.ShowOkPopup(Consts.GetInstance().POPUP_028_3_2_2_IN_RAID_TITLE, Consts.GetInstance().POPUP_028_3_2_2_IN_RAID_DESC, (Action) (() => { }));
      }
      else
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        WebAPI.DefaultUserErrorCallback(e);
        if (!e.Code.Equals("GLD014"))
          return;
        maintenance = true;
      }
    }));
    IEnumerator e1 = ft.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (maintenance)
    {
      if (this.guildPopupInfo.RequestMaintenanceCallback == null)
      {
        Singleton<CommonRoot>.GetInstance().isLoading = true;
        MypageScene.ChangeSceneOnError();
      }
      else
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        this.guildPopupInfo.RequestMaintenanceCallback();
      }
    }
    else if (applied)
    {
      if (this.guildPopupInfo.RequestAlreadyGuildCallback == null)
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        this.ShowOkPopup(Consts.GetInstance().POPUP_028_1_1_5_TITLE, Consts.GetInstance().POPUP_028_1_1_6_NO_APPLICANT, (Action) (() =>
        {
          Singleton<CommonRoot>.GetInstance().isLoading = true;
          MypageScene.ChangeScene(MypageRootMenu.Mode.GUILD, true);
        }));
      }
      else
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        this.guildPopupInfo.RequestAlreadyGuildCallback();
      }
    }
    else if (ft.Result != null)
    {
      if (PlayerAffiliation.Current.status == GuildMembershipStatus.membership)
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        string title;
        string message;
        if (!this.fromChangeGuild)
        {
          title = Consts.GetInstance().GUILD_APPLY_APPLICANT_TITLE;
          message = Consts.GetInstance().GUILD_APPLY_APPLICANT_MESSAGE;
        }
        else if (PlayerAffiliation.Current.applicant_guild_id == this.guildDirectory.guild_id)
        {
          if (!ft.Result.within_applicant_period)
          {
            title = Consts.GetInstance().POPUP_028_1_1_5_TITLE_IN_RAID;
            message = Consts.GetInstance().POPUP_028_1_1_5_IN_RAID;
          }
          else
          {
            title = Consts.GetInstance().POPUP_028_1_1_5_TITLE;
            message = Consts.GetInstance().POPUP_028_1_1_5_1_DESC;
          }
        }
        else
        {
          title = Consts.GetInstance().GUILD_APPLY_APPLICANT_TITLE_CHANGE_GUILD;
          message = Consts.GetInstance().GUILD_APPLY_APPLICANT_MESSAGE_CHANGE_GUILD;
        }
        this.ShowOkPopup(title, message, (Action) (() =>
        {
          Singleton<CommonRoot>.GetInstance().isLoading = true;
          MypageScene.ChangeScene(MypageRootMenu.Mode.GUILD, true);
        }));
      }
      else
      {
        this.guildRegistration = PlayerAffiliation.Current.guild;
        if ((this.guildRegistration != null ? (this.guildRegistration.auto_approval.auto_approval ? 1 : 0) : (this.guildDirectory.auto_approval.auto_approval ? 1 : 0)) != 0)
        {
          Singleton<CommonRoot>.GetInstance().isLoading = false;
          Singleton<CommonRoot>.GetInstance().loadingMode = 0;
          if (PlayerAffiliation.Current.status == GuildMembershipStatus.applicant)
          {
            if (this.guildDirectory != null && this.guildDirectory.in_gvg || this.guildRegistration != null && PlayerAffiliation.Current.onGvgOperation)
              this.ShowOkPopup(Consts.GetInstance().POPUP_028_1_1_5_TITLE_IN_GVG, Consts.GetInstance().POPUP_028_1_1_5_IN_GVG, (Action) (() =>
              {
                if (this.guildPopupInfo.SendRequestCallback == null)
                  return;
                this.guildPopupInfo.SendRequestCallback();
              }));
            else if (!ft.Result.within_applicant_period)
              this.ShowOkPopup(Consts.Format(Consts.GetInstance().POPUP_028_1_1_5_TITLE_IN_RAID), Consts.Format(Consts.GetInstance().POPUP_028_1_1_5_IN_RAID), (Action) (() =>
              {
                if (this.guildPopupInfo.SendRequestCallback == null)
                  return;
                this.guildPopupInfo.SendRequestCallback();
              }));
            else
              this.ShowOkPopup(Consts.Format(Consts.GetInstance().POPUP_028_1_1_5_TITLE), Consts.Format(Consts.GetInstance().POPUP_028_1_1_6_MEMBER_LIMIT), (Action) (() =>
              {
                if (this.guildPopupInfo.SendRequestCallback == null)
                  return;
                this.guildPopupInfo.SendRequestCallback();
              }));
          }
          else if (PlayerAffiliation.Current.status == GuildMembershipStatus.not_exist || PlayerAffiliation.Current.status == GuildMembershipStatus.withdraw)
            this.ShowOkPopup(Consts.Format(Consts.GetInstance().POPUP_028_1_1_5_TITLE), Consts.Format(Consts.GetInstance().POPUP_028_1_1_6_NOT_ELIGIBLE), (Action) (() => { }));
        }
        else if (PlayerAffiliation.Current.status == GuildMembershipStatus.applicant)
        {
          if (this.guildPopupInfo.SendRequestCallback != null)
            this.guildPopupInfo.SendRequestCallback();
          Singleton<CommonRoot>.GetInstance().isLoading = false;
          Singleton<CommonRoot>.GetInstance().loadingMode = 0;
          GameObject gameObject = Singleton<PopupManager>.GetInstance().open(this.guildPopupInfo.guildSendRequestResultPopup);
          if (Object.op_Inequality((Object) ((Component) gameObject.GetComponent<Guild0281151Popup>()).GetComponent<UIWidget>(), (Object) null))
            ((UIRect) ((Component) gameObject.GetComponent<Guild0281151Popup>()).GetComponent<UIWidget>()).alpha = 0.0f;
          gameObject.GetComponent<Guild0281151Popup>().Initialize();
        }
        else if (PlayerAffiliation.Current.status == GuildMembershipStatus.not_exist || PlayerAffiliation.Current.status == GuildMembershipStatus.withdraw)
        {
          Singleton<CommonRoot>.GetInstance().isLoading = false;
          Singleton<CommonRoot>.GetInstance().loadingMode = 0;
          this.ShowOkPopup(Consts.Format(Consts.GetInstance().POPUP_028_1_1_5_TITLE), Consts.Format(Consts.GetInstance().POPUP_028_1_1_6_NOT_ELIGIBLE), (Action) (() => { }));
        }
      }
    }
  }

  private void ShowOkPopup(string title, string message, Action action)
  {
    Singleton<PopupManager>.GetInstance().open(this.commonDlgObj).GetComponent<GuildOkPopup>().Initialize(title, message, ok: action);
  }
}
