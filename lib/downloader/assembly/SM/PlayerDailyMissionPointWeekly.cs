// Decompiled with JetBrains decompiler
// Type: SM.PlayerDailyMissionPointWeekly
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerDailyMissionPointWeekly : KeyCompare
  {
    public int?[] received_rewards;
    public int? point;

    public PlayerDailyMissionPointWeekly()
    {
    }

    public PlayerDailyMissionPointWeekly(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.received_rewards = ((IEnumerable<object>) json[nameof (received_rewards)]).Select<object, int?>((Func<object, int?>) (s =>
      {
        long? nullable = (long?) s;
        return !nullable.HasValue ? new int?() : new int?((int) nullable.GetValueOrDefault());
      })).ToArray<int?>();
      int? nullable1;
      if (json[nameof (point)] != null)
      {
        long? nullable2 = (long?) json[nameof (point)];
        nullable1 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
      }
      else
        nullable1 = new int?();
      this.point = nullable1;
    }
  }
}
