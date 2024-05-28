// Decompiled with JetBrains decompiler
// Type: Colosseum02351Menu
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
public class Colosseum02351Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtRank;
  [SerializeField]
  protected UILabel TxtRankPt;
  [SerializeField]
  protected UILabel TxtRankPtFrom;
  [SerializeField]
  protected UILabel TxtRankPtTo;
  [SerializeField]
  protected UILabel TxtReward;
  [SerializeField]
  protected UILabel TxtReward2;
  [SerializeField]
  protected UILabel TxtRewardNum;
  [SerializeField]
  protected UILabel TxtRewardNum2;
  [SerializeField]
  protected GameObject RewardItem;
  [SerializeField]
  protected GameObject[] RewardPrize;
  [SerializeField]
  protected UIButton ZoomBtn;
  private GameObject popupObject;
  private MasterDataTable.CommonRewardType type;
  private int rewardID;
  private Colosseum0235Scene.Param param;
  private bool canChangeScene;
  [SerializeField]
  private UI2DSprite Emblem;

  public override void onBackButton() => this.IbtnBack();

  public IEnumerator Initialize(
    Colosseum0235Scene.Param param,
    ColosseumRank rank,
    int fromPoint,
    int nextPoint)
  {
    Colosseum02351Menu colosseum02351Menu = this;
    colosseum02351Menu.canChangeScene = true;
    colosseum02351Menu.param = param;
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_000_7_4_2__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    colosseum02351Menu.popupObject = prefabF.Result;
    e = Singleton<CommonRoot>.GetInstance().GetColosseumHeaderComponent().SetInfo(CommonColosseumHeader.BtnMode.Back, new Action(colosseum02351Menu.IbtnBack));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ColosseumRankReward colosseumRankReward = ((IEnumerable<ColosseumRankReward>) MasterData.ColosseumRankRewardList).Where<ColosseumRankReward>((Func<ColosseumRankReward, bool>) (x => x.rank_id == rank.ID && x.reward_type == MasterDataTable.CommonRewardType.emblem)).FirstOrDefault<ColosseumRankReward>();
    if (colosseumRankReward != null)
    {
      Future<Sprite> sprF = EmblemUtility.LoadEmblemSprite(colosseumRankReward.reward_id);
      e = sprF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      colosseum02351Menu.Emblem.sprite2D = sprF.Result;
      sprF = (Future<Sprite>) null;
    }
    colosseum02351Menu.TxtRank.SetTextLocalize(rank.name);
    colosseum02351Menu.TxtRankPtFrom.SetTextLocalize(fromPoint.ToString() + Consts.GetInstance().COLOSSEUM_002351_PT);
    if (nextPoint == -1)
      colosseum02351Menu.TxtRankPtTo.SetTextLocalize("??????" + Consts.GetInstance().COLOSSEUM_002351_PT);
    else
      colosseum02351Menu.TxtRankPtTo.SetTextLocalize(rank.to_point.ToString() + Consts.GetInstance().COLOSSEUM_002351_PT);
    ColosseumRankReward[] colosseumRankRewardArray = ColosseumUtility.GetRankRewardFromID(rank.ID);
    for (int index = 0; index < colosseumRankRewardArray.Length; ++index)
    {
      ColosseumRankReward reward = colosseumRankRewardArray[index];
      string rewardTitle = "";
      string rewardNum = "";
      if (reward.reward_type != MasterDataTable.CommonRewardType.emblem)
      {
        Future<GameObject> uniquePrefabF;
        if (reward.reward_type == MasterDataTable.CommonRewardType.battle_medal)
        {
          uniquePrefabF = Res.Icons.UniqueIconPrefab.Load<GameObject>();
          e = uniquePrefabF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          GameObject result = uniquePrefabF.Result;
          colosseum02351Menu.RewardItem.SetActive(true);
          e = ColosseumUtility.CreateUniqueIcon(result, colosseum02351Menu.RewardItem.transform, reward.reward_type, reward.reward_id, 0, false);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          colosseum02351Menu.TxtRewardNum2.SetTextLocalize("×" + (object) reward.reward_value);
          uniquePrefabF = (Future<GameObject>) null;
        }
        else
        {
          ((Component) colosseum02351Menu.ZoomBtn).gameObject.SetActive(true);
          if (reward.reward_type == MasterDataTable.CommonRewardType.gear || reward.reward_type == MasterDataTable.CommonRewardType.material_gear || reward.reward_type == MasterDataTable.CommonRewardType.gear_body)
          {
            uniquePrefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
            e = uniquePrefabF.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            GameObject result = uniquePrefabF.Result;
            GearGear rew = MasterData.GearGear[reward.reward_id];
            rewardTitle = rew.name;
            if (rew.kind_GearKind == 9)
              rewardNum = rewardNum + "×" + (object) reward.reward_value;
            ((IEnumerable<GameObject>) colosseum02351Menu.RewardPrize).ToggleOnce(0);
            if (reward.reward_type == MasterDataTable.CommonRewardType.gear_body)
            {
              e = ColosseumUtility.CreateWeaponMaterialIcon(result, rew, colosseum02351Menu.RewardPrize[0].transform);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
            }
            else
            {
              e = ColosseumUtility.CreateGearIcon(result, rew, colosseum02351Menu.RewardPrize[0].transform);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
            }
            colosseum02351Menu.type = reward.reward_type;
            colosseum02351Menu.rewardID = rew.ID;
            uniquePrefabF = (Future<GameObject>) null;
            rew = (GearGear) null;
          }
          else if (reward.reward_type == MasterDataTable.CommonRewardType.unit || reward.reward_type == MasterDataTable.CommonRewardType.material_unit)
          {
            uniquePrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
            e = uniquePrefabF.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            GameObject result = uniquePrefabF.Result;
            UnitUnit rew = MasterData.UnitUnit[reward.reward_id];
            rewardTitle = rew.name;
            ((IEnumerable<GameObject>) colosseum02351Menu.RewardPrize).ToggleOnce(1);
            e = ColosseumUtility.CreateUnitIcon(result, rew, colosseum02351Menu.RewardPrize[0].transform);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            colosseum02351Menu.type = reward.reward_type;
            colosseum02351Menu.rewardID = rew.ID;
            uniquePrefabF = (Future<GameObject>) null;
            rew = (UnitUnit) null;
          }
          else
          {
            ((Component) colosseum02351Menu.ZoomBtn).gameObject.SetActive(false);
            uniquePrefabF = Res.Icons.UniqueIconPrefab.Load<GameObject>();
            e = uniquePrefabF.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            GameObject result = uniquePrefabF.Result;
            ((IEnumerable<GameObject>) colosseum02351Menu.RewardPrize).ToggleOnce(2);
            e = ColosseumUtility.CreateUniqueIcon(result, colosseum02351Menu.RewardPrize[2].transform, reward.reward_type, reward.reward_id, 0, false);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            rewardTitle = CommonRewardType.GetRewardName(reward.reward_type, reward.reward_id, reward.reward_value);
            uniquePrefabF = (Future<GameObject>) null;
          }
          colosseum02351Menu.TxtReward.SetText(rewardTitle);
          colosseum02351Menu.TxtRewardNum.SetText(rewardNum);
        }
        rewardTitle = (string) null;
        rewardNum = (string) null;
        reward = (ColosseumRankReward) null;
      }
    }
    colosseumRankRewardArray = (ColosseumRankReward[]) null;
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet() || !this.canChangeScene | (Object.op_Inequality((Object) Singleton<PopupManager>.GetInstanceOrNull(), (Object) null) && Singleton<PopupManager>.GetInstance().isOpen))
      return;
    this.canChangeScene = false;
    Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
    Colosseum0235Scene.ChangeScene(this.param);
  }

  public virtual void IbtnZoom()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.openPopup(this.type, this.rewardID));
  }

  private IEnumerator openPopup(MasterDataTable.CommonRewardType type, int id)
  {
    GameObject popup = Singleton<PopupManager>.GetInstance().open(this.popupObject);
    popup.SetActive(false);
    IEnumerator e = popup.GetComponent<Shop00742Menu>().Init(type, id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
  }

  private enum IconIndex
  {
    EQUIP,
    UNIT,
    UNIQUE,
  }
}
