// Decompiled with JetBrains decompiler
// Type: Quest002271Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Quest002271Scene : NGSceneBase
{
  [SerializeField]
  private Quest002271Menu menu;

  public static void ChangeScene(
    bool stack,
    QuestScoreCampaignProgress progress,
    string title,
    int rank)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_27_1", (stack ? 1 : 0) != 0, (object) progress, (object) title, (object) rank);
  }

  public IEnumerator onStartSceneAsync(QuestScoreCampaignProgress progress, string title, int rank)
  {
    IEnumerator e = this.menu.Initialize(progress, title, rank);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
