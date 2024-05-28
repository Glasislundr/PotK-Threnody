// Decompiled with JetBrains decompiler
// Type: Explore033NearMap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Explore033NearMap : MonoBehaviour
{
  [SerializeField]
  private Color MapColor = Color.white;
  [SerializeField]
  private GameObject MapObjectRoot;
  [Space(8f)]
  [SerializeField]
  private GameObject Statue;
  [SerializeField]
  private GameObject StatueSub;
  [Space(8f)]
  [SerializeField]
  private Color FogColor = Color.white;
  [SerializeField]
  private GameObject FogObjectRoot;
  private GameObject Torch;

  public void CloneAndSetStatue(GameObject prefab, GameObject prefabSub)
  {
    if (!Object.op_Inequality((Object) this.MapObjectRoot, (Object) null))
      return;
    this.Statue = prefab.Clone(this.MapObjectRoot.transform);
    this.StatueSub = prefabSub.Clone(this.MapObjectRoot.transform);
  }

  public void CloneAndSetTorch(GameObject prefab)
  {
    if (!Object.op_Inequality((Object) this.MapObjectRoot, (Object) null))
      return;
    this.Torch = prefab.Clone(this.MapObjectRoot.transform);
  }

  public void ApplyColor()
  {
    if (Object.op_Inequality((Object) this.MapObjectRoot, (Object) null))
      this.SetMaterialColor(this.MapObjectRoot, "_MainColor", this.MapColor);
    if (Object.op_Inequality((Object) this.Statue, (Object) null))
      this.SetMaterialColor(this.Statue, "_MainColor", this.MapColor);
    if (Object.op_Inequality((Object) this.StatueSub, (Object) null))
      this.SetMaterialColor(this.StatueSub, "_MainColor", this.MapColor);
    if (!Object.op_Inequality((Object) this.FogObjectRoot, (Object) null))
      return;
    this.SetMaterialColor(this.FogObjectRoot, "_MainTexColor", this.FogColor);
    RenderSettings.fogColor = this.FogColor;
  }

  private void SetMaterialColor(GameObject rootObject, string propertyName, Color color)
  {
    foreach (Component child in rootObject.transform.GetChildren())
    {
      MeshRenderer component = child.GetComponent<MeshRenderer>();
      if (Object.op_Inequality((Object) component, (Object) null))
        ((Renderer) component).sharedMaterial.SetColor(propertyName, color);
    }
  }
}
