// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleStageGuestAttackMethod
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BattleStageGuestAttackMethod
  {
    public int ID;
    public int stage_guest_unit_BattleStageGuest;
    public int skill_BattleskillSkill;
    public int kind_GearKind;
    public string motion_key;

    public static BattleStageGuestAttackMethod Parse(MasterDataReader reader)
    {
      return new BattleStageGuestAttackMethod()
      {
        ID = reader.ReadInt(),
        stage_guest_unit_BattleStageGuest = reader.ReadInt(),
        skill_BattleskillSkill = reader.ReadInt(),
        kind_GearKind = reader.ReadInt(),
        motion_key = reader.ReadString(true)
      };
    }

    public BattleStageGuest stage_guest_unit
    {
      get
      {
        BattleStageGuest stageGuestUnit;
        if (!MasterData.BattleStageGuest.TryGetValue(this.stage_guest_unit_BattleStageGuest, out stageGuestUnit))
          Debug.LogError((object) ("Key not Found: MasterData.BattleStageGuest[" + (object) this.stage_guest_unit_BattleStageGuest + "]"));
        return stageGuestUnit;
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
      return (IAttackMethod) new BattleStageGuestAttackMethod.GuestAttackMethod(this);
    }

    private class GuestAttackMethod : IAttackMethod
    {
      private BattleStageGuestAttackMethod original_;

      public override object original => (object) this.original_;

      public override GearKind kind { get; protected set; }

      public override BattleskillSkill skill { get; protected set; }

      public override string motionKey => this.original_.motion_key;

      public GuestAttackMethod(BattleStageGuestAttackMethod v)
      {
        this.original_ = v;
        this.kind = v.kind;
        this.skill = v.skill;
      }
    }
  }
}
