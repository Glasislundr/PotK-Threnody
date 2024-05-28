// Decompiled with JetBrains decompiler
// Type: Unit05422Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
public class Unit05422Menu : Unit00422Menu
{
  protected override void SetBottomMode(UnitIcon icon)
  {
    icon.BottomModeValue = UnitIconBase.BottomMode.FriendlyEarth;
  }

  protected override void SetFriendlyEffect(UnitIconBase icon, bool value)
  {
    icon.SetFriendlyEffect(value, true);
  }

  protected override void SetPosessionText(int value, int max)
  {
    this.TxtOwnnumber.SetTextLocalize(string.Format("{0}", (object) value));
  }
}
