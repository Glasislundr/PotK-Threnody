// Decompiled with JetBrains decompiler
// Type: MasterDataTable.MaterialXLevelExp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class MaterialXLevelExp
  {
    public int ID;
    public int point;

    public static MaterialXLevelExp Parse(MasterDataReader reader)
    {
      return new MaterialXLevelExp()
      {
        ID = reader.ReadInt(),
        point = reader.ReadInt()
      };
    }
  }
}
