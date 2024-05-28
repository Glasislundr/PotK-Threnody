// Decompiled with JetBrains decompiler
// Type: MasterDataTable.SeaDateDateSpotDisplaySetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class SeaDateDateSpotDisplaySetting
  {
    public int ID;
    public int datespot_SeaDateDateSpot;
    public DateTime? start_at;
    public DateTime? end_at;
    public int time_zone_SeaHomeTimeZone;
    public string date_name;
    public string spot_background_image;
    public string spot_wayback_background_image;

    public static SeaDateDateSpotDisplaySetting Parse(MasterDataReader reader)
    {
      return new SeaDateDateSpotDisplaySetting()
      {
        ID = reader.ReadInt(),
        datespot_SeaDateDateSpot = reader.ReadInt(),
        start_at = reader.ReadDateTimeOrNull(),
        end_at = reader.ReadDateTimeOrNull(),
        time_zone_SeaHomeTimeZone = reader.ReadInt(),
        date_name = reader.ReadString(true),
        spot_background_image = reader.ReadString(true),
        spot_wayback_background_image = reader.ReadString(true)
      };
    }

    public SeaDateDateSpot datespot
    {
      get
      {
        SeaDateDateSpot datespot;
        if (!MasterData.SeaDateDateSpot.TryGetValue(this.datespot_SeaDateDateSpot, out datespot))
          Debug.LogError((object) ("Key not Found: MasterData.SeaDateDateSpot[" + (object) this.datespot_SeaDateDateSpot + "]"));
        return datespot;
      }
    }

    public SeaHomeTimeZone time_zone
    {
      get
      {
        SeaHomeTimeZone timeZone;
        if (!MasterData.SeaHomeTimeZone.TryGetValue(this.time_zone_SeaHomeTimeZone, out timeZone))
          Debug.LogError((object) ("Key not Found: MasterData.SeaHomeTimeZone[" + (object) this.time_zone_SeaHomeTimeZone + "]"));
        return timeZone;
      }
    }

    public Hashtable GetImageHash()
    {
      return new Hashtable()
      {
        {
          (object) "background",
          string.IsNullOrEmpty(this.spot_background_image) ? (object) "\"black\"" : (object) ("\"" + this.spot_background_image + "\"")
        },
        {
          (object) "wayback_background",
          string.IsNullOrEmpty(this.spot_wayback_background_image) ? (object) "\"black\"" : (object) ("\"" + this.spot_wayback_background_image + "\"")
        }
      };
    }
  }
}
