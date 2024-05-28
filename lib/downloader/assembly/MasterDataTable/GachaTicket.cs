// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GachaTicket
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
  public class GachaTicket
  {
    public int ID;
    public string name;
    public string short_name;
    public int? ticket_image_id;
    public int priority;
    private static readonly string GACHATICKET_BASE_PATH = "GachaTicket/{0}/ticket";
    private static readonly string DEFAULT_GACHATICKET_PATH = "GachaTicket/default/ticket";

    public static GachaTicket Parse(MasterDataReader reader)
    {
      return new GachaTicket()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        short_name = reader.ReadString(true),
        ticket_image_id = reader.ReadIntOrNull(),
        priority = reader.ReadInt()
      };
    }

    public Future<Sprite> LoadSpriteF()
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format(GachaTicket.GACHATICKET_BASE_PATH, (object) this.ticket_image_id.GetValueOrDefault(this.ID)));
    }

    public Future<Sprite> LoadSpriteOrDefault()
    {
      string path = string.Format(GachaTicket.GACHATICKET_BASE_PATH, (object) this.ticket_image_id.GetValueOrDefault(this.ID));
      return Singleton<ResourceManager>.GetInstance().Contains(path) ? Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format(GachaTicket.GACHATICKET_BASE_PATH, (object) this.ticket_image_id.GetValueOrDefault(this.ID))) : Singleton<ResourceManager>.GetInstance().Load<Sprite>(GachaTicket.DEFAULT_GACHATICKET_PATH);
    }
  }
}
