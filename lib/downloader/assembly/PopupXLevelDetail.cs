// Decompiled with JetBrains decompiler
// Type: PopupXLevelDetail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Popup/Unit/PopupXLevelDetail")]
public class PopupXLevelDetail : BackButtonPopupBase
{
  [Header("上部ユニット情報")]
  [SerializeField]
  private Transform lnkIcon_;
  [SerializeField]
  private UILabel txtLevel_;
  [SerializeField]
  private UILabel txtMaxLevel_;
  [Header("中央レベル表")]
  [SerializeField]
  private UILabel txtBaseLevel_;
  [SerializeField]
  private UILabel txtBaseMaxLevel_;
  [SerializeField]
  private UILabel txtAwakedLevel_;
  [SerializeField]
  private UILabel txtAwakedMaxLevel_;
  [SerializeField]
  private PopupXLevelDetail.JobBoard[] jobBoards_;
  [SerializeField]
  private UILabel txtJobTotalLevel_;
  [Header("下部ボタン")]
  [SerializeField]
  private UIButton btnLevelUp_;
  [SerializeField]
  private UIButton btnReincarnation_;
  private GameObject prefab_;
  private bool isLimitMode_;
  private PlayerUnit target_;
  private Action<PlayerUnit> resultLevelUp_;
  private Action<PlayerUnit> resultReincarnation_;

  public static Future<GameObject> createLoader()
  {
    return new ResourceObject("Prefabs/unit004_2/popup_Detail_Lv").Load<GameObject>();
  }

  public static void show(
    GameObject prefab,
    bool bLimitMode,
    PlayerUnit playerUnit,
    Action<PlayerUnit> resultLevelUp,
    Action<PlayerUnit> resultReincarnation)
  {
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(prefab, isNonSe: true, isNonOpenAnime: true);
    PopupXLevelDetail component = gameObject.GetComponent<PopupXLevelDetail>();
    component.setTopObject(gameObject);
    component.prefab_ = prefab;
    component.isLimitMode_ = bLimitMode;
    component.target_ = playerUnit;
    component.resultLevelUp_ = resultLevelUp;
    component.resultReincarnation_ = resultReincarnation;
  }

  private IEnumerator Start()
  {
    PopupXLevelDetail popupXlevelDetail = this;
    ((Component) popupXlevelDetail).gameObject.GetComponent<UIRect>().alpha = 0.0f;
    MasterDataTable.UnitJob jobData = popupXlevelDetail.target_.getJobData();
    bool flag1 = true;
    bool flag2 = true;
    if (popupXlevelDetail.isLimitMode_ || !jobData.is_vertex_x)
    {
      flag1 = false;
      flag2 = false;
    }
    else if (popupXlevelDetail.target_.x_level == 0)
      flag2 = false;
    bool flag3 = false;
    ((UIButtonColor) popupXlevelDetail.btnLevelUp_).isEnabled = flag1;
    ((UIButtonColor) popupXlevelDetail.btnReincarnation_).isEnabled = flag3;
    Future<GameObject> ldIcon = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    yield return (object) ldIcon.Wait();
    UnitIcon icon = ldIcon.Result.Clone(popupXlevelDetail.lnkIcon_).GetComponent<UnitIcon>();
    icon.RarityCenter();
    yield return (object) icon.SetPlayerUnit(popupXlevelDetail.target_, (PlayerUnit[]) null, (PlayerUnit) null, false, false);
    icon.SetIconBoxCollider(false);
    ldIcon = (Future<GameObject>) null;
    icon = (UnitIcon) null;
    popupXlevelDetail.txtLevel_.SetTextLocalize(popupXlevelDetail.target_.total_level);
    popupXlevelDetail.txtMaxLevel_.SetTextLocalize(popupXlevelDetail.target_.total_max_level);
    UnitUnit unit = popupXlevelDetail.target_.unit;
    UnitUnit unitUnit = (UnitUnit) null;
    if (unit.awake_unit_flag)
    {
      UnitEvolutionPattern[] genealogy = UnitEvolutionPattern.getGenealogy(unit.ID);
      for (int index = genealogy.Length - 1; index >= 0; --index)
      {
        if (genealogy[index].target_unit_UnitUnit == unit.ID)
        {
          unitUnit = genealogy[index].unit;
          break;
        }
      }
    }
    if (unitUnit != null)
    {
      UnitUnitParameter parameterData1 = unit.parameter_data;
      UnitUnitParameter parameterData2 = unitUnit.parameter_data;
      int num1 = 10;
      if (parameterData1 != null && parameterData2 != null)
        num1 = parameterData1._initial_max_level + parameterData1.breakthrough_limit * parameterData1._level_per_breakthrough - (parameterData2._initial_max_level + parameterData2.breakthrough_limit * parameterData2._level_per_breakthrough);
      int num2 = popupXlevelDetail.target_.max_level - num1;
      int num3 = Mathf.Min(popupXlevelDetail.target_.level, num2);
      int num4 = Mathf.Max(popupXlevelDetail.target_.level - num2, 0);
      popupXlevelDetail.txtBaseLevel_.SetTextLocalize(num3);
      popupXlevelDetail.txtBaseMaxLevel_.SetTextLocalize(num2);
      popupXlevelDetail.txtAwakedLevel_.SetTextLocalize(num4);
      popupXlevelDetail.txtAwakedMaxLevel_.SetTextLocalize(num1);
    }
    else
    {
      popupXlevelDetail.txtBaseLevel_.SetTextLocalize(popupXlevelDetail.target_.level);
      popupXlevelDetail.txtBaseMaxLevel_.SetTextLocalize(popupXlevelDetail.target_.max_level);
      popupXlevelDetail.txtAwakedLevel_.SetTextLocalize("-");
      popupXlevelDetail.txtAwakedMaxLevel_.SetTextLocalize("-");
    }
    ldIcon = XParamIcon.createLoader();
    yield return (object) ldIcon.Wait();
    JobChangePatterns jobChangePatterns = JobChangeUtil.getJobChangePatterns(popupXlevelDetail.target_);
    MasterDataTable.UnitJob[] unitJobArray;
    if (jobChangePatterns != null)
      unitJobArray = new MasterDataTable.UnitJob[3]
      {
        jobChangePatterns.job5,
        jobChangePatterns.job6,
        jobChangePatterns.job7
      };
    else
      unitJobArray = new MasterDataTable.UnitJob[popupXlevelDetail.jobBoards_.Length];
    HashSet<int> intSet = popupXlevelDetail.target_.changed_job_ids != null ? new HashSet<int>(((IEnumerable<int?>) popupXlevelDetail.target_.changed_job_ids).Where<int?>((Func<int?, bool>) (x => x.HasValue && x.Value != 0)).Select<int?, int>((Func<int?, int>) (y => y.Value))) : new HashSet<int>();
    int[] numArray = popupXlevelDetail.target_?.x_job_status?.levels ?? new int[unitJobArray.Length];
    XParamIcon component = ldIcon.Result.GetComponent<XParamIcon>();
    for (int index = 0; index < popupXlevelDetail.jobBoards_.Length; ++index)
    {
      PopupXLevelDetail.JobBoard jobBoard = popupXlevelDetail.jobBoards_[index];
      if (unitJobArray[index] != null && intSet.Contains(unitJobArray[index].ID))
      {
        jobBoard.txtLevel.SetTextLocalize(numArray.Length > index ? numArray[index] : 0);
        XJobInformation info;
        MasterData.XJobInformation.TryGetValue(unitJobArray[index].ID, out info);
        popupXlevelDetail.setParamIcons(component, jobBoard.icons, info);
      }
      else
      {
        jobBoard.txtLevel.SetTextLocalize("-");
        popupXlevelDetail.setParamIcons(component, jobBoard.icons, (XJobInformation) null);
      }
    }
    popupXlevelDetail.txtJobTotalLevel_.SetTextLocalize(popupXlevelDetail.target_.max_x_level);
    ldIcon = (Future<GameObject>) null;
    Singleton<PopupManager>.GetInstance().startOpenAnime(((Component) popupXlevelDetail).gameObject);
  }

  private void setParamIcons(XParamIcon paramIcon, UI2DSprite[] targets, XJobInformation info)
  {
    string[] paramKeys = info?.paramKeys;
    UI2DSprite uiSprite = paramIcon.uiSprite;
    for (int index = 0; index < targets.Length; ++index)
    {
      Sprite sprite = paramKeys == null || paramKeys.Length <= index ? (Sprite) null : paramIcon[paramKeys[index]];
      UI2DSprite target = targets[index];
      if (Object.op_Inequality((Object) sprite, (Object) null))
      {
        target.sprite2D = sprite;
        ((UIWidget) target).width = ((UIWidget) uiSprite).width;
        ((UIWidget) target).height = ((UIWidget) uiSprite).height;
        ((Component) target).gameObject.SetActive(true);
      }
      else
        ((Component) target).gameObject.SetActive(false);
    }
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public void onClickedLevelUp()
  {
    if (this.isLimitMode_)
      return;
    if (this.target_.x_level >= this.target_.max_x_level)
    {
      Consts instance = Consts.GetInstance();
      Singleton<PopupManager>.GetInstance().monitorCoroutine(PopupCommon.Show(instance.POPUP_XLEVELUP_TITLE_LEVEL_LIMIT, instance.POPUP_XLEVELUP_MESSAGE_LEVEL_LIMIT));
    }
    else
    {
      if (this.IsPushAndSet())
        return;
      this.StartCoroutine(this.doPopup(PopupXLevelUp.createLoader(), (Action<GameObject>) (prefab => PopupXLevelUp.show(prefab, this.target_, new Action<PlayerUnit>(this.onLevelUp), (Action) (() => this.IsPush = false)))));
    }
  }

  public void onClickedReincarnation()
  {
    if (this.isLimitMode_ || this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  private IEnumerator doPopup(Future<GameObject> ldPrefab, Action<GameObject> open)
  {
    yield return (object) ldPrefab.Wait();
    open(ldPrefab.Result);
  }

  private void onLevelUp(PlayerUnit result)
  {
    Singleton<PopupManager>.GetInstance().monitorCoroutine(PopupXLevelDetail.doLevelUp(this.prefab_, this.target_, result, this.resultLevelUp_, this.resultReincarnation_));
  }

  private static IEnumerator doLevelUp(
    GameObject detailPrefab,
    PlayerUnit before,
    PlayerUnit after,
    Action<PlayerUnit> resultLevelUp,
    Action<PlayerUnit> resultReincarnation)
  {
    Future<GameObject> ldResult = PopupXLevelUpResult.createLoader();
    yield return (object) ldResult.Wait();
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    bool bWait = true;
    PopupXLevelUpResult.show(ldResult.Result, before, after, (Action) (() => bWait = false));
    while (bWait)
      yield return (object) null;
    resultLevelUp(after);
    PopupXLevelDetail.show(detailPrefab, false, after, resultLevelUp, resultReincarnation);
  }

  [Serializable]
  private class JobBoard
  {
    public UI2DSprite[] icons;
    public UILabel txtLevel;
  }
}
