// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitGroupSmallCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitGroupSmallCategory
  {
    public int ID;
    public string name;
    public string short_label_name;
    public string description;
    public DateTime? start_at;

    public static UnitGroupSmallCategory Parse(MasterDataReader reader)
    {
      return new UnitGroupSmallCategory()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        short_label_name = reader.ReadString(true),
        description = reader.ReadString(true),
        start_at = reader.ReadDateTimeOrNull()
      };
    }

    public string GetSpriteName() => string.Format("s_classification_{0}", (object) this.ID);

    public enum Type
    {
      All = 1,
      Collabo = 2,
      Ragnarok = 3,
      Seiyugu = 4,
      Shirogaku = 5,
      Teacher = 6,
      SeaPool = 7,
      SeaBeach = 8,
      SeaJungle = 9,
      SecondAngel = 10, // 0x0000000A
      SecondDevil = 11, // 0x0000000B
      SecondBeast = 12, // 0x0000000C
      SecondFairy = 13, // 0x0000000D
      AlcheCollabo = 14, // 0x0000000E
      ShinobiCollabo = 15, // 0x0000000F
      SecondCommand = 16, // 0x00000010
      ThirdIntegral = 17, // 0x00000011
      ThirdImitate = 18, // 0x00000012
      Yggdrasil = 19, // 0x00000013
      FourthRagnarok = 20, // 0x00000014
    }
  }
}
