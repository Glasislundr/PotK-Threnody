// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitAwakeningEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitAwakeningEffect
  {
    public int ID;
    public int x;
    public int y;
    public int width;
    public int height;
    public float god_color_r;
    public float god_color_g;
    public float god_color_b;
    public float god_color_weight;

    public static UnitAwakeningEffect Parse(MasterDataReader reader)
    {
      return new UnitAwakeningEffect()
      {
        ID = reader.ReadInt(),
        x = reader.ReadInt(),
        y = reader.ReadInt(),
        width = reader.ReadInt(),
        height = reader.ReadInt(),
        god_color_r = reader.ReadFloat(),
        god_color_g = reader.ReadFloat(),
        god_color_b = reader.ReadFloat(),
        god_color_weight = reader.ReadFloat()
      };
    }
  }
}
