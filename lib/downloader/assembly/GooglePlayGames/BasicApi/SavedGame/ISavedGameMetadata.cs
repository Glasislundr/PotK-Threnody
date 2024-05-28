// Decompiled with JetBrains decompiler
// Type: GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace GooglePlayGames.BasicApi.SavedGame
{
  public interface ISavedGameMetadata
  {
    bool IsOpen { get; }

    string Filename { get; }

    string Description { get; }

    string CoverImageURL { get; }

    TimeSpan TotalTimePlayed { get; }

    DateTime LastModifiedTimestamp { get; }
  }
}
