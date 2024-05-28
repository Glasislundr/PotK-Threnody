// Decompiled with JetBrains decompiler
// Type: Story0592ScrollItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Story0592ScrollItem : MonoBehaviour
{
  [SerializeField]
  private UILabel mTitleLable;
  private EarthQuestChapter mChapter;
  private List<Story059ItemData> mItemList;

  public void Init(EarthQuestChapter chapter, IEnumerable<Story059ItemData> itemList)
  {
    this.mChapter = chapter;
    this.mTitleLable.SetTextLocalize("{0}\n{1}".F((object) chapter.chapter, (object) chapter.chapter_name));
    this.mItemList = itemList.OrderBy<Story059ItemData, int>((Func<Story059ItemData, int>) (x => x.episode.stage_index)).ThenBy<Story059ItemData, int>((Func<Story059ItemData, int>) (x => x.playType != Story059ItemData.PlayType.MOVIE ? 1 : 0)).ThenBy<Story059ItemData, int>((Func<Story059ItemData, int>) (x =>
    {
      if (x.storyPlayback == null)
        return 0;
      switch (x.storyPlayback.timing)
      {
        case StoryPlaybackTiming.before_battle:
          return 1;
        case StoryPlaybackTiming.located_player_unit:
          return 2;
        case StoryPlaybackTiming.after_battle:
          return 99;
        case StoryPlaybackTiming.turn_init:
          return 3;
        default:
          return 4;
      }
    })).ToList<Story059ItemData>();
  }

  public void onChapterClick() => Story0593Scene.ChangeScene(true, this.mChapter, this.mItemList);
}
