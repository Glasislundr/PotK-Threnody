// Decompiled with JetBrains decompiler
// Type: SM.PlayerStepUpPackHistory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerStepUpPackHistory : KeyCompare
  {
    public int pack_id;
    public int day;
    public int set_id;
    public int id;

    public PlayerStepUpPackHistory()
    {
    }

    public PlayerStepUpPackHistory(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.pack_id = (int) (long) json[nameof (pack_id)];
      this.day = (int) (long) json[nameof (day)];
      this.set_id = (int) (long) json[nameof (set_id)];
      this.id = (int) (long) json[nameof (id)];
    }
  }
}
