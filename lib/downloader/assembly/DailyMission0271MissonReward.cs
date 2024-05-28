// Decompiled with JetBrains decompiler
// Type: DailyMission0271MissonReward
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class DailyMission0271MissonReward : MonoBehaviour
{
  [SerializeField]
  private UILabel rewardName;
  [SerializeField]
  private CreateIconObject rewardThum;

  public IEnumerator Init(MasterDataTable.CommonRewardType type, int id, int quantity)
  {
    this.rewardName.SetTextLocalize(CommonRewardType.GetRewardName(type, id, quantity));
    IEnumerator e = this.rewardThum.CreateThumbnail(type, id, quantity);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
