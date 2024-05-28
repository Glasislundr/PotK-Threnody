// Decompiled with JetBrains decompiler
// Type: MapEdit031MenuView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class MapEdit031MenuView : MapEditMenuBase
{
  public override MapEdit031TopMenu.EditState editState_ => MapEdit031TopMenu.EditState.View;

  protected override IEnumerator initializeAsync()
  {
    yield break;
  }

  protected override void onEnable()
  {
    this.topMenu_.isEnabledButtonStorage_ = true;
    this.ui3DEvent_.isEnabled_ = true;
    this.ui3DEvent_.setEventPress(new EventDelegate.Callback(this.onPress));
    if (this.topMenu_.isEdit_)
      this.ui3DEvent_.setEventLongPress(new EventDelegate.Callback(this.onLongPress));
    else
      this.ui3DEvent_.resetEventLongPress();
    this.ui3DEvent_.setEventDrag(new Action<Vector2>(this.onDrag), new EventDelegate.Callback(this.onDragStart), new EventDelegate.Callback(this.onDragEnd));
    this.topMenu_.setEnabledInformation(true);
    this.topMenu_.updateCost();
  }

  protected override void onDisable() => this.topMenu_.setEnabledInformation(false);

  public override void onBackButton()
  {
  }

  private void onPress() => this.updateTouchInfo();

  private void onLongPress()
  {
    if (!this.updateTouchInfo(true))
      return;
    this.topMenu_.startLayout(this.currentOrnament_);
  }

  private bool updateTouchInfo(bool isLongPress = false)
  {
    MapEditPanel touchPanel = this.getTouchPanel();
    if (Object.op_Inequality((Object) touchPanel, (Object) null))
    {
      MapEditOrnament ornament = touchPanel.ornament_;
      if (isLongPress)
      {
        if (Object.op_Inequality((Object) ornament, (Object) null) && !this.waitAndSet())
        {
          this.topMenu_.setCurrentPanel(touchPanel, cameraCenter: false);
          this.setCurrentOrnament(ornament);
          return true;
        }
      }
      else
      {
        this.setCurrentPanel(touchPanel);
        if (Object.op_Inequality((Object) ornament, (Object) null))
          this.setCurrentOrnament(ornament);
        else
          this.clearCurrentOrnament();
      }
    }
    else if (!isLongPress)
    {
      this.clearCurrentPanel();
      this.clearCurrentOrnament();
    }
    return false;
  }

  private void onDragStart()
  {
    this.cntlCamera_.onPress();
    this.clearCurrentPanel();
    this.clearCurrentOrnament();
  }

  private void onDrag(Vector2 delta) => this.cntlCamera_.onDrag(delta);

  private void onDragEnd()
  {
  }
}
