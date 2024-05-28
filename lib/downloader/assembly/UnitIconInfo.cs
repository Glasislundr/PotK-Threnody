// Decompiled with JetBrains decompiler
// Type: UnitIconInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using UnityEngine;

#nullable disable
public class UnitIconInfo
{
  private PlayerUnit _playerUnit;
  private UnitUnit _unit;
  public bool removeButton;
  public UnitIconBase icon;
  public bool gray;
  public bool for_battle;
  public bool pricessType;
  public bool isSpecialIcon;
  public bool isTrustMaterial;
  public int specialIconType;
  public bool equip;
  public bool button_enable = true;
  public bool selectMarker;
  public bool is_new;
  public bool is_unknown;
  public bool is_awakeUnti;
  public bool is_tower_entry;
  public bool is_used;
  public bool is_rental;
  public bool is_overkillers;
  public Action callback;
  private int m_selected = -1;
  public int count = 1;
  private int m_selectedCount;
  public int alignmentSequence;
  public bool isTempSelectedCount;
  public int tempSelectedCount;
  public bool IsNoCounterAndGray;

  public bool isNormalUnit => this.unit == null || this.unit.IsNormalUnit;

  public int unitLevel => this._playerUnit != (PlayerUnit) null ? this._playerUnit.total_level : 1;

  public PlayerUnit playerUnit
  {
    get => this._playerUnit;
    set
    {
      this._playerUnit = value;
      this.unit = this._playerUnit.unit;
    }
  }

  public UnitUnit unit
  {
    get
    {
      if (this._playerUnit != (PlayerUnit) null && this._unit == null)
        this._unit = this._playerUnit.unit;
      return this._unit;
    }
    set
    {
      if (!(this._playerUnit == (PlayerUnit) null))
        return;
      this._unit = value;
    }
  }

  public int select
  {
    get => this.m_selected;
    set
    {
      this.m_selected = value;
      if (this.m_selected < 0)
      {
        this.SelectedCount = 0;
      }
      else
      {
        if (this.m_selected <= -1 || this.m_selectedCount != 0)
          return;
        this.SelectedCount = 1;
      }
    }
  }

  public void ComposeSelect() => this.m_selected = 0;

  public void ComposeUnSelect()
  {
    this.m_selected = -1;
    if (this.unit.IsNormalUnit)
      return;
    this.icon.SetSelectionCounter(0);
    this.icon.Gray = false;
  }

  public int SelectedCount
  {
    get => this.isTempSelectedCount ? this.tempSelectedCount : this.m_selectedCount;
    set
    {
      this.m_selectedCount = value;
      if (this.m_selectedCount > 0)
      {
        if (this.m_selected < 0)
          this.m_selected = 0;
      }
      else if (this.m_selected > -1)
        this.m_selected = -1;
      if (!Object.op_Inequality((Object) this.icon, (Object) null) || this.unit.IsNormalUnit || this.IsNoCounterAndGray)
        return;
      this.icon.SetSelectionCounter(this.m_selectedCount);
      this.icon.Gray = this.m_selectedCount > 0;
    }
  }

  public void UpdateTowerEntryViewFlag()
  {
    if (this._playerUnit != (PlayerUnit) null)
      this.is_tower_entry = this._playerUnit.tower_is_entry || this._playerUnit.corps_is_entry;
    else
      this.is_tower_entry = false;
  }

  public UnitIconInfo TempCopy()
  {
    UnitIconInfo unitIconInfo = (UnitIconInfo) this.MemberwiseClone();
    unitIconInfo.icon = (UnitIconBase) null;
    return unitIconInfo;
  }
}
