// Decompiled with JetBrains decompiler
// Type: MasterDataTable.SeaHomeTimeZone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class SeaHomeTimeZone
  {
    public int ID;
    public string name;
    public string start_time;
    public string end_time;
    public string image_pattern;

    public static SeaHomeTimeZone Parse(MasterDataReader reader)
    {
      return new SeaHomeTimeZone()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        start_time = reader.ReadString(true),
        end_time = reader.ReadString(true),
        image_pattern = reader.ReadString(true)
      };
    }

    public bool WithIn(DateTime now)
    {
      Tuple<DateTime, DateTime> timeZone = this.GetTimeZone(now);
      return timeZone.Item1 <= now && timeZone.Item2 >= now;
    }

    public Tuple<DateTime, DateTime> GetTimeZone(DateTime dateTime)
    {
      DateTime dateTime1 = DateTime.Parse(string.Format("{0} {1}", (object) dateTime.Date.ToString("d"), (object) this.start_time));
      DateTime dateTime2 = DateTime.Parse(string.Format("{0} {1}", (object) dateTime.Date.ToString("d"), (object) this.end_time));
      if (dateTime1 > dateTime2)
      {
        if (dateTime.Hour >= 12)
          dateTime2 = dateTime2.AddDays(1.0);
        else
          dateTime1 = dateTime1.AddDays(-1.0);
      }
      return new Tuple<DateTime, DateTime>(dateTime1, dateTime2);
    }
  }
}
