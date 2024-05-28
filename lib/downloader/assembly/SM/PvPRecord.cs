// Decompiled with JetBrains decompiler
// Type: SM.PvPRecord
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PvPRecord : KeyCompare
  {
    public int excellent_win;
    public int loss;
    public int draw;
    public int disconnected;
    public int win;
    public int max_consecutive_win;
    public int point_win;
    public int great_win;
    public int entry;
    public int current_consecutive_win;
    public int current_consecutive_loss;

    public PvPRecord()
    {
    }

    public PvPRecord(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.excellent_win = (int) (long) json[nameof (excellent_win)];
      this.loss = (int) (long) json[nameof (loss)];
      this.draw = (int) (long) json[nameof (draw)];
      this.disconnected = (int) (long) json[nameof (disconnected)];
      this.win = (int) (long) json[nameof (win)];
      this.max_consecutive_win = (int) (long) json[nameof (max_consecutive_win)];
      this.point_win = (int) (long) json[nameof (point_win)];
      this.great_win = (int) (long) json[nameof (great_win)];
      this.entry = (int) (long) json[nameof (entry)];
      this.current_consecutive_win = (int) (long) json[nameof (current_consecutive_win)];
      this.current_consecutive_loss = (int) (long) json[nameof (current_consecutive_loss)];
    }
  }
}
