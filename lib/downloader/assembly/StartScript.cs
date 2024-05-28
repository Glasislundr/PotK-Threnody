// Decompiled with JetBrains decompiler
// Type: StartScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using Gsc;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
public class StartScript : MonoBehaviour
{
  public StartScript.StartScene startScene;
  public Font defaultFont;

  public static void Restart()
  {
    if (PerformanceConfig.GetInstance().IsSpeedPriority)
    {
      PerformanceConfig.GetInstance().IsSpeedPriority = false;
      ScreenUtil.RefreshPerformanceResolution();
    }
    MypageScene.ClearCache();
    MypageRootMenu.ClearCache();
    GuildUtil.ClearCache();
    MasterDataCache.CacheClear();
    Singleton<ResourceManager>.GetInstance().ClearPathCache();
    OnDemandDownload.InitVariable();
    SceneManager.LoadScene("start");
    Singleton<ResourceManager>.GetInstance().ClearCache();
    Bootstrap.RebootGSCC();
  }

  private static void destroyIfExists(MonoBehaviour instance)
  {
    if (!Object.op_Inequality((Object) instance, (Object) null))
      return;
    ((Component) instance).gameObject.SingletonDestory();
  }

  private void Awake()
  {
  }

  private IEnumerator Start()
  {
    StartScript startScript = this;
    startScript.cleanupDontDestroyObjects();
    yield return (object) new WaitWhile((Func<bool>) (() => !SDK.Initialized));
    SplashScreenController splashScreen = ((Component) startScript).GetComponent<SplashScreenController>();
    if (Object.op_Inequality((Object) splashScreen, (Object) null))
    {
      splashScreen.ShowSplashScreen();
      yield return (object) new WaitWhile((Func<bool>) (() => !splashScreen.isFinished));
    }
    startScript.defaultFont.RequestCharactersInTexture("ABCDE", 512);
    SceneManager.LoadScene("startup000_6");
  }

  private void cleanupDontDestroyObjects()
  {
    StartScript.destroyIfExists((MonoBehaviour) Singleton<NGSceneManager>.GetInstanceOrNull());
    StartScript.destroyIfExists((MonoBehaviour) Singleton<CommonRoot>.GetInstanceOrNull());
    StartScript.destroyIfExists((MonoBehaviour) Singleton<TutorialRoot>.GetInstanceOrNull());
    StartScript.destroyIfExists((MonoBehaviour) Singleton<NGSoundManager>.GetInstanceOrNull());
    StartScript.destroyIfExists((MonoBehaviour) Singleton<ExploreSceneManager>.GetInstanceOrNull());
    StartScript.destroyIfExists((MonoBehaviour) Singleton<ExploreLotteryCore>.GetInstanceOrNull());
    StartScript.destroyIfExists((MonoBehaviour) Singleton<ExploreDataManager>.GetInstanceOrNull());
  }

  public enum StartScene
  {
    Movie,
  }
}
