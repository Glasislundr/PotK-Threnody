// Decompiled with JetBrains decompiler
// Type: GameCore.Stat.GameServerStat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace GameCore.Stat
{
  public class GameServerStat
  {
    public DateTime startup_at;
    public bool denied_create_room;
    public string masterdata_version;
    public string client_version;
    public string process_number;
    public int active_room_count;
    public int matchingserver_port;
    public int nonstart_room_count;
    public int prepare_room_count;
    public int p2p_room_count;
    public int p2c_room_count;
    public int c2c_room_count;
    public int finishing_room_count;
    public int total_created_room_count;
    public int total_timeout_room_count;
    public int total_finished_room_count;

    public object ToJson()
    {
      return (object) new Dictionary<string, object>()
      {
        {
          "startup_at",
          (object) this.startup_at.ToStringISO8601()
        },
        {
          "denied_create_room",
          (object) this.denied_create_room
        },
        {
          "masterdata_version",
          (object) this.masterdata_version
        },
        {
          "client_version",
          (object) this.client_version
        },
        {
          "process_number",
          (object) this.process_number
        },
        {
          "active_room_count",
          (object) this.active_room_count
        },
        {
          "matchingserver_port",
          (object) this.matchingserver_port
        },
        {
          "nonstart_room_count",
          (object) this.nonstart_room_count
        },
        {
          "prepare_room_count",
          (object) this.prepare_room_count
        },
        {
          "p2p_room_count",
          (object) this.p2p_room_count
        },
        {
          "p2c_room_count",
          (object) this.p2c_room_count
        },
        {
          "c2c_room_count",
          (object) this.c2c_room_count
        },
        {
          "finishing_room_count",
          (object) this.finishing_room_count
        },
        {
          "total_created_room_count",
          (object) this.total_created_room_count
        },
        {
          "total_timeout_room_count",
          (object) this.total_timeout_room_count
        },
        {
          "total_finished_room_count",
          (object) this.total_finished_room_count
        }
      };
    }

    public void FromJson(object obj)
    {
      IDictionary<string, object> dictionary = (IDictionary<string, object>) obj;
      this.startup_at = DateTime.Parse((string) dictionary["startup_at"]);
      this.denied_create_room = dictionary.ContainsKey("denied_create_room") && (bool) dictionary["denied_create_room"];
      this.masterdata_version = (string) dictionary["masterdata_version"];
      this.client_version = (string) dictionary["client_version"];
      this.process_number = (string) dictionary["process_number"];
      this.active_room_count = (int) (long) dictionary["active_room_count"];
      this.matchingserver_port = (int) (long) dictionary["matchingserver_port"];
      this.nonstart_room_count = (int) (long) dictionary["nonstart_room_count"];
      this.prepare_room_count = (int) (long) dictionary["prepare_room_count"];
      this.p2p_room_count = (int) (long) dictionary["p2p_room_count"];
      this.p2c_room_count = (int) (long) dictionary["p2c_room_count"];
      this.c2c_room_count = (int) (long) dictionary["c2c_room_count"];
      this.finishing_room_count = (int) (long) dictionary["finishing_room_count"];
      this.total_created_room_count = (int) (long) dictionary["total_created_room_count"];
      this.total_timeout_room_count = (int) (long) dictionary["total_timeout_room_count"];
      this.total_finished_room_count = (int) (long) dictionary["total_finished_room_count"];
    }
  }
}
