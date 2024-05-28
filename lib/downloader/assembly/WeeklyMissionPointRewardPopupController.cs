// Decompiled with JetBrains decompiler
// Type: WeeklyMissionPointRewardPopupController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class WeeklyMissionPointRewardPopupController : MonoBehaviour
{
  private DailyMissionController controller;
  [SerializeField]
  private UILabel pointAcquiredThisWeekText;
  [SerializeField]
  private List<WeeklyMissionPointRewardItemController> weeklyMissionPointRewardItems;
  [SerializeField]
  private UIGrid grid;
  private Action onCloseAction;

  public IEnumerator Init(
    DailyMissionController controller,
    int currentWeeklyPoint,
    List<PointRewardBox> weeklyPointRewardBoxList,
    Action onCloseAction,
    Action onReceiveAction)
  {
    this.controller = controller;
    this.onCloseAction = onCloseAction;
    ServerTime.NowAppTimeAddDelta();
    this.pointAcquiredThisWeekText.SetTextLocalize(currentWeeklyPoint);
    for (int i = 0; i < this.weeklyMissionPointRewardItems.Count; i++)
    {
      PointRewardBox pointRewardBox = weeklyPointRewardBoxList.FirstOrDefault<PointRewardBox>((Func<PointRewardBox, bool>) (x => x.box_type == i + 1));
      if (pointRewardBox != null)
      {
        ((Component) this.weeklyMissionPointRewardItems[i]).gameObject.SetActive(true);
        this.weeklyMissionPointRewardItems[i].Init(controller, pointRewardBox, currentWeeklyPoint, ((IEnumerable<int?>) SMManager.Get<PlayerDailyMissionPoint>().weekly.received_rewards).Contains<int?>(new int?(pointRewardBox.ID)), onReceiveAction);
      }
      else
        ((Component) this.weeklyMissionPointRewardItems[i]).gameObject.SetActive(false);
    }
    this.grid.Reposition();
    yield return (object) null;
  }

  public void OnClickCloseButton()
  {
    this.onCloseAction();
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
