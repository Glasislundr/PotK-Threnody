// Decompiled with JetBrains decompiler
// Type: NGFullWidthGrid
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class NGFullWidthGrid : UIGrid
{
  protected virtual void Init()
  {
    base.Init();
    ((UIRect) this.mPanel).UpdateAnchors();
    this.cellWidth = this.mPanel.width;
  }

  public virtual void Reposition()
  {
    if (!this.mInitDone)
      base.Init();
    if (Object.op_Inequality((Object) this.mPanel, (Object) null))
    {
      ((UIRect) this.mPanel).UpdateAnchors();
      if ((double) this.cellWidth != (double) this.mPanel.width)
        this.cellWidth = this.mPanel.width;
    }
    base.Reposition();
  }
}
