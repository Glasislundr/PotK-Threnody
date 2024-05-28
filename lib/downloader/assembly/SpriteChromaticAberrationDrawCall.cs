// Decompiled with JetBrains decompiler
// Type: SpriteChromaticAberrationDrawCall
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class SpriteChromaticAberrationDrawCall : MonoBehaviour
{
  public UI2DSprite sprite;
  public UIDrawCall drawcall;
  public Color color = Color.white;
  [Range(-0.1f, 0.1f)]
  public float aberrationPower = 0.02395844f;
  public float wiggle = 0.4f;
  [Range(0.0f, 1f)]
  public float cutoff = 0.5f;

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
    {
      Debug.Log((object) "Material is null.");
    }
    else
    {
      this.drawcall.dynamicMaterial.SetColor("_Color", this.color);
      this.drawcall.dynamicMaterial.SetFloat("_AberrationPower", this.aberrationPower);
      this.drawcall.dynamicMaterial.SetFloat("_Wiggle", this.wiggle);
      this.drawcall.dynamicMaterial.SetFloat("_Cutoff", this.cutoff);
    }
  }
}
