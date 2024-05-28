// Decompiled with JetBrains decompiler
// Type: MasterDataTable.SelectTicketSelectSample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class SelectTicketSelectSample
  {
    public int ID;
    public int ticketID;
    public int? deckID;
    public int entity_type_CommonRewardType;
    public int reward_id;
    public int reward_value;
    public string reward_title;
    public string reward_info;

    public static SelectTicketSelectSample Parse(MasterDataReader reader)
    {
      return new SelectTicketSelectSample()
      {
        ID = reader.ReadInt(),
        ticketID = reader.ReadInt(),
        deckID = reader.ReadIntOrNull(),
        entity_type_CommonRewardType = reader.ReadInt(),
        reward_id = reader.ReadInt(),
        reward_value = reader.ReadInt(),
        reward_title = reader.ReadString(true),
        reward_info = reader.ReadString(true)
      };
    }

    public CommonRewardType entity_type => (CommonRewardType) this.entity_type_CommonRewardType;
  }
}
