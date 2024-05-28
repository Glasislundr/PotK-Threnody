// Decompiled with JetBrains decompiler
// Type: com.adjust.sdk.AdjustEnvironmentExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace com.adjust.sdk
{
  public static class AdjustEnvironmentExtension
  {
    public static string ToLowercaseString(this AdjustEnvironment adjustEnvironment)
    {
      if (adjustEnvironment == AdjustEnvironment.Sandbox)
        return "sandbox";
      return adjustEnvironment == AdjustEnvironment.Production ? "production" : "unknown";
    }
  }
}
