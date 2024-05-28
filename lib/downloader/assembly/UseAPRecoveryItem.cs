// Decompiled with JetBrains decompiler
// Type: UseAPRecoveryItem
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
public class UseAPRecoveryItem : BackButtonMenuBase
{
  private Action btnAct;
  [SerializeField]
  private CreateIconObject dirItemIcon;
  [SerializeField]
  private UILabel txtItemName;
  [SerializeField]
  private UILabel txtProssessionValue;
  private RecoveryItemAPHeal apRecoveryItem;
  [SerializeField]
  public NGTweenGaugeScale ApGauge;
  [SerializeField]
  private UILabel txtSelectedNumberValue;
  [SerializeField]
  private UILabel txtAPRecoveryValue;
  [SerializeField]
  private UILabel txtAP01;
  [SerializeField]
  private UILabel txtAP02;
  [SerializeField]
  private UILabel txtAP03;
  [Header("Slider")]
  [SerializeField]
  private UILabel txtCount;
  [SerializeField]
  private UILabel txtSelectMin;
  [SerializeField]
  private UILabel txtSelectMax;
  [SerializeField]
  private UISlider slider;
  [SerializeField]
  private UIButton[] sliderButtons;
  private int maxCount;
  private int selectedCount = 1;
  private int sliderCount = 1;
  [Header("Button")]
  public SpreadColorButton yesButton;

  public void ibtnYes()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(UseAPRecoveryItem.UseAPItem(this.apRecoveryItem.ID, this.selectedCount, this.btnAct));
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  public static IEnumerator UseAPItem(int apRecoveryItemId, int quantity, Action questChangeScene)
  {
    Singleton<PopupManager>.GetInstance().closeAll(true);
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    yield return (object) null;
    Player player = SMManager.Get<Player>();
    int before_player_ap = player.ap + player.ap_overflow;
    Future<WebAPI.Response.RecoveryItemRecovery> handler = WebAPI.RecoveryItemRecovery(quantity, apRecoveryItemId);
    IEnumerator e = handler.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    WebAPI.Response.RecoveryItemRecovery result = handler.Result;
    if (result == null)
    {
      Singleton<PopupManager>.GetInstance().closeAll();
      yield return (object) null;
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    }
    else
    {
      int after_player_ap = result.player.ap + result.player.ap_overflow;
      Future<GameObject> prefab = new ResourceObject("Prefabs/popup/popup_AP_Recovery_result").Load<GameObject>();
      e = prefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = Singleton<PopupManager>.GetInstance().open(prefab.Result).GetComponent<APRecoveryResult>().Init(before_player_ap, after_player_ap, questChangeScene);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      yield return (object) null;
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      prefab = (Future<GameObject>) null;
    }
  }

  public void SetBtnAct(Action questChangeScene) => this.btnAct = questChangeScene;

  public IEnumerator Init(RecoveryItemAPHeal itemAPHeal, int quantity)
  {
    this.apRecoveryItem = itemAPHeal;
    this.txtItemName.SetTextLocalize(this.apRecoveryItem.name);
    this.txtProssessionValue.SetTextLocalize(quantity);
    IEnumerator e = this.dirItemIcon.CreateThumbnail(MasterDataTable.CommonRewardType.recovery_item, itemAPHeal.ID, visibleBottom: false, isButton: false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Collider) ((Component) this.slider).GetComponent<BoxCollider>()).enabled = false;
    ((Behaviour) this.slider).enabled = false;
    Player player = SMManager.Get<Player>();
    int num1 = player.ap + player.ap_overflow;
    int num2 = (Player.GetApOverChargeLimit() - num1) / this.apRecoveryItem.recovery_amount;
    this.maxCount = quantity >= num2 ? num2 : quantity;
    if (this.maxCount <= 1)
    {
      ((UIProgressBar) this.slider).numberOfSteps = this.maxCount + 1;
      ((Behaviour) this.slider).enabled = false;
      ((Collider) ((Component) this.slider).GetComponent<BoxCollider>()).enabled = false;
      this.txtSelectMin.text = "0";
      this.selectedCount = 1;
      this.sliderCount = 1;
      foreach (UIButtonColor sliderButton in this.sliderButtons)
        sliderButton.isEnabled = false;
      if (this.maxCount < 1)
      {
        ((UIButtonColor) this.yesButton).isEnabled = false;
        this.maxCount = 1;
      }
    }
    else
    {
      ((Collider) ((Component) this.slider).GetComponent<BoxCollider>()).enabled = true;
      ((Behaviour) this.slider).enabled = true;
      ((UIProgressBar) this.slider).numberOfSteps = this.maxCount;
      this.txtSelectMin.text = "1";
      this.selectedCount = 1;
      this.sliderCount = this.selectedCount - 1;
    }
    this.UpdateInfo();
    this.txtSelectMax.SetTextLocalize(this.maxCount.ToString());
  }

  public void OnValueChange()
  {
    this.sliderCount = Mathf.RoundToInt(((UIProgressBar) this.slider).value * ((float) this.maxCount - 1f));
    this.UpdateInfo();
  }

  private void UpdateInfo()
  {
    this.selectedCount = this.maxCount != 1 ? this.sliderCount + 1 : 1;
    this.txtCount.SetTextLocalize(this.selectedCount);
    ((UIProgressBar) this.slider).value = (float) this.sliderCount / ((float) this.maxCount - 1f);
    this.SetApRecoveryGauge(this.selectedCount);
  }

  public void IbtnDecrease()
  {
    --this.sliderCount;
    if (this.sliderCount <= 0)
      this.sliderCount = 0;
    this.UpdateInfo();
  }

  public void IbtnSetMin()
  {
    this.sliderCount = 0;
    this.UpdateInfo();
  }

  public void IbtnSetMax()
  {
    this.sliderCount = this.maxCount - 1;
    this.UpdateInfo();
  }

  public void IbtnIncrease()
  {
    ++this.sliderCount;
    if (this.sliderCount >= this.maxCount - 1)
      this.sliderCount = this.maxCount - 1;
    this.UpdateInfo();
  }

  public void SetApRecoveryGauge(int itemCount, bool doTween = false)
  {
    Player player = SMManager.Get<Player>();
    int num1 = player.ap + player.ap_overflow;
    int num2 = itemCount * this.apRecoveryItem.recovery_amount;
    int num3 = num1 + num2;
    this.txtAP01.SetTextLocalize(num1);
    this.txtAP03.SetTextLocalize(player.ap_max);
    if (num3 > Player.GetApOverChargeLimit())
    {
      num3 = Player.GetApOverChargeLimit();
      num2 = num3 - num1;
    }
    this.txtAP02.SetTextLocalize(num3);
    this.txtAPRecoveryValue.SetTextLocalize(num2);
    if (num3 > player.ap_max)
      this.ApGauge.setValue(player.ap_max, player.ap_max, doTween);
    else
      this.ApGauge.setValue(num3, player.ap_max, doTween);
  }
}
