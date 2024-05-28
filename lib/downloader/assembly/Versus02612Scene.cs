// Decompiled with JetBrains decompiler
// Type: Versus02612Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Versus02612Scene : NGSceneBase
{
  [SerializeField]
  private Versus02612Menu menu;

  public static void ChangeScene(bool stack, int id, int best_class)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("versus026_12", (stack ? 1 : 0) != 0, (object) id, (object) best_class);
  }

  public IEnumerator onStartSceneAsync(int id, int best_class)
  {
    IEnumerator e = this.menu.Init(id, best_class);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
