// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitProficiency
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitProficiency
  {
    public int ID;
    public string proficiency;
    public int gear_rarity;

    public static UnitProficiency Parse(MasterDataReader reader)
    {
      return new UnitProficiency()
      {
        ID = reader.ReadInt(),
        proficiency = reader.ReadString(true),
        gear_rarity = reader.ReadInt()
      };
    }
  }
}
