// Decompiled with JetBrains decompiler
// Type: PartsContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class PartsContainer : MonoBehaviour
{
  [SerializeField]
  private PartsContainer.PartSprite[] partsSprite_;
  [SerializeField]
  private PartsContainer.PartMaterial[] partsMaterial_;

  public Dictionary<string, Sprite> partsSprite
  {
    get
    {
      return this.partsSprite_ == null ? new Dictionary<string, Sprite>() : ((IEnumerable<PartsContainer.PartSprite>) this.partsSprite_).ToDictionary<PartsContainer.PartSprite, string, Sprite>((Func<PartsContainer.PartSprite, string>) (k => k.name), (Func<PartsContainer.PartSprite, Sprite>) (v => v.sprite));
    }
  }

  public Dictionary<string, Material> partsMaterial
  {
    get
    {
      return this.partsMaterial_ == null ? new Dictionary<string, Material>() : ((IEnumerable<PartsContainer.PartMaterial>) this.partsMaterial_).ToDictionary<PartsContainer.PartMaterial, string, Material>((Func<PartsContainer.PartMaterial, string>) (k => k.name), (Func<PartsContainer.PartMaterial, Material>) (v => v.material));
    }
  }

  [Serializable]
  public class PartSprite
  {
    public string name = string.Empty;
    public Sprite sprite;
  }

  [Serializable]
  public class PartMaterial
  {
    public string name = string.Empty;
    public Material material;
  }
}
