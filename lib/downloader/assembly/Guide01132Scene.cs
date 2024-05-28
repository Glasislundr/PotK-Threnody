// Decompiled with JetBrains decompiler
// Type: Guide01132Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Guide01132Scene : NGSceneBase
{
  public Guide01132Menu menu;
  public bool one;

  public override IEnumerator onInitSceneAsync()
  {
    Guide01132Scene guide01132Scene = this;
    Future<GameObject> bgF = (Future<GameObject>) null;
    bgF = !PerformanceConfig.GetInstance().IsSpeedPriority ? new ResourceObject("Prefabs/BackGround/UnitBackground_anim").Load<GameObject>() : new ResourceObject("Prefabs/BackGround/UnitBackground_anim_no_effect").Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    guide01132Scene.backgroundPrefab = bgF.Result;
  }

  public IEnumerator onStartSceneAsync(UnitUnit unit)
  {
    if (!this.one)
    {
      IEnumerator e = this.menu.onStartSceneAsync(unit, true);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.one = true;
    }
  }

  public void onStartScene(UnitUnit unit) => this.StartCoroutine(this.HideTipsLoading());

  private IEnumerator HideTipsLoading()
  {
    yield return (object) new WaitForSeconds(0.1f);
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }
}
