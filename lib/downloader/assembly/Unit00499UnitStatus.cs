// Decompiled with JetBrains decompiler
// Type: Unit00499UnitStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using UnityEngine;

#nullable disable
public class Unit00499UnitStatus : MonoBehaviour
{
  [SerializeField]
  private UISprite slcUnitbaseAfter;
  [SerializeField]
  private GameObject dirAwake;
  [SerializeField]
  private UILabel TxtHp;
  [SerializeField]
  private UILabel TxtJobname;
  [SerializeField]
  private UILabel TxtLucky;
  [SerializeField]
  private UILabel TxtLv;
  [SerializeField]
  private UILabel TxtLvmax;
  [SerializeField]
  private UILabel TxtMagic;
  [SerializeField]
  private UILabel TxtPower;
  [SerializeField]
  private UILabel TxtPrincesstype;
  [SerializeField]
  private UILabel TxtProtect;
  [SerializeField]
  private UILabel TxtSpeed;
  [SerializeField]
  private UILabel TxtSpirit;
  [SerializeField]
  private UILabel TxtTechnique;
  [SerializeField]
  private UILabel TxtCost;
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
  public Unit00499Scene.Mode mode;
  private const string SlcUnitBaseAfterSpriteName = "slc_awakening_Unitbase_After.png__GUI__004-9-9_sozai__004-9-9_sozai_prefab";
  public GameObject linkUnit;
  public GameObject[] TransUpParameter;
  [Header("限界突破表示")]
  [SerializeField]
  [Tooltip("限界突破表示のON/OFFが出来る位置をセット。\n未設定なら限界突破の制御はしない")]
  private GameObject dirLimitBreaks;
  [SerializeField]
  private GameObject[] objLimitBreaks;
  [SerializeField]
  [Tooltip("objLimitBreaks と設定数は同じにする事")]
  private GameObject[] objLimitBreakBlanks;

  private void setLimitBreak(PlayerUnit target, UnitUnit tUnit)
  {
    if (Object.op_Equality((Object) this.dirLimitBreaks, (Object) null))
      return;
    if (tUnit.IsMaterialUnit)
    {
      this.dirLimitBreaks.SetActive(false);
    }
    else
    {
      int num1 = Mathf.Min(target.breakthrough_count, this.objLimitBreaks.Length);
      int num2 = Mathf.Min(tUnit.breakthrough_limit, this.objLimitBreakBlanks.Length);
      for (int index = 0; index < this.objLimitBreaks.Length; ++index)
      {
        bool flag = index < num1;
        this.objLimitBreaks[index].SetActive(flag);
        this.objLimitBreakBlanks[index].SetActive(!flag && index < num2);
      }
      this.dirLimitBreaks.SetActive(true);
    }
  }

  protected void InitNumber(int v1, bool isNormalUnit1, UILabel source)
  {
    ((Action<string, UILabel>) ((v, label) =>
    {
      if (Object.op_Equality((Object) label, (Object) null))
        return;
      label.SetTextLocalize(v);
      ((Component) label).gameObject.SetActive(true);
    }))(isNormalUnit1 ? v1.ToString() : "---", source);
    if (isNormalUnit1)
      return;
    ((UIWidget) source).color = Color.white;
  }

  protected void setStatusMaxStar(GameObject go, bool isDisp)
  {
    if (!Object.op_Inequality((Object) go, (Object) null))
      return;
    go.SetActive(isDisp);
  }

  public void SetStatusTextEarth(PlayerUnit playerUnit)
  {
    this.InitNumber(playerUnit.self_total_hp, true, this.TxtHp);
    this.InitNumber(playerUnit.self_total_strength, true, this.TxtPower);
    this.InitNumber(playerUnit.self_total_intelligence, true, this.TxtMagic);
    this.InitNumber(playerUnit.self_total_vitality, true, this.TxtProtect);
    this.InitNumber(playerUnit.self_total_mind, true, this.TxtSpirit);
    this.InitNumber(playerUnit.self_total_agility, true, this.TxtSpeed);
    this.InitNumber(playerUnit.self_total_dexterity, true, this.TxtTechnique);
    this.InitNumber(playerUnit.self_total_lucky, true, this.TxtLucky);
    this.InitNumber(playerUnit.level, true, this.TxtLv);
    this.InitNumber(playerUnit.max_level, true, this.TxtLvmax);
    this.setStatusMaxStar(this.hpStatusMaxStar, playerUnit.hp.is_max);
    this.setStatusMaxStar(this.powerStatusMaxStar, playerUnit.strength.is_max);
    this.setStatusMaxStar(this.magicStatusMaxStar, playerUnit.intelligence.is_max);
    this.setStatusMaxStar(this.protectStatusMaxStar, playerUnit.vitality.is_max);
    this.setStatusMaxStar(this.spiritStatusMaxStar, playerUnit.mind.is_max);
    this.setStatusMaxStar(this.speedStatusMaxStar, playerUnit.agility.is_max);
    this.setStatusMaxStar(this.techniqueStatusMaxStar, playerUnit.dexterity.is_max);
    this.setStatusMaxStar(this.luckyStatusMaxStar, playerUnit.lucky.is_max);
    this.TxtJobname.SetTextLocalize(playerUnit.getJobData().name);
  }

  public void SetStatusText(PlayerUnit playerUnit, bool enableAwake = false)
  {
    UnitUnit unit = playerUnit.unit;
    bool isNormalUnit = unit.IsNormalUnit;
    this.InitNumber(playerUnit.self_hp_without_x, isNormalUnit, this.TxtHp);
    this.InitNumber(playerUnit.self_strength_without_x, isNormalUnit, this.TxtPower);
    this.InitNumber(playerUnit.self_intelligence_without_x, isNormalUnit, this.TxtMagic);
    this.InitNumber(playerUnit.self_vitality_without_x, isNormalUnit, this.TxtProtect);
    this.InitNumber(playerUnit.self_mind_without_x, isNormalUnit, this.TxtSpirit);
    this.InitNumber(playerUnit.self_agility_without_x, isNormalUnit, this.TxtSpeed);
    this.InitNumber(playerUnit.self_dexterity_without_x, isNormalUnit, this.TxtTechnique);
    this.InitNumber(playerUnit.self_lucky_without_x, isNormalUnit, this.TxtLucky);
    this.InitNumber(playerUnit.level, isNormalUnit, this.TxtLv);
    this.InitNumber(playerUnit.max_level, isNormalUnit, this.TxtLvmax);
    if (isNormalUnit)
    {
      this.setStatusMaxStar(this.hpStatusMaxStar, playerUnit.hp.is_max);
      this.setStatusMaxStar(this.powerStatusMaxStar, playerUnit.strength.is_max);
      this.setStatusMaxStar(this.magicStatusMaxStar, playerUnit.intelligence.is_max);
      this.setStatusMaxStar(this.protectStatusMaxStar, playerUnit.vitality.is_max);
      this.setStatusMaxStar(this.spiritStatusMaxStar, playerUnit.mind.is_max);
      this.setStatusMaxStar(this.speedStatusMaxStar, playerUnit.agility.is_max);
      this.setStatusMaxStar(this.techniqueStatusMaxStar, playerUnit.dexterity.is_max);
      this.setStatusMaxStar(this.luckyStatusMaxStar, playerUnit.lucky.is_max);
    }
    else
    {
      this.setStatusMaxStar(this.hpStatusMaxStar, false);
      this.setStatusMaxStar(this.powerStatusMaxStar, false);
      this.setStatusMaxStar(this.magicStatusMaxStar, false);
      this.setStatusMaxStar(this.protectStatusMaxStar, false);
      this.setStatusMaxStar(this.spiritStatusMaxStar, false);
      this.setStatusMaxStar(this.speedStatusMaxStar, false);
      this.setStatusMaxStar(this.techniqueStatusMaxStar, false);
      this.setStatusMaxStar(this.luckyStatusMaxStar, false);
    }
    this.TxtPrincesstype.SetTextLocalize(playerUnit.unit_type.name);
    this.TxtCost.SetTextLocalize(playerUnit.cost);
    this.TxtJobname.SetTextLocalize(playerUnit.getJobData().name);
    if (unit.awake_unit_flag & enableAwake && Object.op_Inequality((Object) this.dirAwake, (Object) null) && Object.op_Inequality((Object) this.slcUnitbaseAfter, (Object) null))
    {
      this.dirAwake.SetActive(true);
      this.slcUnitbaseAfter.spriteName = "slc_awakening_Unitbase_After.png__GUI__004-9-9_sozai__004-9-9_sozai_prefab";
      ((UIWidget) this.TxtLv).color = Color.white;
    }
    this.setLimitBreak(playerUnit, unit);
  }

  public void SetStatusTextMemory(PlayerUnit playerUnit)
  {
    UnitUnit unit = playerUnit.unit;
    bool isNormalUnit = unit.IsNormalUnit;
    this.InitNumber(playerUnit.memory_hp, isNormalUnit, this.TxtHp);
    this.InitNumber(playerUnit.memory_strength, isNormalUnit, this.TxtPower);
    this.InitNumber(playerUnit.memory_intelligence, isNormalUnit, this.TxtMagic);
    this.InitNumber(playerUnit.memory_vitality, isNormalUnit, this.TxtProtect);
    this.InitNumber(playerUnit.memory_mind, isNormalUnit, this.TxtSpirit);
    this.InitNumber(playerUnit.memory_agility, isNormalUnit, this.TxtSpeed);
    this.InitNumber(playerUnit.memory_dexterity, isNormalUnit, this.TxtTechnique);
    this.InitNumber(playerUnit.memory_lucky, isNormalUnit, this.TxtLucky);
    this.InitNumber(playerUnit.memory_level, isNormalUnit, this.TxtLv);
    this.InitNumber(playerUnit.max_level, isNormalUnit, this.TxtLvmax);
    if (isNormalUnit)
    {
      this.setStatusMaxStar(this.hpStatusMaxStar, playerUnit.is_memory_hp_max);
      this.setStatusMaxStar(this.powerStatusMaxStar, playerUnit.is_memory_strength_max);
      this.setStatusMaxStar(this.magicStatusMaxStar, playerUnit.is_memory_intelligence_max);
      this.setStatusMaxStar(this.protectStatusMaxStar, playerUnit.is_memory_vitality_max);
      this.setStatusMaxStar(this.spiritStatusMaxStar, playerUnit.is_memory_mind_max);
      this.setStatusMaxStar(this.speedStatusMaxStar, playerUnit.is_memory_agility_max);
      this.setStatusMaxStar(this.techniqueStatusMaxStar, playerUnit.is_memory_dexterity_max);
      this.setStatusMaxStar(this.luckyStatusMaxStar, playerUnit.is_memory_lucky_max);
    }
    else
    {
      this.setStatusMaxStar(this.hpStatusMaxStar, false);
      this.setStatusMaxStar(this.powerStatusMaxStar, false);
      this.setStatusMaxStar(this.magicStatusMaxStar, false);
      this.setStatusMaxStar(this.protectStatusMaxStar, false);
      this.setStatusMaxStar(this.spiritStatusMaxStar, false);
      this.setStatusMaxStar(this.speedStatusMaxStar, false);
      this.setStatusMaxStar(this.techniqueStatusMaxStar, false);
      this.setStatusMaxStar(this.luckyStatusMaxStar, false);
    }
    this.TxtPrincesstype.SetTextLocalize(playerUnit.unit_type.name);
    this.TxtCost.SetTextLocalize(playerUnit.cost);
    this.TxtJobname.SetTextLocalize(playerUnit.getJobData().name);
    this.setLimitBreak(playerUnit, unit);
  }
}
