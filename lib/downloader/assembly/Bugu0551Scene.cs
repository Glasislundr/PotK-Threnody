// Decompiled with JetBrains decompiler
// Type: Bugu0551Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Bugu0551Scene : NGSceneBase
{
  [SerializeField]
  private Bugu0551Menu menu;

  public static void ChangeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu055_1", stack);
  }

  public override IEnumerator onInitSceneAsync()
  {
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

  public void onStartScene()
  {
  }

  public override void onEndScene()
  {
  }
}
