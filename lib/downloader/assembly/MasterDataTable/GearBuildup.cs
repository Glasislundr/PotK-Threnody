// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GearBuildup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GearBuildup
  {
    public int ID;
    public int kind_GearKind;
    public int rank;
    public int amount;
    public int max_hp_incremental;
    public int max_strength_incremental;
    public int max_vitality_incremental;
    public int max_intelligence_incremental;
    public int max_mind_incremental;
    public int max_agility_incremental;
    public int max_dexterity_incremental;
    public int max_lucky_incremental;
    public DateTime? start_at;
    public DateTime? end_at;
    public int _default;

    public static GearBuildup Parse(MasterDataReader reader)
    {
      return new GearBuildup()
      {
        ID = reader.ReadInt(),
        kind_GearKind = reader.ReadInt(),
        rank = reader.ReadInt(),
        amount = reader.ReadInt(),
        max_hp_incremental = reader.ReadInt(),
        max_strength_incremental = reader.ReadInt(),
        max_vitality_incremental = reader.ReadInt(),
        max_intelligence_incremental = reader.ReadInt(),
        max_mind_incremental = reader.ReadInt(),
        max_agility_incremental = reader.ReadInt(),
        max_dexterity_incremental = reader.ReadInt(),
        max_lucky_incremental = reader.ReadInt(),
        start_at = reader.ReadDateTimeOrNull(),
        end_at = reader.ReadDateTimeOrNull(),
        _default = reader.ReadInt()
      };
    }

    public GearKind kind
    {
      get
      {
        GearKind kind;
        if (!MasterData.GearKind.TryGetValue(this.kind_GearKind, out kind))
          Debug.LogError((object) ("Key not Found: MasterData.GearKind[" + (object) this.kind_GearKind + "]"));
        return kind;
      }
    }
  }
}
