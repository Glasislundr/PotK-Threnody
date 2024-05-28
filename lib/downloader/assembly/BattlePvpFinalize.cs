// Decompiled with JetBrains decompiler
// Type: BattlePvpFinalize
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class BattlePvpFinalize : BattleMonoBehaviour
{
  private PVPManager _pvpManager;

  private PVPManager pvpManager
  {
    get
    {
      if (Object.op_Equality((Object) this._pvpManager, (Object) null))
        this._pvpManager = Singleton<PVPManager>.GetInstance();
      return this._pvpManager;
    }
  }

  protected override IEnumerator Start_Battle()
  {
    BattlePvpFinalize battlePvpFinalize = this;
    int retryCount = 0;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    BattleCameraFilter.DesotryBattleWin();
    IEnumerator e;
    if (battlePvpFinalize.pvpManager.isResult)
    {
      WebAPI.Response.PvpPlayerStatus result;
      do
      {
        if (retryCount > 0)
          yield return (object) new WaitForSeconds(5f);
        Future<WebAPI.Response.PvpPlayerStatus> futureP = WebAPI.PvpPlayerStatus(new Action<WebAPI.Response.UserError>(battlePvpFinalize.errorCallback));
        e = futureP.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        result = futureP.Result;
        if (result == null)
          yield break;
        else
          futureP = (Future<WebAPI.Response.PvpPlayerStatus>) null;
      }
      while (!result.has_battle_result && retryCount++ < 5);
      if (result.has_battle_result)
      {
        Singleton<CommonRoot>.GetInstance().isLoading = true;
        Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
        Versus0268Scene.ChangeScene();
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        ModalWindow.Show(Consts.GetInstance().PVP_NO_RESULT_ERROR_POPUP_TITLE, Consts.GetInstance().PVP_NO_RESULT_ERROR_POPUP_MESSAGE, new Action(battlePvpFinalize.\u003CStart_Battle\u003Eb__3_0));
      }
    }
    else
    {
      Future<WebAPI.Response.PvpForceClose> futureF = WebAPI.PvpForceClose(true, new Action<WebAPI.Response.UserError>(battlePvpFinalize.errorCallback));
      e = futureF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
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
        futureF = (Future<WebAPI.Response.PvpForceClose>) null;
      }
    }
  }

  private IEnumerator ForceClose()
  {
    IEnumerator e1 = WebAPI.PvpForceClose(false, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e))).Wait();
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
