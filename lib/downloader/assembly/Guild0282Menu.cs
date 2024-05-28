// Decompiled with JetBrains decompiler
// Type: Guild0282Menu
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
public class Guild0282Menu : BackButtonMenuBase
{
  private Action _actionForGvgPopup;
  [SerializeField]
  private float focusAnimDuration = 0.5f;
  [SerializeField]
  private float cameraChangeDuration = 0.5f;
  [SerializeField]
  private UIButton entryButton;
  [SerializeField]
  private GameObject cloudParent;
  [SerializeField]
  private GameObject guildMap;
  [SerializeField]
  private UIScrollView scrollView;
  [SerializeField]
  private List<SightPattern> sightPattern;
  private int sightUseNumber;
  [SerializeField]
  private GameObject uiStatusUsualObj;
  [SerializeField]
  private StatusUsual uiStatusUsual;
  [SerializeField]
  private GameObject uiStatusReadyObj;
  [SerializeField]
  private GameObject dir_battle_ready;
  [SerializeField]
  private StatusReady uiStatusReady;
  [SerializeField]
  private GameObject uiStatusInBattleObj;
  [SerializeField]
  private StatusInBattle uiStatusInBattle;
  [SerializeField]
  private UILabel txt_not_enough_member;
  [SerializeField]
  private GameObject NotEnoughMemberObj;
  [SerializeField]
  private GameObject WaitingForEnteryToStart;
  [SerializeField]
  private GameObject Matching;
  [SerializeField]
  private GameObject WaitingForEnteryMember;
  [SerializeField]
  private GameObject GuildBattleClose;
  [SerializeField]
  private GameObject OutOfTerm;
  [SerializeField]
  private GameObject RuleBoardObj;
  [SerializeField]
  private UILabel TxtRuleDetail;
  private bool isBattleResultAnimation;
  private bool isBackGuildTop;
  private GuildRegistration myGuild;
  private GuildRegistration enGuild;
  [SerializeField]
  private GuildMapUI myGuildUI;
  [SerializeField]
  private GuildMapUI enGuildUI;
  private Guild0282MemberBaseMenu myGuildMemberMenuPrefab;
  private Guild0282GuildBaseMenu myGuildBaseMenuPrefab;
  private Guild0282MemberBaseMenu enGuildMemberMenuPrefab;
  private Guild0282GuildBaseMenu enGuildBaseMenuPrefab;
  private GameObject guildMemberBasePrefab;
  private GameObject guildMemberBasePrefabForGBResult;
  private GameObject guildBasePrefab;
  private GameObject cloudPrefab;
  private GameObject entryConfirmPrefab;
  [SerializeField]
  private Transform middle;
  private bool isTouchBlock;
  private GuildInfoPopup guildInfoPopup;
  private GuildMemberObject guildMemberPopup;
  private bool isCloud;
  private GuildRegistration selectGuildData;
  private GuildMembership selectMemberData;
  [SerializeField]
  private GameObject cloudAnimParent;
  private Action tweenPositionCompleteAction;
  private Action tweenScaleCompleteAction;
  private GuildImageCache myGuildImageCache;
  private GuildImageCache enGuildImageCache;
  private GuildImageCache memberImageCache;
  [SerializeField]
  private GameObject blackBg;
  [SerializeField]
  private GameObject _dyn_battle_edit;
  private GameObject _guildDefTeamPopup;
  private GameObject _guildAtkTeamPopup;
  public const string EntryPressedButton = "ibtn_Sortie_pressed.png__GUI__guild_common__guild_common_prefab";
  public const string EntryIdleButton = "ibtn_Sortie_idle.png__GUI__guild_common__guild_common_prefab";
  private Coroutine TimeCounter;
  private bool isMatingConnecting;
  [SerializeField]
  private GameObject ibtn_Back;
  private Stack<GvgPopup> gvgPopup = new Stack<GvgPopup>();
  private string[] opposed_player_ids;
  [SerializeField]
  private PlayerSituation dir_player_situation;
  [SerializeField]
  private List<GameObject> AdvantageList;
  [SerializeField]
  private List<string> AdvantagePrefabNameList;
  private List<string> LoadedAdvantagePrefabNameList;
  [SerializeField]
  private List<GameObject> DisAdvantageList;
  [SerializeField]
  private List<string> DisAdvantagePrefabNameList;
  private List<string> LoadedDisAdvantagePrefabNameList;
  private const int Advantegejudge = 6;
  private bool _isClosePopupByBackBtn = true;
  private bool fromBattle;
  private bool isAggregatingPopup;

  public bool isFailedInit { get; private set; }

  public Action actionForGvgPopup => this._actionForGvgPopup;

  public GuildRegistration MyGuild => this.myGuild;

  public GuildRegistration EnGuild => this.enGuild;

  public GuildMapUI MyGuildUI => this.myGuildUI;

  public GuildMapUI EnGuildUI => this.enGuildUI;

  public GameObject dyn_battle_edit => this._dyn_battle_edit;

  public GameObject guildDefTeamPopup => this._guildDefTeamPopup;

  public GameObject guildAtkTeamPopup => this._guildAtkTeamPopup;

  public bool isClosePopupByBackBtn
  {
    set => this._isClosePopupByBackBtn = value;
    get => this._isClosePopupByBackBtn;
  }

  public bool IsOpposedPlayer(string player_id)
  {
    return ((IEnumerable<string>) this.opposed_player_ids).Any<string>((Func<string, bool>) (x => x == player_id));
  }

  private IEnumerator GuildShow()
  {
    Future<WebAPI.Response.GuildShow> guildShow = WebAPI.GuildShow(PlayerAffiliation.Current.guild.guild_id, false);
    IEnumerator e = guildShow.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (guildShow.Result == null)
    {
      this.isFailedInit = true;
      MypageScene.ChangeSceneOnError();
    }
    else
    {
      PlayerAffiliation.Current.guild = guildShow.Result.guild;
      this.opposed_player_ids = guildShow.Result.opposed_player_ids;
      this.myGuild = guildShow.Result.guild;
      this.enGuild = guildShow.Result.opponent;
    }
  }

  public IEnumerator InitializeAsyncUpdate()
  {
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (this.guildInfoPopup == null)
      this.guildInfoPopup = new GuildInfoPopup();
    if (this.guildMemberPopup == null)
      this.guildMemberPopup = new GuildMemberObject();
    e = this.ResourceLoad();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.guildInfoPopup.ResourceLoad();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.guildMemberPopup.ResourceLoad();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    switch (this.MyGuild.gvg_status)
    {
      case GvgStatus.not_enough_member:
      case GvgStatus.lock_entry:
      case GvgStatus.can_entry:
      case GvgStatus.matching:
      case GvgStatus.aggregating:
      case GvgStatus.finished:
      case GvgStatus.out_of_term:
        e = this.InitializeUsualAsync();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      case GvgStatus.preparing:
        e = this.InitializeReadyAsync();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      case GvgStatus.fighting:
        e = this.InitializeInBattleAsync();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
    }
  }

  public IEnumerator InitializeAsync(bool guildBattleResult = false, bool fromBattle = false)
  {
    this.isFailedInit = false;
    this.isAggregatingPopup = false;
    this.fromBattle = fromBattle;
    IEnumerator e = this.GuildShow();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!this.isFailedInit)
    {
      e = ServerTime.WaitSync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (this.guildInfoPopup == null)
        this.guildInfoPopup = new GuildInfoPopup();
      if (this.guildMemberPopup == null)
        this.guildMemberPopup = new GuildMemberObject();
      if (guildBattleResult)
      {
        Future<GameObject> fgObj = Res.Prefabs.guild028_2.MemberBase_for_GB_result.Load<GameObject>();
        e = fgObj.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.guildMemberBasePrefabForGBResult = fgObj.Result;
        fgObj = (Future<GameObject>) null;
      }
      e = this.ResourceLoad();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = this.guildInfoPopup.ResourceLoad();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = this.guildMemberPopup.ResourceLoad();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      switch (this.MyGuild.gvg_status)
      {
        case GvgStatus.not_enough_member:
        case GvgStatus.lock_entry:
        case GvgStatus.can_entry:
        case GvgStatus.matching:
        case GvgStatus.aggregating:
        case GvgStatus.finished:
        case GvgStatus.out_of_term:
          e = this.InitializeUsualAsync();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          break;
        case GvgStatus.preparing:
          e = this.InitializeReadyAsync();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          break;
        case GvgStatus.fighting:
          e = this.InitializeInBattleAsync();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          break;
      }
      if (this.selectMemberData != null && !this.IsMember(this.selectMemberData.player.player_id))
        this.selectMemberData = (GuildMembership) null;
      this.isCloud = false;
      if (Object.op_Inequality((Object) this.cloudAnimParent, (Object) null))
        this.CloudDelete(this.cloudAnimParent.transform);
    }
  }

  private bool IsMember(string plyaer_id)
  {
    int? nullable;
    if (this.MyGuild != null)
    {
      nullable = ((IEnumerable<GuildMembership>) this.MyGuild.memberships).FirstIndexOrNull<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id == plyaer_id));
      if (nullable.HasValue)
        return true;
    }
    if (this.EnGuild == null)
      return false;
    nullable = ((IEnumerable<GuildMembership>) this.EnGuild.memberships).FirstIndexOrNull<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id == plyaer_id));
    return nullable.HasValue;
  }

  private IEnumerator InitializeUsualAsync()
  {
    IEnumerator e = this.uiStatusUsual.ResourceLoad(this.MyGuild);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator InitializeReadyAsync()
  {
    IEnumerator e = this.uiStatusReady.ResourceLoad(this.MyGuild, this.EnGuild);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator InitializeInBattleAsync()
  {
    IEnumerator e = this.uiStatusInBattle.ResourceLoad(this.MyGuild, this.EnGuild);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (this.LoadedAdvantagePrefabNameList == null)
      this.LoadedAdvantagePrefabNameList = new List<string>();
    if (this.LoadedDisAdvantagePrefabNameList == null)
      this.LoadedDisAdvantagePrefabNameList = new List<string>();
    int i;
    GameObject obj;
    string objName;
    Future<GameObject> f;
    for (i = 0; i < this.AdvantageList.Count; ++i)
    {
      obj = this.AdvantageList[i];
      objName = this.AdvantagePrefabNameList[i];
      string path = string.Format("Prefabs/guild028_2/{0}", (object) objName);
      if (!this.LoadedAdvantagePrefabNameList.Contains(objName))
      {
        f = Singleton<ResourceManager>.GetInstance().Load<GameObject>(path);
        e = f.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.LoadedAdvantagePrefabNameList.Add(objName);
        f.Result.Clone(obj.transform);
        obj = (GameObject) null;
        objName = (string) null;
        f = (Future<GameObject>) null;
      }
    }
    for (i = 0; i < this.DisAdvantageList.Count; ++i)
    {
      obj = this.DisAdvantageList[i];
      objName = this.DisAdvantagePrefabNameList[i];
      string path = string.Format("Prefabs/guild028_2/{0}", (object) objName);
      if (!this.LoadedDisAdvantagePrefabNameList.Contains(objName))
      {
        f = Singleton<ResourceManager>.GetInstance().Load<GameObject>(path);
        e = f.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.LoadedDisAdvantagePrefabNameList.Add(objName);
        f.Result.Clone(obj.transform);
        obj = (GameObject) null;
        objName = (string) null;
        f = (Future<GameObject>) null;
      }
    }
  }

  public void Initialize(GuildMembership member)
  {
    this.Initialize();
    this.JumpMember(member);
  }

  public void InitializeGBResult(GuildMembership member, int captureStar)
  {
    this.isBattleResultAnimation = true;
    this.isBackGuildTop = true;
    this.Initialize();
    this.StartCoroutine(this.GBResultAnimation(member, captureStar));
  }

  private IEnumerator GBResultAnimation(GuildMembership member, int captureStar)
  {
    Guild0282Menu guild0282menu = this;
    Guild0282MemberBase ene = guild0282menu.EnGuildUI.memberBaseList.Find((Predicate<Guild0282MemberBase>) (x => x.Member.player.player_id == member.player.player_id));
    if (Object.op_Equality((Object) ene, (Object) null))
    {
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      guild0282menu.isBattleResultAnimation = false;
    }
    else
    {
      Vector3 pos1 = guild0282menu.CalcGuildMapPosition(((Component) ene).transform.parent.localPosition, guild0282menu.sightPattern[0].MapScale);
      guild0282menu.Focus(pos1, guild0282menu.sightPattern[0].MapScale, "");
      yield return (object) new WaitForSeconds(guild0282menu.focusAnimDuration);
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      if (captureStar > 0)
      {
        Guild0282MemberBase memberBase = guild0282menu.guildMemberBasePrefabForGBResult.CloneAndGetComponent<Guild0282MemberBase>(((Component) ene).transform);
        memberBase.Initialize(member, guild0282menu, guild0282menu.memberImageCache, true, guild0282menu.MyGuild.gvg_status);
        Singleton<PopupManager>.GetInstance().open(((Component) memberBase).gameObject, isCloned: true);
        memberBase.PlayAnim(member.own_star + captureStar, member.own_star);
        do
        {
          yield return (object) null;
        }
        while (!memberBase.EndAnimation());
        Singleton<PopupManager>.GetInstance().dismiss();
        memberBase = (Guild0282MemberBase) null;
      }
      Vector3 pos2 = guild0282menu.CalcGuildMapPosition(((Component) ene).transform.parent.localPosition, guild0282menu.sightPattern[guild0282menu.sightUseNumber].MapScale);
      guild0282menu.Focus(pos2, guild0282menu.sightPattern[guild0282menu.sightUseNumber].MapScale, "");
      guild0282menu.isBattleResultAnimation = false;
    }
  }

  public void MemberBaseUpdate()
  {
    foreach (GuildMembership membership in this.MyGuild.memberships)
    {
      GuildMembership member = membership;
      Guild0282MemberBase guild0282MemberBase = this.MyGuildUI.memberBaseList.Find((Predicate<Guild0282MemberBase>) (x => x.Member.player.player_id == member.player.player_id));
      if (Object.op_Inequality((Object) guild0282MemberBase, (Object) null))
        guild0282MemberBase.Initialize(member, this, this.memberImageCache, false, this.MyGuild.gvg_status);
    }
    if (this.EnGuild == null)
      return;
    foreach (GuildMembership membership in this.EnGuild.memberships)
    {
      GuildMembership member = membership;
      Guild0282MemberBase guild0282MemberBase = this.EnGuildUI.memberBaseList.Find((Predicate<Guild0282MemberBase>) (x => x.Member.player.player_id == member.player.player_id));
      if (Object.op_Inequality((Object) guild0282MemberBase, (Object) null))
        guild0282MemberBase.Initialize(member, this, this.memberImageCache, true, this.MyGuild.gvg_status);
    }
  }

  private void CloudCreater(Transform trans)
  {
    if (((Object) trans).name == "cloud_anim_pos")
      this.cloudPrefab.Clone(trans);
    foreach (Transform child in trans.GetChildren())
      this.CloudCreater(child);
  }

  private void CloudDelete(Transform trans)
  {
    if (((Object) trans).name == "cloud(Clone)")
      Object.Destroy((Object) ((Component) trans).gameObject);
    foreach (Transform child in trans.GetChildren())
      this.CloudDelete(child);
  }

  private void UpdateActiveUI()
  {
    switch (this.MyGuild.gvg_status)
    {
      case GvgStatus.not_enough_member:
      case GvgStatus.lock_entry:
      case GvgStatus.can_entry:
      case GvgStatus.matching:
      case GvgStatus.aggregating:
      case GvgStatus.finished:
      case GvgStatus.out_of_term:
        this.uiStatusUsualObj.SetActive(true);
        this.uiStatusReadyObj.SetActive(false);
        this.dir_battle_ready.SetActive(false);
        this.dir_player_situation.SetActive(false);
        this.uiStatusInBattleObj.SetActive(false);
        break;
      case GvgStatus.preparing:
        this.uiStatusUsualObj.SetActive(false);
        this.uiStatusReadyObj.SetActive(true);
        this.dir_battle_ready.SetActive(true);
        this.dir_player_situation.SetActive(false);
        this.uiStatusInBattleObj.SetActive(false);
        break;
      case GvgStatus.fighting:
        this.uiStatusUsualObj.SetActive(false);
        this.uiStatusReadyObj.SetActive(false);
        this.dir_battle_ready.SetActive(false);
        this.dir_player_situation.SetActive(true);
        this.uiStatusInBattleObj.SetActive(true);
        break;
    }
  }

  private void InitializeUI()
  {
    switch (this.MyGuild.gvg_status)
    {
      case GvgStatus.not_enough_member:
      case GvgStatus.lock_entry:
      case GvgStatus.can_entry:
      case GvgStatus.matching:
      case GvgStatus.out_of_term:
        this.uiStatusUsual.MyStatus.SetStatus(this.MyGuild);
        this.SetGVGReleaseEntryHour();
        break;
      case GvgStatus.preparing:
        this.uiStatusReady.SetStatus(this.MyGuild, this.EnGuild);
        this.SetGVGStartHour();
        break;
      case GvgStatus.fighting:
        this.uiStatusInBattle.SetStatus(this.MyGuild, this.EnGuild);
        this.SetGVGEndHour();
        int? nullable = ((IEnumerable<GuildMembership>) this.MyGuild.memberships).FirstIndexOrNull<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id == Player.Current.id));
        if (!nullable.HasValue)
          break;
        this.dir_player_situation.Initialize(this.MyGuild.memberships[nullable.Value], this.guildMemberBasePrefab, this.memberImageCache);
        break;
      case GvgStatus.aggregating:
      case GvgStatus.finished:
        this.StartCoroutine(this.Aggregating());
        this.uiStatusUsual.MyStatus.SetStatus(this.MyGuild);
        this.SetGVGReleaseEntryHour();
        break;
    }
  }

  private void UpdateUI()
  {
    switch (this.MyGuild.gvg_status)
    {
      case GvgStatus.not_enough_member:
      case GvgStatus.lock_entry:
      case GvgStatus.can_entry:
      case GvgStatus.matching:
      case GvgStatus.out_of_term:
        this.uiStatusUsual.MyStatus.SetStatus(this.MyGuild);
        this.SetGVGReleaseEntryHour();
        break;
      case GvgStatus.preparing:
        this.uiStatusReady.UpdateStatus(this.MyGuild, this.EnGuild);
        this.SetGVGStartHour();
        break;
      case GvgStatus.fighting:
        this.uiStatusInBattle.UpdateStatus(this.MyGuild, this.EnGuild);
        this.SetGVGEndHour();
        int? nullable = ((IEnumerable<GuildMembership>) this.MyGuild.memberships).FirstIndexOrNull<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id == Player.Current.id));
        if (!nullable.HasValue)
          break;
        this.dir_player_situation.Initialize(this.MyGuild.memberships[nullable.Value], this.guildMemberBasePrefab, this.memberImageCache);
        break;
      case GvgStatus.aggregating:
      case GvgStatus.finished:
        this.StartCoroutine(this.Aggregating());
        this.uiStatusUsual.MyStatus.SetStatus(this.MyGuild);
        this.SetGVGReleaseEntryHour();
        break;
    }
  }

  private void UpdateMattingUI()
  {
    if (!Player.Current.IsGuildMatingOpen())
    {
      this.uiStatusUsualObj.SetActive(true);
      this.uiStatusReadyObj.SetActive(false);
      this.dir_battle_ready.SetActive(false);
      this.uiStatusInBattleObj.SetActive(false);
      this.NotEnoughMemberObj.SetActive(false);
      this.WaitingForEnteryToStart.SetActive(false);
      this.Matching.SetActive(false);
      this.WaitingForEnteryMember.SetActive(false);
      this.GuildBattleClose.SetActive(true);
      this.OutOfTerm.SetActive(false);
      ((UIButtonColor) this.entryButton).isEnabled = false;
    }
    else
    {
      switch (this.MyGuild.gvg_status)
      {
        case GvgStatus.not_enough_member:
          this.NotEnoughMemberObj.SetActive(true);
          this.WaitingForEnteryToStart.SetActive(false);
          this.Matching.SetActive(false);
          this.WaitingForEnteryMember.SetActive(false);
          this.GuildBattleClose.SetActive(false);
          this.OutOfTerm.SetActive(false);
          ((UIButtonColor) this.entryButton).isEnabled = false;
          this.txt_not_enough_member.SetTextLocalize(Consts.Format(Consts.GetInstance().GUILD_MATING_MIN_MEMBERS_COUNT, (IDictionary) new Hashtable()
          {
            {
              (object) "num",
              (object) GuildUtil.GetGuildSettingInt("GVG_MIN_MEMBERS_COUNT")
            }
          }));
          break;
        case GvgStatus.lock_entry:
        case GvgStatus.aggregating:
        case GvgStatus.finished:
          this.NotEnoughMemberObj.SetActive(false);
          this.WaitingForEnteryToStart.SetActive(true);
          this.Matching.SetActive(false);
          this.WaitingForEnteryMember.SetActive(false);
          this.GuildBattleClose.SetActive(false);
          this.OutOfTerm.SetActive(false);
          ((UIButtonColor) this.entryButton).isEnabled = false;
          break;
        case GvgStatus.can_entry:
          this.NotEnoughMemberObj.SetActive(false);
          this.WaitingForEnteryToStart.SetActive(false);
          this.Matching.SetActive(false);
          this.GuildBattleClose.SetActive(false);
          this.OutOfTerm.SetActive(false);
          int? nullable = ((IEnumerable<GuildMembership>) this.MyGuild.memberships).FirstIndexOrNull<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id == Player.Current.id));
          if (!nullable.HasValue)
            break;
          if (this.MyGuild.memberships[nullable.Value].role != GuildRole.master && this.MyGuild.memberships[nullable.Value].role != GuildRole.sub_master)
          {
            this.WaitingForEnteryMember.SetActive(true);
            ((UIButtonColor) this.entryButton).isEnabled = false;
            break;
          }
          this.WaitingForEnteryMember.SetActive(false);
          ((UIButtonColor) this.entryButton).defaultColor = Color.white;
          ((UIButtonColor) this.entryButton).pressed = Color.white;
          this.entryButton.pressedSprite = "ibtn_Sortie_pressed.png__GUI__guild_common__guild_common_prefab";
          ((Component) this.entryButton).gameObject.SetActive(false);
          ((Component) this.entryButton).gameObject.SetActive(true);
          ((UIButtonColor) this.entryButton).isEnabled = true;
          TweenColor component1 = ((Component) this.entryButton).GetComponent<TweenColor>();
          component1.from = Color.white;
          component1.to = Color.white;
          ((UITweener) component1).ResetToBeginning();
          break;
        case GvgStatus.matching:
          this.NotEnoughMemberObj.SetActive(false);
          this.WaitingForEnteryToStart.SetActive(false);
          this.Matching.SetActive(true);
          this.WaitingForEnteryMember.SetActive(false);
          this.GuildBattleClose.SetActive(false);
          this.OutOfTerm.SetActive(false);
          ((UIButtonColor) this.entryButton).defaultColor = Color.gray;
          ((UIButtonColor) this.entryButton).pressed = Color.gray;
          this.entryButton.pressedSprite = "ibtn_Sortie_idle.png__GUI__guild_common__guild_common_prefab";
          ((UIButtonColor) this.entryButton).isEnabled = true;
          TweenColor component2 = ((Component) this.entryButton).GetComponent<TweenColor>();
          component2.from = Color.gray;
          component2.to = Color.gray;
          ((UITweener) component2).ResetToBeginning();
          break;
        case GvgStatus.preparing:
          this.NotEnoughMemberObj.SetActive(false);
          this.WaitingForEnteryToStart.SetActive(false);
          this.Matching.SetActive(false);
          this.WaitingForEnteryMember.SetActive(false);
          this.GuildBattleClose.SetActive(false);
          this.OutOfTerm.SetActive(false);
          break;
        case GvgStatus.fighting:
          this.NotEnoughMemberObj.SetActive(false);
          this.WaitingForEnteryToStart.SetActive(false);
          this.Matching.SetActive(false);
          this.WaitingForEnteryMember.SetActive(false);
          this.GuildBattleClose.SetActive(false);
          this.OutOfTerm.SetActive(false);
          break;
        case GvgStatus.out_of_term:
          this.NotEnoughMemberObj.SetActive(false);
          this.WaitingForEnteryToStart.SetActive(false);
          this.Matching.SetActive(false);
          this.WaitingForEnteryMember.SetActive(false);
          this.OutOfTerm.SetActive(true);
          this.GuildBattleClose.SetActive(false);
          ((UIButtonColor) this.entryButton).isEnabled = false;
          break;
      }
    }
  }

  private void updateRuleDetail()
  {
    string ruleDetail = PlayerAffiliation.Current.gvgPeriod?.rule_detail;
    if (string.IsNullOrEmpty(ruleDetail))
    {
      this.RuleBoardObj.SetActive(false);
    }
    else
    {
      this.TxtRuleDetail.SetTextLocalize(ruleDetail);
      this.RuleBoardObj.SetActive(true);
    }
  }

  public void Initialize()
  {
    this.IsPush = false;
    if (!this.isCloud)
    {
      this.CloudCreater(this.cloudAnimParent.transform);
      this.isCloud = true;
    }
    this.InitializeMapUI(this.MyGuildUI);
    this.InitializeMapUI(this.EnGuildUI);
    try
    {
      this.sightUseNumber = Persist.guildSetting.Data.sightUseNumber;
    }
    catch (Exception ex)
    {
      Persist.guildSetting.Delete();
      this.sightUseNumber = 0;
    }
    this.SetSightPattern(this.sightUseNumber);
    this.UpdateActiveUI();
    this.InitializeUI();
    this.CreateGuildMap();
    this.UpdateMattingUI();
    this.updateRuleDetail();
    this.AdvantageEffect();
  }

  public void InitializeUpdate()
  {
    this.IsPush = false;
    this.UpdateActiveUI();
    this.UpdateUI();
    this.UpdateGuildMap();
    this.UpdateMattingUI();
    this.updateRuleDetail();
    this.scrollView.UpdateScrollbars(true);
    this.AdvantageEffect();
  }

  private void StopCoroutineTimeCounter()
  {
    this.StopCoroutine("TimeCountSprite");
    this.StopCoroutine("TimeCountText");
  }

  private void SetGVGStartHour()
  {
    this.StopCoroutineTimeCounter();
    this.StartCoroutine(GuildUtil.TimeCountSprite(this.uiStatusReady.slc_Remain_hours, this.uiStatusReady.slc_Remain_minutes, (double) GuildUtil.GVGStartHour(), (Action) (() => this.StartCoroutine(this.MapReload())), (NGMenuBase) this));
  }

  private void SetGVGEndHour()
  {
    this.StopCoroutineTimeCounter();
    this.StartCoroutine(GuildUtil.TimeCountSprite(this.uiStatusInBattle.slc_Remain_hours, this.uiStatusInBattle.slc_Remain_minutes, (double) GuildUtil.GVGEndHour(), (Action) (() => this.StartCoroutine(this.MapReload())), (NGMenuBase) this));
  }

  private void SetGVGReleaseEntryHour()
  {
    this.StopCoroutineTimeCounter();
    this.StartCoroutine(GuildUtil.TimeCountText(this.uiStatusUsual.txt_waiting_for_entery_to_start, Consts.GetInstance().GUILD_MAP_ENTRY_EXPIRED_HOUR, (double) GuildUtil.GVGReleaseEntryHour(), (Action) (() =>
    {
      if (this.MyGuild.gvg_status == GvgStatus.matching)
        ModalWindow.Show(Consts.GetInstance().GUILD_MAP_MATING_AUTO_CANCEL_TITLE, Consts.GetInstance().GUILD_MAP_MATING_AUTO_CANCEL_MESSAGE, (Action) (() => this.StartCoroutine(this.MapReload())));
      else
        this.StartCoroutine(this.MapReload());
    }), (MonoBehaviour) this));
  }

  public void onEndScene()
  {
    try
    {
      Persist.guildSetting.Data.sightUseNumber = this.sightUseNumber;
      Persist.guildSetting.Flush();
    }
    catch (Exception ex)
    {
    }
    this.StopCoroutineTimeCounter();
  }

  public void InitializeJump()
  {
    if (this.selectMemberData == null)
      this.JumpGuildBase();
    else
      this.JumpMember(this.selectMemberData);
  }

  public void JumpGuildBase()
  {
    Vector3 mapScale = this.sightPattern[this.sightUseNumber].MapScale;
    this.CloseLingMenu();
    if (this.MyGuild.gvg_status == GvgStatus.fighting)
      this.Focus(this.CalcGuildMapPosition(((Component) this.EnGuildUI.guildBase).transform.parent.localPosition, mapScale), mapScale, "");
    else
      this.Focus(this.CalcGuildMapPosition(((Component) this.MyGuildUI.guildBase).transform.parent.localPosition, mapScale), mapScale, "");
  }

  public void JumpMember(GuildMembership member)
  {
    if (member == null || !this.IsMember(member.player.player_id))
    {
      this.JumpGuildBase();
    }
    else
    {
      this.CloseLingMenu();
      this.selectMemberData = member;
      Guild0282MemberBase guild0282MemberBase1 = this.MyGuildUI.memberBaseList.Find((Predicate<Guild0282MemberBase>) (x => x.Member.player.player_id == member.player.player_id));
      if (Object.op_Inequality((Object) guild0282MemberBase1, (Object) null))
      {
        this.Focus(this.CalcGuildMapPosition(((Component) guild0282MemberBase1).transform.parent.localPosition, this.sightPattern[0].MapScale), this.sightPattern[0].MapScale, "OpenMyMemberBaseMenu");
      }
      else
      {
        Guild0282MemberBase guild0282MemberBase2 = this.EnGuildUI.memberBaseList.Find((Predicate<Guild0282MemberBase>) (x => x.Member.player.player_id == member.player.player_id));
        if (!Object.op_Inequality((Object) guild0282MemberBase2, (Object) null))
          return;
        this.Focus(this.CalcGuildMapPosition(((Component) guild0282MemberBase2).transform.parent.localPosition, this.sightPattern[0].MapScale), this.sightPattern[0].MapScale, "OpenEnMemberBaseMenu");
      }
    }
  }

  private IEnumerator ResourceLoad()
  {
    Guild0282Menu guild0282Menu = this;
    Future<GameObject> fgObj = (Future<GameObject>) null;
    IEnumerator e;
    if (Object.op_Equality((Object) guild0282Menu.myGuildMemberMenuPrefab, (Object) null))
    {
      fgObj = Res.Prefabs.guild028_2.dir_your_guild_member.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      guild0282Menu.myGuildMemberMenuPrefab = fgObj.Result.Clone(guild0282Menu.middle).GetComponent<Guild0282MemberBaseMenu>();
      guild0282Menu.StartCoroutine(guild0282Menu.myGuildMemberMenuPrefab.StartUp());
      guild0282Menu.StartCoroutine(guild0282Menu.myGuildMemberMenuPrefab.ResourceLoad());
    }
    if (Object.op_Equality((Object) guild0282Menu.myGuildBaseMenuPrefab, (Object) null))
    {
      fgObj = Res.Prefabs.guild028_2.dir_your_guild_base.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      guild0282Menu.myGuildBaseMenuPrefab = fgObj.Result.Clone(guild0282Menu.middle).GetComponent<Guild0282GuildBaseMenu>();
      guild0282Menu.StartCoroutine(guild0282Menu.myGuildBaseMenuPrefab.StartUp());
    }
    if (Object.op_Equality((Object) guild0282Menu.enGuildMemberMenuPrefab, (Object) null))
    {
      fgObj = Res.Prefabs.guild028_2.dir_enemy_guild_member.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      guild0282Menu.enGuildMemberMenuPrefab = fgObj.Result.Clone(guild0282Menu.middle).GetComponent<Guild0282MemberBaseMenu>();
      guild0282Menu.StartCoroutine(guild0282Menu.enGuildMemberMenuPrefab.StartUp());
      guild0282Menu.StartCoroutine(guild0282Menu.enGuildMemberMenuPrefab.ResourceLoad());
    }
    if (Object.op_Equality((Object) guild0282Menu.enGuildBaseMenuPrefab, (Object) null))
    {
      fgObj = Res.Prefabs.guild028_2.dir_enemy_guild_base.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      guild0282Menu.enGuildBaseMenuPrefab = fgObj.Result.Clone(guild0282Menu.middle).GetComponent<Guild0282GuildBaseMenu>();
      guild0282Menu.StartCoroutine(guild0282Menu.enGuildBaseMenuPrefab.StartUp());
    }
    if (Object.op_Equality((Object) guild0282Menu.guildMemberBasePrefab, (Object) null))
    {
      fgObj = Res.Prefabs.guild028_2.MemberBase.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      guild0282Menu.guildMemberBasePrefab = fgObj.Result;
      guild0282Menu.memberImageCache = new GuildImageCache();
      e = guild0282Menu.memberImageCache.GuildFrameAnimResourceLoad();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    if (Object.op_Equality((Object) guild0282Menu.guildBasePrefab, (Object) null))
    {
      fgObj = Res.Prefabs.guild028_2.GuildBase.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      guild0282Menu.guildBasePrefab = fgObj.Result;
    }
    guild0282Menu.myGuildImageCache = new GuildImageCache();
    e = guild0282Menu.myGuildImageCache.ResourceLoad(guild0282Menu.MyGuild.appearance);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (guild0282Menu.IsEnemy())
    {
      guild0282Menu.enGuildImageCache = new GuildImageCache();
      e = guild0282Menu.enGuildImageCache.ResourceLoad(guild0282Menu.EnGuild.appearance);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    if (Object.op_Equality((Object) guild0282Menu.cloudPrefab, (Object) null))
    {
      fgObj = Res.Prefabs.guild028_2.cloud.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      guild0282Menu.cloudPrefab = fgObj.Result;
    }
    while (!guild0282Menu.myGuildMemberMenuPrefab.IsInitialize || !guild0282Menu.myGuildBaseMenuPrefab.IsInitialize || !guild0282Menu.enGuildMemberMenuPrefab.IsInitialize || !guild0282Menu.enGuildMemberMenuPrefab.IsInitialize)
      yield return (object) null;
    if (Object.op_Equality((Object) guild0282Menu.guildDefTeamPopup, (Object) null))
    {
      fgObj = Res.Prefabs.guild028_2.dir_guild_DEFteam.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      guild0282Menu._guildDefTeamPopup = fgObj.Result;
    }
    if (Object.op_Equality((Object) guild0282Menu.guildAtkTeamPopup, (Object) null))
    {
      fgObj = Res.Prefabs.guild028_2.dir_guild_ATKteam.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      guild0282Menu._guildAtkTeamPopup = fgObj.Result;
    }
  }

  private void CreateGuildMap(
    GuildMapUI guildMapUI,
    GuildRegistration guildData,
    GuildImageCache guildImageCache,
    bool isEnemy)
  {
    List<GuildMembership> guildMembershipList = new List<GuildMembership>();
    int? nullable1 = ((IEnumerable<GuildMembership>) guildData.memberships).FirstIndexOrNull<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id == Player.Current.id));
    if (nullable1.HasValue)
      guildMembershipList.Add(guildData.memberships[nullable1.Value]);
    guildData.memberships = ((IEnumerable<GuildMembership>) guildData.memberships).Where<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id != Player.Current.id)).ToArray<GuildMembership>();
    int? nullable2 = ((IEnumerable<GuildMembership>) guildData.memberships).FirstIndexOrNull<GuildMembership>((Func<GuildMembership, bool>) (x => x.role == GuildRole.master));
    if (nullable2.HasValue)
      guildMembershipList.Add(guildData.memberships[nullable2.Value]);
    foreach (GuildMembership guildMembership in (IEnumerable<GuildMembership>) ((IEnumerable<GuildMembership>) guildData.memberships).Where<GuildMembership>((Func<GuildMembership, bool>) (x => x.role == GuildRole.sub_master)).OrderByDescending<GuildMembership, int>((Func<GuildMembership, int>) (x => x.contribution)))
      guildMembershipList.Add(guildMembership);
    foreach (GuildMembership guildMembership in (IEnumerable<GuildMembership>) ((IEnumerable<GuildMembership>) guildData.memberships).Where<GuildMembership>((Func<GuildMembership, bool>) (x => x.role != GuildRole.master && x.role != GuildRole.sub_master)).OrderByDescending<GuildMembership, int>((Func<GuildMembership, int>) (x => x.contribution)))
      guildMembershipList.Add(guildMembership);
    guildData.memberships = guildMembershipList.ToArray();
    guildMapUI.guildBase = this.guildBasePrefab.Clone(guildMapUI.guildBasePosition.transform).GetComponent<Guild0282GuildBase>();
    ((Collider) ((Component) guildMapUI.guildBase).GetComponent<BoxCollider>()).enabled = true;
    guildMapUI.guildBase.Initialize(guildData, this, guildImageCache, isEnemy);
    float num1 = guildData.memberships.Length < 10 ? 1f : (float) guildMapUI.memberBasePosition.Count / (float) guildData.memberships.Length;
    float num2 = 0.0f;
    for (int index = 0; index < guildData.memberships.Length; ++index)
    {
      Guild0282MemberBase component = this.guildMemberBasePrefab.Clone(guildMapUI.memberBasePosition[Mathf.FloorToInt(num2)].transform).GetComponent<Guild0282MemberBase>();
      component.Initialize(guildData.memberships[index], this, this.memberImageCache, isEnemy, this.MyGuild.gvg_status);
      guildMapUI.memberBaseList.Add(component);
      num2 += num1;
    }
  }

  private void UpdateGuildMap(
    GuildMapUI guildMapUI,
    GuildRegistration guildData,
    GuildImageCache guildImageCache,
    bool isEnemy)
  {
    List<GuildMembership> guildMembershipList = new List<GuildMembership>();
    int? nullable1 = ((IEnumerable<GuildMembership>) guildData.memberships).FirstIndexOrNull<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id == Player.Current.id));
    if (nullable1.HasValue)
      guildMembershipList.Add(guildData.memberships[nullable1.Value]);
    guildData.memberships = ((IEnumerable<GuildMembership>) guildData.memberships).Where<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id != Player.Current.id)).ToArray<GuildMembership>();
    int? nullable2 = ((IEnumerable<GuildMembership>) guildData.memberships).FirstIndexOrNull<GuildMembership>((Func<GuildMembership, bool>) (x => x.role == GuildRole.master));
    if (nullable2.HasValue)
      guildMembershipList.Add(guildData.memberships[nullable2.Value]);
    foreach (GuildMembership guildMembership in (IEnumerable<GuildMembership>) ((IEnumerable<GuildMembership>) guildData.memberships).Where<GuildMembership>((Func<GuildMembership, bool>) (x => x.role == GuildRole.sub_master)).OrderByDescending<GuildMembership, int>((Func<GuildMembership, int>) (x => x.contribution)))
      guildMembershipList.Add(guildMembership);
    foreach (GuildMembership guildMembership in (IEnumerable<GuildMembership>) ((IEnumerable<GuildMembership>) guildData.memberships).Where<GuildMembership>((Func<GuildMembership, bool>) (x => x.role != GuildRole.master && x.role != GuildRole.sub_master)).OrderByDescending<GuildMembership, int>((Func<GuildMembership, int>) (x => x.contribution)))
      guildMembershipList.Add(guildMembership);
    guildData.memberships = guildMembershipList.ToArray();
    if (Object.op_Inequality((Object) guildMapUI.guildBase, (Object) null))
      guildMapUI.guildBase.InitializeUpdate(guildData, this, guildImageCache, isEnemy);
    for (int index = 0; index < guildMapUI.memberBaseList.Count; ++index)
      guildMapUI.memberBaseList[index].InitializeUpdate(guildData.memberships[index], this, this.memberImageCache, isEnemy, this.MyGuild.gvg_status);
  }

  private void CreateGuildMap()
  {
    this.CreateGuildMap(this.MyGuildUI, this.MyGuild, this.myGuildImageCache, false);
    this.EnGuildUI.SetActive(this.IsEnemy());
    if (!this.IsEnemy())
      return;
    this.CreateGuildMap(this.EnGuildUI, this.EnGuild, this.enGuildImageCache, true);
  }

  private void UpdateGuildMap()
  {
    this.UpdateGuildMap(this.MyGuildUI, this.MyGuild, this.myGuildImageCache, false);
    this.EnGuildUI.SetActive(this.IsEnemy());
    if (!this.IsEnemy())
      return;
    if (this.EnGuildUI.memberBaseList.Count == 0)
      this.CreateGuildMap(this.EnGuildUI, this.EnGuild, this.enGuildImageCache, true);
    else
      this.UpdateGuildMap(this.EnGuildUI, this.EnGuild, this.enGuildImageCache, true);
  }

  private void SetSightPattern(int number)
  {
    for (int index = 0; index < this.sightPattern.Count; ++index)
      this.sightPattern[index].SightImage.SetActive(index == number);
    this.SetTweenScale(this.guildMap, this.sightPattern[number].MapScale, this.cameraChangeDuration, true);
  }

  private IEnumerator ShowMyMemberList()
  {
    Guild0282Menu menu = this;
    menu.isClosePopupByBackBtn = false;
    menu.DoLingMenuDismiss();
    GameObject popup = menu.guildMemberPopup.GuildMemberListPopup.Clone();
    GuildMemberListPopup component = popup.GetComponent<GuildMemberListPopup>();
    popup.SetActive(false);
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = component.Initialize(false, menu, menu.myGuildBaseMenuPrefab, menu.guildMemberPopup, menu.MyGuild, new Action(menu.\u003CShowMyMemberList\u003Eb__132_0));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
  }

  private IEnumerator ShowEnMemberList()
  {
    Guild0282Menu menu = this;
    menu.isClosePopupByBackBtn = false;
    menu.DoLingMenuDismiss();
    GameObject popup = menu.guildMemberPopup.GuildMemberListPopup.Clone();
    GuildMemberListPopup component = popup.GetComponent<GuildMemberListPopup>();
    popup.SetActive(false);
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = component.Initialize(true, menu, menu.myGuildBaseMenuPrefab, menu.guildMemberPopup, menu.EnGuild, new Action(menu.\u003CShowEnMemberList\u003Eb__133_0));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
  }

  public void BackScene()
  {
    if (this.isBackGuildTop)
      MypageScene.ChangeScene(MypageRootMenu.Mode.GUILD);
    else
      this.backScene();
  }

  public override void onBackButton()
  {
    if (Singleton<PopupManager>.GetInstance().isOpen)
    {
      if (!this.isClosePopupByBackBtn)
        return;
      Singleton<PopupManager>.GetInstance().dismiss();
    }
    else
    {
      if (this.gvgPopup.Count > 0 && this.gvgPopup.Peek().state == GuildUtil.GvGPopupState.FacilityList)
        this.closePopup();
      if (GuildUtil.gvgPopupState != GuildUtil.GvGPopupState.None || this.gvgPopup.Count > 0 && this.gvgPopup.Peek().state != GuildUtil.GvGPopupState.None)
        return;
      if (this.IsLingMenu())
        this.DoLingMenuDismiss();
      else if (Singleton<CommonRoot>.GetInstance().guildChatManager.GetCurrentGuildChatStatus() == GuildChatManager.GuildChatStatus.DetailedView)
        Singleton<CommonRoot>.GetInstance().guildChatManager.OnBackButtonClicked();
      else
        MypageScene.ChangeScene(MypageRootMenu.Mode.GUILD, true);
    }
  }

  public void onButtonSightChange()
  {
    this.CloseLingMenu();
    Vector3 newPos;
    // ISSUE: explicit constructor call
    ((Vector3) ref newPos).\u002Ector(this.guildMap.transform.localPosition.x, this.guildMap.transform.localPosition.y);
    Vector3 vector3_1;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_1).\u002Ector(((Component) this.scrollView).transform.localPosition.x, ((Component) this.scrollView).transform.localPosition.y);
    Vector3 vector3_2;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_2).\u002Ector(this.guildMap.transform.localScale.x, this.guildMap.transform.localScale.y);
    ++this.sightUseNumber;
    if (this.sightPattern.Count <= this.sightUseNumber)
      this.sightUseNumber = 0;
    Vector3 vector3_3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_3).\u002Ector(this.sightPattern[this.sightUseNumber].MapScale.x, this.sightPattern[this.sightUseNumber].MapScale.y);
    // ISSUE: explicit constructor call
    ((Vector3) ref newPos).\u002Ector((newPos.x + vector3_1.x) / vector3_2.x * vector3_3.x - vector3_1.x, (newPos.y + vector3_1.y) / vector3_2.y * vector3_3.y - vector3_1.y);
    this.SetSightPattern(this.sightUseNumber);
    this.SetTweenPosition(this.guildMap, newPos, this.cameraChangeDuration, true);
  }

  public void onButtonDrawMyGuild() => this.StartCoroutine(this.ShowMyMemberList());

  public void onButtonDrawEnGuild() => this.StartCoroutine(this.ShowEnMemberList());

  public void onButtonEntry() => this.UpdateMattingUI();

  protected virtual void OnEnable() => UICamera.fallThrough = ((Component) this).gameObject;

  protected virtual void OnDisable() => UICamera.fallThrough = (GameObject) null;

  public void OnPress(bool isDown)
  {
    if (this.isTouchBlock)
      return;
    this.DoLingMenuDismiss();
  }

  public void OnDrag(Vector2 delta)
  {
    if (this.isTouchBlock)
      return;
    this.DoLingMenuDismiss();
    this.scrollView.UpdateScrollbars(true);
  }

  protected override void Update()
  {
    base.Update();
    Vector3 vector3_1;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_1).\u002Ector(this.guildMap.transform.localPosition.x, this.guildMap.transform.localPosition.y);
    Vector3 vector3_2;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_2).\u002Ector(((Component) this.scrollView).transform.localPosition.x, ((Component) this.scrollView).transform.localPosition.y);
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_1).\u002Ector(vector3_1.x + vector3_2.x, vector3_1.y + vector3_2.y);
    this.cloudParent.transform.localPosition = vector3_1;
    this.cloudParent.transform.localScale = this.guildMap.transform.localScale;
  }

  public void onButtonGuildBase(Guild0282GuildBase data, GuildRegistration guild)
  {
    if (this.isTouchBlock || this.IsLingMenu() && !this.DoLingMenuDismiss() || Object.op_Inequality((Object) this.guildMap.GetComponent<iTween>(), (Object) null))
      return;
    this.selectGuildData = guild;
    Vector3 pos = this.CalcGuildMapPosition(((Component) data).transform.parent.localPosition, this.sightPattern[0].MapScale);
    if (data.IsEnemy)
      this.Focus(pos, this.sightPattern[0].MapScale, "OpenEnGuildBaseMenu");
    else
      this.Focus(pos, this.sightPattern[0].MapScale, "OpenMyGuildBaseMenu");
  }

  public void onButtonMemberBase(Guild0282MemberBase data, GuildMembership member)
  {
    if (this.isTouchBlock || this.IsLingMenu() && !this.DoLingMenuDismiss() || Object.op_Inequality((Object) this.guildMap.GetComponent<iTween>(), (Object) null))
      return;
    this.selectMemberData = member;
    Vector3 pos = this.CalcGuildMapPosition(((Component) data).transform.parent.localPosition, this.sightPattern[0].MapScale);
    if (data.IsEnemy)
      this.Focus(pos, this.sightPattern[0].MapScale, "OpenEnMemberBaseMenu");
    else
      this.Focus(pos, this.sightPattern[0].MapScale, "OpenMyMemberBaseMenu");
  }

  public void Focus(Vector3 pos, Vector3 scale, string functionName)
  {
    if (!string.IsNullOrEmpty(functionName))
      ((Component) this).gameObject.SendMessage(functionName);
    this.SetTweenScale(this.guildMap, scale, this.focusAnimDuration, true);
    this.SetTweenPosition(this.guildMap, pos, this.focusAnimDuration, true);
  }

  private void OnOpenMenu()
  {
    this.ibtn_Back.GetComponent<NGTweenParts>().isActive = false;
    ((IEnumerable<UITweener>) this.dir_player_situation.gameObject.GetComponents<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x => x.PlayReverse()));
    ((IEnumerable<UITweener>) this.dir_battle_ready.GetComponents<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x => x.PlayReverse()));
    Singleton<NGSoundManager>.GetInstance().PlaySe("SE_1002");
  }

  public void OpenMyGuildBaseMenu()
  {
    this.OnOpenMenu();
    ((Component) this.myGuildBaseMenuPrefab).GetComponent<Guild0282GuildBaseMenu>().Initialize(this.selectGuildData, this.guildInfoPopup, this.guildMemberPopup, this);
    this.myGuildBaseMenuPrefab.PlayTween(true);
    this.isTouchBlock = false;
  }

  public void OpenMyMemberBaseMenu()
  {
    this.OnOpenMenu();
    ((Component) this.myGuildMemberMenuPrefab).GetComponent<Guild0282MemberBaseMenu>().Initialize(this.selectMemberData, this.guildMemberPopup, this, false, this.MyGuild.gvg_status);
    this.myGuildMemberMenuPrefab.PlayTween(true);
    this.isTouchBlock = false;
  }

  public void OpenEnGuildBaseMenu()
  {
    this.OnOpenMenu();
    ((Component) this.enGuildBaseMenuPrefab).GetComponent<Guild0282GuildBaseMenu>().Initialize(this.selectGuildData, this.guildInfoPopup, this.guildMemberPopup, this);
    this.enGuildBaseMenuPrefab.PlayTween(true);
    this.isTouchBlock = false;
  }

  public void OpenEnMemberBaseMenu()
  {
    this.OnOpenMenu();
    ((Component) this.enGuildMemberMenuPrefab).GetComponent<Guild0282MemberBaseMenu>().Initialize(this.selectMemberData, this.guildMemberPopup, this, true, this.MyGuild.gvg_status);
    this.enGuildMemberMenuPrefab.PlayTween(true);
    this.isTouchBlock = false;
  }

  private bool CloseLingMenu()
  {
    bool flag = false;
    if (((Component) this.enGuildMemberMenuPrefab).gameObject.activeSelf)
      flag = this.enGuildMemberMenuPrefab.PlayTween(false);
    if (((Component) this.enGuildBaseMenuPrefab).gameObject.activeSelf)
      flag = this.enGuildBaseMenuPrefab.PlayTween(false);
    if (((Component) this.myGuildMemberMenuPrefab).gameObject.activeSelf)
      flag = this.myGuildMemberMenuPrefab.PlayTween(false);
    if (((Component) this.myGuildBaseMenuPrefab).gameObject.activeSelf)
      flag = this.myGuildBaseMenuPrefab.PlayTween(false);
    ((IEnumerable<UITweener>) this.dir_battle_ready.GetComponents<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x => x.PlayForward()));
    ((IEnumerable<UITweener>) this.dir_player_situation.gameObject.GetComponents<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x => x.PlayForward()));
    this.ibtn_Back.GetComponent<NGTweenParts>().isActive = true;
    return flag;
  }

  private bool IsLingMenu()
  {
    return ((Component) this.enGuildMemberMenuPrefab).gameObject.activeSelf || ((Component) this.enGuildBaseMenuPrefab).gameObject.activeSelf || ((Component) this.myGuildMemberMenuPrefab).gameObject.activeSelf || ((Component) this.myGuildBaseMenuPrefab).gameObject.activeSelf;
  }

  public bool DoLingMenuDismiss()
  {
    if (!this.IsLingMenu() || Object.op_Inequality((Object) ((Component) this.scrollView).GetComponent<iTween>(), (Object) null) || !this.CloseLingMenu())
      return false;
    Singleton<NGSoundManager>.GetInstance().PlaySe("SE_1004");
    this.selectMemberData = (GuildMembership) null;
    if (this.sightUseNumber != 0)
    {
      Vector3 newPos;
      // ISSUE: explicit constructor call
      ((Vector3) ref newPos).\u002Ector(this.guildMap.transform.localPosition.x, this.guildMap.transform.localPosition.y);
      Vector3 vector3_1;
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3_1).\u002Ector(((Component) this.scrollView).transform.localPosition.x, ((Component) this.scrollView).transform.localPosition.y);
      Vector3 vector3_2;
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3_2).\u002Ector(this.sightPattern[0].MapScale.x, this.sightPattern[0].MapScale.y);
      Vector3 vector3_3;
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3_3).\u002Ector(this.sightPattern[this.sightUseNumber].MapScale.x, this.sightPattern[this.sightUseNumber].MapScale.y);
      // ISSUE: explicit constructor call
      ((Vector3) ref newPos).\u002Ector((newPos.x + vector3_1.x) / vector3_2.x * vector3_3.x - vector3_1.x, (newPos.y + vector3_1.y) / vector3_2.y * vector3_3.y - vector3_1.y);
      this.SetTweenScale(this.guildMap.gameObject, this.sightPattern[this.sightUseNumber].MapScale, this.focusAnimDuration, true);
      this.SetTweenPosition(this.guildMap, newPos, this.focusAnimDuration, true);
    }
    return true;
  }

  private void InitializeMapUI(GuildMapUI ui)
  {
    if (Object.op_Implicit((Object) ui.guildBase))
      Object.Destroy((Object) ((Component) ui.guildBase).gameObject);
    if (ui.memberBaseList != null)
    {
      ui.memberBaseList.ForEach((Action<Guild0282MemberBase>) (x => Object.Destroy((Object) ((Component) x).gameObject)));
      ui.memberBaseList.Clear();
    }
    else
      ui.memberBaseList = new List<Guild0282MemberBase>();
  }

  private bool SetTweenPosition(
    GameObject target,
    Vector3 newPos,
    float duration,
    bool forced = false,
    Action action = null)
  {
    if (Object.op_Implicit((Object) target.GetComponent<TweenPosition>()))
    {
      if (!forced)
        return false;
      Object.Destroy((Object) target.GetComponent<TweenPosition>());
    }
    this.tweenPositionCompleteAction = action;
    Hashtable hashtable = this.CreateHashtable(newPos, duration, "TweenPositionComplete", target);
    iTween.MoveTo(target, hashtable);
    return true;
  }

  private bool SetTweenScale(
    GameObject target,
    Vector3 newScale,
    float duration,
    bool forced = false,
    Action action = null)
  {
    if (Object.op_Implicit((Object) target.GetComponent<TweenScale>()))
    {
      if (!forced)
        return false;
      Object.Destroy((Object) target.GetComponent<TweenScale>());
    }
    this.tweenScaleCompleteAction = action;
    Hashtable hashtable = this.CreateHashtable(newScale, duration, "TweenScaleComplete", target);
    iTween.ScaleTo(target, hashtable);
    return true;
  }

  private Hashtable CreateHashtable(
    Vector3 setVec3,
    float duration,
    string onComplete,
    GameObject target)
  {
    return new Hashtable()
    {
      {
        (object) "x",
        (object) setVec3.x
      },
      {
        (object) "y",
        (object) setVec3.y
      },
      {
        (object) "time",
        (object) duration
      },
      {
        (object) "islocal",
        (object) true
      },
      {
        (object) "oncomplete",
        (object) onComplete
      },
      {
        (object) "oncompletetarget",
        (object) ((Component) this).gameObject
      }
    };
  }

  private void TweenPositionComplete()
  {
    if (this.tweenPositionCompleteAction == null)
      return;
    this.tweenPositionCompleteAction();
  }

  private void TweenScaleComplete()
  {
    if (this.tweenScaleCompleteAction == null)
      return;
    this.tweenScaleCompleteAction();
  }

  private Vector3 CalcGuildMapPosition(Vector3 pos, Vector3 scale)
  {
    return new Vector3(-pos.x * scale.x - ((Component) this.scrollView).transform.localPosition.x, -pos.y * scale.y - ((Component) this.scrollView).transform.localPosition.y);
  }

  public void SetGvgPopup(GuildUtil.GvGPopupState state, Action action)
  {
    GuildUtil.gvgPopupState = state;
    this._actionForGvgPopup = action;
  }

  public void SetActiveMenuBlack(bool isActive) => this.blackBg.SetActive(isActive);

  public void openPopup(
    GameObject org,
    GuildUtil.GvGPopupState state,
    bool isCloned = false,
    bool isStack = false,
    bool isNonOpenAnime = false)
  {
    if (Object.op_Equality((Object) org, (Object) null))
      return;
    this.blackBg.SetActive(true);
    if (this.gvgPopup.Count > 0 && !isStack)
    {
      for (int index = 0; index < this.gvgPopup.Count; ++index)
        Object.Destroy((Object) this.gvgPopup.Pop().popup);
      this.gvgPopup.Clear();
    }
    GvgPopup gvgPopup = new GvgPopup();
    gvgPopup.state = state;
    gvgPopup.action = this._actionForGvgPopup;
    if (this.gvgPopup.Count > 0)
    {
      NGTweenParts component = this.gvgPopup.Peek().popup.GetComponent<NGTweenParts>();
      if (Object.op_Inequality((Object) component, (Object) null))
        component.forceActive(false);
    }
    if (isCloned)
    {
      gvgPopup.popup = org;
      if (Object.op_Equality((Object) org.transform.parent, (Object) null))
        org.transform.parent = this.dyn_battle_edit.transform;
    }
    else
      gvgPopup.popup = org.Clone(this.dyn_battle_edit.transform);
    this.gvgPopup.Push(gvgPopup);
    gvgPopup.popup.transform.localScale = Vector3.one;
    UIPanel component1 = gvgPopup.popup.GetComponent<UIPanel>();
    if (Object.op_Inequality((Object) component1, (Object) null))
      ((UIRect) component1).SetAnchor(((Component) this).transform);
    if (isNonOpenAnime)
      return;
    NGTweenParts component2 = gvgPopup.popup.GetComponent<NGTweenParts>();
    if (!Object.op_Inequality((Object) component2, (Object) null))
      return;
    component2.forceActive(true);
  }

  public void startOpenAnime(GameObject go)
  {
    NGTweenParts component = go.GetComponent<NGTweenParts>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    component.forceActive(true);
  }

  public void closePopup()
  {
    this.IsPush = true;
    GvgPopup popup = this.gvgPopup.Pop();
    GuildUtil.gvgPopupState = this.gvgPopup.Count > 0 ? this.gvgPopup.Peek().state : GuildUtil.GvGPopupState.None;
    this._actionForGvgPopup = this.gvgPopup.Count > 0 ? this.gvgPopup.Peek().action : (Action) null;
    NGTweenParts componentInChildren1 = popup.popup.GetComponentInChildren<NGTweenParts>(false);
    if (Object.op_Inequality((Object) componentInChildren1, (Object) null))
      componentInChildren1.forceActive(false);
    if (this.gvgPopup.Count > 0)
    {
      NGTweenParts componentInChildren2 = this.gvgPopup.Peek().popup.GetComponentInChildren<NGTweenParts>(false);
      if (Object.op_Inequality((Object) componentInChildren2, (Object) null))
        componentInChildren2.forceActive(true);
    }
    this.StartCoroutine(this.waitForTweenFinish(popup, componentInChildren1));
  }

  public void closeAllPopup()
  {
    if (this.gvgPopup.Count <= 0)
      return;
    for (int index = 0; index < this.gvgPopup.Count - 1; ++index)
    {
      GvgPopup popup = this.gvgPopup.Pop();
      NGTweenParts componentInChildren = popup.popup.GetComponentInChildren<NGTweenParts>(false);
      if (Object.op_Inequality((Object) componentInChildren, (Object) null))
        componentInChildren.forceActive(false);
      this.StartCoroutine(this.waitForTweenFinish(popup, componentInChildren, false));
    }
    this.closePopup();
  }

  public IEnumerator waitForTweenFinish(GvgPopup popup, NGTweenParts tween, bool clearPush = true)
  {
    Guild0282Menu guild0282Menu = this;
    if (Object.op_Inequality((Object) tween, (Object) null))
    {
      while (tween.isActive)
        yield return (object) null;
    }
    Object.Destroy((Object) popup.popup);
    if (guild0282Menu.gvgPopup.Count <= 0)
      guild0282Menu.blackBg.SetActive(false);
    if (clearPush)
      guild0282Menu.IsPush = false;
  }

  public void OnButtonMating()
  {
    int? nullable = ((IEnumerable<GuildMembership>) this.MyGuild.memberships).FirstIndexOrNull<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id == Player.Current.id));
    if (!nullable.HasValue || this.MyGuild.memberships[nullable.Value].role != GuildRole.master && this.MyGuild.memberships[nullable.Value].role != GuildRole.sub_master)
      return;
    switch (this.MyGuild.gvg_status)
    {
      case GvgStatus.can_entry:
        this.IsPush = true;
        ModalWindow.ShowYesNo(Consts.GetInstance().GUILD_MAP_MATING_ENTRY_TITLE, Consts.GetInstance().GUILD_MAP_MATING_ENTRY_MESSAGE, (Action) (() => this.StartCoroutine(this.MatingStart())), (Action) (() => this.IsPush = false));
        break;
      case GvgStatus.matching:
        this.IsPush = true;
        ModalWindow.ShowYesNo(Consts.GetInstance().GUILD_MAP_MATING_CANCEL_TITLE, Consts.GetInstance().GUILD_MAP_MATING_CANCEL_MESSAGE, (Action) (() => this.StartCoroutine(this.MatingCancel())), (Action) (() => this.IsPush = false));
        break;
    }
  }

  public IEnumerator MapReload()
  {
    IEnumerator e = this.InitializeAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.InitializeUpdate();
  }

  public IEnumerator MapReload(GuildEventGvg guildEventGvg)
  {
    this.myGuild = guildEventGvg.guild;
    this.enGuild = guildEventGvg.opponent;
    IEnumerator e = this.InitializeAsyncUpdate();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.InitializeUpdate();
  }

  private IEnumerator MatingStart()
  {
    if (!this.isMatingConnecting)
    {
      this.isMatingConnecting = true;
      IEnumerator e = WebAPI.GvgMatchingEntry().Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = this.InitializeAsync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.Initialize();
      this.isMatingConnecting = false;
    }
  }

  private IEnumerator MatingCancel()
  {
    if (!this.isMatingConnecting)
    {
      this.isMatingConnecting = true;
      IEnumerator e = WebAPI.GvgMatchingCancel().Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = this.InitializeAsync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.Initialize();
      this.isMatingConnecting = false;
    }
  }

  public void onButtonATKTeam() => this.StartCoroutine(this.showAtkTeamPopup());

  public void onButtonDEFTeam() => this.StartCoroutine(this.showDefTeamPopup());

  private IEnumerator showAtkTeamPopup()
  {
    Guild0282Menu menu = this;
    int? nullable = ((IEnumerable<GuildMembership>) menu.MyGuild.memberships).FirstIndexOrNull<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id.Equals(Player.Current.id)));
    if (nullable.HasValue)
    {
      GameObject popup = menu.guildAtkTeamPopup.Clone();
      popup.SetActive(false);
      GuildAtkTeamPopup component = popup.GetComponent<GuildAtkTeamPopup>();
      bool success = false;
      IEnumerator e = component.InitializeAsync(false, menu, menu.MyGuild.memberships[nullable.Value], (Action) (() => success = true));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (success)
      {
        popup.SetActive(true);
        menu.openPopup(popup, GuildUtil.GvGPopupState.AtkTeam, true);
      }
      else
        Object.Destroy((Object) popup);
    }
  }

  private IEnumerator showDefTeamPopup()
  {
    Guild0282Menu menu = this;
    int? nullable = ((IEnumerable<GuildMembership>) menu.MyGuild.memberships).FirstIndexOrNull<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id.Equals(Player.Current.id)));
    if (nullable.HasValue)
    {
      GameObject popup = menu.guildDefTeamPopup.Clone();
      popup.SetActive(false);
      GuildDefTeamPopup component = popup.GetComponent<GuildDefTeamPopup>();
      bool success = false;
      IEnumerator e = component.InitializeAsync(false, menu, menu.MyGuild.memberships[nullable.Value], (Action) (() => success = true));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (success)
      {
        popup.SetActive(true);
        menu.openPopup(popup, GuildUtil.GvGPopupState.DefTeam, true);
      }
      else
        Object.Destroy((Object) popup);
    }
  }

  private IEnumerator Aggregating()
  {
    Guild0282Menu guild0282Menu = this;
    if (!guild0282Menu.isAggregatingPopup)
    {
      guild0282Menu.isAggregatingPopup = true;
      while (guild0282Menu.isBattleResultAnimation)
        yield return (object) null;
      // ISSUE: reference to a compiler-generated method
      ModalWindow.Show(Consts.GetInstance().GUILD_BATTLE_RESULT_NOWAGGREGATING_TITLE, Consts.GetInstance().GUILD_BATTLE_RESULT_NOWAGGREGATING_MESSAGE, new Action(guild0282Menu.\u003CAggregating\u003Eb__180_0));
    }
  }

  private bool IsEnemy()
  {
    return GuildUtil.isBattleOrPreparing(this.MyGuild.gvg_status) && this.EnGuild != null;
  }

  private void AdvantageEffect()
  {
    if (!GuildUtil.isBattle(this.MyGuild.gvg_status))
      return;
    int myStar = 0;
    int enStar = 0;
    ((IEnumerable<GuildMembership>) this.MyGuild.memberships).ForEach<GuildMembership>((Action<GuildMembership>) (x => myStar += x.own_star));
    ((IEnumerable<GuildMembership>) this.EnGuild.memberships).ForEach<GuildMembership>((Action<GuildMembership>) (x => enStar += x.own_star));
    int num = myStar - enStar;
    if (num >= 6)
    {
      this.AdvantageList.ForEach((Action<GameObject>) (x => x.SetActive(true)));
      this.DisAdvantageList.ForEach((Action<GameObject>) (x => x.SetActive(false)));
    }
    else if (num <= -6)
    {
      this.AdvantageList.ForEach((Action<GameObject>) (x => x.SetActive(false)));
      this.DisAdvantageList.ForEach((Action<GameObject>) (x => x.SetActive(true)));
    }
    else
    {
      this.AdvantageList.ForEach((Action<GameObject>) (x => x.SetActive(false)));
      this.DisAdvantageList.ForEach((Action<GameObject>) (x => x.SetActive(false)));
    }
  }
}
