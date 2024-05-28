// Decompiled with JetBrains decompiler
// Type: Shop00720ReelPattern
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Shop00720ReelPattern : MonoBehaviour
{
  [SerializeField]
  private UI2DSprite[] Icons;
  [SerializeField]
  private UILabel Description;

  public void Init(List<Sprite> sprites, string txt)
  {
    foreach (var data in ((IEnumerable<UI2DSprite>) this.Icons).Select((s, i) => new
    {
      s = s,
      i = i
    }))
      this.SetIcon(data.s, sprites[data.i]);
    this.SetDiscription(txt);
  }

  private void SetIcon(UI2DSprite target, Sprite sprite) => target.sprite2D = sprite;

  private void SetDiscription(string txt) => this.Description.SetTextLocalize(txt);
}
