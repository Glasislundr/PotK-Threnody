// Decompiled with JetBrains decompiler
// Type: MasterDataTable.RouletteR001FreeRoulette
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class RouletteR001FreeRoulette
  {
    public int ID;
    public string name;
    public string description;
    public int _logic_id;
    public int _deck_id;
    public int _roll_count;
    public int? _period_id;
    public bool _is_campaign;
    public string url;

    public static RouletteR001FreeRoulette Parse(MasterDataReader reader)
    {
      return new RouletteR001FreeRoulette()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        description = reader.ReadString(true),
        _logic_id = reader.ReadInt(),
        _deck_id = reader.ReadInt(),
        _roll_count = reader.ReadInt(),
        _period_id = reader.ReadIntOrNull(),
        _is_campaign = reader.ReadBool(),
        url = reader.ReadStringOrNull(true)
      };
    }
  }
}
