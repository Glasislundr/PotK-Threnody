// Decompiled with JetBrains decompiler
// Type: BurstParticleRendererMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BurstParticleRendererMonitor : MonoBehaviour
{
  private static Dictionary<string, Material> originalMaterial;
  [SerializeField]
  private string materialPath;

  private void Start()
  {
    if (BurstParticleRendererMonitor.originalMaterial == null)
      BurstParticleRendererMonitor.originalMaterial = new Dictionary<string, Material>();
    if (!BurstParticleRendererMonitor.originalMaterial.ContainsKey(this.materialPath))
      BurstParticleRendererMonitor.originalMaterial.Add(this.materialPath, new Material(Resources.Load<Material>(this.materialPath)));
    ((Component) this).GetComponent<Renderer>().material = BurstParticleRendererMonitor.originalMaterial[this.materialPath];
  }
}
