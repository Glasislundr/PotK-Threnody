// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GearExpireDate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GearExpireDate
  {
    public int ID;
    public DateTime end_at;

    public static GearExpireDate Parse(MasterDataReader reader)
    {
      return new GearExpireDate()
      {
        ID = reader.ReadInt(),
        end_at = reader.ReadDateTime()
      };
    }
  }
}
