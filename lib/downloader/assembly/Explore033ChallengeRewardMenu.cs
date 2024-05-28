// Decompiled with JetBrains decompiler
// Type: Explore033ChallengeRewardMenu
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
public class Explore033ChallengeRewardMenu : ResultMenuBase
{
  [SerializeField]
  private GameObject mMainObject;
  [SerializeField]
  private GameObject mMainTitleObject;
  [SerializeField]
  private GameObject mTitle;
  [SerializeField]
  private UIGrid mGrid;
  [SerializeField]
  private UIScrollView mScrollView;
  [SerializeField]
  private GameObject mNoneMessage;
  [SerializeField]
  private CreateIconObject mIconCreater;
  private GameObject mUnitIconPrefab;
  private GameObject mItemIconPrefab;
  private List<Explore033ChallengeRewardMenu.RewardInfo> mPlayRewards = new List<Explore033ChallengeRewardMenu.RewardInfo>();

  public override IEnumerator Init(ResultMenuBase.Param param, Gladiator gladiator)
  {
    IEnumerator e = this.LoadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.CreateRewardIcons(param.challenge_finish.challenge_win_rewards);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mScrollView.ResetPosition();
    this.mMainObject.SetActive(false);
    this.mMainTitleObject.SetActive(false);
  }

  private IEnumerator LoadResources()
  {
    Future<GameObject> prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mUnitIconPrefab = prefabF.Result;
    prefabF = (Future<GameObject>) null;
    prefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mItemIconPrefab = prefabF.Result;
    prefabF = (Future<GameObject>) null;
  }

  public IEnumerator CreateRewardIcons(ChallengeEndChallenge_win_rewards[] rewards)
  {
    GameObject unknownUnit = this.CreateUnknownUnit();
    GameObject unknownItem = this.CreateUnknownItem();
    ChallengeEndChallenge_win_rewards[] challengeWinRewardsArray = rewards;
    for (int index = 0; index < challengeWinRewardsArray.Length; ++index)
    {
      ChallengeEndChallenge_win_rewards reward = challengeWinRewardsArray[index];
      IEnumerator e = this.mIconCreater.CreateThumbnail((MasterDataTable.CommonRewardType) reward.reward_type_id, reward.reward_id, reward.reward_quantity, isButton: false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject unknownObject;
      switch (reward.reward_type_id)
      {
        case 1:
        case 24:
          unknownObject = unknownUnit;
          UnitUnit unitUnit;
          if (MasterData.UnitUnit.TryGetValue(reward.reward_id, out unitUnit))
          {
            UnitIcon component = this.mIconCreater.GetIcon().GetComponent<UnitIcon>();
            if (unitUnit.IsNormalUnit)
            {
              component.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
              component.setLevelText("1");
              break;
            }
            component.RarityCenter();
            break;
          }
          break;
        default:
          unknownObject = unknownItem;
          break;
      }
      Explore033ChallengeRewardMenu.RewardInfo wrapper = this.CreateWrapper(unknownObject, this.mIconCreater.GetIcon());
      wrapper.unknownObject.SetActive(true);
      wrapper.gameObject.SetActive(false);
      this.mPlayRewards.Add(wrapper);
      reward = (ChallengeEndChallenge_win_rewards) null;
    }
    challengeWinRewardsArray = (ChallengeEndChallenge_win_rewards[]) null;
    this.mGrid.repositionNow = true;
    Object.Destroy((Object) unknownUnit);
    Object.Destroy((Object) unknownItem);
  }

  public override IEnumerator Run()
  {
    this.mMainObject.SetActive(true);
    this.mMainTitleObject.SetActive(true);
    this.mTitle.SetActive(false);
    this.mNoneMessage.SetActive(false);
    if (this.mPlayRewards.Count <= 0)
    {
      this.mNoneMessage.SetActive(true);
    }
    else
    {
      yield return (object) new WaitForSeconds(1f);
      IEnumerator e = this.OpenRewardIcon();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public override IEnumerator OnFinish()
  {
    this.mMainObject.SetActive(false);
    this.mMainTitleObject.SetActive(false);
    yield break;
  }

  private Explore033ChallengeRewardMenu.RewardInfo CreateWrapper(
    GameObject unknownObject,
    GameObject iconObject)
  {
    GameObject gameObject = new GameObject("reward obj");
    gameObject.transform.parent = ((Component) this.mGrid).transform;
    gameObject.transform.localScale = Vector3.one;
    iconObject.transform.parent = gameObject.transform;
    iconObject.transform.position = Vector3.zero;
    return new Explore033ChallengeRewardMenu.RewardInfo()
    {
      unknownObject = unknownObject.Clone(gameObject.transform),
      gameObject = iconObject
    };
  }

  private GameObject CreateUnknownUnit()
  {
    UnitIcon component = this.mUnitIconPrefab.CloneAndGetComponent<UnitIcon>();
    component.BottomModeValue = UnitIconBase.BottomMode.Nothing;
    component.BackgroundModeValue = UnitIcon.BackgroundMode.PlayerShadow;
    component.Unknown = true;
    component.NewUnit = false;
    ((Component) ((Component) component).transform.Find("icon")).gameObject.SetActive(false);
    ((Component) component).gameObject.SetActive(false);
    return ((Component) component).gameObject;
  }

  private GameObject CreateUnknownItem()
  {
    ItemIcon component = this.mItemIconPrefab.CloneAndGetComponent<ItemIcon>();
    component.SetEmpty(true);
    component.gear.unknown.SetActive(true);
    component.BottomModeValue = ItemIcon.BottomMode.Nothing;
    ((Component) component).gameObject.SetActive(false);
    return ((Component) component).gameObject;
  }

  private IEnumerator OpenRewardIcon()
  {
    Explore033ChallengeRewardMenu challengeRewardMenu = this;
    NGSoundManager soundManager = Singleton<NGSoundManager>.GetInstance();
    bool isSkip = false;
    GameObject touchObj = challengeRewardMenu.CreateTouchObject((EventDelegate.Callback) (() => isSkip = true));
    for (int i = 0; i < challengeRewardMenu.mPlayRewards.Count; ++i)
    {
      Explore033ChallengeRewardMenu.RewardInfo mPlayReward = challengeRewardMenu.mPlayRewards[i];
      TweenAlpha tweenAlpha1 = mPlayReward.unknownObject.AddComponent<TweenAlpha>();
      tweenAlpha1.to = 0.0f;
      tweenAlpha1.from = 1f;
      ((UITweener) tweenAlpha1).duration = 0.5f;
      UIWidget component = mPlayReward.gameObject.GetComponent<UIWidget>();
      if (Object.op_Inequality((Object) component, (Object) null))
        ((UIRect) component).alpha = 0.0f;
      mPlayReward.gameObject.SetActive(true);
      TweenAlpha tweenAlpha2 = mPlayReward.gameObject.AddComponent<TweenAlpha>();
      tweenAlpha2.to = 1f;
      tweenAlpha2.from = 0.0f;
      ((UITweener) tweenAlpha2).duration = 0.5f;
      if (!isSkip)
      {
        soundManager.playSE("SE_1021", seChannel: i % 3);
        yield return (object) new WaitForSeconds(0.5f);
      }
    }
    if (isSkip)
      yield return (object) new WaitForSeconds(0.5f);
    Object.Destroy((Object) touchObj);
  }

  public void DisableReward()
  {
    this.mMainObject.SetActive(false);
    this.mMainTitleObject.SetActive(false);
  }

  private class RewardInfo
  {
    public GameObject unknownObject;
    public GameObject gameObject;
  }
}
