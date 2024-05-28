// Decompiled with JetBrains decompiler
// Type: PopupXLevelUpResult
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
[AddComponentMenu("Popup/Unit/PopupXLevelUpResult")]
public class PopupXLevelUpResult : BackButtonPopupBase
{
  private static readonly Color NORMAL_COLOR = Color.white;
  private static readonly Color CHANGED_COLOR = Color.yellow;
  [SerializeField]
  private Transform lnkIcon_;
  [SerializeField]
  private UILabel txtLevel_;
  [SerializeField]
  private UILabel txtResultLevel_;
  [SerializeField]
  private UILabel txtMaxLevel_;
  [SerializeField]
  private UIButton btnClose_;
  [SerializeField]
  private NGTweenGaugeScale expGauge_;
  [SerializeField]
  private GameObject objSkip_;
  [SerializeField]
  private PopupXLevelUpResult.ParamControl[] params_;
  private PlayerUnit before_;
  private PlayerUnit after_;
  private int maxLevel_;
  private int fromExp_;
  private int toExp_;
  private int level_;
  private int exp_;
  private int currentLevelStartExp_;
  private int currentLevelMaxExp_;
  private float factor_;
  private bool isWait_ = true;
  private bool isEnd_;
  private Action onEnd_;

  public static Future<GameObject> createLoader()
  {
    return new ResourceObject("Prefabs/unit004_2/popup_XLvUp_StatusUp").Load<GameObject>();
  }

  public static void show(GameObject prefab, PlayerUnit before, PlayerUnit after, Action eventEnd)
  {
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(prefab, isNonSe: true, isNonOpenAnime: true);
    PopupXLevelUpResult component = gameObject.GetComponent<PopupXLevelUpResult>();
    component.setTopObject(gameObject);
    component.before_ = before;
    component.after_ = after;
    component.onEnd_ = eventEnd;
  }

  private IEnumerator Start()
  {
    PopupXLevelUpResult popupXlevelUpResult1 = this;
    ((Component) popupXlevelUpResult1).gameObject.GetComponent<UIRect>().alpha = 0.0f;
    ((UIButtonColor) popupXlevelUpResult1.btnClose_).isEnabled = false;
    int[] numArray1 = popupXlevelUpResult1.getParams(popupXlevelUpResult1.before_);
    int[] numArray2 = popupXlevelUpResult1.getParams(popupXlevelUpResult1.after_);
    for (int index = 0; index < popupXlevelUpResult1.params_.Length; ++index)
    {
      PopupXLevelUpResult.ParamControl paramControl = popupXlevelUpResult1.params_[index];
      paramControl.from_ = numArray1[index];
      paramControl.to_ = numArray2[index];
      paramControl.setStart();
    }
    Future<GameObject> ldIcon = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    yield return (object) ldIcon.Wait();
    UnitIcon icon = ldIcon.Result.Clone(popupXlevelUpResult1.lnkIcon_).GetComponent<UnitIcon>();
    icon.RarityCenter();
    yield return (object) icon.SetPlayerUnit(popupXlevelUpResult1.before_, (PlayerUnit[]) null, (PlayerUnit) null, false, false);
    icon.SetIconBoxCollider(false);
    ldIcon = (Future<GameObject>) null;
    icon = (UnitIcon) null;
    popupXlevelUpResult1.maxLevel_ = popupXlevelUpResult1.after_.max_x_level;
    PopupXLevelUpResult popupXlevelUpResult2 = popupXlevelUpResult1;
    PlayerUnitXJobStatus xJobStatus = popupXlevelUpResult1.before_.x_job_status;
    int totalExp = xJobStatus != null ? xJobStatus.total_exp : 0;
    popupXlevelUpResult2.fromExp_ = totalExp;
    popupXlevelUpResult1.toExp_ = popupXlevelUpResult1.after_.x_job_status.total_exp;
    int level = UnitXLevel.expToLevel(popupXlevelUpResult1.fromExp_);
    popupXlevelUpResult1.txtLevel_.SetTextLocalize(level);
    popupXlevelUpResult1.txtResultLevel_.SetTextLocalize(level);
    ((UIWidget) popupXlevelUpResult1.txtResultLevel_).color = PopupXLevelUpResult.NORMAL_COLOR;
    popupXlevelUpResult1.txtMaxLevel_.SetTextLocalize("/" + (object) popupXlevelUpResult1.maxLevel_);
    popupXlevelUpResult1.updateInfo(false, true);
    Singleton<PopupManager>.GetInstance().startOpenAnime(((Component) popupXlevelUpResult1).gameObject);
    popupXlevelUpResult1.StartCoroutine(popupXlevelUpResult1.doWait());
  }

  private IEnumerator doWait()
  {
    yield return (object) new WaitForSeconds(1f);
    this.isWait_ = false;
  }

  private int[] getParams(PlayerUnit target)
  {
    return new int[8]
    {
      target.hp.x_level,
      target.strength.x_level,
      target.intelligence.x_level,
      target.vitality.x_level,
      target.mind.x_level,
      target.agility.x_level,
      target.dexterity.x_level,
      target.lucky.x_level
    };
  }

  protected override void Update()
  {
    if (this.isWait_)
      return;
    base.Update();
    this.updateInfo(false, false);
  }

  public void onClickedSkip()
  {
    if (this.isWait_ || this.isEnd_)
      return;
    this.exp_ = this.toExp_;
    this.updateInfo(true, false);
  }

  private void updateInfo(bool bSkip, bool bInit)
  {
    if (this.isEnd_)
      return;
    if (bInit)
      this.startExpUp(true);
    else if (this.exp_ < this.toExp_)
    {
      if ((double) this.factor_ < 1.0)
      {
        this.factor_ = Mathf.Clamp(this.factor_ + Time.deltaTime * 4f, 0.0f, 1f);
      }
      else
      {
        Singleton<NGSoundManager>.GetInstance().PlaySe("SE_1018");
        this.startExpUp(false);
      }
    }
    else
    {
      this.objSkip_.SetActive(false);
      this.isEnd_ = true;
      this.StartCoroutine(this.doStartParamUp(bSkip));
    }
    if (this.exp_ >= this.toExp_)
    {
      UnitXLevel data = UnitXLevel.expToData(this.toExp_);
      this.level_ = data.level;
      this.txtResultLevel_.SetTextLocalize(this.level_);
      this.currentLevelStartExp_ = data.from_exp;
      this.currentLevelMaxExp_ = data.to_exp;
      if (this.currentLevelStartExp_ < this.currentLevelMaxExp_)
        this.expGauge_.setValue(this.toExp_ - this.currentLevelStartExp_, this.currentLevelMaxExp_ - this.currentLevelStartExp_, false);
      else
        this.expGauge_.setValue(0, 1, false);
    }
    else
    {
      if (this.currentLevelStartExp_ < this.currentLevelMaxExp_)
      {
        this.exp_ = Mathf.Min(Mathf.RoundToInt((float) (this.currentLevelMaxExp_ - this.currentLevelStartExp_) * this.factor_) + this.currentLevelStartExp_, this.toExp_);
        this.expGauge_.setValue(this.exp_ - this.currentLevelStartExp_, this.currentLevelMaxExp_ - this.currentLevelStartExp_, false);
      }
      else
      {
        this.exp_ = this.toExp_;
        this.expGauge_.setValue(0, 1, false);
      }
      this.txtResultLevel_.SetTextLocalize(UnitXLevel.expToLevel(this.exp_));
    }
    if (!(this.txtLevel_.text != this.txtResultLevel_.text))
      return;
    ((UIWidget) this.txtResultLevel_).color = PopupXLevelUpResult.CHANGED_COLOR;
  }

  private void startExpUp(bool bFirst)
  {
    if (bFirst)
      this.exp_ = this.fromExp_;
    UnitXLevel data = UnitXLevel.expToData(bFirst ? this.fromExp_ : this.currentLevelMaxExp_ + 1);
    this.level_ = data.level;
    this.currentLevelStartExp_ = data.from_exp;
    this.currentLevelMaxExp_ = data.to_exp;
    if (this.currentLevelStartExp_ < this.fromExp_ && this.currentLevelStartExp_ != this.currentLevelMaxExp_)
      this.factor_ = (float) (this.fromExp_ - this.currentLevelStartExp_) / (float) (this.currentLevelMaxExp_ - this.currentLevelStartExp_);
    else
      this.factor_ = 0.0f;
  }

  private IEnumerator doStartParamUp(bool bSkip)
  {
    PopupXLevelUpResult popupXlevelUpResult = this;
    if (bSkip)
    {
      for (int index = 0; index < popupXlevelUpResult.params_.Length; ++index)
        popupXlevelUpResult.params_[index].setEnd();
    }
    else
    {
      for (int n = 0; n < popupXlevelUpResult.params_.Length; ++n)
      {
        PopupXLevelUpResult.ParamControl paramControl = popupXlevelUpResult.params_[n];
        paramControl.setEnd();
        if (paramControl.from_ != paramControl.to_)
        {
          Singleton<NGSoundManager>.GetInstance().PlaySe("SE_1019");
          yield return (object) new WaitForSeconds(0.4f);
        }
      }
    }
    ((UIButtonColor) popupXlevelUpResult.btnClose_).isEnabled = true;
    popupXlevelUpResult.IsPush = false;
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
    this.onEnd_();
  }

  private enum ParamType
  {
    Hp,
    Strength,
    Intelligence,
    Vitality,
    Mind,
    Agility,
    Dexterity,
    Lucky,
  }

  [Serializable]
  private class ParamControl
  {
    public UILabel txtNumber;
    public GameObject topPlus;
    public UILabel txtPlus;

    public int from_ { get; set; }

    public int to_ { get; set; }

    public void setStart()
    {
      this.txtNumber.SetTextLocalize(this.from_);
      ((UIWidget) this.txtNumber).color = PopupXLevelUpResult.NORMAL_COLOR;
      this.topPlus.SetActive(false);
    }

    public void setEnd()
    {
      if (this.from_ == this.to_)
        return;
      this.txtNumber.SetTextLocalize(this.to_);
      ((UIWidget) this.txtNumber).color = PopupXLevelUpResult.CHANGED_COLOR;
      this.txtPlus.SetTextLocalize(this.to_ - this.from_);
      this.topPlus.SetActive(true);
    }
  }
}
