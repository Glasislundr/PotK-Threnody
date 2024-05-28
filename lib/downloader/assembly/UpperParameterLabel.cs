// Decompiled with JetBrains decompiler
// Type: UpperParameterLabel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class UpperParameterLabel : MonoBehaviour
{
  [SerializeField]
  private BlinkSync blink;
  [SerializeField]
  private GameObject blinkBreakthrough;
  [SerializeField]
  private GameObject blinkSkillUp;
  [SerializeField]
  private GameObject blinkDeardegreeup;
  [SerializeField]
  private GameObject blinkUnityValue;
  [SerializeField]
  private UILabel txtBlinkUnityValue;
  [SerializeField]
  private GameObject blinkBuildupUnityValue;
  [SerializeField]
  private UILabel txtBlinkBuildupUnityValue;
  [SerializeField]
  private GameObject breakThrough;
  [SerializeField]
  private GameObject skillUp;
  [SerializeField]
  private GameObject deardegreeup;
  [SerializeField]
  private GameObject unityValue;
  [SerializeField]
  private UILabel txtUnityValue;
  [SerializeField]
  private GameObject buildupUnityValue;
  [SerializeField]
  private UILabel txtBuildupUnityValue;
  private const int DEFAULT_UNITY_VALUE = 0;

  private void InitializeLabel()
  {
    ((Component) this.blink).gameObject.SetActive(false);
    this.breakThrough.SetActive(false);
    this.skillUp.SetActive(false);
    this.unityValue.SetActive(false);
  }

  public void Init(PlayerUnit basePlayerUnit, PlayerUnit materialUnit)
  {
    this.InitializeLabel();
    UnitUnit baseU = basePlayerUnit.unit;
    UnitUnit matU = materialUnit.unit;
    int num = 0;
    float unity = 0.0f;
    bool flag1 = false;
    bool flag2 = false;
    if (basePlayerUnit.unity_value < PlayerUnit.GetUnityValueMax())
    {
      if (matU.IsNormalUnit)
      {
        if (baseU.same_character_id == matU.same_character_id)
        {
          num = Mathf.Min(materialUnit.unity_value + PlayerUnit.GetUnityValue(), PlayerUnit.GetUnityValueMax());
          unity = Mathf.Min(materialUnit.buildup_unity_value_f, (float) PlayerUnit.GetUnityValueMax());
        }
      }
      else if (matU.is_unity_value_up)
      {
        if ((double) basePlayerUnit.unityTotal < (double) PlayerUnit.GetUnityValueMax())
        {
          UnityValueUpPattern valueUpPattern = matU.FindValueUpPattern(baseU, (Func<UnitFamily[]>) (() => basePlayerUnit.Families));
          if (valueUpPattern != null)
            unity = valueUpPattern.up_value;
        }
        if (matU.FindPureValueUpPattern(baseU) != null)
          num = 1;
      }
      if (num > 0)
        flag1 = true;
      if ((double) unity > 0.0)
        flag2 = true;
    }
    bool flag3 = !matU.IsBreakThrough ? (!matU.IsPureValueUp ? baseU.same_character_id == matU.same_character_id && basePlayerUnit.breakthrough_count < baseU.breakthrough_limit : matU.IsBreakThrougPureValueUp(basePlayerUnit)) : matU.CheckBreakThroughMaterial(basePlayerUnit);
    IEnumerable<PlayerUnitSkills> source1 = ((IEnumerable<PlayerUnitSkills>) basePlayerUnit.skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.level < x.skill.upper_level));
    bool isSkill = false;
    if (materialUnit.skills != null)
      isSkill = source1.Any<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (unitBase => ((IEnumerable<PlayerUnitSkills>) materialUnit.skills).Count<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => unitBase.skill_id == x.skill_id)) > 0));
    if (matU.same_character_id == baseU.same_character_id && source1.Count<PlayerUnitSkills>() > 0)
      isSkill = true;
    else
      ((IEnumerable<PlayerUnitSkills>) basePlayerUnit.skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.level < x.skill.upper_level)).ForEach<PlayerUnitSkills>((Action<PlayerUnitSkills>) (y =>
      {
        if (baseU.same_character_id == matU.same_character_id || materialUnit.skills == null || ((IEnumerable<PlayerUnitSkills>) materialUnit.skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (z => y.skill_id == z.skill_id)).Count<PlayerUnitSkills>() == 0)
          return;
        isSkill = true;
      }));
    bool flag4 = false;
    if (!isSkill)
      flag4 = UnitDetailIcon.IsSkillUpMaterial(materialUnit.unit, basePlayerUnit) | matU.IsSkillLevelUpPureValueUp(basePlayerUnit);
    bool flag5 = false;
    if (baseU.trust_target_flag && (double) basePlayerUnit.trust_rate < (double) basePlayerUnit.trust_max_rate && (matU.same_character_id == baseU.same_character_id || matU.IsPureValueUp))
      flag5 = true;
    bool flag6 = baseU.trust_target_flag && (double) basePlayerUnit.trust_rate < (double) basePlayerUnit.trust_max_rate && matU.character.ID == baseU.character.ID;
    bool flag7 = false;
    if (baseU.trust_target_flag && (double) basePlayerUnit.trust_rate < (double) basePlayerUnit.trust_max_rate && matU.IsTrustMaterial(basePlayerUnit))
      flag7 = true;
    if (flag5 | flag7 | flag6)
    {
      UISprite component1 = this.deardegreeup.GetComponent<UISprite>();
      UISprite component2 = this.blinkDeardegreeup.GetComponent<UISprite>();
      component1.spriteName = !basePlayerUnit.unit.IsSea ? (!basePlayerUnit.unit.IsResonanceUnit ? (component2.spriteName = "slc_DearDegree_up") : (component2.spriteName = "slc_Relevance_up")) : (component2.spriteName = "slc_DearDegree_up");
      UISpriteData atlasSprite1 = component1.GetAtlasSprite();
      ((UIWidget) component1).width = atlasSprite1.width;
      ((UIWidget) component1).height = atlasSprite1.height;
      UISpriteData atlasSprite2 = component2.GetAtlasSprite();
      ((UIWidget) component2).width = atlasSprite2.width;
      ((UIWidget) component2).height = atlasSprite2.height;
    }
    GameObject[] gameObjectArray1 = new GameObject[5]
    {
      this.breakThrough,
      this.skillUp,
      this.deardegreeup,
      this.unityValue,
      this.buildupUnityValue
    };
    bool[] source2 = new bool[5]
    {
      flag3,
      isSkill | flag4,
      flag5 | flag7 | flag6,
      flag1,
      flag2
    };
    if (((IEnumerable<bool>) source2).Count<bool>((Func<bool, bool>) (b => b)) > 1)
    {
      for (int index = 0; index < source2.Length; ++index)
      {
        if (Object.op_Inequality((Object) gameObjectArray1[index], (Object) null))
          gameObjectArray1[index].SetActive(false);
      }
      GameObject[] gameObjectArray2 = new GameObject[5]
      {
        this.blinkBreakthrough,
        this.blinkSkillUp,
        this.blinkDeardegreeup,
        this.blinkUnityValue,
        this.blinkBuildupUnityValue
      };
      List<GameObject> blinks = new List<GameObject>();
      for (int index = 0; index < source2.Length; ++index)
      {
        GameObject gameObject = gameObjectArray2[index];
        if (!Object.op_Equality((Object) gameObject, (Object) null))
        {
          bool flag8 = source2[index];
          if (flag8)
            blinks.Add(gameObject);
          gameObject.SetActive(flag8);
        }
      }
      this.blink.resetBlinks((IEnumerable<GameObject>) blinks);
      ((Component) this.blink).gameObject.SetActive(true);
      UILabel txtBlinkUnityValue = this.txtBlinkUnityValue;
      if (txtBlinkUnityValue != null)
        txtBlinkUnityValue.SetTextLocalize(string.Format(Consts.GetInstance().unit_004_8_4_plus_unity_value, (object) num));
      UILabel buildupUnityValue = this.txtBlinkBuildupUnityValue;
      if (buildupUnityValue == null)
        return;
      buildupUnityValue.SetTextLocalize(string.Format(Consts.GetInstance().unit_004_8_4_plus_buildup_unity_value, (object) PlayerUnit.UnityToPercent(unity)));
    }
    else
    {
      for (int index = 0; index < source2.Length; ++index)
      {
        if (Object.op_Inequality((Object) gameObjectArray1[index], (Object) null))
          gameObjectArray1[index].SetActive(source2[index]);
      }
      UILabel txtUnityValue = this.txtUnityValue;
      if (txtUnityValue != null)
        txtUnityValue.SetTextLocalize(string.Format(Consts.GetInstance().unit_004_8_4_plus_unity_value, (object) num));
      UILabel buildupUnityValue = this.txtBuildupUnityValue;
      if (buildupUnityValue == null)
        return;
      buildupUnityValue.SetTextLocalize(string.Format(Consts.GetInstance().unit_004_8_4_plus_buildup_unity_value, (object) PlayerUnit.UnityToPercent(unity)));
    }
  }
}
