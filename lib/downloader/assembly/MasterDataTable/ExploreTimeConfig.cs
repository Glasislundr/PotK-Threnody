// Decompiled with JetBrains decompiler
// Type: MasterDataTable.ExploreTimeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class ExploreTimeConfig
  {
    public int ID;
    public string key;
    public int milli_sec;

    public static ExploreTimeConfig Parse(MasterDataReader reader)
    {
      return new ExploreTimeConfig()
      {
        ID = reader.ReadInt(),
        key = reader.ReadString(true),
        milli_sec = reader.ReadInt()
      };
    }
  }
}
