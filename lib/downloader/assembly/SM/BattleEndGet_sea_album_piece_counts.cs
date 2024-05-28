// Decompiled with JetBrains decompiler
// Type: SM.BattleEndGet_sea_album_piece_counts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class BattleEndGet_sea_album_piece_counts : KeyCompare
  {
    public int album_piece_id;
    public int count;

    public BattleEndGet_sea_album_piece_counts()
    {
    }

    public BattleEndGet_sea_album_piece_counts(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.album_piece_id = (int) (long) json[nameof (album_piece_id)];
      this.count = (int) (long) json[nameof (count)];
    }
  }
}
