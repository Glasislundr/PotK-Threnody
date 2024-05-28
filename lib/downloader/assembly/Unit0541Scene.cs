// Decompiled with JetBrains decompiler
// Type: Unit0541Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Unit0541Scene : NGSceneBase
{
  [SerializeField]
  private Unit0541Menu menu;

  public static void ChangeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit054_1", false);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e = this.menu.InitSceneAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = this.menu.StartSceneAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene() => Singleton<CommonRoot>.GetInstance().isLoading = false;

  public override void onEndScene()
  {
  }
}
