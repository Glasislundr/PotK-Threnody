// Decompiled with JetBrains decompiler
// Type: PieceGetResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
public class PieceGetResult
{
  public string albumName;
  public string pieceName;
  public int count;
  public int same_character_id;

  public PieceGetResult(string album_name, string piece_name, int count, int same_character_id)
  {
    this.albumName = album_name;
    this.pieceName = piece_name;
    this.count = count;
    this.same_character_id = same_character_id;
  }
}
