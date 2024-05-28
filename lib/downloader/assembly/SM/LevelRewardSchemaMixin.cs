// Decompiled with JetBrains decompiler
// Type: SM.LevelRewardSchemaMixin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class LevelRewardSchemaMixin : KeyCompare
  {
    public int reward_quantity;
    public string reward_message;
    public string reward_title;
    public int? reward_id;
    public int reward_type_id;

    public LevelRewardSchemaMixin()
    {
    }

    public LevelRewardSchemaMixin(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.reward_quantity = (int) (long) json[nameof (reward_quantity)];
      this.reward_message = json[nameof (reward_message)] == null ? (string) null : (string) json[nameof (reward_message)];
      this.reward_title = json[nameof (reward_title)] == null ? (string) null : (string) json[nameof (reward_title)];
      int? nullable1;
      if (json[nameof (reward_id)] != null)
      {
        long? nullable2 = (long?) json[nameof (reward_id)];
        nullable1 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
      }
      else
        nullable1 = new int?();
      this.reward_id = nullable1;
      this.reward_type_id = (int) (long) json[nameof (reward_type_id)];
    }
  }
}
