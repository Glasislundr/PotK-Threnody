// Decompiled with JetBrains decompiler
// Type: SM.GvgDeck
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
  public class GvgDeck : KeyCompare
  {
    public int cost_limit;
    public PlayerUnit[] player_units;
    public int member_limit;
    public PlayerUnit[] over_killers;
    public int?[] player_unit_ids;
    public PlayerItem[] player_gears;
    public PlayerAwakeSkill[] player_awake_skills;
    public PlayerGearReisouSchema[] player_reisou_gears;

    public GvgDeck()
    {
    }

    public GvgDeck(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.cost_limit = (int) (long) json[nameof (cost_limit)];
      List<PlayerUnit> playerUnitList1 = new List<PlayerUnit>();
      foreach (object json1 in (List<object>) json[nameof (player_units)])
        playerUnitList1.Add(json1 == null ? (PlayerUnit) null : new PlayerUnit((Dictionary<string, object>) json1));
      this.player_units = playerUnitList1.ToArray();
      this.member_limit = (int) (long) json[nameof (member_limit)];
      List<PlayerUnit> playerUnitList2 = new List<PlayerUnit>();
      foreach (object json2 in (List<object>) json[nameof (over_killers)])
        playerUnitList2.Add(json2 == null ? (PlayerUnit) null : new PlayerUnit((Dictionary<string, object>) json2));
      this.over_killers = playerUnitList2.ToArray();
      this.player_unit_ids = ((IEnumerable<object>) json[nameof (player_unit_ids)]).Select<object, int?>((Func<object, int?>) (s =>
      {
        long? nullable = (long?) s;
        return !nullable.HasValue ? new int?() : new int?((int) nullable.GetValueOrDefault());
      })).ToArray<int?>();
      List<PlayerItem> playerItemList = new List<PlayerItem>();
      foreach (object json3 in (List<object>) json[nameof (player_gears)])
        playerItemList.Add(json3 == null ? (PlayerItem) null : new PlayerItem((Dictionary<string, object>) json3));
      this.player_gears = playerItemList.ToArray();
      List<PlayerAwakeSkill> playerAwakeSkillList = new List<PlayerAwakeSkill>();
      foreach (object json4 in (List<object>) json[nameof (player_awake_skills)])
        playerAwakeSkillList.Add(json4 == null ? (PlayerAwakeSkill) null : new PlayerAwakeSkill((Dictionary<string, object>) json4));
      this.player_awake_skills = playerAwakeSkillList.ToArray();
      List<PlayerGearReisouSchema> gearReisouSchemaList = new List<PlayerGearReisouSchema>();
      foreach (object json5 in (List<object>) json[nameof (player_reisou_gears)])
        gearReisouSchemaList.Add(json5 == null ? (PlayerGearReisouSchema) null : new PlayerGearReisouSchema((Dictionary<string, object>) json5));
      this.player_reisou_gears = gearReisouSchemaList.ToArray();
    }
  }
}
