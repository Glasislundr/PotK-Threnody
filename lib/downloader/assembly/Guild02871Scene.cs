// Decompiled with JetBrains decompiler
// Type: Guild02871Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Guild02871Scene : NGSceneBase
{
  [SerializeField]
  private Guild02871Menu menu;

  public static void ChangeScene(bool stack = true)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("guild028_7_1", stack);
  }

  public IEnumerator onStartSceneAsync()
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    IEnumerator e = this.menu.InitializeAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene() => Singleton<CommonRoot>.GetInstance().HideLoadingLayer();

  public override void onEndScene()
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    DetailController.Release();
  }
}
