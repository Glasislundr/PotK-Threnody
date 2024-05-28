// Decompiled with JetBrains decompiler
// Type: Shop00720RewardList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Shop00720RewardList : BackButtonMenuBase
{
  [SerializeField]
  private NGxScrollMasonry Scroll;

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  public IEnumerator Init(Shop00720Prefabs prefabs, int deckID)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
    yield return (object) null;
    SlotS001MedalDeckEntity[] DeckEntityList = SlotS001MedalDeckEntity.getRewards(deckID);
    AssocList<int, SlotS001MedalDeck> slotS001MedalDeck = MasterData.SlotS001MedalDeck;
    this.Scroll.Reset();
    GameObject prefab = prefabs.DirSlotReward;
    ((Component) this.Scroll.Scroll).gameObject.SetActive(false);
    foreach (KeyValuePair<int, SlotS001MedalDeck> keyValuePair in slotS001MedalDeck)
    {
      SlotS001MedalDeck targetDeck = keyValuePair.Value;
      int[] reelIds = new int[3]
      {
        targetDeck.prize_first,
        targetDeck.prize_second,
        targetDeck.prize_third
      };
      Shop00720RewardPatternData rewardPatternData = new Shop00720RewardPatternData(reelIds);
      if (rewardPatternData.IsEnabled)
      {
        IEnumerator e = this.SetRewardPatternData(rewardPatternData, reelIds, keyValuePair.Key, targetDeck, DeckEntityList, true);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (rewardPatternData.RewardList.Count != 0)
        {
          GameObject gameObject = Object.Instantiate<GameObject>(prefab);
          this.Scroll.Add(gameObject);
          e = gameObject.GetComponent<Shop00720RewardScroll>().Init(rewardPatternData, prefabs);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        else
          continue;
      }
      rewardPatternData = (Shop00720RewardPatternData) null;
    }
    ((Component) this.Scroll.Scroll).gameObject.SetActive(true);
    this.Scroll.ResolvePosition();
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  private void OnEnable() => this.Scroll.ResolvePosition();

  private IEnumerator SetRewardPatternData(
    Shop00720RewardPatternData rewardPatternData,
    int[] reelIds,
    int id,
    SlotS001MedalDeck targetDeck,
    SlotS001MedalDeckEntity[] DeckEntityList,
    bool checkEmpty)
  {
    foreach (SlotS001MedalDeckEntity reward in ((IEnumerable<SlotS001MedalDeckEntity>) DeckEntityList).Where<SlotS001MedalDeckEntity>((Func<SlotS001MedalDeckEntity, bool>) (w => w.deck_id == id)))
      this.SetRewardData(reward, rewardPatternData);
    if (!checkEmpty || rewardPatternData.RewardList.Count != 0)
    {
      IEnumerator e = rewardPatternData.SetSprite(reelIds);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      rewardPatternData.Description = targetDeck.description;
    }
  }

  private void SetRewardData(
    SlotS001MedalDeckEntity reward,
    Shop00720RewardPatternData rewardPatternData)
  {
    Shop00720RewardData shop00720RewardData = new Shop00720RewardData()
    {
      RewardId = reward.reward_id,
      RewardType = reward.reward_type_id,
      Quantity = reward.reward_quantity.HasValue ? reward.reward_quantity.Value : 0
    };
    shop00720RewardData.Description = CommonRewardType.GetRewardName(shop00720RewardData.RewardType, shop00720RewardData.RewardId, shop00720RewardData.Quantity);
    rewardPatternData.RewardList.Add(shop00720RewardData);
  }
}
