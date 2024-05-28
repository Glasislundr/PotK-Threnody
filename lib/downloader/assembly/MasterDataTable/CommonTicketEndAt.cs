// Decompiled with JetBrains decompiler
// Type: MasterDataTable.CommonTicketEndAt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class CommonTicketEndAt
  {
    public int ID;
    public DateTime end_at;

    public static CommonTicketEndAt Parse(MasterDataReader reader)
    {
      return new CommonTicketEndAt()
      {
        ID = reader.ReadInt(),
        end_at = reader.ReadDateTime()
      };
    }
  }
}
