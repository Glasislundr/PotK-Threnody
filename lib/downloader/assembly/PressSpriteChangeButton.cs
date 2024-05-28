// Decompiled with JetBrains decompiler
// Type: PressSpriteChangeButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PressSpriteChangeButton : UIButton
{
  [SerializeField]
  public Sprite idle;
  [SerializeField]
  public Sprite press;

  protected virtual void OnPress(bool isPressed)
  {
    ((UIButtonColor) this).OnPress(isPressed);
    if (!((Behaviour) this).enabled)
      return;
    UI2DSprite component = ((UIButtonColor) this).tweenTarget.GetComponent<UI2DSprite>();
    if (isPressed)
    {
      component.sprite2D = this.press;
    }
    else
    {
      component.sprite2D = this.idle;
      Singleton<NGSoundManager>.GetInstance().PlaySe("SE_1002");
    }
  }
}
