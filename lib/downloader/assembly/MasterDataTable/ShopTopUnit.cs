// Decompiled with JetBrains decompiler
// Type: MasterDataTable.ShopTopUnit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class ShopTopUnit
  {
    public int ID;
    public int unit_id;
    public DateTime? start_at;
    public DateTime? end_at;

    public static ShopTopUnit Parse(MasterDataReader reader)
    {
      return new ShopTopUnit()
      {
        ID = reader.ReadInt(),
        unit_id = reader.ReadInt(),
        start_at = reader.ReadDateTimeOrNull(),
        end_at = reader.ReadDateTimeOrNull()
      };
    }

    public static UnitUnit GetShopTopUnit()
    {
      DateTime now = ShopCommon.LoginTime;
      ShopTopUnit shopTopUnit = ((IEnumerable<ShopTopUnit>) MasterData.ShopTopUnitList).FirstOrDefault<ShopTopUnit>((Func<ShopTopUnit, bool>) (x =>
      {
        if (x.start_at.HasValue && (!x.start_at.HasValue || !(x.start_at.Value <= now)))
          return false;
        if (!x.end_at.HasValue)
          return true;
        if (!x.end_at.HasValue)
          return false;
        DateTime? endAt = x.end_at;
        DateTime dateTime = now;
        return endAt.HasValue && endAt.GetValueOrDefault() >= dateTime;
      }));
      return shopTopUnit != null && MasterData.UnitUnit.ContainsKey(shopTopUnit.unit_id) ? MasterData.UnitUnit[shopTopUnit.unit_id] : (UnitUnit) null;
    }
  }
}
