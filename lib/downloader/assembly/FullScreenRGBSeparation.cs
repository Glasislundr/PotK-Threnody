// Decompiled with JetBrains decompiler
// Type: FullScreenRGBSeparation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
public class FullScreenRGBSeparation : MonoBehaviour
{
  private Material material;
  [Header("Pivot")]
  public Vector2 redPivot = new Vector2(0.5f, 0.5f);
  public Vector2 greenPivot = new Vector2(0.5f, 0.5f);
  public Vector2 bluePivot = new Vector2(0.5f, 0.5f);
  [Header("Scale")]
  public float redScale = 1f;
  public float greenScale = 1f;
  public float blueScale = 1f;
  [Header("Rotation")]
  public float redRotation;
  public float greenRotation;
  public float blueRotation;
  [Header("Offset")]
  public Vector2 redOffset = new Vector2(0.0f, 0.0f);
  public Vector2 greenOffset = new Vector2(0.0f, 0.0f);
  public Vector2 blueOffset = new Vector2(0.0f, 0.0f);
  [Header("Animation")]
  public Vector3 shakeStrengthRGB = Vector3.zero;

  private void Start() => this.material = this.GetFullScreenRGBSeparationMaterial();

  private void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (Object.op_Equality((Object) this.material, (Object) null))
    {
      Debug.Log((object) "Material is null!");
      this.material = this.GetFullScreenRGBSeparationMaterial();
    }
    else
    {
      this.material.SetFloat("_Pivot_R_X", this.redPivot.x);
      this.material.SetFloat("_Pivot_R_Y", this.redPivot.y);
      this.material.SetFloat("_Pivot_G_X", this.greenPivot.x);
      this.material.SetFloat("_Pivot_G_Y", this.greenPivot.y);
      this.material.SetFloat("_Pivot_B_X", this.bluePivot.x);
      this.material.SetFloat("_Pivot_B_Y", this.bluePivot.y);
      this.material.SetFloat("_Scale_R", this.redScale);
      this.material.SetFloat("_Scale_G", this.greenScale);
      this.material.SetFloat("_Scale_B", this.blueScale);
      this.material.SetFloat("_Rotation_Cos_R", Mathf.Cos(this.redRotation));
      this.material.SetFloat("_Rotation_Sin_R", Mathf.Sin(this.redRotation));
      this.material.SetFloat("_Rotation_Cos_G", Mathf.Cos(this.greenRotation));
      this.material.SetFloat("_Rotation_Sin_G", Mathf.Sin(this.greenRotation));
      this.material.SetFloat("_Rotation_Cos_B", Mathf.Cos(this.blueRotation));
      this.material.SetFloat("_Rotation_Sin_B", Mathf.Sin(this.blueRotation));
      this.material.SetFloat("_Offset_R_X", this.redOffset.x + Random.Range(-this.shakeStrengthRGB.x, this.shakeStrengthRGB.x));
      this.material.SetFloat("_Offset_R_Y", this.redOffset.y + Random.Range(-this.shakeStrengthRGB.x, this.shakeStrengthRGB.x));
      this.material.SetFloat("_Offset_G_X", this.greenOffset.x + Random.Range(-this.shakeStrengthRGB.y, this.shakeStrengthRGB.y));
      this.material.SetFloat("_Offset_G_Y", this.greenOffset.y + Random.Range(-this.shakeStrengthRGB.y, this.shakeStrengthRGB.y));
      this.material.SetFloat("_Offset_B_X", this.blueOffset.x + Random.Range(-this.shakeStrengthRGB.z, this.shakeStrengthRGB.z));
      this.material.SetFloat("_Offset_B_Y", this.blueOffset.y + Random.Range(-this.shakeStrengthRGB.z, this.shakeStrengthRGB.z));
      this.material.SetFloat("_Ratio", (float) Screen.height / (float) Screen.width);
      Graphics.Blit((Texture) source, destination, this.material);
    }
  }

  private Material GetFullScreenRGBSeparationMaterial()
  {
    if (Object.op_Equality((Object) Shader.Find("Custom/FullScreenRGBSeparation"), (Object) null))
    {
      Debug.Log((object) "Shader not found!");
      return (Material) null;
    }
    Material separationMaterial = new Material(Shader.Find("Custom/FullScreenRGBSeparation"));
    if (!Object.op_Equality((Object) separationMaterial, (Object) null))
      return separationMaterial;
    Debug.Log((object) "Cannot create material!");
    return separationMaterial;
  }
}
