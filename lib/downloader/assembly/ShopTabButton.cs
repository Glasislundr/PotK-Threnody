// Decompiled with JetBrains decompiler
// Type: ShopTabButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ShopTabButton : SpreadColorButton
{
  [SerializeField]
  private ShopTabType shopTabType;
  private bool isDragOut;

  private void OnDragOut(GameObject draggedObject) => this.isDragOut = true;

  protected override void OnPress(bool isPressed)
  {
    base.OnPress(isPressed);
    if (isPressed || !this.isDragOut || Shop0079Menu.CurrentTabType == this.shopTabType)
      return;
    this.SetSprite(this.disabledSprite);
    this.isDragOut = false;
  }

  protected override void SetState(UIButtonColor.State state, bool instant)
  {
  }
}
