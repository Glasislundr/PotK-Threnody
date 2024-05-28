// Decompiled with JetBrains decompiler
// Type: CorpsQuestBattleEntryMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using DeckOrganization;
using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/CorpsQuest/BattleEntryMenu")]
public class CorpsQuestBattleEntryMenu : BackButtonMenuBase
{
  private const int DECK_UNIT_MAX = 6;
  private const float LINK_WIDTH = 92f;
  private const float LINK_DEFWIDTH = 114f;
  private const float ICON_SCALE = 0.807017565f;
  [SerializeField]
  private UILabel lblTotalPowerValue;
  [SerializeField]
  private GameObject[] linkCharacters;
  [SerializeField]
  private GameObject[] supplyItems;
  [SerializeField]
  private UILabel lblLeaderSkillName;
  [SerializeField]
  private UILabel lblLeaderSkillDesc;
  [SerializeField]
  private GameObject objLeaderSkillZoom;
  [SerializeField]
  private GameObject btnSortie;
  [SerializeField]
  private GameObject btnGearRepair;
  [SerializeField]
  private GameObject btnTeamEdit;
  [SerializeField]
  private ToggleTweenPositionControl toggleAuto_;
  private GameObject unitIconPrefab;
  private GameObject itemIconPrefab;
  private GameObject skillDetailPrefab;
  private GameObject detailPopup;
  private GameObject goHpGauge;
  private List<PlayerUnit> selectedUnits;
  private int PeriodID;
  private int StageID;
  private Modified<PlayerCorps[]> modifiedCorps;
  private Modified<PlayerCorpsDeck[]> modifiedDeck;
  private PlayerCorps playerCorps;
  private PlayerCorpsDeck playerDeck;
  private HashSet<int> usedUnits;
  private Persist.AutoBattleSetting saveData_;
  private PlayerUnitLeader_skills leaderSkill;
  private bool isCompletedOverkillersDeck = true;

  private string totalCombat
  {
    get
    {
      return this.selectedUnits.Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => !(x != (PlayerUnit) null) ? 0 : Judgement.NonBattleParameter.FromPlayerUnit(x).Combat)).ToString();
    }
  }

  private void SetButtonVisibility()
  {
    this.btnSortie.SetActive(false);
    this.btnGearRepair.SetActive(false);
    this.btnTeamEdit.SetActive(false);
    if (this.selectedUnits.Count <= 0 || this.selectedUnits.Any<PlayerUnit>((Func<PlayerUnit, bool>) (u => this.usedUnits.Contains(u.id))) || !this.isCompletedOverkillersDeck)
      this.btnTeamEdit.SetActive(true);
    else if (this.selectedUnits.Any<PlayerUnit>((Func<PlayerUnit, bool>) (u =>
    {
      if (u.equippedGear != (PlayerItem) null && u.equippedGear.broken || u.equippedGear2 != (PlayerItem) null && u.equippedGear2.broken)
        return true;
      return u.equippedGear3 != (PlayerItem) null && u.equippedGear3.broken;
    })))
      this.btnGearRepair.SetActive(true);
    else
      this.btnSortie.SetActive(true);
  }

  private IEnumerator SetSupply()
  {
    PlayerItem[] items = this.playerCorps.supplies;
    IEnumerator e;
    if (Object.op_Equality((Object) this.itemIconPrefab, (Object) null))
    {
      Future<GameObject> itemIcon = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
      e = itemIcon.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.itemIconPrefab = itemIcon.Result;
      itemIcon = (Future<GameObject>) null;
    }
    for (int i = 0; i < this.supplyItems.Length; ++i)
    {
      this.supplyItems[i].transform.Clear();
      ItemIcon n = this.itemIconPrefab.CloneAndGetComponent<ItemIcon>(this.supplyItems[i].transform);
      ((Component) n).transform.localScale = new Vector3()
      {
        x = 0.807017565f,
        y = 0.807017565f
      };
      if (i < items.Length)
      {
        e = n.InitByPlayerItem(items[i]);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        int temp = i;
        n.onClick = (Action<ItemIcon>) (supplyicon => this.StartCoroutine(this.setDetailPopup(items[temp].supply.ID)));
      }
      else
      {
        n.SetModeSupply();
        n.SetEmpty(true);
      }
      n = (ItemIcon) null;
    }
  }

  private IEnumerator setDetailPopup(int itemid)
  {
    IEnumerator e;
    if (Object.op_Equality((Object) this.detailPopup, (Object) null))
    {
      Future<GameObject> prefabF = Res.Prefabs.popup.popup_000_7_4_2__anim_popup01.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.detailPopup = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    GameObject popup = Singleton<PopupManager>.GetInstance().open(this.detailPopup);
    popup.SetActive(false);
    e = popup.GetComponent<Shop00742Menu>().Init(MasterDataTable.CommonRewardType.supply, itemid);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
  }

  private IEnumerator SetTeam()
  {
    this.leaderSkill = (PlayerUnitLeader_skills) null;
    this.lblTotalPowerValue.SetTextLocalize(this.totalCombat);
    int i;
    IEnumerator e;
    if (this.selectedUnits == null || this.selectedUnits.Count <= 0)
    {
      for (i = 0; i < 6; ++i)
      {
        e = this.LoadUnitPrefab(i, (PlayerUnit) null);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      this.resetLeaderSkill();
    }
    else
    {
      for (i = 0; i < this.selectedUnits.Count; ++i)
      {
        e = this.LoadUnitPrefab(i, this.selectedUnits[i]);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (i == 0)
          this.leaderSkill = this.selectedUnits[i].leader_skill;
      }
      this.resetLeaderSkill();
      for (i = this.selectedUnits.Count; i < 6; ++i)
      {
        e = this.LoadUnitPrefab(i, (PlayerUnit) null);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  private void resetLeaderSkill()
  {
    if (!this.isCompletedOverkillersDeck)
    {
      this.lblLeaderSkillName.SetText("---");
      this.lblLeaderSkillDesc.SetText(Consts.GetInstance().QUEST_0028_INDICATOR_LIMITED_OVERKILLERS);
      this.objLeaderSkillZoom.SetActive(false);
    }
    else if (this.leaderSkill != null)
    {
      BattleskillSkill skill = this.leaderSkill.skill;
      this.lblLeaderSkillName.SetTextLocalize(skill.name);
      this.lblLeaderSkillDesc.SetTextLocalize(skill.description);
      this.objLeaderSkillZoom.SetActive(true);
    }
    else
    {
      this.lblLeaderSkillName.SetText("---");
      this.lblLeaderSkillDesc.SetText("-----");
      this.objLeaderSkillZoom.SetActive(false);
    }
  }

  public IEnumerator LoadUnitPrefab(int index, PlayerUnit unit)
  {
    Future<GameObject> unitIconPrefabF;
    IEnumerator e;
    if (Object.op_Equality((Object) this.unitIconPrefab, (Object) null))
    {
      unitIconPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = unitIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.unitIconPrefab = unitIconPrefabF.Result;
      unitIconPrefabF = (Future<GameObject>) null;
    }
    GameObject unitIconGo = this.unitIconPrefab.Clone(this.linkCharacters[index].transform);
    if (Object.op_Equality((Object) this.goHpGauge, (Object) null))
    {
      unitIconPrefabF = Res.Prefabs.tower.dir_hp_gauge.Load<GameObject>();
      e = unitIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.goHpGauge = unitIconPrefabF.Result;
      unitIconPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Inequality((Object) this.goHpGauge, (Object) null) && unit != (PlayerUnit) null)
    {
      UnitIcon component = unitIconGo.GetComponent<UnitIcon>();
      if (Object.op_Inequality((Object) component, (Object) null))
      {
        this.goHpGauge.Clone(unitIconGo.GetComponent<UnitIcon>().hp_gauge.transform);
        component.HpGauge.SetGaugeAndDropoutIcon(100, 100, false);
      }
    }
    unitIconGo.transform.localScale = new Vector3(0.807017565f, 0.807017565f);
    UnitIcon unitScript = unitIconGo.GetComponent<UnitIcon>();
    e = unitScript.setSimpleUnit(unit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitScript.setLevelText(unit);
    unitScript.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    if (unit != (PlayerUnit) null)
    {
      unitScript.onClick = (Action<UnitIconBase>) (x => this.StartCoroutine(this.ChangeDetailScene(unit, index)));
      EventDelegate.Set(unitScript.Button.onLongPress, (EventDelegate.Callback) (() => this.StartCoroutine(this.ChangeDetailScene(unit, index))));
      unitScript.BreakWeapon = unit.IsBrokenEquippedGear;
      unitScript.SpecialIcon = false;
      unitScript.Gray = this.usedUnits.Contains(unit.id);
    }
    else
      unitScript.SetEmpty();
    unitScript.Favorite = false;
  }

  private IEnumerator ChangeDetailScene(PlayerUnit unit, int index)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    yield return (object) null;
    Unit0042Scene.changeScene(true, unit, this.selectedUnits.Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null)).ToArray<PlayerUnit>());
    this.DestroyObject();
  }

  private void DestroyObject()
  {
    foreach (GameObject linkCharacter in this.linkCharacters)
    {
      UnitIcon componentInChildren = linkCharacter.GetComponentInChildren<UnitIcon>();
      if (Object.op_Inequality((Object) componentInChildren, (Object) null))
        Object.Destroy((Object) ((Component) componentInChildren).gameObject);
    }
    foreach (GameObject supplyItem in this.supplyItems)
    {
      ItemIcon componentInChildren = supplyItem.GetComponentInChildren<ItemIcon>();
      if (Object.op_Inequality((Object) componentInChildren, (Object) null))
        Object.Destroy((Object) ((Component) componentInChildren).gameObject);
    }
  }

  private IEnumerator StartSortie()
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    if (this.selectedUnits.Count > 0)
      Singleton<NGSoundManager>.GetInstance().playVoiceByID(this.selectedUnits[0].unit.unitVoicePattern, 70, 0);
    Future<WebAPI.Response.QuestCorpsBattleStart> f = WebAPI.QuestCorpsBattleStart(this.PeriodID, this.StageID, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
    IEnumerator e1 = f.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (f.Result == null)
    {
      MypageScene.ChangeSceneOnError();
    }
    else
    {
      BattleInfo battleInfo = BattleInfo.MakeCorpsBattleInfo(f.Result, this.PeriodID);
      NGBattleManager instance = Singleton<NGBattleManager>.GetInstance();
      instance.deleteSavedEnvironment();
      instance.startBattle(battleInfo);
    }
  }

  private void setCorpsDeckUnits()
  {
    PlayerUnit[] myUnits = SMManager.Get<PlayerUnit[]>();
    this.selectedUnits = this.playerDeck != null ? ((IEnumerable<int>) this.playerDeck.deck_player_unit_ids).Select<int, PlayerUnit>((Func<int, PlayerUnit>) (i => Array.Find<PlayerUnit>(myUnits, (Predicate<PlayerUnit>) (x => x.id == i)))).ToList<PlayerUnit>() : new List<PlayerUnit>();
  }

  protected override void Update()
  {
    base.Update();
    if (this.saveData_ == null || this.saveData_.isAutoBattle == this.toggleAuto_.isSwitch)
      return;
    this.saveData_.isAutoBattle = this.toggleAuto_.isSwitch;
  }

  private void onClosedSkillZoom() => this.IsPush = false;

  private IEnumerator UpdateDisplayDeckUnits()
  {
    CorpsQuestBattleEntryMenu questBattleEntryMenu = this;
    if (questBattleEntryMenu.modifiedDeck.Changed)
    {
      // ISSUE: reference to a compiler-generated method
      questBattleEntryMenu.playerDeck = Array.Find<PlayerCorpsDeck>(SMManager.Get<PlayerCorpsDeck[]>(), new Predicate<PlayerCorpsDeck>(questBattleEntryMenu.\u003CUpdateDisplayDeckUnits\u003Eb__44_0));
      questBattleEntryMenu.modifiedDeck.Commit();
      questBattleEntryMenu.setCorpsDeckUnits();
      OverkillersUtil.checkCompletedDeck(questBattleEntryMenu.selectedUnits.ToArray(), out questBattleEntryMenu.isCompletedOverkillersDeck);
      questBattleEntryMenu.SetButtonVisibility();
      questBattleEntryMenu.DestroyObject();
      IEnumerator e = questBattleEntryMenu.SetTeam();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = questBattleEntryMenu.SetSupply();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public IEnumerator InitializeAsync(int periodId, int stageId)
  {
    this.PeriodID = periodId;
    this.StageID = stageId;
    this.modifiedCorps = SMManager.Observe<PlayerCorps[]>();
    this.modifiedDeck = SMManager.Observe<PlayerCorpsDeck[]>();
    this.playerCorps = Array.Find<PlayerCorps>(SMManager.Get<PlayerCorps[]>(), (Predicate<PlayerCorps>) (x => x.period_id == periodId));
    this.playerDeck = this.playerCorps != null ? Array.Find<PlayerCorpsDeck>(SMManager.Get<PlayerCorpsDeck[]>(), (Predicate<PlayerCorpsDeck>) (x => x.corps_id == this.playerCorps.corps_id)) : (PlayerCorpsDeck) null;
    this.usedUnits = this.playerCorps != null ? new HashSet<int>((IEnumerable<int>) this.playerCorps.used_player_unit_ids) : new HashSet<int>();
    this.setCorpsDeckUnits();
    if (Object.op_Equality((Object) this.skillDetailPrefab, (Object) null))
    {
      Future<GameObject> loader = PopupSkillDetails.createPrefabLoader(Singleton<NGGameDataManager>.GetInstance().IsSea);
      yield return (object) loader.Wait();
      this.skillDetailPrefab = loader.Result;
      loader = (Future<GameObject>) null;
    }
    OverkillersUtil.checkCompletedDeck(this.selectedUnits.ToArray(), out this.isCompletedOverkillersDeck);
    this.SetButtonVisibility();
    try
    {
      this.saveData_ = Persist.autoBattleSetting.Data;
    }
    catch
    {
      Persist.autoBattleSetting.Delete();
      this.saveData_ = Persist.autoBattleSetting.Data = new Persist.AutoBattleSetting();
    }
    this.toggleAuto_.resetSwitch(this.saveData_.isAutoBattle);
    this.DestroyObject();
    IEnumerator e = this.SetTeam();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.SetSupply();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onGearRepairButton()
  {
    if (this.IsPushAndSet())
      return;
    Bugu00524Scene.ChangeScene(true);
  }

  public void onSortieButton()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.StartSortie());
  }

  public void onTeamEditButton()
  {
    if (this.IsPushAndSet())
      return;
    CorpsQuestTeamEditScene.ChangeScene(this.playerCorps.period_id);
  }

  private IEnumerator AutoSelectAsync()
  {
    bool bok = false;
    CommonElement element = CommonElement.none;
    IEnumerator waitPopup = Unit0046ConfirmAutoOrganization.doPopup((string) null, (Action<CommonElement>) (_element =>
    {
      bok = true;
      element = _element;
    }));
    while (waitPopup.MoveNext())
      yield return waitPopup.Current;
    if (bok)
    {
      IEnumerable<PlayerUnit> source = (IEnumerable<PlayerUnit>) this.getUsableUnits().Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.cost <= 75)).ToArray<PlayerUnit>();
      if (element != CommonElement.none)
        source = source.Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.GetElement() == element));
      Creator deckCreator = new Creator((PlayerUnit[]) null, source.ToArray<PlayerUnit>(), (List<DeckOrganization.Filter>) null, 1, 6);
      IEnumerator e1 = deckCreator.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (deckCreator.isSuccess)
      {
        int[] player_unit_ids = new int[0];
        if (deckCreator.result_ != null)
          player_unit_ids = deckCreator.result_.Select<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.id)).ToArray<int>();
        Future<WebAPI.Response.QuestCorpsDeckEdit> f = WebAPI.QuestCorpsDeckEdit(this.PeriodID, player_unit_ids, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
        e1 = f.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        if (f.Result == null)
        {
          MypageScene.ChangeSceneOnError();
        }
        else
        {
          e1 = this.UpdateDisplayDeckUnits();
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
          f = (Future<WebAPI.Response.QuestCorpsDeckEdit>) null;
        }
      }
      else
      {
        bool bwait = true;
        ModalWindow.Show(Consts.GetInstance().UNIT_0046_AUTODECK_ERROR_TITLE, Consts.GetInstance().UNIT_0046_AUTODECK_ERROR_MESSAGE, (Action) (() => bwait = false));
        while (bwait)
          yield return (object) null;
      }
    }
  }

  private IEnumerable<PlayerUnit> getUsableUnits()
  {
    if (this.playerCorps == null)
      return (IEnumerable<PlayerUnit>) new PlayerUnit[0];
    PlayerUnit[] myUnits = SMManager.Get<PlayerUnit[]>();
    return ((IEnumerable<int>) this.playerCorps.entry_player_unit_ids).Where<int>((Func<int, bool>) (i => !((IEnumerable<int>) this.playerCorps.used_player_unit_ids).Contains<int>(i))).Select<int, PlayerUnit>((Func<int, PlayerUnit>) (n => Array.Find<PlayerUnit>(myUnits, (Predicate<PlayerUnit>) (x => x.id == n))));
  }

  public void onAutoEditButton()
  {
    if (this.IsPushAndSet())
      return;
    this.IsPush = false;
    this.StartCoroutine(this.AutoSelectAsync());
  }

  public void IbtnBattleSetting()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.doPopupBattleSetting());
  }

  private IEnumerator doPopupBattleSetting()
  {
    CorpsQuestBattleEntryMenu questBattleEntryMenu = this;
    IEnumerator e = Quest0028PopupBattleSetting.show();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    questBattleEntryMenu.IsPush = false;
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public void onClickedLeaderSkillZoom()
  {
    if (this.leaderSkill == null || Object.op_Equality((Object) this.skillDetailPrefab, (Object) null) || this.IsPushAndSet())
      return;
    PopupSkillDetails.show(this.skillDetailPrefab, new PopupSkillDetails.Param(this.leaderSkill), onClosed: new Action(this.onClosedSkillZoom));
  }
}
