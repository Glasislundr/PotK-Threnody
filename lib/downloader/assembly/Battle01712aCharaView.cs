// Decompiled with JetBrains decompiler
// Type: Battle01712aCharaView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Battle01712aCharaView : MonoBehaviour
{
  public void setSprite(Sprite sprite)
  {
    ((Component) this).GetComponent<NGxMaskSpriteWithScale>().MainUI2DSprite.sprite2D = sprite;
    ((Component) this).GetComponent<NGxMaskSpriteWithScale>().FitMask();
  }
}
