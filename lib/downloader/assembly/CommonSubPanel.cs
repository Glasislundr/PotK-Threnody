// Decompiled with JetBrains decompiler
// Type: CommonSubPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;
using Util;

#nullable disable
public class CommonSubPanel : MonoBehaviour
{
  [SerializeField]
  private GameObject subPanel;

  private void OnEnable()
  {
    if (!IOSUtil.IsDeviceGenerationiPhoneX || Object.op_Equality((Object) this.subPanel, (Object) null))
      return;
    this.subPanel.SetActive(true);
  }

  private void OnDisable()
  {
    if (!IOSUtil.IsDeviceGenerationiPhoneX || Object.op_Equality((Object) this.subPanel, (Object) null))
      return;
    this.subPanel.SetActive(false);
  }
}
