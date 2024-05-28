// Decompiled with JetBrains decompiler
// Type: GuildMemberPositionManagementPopup
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
public class GuildMemberPositionManagementPopup : BackButtonMenuBase
{
  private const int MINIMUM_NUM_RESIGN_MASTER = 2;
  private const int DEFAULT_DLG_WIDTH = 532;
  private const int DEFAULT_DLG_HEIGHT = 322;
  private GuildMemberObject guildMemberObjs;
  private GuildMembership memberInfo;
  private GameObject currentPopup;
  private Guild0282Menu guild282Menu;
  private Guild0282GuildBaseMenu guildBaseMenu;
  private bool isShowMapButton;
  private Action actionAfterRoleChange;
  [SerializeField]
  private UILabel popupTitle;
  [SerializeField]
  private UILabel btnLabelResignMaster;
  [SerializeField]
  private UILabel btnLabelTransferMaster;
  [SerializeField]
  private UILabel btnLabelAssignSubMaster;
  [SerializeField]
  private UILabel btnLabelRivestSubMaster;
  [SerializeField]
  private UILabel btnLabelResignSubMaster;
  [SerializeField]
  private UILabel btnLabelVanish;
  [SerializeField]
  private UIButton btnResignMaster;
  [SerializeField]
  private UIButton btnTransferMaster;
  [SerializeField]
  private UIButton btnAssignSubMaster;
  [SerializeField]
  private UIButton btnDivestSubMaster;
  [SerializeField]
  private UIButton btnResignSubMaster;
  [SerializeField]
  private UIButton btnVanish;
  [SerializeField]
  private UIScrollView scroll;
  [Header("Resign master dlg size")]
  [SerializeField]
  private Vector2 resignMasterDlgSize;
  [SerializeField]
  private Vector2 resignMasterFailedDlgSize;
  [SerializeField]
  private Vector2 resignMasterComfirmDlgSize;
  [SerializeField]
  private Vector2 resignMasterResultDlgSize;
  [Header("Transfer master dlg size")]
  [SerializeField]
  private Vector2 transferMasterDlgSize;
  [SerializeField]
  private Vector2 transferMasterComfirmDlgSize;
  [SerializeField]
  private Vector2 transferMasterResultDlgSize;
  [Header("Assign submaster dlg size")]
  [SerializeField]
  private Vector2 assignSubmasterDlgSize;
  [SerializeField]
  private Vector2 assignSubmasterFailedDlgSize;
  [SerializeField]
  private Vector2 assignSubmasterComfirmDlgSize;
  [SerializeField]
  private Vector2 assignSubmasterResultDlgSize;
  [Header("Divest submaster dlg size")]
  [SerializeField]
  private Vector2 divestSubmasterDlgSize;
  [SerializeField]
  private Vector2 divestSubmasterComfirmDlgSize;
  [SerializeField]
  private Vector2 divestSubmasterResultDlgSize;
  [Header("Resign submaster dlg size")]
  [SerializeField]
  private Vector2 resingSubmasterDlgSize;
  [SerializeField]
  private Vector2 resingSubmasterComfirmDlgSize;
  [SerializeField]
  private Vector2 resingSubmasterResultDlgSize;
  [Header("Vanish member dlg size")]
  [SerializeField]
  private Vector2 vanishMemberDlgSize;
  [SerializeField]
  private Vector2 vanishMemberComfirmDlgSize;
  [SerializeField]
  private Vector2 vanishMemberResultDlgSize;

  public void Initialize(
    Guild0282Menu menu,
    GuildMembership info,
    Guild0282GuildBaseMenu baseMenu,
    GuildMemberObject popupObjs,
    Action action = null,
    bool showMapButton = true)
  {
    if (Object.op_Inequality((Object) ((Component) this).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) this).GetComponent<UIWidget>()).alpha = 0.0f;
    this.guild282Menu = menu;
    this.guildBaseMenu = baseMenu;
    this.memberInfo = info;
    this.guildMemberObjs = popupObjs;
    this.isShowMapButton = showMapButton;
    this.actionAfterRoleChange = action;
    this.popupTitle.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_CHANGE_POSITION_TITLE));
    this.SetButtonLabel();
    this.SetButtonDisp();
  }

  private void SetButtonDisp()
  {
    GuildRole? role = PlayerAffiliation.Current.role;
    GuildRole? nullable1 = new GuildRole?(this.memberInfo.role);
    ((Component) this.btnResignMaster).gameObject.SetActive(false);
    ((Component) this.btnTransferMaster).gameObject.SetActive(false);
    ((Component) this.btnAssignSubMaster).gameObject.SetActive(false);
    ((Component) this.btnResignSubMaster).gameObject.SetActive(false);
    ((Component) this.btnDivestSubMaster).gameObject.SetActive(false);
    ((Component) this.btnVanish).gameObject.SetActive(false);
    GuildRole? nullable2 = role;
    GuildRole guildRole1 = GuildRole.master;
    if (nullable2.GetValueOrDefault() == guildRole1 & nullable2.HasValue)
    {
      GuildRole? nullable3 = nullable1;
      GuildRole guildRole2 = GuildRole.master;
      if (nullable3.GetValueOrDefault() == guildRole2 & nullable3.HasValue)
      {
        ((Component) this.btnResignMaster).gameObject.SetActive(true);
      }
      else
      {
        nullable3 = nullable1;
        GuildRole guildRole3 = GuildRole.sub_master;
        if (nullable3.GetValueOrDefault() == guildRole3 & nullable3.HasValue)
        {
          ((Component) this.btnTransferMaster).gameObject.SetActive(true);
          ((Component) this.btnDivestSubMaster).gameObject.SetActive(true);
          ((Component) this.btnVanish).gameObject.SetActive(true);
        }
        else
        {
          nullable3 = nullable1;
          GuildRole guildRole4 = GuildRole.general;
          if (!(nullable3.GetValueOrDefault() == guildRole4 & nullable3.HasValue))
            return;
          ((Component) this.btnTransferMaster).gameObject.SetActive(true);
          ((Component) this.btnAssignSubMaster).gameObject.SetActive(true);
          ((Component) this.btnVanish).gameObject.SetActive(true);
        }
      }
    }
    else
    {
      GuildRole? nullable4 = role;
      GuildRole guildRole5 = GuildRole.sub_master;
      if (!(nullable4.GetValueOrDefault() == guildRole5 & nullable4.HasValue))
        return;
      if (this.memberInfo.player.player_id == Player.Current.id)
      {
        ((Component) this.btnResignSubMaster).gameObject.SetActive(true);
      }
      else
      {
        if (this.memberInfo.role != GuildRole.general)
          return;
        ((Component) this.btnVanish).gameObject.SetActive(true);
      }
    }
  }

  public void ResetScroll()
  {
    ((UIRect) ((Component) this.scroll).GetComponent<UIPanel>()).UpdateAnchors();
    this.scroll.ResetPosition();
  }

  private void SetButtonLabel()
  {
    this.btnLabelResignMaster.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_CHANGE_POSITION_LABEL_RESIGN_MASTER));
    this.btnLabelTransferMaster.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_CHANGE_POSITION_LABEL_TRANSFER_MASTER));
    this.btnLabelAssignSubMaster.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_CHANGE_POSITION_LABEL_ASSIGN_SUBMASTER));
    this.btnLabelRivestSubMaster.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_CHANGE_POSITION_LABEL_RIVEST_SUBMASTER));
    this.btnLabelResignSubMaster.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_CHANGE_POSITION_LABEL_RESIGN_SUBMASTER));
    this.btnLabelVanish.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_CHANGE_POSITION_LABEL_VANISH));
  }

  private void ShowYesNoPopup(string title, string message, Vector2 size, Action yes = null, Action no = null)
  {
    GameObject prefab = this.guildMemberObjs.GuildPositionManagementPopupYesNo.Clone();
    prefab.SetActive(false);
    prefab.GetComponent<GuildYesNoPopup>().Initialize(title, message, size, yes, no);
    prefab.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    this.currentPopup = prefab;
  }

  private void ShowYesNoPopupRecycle(
    string title,
    string message,
    Vector2 size,
    Action yes = null,
    Action no = null)
  {
    if (Object.op_Equality((Object) this.currentPopup, (Object) null))
      this.currentPopup = this.guildMemberObjs.GuildPositionManagementPopupYesNo.Clone();
    this.currentPopup.SetActive(false);
    this.currentPopup.GetComponent<GuildYesNoPopup>().Initialize(title, message, size, yes, no);
    this.currentPopup.SetActive(true);
    NGTweenParts component = this.currentPopup.GetComponent<NGTweenParts>();
    if (Object.op_Inequality((Object) component, (Object) null))
    {
      component.forceActive(true);
    }
    else
    {
      if (!Object.op_Inequality((Object) this.currentPopup.GetComponent<UIWidget>(), (Object) null))
        return;
      ((UIRect) this.currentPopup.GetComponent<UIWidget>()).alpha = 1f;
    }
  }

  private void ShowOkPopup(string title, string message, Vector2 size, Action ok = null)
  {
    GameObject prefab = this.guildMemberObjs.GuildPositionManagementPopupOk.Clone();
    prefab.SetActive(false);
    prefab.GetComponent<GuildOkPopup>().Initialize(title, message, new Vector2?(size), ok);
    prefab.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }

  private void ShowLoading()
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
  }

  private void HideLoading()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }

  private void HideGuildFooterBadge()
  {
    if (!Persist.guildSetting.Exists || !GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.newApplicant))
      return;
    GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.newApplicant, false);
  }

  private IEnumerator SendResignGuildMaster()
  {
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    while (Singleton<PopupManager>.GetInstance().isOpenNoFinish)
      yield return (object) null;
    this.ShowLoading();
    string errorCode = string.Empty;
    Future<WebAPI.Response.GuildMasterResign> ft = WebAPI.GuildMasterResign(false, (Action<WebAPI.Response.UserError>) (e =>
    {
      this.HideLoading();
      if (e.Code.Equals("GVG002"))
        this.StartCoroutine(this.showPopupOnGvg());
      else
        WebAPI.DefaultUserErrorCallback(e);
      errorCode = e.Code;
    }));
    IEnumerator e1 = ft.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    while (Singleton<PopupManager>.GetInstance().isOpen)
      yield return (object) null;
    if (errorCode.Equals("GLD014"))
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      yield return (object) null;
      MypageScene.ChangeSceneOnError();
    }
    if (ft.Result != null)
    {
      this.HideGuildFooterBadge();
      if (Singleton<NGSceneManager>.GetInstance().sceneName.Equals("guild028_2") && this.actionAfterRoleChange != null)
        this.actionAfterRoleChange();
      this.HideLoading();
      this.currentPopup = (GameObject) null;
      this.ShowOkPopup(Consts.Format(Consts.GetInstance().POPUP_GUILD_CHANGE_POSITION_RESULT_TITLE), Consts.Format(Consts.GetInstance().POPUP_GUILD_RESIGN_MASTER_RESULT), this.resignMasterResultDlgSize, (Action) (() =>
      {
        if (Object.op_Equality((Object) this.guild282Menu, (Object) null))
        {
          MypageScene.ChangeScene(MypageRootMenu.Mode.GUILD, true);
        }
        else
        {
          if (this.actionAfterRoleChange != null)
            this.actionAfterRoleChange();
          this.StartCoroutine(this.showMemberDetailPopup());
        }
      }));
    }
  }

  public void onResignGuildMasterButton()
  {
    if (PlayerAffiliation.Current.onGvgOperation)
      this.StartCoroutine(this.showPopupOnGvg());
    else if (PlayerAffiliation.Current.guild.memberships.Length < 2)
      this.ShowOkPopup(Consts.Format(Consts.GetInstance().POPUP_GUILD_CHANGE_POSITION_TITLE), Consts.Format(Consts.GetInstance().POPUP_GUILD_CANNOT_RESIGN_MASTER), this.resignMasterFailedDlgSize, (Action) (() => { }));
    else
      this.ShowYesNoPopup(Consts.Format(Consts.GetInstance().POPUP_GUILD_CHANGE_POSITION_TITLE), Consts.Format(Consts.GetInstance().POPUP_GUILD_RESIGN_MASTER), this.resignMasterDlgSize, (Action) (() => this.ShowYesNoPopupRecycle(Consts.Format(Consts.GetInstance().POPUP_GUILD_CHANGE_POSITION_CONFIRMATION_TITLE), Consts.Format(Consts.GetInstance().POPUP_GUILD_RESIGN_MASTER_CONFIRMATION), this.resignMasterComfirmDlgSize, (Action) (() => this.StartCoroutine(this.SendResignGuildMaster())), (Action) (() => { }))), (Action) (() => { }));
  }

  private IEnumerator TransferGuildMaster()
  {
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    while (Singleton<PopupManager>.GetInstance().isOpenNoFinish)
      yield return (object) null;
    this.ShowLoading();
    string errorCode = string.Empty;
    Future<WebAPI.Response.GuildMasterTransfer> ft = WebAPI.GuildMasterTransfer(false, this.memberInfo.player.player_id, (Action<WebAPI.Response.UserError>) (e =>
    {
      this.HideLoading();
      if (e.Code.Equals("GVG002"))
        this.StartCoroutine(this.showPopupOnGvg());
      else
        WebAPI.DefaultUserErrorCallback(e);
      errorCode = e.Code;
    }));
    IEnumerator e1 = ft.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    while (Singleton<PopupManager>.GetInstance().isOpen)
      yield return (object) null;
    if (errorCode.Equals("GLD014"))
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      yield return (object) null;
      MypageScene.ChangeSceneOnError();
    }
    if (ft.Result != null)
    {
      this.HideGuildFooterBadge();
      if (Singleton<NGSceneManager>.GetInstance().sceneName.Equals("guild028_2") && this.actionAfterRoleChange != null)
        this.actionAfterRoleChange();
      this.HideLoading();
      this.currentPopup = (GameObject) null;
      this.ShowOkPopup(Consts.Format(Consts.GetInstance().POPUP_GUILD_CHANGE_POSITION_RESULT_TITLE), Consts.Format(Consts.GetInstance().POPUP_GUILD_TRANSFER_MASTER_RESULT), this.transferMasterResultDlgSize, (Action) (() =>
      {
        if (Object.op_Equality((Object) this.guild282Menu, (Object) null))
          MypageScene.ChangeScene(MypageRootMenu.Mode.GUILD, true);
        else
          this.StartCoroutine(this.showMemberDetailPopup());
      }));
    }
  }

  public void onTransferGuildMasterButton()
  {
    if (PlayerAffiliation.Current.onGvgOperation)
      this.StartCoroutine(this.showPopupOnGvg());
    else
      this.ShowYesNoPopup(Consts.Format(Consts.GetInstance().POPUP_GUILD_CHANGE_POSITION_TITLE), Consts.Format(Consts.GetInstance().POPUP_GUILD_TRANSFER_MASTER), this.transferMasterDlgSize, (Action) (() => this.ShowYesNoPopupRecycle(Consts.Format(Consts.GetInstance().POPUP_GUILD_CHANGE_POSITION_CONFIRMATION_TITLE), Consts.Format(Consts.GetInstance().POPUP_GUILD_TRANSFER_MASTER_CONFIRMATION), this.transferMasterComfirmDlgSize, (Action) (() => this.StartCoroutine(this.TransferGuildMaster())), (Action) (() => this.currentPopup = (GameObject) null))), (Action) (() => { }));
  }

  private IEnumerator AssignSubMaster()
  {
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    while (Singleton<PopupManager>.GetInstance().isOpenNoFinish)
      yield return (object) null;
    this.ShowLoading();
    string errorCode = string.Empty;
    Future<WebAPI.Response.GuildSubmasterAssign> ft = WebAPI.GuildSubmasterAssign(false, this.memberInfo.player.player_id, (Action<WebAPI.Response.UserError>) (e =>
    {
      this.HideLoading();
      if (e.Code.Equals("GVG002"))
        this.StartCoroutine(this.showPopupOnGvg());
      else if (e.Code.Equals("GLD020"))
        this.StartCoroutine(this.showPopupNotAvailableMemberChange());
      else
        WebAPI.DefaultUserErrorCallback(e);
      errorCode = e.Code;
    }));
    IEnumerator e1 = ft.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    while (Singleton<PopupManager>.GetInstance().isOpen)
      yield return (object) null;
    if (errorCode.Equals("GLD014"))
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      yield return (object) null;
      MypageScene.ChangeSceneOnError();
    }
    if (ft.Result != null)
    {
      if (Singleton<NGSceneManager>.GetInstance().sceneName.Equals("guild028_2") && this.actionAfterRoleChange != null)
        this.actionAfterRoleChange();
      this.HideLoading();
      this.currentPopup = (GameObject) null;
      this.ShowOkPopup(Consts.Format(Consts.GetInstance().POPUP_GUILD_CHANGE_POSITION_RESULT_TITLE), Consts.Format(Consts.GetInstance().POPUP_GUILD_ASSIGN_SUB_MASTER_RESULT), this.assignSubmasterResultDlgSize, (Action) (() => this.StartCoroutine(this.showMemberDetailPopup())));
    }
  }

  private int GetSubMasterNum()
  {
    return ((IEnumerable<GuildMembership>) PlayerAffiliation.Current.guild.memberships).Count<GuildMembership>((Func<GuildMembership, bool>) (x => x.role == GuildRole.sub_master));
  }

  public void onAssignSubMasterButton()
  {
    if (PlayerAffiliation.Current.onGvgOperation)
    {
      this.StartCoroutine(this.showPopupOnGvg());
    }
    else
    {
      int? nullable = ((IEnumerable<MasterDataTable.GuildSetting>) MasterData.GuildSettingList).FirstIndexOrNull<MasterDataTable.GuildSetting>((Func<MasterDataTable.GuildSetting, bool>) (x => x.ID == 10));
      float result = 0.0f;
      if (nullable.HasValue)
        float.TryParse(MasterData.GuildSettingList[nullable.Value].value, out result);
      if (this.GetSubMasterNum() >= (int) result)
        this.ShowOkPopup(Consts.Format(Consts.GetInstance().POPUP_GUILD_CHANGE_POSITION_TITLE), Consts.Format(Consts.GetInstance().POPUP_GUILD_CANNOT_ASSIGN_SUB_MASTER), this.assignSubmasterFailedDlgSize, (Action) (() => { }));
      else
        this.ShowYesNoPopup(Consts.Format(Consts.GetInstance().POPUP_GUILD_CHANGE_POSITION_TITLE), Consts.Format(Consts.GetInstance().POPUP_GUILD_ASSIGN_SUB_MASTER), this.assignSubmasterDlgSize, (Action) (() => this.ShowYesNoPopupRecycle(Consts.Format(Consts.GetInstance().POPUP_GUILD_CHANGE_POSITION_CONFIRMATION_TITLE), Consts.Format(Consts.GetInstance().POPUP_GUILD_ASSIGN_SUB_MASTER_CONFIRMATION), this.assignSubmasterComfirmDlgSize, (Action) (() => this.StartCoroutine(this.AssignSubMaster())), (Action) (() => this.currentPopup = (GameObject) null))), (Action) (() => { }));
    }
  }

  private IEnumerator DivestSubMaster()
  {
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    while (Singleton<PopupManager>.GetInstance().isOpenNoFinish)
      yield return (object) null;
    this.ShowLoading();
    string errorCode = string.Empty;
    Future<WebAPI.Response.GuildSubmasterDismiss> ft = WebAPI.GuildSubmasterDismiss(false, this.memberInfo.player.player_id, (Action<WebAPI.Response.UserError>) (e =>
    {
      this.HideLoading();
      if (e.Code.Equals("GVG002"))
        this.StartCoroutine(this.showPopupOnGvg());
      else
        WebAPI.DefaultUserErrorCallback(e);
      errorCode = e.Code;
    }));
    IEnumerator e1 = ft.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    while (Singleton<PopupManager>.GetInstance().isOpen)
      yield return (object) null;
    if (errorCode.Equals("GLD014"))
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      yield return (object) null;
      MypageScene.ChangeSceneOnError();
    }
    if (ft.Result != null)
    {
      if (Singleton<NGSceneManager>.GetInstance().sceneName.Equals("guild028_2") && this.actionAfterRoleChange != null)
        this.actionAfterRoleChange();
      this.HideLoading();
      this.currentPopup = (GameObject) null;
      this.ShowOkPopup(Consts.Format(Consts.GetInstance().POPUP_GUILD_CHANGE_POSITION_RESULT_TITLE), Consts.Format(Consts.GetInstance().POPUP_GUILD_DIVEST_SUB_MASTER_RESULT), this.divestSubmasterResultDlgSize, (Action) (() => this.StartCoroutine(this.showMemberDetailPopup())));
    }
  }

  public void onDivestSubMasterButton()
  {
    if (PlayerAffiliation.Current.onGvgOperation)
      this.StartCoroutine(this.showPopupOnGvg());
    else
      this.ShowYesNoPopup(Consts.Format(Consts.GetInstance().POPUP_GUILD_CHANGE_POSITION_TITLE), Consts.Format(Consts.GetInstance().POPUP_GUILD_DIVEST_SUB_MASTER), this.divestSubmasterDlgSize, (Action) (() => this.ShowYesNoPopupRecycle(Consts.Format(Consts.GetInstance().POPUP_GUILD_CHANGE_POSITION_CONFIRMATION_TITLE), Consts.Format(Consts.GetInstance().POPUP_GUILD_DIVEST_SUB_MASTER_CONFIRMATION), this.divestSubmasterComfirmDlgSize, (Action) (() => this.StartCoroutine(this.DivestSubMaster())), (Action) (() => this.currentPopup = (GameObject) null))), (Action) (() => { }));
  }

  private IEnumerator ResignSubMaster()
  {
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    while (Singleton<PopupManager>.GetInstance().isOpenNoFinish)
      yield return (object) null;
    this.ShowLoading();
    string errorCode = string.Empty;
    Future<WebAPI.Response.GuildSubmasterResign> ft = WebAPI.GuildSubmasterResign(false, (Action<WebAPI.Response.UserError>) (e =>
    {
      this.HideLoading();
      if (e.Code.Equals("GVG002"))
        this.StartCoroutine(this.showPopupOnGvg());
      else
        WebAPI.DefaultUserErrorCallback(e);
      errorCode = e.Code;
    }));
    IEnumerator e1 = ft.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    while (Singleton<PopupManager>.GetInstance().isOpen)
      yield return (object) null;
    if (errorCode.Equals("GLD014"))
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      yield return (object) null;
      MypageScene.ChangeSceneOnError();
    }
    if (ft.Result != null)
    {
      this.HideGuildFooterBadge();
      if (Singleton<NGSceneManager>.GetInstance().sceneName.Equals("guild028_2") && this.actionAfterRoleChange != null)
        this.actionAfterRoleChange();
      this.HideLoading();
      this.currentPopup = (GameObject) null;
      this.ShowOkPopup(Consts.Format(Consts.GetInstance().POPUP_GUILD_CHANGE_POSITION_RESULT_TITLE), Consts.Format(Consts.GetInstance().POPUP_GUILD_RESIGN_SUB_MASTER_RESULT), this.assignSubmasterResultDlgSize, (Action) (() =>
      {
        if (Object.op_Equality((Object) this.guild282Menu, (Object) null))
          MypageScene.ChangeScene(MypageRootMenu.Mode.GUILD, true);
        else
          this.StartCoroutine(this.showMemberDetailPopup());
      }));
    }
  }

  public void onResignSubMasterButton()
  {
    if (PlayerAffiliation.Current.onGvgOperation)
      this.StartCoroutine(this.showPopupOnGvg());
    else
      this.ShowYesNoPopup(Consts.Format(Consts.GetInstance().POPUP_GUILD_CHANGE_POSITION_TITLE), Consts.Format(Consts.GetInstance().POPUP_GUILD_RESIGN_SUB_MASTER), this.assignSubmasterDlgSize, (Action) (() => this.ShowYesNoPopupRecycle(Consts.Format(Consts.GetInstance().POPUP_GUILD_CHANGE_POSITION_CONFIRMATION_TITLE), Consts.Format(Consts.GetInstance().POPUP_GUILD_RESIGN_SUB_MASTER_CONFIRMATION), this.assignSubmasterComfirmDlgSize, (Action) (() => this.StartCoroutine(this.ResignSubMaster())), (Action) (() => this.currentPopup = (GameObject) null))), (Action) (() => { }));
  }

  private IEnumerator SendDropRequest()
  {
    GuildMemberPositionManagementPopup positionManagementPopup = this;
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    while (Singleton<PopupManager>.GetInstance().isOpenNoFinish)
      yield return (object) null;
    positionManagementPopup.ShowLoading();
    string errorCode = string.Empty;
    Future<WebAPI.Response.GuildMembershipsBanish> ft = WebAPI.GuildMembershipsBanish(false, positionManagementPopup.memberInfo.player.player_id, (Action<WebAPI.Response.UserError>) (e =>
    {
      this.HideLoading();
      if (e.Code.Equals("GVG002"))
        this.StartCoroutine(this.showPopupOnGvg());
      else if (e.Code.Equals("GLD020"))
        this.StartCoroutine(this.showPopupNotAvailableMemberChange());
      else
        WebAPI.DefaultUserErrorCallback(e);
      errorCode = e.Code;
    }));
    IEnumerator e1 = ft.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    while (Singleton<PopupManager>.GetInstance().isOpen)
      yield return (object) null;
    if (errorCode.Equals("GLD014"))
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      yield return (object) null;
      MypageScene.ChangeSceneOnError();
    }
    if (ft.Result != null)
    {
      if (Persist.guildSetting.Exists)
      {
        GuildUtil.setGuildMemberNum(PlayerAffiliation.Current.guild.memberships.Length);
        Persist.guildSetting.Flush();
      }
      positionManagementPopup.HideLoading();
      positionManagementPopup.currentPopup = (GameObject) null;
      positionManagementPopup.ShowOkPopup(Consts.Format(Consts.GetInstance().POPUP_GUILD_DIVEST_MEMBER_RESULT_TITLE), Consts.Format(Consts.GetInstance().POPUP_GUILD_DIVEST_MEMBER_RESULT, (IDictionary) new Hashtable()
      {
        {
          (object) "member",
          (object) positionManagementPopup.memberInfo.player.player_name
        }
      }), positionManagementPopup.vanishMemberResultDlgSize, (Action) (() =>
      {
        if (Singleton<NGSceneManager>.GetInstance().sceneName.Equals("guild028_2"))
        {
          Guild0282Scene.ChangeSceneOrMemberFocus(this.memberInfo, this.guild282Menu);
        }
        else
        {
          if (this.actionAfterRoleChange == null)
            return;
          this.actionAfterRoleChange();
        }
      }));
    }
  }

  public void onDropFromGuildButton()
  {
    if (PlayerAffiliation.Current.onGvgOperation)
      this.StartCoroutine(this.showPopupOnGvg());
    else
      this.ShowYesNoPopup(Consts.Format(Consts.GetInstance().POPUP_GUILD_DIVEST_MEMBER_TITLE), Consts.Format(Consts.GetInstance().POPUP_GUILD_DIVEST_MEMBER, (IDictionary) new Hashtable()
      {
        {
          (object) "member",
          (object) this.memberInfo.player.player_name
        }
      }), this.vanishMemberDlgSize, (Action) (() => this.ShowYesNoPopupRecycle(Consts.Format(Consts.GetInstance().POPUP_GUILD_DIVEST_MEMBER_CONFIRMATION_TITLE), Consts.Format(Consts.GetInstance().POPUP_GUILD_DIVEST_MEMBER_CONFIRMATION), this.vanishMemberComfirmDlgSize, (Action) (() => this.StartCoroutine(this.SendDropRequest())), (Action) (() => this.currentPopup = (GameObject) null))), (Action) (() => { }));
  }

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().dismiss();

  private IEnumerator showMemberDetailPopup()
  {
    if (Object.op_Inequality((Object) this.guild282Menu, (Object) null))
      this.guild282Menu.MemberBaseUpdate();
    GameObject popup = this.guildMemberObjs.GuildMemberInfoPopup.Clone();
    GuildMemberInfoPopup component = popup.GetComponent<GuildMemberInfoPopup>();
    popup.SetActive(false);
    IEnumerator e = component.Initialize(this.memberInfo.player.player_id, this.guild282Menu, this.guildBaseMenu, this.guildMemberObjs, this.actionAfterRoleChange, this.isShowMapButton);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
  }

  private IEnumerator showPopupOnGvg()
  {
    GuildMemberPositionManagementPopup positionManagementPopup = this;
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    while (Singleton<PopupManager>.GetInstance().isOpenNoFinish)
      yield return (object) null;
    // ISSUE: reference to a compiler-generated method
    positionManagementPopup.ShowOkPopup(Consts.Format(Consts.GetInstance().POPUP_GUILD_CHANGE_POSITION_TITLE), PlayerAffiliation.Current.guild.gvg_status == GvgStatus.preparing ? Consts.GetInstance().GUILD_POSITION_CHANGE_UNAVAILABLE_GVG_PREPARATION : Consts.GetInstance().GUILD_POSITION_CHANGE_UNAVAILABLE_GVG, new Vector2(532f, 322f), new Action(positionManagementPopup.\u003CshowPopupOnGvg\u003Eb__69_0));
  }

  private IEnumerator showPopupNotAvailableMemberChange()
  {
    GuildMemberPositionManagementPopup positionManagementPopup = this;
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    while (Singleton<PopupManager>.GetInstance().isOpenNoFinish)
      yield return (object) null;
    // ISSUE: reference to a compiler-generated method
    positionManagementPopup.ShowOkPopup(Consts.GetInstance().POPUP_028_3_2_2_IN_RAID_TITLE, Consts.GetInstance().POPUP_028_3_2_2_IN_RAID_DESC, new Vector2(532f, 322f), new Action(positionManagementPopup.\u003CshowPopupNotAvailableMemberChange\u003Eb__70_0));
  }
}
