// Decompiled with JetBrains decompiler
// Type: BattleGvgFinalize
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class BattleGvgFinalize : BattleMonoBehaviour
{
  private GVGManager _gvgManager;

  private GVGManager gvgManager
  {
    get
    {
      if (Object.op_Equality((Object) this._gvgManager, (Object) null))
        this._gvgManager = Singleton<GVGManager>.GetInstance();
      return this._gvgManager;
    }
  }

  protected override IEnumerator Start_Battle()
  {
    BattleGvgFinalize battleGvgFinalize = this;
    NGSceneManager sm = Singleton<NGSceneManager>.GetInstance();
    int retryCount = 0;
    Singleton<CommonRoot>.GetInstance().loadingMode = 4;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    BattleCameraFilter.DesotryBattleWin();
    IEnumerator e1;
    if (battleGvgFinalize.env.core.battleInfo.battleId.Equals(string.Empty))
    {
      sm.clearStack();
      sm.destroyLoadedScenes();
      battleGvgFinalize.battleManager.deleteSavedEnvironment();
      GuildUtil.gvgBattleIDServer = string.Empty;
      Guild0282Scene.ChangeSceneBattleFinish(battleGvgFinalize.battleManager.battleInfo.gvgSetting.enemyID, 0, true);
    }
    else if (battleGvgFinalize.gvgManager.isResult)
    {
      List<int> enemyRental = new List<int>();
      List<int> playerRental = new List<int>();
      List<int> playerTotalAttackDamage = new List<int>();
      List<int> enemyTotalAttackDamage = new List<int>();
      foreach (BL.Unit unit in battleGvgFinalize.env.core.playerUnits.value)
        playerTotalAttackDamage.Add(unit.attackDamage + unit.attackOverkillDamage);
      foreach (BL.Unit unit in battleGvgFinalize.env.core.enemyUnits.value)
        enemyTotalAttackDamage.Add(unit.attackDamage + unit.attackOverkillDamage);
      foreach (BL.Unit unit in battleGvgFinalize.env.core.enemyUnits.value)
        enemyRental.Add(unit.is_helper ? 1 : (unit.index != 5 || !unit.playerUnit.is_guest ? 0 : 1));
      foreach (BL.Unit unit in battleGvgFinalize.env.core.playerUnits.value)
        playerRental.Add(unit.is_helper ? 1 : (unit.index != 5 || !unit.playerUnit.is_guest ? 0 : 1));
      string errorCode = string.Empty;
      Future<WebAPI.Response.GvgBattleFinish> ft;
      while (true)
      {
        errorCode = string.Empty;
        bool maintenance = false;
        ft = WebAPI.GvgBattleFinish(battleGvgFinalize.gvgManager.enemyAnnihilationCount, battleGvgFinalize.env.core.battleInfo.battleId, battleGvgFinalize.gvgManager.starNum, battleGvgFinalize.gvgManager.playerAnnihilationCount, battleGvgFinalize.env.core.enemyUnits.value.Select<BL.Unit, int>((Func<BL.Unit, int>) (x => x.deadCountExceptImmediateRebirth)).ToArray<int>(), battleGvgFinalize.env.core.enemyUnits.value.Select<BL.Unit, int>((Func<BL.Unit, int>) (x => x.playerUnit.id)).ToArray<int>(), enemyRental.ToArray(), enemyTotalAttackDamage.ToArray(), battleGvgFinalize.env.core.enemyPoint, battleGvgFinalize.env.core.playerPoint, battleGvgFinalize.env.core.playerUnits.value.Select<BL.Unit, int>((Func<BL.Unit, int>) (x => x.deadCountExceptImmediateRebirth)).ToArray<int>(), battleGvgFinalize.env.core.playerUnits.value.Select<BL.Unit, int>((Func<BL.Unit, int>) (x => x.playerUnit.id)).ToArray<int>(), playerRental.ToArray(), playerTotalAttackDamage.ToArray(), battleGvgFinalize.env.core.isWin, (Action<WebAPI.Response.UserError>) (e =>
        {
          if (e.Code.Equals("GLD014"))
          {
            maintenance = true;
            WebAPI.DefaultUserErrorCallback(e);
          }
          errorCode = e.Code;
        }));
        e1 = ft.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        if (!maintenance)
        {
          if (ft.Result == null)
          {
            ++retryCount;
            if (retryCount < 5)
              yield return (object) new WaitForSeconds(5f);
            else
              goto label_33;
          }
          else
            goto label_32;
        }
        else
          break;
      }
      sm.clearStack();
      sm.destroyLoadedScenes();
      GuildUtil.gvgBattleIDServer = string.Empty;
      MypageScene.ChangeSceneOnError();
      yield break;
label_32:
      battleGvgFinalize.battleManager.deleteSavedEnvironment();
      GuildUtil.gvgBattleIDServer = string.Empty;
      Guild0288Scene.ChangeScene(ft.Result, battleGvgFinalize.battleManager.battleInfo.gvgSetting.enemyID);
      yield break;
label_33:
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string resultErrorTitle;
      string message;
      if (errorCode.Equals("GVG003"))
      {
        resultErrorTitle = Consts.GetInstance().POPUP_GUILD_BATTLE_INVALID_RESULT_ERROR_TITLE;
        message = Consts.GetInstance().POPUP_GUILD_BATTLE_INVALID_RESULT_ERROR_MESSAGE;
      }
      else if (errorCode.Equals("GVG006") || errorCode.Equals("GVG001"))
      {
        resultErrorTitle = Consts.GetInstance().POPUP_GUILD_BATTLE_INVALID_RESULT_ERROR_TITLE;
        message = Consts.GetInstance().POPUP_GUILD_BATTLE_ALREADY_END_MESSAGE;
      }
      else
      {
        resultErrorTitle = Consts.GetInstance().POPUP_GUILD_BATTLE_SEND_RESULT_ERROR_TITLE;
        message = Consts.GetInstance().POPUP_GUILD_BATTLE_SEND_RESULT_ERROR_MESSAGE;
      }
      // ISSUE: reference to a compiler-generated method
      ModalWindow.Show(resultErrorTitle, message, new Action(battleGvgFinalize.\u003CStart_Battle\u003Eb__3_0));
      enemyRental = (List<int>) null;
      playerRental = (List<int>) null;
      playerTotalAttackDamage = (List<int>) null;
      enemyTotalAttackDamage = (List<int>) null;
    }
    else
    {
      string errCode = string.Empty;
      Future<WebAPI.Response.GvgBattleForceClose> futureF = WebAPI.GvgBattleForceClose(battleGvgFinalize.battleManager.battleInfo.battleId, (Action<WebAPI.Response.UserError>) (e =>
      {
        errCode = e.Code;
        if (!e.Code.Equals("GLD014"))
          return;
        WebAPI.DefaultUserErrorCallback(e);
      }));
      e1 = futureF.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (futureF.Result == null)
      {
        if (errCode.Equals("GLD014"))
        {
          sm.clearStack();
          sm.destroyLoadedScenes();
          GuildUtil.gvgBattleIDServer = string.Empty;
          MypageScene.ChangeSceneOnError();
          yield break;
        }
        else
          ModalWindow.Show(Consts.GetInstance().POPUP_GUILD_BATTLE_FORCE_CLOSE_ERROR_TITLE, Consts.GetInstance().POPUP_GUILD_BATTLE_FORCE_CLOSE_ERROR_MESSAGE, (Action) (() => { }));
      }
      sm.clearStack();
      sm.destroyLoadedScenes();
      battleGvgFinalize.battleManager.deleteSavedEnvironment();
      GuildUtil.gvgBattleIDServer = string.Empty;
      Guild0282Scene.ChangeSceneBattleFinish(battleGvgFinalize.battleManager.battleInfo.gvgSetting.enemyID, 0, false);
    }
  }

  private IEnumerator ForceClose()
  {
    BattleGvgFinalize battleGvgFinalize = this;
    NGSceneManager sm = Singleton<NGSceneManager>.GetInstance();
    bool maintenance = false;
    IEnumerator e1 = WebAPI.GvgBattleForceClose(Singleton<NGBattleManager>.GetInstance().battleInfo.battleId, (Action<WebAPI.Response.UserError>) (e =>
    {
      if (e.Code.Equals("GLD014"))
        maintenance = true;
      WebAPI.DefaultUserErrorCallback(e);
    })).Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (maintenance)
    {
      sm.clearStack();
      sm.destroyLoadedScenes();
      GuildUtil.gvgBattleIDServer = string.Empty;
      MypageScene.ChangeSceneOnError();
    }
    else
    {
      sm.clearStack();
      sm.destroyLoadedScenes();
      battleGvgFinalize.battleManager.deleteSavedEnvironment();
      GuildUtil.gvgBattleIDServer = string.Empty;
      Guild0282Scene.ChangeSceneBattleFinish(battleGvgFinalize.battleManager.battleInfo.gvgSetting.enemyID, 0, false);
    }
  }
}
