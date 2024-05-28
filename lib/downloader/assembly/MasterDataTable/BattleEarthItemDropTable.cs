// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleEarthItemDropTable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BattleEarthItemDropTable
  {
    public int ID;
    public int drop_table_id;
    public int reward_type_CommonRewardType;
    public int? reward_id;
    public int quantity;
    public int _appearance;

    public static BattleEarthItemDropTable Parse(MasterDataReader reader)
    {
      return new BattleEarthItemDropTable()
      {
        ID = reader.ReadInt(),
        drop_table_id = reader.ReadInt(),
        reward_type_CommonRewardType = reader.ReadInt(),
        reward_id = reader.ReadIntOrNull(),
        quantity = reader.ReadInt(),
        _appearance = reader.ReadInt()
      };
    }

    public CommonRewardType reward_type => (CommonRewardType) this.reward_type_CommonRewardType;

    public static BattleEarthItemDropTable RandomGetDropItem(int dropTableID)
    {
      IEnumerable<BattleEarthItemDropTable> source = ((IEnumerable<BattleEarthItemDropTable>) MasterData.BattleEarthItemDropTableList).Where<BattleEarthItemDropTable>((Func<BattleEarthItemDropTable, bool>) (x => x.drop_table_id == dropTableID));
      int num1 = Random.Range(0, source.Sum<BattleEarthItemDropTable>((Func<BattleEarthItemDropTable, int>) (x => x._appearance)));
      int num2 = 0;
      foreach (BattleEarthItemDropTable dropItem in (IEnumerable<BattleEarthItemDropTable>) source.OrderByDescending<BattleEarthItemDropTable, int>((Func<BattleEarthItemDropTable, int>) (x => x._appearance)))
      {
        num2 += dropItem._appearance;
        if (num2 >= num1)
          return dropItem;
      }
      return (BattleEarthItemDropTable) null;
    }
  }
}
