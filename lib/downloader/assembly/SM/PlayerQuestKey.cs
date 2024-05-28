// Decompiled with JetBrains decompiler
// Type: SM.PlayerQuestKey
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerQuestKey : KeyCompare
  {
    public string player_id;
    public int max_quantity;
    public int quest_key_id;
    public int quantity;

    public static PlayerQuestKey CreateForKey(int id)
    {
      PlayerQuestKey forKey = new PlayerQuestKey();
      forKey._hasKey = true;
      int num1;
      int num2 = num1 = id;
      forKey.quest_key_id = num1;
      forKey._key = (object) num2;
      return forKey;
    }

    public PlayerQuestKey()
    {
    }

    public PlayerQuestKey(Dictionary<string, object> json)
    {
      this._hasKey = true;
      this.player_id = (string) json[nameof (player_id)];
      this.max_quantity = (int) (long) json[nameof (max_quantity)];
      this._key = (object) (this.quest_key_id = (int) (long) json[nameof (quest_key_id)]);
      this.quantity = (int) (long) json[nameof (quantity)];
    }
  }
}
