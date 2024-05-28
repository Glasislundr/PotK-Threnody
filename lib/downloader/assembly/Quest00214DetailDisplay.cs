// Decompiled with JetBrains decompiler
// Type: Quest00214DetailDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Quest00214DetailDisplay : MonoBehaviour
{
  [SerializeField]
  private UILabel Txt_Stagename;
  [SerializeField]
  private UILabel Txt_Ap;
  [SerializeField]
  private UILabel Txt_SubCondition;
  [SerializeField]
  private UILabel Txt_Condition;
  [SerializeField]
  private UILabel Txt_GenderRestriction;
  [SerializeField]
  private UILabel Txt_Rest;
  [SerializeField]
  private QuestStageEntryInfo EntryInfoScript;
  [SerializeField]
  private List<GameObject> StageNumbers;

  public void StartInit()
  {
    ((Component) this.Txt_GenderRestriction).gameObject.SetActive(false);
    this.EntryInfoScript.IsDisplay = false;
  }

  public void InitDetailDisplay(
    QuestSConverter StageData,
    PlayerQuestSConverter playerData,
    int num)
  {
    this.Txt_Stagename.SetTextLocalize(StageData.name);
    this.Txt_Ap.SetTextLocalize(StageData.lost_ap);
    this.Txt_SubCondition.SetTextLocalize(StageData.stage.victory_condition.sub_name);
    if (StageData.data_type == QuestSConverter.DataType.Character)
      this.Txt_Condition.SetTextLocalize("ストーリーの再生");
    else
      this.Txt_Condition.SetTextLocalize(StageData.stage.victory_condition.name);
    if (Object.op_Inequality((Object) this.Txt_Rest, (Object) null))
    {
      Color color = Color.white;
      string text;
      if (playerData != null && playerData.remain_battle_count.HasValue)
      {
        int num1 = playerData.remain_battle_count.Value;
        text = Consts.Format(Consts.GetInstance().QUEST_0022_LIMIT, (IDictionary) new Hashtable()
        {
          {
            (object) "remains",
            (object) num1
          },
          {
            (object) "max",
            (object) playerData.max_battle_count_limit
          }
        });
        if (num1 == 0)
          color = Color.red;
      }
      else
        text = playerData != null ? Consts.GetInstance().QUEST_0022_LIMIT_NONE : Consts.GetInstance().COMMON_NOVALUE;
      ((Component) this.Txt_Rest).GetComponent<UIWidget>().color = color;
      this.Txt_Rest.SetTextLocalize(text);
    }
    this.StageNumbers.ToggleOnce(num - 1);
    if (StageData.data_type == QuestSConverter.DataType.Character)
    {
      QuestCharacterLimitation[] characterLimitationList = MasterData.QuestCharacterLimitationList;
      QuestCharacterLimitationLabel[] limitationLabelList = MasterData.QuestCharacterLimitationLabelList;
      Func<QuestCharacterLimitation, bool> predicate = (Func<QuestCharacterLimitation, bool>) (n => n.quest_s_id_QuestCharacterS == StageData.ID);
      if (((IEnumerable<QuestCharacterLimitation>) ((IEnumerable<QuestCharacterLimitation>) characterLimitationList).Where<QuestCharacterLimitation>(predicate).ToArray<QuestCharacterLimitation>()).Count<QuestCharacterLimitation>() == 0)
      {
        this.EntryInfoScript.IsDisplay = false;
      }
      else
      {
        this.EntryInfoScript.TextNormal = string.Join(",", ((IEnumerable<QuestCharacterLimitationLabel>) limitationLabelList).Where<QuestCharacterLimitationLabel>((Func<QuestCharacterLimitationLabel, bool>) (n => n.quest_s_id_QuestCharacterS == StageData.ID)).Select<QuestCharacterLimitationLabel, string>((Func<QuestCharacterLimitationLabel, string>) (x => x.label)).ToArray<string>());
        this.EntryInfoScript.Normal = true;
      }
    }
    else if (StageData.data_type == QuestSConverter.DataType.Harmony)
    {
      QuestHarmonyLimitation[] harmonyLimitationList = MasterData.QuestHarmonyLimitationList;
      QuestHarmonyLimitationLabel[] limitationLabelList = MasterData.QuestHarmonyLimitationLabelList;
      Func<QuestHarmonyLimitation, bool> predicate = (Func<QuestHarmonyLimitation, bool>) (n => n.quest_s_id_QuestHarmonyS == StageData.ID);
      if (((IEnumerable<QuestHarmonyLimitation>) ((IEnumerable<QuestHarmonyLimitation>) harmonyLimitationList).Where<QuestHarmonyLimitation>(predicate).ToArray<QuestHarmonyLimitation>()).Count<QuestHarmonyLimitation>() == 0)
      {
        this.EntryInfoScript.IsDisplay = false;
      }
      else
      {
        this.EntryInfoScript.TextNormal = string.Join(",", ((IEnumerable<QuestHarmonyLimitationLabel>) limitationLabelList).Where<QuestHarmonyLimitationLabel>((Func<QuestHarmonyLimitationLabel, bool>) (n => n.quest_s_id_QuestHarmonyS == StageData.ID)).Select<QuestHarmonyLimitationLabel, string>((Func<QuestHarmonyLimitationLabel, string>) (x => x.label)).ToArray<string>());
        this.EntryInfoScript.Normal = true;
      }
    }
    if (StageData.gender_restriction != UnitGender.none)
    {
      ((Component) this.Txt_GenderRestriction).gameObject.SetActive(true);
      this.Txt_GenderRestriction.SetTextLocalize(Consts.Format(Consts.GetInstance().QUEST_002_GENDERRESTRICTION, (IDictionary) new Hashtable()
      {
        {
          (object) "gender",
          (object) UnitGenderText.GetText(StageData.gender_restriction)
        }
      }));
    }
    else
      ((Component) this.Txt_GenderRestriction).gameObject.SetActive(false);
  }

  public void StartTween(bool order)
  {
    foreach (UITweener component in ((Component) this).GetComponents<UITweener>())
    {
      if (component.tweenGroup == 1)
        component.Play(order);
    }
  }
}
