// Decompiled with JetBrains decompiler
// Type: SM.PlayerTalkPartner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerTalkPartner : KeyCompare
  {
    public int unread_count;
    public PlayerTalkMessage message;
    public bool receivable_reward;
    public PlayerCallLetter letter;

    public PlayerTalkPartner()
    {
    }

    public PlayerTalkPartner(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.unread_count = (int) (long) json[nameof (unread_count)];
      this.message = json[nameof (message)] == null ? (PlayerTalkMessage) null : new PlayerTalkMessage((Dictionary<string, object>) json[nameof (message)]);
      this.receivable_reward = (bool) json[nameof (receivable_reward)];
      this.letter = json[nameof (letter)] == null ? (PlayerCallLetter) null : new PlayerCallLetter((Dictionary<string, object>) json[nameof (letter)]);
    }
  }
}
