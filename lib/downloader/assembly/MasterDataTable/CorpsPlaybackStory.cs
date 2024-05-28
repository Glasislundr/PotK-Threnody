// Decompiled with JetBrains decompiler
// Type: MasterDataTable.CorpsPlaybackStory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class CorpsPlaybackStory
  {
    public int ID;
    public string name;
    public int stage_id;
    public int priority;
    public int banner_id;

    public static CorpsPlaybackStory Parse(MasterDataReader reader)
    {
      return new CorpsPlaybackStory()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        stage_id = reader.ReadInt(),
        priority = reader.ReadInt(),
        banner_id = reader.ReadInt()
      };
    }
  }
}
