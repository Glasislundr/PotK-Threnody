// Decompiled with JetBrains decompiler
// Type: Coloseum02342Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using GameCore.FastMiniJSON;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Coloseum02342Scene : NGSceneBase
{
  private const int START_DUEL_COUNT = 0;
  private int duelCount;
  [SerializeField]
  private Coloseum02342Menu menu;
  private GameCore.ColosseumResult battle_result;
  private ColosseumUtility.Info info;
  private Gladiator gladiator;
  public TextAsset test_assets;

  public static void changeScene(
    ColosseumUtility.Info info,
    int opponent_index,
    GameCore.ColosseumResult battle_result)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("colosseum023_4_2", false, (object) info, (object) info.gladiators[opponent_index], (object) battle_result);
  }

  public static void changeScene(
    ColosseumUtility.Info info,
    WebAPI.Response.ColosseumResume resume,
    GameCore.ColosseumResult battle_result)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("colosseum023_4_2", false, (object) info, (object) resume, (object) battle_result);
  }

  public static void changeScene(
    ColosseumUtility.Info info,
    WebAPI.Response.ColosseumTutorialResume resume,
    GameCore.ColosseumResult battle_result)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("colosseum023_4_2", false, (object) info, (object) resume, (object) battle_result);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Coloseum02342Scene coloseum02342Scene = this;
    Future<GameObject> bgF = Res.Prefabs.BackGround.ColosseumBackground.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    coloseum02342Scene.backgroundPrefab = bgF.Result;
  }

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e1;
    if (this.duelCount <= 0)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
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
        this.gladiator = futureF.Result.gladiators[0];
        this.info = new ColosseumUtility.Info(false, futureF.Result);
        this.battle_result = ColosseumBattleCalc.calcColosseum(ColosseumEnvironmentInitializer.initializeData(!Object.op_Inequality((Object) this.test_assets, (Object) null) ? new ColosseumInitialData() : new ColosseumInitialData(new WebAPI.Response.ColosseumStart((Dictionary<string, object>) Json.Deserialize(this.test_assets.text)), 0), (ColosseumEnvironment) null));
        futureF = (Future<WebAPI.Response.ColosseumBoot>) null;
      }
    }
    e1 = this.onStartSceneAsync(this.info, this.battle_result);
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public IEnumerator onStartSceneAsync(
    ColosseumUtility.Info info,
    Gladiator gladiator,
    GameCore.ColosseumResult battle_result)
  {
    this.gladiator = gladiator;
    this.battle_result = battle_result;
    IEnumerator e = this.onStartSceneAsync(info, battle_result);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public IEnumerator onStartSceneAsync(
    ColosseumUtility.Info info,
    WebAPI.Response.ColosseumResume resume,
    GameCore.ColosseumResult battle_result)
  {
    this.gladiator = resume.gladiator;
    this.battle_result = battle_result;
    IEnumerator e = this.onStartSceneAsync(info, battle_result);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public IEnumerator onStartSceneAsync(
    ColosseumUtility.Info info,
    WebAPI.Response.ColosseumTutorialResume resume,
    GameCore.ColosseumResult battle_result)
  {
    this.gladiator = resume.gladiator;
    this.battle_result = battle_result;
    IEnumerator e = this.onStartSceneAsync(info, battle_result);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public IEnumerator onStartSceneAsync(ColosseumUtility.Info info, GameCore.ColosseumResult result)
  {
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    ResourceManager rm = Singleton<ResourceManager>.GetInstance();
    IEnumerator e = OnDemandDownload.WaitLoadUnitResource(((IEnumerable<DuelColosseumResult>) this.battle_result.duelResult).Where<DuelColosseumResult>((Func<DuelColosseumResult, bool>) (x => x.opponent != (BL.Unit) null)).SelectMany<DuelColosseumResult, string>((Func<DuelColosseumResult, IEnumerable<string>>) (x => (IEnumerable<string>) rm.PathsFromUnit(x.opponent.unit))), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.menu.Initialize(info, result, this.gladiator, this.duelCount);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) Singleton<NGDuelDataManager>.GetInstance().PreloadCommonDuelEffect();
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
  }

  public void onStartScene()
  {
    this.menu.StartToBeginning(this.duelCount);
    ++this.duelCount;
  }

  public void onStartScene(
    ColosseumUtility.Info info,
    Gladiator gladiator,
    GameCore.ColosseumResult battle_result)
  {
    this.onStartScene();
  }

  public void onStartScene(
    ColosseumUtility.Info info,
    WebAPI.Response.ColosseumResume resume,
    GameCore.ColosseumResult battle_result)
  {
    this.onStartScene();
  }

  public void onStartScene(
    ColosseumUtility.Info info,
    WebAPI.Response.ColosseumTutorialResume resume,
    GameCore.ColosseumResult battle_result)
  {
    this.onStartScene();
  }

  public void onStartScene(ColosseumUtility.Info info, GameCore.ColosseumResult result)
  {
    this.onStartScene();
  }

  public override void onSceneInitialized()
  {
    base.onSceneInitialized();
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }

  public void Reinitialize()
  {
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    this.menu.StartToBeginning(this.duelCount);
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
    ++this.duelCount;
  }

  public void ReplayScene()
  {
    this.duelCount = 0;
    this.onStartScene();
  }
}
