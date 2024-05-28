// Decompiled with JetBrains decompiler
// Type: UnitStorageMenuBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class UnitStorageMenuBase : UnitSelectMenuBase
{
  [SerializeField]
  protected UILabel txtStorageUnitPossession;
  protected PlayerUnit[] playerUnits;
  private Persist<Persist.UnitSortAndFilterInfo> sortAndFilterInfo;
  [SerializeField]
  protected UIButton ibtnEnter;
  [SerializeField]
  protected UIButton ibtnClear;
  protected PlayerUnit[] displayUnits;
  protected bool isBack;
  protected bool isInit;
  protected int selectMaxInitialValue;

  public Persist<Persist.UnitSortAndFilterInfo> StorageMenuSortAndFilterInfo
  {
    get => this.sortAndFilterInfo;
  }

  public virtual IEnumerator Init(
    PlayerUnit[] units,
    int storageCount,
    Persist<Persist.UnitSortAndFilterInfo> sortInfo,
    bool isBack = false)
  {
    UnitStorageMenuBase unitStorageMenuBase = this;
    Player player = SMManager.Get<Player>();
    IEnumerator e;
    if (unitStorageMenuBase.isInit)
    {
      unitStorageMenuBase.SetDisplayUnits(units);
      unitStorageMenuBase.SetPlayerUnits();
      unitStorageMenuBase.SelectMax = unitStorageMenuBase.SetSelectMax(storageCount, player);
      e = unitStorageMenuBase.UpdateInfoAndScroll(units);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unitStorageMenuBase.UpdateInfomation();
      unitStorageMenuBase.UpdatePosession(storageCount, player);
    }
    else
    {
      unitStorageMenuBase.selectMaxInitialValue = unitStorageMenuBase.SelectMax;
      PlayerDeck[] playerDeck = SMManager.Get<PlayerDeck[]>();
      unitStorageMenuBase.sortAndFilterInfo = sortInfo;
      unitStorageMenuBase.SetDisplayUnits(units);
      unitStorageMenuBase.SetPlayerUnits();
      unitStorageMenuBase.isBack = isBack;
      unitStorageMenuBase.SelectMax = unitStorageMenuBase.SetSelectMax(storageCount, player);
      e = unitStorageMenuBase.Initialize();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unitStorageMenuBase.selectedUnitIcons.Clear();
      unitStorageMenuBase.SetIconType(UnitMenuBase.IconType.Normal);
      unitStorageMenuBase.InitializeInfoEx((IEnumerable<PlayerUnit>) unitStorageMenuBase.displayUnits, (IEnumerable<PlayerMaterialUnit>) null, sortInfo, false, false, true, true, true, false, (Action) (() => this.InitializeAllUnitInfosExtend(playerDeck)));
      e = unitStorageMenuBase.CreateUnitIcon();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unitStorageMenuBase.UpdateInfomation();
      unitStorageMenuBase.UpdatePosession(storageCount, player);
      unitStorageMenuBase.InitializeEnd();
      unitStorageMenuBase.isInit = true;
    }
  }

  private void UpdatePosession(int storageCount, Player player)
  {
    ((UIWidget) this.txtStorageUnitPossession).color = storageCount >= player.max_unit_reserves ? Color.red : Color.white;
    this.txtStorageUnitPossession.SetTextLocalize(string.Format("{0}/{1}", (object) storageCount, (object) player.max_unit_reserves));
    ((UIWidget) this.TxtNumberpossession).color = this.playerUnits.Length >= player.max_units ? Color.red : Color.white;
    this.TxtNumberpossession.SetTextLocalize(string.Format("{0}/{1}", (object) this.playerUnits.Length, (object) player.max_units));
  }

  public virtual void SetDisplayUnits(PlayerUnit[] units) => this.displayUnits = units;

  public virtual void SetPlayerUnits() => this.playerUnits = this.displayUnits;

  public virtual int SetSelectMax(int storageCount, Player player) => this.SelectMax;

  public override void UpdateInfomation()
  {
    int count = this.selectedUnitIcons.Count;
    ((UIWidget) this.TxtNumberselect).color = count >= this.SelectMax ? Color.red : Color.white;
    this.TxtNumberselect.SetTextLocalize(string.Format("{0}/{1}", (object) count, (object) this.SelectMax));
    this.SetEnableIbtnEnter(count);
    this.SetEnableIbtnClear(count);
  }

  private void SetEnableIbtnEnter(int selectedUnitsCount)
  {
    if (selectedUnitsCount > 0 && selectedUnitsCount <= this.SelectMax)
      ((UIButtonColor) this.ibtnEnter).isEnabled = true;
    else
      ((UIButtonColor) this.ibtnEnter).isEnabled = false;
  }

  private void SetEnableIbtnClear(int selectedUnitsCount)
  {
    if (selectedUnitsCount > 0)
      ((UIButtonColor) this.ibtnClear).isEnabled = true;
    else
      ((UIButtonColor) this.ibtnClear).isEnabled = false;
  }

  public override void InitializeAllUnitInfosExtend(PlayerDeck[] playerDeck)
  {
    this.ReflectionSelectUnit();
    this.ConsiderFavoriteUnit();
    this.IgnoreOverkillers();
    this.IgnoreDeckUnit(playerDeck);
    foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
    {
      if (allUnitInfo != null && SMManager.Get<PlayerRentalPlayerUnitIds>() != null && SMManager.Get<PlayerRentalPlayerUnitIds>().rental_player_unit_ids != null && (allUnitInfo.playerUnit.tower_is_entry || allUnitInfo.playerUnit.corps_is_entry || ((IEnumerable<int?>) SMManager.Get<PlayerRentalPlayerUnitIds>().rental_player_unit_ids).Contains<int?>(new int?(allUnitInfo.playerUnit.id))))
      {
        allUnitInfo.select = -1;
        allUnitInfo.gray = true;
        allUnitInfo.button_enable = false;
      }
    }
    this.CreateSelectUnitList();
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    this.backScene();
  }

  public void OnBtnBack()
  {
    this.IbtnClearS();
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public void OnBtnStorageList()
  {
    this.IbtnClearS();
    if (this.IsPushAndSet())
      return;
    Unit004StorageScene.changeSceneList(true);
  }

  public void OnBtnUnitList()
  {
    this.IbtnClearS();
    if (this.IsPushAndSet())
      return;
    Unit00468Scene.changeScene00411(false);
  }

  public void OnBtnStorageIn()
  {
    this.IbtnClearS();
    if (this.IsPushAndSet())
      return;
    Unit004StorageInScene.changeScene(false);
  }

  public void OnBtnStorageOut()
  {
    this.IbtnClearS();
    if (this.IsPushAndSet())
      return;
    Unit004StorageOutScene.changeScene(false);
  }
}
