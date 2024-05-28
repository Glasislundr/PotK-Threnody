// Decompiled with JetBrains decompiler
// Type: gacha006_effectScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class gacha006_effectScene : NGSceneBase
{
  [SerializeField]
  private gacha006_effectMenu menu;
  [SerializeField]
  private UIPanel mainPanel;
  private GameObject gacha2d;
  private GameObject gacha3d;
  private const int MAX_RESULT_COUNT_FOR_EFFECT = 10;

  public IEnumerator onStartSceneAsync(bool isPreview)
  {
    this.menu.isPreview = isPreview;
    yield return (object) this.onStartSceneAsync();
  }

  public IEnumerator onStartSceneAsync()
  {
    Singleton<NGGameDataManager>.GetInstance().baseAmbientLight = RenderSettings.ambientLight;
    RenderSettings.ambientLight = Consts.GetInstance().GACHA_EFFECT_AMBIENT_COLOR;
    IEnumerator e = this.onStartSceneAsync(GachaResultData.GetInstance().GetData());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(GachaResultData.ResultData resultData)
  {
    gacha006_effectScene gacha006EffectScene = this;
    Future<GameObject> future;
    IEnumerator e;
    if (Object.op_Equality((Object) gacha006EffectScene.gacha2d, (Object) null))
    {
      future = Res.Animations.Gacha_Picup.gacha_unit_spawn.Load<GameObject>();
      e = future.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      gacha006EffectScene.gacha2d = future.Result.Clone(((Component) gacha006EffectScene.mainPanel).transform);
      future = (Future<GameObject>) null;
    }
    gacha006EffectScene.gacha2d.SetActive(false);
    if (Object.op_Equality((Object) gacha006EffectScene.gacha3d, (Object) null))
    {
      future = Res.Animations.New_Gacha.prefab.New_Gacha.Load<GameObject>();
      e = future.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      gacha006EffectScene.gacha3d = future.Result.Clone(((Component) gacha006EffectScene).transform.parent);
      future = (Future<GameObject>) null;
    }
    gacha006EffectScene.gacha3d.transform.localPosition = new Vector3(1000f, 0.0f, 0.0f);
    if (Object.op_Equality((Object) gacha006EffectScene.menu.Effect, (Object) null))
      gacha006EffectScene.menu.Effect = gacha006EffectScene.gacha3d.GetComponent<EffectControllerGacha>();
    gacha006EffectScene.menu.Effect.Detail = gacha006EffectScene.gacha2d.GetComponent<EffectControllerGachaDetail>();
    gacha006EffectScene.menu.Effect.Detail.Controller = gacha006EffectScene.menu.Effect;
    gacha006EffectScene.menu.Effect.Menu = gacha006EffectScene.menu;
    ((Behaviour) gacha006EffectScene.menu.Effect).enabled = true;
    if (resultData.resultList.Length <= 10)
    {
      e = gacha006EffectScene.menu.SetEffectData(resultData.resultList);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      List<GachaResultData.Result> resultList = new List<GachaResultData.Result>();
      for (int index = 0; index < resultData.resultList.Length; ++index)
      {
        if (index < 10 || resultData.resultList[index].reward_type_id == 1 && (resultData.resultList[index].is_new || resultData.resultList[index].directionType == GachaDirectionType.pickup))
          resultList.Add(resultData.resultList[index]);
      }
      e = gacha006EffectScene.menu.SetEffectData(resultList.ToArray());
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public void onStartScene()
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public void onStartScene(GachaResultData.ResultData gacha_data) => this.onStartScene();

  public override void onSceneInitialized()
  {
    base.onSceneInitialized();
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }

  public override void onEndScene()
  {
    Time.timeScale = 1f;
    base.onEndScene();
    Singleton<PopupManager>.GetInstance().open((GameObject) null);
  }

  public override IEnumerator onEndSceneAsync()
  {
    yield return (object) new WaitForSeconds(0.5f);
    ((Component) this.menu.Effect).gameObject.SetActive(false);
    this.menu.Effect = (EffectControllerGacha) null;
  }
}
