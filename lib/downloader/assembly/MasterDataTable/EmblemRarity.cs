﻿// Decompiled with JetBrains decompiler
// Type: MasterDataTable.EmblemRarity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class EmblemRarity
  {
    public int ID;
    public string name;
    public int index;

    public static EmblemRarity Parse(MasterDataReader reader)
    {
      return new EmblemRarity()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        index = reader.ReadInt()
      };
    }
  }
}
