// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitSkillupSkillGroupSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitSkillupSkillGroupSetting
  {
    public int ID;
    public int group_id;
    public int skill_id;

    public static UnitSkillupSkillGroupSetting Parse(MasterDataReader reader)
    {
      return new UnitSkillupSkillGroupSetting()
      {
        ID = reader.ReadInt(),
        group_id = reader.ReadInt(),
        skill_id = reader.ReadInt()
      };
    }
  }
}
