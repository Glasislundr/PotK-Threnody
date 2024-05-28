// Decompiled with JetBrains decompiler
// Type: Battle01961Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Battle01961Scene : NGSceneBase
{
  [SerializeField]
  private Battle01961Menu menu;

  public IEnumerator onStartSceneAsync(BL.UnitPosition attack, BL.UnitPosition defense)
  {
    Battle01961Scene battle01961Scene = this;
    IEnumerator e = battle01961Scene.menu.Init(attack, defense);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isActiveBackground3DCamera = false;
    e = ((Component) battle01961Scene).gameObject.GetComponent<AttachSEtoButtonOnBattle>().AddSEtoButton();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Battle019_6_1_RecoveryButton[] componentsInChildren = ((Component) battle01961Scene).GetComponentsInChildren<Battle019_6_1_RecoveryButton>(true);
    if (componentsInChildren.Length != 0)
      componentsInChildren[0].setUseTarget(attack.unit, defense.unit);
  }

  public override void onEndScene()
  {
    Singleton<CommonRoot>.GetInstance().isActiveBackground3DCamera = true;
  }
}
