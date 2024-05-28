// Decompiled with JetBrains decompiler
// Type: MasterDataTable.FacilityLevel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class FacilityLevel
  {
    public int ID;
    public int facility_MapFacility;
    public int level;
    public int unit_UnitUnit;
    public string attach_point_name;

    public static FacilityLevel Parse(MasterDataReader reader)
    {
      return new FacilityLevel()
      {
        ID = reader.ReadInt(),
        facility_MapFacility = reader.ReadInt(),
        level = reader.ReadInt(),
        unit_UnitUnit = reader.ReadInt(),
        attach_point_name = reader.ReadStringOrNull(true)
      };
    }

    public MapFacility facility
    {
      get
      {
        MapFacility facility;
        if (!MasterData.MapFacility.TryGetValue(this.facility_MapFacility, out facility))
          Debug.LogError((object) ("Key not Found: MasterData.MapFacility[" + (object) this.facility_MapFacility + "]"));
        return facility;
      }
    }

    public UnitUnit unit
    {
      get
      {
        UnitUnit unit;
        if (!MasterData.UnitUnit.TryGetValue(this.unit_UnitUnit, out unit))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.unit_UnitUnit + "]"));
        return unit;
      }
    }
  }
}
