// Decompiled with JetBrains decompiler
// Type: EffectController
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
public class EffectController : MonoBehaviour
{
  public UI3DModelCreate model_creater;
  public bool isAnimation;
  public GameObject back_button_;

  protected virtual void Action()
  {
  }

  public virtual void OnAnimationEnd()
  {
    if (Object.op_Inequality((Object) this.back_button_, (Object) null))
      this.back_button_.SetActive(true);
    this.isAnimation = false;
  }

  protected virtual void OnEnable() => UICamera.fallThrough = ((Component) this).gameObject;

  protected virtual void OnDisable()
  {
    UICamera.fallThrough = (GameObject) null;
    Object.Destroy((Object) ((Component) this).gameObject);
  }

  protected IEnumerator SetTextureGearBasic(int gear_id, MeshRenderer mesh_renderer)
  {
    IEnumerator e = this.SetTexture(MasterData.GearGear[gear_id].LoadSpriteBasic(), mesh_renderer);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected IEnumerator SetTextureGearThum(int gear_id, MeshRenderer mesh_renderer)
  {
    IEnumerator e = this.SetTexture(MasterData.GearGear[gear_id].LoadSpriteThumbnail(), mesh_renderer);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected IEnumerator SetTextureSupplyThum(int supply_id, MeshRenderer mesh_renderer)
  {
    IEnumerator e = this.SetTexture(MasterData.SupplySupply[supply_id].LoadSpriteThumbnail(), mesh_renderer);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected IEnumerator SetTexture(Future<Sprite> sprite, MeshRenderer mesh_renderer)
  {
    IEnumerator e = sprite.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Renderer) mesh_renderer).material.mainTexture = (Texture) sprite.Result.texture;
  }

  protected IEnumerator SetTextureUnit(int unit_id, MeshRenderer mesh_renderer, int job_id = -1)
  {
    UnitUnit unitUnit = MasterData.UnitUnit[unit_id];
    int num = job_id > 0 ? job_id : unitUnit.job_UnitJob;
    IEnumerator e;
    if (unitUnit.IsMaterialUnit)
    {
      e = this.SetTexture(unitUnit.LoadSpriteMedium(num, 1f), mesh_renderer);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = this.SetTexture(unitUnit.LoadFullSprite(num, 1f), mesh_renderer);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  protected IEnumerator SetTextureUnitThum(int unit_id, MeshRenderer mesh_renderer)
  {
    IEnumerator e = this.SetTexture(MasterData.UnitUnit[unit_id].LoadSpriteThumbnail(), mesh_renderer);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected IEnumerator SetTextureUnitCutin(int unit_id, MeshRenderer mesh_renderer)
  {
    IEnumerator e = this.SetTexture(MasterData.UnitUnit[unit_id].LoadCutin(), mesh_renderer);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected void SetModelCreate()
  {
    if (!Object.op_Equality((Object) this.model_creater, (Object) null))
      return;
    if (Object.op_Equality((Object) ((Component) this).GetComponent<UI3DModelCreate>(), (Object) null))
      this.model_creater = ((Component) this).gameObject.AddComponent<UI3DModelCreate>();
    else
      this.model_creater = ((Component) this).GetComponent<UI3DModelCreate>();
  }

  protected IEnumerator CreateModel(Transform model_trans, PlayerUnit player_unit, bool flag_happy)
  {
    this.SetModelCreate();
    IEnumerator e = this.model_creater.CreateModel(player_unit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject baseModel = this.model_creater.BaseModel;
    baseModel.SetParent(((Component) model_trans).gameObject);
    this.SetLayer(baseModel.transform, ((Component) model_trans).gameObject.layer);
    if (flag_happy)
    {
      e = this.SetAnimator(player_unit.LoadWinAnimator());
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  protected IEnumerator SetAnimator(Future<RuntimeAnimatorController> animator)
  {
    Future<RuntimeAnimatorController> animator_controller = animator;
    IEnumerator e = animator_controller.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.model_creater.UnitAnimator.runtimeAnimatorController = animator_controller.Result;
  }

  protected void SetLayer(Transform trans, int layer)
  {
    ((Component) trans).gameObject.layer = layer;
    foreach (Transform tran in trans)
      this.SetLayer(tran, layer);
  }

  protected virtual AnimationItemIcon SetCloneItemIcon(
    AnimationItemIcon icon,
    Transform trans,
    GameObject obj,
    GameCore.ItemInfo item,
    bool new_flag = false)
  {
    if (Object.op_Equality((Object) icon, (Object) null))
      icon = obj.Clone(((Component) trans).transform).GetComponent<AnimationItemIcon>();
    ((Component) icon).transform.localPosition = new Vector3(0.0f, 0.0f, -0.05f);
    ((Object) ((Component) icon).gameObject).name = "AnimationItemIcon";
    icon.Set(item, new_flag);
    this.SetLayer(((Component) icon).gameObject.transform, ((Component) trans).gameObject.layer);
    return icon;
  }

  protected virtual AnimationUnitIcon SetCloneUnitIcon(
    AnimationUnitIcon icon,
    Transform trans,
    GameObject obj,
    PlayerUnit unit,
    bool new_flag = false)
  {
    if (Object.op_Equality((Object) icon, (Object) null))
      icon = obj.Clone(((Component) trans).transform).GetComponent<AnimationUnitIcon>();
    ((Component) icon).transform.position = new Vector3(((Component) trans).transform.position.x, ((Component) trans).transform.position.y, ((Component) trans).transform.position.z - 0.05f);
    ((Object) ((Component) icon).gameObject).name = "AnimationUnitIcon";
    icon.Set(unit, new_flag);
    this.SetLayer(((Component) icon).gameObject.transform, ((Component) trans).gameObject.layer);
    return icon;
  }

  protected virtual AnimationUnitIcon SetCloneUnitIcon(
    AnimationUnitIcon icon,
    Transform trans,
    GameObject obj,
    UnitUnit unit,
    bool new_flag = false)
  {
    if (Object.op_Equality((Object) icon, (Object) null))
      icon = obj.Clone(((Component) trans).transform).GetComponent<AnimationUnitIcon>();
    ((Component) icon).transform.position = new Vector3(((Component) trans).transform.position.x, ((Component) trans).transform.position.y, ((Component) trans).transform.position.z - 0.05f);
    ((Object) ((Component) icon).gameObject).name = "AnimationUnitIcon";
    icon.Set(unit, new_flag);
    this.SetLayer(((Component) icon).gameObject.transform, ((Component) trans).gameObject.layer);
    return icon;
  }

  protected void SetAoriEffect(List<GameObject> obj_list, string[] anim_pattern)
  {
    foreach (string str in anim_pattern)
    {
      switch (str)
      {
        case "blue":
          obj_list.ForEachIndex<GameObject>((System.Action<GameObject, int>) ((u, n) => u.SetActive(n == 0)));
          break;
        case "yellow":
          obj_list.ForEachIndex<GameObject>((System.Action<GameObject, int>) ((u, n) => u.SetActive(n == 1)));
          break;
        case "red":
          obj_list.ForEachIndex<GameObject>((System.Action<GameObject, int>) ((u, n) => u.SetActive(n == 2)));
          break;
      }
    }
  }

  protected void SetCommonRarity(List<GameObject> obj_list, int rare_index)
  {
    obj_list.ForEachIndex<GameObject>((System.Action<GameObject, int>) ((u, n) => u.SetActive(n == rare_index)));
  }

  protected void SetActiveList(List<GameObject> obj_list, int rare_index)
  {
    obj_list.ForEachIndex<GameObject>((System.Action<GameObject, int>) ((u, n) => u.SetActive(n == rare_index)));
  }
}
