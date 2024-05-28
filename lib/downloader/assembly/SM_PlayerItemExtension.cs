// Decompiled with JetBrains decompiler
// Type: SM_PlayerItemExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public static class SM_PlayerItemExtension
{
  private const int GUILD_SUPPLY_BOX_TYPE_ID = 4;

  public static PlayerItem[] AllGears(this PlayerItem[] self, Player player)
  {
    return ((IEnumerable<PlayerItem>) self).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.player_id == player.id && x.entity_type == MasterDataTable.CommonRewardType.gear)).ToArray<PlayerItem>();
  }

  public static PlayerItem[] AllGearsWithEquip(this PlayerItem[] self)
  {
    Player player = SMManager.Get<Player>();
    return ((IEnumerable<PlayerItem>) self).Where<PlayerItem>((Func<PlayerItem, bool>) (pi => pi.gear != null && pi.player_id == player.id)).ToArray<PlayerItem>();
  }

  public static PlayerItem[] AllGears(this PlayerItem[] self)
  {
    return ((IEnumerable<PlayerItem>) self).Where<PlayerItem>((Func<PlayerItem, bool>) (pi => !pi.ForBattle && pi.gear != null)).ToArray<PlayerItem>();
  }

  public static PlayerItem[] AllSupplies(this PlayerItem[] self)
  {
    return ((IEnumerable<PlayerItem>) self).Where<PlayerItem>((Func<PlayerItem, bool>) (pi => pi.entity_type == MasterDataTable.CommonRewardType.supply && pi.box_type_id <= 1)).ToArray<PlayerItem>();
  }

  public static PlayerItem[] AllBattleSupplies(this PlayerItem[] self)
  {
    return ((IEnumerable<PlayerItem>) self).Where<PlayerItem>((Func<PlayerItem, bool>) (pi => pi.supply != null && pi.ForBattle && pi.box_type_id != 3 && pi.box_type_id != 4)).ToArray<PlayerItem>();
  }

  public static PlayerItem[] AllTowerSupplies(this PlayerItem[] self)
  {
    return ((IEnumerable<PlayerItem>) self).Where<PlayerItem>((Func<PlayerItem, bool>) (pi => pi.supply != null && pi.box_type_id == 3)).ToArray<PlayerItem>();
  }

  public static PlayerItem[] AllRaidSupplies(this PlayerItem[] self)
  {
    return ((IEnumerable<PlayerItem>) self).Where<PlayerItem>((Func<PlayerItem, bool>) (pi => pi.supply != null && pi.box_type_id == 4)).ToArray<PlayerItem>();
  }

  public static int AmountHavingTargetItem(
    this PlayerItem[] self,
    int entity_id,
    MasterDataTable.CommonRewardType entity_type)
  {
    int num;
    switch (entity_type)
    {
      case MasterDataTable.CommonRewardType.supply:
      case MasterDataTable.CommonRewardType.gear:
        num = ((IEnumerable<PlayerItem>) self).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.entity_type == entity_type && x.entity_id == entity_id)).Sum<PlayerItem>((Func<PlayerItem, int>) (y => y.quantity));
        break;
      default:
        num = 0;
        break;
    }
    return num;
  }
}
