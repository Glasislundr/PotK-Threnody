// Decompiled with JetBrains decompiler
// Type: Story00985Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Story00985Scene : NGSceneBase
{
  [SerializeField]
  private Story00985Menu menu;
  [SerializeField]
  private UILabel TxtTitle;
  [SerializeField]
  private NGxScroll ScrollContainer;

  public static void changeScene(QuestExtraS extra)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_8_5", false, (object) extra.ID, (object) false);
  }

  public static void changeScene(TowerPlaybackStory story)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_8_5", false, (object) story, (object) false);
  }

  public static void changeScene(RaidPlaybackStory story)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_8_5", false, (object) story, (object) false);
  }

  public IEnumerator onStartSceneAsync(int ID, bool listBack)
  {
    this.ScrollContainer.Clear();
    bool flag = false;
    QuestExtraS extra_m = (QuestExtraS) null;
    if (MasterData.QuestExtraS.ContainsKey(ID))
    {
      extra_m = MasterData.QuestExtraS[ID];
      this.TxtTitle.SetTextLocalize(extra_m.quest_m.name);
      flag = true;
    }
    if (flag)
    {
      IEnumerator e = this.menu.InitScene(extra_m, listBack, ID);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.ScrollContainer.ResolvePosition();
  }

  public IEnumerator onStartSceneAsync(TowerPlaybackStory story, bool listBack)
  {
    this.ScrollContainer.Clear();
    if (story != null)
    {
      this.TxtTitle.SetTextLocalize(story.name);
      IEnumerator e = this.menu.InitScene(story, listBack);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.ScrollContainer.ResolvePosition();
  }

  public IEnumerator onStartSceneAsync(RaidPlaybackStory story, bool listBack)
  {
    this.ScrollContainer.Clear();
    if (story != null)
    {
      this.TxtTitle.SetTextLocalize(story.name);
      IEnumerator e = this.menu.InitScene(story, listBack);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.ScrollContainer.ResolvePosition();
  }
}
