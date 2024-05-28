// Decompiled with JetBrains decompiler
// Type: BattlePvnpcFinalize
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BattlePvnpcFinalize : BattleMonoBehaviour
{
  private PVNpcManager _pvnpcManager;

  private PVNpcManager pvnpcManager
  {
    get
    {
      if (Object.op_Equality((Object) this._pvnpcManager, (Object) null))
        this._pvnpcManager = Singleton<PVNpcManager>.GetInstance();
      return this._pvnpcManager;
    }
  }

  protected override IEnumerator Start_Battle()
  {
    BattlePvnpcFinalize battlePvnpcFinalize = this;
    int retryCount = 0;
    Singleton<CommonRoot>.GetInstance().loadingMode = 4;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    BattleCameraFilter.DesotryBattleWin();
    IEnumerator e1;
    if (battlePvnpcFinalize.pvnpcManager.isResult)
    {
      string[] player_id = new string[2]
      {
        battlePvnpcFinalize.pvnpcManager.player.id,
        battlePvnpcFinalize.pvnpcManager.enemy.id
      };
      int[] player_result = new int[2]
      {
        (int) battlePvnpcFinalize.pvnpcManager.GetPlayerVictory(),
        (int) battlePvnpcFinalize.pvnpcManager.GetEnemyVictory()
      };
      int[] player_result_effect = new int[2]
      {
        (int) battlePvnpcFinalize.pvnpcManager.GetPlayerVictoryEffect(),
        (int) battlePvnpcFinalize.pvnpcManager.GetEnemyVictoryEffect()
      };
      int player_battle_result_point = battlePvnpcFinalize.pvnpcManager.envCore.playerPointGauge;
      int target_player_battle_result_point = battlePvnpcFinalize.pvnpcManager.envCore.enemyPointGauge;
      List<int> player_character_id = new List<int>();
      List<int> target_character_id = new List<int>();
      List<int> results_exp = new List<int>();
      foreach (Tuple<int, int, int> tuple in battlePvnpcFinalize.env.core.getPlayerIntimateResult())
      {
        player_character_id.Add(tuple.Item1);
        target_character_id.Add(tuple.Item2);
        results_exp.Add(tuple.Item3);
      }
      List<int> player_unit_id = new List<int>();
      List<int> defeat_count = new List<int>();
      List<int> result_point = new List<int>();
      List<int> dead_count = new List<int>();
      List<int> duel_count = new List<int>();
      foreach (BL.Unit unit in battlePvnpcFinalize.env.core.playerUnits.value)
      {
        player_unit_id.Add(unit.playerUnit.id);
        defeat_count.Add(unit.killCount);
        result_point.Add(unit.pvpPoint);
        dead_count.Add(unit.deadCount);
        duel_count.Add(unit.duelCount);
      }
      List<int> target_player_unit_id = new List<int>();
      List<int> target_defeat_count = new List<int>();
      List<int> target_result_point = new List<int>();
      List<int> target_dead_count = new List<int>();
      List<int> target_duel_count = new List<int>();
      foreach (BL.Unit unit in battlePvnpcFinalize.env.core.enemyUnits.value)
      {
        target_player_unit_id.Add(unit.playerUnit.id);
        target_defeat_count.Add(unit.killCount);
        target_result_point.Add(unit.pvpPoint);
        target_dead_count.Add(unit.deadCount);
        target_duel_count.Add(unit.duelCount);
      }
      string errorCode = string.Empty;
      Future<WebAPI.Response.PvpPlayerNpcFinish> ft;
      while (true)
      {
        errorCode = string.Empty;
        string[] battle_results_player_id = player_id;
        int[] battle_results_player_result = player_result;
        int[] battle_results_player_result_effect = player_result_effect;
        string battleId = battlePvnpcFinalize.env.core.battleInfo.battleId;
        int num1 = player_battle_result_point;
        int num2 = target_player_battle_result_point;
        int[] array1 = player_character_id.ToArray();
        int[] array2 = results_exp.ToArray();
        int[] array3 = target_character_id.ToArray();
        int[] array4 = target_dead_count.ToArray();
        int[] array5 = target_defeat_count.ToArray();
        int[] array6 = target_duel_count.ToArray();
        int[] array7 = target_player_unit_id.ToArray();
        int[] array8 = target_result_point.ToArray();
        int[] array9 = dead_count.ToArray();
        int[] array10 = defeat_count.ToArray();
        int[] array11 = duel_count.ToArray();
        int[] array12 = player_unit_id.ToArray();
        int[] array13 = result_point.ToArray();
        int endPoint = battlePvnpcFinalize._pvnpcManager.endPoint;
        string battle_uuid = battleId;
        int[] intimate_results_character_id = array1;
        int[] intimate_results_exp = array2;
        int[] intimate_results_target_character_id = array3;
        int player_battle_result_point1 = num1;
        int target_player_battle_result_point1 = num2;
        int[] target_unit_results_dead_count = array4;
        int[] target_unit_results_defeat_count = array5;
        int[] target_unit_results_duel_count = array6;
        int[] target_unit_results_player_unit_id = array7;
        int[] target_unit_results_point = array8;
        int[] unit_results_dead_count = array9;
        int[] unit_results_defeat_count = array10;
        int[] unit_results_duel_count = array11;
        int[] unit_results_player_unit_id = array12;
        int[] unit_results_point = array13;
        Action<WebAPI.Response.UserError> userErrorCallback = (Action<WebAPI.Response.UserError>) (e => errorCode = e.Code);
        ft = WebAPI.PvpPlayerNpcFinish(battle_results_player_id, battle_results_player_result, battle_results_player_result_effect, endPoint, battle_uuid, intimate_results_character_id, intimate_results_exp, intimate_results_target_character_id, player_battle_result_point1, target_player_battle_result_point1, target_unit_results_dead_count, target_unit_results_defeat_count, target_unit_results_duel_count, target_unit_results_player_unit_id, target_unit_results_point, unit_results_dead_count, unit_results_defeat_count, unit_results_duel_count, unit_results_player_unit_id, unit_results_point, userErrorCallback);
        e1 = ft.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        if (ft.Result == null)
        {
          ++retryCount;
          if (retryCount < 5)
            yield return (object) new WaitForSeconds(5f);
          else
            goto label_23;
        }
        else
          break;
      }
      battlePvnpcFinalize.battleManager.deleteSavedEnvironment();
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
      Versus0268Scene.ChangeScene(ft.Result);
      yield break;
label_23:
      // ISSUE: reference to a compiler-generated method
      ModalWindow.Show(Consts.GetInstance().PVP_NO_RESULT_ERROR_POPUP_TITLE, Consts.GetInstance().PVP_NO_RESULT_ERROR_POPUP_MESSAGE, new Action(battlePvnpcFinalize.\u003CStart_Battle\u003Eb__3_0));
      player_id = (string[]) null;
      player_result = (int[]) null;
      player_result_effect = (int[]) null;
      player_character_id = (List<int>) null;
      target_character_id = (List<int>) null;
      results_exp = (List<int>) null;
      player_unit_id = (List<int>) null;
      defeat_count = (List<int>) null;
      result_point = (List<int>) null;
      dead_count = (List<int>) null;
      duel_count = (List<int>) null;
      target_player_unit_id = (List<int>) null;
      target_defeat_count = (List<int>) null;
      target_result_point = (List<int>) null;
      target_dead_count = (List<int>) null;
      target_duel_count = (List<int>) null;
    }
    else
    {
      Future<WebAPI.Response.PvpNpcForceClose> futureF = WebAPI.PvpNpcForceClose(true, new Action<WebAPI.Response.UserError>(battlePvnpcFinalize.errorCallback));
      e1 = futureF.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (futureF.Result != null)
      {
        NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
        instance.clearStack();
        instance.destroyLoadedScenes();
        Persist.pvpSuspend.Delete();
        if (Persist.pvpInfo.Data.lastMatchingType == PvpMatchingTypeEnum.class_match)
          Versus02610Scene.ChangeScene(false, true);
        else
          Versus0262Scene.ChangeScene0262(false, Persist.pvpInfo.Data.lastMatchingType, true);
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        futureF = (Future<WebAPI.Response.PvpNpcForceClose>) null;
      }
    }
  }

  private IEnumerator ForceClose()
  {
    IEnumerator e1 = WebAPI.PvpNpcForceClose(false, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e))).Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    instance.clearStack();
    instance.destroyLoadedScenes();
    Persist.pvpSuspend.Delete();
    if (Persist.pvpInfo.Data.lastMatchingType == PvpMatchingTypeEnum.class_match)
      Versus02610Scene.ChangeScene(false, true);
    else
      Versus0262Scene.ChangeScene0262(false, Persist.pvpInfo.Data.lastMatchingType, true);
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  private void errorCallback(WebAPI.Response.UserError error)
  {
    Singleton<NGSceneManager>.GetInstance().StartCoroutine(PopupCommon.Show(error.Code, error.Reason, (Action) (() =>
    {
      NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
      instance.clearStack();
      instance.destroyCurrentScene();
      instance.changeScene(Singleton<CommonRoot>.GetInstance().startScene, false);
    })));
  }
}
