// Decompiled with JetBrains decompiler
// Type: Unit004JobChangeScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using JobChangeData;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit004JobChangeScene : NGSceneBase
{
  private static readonly string DefaultName = "unit004_JobChange";
  [SerializeField]
  private float waitEndLoading_ = 2f;
  private int? playerUnitId_;
  private Unit004JobChangeMenu menu_;
  private GameObject background_;

  public static void changeSceneBySelector(int playerUnitId, int? jobId = null)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(Unit004JobChangeScene.DefaultName, false, (object) new SceneParam(playerUnitId, jobId, (Action) (() => Unit004JobChangeUnitSelectScene.changeScene(false))));
  }

  public static void changeScene(
    bool isStack,
    int playerUnitId,
    int? jobId = null,
    Action eventBackScene = null)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(Unit004JobChangeScene.DefaultName, (isStack ? 1 : 0) != 0, (object) new SceneParam(playerUnitId, jobId, eventBackScene));
  }

  public IEnumerator onStartSceneAsync(SceneParam param)
  {
    Unit004JobChangeScene unit004JobChangeScene = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) null;
    yield return (object) unit004JobChangeScene.setBackground();
    unit004JobChangeScene.menu_ = unit004JobChangeScene.menuBase as Unit004JobChangeMenu;
    if (!unit004JobChangeScene.playerUnitId_.HasValue || unit004JobChangeScene.playerUnitId_.Value != param.playerUnitId_ || unit004JobChangeScene.menu_.isNeedReset)
    {
      unit004JobChangeScene.playerUnitId_ = new int?(param.playerUnitId_);
      yield return (object) unit004JobChangeScene.menu_.initializeAsync(param);
    }
    else
      yield return (object) unit004JobChangeScene.menu_.repairAsync(true);
    unit004JobChangeScene.AutoOpenUnityToutaPopupCoroutine();
  }

  private IEnumerator setBackground()
  {
    Unit004JobChangeScene unit004JobChangeScene = this;
    if (Object.op_Equality((Object) unit004JobChangeScene.background_, (Object) null))
    {
      Future<GameObject> ldPrefab = new ResourceObject("Prefabs/BackGround/UnitBackground_jobChange").Load<GameObject>();
      yield return (object) ldPrefab.Wait();
      unit004JobChangeScene.background_ = ldPrefab.Result;
      ldPrefab = (Future<GameObject>) null;
    }
    if (Object.op_Inequality((Object) unit004JobChangeScene.background_, (Object) null))
      unit004JobChangeScene.backgroundPrefab = unit004JobChangeScene.background_;
  }

  public void onStartScene(SceneParam param) => this.StartCoroutine(this.doWaitEndLoading());

  private IEnumerator doWaitEndLoading()
  {
    yield return (object) new WaitForSeconds(this.waitEndLoading_);
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public IEnumerator onBackSceneAsync(SceneParam param)
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) null;
    yield return (object) this.setBackground();
    if (this.menu_.isNeedReset)
      yield return (object) this.menu_.initializeAsync(param);
    else
      yield return (object) this.menu_.repairAsync(false);
    this.AutoOpenUnityToutaPopupCoroutine();
  }

  public void onBackScene(SceneParam param) => this.StartCoroutine(this.doWaitEndLoading());

  public override IEnumerator onEndSceneAsync()
  {
    yield break;
  }

  private void AutoOpenUnityToutaPopupCoroutine()
  {
    NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
    if (instance.fromPopup != NGGameDataManager.FromPopup.Unit004JobChangeScene)
      return;
    instance.OnceOpenPopup<GameObject>(new GameObject[2]
    {
      this.menu_.unityDetailPrefab,
      this.menu_.stageItemPrefab
    }, (NGMenuBase) null, (Action) null);
    instance.fromPopup = NGGameDataManager.FromPopup.None;
  }
}
