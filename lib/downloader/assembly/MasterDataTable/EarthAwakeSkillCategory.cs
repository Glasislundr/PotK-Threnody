﻿// Decompiled with JetBrains decompiler
// Type: MasterDataTable.EarthAwakeSkillCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class EarthAwakeSkillCategory
  {
    public int ID;
    public string name;
    public string description;
    public DateTime? start_at;

    public static EarthAwakeSkillCategory Parse(MasterDataReader reader)
    {
      return new EarthAwakeSkillCategory()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        description = reader.ReadString(true),
        start_at = reader.ReadDateTimeOrNull()
      };
    }
  }
}
