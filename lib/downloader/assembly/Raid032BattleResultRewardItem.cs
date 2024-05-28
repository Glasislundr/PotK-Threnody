// Decompiled with JetBrains decompiler
// Type: Raid032BattleResultRewardItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Raid032BattleResultRewardItem : MonoBehaviour
{
  [SerializeField]
  private CreateIconObject createIcon;

  public IEnumerator InitAsync(RaidDamageReward reward)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Raid032BattleResultRewardItem resultRewardItem = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      ((Component) resultRewardItem).gameObject.SetActive(true);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) resultRewardItem.createIcon.CreateThumbnail((MasterDataTable.CommonRewardType) reward.reward_type_id, reward.reward_id, reward.reward_quantity, isButton: false);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }
}
