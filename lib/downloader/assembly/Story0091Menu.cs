// Decompiled with JetBrains decompiler
// Type: Story0091Menu
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
public class Story0091Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  private NGxScroll ScrollContainer;
  [SerializeField]
  private GameObject dirNoStory;

  protected virtual void Foreground()
  {
  }

  protected virtual void VScrollBar()
  {
  }

  public override void onBackButton() => this.IbtnBack();

  protected virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_0", false);
  }

  public IEnumerator InitPartButton(PlayerStoryQuestS[] quests, int XL)
  {
    this.dirNoStory.SetActive(quests.Length == 0);
    Array.Reverse((Array) quests);
    Future<GameObject> prefabScrollPartsF = Res.Prefabs.story009_1.vscroll_680_8.Load<GameObject>();
    IEnumerator e = prefabScrollPartsF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefabScrollParts = prefabScrollPartsF.Result;
    ((IEnumerable<PlayerStoryQuestS>) quests).OrderBy<PlayerStoryQuestS, int>((Func<PlayerStoryQuestS, int>) (x => x.quest_story_s.quest_l.priority)).ForEach<PlayerStoryQuestS>((Action<PlayerStoryQuestS>) (q =>
    {
      GameObject gameObject = Object.Instantiate<GameObject>(prefabScrollParts);
      this.ScrollContainer.Add(gameObject);
      gameObject.GetComponent<Story0091ScrollParts>().Init(q, (NGMenuBase) this, XL);
    }));
    this.ScrollContainer.ResolvePosition();
  }
}
