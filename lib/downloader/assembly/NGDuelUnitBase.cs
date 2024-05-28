// Decompiled with JetBrains decompiler
// Type: NGDuelUnitBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

#nullable disable
public abstract class NGDuelUnitBase : MonoBehaviour
{
  public BL.Unit mMyUnitData;
  protected GameObject mRoot3D;
  protected GameObject mVehicleObject;
  protected GameObject[] mWeaponObject = new GameObject[2];
  protected GameObject mShieldObject;
  protected DuelCutin mDuelCI;
  protected DuelCutin mDuelCINew;
  protected GameObject mEffectShadow;
  protected Animator mCharacterAnimator;
  protected Animator mVehicleAnimator;
  protected DuelDuelConfig mConfig;
  public DuelTime mDuelTime;
  protected bool mIsMonster;
  protected static Action<MaterialPropertyBlock, Material, string> SetColorFunc = (Action<MaterialPropertyBlock, Material, string>) ((block, material, key) =>
  {
    if (!material.HasProperty(key))
      return;
    Color color = material.GetColor(key);
    if ((double) color.r == 0.0 && (double) color.g == 0.0 && (double) color.b == 0.0 && (double) color.a == 0.0)
      return;
    block.SetColor(key, color);
  });
  protected static Action<MaterialPropertyBlock, Material, string> SetFloatFunc = (Action<MaterialPropertyBlock, Material, string>) ((block, material, key) =>
  {
    if (!material.HasProperty(key))
      return;
    float num = material.GetFloat(key);
    if ((double) num == 0.0)
      return;
    block.SetFloat(key, num);
  });
  protected static Action<MaterialPropertyBlock, Material, string> SetVectorFunc = (Action<MaterialPropertyBlock, Material, string>) ((block, material, key) =>
  {
    if (!material.HasProperty(key))
      return;
    Vector4 vector = material.GetVector(key);
    if ((double) vector.x == 0.0 && (double) vector.y == 0.0 && (double) vector.z == 0.0 && (double) vector.w == 0.0)
      return;
    block.SetVector(key, vector);
  });
  protected static Func<string, Action<MaterialPropertyBlock, Material, string>, Action<MaterialPropertyBlock, Material>> CreateFunc = (Func<string, Action<MaterialPropertyBlock, Material, string>, Action<MaterialPropertyBlock, Material>>) ((key, func) => (Action<MaterialPropertyBlock, Material>) ((block, material) => func(block, material, key)));
  protected static readonly Action<MaterialPropertyBlock, Material>[] SetMaterialDefaultVariableFuncs = new Action<MaterialPropertyBlock, Material>[10]
  {
    NGDuelUnitBase.CreateFunc("_Color", NGDuelUnitBase.SetColorFunc),
    NGDuelUnitBase.CreateFunc("_body_color", NGDuelUnitBase.SetColorFunc),
    NGDuelUnitBase.CreateFunc("_RimColor", NGDuelUnitBase.SetColorFunc),
    NGDuelUnitBase.CreateFunc("_Rim_Color", NGDuelUnitBase.SetColorFunc),
    NGDuelUnitBase.CreateFunc("_Rim_Exp", NGDuelUnitBase.SetFloatFunc),
    NGDuelUnitBase.CreateFunc("_alpha", NGDuelUnitBase.SetFloatFunc),
    NGDuelUnitBase.CreateFunc("_Alpha", NGDuelUnitBase.SetFloatFunc),
    NGDuelUnitBase.CreateFunc("_EyeLight_MainColor", NGDuelUnitBase.SetColorFunc),
    NGDuelUnitBase.CreateFunc("_EyeLight_EyeColor", NGDuelUnitBase.SetColorFunc),
    NGDuelUnitBase.CreateFunc("_Tile", NGDuelUnitBase.SetVectorFunc)
  };

  public Transform mBipTransform { get; protected set; }

  public GameObject baseGameObject
  {
    get
    {
      return !Object.op_Inequality((Object) this.mVehicleObject, (Object) null) ? ((Component) this).gameObject : this.mVehicleObject;
    }
  }

  protected void loadDuelConfig()
  {
    SkillMetamorphosis metamorphosis = this.mMyUnitData.metamorphosis;
    string duelAnimatorControllerName = this.mMyUnitData.playerUnit.getDuelAnimatorControllerName(metamorphosis != null ? metamorphosis.metamorphosis_id : 0);
    this.mConfig = Array.Find<DuelDuelConfig>(MasterData.DuelDuelConfigList, (Predicate<DuelDuelConfig>) (x => x.controller_name == duelAnimatorControllerName));
    if (this.mConfig == null)
      this.mConfig = Array.Find<DuelDuelConfig>(MasterData.DuelDuelConfigList, (Predicate<DuelDuelConfig>) (x => x.controller_name == "Anim_dummy"));
    this.mDuelTime = new DuelTime(this.mConfig);
  }

  protected IEnumerator createVehicle()
  {
    NGDuelUnitBase ngDuelUnitBase = this;
    if (!string.IsNullOrEmpty(ngDuelUnitBase.mMyUnitData.unit.vehicle_model_name))
    {
      Future<GameObject> fs = ngDuelUnitBase.mMyUnitData.unit.LoadModelDuelVehicle();
      IEnumerator e = fs.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = fs.Result;
      if (Object.op_Equality((Object) result, (Object) null))
      {
        Debug.LogError((object) string.Format("Do not load vehicle_model_name={0}", (object) ngDuelUnitBase.mMyUnitData.unit.vehicle_model_name));
      }
      else
      {
        ngDuelUnitBase.mVehicleObject = result.Clone(ngDuelUnitBase.mRoot3D.transform);
        ngDuelUnitBase.mVehicleObject.transform.position = ((Component) ngDuelUnitBase).transform.position;
        ngDuelUnitBase.mVehicleObject.transform.rotation = ((Component) ngDuelUnitBase).transform.rotation;
        Transform childInFind = ngDuelUnitBase.mVehicleObject.transform.GetChildInFind("ridePoint");
        ((Component) ngDuelUnitBase).transform.parent = childInFind;
        ((Component) ngDuelUnitBase).transform.localScale = Vector3.one;
        ((Component) ngDuelUnitBase).transform.localPosition = new Vector3(0.0f, -0.8f, 0.0f);
        ((Component) ngDuelUnitBase).transform.localRotation = Quaternion.identity;
        ngDuelUnitBase.mVehicleAnimator = ngDuelUnitBase.mVehicleObject.GetComponent<Animator>();
        ((Component) ngDuelUnitBase.mVehicleAnimator).gameObject.GetOrAddComponent<clipEffectPlayer>();
        Future<RuntimeAnimatorController> vac = ngDuelUnitBase.mMyUnitData.unit.LoadAnimatorControllerDuelVehicle(ngDuelUnitBase.mMyUnitData.playerUnit.equippedWeaponGearOrInitial.model_kind);
        e = vac.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (!Object.op_Equality((Object) vac.Result, (Object) null))
        {
          ngDuelUnitBase.mVehicleAnimator.runtimeAnimatorController = vac.Result;
          fs = (Future<GameObject>) null;
          vac = (Future<RuntimeAnimatorController>) null;
        }
      }
    }
  }

  protected virtual IEnumerator createWeapon()
  {
    NGDuelUnitBase ngDuelUnitBase = this;
    if (ngDuelUnitBase.mMyUnitData.unit.non_disp_weapon <= 0)
    {
      Future<GameObject> pg = ngDuelUnitBase.mMyUnitData.playerUnit.equippedWeaponGearOrInitial.LoadModel();
      IEnumerator e = pg.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = pg.Result;
      Transform[] transformArray = new Transform[2];
      if (ngDuelUnitBase.mMyUnitData.playerUnit.isDualWieldWeapon)
      {
        transformArray[0] = ((Component) ngDuelUnitBase).transform.GetChildInFind("weaponl");
        transformArray[1] = ((Component) ngDuelUnitBase).transform.GetChildInFind("weaponr");
      }
      else if (ngDuelUnitBase.mMyUnitData.playerUnit.isLeftHandWeapon)
        transformArray[0] = ((Component) ngDuelUnitBase).transform.GetChildInFind("weaponl");
      else
        transformArray[1] = ((Component) ngDuelUnitBase).transform.GetChildInFind("weaponr");
      for (int index = 0; index < transformArray.Length; ++index)
      {
        if (!Object.op_Equality((Object) transformArray[index], (Object) null))
        {
          GameObject gameObject = result.Clone(transformArray[index]);
          if (!Object.op_Equality((Object) gameObject, (Object) null))
          {
            Quaternion quaternion;
            switch ((GearKindEnum) ngDuelUnitBase.mMyUnitData.playerUnit.unit.kind_GearKind)
            {
              case GearKindEnum.bow:
                quaternion = Quaternion.Euler(0.0f, -90f, -90f);
                break;
              case GearKindEnum.gun:
                quaternion = Quaternion.Euler(0.0f, 90f, -90f);
                break;
              default:
                quaternion = ngDuelUnitBase.mIsMonster ? Quaternion.Euler(0.0f, 0.0f, 180f) : Quaternion.Euler(0.0f, -180f, 0.0f);
                break;
            }
            gameObject.transform.localRotation = quaternion;
            ngDuelUnitBase.mWeaponObject[index] = gameObject;
          }
        }
      }
    }
  }

  protected IEnumerator createShield()
  {
    NGDuelUnitBase ngDuelUnitBase = this;
    if (ngDuelUnitBase.mMyUnitData.unit.non_disp_weapon <= 0)
    {
      GearGear shieldGearOrNull = ngDuelUnitBase.mMyUnitData.playerUnit.equippedShieldGearOrNull;
      if (shieldGearOrNull != null)
      {
        Future<GameObject> pg = shieldGearOrNull.LoadModel();
        IEnumerator e = pg.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        GameObject result = pg.Result;
        ngDuelUnitBase.mShieldObject = result.Clone(((Component) ngDuelUnitBase).transform.GetChildInFind("weaponl"));
        ngDuelUnitBase.mShieldObject.SetActive(false);
      }
    }
  }

  protected IEnumerator createArmor()
  {
    NGDuelUnitBase ngDuelUnitBase = this;
    if (!string.IsNullOrEmpty(ngDuelUnitBase.mMyUnitData.unit.equip_model_name))
    {
      Future<GameObject> fs = ngDuelUnitBase.mMyUnitData.unit.LoadModelDuelEquip();
      IEnumerator e = fs.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = fs.Result;
      if (Object.op_Inequality((Object) result, (Object) null))
      {
        result.transform.position = Vector3.zero;
        Transform childInFind = ((Component) ngDuelUnitBase).transform.GetChildInFind("equippoint_a");
        result.Clone(childInFind);
      }
      fs = (Future<GameObject>) null;
    }
  }

  protected IEnumerator createHelm()
  {
    NGDuelUnitBase ngDuelUnitBase = this;
    if (!string.IsNullOrEmpty(ngDuelUnitBase.mMyUnitData.unit.equip_model_b_name))
    {
      Future<GameObject> fs = ngDuelUnitBase.mMyUnitData.unit.LoadModelDuelEquipB();
      IEnumerator e = fs.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = fs.Result;
      if (Object.op_Inequality((Object) result, (Object) null))
      {
        result.transform.position = Vector3.zero;
        Transform childInFind = ((Component) ngDuelUnitBase).transform.GetChildInFind("equippoint_b");
        result.Clone(childInFind);
      }
      fs = (Future<GameObject>) null;
    }
  }

  protected IEnumerator loadDefaultAnimatorController()
  {
    NGDuelUnitBase ngDuelUnitBase = this;
    Animator myAnimator = ((Component) ngDuelUnitBase).GetComponentInChildren<Animator>();
    if (!Object.op_Equality((Object) myAnimator, (Object) null))
    {
      SkillMetamorphosis metamorphosis = ngDuelUnitBase.mMyUnitData.metamorphosis;
      int metamorphosisId = metamorphosis != null ? metamorphosis.metamorphosis_id : 0;
      Future<RuntimeAnimatorController> frac = ngDuelUnitBase.mMyUnitData.playerUnit.LoadDuelAnimator(metamorphosisId);
      IEnumerator e = frac.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      myAnimator.runtimeAnimatorController = frac.Result;
      Object.op_Equality((Object) myAnimator.avatar, (Object) null);
      int num = myAnimator.avatar.isValid ? 1 : 0;
    }
  }

  protected void setupBodyMeshEffect()
  {
    if (this.mMyUnitData.unit.character.category != UnitCategory.player)
      return;
    SkinnedMeshRenderer componentInChildren = ((Component) this).gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
    MaterialPropertyBlock block = new MaterialPropertyBlock();
    block.SetFloat("_rainbow_On", this.mMyUnitData.unit.rainbow_on ? 1f : 0.0f);
    NGDuelUnitBase.SetPropertyBlockMaterialDefault(block, ((Renderer) componentInChildren).materials);
    ((Renderer) componentInChildren).SetPropertyBlock(block);
  }

  protected IEnumerator createDuelShadow()
  {
    NGDuelUnitBase ngDuelUnitBase = this;
    Future<GameObject> ft = Singleton<ResourceManager>.GetInstance().Load<GameObject>("BattleEffects/duel/unit_shadow_duel");
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!Object.op_Equality((Object) ft.Result, (Object) null))
    {
      Transform parent = Object.op_Inequality((Object) ngDuelUnitBase.mBipTransform, (Object) null) ? ngDuelUnitBase.mBipTransform : ((Component) ngDuelUnitBase).transform;
      ngDuelUnitBase.mEffectShadow = ft.Result.Clone(parent);
      ngDuelUnitBase.mEffectShadow.transform.localScale = new Vector3(ngDuelUnitBase.mMyUnitData.unit.duel_shadow_scale_x, ngDuelUnitBase.mMyUnitData.unit.duel_shadow_scale_z, 1f);
    }
  }

  protected void adjustShadowObj()
  {
    if (!Object.op_Inequality((Object) this.mEffectShadow, (Object) null))
      return;
    Vector3 position = this.mBipTransform.position;
    position.y = 0.0f;
    this.mEffectShadow.transform.position = position;
    this.mEffectShadow.transform.rotation = Quaternion.Euler(new Vector3(-90f, 0.0f, 0.0f));
    this.mEffectShadow.transform.position = new Vector3(this.mEffectShadow.transform.position.x, 0.05f, this.mEffectShadow.transform.position.z);
  }

  public virtual void SetActiveEquipeWeapon(bool active)
  {
    foreach (GameObject gameObject in this.mWeaponObject)
    {
      if (Object.op_Inequality((Object) gameObject, (Object) null))
        gameObject.SetActive(active);
    }
  }

  public virtual void SetActiveShadow(bool active)
  {
    if (!Object.op_Inequality((Object) this.mEffectShadow, (Object) null))
      return;
    this.mEffectShadow.SetActive(active);
  }

  protected virtual IEnumerator CreateCutinObject()
  {
    NGDuelUnitBase ngDuelUnitBase1 = this;
    if ((double) ngDuelUnitBase1.mDuelTime.criticalCutinWaitTime > 0.0)
    {
      NGDuelUnitBase ngDuelUnitBase = ngDuelUnitBase1;
      Future<GameObject> fs = (Future<GameObject>) null;
      SkillMetamorphosis metamorphosis = ngDuelUnitBase1.mMyUnitData.metamorphosis;
      int metamor_id = metamorphosis != null ? metamorphosis.metamorphosis_id : 0;
      UnitCutinInfo unitCutinInfo = (metamor_id != 0 ? Array.Find<UnitModel>(MasterData.UnitModelList, (Predicate<UnitModel>) (x => x.unit_id_UnitUnit == ngDuelUnitBase.mMyUnitData.unitId && x.job_metamor_id == metamor_id)) : (UnitModel) null)?.cutin_frame ?? ngDuelUnitBase1.mMyUnitData.playerUnit.CutinInfo;
      IEnumerator e;
      if (!string.IsNullOrEmpty(unitCutinInfo.prefab_name))
      {
        fs = new ResourceObject("Animations/battle_cutin/" + unitCutinInfo.prefab_name).Load<GameObject>();
        e = fs.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (Object.op_Inequality((Object) fs.Result, (Object) null))
        {
          GameObject gameObject = fs.Result.Clone(ngDuelUnitBase1.mRoot3D.transform);
          gameObject.transform.localPosition = Vector3.zero;
          gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 180f, 0.0f));
          gameObject.SetActive(false);
          ngDuelUnitBase1.mDuelCINew = gameObject.GetComponent<DuelCutin>();
          e = ngDuelUnitBase1.mDuelCINew.Initialize<BL.Unit>(ngDuelUnitBase1.mMyUnitData);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
      fs = ngDuelUnitBase1.mMyUnitData.unit.character.gender != UnitGender.male ? Res.Animations.battle_cutin.battle_cutin_prefab.Load<GameObject>() : Res.Animations.battle_cutin.battle_cutin_male_prefab.Load<GameObject>();
      e = fs.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (Object.op_Inequality((Object) fs.Result, (Object) null))
      {
        GameObject gameObject = fs.Result.Clone(ngDuelUnitBase1.mRoot3D.transform);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 180f, 0.0f));
        gameObject.SetActive(false);
        ngDuelUnitBase1.mDuelCI = gameObject.GetComponent<DuelCutin>();
        e = ngDuelUnitBase1.mDuelCI.Initialize<BL.Unit>(ngDuelUnitBase1.mMyUnitData);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      fs = (Future<GameObject>) null;
    }
  }

  public bool PlayCriticalCutin()
  {
    DuelCutin duelCutin = Object.op_Inequality((Object) this.mDuelCINew, (Object) null) ? this.mDuelCINew : (Object.op_Inequality((Object) this.mDuelCI, (Object) null) ? this.mDuelCI : (DuelCutin) null);
    if (!Object.op_Inequality((Object) duelCutin, (Object) null) || !duelCutin.isTexExist)
      return false;
    ((Component) duelCutin).gameObject.SetActive(true);
    duelCutin.PlayCriticalCutin();
    return true;
  }

  public bool PlaySkillCutin()
  {
    DuelCutin duelCutin = Object.op_Inequality((Object) this.mDuelCINew, (Object) null) ? this.mDuelCINew : (Object.op_Inequality((Object) this.mDuelCI, (Object) null) ? this.mDuelCI : (DuelCutin) null);
    if (!Object.op_Inequality((Object) duelCutin, (Object) null) || !duelCutin.isTexExist)
      return false;
    ((Component) duelCutin).gameObject.SetActive(true);
    duelCutin.PlaySkillCutin();
    return true;
  }

  public bool PlayMultiSkillCutin(int topUnitId, int centerUnitId)
  {
    DuelCutin mDuelCi = Object.op_Inequality((Object) this.mDuelCI, (Object) null) ? this.mDuelCI : (DuelCutin) null;
    if (!Object.op_Inequality((Object) mDuelCi, (Object) null))
      return false;
    this.SetCutinTexture(mDuelCi, topUnitId, DuelCutin.CUTINPOS.TOP);
    this.SetCutinTexture(mDuelCi, centerUnitId, DuelCutin.CUTINPOS.CENTER);
    ((Component) mDuelCi).gameObject.SetActive(true);
    mDuelCi.PlaySkillCutin(DuelCutin.PlayMode.SECOND_PERSON);
    return true;
  }

  public bool PlayMultiSkillCutin(int topUnitId, int centerUnitId, int bottomUnitId)
  {
    DuelCutin mDuelCi = Object.op_Inequality((Object) this.mDuelCI, (Object) null) ? this.mDuelCI : (DuelCutin) null;
    if (!Object.op_Inequality((Object) mDuelCi, (Object) null))
      return false;
    this.SetCutinTexture(mDuelCi, topUnitId, DuelCutin.CUTINPOS.TOP);
    this.SetCutinTexture(mDuelCi, centerUnitId, DuelCutin.CUTINPOS.CENTER);
    this.SetCutinTexture(mDuelCi, bottomUnitId, DuelCutin.CUTINPOS.BOTTOM);
    ((Component) mDuelCi).gameObject.SetActive(true);
    mDuelCi.PlaySkillCutin(DuelCutin.PlayMode.THIRD_PERSON);
    return true;
  }

  protected virtual void SetCutinTexture(DuelCutin cutin, int unitId, DuelCutin.CUTINPOS pos)
  {
    this.StartCoroutine(cutin.LoadAndSetTexture(unitId, pos));
  }

  protected static void SetPropertyBlockMaterialDefault(
    MaterialPropertyBlock block,
    Material[] materials)
  {
    foreach (Material material in materials)
    {
      foreach (Action<MaterialPropertyBlock, Material> defaultVariableFunc in NGDuelUnitBase.SetMaterialDefaultVariableFuncs)
        defaultVariableFunc(block, material);
    }
  }

  [Conditional("UNITY_EDITOR")]
  [Conditional("DEVELOP_BUILD")]
  protected void DuelUnitLog(string log)
  {
  }
}
