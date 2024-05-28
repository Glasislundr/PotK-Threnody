// Decompiled with JetBrains decompiler
// Type: Colosseum0234Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Colosseum0234Scene : NGSceneBase
{
  [SerializeField]
  private Colosseum0234Menu menu;
  private ColosseumUtility.Info collosseumInfo = new ColosseumUtility.Info();
  private RankingPlayer MyRanking;
  private bool isConnect;
  private bool isInit;

  public static void ChangeScene(bool connect = false, bool isStack = false)
  {
    Colosseum0234Scene.Param obj = new Colosseum0234Scene.Param(connect, (int[]) null, (ColosseumUtility.Info) null);
    Singleton<NGSceneManager>.GetInstance().changeScene("colosseum023_4", (isStack ? 1 : 0) != 0, (object) obj, (object) false);
  }

  public static void ChangeScene(int[] opponents, ColosseumUtility.Info collosseumInfo)
  {
    Colosseum0234Scene.Param obj = new Colosseum0234Scene.Param(false, opponents, collosseumInfo);
    Singleton<NGSceneManager>.GetInstance().changeScene("colosseum023_4", false, (object) obj, (object) false);
  }

  public static void ChangeScene(ColosseumUtility.Info collosseumInfo)
  {
    Colosseum0234Scene.Param obj = new Colosseum0234Scene.Param(false, (int[]) null, collosseumInfo);
    Singleton<NGSceneManager>.GetInstance().changeScene("colosseum023_4", false, (object) obj, (object) false);
  }

  public static void ChangeSceneFromBattleResult(Colosseum0234Scene.Param param, bool isTutorial)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("colosseum023_4", false, (object) param, (object) isTutorial);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Colosseum0234Scene colosseum0234Scene = this;
    Future<GameObject> bgF = Res.Prefabs.BackGround.ColosseumBackground.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    colosseum0234Scene.backgroundPrefab = bgF.Result;
  }

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = this.onStartSceneAsync(new Colosseum0234Scene.Param(true, (int[]) null, (ColosseumUtility.Info) null), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(Colosseum0234Scene.Param param, bool isTutorial)
  {
    Colosseum0234Scene scene = this;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    if (Persist.colosseumDeckOrganized.Data.number < 0)
    {
      Persist.colosseumDeckOrganized.Data.number = Persist.deckOrganized.Data.number;
      Persist.colosseumDeckOrganized.Flush();
    }
    if (param.collosseumInfo != null)
      scene.collosseumInfo = param.collosseumInfo;
    IEnumerator e1;
    if (param.connect && !scene.isConnect)
    {
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
      Future<WebAPI.Response.ColosseumBoot> futureF = WebAPI.ColosseumBoot((Action<WebAPI.Response.UserError>) (e =>
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
        scene.collosseumInfo.SetBootInfo(futureF.Result);
        if (scene.collosseumInfo.is_tutorial)
        {
          Future<WebAPI.Response.ColosseumTutorialBoot> tutorialBootF = WebAPI.ColosseumTutorialBoot((Action<WebAPI.Response.UserError>) (e =>
          {
            WebAPI.DefaultUserErrorCallback(e);
            MypageScene.ChangeSceneOnError();
          }));
          e1 = tutorialBootF.Wait();
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
          if (tutorialBootF.Result == null)
          {
            yield break;
          }
          else
          {
            scene.collosseumInfo.SetBootInfo(tutorialBootF.Result);
            tutorialBootF = (Future<WebAPI.Response.ColosseumTutorialBoot>) null;
          }
        }
        scene.isConnect = true;
        futureF = (Future<WebAPI.Response.ColosseumBoot>) null;
      }
    }
    if (scene.collosseumInfo.rankingUpdated || !scene.isInit)
    {
      Future<WebAPI.Response.ColosseumRanking> receive = WebAPI.ColosseumRanking((Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      e1 = receive.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (receive.Result == null)
      {
        yield break;
      }
      else
      {
        e1 = OnDemandDownload.WaitLoadHasUnitResource(false);
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        scene.MyRanking = receive.Result.my_ranking;
        receive = (Future<WebAPI.Response.ColosseumRanking>) null;
      }
    }
    e1 = scene.menu.Initialize(scene, scene.collosseumInfo, param.opponents, isTutorial, scene.MyRanking);
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    scene.isInit = true;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
  }

  public override void onSceneInitialized()
  {
    base.onSceneInitialized();
    this.StartCoroutine(this.procLoading());
  }

  public IEnumerator Restart(Colosseum0234Scene.Param param, bool isTutorial)
  {
    Colosseum0234Scene scene = this;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    if (param.collosseumInfo != null)
      scene.collosseumInfo = param.collosseumInfo;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    scene.isConnect = true;
    Future<WebAPI.Response.ColosseumBoot> futureF = WebAPI.ColosseumBoot((Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = futureF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (futureF.Result != null)
    {
      scene.collosseumInfo.SetBootInfo(futureF.Result);
      if (scene.collosseumInfo.is_tutorial)
      {
        Future<WebAPI.Response.ColosseumTutorialBoot> tutorialBootF = WebAPI.ColosseumTutorialBoot((Action<WebAPI.Response.UserError>) (e =>
        {
          WebAPI.DefaultUserErrorCallback(e);
          MypageScene.ChangeSceneOnError();
        }));
        e1 = tutorialBootF.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        if (tutorialBootF.Result == null)
        {
          yield break;
        }
        else
        {
          scene.collosseumInfo.SetBootInfo(tutorialBootF.Result);
          tutorialBootF = (Future<WebAPI.Response.ColosseumTutorialBoot>) null;
        }
      }
      Future<WebAPI.Response.ColosseumRanking> receive = WebAPI.ColosseumRanking((Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      e1 = receive.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (receive.Result != null)
      {
        e1 = OnDemandDownload.WaitLoadHasUnitResource(false);
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        scene.MyRanking = receive.Result.my_ranking;
        e1 = scene.menu.Initialize(scene, scene.collosseumInfo, param.opponents, isTutorial, scene.MyRanking);
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
        e1 = scene.procLoading();
        while (e1.MoveNext())
          yield return (object) null;
        e1 = (IEnumerator) null;
      }
    }
  }

  public override IEnumerator onEndSceneAsync()
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    yield break;
  }

  public override void onEndScene()
  {
    base.onEndScene();
    Singleton<PopupManager>.GetInstance().closeAll();
  }

  private IEnumerator procLoading()
  {
    yield return (object) new WaitForEndOfFrame();
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public class Param
  {
    public bool connect;
    public int[] opponents;
    public ColosseumUtility.Info collosseumInfo = new ColosseumUtility.Info();

    public Param(bool c, int[] o, ColosseumUtility.Info info)
    {
      this.connect = c;
      this.opponents = o;
      this.collosseumInfo = info;
    }
  }
}
