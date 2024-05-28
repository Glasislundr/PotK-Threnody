// Decompiled with JetBrains decompiler
// Type: GooglePlayGames.BasicApi.ResponseStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace GooglePlayGames.BasicApi
{
  public enum ResponseStatus
  {
    Timeout = -5, // 0xFFFFFFFB
    VersionUpdateRequired = -4, // 0xFFFFFFFC
    NotAuthorized = -3, // 0xFFFFFFFD
    InternalError = -2, // 0xFFFFFFFE
    LicenseCheckFailed = -1, // 0xFFFFFFFF
    Success = 1,
    SuccessWithStale = 2,
  }
}
