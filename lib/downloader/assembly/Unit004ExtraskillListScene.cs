// Decompiled with JetBrains decompiler
// Type: Unit004ExtraskillListScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Unit004ExtraskillListScene : NGSceneBase
{
  [SerializeField]
  private Unit004ExtraskillListMenu menu;

  public static void changeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_extraskill", stack);
  }

  public virtual IEnumerator onStartSceneAsync()
  {
    IEnumerator e = this.menu.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual void onStartScene() => Singleton<CommonRoot>.GetInstance().isLoading = false;

  public override void onEndScene()
  {
  }

  public IEnumerator onBackSceneAsync()
  {
    IEnumerator e = this.menu.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.menu.onBacSceneAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
