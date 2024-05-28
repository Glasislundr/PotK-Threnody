// Decompiled with JetBrains decompiler
// Type: SM.PlayerCorps
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerCorps : KeyCompare
  {
    public int[] cleared_mission_ids;
    public int[] cleared_stage_ids;
    public int corps_id;
    public PlayerCorpsStage[] corps_stages;
    public int[] current_cleared_stage_ids;
    public int[] deck_supply_ids;
    public int[] deck_supply_quantities;
    public int[] entry_player_unit_ids;
    public int period_id;
    public int[] used_player_unit_ids;

    public PlayerCorps()
    {
    }

    public PlayerCorps(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.cleared_mission_ids = ((IEnumerable<object>) json[nameof (cleared_mission_ids)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.cleared_stage_ids = ((IEnumerable<object>) json[nameof (cleared_stage_ids)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.corps_id = (int) (long) json[nameof (corps_id)];
      List<PlayerCorpsStage> playerCorpsStageList = new List<PlayerCorpsStage>();
      foreach (object json1 in (List<object>) json[nameof (corps_stages)])
        playerCorpsStageList.Add(json1 == null ? (PlayerCorpsStage) null : new PlayerCorpsStage((Dictionary<string, object>) json1));
      this.corps_stages = playerCorpsStageList.ToArray();
      this.current_cleared_stage_ids = ((IEnumerable<object>) json[nameof (current_cleared_stage_ids)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.deck_supply_ids = ((IEnumerable<object>) json[nameof (deck_supply_ids)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.deck_supply_quantities = ((IEnumerable<object>) json[nameof (deck_supply_quantities)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.entry_player_unit_ids = ((IEnumerable<object>) json[nameof (entry_player_unit_ids)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.period_id = (int) (long) json[nameof (period_id)];
      this.used_player_unit_ids = ((IEnumerable<object>) json[nameof (used_player_unit_ids)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
    }

    public PlayerItem[] supplies
    {
      get
      {
        int[] supplyQuantities = this.deck_supply_quantities;
        int length = supplyQuantities != null ? ((IEnumerable<int>) supplyQuantities).Count<int>((Func<int, bool>) (x => x > 0)) : 0;
        if (length == 0)
          return new PlayerItem[0];
        PlayerItem[] supplies = new PlayerItem[length];
        int num = 0;
        for (int index = 0; index < this.deck_supply_ids.Length; ++index)
        {
          if (this.deck_supply_quantities[index] != 0)
            supplies[num++] = new PlayerItem()
            {
              entity_id = this.deck_supply_ids[index],
              box_type_id = 5,
              _entity_type = 2,
              id = index + 1,
              quantity = this.deck_supply_quantities[index]
            };
        }
        return supplies;
      }
    }
  }
}
