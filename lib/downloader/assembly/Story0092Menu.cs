// Decompiled with JetBrains decompiler
// Type: Story0092Menu
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
public class Story0092Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  private NGxScroll ScrollContainer;
  private PlayerStoryQuestS backQuest;
  private int XL;

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
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_1", false, (object) this.XL);
  }

  public IEnumerator InitChapterButton(PlayerStoryQuestS quest, PlayerStoryQuestS[] quests, int XL)
  {
    this.XL = XL;
    Array.Reverse((Array) quests);
    Future<GameObject> prefabScrollPartsF = Res.Prefabs.story009_2.story009_2_button.Load<GameObject>();
    IEnumerator e = prefabScrollPartsF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefabScrollParts = prefabScrollPartsF.Result;
    ((IEnumerable<PlayerStoryQuestS>) quests).OrderBy<PlayerStoryQuestS, int>((Func<PlayerStoryQuestS, int>) (x => x.quest_story_s.ID)).ForEach<PlayerStoryQuestS>((Action<PlayerStoryQuestS>) (q =>
    {
      GameObject gameObject = Object.Instantiate<GameObject>(prefabScrollParts);
      this.ScrollContainer.Add(gameObject);
      gameObject.GetComponent<Story0092ScrollParts>().Init(this, q, quest, XL);
    }));
    this.ScrollContainer.ResolvePosition();
  }
}
