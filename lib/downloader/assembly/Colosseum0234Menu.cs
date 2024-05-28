// Decompiled with JetBrains decompiler
// Type: Colosseum0234Menu
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
public class Colosseum0234Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtCustomeName;
  [SerializeField]
  protected UILabel TxtCustomeName2;
  [SerializeField]
  protected UILabel TxtCustomeName3;
  [SerializeField]
  protected UILabel TxtFightnum;
  [SerializeField]
  protected UILabel TxtFightnum2;
  [SerializeField]
  protected UILabel TxtFightnum3;
  [SerializeField]
  protected UILabel TxtLVnumber;
  [SerializeField]
  protected UILabel TxtLVnumber2;
  [SerializeField]
  protected UILabel TxtLVnumber3;
  [SerializeField]
  protected UILabel TxtRankBattle01;
  [SerializeField]
  protected UILabel TxtRankBattle02;
  [SerializeField]
  protected UILabel TxtRankname;
  [SerializeField]
  protected UILabel TxtRankname2;
  [SerializeField]
  protected UILabel TxtRankname3;
  [SerializeField]
  protected UILabel TxtRankpt;
  [SerializeField]
  protected UILabel TxtRankpt2;
  [SerializeField]
  protected UILabel TxtRankpt3;
  [SerializeField]
  private UIButton ibtnRanking;
  [SerializeField]
  private GameObject dirScrollText;
  [SerializeField]
  private GameObject firstBottomObject;
  [SerializeField]
  private GameObject secondBottomObject;
  [SerializeField]
  private GameObject[] unitIcons;
  [SerializeField]
  private GameObject bgCharacter;
  [SerializeField]
  private float matchingChangeTime = 0.3f;
  [SerializeField]
  private GameObject[] opponentUnitIcons;
  [SerializeField]
  private GameObject[] opponentUnitIconsBase;
  [SerializeField]
  private UIButton opponentUpdateButton;
  [SerializeField]
  private GameObject DirBonus;
  [SerializeField]
  private UILabel bonusText;
  [SerializeField]
  private UILabel bonusLimitText;
  [SerializeField]
  private UILabel liderSkillText;
  [SerializeField]
  private GameObject selectButton;
  [SerializeField]
  private GameObject repairButton;
  private Colosseum0234Status statusWnd;
  private bool isRepair;
  private GameObject ScrollTextPrefab;
  private ColosseumUtility.Info colosseumInfo;
  private int[] opponents;
  private DeckInfo deck_;
  private GameObject unitPrefab;
  public Dictionary<int, UnitIcon.SpriteCache> unitIconCache;
  private GameObject nowPopup;
  private Colosseum0234Menu.HeaderType headerType;
  private Colosseum0234Scene scene;

  public int[] Opponents => this.opponents;

  public override void onBackButton()
  {
    if (this.colosseumInfo.is_tutorial)
      this.showBackKeyToast();
    else if (this.headerType == Colosseum0234Menu.HeaderType.HOME)
      this.IbtnHome();
    else
      this.IbtnBack();
  }

  private void WarningYesBtn() => this.StartCoroutine(this.Resume());

  private void WarningNoBtn()
  {
    this.nowPopup = ((Component) ModalWindow.ShowYesNo(Consts.GetInstance().COLOSSEUM_RESUME_TITLE2, Consts.GetInstance().COLOSSEUM_RESUME_MESSAGE2_1, new Action(this.ConfirmationYesBtn), new Action(this.ConfirmationNoBtn))).gameObject;
  }

  private void ConfirmationYesBtn() => this.StartCoroutine(this.doForceClose());

  private void ConfirmationNoBtn()
  {
    Object.DestroyObject((Object) this.nowPopup);
    this.nowPopup = ((Component) ModalWindow.ShowYesNo(Consts.GetInstance().COLOSSEUM_RESUME_TITLE1, Consts.GetInstance().COLOSSEUM_RESUME_MESSAGE1, new Action(this.WarningYesBtn), new Action(this.WarningNoBtn))).gameObject;
  }

  private void DestoryYesBtn()
  {
    this.colosseumInfo.is_battle = false;
    Object.DestroyObject((Object) this.nowPopup);
    this.StartCoroutine(this.Restart());
  }

  private IEnumerator Restart()
  {
    IEnumerator e = this.scene.Restart(new Colosseum0234Scene.Param(true, (int[]) null, (ColosseumUtility.Info) null), this.colosseumInfo.is_tutorial);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator Resume()
  {
    Object.Destroy((Object) this.nowPopup);
    IEnumerator e1;
    if (this.colosseumInfo.is_tutorial)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      Persist.colosseumTutorial.Data.CurrentPage = 2;
      Future<WebAPI.Response.ColosseumTutorialResume> futureF = WebAPI.ColosseumTutorialResume((Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      e1 = futureF.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (futureF.Result != null)
      {
        GameCore.ColosseumResult battle_result = ColosseumBattleCalc.calcColosseum(ColosseumEnvironmentInitializer.initializeData(new ColosseumInitialData(futureF.Result, 0), (ColosseumEnvironment) null));
        Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
        Coloseum02342Scene.changeScene(this.colosseumInfo, futureF.Result, battle_result);
        futureF = (Future<WebAPI.Response.ColosseumTutorialResume>) null;
      }
    }
    else
    {
      Future<WebAPI.Response.ColosseumResume> futureF = WebAPI.ColosseumResume((Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      e1 = futureF.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (futureF.Result != null)
      {
        Singleton<CommonRoot>.GetInstance().isLoading = true;
        GameCore.ColosseumResult battle_result = ColosseumBattleCalc.calcColosseum(ColosseumEnvironmentInitializer.initializeData(new ColosseumInitialData(futureF.Result, 0), (ColosseumEnvironment) null));
        Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
        Coloseum02342Scene.changeScene(this.colosseumInfo, futureF.Result, battle_result);
        futureF = (Future<WebAPI.Response.ColosseumResume>) null;
      }
    }
  }

  private IEnumerator doForceClose(bool bPostMessage = true)
  {
    Colosseum0234Menu colosseum0234Menu = this;
    Object.Destroy((Object) colosseum0234Menu.nowPopup);
    CommonRoot cRoot = Singleton<CommonRoot>.GetInstance();
    cRoot.ShowLoadingLayer(1);
    IEnumerator e1;
    if (colosseum0234Menu.colosseumInfo.is_tutorial)
    {
      Future<WebAPI.Response.ColosseumTutorialForceClose> futureF = WebAPI.ColosseumTutorialForceClose((Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      e1 = futureF.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (futureF.Result == null)
        yield break;
      else
        futureF = (Future<WebAPI.Response.ColosseumTutorialForceClose>) null;
    }
    else
    {
      Future<WebAPI.Response.ColosseumForceClose> futureF = WebAPI.ColosseumForceClose((Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      e1 = futureF.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (futureF.Result == null)
        yield break;
      else
        futureF = (Future<WebAPI.Response.ColosseumForceClose>) null;
    }
    cRoot.HideLoadingLayer();
    cRoot.loadingMode = 0;
    if (bPostMessage)
    {
      colosseum0234Menu.nowPopup = ((Component) ModalWindow.Show(Consts.GetInstance().COLOSSEUM_RESUME_TITLE2, Consts.GetInstance().COLOSSEUM_RESUME_MESSAGE2_2, new Action(colosseum0234Menu.DestoryYesBtn))).gameObject;
    }
    else
    {
      colosseum0234Menu.colosseumInfo.is_battle = false;
      colosseum0234Menu.colosseumInfo.resume_able = true;
      colosseum0234Menu.StartCoroutine(colosseum0234Menu.Restart());
    }
  }

  private void NotGladiators()
  {
    Object.Destroy((Object) this.nowPopup);
    this.IbtnHome();
  }

  private IEnumerator SetLiderSkillInfo()
  {
    GameObject gameObject;
    if (Object.op_Equality((Object) this.ScrollTextPrefab, (Object) null))
    {
      Future<GameObject> h = Res.Prefabs.colosseum.colosseum023_4.ScrollText.Load<GameObject>();
      IEnumerator e = h.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.ScrollTextPrefab = h.Result;
      gameObject = this.ScrollTextPrefab.Clone(this.dirScrollText.transform);
      ((Object) gameObject).name = "ScrollText";
      h = (Future<GameObject>) null;
    }
    else
      gameObject = ((Component) this.dirScrollText.transform.GetChildInFind("ScrollText")).gameObject;
    PlayerUnit playerUnit = ((IEnumerable<PlayerUnit>) this.deck_.player_units).FirstOrDefault<PlayerUnit>();
    if (playerUnit != (PlayerUnit) null)
    {
      if (playerUnit.leader_skill != null)
      {
        this.liderSkillText.SetTextLocalize(playerUnit.leader_skill.skill.name);
        string text = playerUnit.leader_skill.skill.description.Replace("\r", "").Replace("\n", "");
        gameObject.GetComponent<Colosseum0234ScrollText>().StartScroll(text);
      }
      else
      {
        this.liderSkillText.SetTextLocalize(Consts.GetInstance().COLOSSEUM_TOP_NOT);
        gameObject.GetComponent<Colosseum0234ScrollText>().StartScroll(Consts.GetInstance().COLOSSEUM_TOP_NOT_LEADER_SKILL);
      }
    }
    else
    {
      this.liderSkillText.SetTextLocalize(Consts.GetInstance().COLOSSEUM_TOP_NOT);
      gameObject.GetComponent<Colosseum0234ScrollText>().StartScroll(Consts.GetInstance().COLOSSEUM_TOP_NOT_LEADER);
    }
  }

  public IEnumerator Initialize(
    Colosseum0234Scene scene,
    ColosseumUtility.Info colosseumInfo,
    int[] opponents,
    bool isTutorial,
    RankingPlayer myRanking)
  {
    Colosseum0234Menu menu = this;
    Singleton<NGGameDataManager>.GetInstance().IsColosseum = true;
    menu.scene = scene;
    menu.colosseumInfo = colosseumInfo;
    menu.deck_ = menu.GetDeck();
    menu.isRepair = false;
    if (menu.opponents == null)
      menu.opponents = opponents;
    IEnumerator e = menu.SetLiderSkillInfo();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    menu.firstBottomObject.SetActive(false);
    menu.secondBottomObject.SetActive(false);
    if (Object.op_Equality((Object) menu.statusWnd, (Object) null))
      menu.statusWnd = ((Component) menu).gameObject.GetComponentInChildren<Colosseum0234Status>();
    if (Object.op_Inequality((Object) menu.statusWnd, (Object) null))
    {
      e = menu.statusWnd.Initialize(colosseumInfo, menu, myRanking);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
      Debug.LogWarning((object) "ステータスウィンドが無いか、Colosseum0234Statusがアタッチされていません。");
    ((UIButtonColor) menu.ibtnRanking).isEnabled = false;
    if (SMManager.Get<Player>().GetFeatureColosseumRanking())
      ((UIButtonColor) menu.ibtnRanking).isEnabled = true;
    ((UIButtonColor) menu.opponentUpdateButton).isEnabled = true;
    if (colosseumInfo.is_tutorial || colosseumInfo.next_battle_type != 0)
      ((UIButtonColor) menu.opponentUpdateButton).isEnabled = false;
    bool flag = menu.EquipBrokenWeapon();
    menu.selectButton.SetActive(!flag);
    menu.repairButton.SetActive(flag);
    e = menu.SetBgCharacter();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = menu.SetUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = menu.SetOther();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (colosseumInfo.is_battle)
    {
      // ISSUE: reference to a compiler-generated method
      menu.nowPopup = (colosseumInfo.resume_able ? (Component) ModalWindow.ShowYesNo(Consts.GetInstance().COLOSSEUM_RESUME_TITLE1, Consts.GetInstance().COLOSSEUM_RESUME_MESSAGE1, new Action(menu.WarningYesBtn), new Action(menu.WarningNoBtn)) : (Component) ModalWindow.Show(Consts.GetInstance().COLOSSEUM_RESUME_TITLE1, Consts.GetInstance().COLOSSEUM_RESUME_MESSAGE9, new Action(menu.\u003CInitialize\u003Eb__58_0))).gameObject;
    }
    else
    {
      if (colosseumInfo.is_tutorial)
      {
        Singleton<CommonRoot>.GetInstance().DisableColosseumHeaderBtn();
        if (Persist.colosseumTutorial.Data.CurrentPage == 0)
        {
          Persist.colosseumTutorial.Data.CurrentPage = 1;
          // ISSUE: reference to a compiler-generated method
          Singleton<TutorialRoot>.GetInstance().ForceShowAdviceInNextButton("colosseum1", new Dictionary<string, Func<Transform, UIButton>>()
          {
            {
              "colosseum4",
              new Func<Transform, UIButton>(menu.\u003CInitialize\u003Eb__58_1)
            }
          });
        }
      }
      if (isTutorial && Persist.colosseumTutorial.Data.CurrentPage == 3)
      {
        Persist.colosseumTutorial.Data.CurrentPage = 4;
        Singleton<TutorialRoot>.GetInstance().ForceShowAdvice("colosseum4");
        menu.colosseumInfo.is_tutorial = false;
        e = Singleton<CommonRoot>.GetInstance().GetColosseumHeaderComponent().SetInfo(CommonColosseumHeader.BtnMode.Home, new Action(menu.IbtnHome));
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  private bool EquipBrokenWeapon()
  {
    bool flag = false;
    foreach (PlayerUnit playerUnit in this.deck_.player_units)
    {
      if (playerUnit != (PlayerUnit) null)
      {
        if (playerUnit.equippedGear != (PlayerItem) null)
          flag |= playerUnit.equippedGear.broken;
        if (playerUnit.equippedGear2 != (PlayerItem) null)
          flag |= playerUnit.equippedGear2.broken;
        if (playerUnit.equippedGear3 != (PlayerItem) null)
          flag |= playerUnit.equippedGear3.broken;
      }
    }
    return flag;
  }

  private IEnumerator SetBgCharacter()
  {
    foreach (Component component in this.bgCharacter.transform)
      Object.Destroy((Object) component.gameObject);
    PlayerUnit playerUnit = ((IEnumerable<PlayerUnit>) this.deck_.player_units).FirstOrDefault<PlayerUnit>();
    if (playerUnit != (PlayerUnit) null)
    {
      IEnumerator e = playerUnit.unit.LoadQuestWithMask(playerUnit.job_id, this.bgCharacter.transform, this.bgCharacter.GetComponent<UIWidget>().depth, Res.GUI._002_2_sozai.mask_chara.Load<Texture2D>());
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private IEnumerator SetUnitIcon()
  {
    Future<GameObject> unitPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = unitPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unitPrefab = unitPrefabF.Result;
    int i;
    for (i = 0; i < this.deck_.player_units.Length; ++i)
    {
      foreach (Component component in this.unitIcons[i].transform)
        Object.Destroy((Object) component.gameObject);
      UnitIcon unitPlayer = this.unitPrefab.Clone(this.unitIcons[i].transform).GetComponent<UnitIcon>();
      PlayerUnit playerUnit = this.deck_.player_units[i];
      if (i == 0)
      {
        e = unitPlayer.setBottomUnit(playerUnit, this.deck_.player_units);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        unitPlayer.Button.onLongPress.Clear();
      }
      else
      {
        e = unitPlayer.SetPlayerUnit(playerUnit, this.deck_.player_units, (PlayerUnit) null, false, false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        unitPlayer.Button.onLongPress.Clear();
      }
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
      unitPlayer.princessType.DispPrincessType(false);
      unitPlayer = (UnitIcon) null;
      playerUnit = (PlayerUnit) null;
    }
    Gladiator[] gladiators = this.colosseumInfo.gladiators;
    for (i = 0; i < gladiators.Length; ++i)
    {
      UnitUnit unit;
      if (MasterData.UnitUnit.TryGetValue(gladiators[i].leader_unit_id, out unit))
      {
        e = UnitIcon.LoadSprite(unit);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    this.unitIconCache = UnitIcon.CopyCache();
  }

  private IEnumerator SetOther()
  {
    IEnumerator e;
    if (this.opponents == null)
    {
      e = this.ChangeHeader(Colosseum0234Menu.HeaderType.HOME);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.firstBottomObject.SetActive(true);
      this.secondBottomObject.SetActive(false);
    }
    else
    {
      e = this.ChangeHeader(Colosseum0234Menu.HeaderType.MATCHING);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.firstBottomObject.SetActive(false);
      this.secondBottomObject.SetActive(true);
      this.SetOpponents(this.opponents);
    }
    if (this.colosseumInfo.bonus != null && this.colosseumInfo.bonus.Length != 0)
    {
      Bonus[] array = ((IEnumerable<Bonus>) this.colosseumInfo.bonus).Where<Bonus>((Func<Bonus, bool>) (x => x.category != 12)).ToArray<Bonus>();
      if (array.Length == 0)
      {
        this.DirBonus.SetActive(false);
      }
      else
      {
        new BonusDisplay().Set(array, this.bonusText, this.bonusLimitText, false);
        this.DirBonus.SetActive(true);
      }
    }
    else
      this.DirBonus.SetActive(false);
  }

  private void SetOpponents(int[] oppnets = null)
  {
    this.opponents = oppnets == null ? this.GetRandomShuffle(3, this.colosseumInfo.gladiators.Length) : oppnets;
    this.opponentUnitIconsBase[0].SetActive(false);
    this.opponentUnitIconsBase[1].SetActive(false);
    this.opponentUnitIconsBase[2].SetActive(false);
    if (this.colosseumInfo.next_battle_type == 0)
    {
      for (int index = 0; index < 3; ++index)
      {
        Gladiator gladiator = this.colosseumInfo.gladiators[this.opponents[index]];
        this.opponentUnitIconsBase[index].SetActive(true);
        if (index == 0)
        {
          this.TxtCustomeName.SetText(gladiator.name.ToConverter());
          this.TxtFightnum.SetText(gladiator.total_power.ToLocalizeNumberText());
          this.TxtLVnumber.SetText(gladiator.player_level.ToLocalizeNumberText());
          this.TxtRankname.SetText(ColosseumUtility.GetRankName(gladiator.rank_pt));
          this.TxtRankpt.SetText(gladiator.rank_pt.ToLocalizeNumberText());
        }
        else if (index == 1)
        {
          this.TxtCustomeName2.SetText(gladiator.name.ToConverter());
          this.TxtFightnum2.SetText(gladiator.total_power.ToLocalizeNumberText());
          this.TxtLVnumber2.SetText(gladiator.player_level.ToLocalizeNumberText());
          this.TxtRankname2.SetText(ColosseumUtility.GetRankName(gladiator.rank_pt));
          this.TxtRankpt2.SetText(gladiator.rank_pt.ToLocalizeNumberText());
        }
        else
        {
          this.TxtCustomeName3.SetText(gladiator.name.ToConverter());
          this.TxtFightnum3.SetText(gladiator.total_power.ToLocalizeNumberText());
          this.TxtLVnumber3.SetText(gladiator.player_level.ToLocalizeNumberText());
          this.TxtRankname3.SetText(ColosseumUtility.GetRankName(gladiator.rank_pt));
          this.TxtRankpt3.SetText(gladiator.rank_pt.ToLocalizeNumberText());
        }
        foreach (Component component in this.opponentUnitIcons[index].transform)
          Object.Destroy((Object) component.gameObject);
        UnitIcon component1 = this.unitPrefab.Clone(this.opponentUnitIcons[index].transform).GetComponent<UnitIcon>();
        if (Object.op_Inequality((Object) component1, (Object) null))
          component1.setColosseumMatchingUnit(gladiator.leader_unit_id, gladiator.leader_unit_level, gladiator.leader_unit_job_id, this.unitIconCache);
      }
    }
    else if (this.colosseumInfo.next_battle_type == 1)
    {
      this.opponentUnitIconsBase[1].SetActive(true);
      Gladiator gladiator = this.colosseumInfo.gladiators[this.colosseumInfo.gladiators.Length - 2];
      this.TxtCustomeName2.SetText(gladiator.name.ToConverter());
      this.TxtFightnum2.SetText(gladiator.total_power.ToLocalizeNumberText());
      this.TxtLVnumber2.SetText(gladiator.player_level.ToLocalizeNumberText());
      this.TxtRankname2.SetText(ColosseumUtility.GetRankName(gladiator.rank_pt));
      this.TxtRankpt2.SetText(gladiator.rank_pt.ToLocalizeNumberText());
      foreach (Component component in this.opponentUnitIcons[1].transform)
        Object.Destroy((Object) component.gameObject);
      UnitIcon component2 = this.unitPrefab.Clone(this.opponentUnitIcons[1].transform).GetComponent<UnitIcon>();
      if (!Object.op_Inequality((Object) component2, (Object) null))
        return;
      component2.setColosseumMatchingUnit(gladiator.leader_unit_id, gladiator.leader_unit_level, gladiator.leader_unit_job_id, this.unitIconCache);
    }
    else
    {
      if (this.colosseumInfo.next_battle_type != 2)
        return;
      this.opponentUnitIconsBase[1].SetActive(true);
      Gladiator gladiator = this.colosseumInfo.gladiators[this.colosseumInfo.gladiators.Length - 1];
      this.TxtCustomeName2.SetText(gladiator.name.ToConverter());
      this.TxtFightnum2.SetText(gladiator.total_power.ToLocalizeNumberText());
      this.TxtLVnumber2.SetText(gladiator.player_level.ToLocalizeNumberText());
      this.TxtRankname2.SetText(ColosseumUtility.GetRankName(gladiator.rank_pt));
      this.TxtRankpt2.SetText(gladiator.rank_pt.ToLocalizeNumberText());
      foreach (Component component in this.opponentUnitIcons[1].transform)
        Object.Destroy((Object) component.gameObject);
      UnitIcon component3 = this.unitPrefab.Clone(this.opponentUnitIcons[1].transform).GetComponent<UnitIcon>();
      if (!Object.op_Inequality((Object) component3, (Object) null))
        return;
      component3.setColosseumMatchingUnit(gladiator.leader_unit_id, gladiator.leader_unit_level, gladiator.leader_unit_job_id, this.unitIconCache);
    }
  }

  public virtual void IbtnCombine()
  {
    if (this.colosseumInfo.gladiators == null || this.colosseumInfo.gladiators.Length == 0)
    {
      this.nowPopup = ((Component) ModalWindow.Show(Consts.GetInstance().COLOSSEUM_NOT_GLADIATORS_TITLE, Consts.GetInstance().COLOSSEUM_NOT_GLADIATORS_MESSAGE3, new Action(this.NotGladiators))).gameObject;
    }
    else
    {
      bool bCompleted;
      if (this.deck_.isCustom)
        OverkillersUtil.checkCompletedCustomDeck(this.deck_.player_units, out bCompleted);
      else
        OverkillersUtil.checkCompletedDeck(this.deck_.player_units, out bCompleted);
      if (!bCompleted)
      {
        Consts instance = Consts.GetInstance();
        this.StartCoroutine(PopupCommon.Show(instance.QUEST_0028_ERROR_TITLE_OVERKILLERS, instance.QUEST_0028_ERROR_MESSAGE_OVERKILLERS));
      }
      else if (((IEnumerable<PlayerUnit>) this.deck_.player_units).Count<PlayerUnit>((Func<PlayerUnit, bool>) (v => v != (PlayerUnit) null)) <= 2)
        this.StartCoroutine(this.ShowUnitAlertPopup());
      else if (this.deck_.cost > this.deck_.cost_limit)
      {
        this.nowPopup = ((Component) ModalWindow.Show(Consts.GetInstance().VERSUS_00262POPUP_COSTOVER_TITLE, Consts.GetInstance().VERSUS_00262POPUP_COSTOVER_DESCRIPTION, (Action) (() => { }))).gameObject;
      }
      else
      {
        iTween.ValueTo(((Component) this).gameObject, iTween.Hash(new object[10]
        {
          (object) "from",
          (object) 1f,
          (object) "to",
          (object) 0.0f,
          (object) "time",
          (object) this.matchingChangeTime,
          (object) "onupdate",
          (object) "FirstBottomUpdate",
          (object) "oncomplete",
          (object) "FirstBottomFinishEnd"
        }));
        Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
      }
    }
  }

  public virtual void IbtnRepair()
  {
    if (this.IsPushAndSet())
      return;
    Bugu00524Scene.ChangeScene(true);
  }

  public virtual void IbtnWarExperience()
  {
    if (this.IsPushAndSet())
      return;
    Colosseum0236Scene.ChangeScene(this.colosseumInfo);
  }

  public virtual void IbtnRanking()
  {
    if (this.IsPushAndSet())
      return;
    Colosseum02371Scene.ChangeScene(this.colosseumInfo);
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
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    Unit0046Scene.changeScene(false, this.colosseumInfo);
    Unit0046Scene.isQuestEdit = false;
  }

  private void changeSceneCustomDeckEdit()
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    EditCustomDeckScene.changeScene(this.colosseumInfo);
  }

  public virtual void IbtnCostomerUnit() => this.SelectOpponent(0);

  public virtual void IbtnCostomerUnit2() => this.SelectOpponent(1);

  public virtual void IbtnCostomerUnit3() => this.SelectOpponent(2);

  public void IbtnHome()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGDuelDataManager>.GetInstance().Init();
    MypageScene.ChangeScene();
  }

  public void IbtnBack()
  {
    iTween.ValueTo(((Component) this).gameObject, iTween.Hash(new object[10]
    {
      (object) "from",
      (object) 1f,
      (object) "to",
      (object) 0.0f,
      (object) "time",
      (object) this.matchingChangeTime,
      (object) "onupdate",
      (object) "SecondBottomUpdate",
      (object) "oncomplete",
      (object) "SecondBottomFinishEnd"
    }));
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
  }

  public void IbtnOpponentUpdate()
  {
    this.SetOpponents();
    ((UIButtonColor) this.opponentUpdateButton).isEnabled = false;
    this.StartCoroutine(this.WaitOpponentUpdateButton());
  }

  private IEnumerator WaitOpponentUpdateButton()
  {
    yield return (object) new WaitForSeconds(1.5f);
    ((UIButtonColor) this.opponentUpdateButton).isEnabled = true;
  }

  private DeckInfo GetDeck() => Persist.colosseumDeckOrganized.Data.getSelectedDeck();

  private void FirstBottomStartEnd() => Singleton<CommonRoot>.GetInstance().isTouchBlock = false;

  private void FirstBottomUpdate(float value)
  {
    ((UIRect) this.firstBottomObject.GetComponent<UIWidget>()).alpha = value;
  }

  private void FirstBottomFinishEnd()
  {
    this.firstBottomObject.SetActive(false);
    this.secondBottomObject.SetActive(true);
    ((UIRect) this.secondBottomObject.GetComponent<UIWidget>()).alpha = 0.0f;
    this.SetOpponents();
    iTween.ValueTo(((Component) this).gameObject, iTween.Hash(new object[10]
    {
      (object) "from",
      (object) 0.0f,
      (object) "to",
      (object) 1f,
      (object) "time",
      (object) this.matchingChangeTime,
      (object) "onupdate",
      (object) "SecondBottomUpdate",
      (object) "oncomplete",
      (object) "SecondBottomStartEnd"
    }));
    this.StartCoroutine(this.ChangeHeader(Colosseum0234Menu.HeaderType.MATCHING));
    if (!this.colosseumInfo.is_tutorial || Persist.colosseumTutorial.Data.CurrentPage != 1)
      return;
    Persist.colosseumTutorial.Data.CurrentPage = 2;
    Singleton<TutorialRoot>.GetInstance().ForceShowAdvice("colosseum2");
  }

  private void SecondBottomStartEnd() => Singleton<CommonRoot>.GetInstance().isTouchBlock = false;

  private void SecondBottomUpdate(float value)
  {
    ((UIRect) this.secondBottomObject.GetComponent<UIWidget>()).alpha = value;
  }

  private void SecondBottomFinishEnd()
  {
    this.firstBottomObject.SetActive(true);
    this.secondBottomObject.SetActive(false);
    ((UIRect) this.firstBottomObject.GetComponent<UIWidget>()).alpha = 0.0f;
    this.opponents = (int[]) null;
    iTween.ValueTo(((Component) this).gameObject, iTween.Hash(new object[10]
    {
      (object) "from",
      (object) 0.0f,
      (object) "to",
      (object) 1f,
      (object) "time",
      (object) this.matchingChangeTime,
      (object) "onupdate",
      (object) "FirstBottomUpdate",
      (object) "oncomplete",
      (object) "FirstBottomStartEnd"
    }));
    this.StartCoroutine(this.ChangeHeader(Colosseum0234Menu.HeaderType.HOME));
  }

  private int[] GetRandomShuffle(int arrayMax, int rangeMax)
  {
    int[] source = new int[rangeMax];
    for (int index = 0; index < source.Length; ++index)
      source[index] = index;
    int[] array = ((IEnumerable<int>) source).OrderBy<int, Guid>((Func<int, Guid>) (i => Guid.NewGuid())).ToArray<int>();
    int[] destinationArray = new int[arrayMax];
    Array.Copy((Array) array, (Array) destinationArray, 3);
    return destinationArray;
  }

  private IEnumerator ChangeHeader(Colosseum0234Menu.HeaderType type)
  {
    Colosseum0234Menu colosseum0234Menu = this;
    CommonColosseumHeader colosseumHeaderComponent = Singleton<CommonRoot>.GetInstance().GetColosseumHeaderComponent();
    colosseum0234Menu.headerType = type;
    IEnumerator e;
    if (type == Colosseum0234Menu.HeaderType.HOME)
    {
      e = colosseumHeaderComponent.SetInfo(CommonColosseumHeader.BtnMode.Home, new Action(colosseum0234Menu.IbtnHome));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = colosseumHeaderComponent.SetInfo(CommonColosseumHeader.BtnMode.Back, new Action(colosseum0234Menu.IbtnBack));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    if (colosseum0234Menu.colosseumInfo.is_tutorial)
      Singleton<CommonRoot>.GetInstance().DisableColosseumHeaderBtn();
  }

  private void SelectOpponent(int index)
  {
    if (SMManager.Get<Player>().bp < 1)
      this.StartCoroutine(this.ShowCPAlertPopup());
    else
      this.StartCoroutine(this.BattleStart(index));
  }

  private IEnumerator BattleStart(int index)
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 4;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    int idx = this.opponents[index];
    IEnumerator e;
    GameCore.ColosseumResult battle_result;
    if (this.colosseumInfo.is_tutorial)
    {
      Future<WebAPI.Response.ColosseumTutorialStart> futureF = WebAPI.ColosseumTutorialStart(this.deck_.deck_number, this.deck_.deck_type_id, this.colosseumInfo.gladiators[idx].player_id, index, this.deck_.total_combat, (Action<WebAPI.Response.UserError>) (error =>
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        WebAPI.DefaultUserErrorCallback(error);
        MypageScene.ChangeSceneOnError();
      }));
      e = futureF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (futureF.Result == null)
      {
        yield break;
      }
      else
      {
        battle_result = ColosseumBattleCalc.calcColosseum(ColosseumEnvironmentInitializer.initializeData(new ColosseumInitialData(futureF.Result, 0), (ColosseumEnvironment) null));
        futureF = (Future<WebAPI.Response.ColosseumTutorialStart>) null;
      }
    }
    else
    {
      if (this.colosseumInfo.next_battle_type == 1)
        idx = this.colosseumInfo.gladiators.Length - 2;
      else if (this.colosseumInfo.next_battle_type == 2)
        idx = this.colosseumInfo.gladiators.Length - 1;
      Future<WebAPI.Response.ColosseumStart> futureF = WebAPI.ColosseumStart(this.deck_.deck_number, this.deck_.deck_type_id, this.colosseumInfo.gladiators[idx].player_id, index, this.deck_.total_combat, (Action<WebAPI.Response.UserError>) (error =>
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        WebAPI.DefaultUserErrorCallback(error);
        MypageScene.ChangeSceneOnError();
      }));
      e = futureF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (futureF.Result == null)
      {
        yield break;
      }
      else
      {
        battle_result = ColosseumBattleCalc.calcColosseum(ColosseumEnvironmentInitializer.initializeData(new ColosseumInitialData(futureF.Result, 0), (ColosseumEnvironment) null));
        futureF = (Future<WebAPI.Response.ColosseumStart>) null;
      }
    }
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
    Coloseum02342Scene.changeScene(this.colosseumInfo, idx, battle_result);
  }

  private IEnumerator ShowCPAlertPopup()
  {
    Future<GameObject> prefabf = Res.Prefabs.popup.popup_023_4_11__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabf.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(prefabf.Result);
  }

  private IEnumerator ShowCPErrorPopup()
  {
    Future<GameObject> prefabf = Res.Prefabs.popup.popup_023_4_19__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabf.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(prefabf.Result);
  }

  private IEnumerator ShowUnitAlertPopup()
  {
    Colosseum0234Menu colosseum0234Menu = this;
    Future<GameObject> prefabf = Res.Prefabs.popup.popup_023_4_17__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabf.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(prefabf.Result).GetComponent<Popup023417Menu>().execChangeScene = new Action(colosseum0234Menu.popupDeckEditSelect);
  }

  private enum HeaderType
  {
    HOME,
    MATCHING,
  }
}
