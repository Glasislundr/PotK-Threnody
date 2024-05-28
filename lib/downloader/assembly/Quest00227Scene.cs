// Decompiled with JetBrains decompiler
// Type: Quest00227Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Quest00227Scene : NGSceneBase
{
  [SerializeField]
  private Quest00227Menu menu;

  public static void ChangeScene(QuestScoreCampaignProgress qscp, bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_27", (stack ? 1 : 0) != 0, (object) qscp);
  }

  public IEnumerator onStartSceneAsync(QuestScoreCampaignProgress qscp)
  {
    IEnumerator e = this.menu.Initialize(qscp);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
