// Decompiled with JetBrains decompiler
// Type: SpreadColorSprite
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI SpreadColorSprite")]
public class SpreadColorSprite : UISprite
{
  private UIWidget[] objs;

  protected virtual void OnInit()
  {
    this.objs = ((Component) this).gameObject.GetComponentsInChildren<UIWidget>(true);
    base.OnInit();
  }

  public virtual void Invalidate(bool includeChildren)
  {
    if (this.objs != null)
    {
      foreach (UIWidget uiWidget in this.objs)
        uiWidget.color = ((UIWidget) this).color;
    }
    ((UIWidget) this).Invalidate(includeChildren);
  }
}
