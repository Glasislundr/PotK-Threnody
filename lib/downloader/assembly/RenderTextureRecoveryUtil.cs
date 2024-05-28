// Decompiled with JetBrains decompiler
// Type: RenderTextureRecoveryUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RenderTextureRecoveryUtil : MonoBehaviour
{
  private Dictionary<int, RenderTexture> m_cameraInfo;

  public void SaveRenderTexture()
  {
    if (this.m_cameraInfo == null)
      this.m_cameraInfo = new Dictionary<int, RenderTexture>();
    else
      this.m_cameraInfo.Clear();
    foreach (Camera camera in Object.FindObjectsOfType<Camera>())
      this.m_cameraInfo.Add(((Object) ((Component) camera).gameObject).GetInstanceID(), camera.targetTexture);
  }

  public void FixRenderTexture()
  {
    if (this.m_cameraInfo == null || this.m_cameraInfo.Count < 1)
    {
      this.SaveRenderTexture();
    }
    else
    {
      foreach (Camera camera in Object.FindObjectsOfType<Camera>())
      {
        if ((Object.op_Equality((Object) camera.targetTexture, (Object) null) || !camera.targetTexture.IsCreated()) && this.m_cameraInfo.ContainsKey(((Object) ((Component) camera).gameObject).GetInstanceID()))
        {
          RenderTexture renderTexture = (RenderTexture) null;
          if (this.m_cameraInfo.TryGetValue(((Object) ((Component) camera).gameObject).GetInstanceID(), out renderTexture) && Object.op_Inequality((Object) renderTexture, (Object) null))
          {
            renderTexture.Create();
            camera.targetTexture = renderTexture;
          }
        }
      }
    }
  }
}
