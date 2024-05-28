// Decompiled with JetBrains decompiler
// Type: SM.PvPEnd
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
  public class PvPEnd : KeyCompare
  {
    public int deck_type_id;
    public PlayerItem[] before_player_gears;
    public PlayerUnit[] after_player_units;
    public int remaining_times;
    public PvPEndUnlock_messages[] unlock_messages;
    public UnlockQuest[] unlock_quests;
    public PvPEndPlayer_character_intimates_in_battle[] player_character_intimates_in_battle;
    public int[] disappeared_player_gears;
    public PlayerEmblem[] new_emblems;
    public int battle_result;
    public PlayerUnit[] before_player_units;
    public int limit_times;
    public int deck_number;
    public PlayerItem[] after_player_gears;

    public PvPEnd()
    {
    }

    public PvPEnd(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.deck_type_id = (int) (long) json[nameof (deck_type_id)];
      List<PlayerItem> playerItemList1 = new List<PlayerItem>();
      foreach (object json1 in (List<object>) json[nameof (before_player_gears)])
        playerItemList1.Add(json1 == null ? (PlayerItem) null : new PlayerItem((Dictionary<string, object>) json1));
      this.before_player_gears = playerItemList1.ToArray();
      List<PlayerUnit> playerUnitList1 = new List<PlayerUnit>();
      foreach (object json2 in (List<object>) json[nameof (after_player_units)])
        playerUnitList1.Add(json2 == null ? (PlayerUnit) null : new PlayerUnit((Dictionary<string, object>) json2));
      this.after_player_units = playerUnitList1.ToArray();
      this.remaining_times = (int) (long) json[nameof (remaining_times)];
      List<PvPEndUnlock_messages> pendUnlockMessagesList = new List<PvPEndUnlock_messages>();
      foreach (object json3 in (List<object>) json[nameof (unlock_messages)])
        pendUnlockMessagesList.Add(json3 == null ? (PvPEndUnlock_messages) null : new PvPEndUnlock_messages((Dictionary<string, object>) json3));
      this.unlock_messages = pendUnlockMessagesList.ToArray();
      List<UnlockQuest> unlockQuestList = new List<UnlockQuest>();
      foreach (object json4 in (List<object>) json[nameof (unlock_quests)])
        unlockQuestList.Add(json4 == null ? (UnlockQuest) null : new UnlockQuest((Dictionary<string, object>) json4));
      this.unlock_quests = unlockQuestList.ToArray();
      List<PvPEndPlayer_character_intimates_in_battle> intimatesInBattleList = new List<PvPEndPlayer_character_intimates_in_battle>();
      foreach (object json5 in (List<object>) json[nameof (player_character_intimates_in_battle)])
        intimatesInBattleList.Add(json5 == null ? (PvPEndPlayer_character_intimates_in_battle) null : new PvPEndPlayer_character_intimates_in_battle((Dictionary<string, object>) json5));
      this.player_character_intimates_in_battle = intimatesInBattleList.ToArray();
      this.disappeared_player_gears = ((IEnumerable<object>) json[nameof (disappeared_player_gears)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      List<PlayerEmblem> playerEmblemList = new List<PlayerEmblem>();
      foreach (object json6 in (List<object>) json[nameof (new_emblems)])
        playerEmblemList.Add(json6 == null ? (PlayerEmblem) null : new PlayerEmblem((Dictionary<string, object>) json6));
      this.new_emblems = playerEmblemList.ToArray();
      this.battle_result = (int) (long) json[nameof (battle_result)];
      List<PlayerUnit> playerUnitList2 = new List<PlayerUnit>();
      foreach (object json7 in (List<object>) json[nameof (before_player_units)])
        playerUnitList2.Add(json7 == null ? (PlayerUnit) null : new PlayerUnit((Dictionary<string, object>) json7));
      this.before_player_units = playerUnitList2.ToArray();
      this.limit_times = (int) (long) json[nameof (limit_times)];
      this.deck_number = (int) (long) json[nameof (deck_number)];
      List<PlayerItem> playerItemList2 = new List<PlayerItem>();
      foreach (object json8 in (List<object>) json[nameof (after_player_gears)])
        playerItemList2.Add(json8 == null ? (PlayerItem) null : new PlayerItem((Dictionary<string, object>) json8));
      this.after_player_gears = playerItemList2.ToArray();
    }
  }
}
