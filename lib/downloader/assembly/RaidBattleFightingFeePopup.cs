// Decompiled with JetBrains decompiler
// Type: RaidBattleFightingFeePopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RaidBattleFightingFeePopup : BackButtonMenuBase
{
  [SerializeField]
  private Transform mItemAnchorParent;
  private Transform[] mItemAnchors;
  private GameObject mRewardItemPrefab;

  public static IEnumerator show(List<GuildRaid.RaidReward> rewards)
  {
    Future<GameObject> loader = new ResourceObject("Prefabs/popup/popup_FightingFee_detail_popup").Load<GameObject>();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = loader.Result;
    loader = (Future<GameObject>) null;
    if (!Object.op_Equality((Object) result, (Object) null))
    {
      GameObject go = Singleton<PopupManager>.GetInstance().open(result, isNonSe: true, isNonOpenAnime: true);
      go.SetActive(false);
      yield return (object) go.GetComponent<RaidBattleFightingFeePopup>().initAsync(rewards);
      Singleton<PopupManager>.GetInstance().startOpenAnime(go);
      while (go.activeInHierarchy)
        yield return (object) null;
    }
  }

  public override void onBackButton() => this.onClickedClose();

  public void onClickedClose()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  private IEnumerator initAsync(List<GuildRaid.RaidReward> rewards)
  {
    yield return (object) this.loadPrefabs();
    int cnt = 0;
    this.mItemAnchors = new Transform[this.mItemAnchorParent.childCount];
    foreach (Transform transform in this.mItemAnchorParent)
    {
      this.mItemAnchors[cnt] = transform;
      ++cnt;
    }
    cnt = 0;
    foreach (GuildRaid.RaidReward reward in rewards)
    {
      MasterDataTable.CommonRewardType type = reward.Type;
      int id = reward.Id;
      int quantity = reward.Quantity;
      string title = reward.Title;
      yield return (object) this.mRewardItemPrefab.CloneAndGetComponent<Versus02612ScrollRewardItem>(this.mItemAnchors[cnt]).CreateItem(type, id, title, true);
      ++cnt;
    }
  }

  private IEnumerator loadPrefabs()
  {
    if (Object.op_Equality((Object) this.mRewardItemPrefab, (Object) null))
    {
      Future<GameObject> prefabF = new ResourceObject("Prefabs/raid032_battle/dir_Reward_RaidBoss_defeat").Load<GameObject>();
      IEnumerator e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mRewardItemPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
  }
}
