// Decompiled with JetBrains decompiler
// Type: SM.QuestKeyGate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class QuestKeyGate : KeyCompare
  {
    public string title;
    public int quest_key_id;
    public int id;
    public int consume_quantity;
    public string time;

    public QuestKeyGate()
    {
    }

    public QuestKeyGate(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.title = (string) json[nameof (title)];
      this.quest_key_id = (int) (long) json[nameof (quest_key_id)];
      this.id = (int) (long) json[nameof (id)];
      this.consume_quantity = (int) (long) json[nameof (consume_quantity)];
      this.time = json[nameof (time)] == null ? (string) null : (string) json[nameof (time)];
    }
  }
}
