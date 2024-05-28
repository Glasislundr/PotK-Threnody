// Decompiled with JetBrains decompiler
// Type: SM_CoinBonusRewardExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Auth;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public static class SM_CoinBonusRewardExtension
{
  public static IEnumerable<CoinBonusReward> GetActiveList(this CoinBonusReward[] self, string id)
  {
    IEnumerable<CoinBonusReward> source = ((IEnumerable<CoinBonusReward>) self).Where<CoinBonusReward>((Func<CoinBonusReward, bool>) (x => x.coin_product_id.platform == Device.Platform));
    if (source.Count<CoinBonusReward>() == 0)
      source = ((IEnumerable<CoinBonusReward>) self).Where<CoinBonusReward>((Func<CoinBonusReward, bool>) (x => x.coin_product_id.platform == "windows"));
    return source.Where<CoinBonusReward>((Func<CoinBonusReward, bool>) (x => x.coin_product_id.product_id == id));
  }
}
