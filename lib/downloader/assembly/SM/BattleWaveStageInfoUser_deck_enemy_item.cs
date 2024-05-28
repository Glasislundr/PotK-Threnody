// Decompiled with JetBrains decompiler
// Type: SM.BattleWaveStageInfoUser_deck_enemy_item
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class BattleWaveStageInfoUser_deck_enemy_item : KeyCompare
  {
    public int reward_quantity;
    public int id;
    public int reward_id;
    public int reward_type_id;

    public BattleWaveStageInfoUser_deck_enemy_item()
    {
    }

    public BattleWaveStageInfoUser_deck_enemy_item(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.reward_quantity = (int) (long) json[nameof (reward_quantity)];
      this.id = (int) (long) json[nameof (id)];
      this.reward_id = (int) (long) json[nameof (reward_id)];
      this.reward_type_id = (int) (long) json[nameof (reward_type_id)];
    }
  }
}
