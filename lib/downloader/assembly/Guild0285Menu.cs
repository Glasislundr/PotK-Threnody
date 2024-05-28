// Decompiled with JetBrains decompiler
// Type: Guild0285Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Guild0285Menu : GuildApplicantsBarBase
{
  private const int MAX_APPLICANT_NUM = 30;
  private const int Width = 612;
  private const int Height = 157;
  private const int ColumnValue = 1;
  private const int RowValue = 8;
  private const int ScreenValue = 5;
  private GameObject scrollObj;
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private UILabel txtMemberNum;
  [SerializeField]
  private UILabel txtMember;
  [SerializeField]
  private GameObject dirNoApplicant;
  [SerializeField]
  private UILabel txtNoApplicant;
  [SerializeField]
  private UIButton btnAllReject;
  [SerializeField]
  private UIButton btnAllAccept;
  private GameObject commonOkPopup;

  public GameObject CommonOkPupup => this.commonOkPopup;

  public IEnumerator InitializeAsync()
  {
    Guild0285Menu guild0285Menu = this;
    IEnumerator e1 = guild0285Menu.ResourceLoad();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (PlayerAffiliation.Current.onGvgOperation)
    {
      // ISSUE: reference to a compiler-generated method
      guild0285Menu.ShowOkPopup(Consts.GetInstance().GUILD_28_5_MENU_TITLE, PlayerAffiliation.Current.guild.gvg_status == GvgStatus.preparing ? Consts.GetInstance().GUILD_28_5_MENU_ERROR_GVG_PREPARE : Consts.GetInstance().GUILD_28_5_MENU_ERROR_GVG, new Action(guild0285Menu.\u003CInitializeAsync\u003Eb__17_0));
    }
    else
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      Future<WebAPI.Response.GuildTop> guildTop = WebAPI.GuildTop(Persist.guildHeaderChat.Data.latestLogId, (Action<WebAPI.Response.UserError>) (e =>
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        WebAPI.DefaultUserErrorCallback(e);
        Singleton<CommonRoot>.GetInstance().isLoading = true;
        MypageScene.ChangeSceneOnError();
      }));
      e1 = guildTop.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (guildTop.Result != null)
      {
        e1 = guild0285Menu.RefreshList();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      }
    }
  }

  public void Initialize()
  {
    this.txtTitle.SetTextLocalize(Consts.Format(Consts.GetInstance().GUILD_28_5_MENU_TITLE));
    this.txtMember.SetTextLocalize(Consts.Format(Consts.GetInstance().GUILD_28_5_MEMBER_NUM));
    this.txtNoApplicant.SetTextLocalize(Consts.Format(Consts.GetInstance().GUILD_28_5_NO_APPLICANT));
    if (!Persist.guildSetting.Exists)
      return;
    GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.newApplicant, false);
    Persist.guildSetting.Flush();
  }

  public IEnumerator InitApplicantScroll(GuildApplicant[] applicants)
  {
    Guild0285Menu guild0285Menu = this;
    guild0285Menu.allApplicantInfo.Clear();
    guild0285Menu.allApplicantBar.Clear();
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    DateTime nowTime = ServerTime.NowAppTime();
    guild0285Menu.Initialize(nowTime, 612, 157, 8, 5);
    guild0285Menu.CreateApplicantInfo(applicants);
    guild0285Menu.dirNoApplicant.SetActive(guild0285Menu.allApplicantInfo.Count <= 0);
    ((UIButtonColor) guild0285Menu.btnAllReject).isEnabled = guild0285Menu.allApplicantInfo.Count > 0;
    ((UIButtonColor) guild0285Menu.btnAllAccept).isEnabled = guild0285Menu.allApplicantInfo.Count > 0;
    if (guild0285Menu.allApplicantInfo.Count > 0)
    {
      e = guild0285Menu.CreateScrollBase(guild0285Menu.scrollObj);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    guild0285Menu.scroll.ResolvePosition();
    guild0285Menu.txtMemberNum.SetTextLocalize(string.Format("{0}/{1}", (object) PlayerAffiliation.Current.guild.applicants.Length, (object) 30));
    guild0285Menu.InitializeEnd();
  }

  protected override IEnumerator CreateScroll(int info_index, int bar_index)
  {
    Guild0285Menu menu = this;
    Guild0285Scroll scroll = menu.allApplicantBar[bar_index];
    ApplicantBarInfo applicantBarInfo = menu.allApplicantInfo[info_index];
    applicantBarInfo.scroll = scroll;
    ((Component) scroll).gameObject.SetActive(true);
    IEnumerator e = scroll.Initialize(menu, applicantBarInfo.applicant, menu.now);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) scroll).gameObject.SetActive(true);
  }

  public IEnumerator RefreshList()
  {
    IEnumerator e = this.InitApplicantScroll(PlayerAffiliation.Current.guild.applicants);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator ResourceLoad()
  {
    Future<GameObject> fgObj;
    IEnumerator e;
    if (Object.op_Equality((Object) this.scrollObj, (Object) null))
    {
      fgObj = Res.Prefabs.guild028_5.join_request_list.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.scrollObj = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.commonOkPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_guild_common_ok__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.commonOkPopup = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
  }

  public void ShowOkPopup(string title, string message, Action ok = null)
  {
    Singleton<PopupManager>.GetInstance().open(this.commonOkPopup).GetComponent<GuildOkPopup>().Initialize(title, message, ok: ok);
  }

  public void ShowLoading()
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
  }

  public void HideLoading()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }

  public IEnumerator Accept(
    string[] player_ids,
    Action actionSuccess = null,
    Action<WebAPI.Response.UserError> actionFailure = null)
  {
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    while (Singleton<PopupManager>.GetInstance().isOpenNoFinish)
      yield return (object) null;
    this.ShowLoading();
    string errorCode = string.Empty;
    bool isMaintenance = false;
    IEnumerator e1 = WebAPI.GuildApplicantsAccept(false, ((IEnumerable<string>) player_ids).ToArray<string>(), (Action<WebAPI.Response.UserError>) (e =>
    {
      errorCode = e.Code;
      this.HideLoading();
      if (e.Code.Equals("GLD014"))
      {
        WebAPI.DefaultUserErrorCallback(e);
        isMaintenance = true;
        Singleton<CommonRoot>.GetInstance().isLoading = true;
        MypageScene.ChangeSceneOnError();
      }
      else if (actionFailure != null)
        actionFailure(e);
      else
        WebAPI.DefaultUserErrorCallback(e);
    })).Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (!isMaintenance)
    {
      if (errorCode.Equals("GLD006"))
      {
        this.ShowLoading();
        Future<WebAPI.Response.GuildTop> guildTop = WebAPI.GuildTop(Persist.guildHeaderChat.Data.latestLogId, (Action<WebAPI.Response.UserError>) (e =>
        {
          this.HideLoading();
          WebAPI.DefaultUserErrorCallback(e);
          Singleton<CommonRoot>.GetInstance().isLoading = true;
          MypageScene.ChangeSceneOnError();
        }));
        e1 = guildTop.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        if (guildTop.Result == null)
          yield break;
        else
          guildTop = (Future<WebAPI.Response.GuildTop>) null;
      }
      e1 = this.RefreshList();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      this.HideLoading();
      if (string.IsNullOrEmpty(errorCode) && actionSuccess != null)
        actionSuccess();
    }
  }

  public IEnumerator Refuse(
    string[] player_ids,
    Action actionSuccess = null,
    Action<WebAPI.Response.UserError> actionFailure = null)
  {
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    while (Singleton<PopupManager>.GetInstance().isOpenNoFinish)
      yield return (object) null;
    this.ShowLoading();
    string errorCode = string.Empty;
    bool isMaintenance = false;
    IEnumerator e1 = WebAPI.GuildApplicantsReject(false, player_ids, (Action<WebAPI.Response.UserError>) (e =>
    {
      errorCode = e.Code;
      this.HideLoading();
      if (e.Code.Equals("GLD014"))
      {
        WebAPI.DefaultUserErrorCallback(e);
        isMaintenance = true;
        Singleton<CommonRoot>.GetInstance().isLoading = true;
        MypageScene.ChangeSceneOnError();
      }
      else if (actionFailure != null)
        actionFailure(e);
      else
        WebAPI.DefaultUserErrorCallback(e);
    })).Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (!isMaintenance)
    {
      if (errorCode.Equals("GLD006"))
      {
        this.ShowLoading();
        Future<WebAPI.Response.GuildTop> guildTop = WebAPI.GuildTop(Persist.guildHeaderChat.Data.latestLogId, (Action<WebAPI.Response.UserError>) (e =>
        {
          this.HideLoading();
          WebAPI.DefaultUserErrorCallback(e);
          Singleton<CommonRoot>.GetInstance().isLoading = true;
          MypageScene.ChangeSceneOnError();
        }));
        e1 = guildTop.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        if (guildTop.Result == null)
          yield break;
        else
          guildTop = (Future<WebAPI.Response.GuildTop>) null;
      }
      e1 = this.RefreshList();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      this.HideLoading();
      if (string.IsNullOrEmpty(errorCode) && actionSuccess != null)
        actionSuccess();
    }
  }

  public void onRefuseBulkButton()
  {
    if (this.IsPushAndSet())
      return;
    List<string> stringList = new List<string>();
    for (int index = 0; index < this.allApplicantInfo.Count; ++index)
      stringList.Add(this.allApplicantInfo[index].applicant.player.player_id);
    this.StartCoroutine(this.Refuse(stringList.ToArray(), (Action) (() => this.ShowOkPopup(Consts.Format(Consts.GetInstance().GUILD_28_5_REJECT_REQUEST_TITLE), Consts.Format(Consts.GetInstance().GUILD_28_5_REJECT_BULK_DESC))), (Action<WebAPI.Response.UserError>) (error =>
    {
      if (error.Code.Equals("GLD006"))
      {
        this.ShowOkPopup(Consts.GetInstance().GUILD_28_5_REJECT_REQUEST_TITLE, Consts.GetInstance().GUILD_28_5_REFUSE_ERROR);
      }
      else
      {
        if (!error.Code.Equals("GVG002"))
          return;
        this.ShowOkPopup(Consts.GetInstance().GUILD_28_5_REJECT_REQUEST_TITLE, PlayerAffiliation.Current.guild.gvg_status == GvgStatus.preparing ? Consts.GetInstance().GUILD_28_5_REJECT_ERROR_GVG_PREPARE : Consts.GetInstance().GUILD_28_5_REJECT_ERROR_GVG);
      }
    })));
  }

  public void onAcceptBulkButton()
  {
    if (this.IsPushAndSet())
      return;
    int num = PlayerAffiliation.Current.guild.appearance.membership_capacity - PlayerAffiliation.Current.guild.memberships.Length;
    if (num <= 0)
    {
      this.ShowOkPopup(Consts.Format(Consts.GetInstance().GUILD_28_5_ACCEPT_REQUEST_TITLE), Consts.Format(Consts.GetInstance().GUILD_28_5_ACCEAT_FAILED_DESC));
    }
    else
    {
      int count = this.allApplicantInfo.Count;
      if (count > num)
        count = num;
      List<string> stringList = new List<string>();
      for (int index = 0; index < count; ++index)
        stringList.Add(this.allApplicantInfo[index].applicant.player.player_id);
      this.StartCoroutine(this.Accept(stringList.ToArray(), (Action) (() => this.ShowOkPopup(Consts.Format(Consts.GetInstance().GUILD_28_5_ACCEPT_REQUEST_TITLE), Consts.Format(Consts.GetInstance().GUILD_28_5_ACCEAPT_BULK_DESC, (IDictionary) new Hashtable()
      {
        {
          (object) "num",
          (object) count
        }
      }))), (Action<WebAPI.Response.UserError>) (error =>
      {
        if (error.Code.Equals("GLD006"))
          this.ShowOkPopup(Consts.GetInstance().GUILD_28_5_ACCEPT_REQUEST_TITLE, Consts.GetInstance().GUILD_28_5_ACCEPT_ERROR);
        else if (error.Code.Equals("GVG002"))
        {
          this.ShowOkPopup(Consts.GetInstance().GUILD_28_5_ACCEPT_REQUEST_TITLE, PlayerAffiliation.Current.guild.gvg_status == GvgStatus.preparing ? Consts.GetInstance().GUILD_28_5_ACCEPT_ERROR_GVG_PREPARE : Consts.GetInstance().GUILD_28_5_ACCEPT_ERROR_GVG);
        }
        else
        {
          if (!error.Code.Equals("GLD020"))
            return;
          this.ShowOkPopup(Consts.GetInstance().POPUP_028_3_2_2_IN_RAID_TITLE, Consts.GetInstance().POPUP_028_3_2_2_IN_RAID_DESC);
        }
      })));
    }
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }
}
