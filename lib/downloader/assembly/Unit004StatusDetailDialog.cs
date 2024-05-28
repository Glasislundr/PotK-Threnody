// Decompiled with JetBrains decompiler
// Type: Unit004StatusDetailDialog
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
public class Unit004StatusDetailDialog : BackButtonMenuBase
{
  public const int compose_value_center_x = 270;
  [SerializeField]
  private int txt_compose_title_X = 234;
  [SerializeField]
  private int txt_compose_title_X_withAdd = 204;
  [SerializeField]
  private int txt_compose_value_X = 386;
  [SerializeField]
  private int txt_compose_value_X_withAdd = 378;
  [SerializeField]
  private int txt_compose_value_max_X = 392;
  [SerializeField]
  private int txt_compose_pos_max_X_withAdd = 384;
  [Space(20f)]
  [SerializeField]
  private NGxMaskSprite dyn_Unit;
  [SerializeField]
  private UISprite slc_hime_type;
  [SerializeField]
  private UILabel txt_hime_type;
  [SerializeField]
  private UILabel txt_power_value;
  [SerializeField]
  private UILabel txt_average_rising_value;
  [SerializeField]
  private UILabel txt_level_current;
  [SerializeField]
  private UILabel txt_level_max;
  [SerializeField]
  private List<GameObject> slc_LBicon_List;
  [SerializeField]
  private List<GameObject> slc_LBicon_Blue_List;
  [SerializeField]
  private UILabel txt_enforce_current;
  [SerializeField]
  private UILabel txt_enforce_max;
  [SerializeField]
  private UILabel txt_enforce_limit_value;
  [SerializeField]
  private UILabel txt_combine_title;
  [SerializeField]
  private UILabel txt_combina_add_title;
  [SerializeField]
  protected Unit004StatusDetailDialog.StatusDeteil dir_status_hp;
  [SerializeField]
  protected Unit004StatusDetailDialog.StatusDeteil dir_status_attack;
  [SerializeField]
  protected Unit004StatusDetailDialog.StatusDeteil dir_status_magic_attack;
  [SerializeField]
  protected Unit004StatusDetailDialog.StatusDeteil dir_status_defense;
  [SerializeField]
  protected Unit004StatusDetailDialog.StatusDeteil dir_status_mental;
  [SerializeField]
  protected Unit004StatusDetailDialog.StatusDeteil dir_status_speed;
  [SerializeField]
  protected Unit004StatusDetailDialog.StatusDeteil dir_status_technique;
  [SerializeField]
  protected Unit004StatusDetailDialog.StatusDeteil dir_status_lucky;
  protected List<Unit004StatusDetailDialog.StatusDeteil> StatusDetailList = new List<Unit004StatusDetailDialog.StatusDeteil>();

  public virtual void Initialize(PlayerUnit playerUnit, Sprite unitLargeSprite, bool isMemory = false)
  {
    this.SetUnitSprite(playerUnit, unitLargeSprite);
    this.SetUnitStatus(playerUnit, isMemory);
    this.StatusDetailList.Add(this.dir_status_hp);
    this.StatusDetailList.Add(this.dir_status_attack);
    this.StatusDetailList.Add(this.dir_status_magic_attack);
    this.StatusDetailList.Add(this.dir_status_defense);
    this.StatusDetailList.Add(this.dir_status_mental);
    this.StatusDetailList.Add(this.dir_status_speed);
    this.StatusDetailList.Add(this.dir_status_technique);
    this.StatusDetailList.Add(this.dir_status_lucky);
    ((Component) this.txt_combina_add_title).gameObject.SetActive(playerUnit.unit.compose_max_unity_value_setting_id_ComposeMaxUnityValueSetting > 0);
    this.SetCombinePos(playerUnit);
    if (playerUnit.unit.compose_max_unity_value_setting_id_ComposeMaxUnityValueSetting <= 0)
      return;
    this.SetComposeAddValue(playerUnit);
  }

  protected void SetUnitSprite(PlayerUnit playerUnit, Sprite sprite)
  {
    this.dyn_Unit.sprite2D = sprite;
    ((Component) this.dyn_Unit).GetComponent<NGxMaskSpriteWithScale>().FitMask();
  }

  protected void SetUnitStatus(PlayerUnit playerUnit, bool isMemory)
  {
    this.setupProncessType(playerUnit);
    this.setupBreakthroughIcon(playerUnit);
    this.txt_power_value.SetTextLocalize(isMemory ? Judgement.NonBattleParameter.FromPlayerUnitMemoryWithoutGear(playerUnit).Combat : Judgement.NonBattleParameter.FromPlayerUnitWithoutGear(playerUnit).Combat);
    int current1 = isMemory ? playerUnit.memory_hp : playerUnit.self_total_hp;
    int current2 = isMemory ? playerUnit.memory_strength : playerUnit.self_total_strength;
    int current3 = isMemory ? playerUnit.memory_intelligence : playerUnit.self_total_intelligence;
    int current4 = isMemory ? playerUnit.memory_vitality : playerUnit.self_total_vitality;
    int current5 = isMemory ? playerUnit.memory_mind : playerUnit.self_total_mind;
    int current6 = isMemory ? playerUnit.memory_agility : playerUnit.self_total_agility;
    int current7 = isMemory ? playerUnit.memory_dexterity : playerUnit.self_total_dexterity;
    int current8 = isMemory ? playerUnit.memory_lucky : playerUnit.self_total_lucky;
    int levelup1 = isMemory ? playerUnit.MemoryData.hp : playerUnit.hp.level;
    int levelup2 = isMemory ? playerUnit.MemoryData.strength : playerUnit.strength.level;
    int levelup3 = isMemory ? playerUnit.MemoryData.intelligence : playerUnit.intelligence.level;
    int levelup4 = isMemory ? playerUnit.MemoryData.vitality : playerUnit.vitality.level;
    int levelup5 = isMemory ? playerUnit.MemoryData.mind : playerUnit.mind.level;
    int levelup6 = isMemory ? playerUnit.MemoryData.agility : playerUnit.agility.level;
    int levelup7 = isMemory ? playerUnit.MemoryData.dexterity : playerUnit.dexterity.level;
    int levelup8 = isMemory ? playerUnit.MemoryData.lucky : playerUnit.lucky.level;
    float num1 = (float) ((isMemory ? playerUnit.MemoryData.level : playerUnit.level) - 1);
    float num2 = (float) (levelup1 + levelup2 + levelup3 + levelup4 + levelup5 + levelup6 + levelup7 + levelup8);
    float num3 = 0.0f;
    if ((double) num1 > 0.0 && (double) num2 > 0.0)
      num3 = Mathf.Round((float) ((double) num2 / (double) num1 * 10.0)) / 10f;
    this.txt_level_current.SetTextLocalize(isMemory ? playerUnit.MemoryData.level : playerUnit.total_level);
    this.txt_level_max.SetTextLocalize(playerUnit.total_max_level);
    this.txt_enforce_max.SetTextLocalize(playerUnit.buildup_limit);
    this.txt_enforce_limit_value.SetTextLocalize(Consts.Format(Consts.GetInstance().UNIT_STATUS_DETAIL_VAL1, (IDictionary) new Hashtable()
    {
      {
        (object) "val",
        (object) playerUnit.buildupLimitBreakCnt
      }
    }));
    this.txt_average_rising_value.SetTextLocalize(num3.ToString());
    int num4 = 0 + this.dir_status_hp.SetStatus(current1, playerUnit.hp.initial, playerUnit.hp.inheritance, levelup1, playerUnit.hp.buildup, playerUnit.hp.level_up_max_status, playerUnit.hp.compose, playerUnit.compose_hp_max) + this.dir_status_attack.SetStatus(current2, playerUnit.strength.initial, playerUnit.strength.inheritance, levelup2, playerUnit.strength.buildup, playerUnit.strength.level_up_max_status, playerUnit.strength.compose, playerUnit.compose_strength_max) + this.dir_status_magic_attack.SetStatus(current3, playerUnit.intelligence.initial, playerUnit.intelligence.inheritance, levelup3, playerUnit.intelligence.buildup, playerUnit.intelligence.level_up_max_status, playerUnit.intelligence.compose, playerUnit.compose_intelligence_max) + this.dir_status_defense.SetStatus(current4, playerUnit.vitality.initial, playerUnit.vitality.inheritance, levelup4, playerUnit.vitality.buildup, playerUnit.vitality.level_up_max_status, playerUnit.vitality.compose, playerUnit.compose_vitality_max) + this.dir_status_mental.SetStatus(current5, playerUnit.mind.initial, playerUnit.mind.inheritance, levelup5, playerUnit.mind.buildup, playerUnit.mind.level_up_max_status, playerUnit.mind.compose, playerUnit.compose_mind_max) + this.dir_status_speed.SetStatus(current6, playerUnit.agility.initial, playerUnit.agility.inheritance, levelup6, playerUnit.agility.buildup, playerUnit.agility.level_up_max_status, playerUnit.agility.compose, playerUnit.compose_agility_max) + this.dir_status_technique.SetStatus(current7, playerUnit.dexterity.initial, playerUnit.dexterity.inheritance, levelup7, playerUnit.dexterity.buildup, playerUnit.dexterity.level_up_max_status, playerUnit.dexterity.compose, playerUnit.compose_dexterity_max) + this.dir_status_lucky.SetStatus(current8, playerUnit.lucky.initial, playerUnit.lucky.inheritance, levelup8, playerUnit.lucky.buildup, playerUnit.lucky.level_up_max_status, playerUnit.lucky.compose, playerUnit.compose_lucky_max);
    this.txt_enforce_current.SetTextLocalize(playerUnit.buildup_count - num4);
  }

  private void setupProncessType(PlayerUnit playerUnit)
  {
    string str1 = "slc_Princess_";
    string str2;
    switch (playerUnit.unit_type.Enum)
    {
      case UnitTypeEnum.ouki:
        str2 = str1 + "King";
        break;
      case UnitTypeEnum.meiki:
        str2 = str1 + "Life";
        break;
      case UnitTypeEnum.kouki:
        str2 = str1 + "Attack";
        break;
      case UnitTypeEnum.maki:
        str2 = str1 + "Magic";
        break;
      case UnitTypeEnum.syuki:
        str2 = str1 + "Defense";
        break;
      case UnitTypeEnum.syouki:
        str2 = str1 + "Technical";
        break;
      default:
        Debug.LogWarning((object) "タイプ不一致");
        return;
    }
    string str3 = str2 + ".png__GUI__princess_type__princess_type_prefab";
    if (Object.op_Inequality((Object) this.txt_hime_type, (Object) null) && Singleton<NGGameDataManager>.GetInstance().IsSea)
      this.txt_hime_type.SetText(Consts.GetInstance().GetUnitTypeText(playerUnit.unit_type.Enum));
    else
      this.slc_hime_type.spriteName = str3;
  }

  private void setupBreakthroughIcon(PlayerUnit playerUnit)
  {
    for (int index = 0; index < this.slc_LBicon_Blue_List.Count; ++index)
    {
      this.slc_LBicon_List[index].SetActive(index < playerUnit.unit.breakthrough_limit);
      this.slc_LBicon_Blue_List[index].SetActive(index < playerUnit.breakthrough_count);
    }
  }

  protected virtual void SetComposeAddValue(PlayerUnit playerUnit)
  {
    if (playerUnit.unit.compose_max_unity_value_setting_id_ComposeMaxUnityValueSetting <= 0)
      return;
    int[] numArray1 = new int[8]
    {
      playerUnit.UnitTypeParameter.hp_compose_max,
      playerUnit.UnitTypeParameter.strength_compose_max,
      playerUnit.UnitTypeParameter.intelligence_compose_max,
      playerUnit.UnitTypeParameter.vitality_compose_max,
      playerUnit.UnitTypeParameter.mind_compose_max,
      playerUnit.UnitTypeParameter.agility_compose_max,
      playerUnit.UnitTypeParameter.dexterity_compose_max,
      playerUnit.UnitTypeParameter.lucky_compose_max
    };
    int[] numArray2 = new int[8]
    {
      playerUnit.getComposeAddValue(PlayerUnit.ParamType.HP),
      playerUnit.getComposeAddValue(PlayerUnit.ParamType.STRENGTH),
      playerUnit.getComposeAddValue(PlayerUnit.ParamType.INTELLIGENCE),
      playerUnit.getComposeAddValue(PlayerUnit.ParamType.VITALITY),
      playerUnit.getComposeAddValue(PlayerUnit.ParamType.MIND),
      playerUnit.getComposeAddValue(PlayerUnit.ParamType.AGILITY),
      playerUnit.getComposeAddValue(PlayerUnit.ParamType.DEXTERITY),
      playerUnit.getComposeAddValue(PlayerUnit.ParamType.LUCKY)
    };
    for (int index = 0; index < numArray1.Length; ++index)
      this.StatusDetailList[index].Txt_combine_max.SetTextLocalize(string.Format("/{0}({1})", (object) (numArray1[index] + numArray2[index]), (object) numArray2[index]));
  }

  private void SetCombinePos(PlayerUnit playerUnit)
  {
    int num1 = playerUnit.unit.compose_max_unity_value_setting_id_ComposeMaxUnityValueSetting > 0 ? 1 : 0;
    int num2 = num1 != 0 ? this.txt_compose_title_X_withAdd : this.txt_compose_title_X;
    int num3 = num1 != 0 ? this.txt_compose_value_X_withAdd : this.txt_compose_value_X;
    int num4 = num1 != 0 ? this.txt_compose_pos_max_X_withAdd : this.txt_compose_value_max_X;
    ((Component) this.txt_combine_title).transform.localPosition = new Vector3((float) num2, ((Component) this.txt_combine_title).transform.localPosition.y, ((Component) this.txt_combine_title).transform.localPosition.z);
    Vector3 vector3 = new Vector3();
    for (int index = 0; index < this.StatusDetailList.Count; ++index)
    {
      Unit004StatusDetailDialog.StatusDeteil statusDetail = this.StatusDetailList[index];
      Vector3 localPosition = ((Component) statusDetail.Txt_combine).gameObject.transform.localPosition;
      ((Component) statusDetail.Txt_combine).gameObject.transform.localPosition = new Vector3((float) num3, localPosition.y, localPosition.z);
      ((Component) statusDetail.Txt_combine_max).gameObject.transform.localPosition = new Vector3((float) num4, localPosition.y, localPosition.z);
    }
  }

  public void onHelpButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
    List<HelpHelp> list = ((IEnumerable<HelpHelp>) MasterData.HelpHelpList).Where<HelpHelp>((Func<HelpHelp, bool>) (x => x.ID == 249)).OrderByDescending<HelpHelp, int>((Func<HelpHelp, int>) (x => x.priority)).ToList<HelpHelp>();
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
    Help0152Scene.ChangeScene(true, list);
  }

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().dismiss();

  [Serializable]
  protected class StatusDeteil
  {
    [SerializeField]
    private UILabel txt_current;
    [SerializeField]
    private UILabel txt_max;
    private int max_value;
    [SerializeField]
    private UILabel txt_initial;
    [SerializeField]
    private UILabel txt_evolution_takeover;
    [SerializeField]
    private UILabel txt_levelup;
    [SerializeField]
    private UILabel txt_reinforce_value;
    [SerializeField]
    private UILabel txt_levelup_max;
    [SerializeField]
    private UILabel txt_combine;
    [SerializeField]
    private UILabel txt_combine_max;
    [SerializeField]
    private UILabel txt_touta;
    [SerializeField]
    private UILabel txt_touta_max;
    [SerializeField]
    private UILabel txt_master_bonus;
    [SerializeField]
    private UILabel txt_master_bonus_max;
    [SerializeField]
    private UILabel txt_X_levelup;
    [SerializeField]
    private UILabel txt_X_levelup_max;

    public int Max_value
    {
      get => this.max_value;
      set
      {
        this.max_value = value;
        this.txt_max.SetTextLocalize(Consts.Format(Consts.GetInstance().UNIT_STATUS_DETAIL_VAL2, (IDictionary) new Hashtable()
        {
          {
            (object) "val",
            (object) this.max_value
          }
        }));
      }
    }

    public UILabel Txt_combine => this.txt_combine;

    public UILabel Txt_combine_max => this.txt_combine_max;

    public UILabel Txt_touta => this.txt_touta;

    public UILabel Txt_touta_max => this.txt_touta_max;

    public UILabel Txt_master_bonus => this.txt_master_bonus;

    public UILabel Txt_master_bonus_max => this.txt_master_bonus_max;

    public UILabel Txt_X_levelup => this.txt_X_levelup;

    public UILabel Txt_X_levelup_max => this.txt_X_levelup_max;

    public int SetStatus(
      int current,
      int initial,
      int evolution_takeover,
      int levelup,
      int reinforce_value,
      int levelup_max,
      int combine,
      int combine_max)
    {
      int num1 = 0;
      int num2 = levelup + reinforce_value;
      if (num2 > levelup_max)
      {
        reinforce_value -= num2 - levelup_max;
        num1 = num2 - levelup_max;
      }
      this.txt_current.SetTextLocalize(current);
      this.Max_value = initial + evolution_takeover + levelup_max + combine_max;
      this.txt_initial.SetTextLocalize(initial + evolution_takeover);
      this.txt_evolution_takeover.SetTextLocalize(Consts.Format(Consts.GetInstance().UNIT_STATUS_DETAIL_VAL3, (IDictionary) new Hashtable()
      {
        {
          (object) "val",
          (object) evolution_takeover
        }
      }));
      this.txt_levelup.SetTextLocalize(num2);
      this.txt_reinforce_value.SetTextLocalize(Consts.Format(Consts.GetInstance().UNIT_STATUS_DETAIL_VAL3, (IDictionary) new Hashtable()
      {
        {
          (object) "val",
          (object) reinforce_value
        }
      }));
      this.txt_levelup_max.SetTextLocalize(Consts.Format(Consts.GetInstance().UNIT_STATUS_DETAIL_VAL2, (IDictionary) new Hashtable()
      {
        {
          (object) "val",
          (object) levelup_max
        }
      }));
      this.txt_combine.SetTextLocalize(combine);
      this.txt_combine_max.SetTextLocalize(Consts.Format(Consts.GetInstance().UNIT_STATUS_DETAIL_VAL2, (IDictionary) new Hashtable()
      {
        {
          (object) "val",
          (object) combine_max
        }
      }));
      return num1;
    }
  }
}
