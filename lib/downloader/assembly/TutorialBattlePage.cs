// Decompiled with JetBrains decompiler
// Type: TutorialBattlePage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UniLinq;
using UnityEngine;

#nullable disable
public class TutorialBattlePage : TutorialPageBase
{
  [SerializeField]
  private int stageId;
  [SerializeField]
  private int deckId;
  private Dictionary<int, string> turnDict = new Dictionary<int, string>();

  public override IEnumerator Show()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    TutorialBattlePage tutorialBattlePage = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated method
    tutorialBattlePage.StartCoroutine(tutorialBattlePage.\u003C\u003En__0());
    string newchapterBattle1Tutorial = Consts.GetInstance().newchapter_battle1_tutorial;
    tutorialBattlePage.turnDict = tutorialBattlePage.parseTurnMessage(newchapterBattle1Tutorial);
    tutorialBattlePage.StartCoroutine(tutorialBattlePage.startBattle());
    return false;
  }

  private IEnumerator startBattle()
  {
    TutorialBattlePage tutorialBattlePage = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    for (int i = 0; i < 10; ++i)
    {
      Debug.Log((object) ("Wait Battle start...... " + (object) i));
      yield return (object) null;
    }
    List<int> intList = new List<int>();
    foreach (BattleStageGuest battleStageGuest in MasterData.BattleStageGuestList)
    {
      if (battleStageGuest.stage_BattleStage == tutorialBattlePage.stageId)
        intList.Add(battleStageGuest.ID);
    }
    BattleInfo battleInfo = Singleton<NGBattleManager>.GetInstance().battleInfo ?? new BattleInfo();
    battleInfo.quest_type = CommonQuestType.Story;
    battleInfo.battleId = "";
    battleInfo.stageId = tutorialBattlePage.stageId;
    battleInfo.helper = (PlayerHelper) null;
    battleInfo.deckIndex = tutorialBattlePage.deckId;
    battleInfo.guest_ids = intList.ToArray();
    battleInfo.isAutoBattleEnable = false;
    battleInfo.isRetreatEnable = false;
    battleInfo.isStoryEnable = false;
    battleInfo.enemy_items = new Tuple<int, GameCore.Reward>[0];
    battleInfo.playerCallSkillParam = new BattleInfo.CallSkillParam();
    battleInfo.enemyCallSkillParam = new BattleInfo.CallSkillParam();
    IEnumerator e = MasterData.LoadBattleStageEnemy(battleInfo.stage);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = MasterData.LoadBattleMapLandform(battleInfo.stage.map);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    // ISSUE: reference to a compiler-generated method
    battleInfo.enemy_ids = ((IEnumerable<BattleStageEnemy>) MasterData.BattleStageEnemyList).Where<BattleStageEnemy>(new Func<BattleStageEnemy, bool>(tutorialBattlePage.\u003CstartBattle\u003Eb__4_0)).Select<BattleStageEnemy, int>((Func<BattleStageEnemy, int>) (x => x.ID)).ToArray<int>();
    battleInfo.user_enemy_ids = new int[0];
    Singleton<NGBattleManager>.GetInstance().startBattle(battleInfo);
  }

  public void OnPlayerTurnStart(int turn)
  {
    Debug.Log((object) ("turn callback " + (object) turn));
    string message;
    if (!this.turnDict.TryGetValue(turn, out message))
      return;
    this.advice.FinishCallback = (Action) (() => { });
    this.advice.SetMessage(message);
  }

  public void OnBattleFinish() => this.StartCoroutine(this.finishLoop());

  private IEnumerator finishLoop()
  {
    TutorialBattlePage tutorialBattlePage = this;
    NGSceneManager sceneManager = Singleton<NGSceneManager>.GetInstance();
    sceneManager.destroyLoadedScenes();
    sceneManager.changeScene("empty", false);
    while (!sceneManager.isSceneInitialized)
      yield return (object) null;
    Singleton<PopupManager>.GetInstance().closeAll();
    tutorialBattlePage.NextPage();
  }

  private Dictionary<int, string> parseTurnMessage(string message)
  {
    int key = -1;
    Dictionary<int, string> turnMessage = new Dictionary<int, string>();
    List<string> self = new List<string>();
    Regex regex = new Regex("#turn (\\d+)");
    string str = message;
    char[] chArray = new char[1]{ '\n' };
    foreach (string input in str.Split(chArray))
    {
      if (input.StartsWith("#"))
      {
        Match match = regex.Match(input);
        if (match.Success)
        {
          if (key >= 0)
          {
            turnMessage[key] = self.Join("\n");
            self.Clear();
          }
          key = int.Parse(match.Groups[1].Value);
        }
        else
          self.Add(input);
      }
      else
        self.Add(input);
    }
    turnMessage[key] = self.Join("\n");
    foreach (KeyValuePair<int, string> keyValuePair in turnMessage)
      Debug.Log((object) (keyValuePair.Key.ToString() + ": " + keyValuePair.Value));
    return turnMessage;
  }
}
