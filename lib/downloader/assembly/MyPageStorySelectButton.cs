// Decompiled with JetBrains decompiler
// Type: MyPageStorySelectButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MyPageStorySelectButton : MonoBehaviour
{
  [SerializeField]
  private UIButton button;
  [SerializeField]
  private UISprite sprite;
  private Color defaultSpriteColor;

  private void Start() => this.defaultSpriteColor = ((UIWidget) this.sprite).color;

  public void OnPress(bool isDown)
  {
    if (isDown)
      ((UIWidget) this.sprite).color = ((UIButtonColor) this.button).pressed;
    else
      ((UIWidget) this.sprite).color = this.defaultSpriteColor;
  }

  private void OnDragOut(GameObject draggedObject)
  {
    ((UIWidget) this.sprite).color = this.defaultSpriteColor;
  }

  private void OnDragEnd() => ((UIWidget) this.sprite).color = this.defaultSpriteColor;
}
