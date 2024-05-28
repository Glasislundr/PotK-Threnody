// Decompiled with JetBrains decompiler
// Type: ShopItemIconInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;

#nullable disable
public class ShopItemIconInfo
{
  public ShopItemIcon shopItemIcon;
  public string ItemName;
  public int? LimitType;
  public int? LimitCount;
  public CommonPayType PayType;
  public int PayCount;
  public MasterDataTable.CommonRewardType CommonRewardType;
  public int RewardId;
  public PlayerShopArticle playerShopArticle;

  public bool IsPack => this.playerShopArticle.contents.Length != 1;

  public ShopItemIconInfo(PlayerShopArticle playerShopArticle)
  {
    this.playerShopArticle = playerShopArticle;
    this.ItemName = playerShopArticle.article_name;
    if (playerShopArticle.icon_resource != "")
    {
      this.CommonRewardType = (MasterDataTable.CommonRewardType) 0;
      this.RewardId = 0;
    }
    else
    {
      this.CommonRewardType = (MasterDataTable.CommonRewardType) playerShopArticle.contents[0].reward_type_id;
      this.RewardId = playerShopArticle.contents[0].reward_id;
    }
    this.LimitType = playerShopArticle.limit_type;
    this.LimitCount = playerShopArticle.limit;
    this.PayType = (CommonPayType) playerShopArticle.payment_type_id;
    this.PayCount = playerShopArticle.payment_amount;
  }
}
