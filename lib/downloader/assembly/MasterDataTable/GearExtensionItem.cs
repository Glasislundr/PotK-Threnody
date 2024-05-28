// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GearExtensionItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GearExtensionItem
  {
    public int ID;
    public int same_character_id;
    public int? kind_GearKind;
    public int material1_id;
    public int material1_num;
    public int material2_id;
    public int material2_num;
    public int material3_id;
    public int material3_num;
    public int material4_id;
    public int material4_num;
    public int material5_id;
    public int material5_num;

    public static GearExtensionItem Parse(MasterDataReader reader)
    {
      return new GearExtensionItem()
      {
        ID = reader.ReadInt(),
        same_character_id = reader.ReadInt(),
        kind_GearKind = reader.ReadIntOrNull(),
        material1_id = reader.ReadInt(),
        material1_num = reader.ReadInt(),
        material2_id = reader.ReadInt(),
        material2_num = reader.ReadInt(),
        material3_id = reader.ReadInt(),
        material3_num = reader.ReadInt(),
        material4_id = reader.ReadInt(),
        material4_num = reader.ReadInt(),
        material5_id = reader.ReadInt(),
        material5_num = reader.ReadInt()
      };
    }

    public GearKind kind
    {
      get
      {
        if (!this.kind_GearKind.HasValue)
          return (GearKind) null;
        GearKind kind;
        if (!MasterData.GearKind.TryGetValue(this.kind_GearKind.Value, out kind))
          Debug.LogError((object) ("Key not Found: MasterData.GearKind[" + (object) this.kind_GearKind.Value + "]"));
        return kind;
      }
    }
  }
}
