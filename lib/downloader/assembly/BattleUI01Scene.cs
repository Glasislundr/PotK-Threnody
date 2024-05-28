// Decompiled with JetBrains decompiler
// Type: BattleUI01Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BattleUI01Scene : BattleSceneBase
{
  public override List<string> createResourceLoadList()
  {
    NGBattleManager instance = Singleton<NGBattleManager>.GetInstance();
    return instance.isError ? new List<string>() : instance.createResourceLoadList();
  }

  public override IEnumerator onInitSceneAsync()
  {
    BattleUI01Scene battleUi01Scene = this;
    NGBattleManager bm = Singleton<NGBattleManager>.GetInstance();
    if (!bm.isError)
    {
      if (bm.isEarth)
      {
        CommonEarthHeader earthHeaderComponent = Singleton<CommonRoot>.GetInstance().GetEarthHeaderComponent();
        if (Object.op_Inequality((Object) earthHeaderComponent, (Object) null))
          earthHeaderComponent.isActive = false;
      }
      IEnumerator e = bm.initBattle();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (!bm.isError)
      {
        try
        {
          bm.environment.core.sight.value = Persist.battle.Data.sight;
        }
        catch (Exception ex)
        {
          Persist.battle.Delete();
        }
        try
        {
          if (!bm.isOvo)
          {
            if (!bm.isEarth)
            {
              Persist.AutoBattleSetting data = Persist.autoBattleSetting.Data;
              bm.environment.core.isAutoBattle.value = data.isAutoBattle;
              bm.environment.core.isAutoItemMove.value = data.isItemMove;
            }
          }
        }
        catch (Exception ex)
        {
          Persist.autoBattleSetting.Delete();
        }
        try
        {
          bm.environment.core.isTouchWait.value = Persist.battleTouchWait.Data.isTouchWait;
        }
        catch (Exception ex)
        {
          Persist.battleTouchWait.Delete();
        }
        try
        {
          bm.environment.core.isSkillUseConfirmation.value = Persist.battleSkillUseConfirmation.Data.isSkillUseConfirmation;
        }
        catch (Exception ex)
        {
          Persist.battleSkillUseConfirmation.Delete();
        }
        try
        {
          bm.environment.core.isViewUnitType.value = Persist.battleIcon.Data.canDisp;
        }
        catch (Exception ex)
        {
          Persist.battleIcon.Delete();
        }
        try
        {
          bm.environment.core.isViewDengerArea.value = Persist.dangerousAreaIcon.Data.canDisp;
        }
        catch (Exception ex)
        {
          Persist.dangerousAreaIcon.Delete();
        }
        if (bm.battleInfo.pvp)
        {
          bm.noDuelScene = false;
        }
        else
        {
          try
          {
            bm.noDuelScene = Persist.battleNoDuel.Data.noDuelScene;
          }
          catch (Exception ex)
          {
            Persist.battleNoDuel.Delete();
          }
        }
        Singleton<NGSoundManager>.GetInstance().AttachDspBusSetting(bm.environment.core.stage.stage.bus_dsp_setting_name, bm.environment.core.stage.stage.busLevel);
        if (bm.environment.core.phaseState.state == BL.Phase.enemy)
        {
          battleUi01Scene.bgmName = bm.environment.core.stage.stage.field_enemy_bgm;
          battleUi01Scene.bgmFile = bm.environment.core.stage.stage.field_enemy_bgm_file;
        }
        else
        {
          battleUi01Scene.bgmName = bm.environment.core.stage.stage.field_player_bgm;
          battleUi01Scene.bgmFile = bm.environment.core.stage.stage.field_player_bgm_file;
        }
        // ISSUE: reference to a compiler-generated method
        e = battleUi01Scene.\u003C\u003En__0();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  public IEnumerator onStartSceneAsync()
  {
    yield break;
  }

  public void onStartScene()
  {
    NGBattleManager instance1 = Singleton<NGBattleManager>.GetInstance();
    if (instance1.isError)
    {
      this.StartCoroutine(this.doErrorChangeScene());
    }
    else
    {
      NGBattle3DObjectManager manager = instance1.getManager<NGBattle3DObjectManager>();
      if (Object.op_Inequality((Object) manager, (Object) null))
        manager.setRootActive(true);
      BattleAIController controller = instance1.getController<BattleAIController>();
      CommonRoot instance2 = Singleton<CommonRoot>.GetInstance();
      instance2.isActiveBackground3DCamera = false;
      instance2.isActive3DUIMask = Object.op_Inequality((Object) controller, (Object) null) && controller.isAction;
      this.StartCoroutine(this.doSetBattleEnable());
      Singleton<CommonRoot>.GetInstance().isLoading = Singleton<CommonRoot>.GetInstance().isLoading && (instance1.environment.core.phaseState.state == BL.Phase.battle_start || instance1.environment.core.phaseState.state == BL.Phase.pvp_restart);
      if (Singleton<CommonRoot>.GetInstance().isLoading)
        return;
      BattleCameraFilter.Active();
    }
  }

  public override void onEndScene()
  {
    NGBattleManager instance1 = Singleton<NGBattleManager>.GetInstance();
    if (instance1.isError)
      return;
    instance1.isBattleEnable = false;
    CommonRoot instance2 = Singleton<CommonRoot>.GetInstance();
    instance2.isActiveBackground3DCamera = true;
    instance2.isActive3DUIMask = true;
    if (instance1.environment.core.phaseState.state == BL.Phase.enemy)
      this.bgmName = instance1.environment.core.stage.stage.field_enemy_bgm;
    else
      this.bgmName = instance1.environment.core.stage.stage.field_player_bgm;
  }

  public override IEnumerator onDestroySceneAsync()
  {
    NGBattleManager instance = Singleton<NGBattleManager>.GetInstance();
    try
    {
      Persist.battle.Data.sight = instance.environment.core.sight.value;
      Persist.battle.Flush();
    }
    catch (Exception ex)
    {
    }
    if (!instance.isOvo)
    {
      if (!instance.isEarth)
      {
        try
        {
          Persist.autoBattleSetting.Data.isAutoBattle = instance.environment.core.isAutoBattle.value;
          Persist.autoBattleSetting.Flush();
        }
        catch (Exception ex)
        {
        }
      }
    }
    try
    {
      Persist.battleIcon.Data.canDisp = instance.environment.core.isViewUnitType.value;
      Persist.battleIcon.Flush();
    }
    catch (Exception ex)
    {
    }
    try
    {
      Persist.dangerousAreaIcon.Data.canDisp = instance.environment.core.isViewDengerArea.value;
      Persist.dangerousAreaIcon.Flush();
    }
    catch (Exception ex)
    {
    }
    if (!instance.isPvp)
    {
      if (!instance.isPvnpc)
      {
        try
        {
          Persist.battleNoDuel.Data.noDuelScene = instance.noDuelScene;
          Persist.battleNoDuel.Flush();
        }
        catch (Exception ex)
        {
        }
      }
    }
    IEnumerator e = instance.cleanupBattle();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isActive3DUIMask = false;
    Singleton<NGSoundManager>.GetInstance().DetachDspBusSetting();
    e = PVPManager.destroyPVPManager();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<NGDuelDataManager>.GetInstance().Init();
  }

  private IEnumerator doSetBattleEnable()
  {
    NGBattleManager bm = Singleton<NGBattleManager>.GetInstance();
    NGSceneManager sm = Singleton<NGSceneManager>.GetInstance();
    yield return (object) new WaitWhile((Func<bool>) (() => !bm.initialized || !sm.isSceneInitialized));
    bm.isBattleEnable = true;
  }

  private IEnumerator doErrorChangeScene()
  {
    if (!Singleton<PopupManager>.GetInstance().ModalWindowIsOpen)
    {
      NGSceneManager sm = Singleton<NGSceneManager>.GetInstance();
      yield return (object) new WaitWhile((Func<bool>) (() => !sm.isSceneInitialized));
      string str;
      if (Singleton<PVPManager>.GetInstance().errorCode != null)
      {
        str = string.Format(Consts.GetInstance().VERSUS_02694POPUP_CONNECT_ERROR_CODE_DESCRIPTION + "\n" + Consts.GetInstance().VERSUS_BACK_TO_TITLE, (object) Singleton<PVPManager>.GetInstance().errorCode);
        Singleton<PVPManager>.GetInstance().errorCode = (string) null;
      }
      else
        str = Consts.GetInstance().ERROR_POPUP_BATTLE_START_MESSAGE;
      string message = str + "\n\n" + Singleton<NGBattleManager>.GetInstance().errorString;
      bool popupWait = true;
      ModalWindow.Show(Consts.GetInstance().ERROR_POPUP_BATTLE_START_TITLE, message, (Action) (() => popupWait = false));
      yield return (object) new WaitWhile((Func<bool>) (() => popupWait));
      StartScript.Restart();
    }
  }
}
