// Decompiled with JetBrains decompiler
// Type: MasterDataTable.AttackMethod
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class AttackMethod
  {
    public int ID;
    public int unit_UnitUnit;
    public int job_UnitJob;
    public int skill_BattleskillSkill;
    public int kind_GearKind;
    public string motion_key;

    public static AttackMethod Parse(MasterDataReader reader)
    {
      return new AttackMethod()
      {
        ID = reader.ReadInt(),
        unit_UnitUnit = reader.ReadInt(),
        job_UnitJob = reader.ReadInt(),
        skill_BattleskillSkill = reader.ReadInt(),
        kind_GearKind = reader.ReadInt(),
        motion_key = reader.ReadString(true)
      };
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

    public UnitJob job
    {
      get
      {
        UnitJob job;
        if (!MasterData.UnitJob.TryGetValue(this.job_UnitJob, out job))
          Debug.LogError((object) ("Key not Found: MasterData.UnitJob[" + (object) this.job_UnitJob + "]"));
        return job;
      }
    }

    public BattleskillSkill skill
    {
      get
      {
        BattleskillSkill skill;
        if (!MasterData.BattleskillSkill.TryGetValue(this.skill_BattleskillSkill, out skill))
          Debug.LogError((object) ("Key not Found: MasterData.BattleskillSkill[" + (object) this.skill_BattleskillSkill + "]"));
        return skill;
      }
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

    public IAttackMethod CreateInterface()
    {
      return (IAttackMethod) new AttackMethod.MasterAttackMethod(this);
    }

    private class MasterAttackMethod : IAttackMethod
    {
      private AttackMethod original_;

      public override object original => (object) this.original_;

      public override GearKind kind { get; protected set; }

      public override BattleskillSkill skill { get; protected set; }

      public override string motionKey => this.original_.motion_key;

      public MasterAttackMethod(AttackMethod v)
      {
        this.original_ = v;
        this.kind = v.kind;
        this.skill = v.skill;
      }
    }
  }
}
