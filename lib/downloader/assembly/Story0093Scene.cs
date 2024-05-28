// Decompiled with JetBrains decompiler
// Type: Story0093Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Story0093Scene : NGSceneBase
{
  public StoryExecuter executer;
  private float timeScaleTmp;
  private StoryExecuter.ScriptMode modeScript_;
  private static string sceneNormal_ = "story009_3";
  private static string sceneSea_ = "story009_3_sea";
  public const string integralLastScriptIdKey = "IntegralLastScriptId";
  public const string lostLastScriptIdKey = "LostLastScriptId";
  public const string heavenLastScriptIdKey = "HeavenLastScriptId";
  public const string everafterLastScriptIdKey = "EverafterLastScriptId";
  public const string seaLastScriptIdKey = "SeaLastScriptId";
  public const string extraLastScriptIdKey = "ExtraLastScriptId";
  private bool IsErrorStoryPlay;
  private Action endAction;
  private Story0093Scene.DateParam dateParam_;
  private List<Story0093Scene.ContinuousData> continuousDataList_ = new List<Story0093Scene.ContinuousData>();
  private int questXL_ = -1;
  private bool extraQuest_;
  private bool callFlag;

  public static bool setTestScript(string script) => false;

  public static void changeScene(
    bool stack,
    int scriptId,
    bool? isSea = null,
    Action endAction = null,
    List<Story0093Scene.ContinuousData> continuousDataList = null)
  {
    string sceneName = isSea.HasValue ? (isSea.Value ? Story0093Scene.sceneSea_ : Story0093Scene.sceneNormal_) : (Singleton<NGGameDataManager>.GetInstance().IsSea ? Story0093Scene.sceneSea_ : Story0093Scene.sceneNormal_);
    Singleton<NGSceneManager>.GetInstance().changeScene(sceneName, (stack ? 1 : 0) != 0, (object) scriptId, (object) endAction, (object) continuousDataList);
  }

  public static void changeSceneModeDate(
    bool stack,
    int scriptId,
    UnitUnit characterA,
    string background = null,
    Action<int, int> eventSelected = null,
    bool isLoadedScript = false,
    bool? isSea = null,
    int? questionId = null)
  {
    Hashtable replaceTable = new Hashtable()
    {
      {
        (object) nameof (background),
        string.IsNullOrEmpty(background) ? (object) "\"black\"" : (object) ("\"" + background + "\"")
      }
    };
    Story0093Scene.changeSceneModeDate(stack, scriptId, characterA, replaceTable, eventSelected, isLoadedScript, isSea, questionId);
  }

  public static void changeSceneModeDate(
    bool stack,
    int scriptId,
    UnitUnit characterA,
    Hashtable replaceTable,
    Action<int, int> eventSelected = null,
    bool isLoadedScript = false,
    bool? isSea = null,
    int? questionId = null)
  {
    string sceneName = isSea.HasValue ? (isSea.Value ? Story0093Scene.sceneSea_ : Story0093Scene.sceneNormal_) : (Singleton<NGGameDataManager>.GetInstance().IsSea ? Story0093Scene.sceneSea_ : Story0093Scene.sceneNormal_);
    Story0093Scene.DateParam dateParam = new Story0093Scene.DateParam(scriptId, characterA, replaceTable, eventSelected, isLoadedScript, questionId);
    Singleton<NGSceneManager>.GetInstance().changeScene(sceneName, true, (object) dateParam);
  }

  public static IEnumerator loadDateScript(int scriptId, UnitUnit unit, bool isQuestion)
  {
    IEnumerator e = MasterData.LoadDateScriptBase(scriptId);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (unit != null)
    {
      e = MasterData.LoadDateScriptParts(unit.resource_reference_unit_id_UnitUnit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (isQuestion)
      {
        e = MasterData.LoadDateScriptQuestion(unit.resource_reference_unit_id_UnitUnit);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  public static void unloadDateScript()
  {
    MasterDataCache.Unload("DateScriptBase");
    MasterDataCache.Unload("DateScriptParts");
    MasterDataCache.Unload("DateScriptQuestion");
  }

  public static void changeScene009_3(bool stack, int scriptId)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(Story0093Scene.sceneNormal_, true, (object) scriptId);
  }

  public static void changeScene009_3(bool stack, int scriptId, bool call)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(Story0093Scene.sceneNormal_, true, (object) scriptId, (object) call);
  }

  public static void continuousChangeScene009_3(bool stack, int scriptId)
  {
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      Singleton<NGSceneManager>.GetInstance().changeScene(Story0093Scene.sceneSea_, (stack ? 1 : 0) != 0, (object) scriptId);
    else
      Singleton<NGSceneManager>.GetInstance().changeScene(Story0093Scene.sceneNormal_, (stack ? 1 : 0) != 0, (object) scriptId);
  }

  public static void changeScene009_3(
    bool stack,
    int scriptId,
    List<Story0093Scene.ContinuousData> continuousDataList,
    int XL = -1)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(Story0093Scene.sceneNormal_, (stack ? 1 : 0) != 0, (object) scriptId, (object) continuousDataList, (object) XL);
  }

  public static void changeScene009_3(
    bool stack,
    int scriptId,
    List<Story0093Scene.ContinuousData> continuousDataList,
    bool extraQuest)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(Story0093Scene.sceneNormal_, (stack ? 1 : 0) != 0, (object) scriptId, (object) continuousDataList, (object) extraQuest);
  }

  public IEnumerator onStartSceneAsync()
  {
    return this.onStartSceneAsync(MasterData.ScriptScriptList[0].ID);
  }

  public IEnumerator onStartSceneAsync(int scriptId, Action endAction)
  {
    this.endAction = endAction;
    IEnumerator e = this.onStartSceneAsync(scriptId);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(Story0093Scene.DateParam dateParam)
  {
    this.dateParam_ = dateParam;
    if (this.dateParam_ != null)
    {
      this.modeScript_ = StoryExecuter.ScriptMode.Date;
      if (Object.op_Inequality((Object) this.executer, (Object) null))
      {
        this.executer.setMode(this.modeScript_);
        this.executer.setEventSelected(this.dateParam_.eventSelected_);
      }
      IEnumerator e = this.onStartSceneAsync(this.dateParam_.scriptId_);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public IEnumerator onStartSceneAsync(
    int scriptId,
    List<Story0093Scene.ContinuousData> continuousDataList,
    int XL = -1)
  {
    this.continuousDataList_ = continuousDataList;
    this.questXL_ = XL;
    IEnumerator e = this.onStartSceneAsync(scriptId);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    int scriptId,
    List<Story0093Scene.ContinuousData> continuousDataList,
    bool extraQuest)
  {
    this.continuousDataList_ = continuousDataList;
    this.extraQuest_ = extraQuest;
    IEnumerator e = this.onStartSceneAsync(scriptId);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    int scriptId,
    Action endAction,
    List<Story0093Scene.ContinuousData> continuousDataList)
  {
    this.endAction = endAction;
    this.continuousDataList_ = continuousDataList;
    IEnumerator e = this.onStartSceneAsync(scriptId);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(int scriptId, bool call)
  {
    this.callFlag = call;
    IEnumerator e = this.onStartSceneAsync(scriptId);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(int scriptId)
  {
    if (Object.op_Inequality((Object) this.executer, (Object) null))
    {
      Singleton<NGGameDataManager>.GetInstance().questAutoLap = false;
      string[] filepatterns = new string[3]
      {
        "unit_large",
        "Face",
        "VO_"
      };
      Tuple<IEnumerable<UnitUnit>, IEnumerable<string>> resources;
      IEnumerator e;
      if (this.modeScript_ != StoryExecuter.ScriptMode.Date)
      {
        e = MasterData.LoadScriptScript(scriptId);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (MasterData.ScriptScript == null || !MasterData.ScriptScript.ContainsKey(scriptId))
        {
          this.IsErrorStoryPlay = true;
          yield break;
        }
        else
        {
          resources = StoryUtility.GetResource(scriptId);
          if (resources.Item1 != null)
          {
            e = OnDemandDownload.WaitLoadUnitResource(resources.Item1, false, (IEnumerable<string>) filepatterns);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
          if (resources.Item2 != null)
          {
            e = OnDemandDownload.waitLoadSomethingResource(resources.Item2, false);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
          if (this.callFlag && Object.op_Inequality((Object) this.executer.skipBtn, (Object) null))
            this.executer.skipBtn.SetActive(false);
          ((Component) this.executer).gameObject.SetActive(true);
          e = this.executer.initialize(MasterData.ScriptScript[scriptId].script, this.endAction, scriptId, this.continuousDataList_);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          resources = (Tuple<IEnumerable<UnitUnit>, IEnumerable<string>>) null;
        }
      }
      else
      {
        if (!this.dateParam_.isLoadedScript_)
        {
          e = Story0093Scene.loadDateScript(scriptId, this.dateParam_.characterA_, this.dateParam_.isQuestion_);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        if (MasterData.DateScriptBase == null || !MasterData.DateScriptBase.ContainsKey(scriptId))
        {
          this.IsErrorStoryPlay = true;
          yield break;
        }
        else
        {
          string txtscript = MasterData.DateScriptBase[scriptId].script;
          StoryCasting storyCasting = new StoryCasting();
          if (this.dateParam_.characterA_ != null)
          {
            storyCasting.setCasting(this.dateParam_.characterA_.ID);
            if (this.dateParam_.isQuestion_)
              storyCasting.setQuestion(this.dateParam_.question_);
          }
          txtscript = storyCasting.buildScript(txtscript, this.dateParam_.replaceExtension_);
          resources = StoryUtility.GetResource(txtscript);
          if (resources.Item1 != null)
          {
            e = OnDemandDownload.WaitLoadUnitResource(resources.Item1, false, (IEnumerable<string>) filepatterns);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
          if (resources.Item2 != null)
          {
            e = OnDemandDownload.waitLoadSomethingResource(resources.Item2, false);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
          ((Component) this.executer).gameObject.SetActive(true);
          e = this.executer.initialize(txtscript, this.endAction, scriptId, this.continuousDataList_);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          txtscript = (string) null;
          resources = (Tuple<IEnumerable<UnitUnit>, IEnumerable<string>>) null;
        }
      }
      filepatterns = (string[]) null;
      this.SaveLastScriptId(scriptId);
      this.executer.render();
      while (!this.executer.isRenderDone)
        yield return (object) 0;
    }
  }

  private void SaveLastScriptId(int scriptId)
  {
    if (this.continuousDataList_ == null || this.continuousDataList_.Count == 0)
      return;
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      PlayerPrefs.SetInt("SeaLastScriptId", scriptId);
    else if (this.extraQuest_)
    {
      PlayerPrefs.SetInt("ExtraLastScriptId", scriptId);
    }
    else
    {
      switch (this.questXL_)
      {
        case 1:
          PlayerPrefs.SetInt("HeavenLastScriptId", scriptId);
          break;
        case 4:
          PlayerPrefs.SetInt("LostLastScriptId", scriptId);
          break;
        case 6:
          PlayerPrefs.SetInt("IntegralLastScriptId", scriptId);
          break;
        case 7:
          PlayerPrefs.SetInt("EverafterLastScriptId", scriptId);
          break;
      }
    }
    PlayerPrefs.Save();
  }

  public void onStartScene()
  {
    BattleCameraFilter.Inactive();
    this.timeScaleTmp = Time.timeScale;
    Time.timeScale = 1f;
    if (this.IsErrorStoryPlay)
    {
      this.IsErrorStoryPlay = false;
      if (this.endAction == null)
        Singleton<NGSceneManager>.GetInstance().backScene();
      else
        this.endAction();
    }
    else
      Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public void onStartScene(int scriptId, Action endAction) => this.onStartScene();

  public void onStartScene(int scriptId) => this.onStartScene();

  public void onStartScene(Story0093Scene.DateParam dateParam) => this.onStartScene();

  public void onStartScene(
    int scriptId,
    List<Story0093Scene.ContinuousData> continuousDataList,
    int XL = -1)
  {
    this.onStartScene();
  }

  public void onStartScene(
    int scriptId,
    List<Story0093Scene.ContinuousData> continuousDataList,
    bool extraQuest)
  {
    this.onStartScene();
  }

  public void onStartScene(
    int scriptId,
    Action endAction,
    List<Story0093Scene.ContinuousData> continuousDataList)
  {
    this.onStartScene();
  }

  public void onStartScene(int scriptId, bool call) => this.onStartScene();

  public override void onEndScene()
  {
    Time.timeScale = this.timeScaleTmp;
    if (Object.op_Inequality((Object) this.executer, (Object) null))
      this.executer.onEndScene();
    BattleCameraFilter.Active();
    EarthDataManager instanceOrNull = Singleton<EarthDataManager>.GetInstanceOrNull();
    if (Object.op_Inequality((Object) instanceOrNull, (Object) null))
      instanceOrNull.isStoryPlayBackMode = false;
    if (this.modeScript_ != StoryExecuter.ScriptMode.Date)
    {
      MasterDataCache.Unload("ScriptScript");
    }
    else
    {
      if (this.dateParam_.isLoadedScript_)
        return;
      Story0093Scene.unloadDateScript();
    }
  }

  public class DateParam
  {
    public int scriptId_ { get; private set; }

    public UnitUnit characterA_ { get; private set; }

    public Hashtable replaceExtension_ { get; private set; }

    public Action<int, int> eventSelected_ { get; private set; }

    public bool isLoadedScript_ { get; private set; }

    public int question_ { get; private set; }

    public bool isQuestion_ { get; private set; }

    public DateParam(
      int scriptId,
      UnitUnit characterA,
      Hashtable replaceExtension,
      Action<int, int> eventSelected,
      bool isLoadedScript,
      int? questionId)
    {
      this.scriptId_ = scriptId;
      this.characterA_ = characterA;
      this.replaceExtension_ = replaceExtension;
      this.eventSelected_ = eventSelected;
      this.isLoadedScript_ = isLoadedScript;
      this.question_ = questionId.HasValue ? questionId.Value : 0;
      this.isQuestion_ = questionId.HasValue;
    }
  }

  public class ContinuousData
  {
    public int scriptId_ { get; set; }

    public bool continuousFlag_ { get; set; }
  }
}
