// Decompiled with JetBrains decompiler
// Type: Scroll0079Arg
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Purchase;
using MasterDataTable;
using SM;
using System;

#nullable disable
public class Scroll0079Arg
{
  public ProductInfo productInfo;
  public CoinProduct coinProduct;
  public CoinBonus coinBonus;
  public CoinBonusReward coinbonusReward;
  public PlayerCoinBonusInfo playerCoinBonusInfo;
  public WebAPI.Response.CoinbonusHistoryCoin_bonus_details coinbonusHistoryCoinBonusDetails;
  public SimplePackInfo simplePackInfo;
  public BeginnerPackInfo beginnerPackInfo;
  public DateTime now;
  public int sortIndex;

  public Scroll0079Arg(
    ProductInfo productInfo,
    CoinProduct coinProduct,
    CoinBonus coinBonus,
    CoinBonusReward coinbonusReward,
    PlayerCoinBonusInfo playerCoinBonusInfo,
    WebAPI.Response.CoinbonusHistoryCoin_bonus_details coinbonusHistoryCoinBonusDetails,
    SimplePackInfo simplePackInfo,
    BeginnerPackInfo beginnerPackInfo,
    DateTime now,
    int sortIndex)
  {
    this.productInfo = productInfo;
    this.coinProduct = coinProduct;
    this.coinBonus = coinBonus;
    this.coinbonusReward = coinbonusReward;
    this.playerCoinBonusInfo = playerCoinBonusInfo;
    this.coinbonusHistoryCoinBonusDetails = coinbonusHistoryCoinBonusDetails;
    this.simplePackInfo = simplePackInfo;
    this.beginnerPackInfo = beginnerPackInfo;
    this.now = now;
    this.sortIndex = sortIndex;
  }
}
