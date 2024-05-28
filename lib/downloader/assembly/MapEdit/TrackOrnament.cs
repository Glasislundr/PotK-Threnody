// Decompiled with JetBrains decompiler
// Type: MapEdit.TrackOrnament
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace MapEdit
{
  public class TrackOrnament
  {
    public int ID_ { get; private set; }

    public int row_ { get; private set; }

    public int column_ { get; private set; }

    public bool isNew_ { get; private set; }

    public TrackOrnament(int id) => this.init(id, 0, 0, true);

    public TrackOrnament(int id, int row, int column) => this.init(id, row, column, false);

    private void init(int id, int row, int column, bool isnew)
    {
      this.ID_ = id;
      this.row_ = row;
      this.column_ = column;
      this.isNew_ = isnew;
    }
  }
}
