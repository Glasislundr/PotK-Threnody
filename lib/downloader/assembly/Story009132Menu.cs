// Decompiled with JetBrains decompiler
// Type: Story009132Menu
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
public class Story009132Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  private NGxScroll ScrollContainer;
  private PlayerSeaQuestS quest;

  public virtual void Foreground()
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
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_13", false);
  }

  public IEnumerator InitEpisodeButton(PlayerSeaQuestS[] quests, PlayerSeaQuestS quest)
  {
    Story009132Menu menu = this;
    menu.TxtTitle.SetTextLocalize(quest.quest_sea_s.quest_m.name);
    menu.quest = quest;
    Array.Reverse((Array) quests);
    Future<GameObject> prefabScrollPartsF = new ResourceObject("Prefabs/Story009_13_2/story009_13_2_button").Load<GameObject>();
    IEnumerator e = prefabScrollPartsF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabScrollPartsF.Result;
    List<StoryPlaybackSeaDetail> storyList = new List<StoryPlaybackSeaDetail>();
    List<Story0093Scene.ContinuousData> continuousDataList = new List<Story0093Scene.ContinuousData>();
    ((IEnumerable<PlayerSeaQuestS>) quests).OrderBy<PlayerSeaQuestS, int>((Func<PlayerSeaQuestS, int>) (x => x.quest_sea_s.ID)).ForEach<PlayerSeaQuestS>((Action<PlayerSeaQuestS>) (q =>
    {
      if (!q.is_clear || q.quest_sea_s.StoryDetails().Length == 0)
        return;
      foreach (StoryPlaybackSeaDetail storyDetail in q.quest_sea_s.StoryDetails())
      {
        storyList.Add(storyDetail);
        continuousDataList.Add(new Story0093Scene.ContinuousData()
        {
          scriptId_ = storyDetail.script_id,
          continuousFlag_ = storyDetail.continuous_flag
        });
      }
    }));
    foreach (StoryPlaybackSeaDetail story in storyList)
    {
      Story009132ScrollParts component = result.CloneAndGetComponent<Story009132ScrollParts>();
      menu.ScrollContainer.Add(((Component) component).gameObject);
      component.Init(menu, story, continuousDataList);
    }
    menu.ScrollContainer.ResolvePosition();
  }
}
