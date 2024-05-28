// Decompiled with JetBrains decompiler
// Type: MasterDataTable.CallItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class CallItem
  {
    public int ID;
    public int call_item_category_id;
    public int call_item_sub_category_id;
    public int elemental_id;
    public string tips;
    public string tips_talk;

    public static CallItem Parse(MasterDataReader reader)
    {
      return new CallItem()
      {
        ID = reader.ReadInt(),
        call_item_category_id = reader.ReadInt(),
        call_item_sub_category_id = reader.ReadInt(),
        elemental_id = reader.ReadInt(),
        tips = reader.ReadStringOrNull(true),
        tips_talk = reader.ReadStringOrNull(true)
      };
    }
  }
}
