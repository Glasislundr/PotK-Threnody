// Decompiled with JetBrains decompiler
// Type: SM.BattleEndUnlock_messages
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class BattleEndUnlock_messages : KeyCompare
  {
    public string message;
    public string title;

    public BattleEndUnlock_messages()
    {
    }

    public BattleEndUnlock_messages(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.message = (string) json[nameof (message)];
      this.title = (string) json[nameof (title)];
    }
  }
}
