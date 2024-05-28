// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GuildMission
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GuildMission
  {
    public int ID;
    public int priority;
    public int num;
    public int achievement_count;
    public int condition;
    public string name;
    public string detail;
    public string scene;
    public DateTime? start_at;
    public DateTime? end_at;
    public DateTime? published_end_at;

    public static GuildMission Parse(MasterDataReader reader)
    {
      return new GuildMission()
      {
        ID = reader.ReadInt(),
        priority = reader.ReadInt(),
        num = reader.ReadInt(),
        achievement_count = reader.ReadInt(),
        condition = reader.ReadInt(),
        name = reader.ReadString(true),
        detail = reader.ReadString(true),
        scene = reader.ReadString(true),
        start_at = reader.ReadDateTimeOrNull(),
        end_at = reader.ReadDateTimeOrNull(),
        published_end_at = reader.ReadDateTimeOrNull()
      };
    }
  }
}
