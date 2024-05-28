// Decompiled with JetBrains decompiler
// Type: MasterDataTable.TipsLoadingBackground
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class TipsLoadingBackground
  {
    public int ID;
    public string image_name;
    public DateTime? start_at;
    public DateTime? end_at;

    public static TipsLoadingBackground Parse(MasterDataReader reader)
    {
      return new TipsLoadingBackground()
      {
        ID = reader.ReadInt(),
        image_name = reader.ReadString(true),
        start_at = reader.ReadDateTimeOrNull(),
        end_at = reader.ReadDateTimeOrNull()
      };
    }
  }
}
