// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitBattleSkillOrigin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace MasterDataTable
{
  public class UnitBattleSkillOrigin
  {
    public object origin_ { get; private set; }

    public BattleskillSkill skill_ { get; private set; }

    public UnitBattleSkillOrigin(object origin, BattleskillSkill skill)
    {
      this.origin_ = origin;
      this.skill_ = skill;
    }

    public bool IsOriginBasic => this.origin_.GetType() == typeof (UnitSkill);

    public UnitSkill Basic => this.origin_ as UnitSkill;

    public bool IsOriginLeaderBasic => this.origin_.GetType() == typeof (UnitLeaderSkill);

    public UnitLeaderSkill LeaderBasic => this.origin_ as UnitLeaderSkill;

    public bool IsOriginCharacterQuest
    {
      get => this.origin_.GetType() == typeof (UnitSkillCharacterQuest);
    }

    public UnitSkillCharacterQuest CharacterQuest => this.origin_ as UnitSkillCharacterQuest;

    public bool IsOriginHarmonyQuest => this.origin_.GetType() == typeof (UnitSkillHarmonyQuest);

    public UnitSkillHarmonyQuest HarmonyQuest => this.origin_ as UnitSkillHarmonyQuest;

    public bool IsOriginEvolution => this.origin_.GetType() == typeof (UnitSkillEvolution);

    public UnitSkillEvolution Evolution => this.origin_ as UnitSkillEvolution;

    public bool IsOriginAwake => this.origin_.GetType() == typeof (UnitSkillAwake);

    public UnitSkillAwake Awake => this.origin_ as UnitSkillAwake;

    public bool IsOriginSEA => this.origin_.GetType() == typeof (UnitSEASkill);

    public UnitSEASkill SEA => this.origin_ as UnitSEASkill;
  }
}
