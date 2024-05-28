// Decompiled with JetBrains decompiler
// Type: UnitDetailGroupButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
public class UnitDetailGroupButton : UIButton
{
  private string groupTitle;
  private string groupDescription;
  private string groupSpriteName;
  private Action<string, string, string> pressAction;

  public void Init(
    Action<string, string, string> action,
    string title,
    string descript,
    string spriteName)
  {
    this.pressAction = action;
    this.groupTitle = title;
    this.groupDescription = descript;
    this.groupSpriteName = spriteName;
  }

  public void PressButton()
  {
    if (this.pressAction == null)
      return;
    this.pressAction(this.groupTitle, this.groupDescription, this.groupSpriteName);
  }
}
