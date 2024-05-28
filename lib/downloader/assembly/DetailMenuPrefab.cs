// Decompiled with JetBrains decompiler
// Type: DetailMenuPrefab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class DetailMenuPrefab : MonoBehaviour
{
  public DetailMenu normal;
  public MaterialDetailMenu material;
  protected int index = -1;
  private PlayerUnit _playerUnit;
  private UnitUnit unitUnit_;

  public bool isInitalizing { get; protected set; }

  public int Index => this.index;

  public PlayerUnit PlayerUnit => this._playerUnit;

  public IEnumerator Init(
    Unit0042Menu menu,
    int index,
    PlayerUnit playerUnit,
    int infoIndex,
    bool isLimit,
    QuestScoreBonusTimetable[] tables,
    UnitBonus[] unitBonus,
    bool isUpdate = true,
    bool isMaterial = false,
    PlayerUnit baseUnit = null)
  {
    this.isInitalizing = true;
    this.normal.IsPush = true;
    this.material.IsPush = true;
    this.index = index;
    this._playerUnit = playerUnit;
    this.unitUnit_ = playerUnit.unit;
    IEnumerator e;
    if (this.unitUnit_.IsNormalUnit)
    {
      ((Component) this.normal).gameObject.SetActive(true);
      ((Component) this.material).gameObject.SetActive(false);
      e = this.normal.Init(menu, index, playerUnit, infoIndex, isLimit, isMaterial, tables, unitBonus, isUpdate, baseUnit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      ((Component) this.normal).gameObject.SetActive(false);
      ((Component) this.material).gameObject.SetActive(true);
      e = this.material.Init(menu, index, playerUnit, infoIndex, isLimit, isMaterial, tables, unitBonus, isUpdate, (PlayerUnit) null);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.normal.IsPush = false;
    this.material.IsPush = false;
    this.isInitalizing = false;
  }

  public void SetInformationPanelIndex(int index)
  {
    if (!(this.PlayerUnit != (PlayerUnit) null))
      return;
    if (this.unitUnit_.IsNormalUnit)
      this.StartCoroutine(this.normal.SetInformationPanelIndex(index));
    else
      this.StartCoroutine(this.material.SetInformationPanelIndex(index));
  }

  public void SetInformationPaneEnable(bool enable, int? resetCenter = null)
  {
    if (!(this.PlayerUnit != (PlayerUnit) null) || !this.unitUnit_.IsNormalUnit)
      return;
    ((Behaviour) this.normal.InformationScrollView.uiScrollView).enabled = enable;
    if (enable || !resetCenter.HasValue)
      return;
    this.normal.InformationScrollView.resetCenterItem(resetCenter.Value);
  }

  public void SetInformationPanelTab(DetailMenuScrollViewParam.TabMode mode)
  {
    if (!(this.PlayerUnit != (PlayerUnit) null) || !this.unitUnit_.IsNormalUnit)
      return;
    this.normal.SetInformationPanelTab(mode);
  }

  public void SetInformationPanelTab(DetailMenuJobTab.TabMode mode)
  {
    if (!(this.PlayerUnit != (PlayerUnit) null) || !this.unitUnit_.IsNormalUnit)
      return;
    this.normal.SetInformationPanelTab(mode);
  }

  public IEnumerator initAsyncDiffMode(
    PlayerUnit playerUnit,
    PlayerUnit prevUnit,
    int center,
    IDetailMenuContainer menuContainer)
  {
    this._playerUnit = playerUnit;
    ((Component) this.normal).gameObject.SetActive(true);
    ((Component) this.material).gameObject.SetActive(false);
    yield return (object) this.normal.initAsyncDiffMode(playerUnit, prevUnit, center, menuContainer);
  }

  public void resetItemPositionDiffMode(int pos)
  {
    NGHorizontalScrollParts informationScrollView = this.normal.InformationScrollView;
    if (informationScrollView.selected == pos)
      return;
    informationScrollView.scrollView.GetComponent<UIScrollView>().DisableSpring();
    informationScrollView.resetCenterItem(pos);
  }

  public int getCenterItemPositionDiffMode() => this.normal.InformationScrollView.selected;

  public void setActiveDiffMode(bool flag)
  {
    this.normal.InformationScrollView.SeEnable = flag;
    ((Component) this).gameObject.SetActive(flag);
  }

  public IEnumerator initPickup(
    Unit0042PickupMenu pickupMenu,
    int index,
    PlayerUnit playerUnit,
    int infoIndex)
  {
    this.isInitalizing = true;
    this.normal.IsPush = true;
    this.index = index;
    this._playerUnit = playerUnit;
    this.unitUnit_ = playerUnit.unit;
    ((Component) this.normal).gameObject.SetActive(true);
    yield return (object) this.normal.initPickup(pickupMenu, index, playerUnit, infoIndex);
    this.normal.IsPush = false;
    this.isInitalizing = false;
  }

  public void setPickupPanelIndex(int index)
  {
    if (!(this.PlayerUnit != (PlayerUnit) null))
      return;
    this.normal.setPickupPanelIndex(index);
  }
}
