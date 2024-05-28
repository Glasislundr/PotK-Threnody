// Decompiled with JetBrains decompiler
// Type: ExploreChallengePopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniLinq;
using UnityEngine;

#nullable disable
public class ExploreChallengePopup : BackButtonMenuBase
{
  [SerializeField]
  private UILabel TxtTicketValue;
  [SerializeField]
  private UILabel TxtTotalCombat;
  [SerializeField]
  private UILabel TxtMyTeamWinRate;
  [SerializeField]
  private UI2DSprite spriteLeaderGearKind;
  [SerializeField]
  private UILabel TxtLeaderUnitLvNumber;
  [SerializeField]
  private NGxMaskSprite mask_Chara;
  [SerializeField]
  private GameObject[] unitIcons;
  [SerializeField]
  private GameObject[] dirCustome;
  [SerializeField]
  private UILabel[] TxtCustomeName;
  [SerializeField]
  private UILabel[] TxtFightnum;
  [SerializeField]
  private UI2DSprite[] spriteGearKind;
  [SerializeField]
  private UILabel[] TxtLVnumber;
  [SerializeField]
  private UILabel[] TxtWinRate;
  [SerializeField]
  private NGxMaskSprite[] opponentMaskCharas;
  [SerializeField]
  private GameObject dirExploreChallengeMatchup;
  private GameObject unitPrefab;
  private Sprite maskOpponent;
  private ChallengeNpc[] gladiators;
  private int[] opponents;
  private bool isRepair;
  private GameObject nowPopup;
  private ExploreFooter footer;

  public IEnumerator Initialize(ExploreFooter footer)
  {
    ExploreDataManager dataMgr = Singleton<ExploreDataManager>.GetInstance();
    this.nowPopup = (GameObject) null;
    this.footer = footer;
    dataMgr.SetReopenPopupStateChallenge();
    this.TxtTicketValue.SetTextLocalize(Consts.GetInstance().EXPLORE_POPUP_TICKET_VALUE.F((object) Singleton<NGGameDataManager>.GetInstance().challenge_point));
    Future<GameObject> unitPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e1 = unitPrefabF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    this.unitPrefab = unitPrefabF.Result;
    unitPrefabF = (Future<GameObject>) null;
    Future<Sprite> maskOpponentF = new ResourceObject("GUI/explore_other/slc_unit_mask_Opponent").Load<Sprite>();
    e1 = maskOpponentF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    this.maskOpponent = maskOpponentF.Result;
    maskOpponentF = (Future<Sprite>) null;
    if (!WebAPI.IsResponsedAtRecent("ExploreChallengeBoot") || dataMgr.Gladiators == null)
    {
      Future<WebAPI.Response.ExploreChallengeBoot> futureF = WebAPI.ExploreChallengeBoot((Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      e1 = futureF.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (futureF.Result == null)
      {
        yield break;
      }
      else
      {
        dataMgr.Gladiators = futureF.Result.gladiators;
        futureF = (Future<WebAPI.Response.ExploreChallengeBoot>) null;
      }
    }
    this.gladiators = dataMgr.Gladiators;
    yield return (object) this.SetChallengeDeckIcon();
    yield return (object) this.SetOpponents();
  }

  private IEnumerator SetChallengeDeckIcon()
  {
    ExploreDataManager instance = Singleton<ExploreDataManager>.GetInstance();
    PlayerDeck deck = instance.GetChallengeDeck();
    this.TxtTotalCombat.SetTextLocalize(deck.total_combat);
    PlayerChallenge playerChallenge = SMManager.Get<PlayerChallenge>();
    this.TxtMyTeamWinRate.SetTextLocalize(instance.GetWinRate(playerChallenge.win_count, playerChallenge.lose_count));
    PlayerUnit leaderUnit = deck.player_units[0];
    Future<Sprite> futureSprite = leaderUnit.unit.LoadSpriteLarge(leaderUnit.job_id, 1f);
    IEnumerator e = futureSprite.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mask_Chara.sprite2D = futureSprite.Result;
    Future<Sprite> futureMask = new ResourceObject("GUI/explore_other/slc_unit_mask_Leader").Load<Sprite>();
    e = futureMask.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) this.mask_Chara).GetComponent<NGxMaskSpriteWithScale>().maskTexture = futureMask.Result.texture;
    this.spriteLeaderGearKind.sprite2D = ExploreChallengePopup.LoadGearKindSprite((GearKindEnum) leaderUnit.unit.kind_GearKind, leaderUnit.GetElement());
    this.TxtLeaderUnitLvNumber.SetTextLocalize(leaderUnit.total_level);
    this.isRepair = leaderUnit.IsBrokenEquippedGear || this.isRepair;
    for (int i = 1; i < deck.player_units.Length; ++i)
    {
      foreach (Component component in this.unitIcons[i].transform)
        Object.Destroy((Object) component.gameObject);
      UnitIcon unitPlayer = this.unitPrefab.Clone(this.unitIcons[i].transform).GetComponent<UnitIcon>();
      PlayerUnit playerUnit = deck.player_units[i];
      e = unitPlayer.SetPlayerUnit(playerUnit, deck.player_units, (PlayerUnit) null, false, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unitPlayer.Button.onLongPress.Clear();
      ((UIButtonColor) unitPlayer.Button).isEnabled = false;
      if (playerUnit != (PlayerUnit) null)
      {
        unitPlayer.setLevelText(playerUnit);
        unitPlayer.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
        if (i == 0)
          unitPlayer.BreakWeaponOnlyBottom = playerUnit.IsBrokenEquippedGear;
        else
          unitPlayer.BreakWeapon = playerUnit.IsBrokenEquippedGear;
        this.isRepair = playerUnit.IsBrokenEquippedGear || this.isRepair;
      }
      else
        unitPlayer.SetEmpty();
      unitPlayer = (UnitIcon) null;
      playerUnit = (PlayerUnit) null;
    }
  }

  public static Sprite LoadGearKindSprite(GearKindEnum kind, CommonElement element)
  {
    string empty = string.Empty;
    return Resources.Load<Sprite>(!Singleton<NGGameDataManager>.GetInstance().IsSea ? string.Format("Icons/Materials/GearKind_Element_Icon/slc_{0}_{1}_34_30", (object) kind.ToString(), (object) element.ToString()) : string.Format("Icons/Materials/Sea/GearKind_Element_Icon/slc_{0}_{1}_34_30", (object) kind.ToString(), (object) element.ToString()));
  }

  private IEnumerator SetOpponents()
  {
    int arrayMax = Mathf.Min(this.gladiators.Length, 3);
    this.opponents = this.GetRandomShuffle(arrayMax, this.gladiators.Length);
    ((IEnumerable<GameObject>) this.dirCustome).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(false)));
    if (arrayMax == 1)
    {
      yield return (object) this.SetOpponentsInfo(1, this.gladiators[this.opponents[0]]);
    }
    else
    {
      for (int i = 0; i < arrayMax; ++i)
      {
        ChallengeNpc gladiator = this.gladiators[this.opponents[i]];
        yield return (object) this.SetOpponentsInfo(i, gladiator);
      }
    }
  }

  private IEnumerator SetOpponentsInfo(int index, ChallengeNpc data)
  {
    Singleton<ExploreDataManager>.GetInstance();
    UnitUnit unitUnit = MasterData.UnitUnit[data.leader_unit_id];
    this.dirCustome[index].SetActive(true);
    this.TxtCustomeName[index].SetTextLocalize(data.name.ToConverter());
    this.TxtFightnum[index].SetTextLocalize(data.total_power.ToLocalizeNumberText());
    this.TxtLVnumber[index].SetTextLocalize(data.player_level.ToLocalizeNumberText());
    this.spriteGearKind[index].sprite2D = ExploreChallengePopup.LoadGearKindSprite((GearKindEnum) unitUnit.kind_GearKind, unitUnit.GetElement());
    Future<Sprite> futureSprite = unitUnit.LoadSpriteLarge(data.leader_unit_job_id, 1f);
    IEnumerator e = futureSprite.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.opponentMaskCharas[index].sprite2D = futureSprite.Result;
    ((Component) this.opponentMaskCharas[index]).GetComponent<NGxMaskSpriteWithScale>().maskTexture = this.maskOpponent.texture;
  }

  private int[] GetRandomShuffle(int arrayMax, int rangeMax)
  {
    int[] source = new int[rangeMax];
    for (int index = 0; index < source.Length; ++index)
      source[index] = index;
    int[] array = ((IEnumerable<int>) source).OrderBy<int, Guid>((Func<int, Guid>) (i => Guid.NewGuid())).ToArray<int>();
    int[] destinationArray = new int[arrayMax];
    Array.Copy((Array) array, (Array) destinationArray, arrayMax);
    return destinationArray;
  }

  public void IbtnCostomerUnit() => this.SelectOpponent(0);

  public void IbtnCostomerUnit2() => this.SelectOpponent(1);

  public void IbtnCostomerUnit3() => this.SelectOpponent(2);

  private void SelectOpponent(int index)
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.BattleStart(index));
  }

  private IEnumerator BattleStart(int index)
  {
    ExploreChallengePopup exploreChallengePopup = this;
    if (exploreChallengePopup.isRepair)
    {
      if (Object.op_Equality((Object) exploreChallengePopup.nowPopup, (Object) null))
        exploreChallengePopup.nowPopup = ((Component) ModalWindow.ShowYesNo(Consts.GetInstance().EXPLORE_POPUP_REPAIR_CONFIRM_TITLE, Consts.GetInstance().EXPLORE_POPUP_REPAIR_CONFIRM_MESSAGE, new Action(exploreChallengePopup.RepairConfirmationYesBtn), new Action(exploreChallengePopup.RepairConfirmationNoBtn))).gameObject;
    }
    else
    {
      PlayerDeck deck = Singleton<ExploreDataManager>.GetInstance().GetChallengeDeck();
      if (deck.cost > SMManager.Get<Player>().max_cost)
        exploreChallengePopup.nowPopup = ((Component) ModalWindow.Show(Consts.GetInstance().EXPLORE_POPUP_COSTOVER_TITLE, Consts.GetInstance().EXPLORE_POPUP_COSTOVER_DESCRIPTION, (Action) (() => this.StartCoroutine(this.IsPushOff())))).gameObject;
      else if (((IEnumerable<PlayerUnit>) deck.player_units).Where<PlayerUnit>((Func<PlayerUnit, bool>) (v => v != (PlayerUnit) null)).Count<PlayerUnit>() <= 2)
      {
        exploreChallengePopup.StartCoroutine(exploreChallengePopup.ShowUnitAlertPopup());
      }
      else
      {
        Singleton<ExploreSceneManager>.GetInstance().Pause(true);
        Stopwatch sw = new Stopwatch();
        sw.Start();
        yield return (object) exploreChallengePopup.playChallengeMatchUpEffect();
        bool saveFailed = false;
        yield return (object) Singleton<ExploreDataManager>.GetInstance().SaveSuspendData((Action) (() => saveFailed = true));
        Singleton<ExploreSceneManager>.GetInstance().SetReloadDirty();
        if (saveFailed)
        {
          Singleton<ExploreDataManager>.GetInstance().InitReopenPopupState();
          Singleton<CommonRoot>.GetInstance().WhiteFadeOut();
          MypageScene.ChangeScene();
        }
        else
        {
          int index1 = exploreChallengePopup.opponents.Length == 1 ? exploreChallengePopup.opponents[0] : exploreChallengePopup.opponents[index];
          Future<WebAPI.Response.ExploreChallengeStart> futureF = WebAPI.ExploreChallengeStart(exploreChallengePopup.gladiators[index1].player_id, deck.total_combat, (Action<WebAPI.Response.UserError>) (error =>
          {
            Singleton<CommonRoot>.GetInstance().isLoading = false;
            WebAPI.DefaultUserErrorCallback(error);
            MypageScene.ChangeSceneOnError();
          }));
          IEnumerator e = futureF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          if (futureF.Result != null)
          {
            WebAPI.Response.ExploreChallengeStart exploreChallengeStartInfo = futureF.Result;
            GameCore.ColosseumResult battle_result = ColosseumBattleCalc.calcColosseum(ColosseumEnvironmentInitializer.initializeData(new ColosseumInitialData(exploreChallengeStartInfo, 0), (ColosseumEnvironment) null), exploreChallengeStartInfo.gladiator.player_id);
            foreach (DuelColosseumResult duelColosseumResult in battle_result.duelResult)
              duelColosseumResult.isExploreChallenge = true;
            sw.Stop();
            int num = 3000;
            if (sw.ElapsedMilliseconds <= (long) num)
              yield return (object) new WaitForSeconds((float) ((long) num - sw.ElapsedMilliseconds) / 1000f);
            exploreChallengePopup.dirExploreChallengeMatchup.transform.Clear();
            exploreChallengePopup.StartCoroutine(exploreChallengePopup.footer.changeExplore033ChallengeScene(exploreChallengeStartInfo.gladiator, battle_result));
            Singleton<ExploreDataManager>.GetInstance().InitReopenPopupState();
            Singleton<PopupManager>.GetInstance().dismiss();
          }
        }
      }
    }
  }

  private IEnumerator playChallengeMatchUpEffect()
  {
    Future<GameObject> loader = new ResourceObject("Prefabs/explore033_Top/explore_Challenge_MatchUp").Load<GameObject>();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    loader.Result.Clone(this.dirExploreChallengeMatchup.transform);
    Singleton<CommonRoot>.GetInstance().WhiteFadeIn();
  }

  private void RepairConfirmationYesBtn()
  {
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    Bugu00524Scene.ChangeSceneFromExplore(true);
  }

  private void RepairConfirmationNoBtn()
  {
    Object.Destroy((Object) this.nowPopup);
    this.nowPopup = (GameObject) null;
    this.StartCoroutine(this.IsPushOff());
  }

  public void onChallengeTeamFomationButton()
  {
    if (this.IsPushAndSet())
      return;
    Explore033DeckEditScene.ChangeSceneChallengeDeckEdit();
  }

  public void onCloseButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
    Singleton<ExploreDataManager>.GetInstance().InitReopenPopupState();
  }

  public override void onBackButton() => this.onCloseButton();

  private IEnumerator ShowUnitAlertPopup()
  {
    Future<GameObject> prefabf = new ResourceObject("Prefabs/explore033_Top/popup_ChallengeDeckShortage_anim_popup").Load<GameObject>();
    IEnumerator e = prefabf.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(prefabf.Result);
  }
}
