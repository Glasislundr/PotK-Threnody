// Decompiled with JetBrains decompiler
// Type: Guide0114Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Guide0114Scene : NGSceneBase
{
  public Guide0114Menu menu;
  private bool isFirstInit = true;

  public static void changeScene(bool stack, Guide0111Scene.UpdateInfo updateInfo)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("guide011_4", (stack ? 1 : 0) != 0, (object) updateInfo);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Guide0114Scene guide0114Scene = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    Future<GameObject> fBG = Res.Prefabs.BackGround.picturebook.Load<GameObject>();
    IEnumerator e = fBG.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    guide0114Scene.backgroundPrefab = fBG.Result;
  }

  public IEnumerator onStartSceneAsync(Guide0111Scene.UpdateInfo updateInfo)
  {
    long ver = SMManager.Revision<PlayerItem[]>();
    if (updateInfo.version.HasValue)
    {
      long num = ver;
      long? version = updateInfo.version;
      long valueOrDefault = version.GetValueOrDefault();
      if (num == valueOrDefault & version.HasValue)
        goto label_5;
    }
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    Future<WebAPI.Response.ZukanGear> item = WebAPI.ZukanGear((Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    yield return (object) item.Wait();
    if (item.Result == null)
    {
      yield break;
    }
    else
    {
      updateInfo.version = new long?(ver);
      this.isFirstInit = true;
      item = (Future<WebAPI.Response.ZukanGear>) null;
    }
label_5:
    if (this.isFirstInit)
    {
      this.isFirstInit = false;
      IEnumerator e = this.menu.onInitMenuAsync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.menu.IbtnUse();
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public void onStartScene(Guide0111Scene.UpdateInfo updateInfo)
  {
  }

  public override void onEndScene()
  {
    this.menu.StopCreateUnitIconImage();
    Singleton<PopupManager>.GetInstance().closeAll();
  }
}
