// Decompiled with JetBrains decompiler
// Type: FacilityIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FacilityIcon : IconPrefabBase
{
  [SerializeField]
  private GameObject bottom_base;
  [SerializeField]
  private UI2DSprite icon;
  [SerializeField]
  private GameObject kindIconBase;
  [SerializeField]
  private UILabel lblLv;
  [SerializeField]
  private UI2DSprite iconBgSprite;
  private UnitUnit unit;
  private GameObject kindIconPrefab;
  private Action _onClick;
  protected UITweener[] aGray;
  public GameObject quantity;
  [SerializeField]
  private GameObject m_crossForOneDigitCount;
  [SerializeField]
  private GameObject m_crossForTwoDigitsCount;
  [SerializeField]
  private GameObject m_crossForThreeDigitsCount;
  [SerializeField]
  private GameObject m_crossForFourDigitsCount;
  [SerializeField]
  private GameObject[] m_onesDigit;
  [SerializeField]
  private GameObject[] m_tensDigit;
  [SerializeField]
  private GameObject[] m_hundredsDigit;
  [SerializeField]
  private GameObject[] m_thousandsDigit;

  public Action onClick
  {
    set => this._onClick = value;
  }

  public IEnumerator SetUnit(PlayerUnit pu, bool visibleBottom, bool visibleBg)
  {
    FacilityIcon facilityIcon = this;
    facilityIcon.lblLv.SetTextLocalize(pu.unit.facilityLevel);
    ((Component) facilityIcon.iconBgSprite).gameObject.SetActive(visibleBg);
    Future<Sprite> bgSpriteF;
    IEnumerator e;
    if (visibleBg)
    {
      bgSpriteF = new ResourceObject("Icons/Item_Icon_Base").Load<Sprite>();
      e = bgSpriteF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (Object.op_Inequality((Object) bgSpriteF.Result, (Object) null))
        facilityIcon.iconBgSprite.sprite2D = bgSpriteF.Result;
      bgSpriteF = (Future<Sprite>) null;
    }
    if (pu != (PlayerUnit) null)
    {
      facilityIcon.unit = pu.unit;
      bgSpriteF = facilityIcon.unit.LoadSpriteThumbnail();
      e = bgSpriteF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      facilityIcon.icon.sprite2D = bgSpriteF.Result;
      ((UIWidget) facilityIcon.icon).width = ((Texture) bgSpriteF.Result.texture).width;
      ((UIWidget) facilityIcon.icon).height = ((Texture) bgSpriteF.Result.texture).height;
      Future<GameObject> kindIconPrefabF = new ResourceObject("Icons/FacilityKindIcon").Load<GameObject>();
      e = kindIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      facilityIcon.kindIconPrefab = kindIconPrefabF.Result;
      UI2DSprite component = facilityIcon.kindIconPrefab.Clone(facilityIcon.kindIconBase.transform).GetComponent<UI2DSprite>();
      if (Object.op_Inequality((Object) component, (Object) null))
        component.sprite2D = GuildUtil.LoadKindSprite((FacilityCategory) facilityIcon.unit.facility.category_id, pu.GetElement());
      ((Behaviour) facilityIcon.icon).enabled = true;
      facilityIcon.kindIconPrefab.gameObject.SetActive(true);
      facilityIcon.bottom_base.SetActive(visibleBottom);
      facilityIcon.gray = false;
      facilityIcon.aGray = (UITweener[]) null;
      bgSpriteF = (Future<Sprite>) null;
      kindIconPrefabF = (Future<GameObject>) null;
    }
  }

  public void SetEmpty()
  {
    this.bottom_base.SetActive(false);
    ((Component) this.icon).gameObject.SetActive(false);
  }

  public void OnButtonClick()
  {
    if (this._onClick == null)
      return;
    this._onClick();
  }

  public override bool Gray
  {
    get => this.gray;
    set
    {
      if (this.gray == value)
        return;
      this.gray = value;
      if (this.aGray == null)
        this.aGray = ((Component) this).GetComponentsInChildren<UITweener>(true);
      NGTween.playTweens(this.aGray, NGTween.Kind.GRAYOUT, !value);
    }
  }

  public void EnableQuantity(int quantity)
  {
    if (quantity <= 0)
    {
      this.HideCounter();
    }
    else
    {
      if (quantity > 9999)
        quantity = 9999;
      int num1 = quantity % 10;
      int num2 = quantity % 100 / 10;
      int num3 = quantity % 1000 / 100;
      int num4 = quantity % 10000 / 1000;
      FacilityIcon.CounterDigits digits = FacilityIcon.CounterDigits.OneDigit;
      if (quantity < 1)
      {
        this.HideCounter();
      }
      else
      {
        if (quantity >= 10)
          digits = quantity >= 100 ? (quantity >= 1000 ? FacilityIcon.CounterDigits.FourDigits : FacilityIcon.CounterDigits.ThreeDigits) : FacilityIcon.CounterDigits.TwoDigits;
        this.SetCross(digits);
        this.SetOnesDigit(num1);
        this.SetTensDigit(num2, digits);
        this.SetHundredsDigit(num3, digits);
        this.SetThousandsDigit(num4, digits);
        this.QuantitySupply = true;
      }
    }
  }

  private void SetOnesDigit(int num)
  {
    if (num > -1 && num < 10)
    {
      ((IEnumerable<GameObject>) this.m_onesDigit).ToggleOnceEx(num);
    }
    else
    {
      if (!Debug.isDebugBuild)
        return;
      Debug.LogError((object) ("Illegal parameter (num): " + (object) num));
    }
  }

  private void SetTensDigit(int num, FacilityIcon.CounterDigits digits)
  {
    if (num > -1 && num < 10)
    {
      if (FacilityIcon.CounterDigits.TwoDigits > digits)
        ((IEnumerable<GameObject>) this.m_tensDigit).ToggleOnceEx(-1);
      else
        ((IEnumerable<GameObject>) this.m_tensDigit).ToggleOnceEx(num);
    }
    else
    {
      if (!Debug.isDebugBuild)
        return;
      Debug.LogError((object) ("Illegal parameter (num): " + (object) num));
    }
  }

  private void SetHundredsDigit(int num, FacilityIcon.CounterDigits digits)
  {
    if (num > -1 && num < 10)
    {
      if (FacilityIcon.CounterDigits.ThreeDigits > digits)
        ((IEnumerable<GameObject>) this.m_hundredsDigit).ToggleOnceEx(-1);
      else
        ((IEnumerable<GameObject>) this.m_hundredsDigit).ToggleOnceEx(num);
    }
    else
    {
      if (!Debug.isDebugBuild)
        return;
      Debug.LogError((object) ("Illegal parameter (num): " + (object) num));
    }
  }

  private void SetThousandsDigit(int num, FacilityIcon.CounterDigits digits)
  {
    if (num > -1 && num < 10)
    {
      if (FacilityIcon.CounterDigits.FourDigits > digits)
        ((IEnumerable<GameObject>) this.m_thousandsDigit).ToggleOnceEx(-1);
      else
        ((IEnumerable<GameObject>) this.m_thousandsDigit).ToggleOnceEx(num);
    }
    else
    {
      if (!Debug.isDebugBuild)
        return;
      Debug.LogError((object) ("Illegal parameter (num): " + (object) num));
    }
  }

  private void HideCounter()
  {
    this.QuantitySupply = false;
    if (Object.op_Inequality((Object) this.m_crossForOneDigitCount, (Object) null))
      this.m_crossForOneDigitCount.SetActive(false);
    if (Object.op_Inequality((Object) this.m_crossForTwoDigitsCount, (Object) null))
      this.m_crossForTwoDigitsCount.SetActive(false);
    if (Object.op_Inequality((Object) this.m_crossForThreeDigitsCount, (Object) null))
      this.m_crossForThreeDigitsCount.SetActive(false);
    if (Object.op_Inequality((Object) this.m_crossForFourDigitsCount, (Object) null))
      this.m_crossForFourDigitsCount.SetActive(false);
    ((IEnumerable<GameObject>) this.m_onesDigit).ToggleOnceEx(-1);
    ((IEnumerable<GameObject>) this.m_tensDigit).ToggleOnceEx(-1);
    ((IEnumerable<GameObject>) this.m_hundredsDigit).ToggleOnceEx(-1);
    ((IEnumerable<GameObject>) this.m_thousandsDigit).ToggleOnceEx(-1);
  }

  public bool QuantitySupply
  {
    get => this.quantity.activeSelf;
    set => this.quantity.SetActive(value);
  }

  private void SetCross(FacilityIcon.CounterDigits digits)
  {
    ((IEnumerable<GameObject>) new GameObject[4]
    {
      this.m_crossForOneDigitCount,
      this.m_crossForTwoDigitsCount,
      this.m_crossForThreeDigitsCount,
      this.m_crossForFourDigitsCount
    }).ToggleOnceEx((int) digits);
  }

  private enum CounterDigits
  {
    OneDigit,
    TwoDigits,
    ThreeDigits,
    FourDigits,
  }
}
