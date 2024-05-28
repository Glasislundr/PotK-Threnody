// Decompiled with JetBrains decompiler
// Type: EffectControllerArmorSythesis
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

#nullable disable
public class EffectControllerArmorSythesis : EffectController
{
  [SerializeField]
  private GameObject is_new_;
  [SerializeField]
  private GameObject animation_root_;
  [SerializeField]
  private GameObject armor_sythesis_prefab_;
  [SerializeField]
  private Transform armor_sythesis_trans_;
  [SerializeField]
  private CommonRarityAnim common_rarity_anim_;
  [SerializeField]
  private ArmorSythesisAnim armor_sythesis_anim_;
  [SerializeField]
  private int rarity;
  [SerializeField]
  private int gear_id_;
  [SerializeField]
  private GameObject remainingManaSeedContainer;
  [SerializeField]
  private UILabel remainingManaSeedLabel;
  public ArmorSythesis sound_manager_;
  private GameObject AnimationItemIconPrefab;

  private void Awake()
  {
    UIRoot componentInParent = ((Component) this.remainingManaSeedLabel).GetComponentInParent<UIRoot>();
    if (Object.op_Inequality((Object) componentInParent, (Object) null))
    {
      UIRoot component = ((Component) Singleton<CommonRoot>.GetInstance()).GetComponent<UIRoot>();
      componentInParent.manualHeight = component.manualHeight;
      componentInParent.minimumHeight = component.minimumHeight;
    }
    this.remainingManaSeedContainer.transform.localPosition = new Vector3(this.remainingManaSeedContainer.transform.localPosition.x, (IOSUtil.IsDeviceGenerationiPhoneX ? (float) (componentInParent.manualHeight - 144) : (float) componentInParent.manualHeight) / 960f * this.remainingManaSeedContainer.transform.localPosition.y, this.remainingManaSeedContainer.transform.localPosition.z);
  }

  public void EndSE() => this.sound_manager_.OnRarity();

  private IEnumerator initialize()
  {
    EffectControllerArmorSythesis controllerArmorSythesis = this;
    controllerArmorSythesis.animation_root_.SetActive(false);
    Future<GameObject> armor_f;
    IEnumerator e;
    if (Object.op_Equality((Object) controllerArmorSythesis.armor_sythesis_prefab_, (Object) null))
    {
      armor_f = Res.Prefabs.ArmorSythesis.ArmorSythesisAnimation.Load<GameObject>();
      e = armor_f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      controllerArmorSythesis.armor_sythesis_prefab_ = armor_f.Result;
      armor_f = (Future<GameObject>) null;
    }
    GameObject gameObject = controllerArmorSythesis.armor_sythesis_prefab_.Clone(controllerArmorSythesis.armor_sythesis_trans_);
    ((Object) gameObject).name = ((Object) controllerArmorSythesis.armor_sythesis_prefab_).name;
    gameObject.transform.localEulerAngles = Vector3.zero;
    controllerArmorSythesis.armor_sythesis_anim_ = gameObject.GetComponent<ArmorSythesisAnim>();
    if (Object.op_Equality((Object) controllerArmorSythesis.AnimationItemIconPrefab, (Object) null))
    {
      armor_f = Res.Prefabs.ArmorRepair.AnimationItemIcon.Load<GameObject>();
      e = armor_f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      controllerArmorSythesis.AnimationItemIconPrefab = armor_f.Result;
      armor_f = (Future<GameObject>) null;
    }
    if (controllerArmorSythesis.armor_sythesis_anim_.mesh_id_list != null)
      controllerArmorSythesis.armor_sythesis_anim_.mesh_id_list.Clear();
    controllerArmorSythesis.isAnimation = true;
  }

  public IEnumerator Set(
    List<ItemInfo> thum_list,
    bool is_new,
    ItemInfo result_item,
    GameObject back_button,
    string[] anim_pattern,
    ItemInfo baseItem)
  {
    EffectControllerArmorSythesis controllerArmorSythesis = this;
    controllerArmorSythesis.back_button_ = back_button;
    controllerArmorSythesis.back_button_.SetActive(true);
    controllerArmorSythesis.sound_manager_.result = false;
    IEnumerator e = controllerArmorSythesis.initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    int kindGearKind;
    if (result_item.gear == null)
    {
      controllerArmorSythesis.gear_id_ = MasterData.GearGear[11001].ID;
      kindGearKind = MasterData.GearGear[11001].kind_GearKind;
    }
    else
    {
      controllerArmorSythesis.gear_id_ = result_item.gear.ID;
      kindGearKind = result_item.gear.kind_GearKind;
    }
    controllerArmorSythesis.is_new_.SetActive(is_new);
    controllerArmorSythesis.rarity = result_item.gear.rarity.index;
    if (kindGearKind == 9)
    {
      e = controllerArmorSythesis.SetTextureGearBasic(controllerArmorSythesis.gear_id_, controllerArmorSythesis.common_rarity_anim_.image400_);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = controllerArmorSythesis.SetTextureGearBasic(controllerArmorSythesis.gear_id_, controllerArmorSythesis.common_rarity_anim_.image400_blur_);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ((Component) controllerArmorSythesis.common_rarity_anim_.image_).gameObject.SetActive(false);
      ((Component) controllerArmorSythesis.common_rarity_anim_.image400_).gameObject.SetActive(true);
    }
    else
    {
      e = controllerArmorSythesis.SetTextureGearBasic(controllerArmorSythesis.gear_id_, controllerArmorSythesis.common_rarity_anim_.image_);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = controllerArmorSythesis.SetTextureGearBasic(controllerArmorSythesis.gear_id_, controllerArmorSythesis.common_rarity_anim_.image_blur_);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ((Component) controllerArmorSythesis.common_rarity_anim_.image_).gameObject.SetActive(true);
      ((Component) controllerArmorSythesis.common_rarity_anim_.image400_).gameObject.SetActive(false);
    }
    for (int i = 0; i < controllerArmorSythesis.armor_sythesis_anim_.thum_list.Count; ++i)
    {
      if (i < thum_list.Count)
      {
        ((Component) controllerArmorSythesis.armor_sythesis_anim_.thum_list[i]).gameObject.SetActive(true);
        ArmorSythesisAnim.MeshIDList meshIdList = controllerArmorSythesis.armor_sythesis_anim_.AddMeshIdList(controllerArmorSythesis.armor_sythesis_anim_.thum_list[i], thum_list[i], controllerArmorSythesis.AnimationItemIconPrefab);
        e = controllerArmorSythesis.SetTextureGearThum(meshIdList.itemInfo.gear.ID, meshIdList.mesh);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
        ((Component) controllerArmorSythesis.armor_sythesis_anim_.thum_list[i]).gameObject.SetActive(false);
    }
    if (baseItem != null)
    {
      ((Component) controllerArmorSythesis.armor_sythesis_anim_.target_thum).gameObject.SetActive(true);
      ArmorSythesisAnim.MeshIDList meshIdList = controllerArmorSythesis.armor_sythesis_anim_.AddMeshIdList(controllerArmorSythesis.armor_sythesis_anim_.target_thum, baseItem, controllerArmorSythesis.AnimationItemIconPrefab);
      e = controllerArmorSythesis.SetTextureGearThum(meshIdList.itemInfo.gear.ID, meshIdList.mesh);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    controllerArmorSythesis.SetAoriEffect(controllerArmorSythesis.armor_sythesis_anim_.energy_list, anim_pattern);
    int num = controllerArmorSythesis.rarity > 0 ? controllerArmorSythesis.rarity - 1 : controllerArmorSythesis.rarity;
    controllerArmorSythesis.SetCommonRarity(controllerArmorSythesis.common_rarity_anim_.rarity_obj_list_, num);
    CommonRarityAnim.RarityStart target = controllerArmorSythesis.common_rarity_anim_.rarity_list[num];
    foreach (GameObject rarity in target.rarity_list)
      yield return (object) controllerArmorSythesis.SetRarityInstance(Res.Prefabs.common_animation.common_rarity.raritystar2, rarity, target.commonRariry);
    yield return (object) controllerArmorSythesis.SetRarityInstance(Res.Prefabs.common_animation.common_rarity.raritytext2, target.rariryText2, target.commonRariry);
    yield return (object) controllerArmorSythesis.SetRarityInstance(Res.Prefabs.common_animation.common_rarity.raritytext3, target.rariryText3, target.commonRariry);
    yield return (object) controllerArmorSythesis.SetRarityInstance(Res.Prefabs.common_animation.common_rarity.raritytext4, target.rariryText4, target.commonRariry);
    yield return (object) controllerArmorSythesis.SetRarityInstance(Res.Prefabs.common_animation.common_rarity.raritytext5, target.rariryText5, target.commonRariry);
    yield return (object) controllerArmorSythesis.SetRarityInstance(Res.Prefabs.common_animation.common_rarity.raritytextblue3, target.rariryTextBlue3, target.commonRariry);
    yield return (object) controllerArmorSythesis.SetRarityInstance(Res.Prefabs.common_animation.common_rarity.raritytextblue4, target.rariryTextBlue4, target.commonRariry);
    yield return (object) controllerArmorSythesis.SetRarityInstance(Res.Prefabs.common_animation.common_rarity.raritytextblue5, target.rariryTextBlue5, target.commonRariry);
    controllerArmorSythesis.remainingManaSeedContainer.SetActive(false);
    if (result_item.gear != null && result_item.gear.kind.Enum == GearKindEnum.accessories && result_item.gear.disappearance_type_GearDisappearanceType == 1)
    {
      controllerArmorSythesis.remainingManaSeedContainer.SetActive(true);
      controllerArmorSythesis.remainingManaSeedLabel.SetTextLocalize(result_item.gearAccessoryRemainingAmount);
    }
    controllerArmorSythesis.sound_manager_.SetRarity(controllerArmorSythesis.rarity);
    controllerArmorSythesis.animation_root_.SetActive(true);
  }

  private IEnumerator SetRarityInstance(
    ResourceObject resource,
    GameObject target,
    GameObject commonRariry)
  {
    if (!Object.op_Equality((Object) target, (Object) null))
    {
      Future<GameObject> future = resource.Load<GameObject>();
      IEnumerator e = future.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject gameObject = future.Result.Clone(commonRariry.transform);
      ((Object) gameObject).name = ((Object) target).name;
      gameObject.transform.localScale = target.transform.localScale;
      gameObject.transform.localPosition = target.transform.localPosition;
      gameObject.transform.localRotation = target.transform.localRotation;
      foreach (Transform componentsInChild in target.GetComponentsInChildren<Transform>())
      {
        ((Component) componentsInChild).transform.SetParent(gameObject.transform);
        ((Component) componentsInChild).transform.localScale = Vector3.one;
        ((Component) componentsInChild).transform.localPosition = Vector3.zero;
        ((Component) componentsInChild).transform.localRotation = Quaternion.identity;
      }
      Object.Destroy((Object) target);
    }
  }
}
