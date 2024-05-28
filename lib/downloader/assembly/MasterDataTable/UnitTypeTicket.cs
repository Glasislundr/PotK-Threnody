// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitTypeTicket
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitTypeTicket
  {
    public int ID;
    public string name;
    public string short_name;
    public int cost;
    public int max_ticket;
    public bool unit_type_random_flag;
    public int ticketTypeID_UnitTypeTicketType;
    public string ticketTypeParam;
    public int icon_id;
    public int priority;
    public DateTime? end_at;
    public string description;

    public static UnitTypeTicket Parse(MasterDataReader reader)
    {
      return new UnitTypeTicket()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        short_name = reader.ReadString(true),
        cost = reader.ReadInt(),
        max_ticket = reader.ReadInt(),
        unit_type_random_flag = reader.ReadBool(),
        ticketTypeID_UnitTypeTicketType = reader.ReadInt(),
        ticketTypeParam = reader.ReadString(true),
        icon_id = reader.ReadInt(),
        priority = reader.ReadInt(),
        end_at = reader.ReadDateTimeOrNull(),
        description = reader.ReadString(true)
      };
    }

    public UnitTypeTicketType ticketTypeID
    {
      get => (UnitTypeTicketType) this.ticketTypeID_UnitTypeTicketType;
    }
  }
}
