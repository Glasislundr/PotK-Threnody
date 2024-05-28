// Decompiled with JetBrains decompiler
// Type: SpriteTransitionDrawCall
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class SpriteTransitionDrawCall : MonoBehaviour
{
  public UI2DSprite sprite;
  public UIDrawCall drawcall;
  [Header("Dissolve Mask")]
  public Color color = Color.white;
  public bool brighten;
  [Tooltip("Dissolve Mask")]
  public Texture2D mask;
  [Range(0.0f, 1f)]
  public float width;
  [Range(0.0f, 1f)]
  public float disolvePower;
  public Color edgeColor = Color.white;
  [Range(0.0f, 20f)]
  public float edgeMag = 1f;
  [Range(0.0f, 1f)]
  public float edgeWidth;
  [Header("Alpha Mask")]
  [Tooltip("Alpha Mask")]
  public Texture2D alphaMask;
  public float xScale = 1f;
  public float yScale = 1f;
  public float xOffset;
  public float yOffset;
  public bool alphaMaskFlipX;
  public bool alphaMaskFlipY;
  public float xScaleMain = 1f;
  public float yScaleMain = 1f;
  public float xOffsetMain;
  public float yOffsetMain;

  private void Update() => this.RemoveIfNotUsed();

  private void RemoveIfNotUsed()
  {
    if (!Object.op_Equality((Object) this.drawcall, (Object) null) && !Object.op_Equality((Object) this.sprite, (Object) null) && !Object.op_Inequality((Object) this.drawcall, (Object) ((UIWidget) this.sprite).drawCall))
      return;
    Object.Destroy((Object) this);
  }

  private void OnWillRenderObject()
  {
    if (Object.op_Equality((Object) this.drawcall, (Object) null))
      return;
    if (Object.op_Equality((Object) this.drawcall.dynamicMaterial, (Object) null))
      Debug.Log((object) "Material is null.");
    this.drawcall.dynamicMaterial.SetColor("_MainColor", this.color);
    this.drawcall.dynamicMaterial.SetFloat("_Brighten", this.brighten ? 0.0f : 1f);
    this.drawcall.dynamicMaterial.SetTexture("_Mask", (Texture) this.mask);
    this.drawcall.dynamicMaterial.SetFloat("_Width", this.width);
    this.drawcall.dynamicMaterial.SetFloat("_DisolvePower", this.disolvePower);
    this.drawcall.dynamicMaterial.SetColor("_EdgeColor", this.edgeColor);
    this.drawcall.dynamicMaterial.SetFloat("_EdgeMag", this.edgeMag);
    this.drawcall.dynamicMaterial.SetFloat("_EdgeWidth", this.edgeWidth);
    if (Object.op_Inequality((Object) this.alphaMask, (Object) null))
      this.drawcall.dynamicMaterial.SetTexture("_AlphaMask", (Texture) this.alphaMask);
    this.drawcall.dynamicMaterial.SetFloat("_xScale", this.xScale);
    this.drawcall.dynamicMaterial.SetFloat("_yScale", this.yScale);
    this.drawcall.dynamicMaterial.SetFloat("_xOffset", this.xOffset);
    this.drawcall.dynamicMaterial.SetFloat("_yOffset", this.yOffset);
    this.drawcall.dynamicMaterial.SetFloat("_xFlip", this.alphaMaskFlipX ? 1f : 0.0f);
    this.drawcall.dynamicMaterial.SetFloat("_yFlip", this.alphaMaskFlipY ? 1f : 0.0f);
    this.drawcall.dynamicMaterial.SetFloat("_xScaleMain", this.xScaleMain);
    this.drawcall.dynamicMaterial.SetFloat("_yScaleMain", this.yScaleMain);
    this.drawcall.dynamicMaterial.SetFloat("_xOffsetMain", this.xOffsetMain);
    this.drawcall.dynamicMaterial.SetFloat("_yOffsetMain", this.yOffsetMain);
  }
}
