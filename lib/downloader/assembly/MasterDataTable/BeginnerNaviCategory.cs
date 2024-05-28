// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BeginnerNaviCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BeginnerNaviCategory
  {
    public int ID;
    public string name;
    public int priority;

    public static BeginnerNaviCategory Parse(MasterDataReader reader)
    {
      return new BeginnerNaviCategory()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        priority = reader.ReadInt()
      };
    }
  }
}
