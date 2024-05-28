// Decompiled with JetBrains decompiler
// Type: MasterDataTable.InitimateLevel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class InitimateLevel
  {
    public int ID;
    public int level;
    public int from_exp;
    public int to_exp;

    public static InitimateLevel Parse(MasterDataReader reader)
    {
      return new InitimateLevel()
      {
        ID = reader.ReadInt(),
        level = reader.ReadInt(),
        from_exp = reader.ReadInt(),
        to_exp = reader.ReadInt()
      };
    }
  }
}
