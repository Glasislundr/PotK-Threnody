// Decompiled with JetBrains decompiler
// Type: Popup02681Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup02681Menu : Popup02681MenuBase
{
  private bool isPush;
  protected Action onCallback;
  [SerializeField]
  private UILabel TxtTitle;
  [SerializeField]
  private UILabel TxtDescription1;
  [SerializeField]
  private UILabel TxtDescription2;
  [SerializeField]
  private GameObject link_icon;

  public IEnumerator Init(Versus0268Menu.PvpParam.BonusReward reward, int totalWin)
  {
    IEnumerator e = this.link_icon.GetOrAddComponent<CreateIconObject>().CreateThumbnail((MasterDataTable.CommonRewardType) reward.reward_type_id, reward.reward_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    string rewardQuantity = CommonRewardType.GetRewardQuantity((MasterDataTable.CommonRewardType) reward.reward_type_id, reward.reward_id, reward.reward_quantity);
    Consts instance = Consts.GetInstance();
    this.TxtDescription2.text = rewardQuantity + instance.VERSUS_002681_TEXT;
    this.TxtDescription1.SetTextLocalize(Consts.Format(instance.VERSUS_002681_MESSAGE, (IDictionary) new Hashtable()
    {
      {
        (object) "cnt",
        (object) totalWin
      }
    }));
    this.TxtTitle.SetText(instance.VERSUS_002681_TITLE);
  }

  public override void IbtnOK()
  {
    if (this.isPush)
      return;
    this.isPush = true;
    base.IbtnOK();
    if (this.onCallback != null)
      this.onCallback();
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public void SetCallback(Action callback) => this.onCallback = callback;
}
