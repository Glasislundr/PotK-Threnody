// Decompiled with JetBrains decompiler
// Type: Shop00720EffectController
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
public class Shop00720EffectController : EffectController
{
  public string[] textureNameList_1;
  public string[] textureNameList_2;
  public string[] textureNameList_3;
  public int stopTextureId_1;
  public int stopTextureId_2;
  public int stopTextureId_3;
  public int[] transitionPlanList;
  public int rarity;
  public bool loadState;
  [SerializeField]
  private GameObject gb_started;
  private SlotDebug slot_script;
  [SerializeField]
  private Camera camera2D;
  private bool isCutInHiding;
  private List<Shop00720ItemIcon> itemIconList = new List<Shop00720ItemIcon>();
  private PopupSlot100Result slot100Result;
  private Shop00720Menu shopMenu;
  private Shop00720EffectController.CutinType cutinType;
  [SerializeField]
  public DuelCutin duelCutin;
  [SerializeField]
  private GameObject[] bugsCutinInitOff;
  [SerializeField]
  public DuelCutin duelCutinMale;
  [SerializeField]
  private GameObject[] bugsCutinMaleInitOff;
  [SerializeField]
  private GameObject duelCutinNewObj;
  private DuelCutin duelCutinNew;
  private GameObject AnimationUnitIconPrefab;
  private GameObject AnimationItemIconPrefab;
  private GameObject ItemIcon;
  [SerializeField]
  private GameObject new_eft_;
  [SerializeField]
  private List<GetSumContainerList> renpatu_obj_;
  [SerializeField]
  private List<AnimationUnitIcon> renpatu_unit_;
  [SerializeField]
  private List<AnimationItemIcon> renpatu_item_;

  public SlotDebug Slot_script => this.slot_script;

  private void Awake()
  {
    this.slot_script = this.gb_started.GetComponent<SlotDebug>();
    this.StartCoroutine(this.slot_script.Init());
    this.SlotInit();
  }

  public void Bet()
  {
    Debug.Log((object) nameof (Bet));
    this.isCutInHiding = false;
    if (this.slot_script.isReady)
    {
      this.slot_script.SlotStart();
    }
    else
    {
      if (!this.slot_script.isEnd)
        return;
      this.SlotInit();
    }
  }

  public void SlotInit() => this.slot_script.SlotInit();

  public void Skip()
  {
    Debug.Log((object) nameof (Skip));
    this.slot_script.Skip();
  }

  public IEnumerator ResultView100ren()
  {
    Future<GameObject> popupPrefab = Res.Prefabs.popup.popup_medalslot_100result.Load<GameObject>();
    IEnumerator e = popupPrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.slot100Result = Singleton<PopupManager>.GetInstance().open(popupPrefab.Result).GetComponent<PopupSlot100Result>();
    this.slot100Result.Initialized(this.Slot_script, this.shopMenu);
    foreach (Shop00720ItemIcon itemIcon in this.itemIconList)
    {
      ((Component) itemIcon).gameObject.transform.parent = ((Component) this.slot100Result.grid).transform;
      ((Component) itemIcon).gameObject.transform.localPosition = Vector3.zero;
      ((Component) itemIcon).gameObject.transform.localScale = Vector3.one;
      itemIcon.dragScroll.scrollView = this.slot100Result.scrollView;
    }
    this.slot100Result.grid.Reposition();
    foreach (Shop00720ItemIcon itemIcon in this.itemIconList)
      itemIcon.IconView(true);
  }

  public IEnumerator Renpatu(
    WebAPI.Response.SlotS001MedalPayResult[] result_data)
  {
    Shop00720EffectController effectController = this;
    int num1 = result_data.Length - 1;
    foreach (GetSumContainerList sumContainerList in effectController.renpatu_obj_)
    {
      if (Object.op_Inequality((Object) sumContainerList, (Object) null))
        ((Component) sumContainerList).gameObject.SetActive(num1 == 0);
      --num1;
    }
    effectController.new_eft_.SetActive(false);
    Future<GameObject> fIconPrefab;
    IEnumerator e;
    if (result_data.Length > 10)
    {
      effectController.slot_script.slot100 = true;
      if (Object.op_Equality((Object) effectController.ItemIcon, (Object) null))
      {
        fIconPrefab = Res.Animations.Slot_Machines.Machines_Item.Load<GameObject>();
        e = fIconPrefab.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        effectController.ItemIcon = fIconPrefab.Result;
        fIconPrefab = (Future<GameObject>) null;
      }
      Dictionary<int, WebAPI.Response.SlotS001MedalPayResult> dictionary = new Dictionary<int, WebAPI.Response.SlotS001MedalPayResult>();
      foreach (WebAPI.Response.SlotS001MedalPayResult s001MedalPayResult in result_data)
      {
        if (!dictionary.ContainsKey(s001MedalPayResult.reward_result_id))
          dictionary.Add(s001MedalPayResult.reward_result_id, s001MedalPayResult);
      }
      effectController.itemIconList.Clear();
      foreach (WebAPI.Response.SlotS001MedalPayResult s001MedalPayResult in dictionary.Values)
      {
        WebAPI.Response.SlotS001MedalPayResult data = s001MedalPayResult;
        GameObject gameObject = effectController.ItemIcon.Clone();
        int num2 = ((IEnumerable<WebAPI.Response.SlotS001MedalPayResult>) result_data).ToList<WebAPI.Response.SlotS001MedalPayResult>().Count<WebAPI.Response.SlotS001MedalPayResult>((Func<WebAPI.Response.SlotS001MedalPayResult, bool>) (val => val.reward_result_id == data.reward_result_id));
        Shop00720ItemIcon icon = gameObject.GetComponent<Shop00720ItemIcon>();
        e = icon.SetIcon(data, num2);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        icon.IconView(false);
        effectController.itemIconList.Add(icon);
        icon = (Shop00720ItemIcon) null;
      }
    }
    else
    {
      GetSumContainerList renpatu_obj_last = effectController.renpatu_obj_[result_data.Length - 1];
      if (Object.op_Equality((Object) effectController.AnimationItemIconPrefab, (Object) null))
      {
        fIconPrefab = Res.Prefabs.ArmorRepair.AnimationItemIcon.Load<GameObject>();
        e = fIconPrefab.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        effectController.AnimationItemIconPrefab = fIconPrefab.Result;
        fIconPrefab = (Future<GameObject>) null;
      }
      if (Object.op_Equality((Object) effectController.AnimationUnitIconPrefab, (Object) null))
      {
        fIconPrefab = Res.Prefabs.ArmorRepair.AnimationUnitIcon.Load<GameObject>();
        e = fIconPrefab.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        effectController.AnimationUnitIconPrefab = fIconPrefab.Result;
        fIconPrefab = (Future<GameObject>) null;
      }
      int high_rarity = 1;
      for (int i = 0; i < result_data.Length; ++i)
      {
        WebAPI.Response.SlotS001MedalPayResult targetRusult = result_data[i];
        CommonRewardType crt = new CommonRewardType(targetRusult.reward_type_id, targetRusult.reward_result_id, targetRusult.reward_result_quantity, targetRusult.is_new);
        UnitUnit unit = crt.unitUnit;
        GearGear gear = crt.gearGear;
        SupplySupply supply = crt.supplySupply;
        ((Component) renpatu_obj_last.renpatu_unit_light_[i]).gameObject.SetActive(false);
        ((Component) renpatu_obj_last.renpatu_unit_light2_[i]).gameObject.SetActive(false);
        ((Component) renpatu_obj_last.renpatu_item_light_[i]).gameObject.SetActive(false);
        ((Component) renpatu_obj_last.renpatu_item_light2_[i]).gameObject.SetActive(false);
        ((Component) renpatu_obj_last.renpatu_item_other_light_[i]).gameObject.SetActive(false);
        ((Component) renpatu_obj_last.renpatu_item_other_light2_[i]).gameObject.SetActive(false);
        int rarityindex = ((IEnumerable<SlotS001MedalRarity>) MasterData.SlotS001MedalRarityList).SingleOrDefault<SlotS001MedalRarity>((Func<SlotS001MedalRarity, bool>) (sd => sd.ID == targetRusult.rarity_id)).index;
        if (high_rarity < rarityindex)
          high_rarity = rarityindex;
        ((Component) renpatu_obj_last.renpatu_trans_[i]).gameObject.SetActive(true);
        Shop00720EffectRarity component = ((Component) renpatu_obj_last.renpatu_trans_[i]).GetComponent<Shop00720EffectRarity>();
        component.Init();
        component.setRarity(rarityindex);
        if (Object.op_Inequality((Object) effectController.renpatu_unit_[i], (Object) null))
          ((Component) effectController.renpatu_unit_[i]).gameObject.SetActive(false);
        if (Object.op_Inequality((Object) effectController.renpatu_item_[i], (Object) null))
          ((Component) effectController.renpatu_item_[i]).gameObject.SetActive(false);
        ((Component) renpatu_obj_last.renpatu_mesh_[i]).gameObject.SetActive(true);
        if (unit != null)
        {
          e = effectController.SetTextureUnitThum(unit.ID, renpatu_obj_last.renpatu_mesh_[i]);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          e = effectController.SetTextureUnitThum(unit.ID, renpatu_obj_last.renpatu_mesh_blur_[i]);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          effectController.renpatu_unit_[i] = effectController.SetCloneUnitIcon(effectController.renpatu_unit_[i], renpatu_obj_last.renpatu_trans_[i], effectController.AnimationUnitIconPrefab, unit, targetRusult.is_new);
        }
        else if (gear != null)
        {
          e = effectController.SetTextureGearThum(gear.ID, renpatu_obj_last.renpatu_mesh_[i]);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          e = effectController.SetTextureGearThum(gear.ID, renpatu_obj_last.renpatu_mesh_blur_[i]);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          bool isWeaponMaterial = crt.type_ == 35;
          effectController.renpatu_item_[i] = effectController.SetCloneItemIcon(effectController.renpatu_item_[i], renpatu_obj_last.renpatu_trans_[i], effectController.AnimationItemIconPrefab, gear, isWeaponMaterial, targetRusult.is_new);
        }
        else if (supply != null)
        {
          e = effectController.SetTextureSupplyThum(supply.ID, renpatu_obj_last.renpatu_mesh_[i]);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          e = effectController.SetTextureSupplyThum(supply.ID, renpatu_obj_last.renpatu_mesh_blur_[i]);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          effectController.renpatu_item_[i] = effectController.SetCloneSupplyIcon(effectController.renpatu_item_[i], renpatu_obj_last.renpatu_trans_[i], effectController.AnimationItemIconPrefab);
          effectController.SetRealityItemThumnaile(rarityindex, result_data, i, supply);
        }
        else
        {
          e = effectController.SetTextureItemThum(crt, renpatu_obj_last.renpatu_mesh_[i], ((Component) renpatu_obj_last.renpatu_mesh_[i]).gameObject);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          e = effectController.SetTextureItemThum(crt, renpatu_obj_last.renpatu_mesh_blur_[i]);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          effectController.renpatu_item_[i] = effectController.SetCloneSupplyIcon(effectController.renpatu_item_[i], renpatu_obj_last.renpatu_trans_[i], effectController.AnimationItemIconPrefab);
          effectController.SetRealityItemThumnaile(rarityindex, result_data, i, supply);
        }
        crt = (CommonRewardType) null;
        unit = (UnitUnit) null;
        gear = (GearGear) null;
        supply = (SupplySupply) null;
      }
      ((Component) renpatu_obj_last.renpatu_trans_[0]).GetComponent<Shop00720EffectRarity>().setSe(Shop00720EffectRarity.SeName(high_rarity));
      renpatu_obj_last = (GetSumContainerList) null;
    }
  }

  private void SetRealityUnitThumnaile(
    int realityIndex,
    WebAPI.Response.SlotS001MedalPayResult[] result_data,
    int index)
  {
    GetSumContainerList sumContainerList = this.renpatu_obj_[result_data.Length - 1];
    switch (realityIndex)
    {
      case 0:
      case 1:
      case 2:
        ((Renderer) sumContainerList.renpatu_mesh_blur_[index]).enabled = false;
        break;
      case 4:
        ((Component) sumContainerList.renpatu_unit_light_[index]).gameObject.SetActive(true);
        break;
      case 5:
        ((Component) sumContainerList.renpatu_unit_light2_[index]).gameObject.SetActive(true);
        break;
    }
  }

  private void SetRealityItemThumnaile(
    int realityIndex,
    WebAPI.Response.SlotS001MedalPayResult[] result_data,
    int index,
    SupplySupply supply)
  {
    GetSumContainerList sumContainerList = this.renpatu_obj_[result_data.Length - 1];
    switch (realityIndex)
    {
      case 0:
      case 1:
      case 2:
        ((Renderer) sumContainerList.renpatu_mesh_blur_[index]).enabled = false;
        break;
      case 4:
        if (supply != null)
        {
          ((Component) sumContainerList.renpatu_item_light_[index]).gameObject.SetActive(true);
          break;
        }
        ((Component) sumContainerList.renpatu_item_other_light_[index]).gameObject.SetActive(true);
        break;
      case 5:
        if (supply != null)
        {
          ((Component) sumContainerList.renpatu_item_light2_[index]).gameObject.SetActive(true);
          break;
        }
        ((Component) sumContainerList.renpatu_item_other_light2_[index]).gameObject.SetActive(true);
        break;
    }
  }

  protected override AnimationUnitIcon SetCloneUnitIcon(
    AnimationUnitIcon icon,
    Transform trans,
    GameObject obj,
    UnitUnit unit,
    bool new_flag = false)
  {
    if (Object.op_Inequality((Object) icon, (Object) null) && Object.op_Inequality((Object) ((Component) icon).gameObject.transform.parent, (Object) ((Component) trans).transform))
    {
      Object.Destroy((Object) ((Component) icon).gameObject);
      icon = (AnimationUnitIcon) null;
    }
    if (Object.op_Equality((Object) icon, (Object) null))
      icon = obj.Clone(((Component) trans).transform).GetComponent<AnimationUnitIcon>();
    else
      ((Component) icon).gameObject.SetActive(true);
    ((Component) icon).transform.position = new Vector3(((Component) trans).transform.position.x, ((Component) trans).transform.position.y, ((Component) trans).transform.position.z);
    ((Component) icon).transform.localRotation = Quaternion.Euler(0.0f, -180f, 0.0f);
    ((Object) ((Component) icon).gameObject).name = "AnimationUnitIcon";
    icon.Set(unit, new_flag);
    this.SetLayer(((Component) icon).gameObject.transform, ((Component) trans).gameObject.layer);
    return icon;
  }

  private AnimationItemIcon SetCloneItemIcon(
    AnimationItemIcon icon,
    Transform trans,
    GameObject obj,
    GearGear item,
    bool isWeaponMaterial,
    bool new_flag = false)
  {
    if (Object.op_Inequality((Object) icon, (Object) null) && Object.op_Inequality((Object) ((Component) icon).gameObject.transform.parent, (Object) ((Component) trans).transform))
    {
      Object.Destroy((Object) ((Component) icon).gameObject);
      icon = (AnimationItemIcon) null;
    }
    if (Object.op_Equality((Object) icon, (Object) null))
      icon = obj.Clone(((Component) trans).transform).GetComponent<AnimationItemIcon>();
    else
      ((Component) icon).gameObject.SetActive(true);
    ((Component) icon).transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    ((Component) icon).transform.localRotation = Quaternion.Euler(0.0f, -180f, 0.0f);
    ((Object) ((Component) icon).gameObject).name = "AnimationItemIcon";
    icon.Set(item, isWeaponMaterial, new_flag);
    this.SetLayer(((Component) icon).gameObject.transform, ((Component) trans).gameObject.layer);
    return icon;
  }

  private AnimationItemIcon SetCloneSupplyIcon(
    AnimationItemIcon icon,
    Transform trans,
    GameObject obj)
  {
    if (Object.op_Inequality((Object) icon, (Object) null) && Object.op_Inequality((Object) ((Component) icon).gameObject.transform.parent, (Object) ((Component) trans).transform))
    {
      Object.Destroy((Object) ((Component) icon).gameObject);
      icon = (AnimationItemIcon) null;
    }
    if (Object.op_Equality((Object) icon, (Object) null))
      icon = obj.Clone(((Component) trans).transform).GetComponent<AnimationItemIcon>();
    else
      ((Component) icon).gameObject.SetActive(true);
    ((Component) icon).transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    ((Component) icon).transform.localRotation = Quaternion.Euler(0.0f, -180f, 0.0f);
    ((Object) ((Component) icon).gameObject).name = nameof (SetCloneSupplyIcon);
    icon.SetSimpleMode();
    this.SetLayer(((Component) icon).gameObject.transform, ((Component) trans).gameObject.layer);
    return icon;
  }

  private IEnumerator SetTextureItemThum(
    CommonRewardType crt,
    MeshRenderer mesh_renderer,
    GameObject rootObj = null)
  {
    Shop00720EffectController effectController = this;
    Future<Sprite> sprite;
    switch (crt.type_)
    {
      case 4:
        sprite = Res.Icons.Item_Icon_Zeny.Load<Sprite>();
        break;
      case 10:
        sprite = Res.Icons.Item_Icon_Kiseki.Load<Sprite>();
        break;
      case 14:
        sprite = Res.Icons.Item_Icon_Medal.Load<Sprite>();
        break;
      case 15:
        sprite = Res.Icons.Item_Icon_Point.Load<Sprite>();
        break;
      case 17:
        sprite = Res.Icons.Item_Icon_BattleMedal.Load<Sprite>();
        break;
      case 19:
        sprite = MasterData.QuestkeyQuestkey[crt.id_].LoadSpriteThumbnail();
        break;
      case 20:
        sprite = MasterData.GachaTicket[crt.id_].LoadSpriteF();
        break;
      case 21:
        sprite = MasterData.SeasonTicketSeasonTicket[crt.id_].LoadThumneilF();
        break;
      case 25:
        sprite = Res.Icons.Item_Icon_Common.Load<Sprite>();
        break;
      case 29:
        sprite = MasterData.BattleskillSkill[crt.id_].LoadBattleSkillIcon();
        break;
      case 40:
        sprite = MasterData.CommonTicket[crt.id_].LoadIconMSpriteF();
        break;
      default:
        if (!Object.op_Inequality((Object) rootObj, (Object) null))
        {
          yield break;
        }
        else
        {
          rootObj.SetActive(true);
          yield break;
        }
    }
    IEnumerator e = effectController.SetTexture(sprite, mesh_renderer);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator CutInInitializeOld(PlayerUnit unit)
  {
    IEnumerator e;
    if (unit.unit.character.gender == UnitGender.male)
    {
      this.cutinType = Shop00720EffectController.CutinType.Male;
      e = this.duelCutinMale.Initialize<PlayerUnit>(unit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      this.cutinType = Shop00720EffectController.CutinType.Female;
      e = this.duelCutin.Initialize<PlayerUnit>(unit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public IEnumerator CutInInitialize(PlayerUnit unit, Shop00720Menu menu)
  {
    ((Component) this.duelCutin).gameObject.SetActive(false);
    ((Component) this.duelCutinMale).gameObject.SetActive(false);
    this.duelCutinNewObj.SetActive(false);
    this.shopMenu = menu;
    IEnumerator e;
    if (((IEnumerable<UnitGroup>) MasterData.UnitGroupList).FirstOrDefault<UnitGroup>((Func<UnitGroup, bool>) (x => x.unit_id == unit.unit.ID)) != null)
    {
      this.cutinType = Shop00720EffectController.CutinType.New;
      if (Object.op_Equality((Object) this.duelCutinNew, (Object) null))
      {
        Future<GameObject> fs = (Future<GameObject>) null;
        UnitCutinInfo cutinInfo = unit.CutinInfo;
        if (!string.IsNullOrEmpty(cutinInfo.prefab_name))
        {
          fs = new ResourceObject("Animations/battle_cutin/" + cutinInfo.prefab_name).Load<GameObject>();
          if (fs != null)
          {
            e = fs.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            this.duelCutinNew = fs.Result.Clone(this.duelCutinNewObj.transform).GetComponent<DuelCutin>();
            if (Object.op_Inequality((Object) this.duelCutinNew, (Object) null))
            {
              e = this.duelCutinNew.Initialize<PlayerUnit>(unit);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
            }
            else
            {
              e = this.CutInInitializeOld(unit);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
            }
          }
          else
          {
            e = this.CutInInitializeOld(unit);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
        }
        else
        {
          e = this.CutInInitializeOld(unit);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        fs = (Future<GameObject>) null;
      }
    }
    else
    {
      e = this.CutInInitializeOld(unit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public void PlayCutin()
  {
    switch (this.cutinType)
    {
      case Shop00720EffectController.CutinType.Male:
        foreach (GameObject gameObject in this.bugsCutinMaleInitOff)
          gameObject.SetActive(false);
        ((Component) this.duelCutinMale).gameObject.SetActive(true);
        this.duelCutinMale.PlaySkillCutin();
        break;
      case Shop00720EffectController.CutinType.Female:
        foreach (GameObject gameObject in this.bugsCutinInitOff)
          gameObject.SetActive(false);
        ((Component) this.duelCutin).gameObject.SetActive(true);
        this.duelCutin.PlaySkillCutin();
        break;
      case Shop00720EffectController.CutinType.New:
        if (this.isCutInHiding)
          break;
        ((Component) this.camera2D).gameObject.SetActive(false);
        this.duelCutinNewObj.SetActive(true);
        ((Component) this.duelCutinNew).gameObject.SetActive(true);
        this.duelCutinNew.PlaySkillCutin();
        this.duelCutinNew.CameraCutin.transform.localPosition = new Vector3(0.0f, 0.0f, 10f);
        break;
    }
  }

  public void HideCutin()
  {
    switch (this.cutinType)
    {
      case Shop00720EffectController.CutinType.Male:
        ((Component) this.duelCutinMale).gameObject.SetActive(false);
        ((Component) this.duelCutinMale).gameObject.GetComponent<Animator>().ResetTrigger("Start");
        break;
      case Shop00720EffectController.CutinType.Female:
        ((Component) this.duelCutin).gameObject.SetActive(false);
        ((Component) this.duelCutin).gameObject.GetComponent<Animator>().ResetTrigger("Start");
        break;
      case Shop00720EffectController.CutinType.New:
        this.isCutInHiding = true;
        this.duelCutinNewObj.SetActive(false);
        ((Component) this.duelCutinNew).gameObject.SetActive(false);
        ((Component) this.duelCutinNew).gameObject.GetComponent<Animator>().ResetTrigger("Start");
        ((Component) this.camera2D).gameObject.SetActive(true);
        break;
    }
  }

  private enum CutinType
  {
    Male,
    Female,
    New,
  }
}
