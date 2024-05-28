// Decompiled with JetBrains decompiler
// Type: Unit00493Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit00493Scene : NGSceneBase
{
  public Unit00493Menu menu;

  public static void changeScene(
    bool stack,
    UnitUnit MaterialEvolution,
    bool NewFlag = false,
    bool isGacha = false)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("Unit004_9_3", (stack ? 1 : 0) != 0, (object) MaterialEvolution, (object) NewFlag, (object) isGacha);
  }

  public override IEnumerator onInitSceneAsync()
  {
    yield break;
  }

  public IEnumerator onStartSceneAsync(UnitUnit MaterialEvolution, bool NewFlag, bool isGacha)
  {
    Unit00493Scene unit00493Scene = this;
    Future<GameObject> fBG;
    IEnumerator e;
    if (isGacha)
    {
      RenderSettings.ambientLight = Singleton<NGGameDataManager>.GetInstance().baseAmbientLight;
      fBG = Res.Prefabs.BackGround.GachaTopBackground.Load<GameObject>();
      e = fBG.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit00493Scene.backgroundPrefab = fBG.Result;
      ((UIWidget) unit00493Scene.backgroundPrefab.GetComponent<UI2DSprite>()).color = Consts.GetInstance().GACHA_RESULT_BACKGROUND_COLOR;
      Singleton<PopupManager>.GetInstance().onDismiss();
      fBG = (Future<GameObject>) null;
    }
    else
    {
      fBG = Res.Prefabs.BackGround.UnitBackground.Load<GameObject>();
      e = fBG.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit00493Scene.backgroundPrefab = fBG.Result;
      fBG = (Future<GameObject>) null;
    }
    e = unit00493Scene.menu.Init(MaterialEvolution, NewFlag, isGacha);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
