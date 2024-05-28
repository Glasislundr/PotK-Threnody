// Decompiled with JetBrains decompiler
// Type: ExploreRewardBoxPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Explore;
using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ExploreRewardBoxPopup : BackButtonMenuBase
{
  [SerializeField]
  private NGxScroll scroll;
  [SerializeField]
  private GameObject dirMessageRewardsMax;
  [SerializeField]
  public GameObject dirMessageRewardsEmpty;
  [SerializeField]
  private UILabel labelRewardNum;
  [SerializeField]
  private UIButton btnAllOpen;
  [SerializeField]
  private SpreadColorButton btnClose;
  private GameObject createIconObjectPrefab;
  private GameObject detailPopupPrefab;
  private List<int> rewardsId;
  private List<GameObject> iconList;
  private bool connecting;

  public IEnumerator Initialize()
  {
    ExploreBox exploreBox = Singleton<ExploreDataManager>.GetInstance().ExploreBox;
    this.rewardsId = exploreBox.GetRewardsId();
    this.updateInfomation(exploreBox.IsRewardsMax);
    Future<GameObject> prefabF;
    IEnumerator e;
    if (Object.op_Equality((Object) this.createIconObjectPrefab, (Object) null))
    {
      prefabF = new ResourceObject("Prefabs/RewardIcon/createIconObject").Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.createIconObjectPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.detailPopupPrefab, (Object) null))
    {
      prefabF = Res.Prefabs.popup.popup_000_7_4_2__anim_popup01.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.detailPopupPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    e = this.createRewardIcons();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void updateInfomation(bool isRewardsMax)
  {
    this.dirMessageRewardsMax.SetActive(isRewardsMax);
    this.labelRewardNum.SetTextLocalize("{0}/{1}".F((object) this.rewardsId.Count, (object) 40));
    bool flag = this.rewardsId.Count < 1;
    ((UIButtonColor) this.btnAllOpen).isEnabled = !flag;
    this.dirMessageRewardsEmpty.SetActive(flag);
  }

  private IEnumerator createRewardIcons()
  {
    ExploreRewardBoxPopup exploreRewardBoxPopup = this;
    exploreRewardBoxPopup.scroll.Clear();
    ((UIRect) exploreRewardBoxPopup.scroll.scrollView.panel).alpha = 0.0f;
    exploreRewardBoxPopup.iconList = new List<GameObject>();
    foreach (int key in exploreRewardBoxPopup.rewardsId)
    {
      ExploreDropReward reward = MasterData.ExploreDropReward[key];
      GameObject icon = exploreRewardBoxPopup.createIconObjectPrefab.Clone();
      exploreRewardBoxPopup.scroll.Add(icon);
      CreateIconObject createIcon = icon.gameObject.GetComponent<CreateIconObject>();
      IEnumerator e = createIcon.CreateThumbnail(reward.reward_type, reward.reward_id, reward.reward_quantity);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      createIcon.SetDetailEvent(reward.reward_type, reward.reward_id, reward.reward_quantity, exploreRewardBoxPopup.detailPopupPrefab);
      createIcon.addComponentUniqueIconDragScrollView();
      if (reward.reward_type == MasterDataTable.CommonRewardType.unit || reward.reward_type == MasterDataTable.CommonRewardType.material_unit)
        createIcon.GetIcon().GetComponent<UnitIcon>().RarityCenter();
      exploreRewardBoxPopup.iconList.Add(icon);
      reward = (ExploreDropReward) null;
      icon = (GameObject) null;
      createIcon = (CreateIconObject) null;
    }
    // ISSUE: method pointer
    exploreRewardBoxPopup.scroll.GridReposition(new UIGrid.OnReposition((object) exploreRewardBoxPopup, __methodptr(\u003CcreateRewardIcons\u003Eb__13_0)));
  }

  public IEnumerator updateScrollPosition()
  {
    this.scroll.ResolvePosition();
    yield return (object) null;
    this.scroll.scrollView.UpdateScrollbars(true);
  }

  public void onGetButton()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.callGetBoxRewardAPI());
  }

  private IEnumerator callGetBoxRewardAPI()
  {
    ExploreRewardBoxPopup exploreRewardBoxPopup = this;
    exploreRewardBoxPopup.connecting = true;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    ((UIButtonColor) exploreRewardBoxPopup.btnAllOpen).isEnabled = false;
    ((UIButtonColor) exploreRewardBoxPopup.btnClose).isEnabled = false;
    foreach (GameObject icon in exploreRewardBoxPopup.iconList)
      icon.gameObject.GetComponent<CreateIconObject>().DisableLongPressEvent();
    ExploreBox exploreBox = Singleton<ExploreDataManager>.GetInstance().ExploreBox;
    ExploreSceneManager exploreSceneMgr = Singleton<ExploreSceneManager>.GetInstance();
    exploreSceneMgr.Pause(true);
    bool saveFailed = false;
    bool isLimit = false;
    yield return (object) Singleton<ExploreDataManager>.GetInstance().GetBoxReward((Action) (() => saveFailed = true), (Action) (() => isLimit = true));
    if (saveFailed)
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<ExploreDataManager>.GetInstance().InitReopenPopupState();
      exploreSceneMgr.SetReloadDirty();
      MypageScene.ChangeScene();
    }
    else
    {
      exploreSceneMgr.Pause(false);
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      exploreRewardBoxPopup.connecting = false;
      if (exploreSceneMgr.ReloadDirty)
      {
        Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
      }
      else
      {
        bool next = false;
        if (isLimit)
          exploreRewardBoxPopup.StartCoroutine(PopupCommon.Show(Consts.GetInstance().EXPLORE_BOX_GET_TITLE, Consts.GetInstance().EXPLORE_BOX_GET_MESSAGE_IS_LIMIT, (Action) (() => next = true)));
        else
          exploreRewardBoxPopup.StartCoroutine(PopupCommon.Show(Consts.GetInstance().EXPLORE_BOX_GET_TITLE, Consts.GetInstance().EXPLORE_BOX_GET_MESSAGE, (Action) (() => next = true)));
        while (!next)
          yield return (object) null;
        exploreBox = Singleton<ExploreDataManager>.GetInstance().ExploreBox;
        exploreRewardBoxPopup.rewardsId = exploreBox.GetRewardsId();
        if (exploreRewardBoxPopup.rewardsId.Count > 0)
        {
          IEnumerator e = exploreRewardBoxPopup.createRewardIcons();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          exploreRewardBoxPopup.updateInfomation(exploreBox.IsRewardsMax);
          ((UIButtonColor) exploreRewardBoxPopup.btnClose).isEnabled = true;
          exploreRewardBoxPopup.StartCoroutine(exploreRewardBoxPopup.IsPushOff());
        }
        else
        {
          exploreRewardBoxPopup.scroll.Clear();
          Singleton<PopupManager>.GetInstance().dismiss();
        }
      }
    }
  }

  public void onCloseButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public override void onBackButton() => this.onCloseButton();

  private void OnApplicationPause(bool pause)
  {
    if (!pause || this.connecting)
      return;
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
  }
}
