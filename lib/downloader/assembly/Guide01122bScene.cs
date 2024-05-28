// Decompiled with JetBrains decompiler
// Type: Guide01122bScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Guide01122bScene : NGSceneBase
{
  public Guide01122bMenu menu;

  public override IEnumerator onInitSceneAsync()
  {
    Guide01122bScene guide01122bScene = this;
    Future<GameObject> bgF = (Future<GameObject>) null;
    bgF = !PerformanceConfig.GetInstance().IsSpeedPriority ? new ResourceObject("Prefabs/BackGround/UnitBackground_anim").Load<GameObject>() : new ResourceObject("Prefabs/BackGround/UnitBackground_anim_no_effect").Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    guide01122bScene.backgroundPrefab = bgF.Result;
  }

  public IEnumerator onStartSceneAsync(UnitUnit unit)
  {
    IEnumerator e = this.menu.Init(unit, false, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.menu.SetNumber(unit);
  }
}
