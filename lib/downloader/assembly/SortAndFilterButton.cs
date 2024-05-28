// Decompiled with JetBrains decompiler
// Type: SortAndFilterButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SortAndFilterButton : MonoBehaviour
{
  private UIButton button;
  [SerializeField]
  private UISprite sprite;
  private UI2DSprite sprite2d;
  private TweenColor[] tweens;

  public UIButton Button => this.button;

  public UISprite Sprite => this.sprite;

  protected virtual void Awake()
  {
    this.button = ((Component) this).GetComponent<UIButton>();
    if (Object.op_Equality((Object) this.sprite, (Object) null))
      this.sprite = ((Component) this).GetComponent<UISprite>();
    this.sprite2d = ((Component) this).GetComponent<UI2DSprite>();
    this.tweens = ((Component) this).GetComponents<TweenColor>();
  }

  public void SpriteColorGray(bool flag)
  {
    if (this.tweens != null)
    {
      foreach (Behaviour tween in this.tweens)
        tween.enabled = false;
    }
    Color color = Color.gray;
    if (flag)
      color = Color.white;
    if (Object.op_Inequality((Object) this.button, (Object) null))
    {
      ((UIButtonColor) this.button).defaultColor = color;
      ((UIButtonColor) this.button).hover = color;
      ((UIButtonColor) this.button).pressed = color;
    }
    if (Object.op_Inequality((Object) this.sprite, (Object) null))
      ((UIWidget) this.sprite).color = color;
    if (!Object.op_Inequality((Object) this.sprite2d, (Object) null))
      return;
    ((UIWidget) this.sprite2d).color = color;
  }

  public virtual void PressButton()
  {
  }
}
