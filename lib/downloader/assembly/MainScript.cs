// Decompiled with JetBrains decompiler
// Type: MainScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using Gsc.Network;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
public class MainScript : MonoBehaviour
{
  public string commonScene = "common";
  private bool isSceneLoading;
  private bool isGameDataUpdating;

  private IEnumerator Start()
  {
    Singleton<NGSoundManager>.GetInstance().checkIfDLCEnd();
    WebInternalTask.EnableAutoRetry = PerformanceConfig.GetInstance().EnableWebAutoRetry;
    if (PerformanceConfig.GetInstance().IsTuningTitleToHome)
    {
      while (Object.op_Equality((Object) Singleton<NGGameDataManager>.GetInstance(), (Object) null))
        yield return (object) null;
      SceneManager.LoadSceneAsync(this.commonScene, (LoadSceneMode) 1);
      this.isSceneLoading = true;
    }
    else
    {
      while (Object.op_Equality((Object) Singleton<NGGameDataManager>.GetInstance(), (Object) null) || !Singleton<NGGameDataManager>.GetInstance().isInitialized)
        yield return (object) null;
      SceneManager.LoadScene(this.commonScene);
    }
  }

  private void Update()
  {
    if (!this.isSceneLoading)
      return;
    if (!this.isGameDataUpdating && Singleton<NGGameDataManager>.GetInstance().isInitialized)
    {
      Singleton<NGGameDataManager>.GetInstance().bootFirstSceneBefore();
      this.isGameDataUpdating = true;
    }
    else
    {
      if (!this.isGameDataUpdating || Singleton<NGGameDataManager>.GetInstance().IsUpdating || !Object.op_Inequality((Object) Singleton<CommonRoot>.GetInstance(), (Object) null) || !Singleton<CommonRoot>.GetInstance().IsBootSetuped)
        return;
      Singleton<NGGameDataManager>.GetInstance().bootFirstScene("mypage");
      SceneManager.UnloadSceneAsync("main");
      this.isSceneLoading = false;
    }
  }
}
