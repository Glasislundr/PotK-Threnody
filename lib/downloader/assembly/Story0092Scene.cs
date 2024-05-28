// Decompiled with JetBrains decompiler
// Type: Story0092Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Story0092Scene : NGSceneBase
{
  public Story0092Menu menu;
  public UILabel TxtTitle;
  [SerializeField]
  private NGxScroll ScrollContainer;

  public IEnumerator onStartSceneAsync(PlayerStoryQuestS quest, int XL)
  {
    this.TxtTitle.SetTextLocalize(quest.quest_story_s.quest_l.short_name + "　" + quest.quest_story_s.quest_l.name);
    PlayerStoryQuestS[] self = SMManager.Get<PlayerStoryQuestS[]>();
    this.ScrollContainer.Clear();
    IEnumerator e = this.menu.InitChapterButton(quest, self.DisplayScrollM(quest.quest_story_s.quest_xl.ID, quest.quest_story_s.quest_l.ID), XL);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.ScrollContainer.ResolvePosition();
  }
}
