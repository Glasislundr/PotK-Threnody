// Decompiled with JetBrains decompiler
// Type: Story00913Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Story00913Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  private NGxScroll ScrollContainer;
  [SerializeField]
  private GameObject dirNoStory;
  private PlayerStoryQuestS backQuest;

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
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_0", false);
  }

  public IEnumerator InitChapterButton(PlayerSeaQuestS[] quests)
  {
    Array.Reverse((Array) quests);
    this.dirNoStory.SetActive(quests.Length == 0);
    Future<GameObject> prefabScrollPartsF = new ResourceObject("Prefabs/Story009_13/story009_13_button").Load<GameObject>();
    IEnumerator e = prefabScrollPartsF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefabScrollParts = prefabScrollPartsF.Result;
    ((IEnumerable<PlayerSeaQuestS>) quests).OrderBy<PlayerSeaQuestS, int>((Func<PlayerSeaQuestS, int>) (x => x.quest_sea_s.ID)).ForEach<PlayerSeaQuestS>((Action<PlayerSeaQuestS>) (q =>
    {
      if (!q.is_clear || q.quest_sea_s.StoryDetails().Length == 0)
        return;
      GameObject gameObject = Object.Instantiate<GameObject>(prefabScrollParts);
      this.ScrollContainer.Add(gameObject);
      gameObject.GetComponent<Story00913ScrollParts>().Init(this, q);
    }));
    this.ScrollContainer.ResolvePosition();
  }
}
