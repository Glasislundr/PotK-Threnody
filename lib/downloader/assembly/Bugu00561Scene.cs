// Decompiled with JetBrains decompiler
// Type: Bugu00561Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Bugu00561Scene : NGSceneBase
{
  public Bugu00561Menu menu;

  public static void changeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_6_1", stack);
  }

  public static void changeScene(bool stack, ItemInfo item)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_6_1", (stack ? 1 : 0) != 0, (object) item);
  }

  public static void changeScene(bool stack, ItemInfo item, bool isNew, bool isScreenTouch)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_6_1", (stack ? 1 : 0) != 0, (object) item, (object) isNew, (object) isScreenTouch);
  }

  public static void changeScene(
    bool stack,
    ItemInfo item,
    bool isNew,
    bool isScreenTouch,
    bool isGacha)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_6_1", (stack ? 1 : 0) != 0, (object) item, (object) isNew, (object) isScreenTouch, (object) isGacha);
  }

  public static void changeScene(
    bool stack,
    ItemInfo item,
    bool isNew,
    bool isScreenTouch,
    bool isGacha,
    int counter)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_6_1", (stack ? 1 : 0) != 0, (object) item, (object) isNew, (object) isScreenTouch, (object) isGacha, (object) counter);
  }

  public static void changeScene(bool stack, ItemInfo item, ItemInfo[] items, int index)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_6_1", (stack ? 1 : 0) != 0, (object) item, (object) items, (object) index);
  }

  public IEnumerator onStartSceneAsync(ItemInfo item)
  {
    Bugu00561Scene bugu00561Scene = this;
    if (Singleton<NGGameDataManager>.GetInstance().IsEarth)
      bugu00561Scene.bgmName = "bgm104";
    else
      bugu00561Scene.headerType = CommonRoot.HeaderType.Normal;
    IEnumerator coroutine = bugu00561Scene.startSceneAsync(item, false, false, false, 0);
    while (coroutine.MoveNext())
      yield return coroutine.Current;
  }

  public IEnumerator onStartSceneAsync(ItemInfo item, bool isNew = false, bool isScreenTouch = false)
  {
    Bugu00561Scene bugu00561Scene = this;
    if (Singleton<NGGameDataManager>.GetInstance().IsEarth)
      bugu00561Scene.bgmName = "bgm104";
    else
      bugu00561Scene.headerType = CommonRoot.HeaderType.Normal;
    IEnumerator coroutine = bugu00561Scene.startSceneAsync(item, isNew, isScreenTouch, false, 0);
    while (coroutine.MoveNext())
      yield return coroutine.Current;
  }

  public IEnumerator onStartSceneAsync(
    ItemInfo item,
    bool isNew,
    bool isScreenTouch,
    bool isGacha)
  {
    Bugu00561Scene bugu00561Scene = this;
    if (Singleton<NGGameDataManager>.GetInstance().IsEarth)
      bugu00561Scene.bgmName = "bgm104";
    else
      bugu00561Scene.headerType = CommonRoot.HeaderType.Normal;
    IEnumerator coroutine = bugu00561Scene.startSceneAsync(item, isNew, isScreenTouch, isGacha, 0);
    while (coroutine.MoveNext())
      yield return coroutine.Current;
  }

  public IEnumerator onStartSceneAsync(
    ItemInfo item,
    bool isNew,
    bool isScreenTouch,
    bool isGacha,
    int counter)
  {
    Bugu00561Scene bugu00561Scene = this;
    if (Singleton<NGGameDataManager>.GetInstance().IsEarth)
      bugu00561Scene.bgmName = "bgm104";
    else
      bugu00561Scene.headerType = CommonRoot.HeaderType.Normal;
    IEnumerator coroutine = bugu00561Scene.startSceneAsync(item, isNew, isScreenTouch, isGacha, counter);
    while (coroutine.MoveNext())
      yield return coroutine.Current;
  }

  public IEnumerator startSceneAsync(
    ItemInfo item,
    bool isNew,
    bool isScreenTouch,
    bool isGacha,
    int counter)
  {
    Bugu00561Scene bugu00561Scene = this;
    if (Singleton<NGGameDataManager>.GetInstance().IsEarth)
      bugu00561Scene.bgmName = "bgm104";
    else
      bugu00561Scene.headerType = CommonRoot.HeaderType.Normal;
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
      bugu00561Scene.backgroundPrefab = fBG.Result;
      ((UIWidget) bugu00561Scene.backgroundPrefab.GetComponent<UI2DSprite>()).color = Consts.GetInstance().GACHA_RESULT_BACKGROUND_COLOR;
      Singleton<PopupManager>.GetInstance().onDismiss();
      fBG = (Future<GameObject>) null;
    }
    else
    {
      fBG = Res.Prefabs.BackGround.DefaultBackground.Load<GameObject>();
      e = fBG.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      bugu00561Scene.backgroundPrefab = fBG.Result;
      fBG = (Future<GameObject>) null;
    }
    e = bugu00561Scene.menu.InitDetailedScreen(item, (ItemInfo[]) null, isNew, isScreenTouch, counter);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().closeAll();
  }

  public IEnumerator onStartSceneAsync(ItemInfo item, ItemInfo[] items, int index)
  {
    Bugu00561Scene bugu00561Scene = this;
    if (Singleton<NGGameDataManager>.GetInstance().IsEarth)
      bugu00561Scene.bgmName = "bgm104";
    else
      bugu00561Scene.headerType = CommonRoot.HeaderType.Normal;
    Future<GameObject> bgF = Res.Prefabs.BackGround.DefaultBackground.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    bugu00561Scene.backgroundPrefab = bgF.Result;
    e = bugu00561Scene.menu.InitDetailedScreen(item, items, false, false, index: index);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().closeAll();
  }
}
