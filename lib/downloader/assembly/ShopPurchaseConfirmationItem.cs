// Decompiled with JetBrains decompiler
// Type: ShopPurchaseConfirmationItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class ShopPurchaseConfirmationItem : MonoBehaviour
{
  [SerializeField]
  private Transform ThumParent;
  [SerializeField]
  private UILabel ItemName;
  [SerializeField]
  private UILabel ItemCount;
  [SerializeField]
  private UIButton button;
  [SerializeField]
  private UIDragScrollView uiDragScrollView;
  private PlayerShopArticleContents playerShopArticleContents;

  public IEnumerator Init(
    PlayerShopArticleContents playerShopArticleContents,
    UIScrollView uiScrollView)
  {
    this.playerShopArticleContents = playerShopArticleContents;
    yield return (object) ShopCommon.CreateThum(this.ThumParent, (MasterDataTable.CommonRewardType) playerShopArticleContents.reward_type_id, playerShopArticleContents.reward_id);
    this.ItemName.text = CommonRewardType.GetRewardName((MasterDataTable.CommonRewardType) playerShopArticleContents.reward_type_id, playerShopArticleContents.reward_id);
    this.ItemCount.text = CommonRewardType.GetRewardQuantity((MasterDataTable.CommonRewardType) playerShopArticleContents.reward_type_id, playerShopArticleContents.reward_id, playerShopArticleContents.reward_quantity);
    this.uiDragScrollView.scrollView = uiScrollView;
  }

  public void OnIcon()
  {
    MasterDataTable.CommonRewardType rewardTypeId = (MasterDataTable.CommonRewardType) this.playerShopArticleContents.reward_type_id;
    int rewardId = this.playerShopArticleContents.reward_id;
    switch ((MasterDataTable.CommonRewardType) this.playerShopArticleContents.reward_type_id)
    {
      case MasterDataTable.CommonRewardType.gacha_ticket:
      case MasterDataTable.CommonRewardType.unit_ticket:
      case MasterDataTable.CommonRewardType.stamp:
      case MasterDataTable.CommonRewardType.challenge_point:
      case MasterDataTable.CommonRewardType.recovery_item:
        Singleton<CommonRoot>.GetInstance().StartCoroutine(ShopCommon.ShowSimpleDetailPopup(rewardTypeId, rewardId));
        break;
      default:
        Singleton<CommonRoot>.GetInstance().StartCoroutine(ShopCommon.ShowMoreDetailPopup(rewardTypeId, rewardId));
        break;
    }
  }
}
