// Decompiled with JetBrains decompiler
// Type: MasterDataTable.ReisouDrilling
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class ReisouDrilling
  {
    public int ID;
    public int rare_1;
    public int rare_2;
    public int rare_3;
    public int rare_4;
    public int rare_5;
    public int rare_6;
    public int rare_7;

    public static ReisouDrilling Parse(MasterDataReader reader)
    {
      return new ReisouDrilling()
      {
        ID = reader.ReadInt(),
        rare_1 = reader.ReadInt(),
        rare_2 = reader.ReadInt(),
        rare_3 = reader.ReadInt(),
        rare_4 = reader.ReadInt(),
        rare_5 = reader.ReadInt(),
        rare_6 = reader.ReadInt(),
        rare_7 = reader.ReadInt()
      };
    }

    public static int GetReisouDrilling(int rarity)
    {
      ReisouDrilling reisouDrilling = MasterData.ReisouDrilling.FirstOrDefault<KeyValuePair<int, ReisouDrilling>>().Value;
      switch (rarity)
      {
        case 1:
          return reisouDrilling.rare_1;
        case 2:
          return reisouDrilling.rare_2;
        case 3:
          return reisouDrilling.rare_3;
        case 4:
          return reisouDrilling.rare_4;
        case 5:
          return reisouDrilling.rare_5;
        case 6:
          return reisouDrilling.rare_6;
        default:
          return reisouDrilling.rare_7;
      }
    }
  }
}
