// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitHomeVoicePattern
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitHomeVoicePattern
  {
    public int ID;
    public int cue_id;
    public DateTime? start_at;
    public DateTime? end_at;

    public static UnitHomeVoicePattern Parse(MasterDataReader reader)
    {
      return new UnitHomeVoicePattern()
      {
        ID = reader.ReadInt(),
        cue_id = reader.ReadInt(),
        start_at = reader.ReadDateTimeOrNull(),
        end_at = reader.ReadDateTimeOrNull()
      };
    }

    public static List<int> GetEnableIDList()
    {
      DateTime now = ServerTime.NowAppTime();
      return ((IEnumerable<UnitHomeVoicePattern>) MasterData.UnitHomeVoicePatternList).Where<UnitHomeVoicePattern>((Func<UnitHomeVoicePattern, bool>) (x =>
      {
        if (x.start_at.HasValue && !(x.start_at.Value <= now))
          return false;
        return !x.end_at.HasValue || x.end_at.Value >= now;
      })).Select<UnitHomeVoicePattern, int>((Func<UnitHomeVoicePattern, int>) (x => x.cue_id)).ToList<int>();
    }
  }
}
