// Decompiled with JetBrains decompiler
// Type: AnimationItemIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AnimationItemIcon : MonoBehaviour
{
  [SerializeField]
  private SpriteRenderer break_;
  [SerializeField]
  private SpriteRenderer frame_;
  [SerializeField]
  private SpriteRenderer favorite_;
  [SerializeField]
  private SpriteRenderer type_;
  [SerializeField]
  private GameObject star_;
  [SerializeField]
  private SpriteRenderer rank_;
  [SerializeField]
  private GameObject new_;
  [SerializeField]
  private List<GameObject> star_list_;
  [SerializeField]
  private List<Sprite> rank_list_;
  [SerializeField]
  private SpriteRenderer back_;
  private Vector3 m_starIntialLocalPosition;
  public Sprite[] backSprite;
  public Sprite[] backSpriteSpecificationOfEquipmentUnits;
  public Sprite backSpriteWeaponMaterial;
  public Sprite backSpriteReisou;

  public ItemInfo itemInfo { get; set; }

  private void Awake()
  {
    if (Debug.isDebugBuild)
      Debug.Log((object) "Save original star position.");
    this.m_starIntialLocalPosition = this.star_.transform.localPosition;
    if (!Debug.isDebugBuild)
      return;
    Debug.Log((object) ("m_starIntialLocalPosition.x: " + this.m_starIntialLocalPosition.x.ToString()));
  }

  public void SetBroken(bool flag) => ((Component) this.break_).gameObject.SetActive(flag);

  public void SetFavorite(bool flag) => ((Component) this.favorite_).gameObject.SetActive(flag);

  public void SetNew(bool flag) => this.new_.gameObject.SetActive(flag);

  private Sprite GetGearTypeIcon(GearKindEnum kind, CommonElement element)
  {
    return Resources.Load<Sprite>(string.Format("Icons/Materials/GearKind_Element_Icon/slc_{0}_{1}_34_30", (object) kind.ToString(), (object) element.ToString()));
  }

  public void SetType(GearKind kind, CommonElement element)
  {
    if (kind.isEquip)
    {
      ((Component) this.type_).gameObject.SetActive(true);
      this.type_.sprite = this.GetGearTypeIcon(kind.Enum, element);
      this.star_.transform.localPosition = this.m_starIntialLocalPosition;
    }
    else
    {
      ((Component) this.type_).gameObject.SetActive(false);
      this.star_.transform.localPosition = new Vector3(0.0f, this.star_.transform.localPosition.y, this.star_.transform.localPosition.z);
    }
    if (!Debug.isDebugBuild)
      return;
    Debug.Log((object) ("Moved star. star_.transform.localPosition.x: " + this.star_.transform.localPosition.x.ToString()));
  }

  public void SetStar(GearRarity rarity)
  {
    this.star_.gameObject.SetActive(true);
    this.star_list_.ToggleOnce(rarity.index);
  }

  public void SetRank(int id)
  {
    if (!this.itemInfo.gear.kind.isEquip || id >= this.rank_list_.Count)
    {
      ((Component) this.rank_).gameObject.SetActive(false);
    }
    else
    {
      ((Component) this.rank_).gameObject.SetActive(true);
      this.rank_.sprite = this.rank_list_[id];
    }
  }

  public void Set(ItemInfo itemInfo, bool is_new = false)
  {
    ((Component) this.frame_).gameObject.SetActive(true);
    if (itemInfo.gear == null)
      return;
    this.back_.sprite = this.backSprite[0];
    if (itemInfo.isWeaponMaterial)
      this.back_.sprite = this.backSpriteWeaponMaterial;
    else if (itemInfo.gear != null)
      this.back_.sprite = !itemInfo.isReisou ? (!itemInfo.gear.hasSpecificationOfEquipmentUnits ? this.backSprite[itemInfo.gear.customize_flag] : this.backSpriteSpecificationOfEquipmentUnits[itemInfo.gear.customize_flag]) : this.backSpriteReisou;
    this.itemInfo = itemInfo;
    this.SetBroken(this.itemInfo.broken);
    this.SetFavorite(this.itemInfo.favorite);
    this.SetType(this.itemInfo.gear.kind, this.itemInfo.GetElement());
    this.SetStar(this.itemInfo.gear.rarity);
    this.SetRank(this.itemInfo.gearLevel);
    this.SetNew(is_new);
  }

  public void Set(GearGear item, bool isWeaponMaterial, bool is_new = false)
  {
    ((Component) this.frame_).gameObject.SetActive(true);
    this.back_.sprite = !isWeaponMaterial ? (!item.hasSpecificationOfEquipmentUnits ? this.backSprite[item.customize_flag] : this.backSpriteSpecificationOfEquipmentUnits[item.customize_flag]) : this.backSpriteWeaponMaterial;
    this.SetBroken(false);
    this.SetFavorite(false);
    this.SetType(item.kind, item.GetElement());
    this.SetStar(item.rarity);
    ((Component) this.rank_).gameObject.SetActive(false);
    this.SetNew(is_new);
  }

  public void SetSimpleMode()
  {
    this.SetBroken(false);
    this.SetFavorite(false);
    ((Component) this.type_).gameObject.SetActive(false);
    ((Component) this.rank_).gameObject.SetActive(false);
    this.SetNew(false);
    ((Component) this.frame_).gameObject.SetActive(false);
    this.star_.gameObject.SetActive(false);
    this.back_.sprite = this.backSprite[0];
  }
}
