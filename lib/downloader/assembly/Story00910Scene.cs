// Decompiled with JetBrains decompiler
// Type: Story00910Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Story00910Scene : NGSceneBase
{
  [SerializeField]
  private Story00910Menu menu;

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = this.menu.InitScene();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override void onEndScene() => this.menu.EndScene();
}
