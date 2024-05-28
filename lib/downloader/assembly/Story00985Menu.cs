// Decompiled with JetBrains decompiler
// Type: Story00985Menu
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
public class Story00985Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtChapter;
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  private NGxScroll ScrollContainer;
  private bool listBack;
  private int quest_id;
  private GameObject scrollPartsPrefab;

  public virtual void Foreground()
  {
  }

  public virtual void IbtnChapter05()
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
    if (this.listBack)
      Singleton<NGSceneManager>.GetInstance().changeScene("story009_8_3", false, (object) this.quest_id);
    else
      Singleton<NGSceneManager>.GetInstance().changeScene("story009_8", false, (object) false);
  }

  private IEnumerator loadPrefab()
  {
    Future<GameObject> prefabScrollPartsF = Res.Prefabs.story009_8_5.story00985scroll.Load<GameObject>();
    IEnumerator e = prefabScrollPartsF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.scrollPartsPrefab = prefabScrollPartsF.Result;
  }

  public IEnumerator InitScene(QuestExtraS extra_m, bool listBack, int id)
  {
    this.listBack = listBack;
    this.quest_id = id;
    IEnumerator e = this.loadPrefab();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    int[] clearedIDs = Singleton<NGGameDataManager>.GetInstance().clearedExtraQuestSIds;
    List<QuestExtraS> list = ((IEnumerable<StoryPlaybackExtra>) MasterData.StoryPlaybackExtraList.DisplayList(ServerTime.NowAppTime())).Where<StoryPlaybackExtra>((Func<StoryPlaybackExtra, bool>) (x => x.quest.quest_m.ID == extra_m.quest_m.ID && ((IEnumerable<int>) clearedIDs).Contains<int>(x.quest.ID))).OrderBy<StoryPlaybackExtra, int>((Func<StoryPlaybackExtra, int>) (x => x.quest.priority)).Select<StoryPlaybackExtra, QuestExtraS>((Func<StoryPlaybackExtra, QuestExtraS>) (x => x.quest)).ToList<QuestExtraS>();
    List<Story0093Scene.ContinuousData> continuousDataList = new List<Story0093Scene.ContinuousData>();
    List<StoryPlaybackExtraDetail> detailList = new List<StoryPlaybackExtraDetail>();
    foreach (QuestExtraS questExtraS in list)
    {
      QuestExtraS extraData = questExtraS;
      ((IEnumerable<StoryPlaybackExtraDetail>) MasterData.StoryPlaybackExtraDetailList).ForEach<StoryPlaybackExtraDetail>((Action<StoryPlaybackExtraDetail>) (detail =>
      {
        if (detail.quest != extraData || detail.timing == StoryPlaybackTiming.located_player_unit)
          return;
        detailList.Add(detail);
        continuousDataList.Add(new Story0093Scene.ContinuousData()
        {
          scriptId_ = detail.script_id,
          continuousFlag_ = detail.continuous_flag
        });
      }));
    }
    foreach (StoryPlaybackExtraDetail playbackExtraDetail in detailList)
      this.ScrollInit(playbackExtraDetail.name, playbackExtraDetail.script_id, continuousDataList);
    this.ScrollContainer.ResolvePosition();
  }

  public IEnumerator InitScene(TowerPlaybackStory story, bool listBack)
  {
    this.listBack = listBack;
    IEnumerator e = this.loadPrefab();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    int[] clearedTowerStageIds = Singleton<NGGameDataManager>.GetInstance().clearedTowerStageIds;
    foreach (TowerPlaybackStoryDetail playbackStoryDetail in ((IEnumerable<TowerPlaybackStoryDetail>) MasterData.TowerPlaybackStoryDetailList).Where<TowerPlaybackStoryDetail>((Func<TowerPlaybackStoryDetail, bool>) (x => x.story.ID == story.ID)).ToArray<TowerPlaybackStoryDetail>())
    {
      TowerPlaybackStoryDetail detail = playbackStoryDetail;
      if (((IEnumerable<int>) clearedTowerStageIds).Any<int>((Func<int, bool>) (x => x == detail.stage.stage_id)))
        this.ScrollInit(detail.name, detail.script_id, (List<Story0093Scene.ContinuousData>) null);
      else
        break;
    }
    this.ScrollContainer.ResolvePosition();
  }

  public IEnumerator InitScene(RaidPlaybackStory story, bool listBack)
  {
    this.listBack = listBack;
    IEnumerator e = this.loadPrefab();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    int[] clearedIds = Singleton<NGGameDataManager>.GetInstance().clearedRaidQuestSIds;
    int[] clearedStageIds = ((IEnumerable<GuildRaid>) MasterData.GuildRaidList).Where<GuildRaid>((Func<GuildRaid, bool>) (x => x.period_id == story.period_id && ((IEnumerable<int>) clearedIds).Contains<int>(x.ID))).Select<GuildRaid, int>((Func<GuildRaid, int>) (x => x.stage_id)).ToArray<int>();
    foreach (StoryPlaybackRaidDetail playbackRaidDetail in ((IEnumerable<StoryPlaybackRaidDetail>) MasterData.StoryPlaybackRaidDetailList).Where<StoryPlaybackRaidDetail>((Func<StoryPlaybackRaidDetail, bool>) (x => ((IEnumerable<int>) clearedStageIds).Contains<int>(x.stage_id))).ToArray<StoryPlaybackRaidDetail>())
      this.ScrollInit(playbackRaidDetail.name, playbackRaidDetail.script_id, (List<Story0093Scene.ContinuousData>) null);
    this.ScrollContainer.ResolvePosition();
  }

  public void ScrollInit(
    string name,
    int script_id,
    List<Story0093Scene.ContinuousData> continuousDataList)
  {
    GameObject gameObject = Object.Instantiate<GameObject>(this.scrollPartsPrefab);
    gameObject.GetComponent<Story00985Scroll>().Init(name, script_id, (NGMenuBase) this, continuousDataList);
    this.ScrollContainer.Add(gameObject);
  }
}
