// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleVictoryCondition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BattleVictoryCondition
  {
    public int ID;
    public string name;
    public string sub_name;
    public int? enemy_BattleStageEnemy;
    public int? turn;
    public int? elapsed_turn;
    public int? win_area_confition_group_id;
    public int? lose_area_confition_group_id;
    public string lose_on_unit_dead;
    public string lose_on_gesut_dead;
    public int gameover_type_guest;
    public string victory_text;
    public string lose_text;

    public static BattleVictoryCondition Parse(MasterDataReader reader)
    {
      return new BattleVictoryCondition()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        sub_name = reader.ReadString(true),
        enemy_BattleStageEnemy = reader.ReadIntOrNull(),
        turn = reader.ReadIntOrNull(),
        elapsed_turn = reader.ReadIntOrNull(),
        win_area_confition_group_id = reader.ReadIntOrNull(),
        lose_area_confition_group_id = reader.ReadIntOrNull(),
        lose_on_unit_dead = reader.ReadStringOrNull(true),
        lose_on_gesut_dead = reader.ReadStringOrNull(true),
        gameover_type_guest = reader.ReadInt(),
        victory_text = reader.ReadString(true),
        lose_text = reader.ReadString(true)
      };
    }

    public BattleStageEnemy enemy
    {
      get
      {
        if (!this.enemy_BattleStageEnemy.HasValue)
          return (BattleStageEnemy) null;
        BattleStageEnemy enemy;
        if (!MasterData.BattleStageEnemy.TryGetValue(this.enemy_BattleStageEnemy.Value, out enemy))
          Debug.LogError((object) ("Key not Found: MasterData.BattleStageEnemy[" + (object) this.enemy_BattleStageEnemy.Value + "]"));
        return enemy;
      }
    }
  }
}
