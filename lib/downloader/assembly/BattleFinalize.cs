// Decompiled with JetBrains decompiler
// Type: BattleFinalize
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
public class BattleFinalize : BattleMonoBehaviour
{
  private bool isDoApiStart;
  private bool isDoApiEnd;
  private Future<BattleEnd> apiResult;

  public IEnumerator DoAPI()
  {
    BattleFinalize battleFinalize = this;
    battleFinalize.isDoApiStart = true;
    WebAPI.Request.BattleFinish request = new WebAPI.Request.BattleFinish();
    request.quest_type = battleFinalize.env.core.battleInfo.quest_type;
    request.win = battleFinalize.env.core.isWin;
    request.is_game_over = battleFinalize.env.core.allDeadUnitsp(BL.ForceID.player);
    request.battle_uuid = battleFinalize.env.core.battleInfo.battleId;
    request.player_money = 0;
    request.battle_turn = battleFinalize.env.core.phaseState.turnCount;
    request.continue_count = battleFinalize.env.core.continueCount;
    foreach (BL.Unit unit in battleFinalize.env.core.playerUnits.value)
    {
      if (unit.duelHistory != null)
      {
        foreach (BL.DuelHistory duelHistory in unit.duelHistory)
        {
          request.duels_damage.Add(duelHistory.inflictTotalDamage);
          request.duels_hit_damage.Add(duelHistory.sufferTotalDamage);
          request.duels_critical_count.Add(duelHistory.criticalCount);
          request.duels_max_damage.Add(duelHistory.inflictMaxDamage);
          request.week_element_attack_count += unit.playerUnit.is_guest ? 0 : duelHistory.weekElementAttackCount;
          request.week_kind_attack_count += unit.playerUnit.is_guest ? 0 : duelHistory.weekKindAttackCount;
        }
      }
    }
    for (int row = 0; row < battleFinalize.env.core.battleInfo.stage.map_height; ++row)
    {
      for (int column = 0; column < battleFinalize.env.core.battleInfo.stage.map_width; ++column)
      {
        BL.Panel fieldPanel = battleFinalize.env.core.getFieldPanel(row, column);
        if (fieldPanel.fieldEvent != null && fieldPanel.fieldEvent.isCompleted)
        {
          request.panel_reward.Add(fieldPanel.fieldEvent.reward);
          request.panel_entity_ids.Add(fieldPanel.fieldEventId);
        }
      }
    }
    foreach (BL.Unit unit in battleFinalize.env.core.enemyUnits.value)
    {
      if (unit.hasDrop && unit.drop.isCompleted)
      {
        request.panel_reward.Add(unit.drop.reward);
        request.drop_entity_ids.Add(unit.playerUnit.id);
      }
    }
    request.use_skill_group = new int[4]
    {
      0,
      0,
      battleFinalize.env.core.playerCallSkillState.countSkillUses,
      0
    };
    Dictionary<int, int> source = new Dictionary<int, int>();
    foreach (BL.Unit unit1 in battleFinalize.env.core.playerUnits.value)
    {
      WebAPI.Request.BattleFinish.UnitResult unitResult = new WebAPI.Request.BattleFinish.UnitResult();
      unitResult.player_unit_id = unit1.playerUnit.id;
      unitResult.total_damage = unit1.attackDamage + unit1.attackSubDamage;
      if (unit1.is_leader)
      {
        foreach (BL.Unit unit2 in battleFinalize.env.core.enemyUnits.value)
          unitResult.total_damage += unit2.receivedLandformDamage;
      }
      unitResult.total_damage_count = unit1.attackCount;
      unitResult.total_kill_count = unit1.killCount;
      unitResult.remaining_hp = unit1.hp;
      unitResult.rental = unit1.is_helper ? 1 : (unit1.index != 5 || !unit1.playerUnit.is_guest ? 0 : 1);
      unitResult.received_damage = unit1.receivedDamage + unit1.receivedSubDamage + unit1.receivedLandformDamage;
      unitResult.guest = unit1.playerUnit.is_guest ? 1 : 0;
      unitResult.max_hp = unit1.hpMax;
      request.units.Add(unitResult);
      List<int> skillUsesIds = unit1.skillUsesIds;
      // ISSUE: explicit non-virtual call
      int count = skillUsesIds != null ? __nonvirtual (skillUsesIds.Count) : 0;
      for (int index = 0; index < count; ++index)
      {
        int skillUsesId = unit1.skillUsesIds[index];
        if (source.ContainsKey(skillUsesId))
          source[skillUsesId] += unit1.skillUsesCounts[index];
        else
          source.Add(skillUsesId, unit1.skillUsesCounts[index]);
      }
      int[] useSkillGroup = unit1.use_skill_group;
      request.use_skill_group[0] += useSkillGroup[0];
      request.use_skill_group[1] += useSkillGroup[1];
      request.use_skill_group[3] += useSkillGroup[3];
    }
    request.use_skills = source.Select<KeyValuePair<int, int>, WebAPI.Request.BattleFinish.SkillResult>((Func<KeyValuePair<int, int>, WebAPI.Request.BattleFinish.SkillResult>) (x => new WebAPI.Request.BattleFinish.SkillResult()
    {
      ID = x.Key,
      count = x.Value
    })).ToList<WebAPI.Request.BattleFinish.SkillResult>();
    foreach (BL.Unit unit in battleFinalize.env.core.enemyUnits.value)
    {
      WebAPI.Request.BattleFinish.EnemyResult enemyResult = new WebAPI.Request.BattleFinish.EnemyResult();
      enemyResult.enemy_id = unit.playerUnit.id;
      enemyResult.dead_count = unit.hp <= 0 ? 1 : 0;
      enemyResult.kill_count = unit.killCount;
      if (unit.killedBy != (BL.Unit) null)
      {
        enemyResult.level_difference = unit.playerUnit.level - unit.killedBy.playerUnit.level;
        enemyResult.kill_by_playerunit_id = unit.killedBy.playerUnit.id;
        enemyResult.overkill_damage = unit.overkillDamage;
      }
      else
      {
        enemyResult.level_difference = 0;
        enemyResult.overkill_damage = 0;
        enemyResult.kill_by_playerunit_id = 0;
      }
      request.enemies.Add(enemyResult);
    }
    foreach (BL.Unit unit in battleFinalize.env.core.playerUnits.value)
    {
      if (!unit.is_helper && !unit.playerUnit.is_guest)
      {
        if (unit.playerUnit.equippedGear != (PlayerItem) null)
          request.gears.Add(new WebAPI.Request.BattleFinish.GearResult()
          {
            player_gear_id = unit.playerUnit.equippedGear.id,
            kill_count = unit.killCount,
            damage_count = unit.attackCount
          });
        if (unit.playerUnit.equippedGear2 != (PlayerItem) null)
          request.gears.Add(new WebAPI.Request.BattleFinish.GearResult()
          {
            player_gear_id = unit.playerUnit.equippedGear2.id,
            kill_count = unit.killCount,
            damage_count = unit.attackCount
          });
      }
    }
    foreach (BL.Item obj in battleFinalize.env.core.itemList.value)
    {
      int num = obj.initialAmount - obj.amount;
      if (num > 0)
        request.supplies.Add(new WebAPI.Request.BattleFinish.SupplyResult()
        {
          supply_id = obj.itemId,
          use_quantity = num
        });
    }
    foreach (Tuple<int, int, int> tuple in battleFinalize.env.core.getPlayerIntimateResult())
      request.intimates.Add(new WebAPI.Request.BattleFinish.IntimateResult()
      {
        character_id = tuple.Item1,
        target_character_id = tuple.Item2,
        exp = tuple.Item3
      });
    battleFinalize.apiResult = WebAPI.BattleFinish(request, battleFinalize.env, new Action<WebAPI.Response.UserError>(battleFinalize.errorCallback));
    IEnumerator e = battleFinalize.apiResult.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (battleFinalize.apiResult.Result != null)
    {
      if (battleFinalize.env.core.battleInfo.quest_type == CommonQuestType.Extra)
      {
        e = WebAPI.QuestProgressExtra((Action<WebAPI.Response.UserError>) (error => WebAPI.DefaultUserErrorCallback(error))).Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        WebAPI.SetLatestResponsedAt("QuestProgressExtra");
        Singleton<NGGameDataManager>.GetInstance().IsConnectedResultQuestProgressExtra = true;
      }
      battleFinalize.isDoApiEnd = true;
    }
  }

  public static (List<WebAPI.Request.BattleFinish.UnitResult> units, List<WebAPI.Request.BattleFinish.SkillResult> use_skills, int[] use_skill_group) CreateUnitResults(
    BL bl,
    Stack<List<BL.Unit>> waveEnemies = null)
  {
    List<WebAPI.Request.BattleFinish.UnitResult> unitResultList = new List<WebAPI.Request.BattleFinish.UnitResult>(bl.playerUnits.value.Count);
    int[] numArray = new int[4]
    {
      0,
      0,
      bl.playerCallSkillState.countSkillUses,
      0
    };
    Dictionary<int, int> source = new Dictionary<int, int>();
    foreach (BL.Unit unit in bl.playerUnits.value)
    {
      WebAPI.Request.BattleFinish.UnitResult unitResult = new WebAPI.Request.BattleFinish.UnitResult()
      {
        player_unit_id = unit.playerUnit.id,
        total_damage = unit.attackDamage + unit.attackSubDamage,
        total_damage_count = unit.attackCount,
        total_kill_count = unit.killCount,
        remaining_hp = unit.hp,
        rental = unit.is_helper ? 1 : (unit.index != 5 || !unit.playerUnit.is_guest ? 0 : 1),
        received_damage = unit.receivedDamage + unit.receivedSubDamage + unit.receivedLandformDamage,
        guest = unit.playerUnit.is_guest ? 1 : 0,
        max_hp = unit.hpMax
      };
      if (unit.is_leader)
      {
        unitResult.total_damage += bl.enemyUnits.value.Sum<BL.Unit>((Func<BL.Unit, int>) (x => x.receivedLandformDamage));
        if (waveEnemies != null)
        {
          foreach (List<BL.Unit> waveEnemy in waveEnemies)
            unitResult.total_damage += waveEnemy.Sum<BL.Unit>((Func<BL.Unit, int>) (x => x.receivedLandformDamage));
        }
      }
      unitResultList.Add(unitResult);
      List<int> skillUsesIds = unit.skillUsesIds;
      // ISSUE: explicit non-virtual call
      int count = skillUsesIds != null ? __nonvirtual (skillUsesIds.Count) : 0;
      for (int index = 0; index < count; ++index)
      {
        int skillUsesId = unit.skillUsesIds[index];
        if (source.ContainsKey(skillUsesId))
          source[skillUsesId] += unit.skillUsesCounts[index];
        else
          source.Add(skillUsesId, unit.skillUsesCounts[index]);
      }
      int[] useSkillGroup = unit.use_skill_group;
      numArray[0] += useSkillGroup[0];
      numArray[1] += useSkillGroup[1];
      numArray[3] += useSkillGroup[3];
    }
    return (unitResultList, source.Select<KeyValuePair<int, int>, WebAPI.Request.BattleFinish.SkillResult>((Func<KeyValuePair<int, int>, WebAPI.Request.BattleFinish.SkillResult>) (x => new WebAPI.Request.BattleFinish.SkillResult()
    {
      ID = x.Key,
      count = x.Value
    })).ToList<WebAPI.Request.BattleFinish.SkillResult>(), numArray);
  }

  public static List<WebAPI.Request.BattleFinish.GearResult> CreateGearResults(BL bl)
  {
    List<WebAPI.Request.BattleFinish.GearResult> gearResults = new List<WebAPI.Request.BattleFinish.GearResult>();
    foreach (BL.Unit unit in bl.playerUnits.value)
    {
      if (!unit.is_helper && !unit.playerUnit.is_guest)
      {
        PlayerItem equippedGear;
        if ((equippedGear = unit.playerUnit.equippedGear) != (PlayerItem) null)
          gearResults.Add(new WebAPI.Request.BattleFinish.GearResult()
          {
            player_gear_id = equippedGear.id,
            kill_count = unit.killCount,
            damage_count = unit.attackCount
          });
        PlayerItem equippedGear2;
        if ((equippedGear2 = unit.playerUnit.equippedGear2) != (PlayerItem) null)
          gearResults.Add(new WebAPI.Request.BattleFinish.GearResult()
          {
            player_gear_id = equippedGear2.id,
            kill_count = unit.killCount,
            damage_count = unit.attackCount
          });
      }
    }
    return gearResults;
  }

  public IEnumerator BattleResultChange()
  {
    BattleFinalize battleFinalize = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 4;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    BattleCameraFilter.DesotryBattleWin();
    if (battleFinalize.isDoApiStart)
    {
      while (!battleFinalize.isDoApiEnd)
        yield return (object) null;
    }
    else
      yield return (object) battleFinalize.DoAPI();
    if (!battleFinalize.env.core.battleInfo.isEarthMode)
    {
      Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
      BattleUI05Scene.ChangeScene(battleFinalize.env.core.battleInfo, battleFinalize.env.core.isWin, battleFinalize.apiResult.Result);
    }
    else
    {
      battleFinalize.battleManager.deleteSavedEnvironment();
      BattleUI55Scene.ChangeScene(battleFinalize.env.core.battleInfo, battleFinalize.env.core.isWin, battleFinalize.apiResult.Result);
    }
  }

  private void errorCallback(WebAPI.Response.UserError error)
  {
    if (Singleton<NGGameDataManager>.GetInstance().IsSea && string.Equals(error.Code, "SEA000"))
      this.StartCoroutine(PopupUtility.SeaError(error));
    else
      Singleton<NGSceneManager>.GetInstance().StartCoroutine(PopupCommon.Show(error.Code, error.Reason, (Action) (() =>
      {
        NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
        instance.clearStack();
        instance.destroyCurrentScene();
        instance.changeScene(Singleton<CommonRoot>.GetInstance().startScene, false);
      })));
  }
}
