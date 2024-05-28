// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GearSpecificationOfEquipmentUnit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GearSpecificationOfEquipmentUnit
  {
    public int ID;
    public int group_id;
    public int unit_id;
    public int? job_id;

    public static GearSpecificationOfEquipmentUnit Parse(MasterDataReader reader)
    {
      return new GearSpecificationOfEquipmentUnit()
      {
        ID = reader.ReadInt(),
        group_id = reader.ReadInt(),
        unit_id = reader.ReadInt(),
        job_id = reader.ReadIntOrNull()
      };
    }
  }
}
