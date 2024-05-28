// Decompiled with JetBrains decompiler
// Type: WithNumber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class WithNumber : MonoBehaviour
{
  public Collider boxCollider;
  public UIButton button;
  public Transform trans;
  public UIDragScrollView scrollView;
  private WithNumberInfo wni;
  public Transform dir_type;
  public UI2DSprite slc_character;
  public GameObject slc_usilhoutte;
  public GameObject slc_esilhoutte;
  public GameObject slc_unknown;
  public GameObject slc_background;
  public GameObject slc_gearbackground;
  public GameObject gearKindIconPrefab;
  public GearKindIcon gearKindIcon;
  [SerializeField]
  protected AttackClassIcon attackClassIcon;
  public UILabel txt_num;
  public GameObject slc_hatena;
  public Transform zukanTransform;
  public Action pressEvent;
  [SerializeField]
  private Sprite[] gearBgSprite;
  [SerializeField]
  private Sprite[] gearBgSpriteSp;
  [SerializeField]
  private Sprite gearBgSpriteUnknown;

  public WithNumberInfo withNumberInfo
  {
    get => this.wni;
    set => this.wni = value;
  }

  public virtual void pressButton()
  {
    if (this.pressEvent == null)
      return;
    this.pressEvent();
  }

  public IEnumerator CreateIcon()
  {
    if (Object.op_Equality((Object) this.gearKindIcon, (Object) null))
      this.gearKindIcon = this.gearKindIconPrefab.Clone(this.dir_type).GetComponent<GearKindIcon>();
    this.slc_character.sprite2D = (Sprite) null;
    this.SetStatus(this.withNumberInfo.icon.withNumberInfo.status);
    ((Component) this.gearKindIcon).gameObject.SetActive(true);
    if (this.withNumberInfo.icon.withNumberInfo.unitData.Unit != null)
      this.CreateType(this.withNumberInfo.icon.withNumberInfo.gearKind, this.withNumberInfo.icon.withNumberInfo.element);
    if (this.withNumberInfo.icon.withNumberInfo.unitData.Gear != null)
      this.CreateType(this.withNumberInfo.icon.withNumberInfo.unitData.Gear.kind, this.withNumberInfo.icon.withNumberInfo.unitData.Gear.GetElement());
    while (this.withNumberInfo.icon.withNumberInfo.spriteCash == null)
      yield return (object) null;
    while (Object.op_Equality((Object) this.withNumberInfo.icon.withNumberInfo.spriteCash.sprite, (Object) null))
      yield return (object) null;
    this.slc_character.sprite2D = this.withNumberInfo.icon.withNumberInfo.spriteCash.sprite;
    this.setAttackClassIcon(this.attackClassIcon, this.withNumberInfo.unitData.Gear, this.withNumberInfo.elementIconPrefab);
  }

  private void CreateType(GearKind gearKind, CommonElement element)
  {
    this.NumberPosition(gearKind);
    if (Object.op_Equality((Object) this.gearKindIcon, (Object) null))
      this.gearKindIcon = this.gearKindIconPrefab.Clone(this.dir_type).GetComponent<GearKindIcon>();
    this.gearKindIcon.Init(gearKind, element);
    if (!(this.withNumberInfo.icon.withNumberInfo.unitData.History == new DateTime()))
      return;
    this.gearKindIcon.None();
  }

  private void NumberPosition(GearKind gearKind)
  {
    bool flag = true;
    UnitUnit unit = this.withNumberInfo.icon.withNumberInfo.unitData.Unit;
    if (unit != null)
    {
      if (unit.IsNormalUnit)
      {
        if (unit.character.category == UnitCategory.enemy)
          flag = false;
      }
      else if (gearKind.Enum != GearKindEnum.unique_wepon && !gearKind.isEquip)
        flag = false;
    }
    else if (!gearKind.isEquip)
      flag = false;
    if (flag)
    {
      this.zukanTransform.localPosition = new Vector3(0.0f, this.zukanTransform.localPosition.y, 0.0f);
    }
    else
    {
      ((Component) this.gearKindIcon).gameObject.SetActive(false);
      this.zukanTransform.localPosition = new Vector3(-12f, this.zukanTransform.localPosition.y, 0.0f);
    }
  }

  private void SetStatus(WithNumber.ZUKAN_STATUS status)
  {
    this.withNumberInfo.icon.withNumberInfo.buttonOn = false;
    ((Behaviour) this.button).enabled = false;
    int num = 0;
    switch (status)
    {
      case WithNumber.ZUKAN_STATUS.NOT_UNKNOWN:
        this.slc_usilhoutte.SetActive(false);
        this.slc_esilhoutte.SetActive(false);
        this.slc_unknown.SetActive(false);
        this.slc_background.SetActive(true);
        ((Component) this.slc_character).gameObject.SetActive(true);
        this.withNumberInfo.icon.withNumberInfo.buttonOn = true;
        num = this.withNumberInfo.icon.withNumberInfo.unitData.Unit.history_group_number % 10000;
        ((Behaviour) this.button).enabled = true;
        break;
      case WithNumber.ZUKAN_STATUS.G_NOT_UNKNOWN:
        this.slc_unknown.SetActive(false);
        this.slc_gearbackground.SetActive(true);
        GearGear gear = this.withNumberInfo.icon.withNumberInfo.unitData.Gear;
        if (gear.hasSpecificationOfEquipmentUnits)
          this.slc_gearbackground.GetComponent<UI2DSprite>().sprite2D = this.gearBgSpriteSp[gear.customize_flag];
        else
          this.slc_gearbackground.GetComponent<UI2DSprite>().sprite2D = this.gearBgSprite[gear.customize_flag];
        ((Component) this.slc_character).gameObject.SetActive(true);
        this.withNumberInfo.icon.withNumberInfo.buttonOn = true;
        num = this.withNumberInfo.icon.withNumberInfo.unitData.Gear.history_group_number % 10000;
        ((Behaviour) this.button).enabled = true;
        break;
      case WithNumber.ZUKAN_STATUS.U_UNKNOWN:
        this.slc_usilhoutte.SetActive(true);
        this.slc_esilhoutte.SetActive(false);
        this.slc_unknown.SetActive(true);
        this.slc_background.SetActive(false);
        ((Component) this.slc_character).gameObject.SetActive(false);
        num = this.withNumberInfo.icon.withNumberInfo.unitData.Unit.history_group_number % 10000;
        break;
      case WithNumber.ZUKAN_STATUS.E_UNKNOWN:
        this.slc_usilhoutte.SetActive(false);
        this.slc_esilhoutte.SetActive(true);
        this.slc_unknown.SetActive(false);
        this.slc_background.SetActive(false);
        ((Component) this.slc_character).gameObject.SetActive(false);
        num = this.withNumberInfo.icon.withNumberInfo.unitData.Unit.history_group_number % 10000;
        break;
      case WithNumber.ZUKAN_STATUS.G_UNKNOWN:
        this.slc_gearbackground.SetActive(true);
        this.slc_gearbackground.GetComponent<UI2DSprite>().sprite2D = this.gearBgSpriteUnknown;
        this.slc_unknown.SetActive(true);
        ((Component) this.slc_character).gameObject.SetActive(false);
        num = this.withNumberInfo.icon.withNumberInfo.unitData.Gear.history_group_number % 10000;
        break;
      default:
        this.slc_usilhoutte.SetActive(true);
        this.slc_esilhoutte.SetActive(false);
        this.slc_unknown.SetActive(true);
        this.slc_background.SetActive(false);
        ((Component) this.slc_character).gameObject.SetActive(false);
        break;
    }
    if (Object.op_Inequality((Object) this.attackClassIcon, (Object) null))
      ((Component) this.attackClassIcon).gameObject.SetActive(false);
    ((Component) this.txt_num).gameObject.SetActive(true);
    this.txt_num.SetTextLocalize(num.ToString().PadLeft(4, '0'));
    this.slc_hatena.SetActive(false);
  }

  private void setAttackClassIcon(AttackClassIcon icon, GearGear gear, GameObject elementPrefab)
  {
    if (!Object.op_Inequality((Object) icon, (Object) null))
      return;
    if (gear != null && gear.kind.is_attack)
    {
      if (gear.hasAttackClass)
      {
        icon.Initialize(gear.gearClassification.attack_classification, gear.attachedElement);
        ((Component) icon).gameObject.SetActive(true);
        return;
      }
      if (gear.attachedElement != CommonElement.none && Object.op_Inequality((Object) elementPrefab, (Object) null))
      {
        ((Component) icon).GetComponent<UI2DSprite>().sprite2D = elementPrefab.GetComponent<CommonElementIcon>().getIcon(gear.attachedElement);
        ((Component) icon).gameObject.SetActive(true);
        return;
      }
    }
    ((Component) icon).gameObject.SetActive(false);
  }

  public void SetPressEvent(Action evt) => this.pressEvent = evt;

  public void Reset()
  {
  }

  public enum ZUKAN_STATUS
  {
    NOT_UNKNOWN,
    G_NOT_UNKNOWN,
    U_UNKNOWN,
    E_UNKNOWN,
    G_UNKNOWN,
  }
}
