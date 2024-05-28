// Decompiled with JetBrains decompiler
// Type: GachaPickupSelectScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Gacha/PickupSelect/Scene")]
public class GachaPickupSelectScene : NGSceneBase
{
  private static readonly string defName = "gacha006_UnitSelect";
  private GachaPickupSelectMenu menu_;

  public static void changeScene(bool bStack, GachaModuleGacha module, Action callbackChanged)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(GachaPickupSelectScene.defName, (bStack ? 1 : 0) != 0, (object) module, (object) callbackChanged);
  }

  public override IEnumerator onInitSceneAsync()
  {
    GachaPickupSelectScene pickupSelectScene = this;
    pickupSelectScene.menu_ = pickupSelectScene.menuBase as GachaPickupSelectMenu;
    IEnumerator e = pickupSelectScene.menu_.doLoadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(GachaModuleGacha module, Action callbackChanged)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    IEnumerator e = this.menu_.onStartSceneAsync(module, callbackChanged);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene(GachaModuleGacha module, Action callbackChanged)
  {
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public IEnumerator onBackSceneAsync(GachaModuleGacha module, Action callbackChanged)
  {
    yield break;
  }

  public void onBackScene(GachaModuleGacha module, Action callbackChanged)
  {
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }
}
