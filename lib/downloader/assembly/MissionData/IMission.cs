// Decompiled with JetBrains decompiler
// Type: MissionData.IMission
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;

#nullable disable
namespace MissionData
{
  public interface IMission
  {
    bool isDaily { get; }

    bool isGuild { get; }

    bool isShow { get; }

    MissionType missionType { get; }

    object original { get; }

    int ID { get; }

    int priority { get; }

    int progress_max { get; }

    int condition { get; }

    int own_progress_max { get; }

    string name { get; }

    string detail { get; }

    string scene { get; }

    int point { get; }
  }
}
