// Decompiled with JetBrains decompiler
// Type: BattleUI55ResultMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class BattleUI55ResultMenu : ResultMenuBase
{
  [SerializeField]
  protected UILabel TxtSubTitle24;
  [SerializeField]
  protected UILabel TxtGetZenie28;
  [SerializeField]
  private GameObject Title;
  [SerializeField]
  private GameObject Scene_Result;
  [SerializeField]
  private UIGrid grid;
  private new BattleInfo info;
  private BattleEnd result;
  private const float LINK_WIDTH = 120f;
  private const float LINK_DEFWIDTH = 136f;
  private const float scale = 0.882352948f;
  private List<GameObject> mResultUnitPanels = new List<GameObject>();
  private static readonly Vector3 cell6Scale = new Vector3(0.85f, 0.85f, 1f);
  private const int cell6Width = 104;
  private const int cell6Height = 274;
  private const int cell6 = 6;
  private static readonly Vector3 cell5Scale = new Vector3(1f, 1f, 1f);
  private const int cell5Width = 122;
  private const int cell5Height = 316;
  private const int cell5 = 5;

  public override void OnDestroy()
  {
    this.mResultUnitPanels.Clear();
    base.OnDestroy();
  }

  private void Awake() => this.mResultUnitPanels.Clear();

  public override IEnumerator Init(BattleInfo info, BattleEnd result)
  {
    BattleUI55ResultMenu battleUi55ResultMenu = this;
    battleUi55ResultMenu.CharacterIntimateUpPrefabName = "Prefabs/battle/popup_070_22_1__anim_popup01";
    Future<GameObject> prefabF = Res.Prefabs.battleUI_55.dir_Unit_zero.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battleUi55ResultMenu.DirUnitPrefab = prefabF.Result;
    battleUi55ResultMenu.info = info;
    battleUi55ResultMenu.result = result;
    if (info != null)
      battleUi55ResultMenu.SetQuestName(MasterData.EarthQuestEpisode[info.quest_s_id].episode_name);
    battleUi55ResultMenu.SetZenie(result.player_incr_money);
    battleUi55ResultMenu.Scene_Result.SetActive(false);
  }

  public void SetZenie(long zenie) => this.TxtGetZenie28.SetTextLocalize(zenie);

  public void SetQuestName(string txt) => this.TxtSubTitle24.SetTextLocalize(txt);

  public override IEnumerator Run()
  {
    BattleUI55ResultMenu battleUi55ResultMenu = this;
    battleUi55ResultMenu.Scene_Result.SetActive(true);
    battleUi55ResultMenu.Title.SetActive(true);
    BattleUI55ResultMenu.Runner[] runnerArray = new BattleUI55ResultMenu.Runner[3]
    {
      new BattleUI55ResultMenu.Runner(battleUi55ResultMenu.InitObjects),
      new BattleUI55ResultMenu.Runner(battleUi55ResultMenu.ShowUnitEXPForEarth),
      new BattleUI55ResultMenu.Runner(((ResultMenuBase) battleUi55ResultMenu).CharacterIntimatesPopup)
    };
    for (int index = 0; index < runnerArray.Length; ++index)
    {
      IEnumerator e = runnerArray[index]();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    runnerArray = (BattleUI55ResultMenu.Runner[]) null;
  }

  public override IEnumerator OnFinish()
  {
    this.Scene_Result.SetActive(false);
    this.Title.SetActive(false);
    yield break;
  }

  private IEnumerator InitObjects()
  {
    BattleUI55ResultMenu battleUi55ResultMenu = this;
    // ISSUE: reference to a compiler-generated method
    int? nullable = ((IEnumerable<PlayerUnit>) battleUi55ResultMenu.result.before_player_units).FirstIndexOrNull<PlayerUnit>(new Func<PlayerUnit, bool>(battleUi55ResultMenu.\u003CInitObjects\u003Eb__19_0));
    if (nullable.HasValue)
      battleUi55ResultMenu.mvp_index = nullable.Value;
    battleUi55ResultMenu.questType = battleUi55ResultMenu.info.quest_type;
    battleUi55ResultMenu.questSID = battleUi55ResultMenu.info.quest_s_id;
    battleUi55ResultMenu.beforeUnits = ((IEnumerable<PlayerUnit>) battleUi55ResultMenu.result.before_player_units).ToDictionary<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.id));
    battleUi55ResultMenu.afterUnits = ((IEnumerable<PlayerUnit>) battleUi55ResultMenu.result.after_player_units).ToDictionary<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.id));
    battleUi55ResultMenu.beforeGears = ((IEnumerable<PlayerItem>) battleUi55ResultMenu.result.before_player_gears).ToDictionary<PlayerItem, int>((Func<PlayerItem, int>) (x => x.id));
    battleUi55ResultMenu.afterGears = ((IEnumerable<PlayerItem>) battleUi55ResultMenu.result.after_player_gears).ToDictionary<PlayerItem, int>((Func<PlayerItem, int>) (x => x.id));
    battleUi55ResultMenu.characterIntimates.AddRange((IEnumerable<BattleEndPlayer_character_intimates_in_battle>) battleUi55ResultMenu.result.player_character_intimates_in_battle);
    yield return (object) new WaitForSeconds(0.1f);
  }

  protected IEnumerator ShowUnitEXPForEarth()
  {
    BattleUI55ResultMenu battleUi55ResultMenu = this;
    List<ColosseumResultUnit> unitList = new List<ColosseumResultUnit>();
    List<GaugeRunner> unitLevelUpGauge = new List<GaugeRunner>();
    List<GaugeRunner> gearLevelUpGauge = new List<GaugeRunner>();
    battleUi55ResultMenu.grid.arrangement = (UIGrid.Arrangement) 0;
    if (battleUi55ResultMenu.afterUnits.Count > 10)
    {
      battleUi55ResultMenu.grid.cellHeight = 274f;
      battleUi55ResultMenu.grid.cellWidth = 104f;
      battleUi55ResultMenu.grid.maxPerLine = 6;
    }
    else
    {
      battleUi55ResultMenu.grid.cellHeight = 316f;
      battleUi55ResultMenu.grid.cellWidth = 122f;
      battleUi55ResultMenu.grid.maxPerLine = 5;
    }
    int idx = 0;
    ColosseumResultUnit unit;
    IEnumerator e;
    foreach (KeyValuePair<int, PlayerUnit> afterUnit1 in battleUi55ResultMenu.afterUnits)
    {
      PlayerUnit afterUnit = afterUnit1.Value;
      PlayerUnit beforeUnit = battleUi55ResultMenu.beforeUnits.First<KeyValuePair<int, PlayerUnit>>((Func<KeyValuePair<int, PlayerUnit>, bool>) (x => x.Value.id == afterUnit.id)).Value;
      unit = battleUi55ResultMenu.DirUnitPrefab.Clone(((Component) battleUi55ResultMenu.grid).transform).GetComponent<ColosseumResultUnit>();
      if (Singleton<EarthDataManager>.GetInstance().characterDict[afterUnit.id].isFall)
        unit.SetDeath();
      unitList.Add(unit);
      e = unit.Init(beforeUnit, afterUnit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unitLevelUpGauge.Add(unit.GetUnitExpGaugeRunner((Func<GameObject, int, IEnumerator>) null));
      gearLevelUpGauge.AddRange((IEnumerable<GaugeRunner>) unit.GetGearExpGaugeRunner(battleUi55ResultMenu.beforeGears, battleUi55ResultMenu.afterGears, (Func<GameObject, int, IEnumerator>) null));
      ++idx;
      unit = (ColosseumResultUnit) null;
    }
    battleUi55ResultMenu.grid.Reposition();
    yield return (object) null;
    battleUi55ResultMenu.DirUnitExp.SetActive(true);
    yield return (object) new WaitForSeconds(1.5f);
    if (battleUi55ResultMenu.mvp_index != -1)
    {
      unitList[battleUi55ResultMenu.mvp_index].SetMVP(true);
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1017", delay: 0.05f);
      yield return (object) new WaitForSeconds(0.4f);
    }
    if (Object.op_Inequality((Object) unitList.FirstOrDefault<ColosseumResultUnit>((Func<ColosseumResultUnit, bool>) (x => x.GetUnitExp != 0)), (Object) null))
    {
      foreach (ColosseumResultUnit colosseumResultUnit in unitList)
        colosseumResultUnit.ShowUnitExp();
      e = GaugeRunner.Run(unitLevelUpGauge.ToArray());
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      yield return (object) new WaitForSeconds(1.1f);
    }
    battleUi55ResultMenu.makeDiffGearAccessoryRemainingAmounts(battleUi55ResultMenu.beforeGears.Select<KeyValuePair<int, PlayerItem>, PlayerItem>((Func<KeyValuePair<int, PlayerItem>, PlayerItem>) (kv => kv.Value)).ToArray<PlayerItem>());
    GameObject UnitLevelupPrefab = (GameObject) null;
    GameObject popup1;
    Future<GameObject> UnitLevelUpPrefabF;
    foreach (ColosseumResultUnit colosseumResultUnit in unitList)
    {
      unit = colosseumResultUnit;
      if (unit.IsLevelUP())
      {
        if (Object.op_Equality((Object) UnitLevelupPrefab, (Object) null))
        {
          UnitLevelUpPrefabF = Res.Prefabs.battle.UnitLevelUpPrefab.Load<GameObject>();
          e = UnitLevelUpPrefabF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          UnitLevelupPrefab = UnitLevelUpPrefabF.Result;
          UnitLevelUpPrefabF = (Future<GameObject>) null;
        }
        popup1 = battleUi55ResultMenu.OpenPopup(UnitLevelupPrefab);
        popup1.SetActive(false);
        Battle020191Menu o = popup1.GetComponent<Battle020191Menu>();
        e = o.Init(unit.BeforeUnit, unit.AfterUnit, battleUi55ResultMenu.beforeGears, battleUi55ResultMenu.diffGearAccessoryRemainingAmounts, true);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        popup1.SetActive(true);
        bool isFinished = false;
        o.SetCallback((Action) (() => isFinished = true));
        while (!isFinished)
          yield return (object) null;
        yield return (object) new WaitForSeconds(0.6f);
        popup1 = (GameObject) null;
        o = (Battle020191Menu) null;
      }
      if (battleUi55ResultMenu.disappearedPlayerGears != null && battleUi55ResultMenu.disappearedPlayerGears.Count > 0)
      {
        PlayerItem beforeGear = (PlayerItem) null;
        PlayerUnit beforeUnit = unit.BeforeUnit;
        PlayerItem[] equippedGears = new PlayerItem[2]
        {
          beforeUnit.equippedGear,
          beforeUnit.equippedGear2
        };
        for (int i = 0; i < equippedGears.Length; ++i)
        {
          if (equippedGears[i] != (PlayerItem) null)
            beforeGear = battleUi55ResultMenu.beforeGears[equippedGears[i].id];
          if (beforeGear != (PlayerItem) null)
          {
            PlayerItem playerItem = battleUi55ResultMenu.disappearedPlayerGears.FirstOrDefault<PlayerItem>((Func<PlayerItem, bool>) (x => x.id == beforeGear.id));
            GearDisappearanceType[] disappearanceTypeList = MasterData.GearDisappearanceTypeList;
            bool isdisappearedPopupFinished = false;
            if (playerItem != (PlayerItem) null)
            {
              int disappearanceType = playerItem.gear.disappearance_type_GearDisappearanceType;
              GearDisappearanceType disappearanceType1 = ((IEnumerable<GearDisappearanceType>) disappearanceTypeList).FirstOrDefault<GearDisappearanceType>((Func<GearDisappearanceType, bool>) (x => x.ID == disappearanceType));
              string accessoryDisappearTitle = Consts.GetInstance().RESULT_MENU_BASE_ACCESSORY_DISAPPEAR_TITLE;
              battleUi55ResultMenu.StartCoroutine(PopupCommon.Show(accessoryDisappearTitle, disappearanceType1.discription, (Action) (() => isdisappearedPopupFinished = true)));
              while (!isdisappearedPopupFinished)
                yield return (object) null;
            }
          }
        }
        equippedGears = (PlayerItem[]) null;
      }
      unit = (ColosseumResultUnit) null;
    }
    if (Object.op_Implicit((Object) unitList.FirstOrDefault<ColosseumResultUnit>((Func<ColosseumResultUnit, bool>) (x => x.IsProficiencyLevelUp()))))
    {
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1027", delay: 0.1f);
      foreach (ColosseumResultUnit colosseumResultUnit in unitList)
        colosseumResultUnit.OpenAfterProficiency();
      yield return (object) new WaitForSeconds(0.7f);
    }
    bool isGearLevelUp = false;
    foreach (GaugeRunner runner in gearLevelUpGauge)
    {
      if (runner != null)
      {
        isGearLevelUp = true;
        e = GaugeRunner.Run(runner);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    if (isGearLevelUp)
    {
      foreach (ColosseumResultUnit colosseumResultUnit in unitList)
        colosseumResultUnit.ShowWeaponRankUp();
      yield return (object) new WaitForSeconds(1f);
    }
    foreach (ColosseumResultUnit colosseumResultUnit in unitList)
    {
      unit = colosseumResultUnit;
      popup1 = (GameObject) null;
      if (unit.ShowGearUpgradeSkill())
      {
        if (Object.op_Equality((Object) popup1, (Object) null))
        {
          UnitLevelUpPrefabF = Res.Prefabs.battle.BuguSkillGetPrefab.Load<GameObject>();
          e = UnitLevelUpPrefabF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          popup1 = UnitLevelUpPrefabF.Result;
          UnitLevelUpPrefabF = (Future<GameObject>) null;
        }
        foreach (Tuple<bool, PlayerItem, GearGearSkill> gearUpgradeSkill in unit.GearUpgradeSkills)
        {
          GameObject popup2 = battleUi55ResultMenu.OpenPopup(popup1);
          popup2.SetActive(false);
          BattleResultBuguSkillGet o = popup2.GetComponent<BattleResultBuguSkillGet>();
          e = o.Init(gearUpgradeSkill.Item1, new GameCore.ItemInfo(gearUpgradeSkill.Item2), gearUpgradeSkill.Item3);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          popup2.SetActive(true);
          o.doStart();
          bool isFinished = false;
          o.SetCallback((Action) (() => isFinished = true));
          while (!isFinished)
            yield return (object) null;
          yield return (object) new WaitForSeconds(0.6f);
          popup2 = (GameObject) null;
          o = (BattleResultBuguSkillGet) null;
        }
      }
      popup1 = (GameObject) null;
      unit = (ColosseumResultUnit) null;
    }
    unitList.Clear();
    unitLevelUpGauge.Clear();
    gearLevelUpGauge.Clear();
  }

  private delegate IEnumerator Runner();
}
