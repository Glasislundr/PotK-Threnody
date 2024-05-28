// Decompiled with JetBrains decompiler
// Type: Story00983Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Story00983Scene : NGSceneBase
{
  [SerializeField]
  private Story00983Menu menu;
  [SerializeField]
  private UILabel TxtTitle;
  [SerializeField]
  private NGxScroll ScrollContainer;

  public IEnumerator onStartSceneAsync(int ID)
  {
    this.ScrollContainer.Clear();
    bool flag = false;
    QuestExtraS extra = (QuestExtraS) null;
    if (MasterData.QuestExtraS.ContainsKey(ID))
    {
      extra = MasterData.QuestExtraS[ID];
      this.TxtTitle.SetTextLocalize(extra.quest_l.name);
      flag = true;
    }
    if (flag)
    {
      IEnumerator e = this.menu.InitScene(extra);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.ScrollContainer.ResolvePosition();
  }
}
