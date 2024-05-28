// Decompiled with JetBrains decompiler
// Type: CustomDeck.Util
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace CustomDeck
{
  public static class Util
  {
    public const int LEVEL_UNLOCK = 250;

    public static Util.RestoreUnit checkCreateRestoreUnit(PlayerUnit target)
    {
      return target != (PlayerUnit) null && (((IEnumerable<int?>) target.equip_gear_ids).Any<int?>((Func<int?, bool>) (x => x.HasValue)) || ((IEnumerable<int>) target.over_killers_player_unit_ids).Any<int>((Func<int, bool>) (x => x > 0)) || ((IEnumerable<int?>) target.equip_awake_skill_ids).Any<int?>((Func<int?, bool>) (x => x.HasValue))) ? new Util.RestoreUnit(target) : (Util.RestoreUnit) null;
    }

    public static Util.RestoreGear checkCreateRestoreGear(PlayerItem target)
    {
      return target != (PlayerItem) null && target.gear != null && target.equipped_reisou_player_gear_id != 0 ? new Util.RestoreGear(target) : (Util.RestoreGear) null;
    }

    public static bool checkUnlockedPlayerLevel(int level)
    {
      int num = level >= 250 ? 1 : 0;
      Persist<Persist.CustomDeckTutorial> customDeckTutorial = Persist.customDeckTutorial;
      if (num != 0)
        return num != 0;
      if (!customDeckTutorial.Data.isUnlocked)
        return num != 0;
      customDeckTutorial.Data.isUnlocked = false;
      customDeckTutorial.Flush();
      return num != 0;
    }

    public static void resetPlayerUnits(Dictionary<int, Util.RestoreUnit> restoreData)
    {
      PlayerUnit[] data = SMManager.Get<PlayerUnit[]>();
      int num = 0;
      for (int index = 0; index < data.Length; ++index)
      {
        if (!PlayerCustomDeck.isCustom(data[index]))
        {
          Util.RestoreUnit restoreUnit;
          if ((restoreUnit = Util.checkCreateRestoreUnit(data[index])) != null)
            restoreData[data[index].id] = restoreUnit;
          data[index] = PlayerCustomDeck.cloneByCustom(data[index]);
          ++num;
        }
      }
      if (num <= 0)
        return;
      SMManager.UpdateList<PlayerUnit>(data, true);
    }

    public static void restorePlayerUnits(Dictionary<int, Util.RestoreUnit> restoreData)
    {
      PlayerUnit[] data = SMManager.Get<PlayerUnit[]>();
      int num = 0;
      for (int index = 0; index < data.Length; ++index)
      {
        if (PlayerCustomDeck.isCustom(data[index]))
        {
          Util.RestoreUnit restoreUnit;
          restoreData.TryGetValue(data[index].id, out restoreUnit);
          PlayerUnit playerUnit = new PlayerUnit();
          playerUnit.restoreByCustomDeck(data[index], restoreUnit);
          data[index] = playerUnit;
          ++num;
        }
      }
      if (num <= 0)
        return;
      SMManager.UpdateList<PlayerUnit>(data, true);
    }

    public static void resetPlayerGears(Dictionary<int, Util.RestoreGear> restoreData)
    {
      PlayerItem[] data = SMManager.Get<PlayerItem[]>();
      int num = 0;
      for (int index = 0; index < data.Length; ++index)
      {
        if (!PlayerCustomDeck.isCustom(data[index]) && data[index].gear != null)
        {
          Util.RestoreGear restoreGear;
          if ((restoreGear = Util.checkCreateRestoreGear(data[index])) != null)
            restoreData[data[index].id] = restoreGear;
          data[index] = PlayerCustomDeck.cloneByCustom(data[index]);
          ++num;
        }
      }
      if (num <= 0)
        return;
      SMManager.UpdateList<PlayerItem>(data, true);
    }

    public static void restorePlayerGears(Dictionary<int, Util.RestoreGear> restoreData)
    {
      PlayerItem[] data = SMManager.Get<PlayerItem[]>();
      int num = 0;
      for (int index = 0; index < data.Length; ++index)
      {
        if (PlayerCustomDeck.isCustom(data[index]))
        {
          Util.RestoreGear restoreGear;
          restoreData.TryGetValue(data[index].id, out restoreGear);
          PlayerItem playerItem = new PlayerItem();
          playerItem.restoreByCustomDeck(data[index], restoreGear);
          data[index] = playerItem;
          ++num;
        }
      }
      if (num <= 0)
        return;
      SMManager.UpdateList<PlayerItem>(data, true);
    }

    public static JobRank getJobRank(PlayerUnit playerUnit, int jobId)
    {
      JobRank jobRank = JobRank.Normal;
      JobChangePatterns jobChangePatterns = JobChangeUtil.getJobChangePatterns(playerUnit);
      if (jobChangePatterns != null)
      {
        if (jobId == jobChangePatterns.job2_UnitJob)
        {
          jobRank = JobRank.Vertex1;
        }
        else
        {
          int num1 = jobId;
          int? nullable = jobChangePatterns.job3_UnitJob;
          int valueOrDefault1 = nullable.GetValueOrDefault();
          if (num1 == valueOrDefault1 & nullable.HasValue)
          {
            jobRank = JobRank.Vertex2;
          }
          else
          {
            int num2 = jobId;
            nullable = jobChangePatterns.job4_UnitJob;
            int valueOrDefault2 = nullable.GetValueOrDefault();
            if (num2 == valueOrDefault2 & nullable.HasValue)
              jobRank = JobRank.Vertex3;
          }
        }
      }
      return jobRank;
    }

    public class RestoreUnit
    {
      public int?[] equip_gear_ids { get; private set; }

      public int[] over_killers_player_unit_ids { get; private set; }

      public int?[] equip_awake_skill_ids { get; private set; }

      public RestoreUnit(PlayerUnit target)
      {
        int?[] equipGearIds = target.equip_gear_ids;
        this.equip_gear_ids = equipGearIds != null ? ((IEnumerable<int?>) equipGearIds).ToArray<int?>() : (int?[]) null;
        int[] killersPlayerUnitIds = target.over_killers_player_unit_ids;
        this.over_killers_player_unit_ids = killersPlayerUnitIds != null ? ((IEnumerable<int>) killersPlayerUnitIds).ToArray<int>() : (int[]) null;
        int?[] equipAwakeSkillIds = target.equip_awake_skill_ids;
        this.equip_awake_skill_ids = equipAwakeSkillIds != null ? ((IEnumerable<int?>) equipAwakeSkillIds).ToArray<int?>() : (int?[]) null;
      }
    }

    public class RestoreGear
    {
      public int equipped_reisou_player_gear_id { get; private set; }

      public RestoreGear(PlayerItem target)
      {
        this.equipped_reisou_player_gear_id = target.equipped_reisou_player_gear_id;
      }
    }
  }
}
