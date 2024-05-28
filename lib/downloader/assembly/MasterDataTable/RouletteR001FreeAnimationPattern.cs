// Decompiled with JetBrains decompiler
// Type: MasterDataTable.RouletteR001FreeAnimationPattern
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class RouletteR001FreeAnimationPattern
  {
    public int ID;
    public int action_pattern_id;
    public int weight;
    public int cutin;

    public static RouletteR001FreeAnimationPattern Parse(MasterDataReader reader)
    {
      return new RouletteR001FreeAnimationPattern()
      {
        ID = reader.ReadInt(),
        action_pattern_id = reader.ReadInt(),
        weight = reader.ReadInt(),
        cutin = reader.ReadInt()
      };
    }
  }
}
