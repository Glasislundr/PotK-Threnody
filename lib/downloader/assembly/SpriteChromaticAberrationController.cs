// Decompiled with JetBrains decompiler
// Type: SpriteChromaticAberrationController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[RequireComponent(typeof (UI2DSprite))]
public class SpriteChromaticAberrationController : MonoBehaviour
{
  private UI2DSprite sprite;
  private SpriteChromaticAberrationDrawCall drawcallExtension;
  public UIDrawCall drawCall;
  public bool isAdditiveMode;
  private Shader spriteChromaticAberrationShader;
  private Shader spriteChromaticAberrationAdditiveShader;
  private bool isShaderChanged;
  public Color color = Color.white;
  [Range(-0.1f, 0.1f)]
  public float aberrationPower = 0.02395844f;
  public float wiggle = 0.4f;
  [Range(0.0f, 1f)]
  public float cutoff = 0.5f;

  private void Start()
  {
    this.spriteChromaticAberrationShader = Shader.Find("PUNK/UI/Sprite_Chromatic Aberration2");
    this.spriteChromaticAberrationAdditiveShader = Shader.Find("PUNK/UI/Sprite_Chromatic Aberration_Additive2");
    this.sprite = ((Component) this).GetComponent<UI2DSprite>();
    this.UpdateShader();
  }

  private bool UpdateShader()
  {
    if (!this.isAdditiveMode && Object.op_Inequality((Object) ((UIWidget) this.sprite).shader, (Object) this.spriteChromaticAberrationShader))
    {
      ((UIWidget) this.sprite).shader = this.spriteChromaticAberrationShader;
      return true;
    }
    if (!this.isAdditiveMode || !Object.op_Inequality((Object) ((UIWidget) this.sprite).shader, (Object) this.spriteChromaticAberrationAdditiveShader))
      return false;
    ((UIWidget) this.sprite).shader = this.spriteChromaticAberrationAdditiveShader;
    return true;
  }

  private void LateUpdate() => this.UpdateMaterialSettings();

  private void UpdateMaterialSettings()
  {
    if (Object.op_Equality((Object) ((UIWidget) this.sprite).drawCall, (Object) null))
    {
      ((UIRect) this.sprite).Invalidate(true);
    }
    else
    {
      this.isShaderChanged = this.UpdateShader();
      if (this.isShaderChanged)
        return;
      if (Object.op_Inequality((Object) this.drawCall, (Object) ((UIWidget) this.sprite).drawCall))
      {
        this.drawCall = ((UIWidget) this.sprite).drawCall;
        this.drawcallExtension = ((Component) ((UIWidget) this.sprite).drawCall).gameObject.GetOrAddComponent<SpriteChromaticAberrationDrawCall>();
      }
      this.drawcallExtension.sprite = this.sprite;
      this.drawcallExtension.drawcall = ((UIWidget) this.sprite).drawCall;
      this.drawcallExtension.color = this.color;
      this.drawcallExtension.aberrationPower = this.aberrationPower;
      this.drawcallExtension.wiggle = this.wiggle;
      this.drawcallExtension.cutoff = this.cutoff;
    }
  }
}
