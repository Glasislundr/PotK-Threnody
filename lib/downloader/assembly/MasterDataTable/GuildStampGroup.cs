﻿// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GuildStampGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GuildStampGroup
  {
    public int ID;
    public string name;
    public int iconID;

    public static GuildStampGroup Parse(MasterDataReader reader)
    {
      return new GuildStampGroup()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        iconID = reader.ReadInt()
      };
    }
  }
}
