// Decompiled with JetBrains decompiler
// Type: SM.PlayerAlbum
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerAlbum : KeyCompare
  {
    public bool is_end;
    public PlayerAlbumPiece[] player_album_piece;
    public int id;
    public int album_id;

    public PlayerAlbum()
    {
    }

    public PlayerAlbum(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.is_end = (bool) json[nameof (is_end)];
      List<PlayerAlbumPiece> playerAlbumPieceList = new List<PlayerAlbumPiece>();
      foreach (object json1 in (List<object>) json[nameof (player_album_piece)])
        playerAlbumPieceList.Add(json1 == null ? (PlayerAlbumPiece) null : new PlayerAlbumPiece((Dictionary<string, object>) json1));
      this.player_album_piece = playerAlbumPieceList.ToArray();
      this.id = (int) (long) json[nameof (id)];
      this.album_id = (int) (long) json[nameof (album_id)];
    }
  }
}
