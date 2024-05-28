// Decompiled with JetBrains decompiler
// Type: Battle020191Menu
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
public class Battle020191Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtCharacterName30;
  [SerializeField]
  protected UILabel TxtLvAfter30;
  [SerializeField]
  protected UILabel TxtLvBefore30;
  [SerializeField]
  protected UILabel TxtSkillName26;
  [SerializeField]
  protected UILabel TxtSkilleffect0124;
  private bool isSkip;
  private bool isRunning = true;
  private Action onCallback;
  private bool skillLearn;
  public GameObject SkillBox;
  public GameObject GearIncrementalIcon;
  [SerializeField]
  private GameObject CharaSprite;
  [SerializeField]
  private UI2DSprite UnitSkillSprite;
  [SerializeField]
  private UISprite princessTypeSprite;
  [SerializeField]
  private UILabel[] parameterLabels;
  [SerializeField]
  private GameObject[] parameterMaxStar;
  [SerializeField]
  private GameObject[] parameterGauges;
  [SerializeField]
  private Battle020191Menu.GaugeObject[] gaugeObjectList;
  private List<Battle020191Menu.PlayData> PlayList = new List<Battle020191Menu.PlayData>();
  private int gaugeMax = 1;
  private float[] gearIncrementals = new float[8];
  private bool isFromUnitBase;

  private void Start() => Singleton<NGSoundManager>.GetInstance().playSE("SE_1023");

  public IEnumerator Init(
    PlayerUnit before,
    PlayerUnit after,
    Dictionary<int, PlayerItem> beforeGears,
    Dictionary<int, int> diffGearAccessoryRemainingAmounts,
    bool fromEarth = false,
    bool fromUnitBase = false)
  {
    Battle020191Menu battle020191Menu = this;
    battle020191Menu.SkillBox.SetActive(false);
    Dictionary<int, PlayerUnitSkills> beforeSkills = ((IEnumerable<PlayerUnitSkills>) before.skills).ToDictionary<PlayerUnitSkills, int>((Func<PlayerUnitSkills, int>) (x => x.skill_id));
    PlayerUnitSkills[] skills = ((IEnumerable<PlayerUnitSkills>) after.skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => !beforeSkills.ContainsKey(x.skill_id))).ToArray<PlayerUnitSkills>();
    battle020191Menu.isFromUnitBase = fromUnitBase;
    IEnumerator e;
    if (skills.Length != 0)
    {
      battle020191Menu.skillLearn = true;
      Future<Sprite> skillIcon = skills[0].skill.LoadBattleSkillIcon();
      e = skillIcon.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      battle020191Menu.UnitSkillSprite.sprite2D = skillIcon.Result;
      battle020191Menu.TxtSkillName26.SetTextLocalize(skills[0].skill.name);
      battle020191Menu.TxtSkilleffect0124.SetTextLocalize(skills[0].skill.description);
      skillIcon = (Future<Sprite>) null;
    }
    if (fromUnitBase)
    {
      battle020191Menu.CharaSprite.SetActive(false);
      ((Collider) ((Component) battle020191Menu).GetComponent<BoxCollider>()).enabled = false;
      ((Behaviour) ((Component) battle020191Menu).GetComponent<UIButton>()).enabled = false;
    }
    else
    {
      e = battle020191Menu.SetCharacterViewUnit(before.unit, before.getJobData().ID);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    battle020191Menu.TxtCharacterName30.SetTextLocalize(before.unit.name);
    battle020191Menu.TxtLvBefore30.SetTextLocalize(before.level);
    battle020191Menu.TxtLvAfter30.SetTextLocalize(after.level);
    if (!fromEarth)
      battle020191Menu.SetPrincessType(after);
    else
      ((Component) battle020191Menu.princessTypeSprite).gameObject.SetActive(false);
    UnitTypeParameter unitTypeParameter = after.UnitTypeParameter;
    int[] source = new int[8]
    {
      after.hp.initial + after.hp.inheritance + after.hp.level_up_max_status + unitTypeParameter.hp_compose_max,
      after.strength.initial + after.strength.inheritance + after.strength.level_up_max_status + unitTypeParameter.strength_compose_max,
      after.intelligence.initial + after.intelligence.inheritance + after.intelligence.level_up_max_status + unitTypeParameter.intelligence_compose_max,
      after.vitality.initial + after.vitality.inheritance + after.vitality.level_up_max_status + unitTypeParameter.vitality_compose_max,
      after.mind.initial + after.mind.inheritance + after.mind.level_up_max_status + unitTypeParameter.mind_compose_max,
      after.agility.initial + after.agility.inheritance + after.agility.level_up_max_status + unitTypeParameter.agility_compose_max,
      after.dexterity.initial + after.dexterity.inheritance + after.dexterity.level_up_max_status + unitTypeParameter.dexterity_compose_max,
      after.lucky.initial + after.lucky.inheritance + after.lucky.level_up_max_status + unitTypeParameter.lucky_compose_max
    };
    battle020191Menu.gaugeMax = ((IEnumerable<int>) source).Max();
    battle020191Menu.SetParam(Battle020191Menu.ParameterType.HP, before.self_total_hp, after.self_total_hp);
    battle020191Menu.SetParam(Battle020191Menu.ParameterType.STR, before.self_total_strength, after.self_total_strength);
    battle020191Menu.SetParam(Battle020191Menu.ParameterType.INT, before.self_total_intelligence, after.self_total_intelligence);
    battle020191Menu.SetParam(Battle020191Menu.ParameterType.VIT, before.self_total_vitality, after.self_total_vitality);
    battle020191Menu.SetParam(Battle020191Menu.ParameterType.MND, before.self_total_mind, after.self_total_mind);
    battle020191Menu.SetParam(Battle020191Menu.ParameterType.AGI, before.self_total_agility, after.self_total_agility);
    battle020191Menu.SetParam(Battle020191Menu.ParameterType.DEX, before.self_total_dexterity, after.self_total_dexterity);
    battle020191Menu.SetParam(Battle020191Menu.ParameterType.LUK, before.self_total_lucky, after.self_total_lucky);
    battle020191Menu.parameterMaxStar[0].SetActive(after.hp.is_max);
    battle020191Menu.parameterMaxStar[1].SetActive(after.strength.is_max);
    battle020191Menu.parameterMaxStar[2].SetActive(after.intelligence.is_max);
    battle020191Menu.parameterMaxStar[3].SetActive(after.vitality.is_max);
    battle020191Menu.parameterMaxStar[4].SetActive(after.mind.is_max);
    battle020191Menu.parameterMaxStar[5].SetActive(after.agility.is_max);
    battle020191Menu.parameterMaxStar[6].SetActive(after.dexterity.is_max);
    battle020191Menu.parameterMaxStar[7].SetActive(after.lucky.is_max);
    battle020191Menu.gearIncrementals[0] = 0.0f;
    battle020191Menu.gearIncrementals[1] = 0.0f;
    battle020191Menu.gearIncrementals[2] = 0.0f;
    battle020191Menu.gearIncrementals[3] = 0.0f;
    battle020191Menu.gearIncrementals[4] = 0.0f;
    battle020191Menu.gearIncrementals[5] = 0.0f;
    battle020191Menu.gearIncrementals[6] = 0.0f;
    battle020191Menu.gearIncrementals[7] = 0.0f;
    if (before.equip_gear_ids != null && before.equip_gear_ids.Length != 0)
    {
      foreach (int? equipGearId in before.equip_gear_ids)
      {
        int num;
        if (equipGearId.HasValue && diffGearAccessoryRemainingAmounts.TryGetValue(equipGearId.Value, out num) && num != 0)
        {
          GearGear gear = beforeGears[equipGearId.Value].gear;
          battle020191Menu.gearIncrementals[0] += gear.hp_incremental_ratio;
          battle020191Menu.gearIncrementals[1] += gear.strength_incremental_ratio;
          battle020191Menu.gearIncrementals[2] += gear.intelligence_incremental_ratio;
          battle020191Menu.gearIncrementals[3] += gear.vitality_incremental_ratio;
          battle020191Menu.gearIncrementals[4] += gear.mind_incremental_ratio;
          battle020191Menu.gearIncrementals[5] += gear.agility_incremental_ratio;
          battle020191Menu.gearIncrementals[6] += gear.dexterity_incremental_ratio;
          battle020191Menu.gearIncrementals[7] += gear.lucky_incremental_ratio;
        }
      }
    }
    battle020191Menu.GearIncrementalIcon.SetActive(((IEnumerable<float>) battle020191Menu.gearIncrementals).Any<float>((Func<float, bool>) (x => (double) x > 0.0 || (double) x < 0.0)));
    foreach (Battle020191Menu.PlayData play in battle020191Menu.PlayList)
    {
      Battle020191Menu.GaugeObject gaugeObject = battle020191Menu.gaugeObjectList[(int) play.type];
      float gaugeRatio = battle020191Menu.GetGaugeRatio(play.before);
      Transform transform1 = gaugeObject.BlueGauge.transform;
      Vector3 vector3_1 = new Vector3();
      vector3_1.x = gaugeRatio;
      vector3_1.y = 1f;
      Vector3 vector3_2 = vector3_1;
      transform1.localScale = vector3_2;
      Transform transform2 = gaugeObject.YellowGauge.transform;
      vector3_1 = new Vector3();
      vector3_1.x = gaugeRatio;
      vector3_1.y = 1f;
      Vector3 vector3_3 = vector3_1;
      transform2.localScale = vector3_3;
      gaugeObject.DirUpper.SetActive(false);
      gaugeObject.GearIncrementalUp.SetActive((double) battle020191Menu.gearIncrementals[(int) play.type] > 0.0);
      gaugeObject.GearIncrementalDown.SetActive((double) battle020191Menu.gearIncrementals[(int) play.type] < 0.0);
    }
    battle020191Menu.StartCoroutine(battle020191Menu.Play());
    Singleton<NGSoundManager>.GetInstance().playVoiceByID(before.unit.unitVoicePattern, 61, 0);
  }

  private void SetParam(Battle020191Menu.ParameterType type, int beforeState, int afterState)
  {
    UILabel parameterLabel = this.parameterLabels[(int) type];
    if (beforeState < afterState)
      ((UIWidget) parameterLabel).color = Color.yellow;
    parameterLabel.SetTextLocalize(afterState);
    this.PlayList.Add(new Battle020191Menu.PlayData()
    {
      type = type,
      before = beforeState,
      after = afterState
    });
  }

  private float GetGaugeRatio(Battle020191Menu.ParameterType type, int value)
  {
    float num = 99f;
    if (type == Battle020191Menu.ParameterType.HP)
      num *= 2f;
    return (float) value / num;
  }

  private float GetGaugeRatio(int value) => (float) value / (float) this.gaugeMax;

  private IEnumerator Play()
  {
    yield return (object) new WaitForSeconds(1f);
    foreach (Battle020191Menu.PlayData play in this.PlayList)
    {
      int num = play.after - play.before;
      if (num > 0)
      {
        Battle020191Menu.GaugeObject gaugeObject = this.gaugeObjectList[(int) play.type];
        TweenScale tween = gaugeObject.YellowGauge.AddComponent<TweenScale>();
        tween.from = new Vector3()
        {
          x = this.GetGaugeRatio(play.before),
          y = 1f
        };
        tween.to = new Vector3()
        {
          x = this.GetGaugeRatio(play.after),
          y = 1f
        };
        ((UITweener) tween).duration = 0.25f;
        if (!this.isSkip)
          Singleton<NGSoundManager>.GetInstance().playSE("SE_1019");
        gaugeObject.DirUpper.SetActive(true);
        gaugeObject.TxtUppt.SetTextLocalize(num);
        while (!this.isSkip && ((Behaviour) tween).enabled)
          yield return (object) null;
        tween = (TweenScale) null;
      }
    }
    if (this.skillLearn)
    {
      yield return (object) new WaitForSeconds(0.4f);
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1024");
      this.SkillBox.SetActive(true);
      yield return (object) new WaitForSeconds(0.3f);
    }
    this.isRunning = false;
  }

  public virtual void IbtnScreen()
  {
    if (this.isRunning && !this.isSkip)
      this.isSkip = true;
    else
      this.StartCoroutine(this.toClose());
  }

  private IEnumerator toClose()
  {
    while (this.isRunning)
      yield return (object) null;
    if (this.onCallback != null)
      this.onCallback();
    if (!this.isFromUnitBase)
      Singleton<PopupManager>.GetInstance().dismiss();
  }

  public void SetCallback(Action callback) => this.onCallback = callback;

  private IEnumerator SetCharacterViewUnit(UnitUnit unit, int job_id)
  {
    Future<GameObject> future = unit.LoadMypage();
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject texObj = future.Result.Clone(this.CharaSprite.transform);
    e = unit.SetLargeSpriteSnap(job_id, texObj, 4);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = unit.SetLargeSpriteWithMask(job_id, texObj, Res.GUI._020_19_1_sozai.mask_Chara.Load<Texture2D>(), 5, -146, 36);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void SetPrincessType(PlayerUnit unit)
  {
    string str1 = "slc_Princess_";
    string str2;
    switch (unit.unit_type.Enum)
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
    ((Component) this.princessTypeSprite).gameObject.SetActive(true);
    this.princessTypeSprite.spriteName = str2 + ".png__GUI__princess_type__princess_type_prefab";
    ((UIWidget) this.princessTypeSprite).width = this.princessTypeSprite.GetAtlasSprite().width;
    ((UIWidget) this.princessTypeSprite).height = this.princessTypeSprite.GetAtlasSprite().height;
  }

  public override void onBackButton() => this.showBackKeyToast();

  [Serializable]
  private struct GaugeObject
  {
    public GameObject BlueGauge;
    public GameObject YellowGauge;
    public GameObject DirUpper;
    public UILabel TxtUppt;
    public GameObject GearIncrementalUp;
    public GameObject GearIncrementalDown;
  }

  private enum ParameterType
  {
    HP,
    STR,
    INT,
    VIT,
    MND,
    AGI,
    DEX,
    LUK,
    SIZE,
  }

  private class PlayData
  {
    public Battle020191Menu.ParameterType type;
    public int before;
    public int after;
  }
}
