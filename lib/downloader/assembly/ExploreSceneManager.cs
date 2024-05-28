// Decompiled with JetBrains decompiler
// Type: ExploreSceneManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (ExploreTaskManager), typeof (ExploreScreenEffectController), typeof (ExploreModelController))]
[DefaultExecutionOrder(-1)]
public class ExploreSceneManager : Singleton<ExploreSceneManager>
{
  private int mLoopSeChannel = -1;

  public ExploreTaskManager Task { get; private set; }

  public ExploreModelController Model { get; private set; }

  public ExploreScreenEffectController ScreenEffect { get; private set; }

  public Explore033TopMenu TopMenu { get; set; }

  public ExploreFooter Footer { get; set; }

  public bool ReloadDirty { get; private set; }

  public void SetReloadDirty() => this.ReloadDirty = true;

  public bool IsBackScreen
  {
    get
    {
      CommonRoot instance = Singleton<CommonRoot>.GetInstance();
      return Singleton<PopupManager>.GetInstance().isOpen || instance.DailyMissionController.IsOpened || instance.guildChatManager.isDetailViewOpened || !this.IsSceneActive;
    }
  }

  public bool IsSceneActive
  {
    get
    {
      return Singleton<NGSceneManager>.GetInstance().sceneName.Equals("explore033_Top") && Singleton<NGSceneManager>.GetInstance().isSceneInitialized;
    }
  }

  protected override void Initialize()
  {
    this.Task = ((Component) this).GetComponent<ExploreTaskManager>();
    this.Model = ((Component) this).GetComponent<ExploreModelController>();
    this.ScreenEffect = ((Component) this).GetComponent<ExploreScreenEffectController>();
  }

  public static void DestroyInstance()
  {
    ExploreSceneManager instanceOrNull = Singleton<ExploreSceneManager>.GetInstanceOrNull();
    if (Object.op_Equality((Object) instanceOrNull, (Object) null))
      return;
    Object.Destroy((Object) ((Component) instanceOrNull).gameObject);
    instanceOrNull.clearInstance();
    Singleton<CommonRoot>.GetInstance().SetFooterEnable(true);
  }

  public void Pause(bool pause) => this.Task.Pause(pause);

  public void PlaySe(string clipName, float delay = 0.0f)
  {
    if (!this.IsSceneActive)
      return;
    Singleton<NGSoundManager>.GetInstance().PlaySe(clipName, delay: delay);
  }

  public void PlayLoopSe(string clipName)
  {
    if (!this.IsSceneActive)
      return;
    this.mLoopSeChannel = Singleton<NGSoundManager>.GetInstance().PlaySe(clipName, true);
  }

  public void StopLoopSe()
  {
    if (this.mLoopSeChannel == -1)
      return;
    Singleton<NGSoundManager>.GetInstance().StopSe(this.mLoopSeChannel);
    this.mLoopSeChannel = -1;
  }

  public void StartReload()
  {
    this.Pause(true);
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Singleton<NGSceneManager>.GetInstance().StartCoroutine(this.Reload());
  }

  private IEnumerator Reload()
  {
    yield return (object) Singleton<NGSceneManager>.GetInstance().destroyLoadedScenesImmediate();
    Singleton<ExploreDataManager>.GetInstance().CleanLog();
    Explore033TopScene.changeScene(false, false);
  }

  public IEnumerator OnStartExploreSceneAsync()
  {
    Singleton<ExploreDataManager>.GetInstance().UpdateBlUnitsFromPlayerDeck();
    this.ScreenEffect.TransitionFullIn();
    yield return (object) this.Model.CreateAllExploreModel();
    this.Task.Initialize();
    yield return (object) this.ScreenEffect.WaitForTransitionFull();
  }

  public IEnumerator PriorityToExploreUpdate(Action changeScene)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    this.Pause(true);
    bool saveFailed = false;
    yield return (object) Singleton<ExploreDataManager>.GetInstance().SaveSuspendData((Action) (() => saveFailed = true));
    this.SetReloadDirty();
    if (saveFailed)
    {
      Singleton<ExploreDataManager>.GetInstance().InitReopenPopupState();
      MypageScene.ChangeSceneOnError();
    }
    else if (changeScene != null)
      changeScene();
  }

  public void OnBackExploreScene() => this.Task.OnBackExploreScene();

  public void OnEndExploreScene()
  {
    Singleton<PopupManager>.GetInstance().dismissWithoutAnim();
    this.StopLoopSe();
  }

  public void OnDestoryExploreScene()
  {
    this.Pause(true);
    this.Model.SetExploreVisible(false);
  }

  private void OnApplicationPause(bool pause)
  {
    if (!pause)
      return;
    this.SetReloadDirty();
  }
}
