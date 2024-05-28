// Decompiled with JetBrains decompiler
// Type: Guild028116Popup
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
public class Guild028116Popup : BackButtonMenuBase
{
  private GuildInfoPopup guildPopupInfo;
  private GuildRegistration guildRegistration;
  private GuildDirectory guildDirectory;
  [SerializeField]
  private UILabel popupTitle;
  [SerializeField]
  private UILabel popupDesc;
  [SerializeField]
  private UILabel guildName;
  [SerializeField]
  private UILabel guildTitle;
  [SerializeField]
  private UI2DSprite guildTitleImage;
  private bool changeGuild;

  public void Initialize(GuildRegistration guild, GuildInfoPopup popup, bool fromChangeGuild = false)
  {
    this.changeGuild = fromChangeGuild;
    this.guildDirectory = (GuildDirectory) null;
    this.guildRegistration = guild;
    this.Initialize(this.guildRegistration.guild_name, popup);
    this.StartCoroutine(this.SetGuildData(guild.appearance));
  }

  public void Initialize(GuildDirectory guild, GuildInfoPopup popup, bool fromChangeGuild = false)
  {
    this.changeGuild = fromChangeGuild;
    this.guildRegistration = (GuildRegistration) null;
    this.guildDirectory = guild;
    this.Initialize(this.guildDirectory.guild_name, popup);
    this.StartCoroutine(this.SetGuildData(guild.appearance));
  }

  private void Initialize(string name, GuildInfoPopup popup)
  {
    this.guildPopupInfo = popup;
    this.guildName.SetTextLocalize(name);
    this.popupTitle.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_1_1_6_TITLE));
    this.popupDesc.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_1_1_6_DESC));
  }

  private IEnumerator SetGuildData(GuildAppearance data)
  {
    Future<Sprite> futureGuildTitleImage = EmblemUtility.LoadGuildEmblemSprite(data._current_emblem);
    IEnumerator e = futureGuildTitleImage.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.guildTitleImage.sprite2D = futureGuildTitleImage.Result;
    if (EmblemUtility.GuildEnblemData(data._current_emblem) != null)
      this.guildTitle.SetTextLocalize(EmblemUtility.GuildEnblemData(data._current_emblem).name);
    else
      this.guildTitle.SetTextLocalize(string.Empty);
  }

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().dismiss();

  public void onButtonYes() => this.StartCoroutine(this.SendCancelRequest());

  private IEnumerator SendCancelRequest()
  {
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    while (Singleton<PopupManager>.GetInstance().isOpenNoFinish)
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
    string target_guild_id = PlayerAffiliation.Current.guild_id;
    if (PlayerAffiliation.Current.status == GuildMembershipStatus.applicant && PlayerAffiliation.Current.guild_id == null || this.changeGuild)
    {
      this.changeGuild = true;
      target_guild_id = PlayerAffiliation.Current.applicant_guild_id;
    }
    bool applied = false;
    Future<WebAPI.Response.GuildApplicantsCancel> ft = WebAPI.GuildApplicantsCancel(target_guild_id, (Action<WebAPI.Response.UserError>) (e =>
    {
      if (e.Code.Equals("GLD001"))
        applied = true;
      else
        WebAPI.DefaultUserErrorCallback(e);
    }));
    IEnumerator e1 = ft.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    if (ft.Result == null && !applied)
    {
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
      MypageScene.ChangeSceneOnError();
    }
    else if (applied)
    {
      Singleton<PopupManager>.GetInstance().dismiss();
      Future<GameObject> fObj = Res.Prefabs.popup.popup_028_guild_common_ok__anim_popup01.Load<GameObject>();
      e1 = fObj.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      GameObject result = fObj.Result;
      Singleton<PopupManager>.GetInstance().open(result).GetComponent<GuildOkPopup>().Initialize(Consts.Format(Consts.GetInstance().POPUP_028_1_1_6_TITLE), Consts.Format(Consts.GetInstance().POPUP_028_1_1_6_NO_APPLICANT), ok: (Action) (() =>
      {
        Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
        MypageScene.ChangeScene(MypageRootMenu.Mode.GUILD, true);
      }));
    }
    else
    {
      if (this.guildPopupInfo.CancelRequeestCallback != null)
        this.guildPopupInfo.CancelRequeestCallback();
      GameObject gameObject = Singleton<PopupManager>.GetInstance().open(this.guildPopupInfo.guildCancelRequestResultPopup);
      if (Object.op_Inequality((Object) ((Component) gameObject.GetComponent<Guild0281161Popup>()).GetComponent<UIWidget>(), (Object) null))
        ((UIRect) ((Component) gameObject.GetComponent<Guild0281161Popup>()).GetComponent<UIWidget>()).alpha = 0.0f;
      gameObject.GetComponent<Guild0281161Popup>().Initialize(this.changeGuild);
    }
  }
}
