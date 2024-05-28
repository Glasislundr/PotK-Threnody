// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GearDrillingExpMythology
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GearDrillingExpMythology
  {
    public int ID;
    public string name;
    public int reisou_type_id_GearReisouType;
    public int? holy;
    public int? chaos;

    public static GearDrillingExpMythology Parse(MasterDataReader reader)
    {
      return new GearDrillingExpMythology()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        reisou_type_id_GearReisouType = reader.ReadInt(),
        holy = reader.ReadIntOrNull(),
        chaos = reader.ReadIntOrNull()
      };
    }

    public GearReisouType reisou_type_id => (GearReisouType) this.reisou_type_id_GearReisouType;
  }
}
