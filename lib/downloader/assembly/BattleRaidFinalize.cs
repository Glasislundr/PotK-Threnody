// Decompiled with JetBrains decompiler
// Type: BattleRaidFinalize
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;

#nullable disable
public class BattleRaidFinalize : BattleMonoBehaviour
{
  protected override IEnumerator Start_Battle()
  {
    BattleRaidFinalize battleRaidFinalize = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 4;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    BattleCameraFilter.DesotryBattleWin();
    WebAPI.Request.BattleFinish req = new WebAPI.Request.BattleFinish();
    req.quest_type = battleRaidFinalize.env.core.battleInfo.quest_type;
    req.win = battleRaidFinalize.env.core.isWin;
    req.is_game_over = battleRaidFinalize.env.core.allDeadUnitsp(BL.ForceID.player);
    req.is_retire = battleRaidFinalize.battleManager.isRetire;
    req.battle_uuid = battleRaidFinalize.env.core.battleInfo.battleId;
    req.player_money = 0;
    req.battle_turn = battleRaidFinalize.env.core.phaseState.turnCount;
    req.continue_count = battleRaidFinalize.env.core.continueCount;
    foreach (BL.Unit unit in battleRaidFinalize.env.core.playerUnits.value)
    {
      if (unit.duelHistory != null)
      {
        foreach (BL.DuelHistory duelHistory in unit.duelHistory)
        {
          req.duels_damage.Add(duelHistory.inflictTotalDamage);
          req.duels_hit_damage.Add(duelHistory.sufferTotalDamage);
          req.duels_critical_count.Add(duelHistory.criticalCount);
          req.duels_max_damage.Add(duelHistory.inflictMaxDamage);
          req.week_element_attack_count += unit.playerUnit.is_guest ? 0 : duelHistory.weekElementAttackCount;
          req.week_kind_attack_count += unit.playerUnit.is_guest ? 0 : duelHistory.weekKindAttackCount;
        }
      }
    }
    for (int row = 0; row < battleRaidFinalize.env.core.battleInfo.stage.map_height; ++row)
    {
      for (int column = 0; column < battleRaidFinalize.env.core.battleInfo.stage.map_width; ++column)
      {
        BL.Panel fieldPanel = battleRaidFinalize.env.core.getFieldPanel(row, column);
        if (fieldPanel.fieldEvent != null && fieldPanel.fieldEvent.isCompleted)
        {
          req.panel_reward.Add(fieldPanel.fieldEvent.reward);
          req.panel_entity_ids.Add(fieldPanel.fieldEventId);
        }
      }
    }
    foreach (BL.Unit unit in battleRaidFinalize.env.core.enemyUnits.value)
    {
      if (unit.hasDrop && unit.drop.isCompleted)
      {
        req.panel_reward.Add(unit.drop.reward);
        req.drop_entity_ids.Add(unit.playerUnit.id);
      }
    }
    foreach (BL.Unit unit in battleRaidFinalize.env.core.playerUnits.value)
      req.units.Add(new WebAPI.Request.BattleFinish.UnitResult()
      {
        player_unit_id = unit.playerUnit.id,
        total_damage = unit.attackDamage,
        total_damage_count = unit.attackCount,
        total_kill_count = unit.killCount,
        remaining_hp = unit.hp,
        rental = unit.is_helper ? 1 : (unit.index != 5 || !unit.playerUnit.is_guest ? 0 : 1),
        received_damage = unit.receivedDamage,
        guest = unit.playerUnit.is_guest ? 1 : 0
      });
    foreach (BL.Unit unit in battleRaidFinalize.env.core.enemyUnits.value)
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
      enemyResult.level = unit.playerUnit.level;
      enemyResult.damage = Math.Max(0, unit.initialHp - unit.hp);
      enemyResult.isBoss = Singleton<NGBattleManager>.GetInstance().isRaidBoss(unit);
      req.enemies.Add(enemyResult);
    }
    foreach (BL.Unit unit in battleRaidFinalize.env.core.playerUnits.value)
    {
      if (!unit.is_helper && !unit.playerUnit.is_guest)
      {
        if (unit.playerUnit.equippedGear != (PlayerItem) null)
          req.gears.Add(new WebAPI.Request.BattleFinish.GearResult()
          {
            player_gear_id = unit.playerUnit.equippedGear.id,
            kill_count = unit.killCount,
            damage_count = unit.attackCount
          });
        if (unit.playerUnit.equippedGear2 != (PlayerItem) null)
          req.gears.Add(new WebAPI.Request.BattleFinish.GearResult()
          {
            player_gear_id = unit.playerUnit.equippedGear2.id,
            kill_count = unit.killCount,
            damage_count = unit.attackCount
          });
        if (unit.playerUnit.equippedGear3 != (PlayerItem) null)
          req.gears.Add(new WebAPI.Request.BattleFinish.GearResult()
          {
            player_gear_id = unit.playerUnit.equippedGear3.id,
            kill_count = unit.killCount,
            damage_count = unit.attackCount
          });
      }
    }
    foreach (BL.Item obj in battleRaidFinalize.env.core.itemList.value)
    {
      int num = obj.initialAmount - obj.amount;
      if (num > 0)
        req.supplies.Add(new WebAPI.Request.BattleFinish.SupplyResult()
        {
          supply_id = obj.itemId,
          use_quantity = num
        });
    }
    foreach (Tuple<int, int, int> tuple in battleRaidFinalize.env.core.getPlayerIntimateResult())
      req.intimates.Add(new WebAPI.Request.BattleFinish.IntimateResult()
      {
        character_id = tuple.Item1,
        target_character_id = tuple.Item2,
        exp = tuple.Item3
      });
    Future<WebAPI.Response.GuildraidBattleFinish> f = WebAPI.RaidBattleFinish(req, battleRaidFinalize.env, new Action<WebAPI.Response.UserError>(battleRaidFinalize.errorCallback));
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (f.Result != null)
      BattleUI05Scene.ChangeScene(battleRaidFinalize.env.core.battleInfo, req.win, req.is_game_over, req.is_retire, f.Result);
  }

  private void errorCallback(WebAPI.Response.UserError error)
  {
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    string title;
    string message;
    if (error.Code.Equals("GLD003"))
    {
      if (Persist.battleEnvironment.Exists)
        Persist.battleEnvironment.Delete();
      title = Consts.GetInstance().GUILD_NOT_MEMBER;
      message = Consts.GetInstance().RAID_BATTLE_END_NOT_MEMBER;
    }
    else
    {
      title = error.Code;
      message = error.Reason;
    }
    Singleton<NGSceneManager>.GetInstance().StartCoroutine(PopupCommon.Show(title, message, (Action) (() =>
    {
      NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
      instance.clearStack();
      instance.destroyCurrentScene();
      instance.changeScene(Singleton<CommonRoot>.GetInstance().startScene, false);
    })));
  }
}
