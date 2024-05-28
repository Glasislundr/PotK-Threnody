// Decompiled with JetBrains decompiler
// Type: SeaTalkMission
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
public class SeaTalkMission : BackButtonPopupWindow
{
  private const int ITEM_HEIGHT = 130;
  [SerializeField]
  private UILabel partnerName;
  [SerializeField]
  private GameObject pledgeComp;
  [SerializeField]
  private GameObject pledgeCompGauge;
  [SerializeField]
  private UIProgressBar progressBar;
  [SerializeField]
  private UILabel missonClearCount;
  [SerializeField]
  private UILabel missionTotalCount;
  [SerializeField]
  private NGxMaskSprite unitStandSprite;
  [SerializeField]
  private NGxMaskSpriteWithScale maskSpriteWithScale;
  [SerializeField]
  private UIScrollView scrollView;
  private TalkUnitInfo talkUnitInfo;
  private List<PlayerCallMission> playerCallMissions;
  private GameObject missionItemPrefab;

  public IEnumerator Init(TalkUnitInfo talkUnitInfo, PlayerCallMission[] playerCallMissions)
  {
    this.talkUnitInfo = talkUnitInfo;
    this.playerCallMissions = ((IEnumerable<PlayerCallMission>) playerCallMissions).ToList<PlayerCallMission>();
    yield return (object) this.LoadResource();
    this.partnerName.text = talkUnitInfo.unit.name;
    Future<Sprite> f = talkUnitInfo.unit.LoadSpriteLarge(talkUnitInfo.unit.job_UnitJob, 1f);
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unitStandSprite.sprite2D = f.Result;
    this.maskSpriteWithScale.FitMask();
    yield return (object) this.CreateMissions();
  }

  private IEnumerator LoadResource()
  {
    Future<GameObject> f = new ResourceObject("Prefabs/sea030_PledgeMission/dir_pledgeMission_List_sea").Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.missionItemPrefab = f.Result;
  }

  private IEnumerator CreateMissions()
  {
    this.missionTotalCount.text = string.Format("/{0}", (object) this.playerCallMissions.Count);
    this.playerCallMissions = this.playerCallMissions.OrderBy<PlayerCallMission, int>((Func<PlayerCallMission, int>) (x => x.mission_id)).ToList<PlayerCallMission>();
    for (int index = 0; index < this.playerCallMissions.Count; ++index)
      this.playerCallMissions[index].sequentialNumber = index + 1;
    List<PlayerCallMission> first = new List<PlayerCallMission>();
    List<PlayerCallMission> second = new List<PlayerCallMission>();
    foreach (PlayerCallMission playerCallMission in this.playerCallMissions)
    {
      if (playerCallMission.mission_status == 4)
        second.Add(playerCallMission);
      else
        first.Add(playerCallMission);
    }
    this.playerCallMissions = first.Concat<PlayerCallMission>((IEnumerable<PlayerCallMission>) second).ToList<PlayerCallMission>();
    int clearCount = 0;
    int height = 0;
    this.missionItemPrefab.SetActive(false);
    for (int i = 0; i < this.playerCallMissions.Count; ++i)
    {
      PlayerCallMission playerCallMission = this.playerCallMissions[i];
      CallMission callMission;
      MasterData.CallMission.TryGetValue(playerCallMission.mission_id, out callMission);
      if (playerCallMission.count >= callMission.number_times)
        ++clearCount;
      GameObject go = this.missionItemPrefab.Clone(((Component) this.scrollView).transform);
      yield return (object) go.GetComponent<SeaTalkMissionItem>().Init(this.talkUnitInfo, playerCallMission, callMission, this.playerCallMissions);
      go.transform.localPosition = new Vector3(0.0f, (float) height, 0.0f);
      height -= 130;
      go = (GameObject) null;
    }
    foreach (Component component in ((Component) this.scrollView).transform)
      component.gameObject.SetActive(true);
    this.missonClearCount.text = clearCount.ToString();
    this.progressBar.value = (float) clearCount / (float) this.playerCallMissions.Count;
    this.scrollView.ResetPosition();
    if (clearCount >= this.playerCallMissions.Count)
    {
      this.pledgeComp.SetActive(true);
      this.pledgeCompGauge.SetActive(true);
    }
    else
    {
      this.pledgeComp.SetActive(false);
      this.pledgeCompGauge.SetActive(false);
    }
  }

  public override void onBackButton()
  {
    if (Object.op_Implicit((Object) Object.FindObjectOfType<SeaTalkMissionRewardReceive>()))
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }
}
