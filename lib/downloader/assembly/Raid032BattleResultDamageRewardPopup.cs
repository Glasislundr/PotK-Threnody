// Decompiled with JetBrains decompiler
// Type: Raid032BattleResultDamageRewardPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Raid032BattleResultDamageRewardPopup : MonoBehaviour
{
  [SerializeField]
  private UILabel percentageLabel;
  [SerializeField]
  private Raid032BattleResultRewardItem rewardItem;

  public IEnumerator InitAsync(RaidDamageReward reward)
  {
    this.percentageLabel.SetTextLocalize(string.Format("{0}％", (object) (reward.damage_ratio / 100)));
    yield return (object) this.rewardItem.InitAsync(reward);
  }
}
