// Decompiled with JetBrains decompiler
// Type: MaterialController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MaterialController : BattleMonoBehaviour
{
  private Shader grayScaleShader;

  public void Initialize() => this.grayScaleShader = Shader.Find("Custom/LegacyDiffuseGrayScale");

  public Material[] CreateGrayScaleMaterial(Renderer render)
  {
    if (Object.op_Equality((Object) this.grayScaleShader, (Object) null))
      this.Initialize();
    Material[] materials = render.materials;
    Material[] grayScaleMaterial = new Material[materials.Length];
    for (int index = 0; index < materials.Length; ++index)
    {
      Material material = new Material(materials[index]);
      material.shader = this.grayScaleShader;
      material.color = new Color(0.5529412f, 0.5529412f, 0.5529412f);
      material.EnableKeyword("_APPLY_GRAYSCALE_ON");
      grayScaleMaterial[index] = material;
    }
    return grayScaleMaterial;
  }

  public Material[] CreateGrayScaleMaterial(Material[] targetMaterials)
  {
    if (Object.op_Equality((Object) this.grayScaleShader, (Object) null))
      this.Initialize();
    Material[] materialArray = targetMaterials;
    Material[] grayScaleMaterial = new Material[materialArray.Length];
    for (int index = 0; index < materialArray.Length; ++index)
    {
      Material material = new Material(materialArray[index]);
      material.shader = this.grayScaleShader;
      material.color = new Color(0.5529412f, 0.5529412f, 0.5529412f);
      material.EnableKeyword("_APPLY_GRAYSCALE_ON");
      grayScaleMaterial[index] = material;
    }
    return grayScaleMaterial;
  }
}
