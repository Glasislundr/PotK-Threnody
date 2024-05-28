// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitJob
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
  public class UnitJob
  {
    public int ID;
    public string name;
    public string flavor;
    public int move_type_UnitMoveType;
    public int movement;
    public int hp_initial;
    public int strength_initial;
    public int vitality_initial;
    public int intelligence_initial;
    public int mind_initial;
    public int agility_initial;
    public int dexterity_initial;
    public int lucky_initial;
    public int job_rank_UnitJobRank;
    public string job_characteristics_id;
    public string spWeaponName1;
    public string spWeaponName2;
    public int? classification_GearClassificationPattern;
    public int new_cost;
    public string variable_magic_bullet_name;
    public int? rendering_pattern_UnitRenderingPattern;
    public DateTime? start_at;
    private JobCharacteristics[] jobAbilities_;
    private int[] jobAbilityIds_;

    public static UnitJob Parse(MasterDataReader reader)
    {
      return new UnitJob()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        flavor = reader.ReadString(true),
        move_type_UnitMoveType = reader.ReadInt(),
        movement = reader.ReadInt(),
        hp_initial = reader.ReadInt(),
        strength_initial = reader.ReadInt(),
        vitality_initial = reader.ReadInt(),
        intelligence_initial = reader.ReadInt(),
        mind_initial = reader.ReadInt(),
        agility_initial = reader.ReadInt(),
        dexterity_initial = reader.ReadInt(),
        lucky_initial = reader.ReadInt(),
        job_rank_UnitJobRank = reader.ReadInt(),
        job_characteristics_id = reader.ReadStringOrNull(true),
        spWeaponName1 = reader.ReadString(true),
        spWeaponName2 = reader.ReadString(true),
        classification_GearClassificationPattern = reader.ReadIntOrNull(),
        new_cost = reader.ReadInt(),
        variable_magic_bullet_name = reader.ReadString(true),
        rendering_pattern_UnitRenderingPattern = reader.ReadIntOrNull(),
        start_at = reader.ReadDateTimeOrNull()
      };
    }

    public UnitMoveType move_type => (UnitMoveType) this.move_type_UnitMoveType;

    public UnitJobRank job_rank => (UnitJobRank) this.job_rank_UnitJobRank;

    public GearClassificationPattern classification
    {
      get
      {
        if (!this.classification_GearClassificationPattern.HasValue)
          return (GearClassificationPattern) null;
        GearClassificationPattern classification;
        if (!MasterData.GearClassificationPattern.TryGetValue(this.classification_GearClassificationPattern.Value, out classification))
          Debug.LogError((object) ("Key not Found: MasterData.GearClassificationPattern[" + (object) this.classification_GearClassificationPattern.Value + "]"));
        return classification;
      }
    }

    public UnitRenderingPattern rendering_pattern
    {
      get
      {
        if (!this.rendering_pattern_UnitRenderingPattern.HasValue)
          return (UnitRenderingPattern) null;
        UnitRenderingPattern renderingPattern;
        if (!MasterData.UnitRenderingPattern.TryGetValue(this.rendering_pattern_UnitRenderingPattern.Value, out renderingPattern))
          Debug.LogError((object) ("Key not Found: MasterData.UnitRenderingPattern[" + (object) this.rendering_pattern_UnitRenderingPattern.Value + "]"));
        return renderingPattern;
      }
    }

    public JobCharacteristics[] JobAbilities
    {
      get
      {
        return this.jobAbilities_ ?? (this.jobAbilities_ = ((IEnumerable<int>) this.JobAbilityIds).Select<int, JobCharacteristics>((Func<int, JobCharacteristics>) (i => MasterData.JobCharacteristics[i])).ToArray<JobCharacteristics>());
      }
    }

    public int[] JobAbilityIds
    {
      get
      {
        return this.jobAbilityIds_ ?? (this.jobAbilityIds_ = this.stringToIntegers(this.job_characteristics_id));
      }
    }

    private int[] stringToIntegers(string str)
    {
      if (string.IsNullOrEmpty(str))
        return new int[0];
      double result;
      return ((IEnumerable<string>) str.Split(',')).Select<string, int>((Func<string, int>) (s => !double.TryParse(s.Trim(), out result) ? 0 : (int) Math.Floor(result))).ToArray<int>();
    }

    public bool is_vertex_x
    {
      get
      {
        switch (this.job_rank)
        {
          case UnitJobRank.vertex1x:
          case UnitJobRank.vertex2x:
          case UnitJobRank.vertex3x:
            return true;
          default:
            return false;
        }
      }
    }
  }
}
