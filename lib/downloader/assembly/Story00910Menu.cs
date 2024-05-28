// Decompiled with JetBrains decompiler
// Type: Story00910Menu
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
using UnityEngine;

#nullable disable
public class Story00910Menu : BackButtonMenuBase
{
  private PlayerHarmonyQuestM[] harmoniesM;
  private PlayerHarmonyQuestS[] harmoniesS;
  private GameObject combiPrefab;
  private GameObject trioPrefab;
  private GameObject unitIconPrefab;
  [SerializeField]
  private NGxScroll ngxScroll;
  [SerializeField]
  private GameObject dirNoStory;

  protected virtual void Foreground()
  {
  }

  protected virtual void VScrollBar()
  {
  }

  public override void onBackButton() => this.IbtnBack();

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_0", false);
  }

  private IEnumerator API()
  {
    Story00910Menu story00910Menu = this;
    string KeyWebAPI = "QuestProgressHarmony";
    if (!WebAPI.IsResponsedAtContainsKey(KeyWebAPI))
    {
      story00910Menu.harmoniesM = (PlayerHarmonyQuestM[]) null;
      story00910Menu.harmoniesS = (PlayerHarmonyQuestS[]) null;
      // ISSUE: reference to a compiler-generated method
      Future<WebAPI.Response.QuestProgressHarmony> response = WebAPI.QuestProgressHarmony(new Action<WebAPI.Response.UserError>(story00910Menu.\u003CAPI\u003Eb__11_0));
      IEnumerator e = response.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (response.Result != null)
      {
        story00910Menu.harmoniesM = response.Result.harmonies;
        story00910Menu.harmoniesS = response.Result.player_harmony_quests;
        WebAPI.SetLatestResponsedAt(KeyWebAPI);
        response = (Future<WebAPI.Response.QuestProgressHarmony>) null;
      }
    }
    else
    {
      story00910Menu.harmoniesM = SMManager.Get<PlayerHarmonyQuestM[]>();
      story00910Menu.harmoniesS = SMManager.Get<PlayerHarmonyQuestS[]>();
    }
  }

  private IEnumerator UsePrefab()
  {
    this.combiPrefab = (GameObject) null;
    Future<GameObject> obj = Res.Prefabs.quest002_14.dir_Conbi.Load<GameObject>();
    IEnumerator e = obj.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.combiPrefab = obj.Result;
    this.trioPrefab = (GameObject) null;
    Future<GameObject> trioPrefabF = Res.Prefabs.quest002_14.dir_Trio.Load<GameObject>();
    e = trioPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.trioPrefab = trioPrefabF.Result;
    this.unitIconPrefab = (GameObject) null;
    Future<GameObject> unitIconPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    e = unitIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unitIconPrefab = unitIconPrefabF.Result;
  }

  private IEnumerator CreateList()
  {
    Story00910Menu story00910Menu = this;
    story00910Menu.DeleteList();
    ((Component) story00910Menu.ngxScroll.scrollView).gameObject.SetActive(false);
    PlayerHarmonyQuestM[] playerHarmonyQuestMArray = story00910Menu.harmoniesM;
    for (int index = 0; index < playerHarmonyQuestMArray.Length; ++index)
    {
      PlayerHarmonyQuestM harmony = playerHarmonyQuestMArray[index];
      if (harmony.is_playable)
      {
        QuestHarmonyS questSTable = ((IEnumerable<QuestHarmonyS>) MasterData.QuestHarmonySList).Where<QuestHarmonyS>((Func<QuestHarmonyS, bool>) (x => x.quest_m_QuestHarmonyM == harmony.quest_m_id.ID)).FirstOrDefault<QuestHarmonyS>();
        IEnumerator e;
        if (questSTable != null)
        {
          if (questSTable.target_unit2 == null)
          {
            GameObject gameObject = story00910Menu.combiPrefab.Clone();
            story00910Menu.ngxScroll.Add(gameObject);
            e = gameObject.GetComponent<Quest00214DirCombi>().InitStoryPlayBack(questSTable, story00910Menu.harmoniesS, story00910Menu.unitIconPrefab, new Action<int, int>(story00910Menu.SelectHarmony));
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
          else
          {
            GameObject gameObject = story00910Menu.trioPrefab.Clone();
            story00910Menu.ngxScroll.Add(gameObject);
            e = gameObject.GetComponent<Quest00214DirTrio>().InitStoryPlayBack(questSTable, new Action<int, int[], int>(story00910Menu.SelectHarmonyTrio));
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
        }
      }
    }
    playerHarmonyQuestMArray = (PlayerHarmonyQuestM[]) null;
    ((Component) story00910Menu.ngxScroll.scrollView).gameObject.SetActive(true);
    story00910Menu.dirNoStory.SetActive(((Component) story00910Menu.ngxScroll.grid).transform.childCount <= 0);
    story00910Menu.ngxScroll.ResolvePosition();
  }

  private bool isOpenQuest(
    QuestHarmonyReleaseCondition[] releaseCondirions,
    PlayerHarmonyQuestS playerQuestS)
  {
    return ((IEnumerable<QuestHarmonyReleaseCondition>) releaseCondirions).Where<QuestHarmonyReleaseCondition>((Func<QuestHarmonyReleaseCondition, bool>) (x => x.quest_s_QuestHarmonyS == playerQuestS.quest_harmony_s.ID)).FirstOrDefault<QuestHarmonyReleaseCondition>().required_intimacy_level <= this.GetCharacterIntimate(playerQuestS.quest_harmony_s.unit_UnitUnit, playerQuestS.quest_harmony_s.target_unit_UnitUnit);
  }

  private int GetCharacterIntimate(int mainUnitId, int targetUnitId)
  {
    UnitUnit mainUnit = MasterData.UnitUnit[mainUnitId];
    UnitUnit targetUnit = MasterData.UnitUnit[targetUnitId];
    PlayerCharacterIntimate characterIntimate = ((IEnumerable<PlayerCharacterIntimate>) SMManager.Get<PlayerCharacterIntimate[]>()).FirstOrDefault<PlayerCharacterIntimate>((Func<PlayerCharacterIntimate, bool>) (x =>
    {
      if (x._character == mainUnit.character_UnitCharacter && x._target_character == targetUnit.character_UnitCharacter)
        return true;
      return x._character == targetUnit.character_UnitCharacter && x._target_character == mainUnit.character_UnitCharacter;
    }));
    return characterIntimate != null ? characterIntimate.level : 0;
  }

  private void DeleteList() => this.ngxScroll.Clear();

  public IEnumerator InitScene()
  {
    IEnumerator e = this.API();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (this.harmoniesM != null)
    {
      e = this.UsePrefab();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = this.CreateList();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public void EndScene() => this.DeleteList();

  private void SelectHarmony(int unitId, int targetUnitId)
  {
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_11", true, (object) unitId, (object) targetUnitId);
  }

  private void SelectHarmonyTrio(int unitId, int[] targetUnitIds, int questSId)
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_12", true, (object) unitId, (object) targetUnitIds[0], (object) targetUnitIds[1], (object) questSId);
  }
}
