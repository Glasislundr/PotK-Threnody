// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleStageUserUnit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BattleStageUserUnit
  {
    public int ID;
    public int stage_BattleStage;
    public int deck_position;
    public int money;
    public int initial_coordinate_x;
    public int initial_coordinate_y;
    public int? reinforcement_BattleReinforcement;
    public float initial_direction;
    public int ai_move_group;
    public string ai_attack;
    public string ai_move;
    public string ai_heal;

    public static BattleStageUserUnit Parse(MasterDataReader reader)
    {
      return new BattleStageUserUnit()
      {
        ID = reader.ReadInt(),
        stage_BattleStage = reader.ReadInt(),
        deck_position = reader.ReadInt(),
        money = reader.ReadInt(),
        initial_coordinate_x = reader.ReadInt(),
        initial_coordinate_y = reader.ReadInt(),
        reinforcement_BattleReinforcement = reader.ReadIntOrNull(),
        initial_direction = reader.ReadFloat(),
        ai_move_group = reader.ReadInt(),
        ai_attack = reader.ReadString(true),
        ai_move = reader.ReadString(true),
        ai_heal = reader.ReadString(true)
      };
    }

    public BattleStage stage
    {
      get
      {
        BattleStage stage;
        if (!MasterData.BattleStage.TryGetValue(this.stage_BattleStage, out stage))
          Debug.LogError((object) ("Key not Found: MasterData.BattleStage[" + (object) this.stage_BattleStage + "]"));
        return stage;
      }
    }

    public BattleReinforcement reinforcement
    {
      get
      {
        if (!this.reinforcement_BattleReinforcement.HasValue)
          return (BattleReinforcement) null;
        BattleReinforcement reinforcement;
        if (!MasterData.BattleReinforcement.TryGetValue(this.reinforcement_BattleReinforcement.Value, out reinforcement))
          Debug.LogError((object) ("Key not Found: MasterData.BattleReinforcement[" + (object) this.reinforcement_BattleReinforcement.Value + "]"));
        return reinforcement;
      }
    }
  }
}
