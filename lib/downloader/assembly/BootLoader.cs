// Decompiled with JetBrains decompiler
// Type: BootLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using Gsc.Auth;
using System;
using System.Collections;
using System.IO;
using UniLinq;
using UnityEngine;

#nullable disable
public class BootLoader : MonoBehaviour
{
  public static BootLoader Lunch()
  {
    BootLoader orAddComponent = GameObject.Find("DontDestroyObject").GetOrAddComponent<BootLoader>();
    orAddComponent.StartBoot();
    return orAddComponent;
  }

  public bool End { get; private set; }

  private void StartBoot()
  {
    this.End = false;
    this.StopCoroutine("LunchCore");
    this.StartCoroutine("LunchCore");
  }

  private IEnumerator LunchCore()
  {
    string file = StorageUtil.persistentDataPath + "/" + Revision.ApplicationVersion;
    if (!File.Exists(file))
    {
      yield return (object) this.CleanupDuringBattle();
      File.WriteAllText(file, "");
    }
    Future<WebAPI.Response.PlayerBootRelease> f = WebAPI.PlayerBoot();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    WebAPI.Response.PlayerBootRelease boot = f.Result;
    if (boot.is_end_service)
    {
      Consts instance = Consts.GetInstance();
      string message = "";
      switch (Device.Platform)
      {
        case "appstore":
          message = instance.END_IOS_TEXT;
          break;
        case "googleplay":
          message = instance.END_ANDROID_TEXT;
          break;
        case "dmmgamesstore":
          message = instance.END_WINDOWS_TEXT;
          break;
        case "aumarket":
          message = instance.END_AU_GAME_TEXT;
          break;
      }
      ModalWindow.Show("", message, (Action) (() => StartScript.Restart()));
    }
    else
    {
      BootLoader.BootSequence sequence = new BootLoader.BootSequence(boot);
      e = sequence.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (sequence.isCancelContinueBattle)
      {
        if (boot.player_during_tower_battle)
        {
          int battle_turn = 0;
          int[] result_enemy_kill_count = new int[0];
          try
          {
            if (Persist.battleEnvironment.Exists)
            {
              if (Persist.battleEnvironment.Data.core != null)
              {
                battle_turn = Persist.battleEnvironment.Data.core.phaseState.turnCount;
                result_enemy_kill_count = Persist.battleEnvironment.Data.core.enemyUnits.value.Select<BL.Unit, int>((Func<BL.Unit, int>) (x => x.killCount)).ToArray<int>();
              }
            }
          }
          catch (Exception ex)
          {
            battle_turn = 0;
            result_enemy_kill_count = new int[0];
            Debug.LogException(ex);
          }
          e = WebAPI.TowerBattleRetire(battle_turn, result_enemy_kill_count).Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          boot.player_during_tower_battle = false;
        }
        else if (boot.player_during_corps_battle)
        {
          e = WebAPI.QuestCorpsBattleForceClose().Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          boot.player_during_corps_battle = false;
        }
        else
        {
          int continue_count = 0;
          try
          {
            if (Persist.battleEnvironment.Exists && Persist.battleEnvironment.Data.core != null)
              continue_count = Persist.battleEnvironment.Data.core.continueCount;
            continue_count = Mathf.Max(continue_count, WebAPI.LastPlayerBoot.continue_count);
          }
          catch (Exception ex)
          {
            Debug.LogException(ex);
          }
          e = WebAPI.BattleRetire(continue_count).Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          boot.player_during_battle = false;
          boot.player_during_sea_battle = false;
          boot.player_during_raid_battle = false;
        }
        Persist.cacheInfo.Delete();
        Persist.battleEnvironment.Delete();
      }
      this.End = true;
    }
  }

  private IEnumerator CleanupDuringBattle()
  {
    Future<WebAPI.Response.PlayerBootRelease> bootF = WebAPI.PlayerBoot();
    IEnumerator e = bootF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (bootF.Result.player_during_battle || bootF.Result.player_during_sea_battle || bootF.Result.player_during_raid_battle)
    {
      e = WebAPI.BattleForceClose().Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Persist.battleEnvironment.Delete();
    }
    else if (bootF.Result.player_during_tower_battle)
    {
      e = WebAPI.TowerBattleForceClose().Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Persist.battleEnvironment.Delete();
    }
    else if (bootF.Result.player_during_corps_battle)
    {
      e = WebAPI.QuestCorpsBattleForceClose().Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Persist.battleEnvironment.Delete();
    }
    else if (bootF.Result.player_during_pvp)
    {
      if (string.IsNullOrEmpty(bootF.Result.pvp_token))
      {
        e = WebAPI.PvpNpcForceClose(true).Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        e = WebAPI.PvpForceClose(true).Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    Persist.pvpSuspend.Delete();
    Persist.earthBattleEnvironment.Delete();
  }

  private class BootSequence
  {
    private readonly WebAPI.Response.PlayerBootRelease boot;
    private BootLoader.BootSequence.SequenceStatus status;

    public bool isCancelContinueBattle { get; private set; }

    public BootSequence(WebAPI.Response.PlayerBootRelease boot_)
    {
      this.boot = boot_;
      this.isCancelContinueBattle = false;
    }

    public IEnumerator Wait()
    {
      Consts c = Consts.GetInstance();
      StartupDownLoad.SetLastestVersion(this.boot.dlc_latest_version);
      bool flag = false;
      try
      {
        flag = Persist.cacheInfo.Data.hasDeleted;
      }
      catch
      {
        Persist.cacheInfo.Delete();
      }
      IEnumerator e;
      if (flag)
      {
        if (this.boot.player_during_battle || this.boot.player_during_sea_battle || this.boot.player_during_raid_battle || this.boot.player_during_tower_battle || this.boot.player_during_corps_battle)
        {
          this.isCancelContinueBattle = true;
          this.status = BootLoader.BootSequence.SequenceStatus.Noop;
        }
        else if (Persist.battleEnvironment.Exists)
          Persist.battleEnvironment.Delete();
        if (this.boot.player_during_pvp)
        {
          e = this.PvPClose();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        if (Persist.cacheInfo.Exists)
          Persist.cacheInfo.Delete();
      }
      else if (this.boot.player_during_battle || this.boot.player_during_sea_battle)
      {
        do
        {
          if (this.boot.player_during_battle && NGGameDataManager.SeaChangeFlag)
            NGGameDataManager.SeaChangeFlag = false;
          this.validatePersistBattleUuid();
          this.status = BootLoader.BootSequence.SequenceStatus.Wait;
          this.procLocalBattleResumeConfirmPopup(true);
          while (this.status == BootLoader.BootSequence.SequenceStatus.Wait)
            yield return (object) null;
        }
        while (this.status == BootLoader.BootSequence.SequenceStatus.Confirm);
        int status = (int) this.status;
      }
      else if (this.boot.player_during_tower_battle || this.boot.player_during_raid_battle || this.boot.player_during_corps_battle)
      {
        do
        {
          NGGameDataManager.SeaChangeFlag = false;
          this.validatePersistBattleUuid();
          this.status = BootLoader.BootSequence.SequenceStatus.Wait;
          this.procLocalBattleResumeConfirmPopup(false);
          while (this.status == BootLoader.BootSequence.SequenceStatus.Wait)
            yield return (object) null;
        }
        while (this.status == BootLoader.BootSequence.SequenceStatus.Confirm);
        int status = (int) this.status;
      }
      else if (this.boot.player_during_pvp)
      {
        do
        {
          NGGameDataManager.SeaChangeFlag = false;
          this.status = BootLoader.BootSequence.SequenceStatus.Wait;
          ModalWindow.ShowYesNo(c.boot_continue_pvp_title, c.boot_continue_pvp_text, (Action) (() => this.status = BootLoader.BootSequence.SequenceStatus.Break), (Action) (() => ModalWindow.ShowYesNo(c.boot_continue_pvp_exit_confirm_title, c.boot_continue_pvp_exit_confirm_text, (Action) (() => ModalWindow.Show(c.boot_continue_pvp_exit_done_title, c.boot_continue_pvp_exit_done_text, (Action) (() => this.status = BootLoader.BootSequence.SequenceStatus.Noop))), (Action) (() => this.status = BootLoader.BootSequence.SequenceStatus.Confirm))));
          while (this.status == BootLoader.BootSequence.SequenceStatus.Wait)
            yield return (object) null;
        }
        while (this.status == BootLoader.BootSequence.SequenceStatus.Confirm);
        if (this.status != BootLoader.BootSequence.SequenceStatus.Break)
        {
          e = this.PvPClose();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
      else if (this.boot.player_during_pvp_result)
      {
        NGGameDataManager.SeaChangeFlag = false;
        this.status = BootLoader.BootSequence.SequenceStatus.Wait;
        ModalWindow.Show(c.boot_continue_pvp_result_title, c.boot_continue_pvp_result_text, (Action) (() => this.status = BootLoader.BootSequence.SequenceStatus.Noop));
        while (this.status == BootLoader.BootSequence.SequenceStatus.Wait)
          yield return (object) null;
      }
      else if (this.boot.player_during_sea_date)
      {
        do
        {
          this.status = BootLoader.BootSequence.SequenceStatus.Wait;
          ModalWindow.ShowYesNo(c.boot_continue_date_title, c.boot_continue_date_text, (Action) (() => this.status = BootLoader.BootSequence.SequenceStatus.Break), (Action) (() => ModalWindow.ShowYesNo(c.boot_continue_date_exit_confirm_title, c.boot_continue_date_exit_confirm_text, (Action) (() => ModalWindow.Show(c.boot_continue_date_exit_done_title, c.boot_continue_date_exit_done_text, (Action) (() => this.status = BootLoader.BootSequence.SequenceStatus.Noop))), (Action) (() => this.status = BootLoader.BootSequence.SequenceStatus.Confirm))));
          while (this.status == BootLoader.BootSequence.SequenceStatus.Wait)
            yield return (object) null;
        }
        while (this.status == BootLoader.BootSequence.SequenceStatus.Confirm);
        if (this.status != BootLoader.BootSequence.SequenceStatus.Break)
        {
          e = this.DateClose();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
      else if (NGGameDataManager.SeaChangeFlag && !this.boot.sea_is_opened)
      {
        NGGameDataManager.SeaChangeFlag = false;
        this.status = BootLoader.BootSequence.SequenceStatus.Wait;
        ModalWindow.Show(c.boot_sea_not_open_confirm_title, c.boot_sea_not_open_confirm_text, (Action) (() => this.status = BootLoader.BootSequence.SequenceStatus.Noop));
        while (this.status == BootLoader.BootSequence.SequenceStatus.Wait)
          yield return (object) null;
      }
      else if (this.boot.during_retry_gacha)
      {
        NGGameDataManager.SeaChangeFlag = false;
        this.status = BootLoader.BootSequence.SequenceStatus.Wait;
        ModalWindow.Show(c.GACHA_NOT_END_TITLE, c.GACHA_NOT_END_DESCRIPTION, (Action) (() => this.status = BootLoader.BootSequence.SequenceStatus.Noop));
        while (this.status == BootLoader.BootSequence.SequenceStatus.Wait)
          yield return (object) null;
      }
    }

    private void validatePersistBattleUuid()
    {
      if (!Persist.battleEnvironment.Exists)
        return;
      try
      {
        string battleId = Persist.battleEnvironment.Data.core.battleInfo.battleId;
        if (string.IsNullOrEmpty(this.boot.battle_uuid) || battleId.Equals(this.boot.battle_uuid))
          return;
        Persist.battleEnvironment.Delete();
      }
      catch
      {
        Persist.battleEnvironment.Delete();
      }
    }

    private void procLocalBattleResumeConfirmPopup(bool enableContinue)
    {
      Consts instance = Consts.GetInstance();
      string resumeTitle = enableContinue ? instance.boot_continue_battle_title : instance.TOWER_BATTLE_RESUME_TITLE;
      string str1;
      if (!enableContinue)
        str1 = instance.TOWER_BATTLE_RESUME_DESC;
      else
        str1 = Consts.Format(instance.boot_continue_battle_text, (IDictionary) new Hashtable()
        {
          {
            (object) "count",
            (object) this.boot.continue_count
          }
        });
      string message = str1;
      string str2;
      if (!enableContinue)
        str2 = instance.TOWER_BATTLE_RESUME_NO_DESC2;
      else
        str2 = Consts.Format(instance.boot_continue_battle_text2, (IDictionary) new Hashtable()
        {
          {
            (object) "count",
            (object) this.boot.continue_count
          }
        });
      string resumeMsg2 = str2;
      string retireTitle = enableContinue ? instance.boot_continue_battle_exit_confirm_title : instance.TOWER_BATTLE_RESUME_NO_TITLE;
      string str3;
      if (!enableContinue)
        str3 = instance.TOWER_BATTLE_RESUME_NO_DESC;
      else
        str3 = Consts.Format(instance.boot_continue_battle_exit_confirm_text, (IDictionary) new Hashtable()
        {
          {
            (object) "count",
            (object) this.boot.continue_count
          }
        });
      string retireMsg = str3;
      string retireTitle2 = enableContinue ? instance.boot_continue_battle_exit_done_title : instance.TOWER_BATTLE_RETIRE_COMPLETE_TITLE;
      string retireMsg2 = enableContinue ? instance.boot_continue_battle_exit_done_text : instance.TOWER_BATTLE_RETIRE_COMPLETE_DESC;
      ModalWindow.ShowYesNo(resumeTitle, message, (Action) (() =>
      {
        if (Persist.battleEnvironment.Exists)
          this.status = BootLoader.BootSequence.SequenceStatus.Break;
        else
          ModalWindow.ShowYesNo(resumeTitle, resumeMsg2, (Action) (() => this.status = BootLoader.BootSequence.SequenceStatus.Break), (Action) (() => this.status = BootLoader.BootSequence.SequenceStatus.Confirm));
      }), (Action) (() => ModalWindow.ShowYesNo(retireTitle, retireMsg, (Action) (() => ModalWindow.Show(retireTitle2, retireMsg2, (Action) (() =>
      {
        this.isCancelContinueBattle = true;
        this.status = BootLoader.BootSequence.SequenceStatus.Noop;
      }))), (Action) (() => this.status = BootLoader.BootSequence.SequenceStatus.Confirm))));
    }

    private IEnumerator PvPClose()
    {
      this.boot.player_during_pvp = false;
      IEnumerator e;
      if (string.IsNullOrEmpty(this.boot.pvp_token))
      {
        e = WebAPI.PvpNpcForceClose(true).Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        e = WebAPI.PvpForceClose(true).Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      Persist.pvpSuspend.Delete();
    }

    private IEnumerator DateClose()
    {
      this.boot.player_during_sea_date = false;
      IEnumerator e = WebAPI.SeaDateForceClose().Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }

    private enum SequenceStatus
    {
      Noop,
      Wait,
      Break,
      Confirm,
    }
  }
}
