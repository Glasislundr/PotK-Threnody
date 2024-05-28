// Decompiled with JetBrains decompiler
// Type: PopupUnitRegression
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
[AddComponentMenu("Popup/Unit/Regression")]
public class PopupUnitRegression : BackButtonPopupBase
{
  [SerializeField]
  private PopupUnitRegression.Step step_;
  [SerializeField]
  private Transform lnkBefore_;
  [SerializeField]
  private LimitBreakIndicator indicatorBefore_;
  [SerializeField]
  private Transform lnkAfter_;
  [SerializeField]
  private LimitBreakIndicator indicatorAfter_;
  [SerializeField]
  private UILabel txtNum_;
  [SerializeField]
  private UIButton btnRegression_;
  [SerializeField]
  [Tooltip("演出コントロール用")]
  private Animator animator_;
  private GameObject objBefore_;
  private GameObject objAfter_;
  private bool isWait_;
  private bool isSkipped_;
  private PlayerUnit target_;
  private UnitUnit unitRegression_;
  private Action actNext_;
  private Action actClose_;
  private Action actHelp_;
  private Action actItem_;
  private Action actUnitDetail_;
  private Action oneshotOnTweenFinished_;

  public void initailize(
    GameObject prefabIcon,
    PlayerUnit target,
    int regressionId,
    Action actNext,
    Action actClose = null,
    Action actHelp = null,
    Action actItem = null,
    Action actUnitDetail = null)
  {
    this.setTopObject(((Component) this).gameObject);
    if (Object.op_Inequality((Object) this.lnkBefore_, (Object) null))
      this.objBefore_ = prefabIcon.Clone(this.lnkBefore_);
    if (Object.op_Inequality((Object) this.lnkAfter_, (Object) null))
      this.objAfter_ = prefabIcon.Clone(this.lnkAfter_);
    ((UIRect) ((Component) this).GetComponent<UIPanel>()).alpha = 0.0f;
    this.isWait_ = true;
    this.isSkipped_ = false;
    this.target_ = target;
    this.unitRegression_ = regressionId != 0 ? MasterData.UnitUnit[regressionId] : (UnitUnit) null;
    if (actClose == null)
      actClose = actNext;
    this.actNext_ = actNext;
    this.actClose_ = actClose;
    this.actHelp_ = actHelp;
    this.actItem_ = actItem;
    this.actUnitDetail_ = actUnitDetail;
    Consts instance = Consts.GetInstance();
    int itemId = instance.ITEM_REGRESSION_ID;
    int regressionQuantity = instance.ITEM_REGRESSION_QUANTITY;
    PlayerMaterialUnit playerMaterialUnit = Array.Find<PlayerMaterialUnit>(SMManager.Get<PlayerMaterialUnit[]>(), (Predicate<PlayerMaterialUnit>) (x => x._unit == itemId));
    int quantity = playerMaterialUnit != null ? playerMaterialUnit.quantity : 0;
    if (Object.op_Inequality((Object) this.txtNum_, (Object) null))
    {
      this.txtNum_.SetTextLocalize(Consts.Format(instance.unit_004_9_9_possession_text, (IDictionary) new Hashtable()
      {
        {
          (object) "Count",
          (object) quantity
        }
      }));
      ((UIWidget) this.txtNum_).color = quantity >= regressionQuantity ? Color.yellow : Color.red;
    }
    if (!Object.op_Inequality((Object) this.btnRegression_, (Object) null))
      return;
    ((UIButtonColor) this.btnRegression_).isEnabled = this.step_ != PopupUnitRegression.Step._Fin ? quantity >= regressionQuantity : Object.op_Equality((Object) this.animator_, (Object) null);
  }

  private IEnumerator Start()
  {
    PopupUnitRegression popupUnitRegression = this;
    if (Object.op_Inequality((Object) popupUnitRegression.objBefore_, (Object) null))
    {
      UnitIcon component = popupUnitRegression.objBefore_.GetComponent<UnitIcon>();
      yield return (object) popupUnitRegression.initIcon(component, popupUnitRegression.target_);
    }
    if (Object.op_Inequality((Object) popupUnitRegression.indicatorBefore_, (Object) null))
      popupUnitRegression.indicatorBefore_.set(popupUnitRegression.target_.breakthrough_count, popupUnitRegression.target_.unit.breakthrough_limit);
    if (Object.op_Inequality((Object) popupUnitRegression.objAfter_, (Object) null))
    {
      UnitIcon component = popupUnitRegression.objAfter_.GetComponent<UnitIcon>();
      PlayerUnit byUnitunit = PlayerUnit.create_by_unitunit(popupUnitRegression.unitRegression_, 1);
      byUnitunit._unit_type = popupUnitRegression.target_._unit_type;
      for (int index = 0; index < Hard.MasterDataTable.Data.UnitRegressionUnitTypes.Length; ++index)
      {
        if (Hard.MasterDataTable.Data.UnitRegressionUnitTypes[index].IsMatch(popupUnitRegression.target_))
        {
          byUnitunit._unit_type = (int) Hard.MasterDataTable.Data.UnitRegressionUnitTypes[index].target_type;
          break;
        }
      }
      yield return (object) popupUnitRegression.initIcon(component, byUnitunit);
    }
    if (Object.op_Inequality((Object) popupUnitRegression.indicatorAfter_, (Object) null))
      popupUnitRegression.indicatorAfter_.set(Mathf.Min(popupUnitRegression.target_.unity_value, popupUnitRegression.unitRegression_.breakthrough_limit), popupUnitRegression.unitRegression_.breakthrough_limit);
    if (popupUnitRegression.step_ == PopupUnitRegression.Step._Fin)
      popupUnitRegression.changeIconToAfter(false);
    // ISSUE: reference to a compiler-generated method
    popupUnitRegression.oneshotOnTweenFinished_ = new Action(popupUnitRegression.\u003CStart\u003Eb__22_0);
    popupUnitRegression.isWait_ = false;
    Singleton<PopupManager>.GetInstance().startOpenAnime(((Component) popupUnitRegression).gameObject);
  }

  private IEnumerator initIcon(UnitIcon icon, PlayerUnit unit)
  {
    yield return (object) icon.SetPlayerUnit(unit, (PlayerUnit[]) null, (PlayerUnit) null, false, false);
    icon.ShowBottomInfos(UnitSortAndFilter.SORT_TYPES.Level);
    icon.setLevelText(unit);
    icon.SetIconBoxCollider(false);
  }

  public void onTweenFinished()
  {
    if (this.oneshotOnTweenFinished_ == null)
      return;
    this.oneshotOnTweenFinished_();
    this.oneshotOnTweenFinished_ = (Action) null;
  }

  private IEnumerator doWaitEffect()
  {
    PopupUnitRegression popupUnitRegression = this;
    yield return (object) new WaitForAnimation(popupUnitRegression.animator_);
    if (Object.op_Inequality((Object) popupUnitRegression.btnRegression_, (Object) null))
      ((UIButtonColor) popupUnitRegression.btnRegression_).isEnabled = true;
    if (popupUnitRegression.isSkipped_)
    {
      clipEffectPlayer componentInChildren = ((Component) popupUnitRegression).gameObject.GetComponentInChildren<clipEffectPlayer>();
      NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
      if (Object.op_Inequality((Object) componentInChildren, (Object) null) && Object.op_Inequality((Object) instance, (Object) null) && componentInChildren.lastPlaySound != -1)
        instance.StopSe(componentInChildren.lastPlaySound);
    }
    popupUnitRegression.isSkipped_ = true;
    if (Object.op_Inequality((Object) popupUnitRegression.objBefore_, (Object) null) && Object.op_Inequality((Object) popupUnitRegression.objAfter_, (Object) null) && popupUnitRegression.objBefore_.activeSelf)
      popupUnitRegression.changeIconToAfter(true);
  }

  public override void onBackButton() => this.onClickedClose();

  public void onClickedClose()
  {
    if (this.isWait_)
      return;
    this.actClose_();
  }

  public void onClickedRegression()
  {
    if (this.isWait_)
      return;
    this.actNext_();
  }

  public void onClickedHelp()
  {
    if (this.isWait_ || this.actHelp_ == null)
      return;
    this.actHelp_();
  }

  public void onClickedItemIcon()
  {
    if (this.isWait_ || this.actItem_ == null)
      return;
    this.actItem_();
  }

  public void onClickedUnitDetail()
  {
    if (this.isWait_ || this.target_ == (PlayerUnit) null)
      return;
    this.actUnitDetail_();
    Unit0042Scene.changeSceneEvolutionUnit(true, this.target_, (PlayerUnit[]) null);
  }

  public void onClickedSkip()
  {
    if (this.isWait_ || this.isSkipped_ || Object.op_Equality((Object) this.animator_, (Object) null))
      return;
    this.isSkipped_ = true;
    this.animator_.speed *= 5f;
  }

  public void changeIconToAfter(bool bToAfter)
  {
    if (Object.op_Inequality((Object) this.objBefore_, (Object) null))
      this.objBefore_.SetActive(!bToAfter);
    if (!Object.op_Inequality((Object) this.objAfter_, (Object) null))
      return;
    this.objAfter_.SetActive(bToAfter);
  }

  public enum Step
  {
    _1st,
    _2nd,
    _Fin,
  }
}
