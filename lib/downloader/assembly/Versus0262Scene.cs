// Decompiled with JetBrains decompiler
// Type: Versus0262Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Versus0262Scene : NGSceneBase
{
  [SerializeField]
  private Versus0262Menu menu;
  private static bool is_loading_draw;
  private WebAPI.Response.PvpBoot pvpInfo;

  public static void ChangeScene0262(bool stack, PvpMatchingTypeEnum type, bool loading_draw = false)
  {
    Versus0262Scene.is_loading_draw = loading_draw;
    Singleton<NGSceneManager>.GetInstance().changeScene("versus026_2", (stack ? 1 : 0) != 0, (object) type);
  }

  public static void ChangeScene0262(
    bool stack,
    PvpMatchingTypeEnum type,
    WebAPI.Response.PvpBoot pvpInfo)
  {
    Versus0262Scene.is_loading_draw = false;
    Singleton<NGSceneManager>.GetInstance().changeScene("versus026_2", (stack ? 1 : 0) != 0, (object) type, (object) pvpInfo);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Versus0262Scene versus0262Scene = this;
    Future<GameObject> bgF = Res.Prefabs.BackGround.MultiBackground.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    versus0262Scene.backgroundPrefab = bgF.Result;
  }

  public IEnumerator onStartSceneAsync(PvpMatchingTypeEnum type)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = Versus0262Scene.is_loading_draw;
    IEnumerator e1;
    if (this.pvpInfo == null)
    {
      if (Singleton<NGGameDataManager>.GetInstance().isCallHomeUpdateAllData)
      {
        Future<WebAPI.Response.HomeStartUp2> handler = WebAPI.HomeStartUp2((Action<WebAPI.Response.UserError>) (e =>
        {
          WebAPI.DefaultUserErrorCallback(e);
          MypageScene.ChangeSceneOnError();
        }));
        e1 = handler.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        if (handler.Result == null)
        {
          yield break;
        }
        else
        {
          Singleton<NGGameDataManager>.GetInstance().isCallHomeUpdateAllData = false;
          handler = (Future<WebAPI.Response.HomeStartUp2>) null;
        }
      }
      else
      {
        e1 = WebAPI.HomeStartUp().Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
      }
      Future<WebAPI.Response.PvpBoot> futureF = WebAPI.PvpBoot((Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      e1 = futureF.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (futureF.Result == null)
      {
        yield break;
      }
      else
      {
        this.pvpInfo = futureF.Result;
        futureF = (Future<WebAPI.Response.PvpBoot>) null;
      }
    }
    e1 = this.onStartSceneAsync(type, this.pvpInfo);
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public IEnumerator onStartSceneAsync(PvpMatchingTypeEnum type, WebAPI.Response.PvpBoot pvpInfo)
  {
    Versus0262Scene versus0262Scene = this;
    IEnumerator e = versus0262Scene.menu.Init(type, pvpInfo);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Singleton<CommonRoot>.GetInstance().isLoading)
      versus0262Scene.StartCoroutine(versus0262Scene.doWaitHideLoadingLayer());
  }

  private IEnumerator doWaitHideLoadingLayer()
  {
    yield return (object) null;
    yield return (object) null;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public override void onSceneInitialized()
  {
    base.onSceneInitialized();
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }
}
