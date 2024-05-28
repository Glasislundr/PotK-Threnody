// Decompiled with JetBrains decompiler
// Type: SM.BattleWaveStageInfo
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
  public class BattleWaveStageInfo : KeyCompare
  {
    public int[] enemy;
    public int stage_id;
    public PlayerUnit[] user_deck_units;
    public BattleWaveStageInfoPanel_item[] panel_item;
    public BattleWaveStageInfoUser_deck_enemy_item[] user_deck_enemy_item;
    public PlayerItem[] user_deck_gears;
    public int[] user_deck_enemy;
    public BattleWaveStageInfoEnemy_item[] enemy_item;
    public int[] panel;

    public BattleWaveStageInfo()
    {
    }

    public BattleWaveStageInfo(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.enemy = ((IEnumerable<object>) json[nameof (enemy)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.stage_id = (int) (long) json[nameof (stage_id)];
      List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
      foreach (object json1 in (List<object>) json[nameof (user_deck_units)])
        playerUnitList.Add(json1 == null ? (PlayerUnit) null : new PlayerUnit((Dictionary<string, object>) json1));
      this.user_deck_units = playerUnitList.ToArray();
      List<BattleWaveStageInfoPanel_item> stageInfoPanelItemList = new List<BattleWaveStageInfoPanel_item>();
      foreach (object json2 in (List<object>) json[nameof (panel_item)])
        stageInfoPanelItemList.Add(json2 == null ? (BattleWaveStageInfoPanel_item) null : new BattleWaveStageInfoPanel_item((Dictionary<string, object>) json2));
      this.panel_item = stageInfoPanelItemList.ToArray();
      List<BattleWaveStageInfoUser_deck_enemy_item> userDeckEnemyItemList = new List<BattleWaveStageInfoUser_deck_enemy_item>();
      foreach (object json3 in (List<object>) json[nameof (user_deck_enemy_item)])
        userDeckEnemyItemList.Add(json3 == null ? (BattleWaveStageInfoUser_deck_enemy_item) null : new BattleWaveStageInfoUser_deck_enemy_item((Dictionary<string, object>) json3));
      this.user_deck_enemy_item = userDeckEnemyItemList.ToArray();
      List<PlayerItem> playerItemList = new List<PlayerItem>();
      foreach (object json4 in (List<object>) json[nameof (user_deck_gears)])
        playerItemList.Add(json4 == null ? (PlayerItem) null : new PlayerItem((Dictionary<string, object>) json4));
      this.user_deck_gears = playerItemList.ToArray();
      this.user_deck_enemy = ((IEnumerable<object>) json[nameof (user_deck_enemy)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      List<BattleWaveStageInfoEnemy_item> stageInfoEnemyItemList = new List<BattleWaveStageInfoEnemy_item>();
      foreach (object json5 in (List<object>) json[nameof (enemy_item)])
        stageInfoEnemyItemList.Add(json5 == null ? (BattleWaveStageInfoEnemy_item) null : new BattleWaveStageInfoEnemy_item((Dictionary<string, object>) json5));
      this.enemy_item = stageInfoEnemyItemList.ToArray();
      this.panel = ((IEnumerable<object>) json[nameof (panel)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
    }
  }
}
