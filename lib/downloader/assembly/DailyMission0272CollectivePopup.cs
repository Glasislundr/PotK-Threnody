// Decompiled with JetBrains decompiler
// Type: DailyMission0272CollectivePopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/DailyMission/Popup/CollectivePopup")]
public class DailyMission0272CollectivePopup : BackButtonMonoBehaiviour
{
  [SerializeField]
  private UIScrollView scrollView;
  [Header("MissionReward Control")]
  [SerializeField]
  private UIGrid grid;
  [Header("PointReward Control")]
  [SerializeField]
  private GameObject topPointReward;
  [SerializeField]
  private GameObject objBorderline;
  [SerializeField]
  private UIGrid gridPoint;
  private bool notReceiveMisson;
  private PointReward[] pointRewards;
  private GameObject pointRewardPrefab;
  private const int INDEX_MISSION = 0;
  private const int INDEX_POINT = 1;

  public void Initialize(
    List<DailyMission0272Panel.RewardViewModel> reward_list,
    List<PointReward> point_reward_list,
    bool notReceive,
    GameObject[] itemPrefabs)
  {
    ((Component) this).GetComponent<UIRect>().alpha = 0.0f;
    GameObject itemPrefab = itemPrefabs[0];
    int count = reward_list.Count;
    for (int index = 0; index < count; ++index)
      itemPrefab.Clone(((Component) this.grid).transform).GetComponent<DailyMission0272CollectivePopupList>().Initialize(reward_list[index], index);
    bool flag = !point_reward_list.IsNullOrEmpty<PointReward>();
    if (flag)
    {
      this.pointRewards = point_reward_list.ToArray();
      Vector2 vector2 = Vector2.op_Implicit(this.topPointReward.transform.localPosition);
      vector2.y = -this.grid.cellHeight * (float) count;
      this.topPointReward.transform.localPosition = Vector2.op_Implicit(vector2);
      this.objBorderline.SetActive(count > 0);
      this.pointRewardPrefab = itemPrefabs[1];
    }
    else
      this.pointRewards = (PointReward[]) null;
    this.topPointReward.SetActive(flag);
    this.notReceiveMisson = notReceive;
  }

  private IEnumerator Start()
  {
    DailyMission0272CollectivePopup mission0272CollectivePopup = this;
    bool bExistPointReward = !mission0272CollectivePopup.pointRewards.IsNullOrEmpty<PointReward>();
    if (bExistPointReward)
    {
      int nlast = mission0272CollectivePopup.pointRewards.Length - 1;
      for (int n = 0; n < mission0272CollectivePopup.pointRewards.Length; ++n)
      {
        IEnumerator e = mission0272CollectivePopup.pointRewardPrefab.Clone(((Component) mission0272CollectivePopup.gridPoint).transform).GetComponent<MissionPointRewardDetailItemController>().Init(mission0272CollectivePopup.pointRewards[n], true, n == nlast);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    yield return (object) null;
    mission0272CollectivePopup.grid.Reposition();
    if (bExistPointReward)
      mission0272CollectivePopup.gridPoint.Reposition();
    mission0272CollectivePopup.scrollView.ResetPosition();
    Singleton<PopupManager>.GetInstance().startOpenAnime(((Component) mission0272CollectivePopup).gameObject);
  }

  public void IbtnNo()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    if (!this.notReceiveMisson)
      return;
    ModalWindow.Show("ミッション報酬受け取り", "報酬が上限に達して受け取れないミッションがあります", (Action) (() => { }));
  }

  public override void onBackButton() => this.IbtnNo();
}
