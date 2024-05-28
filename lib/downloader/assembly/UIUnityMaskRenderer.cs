// Decompiled with JetBrains decompiler
// Type: UIUnityMaskRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UIUnityMaskRenderer : UIWidget
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
  private UIDrawCall.Clipping nowClip;
  private MaterialPropertyBlock mbp;

  public Renderer cachedRenderer
  {
    get
    {
      if (Object.op_Equality((Object) this.mRenderer, (Object) null))
        this.mRenderer = ((Component) this).GetComponent<Renderer>();
      return this.mRenderer;
    }
  }

  protected virtual void OnInit()
  {
    if (this.geometry == null)
      this.geometry = new UIGeometry();
    if (this.mCorners == null)
      this.mCorners = new Vector3[4];
    base.OnInit();
  }

  public virtual Vector3[] worldCorners
  {
    get
    {
      Vector2 pivotOffset = this.pivotOffset;
      float num1 = -pivotOffset.x * (float) this.mWidth;
      float num2 = -pivotOffset.y * (float) this.mHeight;
      float num3 = num1 + (float) this.mWidth;
      float num4 = num2 + (float) this.mHeight;
      Matrix4x4 matrix4x4 = Matrix4x4.op_Multiply(((UIRect) this).cachedTransform.localToWorldMatrix, Matrix4x4.Scale(new Vector3(1f / 1000f, 1f / 1000f, 1f / 1000f)));
      this.mCorners[0] = ((Matrix4x4) ref matrix4x4).MultiplyPoint(new Vector3(num1, num2, 0.0f));
      this.mCorners[1] = ((Matrix4x4) ref matrix4x4).MultiplyPoint(new Vector3(num1, num4, 0.0f));
      this.mCorners[2] = ((Matrix4x4) ref matrix4x4).MultiplyPoint(new Vector3(num3, num4, 0.0f));
      this.mCorners[3] = ((Matrix4x4) ref matrix4x4).MultiplyPoint(new Vector3(num3, num2, 0.0f));
      return this.mCorners;
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
            string str = ((Object) sharedMaterial.shader).name.Replace(" UnityRender (AlphaClip)", "").Replace(" UnityRender (SoftClip)", "");
            Shader shader = (Shader) null;
            if (Object.op_Inequality((Object) this.panel, (Object) null) && this.panel.clipping == 3)
            {
              shader = Shader.Find(str + " UnityRender (SoftClip)");
              this.nowClip = (UIDrawCall.Clipping) 3;
            }
            else if (Object.op_Inequality((Object) this.panel, (Object) null) && this.panel.clipping == 2)
            {
              shader = Shader.Find(str + " UnityRender (AlphaClip)");
              this.nowClip = (UIDrawCall.Clipping) 2;
            }
            if (Object.op_Equality((Object) shader, (Object) null))
            {
              shader = Shader.Find(str);
              this.nowClip = (UIDrawCall.Clipping) 0;
            }
            Material material = new Material(shader);
            material.CopyPropertiesFromMaterial(sharedMaterial);
            ((Object) material).name = ((Object) material).name + " (Copy)";
            materialList.Add(material);
          }
        }
        this.mMats = materialList.ToArray();
      }
      if (this.CheckMaterial(this.mMats) && Application.isPlaying && this.cachedRenderer.materials != this.mMats)
        this.cachedRenderer.materials = this.mMats;
      if (this.mbp != null && !this.mbp.isEmpty)
        this.cachedRenderer.SetPropertyBlock(this.mbp);
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
      if (this.CheckMaterial(this.mMats) && Object.op_Inequality((Object) this.drawCall, (Object) null))
      {
        this.renderQueue = this.drawCall.finalRenderQueue;
        for (int index = 0; index < this.mMats.Length; ++index)
        {
          if (this.mMats[index].renderQueue != this.renderQueue)
            this.mMats[index].renderQueue = this.renderQueue;
        }
      }
    }
    else if (this.ExistSharedMaterial0() && Object.op_Inequality((Object) this.drawCall, (Object) null))
    {
      this.renderQueue = this.drawCall.finalRenderQueue;
      for (int index = 0; index < this.cachedRenderer.sharedMaterials.Length; ++index)
        this.cachedRenderer.sharedMaterials[index].renderQueue = this.renderQueue;
    }
    if (!Object.op_Inequality((Object) this.panel, (Object) null) || this.panel.clipping == this.nowClip)
      return;
    if (this.mMats != null)
    {
      for (int index = 0; index < this.mMats.Length; ++index)
      {
        Object.Destroy((Object) this.mMats[index]);
        this.mMats[index] = (Material) null;
      }
      this.mMats = (Material[]) null;
    }
    if (this.allowSharedMaterial)
      return;
    if (!this.CheckMaterial(this.mMats))
    {
      List<Material> materialList = new List<Material>();
      foreach (Material sharedMaterial in this.cachedRenderer.sharedMaterials)
      {
        if (!Object.op_Equality((Object) sharedMaterial, (Object) null))
        {
          string str = ((Object) sharedMaterial.shader).name.Replace(" UnityRender (AlphaClip)", "").Replace(" UnityRender (SoftClip)", "");
          Shader shader = (Shader) null;
          if (Object.op_Inequality((Object) this.panel, (Object) null) && this.panel.clipping == 3)
          {
            shader = Shader.Find(str + " UnityRender (SoftClip)");
            this.nowClip = (UIDrawCall.Clipping) 3;
          }
          else if (Object.op_Inequality((Object) this.panel, (Object) null) && this.panel.clipping == 2)
          {
            shader = Shader.Find(str + " UnityRender (AlphaClip)");
            this.nowClip = (UIDrawCall.Clipping) 2;
          }
          if (Object.op_Equality((Object) shader, (Object) null))
          {
            shader = Shader.Find(str);
            this.nowClip = (UIDrawCall.Clipping) 0;
          }
          Material material = new Material(shader);
          material.CopyPropertiesFromMaterial(sharedMaterial);
          ((Object) material).name = ((Object) material).name + " (Copy)";
          materialList.Add(material);
        }
      }
      this.mMats = materialList.ToArray();
    }
    if (this.CheckMaterial(this.mMats) && Application.isPlaying && this.cachedRenderer.materials != this.mMats)
      this.cachedRenderer.materials = this.mMats;
    if (this.mbp == null || this.mbp.isEmpty)
      return;
    this.cachedRenderer.SetPropertyBlock(this.mbp);
  }

  private void OnWillRenderObject()
  {
    if (Object.op_Equality((Object) this.drawCall, (Object) null) || this.mMats == null || this.mMats.Length == 0 || !this.drawCall.isClipped || this.drawCall.clipping == 4)
      return;
    Vector2 vector2_1;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_1).\u002Ector(-this.drawCall.clipRange.x / this.drawCall.clipRange.z, -this.drawCall.clipRange.y / this.drawCall.clipRange.w);
    Vector2 vector2_2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_2).\u002Ector(1f / this.drawCall.clipRange.z, 1f / this.drawCall.clipRange.w);
    Vector2 vector2_3;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_3).\u002Ector(1000f, 1000f);
    if ((double) this.drawCall.clipSoftness.x > 0.0)
      vector2_3.x = this.drawCall.clipRange.z / this.drawCall.clipSoftness.x;
    if ((double) this.drawCall.clipSoftness.y > 0.0)
      vector2_3.y = this.drawCall.clipRange.w / this.drawCall.clipSoftness.y;
    for (int index = 0; index < this.mMats.Length; ++index)
    {
      Material mMat = this.mMats[index];
      Matrix4x4 localToWorldMatrix = ((UIRect) this.panel).cachedTransform.localToWorldMatrix;
      Matrix4x4 inverse = ((Matrix4x4) ref localToWorldMatrix).inverse;
      mMat.SetMatrix("_Trans", inverse);
      this.mMats[index].SetVector("_ViewScale", new Vector4(vector2_2.x, vector2_2.y, vector2_1.x, vector2_1.y));
      this.mMats[index].SetVector("_ClipSharpness", Vector4.op_Implicit(vector2_3));
    }
  }

  public void SetTexture(string name, Texture tex)
  {
    if (Object.op_Equality((Object) tex, (Object) null))
      return;
    if (this.mbp == null)
      this.mbp = new MaterialPropertyBlock();
    this.mbp.SetTexture(name, tex);
    if (this.mbp == null || this.mbp.isEmpty)
      return;
    this.cachedRenderer.SetPropertyBlock(this.mbp);
  }

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
