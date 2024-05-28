// Decompiled with JetBrains decompiler
// Type: Popup023418Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup023418Menu : BackButtonMenuBase
{
  protected Action onCallback;
  [SerializeField]
  private UILabel TxtDescription1;
  [SerializeField]
  private UILabel TxtDescription2;
  [SerializeField]
  private GameObject link_Icon;

  protected string GetRewardTypeText(MasterDataTable.CommonRewardType type, int id)
  {
    string rewardTypeText = "";
    switch (type)
    {
      case MasterDataTable.CommonRewardType.battle_medal:
        rewardTypeText = Consts.GetInstance().UNIQUE_ICON_BATTLE_MEDAL;
        break;
      case MasterDataTable.CommonRewardType.common_ticket:
        rewardTypeText = CommonRewardType.GetRewardName(type, id);
        break;
    }
    return rewardTypeText;
  }

  protected string GetRewardTypeUnitText(MasterDataTable.CommonRewardType type)
  {
    string rewardTypeUnitText = "";
    switch (type)
    {
      case MasterDataTable.CommonRewardType.battle_medal:
        rewardTypeUnitText = Consts.GetInstance().UNIQUE_ICON_MEDAL_COUNT;
        break;
      case MasterDataTable.CommonRewardType.common_ticket:
        rewardTypeUnitText = Consts.GetInstance().UNIQUE_ICON_COMMON_TICKET_COUNT;
        break;
    }
    return rewardTypeUnitText;
  }

  public IEnumerator Init(ResultMenuBase.BonusReward reward, int totalWin)
  {
    Future<GameObject> uniquePrefabF = Res.Icons.UniqueIconPrefab.Load<GameObject>();
    IEnumerator e = uniquePrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = ColosseumUtility.CreateUniqueIcon(uniquePrefabF.Result, this.link_Icon.transform, (MasterDataTable.CommonRewardType) reward.reward_type_id, reward.reward_id, 0, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    string rewardTypeText = this.GetRewardTypeText((MasterDataTable.CommonRewardType) reward.reward_type_id, reward.reward_id);
    string localizeNumberText = reward.reward_quantity.ToLocalizeNumberText();
    string rewardTypeUnitText = this.GetRewardTypeUnitText((MasterDataTable.CommonRewardType) reward.reward_type_id);
    this.TxtDescription2.SetTextLocalize(Consts.Format(Consts.GetInstance().COLOSSEUM_0023418_TEXT2, (IDictionary) new Hashtable()
    {
      {
        (object) "name",
        (object) rewardTypeText
      },
      {
        (object) "value",
        (object) localizeNumberText
      },
      {
        (object) "unit",
        (object) rewardTypeUnitText
      }
    }));
    this.TxtDescription1.SetTextLocalize(Consts.Format(Consts.GetInstance().COLOSSEUM_0023418_TEXT1, (IDictionary) new Hashtable()
    {
      {
        (object) "cnt",
        (object) totalWin
      }
    }));
  }

  public void SetCallback(Action callback) => this.onCallback = callback;

  public virtual void IbtnOK()
  {
    if (this.IsPushAndSet())
      return;
    if (this.onCallback != null)
      this.onCallback();
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnOK();
}
