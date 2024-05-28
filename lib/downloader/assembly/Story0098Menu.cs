// Decompiled with JetBrains decompiler
// Type: Story0098Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Story0098Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  private NGxScroll scroll;
  [SerializeField]
  private GameObject dirNoStory;

  public virtual void Foreground()
  {
  }

  public virtual void IbtnEvent()
  {
  }

  public virtual void VScrollBar()
  {
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_0", false);
  }

  public IEnumerator InitScene(bool connect)
  {
    Story0098Menu story0098Menu = this;
    NGGameDataManager gameDataManager = Singleton<NGGameDataManager>.GetInstance();
    story0098Menu.scroll.Clear();
    IEnumerator e;
    if (connect || gameDataManager.clearedExtraQuestSIds == null || gameDataManager.clearedTowerStageIds == null || gameDataManager.clearedRaidQuestSIds == null)
    {
      // ISSUE: reference to a compiler-generated method
      Future<WebAPI.Response.QuestHistoryExtra> ft = WebAPI.QuestHistoryExtra(new Action<WebAPI.Response.UserError>(story0098Menu.\u003CInitScene\u003Eb__8_0));
      e = ft.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (ft.Result == null)
      {
        yield break;
      }
      else
      {
        gameDataManager.clearedExtraQuestSIds = ft.Result.cleared_quest_s_ids;
        gameDataManager.clearedTowerStageIds = ft.Result.cleared_tower_stage_ids;
        gameDataManager.clearedRaidQuestSIds = ft.Result.cleared_guildraid_quest_s_ids;
        ft = (Future<WebAPI.Response.QuestHistoryExtra>) null;
      }
    }
    e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<GameObject> ScrollPrefab = Res.Prefabs.story009_8.story0098scroll.Load<GameObject>();
    e = ScrollPrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefab = ScrollPrefab.Result;
    QuestExtraS[] eventlist = story0098Menu.getValidExtraStoryData();
    QuestExtraS[] questExtraSArray = eventlist;
    int index;
    for (index = 0; index < questExtraSArray.Length; ++index)
    {
      QuestExtraS extra = questExtraSArray[index];
      e = story0098Menu.ScrollInit(extra, prefab);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    questExtraSArray = (QuestExtraS[]) null;
    story0098Menu.dirNoStory.SetActive(eventlist.Length == 0);
    TowerPlaybackStory[] towerPlaybackStoryArray = story0098Menu.getValidTowerStoryData();
    for (index = 0; index < towerPlaybackStoryArray.Length; ++index)
    {
      TowerPlaybackStory story = towerPlaybackStoryArray[index];
      e = story0098Menu.ScrollInit(story.stage, story, prefab);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    towerPlaybackStoryArray = (TowerPlaybackStory[]) null;
    RaidPlaybackStory[] raidPlaybackStoryArray = story0098Menu.getValidRaidStoryData();
    for (index = 0; index < raidPlaybackStoryArray.Length; ++index)
    {
      RaidPlaybackStory story = raidPlaybackStoryArray[index];
      e = story0098Menu.ScrollInit(story, prefab);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    raidPlaybackStoryArray = (RaidPlaybackStory[]) null;
    story0098Menu.scroll.ResolvePosition();
  }

  private QuestExtraS[] getValidExtraStoryData()
  {
    int[] clearedIDs = Singleton<NGGameDataManager>.GetInstance().clearedExtraQuestSIds;
    return ((IEnumerable<StoryPlaybackExtra>) MasterData.StoryPlaybackExtraList.DisplayList(ServerTime.NowAppTime())).Where<StoryPlaybackExtra>((Func<StoryPlaybackExtra, bool>) (x => ((IEnumerable<int>) clearedIDs).Contains<int>(x.quest.ID))).Distinct<StoryPlaybackExtra>((IEqualityComparer<StoryPlaybackExtra>) new LambdaEqualityComparer<StoryPlaybackExtra>((Func<StoryPlaybackExtra, StoryPlaybackExtra, bool>) ((a, b) =>
    {
      QuestExtraS quest1 = a.quest;
      QuestExtraS quest2 = b.quest;
      if (!(quest1.seek_index == quest2.seek_index))
        return false;
      return quest1.seek_type != QuestExtra.SeekType.L ? quest1.quest_m_QuestExtraM == quest2.quest_m_QuestExtraM : quest1.quest_l_QuestExtraL == quest2.quest_l_QuestExtraL;
    }))).OrderBy<StoryPlaybackExtra, int>((Func<StoryPlaybackExtra, int>) (x => x.quest.quest_m.priority)).Select<StoryPlaybackExtra, QuestExtraS>((Func<StoryPlaybackExtra, QuestExtraS>) (x => x.quest)).ToArray<QuestExtraS>();
  }

  private TowerPlaybackStory[] getValidTowerStoryData()
  {
    int[] clearedTowerStageIDs = Singleton<NGGameDataManager>.GetInstance().clearedTowerStageIds;
    return ((IEnumerable<TowerPlaybackStory>) MasterData.TowerPlaybackStoryList).Where<TowerPlaybackStory>((Func<TowerPlaybackStory, bool>) (x => ((IEnumerable<int>) clearedTowerStageIDs).Contains<int>(x.stage.stage_id))).OrderBy<TowerPlaybackStory, int>((Func<TowerPlaybackStory, int>) (x => x.priority)).ToArray<TowerPlaybackStory>();
  }

  private RaidPlaybackStory[] getValidRaidStoryData()
  {
    int[] clearedIds = Singleton<NGGameDataManager>.GetInstance().clearedRaidQuestSIds;
    HashSet<int> raidPeriodIds = new HashSet<int>(((IEnumerable<GuildRaid>) MasterData.GuildRaidList).Where<GuildRaid>((Func<GuildRaid, bool>) (x => ((IEnumerable<int>) clearedIds).Contains<int>(x.ID))).Select<GuildRaid, int>((Func<GuildRaid, int>) (x => x.period_id)));
    return ((IEnumerable<RaidPlaybackStory>) MasterData.RaidPlaybackStoryList).Where<RaidPlaybackStory>((Func<RaidPlaybackStory, bool>) (x =>
    {
      if (x.display_expire_at.HasValue)
      {
        DateTime? displayExpireAt = x.display_expire_at;
        DateTime dateTime = ServerTime.NowAppTime();
        if ((displayExpireAt.HasValue ? (displayExpireAt.GetValueOrDefault() > dateTime ? 1 : 0) : 0) == 0)
          return false;
      }
      return raidPeriodIds.Contains(x.period_id);
    })).OrderBy<RaidPlaybackStory, int>((Func<RaidPlaybackStory, int>) (x => x.priority)).ToArray<RaidPlaybackStory>();
  }

  public IEnumerator ScrollInit(QuestExtraS extra, GameObject prefab)
  {
    GameObject scrollObj = Object.Instantiate<GameObject>(prefab);
    scrollObj.transform.parent = ((Component) this.scroll.grid).transform;
    scrollObj.transform.localScale = Vector3.one;
    scrollObj.transform.localPosition = Vector3.zero;
    IEnumerator e = scrollObj.GetComponent<Story0098Scroll>().InitScroll(extra);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (extra.seek_type == QuestExtra.SeekType.L)
      EventDelegate.Set(scrollObj.GetComponent<Story0098Scroll>().BtnFormation.onClick, (EventDelegate.Callback) (() => this.changeScene00983(extra)));
    else if (extra.seek_type == QuestExtra.SeekType.M)
      EventDelegate.Set(scrollObj.GetComponent<Story0098Scroll>().BtnFormation.onClick, (EventDelegate.Callback) (() => this.changeScene00985(extra)));
  }

  public IEnumerator ScrollInit(TowerStage tower, TowerPlaybackStory story, GameObject prefab)
  {
    GameObject scrollObj = Object.Instantiate<GameObject>(prefab);
    scrollObj.transform.parent = ((Component) this.scroll.grid).transform;
    scrollObj.transform.localScale = Vector3.one;
    scrollObj.transform.localPosition = Vector3.zero;
    IEnumerator e = scrollObj.GetComponent<Story0098Scroll>().InitScroll(story);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    EventDelegate.Set(scrollObj.GetComponent<Story0098Scroll>().BtnFormation.onClick, (EventDelegate.Callback) (() => this.changeScene00985(tower, story)));
  }

  public IEnumerator ScrollInit(RaidPlaybackStory story, GameObject prefab)
  {
    GameObject scrollObj = Object.Instantiate<GameObject>(prefab);
    scrollObj.transform.parent = ((Component) this.scroll.grid).transform;
    scrollObj.transform.localScale = Vector3.one;
    scrollObj.transform.localPosition = Vector3.zero;
    IEnumerator e = scrollObj.GetComponent<Story0098Scroll>().InitScroll(story);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    EventDelegate.Set(scrollObj.GetComponent<Story0098Scroll>().BtnFormation.onClick, (EventDelegate.Callback) (() => this.changeScene00985(story)));
  }

  public void changeScene00983(QuestExtraS extra)
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_8_3", false, (object) extra.ID);
  }

  public void changeScene00985(QuestExtraS extra)
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
    Story00985Scene.changeScene(extra);
  }

  public void changeScene00985(TowerStage tower, TowerPlaybackStory story)
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
    Story00985Scene.changeScene(story);
  }

  public void changeScene00985(RaidPlaybackStory story)
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
    Story00985Scene.changeScene(story);
  }
}
