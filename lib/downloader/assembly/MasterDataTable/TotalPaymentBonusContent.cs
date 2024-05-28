// Decompiled with JetBrains decompiler
// Type: MasterDataTable.TotalPaymentBonusContent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class TotalPaymentBonusContent
  {
    public int ID;
    public string reward_id;

    public static TotalPaymentBonusContent Parse(MasterDataReader reader)
    {
      return new TotalPaymentBonusContent()
      {
        ID = reader.ReadInt(),
        reward_id = reader.ReadString(true)
      };
    }
  }
}
