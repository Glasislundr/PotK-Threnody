// Decompiled with JetBrains decompiler
// Type: LumpToutaResultScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Unit/LumpTouta/ResultScene")]
public class LumpToutaResultScene : NGSceneBase
{
  [SerializeField]
  private LumpToutaResultMenu menu;

  public override IEnumerator onInitSceneAsync()
  {
    LumpToutaResultScene toutaResultScene = this;
    Future<GameObject> bgF = Res.Prefabs.BackGround.UnitBackground_60.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    toutaResultScene.backgroundPrefab = bgF.Result;
  }

  public IEnumerator onStartSceneAsync(
    List<Unit004832Menu.ResultPlayerUnit> resultPlayerUnits,
    int incrementMedal,
    GainTrustResult[] gainTrustResults,
    UnlockQuest[] unlockQuests)
  {
    yield return (object) this.menu.Init(resultPlayerUnits, incrementMedal, gainTrustResults, unlockQuests);
  }

  public void onStartScene(
    List<Unit004832Menu.ResultPlayerUnit> resultPlayerUnits,
    int incrementMedal,
    GainTrustResult[] gainTrustResults,
    UnlockQuest[] unlockQuests)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    this.menu.OnStartScene();
  }
}
