// Decompiled with JetBrains decompiler
// Type: ModelUnits
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ModelUnits : MonoBehaviour
{
  public Dictionary<int, GameObject> Characters = new Dictionary<int, GameObject>();
  [HideInInspector]
  public List<GameObject> ModelList = new List<GameObject>();
  private static ModelUnits instance;
  [HideInInspector]
  public GameObject teamEdit;
  [HideInInspector]
  public GameObject ChangeSceneUnit0041;
  [HideInInspector]
  public GameObject ChangeScene004682;
  private CoroutineData<bool> coroutineMemory;
  [HideInInspector]
  public string currentSceneName = "";
  [HideInInspector]
  public PlayerDeck[] PlayerDecks;
  private List<UI3DModel> ui3DModels = new List<UI3DModel>();
  private bool stop;
  private bool loaded;
  private static bool isInitialized;
  private GameObject ui3DModelPrefab;
  private float prevTime;

  public static ModelUnits Instance => ModelUnits.instance;

  private void Awake()
  {
    if (ModelUnits.isInitialized)
      Object.Destroy((Object) this);
    else
      ModelUnits.instance = this;
    ModelUnits.isInitialized = true;
  }

  private void OnDestroy()
  {
    if (!Object.op_Equality((Object) ModelUnits.instance, (Object) this))
      return;
    ModelUnits.isInitialized = false;
  }

  private static void GetModelUnit() => new GameObject("ModelUnit").AddComponent<ModelUnits>();

  private IEnumerator LoadSLC3DModel()
  {
    ModelUnits modelUnits = this;
    modelUnits.stop = false;
    Future<GameObject> slc3DModel = Res.Prefabs.gacha006_8.slc_3DModel.Load<GameObject>();
    IEnumerator e = slc3DModel.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    modelUnits.ui3DModelPrefab = slc3DModel.Result;
    for (int index = 0; index < modelUnits.PlayerDecks.Length; ++index)
    {
      GameObject gameObject = new GameObject("Parent");
      UI3DModel component = modelUnits.ui3DModelPrefab.Clone(gameObject.transform).GetComponent<UI3DModel>();
      if (Object.op_Equality((Object) component, (Object) null))
        Debug.LogWarning((object) "ui3DModel null");
      component.lightOn = index == 0;
      modelUnits.ui3DModels.Add(component);
    }
    for (int j = 0; j < modelUnits.PlayerDecks.Length; ++j)
    {
      for (int i = 0; i < 5; ++i)
      {
        if (modelUnits.ui3DModels.Count <= i || modelUnits.stop)
        {
          yield break;
        }
        else
        {
          UI3DModel ui3DModel = modelUnits.ui3DModels[i];
          ui3DModel.widget.depth = 18 - i;
          if (modelUnits.PlayerDecks[j].player_units[i] != (PlayerUnit) null)
          {
            modelUnits.StartCoroutine(ui3DModel.UnitEdit(modelUnits.PlayerDecks[j].player_units[i]));
            while (Object.op_Equality((Object) ui3DModel.model_creater_.UnitModel, (Object) null))
              yield return (object) null;
          }
          ((Component) ui3DModel.ModelCamera).transform.localPosition = new Vector3((float) (j * 1000), (float) (i * 1000), 0.0f);
          ui3DModel = (UI3DModel) null;
        }
      }
    }
    yield return (object) null;
    yield return (object) null;
  }

  private IEnumerator PreLoad3DModel()
  {
    ModelUnits modelUnits = this;
    yield return (object) modelUnits.StartCoroutine(modelUnits.LoadSLC3DModel());
    foreach (UI3DModel ui3Dmodel in modelUnits.ui3DModels)
      ui3Dmodel.DestroyModelCamera();
    modelUnits.DestroyModelUnits();
  }

  public void LoadPlayerDeck(PlayerDeck[] pd)
  {
    if (this.loaded)
      return;
    this.loaded = true;
    this.PlayerDecks = pd;
    Debug.LogWarning((object) ("LoadPlayerDeck !!!!!! " + (object) this.PlayerDecks.Length));
    this.StartCoroutine(this.PreLoad3DModel());
  }

  public void SetScene(string str = "")
  {
    this.ChangeSceneUnit0041.SetActive(false);
    this.StartCoroutine("unit_0041SetActive");
  }

  public GameObject InstantiateModelUnits(GameObject character)
  {
    return this.Characters.ContainsValue(character) ? ObjectPoolController.Instantiate(character) : (GameObject) null;
  }

  public void DestroyModelUnits()
  {
    for (int index = 0; index < this.ModelList.Count; ++index)
    {
      if (Object.op_Implicit((Object) this.ModelList[index]) && Object.op_Implicit((Object) this.ModelList[index].gameObject))
        ObjectPoolController.Destroy(this.ModelList[index].gameObject);
    }
    if (this.ModelList.Count <= 500)
      return;
    this.DidReceiveMemoryWarning(this.ModelList.Count.ToString());
  }

  private void LayerChange(int layer, Transform to)
  {
    ((Component) to).gameObject.layer = layer;
    foreach (Component componentsInChild in ((Component) to).GetComponentsInChildren<Transform>())
      componentsInChild.gameObject.layer = layer;
  }

  private IEnumerator StartCheckMemory()
  {
    if (this.coroutineMemory == null)
    {
      this.coroutineMemory = ModelUnits.instance.StartCoroutine<bool>(this.CheckUsedMemory());
      yield return (object) this.coroutineMemory.Coroutine;
    }
    else if (this.coroutineMemory != null && !this.coroutineMemory.Running)
    {
      this.coroutineMemory = ModelUnits.instance.StartCoroutine<bool>(this.CheckUsedMemory());
      yield return (object) this.coroutineMemory.Coroutine;
    }
  }

  private IEnumerator CheckUsedMemory()
  {
    int time = 10;
    while (true)
    {
      yield return (object) new WaitForSeconds(1f);
      --time;
      if (time > 0)
      {
        yield return (object) null;
      }
      else
      {
        if (time < 0)
          time = 0;
        this.coroutineMemory.Stop();
      }
    }
  }

  private IEnumerator MyUpdate()
  {
    while (true)
    {
      do
      {
        yield return (object) new WaitForSeconds(1f);
      }
      while (this.coroutineMemory == null);
      Debug.LogError((object) ("Update coroutineMemory " + this.coroutineMemory.Running.ToString()));
    }
  }

  private void OnApplicationPause(bool paused)
  {
    if (!paused || this.coroutineMemory == null)
      return;
    this.coroutineMemory.Stop();
  }

  public void DidReceiveMemoryWarning(string message)
  {
    if ((double) Time.realtimeSinceStartup - (double) this.prevTime < 300.0)
      return;
    this.prevTime = Time.realtimeSinceStartup;
    for (int index = 0; index < this.ModelList.Count; ++index)
    {
      if (Object.op_Implicit((Object) this.ModelList[index].gameObject))
      {
        this.ModelList[index].SetActive(false);
        Object.Destroy((Object) this.ModelList[index]);
      }
    }
    this.ModelList.Clear();
    GC.Collect();
    GC.WaitForPendingFinalizers();
    Singleton<ResourceManager>.GetInstance().ClearCache();
    Resources.UnloadUnusedAssets();
  }
}
