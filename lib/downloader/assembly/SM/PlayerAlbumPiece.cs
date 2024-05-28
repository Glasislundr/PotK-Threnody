// Decompiled with JetBrains decompiler
// Type: SM.PlayerAlbumPiece
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerAlbumPiece : KeyCompare
  {
    public int count;
    public bool is_open;
    public int id;
    public int piece_id;
    public int album_id;

    public PlayerAlbumPiece()
    {
    }

    public PlayerAlbumPiece(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.count = (int) (long) json[nameof (count)];
      this.is_open = (bool) json[nameof (is_open)];
      this.id = (int) (long) json[nameof (id)];
      this.piece_id = (int) (long) json[nameof (piece_id)];
      this.album_id = (int) (long) json[nameof (album_id)];
    }
  }
}
