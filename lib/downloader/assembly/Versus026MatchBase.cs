// Decompiled with JetBrains decompiler
// Type: Versus026MatchBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using Net;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Versus026MatchBase : BackButtonMenuBase
{
  public static readonly int PVP_TUTORIAL_FRIEND_END_PAGE = 100;
  [SerializeField]
  protected UILabel txtLeaderSkill;
  [SerializeField]
  protected UILabel txtPass;
  [SerializeField]
  protected UILabel txtTimeLimit;
  [SerializeField]
  protected UILabel txtTotalPower;
  [SerializeField]
  protected UILabel txtTitle;
  [SerializeField]
  protected UILabel txtBonus;
  [SerializeField]
  protected GameObject dirScrollText;
  [SerializeField]
  protected GameObject bgCharacter;
  [SerializeField]
  protected GameObject[] unitIcons;
  [SerializeField]
  protected GameObject dirBonus;
  [SerializeField]
  protected UIButton btnBreakRepair;
  [SerializeField]
  protected UIButton btnStartMatch;
  [SerializeField]
  protected GameObject objButtonDeckEdit;
  [Header("StatusBoards Control (～ TopChangeStatusBoards)")]
  [SerializeField]
  protected UICenterOnChild uiCenter;
  [SerializeField]
  protected Versus026MatchBase.StatusBoard[] statusBoards;
  [SerializeField]
  protected GameObject topChangeStatusBoards;
  [Space(10f)]
  [SerializeField]
  [Tooltip("リーダーの「X」表示位置")]
  protected Vector2 posLeaderRegulation;
  protected bool isFriendMatch;
  protected PvpMatchingTypeEnum type;
  protected WebAPI.Response.PvpBoot pvpInfo;
  protected DeckInfo deckInfo;
  protected PvpRulePeriod rulePeriod;
  protected Func<PlayerUnit, bool> ruleChecker;
  private bool isMatched;
  protected bool isRepair;
  protected bool isCancel;
  private bool isCloseFriendMatchPopup;
  private BattleInfo battleInfo;
  private GameObject scrollTextPrefab;
  private int currentStatusBoard;

  public virtual IEnumerator Init(PvpMatchingTypeEnum type, WebAPI.Response.PvpBoot pvpInfo)
  {
    this.pvpInfo = pvpInfo;
    this.deckInfo = this.GetDeck();
    this.isMatched = false;
    this.isRepair = false;
    this.type = type;
    this.ruleChecker = (Func<PlayerUnit, bool>) null;
    switch (type)
    {
      case PvpMatchingTypeEnum.friend:
      case PvpMatchingTypeEnum.guest:
        this.isFriendMatch = true;
        break;
      default:
        this.isFriendMatch = false;
        if (type == PvpMatchingTypeEnum.class_match)
        {
          int? remainingTime = pvpInfo.rule?.remaining_time;
          if (0 < remainingTime.GetValueOrDefault() & remainingTime.HasValue && MasterData.PvpRulePeriod.TryGetValue(pvpInfo.rule.rule_period_id, out this.rulePeriod))
          {
            this.ruleChecker = BattleUnitRule.createChecker(this.rulePeriod.rule_no);
            break;
          }
          break;
        }
        break;
    }
    if (this.ruleChecker == null)
      this.rulePeriod = (PvpRulePeriod) null;
    IEnumerator e = this.SetLeaderSkillInfo();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.SetBgCharacter();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.SetUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) this.uiCenter, (Object) null))
    {
      int countWait = 0;
      while (!((Component) this.uiCenter).gameObject.activeInHierarchy)
      {
        ++countWait;
        yield return (object) null;
      }
      if (countWait != 0)
        yield return (object) null;
    }
    this.setStatusBoards();
    this.SetMatchButton();
  }

  public virtual void IbtnStartMatch()
  {
    if (this.IsPushAndSet())
      return;
    if (!this.isCompletedOverkillersDeck)
      this.errorMatchingOverkillers();
    else if (this.isMatched)
    {
      Debug.LogError((object) "Already connect matching");
    }
    else
    {
      if (this.GetBoolCostOver())
        return;
      this.StartCoroutine(this.PopupConfirmationMatching());
    }
  }

  protected virtual bool isCompletedOverkillersDeck
  {
    get
    {
      bool bCompleted = true;
      if (this.deckInfo != null)
      {
        if (this.deckInfo.isCustom)
          OverkillersUtil.checkCompletedCustomDeck(this.deckInfo.player_units, out bCompleted);
        else
          OverkillersUtil.checkCompletedDeck(this.deckInfo.player_units, out bCompleted);
      }
      return bCompleted;
    }
  }

  private void errorMatchingOverkillers()
  {
    Consts instance = Consts.GetInstance();
    this.StartCoroutine(PopupCommon.Show(instance.QUEST_0028_ERROR_TITLE_OVERKILLERS, instance.QUEST_0028_ERROR_MESSAGE_OVERKILLERS));
  }

  protected bool[] getUnitRegulation()
  {
    PlayerUnit[] playerUnits = this.deckInfo.player_units;
    Func<PlayerUnit, bool> func = this.ruleChecker != null ? (Func<PlayerUnit, bool>) (x => !(x != (PlayerUnit) null) || this.ruleChecker(x)) : (Func<PlayerUnit, bool>) (y => true);
    bool[] unitRegulation = new bool[playerUnits.Length];
    for (int index = 0; index < playerUnits.Length; ++index)
      unitRegulation[index] = func(playerUnits[index]);
    return unitRegulation;
  }

  public virtual void IbtnOrganization()
  {
    if (this.IsPushAndSet())
      return;
    this.popupDeckEditSelect();
  }

  private void popupDeckEditSelect()
  {
    PopupBattleEditSelect.show(new Action(this.changeSceneNormalDeckEdit), new Action(this.changeSceneCustomDeckEdit));
  }

  private void changeSceneNormalDeckEdit()
  {
    Unit0046Scene.changeSceneVersus(true);
    Unit0046Scene.isQuestEdit = false;
  }

  private void changeSceneCustomDeckEdit()
  {
    EditCustomDeckScene.changeScene(true, bSetFromScene: true);
  }

  public virtual void IbtnWarExperience()
  {
  }

  public void IbtnRepair()
  {
    if (this.IsPushAndSet())
      return;
    Bugu00524Scene.ChangeScene(true);
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet() || this.isMatched)
      return;
    this.backScene();
  }

  protected override void backScene()
  {
    Singleton<NGSceneManager>.GetInstance().clearStack();
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
    Singleton<NGSceneManager>.GetInstance().changeScene("versus026_1", false);
  }

  protected void SetMatchButton()
  {
    GameObject[] gameObjectArray = new GameObject[3]
    {
      ((Component) this.btnBreakRepair).gameObject,
      ((Component) this.btnStartMatch).gameObject,
      this.objButtonDeckEdit
    };
    bool[] flagArray;
    if (Object.op_Inequality((Object) this.objButtonDeckEdit, (Object) null))
    {
      if (((IEnumerable<bool>) this.getUnitRegulation()).Any<bool>((Func<bool, bool>) (x => !x)))
        flagArray = new bool[3]{ false, false, true };
      else
        flagArray = new bool[3]
        {
          this.isRepair,
          !this.isRepair,
          false
        };
    }
    else
      flagArray = new bool[2]
      {
        this.isRepair,
        !this.isRepair
      };
    for (int index = 0; index < flagArray.Length; ++index)
      gameObjectArray[index].SetActive(flagArray[index]);
  }

  private void setStatusBoards()
  {
    if (this.statusBoards.IsNullOrEmpty<Versus026MatchBase.StatusBoard>())
    {
      this.SetBonusDisplay();
    }
    else
    {
      int firstIndex = -1;
      bool flag = false;
      Bonus[] bonusArray = this.bonusArray;
      if (!bonusArray.IsNullOrEmpty<Bonus>())
      {
        firstIndex = 0;
        this.setBonusDisplay(bonusArray, this.statusBoards[firstIndex].txtDetail, this.statusBoards[firstIndex].txtTimeLimit);
        this.statusBoards[firstIndex].top.SetActive(true);
      }
      else
        this.statusBoards[0].top.SetActive(false);
      if (this.rulePeriod != null)
      {
        firstIndex = 1;
        this.statusBoards[firstIndex].txtDetail.SetTextLocalize(this.rulePeriod.rule_detail);
        this.statusBoards[firstIndex].txtTimeLimit.SetTextLocalize(Bonus.ToRemainingTime(this.pvpInfo.rule.remaining_time));
        this.statusBoards[firstIndex].top.SetActive(true);
      }
      else
        this.statusBoards[1].top.SetActive(false);
      if (firstIndex >= 0)
      {
        this.setCenterStatusBoard(firstIndex);
        this.dirBonus.SetActive(true);
        flag = ((IEnumerable<Versus026MatchBase.StatusBoard>) this.statusBoards).Count<Versus026MatchBase.StatusBoard>((Func<Versus026MatchBase.StatusBoard, bool>) (x => x.top.activeSelf)) > 1;
        this.topChangeStatusBoards.SetActive(flag);
      }
      else
        this.dirBonus.SetActive(false);
      UIScrollView inParents = NGUITools.FindInParents<UIScrollView>(((Component) this.uiCenter).gameObject);
      if (Object.op_Inequality((Object) inParents, (Object) null))
        ((Behaviour) inParents).enabled = flag;
      if (flag)
      {
        // ISSUE: method pointer
        this.uiCenter.onFinished = new SpringPanel.OnFinished((object) this, __methodptr(onChangedStatusBoard));
      }
      else
        this.uiCenter.onFinished = (SpringPanel.OnFinished) null;
    }
  }

  private void onChangedStatusBoard()
  {
    GameObject go = this.uiCenter.centeredObject;
    this.currentStatusBoard = ((IEnumerable<Versus026MatchBase.StatusBoard>) this.statusBoards).Select<Versus026MatchBase.StatusBoard, GameObject>((Func<Versus026MatchBase.StatusBoard, GameObject>) (x => x.top)).FirstIndexOrNull<GameObject>((Func<GameObject, bool>) (y => Object.op_Equality((Object) y, (Object) go))) ?? this.currentStatusBoard;
  }

  public void onChangeStatusBoard() => this.setCenterStatusBoard();

  private void setCenterStatusBoard(int firstIndex = -1)
  {
    bool flag = true;
    if (firstIndex < 0)
    {
      firstIndex = this.currentStatusBoard == 0 ? 1 : 0;
      if (!this.statusBoards[firstIndex].top.activeSelf)
        return;
      flag = false;
    }
    this.currentStatusBoard = firstIndex;
    if (flag)
    {
      Transform transform = this.statusBoards[this.currentStatusBoard].top.transform;
      float x = transform.localPosition.x;
      while (Object.op_Inequality((Object) (transform = transform.parent), (Object) null))
      {
        UIScrollView component = ((Component) transform).GetComponent<UIScrollView>();
        if (Object.op_Inequality((Object) component, (Object) null))
        {
          component.DisableSpring();
          Vector2 vector2;
          // ISSUE: explicit constructor call
          ((Vector2) ref vector2).\u002Ector(-x, transform.localPosition.y);
          transform.localPosition = Vector2.op_Implicit(vector2);
          this.StartCoroutine(this.delaySetCenter());
          return;
        }
        x += transform.localPosition.x;
      }
    }
    this.uiCenter.CenterOn(this.statusBoards[this.currentStatusBoard].top.transform);
  }

  private IEnumerator delaySetCenter()
  {
    yield return (object) null;
    this.uiCenter.CenterOn(this.statusBoards[this.currentStatusBoard].top.transform);
  }

  private Bonus[] bonusArray
  {
    get
    {
      return this.pvpInfo.bonus.IsNullOrEmpty<Bonus>() ? (Bonus[]) null : ((IEnumerable<Bonus>) this.pvpInfo.bonus).Where<Bonus>((Func<Bonus, bool>) (x => x.category != 12)).ToArray<Bonus>();
    }
  }

  private void SetBonusDisplay()
  {
    Bonus[] bonusArray = this.bonusArray;
    if (!bonusArray.IsNullOrEmpty<Bonus>())
    {
      this.setBonusDisplay(bonusArray, this.txtBonus, this.txtTimeLimit);
      this.dirBonus.SetActive(true);
    }
    else
      this.dirBonus.SetActive(false);
  }

  private void setBonusDisplay(Bonus[] bonus, UILabel ulDetail, UILabel ulTimeLimit)
  {
    new BonusDisplay().Set(bonus, ulDetail, ulTimeLimit, false, true);
  }

  private IEnumerator SetLeaderSkillInfo()
  {
    GameObject gameObject;
    if (Object.op_Equality((Object) this.scrollTextPrefab, (Object) null))
    {
      Future<GameObject> h = Res.Prefabs.colosseum.colosseum023_4.ScrollText.Load<GameObject>();
      IEnumerator e = h.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.scrollTextPrefab = h.Result;
      gameObject = this.scrollTextPrefab.Clone(this.dirScrollText.transform);
      ((Object) gameObject).name = "ScrollText";
      h = (Future<GameObject>) null;
    }
    else
      gameObject = ((Component) this.dirScrollText.transform.GetChildInFind("ScrollText")).gameObject;
    PlayerUnit leaderUnit = this.GetLeaderUnit(this.deckInfo);
    if (leaderUnit != (PlayerUnit) null)
    {
      if (leaderUnit.leader_skill != null)
      {
        this.txtLeaderSkill.SetTextLocalize(leaderUnit.leader_skill.skill.name);
        string text = leaderUnit.leader_skill.skill.description.Replace("\r", "").Replace("\n", "");
        gameObject.GetComponent<Colosseum0234ScrollText>().StartScroll(text);
      }
      else
      {
        this.txtLeaderSkill.SetTextLocalize("なし");
        gameObject.GetComponent<Colosseum0234ScrollText>().StartScroll("リーダースキルがありません");
      }
    }
    else
    {
      this.txtLeaderSkill.SetTextLocalize("なし");
      gameObject.GetComponent<Colosseum0234ScrollText>().StartScroll("ユニットが編成されていません");
    }
    this.txtTotalPower.text = this.deckInfo.total_combat.ToLocalizeNumberText();
  }

  private IEnumerator SetBgCharacter()
  {
    foreach (Component component in this.bgCharacter.transform)
      Object.Destroy((Object) component.gameObject);
    PlayerUnit leaderUnit = this.GetLeaderUnit(this.deckInfo);
    if (!(leaderUnit == (PlayerUnit) null))
      yield return (object) leaderUnit.unit.LoadQuestWithMask(leaderUnit.job_id, this.bgCharacter.transform, this.bgCharacter.GetComponent<UIWidget>().depth, Res.GUI._002_2_sozai.mask_chara.Load<Texture2D>());
  }

  private IEnumerator SetUnitIcon()
  {
    Future<GameObject> unitPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = unitPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject unitPrefab = unitPrefabF.Result;
    bool bCustom = this.deckInfo.isCustom;
    bool[] bRegulations = this.getUnitRegulation();
    for (int i = 0; i < this.deckInfo.player_units.Length; ++i)
    {
      foreach (Component component in this.unitIcons[i].transform)
        Object.Destroy((Object) component.gameObject);
      UnitIcon unitPlayer = unitPrefab.Clone(this.unitIcons[i].transform).GetComponent<UnitIcon>();
      PlayerUnit playerUnit = this.deckInfo.player_units[i];
      if (i == 0)
        yield return (object) unitPlayer.setBottomUnit(playerUnit, this.deckInfo.player_units);
      else
        yield return (object) unitPlayer.SetPlayerUnit(playerUnit, this.deckInfo.player_units, (PlayerUnit) null, false, false);
      if (playerUnit != (PlayerUnit) null)
      {
        if (bCustom)
          this.setEventLongPressedByCustom(unitPlayer.Button, playerUnit, this.deckInfo.player_units);
        unitPlayer.setLevelText(playerUnit);
        unitPlayer.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
        if (i == 0)
        {
          unitPlayer.BreakWeaponOnlyBottom = playerUnit.IsBrokenEquippedGear;
          if (bRegulations[i])
            unitPlayer.SetRegulation(UnitIcon.Regulation.None);
          else
            unitPlayer.SetRegulation(this.posLeaderRegulation);
        }
        else
        {
          unitPlayer.BreakWeapon = playerUnit.IsBrokenEquippedGear;
          unitPlayer.SetRegulation(bRegulations[i] ? UnitIcon.Regulation.None : UnitIcon.Regulation.Default);
        }
        this.isRepair = playerUnit.IsBrokenEquippedGear || this.isRepair;
      }
      else
        unitPlayer.SetEmpty();
      unitPlayer.princessType.DispPrincessType(false);
      unitPlayer = (UnitIcon) null;
      playerUnit = (PlayerUnit) null;
    }
  }

  private void setEventLongPressedByCustom(
    LongPressButton btn,
    PlayerUnit baseUnit,
    PlayerUnit[] units)
  {
    EventDelegate.Set(btn.onLongPress, (EventDelegate.Callback) (() => Unit0042Scene.changeSceneCustomDeck(true, baseUnit, units)));
  }

  private DeckInfo GetDeck() => Persist.versusDeckOrganized.Data.getSelectedDeck();

  private PlayerUnit GetLeaderUnit(DeckInfo deck)
  {
    return ((IEnumerable<PlayerUnit>) deck.player_units).FirstOrDefault<PlayerUnit>();
  }

  private bool GetBoolCostOver()
  {
    if (this.deckInfo.cost <= this.deckInfo.cost_limit)
      return false;
    Consts instance = Consts.GetInstance();
    ModalWindow.Show(instance.VERSUS_00262POPUP_COSTOVER_TITLE, instance.VERSUS_00262POPUP_COSTOVER_DESCRIPTION, (Action) (() => this.CancelBattle()));
    return true;
  }

  private void StartMatch()
  {
    this.battleInfo = new BattleInfo();
    this.battleInfo.pvp = true;
    this.StartCoroutine(this.doMatchingAndStartBattle());
  }

  private IEnumerator doMatchingAndStartBattle()
  {
    Versus026MatchBase versus026MatchBase = this;
    if (!versus026MatchBase.isMatched)
    {
      versus026MatchBase.isMatched = true;
      Persist.battleEnvironment.Delete();
      PVNpcManager.destroyPVNpcManager();
      Singleton<CommonRoot>.GetInstance().loadingMode = 2;
      Future<WebAPI.Response.PvpBoot> futureF = WebAPI.PvpBoot((Action<WebAPI.Response.UserError>) (e =>
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        this.CancelBattle();
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      IEnumerator e1 = futureF.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (futureF.Result != null)
      {
        versus026MatchBase.pvpInfo = futureF.Result;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        if (!versus026MatchBase.IsMatchingBeginCheck())
        {
          versus026MatchBase.CancelBattle();
          versus026MatchBase.IsPush = true;
          e1 = versus026MatchBase.ErrorMathcingBegin();
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
        }
        else if (versus026MatchBase.pvpInfo.pvp_maintenance)
        {
          Singleton<PopupManager>.GetInstance().onDismiss();
          if (!versus026MatchBase.IsPushAndSet())
            yield return (object) PopupCommon.Show(versus026MatchBase.pvpInfo.pvp_maintenance_title, versus026MatchBase.pvpInfo.pvp_maintenance_message, (Action) (() =>
            {
              NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
              instance.clearStack();
              instance.destroyCurrentScene();
              instance.changeScene(Singleton<CommonRoot>.GetInstance().startScene, false);
            }));
        }
        else if (versus026MatchBase.pvpInfo.is_battle)
        {
          versus026MatchBase.isCancel = false;
          versus026MatchBase.PopupReadyErrorMatching(new Action(versus026MatchBase.CancelBattle));
          yield return (object) new WaitWhile((Func<bool>) (() => Singleton<PopupManager>.GetInstance().isOpen || !this.isCancel));
          StartScript.Restart();
        }
        else if (!versus026MatchBase.pvpInfo.is_latest_client_version)
        {
          Future<GameObject> f = Res.Prefabs.popup.popup_026_1_1__anim_popup01.Load<GameObject>();
          e1 = f.Wait();
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
          Singleton<PopupManager>.GetInstance().open(f.Result).GetComponent<Popup02611Menu>().Init();
        }
        else
        {
          versus026MatchBase.isCancel = false;
          GameObject popup_obj = (GameObject) null;
          yield return (object) versus026MatchBase.PopupConnectMatching((Action<GameObject>) (obj => popup_obj = obj));
          PVPManager pvpManager = PVPManager.createPVPManager();
          PVPMatching mm = pvpManager.getMatchingBehaviour();
          pvpManager.matchingType = versus026MatchBase.pvpInfo.is_tutorial_battle_end || !versus026MatchBase.pvpInfo.is_tutorial ? versus026MatchBase.type : PvpMatchingTypeEnum.tutorial;
          string roomkey = versus026MatchBase.SetRoomKey(MasterData.PvpMatchingType[(int) versus026MatchBase.type].room_key);
          mm.setMatchingServer(versus026MatchBase.pvpInfo.matching_host, versus026MatchBase.pvpInfo.matching_port);
          Singleton<CommonRoot>.GetInstance().loadingMode = 4;
          Future<MatchingPeer.MatchedConfirmation> mF = (Future<MatchingPeer.MatchedConfirmation>) null;
          PvpClassKind pvpClassKind = (PvpClassKind) null;
          MasterData.PvpClassKind.TryGetValue(versus026MatchBase.pvpInfo.current_class, out pvpClassKind);
          if (versus026MatchBase.type == PvpMatchingTypeEnum.class_match && pvpClassKind.cpu_defeats_count.HasValue && pvpClassKind.cpu_defeats_count.Value <= versus026MatchBase.pvpInfo.pvp_class_record.pvp_record.current_consecutive_loss)
          {
            versus026MatchBase.battleInfo.pvp_vs_npc = true;
          }
          else
          {
            mF = mm.matchingPVP(versus026MatchBase.deckInfo.deck_type_id, versus026MatchBase.deckInfo.deck_number, roomkey);
            e1 = mF.Wait();
            while (e1.MoveNext())
            {
              mm.IsCancel = versus026MatchBase.isCancel;
              yield return e1.Current;
            }
            e1 = (IEnumerator) null;
            if (versus026MatchBase.isCancel)
            {
              versus026MatchBase.CancelBattle();
              yield break;
            }
            else if (mF.Exception != null)
            {
              Consts instance = Consts.GetInstance();
              if (mF.Exception.ToString().IndexOf("timeout") != -1 || mF.Exception.ToString().IndexOf("UN-Matched") != -1)
                versus026MatchBase.PopupMatchingServerConnectError(new Action(versus026MatchBase.CancelBattle), instance.VERSUS_002645POPUP_TITLE, instance.VERSUS_002645POPUP_DESCRIPTION);
              else if (mF.Exception.ToString().IndexOf("Connect Failed") != -1)
              {
                string description = instance.VERSUS_MACTHINGSERVER_DONT_CONNECT_DESCRIPTION + "\n" + instance.VERSUS_AFTER_WHILE_RETRY;
                versus026MatchBase.PopupMatchingServerConnectError(new Action(versus026MatchBase.CancelBattle), instance.VERSUS_002645POPUP_TITLE, description);
              }
              else if (mF.Exception.ToString().IndexOf("Error-HartBeat") != -1)
              {
                string description = instance.VERSUS_0026CANCEL_DESCRIPTION + "\n" + instance.VERSUS_AFTER_WHILE_RETRY;
                versus026MatchBase.PopupMatchingServerConnectError(new Action(versus026MatchBase.CancelBattle), instance.VERSUS_002645POPUP_TITLE, description);
              }
              else
              {
                string description = instance.VERSUS_MACTHINGSERVER_ANOTHER_DESCRIPTION + "\n" + instance.VERSUS_AFTER_WHILE_RETRY;
                versus026MatchBase.PopupMatchingServerConnectError(new Action(versus026MatchBase.CancelBattle), instance.VERSUS_002645POPUP_TITLE, description);
              }
              Debug.LogError((object) (" === Matching error:" + (object) mF.Exception));
              yield break;
            }
            else if (mF.Result.player_id.Equals("Start-NPC"))
              versus026MatchBase.battleInfo.pvp_vs_npc = true;
          }
          if (Object.op_Implicit((Object) popup_obj))
          {
            popup_obj.GetComponent<Popup02642SerchMatching>().DisableButton();
            yield return (object) null;
          }
          bool isSuccess = false;
          if (versus026MatchBase.battleInfo.pvp_vs_npc)
          {
            yield return (object) versus026MatchBase.prepareToStartPvNpc((Action) (() => isSuccess = true));
          }
          else
          {
            yield return (object) versus026MatchBase.GetEnemyInfo(mF.Result.player_id, (Action<WebAPI.Response.PvpFriend>) (res => Singleton<PVPManager>.GetInstance().enemyInfo = res));
            if (Singleton<PVPManager>.GetInstance().enemyInfo == null)
            {
              Singleton<CommonRoot>.GetInstance().loadingMode = 0;
              yield break;
            }
            else
              yield return (object) versus026MatchBase.prepareToStartPvP((Action) (() => isSuccess = true));
          }
          if (isSuccess)
            versus026MatchBase.startBattle();
        }
      }
    }
  }

  private IEnumerator prepareToStartPvP(Action successCallback)
  {
    Versus026MatchBase versus026MatchBase = this;
    if (versus026MatchBase.isFriendMatch)
    {
      Singleton<PopupManager>.GetInstance().onDismiss();
      versus026MatchBase.isCloseFriendMatchPopup = false;
      yield return (object) versus026MatchBase.PopupFindMatching();
      // ISSUE: reference to a compiler-generated method
      yield return (object) new WaitWhile(new Func<bool>(versus026MatchBase.\u003CprepareToStartPvP\u003Eb__65_0));
      if (versus026MatchBase.isCancel)
      {
        versus026MatchBase.PopupCancelBattle(new Action(versus026MatchBase.CancelBattle), true);
        yield break;
      }
    }
    PVPMatching mm = Singleton<PVPManager>.GetInstance().getMatchingBehaviour();
    Future<MatchingPeer.Matched> rF = mm.matchingReady(!versus026MatchBase.isCancel);
    IEnumerator e = rF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (rF.Exception != null)
    {
      Debug.LogError((object) (" === MatchingReady error:" + (object) rF.Exception));
      Consts instance = Consts.GetInstance();
      if (rF.Exception.ToString().IndexOf("Cancel") != -1)
        versus026MatchBase.PopupCancelBattle(new Action(versus026MatchBase.CancelBattle), false);
      else if (rF.Exception.ToString().IndexOf("Redis Connect Failed") != -1)
      {
        string description = string.Format(instance.VERSUS_02694POPUP_CONNECT_ERROR_CODE_DESCRIPTION, (object) "002601") + "\n" + instance.VERSUS_AFTER_WHILE_RETRY;
        versus026MatchBase.PopupMatchingServerConnectError(new Action(versus026MatchBase.CancelBattle), instance.VERSUS_002645POPUP_TITLE, description);
      }
      else if (rF.Exception.ToString().IndexOf("GameServer Doesnt Exist") != -1)
      {
        string description = string.Format(instance.VERSUS_02694POPUP_CONNECT_ERROR_CODE_DESCRIPTION, (object) "002602") + "\n" + instance.VERSUS_AFTER_WHILE_RETRY;
        versus026MatchBase.PopupMatchingServerConnectError(new Action(versus026MatchBase.CancelBattle), instance.VERSUS_002645POPUP_TITLE, description);
      }
      else if (rF.Exception.ToString().IndexOf("Room Full") != -1)
      {
        string description = string.Format(instance.VERSUS_02694POPUP_CONNECT_ERROR_CODE_DESCRIPTION, (object) "002603") + "\n" + instance.VERSUS_AFTER_WHILE_RETRY;
        versus026MatchBase.PopupMatchingServerConnectError(new Action(versus026MatchBase.CancelBattle), instance.VERSUS_002645POPUP_TITLE, description);
      }
      else if (rF.Exception.ToString().IndexOf("Version Mismatch") != -1)
      {
        string description = string.Format(instance.VERSUS_02694POPUP_CONNECT_ERROR_CODE_DESCRIPTION, (object) "002604") + "\n" + instance.VERSUS_AFTER_WHILE_RETRY;
        versus026MatchBase.PopupMatchingServerConnectError(new Action(versus026MatchBase.CancelBattle), instance.VERSUS_002645POPUP_TITLE, description);
      }
      else if (rF.Exception.ToString().IndexOf("GameServer Connect Failed") != -1)
      {
        string description = string.Format(instance.VERSUS_02694POPUP_CONNECT_ERROR_CODE_DESCRIPTION, (object) "002605") + "\n" + instance.VERSUS_AFTER_WHILE_RETRY;
        versus026MatchBase.PopupMatchingServerConnectError(new Action(versus026MatchBase.CancelBattle), instance.VERSUS_002645POPUP_TITLE, description);
      }
      else if (rF.Exception.ToString().IndexOf("GameServer Exception") != -1)
      {
        string description = string.Format(instance.VERSUS_02694POPUP_CONNECT_ERROR_CODE_DESCRIPTION, (object) "002606") + "\n" + instance.VERSUS_AFTER_WHILE_RETRY;
        versus026MatchBase.PopupMatchingServerConnectError(new Action(versus026MatchBase.CancelBattle), instance.VERSUS_002645POPUP_TITLE, description);
      }
      else if (rF.Exception.ToString().IndexOf("Heartbeat Failed") != -1)
      {
        string description = instance.VERSUS_0026CANCEL_DESCRIPTION + "\n" + instance.VERSUS_AFTER_WHILE_RETRY;
        versus026MatchBase.PopupMatchingServerConnectError(new Action(versus026MatchBase.CancelBattle), instance.VERSUS_002645POPUP_TITLE, description);
      }
      else if (rF.Exception.ToString().IndexOf("Create Room Request Error") != -1)
      {
        versus026MatchBase.isCancel = false;
        string description = string.Format(instance.VERSUS_0026_CREATE_ROOM_FAILED, (object) "002624") + "\n" + instance.VERSUS_BACK_TO_TITLE;
        versus026MatchBase.PopupCreateRoomError(new Action(versus026MatchBase.CancelBattle), instance.VERSUS_002645POPUP_TITLE, description);
        // ISSUE: reference to a compiler-generated method
        yield return (object) new WaitWhile(new Func<bool>(versus026MatchBase.\u003CprepareToStartPvP\u003Eb__65_1));
        StartScript.Restart();
      }
      else if (rF.Exception.ToString().IndexOf("Create Room Failed") != -1)
      {
        versus026MatchBase.isCancel = false;
        string description = string.Format(instance.VERSUS_0026_CREATE_ROOM_FAILED, (object) "002625") + "\n" + instance.VERSUS_BACK_TO_TITLE;
        versus026MatchBase.PopupCreateRoomError(new Action(versus026MatchBase.CancelBattle), instance.VERSUS_002645POPUP_TITLE, description);
        // ISSUE: reference to a compiler-generated method
        yield return (object) new WaitWhile(new Func<bool>(versus026MatchBase.\u003CprepareToStartPvP\u003Eb__65_2));
        StartScript.Restart();
      }
      else if (rF.Exception.ToString().IndexOf("Create Room Response Failed") != -1)
      {
        versus026MatchBase.isCancel = false;
        string description = string.Format(instance.VERSUS_0026_CREATE_ROOM_FAILED, (object) "002626") + "\n" + instance.VERSUS_BACK_TO_TITLE;
        versus026MatchBase.PopupCreateRoomError(new Action(versus026MatchBase.CancelBattle), instance.VERSUS_002645POPUP_TITLE, description);
        // ISSUE: reference to a compiler-generated method
        yield return (object) new WaitWhile(new Func<bool>(versus026MatchBase.\u003CprepareToStartPvP\u003Eb__65_3));
        StartScript.Restart();
      }
      else
      {
        versus026MatchBase.isCancel = false;
        string description = string.Format(instance.VERSUS_0026_CREATE_ROOM_FAILED, (object) "002627") + "\n" + instance.VERSUS_BACK_TO_TITLE;
        versus026MatchBase.PopupCreateRoomError(new Action(versus026MatchBase.CancelBattle), instance.VERSUS_002645POPUP_TITLE, description);
        // ISSUE: reference to a compiler-generated method
        yield return (object) new WaitWhile(new Func<bool>(versus026MatchBase.\u003CprepareToStartPvP\u003Eb__65_4));
        StartScript.Restart();
      }
    }
    else
    {
      versus026MatchBase.setMatched(rF.Result);
      mm.cleanupDestroy();
      successCallback();
    }
  }

  private IEnumerator prepareToStartPvNpc(Action successCallback)
  {
    Versus026MatchBase versus026MatchBase = this;
    int tmpLoadingMode = Singleton<CommonRoot>.GetInstance().loadingMode;
    Singleton<CommonRoot>.GetInstance().loadingMode = 2;
    // ISSUE: reference to a compiler-generated method
    Future<WebAPI.Response.PvpPlayerNpcStart> startF = WebAPI.PvpPlayerNpcStart(versus026MatchBase.deckInfo.deck_number, versus026MatchBase.deckInfo.deck_type_id, (int) versus026MatchBase.type, new Action<WebAPI.Response.UserError>(versus026MatchBase.\u003CprepareToStartPvNpc\u003Eb__66_0));
    IEnumerator e = startF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (startF.Result != null)
    {
      PVNpcManager pvNpcManager = PVNpcManager.createPVNpcManager();
      pvNpcManager.matchingType = Singleton<PVPManager>.GetInstance().matchingType;
      pvNpcManager.player = startF.Result.player;
      pvNpcManager.enemy = startF.Result.target_player;
      pvNpcManager.stage = startF.Result.stage;
      versus026MatchBase.battleInfo = BattleInfo.MakePvNpcBattleInfo(startF.Result);
      yield return (object) versus026MatchBase.GetEnemyInfo(pvNpcManager.enemy.id, (Action<WebAPI.Response.PvpFriend>) (res => Singleton<PVNpcManager>.GetInstance().enemyInfo = res));
      if (Singleton<PVNpcManager>.GetInstance().enemyInfo != null)
      {
        versus026MatchBase.setMatched((MatchingPeer.Matched) null);
        yield return (object) PVPManager.destroyPVPManager();
        Singleton<CommonRoot>.GetInstance().loadingMode = tmpLoadingMode;
        successCallback();
      }
    }
  }

  private void startBattle()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    Singleton<CommonRoot>.GetInstance().loadingMode = 4;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Singleton<NGBattleManager>.GetInstance().startBattle(this.battleInfo);
  }

  private void setMatched(MatchingPeer.Matched matched)
  {
    if (matched != null)
    {
      this.battleInfo.host = matched.host;
      this.battleInfo.port = matched.port;
      this.battleInfo.battleToken = matched.battleToken;
    }
    else
    {
      this.battleInfo.host = (string) null;
      this.battleInfo.port = 0;
      this.battleInfo.battleToken = (string) null;
    }
  }

  private IEnumerator GetEnemyInfo(string id, Action<WebAPI.Response.PvpFriend> callback)
  {
    Versus026MatchBase versus026MatchBase = this;
    // ISSUE: reference to a compiler-generated method
    Future<WebAPI.Response.PvpFriend> f = WebAPI.PvpFriend(id, new Action<WebAPI.Response.UserError>(versus026MatchBase.\u003CGetEnemyInfo\u003Eb__69_0));
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    callback(f.Result);
  }

  protected virtual string SetRoomKey(string key) => key;

  public void OkFriendMatch() => this.isCloseFriendMatchPopup = true;

  public void CancelFriendMatch()
  {
    this.isCloseFriendMatchPopup = true;
    this.isCancel = true;
  }

  public void CancelBattle()
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<PopupManager>.GetInstance().onDismiss();
    PVPManager.createPVPManager().getMatchingBehaviour().cleanupDestroy();
    this.isMatched = false;
    this.IsPush = false;
  }

  private IEnumerator PopupConfirmationMatching()
  {
    Versus026MatchBase versus026MatchBase = this;
    if (versus026MatchBase.isMatched)
    {
      Debug.LogError((object) "Already connect matching");
    }
    else
    {
      Future<GameObject> popupF = Res.Prefabs.popup.popup_026_4_1__anim_popup01.Load<GameObject>();
      IEnumerator e = popupF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<PopupManager>.GetInstance().open(popupF.Result).GetComponent<Popup02641Menu>().Initialize(new Action(versus026MatchBase.StartMatch), new Action(versus026MatchBase.CancelBattle));
    }
  }

  private void PopupCancelBattle(Action OkAction, bool byMyself)
  {
    if (!this.isMatched)
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    Consts instance = Consts.GetInstance();
    ModalWindow.Show(byMyself ? instance.VERSUS_0026CANCEL_MYSELF_TITLE : instance.VERSUS_0026CANCEL_TITLE, byMyself ? instance.VERSUS_0026CANCEL_MYSELF_DESCRIPTION : instance.VERSUS_0026CANCEL_DESCRIPTION, (Action) (() => OkAction()));
  }

  private void PopupTimeOutMatching(Action OkAction)
  {
    if (!this.isMatched)
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    Consts instance = Consts.GetInstance();
    ModalWindow.Show(instance.VERSUS_002645POPUP_TITLE, instance.VERSUS_002645POPUP_DESCRIPTION, (Action) (() => OkAction()));
  }

  private void PopupMatchingServerConnectError(Action OkAction, string title, string description)
  {
    if (!this.isMatched)
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    Consts.GetInstance();
    ModalWindow.Show(title, description, (Action) (() => OkAction()));
  }

  private void PopupReadyErrorMatching(Action OkAction)
  {
    if (!this.isMatched)
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    Consts instance = Consts.GetInstance();
    ModalWindow.Show(instance.VERSUS_0026_READY_ERROR_TITLE, instance.VERSUS_0026_READY_ERROR_MESSAGE, (Action) (() =>
    {
      OkAction();
      this.isCancel = true;
    }));
  }

  private void PopupCreateRoomError(Action OkAction, string title, string description)
  {
    if (!this.isMatched)
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    Consts.GetInstance();
    ModalWindow.Show(title, description, (Action) (() =>
    {
      OkAction();
      this.isCancel = true;
    }));
  }

  private IEnumerator PopupConnectMatching(Action<GameObject> obj)
  {
    Versus026MatchBase versus026MatchBase = this;
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_026_4_2__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(prefabF.Result);
    // ISSUE: reference to a compiler-generated method
    gameObject.GetComponent<Popup02642SerchMatching>().Init(new Action(versus026MatchBase.\u003CPopupConnectMatching\u003Eb__80_0));
    if (obj != null)
      obj(gameObject);
  }

  private IEnumerator PopupFindMatching()
  {
    Versus026MatchBase versus026MatchBase = this;
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_026_4_6__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    // ISSUE: reference to a compiler-generated method
    yield return (object) Singleton<PopupManager>.GetInstance().open(prefabF.Result).GetComponent<Popup02646Menu>().Init(new Action(versus026MatchBase.OkFriendMatch), new Action(versus026MatchBase.CancelFriendMatch), new Action(versus026MatchBase.\u003CPopupFindMatching\u003Eb__81_0), Singleton<PVPManager>.GetInstance().enemyInfo);
  }

  protected virtual bool IsMatchingBeginCheck() => true;

  protected virtual IEnumerator ErrorMathcingBegin()
  {
    yield break;
  }

  public override void onBackButton() => this.IbtnBack();

  [Serializable]
  protected class StatusBoard
  {
    public GameObject top;
    public UILabel txtDetail;
    public UILabel txtTimeLimit;
  }
}
