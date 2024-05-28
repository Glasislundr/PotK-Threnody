// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitProficiencyIncr
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitProficiencyIncr
  {
    public int ID;
    public int kind_GearKind;
    public int proficiency_UnitProficiency;
    public int physical_attack;
    public int magic_attack;
    public int physical_defense;
    public int magic_defense;
    public int hit;
    public int evasion;

    public static UnitProficiencyIncr Parse(MasterDataReader reader)
    {
      return new UnitProficiencyIncr()
      {
        ID = reader.ReadInt(),
        kind_GearKind = reader.ReadInt(),
        proficiency_UnitProficiency = reader.ReadInt(),
        physical_attack = reader.ReadInt(),
        magic_attack = reader.ReadInt(),
        physical_defense = reader.ReadInt(),
        magic_defense = reader.ReadInt(),
        hit = reader.ReadInt(),
        evasion = reader.ReadInt()
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

    public UnitProficiency proficiency
    {
      get
      {
        UnitProficiency proficiency;
        if (!MasterData.UnitProficiency.TryGetValue(this.proficiency_UnitProficiency, out proficiency))
          Debug.LogError((object) ("Key not Found: MasterData.UnitProficiency[" + (object) this.proficiency_UnitProficiency + "]"));
        return proficiency;
      }
    }

    public static UnitProficiencyIncr FromKindProficiency(GearKind kind, UnitProficiency prof)
    {
      return MasterData.UniqueUnitProficiencyIncrBy(kind, prof);
    }
  }
}
