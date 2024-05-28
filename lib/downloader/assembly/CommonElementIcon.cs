// Decompiled with JetBrains decompiler
// Type: CommonElementIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using UnityEngine;

#nullable disable
public class CommonElementIcon : MonoBehaviour
{
  public UI2DSprite iconSprite;
  [SerializeField]
  private Sprite[] icons;
  private const int DEPTH = 10000;

  public Sprite getIcon(CommonElement element)
  {
    int index = (int) (element - 1);
    return index < 0 || index >= this.icons.Length ? (Sprite) null : this.icons[index];
  }

  public void Init(CommonElement element, bool resize = false)
  {
    this.iconSprite.sprite2D = this.getIcon(element);
    if (resize && Object.op_Inequality((Object) this.iconSprite.sprite2D, (Object) null))
      ((UIWidget) this.iconSprite).SetDimensions(((Texture) this.iconSprite.sprite2D.texture).width, ((Texture) this.iconSprite.sprite2D.texture).height);
    ((UIWidget) this.iconSprite).depth = 10000;
  }

  public void SetDimensions(int width, int height)
  {
    ((Component) this).GetComponent<UIWidget>().SetDimensions(width, height);
  }
}
