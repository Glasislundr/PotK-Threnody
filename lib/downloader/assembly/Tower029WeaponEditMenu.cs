// Decompiled with JetBrains decompiler
// Type: Tower029WeaponEditMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Tower029WeaponEditMenu : UnitMenuBase
{
  private GameObject goHpGauge;

  private void SetUnitIconGray(int unit_index)
  {
    if (unit_index >= this.allUnitIcons.Count || Object.op_Equality((Object) this.allUnitIcons[unit_index], (Object) null))
      return;
    UnitIcon allUnitIcon = (UnitIcon) this.allUnitIcons[unit_index];
    if ((double) allUnitIcon.PlayerUnit.tower_hitpoint_rate <= 0.0)
      allUnitIcon.Gray = true;
    if (Object.op_Equality((Object) allUnitIcon.HpGauge, (Object) null))
      return;
    allUnitIcon.HpGauge.SetGaugeAndDropoutIcon(allUnitIcon.PlayerUnit.TowerHp, allUnitIcon.PlayerUnit.total_hp, false);
  }

  public IEnumerator InitializeAsync()
  {
    Tower029WeaponEditMenu tower029WeaponEditMenu = this;
    IEnumerable<PlayerUnit> playerUnits = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (u => u.tower_is_entry));
    IEnumerator e = tower029WeaponEditMenu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    tower029WeaponEditMenu.SetIconType(UnitMenuBase.IconType.NormalWithHpGauge);
    tower029WeaponEditMenu.InitializeInfo(playerUnits, (IEnumerable<PlayerMaterialUnit>) null, Persist.tower029UnitListSortAndFilter, true, false, true, true, false);
    e = tower029WeaponEditMenu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    tower029WeaponEditMenu.TxtNumberpossession.SetTextLocalize(Consts.GetInstance().TOWER_TEAM_EDIT_ALIVE_UNIT);
    int num = playerUnits.Count<PlayerUnit>();
    tower029WeaponEditMenu.TxtNumber.SetTextLocalize(string.Format("{0}/{1}", (object) playerUnits.Where<PlayerUnit>((Func<PlayerUnit, bool>) (u => (double) u.TowerHpRate > 0.0)).Count<PlayerUnit>(), (object) num));
    tower029WeaponEditMenu.lastReferenceUnitID = -1;
    tower029WeaponEditMenu.lastReferenceUnitIndex = -1;
    tower029WeaponEditMenu.InitializeEnd();
  }

  public override void ForBattle(Func<UnitIconInfo, PlayerUnit, bool> func)
  {
    if (TowerUtil.towerDeckUnits == null)
      return;
    List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
    PlayerUnit[] source = SMManager.Get<PlayerUnit[]>();
    for (int i = 0; i < TowerUtil.towerDeckUnits.Length; i++)
    {
      PlayerUnit playerUnit = ((IEnumerable<PlayerUnit>) source).Where<PlayerUnit>((Func<PlayerUnit, bool>) (u => u.id == TowerUtil.towerDeckUnits[i].player_unit_id)).FirstOrDefault<PlayerUnit>();
      if (playerUnit != (PlayerUnit) null)
        playerUnitList.Add(playerUnit);
    }
    foreach (PlayerUnit playerUnit in playerUnitList)
    {
      PlayerUnit unit = playerUnit;
      if (!(unit == (PlayerUnit) null))
      {
        UnitIconInfo unitIconInfo = this.allUnitInfos.FirstOrDefault<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => func(x, unit)));
        if (unitIconInfo != null)
          unitIconInfo.for_battle = true;
      }
    }
  }

  public override void UpdateAllUnitTowerEntryView()
  {
  }

  public override void onBackButton() => this.backScene();

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
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
    this.SetUnitIconGray(unit_index);
  }

  protected override void CreateUnitIconCache(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    base.CreateUnitIconCache(info_index, unit_index);
    this.CreateUnitIconAction(info_index, unit_index);
    this.SetUnitIconGray(unit_index);
  }

  protected void CreateUnitIconAction(int info_index, int unit_index)
  {
    UnitIconBase unitIcon = this.allUnitIcons[unit_index];
    if (unitIcon.PlayerUnit == (PlayerUnit) null)
      Debug.LogError((object) "Tower029WeaponEditMenu CreateUnitIconAction PlayerUnit == null");
    unitIcon.onClick = (Action<UnitIconBase>) (ui =>
    {
      this.lastReferenceUnitID = unitIcon.PlayerUnit.id;
      this.lastReferenceUnitIndex = this.GetUnitInfoDisplayIndex(unitIcon.PlayerUnit);
      Unit0044Scene.ChangeScene(true, unitIcon.PlayerUnit, 1);
    });
  }

  protected override IEnumerator CreateUnitIconBase(GameObject prefab)
  {
    Tower029WeaponEditMenu tower029WeaponEditMenu = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = tower029WeaponEditMenu.\u003C\u003En__1(prefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Equality((Object) tower029WeaponEditMenu.goHpGauge, (Object) null))
    {
      Future<GameObject> f = Res.Prefabs.tower.dir_hp_gauge.Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      tower029WeaponEditMenu.goHpGauge = f.Result;
      f = (Future<GameObject>) null;
    }
    if (Object.op_Inequality((Object) tower029WeaponEditMenu.goHpGauge, (Object) null))
    {
      for (int index = 0; index < tower029WeaponEditMenu.allUnitIcons.Count; ++index)
      {
        UnitIcon allUnitIcon = (UnitIcon) tower029WeaponEditMenu.allUnitIcons[index];
        tower029WeaponEditMenu.goHpGauge.Clone(allUnitIcon.hp_gauge.transform);
        if (allUnitIcon.PlayerUnit != (PlayerUnit) null)
          allUnitIcon.HpGauge.SetGaugeAndDropoutIcon(allUnitIcon.PlayerUnit.TowerHp, allUnitIcon.PlayerUnit.total_hp, false);
      }
    }
  }
}
