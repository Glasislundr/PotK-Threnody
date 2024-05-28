// Decompiled with JetBrains decompiler
// Type: GodAndBlackEffectManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class GodAndBlackEffectManager : MonoBehaviour
{
  [Tooltip("The original texture.")]
  public Texture2D inputTexture;
  [Tooltip("(X, Y) is the lower left point of clip region, which starts from the lower left point of the input texture. ※The ratio of the clip region shoud be the same as 720 : 1136.")]
  public Rect clipRegion;
  [Tooltip("The main color of the image. ※The alpha channel is not used.")]
  public Color mainColor;
  [Tooltip("How close is the current color to the main color. 0: totally use the current color, 1: totally use the main color. ※The alpha channel is not used.")]
  [Range(0.0f, 1f)]
  public float mainColorStrength;
  [Tooltip("result_color = current_color * brightness. ※The alpha channel is not changed.")]
  [Range(0.0f, 3f)]
  public float brightness;
  [Tooltip("0: totally grey, 1: something like overexposed. ※The alpha channel is not changed.")]
  [Range(0.0f, 3f)]
  public float colorfulness;
  [Tooltip("How much the r, g, and b channels' value are close to each other. ※The alpha channel is not changed.")]
  [Range(0.0f, 3f)]
  public float contrast;
  [Tooltip("How much the image is close to white. 0: totally show current image, 1: totally show white image. ※The alpha channel is not changed.")]
  [Range(0.0f, 1f)]
  public float whiteStrength;
  [Tooltip("The strength of blur effect. 0: totally show clear image, 1: totally show the blured image. ※The alpha channel is not changed.")]
  [Range(0.0f, 1f)]
  public float blurStrength;
  public UITexture outputTexture;
  public UITexture originalTexture;

  private void Start()
  {
  }

  public void AdjustImageColor()
  {
    ((UIWidget) this.originalTexture).mainTexture = (Texture) this.inputTexture;
    UIWidget component1 = ((Component) this.originalTexture).GetComponent<UIWidget>();
    component1.width = ((Texture) this.inputTexture).width;
    component1.height = ((Texture) this.inputTexture).height;
    ((UIWidget) this.outputTexture).mainTexture = (Texture) GodAndBlackEffectManager.GetBlackOrGodTexture(this.inputTexture, this.clipRegion, this.brightness, this.colorfulness, this.contrast, this.blurStrength, this.whiteStrength, this.mainColor, this.mainColorStrength);
    UIWidget component2 = ((Component) this.outputTexture).GetComponent<UIWidget>();
    component2.width = ((UIWidget) this.outputTexture).width;
    component2.height = ((UIWidget) this.outputTexture).height;
  }

  private void Update()
  {
  }

  private void OnGUI()
  {
    if (!GUI.Button(new Rect((float) (Screen.width / 3), (float) ((double) Screen.height * 2.5 / 15.0), (float) (Screen.width / 3), (float) (Screen.height / 15)), "Adjust Image Color"))
      return;
    this.AdjustImageColor();
  }

  public static Texture2D GetBlackOrGodTexture(
    Texture2D inputTexture,
    Rect clipRegion,
    float brightness,
    float colorfulness,
    float contrast,
    float blurStrength,
    float whiteStrength,
    Color mainColor,
    float mainColorStrength)
  {
    DateTime now = DateTime.Now;
    int x = (int) ((Rect) ref clipRegion).x;
    int y = (int) ((Rect) ref clipRegion).y;
    int width = (int) ((Rect) ref clipRegion).width;
    int height = (int) ((Rect) ref clipRegion).height;
    Color[] pixels = GlowingBorderEffectManager.DuplicateTexture(inputTexture).GetPixels(x, y, width, height);
    Color[] colorArray1 = new Color[pixels.Length];
    for (int index = 0; index < pixels.Length; ++index)
    {
      Color color1 = pixels[index];
      color1.r = Mathf.Lerp(color1.r, mainColor.r, mainColorStrength);
      color1.g = Mathf.Lerp(color1.g, mainColor.g, mainColorStrength);
      color1.b = Mathf.Lerp(color1.b, mainColor.b, mainColorStrength);
      color1.r *= brightness;
      color1.g *= brightness;
      color1.b *= brightness;
      float num = (float) ((double) color1.r * 0.21250000596046448 + (double) color1.g * 0.715399980545044 + (double) color1.b * 0.072099998593330383);
      color1.r = Mathf.LerpUnclamped(num, color1.r, colorfulness);
      color1.g = Mathf.LerpUnclamped(num, color1.g, colorfulness);
      color1.b = Mathf.LerpUnclamped(num, color1.b, colorfulness);
      Color color2;
      // ISSUE: explicit constructor call
      ((Color) ref color2).\u002Ector(0.5f, 0.5f, 0.5f);
      color1.r = Mathf.LerpUnclamped(color2.r, color1.r, contrast);
      color1.g = Mathf.LerpUnclamped(color2.g, color1.g, contrast);
      color1.b = Mathf.LerpUnclamped(color2.b, color1.b, contrast);
      color1.r = Mathf.Lerp(color1.r, 1f, whiteStrength);
      color1.g = Mathf.Lerp(color1.g, 1f, whiteStrength);
      color1.b = Mathf.Lerp(color1.b, 1f, whiteStrength);
      colorArray1[index] = color1;
    }
    int num1 = 1;
    float[] numArray = new float[9]
    {
      0.087133f,
      0.120917f,
      0.087133f,
      0.120917f,
      0.167799f,
      0.120917f,
      0.087133f,
      0.120917f,
      0.087133f
    };
    Color[] colorArray2 = new Color[pixels.Length];
    for (int index1 = 0; index1 < width; ++index1)
    {
      for (int index2 = 0; index2 < height; ++index2)
      {
        int index3 = index1 + index2 * width;
        Color color3 = Color.clear;
        for (int index4 = index1 - num1; index4 <= index1 + num1; ++index4)
        {
          for (int index5 = index2 - num1; index5 <= index2 + num1; ++index5)
          {
            if (0 <= index4 && index4 < width && 0 <= index5 && index5 < height)
            {
              int index6 = index4 + index5 * width;
              int index7 = index4 - (index1 - num1) + (index5 - (index2 - num1)) * (2 * num1 + 1);
              color3 = Color.op_Addition(color3, Color.op_Multiply(colorArray1[index6], numArray[index7]));
            }
          }
        }
        Color color4 = colorArray1[index3];
        color3.r = Mathf.Lerp(color4.r, color3.r, blurStrength);
        color3.g = Mathf.Lerp(color4.g, color3.g, blurStrength);
        color3.b = Mathf.Lerp(color4.b, color3.b, blurStrength);
        color3.a = colorArray1[index3].a;
        colorArray2[index3] = color3;
      }
    }
    Texture2D blackOrGodTexture = new Texture2D((int) ((Rect) ref clipRegion).width, (int) ((Rect) ref clipRegion).height);
    blackOrGodTexture.SetPixels(colorArray2);
    blackOrGodTexture.Apply();
    return blackOrGodTexture;
  }

  public static Color GetColorFromColorTemperature(float kelvin)
  {
    float num1 = kelvin / 100f;
    float num2;
    float num3;
    float num4;
    if ((double) num1 <= 66.0)
    {
      num2 = (float) byte.MaxValue;
      num3 = (float) (99.4708023071289 * (double) Mathf.Log(num1) - 161.11956787109375);
      num4 = (double) num1 > 19.0 ? (float) (138.51773071289063 * (double) Mathf.Log(num1 - 10f) - 305.0447998046875) : 0.0f;
    }
    else
    {
      num2 = 329.69873f * Mathf.Pow(num1 - 60f, -0.133204758f);
      num3 = 288.122162f * Mathf.Pow(num1 - 60f, -0.0755148456f);
      num4 = (float) byte.MaxValue;
    }
    return new Color(Mathf.Clamp01(num2 / (float) byte.MaxValue), Mathf.Clamp01(num3 / (float) byte.MaxValue), Mathf.Clamp01(num4 / (float) byte.MaxValue));
  }
}
