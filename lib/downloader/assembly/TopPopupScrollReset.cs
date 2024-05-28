// Decompiled with JetBrains decompiler
// Type: TopPopupScrollReset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TopPopupScrollReset : MonoBehaviour
{
  [SerializeField]
  private UIScrollBar scrollBar;

  public void OnEnable() => ((UIProgressBar) this.scrollBar).value = 0.0f;
}
