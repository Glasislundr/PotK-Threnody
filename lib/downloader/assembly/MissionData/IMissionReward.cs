// Decompiled with JetBrains decompiler
// Type: MissionData.IMissionReward
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace MissionData
{
  public interface IMissionReward
  {
    bool isDaily { get; }

    bool isGuild { get; }

    object original { get; }

    int reward_quantity { get; }

    string reward_message { get; }

    int reward_type_id { get; }

    int reward_id { get; }
  }
}
