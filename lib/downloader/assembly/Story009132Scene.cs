// Decompiled with JetBrains decompiler
// Type: Story009132Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Story009132Scene : NGSceneBase
{
  public Story009132Menu menu;
  [SerializeField]
  private NGxScroll ScrollContainer;

  public IEnumerator onStartSceneAsync(PlayerSeaQuestS quest)
  {
    PlayerSeaQuestS[] quests = SMManager.Get<PlayerSeaQuestS[]>().S(quest.quest_sea_s.quest_xl.ID, quest.quest_sea_s.quest_l.ID, quest.quest_sea_s.quest_m.ID);
    this.ScrollContainer.Clear();
    IEnumerator e = this.menu.InitEpisodeButton(quests, quest);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.ScrollContainer.ResolvePosition();
  }
}
