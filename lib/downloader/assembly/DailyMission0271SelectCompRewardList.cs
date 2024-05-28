// Decompiled with JetBrains decompiler
// Type: DailyMission0271SelectCompRewardList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class DailyMission0271SelectCompRewardList : MonoBehaviour
{
  [SerializeField]
  private UILabel rewardName;
  [SerializeField]
  private CreateIconObject iconRoot;
  [SerializeField]
  private GameObject btnDetail;
  private int reward_group_id;
  private BingoRewardGroup completeReward;
  private DailyMission0271MissionRoot missionRoot;
  private GameObject compleateMissionRewardDetailPrefab;

  public IEnumerator Init(
    BingoRewardGroup completeReward,
    DailyMission0271MissionRoot missionRoot,
    GameObject compleateMissionRewardDetailPrefab)
  {
    this.completeReward = completeReward;
    this.compleateMissionRewardDetailPrefab = compleateMissionRewardDetailPrefab;
    this.reward_group_id = completeReward.reward_group_id;
    this.missionRoot = missionRoot;
    this.rewardName.SetTextLocalize(CommonRewardType.GetRewardName(completeReward.reward_type_id, completeReward.reward_id, completeReward.reward_quantity));
    IEnumerator e = this.iconRoot.CreateThumbnail(completeReward.reward_type_id, completeReward.reward_id, completeReward.reward_quantity, isButton: false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.btnDetail.SetActive(Shop00742Menu.IsEnableShowPopup(completeReward.reward_type_id));
  }

  public void onCompReward()
  {
    if (this.missionRoot.menu.IsPushAndSet())
      return;
    this.StartCoroutine(this.CompRewardPopup());
  }

  public IEnumerator CompRewardPopup()
  {
    DailyMission0271SelectCompRewardList selectCompRewardList = this;
    Future<GameObject> futureF = Res.Prefabs.popup.popup_027_4__anim_popup01.Load<GameObject>();
    IEnumerator e = futureF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject popupPrefab = futureF.Result.Clone();
    popupPrefab.SetActive(false);
    // ISSUE: reference to a compiler-generated method
    e = popupPrefab.GetComponent<DailyMission0271CompRewardPopup>().Init(selectCompRewardList.completeReward, new Action(selectCompRewardList.\u003CCompRewardPopup\u003Eb__9_0));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popupPrefab.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(popupPrefab, isCloned: true);
  }

  public void onDetailReward()
  {
    if (this.missionRoot.menu.IsPushAndSet())
      return;
    this.StartCoroutine(this.setDetailPopup());
  }

  private IEnumerator setDetailPopup()
  {
    GameObject popupPrefab = this.compleateMissionRewardDetailPrefab.Clone();
    popupPrefab.SetActive(false);
    IEnumerator e = popupPrefab.GetComponent<Shop00742Menu>().Init(this.completeReward.reward_type_id, this.completeReward.reward_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popupPrefab.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(popupPrefab, isCloned: true);
  }
}
