// Decompiled with JetBrains decompiler
// Type: Popup02686Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup02686Menu : BackButtonMonoBehaiviour
{
  [SerializeField]
  private GameObject link_icon;
  [SerializeField]
  private UILabel txtDescription;
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private UILabel txtShowText;
  private bool isPush;
  private Action onCallback;

  public IEnumerator Init(Versus0268Menu.PvpParam.FirstBattleReward reward)
  {
    IEnumerator e = this.link_icon.GetOrAddComponent<CreateIconObject>().CreateThumbnail((MasterDataTable.CommonRewardType) reward.reward_type_id, reward.reward_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Consts instance = Consts.GetInstance();
    this.txtShowText.SetTextLocalize(Consts.Format(reward.show_text));
    this.txtTitle.SetTextLocalize(Consts.Format(instance.VERSUS_002686_TITLE));
    this.txtDescription.SetTextLocalize(Consts.Format(instance.VERSUS_002686_DESCRIPTION));
  }

  public void SetCallback(Action callback) => this.onCallback = callback;

  public void IbtnOK()
  {
    if (this.isPush)
      return;
    this.isPush = true;
    if (this.onCallback != null)
      this.onCallback();
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnOK();
}
