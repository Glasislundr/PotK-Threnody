// Decompiled with JetBrains decompiler
// Type: ScreenUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using UnityEngine;

#nullable disable
public static class ScreenUtil
{
  private const int STANDARD_SCREEN_HEIGHT = 1136;
  private static Vector2Int initResolution;

  private static void StoreInitResolution()
  {
    if (((Vector2Int) ref ScreenUtil.initResolution).x > 0 && ((Vector2Int) ref ScreenUtil.initResolution).y > 0)
      return;
    Resolution currentResolution1 = Screen.currentResolution;
    int width = ((Resolution) ref currentResolution1).width;
    Resolution currentResolution2 = Screen.currentResolution;
    int height = ((Resolution) ref currentResolution2).height;
    ScreenUtil.initResolution = new Vector2Int(width, height);
  }

  public static void RefreshPerformanceResolution()
  {
    if (PerformanceConfig.GetInstance().IsSpeedPriority)
      ScreenUtil.SetStandardResolution();
    else
      ScreenUtil.SetDefaultResolution();
  }

  public static void SetResolution(int width, int height)
  {
  }

  public static void SetDefaultResolution()
  {
    ScreenUtil.StoreInitResolution();
    ScreenUtil.SetResolution(((Vector2Int) ref ScreenUtil.initResolution).x, ((Vector2Int) ref ScreenUtil.initResolution).y);
  }

  public static bool IsStandardResolutionOrLess()
  {
    Resolution currentResolution = Screen.currentResolution;
    return ((Resolution) ref currentResolution).height <= 1136;
  }

  public static void SetStandardResolution()
  {
    ScreenUtil.StoreInitResolution();
    if (((Vector2Int) ref ScreenUtil.initResolution).y <= 1136)
      return;
    float num = 1136f / (float) ((Vector2Int) ref ScreenUtil.initResolution).y;
    ScreenUtil.SetResolution((int) ((double) ((Vector2Int) ref ScreenUtil.initResolution).x * (double) num), 1136);
  }

  public static void SetResolution_2_3()
  {
    ScreenUtil.StoreInitResolution();
    ScreenUtil.SetResolution(((Vector2Int) ref ScreenUtil.initResolution).x * 2 / 3, ((Vector2Int) ref ScreenUtil.initResolution).y * 2 / 3);
  }

  public static void SetResolution_1_2()
  {
    ScreenUtil.StoreInitResolution();
    ScreenUtil.SetResolution(((Vector2Int) ref ScreenUtil.initResolution).x / 2, ((Vector2Int) ref ScreenUtil.initResolution).y / 2);
  }

  public static void SetResolution_1_3()
  {
    ScreenUtil.StoreInitResolution();
    ScreenUtil.SetResolution(((Vector2Int) ref ScreenUtil.initResolution).x / 3, ((Vector2Int) ref ScreenUtil.initResolution).y / 3);
  }

  public static void SetResolution_1_4()
  {
    ScreenUtil.StoreInitResolution();
    ScreenUtil.SetResolution(((Vector2Int) ref ScreenUtil.initResolution).x / 4, ((Vector2Int) ref ScreenUtil.initResolution).y / 4);
  }
}
