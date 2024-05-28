// Decompiled with JetBrains decompiler
// Type: Bugu05561Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Bugu05561Scene : NGSceneBase
{
  public Bugu05561Menu menu;

  public static void changeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu055_6_1", stack);
  }

  public static void changeScene(bool stack, ItemInfo item)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu055_6_1", (stack ? 1 : 0) != 0, (object) item);
  }

  public static void changeScene(bool stack, ItemInfo item, bool isNew, bool isScreenTouch)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu055_6_1", (stack ? 1 : 0) != 0, (object) item, (object) isNew, (object) isScreenTouch);
  }

  public IEnumerator onStartSceneAsync(ItemInfo item)
  {
    IEnumerator coroutine = this.startSceneAsync(item, false, false, false);
    while (coroutine.MoveNext())
      yield return coroutine.Current;
  }

  public IEnumerator onStartSceneAsync(ItemInfo item, bool isNew = false, bool isScreenTouch = false)
  {
    IEnumerator coroutine = this.startSceneAsync(item, isNew, isScreenTouch, false);
    while (coroutine.MoveNext())
      yield return coroutine.Current;
  }

  public IEnumerator startSceneAsync(ItemInfo item, bool isNew, bool isScreenTouch, bool isGacha)
  {
    Bugu05561Scene bugu05561Scene = this;
    Future<GameObject> fBG;
    IEnumerator e;
    if (isGacha)
    {
      fBG = Res.Prefabs.BackGround.GachaTopBackground.Load<GameObject>();
      e = fBG.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      bugu05561Scene.backgroundPrefab = fBG.Result;
      ((UIWidget) bugu05561Scene.backgroundPrefab.GetComponent<UI2DSprite>()).color = Consts.GetInstance().GACHA_RESULT_BACKGROUND_COLOR;
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
      bugu05561Scene.backgroundPrefab = fBG.Result;
      fBG = (Future<GameObject>) null;
    }
    e = bugu05561Scene.menu.InitDetailedScreen(item, (ItemInfo[]) null, isNew, isScreenTouch);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().closeAll();
  }
}
