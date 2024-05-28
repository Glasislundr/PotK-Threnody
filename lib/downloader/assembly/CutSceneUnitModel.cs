// Decompiled with JetBrains decompiler
// Type: CutSceneUnitModel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CutSceneUnitModel : NGDuelUnitBase
{
  private GameObject mMainCameraObj;
  private GameObject mSubCameraObj;
  private Animator mCameraAnimator;
  private BattleskillDuelEffect mSkillEffect;
  private bool hasVehicleAnime;
  private Dictionary<string, ClipEventEffectData> mPreLoadClipDataList = new Dictionary<string, ClipEventEffectData>();
  private Dictionary<string, GameObject> mPreLoadEffectPrefabList = new Dictionary<string, GameObject>();

  public UnitUnit Unit => this.mMyUnitData.unit;

  public Transform Bip => this.mBipTransform;

  public Transform Root3D => this.mRoot3D.transform;

  public IEnumerator Initialize(
    BL.Unit unit,
    BL.Skill skill,
    GameObject root3d,
    GameObject mainCameraObj,
    GameObject subCameraObj)
  {
    CutSceneUnitModel cutSceneUnitModel = this;
    cutSceneUnitModel.mMyUnitData = unit;
    cutSceneUnitModel.mRoot3D = root3d;
    cutSceneUnitModel.mCharacterAnimator = ((Component) cutSceneUnitModel).gameObject.GetComponent<Animator>();
    cutSceneUnitModel.mBipTransform = ((Component) cutSceneUnitModel).transform.GetChildInFind("Bip");
    cutSceneUnitModel.mIsMonster = cutSceneUnitModel.mMyUnitData.unit.character.category == UnitCategory.enemy;
    ((Component) cutSceneUnitModel).transform.localScale = new Vector3(cutSceneUnitModel.mMyUnitData.unit.duel_model_scale, cutSceneUnitModel.mMyUnitData.unit.duel_model_scale, cutSceneUnitModel.mMyUnitData.unit.duel_model_scale);
    cutSceneUnitModel.setupBodyMeshEffect();
    cutSceneUnitModel.loadDuelConfig();
    IEnumerator e = cutSceneUnitModel.createHelm();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = cutSceneUnitModel.createArmor();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = cutSceneUnitModel.createShield();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = cutSceneUnitModel.createWeapon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = cutSceneUnitModel.createVehicle();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = cutSceneUnitModel.createDuelShadow();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    cutSceneUnitModel.mSkillEffect = skill.skill.duel_effect;
    cutSceneUnitModel.mMainCameraObj = mainCameraObj;
    cutSceneUnitModel.mCameraAnimator = cutSceneUnitModel.mMainCameraObj.GetComponent<Animator>();
    cutSceneUnitModel.mSubCameraObj = subCameraObj;
    e = cutSceneUnitModel.CreateCutinObject();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = cutSceneUnitModel.SetupCameraAnime();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = cutSceneUnitModel.SetupUnitAnime();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = cutSceneUnitModel.PreLoadClipEventEffect();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator SetupCameraAnime()
  {
    Future<RuntimeAnimatorController> fc = Singleton<ResourceManager>.GetInstance().Load<RuntimeAnimatorController>(this.mSkillEffect.duel_camera_animator_name);
    IEnumerator e = fc.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mCameraAnimator.runtimeAnimatorController = fc.Result;
  }

  private IEnumerator SetupUnitAnime()
  {
    CutSceneUnitModel cutSceneUnitModel = this;
    Object.Destroy((Object) ((Component) cutSceneUnitModel.mCharacterAnimator).gameObject.GetComponent<clipEffectPlayer>());
    ((Component) cutSceneUnitModel.mCharacterAnimator).gameObject.AddComponent<CutSceneClipEffectPlayer>();
    Transform transform = cutSceneUnitModel.baseGameObject.transform;
    transform.position = Vector3.zero;
    transform.rotation = Quaternion.identity;
    if (cutSceneUnitModel.mSkillEffect.vehicle_link_off && Object.op_Inequality((Object) cutSceneUnitModel.mVehicleObject, (Object) null))
    {
      ((Component) cutSceneUnitModel).transform.parent = cutSceneUnitModel.mVehicleObject.transform.parent;
      ((Component) cutSceneUnitModel).transform.localPosition = Vector3.zero;
      ((Component) cutSceneUnitModel).transform.localRotation = Quaternion.identity;
    }
    Future<RuntimeAnimatorController> fu = Singleton<ResourceManager>.GetInstance().Load<RuntimeAnimatorController>(cutSceneUnitModel.mSkillEffect.duel_animator_name);
    IEnumerator e = fu.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    cutSceneUnitModel.mCharacterAnimator.runtimeAnimatorController = fu.Result;
    if (!string.IsNullOrEmpty(cutSceneUnitModel.mSkillEffect.duel_vehicle_animator_name) && Object.op_Inequality((Object) cutSceneUnitModel.mVehicleAnimator, (Object) null))
    {
      Future<RuntimeAnimatorController> fv = Singleton<ResourceManager>.GetInstance().Load<RuntimeAnimatorController>(cutSceneUnitModel.mSkillEffect.duel_vehicle_animator_name);
      e = fv.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      cutSceneUnitModel.mVehicleAnimator.runtimeAnimatorController = fv.Result;
      cutSceneUnitModel.hasVehicleAnime = true;
      fv = (Future<RuntimeAnimatorController>) null;
    }
  }

  public void StartAnime()
  {
    this.mCameraAnimator.SetTrigger("Start");
    this.mCharacterAnimator.SetTrigger("Start");
    if (!this.hasVehicleAnime)
      return;
    this.mVehicleAnimator.SetTrigger("Start");
  }

  public float GetAnimeSpan() => this.mSkillEffect.duel_koyu_wait_time;

  private void LateUpdate() => this.adjustShadowObj();

  public ClipEventEffectData GetPreLoadClipDataList(string name)
  {
    return !this.mPreLoadClipDataList.ContainsKey(name) ? (ClipEventEffectData) null : this.mPreLoadClipDataList[name];
  }

  public GameObject GetPreLoadEffectPrefab(string name)
  {
    return !this.mPreLoadEffectPrefabList.ContainsKey(name) ? (GameObject) null : this.mPreLoadEffectPrefabList[name];
  }

  private IEnumerator PreLoadClipEventEffect()
  {
    IEnumerator e;
    foreach (UnitUnit preloadCutinUnit in this.mSkillEffect.preloadCutinUnitList)
    {
      e = preloadCutinUnit.LoadCutin().Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    foreach (string preloadEffectFileName in this.mSkillEffect.preloadEffectFileNameList)
    {
      e = this.PreLoadEffectPrefab(preloadEffectFileName);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    foreach (string listName in this.mSkillEffect.preloadClipEventEffectDataFileNameList)
    {
      if (!this.mPreLoadClipDataList.ContainsKey(listName))
      {
        Future<ClipEventEffectData> ft = Singleton<ResourceManager>.GetInstance().Load<ClipEventEffectData>(string.Format("BattleEffects/duel/{0}", (object) listName));
        e = ft.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        ClipEventEffectData result = ft.Result;
        this.mPreLoadClipDataList.Add(listName, result);
        foreach (ClipEventEffectData.EffectData data in result.dataList)
        {
          e = this.PreLoadEffectPrefab(data.effect_name);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        ft = (Future<ClipEventEffectData>) null;
      }
    }
  }

  private IEnumerator PreLoadEffectPrefab(string effectName)
  {
    if (!this.mPreLoadEffectPrefabList.ContainsKey(effectName))
    {
      Future<GameObject> ft = Singleton<ResourceManager>.GetInstance().Load<GameObject>(string.Format("BattleEffects/duel/{0}", (object) effectName));
      IEnumerator e = ft.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mPreLoadEffectPrefabList.Add(effectName, ft.Result);
      ft = (Future<GameObject>) null;
    }
  }
}
