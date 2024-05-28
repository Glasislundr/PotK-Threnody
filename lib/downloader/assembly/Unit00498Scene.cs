// Decompiled with JetBrains decompiler
// Type: Unit00498Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Unit00498Scene : NGSceneBase
{
  [SerializeField]
  private Unit00498Menu menu;
  private GameObject background_;

  public static void changeSceneByJobChange(
    bool stack,
    PlayerUnit beforeUnit,
    PlayerUnit afterUnit)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_JobChange_Complete", (stack ? 1 : 0) != 0, (object) beforeUnit, (object) afterUnit, (object) Unit00499Scene.Mode.JobChange, (object) false);
  }

  public static void changeScene(
    bool stack,
    PlayerUnit basePlayerUnit,
    PlayerUnit resultPlayerUnit,
    Unit00499Scene.Mode mode,
    bool fromEarth)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_9_8", (stack ? 1 : 0) != 0, (object) basePlayerUnit, (object) resultPlayerUnit, (object) fromEarth);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Unit00498Scene unit00498Scene = this;
    Future<GameObject> bgF = Res.Prefabs.BackGround.UnitBackground_60.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00498Scene.backgroundPrefab = bgF.Result;
  }

  public void onStartScene(
    PlayerUnit basePlayerUnit,
    PlayerUnit resultPlayerUnit,
    Unit00499Scene.Mode mode,
    bool fromEarth)
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    this.StartCoroutine(this.CharacterStory());
  }

  public IEnumerator onStartSceneAsync(
    PlayerUnit basePlayerUnit,
    PlayerUnit resultPlayerUnit,
    Unit00499Scene.Mode mode,
    bool fromEarth)
  {
    yield return (object) this.setBackground(mode);
    IEnumerator e = this.menu.setCharacter(basePlayerUnit, resultPlayerUnit, mode);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (fromEarth)
      Singleton<CommonRoot>.GetInstance().EnableEarthHeader();
    Singleton<PopupManager>.GetInstance().closeAll();
  }

  private IEnumerator setBackground(Unit00499Scene.Mode mode)
  {
    Unit00498Scene unit00498Scene = this;
    if (Object.op_Equality((Object) unit00498Scene.background_, (Object) null) && mode == Unit00499Scene.Mode.JobChange)
    {
      Future<GameObject> ldPrefab = new ResourceObject("Prefabs/BackGround/UnitBackground_jobChange_Result").Load<GameObject>();
      yield return (object) ldPrefab.Wait();
      unit00498Scene.background_ = ldPrefab.Result;
      ldPrefab = (Future<GameObject>) null;
    }
    if (Object.op_Inequality((Object) unit00498Scene.background_, (Object) null))
      unit00498Scene.backgroundPrefab = unit00498Scene.background_;
  }

  public IEnumerator CharacterStory()
  {
    IEnumerator e = UnitEvolutionResultData.GetInstance().CharacterStoryPopup();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void TutorialAdvice()
  {
    this.endTweens();
    this.StartCoroutine(this.Advice());
  }

  public IEnumerator Advice()
  {
    Unit00498Scene unit00498Scene = this;
    while (((IEnumerable<UITweener>) unit00498Scene.tweens).Any<UITweener>((Func<UITweener, bool>) (x => ((Component) x).gameObject.activeInHierarchy && x.tweenGroup == 11 && ((Behaviour) x).enabled)))
      yield return (object) null;
    Singleton<TutorialRoot>.GetInstance().CurrentAdvise();
  }
}
