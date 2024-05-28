// Decompiled with JetBrains decompiler
// Type: BattleCorpsFinalize
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
public class BattleCorpsFinalize : BattleMonoBehaviour
{
  protected override IEnumerator Start_Battle()
  {
    BattleCorpsFinalize battleCorpsFinalize = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    BattleCameraFilter.DesotryBattleWin();
    List<WebAPI.Request.BattleFinish.SupplyResult> source1 = new List<WebAPI.Request.BattleFinish.SupplyResult>();
    foreach (BL.Item obj in battleCorpsFinalize.env.core.itemList.value)
    {
      int num = obj.initialAmount - obj.amount;
      if (num > 0)
        source1.Add(new WebAPI.Request.BattleFinish.SupplyResult()
        {
          supply_id = obj.itemId,
          use_quantity = num
        });
    }
    List<BL.Unit> source2 = battleCorpsFinalize.env.core.playerUnits.value;
    BL.Unit[] array = battleCorpsFinalize.env.core.enemyUnits.value.Concat<BL.Unit>(battleCorpsFinalize.env.core.facilityUnits.value.Where<BL.Unit>((Func<BL.Unit, bool>) (x => !x.facility.isSkillUnit))).ToArray<BL.Unit>();
    string errorCode = string.Empty;
    Future<WebAPI.Response.QuestCorpsBattleFinish> api = WebAPI.QuestCorpsBattleFinish(battleCorpsFinalize.env.core.phaseState.turnCount, battleCorpsFinalize.env.core.battleInfo.battleId, string.Empty, 0, ((IEnumerable<BL.Unit>) array).Select<BL.Unit, int>((Func<BL.Unit, int>) (x => x.hp)).ToArray<int>(), ((IEnumerable<BL.Unit>) array).Select<BL.Unit, int>((Func<BL.Unit, int>) (x => x.playerUnit.id)).ToArray<int>(), ((IEnumerable<BL.Unit>) array).Select<BL.Unit, int>((Func<BL.Unit, int>) (x => x.killCount)).ToArray<int>(), source2.Select<BL.Unit, int>((Func<BL.Unit, int>) (x => x.hp)).ToArray<int>(), source2.Select<BL.Unit, int>((Func<BL.Unit, int>) (x => x.hpMax)).ToArray<int>(), source2.Select<BL.Unit, int>((Func<BL.Unit, int>) (x => x.playerUnit.id)).ToArray<int>(), source1.Select<WebAPI.Request.BattleFinish.SupplyResult, int>((Func<WebAPI.Request.BattleFinish.SupplyResult, int>) (x => x.supply_id)).ToArray<int>(), source1.Select<WebAPI.Request.BattleFinish.SupplyResult, int>((Func<WebAPI.Request.BattleFinish.SupplyResult, int>) (x => x.use_quantity)).ToArray<int>(), battleCorpsFinalize.env.core.isWin, (Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      errorCode = e.Code;
    }));
    IEnumerator e1 = api.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (api.Result == null)
    {
      if (errorCode == "CPS001")
      {
        e1 = WebAPI.QuestCorpsBattleForceClose().Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
      }
      MypageScene.ChangeSceneOnError();
    }
    else if (battleCorpsFinalize.env.core.isWin)
    {
      CorpsQuestBattleResultScene.ChangeScene(battleCorpsFinalize.env.core.battleInfo, api.Result);
    }
    else
    {
      NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
      instance.clearStack();
      instance.destroyCurrentScene();
      instance.destroyLoadedScenes();
      CorpsQuestTopScene.ChangeScene(((IEnumerable<PlayerCorps>) api.Result.player_corps_list).First<PlayerCorps>().period_id);
    }
  }
}
