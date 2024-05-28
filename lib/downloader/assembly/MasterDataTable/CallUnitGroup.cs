// Decompiled with JetBrains decompiler
// Type: MasterDataTable.CallUnitGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class CallUnitGroup
  {
    public int ID;
    public int large_category_id;
    public int small_category_id;
    public int clothing_category_id;
    public int generation_category_id;
    public int group_type;
    public int dislike_group_id;

    public static CallUnitGroup Parse(MasterDataReader reader)
    {
      return new CallUnitGroup()
      {
        ID = reader.ReadInt(),
        large_category_id = reader.ReadInt(),
        small_category_id = reader.ReadInt(),
        clothing_category_id = reader.ReadInt(),
        generation_category_id = reader.ReadInt(),
        group_type = reader.ReadInt(),
        dislike_group_id = reader.ReadInt()
      };
    }
  }
}
