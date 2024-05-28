// Decompiled with JetBrains decompiler
// Type: WeaponTrail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using UnityEngine;

#nullable disable
public class WeaponTrail : MonoBehaviour
{
  private WeaponTrailMeshRender meshRender;
  public Color MeshDefaultColor = new Color(0.419607848f, 0.6f, 0.627451f, 0.8235294f);
  public int MeshSplitCount = 512;
  public float MeshHeight = 1.1f;
  public float CurveVolume = 0.5f;
  public Transform CaptureTarget;
  public Material WeaponTrailMaterial;

  public void On(Transform parent = null, NGDuelUnit unit = null)
  {
    if (Object.op_Inequality((Object) this.meshRender, (Object) null))
      return;
    Color meshDefaultColor = this.MeshDefaultColor;
    if (Object.op_Inequality((Object) unit, (Object) null))
    {
      DuelElementTrailEffect elementTrailEffect = unit.GetElementTrailEffect();
      if (elementTrailEffect != null)
      {
        if (!string.IsNullOrEmpty(elementTrailEffect.trail_effect_name))
          Singleton<NGDuelDataManager>.GetInstance().GetPreloadDuelEffect(elementTrailEffect.trail_effect_name, ((Component) this).transform.parent);
        if (elementTrailEffect.trail_color_r.HasValue && elementTrailEffect.trail_color_g.HasValue && elementTrailEffect.trail_color_b.HasValue && elementTrailEffect.trail_color_a.HasValue)
        {
          // ISSUE: explicit constructor call
          ((Color) ref meshDefaultColor).\u002Ector((float) elementTrailEffect.trail_color_r.Value / (float) byte.MaxValue, (float) elementTrailEffect.trail_color_g.Value / (float) byte.MaxValue, (float) elementTrailEffect.trail_color_b.Value / (float) byte.MaxValue, (float) elementTrailEffect.trail_color_a.Value / (float) byte.MaxValue);
        }
      }
    }
    GameObject gameObject = new GameObject();
    gameObject.layer = ((Component) this).gameObject.layer;
    ((Object) gameObject).name = "DynamicMeshRenderForWeaponTrail";
    gameObject.transform.parent = parent;
    this.meshRender = gameObject.AddComponent<WeaponTrailMeshRender>();
    this.meshRender.MeshCurrentColor = meshDefaultColor;
    this.meshRender.MeshSplitCount = this.MeshSplitCount;
    this.meshRender.MeshHeight = this.MeshHeight;
    this.meshRender.CurveVolume = this.CurveVolume;
    this.meshRender.CaptureTarget = this.CaptureTarget;
    this.meshRender.WeaponTrailMaterial = this.WeaponTrailMaterial;
    this.meshRender.On();
  }

  public void Off()
  {
    if (Object.op_Equality((Object) this.meshRender, (Object) null))
      return;
    this.meshRender.Off();
  }
}
