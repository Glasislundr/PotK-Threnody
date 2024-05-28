// Decompiled with JetBrains decompiler
// Type: SM.AlbumPiece
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class AlbumPiece : KeyCompare
  {
    public int count;
    public string name;
    public int album_id;
    public int same_character_id;
    public int id;
    public int piece_id;

    public AlbumPiece()
    {
    }

    public AlbumPiece(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.count = (int) (long) json[nameof (count)];
      this.name = (string) json[nameof (name)];
      this.album_id = (int) (long) json[nameof (album_id)];
      this.same_character_id = (int) (long) json[nameof (same_character_id)];
      this.id = (int) (long) json[nameof (id)];
      this.piece_id = (int) (long) json[nameof (piece_id)];
    }
  }
}
