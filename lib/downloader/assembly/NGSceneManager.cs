// Decompiled with JetBrains decompiler
// Type: NGSceneManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UniLinq;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
public class NGSceneManager : Singleton<NGSceneManager>
{
  private Stack<NGSceneManager.SceneWrapper> sceneStack = new Stack<NGSceneManager.SceneWrapper>();
  private Dictionary<string, NGSceneManager.SceneWrapper> loadedScenes = new Dictionary<string, NGSceneManager.SceneWrapper>();
  private NGSceneManager.SceneWrapper tempScene;
  private NGSceneManager.SceneWrapper currentScene;
  private bool currentIsSea;
  private AsyncOperation loadAsync;
  private List<NGSceneManager.SceneWrapper> destroyedSceneList;
  private Queue<IEnumerator> changeSceneQueue = new Queue<IEnumerator>();
  public CommonRoot.HeaderType? LastHeaderType;
  public Action act;
  private NGSceneManager.ChangeSceneParam savedChangeSceneParam;
  private bool isError;
  private bool isStartSceneInit;
  private string changingSceneName;
  private Stack<NGSceneManager.SceneLog> stackChangeLog = new Stack<NGSceneManager.SceneLog>();
  private const int MAX_CHANGE_LOG = 15;
  private const int MAX_SCENE_STACK = 7;
  private bool isDangerAsyncLoadResource;

  public Stack<NGSceneManager.SceneWrapper> SceneStack => this.sceneStack;

  public NGSceneManager.ChangeSceneParam GetSavedChangeSceneParam() => this.savedChangeSceneParam;

  public bool HasSavedChangeSceneParam() => this.savedChangeSceneParam != null;

  public void ClearSavedChangeSceneParam()
  {
    this.savedChangeSceneParam = (NGSceneManager.ChangeSceneParam) null;
  }

  public void SaveCurrentChangeSceneParam()
  {
    this.savedChangeSceneParam = new NGSceneManager.ChangeSceneParam(this.currentScene.sceneBase.sceneName, this.currentScene.args, false, false);
  }

  public object[] ReplaceSavedChangeSceneParamArgs(object[] newArgs)
  {
    if (this.savedChangeSceneParam == null)
      return (object[]) null;
    object[] args = this.savedChangeSceneParam.args;
    this.savedChangeSceneParam = new NGSceneManager.ChangeSceneParam(this.savedChangeSceneParam.sceneName, newArgs, this.savedChangeSceneParam.isStack, this.savedChangeSceneParam.isBackScene);
    return args;
  }

  public void RebootScene(bool bDestroyCurrentScene = true)
  {
    string name = this.currentScene.name;
    object[] args = this.currentScene.args;
    if (bDestroyCurrentScene)
      this.destroyCurrentScene();
    this.changeScene(name, false, args);
  }

  public bool isSceneInitialized
  {
    get => !this.isStartSceneInit && this.tempScene == null && this.loadAsync == null;
  }

  public int changeSceneQueueCount => this.changeSceneQueue.Count;

  protected override void Initialize()
  {
    this.tempScene = (NGSceneManager.SceneWrapper) null;
    this.currentScene = (NGSceneManager.SceneWrapper) null;
    this.currentIsSea = false;
    this.loadAsync = (AsyncOperation) null;
    this.destroyedSceneList = new List<NGSceneManager.SceneWrapper>();
  }

  public void ChangeErrorPage()
  {
    this.destroyLoadedScenes();
    this.isError = true;
  }

  public void SetCurrentSceneArgs(params object[] args)
  {
    if (this.currentScene == null)
      return;
    this.currentScene.args = args;
  }

  public Queue<Func<IEnumerator>> quePreSceneChangeAsync { get; set; } = new Queue<Func<IEnumerator>>();

  public NGSceneManager.SceneLog FindAliveSceneLog()
  {
    NGSceneManager.SceneLog aliveSceneLog;
    for (aliveSceneLog = this.stackChangeLog.Any<NGSceneManager.SceneLog>() ? this.stackChangeLog.Peek() : (NGSceneManager.SceneLog) null; aliveSceneLog != null && !aliveSceneLog.isAliveArgs; aliveSceneLog = this.stackChangeLog.Any<NGSceneManager.SceneLog>() ? this.stackChangeLog.Peek() : (NGSceneManager.SceneLog) null)
      this.stackChangeLog.Pop();
    return aliveSceneLog;
  }

  private void pushChangeSceneLog(string name, object[] args, bool topGlobalBack, bool isSea)
  {
    this.stackChangeLog.Push(new NGSceneManager.SceneLog(name, args, topGlobalBack, isSea));
    if (this.stackChangeLog.Count <= 15)
      return;
    NGSceneManager.SceneLog[] array = this.stackChangeLog.ToArray();
    this.stackChangeLog.Clear();
    for (int index = 14; index >= 0; --index)
      this.stackChangeLog.Push(array[index]);
  }

  private void pushScene(NGSceneManager.SceneWrapper scene)
  {
    this.sceneStack.Push(scene);
    int count = this.sceneStack.Count;
    if (count <= 7)
      return;
    NGSceneManager.SceneWrapper[] array = this.sceneStack.ToArray();
    this.sceneStack.Clear();
    for (int index = 6; index >= 0; --index)
      this.sceneStack.Push(array[index]);
    for (int index = 7; index < count; ++index)
    {
      NGSceneManager.SceneWrapper scene1 = array[index];
      if (!scene1.sceneBase.isDontAutoDestroy)
        this.destroyScene(scene1);
    }
  }

  public List<NGSceneManager.SavedSceneLog> exportSceneChangeLog()
  {
    return this.stackChangeLog.Select<NGSceneManager.SceneLog, NGSceneManager.SavedSceneLog>((Func<NGSceneManager.SceneLog, NGSceneManager.SavedSceneLog>) (x => new NGSceneManager.SavedSceneLog(x))).Reverse<NGSceneManager.SavedSceneLog>().ToList<NGSceneManager.SavedSceneLog>();
  }

  public void importSceneChangeLog(List<NGSceneManager.SavedSceneLog> logs)
  {
    foreach (NGSceneManager.SceneLog log in logs)
      this.stackChangeLog.Push(log);
  }

  private IEnumerator doLoadLevelAdditiveAsync(NGSceneManager.SceneWrapper scene)
  {
    this.loadAsync = SceneManager.LoadSceneAsync(scene.name, (LoadSceneMode) 1);
    yield return (object) this.loadAsync;
  }

  public Coroutine loadResource(GameObject gameObject)
  {
    return this.StartCoroutine(Singleton<ResourceManager>.GetInstance().LoadResource(gameObject));
  }

  private IEnumerator invokeMethod(object obj, string methodName, object[] parameters)
  {
    MethodInfo method = NGSceneManager.getMethod(obj, methodName, parameters);
    if (method != (MethodInfo) null)
    {
      if (method.Invoke(obj, parameters) is IEnumerator e)
      {
        while (e.MoveNext())
          yield return e.Current;
      }
      e = (IEnumerator) null;
    }
  }

  private IEnumerator invokeMethod(object obj, MethodInfo method, object[] parameters)
  {
    if (method != (MethodInfo) null)
    {
      if (method.Invoke(obj, parameters) is IEnumerator e)
      {
        while (e.MoveNext())
          yield return e.Current;
      }
      e = (IEnumerator) null;
    }
  }

  private string getTypeName(IEnumerable<System.Type> xs)
  {
    return string.Join(", ", xs.Select<System.Type, string>((Func<System.Type, string>) (x => x.ToString())).ToArray<string>());
  }

  public bool IsDangerAsyncLoadResource() => this.isDangerAsyncLoadResource;

  private IEnumerator changeSceneUpdate(bool isBackScene)
  {
    NGSceneManager ngSceneManager = this;
    while (true)
    {
      ngSceneManager.isError = false;
      float startTime = Time.time;
      Singleton<CommonRoot>.GetInstance().isTouchBlockAutoClose = true;
      Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
      IEnumerator e;
      if (ngSceneManager.currentScene != null && Object.op_Inequality((Object) ngSceneManager.currentScene.sceneBase, (Object) null))
      {
        ngSceneManager.currentScene.sceneBase.endTweens();
        ngSceneManager.currentScene.sceneBase.onEndScene();
        if (ngSceneManager.currentScene.sceneBase.currentSceneGuildChatDisplayingStatus != NGSceneBase.GuildChatDisplayingStatus.Closed && Object.op_Inequality((Object) Singleton<CommonRoot>.GetInstance().guildChatManager, (Object) null))
          Singleton<CommonRoot>.GetInstance().guildChatManager.SimpleChatActive();
        ngSceneManager.currentScene.sceneBase.SetupGuildChatAfterEndScene();
        e = ngSceneManager.currentScene.sceneBase.onEndSceneAsync();
        while (e.MoveNext())
        {
          if (!ngSceneManager.errorCheck())
            yield return e.Current;
          else
            goto label_105;
        }
        e = (IEnumerator) null;
      }
      if (ngSceneManager.loadAsync != null)
      {
        while (!ngSceneManager.loadAsync.isDone)
          yield return (object) new WaitForEndOfFrame();
      }
      if (ngSceneManager.currentScene != null)
      {
        while (Object.op_Inequality((Object) ngSceneManager.currentScene.sceneBase, (Object) null) && !ngSceneManager.currentScene.sceneBase.isTweenFinished)
        {
          if (!ngSceneManager.errorCheck())
          {
            yield return (object) null;
            if ((double) Time.time - (double) startTime > (double) ngSceneManager.currentScene.sceneBase.tweenTimeoutTime)
              break;
          }
          else
            goto label_105;
        }
        if (ngSceneManager.tempScene.isStack)
        {
          ngSceneManager.pushChangeSceneLog(ngSceneManager.currentScene.name, ngSceneManager.currentScene.args, ngSceneManager.currentScene.sceneBase.IsTopGlobalBack, ngSceneManager.currentIsSea);
          ngSceneManager.pushScene(ngSceneManager.currentScene);
        }
        if (Object.op_Inequality((Object) ngSceneManager.currentScene.sceneObject, (Object) null))
        {
          if (ngSceneManager.currentScene.sceneBase.isAlphaActive)
            ((UIRect) ngSceneManager.currentScene.sceneObject.GetComponent<UIPanel>()).alpha = 0.0f;
          else
            ngSceneManager.currentScene.sceneObject.SetActive(false);
        }
        if (Object.op_Inequality((Object) ngSceneManager.currentScene.sceneBase, (Object) null) && ngSceneManager.currentScene.sceneBase.lockLayout == NGSceneBase.LockLayout.Heaven && !ngSceneManager.currentScene.sceneBase.IsModifiedIsSea)
          Singleton<NGGameDataManager>.GetInstance().IsSea = ngSceneManager.currentScene.sceneBase.stackIsSea;
      }
      else
        ngSceneManager.tempScene.isStack = false;
      while (Object.op_Equality((Object) ngSceneManager.tempScene.sceneObject, (Object) null))
        yield return (object) new WaitForEndOfFrame();
      ngSceneManager.tempScene.SetMethod();
      if (ngSceneManager.tempScene.sceneBase.lockLayout != NGSceneBase.LockLayout.None)
      {
        NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
        switch (ngSceneManager.tempScene.sceneBase.lockLayout)
        {
          case NGSceneBase.LockLayout.Heaven:
            ngSceneManager.tempScene.sceneBase.stackIsSea = instance.IsSea;
            instance.IsSea = false;
            ngSceneManager.tempScene.sceneBase.revisionIsSea = instance.revisionIsSea;
            break;
          case NGSceneBase.LockLayout.ResetHeaven:
            instance.IsSea = false;
            instance.IsColosseum = false;
            break;
          case NGSceneBase.LockLayout.ResetSea:
            instance.IsSea = true;
            instance.IsColosseum = false;
            break;
          case NGSceneBase.LockLayout.ResetColosseum:
            instance.IsSea = false;
            instance.IsColosseum = true;
            break;
        }
      }
      if (ngSceneManager.loadAsync != null)
      {
        List<string> resourceLoadList = ngSceneManager.tempScene.sceneBase.createResourceLoadList();
        if (resourceLoadList != null && resourceLoadList.Count > 0)
        {
          e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<string>) resourceLoadList, ngSceneManager.tempScene.sceneBase.createResourceLoadListConfirmDLC());
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
      ngSceneManager.tempScene.sceneObject.SetActive(true);
      UIPanel tempUIPanel = ngSceneManager.tempScene.sceneObject.GetComponent<UIPanel>();
      if (Object.op_Inequality((Object) tempUIPanel, (Object) null))
        ((UIRect) tempUIPanel).alpha = 0.0f;
      ngSceneManager.isDangerAsyncLoadResource = true;
      while (ngSceneManager.quePreSceneChangeAsync.Any<Func<IEnumerator>>())
      {
        e = ngSceneManager.quePreSceneChangeAsync.Dequeue()();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      if (ngSceneManager.loadAsync != null)
      {
        if (ngSceneManager.tempScene.sceneBase.checkIsUseDLC())
        {
          e = Singleton<ResourceManager>.GetInstance().LoadResource(ngSceneManager.tempScene.sceneObject);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        e = ngSceneManager.tempScene.sceneBase.onInitSceneAsync();
        while (e.MoveNext())
        {
          if (!ngSceneManager.errorCheck())
            yield return e.Current;
          else
            goto label_105;
        }
        e = (IEnumerator) null;
      }
      ngSceneManager.loadAsync = (AsyncOperation) null;
      ngSceneManager.tempScene.sceneBase.IsPush = false;
      if (isBackScene && ngSceneManager.tempScene.backSceneAsync != (MethodInfo) null)
      {
        e = ngSceneManager.invokeMethod((object) ngSceneManager.tempScene.sceneBase, ngSceneManager.tempScene.backSceneAsync, ngSceneManager.tempScene.args);
        while (e.MoveNext())
        {
          if (!ngSceneManager.errorCheck())
            yield return e.Current;
          else
            goto label_105;
        }
        e = (IEnumerator) null;
      }
      else
      {
        e = ngSceneManager.invokeMethod((object) ngSceneManager.tempScene.sceneBase, ngSceneManager.tempScene.startSceneAsync, ngSceneManager.tempScene.args);
        while (e.MoveNext())
        {
          if (!ngSceneManager.errorCheck())
            yield return e.Current;
          else
            goto label_105;
        }
        e = (IEnumerator) null;
      }
      ngSceneManager.tempScene.sceneBase.SetupGuildChatForNextScene();
      ngSceneManager.currentScene = ngSceneManager.tempScene;
      ngSceneManager.currentIsSea = Singleton<NGGameDataManager>.GetInstance().IsSea;
      NGSceneManager.SceneWrapper[] array = ngSceneManager.destroyedSceneList.ToArray();
      ngSceneManager.destroyedSceneList.Clear();
      NGSceneManager.SceneWrapper[] sceneWrapperArray = array;
      for (int index = 0; index < sceneWrapperArray.Length; ++index)
      {
        NGSceneManager.SceneWrapper scene = sceneWrapperArray[index];
        if (scene.name != ngSceneManager.currentScene.name)
        {
          if (Object.op_Inequality((Object) scene.sceneBase, (Object) null))
          {
            e = scene.sceneBase.onDestroySceneAsync();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
          Scene sceneByName = SceneManager.GetSceneByName(scene.name);
          if (((Scene) ref sceneByName).IsValid())
            yield return (object) SceneManager.UnloadSceneAsync(scene.name);
          ngSceneManager.loadedScenes.Remove(scene.name);
        }
        scene = (NGSceneManager.SceneWrapper) null;
      }
      sceneWrapperArray = (NGSceneManager.SceneWrapper[]) null;
      UnitIcon.ClearCache();
      ItemIcon.ClearCache();
      UniqueIcons.ClearCache();
      UIDrawCall.ReleaseInactive();
      if (ngSceneManager.currentScene.sceneBase.needGarbageCollectionOnLoaded)
      {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
      }
      Singleton<ResourceManager>.GetInstance().ClearCache();
      yield return (object) Resources.UnloadUnusedAssets();
      ngSceneManager.isDangerAsyncLoadResource = false;
      e = ngSceneManager.tempScene.sceneBase.setupCommonRoot();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ngSceneManager.tempScene.sceneBase.startTweens();
      ngSceneManager.StartCoroutine(ngSceneManager.tempScene.sceneBase.PlayBGM());
      startTime = Time.time;
      if (isBackScene && ngSceneManager.tempScene.backScene != (MethodInfo) null)
      {
        e = ngSceneManager.invokeMethod((object) ngSceneManager.tempScene.sceneBase, ngSceneManager.tempScene.backScene, ngSceneManager.tempScene.args);
        while (e.MoveNext())
        {
          if (!ngSceneManager.errorCheck())
            yield return e.Current;
          else
            goto label_105;
        }
        e = (IEnumerator) null;
      }
      else
      {
        e = ngSceneManager.invokeMethod((object) ngSceneManager.tempScene.sceneBase, ngSceneManager.tempScene.startScene, ngSceneManager.tempScene.args);
        while (e.MoveNext())
        {
          if (!ngSceneManager.errorCheck())
            yield return e.Current;
          else
            goto label_105;
        }
        e = (IEnumerator) null;
      }
      if (Object.op_Inequality((Object) tempUIPanel, (Object) null))
        ((UIRect) tempUIPanel).alpha = 1f;
      ngSceneManager.currentScene.sceneBase.onSceneInitialized();
      TutorialRoot instance1 = Singleton<TutorialRoot>.GetInstance();
      if (Object.op_Inequality((Object) instance1, (Object) null))
        instance1.OnChangeSceneFinish(ngSceneManager.currentScene.name);
      NGSceneManager.SceneWrapper tmpCurrent = ngSceneManager.currentScene;
      ngSceneManager.tempScene = (NGSceneManager.SceneWrapper) null;
      while (Object.op_Inequality((Object) tmpCurrent.sceneBase, (Object) null) && !tmpCurrent.sceneBase.isTweenFinished)
      {
        yield return (object) null;
        if ((double) Time.time - (double) startTime > (double) tmpCurrent.sceneBase.tweenTimeoutTime)
          break;
      }
      Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
label_105:
      if (ngSceneManager.errorCheck())
      {
        ngSceneManager.currentScene = (NGSceneManager.SceneWrapper) null;
        string startScene = Singleton<CommonRoot>.GetInstance().startScene;
        ngSceneManager.destroyLoadedScenes();
        if (ngSceneManager.loadedScenes.ContainsKey(startScene))
        {
          ngSceneManager.tempScene = ngSceneManager.loadedScenes[startScene];
          ngSceneManager.tempScene.args = new object[0];
          ngSceneManager.tempScene.isStack = false;
        }
        else
        {
          ngSceneManager.tempScene = new NGSceneManager.SceneWrapper(startScene, new object[0], false);
          ngSceneManager.StartCoroutine(ngSceneManager.doLoadLevelAdditiveAsync(ngSceneManager.tempScene));
        }
      }
      else
        break;
    }
    ngSceneManager.isError = false;
  }

  public bool IsSeaByChangeScene()
  {
    return this.currentScene != null && Object.op_Inequality((Object) this.currentScene.sceneBase, (Object) null) && this.currentScene.sceneBase.lockLayout == NGSceneBase.LockLayout.Heaven && !this.currentScene.sceneBase.IsModifiedIsSea && this.currentScene.sceneBase.stackIsSea;
  }

  private bool errorCheck() => this.isError;

  private bool changeSceneCore(string sceneName, object[] args, bool isStack, bool isBackScene)
  {
    this.enqueueScene(sceneName, args, isStack, isBackScene);
    return true;
  }

  private void enqueueScene(string sceneName, object[] args, bool isStack, bool isBackScene)
  {
    if (!string.IsNullOrEmpty(this.changingSceneName) && this.changingSceneName == sceneName)
      return;
    IEnumerator enumerator = this.processChangeSceneQueueCore(new NGSceneManager.ChangeSceneParam(sceneName, args, isStack, isBackScene));
    this.isStartSceneInit = true;
    this.changingSceneName = sceneName;
    this.changeSceneQueue.Enqueue(enumerator);
    if (this.changeSceneQueue.Count != 1)
      return;
    this.StartCoroutine(this.processChangeSceneQueueLoop());
  }

  private IEnumerator processChangeSceneQueueLoop()
  {
    while (this.changeSceneQueue.Count > 0)
    {
      IEnumerator e = this.changeSceneQueue.Peek();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      while (!this.isSceneInitialized)
        yield return (object) null;
      this.changingSceneName = (string) null;
      this.changeSceneQueue.Dequeue();
    }
  }

  private IEnumerator processChangeSceneQueueCore(NGSceneManager.ChangeSceneParam nextScene)
  {
    string sceneName = nextScene.sceneName;
    object[] args = nextScene.args;
    bool isStack = nextScene.isStack;
    bool isBackScene = nextScene.isBackScene;
    IEnumerator e;
    if (this.loadedScenes.ContainsKey(sceneName))
    {
      this.tempScene = this.loadedScenes[sceneName];
      this.tempScene.args = args;
      this.tempScene.isStack = isStack;
      this.tempScene.isEnabledBack = true;
      e = this.changeSceneUpdate(isBackScene);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      this.tempScene = new NGSceneManager.SceneWrapper(sceneName, args, isStack);
      this.loadAsync = SceneManager.LoadSceneAsync(this.tempScene.name, (LoadSceneMode) 1);
      e = this.changeSceneUpdate(isBackScene);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.isStartSceneInit = false;
  }

  public bool changeScene(string sceneName, bool isStack = false, params object[] args)
  {
    if (sceneName == Singleton<CommonRoot>.GetInstance().startScene)
      Singleton<CommonRoot>.GetInstance().setStartScene("mypage");
    if (this.act != null)
      this.act();
    ModelUnits.Instance.currentSceneName = sceneName;
    if (Object.op_Inequality((Object) Singleton<NGMessageUI>.GetInstance(), (Object) null))
      Singleton<NGMessageUI>.GetInstance().TurnOff();
    return this.changeSceneCore(sceneName, args, isStack, false);
  }

  public void waitSceneAction(Action callback)
  {
    this.StartCoroutine(this.waitSceneActionImpl(callback));
  }

  private IEnumerator waitSceneActionImpl(Action callback)
  {
    while (!this.isSceneInitialized)
      yield return (object) null;
    callback();
  }

  public bool backScene()
  {
    NGSceneManager.SceneLog sceneLog = this.stackChangeLog.Any<NGSceneManager.SceneLog>() ? this.stackChangeLog.Pop() : (NGSceneManager.SceneLog) null;
    NGSceneManager.SceneWrapper sceneWrapper = this.sceneStack.Any<NGSceneManager.SceneWrapper>() ? this.sceneStack.Pop() : (NGSceneManager.SceneWrapper) null;
    if (sceneLog == null && sceneWrapper == null)
      return false;
    if (sceneWrapper == null && !sceneLog.isAliveArgs)
    {
      do
      {
        sceneLog = this.stackChangeLog.Any<NGSceneManager.SceneLog>() ? this.stackChangeLog.Pop() : (NGSceneManager.SceneLog) null;
      }
      while (sceneLog != null && !sceneLog.isAliveArgs);
      if (sceneLog == null)
        return false;
    }
    if (!this.currentScene.sceneBase.isDontAutoDestroy)
      this.destroyScene(this.currentScene);
    this.destoryNonStackScenes();
    if (sceneLog != null)
    {
      NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
      if (Object.op_Inequality((Object) instance, (Object) null))
        instance.IsSea = sceneLog.isSea;
    }
    return sceneWrapper == null ? this.changeSceneCore(sceneLog.name, sceneLog.args, false, false) : this.changeSceneCore(sceneWrapper.name, sceneWrapper.args, false, sceneWrapper.isEnabledBack);
  }

  public void ModifySceneStack(NGSceneManager.ChangeSceneParam param)
  {
    NGSceneManager.SceneWrapper sceneWrapper = this.sceneStack.FirstOrDefault<NGSceneManager.SceneWrapper>((Func<NGSceneManager.SceneWrapper, bool>) (x => x.name == param.sceneName));
    sceneWrapper.args = param.args;
    sceneWrapper.isStack = param.isStack;
  }

  public bool backScene(string sceneName)
  {
    if (!this.sceneStack.Any<NGSceneManager.SceneWrapper>((Func<NGSceneManager.SceneWrapper, bool>) (x => x.name == sceneName)) && !this.stackChangeLog.Any<NGSceneManager.SceneLog>((Func<NGSceneManager.SceneLog, bool>) (x => x.name == sceneName)))
      return false;
    if (!this.currentScene.sceneBase.isDontAutoDestroy)
      this.destroyScene(this.currentScene);
    NGSceneManager.SceneLog sceneLog;
    do
    {
      sceneLog = this.stackChangeLog.Any<NGSceneManager.SceneLog>() ? this.stackChangeLog.Pop() : (NGSceneManager.SceneLog) null;
    }
    while (sceneLog != null && sceneLog.name != sceneName);
    NGSceneManager.SceneWrapper scene;
    for (scene = this.sceneStack.Any<NGSceneManager.SceneWrapper>() ? this.sceneStack.Pop() : (NGSceneManager.SceneWrapper) null; scene != null && scene.name != sceneName; scene = this.sceneStack.Any<NGSceneManager.SceneWrapper>() ? this.sceneStack.Pop() : (NGSceneManager.SceneWrapper) null)
    {
      if (!scene.sceneBase.isDontAutoDestroy)
        this.destroyScene(scene);
    }
    this.destoryNonStackScenes();
    if (scene == null && !sceneLog.isAliveArgs)
    {
      do
      {
        sceneLog = this.stackChangeLog.Any<NGSceneManager.SceneLog>() ? this.stackChangeLog.Pop() : (NGSceneManager.SceneLog) null;
      }
      while (sceneLog != null && !sceneLog.isAliveArgs);
      if (sceneLog == null)
        return false;
    }
    if (sceneLog != null)
    {
      NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
      if (Object.op_Inequality((Object) instance, (Object) null))
        instance.IsSea = sceneLog.isSea;
    }
    return scene == null ? this.changeSceneCore(sceneLog.name, sceneLog.args, false, false) : this.changeSceneCore(scene.name, scene.args, false, scene.isEnabledBack);
  }

  public bool clearStackBeforeTopGlobalBack()
  {
    int num = this.stackChangeLog.Count<NGSceneManager.SceneLog>((Func<NGSceneManager.SceneLog, bool>) (x => !x.isTopGlobalBack));
    if (this.currentScene.sceneBase.IsTopGlobalBack && num == 0)
    {
      this.currentScene.isEnabledBack = false;
      return true;
    }
    if (num != 0)
    {
      do
      {
        if (this.sceneStack.Any<NGSceneManager.SceneWrapper>())
        {
          NGSceneManager.SceneWrapper scene = this.sceneStack.Pop();
          if (!scene.sceneBase.isDontAutoDestroy)
            this.destroyScene(scene);
        }
        if (!this.stackChangeLog.Pop().isTopGlobalBack)
          --num;
      }
      while (num > 0);
    }
    if (this.sceneStack.Any<NGSceneManager.SceneWrapper>())
      this.sceneStack.Peek().isEnabledBack = false;
    return false;
  }

  public bool isMatchSceneNameInStack(string pattern)
  {
    Regex reg = new Regex(pattern);
    return this.stackChangeLog.Any<NGSceneManager.SceneLog>((Func<NGSceneManager.SceneLog, bool>) (x => reg.IsMatch(x.name)));
  }

  public string getSceneStackPath()
  {
    return string.Join(".", this.stackChangeLog.Select<NGSceneManager.SceneLog, string>((Func<NGSceneManager.SceneLog, string>) (x => x.name)));
  }

  public void clearStack()
  {
    foreach (NGSceneManager.SceneWrapper scene in this.sceneStack)
    {
      if (!scene.sceneBase.isDontAutoDestroy)
        this.destroyScene(scene);
    }
    this.sceneStack.Clear();
    this.stackChangeLog.Clear();
  }

  public int clearStack(string sceneName)
  {
    int num1 = this.stackChangeLog.Count<NGSceneManager.SceneLog>((Func<NGSceneManager.SceneLog, bool>) (x => x.name == sceneName));
    if (num1 == 0)
      return 0;
    int num2 = 0;
    do
    {
      if (this.sceneStack.Any<NGSceneManager.SceneWrapper>())
      {
        NGSceneManager.SceneWrapper scene = this.sceneStack.Pop();
        if (!scene.sceneBase.isDontAutoDestroy)
          this.destroyScene(scene);
      }
      if (this.stackChangeLog.Pop().name == sceneName)
        --num1;
      ++num2;
    }
    while (num1 > 0);
    return num2;
  }

  public void destoryNonStackScenes()
  {
    foreach (NGSceneManager.SceneWrapper scene in this.loadedScenes.Values)
    {
      if (!this.sceneStack.Contains(scene) && !scene.sceneBase.isDontAutoDestroy)
        this.destroyScene(scene);
    }
  }

  private bool destroyScene(NGSceneManager.SceneWrapper scene)
  {
    if (!this.destroyedSceneList.Contains(scene))
      this.destroyedSceneList.Add(scene);
    return true;
  }

  public bool destroyScene(string sceneName)
  {
    return this.loadedScenes.ContainsKey(sceneName) && this.destroyScene(this.loadedScenes[sceneName]);
  }

  public bool destroyScene(GameObject sceneObject)
  {
    NGSceneManager.SceneWrapper scene = (NGSceneManager.SceneWrapper) null;
    foreach (NGSceneManager.SceneWrapper sceneWrapper in this.loadedScenes.Values)
    {
      if (Object.op_Equality((Object) sceneWrapper.sceneObject, (Object) sceneObject))
      {
        scene = sceneWrapper;
        break;
      }
    }
    return scene != null && this.destroyScene(scene);
  }

  public bool destroyCurrentScene()
  {
    return this.currentScene != null && this.destroyScene(this.currentScene);
  }

  public void destroyLoadedScenes()
  {
    this.clearStack();
    foreach (NGSceneManager.SceneWrapper scene in this.loadedScenes.Values)
      this.destroyScene(scene);
  }

  public IEnumerator destroyLoadedScenesImmediate()
  {
    this.clearStack();
    IEnumerator e;
    if (this.currentScene != null && Object.op_Inequality((Object) this.currentScene.sceneBase, (Object) null))
    {
      this.currentScene.sceneBase.onEndScene();
      e = this.currentScene.sceneBase.onEndSceneAsync();
      while (e.MoveNext() && !this.errorCheck())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    foreach (NGSceneManager.SceneWrapper scene in this.loadedScenes.Values)
      this.destroyScene(scene);
    NGSceneManager.SceneWrapper[] array = this.destroyedSceneList.ToArray();
    this.destroyedSceneList.Clear();
    NGSceneManager.SceneWrapper[] sceneWrapperArray = array;
    for (int index = 0; index < sceneWrapperArray.Length; ++index)
    {
      NGSceneManager.SceneWrapper scene = sceneWrapperArray[index];
      if (Object.op_Inequality((Object) scene.sceneBase, (Object) null))
      {
        e = scene.sceneBase.onDestroySceneAsync();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      Scene sceneByName = SceneManager.GetSceneByName(scene.name);
      if (((Scene) ref sceneByName).IsValid())
        yield return (object) SceneManager.UnloadSceneAsync(scene.name);
      this.loadedScenes.Remove(scene.name);
      scene = (NGSceneManager.SceneWrapper) null;
    }
    sceneWrapperArray = (NGSceneManager.SceneWrapper[]) null;
  }

  public void onChangeSceneAwake(NGSceneBase sceneBase)
  {
    if (sceneBase.isAlphaActive)
      ((UIRect) ((Component) sceneBase).gameObject.GetComponent<UIPanel>()).alpha = 0.0f;
    else
      ((Component) sceneBase).gameObject.SetActive(false);
    this.tempScene.sceneBase = sceneBase;
    this.loadedScenes[this.tempScene.name] = this.tempScene;
    sceneBase.sceneName = this.tempScene.name;
    foreach (Component componentsInChild in ((Component) sceneBase).GetComponentsInChildren<UICamera>(true))
      Object.Destroy((Object) componentsInChild.gameObject);
    UIRoot component1 = ((Component) sceneBase).gameObject.GetComponent<UIRoot>();
    if (!Object.op_Inequality((Object) component1, (Object) null))
      return;
    UIRoot component2 = ((Component) Singleton<CommonRoot>.GetInstance()).GetComponent<UIRoot>();
    component1.manualHeight = component2.manualHeight;
    component1.minimumHeight = component2.minimumHeight;
  }

  public string sceneName
  {
    get
    {
      if (this.tempScene != null)
        return this.tempScene.name;
      return this.currentScene != null ? this.currentScene.name : (string) null;
    }
  }

  public NGSceneBase sceneBase
  {
    get
    {
      if (this.tempScene != null)
        return this.tempScene.sceneBase;
      return this.currentScene != null ? this.currentScene.sceneBase : (NGSceneBase) null;
    }
  }

  public void RestartGame() => this.StartCoroutine(this.RestartCoroutine());

  private IEnumerator RestartCoroutine()
  {
    yield return (object) null;
    StartScript.Restart();
  }

  public static MethodInfo getMethod(object obj, string methodName, object[] parameters)
  {
    MethodInfo[] array1 = ((IEnumerable<MethodInfo>) obj.GetType().GetMethods()).Where<MethodInfo>((Func<MethodInfo, bool>) (method => method.Name == methodName)).ToArray<MethodInfo>();
    if (array1.Length != 0)
    {
      System.Type[] array2 = ((IEnumerable<object>) parameters).Select<object, System.Type>((Func<object, System.Type>) (x => x?.GetType())).ToArray<System.Type>();
      int length = array2.Length;
      for (int index = 0; index < array1.Length; ++index)
      {
        MethodInfo method = array1[index];
        ParameterInfo[] parameters1 = method.GetParameters();
        if (parameters1.Length == length && ((IEnumerable<ParameterInfo>) parameters1).Select<ParameterInfo, System.Type, bool>((IEnumerable<System.Type>) array2, (Func<ParameterInfo, System.Type, bool>) ((x, y) => y == (System.Type) null || x.ParameterType == y)).All<bool>((Func<bool, bool>) (_ => _)))
          return method;
      }
    }
    return (MethodInfo) null;
  }

  public class SceneWrapper
  {
    public string name;
    public object[] args;
    public bool isStack;
    public NGSceneBase sceneBase;
    public bool isEnabledBack = true;
    public MethodInfo startSceneAsync;
    public MethodInfo startScene;
    public MethodInfo backSceneAsync;
    public MethodInfo backScene;

    public GameObject sceneObject
    {
      get
      {
        return Object.op_Inequality((Object) this.sceneBase, (Object) null) ? ((Component) this.sceneBase).gameObject : (GameObject) null;
      }
    }

    public SceneWrapper(string n, object[] a, bool s)
    {
      this.name = n;
      this.args = a;
      this.isStack = s;
      this.sceneBase = (NGSceneBase) null;
    }

    public void SetMethod()
    {
      if (Object.op_Equality((Object) this.sceneObject, (Object) null))
        return;
      this.startSceneAsync = NGSceneManager.getMethod((object) this.sceneBase, "onStartSceneAsync", this.args);
      this.startScene = NGSceneManager.getMethod((object) this.sceneBase, "onStartScene", this.args);
      this.backSceneAsync = NGSceneManager.getMethod((object) this.sceneBase, "onBackSceneAsync", this.args);
      this.backScene = NGSceneManager.getMethod((object) this.sceneBase, "onBackScene", this.args);
    }

    public override string ToString()
    {
      return string.Format("SceneWrapper {0} : {1} : {2} : {3} : {4}", (object) this.name, (object) this.args, (object) this.isStack, (object) this.sceneObject, (object) this.sceneBase);
    }
  }

  public class SavedSceneLog : NGSceneManager.SceneLog
  {
    private Func<object[]> createParams_;
    private object[] instArgs_;

    public SavedSceneLog(NGSceneManager.SceneLog log)
      : base(log.name, log.args, log.isTopGlobalBack, log.isSea)
    {
      this.instArgs_ = this.args;
    }

    public Func<object[]> createParams
    {
      get => this.createParams_;
      set
      {
        this.createParams_ = value;
        if (value == null)
          return;
        this.clearArgs();
      }
    }

    public override object[] args
    {
      get
      {
        Func<object[]> createParams = this.createParams;
        return (createParams != null ? createParams() : (object[]) null) ?? base.args;
      }
    }

    public void clearArgs() => this.instArgs_ = (object[]) null;

    public override bool isAliveArgs => this.createParams != null || base.isAliveArgs;
  }

  public class SceneLog
  {
    private WeakReference wArgs_;

    public string name { get; private set; }

    public virtual object[] args
    {
      get
      {
        return this.wArgs_ == null || !this.wArgs_.IsAlive ? new object[0] : (object[]) this.wArgs_.Target;
      }
      protected set
      {
        this.wArgs_ = value == null || value.Length == 0 ? (WeakReference) null : new WeakReference((object) value);
      }
    }

    public virtual bool isAliveArgs => this.wArgs_ == null || this.wArgs_.IsAlive;

    public bool isTopGlobalBack { get; private set; }

    public bool isSea { get; private set; }

    public SceneLog(string name, object[] args, bool topGlobalBack, bool isSea)
    {
      this.name = name;
      this.args = args;
      this.isTopGlobalBack = topGlobalBack;
      this.isSea = isSea;
    }
  }

  public class ChangeSceneParam
  {
    public readonly string sceneName;
    public readonly object[] args;
    public readonly bool isStack;
    public readonly bool isBackScene;

    public ChangeSceneParam(string sceneName_, object[] args_, bool isStack_, bool isBackScene_)
    {
      this.sceneName = sceneName_;
      this.args = args_;
      this.isStack = isStack_;
      this.isBackScene = isBackScene_;
    }
  }
}
