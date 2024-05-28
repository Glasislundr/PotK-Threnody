// Decompiled with JetBrains decompiler
// Type: MasterDataTable.PvpVictoryEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class PvpVictoryEffect
  {
    public int ID;
    public int victory_type_PvpVictoryTypeEnum;

    public static PvpVictoryEffect Parse(MasterDataReader reader)
    {
      return new PvpVictoryEffect()
      {
        ID = reader.ReadInt(),
        victory_type_PvpVictoryTypeEnum = reader.ReadInt()
      };
    }

    public PvpVictoryTypeEnum victory_type
    {
      get => (PvpVictoryTypeEnum) this.victory_type_PvpVictoryTypeEnum;
    }
  }
}
