// Decompiled with JetBrains decompiler
// Type: Gacha00611Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Gacha00611Scene : NGSceneBase
{
  private Gacha00611Menu menu;
  private bool isPlay;

  public static void changeScene(
    bool stack,
    bool newflag,
    int countWeapon,
    GameCore.ItemInfo revTargetData,
    int addReisouJewel)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("gacha006_11", (stack ? 1 : 0) != 0, (object) newflag, (object) countWeapon, (object) revTargetData, (object) addReisouJewel);
  }

  public static void ChangeScene(
    bool stack,
    bool newFlag,
    int countWeapon,
    GameCore.ItemInfo TargetData,
    GameCore.ItemInfo baseData,
    PlayerItem TargetReisouInfo,
    PlayerItem baseReisouInfo,
    Action backSceneCallback,
    bool showEquipUnit,
    int addReisouJewel)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("gacha006_11", (stack ? 1 : 0) != 0, (object) newFlag, (object) countWeapon, (object) TargetData, (object) baseData, (object) TargetReisouInfo, (object) baseReisouInfo, (object) backSceneCallback, (object) true, (object) addReisouJewel);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Gacha00611Scene gacha00611Scene = this;
    Future<GameObject> bgF = Res.Prefabs.BackGround.GachaTopBackground.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    gacha00611Scene.backgroundPrefab = bgF.Result;
    ((UIWidget) gacha00611Scene.backgroundPrefab.GetComponent<UI2DSprite>()).color = Consts.GetInstance().GACHA_RESULT_BACKGROUND_COLOR;
  }

  public IEnumerator onStartSceneAsync(bool NewFlag, GameCore.ItemInfo TargetData)
  {
    IEnumerator e = this.onStartSceneAsync(NewFlag, 0, TargetData, 0);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    bool NewFlag,
    int countWeapon,
    GameCore.ItemInfo TargetData,
    int addReisouJewel)
  {
    IEnumerator e = this.onStartSceneAsync(NewFlag, countWeapon, TargetData, (GameCore.ItemInfo) null, (PlayerItem) null, (PlayerItem) null, (Action) null, false, addReisouJewel);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    bool NewFlag,
    GameCore.ItemInfo TargetData,
    GameCore.ItemInfo baseData,
    PlayerItem TargetReisouInfo,
    PlayerItem baseReisouInfo,
    Action backSceneCallback,
    int addReisouJewel)
  {
    IEnumerator e = this.onStartSceneAsync(NewFlag, 0, TargetData, baseData, TargetReisouInfo, baseReisouInfo, backSceneCallback, false, addReisouJewel);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    bool NewFlag,
    int countWeapon,
    GameCore.ItemInfo TargetData,
    GameCore.ItemInfo baseData,
    PlayerItem TargetReisouInfo,
    PlayerItem baseReisouInfo,
    Action backSceneCallback,
    bool showEquipUnit,
    int addReisouJewel)
  {
    Gacha00611Scene gacha00611Scene = this;
    RenderSettings.ambientLight = Singleton<NGGameDataManager>.GetInstance().baseAmbientLight;
    IEnumerator e;
    if (Object.op_Equality((Object) gacha00611Scene.menu, (Object) null))
    {
      Future<GameObject> handler = Res.Prefabs.gacha006_11.MainPanel.Load<GameObject>();
      e = handler.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      gacha00611Scene.menu = handler.Result.Clone(((Component) gacha00611Scene).transform).GetComponent<Gacha00611Menu>();
      ((UIRect) ((Component) gacha00611Scene.menu).GetComponent<UIPanel>()).SetAnchor(((Component) gacha00611Scene).transform);
      handler = (Future<GameObject>) null;
    }
    gacha00611Scene.menuBase = (NGMenuBase) gacha00611Scene.menu;
    gacha00611Scene.menuBase.IsPush = false;
    e = gacha00611Scene.menu.Initialize(TargetData, NewFlag, countWeapon, baseInfo: baseData, TargetReisouInfo: TargetReisouInfo, baseReisouInfo: baseReisouInfo, showEquipUnit: showEquipUnit, addReisouJewel: new int?(addReisouJewel));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    gacha00611Scene.menu.SetChangeScene(backSceneCallback);
    ((Component) gacha00611Scene.menu.BackSceneButton).gameObject.SetActive(true);
  }

  public void onStartScene() => Singleton<PopupManager>.GetInstance().closeAll();

  public void onStartScene(bool NewFlag, GameCore.ItemInfo TargetData)
  {
    Singleton<PopupManager>.GetInstance().closeAll();
  }

  public void onStartScene(bool NewFlag, int countWeapon, GameCore.ItemInfo TargetData, int addReisouJewel)
  {
    Singleton<PopupManager>.GetInstance().closeAll();
  }

  public void onStartScene(
    bool NewFlag,
    GameCore.ItemInfo TargetData,
    GameCore.ItemInfo baseData,
    PlayerItem TargetReisouInfo,
    PlayerItem baseReisouInfo,
    Action backSceneCallback,
    int addReisouJewel)
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    this.isPlay = true;
  }

  public void onStartScene(
    bool NewFlag,
    int countWeapon,
    GameCore.ItemInfo TargetData,
    GameCore.ItemInfo baseData,
    PlayerItem TargetReisouInfo,
    PlayerItem baseReisouInfo,
    Action backSceneCallback,
    bool showEquipUnit,
    int addReisouJewel)
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    this.isPlay = true;
  }

  public override void onSceneInitialized()
  {
    if (this.isPlay)
      this.menu.StartAnime();
    this.StartCoroutine(this.menu.PlayGetReisouJewel());
  }

  public override void onEndScene() => this.menu.StopAnime();
}
