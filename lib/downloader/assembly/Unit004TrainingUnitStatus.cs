// Decompiled with JetBrains decompiler
// Type: Unit004TrainingUnitStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using UnitStatusInformation;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Unit/Training/UnitStatus")]
public class Unit004TrainingUnitStatus : MonoBehaviour
{
  [SerializeField]
  [Tooltip("表示形式")]
  private Unit004TrainingUnitStatus.LayoutType layout_;
  [Header("従来の情報")]
  [SerializeField]
  private UILabel txtHp;
  [SerializeField]
  private UILabel txtJobname;
  [SerializeField]
  private UILabel txtLucky;
  [SerializeField]
  private UILabel txtLv;
  [SerializeField]
  private UILabel txtLvmax;
  [SerializeField]
  private UILabel txtMagic;
  [SerializeField]
  private UILabel txtPower;
  [SerializeField]
  private UILabel txtProtect;
  [SerializeField]
  private UILabel txtSpeed;
  [SerializeField]
  private UILabel txtSpirit;
  [SerializeField]
  private UILabel txtTechnique;
  [SerializeField]
  private UILabel txtCost;
  [SerializeField]
  private GameObject hpStatusMaxStar;
  [SerializeField]
  private GameObject powerStatusMaxStar;
  [SerializeField]
  private GameObject magicStatusMaxStar;
  [SerializeField]
  private GameObject protectStatusMaxStar;
  [SerializeField]
  private GameObject spiritStatusMaxStar;
  [SerializeField]
  private GameObject speedStatusMaxStar;
  [SerializeField]
  private GameObject techniqueStatusMaxStar;
  [SerializeField]
  private GameObject luckyStatusMaxStar;
  [SerializeField]
  private GameObject[] objLimitBreaks;
  [SerializeField]
  private GameObject[] objLimitBreakBlanks;
  public GameObject lnkUnit;
  [Header("進化用追加情報")]
  [SerializeField]
  private Color colorNormalTakeover_;
  [SerializeField]
  private Color colorMaxTakeover_;
  [SerializeField]
  private GrowthDegrees growthDegrees_;
  [SerializeField]
  private Unit004TrainingUnitStatus.ParamDetail[] paramDetails_;
  private PlayerUnit current_;

  public GrowthDegree growthDegree { get; private set; }

  private void initNumber(int v1, UILabel source)
  {
    if (Object.op_Equality((Object) source, (Object) null))
      return;
    source.SetTextLocalize(v1);
  }

  private void setStatusMaxStar(GameObject go, bool isDisp)
  {
    if (!Object.op_Inequality((Object) go, (Object) null))
      return;
    go.SetActive(isDisp);
  }

  public void SetStatusText(PlayerUnit playerUnit, bool disableCheckColor = false)
  {
    this.current_ = playerUnit;
    UnitUnit unit = playerUnit.unit;
    if (this.layout_ == Unit004TrainingUnitStatus.LayoutType.Reincarnation)
    {
      this.initNumber(playerUnit.self_hp_without_x, this.txtHp);
      this.initNumber(playerUnit.self_strength_without_x, this.txtPower);
      this.initNumber(playerUnit.self_intelligence_without_x, this.txtMagic);
      this.initNumber(playerUnit.self_vitality_without_x, this.txtProtect);
      this.initNumber(playerUnit.self_mind_without_x, this.txtSpirit);
      this.initNumber(playerUnit.self_agility_without_x, this.txtSpeed);
      this.initNumber(playerUnit.self_dexterity_without_x, this.txtTechnique);
      this.initNumber(playerUnit.self_lucky_without_x, this.txtLucky);
    }
    else
    {
      this.initNumber(playerUnit.self_total_hp, this.txtHp);
      this.initNumber(playerUnit.self_total_strength, this.txtPower);
      this.initNumber(playerUnit.self_total_intelligence, this.txtMagic);
      this.initNumber(playerUnit.self_total_vitality, this.txtProtect);
      this.initNumber(playerUnit.self_total_mind, this.txtSpirit);
      this.initNumber(playerUnit.self_total_agility, this.txtSpeed);
      this.initNumber(playerUnit.self_total_dexterity, this.txtTechnique);
      this.initNumber(playerUnit.self_total_lucky, this.txtLucky);
    }
    this.initNumber(playerUnit.level, this.txtLv);
    this.initNumber(playerUnit.max_level, this.txtLvmax);
    this.setStatusMaxStar(this.hpStatusMaxStar, playerUnit.hp.is_max);
    this.setStatusMaxStar(this.powerStatusMaxStar, playerUnit.strength.is_max);
    this.setStatusMaxStar(this.magicStatusMaxStar, playerUnit.intelligence.is_max);
    this.setStatusMaxStar(this.protectStatusMaxStar, playerUnit.vitality.is_max);
    this.setStatusMaxStar(this.spiritStatusMaxStar, playerUnit.mind.is_max);
    this.setStatusMaxStar(this.speedStatusMaxStar, playerUnit.agility.is_max);
    this.setStatusMaxStar(this.techniqueStatusMaxStar, playerUnit.dexterity.is_max);
    this.setStatusMaxStar(this.luckyStatusMaxStar, playerUnit.lucky.is_max);
    this.txtCost.SetTextLocalize(playerUnit.cost);
    this.txtJobname.SetTextLocalize(playerUnit.getJobData().name);
    this.setLimitBreak(playerUnit, unit);
    this.setParamDetails((PlayerUnit) null, disableCheckColor);
  }

  private void setLimitBreak(PlayerUnit target, UnitUnit tUnit)
  {
    if (this.objLimitBreaks == null || this.objLimitBreaks.Length == 0 || this.objLimitBreaks.Length != this.objLimitBreakBlanks.Length)
      return;
    int num1 = Mathf.Min(target.breakthrough_count, this.objLimitBreaks.Length);
    int num2 = Mathf.Min(tUnit.breakthrough_limit, this.objLimitBreakBlanks.Length);
    for (int index = 0; index < this.objLimitBreaks.Length; ++index)
    {
      bool flag = index < num1;
      this.objLimitBreaks[index].SetActive(flag);
      this.objLimitBreakBlanks[index].SetActive(!flag && index < num2);
    }
  }

  public void setParamDetails(PlayerUnit target, bool disableCheckColor = false)
  {
    if (this.layout_ <= Unit004TrainingUnitStatus.LayoutType.Normal)
      return;
    PlayerUnit playerUnit1 = this.layout_ != Unit004TrainingUnitStatus.LayoutType.EvoBefore || !(target != (PlayerUnit) null) ? this.current_ : target;
    int[] maxTakeovers = (int[]) null;
    int[] takeovers = (int[]) null;
    if (this.layout_ != Unit004TrainingUnitStatus.LayoutType.EvoBefore || target != (PlayerUnit) null)
    {
      PlayerUnit playerUnit2 = this.layout_ != Unit004TrainingUnitStatus.LayoutType.EvoAfter || !(target != (PlayerUnit) null) ? this.current_ : target;
      maxTakeovers = new int[8]
      {
        Util.CalcMaxTakeover(playerUnit2.hp.initial, playerUnit2.hp.inheritance, playerUnit2.hp.level_up_max_status, playerUnit2.compose_hp_max),
        Util.CalcMaxTakeover(playerUnit2.lucky.initial, playerUnit2.lucky.inheritance, playerUnit2.lucky.level_up_max_status, playerUnit2.compose_lucky_max),
        Util.CalcMaxTakeover(playerUnit2.intelligence.initial, playerUnit2.intelligence.inheritance, playerUnit2.intelligence.level_up_max_status, playerUnit2.compose_intelligence_max),
        Util.CalcMaxTakeover(playerUnit2.strength.initial, playerUnit2.strength.inheritance, playerUnit2.strength.level_up_max_status, playerUnit2.compose_strength_max),
        Util.CalcMaxTakeover(playerUnit2.vitality.initial, playerUnit2.vitality.inheritance, playerUnit2.vitality.level_up_max_status, playerUnit2.compose_vitality_max),
        Util.CalcMaxTakeover(playerUnit2.agility.initial, playerUnit2.agility.inheritance, playerUnit2.agility.level_up_max_status, playerUnit2.compose_agility_max),
        Util.CalcMaxTakeover(playerUnit2.mind.initial, playerUnit2.mind.inheritance, playerUnit2.mind.level_up_max_status, playerUnit2.compose_mind_max),
        Util.CalcMaxTakeover(playerUnit2.dexterity.initial, playerUnit2.dexterity.inheritance, playerUnit2.dexterity.level_up_max_status, playerUnit2.compose_dexterity_max)
      };
      takeovers = new int[8]
      {
        playerUnit1.hp.inheritance,
        playerUnit1.lucky.inheritance,
        playerUnit1.intelligence.inheritance,
        playerUnit1.strength.inheritance,
        playerUnit1.vitality.inheritance,
        playerUnit1.agility.inheritance,
        playerUnit1.mind.inheritance,
        playerUnit1.dexterity.inheritance
      };
    }
    int[] numArray = (int[]) null;
    int num = -1;
    switch (this.layout_)
    {
      case Unit004TrainingUnitStatus.LayoutType.EvoBefore:
        numArray = maxTakeovers;
        if (target != (PlayerUnit) null)
        {
          num = (int) Util.CalcGrowthDegree(takeovers, maxTakeovers);
          break;
        }
        break;
      case Unit004TrainingUnitStatus.LayoutType.EvoAfter:
        numArray = new int[8]
        {
          playerUnit1.hp.initial,
          playerUnit1.lucky.initial,
          playerUnit1.intelligence.initial,
          playerUnit1.strength.initial,
          playerUnit1.vitality.initial,
          playerUnit1.agility.initial,
          playerUnit1.mind.initial,
          playerUnit1.dexterity.initial
        };
        break;
    }
    if (numArray != null)
    {
      Color[] colors = new Color[2]
      {
        this.colorNormalTakeover_,
        !disableCheckColor ? this.colorMaxTakeover_ : this.colorNormalTakeover_
      };
      for (int index = 0; index < this.paramDetails_.Length; ++index)
        this.paramDetails_[index].set(takeovers[index], numArray[index], maxTakeovers[index], colors);
    }
    else
    {
      for (int index = 0; index < this.paramDetails_.Length; ++index)
        this.paramDetails_[index].setNull(this.colorNormalTakeover_);
    }
    this.growthDegree = num != -1 ? (GrowthDegree) num : GrowthDegree.E;
    if (!Object.op_Inequality((Object) this.growthDegrees_, (Object) null))
      return;
    if (num != -1)
      this.growthDegrees_.Show(this.growthDegree);
    else
      this.growthDegrees_.Hide();
  }

  public enum LayoutType
  {
    Reincarnation = -1, // 0xFFFFFFFF
    Normal = 0,
    EvoBefore = 1,
    EvoAfter = 2,
  }

  private enum ParamType
  {
    Hp,
    Lucky,
    Magic,
    Power,
    Protect,
    Speed,
    Spirit,
    Technique,
  }

  [Serializable]
  private class ParamDetail
  {
    [Tooltip("進化引継ぎ値")]
    public UILabel txtTakeover_;
    [Tooltip("初期値/進化引継ぎ最大値(layout_ に依る)")]
    public UILabel txtOther_;

    public void set(int nTakeover, int nOther, int maxTakeover, Color[] colors)
    {
      this.txtTakeover_.SetTextLocalize(nTakeover);
      ((UIWidget) this.txtTakeover_).color = colors[nTakeover < maxTakeover ? 0 : 1];
      this.txtOther_.SetTextLocalize(nOther);
    }

    public void setNull(Color col)
    {
      this.txtTakeover_.SetTextLocalize("-");
      ((UIWidget) this.txtTakeover_).color = col;
      this.txtOther_.SetTextLocalize("-");
    }
  }
}
