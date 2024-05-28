// Decompiled with JetBrains decompiler
// Type: MasterDataTable.SeasonTicketSeasonTicket
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using UnityEngine;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class SeasonTicketSeasonTicket
  {
    public int ID;
    public string name;
    public int max_quantity;
    public string description;
    public int increase_match;

    public static SeasonTicketSeasonTicket Parse(MasterDataReader reader)
    {
      return new SeasonTicketSeasonTicket()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        max_quantity = reader.ReadInt(),
        description = reader.ReadString(true),
        increase_match = reader.ReadInt()
      };
    }

    public Future<Sprite> LoadThumneilF()
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>("SeasonTicket/thum");
    }

    public Future<Sprite> LoadLargeF()
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>("SeasonTicket/large");
    }
  }
}
