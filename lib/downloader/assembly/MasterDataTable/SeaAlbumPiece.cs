// Decompiled with JetBrains decompiler
// Type: MasterDataTable.SeaAlbumPiece
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class SeaAlbumPiece
  {
    public int ID;
    public int album_id;
    public int piece_id;
    public int same_character_id;
    public string name;
    public int count;
    public bool is_released;

    public static SeaAlbumPiece Parse(MasterDataReader reader)
    {
      return new SeaAlbumPiece()
      {
        ID = reader.ReadInt(),
        album_id = reader.ReadInt(),
        piece_id = reader.ReadInt(),
        same_character_id = reader.ReadInt(),
        name = reader.ReadString(true),
        count = reader.ReadInt(),
        is_released = reader.ReadBool()
      };
    }
  }
}
