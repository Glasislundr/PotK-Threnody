// Decompiled with JetBrains decompiler
// Type: ShopBannerDotContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ShopBannerDotContainer : MonoBehaviour
{
  [SerializeField]
  private GameObject OnDot;
  [SerializeField]
  private GameObject OffDot;

  public void On()
  {
    this.OnDot.SetActive(true);
    this.OffDot.SetActive(false);
  }

  public void Off()
  {
    this.OnDot.SetActive(false);
    this.OffDot.SetActive(true);
  }
}
