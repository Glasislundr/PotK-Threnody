// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GuildStamp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GuildStamp
  {
    public int ID;
    public int groupID_GuildStampGroup;

    public static GuildStamp Parse(MasterDataReader reader)
    {
      return new GuildStamp()
      {
        ID = reader.ReadInt(),
        groupID_GuildStampGroup = reader.ReadInt()
      };
    }

    public GuildStampGroup groupID
    {
      get
      {
        GuildStampGroup groupId;
        if (!MasterData.GuildStampGroup.TryGetValue(this.groupID_GuildStampGroup, out groupId))
          Debug.LogError((object) ("Key not Found: MasterData.GuildStampGroup[" + (object) this.groupID_GuildStampGroup + "]"));
        return groupId;
      }
    }
  }
}
