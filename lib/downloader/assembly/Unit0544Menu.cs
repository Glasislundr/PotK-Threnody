// Decompiled with JetBrains decompiler
// Type: Unit0544Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Earth/unit054_4/Menu")]
public class Unit0544Menu : Unit0044Menu
{
  public override Persist<Persist.ItemSortAndFilterInfo> GetPersist()
  {
    return Persist.bugu0544SortAndFilter;
  }

  protected override void ChangeDetailScene(GameCore.ItemInfo item)
  {
    Unit05443Scene.changeSceneLimited(true, item);
  }

  protected override void BottomInfoUpdate()
  {
    int num = 0;
    foreach (PlayerItem playerItem in SMManager.Get<PlayerItem[]>())
    {
      if (!playerItem.isSupply() && !playerItem.isReisou())
        ++num;
    }
    this.TxtNumberpossession.SetTextLocalize(string.Format("{0}", (object) num));
  }

  protected override List<PlayerItem> GetItemList()
  {
    PlayerItem[] source1 = SMManager.Get<PlayerItem[]>();
    UnitUnit baseUnit = this.BasePlayerUnit.unit;
    IEnumerable<PlayerItem> source2 = (IEnumerable<PlayerItem>) null;
    if (baseUnit.IsAllEquipUnit)
      source2 = ((IEnumerable<PlayerItem>) source1).Where<PlayerItem>((Func<PlayerItem, bool>) (x => !x.broken && x.isWeapon() && x.gear.enableEquipmentUnit(this.BasePlayerUnit)));
    else if (baseUnit.awake_unit_flag)
    {
      switch (this.ChangeGearIndex)
      {
        case 1:
          source2 = ((IEnumerable<PlayerItem>) source1).Where<PlayerItem>((Func<PlayerItem, bool>) (x =>
          {
            if (x.broken || !x.isWeapon())
              return false;
            GearGear gear = x.gear;
            return gear.kind_GearKind == baseUnit.kind_GearKind && gear.enableEquipmentUnit(this.BasePlayerUnit);
          }));
          break;
        case 2:
          source2 = ((IEnumerable<PlayerItem>) source1).Where<PlayerItem>((Func<PlayerItem, bool>) (x =>
          {
            if (x.broken || !x.isWeapon())
              return false;
            GearGear gear = x.gear;
            return (gear.kind_GearKind == 7 || gear.kind_GearKind == 10) && gear.enableEquipmentUnit(this.BasePlayerUnit);
          }));
          break;
      }
    }
    if (source2 == null)
      source2 = ((IEnumerable<PlayerItem>) source1).Where<PlayerItem>((Func<PlayerItem, bool>) (x =>
      {
        if (x.broken || !x.isWeapon())
          return false;
        GearGear gear = x.gear;
        return (gear.kind_GearKind == 7 || gear.kind_GearKind == 10 || gear.kind_GearKind == baseUnit.kind_GearKind) && gear.enableEquipmentUnit(this.BasePlayerUnit);
      }));
    MasterDataTable.UnitJob jobData = this.BasePlayerUnit.getJobData();
    int classification_GearClassificationPattern = jobData.classification_GearClassificationPattern.IsValid() ? jobData.classification_GearClassificationPattern.Value : 0;
    return classification_GearClassificationPattern.IsValid() ? source2.Where<PlayerItem>((Func<PlayerItem, bool>) (x =>
    {
      GearGear gear = x.gear;
      int? classificationPattern = gear.classification_GearClassificationPattern;
      int num = classification_GearClassificationPattern;
      return classificationPattern.GetValueOrDefault() == num & classificationPattern.HasValue || gear.kind.isNonWeapon;
    })).ToList<PlayerItem>() : source2.ToList<PlayerItem>();
  }
}
