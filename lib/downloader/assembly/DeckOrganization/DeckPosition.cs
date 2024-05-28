// Decompiled with JetBrains decompiler
// Type: DeckOrganization.DeckPosition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace DeckOrganization
{
  public class DeckPosition
  {
    public int index_ { get; private set; }

    public Unit unit_ { get; private set; }

    public DeckPosition(int index)
    {
      this.index_ = index;
      this.unit_ = (Unit) null;
    }

    public void setUnit(Unit unit)
    {
      if (this.unit_ != null)
        this.unit_.setIndex();
      this.unit_ = unit;
      unit?.setIndex(this.index_);
    }
  }
}
