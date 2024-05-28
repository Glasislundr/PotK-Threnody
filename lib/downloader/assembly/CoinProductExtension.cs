// Decompiled with JetBrains decompiler
// Type: CoinProductExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Auth;
using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public static class CoinProductExtension
{
  public static CoinProduct GetActiveProductData(this CoinProduct[] self, string productId)
  {
    string platform = Device.Platform;
    IEnumerable<CoinProduct> source = ((IEnumerable<CoinProduct>) self).Where<CoinProduct>((Func<CoinProduct, bool>) (x => x.platform == platform));
    if (source.Count<CoinProduct>() == 0)
      source = ((IEnumerable<CoinProduct>) self).Where<CoinProduct>((Func<CoinProduct, bool>) (x => x.platform == "windows"));
    return source.Where<CoinProduct>((Func<CoinProduct, bool>) (x => x.product_id == productId)).First<CoinProduct>();
  }
}
