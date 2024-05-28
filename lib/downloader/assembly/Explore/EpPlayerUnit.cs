// Decompiled with JetBrains decompiler
// Type: Explore.EpPlayerUnit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;

#nullable disable
namespace Explore
{
  public class EpPlayerUnit
  {
    private readonly float[] RARITY_COEFFICIENTS = new float[6]
    {
      2.3f,
      2f,
      1.9f,
      1.4f,
      1.2f,
      1f
    };

    public BL.Unit BlUnit { get; private set; }

    public EpPlayerUnit(BL.Unit blUnit, int index)
    {
      this.BlUnit = blUnit;
      this.BlUnit.index = index;
      UnitUnitParameter unitUnitParameter;
      MasterData.UnitUnitParameter.TryGetValue(this.BlUnit.unit.ID, out unitUnitParameter);
      MasterDataTable.UnitJob job = this.BlUnit.unit.job;
      int index1 = Math.Min(this.BlUnit.unit.rarity.index, 5);
      int num1 = job.strength_initial + (unitUnitParameter != null ? unitUnitParameter.strength_max : 0);
      int num2 = job.intelligence_initial + (unitUnitParameter != null ? unitUnitParameter.intelligence_max : 0);
      this.Hp = (int) ((double) (job.hp_initial + (unitUnitParameter != null ? unitUnitParameter.hp_max : 0)) * (double) this.RARITY_COEFFICIENTS[index1]);
      this.Def = (int) ((double) (job.vitality_initial + (unitUnitParameter != null ? unitUnitParameter.vitality_max : 0)) * (double) this.RARITY_COEFFICIENTS[index1]);
      this.Atk = (int) ((num1 > num2 ? (double) num1 : (double) num2) * (double) this.RARITY_COEFFICIENTS[index1]);
      this.Element = this.BlUnit.duelSkills.Length != 0 ? this.BlUnit.duelSkills[0].skill.element : CommonElement.none;
      this.PrincessType = this.BlUnit.playerUnit.unit_type.Enum;
      this.GearKind = this.BlUnit.weapon.gear.kind.Enum;
    }

    public int Hp { get; private set; }

    public int Atk { get; private set; }

    public int Def { get; private set; }

    public CommonElement Element { get; private set; }

    public UnitTypeEnum PrincessType { get; private set; }

    public GearKindEnum GearKind { get; private set; }
  }
}
