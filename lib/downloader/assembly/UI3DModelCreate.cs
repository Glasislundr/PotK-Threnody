// Decompiled with JetBrains decompiler
// Type: UI3DModelCreate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class UI3DModelCreate : MonoBehaviour
{
  [SerializeField]
  private GameObject base_model_;
  [SerializeField]
  protected GameObject unit_model_;
  [SerializeField]
  private GameObject equip_model_;
  [SerializeField]
  private GameObject tiara_model_;
  [SerializeField]
  private GameObject equip_gear_model_;
  [SerializeField]
  private GameObject equip_shield_model_;
  [SerializeField]
  private GameObject vehicle_model_;
  [SerializeField]
  private GameObject unit_effect_model_;
  [SerializeField]
  protected Animator unit_animator_;
  [SerializeField]
  protected Animator vehicle_animator_;
  private Future<GameObject> unit_;
  private Future<GameObject> equip_;
  private Future<GameObject> equip_gear_;
  private Future<GameObject> equip_shield_;
  private Future<GameObject> vehicle_;
  private Future<GameObject> tiara_;
  private string effect_node_ = string.Empty;
  private Future<GameObject> unit_effect_;
  private Future<GameObject> unit_awake_effect_;
  protected Future<RuntimeAnimatorController> unit_runtime_;
  protected Future<RuntimeAnimatorController> vehicle_runtime_;
  protected clipEffectPlayer clip_effect_player;
  protected bool is_left_hand;
  protected bool is_dual_wield;
  protected bool is_rainbow;
  public bool winAnimator_;

  public GameObject BaseModel
  {
    get => this.base_model_;
    set => this.base_model_ = value;
  }

  public GameObject UnitModel
  {
    get => this.unit_model_;
    set => this.unit_model_ = value;
  }

  public GameObject VehicleModel
  {
    get => this.vehicle_model_;
    set => this.vehicle_model_ = value;
  }

  public GameObject EquipGearModel
  {
    get => this.equip_gear_model_;
    set => this.equip_gear_model_ = value;
  }

  public GameObject EquipShieldModel => this.equip_shield_model_;

  public GameObject EquipModel
  {
    get => this.equip_model_;
    set => this.equip_model_ = value;
  }

  public GameObject UnitEffectModel
  {
    get => this.unit_effect_model_;
    set => this.unit_effect_model_ = value;
  }

  public GameObject TiaraModel
  {
    get => this.tiara_model_;
    set => this.tiara_model_ = value;
  }

  public Animator UnitAnimator
  {
    get => this.unit_animator_;
    set => this.unit_animator_ = value;
  }

  public Animator VehicleAnimator
  {
    get => this.vehicle_animator_;
    set => this.vehicle_animator_ = value;
  }

  public void SetNull()
  {
    this.unit_ = (Future<GameObject>) null;
    this.equip_ = (Future<GameObject>) null;
    this.equip_gear_ = (Future<GameObject>) null;
    this.vehicle_ = (Future<GameObject>) null;
    this.effect_node_ = string.Empty;
    this.unit_effect_ = (Future<GameObject>) null;
    this.unit_awake_effect_ = (Future<GameObject>) null;
    this.unit_animator_ = (Animator) null;
    this.vehicle_animator_ = (Animator) null;
    this.unit_model_ = (GameObject) null;
    this.equip_model_ = (GameObject) null;
    this.unit_effect_model_ = (GameObject) null;
    this.equip_gear_model_ = (GameObject) null;
    this.equip_shield_model_ = (GameObject) null;
    this.vehicle_model_ = (GameObject) null;
    this.unit_runtime_ = (Future<RuntimeAnimatorController>) null;
    this.vehicle_runtime_ = (Future<RuntimeAnimatorController>) null;
  }

  protected IEnumerator InitUnitUnitAnimator(UnitUnit unit_data, int job_id)
  {
    this.unit_runtime_ = !this.winAnimator_ ? unit_data.LoadInitialDuelAnimator(job_id) : unit_data.LoadInitialWinAnimator(job_id);
    IEnumerator e = this.unit_runtime_.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.vehicle_runtime_ = unit_data.LoadAnimatorControllerDuelVehicle(unit_data.GetInitialGear(job_id).model_kind);
    e = this.vehicle_runtime_.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected IEnumerator InitPlayerUnitAnimator(PlayerUnit player_unit_data)
  {
    this.unit_runtime_ = !this.winAnimator_ ? player_unit_data.LoadDuelAnimator() : player_unit_data.LoadWinAnimator();
    IEnumerator e = this.unit_runtime_.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.vehicle_runtime_ = player_unit_data.unit.LoadAnimatorControllerDuelVehicle(player_unit_data.equippedWeaponGearOrInitial.model_kind);
    e = this.vehicle_runtime_.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected IEnumerator InitUnitUnitModel(UnitUnit unit_data, int job_id)
  {
    this.unit_ = unit_data.LoadModelDuel(job_id);
    IEnumerator e = this.unit_.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.vehicle_ = unit_data.LoadModelDuelVehicle();
    e = this.vehicle_.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.equip_ = unit_data.LoadModelDuelEquip();
    e = this.equip_.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.tiara_ = unit_data.LoadModelDuelEquipB();
    e = this.tiara_.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unit_effect_ = unit_data.LoadModelUnitAuraEffect(out this.effect_node_, job_id);
    e = this.unit_effect_.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unit_awake_effect_ = unit_data.LoadAwakeEffect();
    e = this.unit_awake_effect_.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected IEnumerator InitUnitModel(PlayerUnit playerUnit)
  {
    this.unit_ = playerUnit.LoadModelDuel();
    IEnumerator e = this.unit_.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.vehicle_ = playerUnit.unit.LoadModelDuelVehicle();
    e = this.vehicle_.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.equip_ = playerUnit.unit.LoadModelDuelEquip();
    e = this.equip_.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.tiara_ = playerUnit.unit.LoadModelDuelEquipB();
    e = this.tiara_.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unit_effect_ = playerUnit.LoadModelUnitAuraEffect(out this.effect_node_);
    e = this.unit_effect_.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unit_awake_effect_ = playerUnit.unit.LoadAwakeEffect();
    e = this.unit_awake_effect_.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator InitGear(UnitUnit unit_data)
  {
    this.unit_ = unit_data.LoadModelDuel();
    IEnumerator e = this.unit_.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.vehicle_ = unit_data.LoadModelDuelVehicle();
    e = this.vehicle_.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.equip_ = unit_data.LoadModelDuelEquip();
    e = this.equip_.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.tiara_ = unit_data.LoadModelDuelEquipB();
    e = this.tiara_.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unit_effect_ = unit_data.LoadModelUnitAuraEffect(out this.effect_node_);
    e = this.unit_effect_.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unit_awake_effect_ = unit_data.LoadAwakeEffect();
    e = this.unit_awake_effect_.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected IEnumerator LoadGear(GearGear gear_data)
  {
    if (Singleton<ResourceManager>.GetInstance().Contains(string.Format("Gears/{0}/3D/prefab", (object) gear_data.resource_reference_gear_id.ID)))
    {
      this.equip_gear_ = gear_data.LoadModel();
      IEnumerator e = this.equip_gear_.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public IEnumerator CreateModel(UnitUnit unit_data, int job_id = 0)
  {
    this.SetNull();
    GearGear initialGear = unit_data.GetInitialGear(job_id);
    IEnumerator e = this.InitUnitUnitAnimator(unit_data, job_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.InitUnitUnitModel(unit_data, job_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (unit_data.non_disp_weapon == 0)
    {
      e = this.LoadGear(initialGear);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.is_left_hand = unit_data.isLeftHandInitialWeapon(job_id);
    this.is_dual_wield = unit_data.isDualWieldInitialWeapon(job_id);
    this.is_rainbow = unit_data.rainbow_on;
    Object.Destroy((Object) this.BaseModel);
    Object.Destroy((Object) this.UnitModel);
    this.Create(unit_data);
    this.SetGear(initialGear.kind, unit_data.character.category);
    this.clip_effect_player.DeteilUnit = unit_data;
  }

  public IEnumerator CreateModel(PlayerUnit player_unit_data, int? setupShield = null)
  {
    this.SetNull();
    IEnumerator e = this.InitPlayerUnitAnimator(player_unit_data);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.InitUnitModel(player_unit_data);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (player_unit_data.unit.non_disp_weapon == 0)
    {
      e = this.LoadGear(player_unit_data.equippedWeaponGearOrInitial);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (setupShield.HasValue)
      {
        GearGear shieldGearOrNull = player_unit_data.equippedShieldGearOrNull;
        if (shieldGearOrNull == null)
          MasterData.GearGear.TryGetValue(setupShield.Value, out shieldGearOrNull);
        if (shieldGearOrNull != null)
        {
          this.equip_shield_ = shieldGearOrNull.LoadModel();
          yield return (object) this.equip_shield_.Wait();
        }
      }
    }
    this.is_left_hand = player_unit_data.isLeftHandWeapon;
    this.is_dual_wield = player_unit_data.isDualWieldWeapon;
    this.is_rainbow = player_unit_data.unit.rainbow_on;
    this.Create(player_unit_data.unit);
    this.SetGear(player_unit_data.equippedWeaponGearOrInitial.kind, player_unit_data.unit.character.category);
    this.SetShield();
    this.clip_effect_player.DeteilUnit = player_unit_data.unit;
  }

  protected void Create(UnitUnit unit_data)
  {
    this.base_model_ = (Object.op_Implicit((Object) this.vehicle_.Result) ? this.vehicle_.Result : this.unit_.Result).Clone(((Component) this).transform);
    this.unit_model_ = this.base_model_;
    this.unit_animator_ = this.unit_model_.GetComponentInChildren<Animator>();
    this.unit_animator_.runtimeAnimatorController = this.unit_runtime_.Result;
    this.clip_effect_player = ((Component) this.unit_animator_).gameObject.GetComponent<clipEffectPlayer>();
    if (Object.op_Equality((Object) this.clip_effect_player, (Object) null))
      this.clip_effect_player = ((Component) this.unit_animator_).gameObject.AddComponent<clipEffectPlayer>();
    if (Object.op_Inequality((Object) this.vehicle_.Result, (Object) null))
    {
      Transform childInFind = this.unit_model_.transform.GetChildInFind("ridePoint");
      if (Object.op_Inequality((Object) childInFind, (Object) null))
      {
        this.vehicle_model_ = this.unit_model_;
        this.vehicle_animator_ = this.vehicle_model_.GetComponentInChildren<Animator>();
        this.vehicle_animator_.runtimeAnimatorController = this.vehicle_runtime_.Result;
        this.unit_model_ = this.unit_.Result.Clone(childInFind);
        this.unit_animator_ = this.unit_model_.GetComponentInChildren<Animator>();
        this.unit_animator_.runtimeAnimatorController = this.unit_runtime_.Result;
        this.unit_model_.transform.localPosition = new Vector3(0.0f, -0.8f, 0.0f);
        this.clip_effect_player = ((Component) this.unit_animator_).gameObject.GetComponent<clipEffectPlayer>();
        if (Object.op_Equality((Object) this.clip_effect_player, (Object) null))
          this.clip_effect_player = ((Component) this.unit_animator_).gameObject.AddComponent<clipEffectPlayer>();
      }
      else
        Debug.LogWarning((object) "raidePoint が　みつからない。");
    }
    SkinnedMeshRenderer componentInChildren = this.unit_model_.GetComponentInChildren<SkinnedMeshRenderer>();
    MaterialPropertyBlock materialPropertyBlock1 = new MaterialPropertyBlock();
    materialPropertyBlock1.SetFloat("_rainbow_On", this.is_rainbow ? 1f : 0.0f);
    MaterialPropertyBlock materialPropertyBlock2 = materialPropertyBlock1;
    ((Renderer) componentInChildren).SetPropertyBlock(materialPropertyBlock2);
    Transform childInFind1 = this.unit_model_.transform.GetChildInFind("equippoint_a");
    if (Object.op_Inequality((Object) childInFind1, (Object) null))
      this.equip_model_ = this.equip_.Result.Clone(childInFind1);
    Transform childInFind2 = this.unit_model_.transform.GetChildInFind("equippoint_b");
    if (Object.op_Inequality((Object) childInFind2, (Object) null))
      this.tiara_model_ = this.tiara_.Result.Clone(childInFind2);
    Transform parent = (Transform) null;
    if (!string.IsNullOrEmpty(this.effect_node_))
      parent = this.unit_model_.transform.GetChildInFind(this.effect_node_);
    if (Object.op_Equality((Object) parent, (Object) null))
      parent = this.unit_model_.transform.GetChildInFind("Bip");
    if (Object.op_Equality((Object) parent, (Object) null))
      parent = this.unit_model_.transform.GetChildInFind("bip");
    if (Object.op_Inequality((Object) parent, (Object) null))
      this.unit_effect_model_ = this.unit_effect_.Result.Clone(parent);
    unit_data.ProcessAttachAwakeEffect(this.unit_model_, this.unit_awake_effect_.Result);
  }

  public void ResetAnimator()
  {
    this.unit_animator_.runtimeAnimatorController = this.unit_runtime_.Result;
    this.unit_animator_.Play("wait", 0, 0.0f);
    if (!Object.op_Inequality((Object) this.vehicle_animator_, (Object) null))
      return;
    this.vehicle_animator_.Play("wait", 0, 0.0f);
  }

  public void SetGear(GearKind gear_data, UnitCategory category)
  {
    Object.Destroy((Object) this.equip_gear_model_);
    this.equip_gear_model_ = (GameObject) null;
    if (this.equip_gear_ == null)
      return;
    if (this.is_dual_wield)
    {
      Transform childInFind1 = this.unit_model_.transform.GetChildInFind("weaponl");
      Transform childInFind2 = this.unit_model_.transform.GetChildInFind("weaponr");
      this.SetGearPoint(childInFind1, gear_data, category);
      this.SetGearPoint(childInFind2, gear_data, category);
    }
    else
      this.SetGearPoint(this.unit_model_.transform.GetChildInFind(this.is_left_hand ? "weaponl" : "weaponr"), gear_data, category);
  }

  private void SetGearPoint(Transform gearPoint, GearKind gear_data, UnitCategory category)
  {
    if (Object.op_Equality((Object) gearPoint, (Object) null))
      return;
    if (this.equip_gear_.HasResult)
      this.equip_gear_model_ = this.equip_gear_.Result.Clone(gearPoint);
    if (Object.op_Equality((Object) this.equip_gear_model_, (Object) null))
      return;
    switch (gear_data.Enum)
    {
      case GearKindEnum.sword:
      case GearKindEnum.axe:
      case GearKindEnum.spear:
      case GearKindEnum.staff:
      case GearKindEnum.shield:
      case GearKindEnum.unique_wepon:
      case GearKindEnum.smith:
        if (category == UnitCategory.player)
        {
          this.equip_gear_model_.transform.localRotation = Quaternion.Euler(0.0f, 180f, 0.0f);
          break;
        }
        this.equip_gear_model_.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 180f);
        break;
      case GearKindEnum.bow:
        this.equip_gear_model_.transform.localRotation = Quaternion.Euler(0.0f, -90f, -90f);
        break;
      case GearKindEnum.gun:
        this.equip_gear_model_.transform.localRotation = Quaternion.Euler(0.0f, 90f, -90f);
        break;
    }
  }

  private void SetShield()
  {
    if (Object.op_Inequality((Object) this.equip_shield_model_, (Object) null))
    {
      Object.Destroy((Object) this.equip_shield_model_);
      this.equip_shield_model_ = (GameObject) null;
    }
    if (this.equip_shield_ == null)
      return;
    this.equip_shield_model_ = this.equip_shield_.Result.Clone(this.unit_model_.transform.GetChildInFind("weaponl"));
    this.equip_shield_model_.SetActive(false);
  }

  private void OnEnable()
  {
    RenderSettings.ambientLight = Consts.GetInstance().UI3DMODEL_AMBIENT_COLOR;
  }

  private void OnDisable()
  {
    if (UI3DModel.countEnabled != 0)
      return;
    RenderSettings.ambientLight = Consts.GetInstance().UI3DMODEL_DEFAULT_AMBIENT_COLOR;
  }
}
