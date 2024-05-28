// Decompiled with JetBrains decompiler
// Type: ExploreModelController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (ExploreSceneManager))]
public class ExploreModelController : MonoBehaviour
{
  [SerializeField]
  private Transform mRoot3d;
  [SerializeField]
  private Transform mRootExplore;
  [SerializeField]
  private Transform mRootCamp;
  [SerializeField]
  private Transform mRootNonBattle;
  [SerializeField]
  private Transform mTreasureBoxAnchor;
  [Header("Camera")]
  [SerializeField]
  private Camera mMainCamera;
  [SerializeField]
  private Vector3 mMainCameraPosSoldier;
  [SerializeField]
  private Vector3 mMainCameraPosRider;
  [Space(8f)]
  [SerializeField]
  private Explore033TargetLookCamera mTreasureCamera;
  [SerializeField]
  private Vector3 mTreasureCameraPosSoldier;
  [SerializeField]
  private Vector3 mTreasureCameraPosRider;
  [SerializeField]
  private Vector3[] mTreasureCameraPath = new Vector3[0];
  [Space(8f)]
  [SerializeField]
  private Camera mCampCamera;
  [Header("ExclamationMark")]
  [SerializeField]
  private Vector3 mExclamationPosSoldier;
  [SerializeField]
  private Vector3 mExclamationPosRider;
  private GameObject mMapPrefab;
  private GameObject mMapObj;
  private GameObject mFirePrefab;
  private GameObject mStatuePrefab;
  private GameObject mStatueSubPrefab;
  private GameObject mDistantPrefab;
  private GameObject mDistantObj;
  private PlayerUnit mPlayerUnit;
  private GameObject mPlayerUnitModel;
  private Animator mPlayerUnitAnimator;
  private Animator mPlayerVehicleAnimator;
  private string mTriggerRun;
  private GameObject mExclamation;
  private GameObject mExclamationPrefab;
  private Explore3DModelCreate mExploreModelCreate;
  private Animator mTreasureBoxAnimator;
  private PlayerUnit[] mPlayerUnits = new PlayerUnit[5];
  private Explore3DModelCreate[] mCampModelCreates;
  private GameObject mCampBase;

  private void Awake()
  {
    this.mExploreModelCreate = ((Component) this.mRootExplore).GetComponentInChildren<Explore3DModelCreate>();
    this.mCampModelCreates = ((Component) this.mRootCamp).gameObject.GetComponentsInChildren<Explore3DModelCreate>();
  }

  public IEnumerator CreateAllExploreModel()
  {
    yield return (object) this.LoadExploreMapModel();
    this.CreateExploreMap();
    yield return (object) this.CreateTreasureBox();
    this.CloseTreasureBox();
    yield return (object) this.CreateCampBase();
    yield return (object) this.ReLoadAllUnitModels();
  }

  public void SetExploreVisible(bool visible)
  {
    ((Component) this.mRoot3d).gameObject.SetActive(visible);
  }

  public void SetBattleMapMode(bool enable)
  {
    if (enable)
    {
      this.mMapObj.GetComponentInChildren<Animator>().speed = 0.0f;
      this.mDistantObj.GetComponentInChildren<Explore033DistantMap>().speed = 0.0f;
    }
    else
    {
      this.mMapObj.GetComponentInChildren<Animator>().speed = 1f;
      this.mDistantObj.GetComponentInChildren<Explore033DistantMap>().speed = 1f;
    }
    ((Component) this.mRootNonBattle).gameObject.SetActive(!enable);
  }

  public void ResetCameraPosition()
  {
    if (Object.op_Inequality((Object) this.mPlayerVehicleAnimator, (Object) null))
    {
      ((Component) this.mMainCamera).transform.localPosition = this.mMainCameraPosRider;
      ((Component) this.mTreasureCamera).transform.localPosition = this.mTreasureCameraPosRider;
    }
    else
    {
      ((Component) this.mMainCamera).transform.localPosition = this.mMainCameraPosSoldier;
      ((Component) this.mTreasureCamera).transform.localPosition = this.mTreasureCameraPosSoldier;
    }
  }

  public void ChangeMainCamera()
  {
    ((Component) this.mMainCamera).gameObject.SetActive(true);
    ((Component) this.mTreasureCamera).gameObject.SetActive(false);
  }

  public void ChangeTreasureCamera()
  {
    ((Component) this.mTreasureCamera).gameObject.SetActive(true);
    ((Component) this.mMainCamera).gameObject.SetActive(false);
  }

  public void PlayTreasureCamera()
  {
    this.ResetCameraPosition();
    List<Vector3> vector3List = new List<Vector3>()
    {
      ((Component) this.mMainCamera).transform.localPosition
    };
    vector3List.AddRange((IEnumerable<Vector3>) this.mTreasureCameraPath);
    vector3List.Add(((Component) this.mTreasureCamera).transform.localPosition);
    Vector3[] targetPath = new Vector3[2]
    {
      new Vector3(-0.5f, 0.4f, 0.0f),
      new Vector3(this.mTreasureBoxAnchor.localPosition.x, 0.4f, this.mTreasureBoxAnchor.localPosition.z)
    };
    this.mTreasureCamera.MoveByPath(vector3List.ToArray(), targetPath, 0.5f);
  }

  public IEnumerator LoadExploreMapModel()
  {
    ExploreFloor floorData = Singleton<ExploreDataManager>.GetInstance().FloorData;
    Future<GameObject> loader = new ResourceObject(string.Format("BattleMaps/{0}/3D/explore/distant/{1:000}_distant_prefab", (object) floorData.folder_path, (object) floorData.map_distant_id)).Load<GameObject>();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mDistantPrefab = loader.Result;
    loader = (Future<GameObject>) null;
    loader = new ResourceObject(string.Format("BattleMaps/{0}/3D/explore/near/{1:000}_prefab", (object) floorData.folder_path, (object) floorData.map_near_id)).Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mMapPrefab = loader.Result;
    loader = (Future<GameObject>) null;
    loader = new ResourceObject(string.Format("BattleMaps/{0}/3D/explore/near/statue_{1:000}_prefab", (object) floorData.folder_path, (object) floorData.map_statue_id)).Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mStatuePrefab = loader.Result;
    loader = (Future<GameObject>) null;
    loader = new ResourceObject(string.Format("BattleMaps/{0}/3D/explore/near/weapon_{1:000}_prefab", (object) floorData.folder_path, (object) floorData.map_statue_sub_id)).Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mStatueSubPrefab = loader.Result;
    loader = (Future<GameObject>) null;
    loader = new ResourceObject(string.Format("BattleMaps/{0}/3D/explore/near/ef_fire_{1:000}_prefab", (object) floorData.folder_path, (object) floorData.map_fire_id)).Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mFirePrefab = loader.Result;
    loader = (Future<GameObject>) null;
  }

  public void CreateExploreMap()
  {
    if (Object.op_Inequality((Object) this.mDistantObj, (Object) null))
      Object.Destroy((Object) this.mDistantObj);
    this.mDistantObj = this.mDistantPrefab.Clone(this.mRoot3d);
    if (Object.op_Inequality((Object) this.mMapObj, (Object) null))
      Object.Destroy((Object) this.mMapObj);
    this.mMapObj = this.mMapPrefab.Clone(this.mRoot3d);
    Explore033NearMap component = this.mMapObj.GetComponent<Explore033NearMap>();
    component.CloneAndSetStatue(this.mStatuePrefab, this.mStatueSubPrefab);
    component.ApplyColor();
    component.CloneAndSetTorch(this.mFirePrefab);
  }

  public IEnumerator ReLoadAllUnitModels()
  {
    this.SetCampVisible(false);
    PlayerUnit unit = Singleton<ExploreDataManager>.GetInstance().GetExploreMainUnit();
    yield return (object) this.LoadExploreUnitModel(unit);
    yield return (object) this.CreateExploreUnit(unit);
    this.WaitPlayerUnit();
    this.SetCampVisible(true);
    PlayerUnit[] units = Singleton<ExploreDataManager>.GetInstance().GetExploreUnits();
    yield return (object) this.LoadCampUnitModels(units);
    this.CreateCampUnits(units);
    this.SetCampVisible(false);
  }

  public IEnumerator LoadExploreUnitModel(PlayerUnit playerUnit)
  {
    if (playerUnit != this.mPlayerUnit)
    {
      yield return (object) this.mExploreModelCreate.LoadModel(playerUnit);
      if (Object.op_Equality((Object) this.mExclamationPrefab, (Object) null))
      {
        Future<GameObject> loader = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/common/explore_exclamation_mark");
        IEnumerator e = loader.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.mExclamationPrefab = loader.Result;
        loader = (Future<GameObject>) null;
      }
    }
  }

  public IEnumerator CreateExploreUnit(PlayerUnit playerUnit)
  {
    if (playerUnit != this.mPlayerUnit)
    {
      this.mExclamation = (GameObject) null;
      this.mPlayerUnitModel = (GameObject) null;
      yield return (object) this.mExploreModelCreate.CreateExploreModel();
      this.mPlayerUnit = playerUnit;
      this.mPlayerUnitModel = this.mExploreModelCreate.BaseModel;
      this.mPlayerUnitAnimator = this.mExploreModelCreate.UnitAnimator;
      this.mPlayerVehicleAnimator = this.mExploreModelCreate.VehicleAnimator;
      this.mTriggerRun = "to_run";
      string str = "to_explore_run";
      foreach (AnimatorControllerParameter parameter in this.mPlayerUnitAnimator.parameters)
      {
        if (parameter.name.Equals(str))
        {
          this.mTriggerRun = str;
          break;
        }
      }
      this.mPlayerUnitModel.SetParent(((Component) this.mExploreModelCreate).gameObject);
      this.mExploreModelCreate.AdjustTransform();
      Object.Destroy((Object) this.mPlayerUnitModel.GetComponent<clipEffectPlayer>());
      ExploreClipEffectPlayer clipEffectPlayer = this.mPlayerUnitModel.AddComponent<ExploreClipEffectPlayer>();
      clipEffectPlayer.SetUnit(playerUnit.unit);
      switch (Singleton<ExploreDataManager>.GetInstance().FloorData.folder_path)
      {
        case 10001:
          clipEffectPlayer.setGroundStatus(MasterData.BattleLandform[13]);
          break;
        case 10002:
          clipEffectPlayer.setGroundStatus(MasterData.BattleLandform[91]);
          break;
      }
      clipEffectPlayer.IsFootStepSoundOnly = true;
      this.setExclamationMark();
      this.resetCameraPosition();
    }
  }

  private void setExclamationMark()
  {
    this.mExclamation = this.mExclamationPrefab.Clone(this.mPlayerUnitModel.transform);
    this.mExclamation.transform.localPosition = !Object.op_Inequality((Object) this.mPlayerVehicleAnimator, (Object) null) ? this.mExclamationPosSoldier : this.mExclamationPosRider;
    this.mExclamation.GetComponent<Explore033EasyBillBoard>().mTargetCamera = this.mMainCamera;
    this.CloseExclamationMark();
  }

  public void OpenExclamationMark()
  {
    this.mExclamation.SetActive(true);
    Singleton<ExploreSceneManager>.GetInstance().PlaySe("SE_2450");
  }

  public void CloseExclamationMark() => this.mExclamation.SetActive(false);

  private void resetCameraPosition()
  {
    if (Object.op_Inequality((Object) this.mExploreModelCreate.VehicleModel, (Object) null))
    {
      ((Component) this.mMainCamera).transform.localPosition = this.mMainCameraPosRider;
      ((Component) this.mTreasureCamera).transform.localPosition = this.mTreasureCameraPosRider;
    }
    else
    {
      ((Component) this.mMainCamera).transform.localPosition = this.mMainCameraPosSoldier;
      ((Component) this.mTreasureCamera).transform.localPosition = this.mTreasureCameraPosSoldier;
    }
  }

  public void RunPlayerUnit()
  {
    if (!this.mExploreModelCreate.IsNoRun)
    {
      this.resetPlayerUnitAnimationTrigger("to_wait");
      this.setPlayerUnitAnimationTrigger(this.mTriggerRun);
    }
    this.mMapObj.GetComponentInChildren<Animator>().speed = 1f;
    this.mDistantObj.GetComponentInChildren<Explore033DistantMap>().speed = 1f;
  }

  public void WaitPlayerUnit()
  {
    if (!this.mExploreModelCreate.IsNoRun)
    {
      this.setPlayerUnitAnimationTrigger(this.mTriggerRun);
      this.setPlayerUnitAnimationTrigger("to_wait");
    }
    this.mMapObj.GetComponentInChildren<Animator>().speed = 0.0f;
    this.mDistantObj.GetComponentInChildren<Explore033DistantMap>().speed = 0.0f;
  }

  public void WaitCampUnits()
  {
    this.resetCampUnitsAnimationTrigger("to_damagewait");
    this.setCampUnitsAnimationTrigger("to_wait");
  }

  public void LostWaitCampUnits()
  {
    this.resetCampUnitsAnimationTrigger("to_wait");
    this.setCampUnitsAnimationTrigger("to_damagewait");
  }

  private void setPlayerUnitAnimationTrigger(string trigger)
  {
    this.mPlayerUnitAnimator.SetTrigger(trigger);
    if (!Object.op_Inequality((Object) this.mPlayerVehicleAnimator, (Object) null))
      return;
    this.mPlayerVehicleAnimator.SetTrigger(trigger);
  }

  private void resetPlayerUnitAnimationTrigger(string trigger)
  {
    this.mPlayerUnitAnimator.ResetTrigger(trigger);
    if (!Object.op_Inequality((Object) this.mPlayerVehicleAnimator, (Object) null))
      return;
    this.mPlayerVehicleAnimator.ResetTrigger(trigger);
  }

  private void setCampUnitsAnimationTrigger(string trigger)
  {
    foreach (Explore3DModelCreate mCampModelCreate in this.mCampModelCreates)
    {
      Animator unitAnimator = mCampModelCreate.UnitAnimator;
      if (!Object.op_Equality((Object) unitAnimator, (Object) null))
      {
        Animator vehicleAnimator = mCampModelCreate.VehicleAnimator;
        unitAnimator.SetTrigger(trigger);
        if (Object.op_Inequality((Object) vehicleAnimator, (Object) null))
          vehicleAnimator.SetTrigger(trigger);
      }
    }
  }

  private void resetCampUnitsAnimationTrigger(string trigger)
  {
    foreach (Explore3DModelCreate mCampModelCreate in this.mCampModelCreates)
    {
      Animator unitAnimator = mCampModelCreate.UnitAnimator;
      if (!Object.op_Equality((Object) unitAnimator, (Object) null))
      {
        Animator vehicleAnimator = mCampModelCreate.VehicleAnimator;
        unitAnimator.ResetTrigger(trigger);
        if (Object.op_Inequality((Object) vehicleAnimator, (Object) null))
          vehicleAnimator.ResetTrigger(trigger);
      }
    }
  }

  public IEnumerator CreateTreasureBox()
  {
    if (Object.op_Equality((Object) this.mTreasureBoxAnimator, (Object) null))
    {
      Future<GameObject> loader = new ResourceObject("Prefabs/common/explore_treasure_01").Load<GameObject>();
      IEnumerator e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mTreasureBoxAnimator = loader.Result.CloneAndGetComponent<Animator>(this.mTreasureBoxAnchor);
      loader = (Future<GameObject>) null;
    }
  }

  public void OpenTreasureBox()
  {
    ((Component) this.mTreasureBoxAnimator).gameObject.SetActive(true);
    Singleton<ExploreSceneManager>.GetInstance().PlaySe("SE_2451");
  }

  public void CloseTreasureBox()
  {
    ((Component) this.mTreasureBoxAnimator).gameObject.SetActive(false);
  }

  public void SetCampVisible(bool visible)
  {
    ((Component) this.mRootCamp).gameObject.SetActive(visible);
    ((Component) this.mRootExplore).gameObject.SetActive(!visible);
    ((Component) this.mCampCamera).gameObject.SetActive(visible);
    ((Component) this.mMainCamera).gameObject.SetActive(!visible);
    if (visible)
      Singleton<ExploreSceneManager>.GetInstance().PlayLoopSe("SE_2452");
    else
      Singleton<ExploreSceneManager>.GetInstance().StopLoopSe();
  }

  public IEnumerator CreateCampBase()
  {
    Future<GameObject> loader = new ResourceObject(string.Format("Prefabs/common/{0}/explore_camp", (object) Singleton<ExploreDataManager>.GetInstance().FloorData.folder_path)).Load<GameObject>();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    loader.Result.Clone(this.mRootCamp);
  }

  public IEnumerator LoadCampUnitModels(PlayerUnit[] playerUnits)
  {
    int index1 = 0;
    Explore3DModelCreate[] explore3DmodelCreateArray = this.mCampModelCreates;
    for (int index2 = 0; index2 < explore3DmodelCreateArray.Length; ++index2)
    {
      Explore3DModelCreate explore3DmodelCreate = explore3DmodelCreateArray[index2];
      if (index1 < playerUnits.Length && playerUnits[index1] != (PlayerUnit) null && this.mPlayerUnits[index1] != playerUnits[index1])
        yield return (object) explore3DmodelCreate.LoadModel(playerUnits[index1]);
      ++index1;
    }
    explore3DmodelCreateArray = (Explore3DModelCreate[]) null;
  }

  public void CreateCampUnits(PlayerUnit[] playerUnits)
  {
    int index = 0;
    foreach (Explore3DModelCreate mCampModelCreate in this.mCampModelCreates)
    {
      if (index < playerUnits.Length && playerUnits[index] != (PlayerUnit) null)
      {
        if (this.mPlayerUnits[index] != playerUnits[index])
        {
          mCampModelCreate.CreateModel();
          mCampModelCreate.BaseModel.SetParent(((Component) mCampModelCreate).gameObject);
          mCampModelCreate.AdjustTransform();
          this.mPlayerUnits[index] = playerUnits[index];
        }
      }
      else
      {
        mCampModelCreate.DestroyModel();
        this.mPlayerUnits[index] = (PlayerUnit) null;
      }
      ++index;
    }
  }
}
