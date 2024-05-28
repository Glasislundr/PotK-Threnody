// Decompiled with JetBrains decompiler
// Type: UIUnityRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UIUnityRenderer : UIWidget
{
  public bool allowSharedMaterial;
  [HideInInspector]
  [SerializeField]
  private Renderer mRenderer;
  [HideInInspector]
  [SerializeField]
  private int renderQueue = -1;
  [HideInInspector]
  [SerializeField]
  private Material[] mMats;

  public Renderer cachedRenderer
  {
    get
    {
      if (Object.op_Equality((Object) this.mRenderer, (Object) null))
        this.mRenderer = ((Component) this).GetComponent<Renderer>();
      return this.mRenderer;
    }
  }

  public virtual Material material
  {
    get
    {
      if (!this.ExistSharedMaterial0())
      {
        Debug.LogError((object) "Renderer or Material is not found.");
        return (Material) null;
      }
      if (this.allowSharedMaterial)
        return this.cachedRenderer.sharedMaterials[0];
      if (!this.CheckMaterial(this.mMats))
      {
        List<Material> materialList = new List<Material>();
        foreach (Material sharedMaterial in this.cachedRenderer.sharedMaterials)
        {
          if (!Object.op_Equality((Object) sharedMaterial, (Object) null))
          {
            Material material = new Material(sharedMaterial);
            ((Object) material).name = ((Object) material).name + " (Copy)";
            materialList.Add(material);
          }
        }
        this.mMats = materialList.ToArray();
      }
      if (this.CheckMaterial(this.mMats) && Application.isPlaying && this.cachedRenderer.materials != this.mMats)
        this.cachedRenderer.materials = this.mMats;
      return this.mMats[0];
    }
    set
    {
      throw new NotImplementedException(((object) this).GetType().ToString() + " has no material setter");
    }
  }

  public virtual Shader shader
  {
    get
    {
      if (!this.allowSharedMaterial)
      {
        if (this.CheckMaterial(this.mMats))
          return this.mMats[0].shader;
      }
      else if (this.ExistSharedMaterial0())
        return this.cachedRenderer.sharedMaterials[0].shader;
      return (Shader) null;
    }
    set
    {
      throw new NotImplementedException(((object) this).GetType().ToString() + " has no shader setter");
    }
  }

  private bool ExistSharedMaterial0()
  {
    return Object.op_Inequality((Object) this.cachedRenderer, (Object) null) && this.CheckMaterial(this.cachedRenderer.sharedMaterials);
  }

  private bool CheckMaterial(Material[] mats)
  {
    if (mats == null || mats.Length == 0)
      return false;
    for (int index = 0; index < mats.Length; ++index)
    {
      if (Object.op_Inequality((Object) mats[index], (Object) null))
        return true;
    }
    return false;
  }

  private void OnDestroy()
  {
    if (this.mMats == null)
      return;
    for (int index = 0; index < this.mMats.Length; ++index)
    {
      Object.DestroyImmediate((Object) this.mMats[index]);
      this.mMats[index] = (Material) null;
    }
    this.mMats = (Material[]) null;
  }

  protected virtual void OnUpdate()
  {
    base.OnUpdate();
    if (!this.allowSharedMaterial)
    {
      if (!this.CheckMaterial(this.mMats) || !Object.op_Inequality((Object) this.drawCall, (Object) null))
        return;
      this.renderQueue = this.drawCall.finalRenderQueue;
      for (int index = 0; index < this.mMats.Length; ++index)
      {
        if (this.mMats[index].renderQueue != this.renderQueue)
          this.mMats[index].renderQueue = this.renderQueue;
      }
    }
    else
    {
      if (!this.ExistSharedMaterial0() || !Object.op_Inequality((Object) this.drawCall, (Object) null))
        return;
      this.renderQueue = this.drawCall.finalRenderQueue;
      for (int index = 0; index < this.cachedRenderer.sharedMaterials.Length; ++index)
      {
        if (Object.op_Inequality((Object) this.cachedRenderer.sharedMaterials[index], (Object) null))
          this.cachedRenderer.sharedMaterials[index].renderQueue = this.renderQueue;
      }
    }
  }

  private void LateUpdate() => ((UIRect) this).OnUpdate();

  public virtual void OnFill(
    BetterList<Vector3> verts,
    BetterList<Vector2> uvs,
    BetterList<Color32> cols)
  {
    verts.Add(new Vector3(10000f, 10000f));
    verts.Add(new Vector3(10000f, 10000f));
    verts.Add(new Vector3(10000f, 10000f));
    verts.Add(new Vector3(10000f, 10000f));
    uvs.Add(new Vector2(0.0f, 0.0f));
    uvs.Add(new Vector2(0.0f, 1f));
    uvs.Add(new Vector2(1f, 1f));
    uvs.Add(new Vector2(1f, 0.0f));
    cols.Add(Color32.op_Implicit(this.color));
    cols.Add(Color32.op_Implicit(this.color));
    cols.Add(Color32.op_Implicit(this.color));
    cols.Add(Color32.op_Implicit(this.color));
  }
}
