// Decompiled with JetBrains decompiler
// Type: MasterDataTable.CommonTicket
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
  public class CommonTicket
  {
    public int ID;
    public string name;
    public string description;
    public int type_CommonTicketType;
    public int icon_id;

    public static CommonTicket Parse(MasterDataReader reader)
    {
      return new CommonTicket()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        description = reader.ReadString(true),
        type_CommonTicketType = reader.ReadInt(),
        icon_id = reader.ReadInt()
      };
    }

    public CommonTicketType type => (CommonTicketType) this.type_CommonTicketType;

    public Future<Sprite> LoadIconMSpriteF()
    {
      int num = this.icon_id != 0 ? this.icon_id : this.ID;
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>("Coin/{0}/Coin_M".F((object) num));
    }

    public Future<Sprite> LoadIconSSpriteF()
    {
      int num = this.icon_id != 0 ? this.icon_id : this.ID;
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>("Coin/{0}/Coin_S".F((object) num));
    }
  }
}
