// Decompiled with JetBrains decompiler
// Type: MasterDataTable.CoinChargeLimit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class CoinChargeLimit
  {
    public int ID;
    public int age_limit;
    public string currency;
    public float charge_limit_per_month;

    public static CoinChargeLimit Parse(MasterDataReader reader)
    {
      return new CoinChargeLimit()
      {
        ID = reader.ReadInt(),
        age_limit = reader.ReadInt(),
        currency = reader.ReadString(true),
        charge_limit_per_month = reader.ReadFloat()
      };
    }
  }
}
