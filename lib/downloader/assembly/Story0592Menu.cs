// Decompiled with JetBrains decompiler
// Type: Story0592Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Story0592Menu : BackButtonMenuBase
{
  [SerializeField]
  private UIGrid mGrid;
  [SerializeField]
  private UIScrollView mScrollView;
  private GameObject mScrollParts;

  public IEnumerator InitSceneAsync()
  {
    Future<GameObject> scrollPartsF = Res.Prefabs.story059_2.story0592scroll.Load<GameObject>();
    IEnumerator e = scrollPartsF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mScrollParts = scrollPartsF.Result;
  }

  public IEnumerator StartSceneAsync()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    Story0592Menu.\u003C\u003Ec__DisplayClass4_0 cDisplayClass40 = new Story0592Menu.\u003C\u003Ec__DisplayClass4_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass40.\u003C\u003E4__this = this;
    IEnumerable<Story059ItemData> first = ((IEnumerable<EarthQuestPologue>) MasterData.EarthQuestPologueList).Where<EarthQuestPologue>((Func<EarthQuestPologue, bool>) (x => x.type == "movie")).Select<EarthQuestPologue, Story059ItemData>((Func<EarthQuestPologue, Story059ItemData>) (x => new Story059ItemData(x.episode.ID, Story059ItemData.PlayType.MOVIE, x.arg1)));
    // ISSUE: reference to a compiler-generated field
    cDisplayClass40.quesetProgress = Singleton<EarthDataManager>.GetInstance().questProgress;
    // ISSUE: reference to a compiler-generated method
    IEnumerable<Story059ItemData> story059ItemDatas = ((IEnumerable<EarthQuestEpisode>) MasterData.EarthQuestEpisodeList).Where<EarthQuestEpisode>((Func<EarthQuestEpisode, bool>) (x => x.script.HasValue)).Where<EarthQuestEpisode>(new Func<EarthQuestEpisode, bool>(cDisplayClass40.\u003CStartSceneAsync\u003Eb__3)).Select<EarthQuestEpisode, Story059ItemData>((Func<EarthQuestEpisode, Story059ItemData>) (x => new Story059ItemData(x.ID, Story059ItemData.PlayType.STORY_SCRIPT)));
    // ISSUE: reference to a compiler-generated method
    IEnumerable<Story059ItemData> second1 = ((IEnumerable<EarthQuestStoryPlayback>) MasterData.EarthQuestStoryPlaybackList).Where<EarthQuestStoryPlayback>(new Func<EarthQuestStoryPlayback, bool>(cDisplayClass40.\u003CStartSceneAsync\u003Eb__5)).Select<EarthQuestStoryPlayback, Story059ItemData>((Func<EarthQuestStoryPlayback, Story059ItemData>) (x => new Story059ItemData(x.episode.ID, Story059ItemData.PlayType.STORY_SCRIPT, storyPlaybackID: x.ID)));
    IEnumerable<Story059ItemData> second2 = story059ItemDatas;
    foreach (IGrouping<EarthQuestChapter, Story059ItemData> itemList in first.Concat<Story059ItemData>(second2).Concat<Story059ItemData>(second1).GroupBy<Story059ItemData, EarthQuestChapter>((Func<Story059ItemData, EarthQuestChapter>) (x => x.chaptor)))
      this.mScrollParts.CloneAndGetComponent<Story0592ScrollItem>(((Component) this.mGrid).gameObject).Init(itemList.Key, (IEnumerable<Story059ItemData>) itemList);
    // ISSUE: method pointer
    this.mGrid.onReposition = new UIGrid.OnReposition((object) cDisplayClass40, __methodptr(\u003CStartSceneAsync\u003Eb__7));
    this.mGrid.Reposition();
    yield break;
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }
}
