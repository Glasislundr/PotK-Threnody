// Decompiled with JetBrains decompiler
// Type: MyPageGuildMenu
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
public class MyPageGuildMenu : MyPageSubMenu
{
  [Header("Guild Infomation")]
  [SerializeField]
  private UILabel mLevel;
  [SerializeField]
  private UILabel mNextExp;
  [SerializeField]
  private NGTweenGaugeScale mExpGauge;
  [SerializeField]
  private UILabel mMemberCount;
  [SerializeField]
  private UILabel mMemberCountMax;
  [SerializeField]
  private GameObject mMasterBadge;
  [Header("Bulletin Board")]
  [SerializeField]
  private UI2DSprite mGuildEmblem;
  [SerializeField]
  private UILabel mMessageLbl;
  [SerializeField]
  private UIButton mMessageEditBtn;
  [SerializeField]
  private Transform mMessageEnd;
  [Header("Guild Base")]
  [SerializeField]
  private UILabel mGuildName;
  [SerializeField]
  private Guild0281FacilityInfoController mFacilityInfoController;
  [Header("Button Controller")]
  [SerializeField]
  private GuildHuntingButton mHuntingButton;
  [SerializeField]
  private GuildRaidButton mRaidButton;
  [SerializeField]
  private GuildBattleButton mBattleButton;
  [SerializeField]
  private GameObject mManagementBadge;
  [Header("Anchor")]
  [SerializeField]
  private Transform mGuildBaseAnchor;
  [SerializeField]
  private Transform mUi3dModelAnchor;
  [SerializeField]
  private Transform[] mCloudAnchors;
  [Header("Resource")]
  [SerializeField]
  private Material mUi3dModelMaterial;
  [Header("Windows Adjust Parts")]
  [SerializeField]
  private GameObject[] mWinAdjObj = new GameObject[0];
  [SerializeField]
  private Vector3[] mWinAdjPos = new Vector3[0];
  [SerializeField]
  private Vector3[] mWinAdjScl = new Vector3[0];
  private GameObject mGuildBasePrefab;
  private GameObject mFacilityLvUpAnimPrafeb;
  private GameObject mBBSViewDialogPrefab;
  private GameObject mBBSEditorDialogPrefab;
  private GameObject mBattleLogPopupPrefab;
  private GameObject mBattleMemberScorePopupPrefab;
  private Guild0282GuildBase mGuildBase;
  private Vector3 mGuildBaseRootPos;
  private GuildMemberObject mGuildMemberObj;
  private GuildInfoPopup mGuildInfoPopup;
  private GuildImageCache mGuildImageCache;
  private GuildRegistration mGuildData;
  private EventInfo mEventInfo;
  private UI3DModel mUi3dModel;
  private int mUi3dModelUnitId = -1;
  private long mUi3dModelUnitRev = -1;
  private long mUi3dModelDeckRev = -1;

  public IEnumerator InitSceneAsync(MypageRootMenu rootMenu)
  {
    MyPageGuildMenu parent = this;
    parent.RootMenu = rootMenu;
    parent.SoundMgr = Singleton<NGSoundManager>.GetInstance();
    parent.mGuildImageCache = new GuildImageCache();
    IEnumerator e = parent.LoadPopupPrefabs();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = parent.CreateGuildBase();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = parent.CreateUi3dModel();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = parent.CreateCloudEffect();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = parent.mFacilityInfoController.InitializeAsync(parent);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    parent.AdjustPartsOnWindows();
  }

  private IEnumerator LoadPopupPrefabs()
  {
    Future<GameObject> ft = Res.Prefabs.guild028_2.dir_guildbase_main_facility_level_up_anim.Load<GameObject>();
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mFacilityLvUpAnimPrafeb = ft.Result;
    ft = (Future<GameObject>) null;
    ft = Res.Prefabs.popup.popup_028_guild_chat_bbs__anim_popup01.Load<GameObject>();
    e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mBBSViewDialogPrefab = ft.Result;
    ft = (Future<GameObject>) null;
    ft = Res.Prefabs.popup.popup_028_guild_chat_bbs_edit__anim_popup01.Load<GameObject>();
    e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mBBSEditorDialogPrefab = ft.Result;
    ft = (Future<GameObject>) null;
  }

  private IEnumerator CreateGuildBase()
  {
    Future<GameObject> ft = Res.Prefabs.guild028_2.GuildBase.Load<GameObject>();
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mGuildBasePrefab = ft.Result;
    this.mGuildBase = this.mGuildBasePrefab.CloneAndGetComponent<Guild0282GuildBase>(this.mGuildBaseAnchor);
    ((Collider) ((Component) this.mGuildBase).GetComponent<BoxCollider>()).enabled = false;
    this.mGuildBaseRootPos = this.mGuildBase.GetRootPosition();
  }

  private IEnumerator CreateUi3dModel()
  {
    Future<GameObject> ft = Res.Prefabs.gacha006_8.slc_3DModel.Load<GameObject>();
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mUi3dModel = ft.Result.CloneAndGetComponent<UI3DModel>(this.mUi3dModelAnchor);
    this.mUi3dModel.SetScale = 220f;
    ((UIWidget) this.mUi3dModel.uiTexture).depth = 5;
  }

  private IEnumerator CreateCloudEffect()
  {
    Future<GameObject> ft = Res.Prefabs.guild028_2.cloud.Load<GameObject>();
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    foreach (Transform mCloudAnchor in this.mCloudAnchors)
      this.CloneCloudEffects(ft.Result, mCloudAnchor);
  }

  private void CloneCloudEffects(GameObject prefab, Transform anchor)
  {
    if (((Object) anchor).name == "cloud_anim_pos")
      prefab.Clone(anchor);
    foreach (Transform child in anchor.GetChildren())
      this.CloneCloudEffects(prefab, child);
  }

  private void AdjustPartsOnWindows()
  {
    for (int index = 0; index < this.mWinAdjObj.Length; ++index)
    {
      this.mWinAdjObj[index].transform.localPosition = this.mWinAdjPos[index];
      this.mWinAdjObj[index].transform.localScale = this.mWinAdjScl[index];
    }
  }

  public IEnumerator OnStartSceneAsync()
  {
    MyPageGuildMenu myPageGuildMenu = this;
    PlayerAffiliation current = PlayerAffiliation.Current;
    if ((current != null ? (!current.isGuildMember() ? 1 : 0) : 1) != 0)
    {
      myPageGuildMenu.mGuildData = (GuildRegistration) null;
      myPageGuildMenu.mEventInfo = (EventInfo) null;
    }
    else
    {
      myPageGuildMenu.mGuildData = current.guild;
      myPageGuildMenu.mEventInfo = Array.Find<EventInfo>(myPageGuildMenu.RootMenu.GuildTopResponse.event_infos, (Predicate<EventInfo>) (x => x.IsGuild()));
      myPageGuildMenu.UpdateRoleRelationObject();
      myPageGuildMenu.mHuntingButton.UpdateButtonState(myPageGuildMenu.mEventInfo);
      IEnumerator e = myPageGuildMenu.mRaidButton.UpdateButtonState(myPageGuildMenu.RootMenu.GuildTopResponse);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      myPageGuildMenu.mBattleButton.UpdateButtonState();
      e = myPageGuildMenu.mGuildImageCache.ResourceLoad(myPageGuildMenu.mGuildData.appearance);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      myPageGuildMenu.mGuildBase.SetSprite(myPageGuildMenu.mGuildData.appearance, myPageGuildMenu.mGuildImageCache, myPageGuildMenu.mFacilityLvUpAnimPrafeb);
      myPageGuildMenu.mGuildBase.AdjustRootPositionByFortress(myPageGuildMenu.mGuildBaseRootPos);
      myPageGuildMenu.mGuildName.SetTextLocalize(myPageGuildMenu.mGuildData.guild_name);
      myPageGuildMenu.UpdateGuildInfomation();
      e = myPageGuildMenu.UpdateGuildEmblem();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      myPageGuildMenu.UpdateGuildBulletinBoardMessage();
      myPageGuildMenu.mFacilityInfoController.RefreshGuildFacilityStatus();
      e = myPageGuildMenu.UpdateUi3dModel();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public void OnIntroFinished()
  {
    ((Component) this.mUi3dModel.ModelCamera).gameObject.SetActive(true);
  }

  public void OnEndScene() => ((Component) this.mUi3dModel.ModelCamera).gameObject.SetActive(false);

  public void OnPushGuildBase() => this.RootMenu.SwitchGuildMenu();

  public void OnPushBBSView()
  {
    if (this.RootMenu.IsPushAndSet())
      return;
    GuildChatBBSViewerController component = this.mBBSViewDialogPrefab.Clone().GetComponent<GuildChatBBSViewerController>();
    component.InitializeBBSViewerDialog();
    Singleton<PopupManager>.GetInstance().open(((Component) component).gameObject, isCloned: true);
  }

  public void OnPushBBSEdit()
  {
    if (this.RootMenu.IsPushAndSet())
      return;
    GuildChatBBSEditorController component = this.mBBSEditorDialogPrefab.Clone().GetComponent<GuildChatBBSEditorController>();
    component.InitializeBBSEditorDialog((Action) (() =>
    {
      this.mGuildData = PlayerAffiliation.Current.guild;
      this.UpdateGuildBulletinBoardMessage();
    }));
    Singleton<PopupManager>.GetInstance().open(((Component) component).gameObject, isCloned: true);
    ((Component) component).gameObject.SetActive(false);
    ((Component) component).gameObject.SetActive(true);
  }

  public void OnPushGuildHunting()
  {
    if (this.RootMenu.IsPushAndSet())
      return;
    this.SoundMgr?.playSE("SE_1002");
    Quest00230Scene.ChangeScene(true, this.mEventInfo);
  }

  public void OnPushRaidMap()
  {
    if (this.RootMenu.IsPushAndSet())
      return;
    this.SoundMgr?.playSE("SE_1002");
    RaidTopScene.ChangeScene();
  }

  public void OnPushGuildMap()
  {
    if (this.RootMenu.IsPushAndSet())
      return;
    this.SoundMgr?.playSE("SE_1002");
    Guild0282Scene.ChangeScene();
  }

  public void OnPushGuildManagement()
  {
    if (this.RootMenu.IsPushAndSet())
      return;
    Guild0283Scene.ChangeScene();
  }

  public void OnPushGuildMember()
  {
    if (this.RootMenu.IsPushAndSet())
      return;
    if (Persist.guildSetting.Exists)
    {
      GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.changeRole, false);
      GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.newMember, false);
      Persist.guildSetting.Flush();
    }
    this.StartCoroutine(this.StartOpenGuildMemberPopup());
  }

  private IEnumerator StartOpenGuildMemberPopup()
  {
    MyPageGuildMenu myPageGuildMenu = this;
    IEnumerator e;
    if (myPageGuildMenu.mGuildMemberObj == null)
    {
      myPageGuildMenu.mGuildMemberObj = new GuildMemberObject();
      e = myPageGuildMenu.mGuildMemberObj.ResourceLoad();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    GuildMemberListPopup popup = myPageGuildMenu.mGuildMemberObj.GuildMemberListPopup.CloneAndGetComponent<GuildMemberListPopup>();
    ((Component) popup).transform.localScale = Vector3.zero;
    e = popup.Initialize(myPageGuildMenu.mGuildMemberObj, new Action(myPageGuildMenu.OnPushGuildMember));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(((Component) popup).gameObject, isCloned: true);
    ((Component) popup).gameObject.SetActive(false);
    ((Component) popup).gameObject.SetActive(true);
  }

  public void OnPushGuildInfo()
  {
    if (this.RootMenu.IsPushAndSet())
      return;
    this.StartCoroutine(this.StartOpenGuildInfoPopup());
  }

  private IEnumerator StartOpenGuildInfoPopup()
  {
    MyPageGuildMenu myPageGuildMenu = this;
    if (myPageGuildMenu.mGuildInfoPopup == null)
    {
      myPageGuildMenu.mGuildInfoPopup = new GuildInfoPopup();
      myPageGuildMenu.mGuildInfoPopup.SetSendRequestCallback(new Action(myPageGuildMenu.OnPushGuildInfo));
      myPageGuildMenu.mGuildInfoPopup.SetCancelRequestCallback(new Action(myPageGuildMenu.OnPushGuildInfo));
      IEnumerator e = myPageGuildMenu.mGuildInfoPopup.ResourceLoad();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    Guild028114Popup component = myPageGuildMenu.mGuildInfoPopup.guildInfoPopup.CloneAndGetComponent<Guild028114Popup>();
    ((Component) component).transform.localScale = Vector3.zero;
    component.Initialize(myPageGuildMenu.mGuildInfoPopup);
    Singleton<PopupManager>.GetInstance().open(((Component) component).gameObject, isCloned: true);
    ((Component) component).gameObject.SetActive(false);
    ((Component) component).gameObject.SetActive(true);
  }

  public void OnPushBattleLog()
  {
    if (this.RootMenu.IsPushAndSet())
      return;
    this.StartCoroutine(this.StartOpenBattleLog());
  }

  private IEnumerator StartOpenBattleLog()
  {
    Future<GameObject> popupF;
    IEnumerator e;
    if (Object.op_Equality((Object) this.mBattleLogPopupPrefab, (Object) null))
    {
      popupF = Res.Prefabs.popup.popup_028_guild_battle_records__anim_popup01.Load<GameObject>();
      e = popupF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mBattleLogPopupPrefab = popupF.Result;
      popupF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.mBattleMemberScorePopupPrefab, (Object) null))
    {
      popupF = Res.Prefabs.popup.popup_028_guild_member_battle_records__anim_popup01.Load<GameObject>();
      e = popupF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mBattleMemberScorePopupPrefab = popupF.Result;
      popupF = (Future<GameObject>) null;
    }
    GuildBattleRecordPopup popup = this.mBattleLogPopupPrefab.CloneAndGetComponent<GuildBattleRecordPopup>();
    ((Component) popup).transform.localScale = Vector3.zero;
    bool success = false;
    e = popup.InitializeAsync(false, this.mGuildData.guild_id, this.mBattleMemberScorePopupPrefab, (Action) (() => success = true));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (success)
    {
      Singleton<PopupManager>.GetInstance().open(((Component) popup).gameObject, isCloned: true);
      ((Component) popup).gameObject.SetActive(false);
      ((Component) popup).gameObject.SetActive(true);
      popup.InitScrollPosition();
    }
    else
      Object.Destroy((Object) ((Component) popup).gameObject);
  }

  private void UpdateGuildInfomation()
  {
    GuildAppearance appearance = this.mGuildData.appearance;
    this.mLevel.SetTextLocalize(appearance.level);
    this.mNextExp.SetTextLocalize(Consts.Format(Consts.GetInstance().Guild0281MENU_NEXTEXP, (IDictionary) new Hashtable()
    {
      {
        (object) "nextExp",
        (object) appearance.experience_next
      }
    }));
    this.mMemberCount.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_1_1_4_CURRENT_MEMBER, (IDictionary) new Hashtable()
    {
      {
        (object) "currentMember",
        (object) appearance.membership_num
      }
    }));
    this.mMemberCountMax.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_1_1_4_MAX_MEMBER, (IDictionary) new Hashtable()
    {
      {
        (object) "maxMember",
        (object) appearance.membership_capacity
      }
    }));
    if (appearance.experience_next > 0)
      this.mExpGauge.setValue(appearance.experience, appearance.experience + appearance.experience_next);
    else
      this.mExpGauge.setValue(0, 1);
  }

  private void UpdateGuildBulletinBoardMessage()
  {
    bool flag = false;
    string text = this.mGuildData.private_message;
    if (text.Length > 67)
    {
      text = text.Substring(0, 67);
      flag = true;
    }
    this.mMessageLbl.SetTextLocalize(text);
    int characterIndexAtPosition = this.mMessageLbl.GetCharacterIndexAtPosition(this.mMessageEnd.position);
    if (text.Length > characterIndexAtPosition)
    {
      text = text.Substring(0, characterIndexAtPosition);
      flag = true;
    }
    if (flag)
      text += Consts.GetInstance().GUILD_CHAT_ELLIPSIS;
    this.mMessageLbl.SetTextLocalize(text);
  }

  private IEnumerator UpdateGuildEmblem()
  {
    Future<Sprite> ft = EmblemUtility.LoadGuildEmblemSprite(this.mGuildData.appearance._current_emblem);
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) ft.Result, (Object) null))
      this.mGuildEmblem.sprite2D = ft.Result;
  }

  private void UpdateRoleRelationObject()
  {
    GuildMembership guildMembership = Array.Find<GuildMembership>(this.mGuildData.memberships, (Predicate<GuildMembership>) (x => x.player.player_id == Player.Current.id));
    ((Component) this.mMessageEditBtn).gameObject.SetActive(guildMembership != null && guildMembership.role == GuildRole.master);
    if (guildMembership != null)
    {
      bool flag1 = guildMembership.role == GuildRole.master;
      bool flag2 = guildMembership.role == GuildRole.master || guildMembership.role == GuildRole.sub_master;
      this.mMasterBadge.SetActive(flag1);
      ((Component) this.mMessageEditBtn).gameObject.SetActive(flag1);
      this.mManagementBadge.SetActive(flag2 && GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.newApplicant));
    }
    else
    {
      this.mMasterBadge.SetActive(false);
      ((Component) this.mMessageEditBtn).gameObject.SetActive(false);
      this.mManagementBadge.SetActive(false);
    }
  }

  private IEnumerator UpdateUi3dModel()
  {
    int playerUnitId = MypageUnitUtil.getUnitId();
    long unitRev = SMManager.Revision<PlayerUnit>();
    long deckRev = SMManager.Revision<PlayerDeck[]>();
    if (playerUnitId != this.mUi3dModelUnitId || unitRev != this.mUi3dModelUnitRev || deckRev != this.mUi3dModelDeckRev)
    {
      PlayerUnit unit = (PlayerUnit) null;
      if (playerUnitId != 0)
        unit = Array.Find<PlayerUnit>(SMManager.Get<PlayerUnit[]>(), (Predicate<PlayerUnit>) (x => x.id == playerUnitId));
      if (unit == (PlayerUnit) null)
        unit = SMManager.Get<PlayerDeck[]>()[0].player_units[0];
      IEnumerator e = this.mUi3dModel.GuildUnit(unit, this.mUi3dModelMaterial);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mUi3dModelUnitId = unit.id;
      this.mUi3dModelUnitRev = unitRev;
      this.mUi3dModelDeckRev = deckRev;
      unit = (PlayerUnit) null;
    }
  }

  public IEnumerator OnFacilityLvup(GuildBaseType type)
  {
    this.mGuildData = PlayerAffiliation.Current.guild;
    IEnumerator e = this.mGuildImageCache.ResourceLoad(this.mGuildData.appearance);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mGuildBase.SetSprite(this.mGuildData.appearance, this.mGuildImageCache, this.mFacilityLvUpAnimPrafeb);
    this.mGuildBase.AdjustRootPositionByFortress(this.mGuildBaseRootPos);
    this.mGuildBase.HQLevelUp(type, this.mGuildImageCache);
  }
}
