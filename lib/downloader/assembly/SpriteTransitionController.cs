// Decompiled with JetBrains decompiler
// Type: SpriteTransitionController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[RequireComponent(typeof (UI2DSprite))]
public class SpriteTransitionController : MonoBehaviour
{
  private UI2DSprite sprite;
  [Header("Dissolve Mask")]
  public Color color = Color.white;
  public bool brighten;
  [Tooltip("Dissolve Mask")]
  public Texture2D mask;
  [Range(0.0f, 1f)]
  public float width;
  [Range(0.0f, 1f)]
  public float disolvePower;
  public SpriteTransitionController.EdgeEffectMode edgeEffectMode;
  private SpriteTransitionController.EdgeEffectMode edgeEffectModeLog;
  public Color edgeColor = Color.white;
  [Range(0.0f, 20f)]
  public float edgeMag = 1f;
  [Range(0.0f, 1f)]
  public float edgeWidth;
  [Header("Alpha Mask")]
  [Tooltip("Alpha Mask")]
  public Texture2D alphaMask;
  public TextureWrapMode maskWrapMode = (TextureWrapMode) 1;
  public float xScale = 1f;
  public float yScale = 1f;
  public float xOffset;
  public float yOffset;
  public bool alphaMaskFlipX;
  public bool alphaMaskFlipY;
  [Header("Sprite")]
  public TextureWrapMode spriteWrapMode = (TextureWrapMode) 1;
  public float xScaleMain = 1f;
  public float yScaleMain = 1f;
  public float xOffsetMain;
  public float yOffsetMain;
  private SpriteTransitionDrawCall drawcallExtension;
  public UIDrawCall drawCall;
  private bool useAlphaMask;

  private void Start()
  {
    this.sprite = ((Component) this).GetComponent<UI2DSprite>();
    this.SetShader();
  }

  private void SetShader()
  {
    if (Object.op_Inequality((Object) this.alphaMask, (Object) null))
    {
      if (this.edgeEffectMode == SpriteTransitionController.EdgeEffectMode.Additive)
        ((UIWidget) this.sprite).shader = Shader.Find("PUNK/UI/Sprite_Transition_mask_disolve_additive");
      else if (this.edgeEffectMode == SpriteTransitionController.EdgeEffectMode.Multiply)
        ((UIWidget) this.sprite).shader = Shader.Find("PUNK/UI/Sprite_Transition_mask_disolve_multiply");
      else if (this.alphaMaskFlipX)
        ((UIWidget) this.sprite).shader = Shader.Find("PUNK/UI/Sprite_Transition_mask (MaskFlipX)");
      else if (this.alphaMaskFlipY)
        ((UIWidget) this.sprite).shader = Shader.Find("PUNK/UI/Sprite_Transition_mask (MaskFlipY)");
      else
        ((UIWidget) this.sprite).shader = Shader.Find("PUNK/UI/Sprite_Transition_Mask");
      this.useAlphaMask = true;
    }
    else
    {
      if (this.edgeEffectMode == SpriteTransitionController.EdgeEffectMode.Additive)
        ((UIWidget) this.sprite).shader = Shader.Find("PUNK/UI/Sprite_Transition_disolve_additive");
      else if (this.edgeEffectMode == SpriteTransitionController.EdgeEffectMode.Multiply)
        ((UIWidget) this.sprite).shader = Shader.Find("PUNK/UI/Sprite_Transition_disolve_multiply");
      else
        ((UIWidget) this.sprite).shader = Shader.Find("PUNK/UI/Sprite_Transition");
      this.useAlphaMask = false;
    }
    this.edgeEffectModeLog = this.edgeEffectMode;
  }

  private void LateUpdate() => this.UpdateMaterialSettings();

  private void UpdateMaterialSettings()
  {
    if (Object.op_Equality((Object) this.sprite, (Object) null))
    {
      Debug.Log((object) "Sprite is null.");
      this.sprite = ((Component) this).GetComponent<UI2DSprite>();
    }
    if (!this.useAlphaMask && Object.op_Inequality((Object) this.alphaMask, (Object) null))
    {
      if (this.edgeEffectMode == SpriteTransitionController.EdgeEffectMode.Additive)
        ((UIWidget) this.sprite).shader = Shader.Find("PUNK/UI/Sprite_Transition_mask_disolve_additive");
      else if (this.edgeEffectMode == SpriteTransitionController.EdgeEffectMode.Multiply)
        ((UIWidget) this.sprite).shader = Shader.Find("PUNK/UI/Sprite_Transition_mask_disolve_multiply");
      else if (this.alphaMaskFlipX)
        ((UIWidget) this.sprite).shader = Shader.Find("PUNK/UI/Sprite_Transition_mask (MaskFlipX)");
      else if (this.alphaMaskFlipY)
        ((UIWidget) this.sprite).shader = Shader.Find("PUNK/UI/Sprite_Transition_mask (MaskFlipY)");
      else
        ((UIWidget) this.sprite).shader = Shader.Find("PUNK/UI/Sprite_Transition_Mask");
      this.useAlphaMask = true;
      this.SetShader();
    }
    else if (this.useAlphaMask && Object.op_Equality((Object) this.alphaMask, (Object) null))
      this.SetShader();
    else if (this.edgeEffectModeLog != this.edgeEffectMode)
      this.SetShader();
    else if (Object.op_Equality((Object) ((UIWidget) this.sprite).drawCall, (Object) null))
    {
      ((UIRect) this.sprite).Invalidate(true);
    }
    else
    {
      if (Object.op_Inequality((Object) this.drawCall, (Object) ((UIWidget) this.sprite).drawCall))
      {
        this.drawCall = ((UIWidget) this.sprite).drawCall;
        this.drawcallExtension = ((Component) ((UIWidget) this.sprite).drawCall).gameObject.GetOrAddComponent<SpriteTransitionDrawCall>();
      }
      this.drawcallExtension.sprite = this.sprite;
      this.drawcallExtension.drawcall = ((UIWidget) this.sprite).drawCall;
      ((UIWidget) this.sprite).mainTexture.wrapMode = this.spriteWrapMode;
      if (Object.op_Inequality((Object) this.alphaMask, (Object) null))
        ((Texture) this.alphaMask).wrapMode = this.maskWrapMode;
      this.drawcallExtension.color = this.color;
      this.drawcallExtension.brighten = this.brighten;
      this.drawcallExtension.mask = this.mask;
      this.drawcallExtension.width = this.width;
      this.drawcallExtension.disolvePower = this.disolvePower;
      this.drawcallExtension.edgeColor = this.edgeColor;
      this.drawcallExtension.edgeMag = this.edgeMag;
      this.drawcallExtension.edgeWidth = this.edgeWidth;
      this.drawcallExtension.alphaMask = this.alphaMask;
      this.drawcallExtension.xOffset = this.xOffset;
      this.drawcallExtension.yOffset = this.yOffset;
      this.drawcallExtension.xScale = this.xScale;
      this.drawcallExtension.yScale = this.yScale;
      this.drawcallExtension.alphaMaskFlipX = this.alphaMaskFlipX;
      this.drawcallExtension.alphaMaskFlipY = this.alphaMaskFlipY;
      this.drawcallExtension.xOffsetMain = this.xOffsetMain;
      this.drawcallExtension.yOffsetMain = this.yOffsetMain;
      this.drawcallExtension.xScaleMain = this.xScaleMain;
      this.drawcallExtension.yScaleMain = this.yScaleMain;
    }
  }

  public enum EdgeEffectMode
  {
    None,
    Additive,
    Multiply,
  }
}
