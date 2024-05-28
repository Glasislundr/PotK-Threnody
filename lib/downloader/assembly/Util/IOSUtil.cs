// Decompiled with JetBrains decompiler
// Type: Util.IOSUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Util
{
  public static class IOSUtil
  {
    private static bool? isDeviceGenerationiPhoneX;
    private static bool? isDeviceGenerationiPad;
    private static Rect? safeArea;

    public static bool IsDeviceGenerationiPhoneX
    {
      get
      {
        if (IOSUtil.isDeviceGenerationiPhoneX.HasValue)
          return IOSUtil.isDeviceGenerationiPhoneX.Value;
        IOSUtil.isDeviceGenerationiPhoneX = new bool?(UIRoot.IsSafeArea());
        return IOSUtil.isDeviceGenerationiPhoneX.Value;
      }
    }

    public static bool IsDeviceGenerationiPad
    {
      get
      {
        if (IOSUtil.isDeviceGenerationiPad.HasValue)
          return IOSUtil.isDeviceGenerationiPad.Value;
        IOSUtil.isDeviceGenerationiPad = new bool?((double) ((float) Screen.height / (float) Screen.width) < 1.3400000333786011);
        return IOSUtil.isDeviceGenerationiPad.Value;
      }
    }

    public static Rect SafeArea
    {
      get
      {
        if (IOSUtil.safeArea.HasValue)
          return IOSUtil.safeArea.Value;
        IOSUtil.safeArea = new Rect?(Screen.safeArea);
        return IOSUtil.safeArea.Value;
      }
    }
  }
}
