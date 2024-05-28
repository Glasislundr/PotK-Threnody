// Decompiled with JetBrains decompiler
// Type: UrlClick
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using DeviceKit;
using UnityEngine;

#nullable disable
public class UrlClick : MonoBehaviour
{
  [SerializeField]
  private UILabel label;

  private void OnClick()
  {
    string urlAtPosition = this.label.GetUrlAtPosition(((RaycastHit) ref UICamera.lastHit).point);
    if (urlAtPosition == null)
      return;
    Debug.Log((object) ("open url " + urlAtPosition));
    App.OpenUrl(urlAtPosition);
  }
}
