// Decompiled with JetBrains decompiler
// Type: UISpriteExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public static class UISpriteExtension
{
  public static void ChangeSprite(this UISprite target, string newName, bool resizeTarget = true)
  {
    if (Object.op_Equality((Object) target, (Object) null))
    {
      Debug.LogWarning((object) "UISprite is Null");
    }
    else
    {
      if (target.atlas.spriteList.Find((Predicate<UISpriteData>) (x => x.name == newName)) == null)
        return;
      target.spriteName = newName;
      UISpriteData atlasSprite = target.GetAtlasSprite();
      if (atlasSprite == null)
      {
        Debug.LogWarning((object) "Atlas内のSprite取得失敗");
      }
      else
      {
        if (!resizeTarget)
          return;
        ((UIWidget) target).width = atlasSprite.width;
        ((UIWidget) target).height = atlasSprite.height;
      }
    }
  }
}
