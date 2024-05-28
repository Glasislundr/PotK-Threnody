// Decompiled with JetBrains decompiler
// Type: MasterDataTable.AwakeSkillCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class AwakeSkillCategory
  {
    public int ID;
    public string name;
    public string description;
    public DateTime? start_at;

    public static string GetEquipableText(int battleSkillId)
    {
      AwakeSkillCategory awakeSkillCategory = ((IEnumerable<AwakeSkillCategory>) MasterData.AwakeSkillCategoryList).FirstOrDefault<AwakeSkillCategory>((Func<AwakeSkillCategory, bool>) (x => x.ID == battleSkillId));
      return awakeSkillCategory == null || awakeSkillCategory.ID == 1 ? string.Empty : string.Format("{0}:{1}", (object) awakeSkillCategory.name, (object) awakeSkillCategory.description);
    }

    public static AwakeSkillCategory Parse(MasterDataReader reader)
    {
      return new AwakeSkillCategory()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        description = reader.ReadString(true),
        start_at = reader.ReadDateTimeOrNull()
      };
    }

    public enum Type
    {
      Normal = 1,
      Dress = 2,
      Trust = 3,
      SecondIllusion = 4,
      SecondDevil = 5,
      SecondAngel = 6,
      SecondBeast = 7,
      SecondFairy = 8,
      SecondCommand = 9,
      ThirdIntegral = 10, // 0x0000000A
      School = 11, // 0x0000000B
      ThirdImitate = 12, // 0x0000000C
      FourthRagnarok = 13, // 0x0000000D
    }
  }
}
