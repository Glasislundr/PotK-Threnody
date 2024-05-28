// Decompiled with JetBrains decompiler
// Type: Unit004StorageMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
public class Unit004StorageMenu : UnitMenuBase
{
  [NonSerialized]
  public bool isInit;

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  public IEnumerator Init(PlayerUnit[] storageUnits)
  {
    Unit004StorageMenu unit004StorageMenu = this;
    foreach (PlayerUnit storageUnit in storageUnits)
      storageUnit.is_storage = true;
    Player player = SMManager.Get<Player>();
    string textNumFormat = storageUnits.Length > player.max_unit_reserves ? "[ff0000]{0}[-]/{1}" : "{0}/{1}";
    IEnumerator e;
    if (unit004StorageMenu.isInit)
    {
      e = unit004StorageMenu.UpdateInfoAndScroll(storageUnits);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit004StorageMenu.TxtNumber.SetTextLocalize(string.Format(textNumFormat, (object) storageUnits.Length, (object) player.max_unit_reserves));
    }
    else
    {
      SMManager.Get<PlayerDeck[]>();
      e = unit004StorageMenu.Initialize();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit004StorageMenu.SetIconType(UnitMenuBase.IconType.Normal);
      unit004StorageMenu.InitializeInfoEx((IEnumerable<PlayerUnit>) storageUnits, (IEnumerable<PlayerMaterialUnit>) null, Persist.unit004StorageSortAndFilter, false, false, true, true, true, false);
      e = unit004StorageMenu.CreateUnitIcon();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit004StorageMenu.TxtNumber.SetTextLocalize(string.Format(textNumFormat, (object) storageUnits.Length, (object) player.max_unit_reserves));
      unit004StorageMenu.InitializeEnd();
      unit004StorageMenu.isInit = true;
    }
  }

  protected override IEnumerator CreateUnitIcon(
    int info_index,
    int unit_index,
    PlayerUnit baseUnit = null)
  {
    IEnumerator e = base.CreateUnitIcon(info_index, unit_index, baseUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.CreateUnitIconAction(info_index, unit_index);
  }

  protected override void CreateUnitIconCache(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    base.CreateUnitIconCache(info_index, unit_index);
    this.CreateUnitIconAction(info_index, unit_index);
  }

  private void CreateUnitIconAction(int info_index, int unit_index)
  {
    UnitIconBase unitIcon = this.allUnitIcons[unit_index];
    unitIcon.onClick = (Action<UnitIconBase>) (ui => this.onClickUnitIcon(unitIcon.PlayerUnit));
    EventDelegate.Set(unitIcon.Button.onLongPress, (EventDelegate.Callback) (() =>
    {
      if (this.IsPushAndSet())
        return;
      this.onLongPressUnitIcon(unitIcon.PlayerUnit);
    }));
  }

  private void onClickUnitIcon(PlayerUnit unit)
  {
    if (this.IsPushAndSet())
      return;
    Unit0042Scene.changeScene(true, unit, this.getUnits());
  }

  private void onLongPressUnitIcon(PlayerUnit unit)
  {
    Unit0042Scene.changeScene(true, unit, this.getUnits());
  }

  public void OnBtnUnitList()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().clearStack();
    Unit00468Scene.changeScene00411(false);
  }

  public void OnBtnStorageOut()
  {
    if (this.IsPushAndSet())
      return;
    Unit004StorageOutScene.changeScene(true);
  }

  public void OnBtnSell()
  {
    if (this.IsPushAndSet())
      return;
    Unit004StorageScene.changeSceneSell(false, false);
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    this.backScene();
  }
}
