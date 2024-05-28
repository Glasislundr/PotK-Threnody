// Decompiled with JetBrains decompiler
// Type: MasterDataTable.MapFacility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class MapFacility
  {
    public int ID;
    public int category_id;
    public bool is_target;
    public bool is_puton;
    public bool is_view;
    public int max_lv;
    public int type;
    public int max_possession;
    public int sell_price;

    public static MapFacility Parse(MasterDataReader reader)
    {
      return new MapFacility()
      {
        ID = reader.ReadInt(),
        category_id = reader.ReadInt(),
        is_target = reader.ReadBool(),
        is_puton = reader.ReadBool(),
        is_view = reader.ReadBool(),
        max_lv = reader.ReadInt(),
        type = reader.ReadInt(),
        max_possession = reader.ReadInt(),
        sell_price = reader.ReadInt()
      };
    }

    public string name
    {
      get
      {
        FacilityLevel facilityLevel = ((IEnumerable<FacilityLevel>) MasterData.FacilityLevelList).Where<FacilityLevel>((Func<FacilityLevel, bool>) (x => x.facility_MapFacility == this.ID && x.level == 1)).FirstOrDefault<FacilityLevel>();
        return facilityLevel == null ? Consts.GetInstance().GUILD_FACILITY_PRESENT_OTHER : facilityLevel.unit.name;
      }
    }
  }
}
