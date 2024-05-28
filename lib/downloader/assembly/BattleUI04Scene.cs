// Decompiled with JetBrains decompiler
// Type: BattleUI04Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class BattleUI04Scene : BattleSceneBase
{
  [SerializeField]
  private BattleUI04Menu menu;

  public override IEnumerator onInitSceneAsync()
  {
    IEnumerator e = this.menu.LoadPrefab();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(BL.UnitPosition attack, BL.UnitPosition defense)
  {
    BattleUI04Scene battleUi04Scene = this;
    IEnumerator e = battleUi04Scene.menu.Init(attack, defense);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isActiveBackground3DCamera = false;
    e = ((Component) battleUi04Scene).gameObject.GetComponent<AttachSEtoButtonOnBattle>().AddSEtoButton();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override void onSceneInitialized()
  {
    base.onSceneInitialized();
    this.menu.onInitialized();
  }

  public override void onEndScene()
  {
    this.menu.onEndScene();
    Singleton<CommonRoot>.GetInstance().isActiveBackground3DCamera = true;
  }
}
