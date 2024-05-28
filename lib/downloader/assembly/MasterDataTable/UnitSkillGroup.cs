// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitSkillGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitSkillGroup
  {
    public int ID;
    public int unit_UnitUnit;
    public int skill_groupID;

    public static UnitSkillGroup Parse(MasterDataReader reader)
    {
      return new UnitSkillGroup()
      {
        ID = reader.ReadInt(),
        unit_UnitUnit = reader.ReadInt(),
        skill_groupID = reader.ReadInt()
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
  }
}
