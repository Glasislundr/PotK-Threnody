// Decompiled with JetBrains decompiler
// Type: NGxMaskSpriteWithScale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (UI2DSprite))]
public class NGxMaskSpriteWithScale : MonoBehaviour
{
  [SerializeField]
  private Texture2D _maskTexture;
  public UI2DSprite MainUI2DSprite;
  public bool isMultiMaskColor;
  private Texture2D maskTextureTemp;
  private Texture2D lastMaskTexture;
  private bool enableMask = true;
  public int xOffsetPixel;
  public int yOffsetPixel;
  public float scale = 1f;
  public bool isTopFit;
  [HideInInspector]
  public float topOffset;
  private Color beforeColor = Color.white;
  [SerializeField]
  private float xOffsetPixelForAnimation;
  [SerializeField]
  private float yOffsetPixelForAnimation;
  private float beforeXOffsetPixelForAnimation;
  private float beforeYOffsetPixelForAnimation;

  public Texture2D maskTexture
  {
    get => this._maskTexture;
    set
    {
      this._maskTexture = value;
      this.maskTextureTemp = this._maskTexture;
      this.FitMask();
    }
  }

  public float spriteAlpha
  {
    get
    {
      return Object.op_Inequality((Object) this.MainUI2DSprite, (Object) null) ? ((UIWidget) this.MainUI2DSprite).color.a * ((UIRect) this.MainUI2DSprite).finalAlpha : 0.0f;
    }
  }

  public void Start()
  {
    if (!Object.op_Inequality((Object) this.MainUI2DSprite, (Object) null) || !Object.op_Inequality((Object) ((UIWidget) this.MainUI2DSprite).material, (Object) null))
      return;
    this.FitMask();
  }

  private void setMask() => this.maskTexture = this._maskTexture;

  private void Update()
  {
    if (Object.op_Equality((Object) this.MainUI2DSprite, (Object) null))
      return;
    Material material = ((UIWidget) this.MainUI2DSprite).material;
    if (Object.op_Equality((Object) material, (Object) null))
      return;
    if ((double) this.beforeXOffsetPixelForAnimation != (double) this.xOffsetPixelForAnimation || (double) this.beforeYOffsetPixelForAnimation != (double) this.yOffsetPixelForAnimation)
      this.FitMask();
    Color color = ((UIWidget) this.MainUI2DSprite).color;
    color.a *= ((UIRect) this.MainUI2DSprite).finalAlpha;
    if (!Color.op_Inequality(this.beforeColor, color))
      return;
    material.SetColor("_Color", color);
    this.beforeColor = color;
    ((UIWidget) this.MainUI2DSprite).material = (Material) null;
    ((UIWidget) this.MainUI2DSprite).material = material;
    ((UIWidget) this.MainUI2DSprite).SetDirty();
  }

  public void SetMaskEnable(bool enable)
  {
    this.enableMask = enable;
    this._maskTexture = !enable ? Resources.Load<Texture2D>("sprites/1x1_black") : this.maskTextureTemp;
    this.FitMask();
  }

  public void FitMask()
  {
    if (Object.op_Equality((Object) ((UIWidget) this.MainUI2DSprite).mainTexture, (Object) null) || Object.op_Equality((Object) this.maskTexture, (Object) null))
      return;
    if (this.enableMask)
    {
      ((UIWidget) this.MainUI2DSprite).SetDimensions(((Texture) this.maskTexture).width, ((Texture) this.maskTexture).height);
    }
    else
    {
      Texture mainTexture = ((UIWidget) this.MainUI2DSprite).mainTexture;
      ((UIWidget) this.MainUI2DSprite).SetDimensions(mainTexture.width, mainTexture.height);
    }
    float width1 = (float) ((UIWidget) this.MainUI2DSprite).width;
    float height1 = (float) ((UIWidget) this.MainUI2DSprite).height;
    float width2 = (float) ((UIWidget) this.MainUI2DSprite).mainTexture.width;
    float height2 = (float) ((UIWidget) this.MainUI2DSprite).mainTexture.height;
    double num1 = (double) height2 / (double) height1;
    float num2 = width2 / width1;
    float num3 = Mathf.Max((float) num1, num2);
    float num4 = Mathf.Min((float) num1, num2);
    float num5 = (float) num1 / num3 / num4 / this.scale;
    float num6 = num2 / num3 / num4 / this.scale;
    float num7 = ((float) this.xOffsetPixel + this.xOffsetPixelForAnimation) / width2;
    float num8 = ((float) this.yOffsetPixel + this.yOffsetPixelForAnimation) / height2;
    if (this.isTopFit)
    {
      this.topOffset = (float) (((double) height1 * (double) num4 - (double) height1) / 2.0);
      num8 += this.topOffset / height2;
    }
    else
      this.topOffset = 0.0f;
    float num9 = num7 - (float) (((double) num5 - 1.0) / 2.0);
    float num10 = num8 - (float) (((double) num6 - 1.0) / 2.0);
    string str1 = string.Empty;
    if (Object.op_Inequality((Object) ((UIWidget) this.MainUI2DSprite).panel, (Object) null))
    {
      UIDrawCall.Clipping clipping = ((UIWidget) this.MainUI2DSprite).panel.clipping;
      if (clipping != 2)
      {
        if (clipping == 3)
          str1 = " (SoftClip)";
      }
      else
        str1 = " (AlphaClip)";
    }
    string str2 = string.Format("dynamic {0}x{1} x({2}) y({3}){4}", (object) num5, (object) num6, (object) num9, (object) num10, (object) str1);
    if (!Object.op_Equality((Object) ((UIWidget) this.MainUI2DSprite).material, (Object) null) && !Object.op_Inequality((Object) this.lastMaskTexture, (Object) this.maskTexture) && !(str2 != ((Object) ((UIWidget) this.MainUI2DSprite).material).name))
      return;
    Material material = new Material(Shader.Find(string.Format("Unlit/{0}{1}", this.isMultiMaskColor ? (object) "AlphaMaskMultiColor" : (object) "AlphaMask", (object) str1)));
    ((Object) material).name = str2;
    material.SetTexture("_MaskTex", (Texture) this.maskTexture);
    material.SetFloat("_xScale", num5);
    material.SetFloat("_yScale", num6);
    material.SetFloat("_xOffset", num9);
    material.SetFloat("_yOffset", num10);
    ((UIWidget) this.MainUI2DSprite).material = material;
    ((UIWidget) this.MainUI2DSprite).mainTexture.wrapMode = (TextureWrapMode) 1;
    this.lastMaskTexture = this.maskTexture;
    this.beforeXOffsetPixelForAnimation = this.xOffsetPixelForAnimation;
    this.beforeYOffsetPixelForAnimation = this.yOffsetPixelForAnimation;
    ((UIWidget) this.MainUI2DSprite).MarkAsChanged();
  }
}
