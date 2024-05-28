// Decompiled with JetBrains decompiler
// Type: GooglePlayGames.BasicApi.SavedGame.SelectUIStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace GooglePlayGames.BasicApi.SavedGame
{
  public enum SelectUIStatus
  {
    BadInputError = -4, // 0xFFFFFFFC
    AuthenticationError = -3, // 0xFFFFFFFD
    TimeoutError = -2, // 0xFFFFFFFE
    InternalError = -1, // 0xFFFFFFFF
    SavedGameSelected = 1,
    UserClosedUI = 2,
  }
}
