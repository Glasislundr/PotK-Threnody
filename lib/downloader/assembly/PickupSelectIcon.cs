// Decompiled with JetBrains decompiler
// Type: PickupSelectIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Prefabs/Icons/PickupSelectIcon")]
public class PickupSelectIcon : MonoBehaviour
{
  [SerializeField]
  private PickupSelectIcon.Mold[] molds_;
  [SerializeField]
  [Tooltip("共通の状態を表示するオブジェクト類のTop")]
  private GameObject objOptions_;
  [SerializeField]
  private UI2DSprite kind_;
  [SerializeField]
  private AttackClassIcon attackClass_;
  [SerializeField]
  private GameObject objNotPossessed_;
  [SerializeField]
  private GameObject objSelected_;
  [SerializeField]
  private LongPressButton button_;
  [SerializeField]
  private UILabel txtNumber_;
  [Header("数値表示")]
  [SerializeField]
  [Tooltip("数量")]
  private PickupSelectIcon.NumberModule quantity_;
  [SerializeField]
  [Tooltip("残り回数")]
  private PickupSelectIcon.NumberModule remaining_;
  [SerializeField]
  private PickupSelectIcon.SpriteArray spriteNumbers_;
  [Header("武具系下地")]
  [SerializeField]
  private PickupSelectIcon.SpriteArray back_;
  [SerializeField]
  private PickupSelectIcon.SpriteArray backSpecificationOfEquipmentUnits_;
  [SerializeField]
  private Sprite backGearBody_;
  [SerializeField]
  private Sprite backReisou_;
  private object master_;
  private PickupSelectIcon.Type? current_;
  private PickupSelectIcon.Mold mold_;
  private UITweener[] tweeners_;
  private bool? gray_;
  private Vector3? offsetCross_;

  public static Future<GameObject> createLoader()
  {
    return new ResourceObject("Prefabs/UnitIcon/UnitItem_Icon").Load<GameObject>();
  }

  public LongPressButton button => this.button_;

  public UnitUnit masterUnit => this.master_ as UnitUnit;

  public GearGear masterGear => this.master_ as GearGear;

  public PickupSelectIcon.Type type
  {
    get => !this.current_.HasValue ? PickupSelectIcon.Type.Blank : this.current_.Value;
  }

  public int position { get; private set; } = -1;

  public void setPosition(int pos = -1) => this.position = pos;

  public void setBlank()
  {
    this.setType(PickupSelectIcon.Type.Blank);
    this.master_ = (object) null;
  }

  public void initialize(UnitUnit unit, int? quantity = null)
  {
    if (this.master_ == unit)
      return;
    this.master_ = (object) unit;
    this.setType(this.checkType<UnitUnit>(unit, false));
    if (unit == null)
      return;
    this.setHistoryNumber(unit.history_group_number);
    this.setGearKindIcon((GearKindEnum) unit.kind_GearKind, unit.GetElement());
    this.setQuantity(quantity);
  }

  public void initialize(GearGear gear, bool bGearBody, int? quantity = null)
  {
    if (this.master_ == gear)
      return;
    this.master_ = (object) gear;
    this.setType(this.checkType<GearGear>(gear, bGearBody));
    if (gear == null)
      return;
    this.setHistoryNumber(gear.history_group_number);
    this.setGearKindIcon((GearKindEnum) gear.kind_GearKind, gear.GetElement());
    this.setQuantity(quantity);
    UI2DSprite back = this.mold_.back;
    PickupSelectIcon.Type? current = this.current_;
    Sprite sprite = 6 == (int) current.GetValueOrDefault() & current.HasValue ? this.backGearBody_ : (gear.isReisou() ? this.backReisou_ : (gear.hasSpecificationOfEquipmentUnits ? this.backSpecificationOfEquipmentUnits_[gear.customize_flag] : this.back_[gear.customize_flag]));
    back.sprite2D = sprite;
    if (!gear.isReisou() && gear.hasAttackClass)
    {
      ((Component) this.attackClass_).gameObject.SetActive(true);
      this.attackClass_.Initialize(gear.gearClassification.attack_classification, gear.attachedElement);
    }
    else
      ((Component) this.attackClass_).gameObject.SetActive(false);
    this.setRemain(gear.isManaSeed() ? gear.disappearance_num : new int?());
  }

  public void loadIcon()
  {
    switch (this.mold_.type)
    {
      case PickupSelectIcon.MoldType.Unit:
        if (UnitIcon.IsCache(this.masterUnit))
        {
          this.mold_.icon.sprite2D = UnitIcon.GetSprite(this.masterUnit);
          break;
        }
        this.StartCoroutine(this.doLoadIcon<UnitUnit>(this.mold_, this.masterUnit));
        break;
      case PickupSelectIcon.MoldType.Gear:
        if (ItemIcon.IsCache(this.masterGear))
        {
          this.mold_.icon.sprite2D = ItemIcon.GetSprite(this.masterGear);
          break;
        }
        this.StartCoroutine(this.doLoadIcon<GearGear>(this.mold_, this.masterGear));
        break;
    }
  }

  public IEnumerator doLoadIcon()
  {
    IEnumerator e;
    switch (this.mold_.type)
    {
      case PickupSelectIcon.MoldType.Unit:
        e = this.doLoadIcon<UnitUnit>(this.mold_, this.masterUnit);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      case PickupSelectIcon.MoldType.Gear:
        e = this.doLoadIcon<GearGear>(this.mold_, this.masterGear);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
    }
  }

  private PickupSelectIcon.Type checkType<T>(T dat, bool bGearBody) where T : class
  {
    T obj = dat;
    if ((object) obj != null)
    {
      switch (obj)
      {
        case UnitUnit unitUnit:
          return !unitUnit.IsNormalUnit ? PickupSelectIcon.Type.Material : PickupSelectIcon.Type.Unit;
        case GearGear gearGear:
          if (bGearBody)
            return PickupSelectIcon.Type.GearBody;
          if (gearGear.isMaterial())
            return PickupSelectIcon.Type.GearMaterial;
          return !gearGear.isReisou() ? PickupSelectIcon.Type.Gear : PickupSelectIcon.Type.Reisou;
      }
    }
    return PickupSelectIcon.Type.Blank;
  }

  private void setType(PickupSelectIcon.Type type)
  {
    if (this.current_.HasValue)
    {
      int num = (int) type;
      PickupSelectIcon.Type? current = this.current_;
      int valueOrDefault = (int) current.GetValueOrDefault();
      if (num == valueOrDefault & current.HasValue)
        return;
    }
    PickupSelectIcon.MoldType moldType;
    switch (type)
    {
      case PickupSelectIcon.Type.Unit:
      case PickupSelectIcon.Type.Material:
        moldType = PickupSelectIcon.MoldType.Unit;
        break;
      case PickupSelectIcon.Type.Gear:
      case PickupSelectIcon.Type.Reisou:
      case PickupSelectIcon.Type.GearMaterial:
      case PickupSelectIcon.Type.GearBody:
        moldType = PickupSelectIcon.MoldType.Gear;
        break;
      default:
        moldType = PickupSelectIcon.MoldType.Blank;
        break;
    }
    for (int index = 0; index < this.molds_.Length; ++index)
    {
      PickupSelectIcon.Mold mold = this.molds_[index];
      if (Object.op_Implicit((Object) mold.top))
        mold.top.SetActive(mold.type == moldType);
      if (this.molds_[index].type == moldType)
        this.mold_ = mold;
    }
    this.current_ = new PickupSelectIcon.Type?(type);
    this.objOptions_.SetActive(moldType != 0);
    ((Component) this.kind_).gameObject.SetActive(type != PickupSelectIcon.Type.GearMaterial);
  }

  private IEnumerator doLoadIcon<T>(PickupSelectIcon.Mold mold, T data) where T : class
  {
    T obj = data;
    IEnumerator e;
    if ((object) obj != null)
    {
      switch (obj)
      {
        case UnitUnit unit:
          e = UnitIcon.LoadSprite((Action<Sprite>) (s => mold.icon.sprite2D = s), unit);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          break;
        case GearGear gear:
          e = ItemIcon.LoadSprite(gear, (Action<Sprite>) (s => mold.icon.sprite2D = s));
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          break;
      }
    }
  }

  private void setGearKindIcon(GearKindEnum kind, CommonElement element)
  {
    this.kind_.sprite2D = GearKindIcon.LoadSprite(kind, element);
  }

  private void setHistoryNumber(int num)
  {
    int num1 = num % 10000;
    this.txtNumber_.SetTextLocalize(num1 != 0 ? string.Format(Consts.GetInstance().HISTORY_NUMBER, (object) num1) : Consts.GetInstance().HISTORY_NUMBER_ZERO);
  }

  public bool gray
  {
    get => this.gray_.HasValue && this.gray_.Value;
    set
    {
      if (this.gray_.HasValue)
      {
        int num1 = value ? 1 : 0;
        bool? gray = this.gray_;
        int num2 = gray.GetValueOrDefault() ? 1 : 0;
        if (num1 == num2 & gray.HasValue)
          return;
      }
      this.gray_ = new bool?(value);
      NGTween.playTweens(this.tweeners, NGTween.Kind.GRAYOUT, !value);
    }
  }

  private UITweener[] tweeners
  {
    get
    {
      return this.tweeners_ ?? (this.tweeners_ = ((Component) this).GetComponentsInChildren<UITweener>(true));
    }
  }

  public bool notPossessed
  {
    get => this.objNotPossessed_.activeSelf;
    set => this.objNotPossessed_.SetActive(value);
  }

  public bool selected
  {
    get => this.objSelected_.activeSelf;
    set => this.objSelected_.SetActive(value);
  }

  private void setQuantity(int? quantity = null)
  {
    this.quantity_.set(this.spriteNumbers_, quantity);
  }

  private void setRemain(int? remain = null) => this.remaining_.set(this.spriteNumbers_, remain);

  public enum Type
  {
    Blank,
    Unit,
    Material,
    Gear,
    Reisou,
    GearMaterial,
    GearBody,
  }

  private enum MoldType
  {
    Blank,
    Unit,
    Gear,
  }

  [Serializable]
  private class SpriteArray
  {
    [SerializeField]
    private Sprite[] sprites;

    public Sprite this[int i]
    {
      get => i < 0 || i >= this.sprites.Length ? this.sprites[0] : this.sprites[i];
    }
  }

  [Serializable]
  private class NumberModule
  {
    public GameObject top;
    public Transform posSymbol;
    public UI2DSprite[] digits;
    public bool isRightAligned;
    private Vector3? offsetSymbol_;

    public void set(PickupSelectIcon.SpriteArray nums, int? quantity = null)
    {
      if (!quantity.HasValue)
      {
        this.top.SetActive(false);
      }
      else
      {
        this.top.SetActive(true);
        if (!this.offsetSymbol_.HasValue)
          this.offsetSymbol_ = new Vector3?(Vector3.op_Subtraction(this.posSymbol.localPosition, ((Component) this.digits[0]).transform.localPosition));
        int num1 = 1;
        for (int index = 0; index < this.digits.Length; ++index)
          num1 *= 10;
        int num2 = num1 - 1;
        string str = Mathf.Min(quantity.Value, num2).ToString();
        int num3 = str.Length - 1;
        int num4 = this.isRightAligned ? 0 : this.digits.Length - num3 - 1;
        for (int index1 = 0; index1 < str.Length; ++index1)
        {
          Sprite num5 = nums[(int) str[num3 - index1] - 48];
          int index2 = index1 + num4;
          this.digits[index2].sprite2D = num5;
          ((UIWidget) this.digits[index2]).width = ((Texture) num5.texture).width;
          ((Component) this.digits[index2]).gameObject.SetActive(true);
        }
        if (this.isRightAligned)
        {
          for (int index = num3 + 1; index < this.digits.Length; ++index)
            ((Component) this.digits[index]).gameObject.SetActive(false);
        }
        else
        {
          for (int index = num4 - 1; index >= 0; --index)
            ((Component) this.digits[index]).gameObject.SetActive(false);
        }
        this.posSymbol.localPosition = Vector3.op_Addition(((Component) this.digits[this.isRightAligned ? num3 : num4]).transform.localPosition, this.offsetSymbol_.Value);
      }
    }
  }

  [Serializable]
  private class Mold
  {
    public PickupSelectIcon.MoldType type;
    public GameObject top;
    public UI2DSprite icon;
    public UI2DSprite back;
  }
}
