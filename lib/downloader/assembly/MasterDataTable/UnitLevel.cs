// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitLevel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitLevel
  {
    public int ID;
    public int pattern_id;
    public int level;
    public int from_exp;
    public int to_exp;

    public static UnitLevel Parse(MasterDataReader reader)
    {
      return new UnitLevel()
      {
        ID = reader.ReadInt(),
        pattern_id = reader.ReadInt(),
        level = reader.ReadInt(),
        from_exp = reader.ReadInt(),
        to_exp = reader.ReadInt()
      };
    }
  }
}
