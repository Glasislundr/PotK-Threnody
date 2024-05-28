// Decompiled with JetBrains decompiler
// Type: Title0241Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Title0241Scene : NGSceneBase
{
  [SerializeField]
  private Title0241Menu menu;

  public static void ChangeScene00241(bool stack, string target_player_id)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("title024_1", (stack ? 1 : 0) != 0, (object) target_player_id);
  }

  public IEnumerator onStartSceneAsync(string target_player_id)
  {
    IEnumerator e = this.menu.Init(target_player_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
