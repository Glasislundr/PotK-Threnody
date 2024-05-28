// Decompiled with JetBrains decompiler
// Type: Unit004JobChangeUnitSelectMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
public class Unit004JobChangeUnitSelectMenu : UnitMenuBase
{
  private long? wRevision_;

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  public bool isNeedReset
  {
    get
    {
      bool isNeedReset = !this.wRevision_.HasValue;
      long num = SMManager.Revision<PlayerUnit[]>();
      if (!isNeedReset)
        isNeedReset = this.wRevision_.Value != num;
      this.wRevision_ = new long?(num);
      return isNeedReset;
    }
  }

  public IEnumerator Init(PlayerUnit[] targets)
  {
    Unit004JobChangeUnitSelectMenu changeUnitSelectMenu = this;
    IEnumerator e = changeUnitSelectMenu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    changeUnitSelectMenu.InitializeInfo((IEnumerable<PlayerUnit>) targets, (IEnumerable<PlayerMaterialUnit>) null, Persist.unit004JobChangeUnitSelectSortAndFilter, false, false, true, true, false);
    e = changeUnitSelectMenu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    changeUnitSelectMenu.lastReferenceUnitID = -1;
    changeUnitSelectMenu.InitializeEnd();
  }

  private void ChangeScene(UnitIconBase unitIcon)
  {
    if (!(unitIcon.PlayerUnit != (PlayerUnit) null) || this.IsPushAndSet())
      return;
    this.lastReferenceUnitID = unitIcon.PlayerUnit.id;
    this.lastReferenceUnitIndex = this.GetUnitInfoDisplayIndex(unitIcon.PlayerUnit);
    Unit004JobChangeScene.changeSceneBySelector(unitIcon.PlayerUnit.id);
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
    this.allUnitIcons[unit_index].onClick = (Action<UnitIconBase>) (ui => this.ChangeScene(ui));
  }

  public override void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<CommonRoot>.GetInstance().setStartScene("mypage");
    this.backScene();
  }
}
