// Decompiled with JetBrains decompiler
// Type: Sea030HomeDateConfirmPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Sea030HomeDateConfirmPopup : BackButtonMonoBehaiviour
{
  [SerializeField]
  private UILabel descriptionLabel;
  [SerializeField]
  private UILabel consumeDpLabel;
  [SerializeField]
  private UILabel dpLabel;
  [SerializeField]
  private GameObject cauctionObject;
  [SerializeField]
  private UIButton decideButton;
  private Action<SeaDateDateSpotDisplaySetting> pushCallback;
  private SeaDateDateSpotDisplaySetting settingData;

  public void Init(
    PlayerUnit playerUnit,
    SeaDateDateSpotDisplaySetting setting,
    Action<SeaDateDateSpotDisplaySetting> callback)
  {
    Consts instance = Consts.GetInstance();
    SeaPlayer seaPlayer = SMManager.Get<SeaPlayer>();
    this.pushCallback = callback;
    this.settingData = setting;
    this.cauctionObject.SetActive((double) playerUnit.trust_rate >= (double) playerUnit.trust_max_rate);
    this.consumeDpLabel.SetTextLocalize(setting.datespot.date_point_cost);
    if (setting.datespot.date_point_cost > seaPlayer.dp)
      ((UIWidget) this.dpLabel).color = Color.red;
    this.dpLabel.SetTextLocalize(seaPlayer.dp);
    this.descriptionLabel.SetTextLocalize(Consts.Format(instance.DATE_CONFIRM_DESCRIPTION_TEXT, (IDictionary) new Hashtable()
    {
      {
        (object) "unit_name",
        (object) playerUnit.unit.name
      },
      {
        (object) "spot_name",
        (object) setting.date_name
      }
    }));
    ((UIButtonColor) this.decideButton).isEnabled = setting.datespot.date_point_cost <= seaPlayer.dp;
  }

  public void IbtnDecide()
  {
    if (this.pushCallback == null)
      return;
    this.pushCallback(this.settingData);
  }

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().dismiss();
}
