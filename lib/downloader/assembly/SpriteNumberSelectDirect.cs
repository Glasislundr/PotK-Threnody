// Decompiled with JetBrains decompiler
// Type: SpriteNumberSelectDirect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SpriteNumberSelectDirect : MonoBehaviour
{
  [SerializeField]
  private string spritePrefix = "slc_SetupTime";
  [SerializeField]
  private string spriteExt = ".png__GUI__battleUI_01__battleUI_01_prefab";
  private UISprite sprite;

  private void Awake() => this.initialize();

  private void initialize()
  {
    if (!Object.op_Equality((Object) this.sprite, (Object) null))
      return;
    this.sprite = ((Component) this).gameObject.GetComponent<UISprite>();
  }

  public void setNumber(int n) => this.setName(n.ToString());

  public void setName(string name)
  {
    this.initialize();
    this.setSprite(name);
  }

  public void setNumber(int n, Color col)
  {
    this.initialize();
    this.setName(n.ToString(), col);
  }

  public void setName(string name, Color col)
  {
    this.initialize();
    this.setSprite(name);
    ((UIWidget) this.sprite).color = col;
  }

  private void setSprite(string name)
  {
    this.sprite.spriteName = string.Format("{0}{1}{2}", (object) this.spritePrefix, (object) name, (object) this.spriteExt);
    UISpriteData atlasSprite = this.sprite.GetAtlasSprite();
    ((UIWidget) this.sprite).width = atlasSprite.width;
    ((UIWidget) this.sprite).height = atlasSprite.height;
  }
}
