// Decompiled with JetBrains decompiler
// Type: MasterDataTable.CorpsStageOpenConditions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class CorpsStageOpenConditions
  {
    public int ID;
    public string conditions_numbers;
    public string logic;

    public static CorpsStageOpenConditions Parse(MasterDataReader reader)
    {
      return new CorpsStageOpenConditions()
      {
        ID = reader.ReadInt(),
        conditions_numbers = reader.ReadStringOrNull(true),
        logic = reader.ReadStringOrNull(true)
      };
    }
  }
}
