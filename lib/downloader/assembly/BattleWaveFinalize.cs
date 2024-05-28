// Decompiled with JetBrains decompiler
// Type: BattleWaveFinalize
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public class BattleWaveFinalize : BattleMonoBehaviour
{
  protected override IEnumerator Start_Battle()
  {
    BattleWaveFinalize battleWaveFinalize = this;
    BattleCameraFilter.DesotryBattleWin();
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(4);
    int questType = (int) battleWaveFinalize.env.core.battleInfo.quest_type;
    int win = battleWaveFinalize.env.core.isWin ? 1 : 0;
    bool flag = battleWaveFinalize.env.core.allDeadUnitsp(BL.ForceID.player);
    string battleId = battleWaveFinalize.env.core.battleInfo.battleId;
    int absoluteTurnCount = battleWaveFinalize.env.core.phaseState.absoluteTurnCount;
    int continueCount = battleWaveFinalize.env.core.continueCount;
    List<int> intList1 = new List<int>();
    List<int> intList2 = new List<int>();
    List<int> intList3 = new List<int>();
    List<int> intList4 = new List<int>();
    int num1 = 0;
    int num2 = 0;
    foreach (BL.Unit unit in battleWaveFinalize.env.core.playerUnits.value)
    {
      if (unit.duelHistory != null)
      {
        foreach (BL.DuelHistory duelHistory in unit.duelHistory)
        {
          intList1.Add(duelHistory.inflictTotalDamage);
          intList2.Add(duelHistory.sufferTotalDamage);
          intList3.Add(duelHistory.criticalCount);
          intList4.Add(duelHistory.inflictMaxDamage);
          num1 += duelHistory.weekElementAttackCount;
          num2 += duelHistory.weekKindAttackCount;
        }
      }
    }
    (List<WebAPI.Request.BattleFinish.UnitResult> units, List<WebAPI.Request.BattleFinish.SkillResult> use_skills, int[] use_skill_group) unitResults = BattleFinalize.CreateUnitResults(battleWaveFinalize.env.core, battleWaveFinalize.env.core.currentWave > 0 ? battleWaveFinalize.env.waveEnemiesStack : (Stack<List<BL.Unit>>) null);
    List<WebAPI.Request.BattleFinish.GearResult> gearResults = BattleFinalize.CreateGearResults(battleWaveFinalize.env.core);
    List<WebAPI.Request.BattleFinish.SupplyResult> source1 = new List<WebAPI.Request.BattleFinish.SupplyResult>();
    foreach (BL.Item obj in battleWaveFinalize.env.core.itemList.value)
    {
      int num3 = obj.initialAmount - obj.amount;
      if (num3 > 0)
        source1.Add(new WebAPI.Request.BattleFinish.SupplyResult()
        {
          supply_id = obj.itemId,
          use_quantity = num3
        });
    }
    List<WebAPI.Request.BattleFinish.IntimateResult> source2 = new List<WebAPI.Request.BattleFinish.IntimateResult>();
    foreach (Tuple<int, int, int> tuple in battleWaveFinalize.env.core.getPlayerIntimateResult())
      source2.Add(new WebAPI.Request.BattleFinish.IntimateResult()
      {
        character_id = tuple.Item1,
        target_character_id = tuple.Item2,
        exp = tuple.Item3
      });
    List<BattleWaveFinishInfo> battleWaveFinishInfoList = new List<BattleWaveFinishInfo>();
    List<Tuple<BL.DropData, int>> dropDatas = new List<Tuple<BL.DropData, int>>();
    for (int row = 0; row < battleWaveFinalize.env.core.battleInfo.stage.map_height; ++row)
    {
      for (int column = 0; column < battleWaveFinalize.env.core.battleInfo.stage.map_width; ++column)
      {
        BL.Panel fieldPanel = battleWaveFinalize.env.core.getFieldPanel(row, column);
        if (fieldPanel.fieldEvent != null && fieldPanel.fieldEvent.isCompleted)
          dropDatas.Add(new Tuple<BL.DropData, int>(fieldPanel.fieldEvent, fieldPanel.fieldEventId));
      }
    }
    battleWaveFinishInfoList.Add(battleWaveFinalize.CreateBattleFinishInfo(battleWaveFinalize.env.core.stage.id, battleWaveFinalize.env.core.enemyUnits.value, dropDatas));
    if (battleWaveFinalize.env.core.currentWave > 0)
    {
      for (--battleWaveFinalize.env.core.currentWave; battleWaveFinalize.env.core.currentWave >= 0; --battleWaveFinalize.env.core.currentWave)
        battleWaveFinishInfoList.Add(battleWaveFinalize.CreateBattleFinishInfo(battleWaveFinalize.env.core.stage.id, battleWaveFinalize.env.waveEnemiesStack.Pop(), battleWaveFinalize.env.waveDropStack.Pop()));
    }
    Future<WebAPI.Response.BattleWaveFinish> f = WebAPI.BattleWaveFinish((WebAPI.Request.IBattleWaveFinish) new BattleWaveFinalize.ImpBattleWaveFinish()
    {
      battle_turn = absoluteTurnCount,
      battle_uuid = battleId,
      continue_count = continueCount,
      duels_critical_count = intList3.ToArray(),
      duels_damage = intList1.ToArray(),
      duels_hit_damage = intList2.ToArray(),
      duels_max_damage = intList4.ToArray(),
      gear_results_damage_count = gearResults.Select<WebAPI.Request.BattleFinish.GearResult, int>((Func<WebAPI.Request.BattleFinish.GearResult, int>) (x => x.damage_count)).ToArray<int>(),
      gear_results_kill_count = gearResults.Select<WebAPI.Request.BattleFinish.GearResult, int>((Func<WebAPI.Request.BattleFinish.GearResult, int>) (x => x.kill_count)).ToArray<int>(),
      gear_results_player_gear_id = gearResults.Select<WebAPI.Request.BattleFinish.GearResult, int>((Func<WebAPI.Request.BattleFinish.GearResult, int>) (x => x.player_gear_id)).ToArray<int>(),
      intimate_result_target_player_character_id = source2.Select<WebAPI.Request.BattleFinish.IntimateResult, int>((Func<WebAPI.Request.BattleFinish.IntimateResult, int>) (x => x.target_character_id)).ToArray<int>(),
      intimate_results_exp = source2.Select<WebAPI.Request.BattleFinish.IntimateResult, int>((Func<WebAPI.Request.BattleFinish.IntimateResult, int>) (x => x.exp)).ToArray<int>(),
      intimate_results_player_character_id = source2.Select<WebAPI.Request.BattleFinish.IntimateResult, int>((Func<WebAPI.Request.BattleFinish.IntimateResult, int>) (x => x.character_id)).ToArray<int>(),
      is_game_over = flag,
      supply_results_supply_id = source1.Select<WebAPI.Request.BattleFinish.SupplyResult, int>((Func<WebAPI.Request.BattleFinish.SupplyResult, int>) (x => x.supply_id)).ToArray<int>(),
      supply_results_use_quantity = source1.Select<WebAPI.Request.BattleFinish.SupplyResult, int>((Func<WebAPI.Request.BattleFinish.SupplyResult, int>) (x => x.use_quantity)).ToArray<int>(),
      unit_results_max_hp = unitResults.units.Select<WebAPI.Request.BattleFinish.UnitResult, int>((Func<WebAPI.Request.BattleFinish.UnitResult, int>) (x => x.max_hp)).ToArray<int>(),
      unit_results_player_unit_id = unitResults.units.Select<WebAPI.Request.BattleFinish.UnitResult, int>((Func<WebAPI.Request.BattleFinish.UnitResult, int>) (x => x.player_unit_id)).ToArray<int>(),
      unit_results_received_damage = unitResults.units.Select<WebAPI.Request.BattleFinish.UnitResult, int>((Func<WebAPI.Request.BattleFinish.UnitResult, int>) (x => x.received_damage)).ToArray<int>(),
      unit_results_remaining_hp = unitResults.units.Select<WebAPI.Request.BattleFinish.UnitResult, int>((Func<WebAPI.Request.BattleFinish.UnitResult, int>) (x => x.remaining_hp)).ToArray<int>(),
      unit_results_rental = unitResults.units.Select<WebAPI.Request.BattleFinish.UnitResult, int>((Func<WebAPI.Request.BattleFinish.UnitResult, int>) (x => x.rental)).ToArray<int>(),
      unit_results_total_damage = unitResults.units.Select<WebAPI.Request.BattleFinish.UnitResult, int>((Func<WebAPI.Request.BattleFinish.UnitResult, int>) (x => x.total_damage)).ToArray<int>(),
      unit_results_total_damage_count = unitResults.units.Select<WebAPI.Request.BattleFinish.UnitResult, int>((Func<WebAPI.Request.BattleFinish.UnitResult, int>) (x => x.total_damage_count)).ToArray<int>(),
      unit_results_total_kill_count = unitResults.units.Select<WebAPI.Request.BattleFinish.UnitResult, int>((Func<WebAPI.Request.BattleFinish.UnitResult, int>) (x => x.total_kill_count)).ToArray<int>(),
      use_skill_group = ((IEnumerable<int>) unitResults.use_skill_group).ToArray<int>(),
      use_skill_ids = unitResults.use_skills.Select<WebAPI.Request.BattleFinish.SkillResult, int>((Func<WebAPI.Request.BattleFinish.SkillResult, int>) (x => x.ID)).ToArray<int>(),
      use_skill_id_counts = unitResults.use_skills.Select<WebAPI.Request.BattleFinish.SkillResult, int>((Func<WebAPI.Request.BattleFinish.SkillResult, int>) (x => x.count)).ToArray<int>(),
      info = battleWaveFinishInfoList.ToArray(),
      weak_element_attack_count = num1,
      weak_kind_attack_count = num2,
      win = win,
      guests = unitResults.units.Select<WebAPI.Request.BattleFinish.UnitResult, int>((Func<WebAPI.Request.BattleFinish.UnitResult, int>) (x => x.guest)).ToArray<int>()
    }, (Action<WebAPI.Response.UserError>) (error =>
    {
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      WebAPI.DefaultUserErrorCallback(error);
    }));
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (f.Result != null)
    {
      if (win == 0)
      {
        e = WebAPI.QuestProgressExtra((Action<WebAPI.Response.UserError>) (error =>
        {
          Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
          WebAPI.DefaultUserErrorCallback(error);
        })).Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        WebAPI.SetLatestResponsedAt("QuestProgressExtra");
        Singleton<NGGameDataManager>.GetInstance().IsConnectedResultQuestProgressExtra = true;
      }
      Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
      BattleUI05Scene.ChangeScene(battleWaveFinalize.env.core.battleInfo, battleWaveFinalize.env.core.isWin, f.Result);
    }
  }

  private BattleWaveFinishInfo CreateBattleFinishInfo(
    int stage_id,
    List<BL.Unit> enemys,
    List<Tuple<BL.DropData, int>> dropDatas)
  {
    BattleWaveFinishInfo battleFinishInfo = new BattleWaveFinishInfo();
    battleFinishInfo.stage_id = stage_id;
    List<int> intList1 = new List<int>();
    List<int> intList2 = new List<int>();
    List<int> intList3 = new List<int>();
    List<int> intList4 = new List<int>();
    List<int> intList5 = new List<int>();
    List<int> intList6 = new List<int>();
    foreach (BL.Unit enemy in enemys)
    {
      if (enemy.hasDrop && enemy.drop.isCompleted)
        intList1.Add(enemy.playerUnit.id);
      intList2.Add(enemy.playerUnit.id);
      intList3.Add(enemy.hp <= 0 ? 1 : 0);
      intList4.Add(enemy.killCount);
      if (enemy.killedBy != (BL.Unit) null)
      {
        intList5.Add(enemy.playerUnit.level - enemy.killedBy.playerUnit.level);
        intList6.Add(enemy.overkillDamage);
      }
      else
      {
        intList5.Add(0);
        intList6.Add(0);
      }
    }
    battleFinishInfo.drop_entity_ids = intList1.ToArray();
    battleFinishInfo.enemy_results_enemy_id = intList2.ToArray();
    battleFinishInfo.enemy_results_dead_count = intList3.ToArray();
    battleFinishInfo.enemy_results_kill_count = intList4.ToArray();
    battleFinishInfo.enemy_results_level_difference = intList5.ToArray();
    battleFinishInfo.enemy_results_overkill_damage = intList6.ToArray();
    List<int> intList7 = new List<int>();
    foreach (Tuple<BL.DropData, int> dropData in dropDatas)
      intList7.Add(dropData.Item2);
    battleFinishInfo.panel_entity_ids = intList7.ToArray();
    return battleFinishInfo;
  }

  private class ImpBattleWaveFinish : WebAPI.Request.IBattleWaveFinish
  {
    public int battle_turn { get; set; }

    public string battle_uuid { get; set; }

    public int continue_count { get; set; }

    public int[] duels_critical_count { get; set; }

    public int[] duels_damage { get; set; }

    public int[] duels_hit_damage { get; set; }

    public int[] duels_max_damage { get; set; }

    public int[] gear_results_damage_count { get; set; }

    public int[] gear_results_kill_count { get; set; }

    public int[] gear_results_player_gear_id { get; set; }

    public int[] intimate_result_target_player_character_id { get; set; }

    public int[] intimate_results_exp { get; set; }

    public int[] intimate_results_player_character_id { get; set; }

    public bool is_game_over { get; set; }

    public int[] supply_results_supply_id { get; set; }

    public int[] supply_results_use_quantity { get; set; }

    public int[] unit_results_max_hp { get; set; }

    public int[] unit_results_player_unit_id { get; set; }

    public int[] unit_results_received_damage { get; set; }

    public int[] unit_results_remaining_hp { get; set; }

    public int[] unit_results_rental { get; set; }

    public int[] unit_results_total_damage { get; set; }

    public int[] unit_results_total_damage_count { get; set; }

    public int[] unit_results_total_kill_count { get; set; }

    public int[] use_skill_group { get; set; }

    public int[] use_skill_ids { get; set; }

    public int[] use_skill_id_counts { get; set; }

    public BattleWaveFinishInfo[] info { get; set; }

    public int weak_element_attack_count { get; set; }

    public int weak_kind_attack_count { get; set; }

    public int win { get; set; }

    public int[] guests { get; set; }
  }
}
