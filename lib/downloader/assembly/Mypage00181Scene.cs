// Decompiled with JetBrains decompiler
// Type: Mypage00181Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Mypage00181Scene : NGSceneBase
{
  [SerializeField]
  private Mypage00181Menu menu;

  public Mypage00181Menu Menu
  {
    get => this.menu;
    set => this.menu = value;
  }

  public override IEnumerator onInitSceneAsync()
  {
    IEnumerator e = this.Menu.onInitMenuAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync()
  {
    if (!this.Menu.initFlag)
    {
      IEnumerator e = this.Menu.onStartMenuAsync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public void onStartScene()
  {
    if (this.Menu.initFlag)
      return;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }
}
