// Decompiled with JetBrains decompiler
// Type: Raid032BattleResultScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Raid032BattleResultScene : NGSceneBase
{
  [SerializeField]
  private GameObject touchToNext;
  [SerializeField]
  private GameObject txt_SimulatedBattle_result;
  private List<IRaidResultMenu> sequences;
  private IRaidResultMenu currentMenu;
  private bool isSurvive;
  private int quest_s_id;
  private bool isSimulation;
  private GuildRaid masterData;

  public static void ChangeScene(BattleInfo info, WebAPI.Response.GuildraidBattleFinish result)
  {
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    instance.clearStack();
    instance.destroyCurrentScene();
    instance.destroyLoadedScenes();
    instance.changeScene("raid032_result", false, (object) info, (object) result);
  }

  public IEnumerator onStartSceneAsync(
    BattleInfo info,
    WebAPI.Response.GuildraidBattleFinish result)
  {
    Raid032BattleResultScene battleResultScene = this;
    battleResultScene.isSimulation = info.isSimulation;
    battleResultScene.quest_s_id = battleResultScene.isSimulation ? info.quest_s_id : result.quest_s_id;
    if (!MasterData.GuildRaid.TryGetValue(battleResultScene.quest_s_id, out battleResultScene.masterData))
    {
      Debug.LogError((object) ("There is no MasterData in local [ID:" + (object) battleResultScene.quest_s_id + "]"));
    }
    else
    {
      PlayerUnit bossUnit = (PlayerUnit) null;
      yield return (object) battleResultScene.LoadBossUnit(battleResultScene.quest_s_id, result.loop_count, (Action<PlayerUnit>) (u => bossUnit = u));
      battleResultScene.isSurvive = !result.already_defeated && bossUnit.hp.initial > result.boss_total_damage;
      battleResultScene.sequences = new List<IRaidResultMenu>();
      battleResultScene.sequences.Add((IRaidResultMenu) ((Component) battleResultScene).GetComponent<Raid032BattleResultMenu>());
      if (((result.damage_rewards == null ? 0 : (result.damage_rewards.Length != 0 ? 1 : 0)) | (result.defeat_rewards == null ? (false ? 1 : 0) : (result.defeat_rewards.Length != 0 ? 1 : 0))) != 0)
        battleResultScene.sequences.Add((IRaidResultMenu) ((Component) battleResultScene).GetComponent<Raid032BattleResultRewardPopupMenu>());
      IEnumerator e = battleResultScene.InitMenus(battleResultScene.masterData, info, result, bossUnit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private IEnumerator LoadBossUnit(int raid_quest_s_id, int loopCount, Action<PlayerUnit> complete)
  {
    yield return (object) MasterData.LoadBattleStageEnemy(MasterData.BattleStage[this.masterData.stage_id]);
    complete(PlayerUnit.FromEnemy(this.masterData.getBoss(), raidLoopCount: loopCount, raidID: raid_quest_s_id, isRaidBoss: true));
  }

  public void onStartScene(BattleInfo info, WebAPI.Response.GuildraidBattleFinish result)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    this.StartCoroutine(this.RunMenus(info, result));
  }

  public override void onEndScene() => this.sequences.Clear();

  private IEnumerator InitMenus(
    GuildRaid masterData,
    BattleInfo info,
    WebAPI.Response.GuildraidBattleFinish result,
    PlayerUnit bossUnit)
  {
    this.touchToNext.SetActive(false);
    foreach (IRaidResultMenu sequence in this.sequences)
    {
      if (sequence != null)
      {
        IEnumerator e = sequence.Init(masterData, info, result, bossUnit);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  private IEnumerator RunMenus(BattleInfo info, WebAPI.Response.GuildraidBattleFinish result)
  {
    Raid032BattleResultScene battleResultScene = this;
    battleResultScene.txt_SimulatedBattle_result.SetActive(battleResultScene.isSimulation);
    if (result.already_defeated)
    {
      bool next = false;
      battleResultScene.StartCoroutine(PopupCommon.Show(Consts.GetInstance().GUILD_RAID_RP_BACK_TITLE, Consts.GetInstance().GUILD_RAID_RP_BACK_MESSAGE, (Action) (() => next = true)));
      while (!next)
        yield return (object) null;
    }
    List<IRaidResultMenu>.Enumerator seqe = battleResultScene.sequences.GetEnumerator();
    IEnumerator e;
    while (seqe.MoveNext())
    {
      battleResultScene.currentMenu = seqe.Current;
      if (battleResultScene.currentMenu != null)
      {
        battleResultScene.touchToNext.SetActive(true);
        e = battleResultScene.currentMenu.Run();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        while (!battleResultScene.currentMenu.isSkip)
          yield return (object) null;
        e = battleResultScene.currentMenu.OnFinish();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
        break;
    }
    battleResultScene.currentMenu = (IRaidResultMenu) null;
    battleResultScene.touchToNext.SetActive(true);
    ((Collider) battleResultScene.touchToNext.GetComponent<BoxCollider>()).enabled = false;
    while (seqe.MoveNext())
    {
      e = seqe.Current.Run();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
    if (battleResultScene.isSurvive || battleResultScene.isSimulation)
      Raid032BattleScene.changeScene(false, result.loop_count, battleResultScene.quest_s_id, battleResultScene.isSimulation, true);
    else
      RaidTopScene.ChangeSceneBattleFinish();
  }

  public void IbtnTouchToNext()
  {
    if (this.currentMenu == null)
      return;
    this.currentMenu.isSkip = true;
  }

  private class BackButtonToastBehaviour : BackButtonMenuBase
  {
    public override void onBackButton() => this.showBackKeyToast();
  }
}
