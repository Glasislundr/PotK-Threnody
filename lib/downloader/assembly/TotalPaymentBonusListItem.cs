// Decompiled with JetBrains decompiler
// Type: TotalPaymentBonusListItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TotalPaymentBonusListItem : MonoBehaviour
{
  [SerializeField]
  private UILabel mPaidValueLbl;
  [SerializeField]
  private UIGrid mIconGrid;
  [SerializeField]
  private UIButton mGetButton;
  [SerializeField]
  private GameObject mPositiveRoot;
  [SerializeField]
  private GameObject mNegativeRoot;
  private int mBonusItemId;
  private TotalPaymentBonusPopup mParentPopup;

  public IEnumerator Initialize(
    PlayerPaymentBonusReceiveHistory bonus,
    TotalPaymentBonusPopup parent)
  {
    this.mBonusItemId = bonus.bonus_item_id;
    this.mParentPopup = parent;
    this.SetPaidValue(bonus.require_paid_coins);
    TotalPaymentBonusContent content;
    if (MasterData.TotalPaymentBonusContent.TryGetValue(bonus.bonus_item_id, out content))
    {
      Future<GameObject> loader = new ResourceObject("Prefabs/totalpaymentbonus/dir_TotalPaymentBonus_IconOnly_Item").Load<GameObject>();
      IEnumerator e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject iconPrefab = loader.Result;
      foreach (TotalPaymentBonusReward reward in this.ConvertToRewardList(content))
      {
        e = iconPrefab.CloneAndGetComponent<ItemIconDetail>(((Component) this.mIconGrid).transform).Init(reward.reward_type, reward.reward_id, reward.reward_quantity);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      this.mIconGrid.Reposition();
      loader = (Future<GameObject>) null;
      iconPrefab = (GameObject) null;
    }
    if (bonus.is_received)
    {
      this.mPositiveRoot.SetActive(false);
      this.mNegativeRoot.SetActive(true);
    }
    else
      ((UIButtonColor) this.mGetButton).isEnabled = bonus.is_archived;
  }

  private void SetPaidValue(int require)
  {
    this.mPaidValueLbl.SetTextLocalize(string.Format("累計姫石[ffff00]{0}[-]個\n消費で受取可能", (object) require));
  }

  public void OnPushOpenButton() => this.mParentPopup.OnListItemGetButton(this.mBonusItemId);

  private List<TotalPaymentBonusReward> ConvertToRewardList(TotalPaymentBonusContent content)
  {
    List<TotalPaymentBonusReward> rewardList = new List<TotalPaymentBonusReward>();
    string rewardId = content.reward_id;
    char[] chArray = new char[1]{ ',' };
    foreach (string str in rewardId.Split(chArray))
    {
      double result = 0.0;
      if (double.TryParse(str.Trim(), out result))
      {
        TotalPaymentBonusReward paymentBonusReward = MasterData.TotalPaymentBonusReward[(int) result];
        rewardList.Add(paymentBonusReward);
      }
    }
    return rewardList;
  }
}
