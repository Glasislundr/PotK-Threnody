// Decompiled with JetBrains decompiler
// Type: UISpriteUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class UISpriteUpdater : MonoBehaviour
{
  [SerializeField]
  private UISpriteUpdater.Timing timing_ = UISpriteUpdater.Timing.NextFrame;
  [SerializeField]
  private UISprite[] sprites_;

  public void UpdateSprites()
  {
    if (this.timing_ == UISpriteUpdater.Timing.Immediate)
      this.updateSprites();
    else
      this.StartCoroutine(this.coDelayUpdate());
  }

  private IEnumerator coDelayUpdate()
  {
    if (this.timing_ == UISpriteUpdater.Timing.NextFrame)
      yield return (object) null;
    else
      yield return (object) new WaitForEndOfFrame();
    this.updateSprites();
  }

  private void updateSprites()
  {
    foreach (UIWidget sprite in this.sprites_)
      sprite.MarkAsChanged();
  }

  private enum Timing
  {
    Immediate,
    NextFrame,
    EndOfFrame,
  }
}
