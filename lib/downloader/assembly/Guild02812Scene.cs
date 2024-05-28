// Decompiled with JetBrains decompiler
// Type: Guild02812Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Guild02812Scene : NGSceneBase
{
  [SerializeField]
  private Guild02812Menu menu;

  public static void ChangeScene()
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("guild028_1_2", true);
  }

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = this.menu.InitializeAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene() => this.menu.Initialize();
}
