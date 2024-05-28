// Decompiled with JetBrains decompiler
// Type: SpriteSelectDirect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (UISprite))]
public class SpriteSelectDirect : MonoBehaviour
{
  [SerializeField]
  protected string spritePrefix;
  [SerializeField]
  protected string spriteExt;
  [SerializeField]
  private UISprite sprite;
  protected string spriteName = string.Empty;

  public UISprite Sprite => this.sprite;

  public virtual void SetSpriteName<T>(T n, bool resizeTarget = true)
  {
    this.spriteName = string.Format(this.spritePrefix + "{0}" + this.spriteExt, (object) n);
    this.sprite.ChangeSprite(this.spriteName, resizeTarget);
  }
}
