// Decompiled with JetBrains decompiler
// Type: UISpriteExtentions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class UISpriteExtentions
{
  public static void SetSprite(this UISprite self, string fileName)
  {
    UISpriteData sprite = self.atlas.GetSprite(fileName);
    if (sprite != null)
    {
      ((Component) self).gameObject.SetActive(true);
      self.spriteName = fileName;
      UIWidget component = ((Component) self).GetComponent<UIWidget>();
      Vector3 localPosition = ((Component) component).transform.localPosition;
      component.SetRect(0.0f, 0.0f, (float) sprite.width, (float) sprite.height);
      ((Component) component).transform.localPosition = localPosition;
    }
    else
    {
      Debug.LogError((object) string.Format("＋＋＋＋＋＋＋　{0} がありません　＋＋＋＋＋＋＋", (object) fileName));
      ((Component) self).gameObject.SetActive(false);
    }
  }
}
