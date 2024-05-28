// Decompiled with JetBrains decompiler
// Type: MypageMissionClearInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class MypageMissionClearInfo : MonoBehaviour
{
  [SerializeField]
  private MypagePopupInformation popup_;
  private List<string> messages_;

  public IEnumerator initOne()
  {
    IEnumerator ep = this.popup_.init();
    while (ep.MoveNext())
      yield return ep.Current;
  }

  public void init(Player player)
  {
    if (!player.is_open_mission)
      return;
    this.messages_ = new List<string>();
    List<int> missionids = new List<int>();
    bool flag = false;
    Persist.MissionHistory.IDList daily;
    Persist.MissionHistory.IDList mission;
    try
    {
      daily = Persist.missionHistory.Data.daily;
      mission = Persist.missionHistory.Data.mission;
    }
    catch
    {
      Persist.missionHistory.Delete();
      Persist.missionHistory.Data.setDefault();
      daily = Persist.missionHistory.Data.daily;
      mission = Persist.missionHistory.Data.mission;
      flag = true;
    }
    if (player.clear_daily_mission_ids == null || player.clear_daily_mission_ids.mission_ids == null || !((IEnumerable<int?>) player.clear_daily_mission_ids.mission_ids).Any<int?>((Func<int?, bool>) (i => i.HasValue)))
      return;
    DateTime exact = DateTime.ParseExact(player.clear_daily_mission_ids.date, "yyyy-MM-dd", (IFormatProvider) null);
    if (daily.date.HasValue)
    {
      DateTime dateTime = exact;
      DateTime? date = daily.date;
      if ((date.HasValue ? (dateTime != date.GetValueOrDefault() ? 1 : 0) : 1) == 0)
        goto label_10;
    }
    daily.date = new DateTime?(exact);
    daily.ids.Clear();
    flag = true;
label_10:
    if (player.clear_daily_mission_ids.mission_ids.Length != 0)
    {
      foreach (int? missionId in player.clear_daily_mission_ids.mission_ids)
      {
        if (missionId.HasValue)
          missionids.Add(missionId.Value);
      }
    }
    List<int> collection1 = new List<int>();
    List<int> collection2 = new List<int>();
    List<DailyMission> source = new List<DailyMission>();
    foreach (int key in missionids)
    {
      DailyMission dailyMission;
      if (MasterData.DailyMission.TryGetValue(key, out dailyMission))
      {
        if (dailyMission.mission_type == MissionType.daily)
        {
          if (!daily.ids.Contains(key))
            collection2.Add(key);
          else
            continue;
        }
        else if (!mission.ids.Contains(key))
          collection1.Add(key);
        else
          continue;
        source.Add(dailyMission);
      }
    }
    if (source.Count > 0)
    {
      if (source.Count > 1)
        source = source.OrderBy<DailyMission, int>((Func<DailyMission, int>) (m => m.priority)).ToList<DailyMission>();
      this.messages_.AddRange((IEnumerable<string>) source.Select<DailyMission, string>((Func<DailyMission, string>) (m => m.name)).ToList<string>());
    }
    if (collection2.Count > 0)
    {
      daily.ids.AddRange((IEnumerable<int>) collection2);
      flag = true;
    }
    List<int> list = mission.ids.Where<int>((Func<int, bool>) (i => !missionids.Contains(i))).ToList<int>();
    if (list.Count > 0)
    {
      foreach (int num in list)
        mission.ids.Remove(num);
      flag = true;
    }
    if (collection1.Count > 0)
    {
      if (!mission.date.HasValue)
        mission.date = new DateTime?(ServerTime.NowAppTime());
      mission.ids.AddRange((IEnumerable<int>) collection1);
      flag = true;
    }
    if (!flag)
      return;
    Persist.missionHistory.Flush();
  }

  public void startDisplay() => this.popup_.resume();

  public void clearAll()
  {
    this.popup_.clearAll();
    if (this.messages_ == null || this.messages_.Count <= 0)
      return;
    this.messages_.Clear();
  }

  private void Update()
  {
    if (this.messages_ == null || this.messages_.Count <= 0)
      return;
    foreach (string message in this.messages_)
      this.popup_.setMessage(message);
    this.messages_.Clear();
  }
}
