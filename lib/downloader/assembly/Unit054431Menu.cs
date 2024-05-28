// Decompiled with JetBrains decompiler
// Type: Unit054431Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Earth/unit054_4_3_1/Menu")]
public class Unit054431Menu : Unit004431Menu
{
  protected override PlayerItem[] GetAllGears(PlayerItem[] items)
  {
    return ((IEnumerable<PlayerItem>) items).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.gear != null)).ToArray<PlayerItem>();
  }

  public override IEnumerator Init(
    Player player,
    PlayerUnit[] playerUnits,
    Unit004431Menu.Param sendParam,
    bool isEquip)
  {
    this.isEarthMode = true;
    this.SetIconType(UnitMenuBase.IconType.Normal);
    this.TxtNumber.SetTextLocalize(string.Format("{0}", (object) playerUnits.Length));
    return base.Init(player, playerUnits, sendParam, isEquip);
  }

  protected override void ChangeDetailScene(PlayerUnit unit)
  {
    Unit0542Scene.changeSceneEvolutionUnit(true, unit, this.getUnits());
  }

  protected override PlayerUnit[] getCanEquippedUnits(PlayerUnit[] playerUnits)
  {
    if (this.choiceGear == (PlayerItem) null)
      return new PlayerUnit[0];
    GearGear gear = this.choiceGear.gear;
    IEnumerable enumerable = this.sendParam.gearKindId == 7 || this.sendParam.gearKindId == 10 ? (IEnumerable) ((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (v => v.unit.IsNormalUnit && gear.enableEquipmentUnit(v))) : (IEnumerable) ((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (v =>
    {
      UnitUnit unit = v.unit;
      return unit.IsNormalUnit && (unit.kind_GearKind == this.sendParam.gearKindId && gear.enableEquipmentUnit(v) || unit.IsAllEquipUnit && gear.enableEquipmentUnit(v));
    }));
    List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
    GearKind kind = gear.kind;
    foreach (PlayerUnit playerUnit in enumerable)
    {
      MasterDataTable.UnitJob jobData = playerUnit.getJobData();
      int num1 = 0;
      int? classificationPattern;
      if (jobData.classification_GearClassificationPattern.HasValue)
      {
        classificationPattern = jobData.classification_GearClassificationPattern;
        int num2 = 0;
        if (!(classificationPattern.GetValueOrDefault() == num2 & classificationPattern.HasValue))
          num1 = jobData.classification_GearClassificationPattern.Value;
      }
      if (num1 == 0)
      {
        playerUnitList.Add(playerUnit);
      }
      else
      {
        int num3 = num1;
        classificationPattern = gear.classification_GearClassificationPattern;
        int valueOrDefault = classificationPattern.GetValueOrDefault();
        if (num3 == valueOrDefault & classificationPattern.HasValue || kind.isNonWeapon)
          playerUnitList.Add(playerUnit);
      }
    }
    return playerUnitList.ToArray();
  }
}
