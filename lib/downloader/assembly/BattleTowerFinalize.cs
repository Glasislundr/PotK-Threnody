// Decompiled with JetBrains decompiler
// Type: BattleTowerFinalize
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

#nullable disable
public class BattleTowerFinalize : BattleMonoBehaviour
{
  protected override IEnumerator Start_Battle()
  {
    BattleTowerFinalize battleTowerFinalize = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    BattleCameraFilter.DesotryBattleWin();
    IEnumerator e;
    if (battleTowerFinalize.battleManager.isRetire)
    {
      e = battleTowerFinalize.BattleRetire();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      foreach (BL.Unit unit in battleTowerFinalize.env.core.playerUnits.value)
        unit.playerUnit.tower_hitpoint_rate = (float) (unit.hp * 100) / (float) unit.parameter.Hp;
      foreach (BL.Unit unit in battleTowerFinalize.env.core.enemyUnits.value)
        unit.playerUnit.tower_hitpoint_rate = (float) (unit.hp * 100) / (float) unit.parameter.Hp;
      bool flag = false;
      BL.Unit[] unitArray = (BL.Unit[]) null;
      if (!battleTowerFinalize.env.core.isWin && battleTowerFinalize.env.core.facilityUnits != null && battleTowerFinalize.env.core.facilityUnits.value != null)
      {
        unitArray = battleTowerFinalize.env.core.facilityUnits.value.Where<BL.Unit>((Func<BL.Unit, bool>) (x => !x.facility.isSkillUnit)).ToArray<BL.Unit>();
        if (((IEnumerable<BL.Unit>) unitArray).Any<BL.Unit>())
        {
          flag = true;
          foreach (BL.Unit unit in unitArray)
            unit.playerUnit.tower_hitpoint_rate = (float) (unit.hp * 100) / (float) unit.parameter.Hp;
        }
      }
      long damage = 0;
      bool isOverkill = false;
      int[] overkillEnemyIDs = ((IEnumerable<TowerOverkill>) MasterData.TowerOverkillList).Select<TowerOverkill, int>((Func<TowerOverkill, int>) (x => x.enemyID)).ToArray<int>();
      if (overkillEnemyIDs.Length != 0)
        battleTowerFinalize.env.core.enemyUnits.value.ForEach((Action<BL.Unit>) (x =>
        {
          if (!((IEnumerable<int>) overkillEnemyIDs).FirstIndexOrNull<int>((Func<int, bool>) (id => id == x.specificId)).HasValue)
            return;
          damage += (long) x.overkillDamage;
          if (damage > 2000000000L)
            damage = 2000000000L;
          isOverkill = true;
        }));
      List<WebAPI.Request.BattleFinish.SupplyResult> source1 = new List<WebAPI.Request.BattleFinish.SupplyResult>();
      foreach (BL.Item obj in battleTowerFinalize.env.core.itemList.value)
      {
        int num = obj.initialAmount - obj.amount;
        if (num > 0)
          source1.Add(new WebAPI.Request.BattleFinish.SupplyResult()
          {
            supply_id = obj.itemId,
            use_quantity = num
          });
      }
      IEnumerable<BL.Unit> source2 = flag ? battleTowerFinalize.env.core.enemyUnits.value.Concat<BL.Unit>((IEnumerable<BL.Unit>) unitArray) : (IEnumerable<BL.Unit>) battleTowerFinalize.env.core.enemyUnits.value;
      Future<WebAPI.Response.TowerBattleFinish> f = new TowerBattleResult(SMManager.Get<Player>().id).webAPI(battleTowerFinalize.env.core.phaseState.turnCount, battleTowerFinalize.env.core.battleInfo.battleId, (int) damage, source2.Select<BL.Unit, float>((Func<BL.Unit, float>) (x => x.playerUnit.TowerHpRate)).ToArray<float>(), source2.Select<BL.Unit, int>((Func<BL.Unit, int>) (x => x.playerUnit.id)).ToArray<int>(), source2.Select<BL.Unit, int>((Func<BL.Unit, int>) (x => x.killCount)).ToArray<int>(), battleTowerFinalize.env.core.playerUnits.value.Select<BL.Unit, float>((Func<BL.Unit, float>) (x => x.playerUnit.TowerHpRate)).ToArray<float>(), battleTowerFinalize.env.core.playerUnits.value.Select<BL.Unit, int>((Func<BL.Unit, int>) (x => x.playerUnit.id)).ToArray<int>(), source1.Select<WebAPI.Request.BattleFinish.SupplyResult, int>((Func<WebAPI.Request.BattleFinish.SupplyResult, int>) (x => x.supply_id)).ToArray<int>(), source1.Select<WebAPI.Request.BattleFinish.SupplyResult, int>((Func<WebAPI.Request.BattleFinish.SupplyResult, int>) (x => x.use_quantity)).ToArray<int>(), battleTowerFinalize.env.core.isWin);
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (f.Result == null)
        TowerUtil.GotoMypage();
      else if (battleTowerFinalize.env.core.isWin)
      {
        Tower029BattleResultScene.ChangeScene(battleTowerFinalize.env.core.battleInfo, f.Result, isOverkill);
      }
      else
      {
        NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
        instance.clearStack();
        instance.destroyCurrentScene();
        instance.destroyLoadedScenes();
        Tower029TopScene.ChangeScene(false);
      }
    }
  }

  private IEnumerator BattleRetire()
  {
    BattleTowerFinalize battleTowerFinalize = this;
    Future<WebAPI.Response.TowerBattleRetire> ft = WebAPI.TowerBattleRetire(battleTowerFinalize.env.core.phaseState.turnCount, battleTowerFinalize.env.core.enemyUnits.value.Select<BL.Unit, int>((Func<BL.Unit, int>) (x => x.killCount)).ToArray<int>(), (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
    IEnumerator e1 = ft.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (ft.Result == null)
    {
      TowerUtil.GotoMypage();
    }
    else
    {
      NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
      instance.clearStack();
      instance.destroyCurrentScene();
      instance.destroyLoadedScenes();
      Tower029TopScene.ChangeScene(false);
    }
  }
}
