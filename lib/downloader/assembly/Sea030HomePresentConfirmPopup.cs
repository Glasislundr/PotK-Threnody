// Decompiled with JetBrains decompiler
// Type: Sea030HomePresentConfirmPopup
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
public class Sea030HomePresentConfirmPopup : BackButtonMonoBehaiviour
{
  [SerializeField]
  private GameObject dirCauction;
  [SerializeField]
  private UILabel itemSelected;
  [SerializeField]
  private UILabel itemPssession;
  [SerializeField]
  private GameObject numberThum;
  [SerializeField]
  private UIButton decideButton;
  [SerializeField]
  private UISlider slider;
  [SerializeField]
  private GameObject mark;
  [SerializeField]
  private LongPressButton plusButton;
  [SerializeField]
  private LongPressButton minusButton;
  [SerializeField]
  private GameObject thumRoot;
  [SerializeField]
  private LoveGaugeController loveGauge;
  [SerializeField]
  private UILabel amount;
  private int selectedCount = 1;
  private int maxCount;
  private GameCore.ItemInfo presentInfo;
  private float baseTrustUp;
  private PlayerUnit currentUnit;
  private Action<GameCore.ItemInfo, int> apiCall;
  private bool isPush;
  private const int trustMaXCount = 20;

  public IEnumerator Init(PlayerUnit unit, GameCore.ItemInfo present, Action<GameCore.ItemInfo, int> apiCallback)
  {
    Sea030HomePresentConfirmPopup presentConfirmPopup = this;
    SeaPresentPresent seaPresentPresent = ((IEnumerable<SeaPresentPresent>) MasterData.SeaPresentPresentList).FirstOrDefault<SeaPresentPresent>((Func<SeaPresentPresent, bool>) (x => x.gear_id == present.gear.ID));
    SeaPresentPresentAffinity presentPresentAffnity = SeaPresentPresentAffinity.GetSeaPresentPresentAffnity(unit.unit, present.gear.ID);
    presentConfirmPopup.baseTrustUp = (float) ((seaPresentPresent != null ? (double) seaPresentPresent.trust_base : 0.0) + (presentPresentAffnity != null ? (double) presentPresentAffnity.affinity.coefficient : 0.0));
    presentConfirmPopup.apiCall = apiCallback;
    presentConfirmPopup.presentInfo = present;
    presentConfirmPopup.currentUnit = unit;
    int num1 = (int) Math.Ceiling(((Decimal) unit.trust_max_rate - (Decimal) unit.trust_rate) / (Decimal) presentConfirmPopup.baseTrustUp);
    presentConfirmPopup.maxCount = Mathf.Min(num1, presentConfirmPopup.presentInfo.quantity);
    presentConfirmPopup.plusButton.onLongPressLoop = new Func<IEnumerator>(presentConfirmPopup.OnPlusLoop);
    presentConfirmPopup.minusButton.onLongPressLoop = new Func<IEnumerator>(presentConfirmPopup.OnMinusLoop);
    if ((double) unit.trust_rate < (double) unit.trust_max_rate)
    {
      presentConfirmPopup.selectedCount = 1;
      ((UIButtonColor) presentConfirmPopup.plusButton).isEnabled = presentConfirmPopup.presentInfo.quantity > 0;
      ((UIButtonColor) presentConfirmPopup.minusButton).isEnabled = presentConfirmPopup.presentInfo.quantity > 0;
      ((UIButtonColor) presentConfirmPopup.decideButton).isEnabled = true;
      presentConfirmPopup.itemSelected.SetTextLocalize(presentConfirmPopup.maxCount);
      ((UIProgressBar) presentConfirmPopup.slider).numberOfSteps = presentConfirmPopup.maxCount + 1;
      presentConfirmPopup.mark.SetActive(true);
      presentConfirmPopup.UpdateInfo();
    }
    else
    {
      presentConfirmPopup.selectedCount = 1;
      ((UIButtonColor) presentConfirmPopup.decideButton).isEnabled = true;
      presentConfirmPopup.maxCount = Mathf.Min(20, presentConfirmPopup.presentInfo.quantity);
      presentConfirmPopup.itemSelected.SetTextLocalize(presentConfirmPopup.maxCount);
      ((UIProgressBar) presentConfirmPopup.slider).numberOfSteps = presentConfirmPopup.maxCount + 1;
      presentConfirmPopup.itemPssession.SetTextLocalize(presentConfirmPopup.selectedCount);
      presentConfirmPopup.mark.SetActive(true);
      presentConfirmPopup.UpdateInfo();
    }
    presentConfirmPopup.dirCauction.SetActive((double) presentConfirmPopup.currentUnit.trust_max_rate <= (double) presentConfirmPopup.currentUnit.trust_rate);
    double num2 = Math.Round((double) presentConfirmPopup.currentUnit.trust_rate * 100.0) / 100.0;
    presentConfirmPopup.amount.SetTextLocalize(string.Format("{0}{1}", (object) num2, (object) Consts.GetInstance().PERCENT));
    presentConfirmPopup.StartCoroutine(presentConfirmPopup.loveGauge.setValue((int) presentConfirmPopup.currentUnit.trust_rate, (int) presentConfirmPopup.currentUnit.trust_rate, (int) presentConfirmPopup.currentUnit.trust_max_rate, (int) Consts.GetInstance().TRUST_RATE_LEVEL_SIZE, false));
    Future<GameObject> iconFuture = Res.Prefabs.Sea.ItemIcon.prefab_sea.Load<GameObject>();
    IEnumerator e = iconFuture.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = iconFuture.Result.CloneAndGetComponent<ItemIcon>(presentConfirmPopup.thumRoot).InitByItemInfo(present);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    List<SwitchUnitComponentBase> list = ((IEnumerable<SwitchUnitComponentBase>) ((Component) presentConfirmPopup).GetComponentsInChildren<SwitchUnitComponentBase>(true)).ToList<SwitchUnitComponentBase>();
    for (int index = 0; index < list.Count; ++index)
      list[index].SwitchMaterial(unit.unit.ID);
  }

  private bool isPushAndSet()
  {
    if (this.isPush)
      return true;
    this.isPush = true;
    return false;
  }

  public void UpdateInfo()
  {
    this.itemPssession.SetTextLocalize(this.selectedCount);
    ((UIProgressBar) this.slider).value = (float) this.selectedCount / (float) this.maxCount;
  }

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().dismiss();

  public void OnDecide()
  {
    if (this.isPushAndSet())
      return;
    if (this.selectedCount <= 0)
      Singleton<PopupManager>.GetInstance().dismiss();
    else
      this.apiCall(this.presentInfo, this.selectedCount);
  }

  public void OnPlus()
  {
    this.selectedCount = Mathf.Min(++this.selectedCount, this.maxCount);
    this.UpdateInfo();
  }

  public void OnMinus()
  {
    this.selectedCount = Mathf.Max(--this.selectedCount, 0);
    this.UpdateInfo();
  }

  public IEnumerator OnPlusLoop()
  {
    Sea030HomePresentConfirmPopup presentConfirmPopup1 = this;
    while (presentConfirmPopup1.selectedCount < presentConfirmPopup1.maxCount)
    {
      Sea030HomePresentConfirmPopup presentConfirmPopup2 = presentConfirmPopup1;
      Sea030HomePresentConfirmPopup presentConfirmPopup3 = presentConfirmPopup1;
      int num1 = presentConfirmPopup1.selectedCount + 1;
      int num2 = num1;
      presentConfirmPopup3.selectedCount = num2;
      int num3 = Mathf.Min(num1, presentConfirmPopup1.maxCount);
      presentConfirmPopup2.selectedCount = num3;
      presentConfirmPopup1.UpdateInfo();
      yield return (object) null;
    }
  }

  public IEnumerator OnMinusLoop()
  {
    Sea030HomePresentConfirmPopup presentConfirmPopup1 = this;
    while (presentConfirmPopup1.selectedCount > 0)
    {
      Sea030HomePresentConfirmPopup presentConfirmPopup2 = presentConfirmPopup1;
      Sea030HomePresentConfirmPopup presentConfirmPopup3 = presentConfirmPopup1;
      int num1 = presentConfirmPopup1.selectedCount - 1;
      int num2 = num1;
      presentConfirmPopup3.selectedCount = num2;
      int num3 = Mathf.Max(num1, 0);
      presentConfirmPopup2.selectedCount = num3;
      presentConfirmPopup1.UpdateInfo();
      yield return (object) null;
    }
  }

  public void OnValueChange()
  {
    this.selectedCount = Mathf.RoundToInt(((UIProgressBar) this.slider).value * (float) this.maxCount);
    this.UpdateInfo();
  }
}
