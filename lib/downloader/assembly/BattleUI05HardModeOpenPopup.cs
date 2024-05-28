// Decompiled with JetBrains decompiler
// Type: BattleUI05HardModeOpenPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
public class BattleUI05HardModeOpenPopup : NGMenuBase
{
  public UILabel TxtTitle;
  public UILabel TxtDescription;
  private BattleUI05HardModeOpenMenu menu;

  public void IbtnPopupOk() => this.menu.ToNext = true;

  public void Init(BattleUI05HardModeOpenMenu menu, string title, string message)
  {
    this.menu = menu;
    this.TxtTitle.SetText(title);
    this.TxtDescription.SetText(message);
  }
}
