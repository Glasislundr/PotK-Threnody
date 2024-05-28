// Decompiled with JetBrains decompiler
// Type: MissionData.IMissionAchievement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace MissionData
{
  public interface IMissionAchievement
  {
    bool isDaily { get; }

    bool isGuild { get; }

    bool isShow { get; }

    bool isCleared { get; }

    bool isOwnCleared { get; }

    bool isReceived { get; }

    object original { get; }

    int progress_count { get; }

    int own_progress_count { get; }

    IMissionReward[] rewards { get; }

    int mission_id { get; }

    IMission mission { get; }
  }
}
