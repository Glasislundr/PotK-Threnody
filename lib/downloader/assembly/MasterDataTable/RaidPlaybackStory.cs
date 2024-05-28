// Decompiled with JetBrains decompiler
// Type: MasterDataTable.RaidPlaybackStory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class RaidPlaybackStory
  {
    public int ID;
    public string name;
    public int period_id;
    public int priority;
    public DateTime? display_expire_at;

    public static RaidPlaybackStory Parse(MasterDataReader reader)
    {
      return new RaidPlaybackStory()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        period_id = reader.ReadInt(),
        priority = reader.ReadInt(),
        display_expire_at = reader.ReadDateTimeOrNull()
      };
    }
  }
}
