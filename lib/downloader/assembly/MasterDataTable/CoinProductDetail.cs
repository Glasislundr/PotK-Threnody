// Decompiled with JetBrains decompiler
// Type: MasterDataTable.CoinProductDetail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class CoinProductDetail
  {
    public int ID;
    public int group_no;
    public string detail;
    public string image_url;
    public int? image_width;
    public int? image_height;

    public static CoinProductDetail Parse(MasterDataReader reader)
    {
      return new CoinProductDetail()
      {
        ID = reader.ReadInt(),
        group_no = reader.ReadInt(),
        detail = reader.ReadStringOrNull(true),
        image_url = reader.ReadStringOrNull(true),
        image_width = reader.ReadIntOrNull(),
        image_height = reader.ReadIntOrNull()
      };
    }
  }
}
