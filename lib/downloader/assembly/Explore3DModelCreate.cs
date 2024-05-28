// Decompiled with JetBrains decompiler
// Type: Explore3DModelCreate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Explore3DModelCreate : UI3DModelCreate
{
  [Header("Adjust Position")]
  [SerializeField]
  private Vector3 mSoldierPos;
  [SerializeField]
  private Vector3 mRiderPos;
  [Header("Adjust Rotation")]
  [SerializeField]
  private Vector3 mSoldierRot;
  [SerializeField]
  private Vector3 mRiderRot;
  private UnitUnit unit_data;
  private int job_id;
  private GameObject shadowPrefab;

  public bool IsNoRun { get; private set; }

  public IEnumerator LoadModel(PlayerUnit playerUnit)
  {
    Explore3DModelCreate explore3DmodelCreate = this;
    explore3DmodelCreate.SetNull();
    explore3DmodelCreate.unit_data = playerUnit.unit;
    explore3DmodelCreate.job_id = playerUnit.job_id;
    yield return (object) explore3DmodelCreate.InitPlayerUnitAnimator(playerUnit);
    yield return (object) explore3DmodelCreate.InitUnitModel(playerUnit);
    if (explore3DmodelCreate.unit_data.non_disp_weapon == 0)
      yield return (object) explore3DmodelCreate.LoadGear(playerUnit.equippedWeaponGearOrInitial);
    explore3DmodelCreate.is_left_hand = playerUnit.isLeftHandWeapon;
    explore3DmodelCreate.is_dual_wield = playerUnit.isDualWieldWeapon;
    explore3DmodelCreate.is_rainbow = playerUnit.unit.rainbow_on;
    if (Object.op_Equality((Object) explore3DmodelCreate.shadowPrefab, (Object) null))
    {
      Future<GameObject> loader = Singleton<ResourceManager>.GetInstance().Load<GameObject>("BattleEffects/duel/unit_shadow_duel");
      IEnumerator e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      explore3DmodelCreate.shadowPrefab = loader.Result;
      loader = (Future<GameObject>) null;
    }
  }

  public void CreateModel()
  {
    this.DestroyModel();
    this.Create(this.unit_data);
    this.SetGear(this.unit_data.initial_gear.kind, this.unit_data.character.category);
    this.setUnitShadow();
    this.clip_effect_player.DeteilUnit = this.unit_data;
  }

  public IEnumerator CreateExploreModel()
  {
    Explore3DModelCreate explore3DmodelCreate = this;
    explore3DmodelCreate.DestroyModel();
    explore3DmodelCreate.Create(explore3DmodelCreate.unit_data);
    string animatorControllerName = explore3DmodelCreate.getCommonAnimatorControllerName();
    if (!string.IsNullOrEmpty(animatorControllerName))
      yield return (object) explore3DmodelCreate.loadCommonAnimatorController(animatorControllerName);
    explore3DmodelCreate.IsNoRun = explore3DmodelCreate.checkNoRun();
    explore3DmodelCreate.SetGear(explore3DmodelCreate.unit_data.initial_gear.kind, explore3DmodelCreate.unit_data.character.category);
    explore3DmodelCreate.setUnitShadow();
    explore3DmodelCreate.clip_effect_player.DeteilUnit = explore3DmodelCreate.unit_data;
  }

  private string getCommonAnimatorControllerName()
  {
    string controllerName = ((Object) this.unit_animator_.runtimeAnimatorController).name;
    ExploreCommonAnimation exploreCommonAnimation = ((IEnumerable<ExploreCommonAnimation>) MasterData.ExploreCommonAnimationList).FirstOrDefault<ExploreCommonAnimation>((Func<ExploreCommonAnimation, bool>) (x => x.target.Equals(controllerName)));
    if (exploreCommonAnimation != null)
      return !string.IsNullOrEmpty(exploreCommonAnimation.changed) ? exploreCommonAnimation.changed : string.Empty;
    MasterDataTable.UnitJob unitJob;
    if (Object.op_Inequality((Object) this.vehicle_animator_, (Object) null) || MasterData.UnitJob.TryGetValue(this.job_id, out unitJob) && unitJob.move_type == UnitMoveType.fuyu)
      return string.Empty;
    foreach (AnimatorControllerParameter parameter in this.unit_animator_.parameters)
    {
      if (parameter.name.Equals("to_run"))
        return string.Empty;
    }
    return "explore_senpei";
  }

  private bool checkNoRun()
  {
    string controllerName = ((Object) this.unit_animator_.runtimeAnimatorController).name;
    ExploreCommonAnimation exploreCommonAnimation = ((IEnumerable<ExploreCommonAnimation>) MasterData.ExploreCommonAnimationList).FirstOrDefault<ExploreCommonAnimation>((Func<ExploreCommonAnimation, bool>) (x => x.target.Equals(controllerName)));
    if (exploreCommonAnimation != null)
      return exploreCommonAnimation.no_run;
    MasterDataTable.UnitJob unitJob;
    return MasterData.UnitJob.TryGetValue(this.job_id, out unitJob) && unitJob.move_type == UnitMoveType.fuyu;
  }

  private IEnumerator loadCommonAnimatorController(string fileName)
  {
    Explore3DModelCreate explore3DmodelCreate = this;
    explore3DmodelCreate.unit_runtime_ = Singleton<ResourceManager>.GetInstance().Load<RuntimeAnimatorController>("Animators/explore/" + fileName);
    IEnumerator e = explore3DmodelCreate.unit_runtime_.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    explore3DmodelCreate.unit_animator_.runtimeAnimatorController = explore3DmodelCreate.unit_runtime_.Result;
  }

  private void setUnitShadow()
  {
    Transform transform = this.BaseModel.gameObject.transform.GetChildInFind("Bip");
    if (Object.op_Equality((Object) null, (Object) transform))
      transform = this.BaseModel.gameObject.transform;
    GameObject gameObject = this.shadowPrefab.Clone(this.BaseModel.transform);
    Vector3 localPosition = ((Component) transform).transform.localPosition;
    localPosition.y = 0.05f;
    gameObject.transform.localPosition = localPosition;
    gameObject.transform.rotation = Quaternion.Euler(new Vector3(-90f, 0.0f, 90f));
    gameObject.transform.localScale = new Vector3(this.unit_data.duel_shadow_scale_x, this.unit_data.duel_shadow_scale_z, 1f);
  }

  public void DestroyModel()
  {
    Object.Destroy((Object) this.BaseModel);
    Object.Destroy((Object) this.UnitModel);
  }

  public void AdjustTransform()
  {
    if (!Object.op_Inequality((Object) this.BaseModel, (Object) null))
      return;
    if (Object.op_Inequality((Object) this.VehicleModel, (Object) null))
    {
      this.BaseModel.transform.localPosition = this.mRiderPos;
      this.BaseModel.transform.localRotation = Quaternion.Euler(this.mRiderRot);
    }
    else
    {
      this.BaseModel.transform.localPosition = this.mSoldierPos;
      this.BaseModel.transform.localRotation = Quaternion.Euler(this.mSoldierRot);
    }
  }

  private void OnEnable()
  {
    if (!this.is_rainbow)
      return;
    SkinnedMeshRenderer componentInChildren = this.unit_model_.GetComponentInChildren<SkinnedMeshRenderer>();
    MaterialPropertyBlock materialPropertyBlock1 = new MaterialPropertyBlock();
    materialPropertyBlock1.SetFloat("_rainbow_On", 1f);
    MaterialPropertyBlock materialPropertyBlock2 = materialPropertyBlock1;
    ((Renderer) componentInChildren).SetPropertyBlock(materialPropertyBlock2);
  }

  private void OnDisable()
  {
  }
}
