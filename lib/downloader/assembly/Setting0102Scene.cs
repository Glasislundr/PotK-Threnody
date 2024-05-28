// Decompiled with JetBrains decompiler
// Type: Setting0102Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Setting0102Scene : NGSceneBase
{
  [SerializeField]
  private Setting0102Menu menu;

  public override IEnumerator onInitSceneAsync()
  {
    IEnumerator e = this.menu.onInitSceneAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override void onEndScene() => this.menu.onEndScene();
}
