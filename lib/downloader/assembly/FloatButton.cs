// Decompiled with JetBrains decompiler
// Type: FloatButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class FloatButton : UIButton
{
  public List<EventDelegate> onOver = new List<EventDelegate>();
  public List<EventDelegate> onOut = new List<EventDelegate>();
  protected bool isActive;

  protected virtual void OnPress(bool isPressed)
  {
    ((UIButtonColor) this).OnPress(isPressed);
    if (isPressed)
      this.OnOver();
    else
      this.OnOut();
  }

  protected virtual void OnDragOver()
  {
    base.OnDragOver();
    this.OnOver();
  }

  protected virtual void OnDragOut()
  {
    base.OnDragOut();
    this.OnOut();
  }

  protected virtual void doExecute(List<EventDelegate> events, bool active)
  {
    if (!((UIButtonColor) this).isEnabled || this.isActive != !active)
      return;
    UIButton.current = (UIButton) this;
    EventDelegate.Execute(events);
    UIButton.current = (UIButton) null;
    this.isActive = active;
  }

  protected virtual void OnOver() => this.doExecute(this.onOver, true);

  protected virtual void OnOut() => this.doExecute(this.onOut, false);
}
