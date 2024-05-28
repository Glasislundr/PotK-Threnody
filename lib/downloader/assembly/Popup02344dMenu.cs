// Decompiled with JetBrains decompiler
// Type: Popup02344dMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Popup02344dMenu : NGMenuBase
{
  protected Action onCallback;
  [SerializeField]
  private UI2DSprite TitleImg;
  [SerializeField]
  private UILabel TxtDescription1;
  [SerializeField]
  private UILabel TxtDescription2;
  [SerializeField]
  private GameObject link_Icon;

  public IEnumerator Init(RankUpInfoRank_up_rewards[] rewards)
  {
    RankUpInfoRank_up_rewards infoRankUpRewards1 = ((IEnumerable<RankUpInfoRank_up_rewards>) rewards).FirstOrDefault<RankUpInfoRank_up_rewards>((Func<RankUpInfoRank_up_rewards, bool>) (x => x.reward_type_id == 16));
    IEnumerator e;
    if (infoRankUpRewards1 != null)
    {
      Future<Sprite> sprF = EmblemUtility.LoadEmblemSprite(infoRankUpRewards1.reward_id);
      e = sprF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.TitleImg.sprite2D = sprF.Result;
      sprF = (Future<Sprite>) null;
    }
    RankUpInfoRank_up_rewards reward = ((IEnumerable<RankUpInfoRank_up_rewards>) rewards).FirstOrDefault<RankUpInfoRank_up_rewards>((Func<RankUpInfoRank_up_rewards, bool>) (x => x.reward_type_id != 16 && x.reward_type_id != 17));
    if (reward != null)
    {
      Future<GameObject> gearPrefabF;
      if (reward.reward_type_id == 3 || reward.reward_type_id == 26 || reward.reward_type_id == 35)
      {
        gearPrefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
        e = gearPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        GameObject result = gearPrefabF.Result;
        GearGear gear = ((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x => x.ID == reward.reward_id));
        if (reward.reward_type_id == 35)
        {
          e = ColosseumUtility.CreateWeaponMaterialIcon(result, gear, this.link_Icon.transform, false);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        else
        {
          e = ColosseumUtility.CreateGearIcon(result, gear, this.link_Icon.transform, false);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        gearPrefabF = (Future<GameObject>) null;
      }
      else if (reward.reward_type_id == 1 || reward.reward_type_id == 24)
      {
        gearPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
        e = gearPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        e = ColosseumUtility.CreateUnitIcon(gearPrefabF.Result, ((IEnumerable<UnitUnit>) MasterData.UnitUnitList).FirstOrDefault<UnitUnit>((Func<UnitUnit, bool>) (x => x.ID == reward.reward_id)), this.link_Icon.transform, false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        gearPrefabF = (Future<GameObject>) null;
      }
      else
      {
        gearPrefabF = Res.Icons.UniqueIconPrefab.Load<GameObject>();
        e = gearPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        e = ColosseumUtility.CreateUniqueIcon(gearPrefabF.Result, this.link_Icon.transform, (MasterDataTable.CommonRewardType) reward.reward_type_id, reward.reward_id, 0, false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        gearPrefabF = (Future<GameObject>) null;
      }
      this.TxtDescription1.SetTextLocalize(CommonRewardType.GetRewardName((MasterDataTable.CommonRewardType) reward.reward_type_id, reward.reward_id, reward.reward_quantity));
    }
    RankUpInfoRank_up_rewards infoRankUpRewards2 = ((IEnumerable<RankUpInfoRank_up_rewards>) rewards).FirstOrDefault<RankUpInfoRank_up_rewards>((Func<RankUpInfoRank_up_rewards, bool>) (x => x.reward_type_id == 17));
    if (infoRankUpRewards2 != null)
      this.TxtDescription2.SetTextLocalize(CommonRewardType.GetRewardName((MasterDataTable.CommonRewardType) infoRankUpRewards2.reward_type_id, infoRankUpRewards2.reward_id, infoRankUpRewards2.reward_quantity));
  }

  public void SetCallback(Action callback) => this.onCallback = callback;

  public virtual void IbtnOK()
  {
    if (this.IsPushAndSet())
      return;
    if (this.onCallback != null)
      this.onCallback();
    Singleton<PopupManager>.GetInstance().onDismiss();
  }
}
