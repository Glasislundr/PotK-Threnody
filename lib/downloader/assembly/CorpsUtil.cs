// Decompiled with JetBrains decompiler
// Type: CorpsUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using LocaleTimeZone;
using MasterDataTable;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
public class CorpsUtil
{
  public CorpsPeriod[] activePeriods
  {
    get
    {
      HashSet<int> periods = new HashSet<int>((IEnumerable<int>) (Singleton<NGGameDataManager>.GetInstance().corps_period_ids ?? new int[0]));
      DateTime nowJst = TimeZoneInfo.ConvertTime(ServerTime.NowAppTimeAddDelta(), Japan.CreateTimeZone());
      return ((IEnumerable<CorpsPeriod>) MasterData.CorpsPeriodList).Where<CorpsPeriod>((Func<CorpsPeriod, bool>) (x => periods.Contains(x.ID) && x.start_at <= nowJst && x.end_at > nowJst)).ToArray<CorpsPeriod>();
    }
  }

  public enum UnitSelectionMode
  {
    Auto,
    Manual,
  }

  public enum SequenceType
  {
    None = -1, // 0xFFFFFFFF
    Start = 0,
    Reset = 1,
  }
}
