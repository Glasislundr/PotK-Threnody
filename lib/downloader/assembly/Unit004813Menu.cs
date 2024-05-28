// Decompiled with JetBrains decompiler
// Type: Unit004813Menu
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
public class Unit004813Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UIButton BackBtn;
  [SerializeField]
  private UILabel characterName;
  [SerializeField]
  private GameObject[] SlcLBiconNone;
  [SerializeField]
  private GameObject[] SlcLBiconBlue;
  [SerializeField]
  private GameObject[] SlcLBiconBlueEff;
  [SerializeField]
  private GameObject slc_Limitbreak;
  [SerializeField]
  private GameObject DirSkillLevelup;
  [SerializeField]
  private GameObject DirLvmax;
  [SerializeField]
  private UILabel beforeLvmax;
  [SerializeField]
  private UILabel afterLvmax;
  [SerializeField]
  private GameObject slcArrow;
  [SerializeField]
  private UILabel lvmaxUnchanged;
  [SerializeField]
  private GameObject unitTexture;
  [SerializeField]
  private GameObject growthParameter;
  [SerializeField]
  public LoveGaugeController loveGaugeController;
  [SerializeField]
  private UILabel textDearPercent;
  [SerializeField]
  private GameObject lnkOverkillersSkill;
  public UIButton skipButton;
  public List<List<PlayerUnit>> selectedMaterialPlayerUnits;
  public List<Unit004832Menu.ResultPlayerUnit> resultPlayerUnits;
  public List<Unit004832Menu.OhterInfo> otherInfos;
  public List<Dictionary<string, object>> showPopupDatas;
  private PlayerUnit BeforeUnit;
  private PlayerUnit AfterUnit;
  private int displaySkillIndex;
  private GameObject SkillLevelupPrefab;
  private GameObject[] SkillLevelupObject = new GameObject[2];
  private EffectSkillAcquisition overkillersSkill;
  private bool waitNextOverkillersSkill;
  private GameObject UnityGrowthResultPrefab;
  private GrowthParameter parameterPanel;
  private Dictionary<int, Sprite> dicSkillIcons = new Dictionary<int, Sprite>();
  private int medal;
  private bool isExtraSkillRelease;
  private bool isExtraSkillRemembere;
  private NGSoundManager sm;
  private bool isDispNextSkill;
  private bool isPlayPopup;
  private bool isFirstTimeGaugeMax = true;

  public Unit00468Scene.Mode mode { get; set; }

  public Dictionary<string, object> showPopupData { get; set; }

  public Action onFinished { get; set; }

  private void OnDisable()
  {
    if (Object.op_Inequality((Object) this.sm, (Object) null))
      this.sm.stopVoice();
    if (!Object.op_Inequality((Object) this.loveGaugeController, (Object) null))
      return;
    this.loveGaugeController.StopSE();
  }

  private IEnumerator CreateLevelupSkillList(
    PlayerUnit beforeUnit,
    PlayerUnit afterUnit,
    List<LevelupSkill> slist)
  {
    PlayerUnitSkills[] playerUnitSkillsArray1 = afterUnit.skills;
    for (int index1 = 0; index1 < playerUnitSkillsArray1.Length; ++index1)
    {
      PlayerUnitSkills afterSkill = playerUnitSkillsArray1[index1];
      bool flg_exist = false;
      PlayerUnitSkills[] playerUnitSkillsArray2 = beforeUnit.skills;
      for (int index2 = 0; index2 < playerUnitSkillsArray2.Length; ++index2)
      {
        PlayerUnitSkills beforeSkill = playerUnitSkillsArray2[index2];
        if (!flg_exist)
        {
          if (afterSkill.skill_id == beforeSkill.skill_id && beforeSkill.level < afterSkill.level)
          {
            slist.Add(new LevelupSkill(beforeSkill.level, afterSkill.level, afterSkill.skill));
            flg_exist = true;
          }
          UnitSkillEvolution unitSkillEvolution = UnitSkillEvolution.getUnitSkillEvolution(beforeUnit.unit.ID, beforeSkill.skill_id, afterSkill.skill_id);
          if (unitSkillEvolution != null)
          {
            slist.Add(new LevelupSkill(beforeSkill.level, unitSkillEvolution.level, beforeSkill.skill));
            flg_exist = true;
            yield return (object) this.AddEvolutionSkill(beforeSkill);
          }
        }
      }
      playerUnitSkillsArray2 = (PlayerUnitSkills[]) null;
      afterSkill = (PlayerUnitSkills) null;
    }
    playerUnitSkillsArray1 = (PlayerUnitSkills[]) null;
  }

  private IEnumerator AddEvolutionSkill(PlayerUnitSkills beforeSkill)
  {
    Future<Sprite> iconF = beforeSkill.skill.LoadBattleSkillIcon();
    IEnumerator e = iconF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.dicSkillIcons[beforeSkill.skill.ID] = iconF.Result;
  }

  private void SetMaxLv(int beforeLv, int afterLv)
  {
    bool flag = beforeLv < afterLv;
    ((Component) this.beforeLvmax).gameObject.SetActive(flag);
    ((Component) this.afterLvmax).gameObject.SetActive(flag);
    this.slcArrow.SetActive(flag);
    ((Component) this.lvmaxUnchanged).gameObject.SetActive(!flag);
    if (flag)
    {
      this.beforeLvmax.SetTextLocalize(beforeLv);
      this.afterLvmax.SetTextLocalize(afterLv);
    }
    else
      this.lvmaxUnchanged.SetTextLocalize(afterLv);
  }

  private void SetLimitbreak(int beforeBreakCount, int afterBreakCount, int breakthrough_limit)
  {
    for (int index = 0; index < this.SlcLBiconNone.Length; ++index)
    {
      this.SlcLBiconNone[index].SetActive(index >= afterBreakCount && index < breakthrough_limit);
      this.SlcLBiconBlue[index].SetActive(index < afterBreakCount);
      this.SlcLBiconBlueEff[index].SetActive(index >= beforeBreakCount && index < afterBreakCount);
    }
    if (breakthrough_limit != 0)
      return;
    this.slc_Limitbreak.SetActive(false);
  }

  private IEnumerator SetUnitTexture(UnitUnit unit, int job_id)
  {
    foreach (Component component in this.unitTexture.transform)
      Object.Destroy((Object) component.gameObject);
    this.unitTexture.transform.Clear();
    Future<GameObject> future = unit.LoadMypage();
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject texObj = future.Result.Clone(this.unitTexture.transform);
    e = unit.SetLargeSpriteSnap(job_id, texObj, 4);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = unit.SetLargeSpriteWithMask(job_id, texObj, Res.GUI._004_8_13_sozai.mask_Chara.Load<Texture2D>(), 5, -146, 36);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator setCharacter(PlayerUnit beforeUnit, PlayerUnit afterUnit, List<int> otherData)
  {
    Unit004813Menu m = this;
    m.BeforeUnit = beforeUnit;
    m.AfterUnit = afterUnit;
    m.isPlayPopup = false;
    bool isSuccess = otherData[0] == 1;
    m.medal = otherData[1];
    m.isExtraSkillRelease = otherData[2] == 1;
    m.isExtraSkillRemembere = otherData[3] == 1;
    m.characterName.SetTextLocalize(m.AfterUnit.unit.name);
    Future<GameObject> loader = GrowthParameter.LoadPrefab();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    foreach (Component component in m.growthParameter.transform)
      Object.Destroy((Object) component.gameObject);
    m.growthParameter.transform.Clear();
    m.parameterPanel = GrowthParameter.GetInstance(loader.Result, m.growthParameter.transform);
    Future<GameObject> SkillLevelupPrefabF = Res.Prefabs.unit004_8_13.dir_skill.Load<GameObject>();
    e = SkillLevelupPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    m.SkillLevelupPrefab = SkillLevelupPrefabF.Result;
    if (m.AfterUnit.skills != null)
    {
      PlayerUnitSkills[] playerUnitSkillsArray = m.AfterUnit.skills;
      for (int index = 0; index < playerUnitSkillsArray.Length; ++index)
      {
        PlayerUnitSkills skill = playerUnitSkillsArray[index];
        Future<Sprite> iconF = skill.skill.LoadBattleSkillIcon();
        e = iconF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (!m.dicSkillIcons.ContainsKey(skill.skill.ID))
          m.dicSkillIcons[skill.skill.ID] = iconF.Result;
        iconF = (Future<Sprite>) null;
        skill = (PlayerUnitSkills) null;
      }
      playerUnitSkillsArray = (PlayerUnitSkills[]) null;
    }
    m.SetMaxLv(m.BeforeUnit.max_level, m.AfterUnit.max_level);
    m.SetLimitbreak(m.BeforeUnit.breakthrough_count, m.AfterUnit.breakthrough_count, m.AfterUnit.unit.breakthrough_limit);
    UnitTypeParameter unitTypeParameter = m.AfterUnit.UnitTypeParameter;
    int maxPt = ((IEnumerable<int>) new int[8]
    {
      m.AfterUnit.hp.initial + m.AfterUnit.hp.inheritance + m.AfterUnit.hp.level_up_max_status + unitTypeParameter.hp_compose_max,
      m.AfterUnit.strength.initial + m.AfterUnit.strength.inheritance + m.AfterUnit.strength.level_up_max_status + unitTypeParameter.strength_compose_max,
      m.AfterUnit.intelligence.initial + m.AfterUnit.intelligence.inheritance + m.AfterUnit.intelligence.level_up_max_status + unitTypeParameter.intelligence_compose_max,
      m.AfterUnit.vitality.initial + m.AfterUnit.vitality.inheritance + m.AfterUnit.vitality.level_up_max_status + unitTypeParameter.vitality_compose_max,
      m.AfterUnit.mind.initial + m.AfterUnit.mind.inheritance + m.AfterUnit.mind.level_up_max_status + unitTypeParameter.mind_compose_max,
      m.AfterUnit.agility.initial + m.AfterUnit.agility.inheritance + m.AfterUnit.agility.level_up_max_status + unitTypeParameter.agility_compose_max,
      m.AfterUnit.dexterity.initial + m.AfterUnit.dexterity.inheritance + m.AfterUnit.dexterity.level_up_max_status + unitTypeParameter.dexterity_compose_max,
      m.AfterUnit.lucky.initial + m.AfterUnit.lucky.inheritance + m.AfterUnit.lucky.level_up_max_status + unitTypeParameter.lucky_compose_max
    }).Max();
    m.parameterPanel.SetParameter(GrowthParameter.ParameterType.HP, m.BeforeUnit.self_total_hp, m.AfterUnit.self_total_hp, m.BeforeUnit.hp.buildup != m.AfterUnit.hp.buildup, maxPt);
    m.parameterPanel.SetParameter(GrowthParameter.ParameterType.STR, m.BeforeUnit.self_total_strength, m.AfterUnit.self_total_strength, m.BeforeUnit.strength.buildup != m.AfterUnit.strength.buildup, maxPt);
    m.parameterPanel.SetParameter(GrowthParameter.ParameterType.INT, m.BeforeUnit.self_total_intelligence, m.AfterUnit.self_total_intelligence, m.BeforeUnit.intelligence.buildup != m.AfterUnit.intelligence.buildup, maxPt);
    m.parameterPanel.SetParameter(GrowthParameter.ParameterType.VIT, m.BeforeUnit.self_total_vitality, m.AfterUnit.self_total_vitality, m.BeforeUnit.vitality.buildup != m.AfterUnit.vitality.buildup, maxPt);
    m.parameterPanel.SetParameter(GrowthParameter.ParameterType.MND, m.BeforeUnit.self_total_mind, m.AfterUnit.self_total_mind, m.BeforeUnit.mind.buildup != m.AfterUnit.mind.buildup, maxPt);
    m.parameterPanel.SetParameter(GrowthParameter.ParameterType.AGI, m.BeforeUnit.self_total_agility, m.AfterUnit.self_total_agility, m.BeforeUnit.agility.buildup != m.AfterUnit.agility.buildup, maxPt);
    m.parameterPanel.SetParameter(GrowthParameter.ParameterType.DEX, m.BeforeUnit.self_total_dexterity, m.AfterUnit.self_total_dexterity, m.BeforeUnit.dexterity.buildup != m.AfterUnit.dexterity.buildup, maxPt);
    m.parameterPanel.SetParameter(GrowthParameter.ParameterType.LUK, m.BeforeUnit.self_total_lucky, m.AfterUnit.self_total_lucky, m.BeforeUnit.lucky.buildup != m.AfterUnit.lucky.buildup, maxPt);
    m.parameterPanel.SetParameterMaxStar(GrowthParameter.ParameterType.HP, m.AfterUnit.hp.is_max);
    m.parameterPanel.SetParameterMaxStar(GrowthParameter.ParameterType.STR, m.AfterUnit.strength.is_max);
    m.parameterPanel.SetParameterMaxStar(GrowthParameter.ParameterType.INT, m.AfterUnit.intelligence.is_max);
    m.parameterPanel.SetParameterMaxStar(GrowthParameter.ParameterType.VIT, m.AfterUnit.vitality.is_max);
    m.parameterPanel.SetParameterMaxStar(GrowthParameter.ParameterType.MND, m.AfterUnit.mind.is_max);
    m.parameterPanel.SetParameterMaxStar(GrowthParameter.ParameterType.AGI, m.AfterUnit.agility.is_max);
    m.parameterPanel.SetParameterMaxStar(GrowthParameter.ParameterType.DEX, m.AfterUnit.dexterity.is_max);
    m.parameterPanel.SetParameterMaxStar(GrowthParameter.ParameterType.LUK, m.AfterUnit.lucky.is_max);
    m.parameterPanel.GaugesScaleZero();
    yield return (object) null;
    m.parameterPanel.SetParameterEffects();
    m.UnityGrowthResultPrefab = (GameObject) null;
    Future<GameObject> ft;
    if ((double) m.AfterUnit.unityTotal > (double) m.BeforeUnit.unityTotal)
    {
      ft = new ResourceObject("Prefabs/unit/Popup_UnitTouta_Result").Load<GameObject>();
      e = ft.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      m.UnityGrowthResultPrefab = ft.Result;
      ft = (Future<GameObject>) null;
    }
    List<LevelupSkill> slist = new List<LevelupSkill>();
    e = m.CreateLevelupSkillList(m.BeforeUnit, m.AfterUnit, slist);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    List<LevelupSkill> levelupSkills1 = new List<LevelupSkill>();
    List<LevelupSkill> levelupSkills2 = new List<LevelupSkill>();
    int index1 = 0;
    int num1 = 0;
    for (; index1 < slist.Count; ++index1)
    {
      if (slist[index1].skill.skill_type != BattleskillSkillType.magic && slist[index1].skill.skill_type != BattleskillSkillType.leader)
      {
        if (num1 < 4)
          levelupSkills1.Add(slist[index1]);
        else
          levelupSkills2.Add(slist[index1]);
        ++num1;
      }
    }
    ((Component) m.BackBtn).gameObject.SetActive(true);
    foreach (Component component in m.DirSkillLevelup.transform)
      Object.Destroy((Object) component.gameObject);
    m.DirSkillLevelup.transform.Clear();
    m.displaySkillIndex = 0;
    if (levelupSkills1.Count > 0)
    {
      m.SkillLevelupObject[0] = m.SkillLevelupPrefab.Clone(m.DirSkillLevelup.transform);
      m.SkillLevelupObject[0].GetComponent<SkillLevelup>().SetSkillSlots(m, m.BeforeUnit, m.AfterUnit, m.dicSkillIcons, levelupSkills1);
      m.SkillLevelupObject[0].GetComponent<SkillLevelup>().StartTween();
    }
    if (levelupSkills2.Count > 0)
    {
      m.isDispNextSkill = true;
      m.SkillLevelupObject[1] = m.SkillLevelupPrefab.Clone(m.DirSkillLevelup.transform);
      m.SkillLevelupObject[1].GetComponent<SkillLevelup>().SetSkillSlots(m, m.BeforeUnit, m.AfterUnit, m.dicSkillIcons, levelupSkills2);
      m.SkillLevelupObject[1].SetActive(false);
    }
    if (Object.op_Inequality((Object) m.lnkOverkillersSkill, (Object) null) && m.AfterUnit.unit.exist_overkillers_skill)
    {
      m.lnkOverkillersSkill.SetActive(false);
      OverkillersSkillRelease s = m.AfterUnit.overkillersSkill;
      if (s != null && (double) m.BeforeUnit.unityTotal < (double) s.unity_value && (double) s.unity_value <= (double) m.AfterUnit.unityTotal)
      {
        ft = EffectSkillAcquisition.createLoader();
        e = ft.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        m.overkillersSkill = ft.Result.Clone(m.lnkOverkillersSkill.transform).GetComponent<EffectSkillAcquisition>();
        m.overkillersSkill.initialize(s.skill);
        ((Component) m.overkillersSkill).GetComponent<UIPanel>().depth = m.lnkOverkillersSkill.GetComponent<UIPanel>().depth + 1;
        ((Component) m.lnkOverkillersSkill.GetComponentInChildren<UIButton>(true)).gameObject.SetActive(true);
        ft = (Future<GameObject>) null;
      }
      else
        m.overkillersSkill = (EffectSkillAcquisition) null;
      s = (OverkillersSkillRelease) null;
    }
    else if (Object.op_Inequality((Object) m.lnkOverkillersSkill, (Object) null))
      m.lnkOverkillersSkill.SetActive(false);
    Consts consts = Consts.GetInstance();
    ((Component) m.loveGaugeController).gameObject.SetActive(false);
    if (m.AfterUnit.unit.trust_target_flag)
    {
      string dearSpriteName = "slc_text_love_parameter.png__GUI__004-8-13_sozai__004-8-13_sozai_prefab";
      string relevanceSpriteName = "slc_text_Relevance.png__GUI__004-8-13_sozai__004-8-13_sozai_prefab";
      double num2 = Math.Round((double) m.AfterUnit.trust_rate * 100.0) / 100.0;
      m.textDearPercent.SetTextLocalize(Consts.Format(consts.UNIT_TRUST_RATE_PERCENT, (IDictionary) new Hashtable()
      {
        {
          (object) "trust_rate",
          (object) string.Format("{0}", (object) num2)
        }
      }));
      ((Component) m.loveGaugeController).gameObject.SetActive(true);
      m.loveGaugeController.SetGaugeText(m.AfterUnit.unit, dearSpriteName, relevanceSpriteName);
      m.StartCoroutine(m.loveGaugeController.setValue((int) m.BeforeUnit.trust_rate, (int) m.AfterUnit.trust_rate, (int) m.AfterUnit.trust_max_rate, (int) consts.TRUST_RATE_LEVEL_SIZE, true, true));
    }
    e = m.SetUnitTexture(m.AfterUnit.unit, m.AfterUnit.job_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    m.sm = Singleton<NGSoundManager>.GetInstance();
    if (Object.op_Inequality((Object) m.sm, (Object) null))
    {
      m.sm.stopVoice();
      if (isSuccess)
        m.sm.playVoiceByID(m.AfterUnit.unit.unitVoicePattern, 56);
    }
    m.isFirstTimeGaugeMax = (double) m.BeforeUnit.trust_rate < (double) consts.TRUST_RATE_LEVEL_SIZE && (double) m.AfterUnit.trust_rate >= (double) consts.TRUST_RATE_LEVEL_SIZE;
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet() || this.isPlayPopup)
      return;
    this.isPlayPopup = true;
    this.StartCoroutine(this.PopupSequence());
  }

  public void OnSkipButton()
  {
    this.selectedMaterialPlayerUnits.Clear();
    this.resultPlayerUnits.Clear();
    this.otherInfos.Clear();
    this.showPopupDatas.Clear();
    this.IbtnBack();
  }

  public override void onBackButton() => this.IbtnBack();

  public void StartNextTween()
  {
    ++this.displaySkillIndex;
    if (this.displaySkillIndex < this.SkillLevelupObject.Length)
    {
      this.SkillLevelupObject[this.displaySkillIndex].SetActive(true);
      this.SkillLevelupObject[this.displaySkillIndex].GetComponent<SkillLevelup>().StartTween();
    }
    if (this.displaySkillIndex > this.SkillLevelupObject.Length - 1)
      return;
    this.isDispNextSkill = false;
  }

  private GameObject OpenPopup(GameObject original)
  {
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(original);
    ((Component) gameObject.transform.parent.Find("Popup Mask")).gameObject.GetComponent<TweenAlpha>().to = 0.75f;
    return gameObject;
  }

  private IEnumerator PopupSequence()
  {
    Unit004813Menu unit004813Menu = this;
    Unit004813Menu.Runner[] runnerArray1 = new Unit004813Menu.Runner[9]
    {
      new Unit004813Menu.Runner(unit004813Menu.UnityUpPopup),
      new Unit004813Menu.Runner(unit004813Menu.NextSkillPopup),
      new Unit004813Menu.Runner(unit004813Menu.OverkillersSkillPopup),
      new Unit004813Menu.Runner(unit004813Menu.MedalPopup),
      new Unit004813Menu.Runner(unit004813Menu.CharacterQuestStoryPopup),
      new Unit004813Menu.Runner(unit004813Menu.SkillRankUpPopup),
      new Unit004813Menu.Runner(unit004813Menu.ExtraSKillReleaseSequence),
      new Unit004813Menu.Runner(unit004813Menu.ExtraSKillRemembereSequence),
      new Unit004813Menu.Runner(unit004813Menu.FinishSequence)
    };
    unit004813Menu.loveGaugeController.StopGaugeAnimation();
    Unit004813Menu.Runner[] runnerArray = runnerArray1;
    for (int index = 0; index < runnerArray.Length; ++index)
    {
      IEnumerator e = runnerArray[index]();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    runnerArray = (Unit004813Menu.Runner[]) null;
  }

  private IEnumerator UnityUpPopup()
  {
    if (!Object.op_Equality((Object) this.UnityGrowthResultPrefab, (Object) null))
    {
      PopupUnityGrowthResult popup = this.OpenPopup(this.UnityGrowthResultPrefab).GetComponent<PopupUnityGrowthResult>();
      IEnumerator e = popup.Initialize(this.BeforeUnit, this.AfterUnit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      bool isFinished = false;
      popup.SetOnFinishCallback((Action) (() => isFinished = true));
      yield return (object) new WaitForSeconds(0.33f);
      popup.StartGaugeAnime();
      while (!isFinished)
        yield return (object) null;
    }
  }

  private IEnumerator NextSkillPopup()
  {
    if (this.isDispNextSkill)
    {
      this.SkillLevelupObject[this.displaySkillIndex].GetComponent<SkillLevelup>().EndTween();
      while (this.isDispNextSkill)
        yield return (object) null;
    }
  }

  public void onClickedNextOverkillersSkill() => this.waitNextOverkillersSkill = false;

  private IEnumerator OverkillersSkillPopup()
  {
    if (!Object.op_Equality((Object) this.overkillersSkill, (Object) null))
    {
      this.waitNextOverkillersSkill = true;
      this.lnkOverkillersSkill.SetActive(true);
      while (this.waitNextOverkillersSkill || this.overkillersSkill.isRunning)
        yield return (object) null;
      ((Component) this.lnkOverkillersSkill.GetComponentInChildren<UIButton>()).gameObject.SetActive(false);
    }
  }

  private IEnumerator MedalPopup()
  {
    if (this.medal != 0)
    {
      Future<GameObject> prefabf = Res.Prefabs.popup.popup_004_8_13__anim_popup01.Load<GameObject>();
      IEnumerator e = prefabf.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = prefabf.Result;
      Popup004813Menu component = Singleton<PopupManager>.GetInstance().openAlert(result).GetComponent<Popup004813Menu>();
      component.SetText(this.medal);
      bool isFinished = false;
      component.SetIbtnOKCallback((Action) (() => isFinished = true));
      while (!isFinished)
        yield return (object) null;
    }
  }

  private IEnumerator CharacterQuestStoryPopup()
  {
    UnlockQuest[] unlockQuests = (UnlockQuest[]) this.showPopupData["unlockQuests"];
    Future<GameObject> prefab = Res.Prefabs.battle.popup_020_11_2__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (unlockQuests != null && unlockQuests.Length != 0)
    {
      UnlockQuest[] unlockQuestArray = unlockQuests;
      for (int index = 0; index < unlockQuestArray.Length; ++index)
      {
        QuestCharacterS quest = MasterData.QuestCharacterS[unlockQuestArray[index].quest_s_id];
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1028", delay: 0.8f);
        Battle020112Menu o = this.OpenPopup(prefab.Result).GetComponent<Battle020112Menu>();
        e = o.Init(quest);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        bool isFinished = false;
        o.SetCallback((Action) (() => isFinished = true));
        while (!isFinished)
          yield return (object) null;
        o = (Battle020112Menu) null;
      }
      unlockQuestArray = (UnlockQuest[]) null;
    }
  }

  private IEnumerator SkillRankUpPopup()
  {
    int beforeSkillId = (int) this.showPopupData["beforeSkillId"];
    int afterSkillId = (int) this.showPopupData["afterSkillId"];
    if (beforeSkillId > 0 && afterSkillId > 0)
    {
      Future<GameObject> charaSkillPrefabF = Res.Prefabs.battle.CharaSkillRankUpPrefab.Load<GameObject>();
      IEnumerator e = charaSkillPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject popup = this.OpenPopup(charaSkillPrefabF.Result);
      popup.SetActive(false);
      Battle02029Menu o = popup.GetComponent<Battle02029Menu>();
      e = o.InitForEvolution(this.AfterUnit.unit.ID, afterSkillId, beforeSkillId);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      bool isFinished = false;
      o.SetCallback((Action) (() => isFinished = true));
      this.unitTexture.SetActive(false);
      popup.SetActive(true);
      yield return (object) new WaitForSeconds(0.6f);
      while (!isFinished)
        yield return (object) null;
      charaSkillPrefabF = (Future<GameObject>) null;
      popup = (GameObject) null;
      o = (Battle02029Menu) null;
    }
  }

  private IEnumerator ExtraSKillReleaseSequence()
  {
    if (this.isExtraSkillRelease)
    {
      yield return (object) new WaitForSeconds(0.6f);
      GameObject Prefab = (GameObject) null;
      Future<GameObject> prefabF;
      IEnumerator e;
      if (this.AfterUnit.unit.IsSea)
      {
        prefabF = new ResourceObject("Animations/extraskill/FavorabilityRatingEffect").Load<GameObject>();
        e = prefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        Prefab = prefabF.Result.Clone(Singleton<CommonRoot>.GetInstance().LoadTmpObj.transform);
        prefabF = (Future<GameObject>) null;
      }
      else if (this.AfterUnit.unit.IsResonanceUnit)
      {
        prefabF = new ResourceObject("Animations/common_gear_skill/CommonGearSkillEffect").Load<GameObject>();
        e = prefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        Prefab = prefabF.Result.Clone(Singleton<CommonRoot>.GetInstance().LoadTmpObj.transform);
        prefabF = (Future<GameObject>) null;
      }
      if (!Object.op_Equality((Object) Prefab, (Object) null))
      {
        bool isFinished = false;
        e = Prefab.GetComponent<FavorabilityRatingEffect>().Init(FavorabilityRatingEffect.AnimationType.SkillFrameRelease, this.AfterUnit, (Action) (() =>
        {
          Singleton<PopupManager>.GetInstance().dismiss();
          isFinished = true;
        }));
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        Singleton<PopupManager>.GetInstance().open(Prefab, isCloned: true);
        Prefab.GetComponent<FavorabilityRatingEffect>().StartEffect();
        while (!isFinished)
          yield return (object) null;
        Prefab = (GameObject) null;
      }
    }
  }

  private IEnumerator ExtraSKillRemembereSequence()
  {
    if (this.isExtraSkillRemembere)
    {
      yield return (object) new WaitForSeconds(0.6f);
      GameObject Prefab = (GameObject) null;
      Future<GameObject> prefabF;
      IEnumerator e;
      if (this.AfterUnit.unit.IsSea)
      {
        prefabF = new ResourceObject("Animations/extraskill/FavorabilityRatingEffect").Load<GameObject>();
        e = prefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        Prefab = prefabF.Result.Clone(Singleton<CommonRoot>.GetInstance().LoadTmpObj.transform);
        prefabF = (Future<GameObject>) null;
      }
      else if (this.AfterUnit.unit.IsResonanceUnit)
      {
        prefabF = new ResourceObject("Animations/common_gear_skill/CommonGearSkillEffect").Load<GameObject>();
        e = prefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        Prefab = prefabF.Result.Clone(Singleton<CommonRoot>.GetInstance().LoadTmpObj.transform);
        prefabF = (Future<GameObject>) null;
      }
      if (!Object.op_Equality((Object) Prefab, (Object) null))
      {
        bool isFinished = false;
        e = Prefab.GetComponent<FavorabilityRatingEffect>().Init(FavorabilityRatingEffect.AnimationType.SkillRelease, this.AfterUnit, (Action) (() =>
        {
          Singleton<PopupManager>.GetInstance().dismiss();
          isFinished = true;
        }), !this.isFirstTimeGaugeMax);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        Singleton<PopupManager>.GetInstance().open(Prefab, isCloned: true);
        Prefab.GetComponent<FavorabilityRatingEffect>().StartEffect();
        while (!isFinished)
          yield return (object) null;
        Prefab = (GameObject) null;
      }
    }
  }

  private IEnumerator FinishSequence()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit004813Menu unit004813Menu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (Object.op_Inequality((Object) unit004813Menu.parameterPanel, (Object) null))
      unit004813Menu.parameterPanel.RemoveTween();
    if (Object.op_Inequality((Object) unit004813Menu.overkillersSkill, (Object) null))
    {
      Object.Destroy((Object) ((Component) unit004813Menu.overkillersSkill).gameObject);
      unit004813Menu.overkillersSkill = (EffectSkillAcquisition) null;
    }
    if (unit004813Menu.onFinished != null)
    {
      unit004813Menu.onFinished();
      return false;
    }
    if (unit004813Menu.mode == Unit00468Scene.Mode.UnitLumpTouta)
    {
      Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
      try
      {
        unit004813Menu.selectedMaterialPlayerUnits.RemoveAt(0);
        unit004813Menu.resultPlayerUnits.RemoveAt(0);
        unit004813Menu.otherInfos.RemoveAt(0);
        unit004813Menu.showPopupDatas.RemoveAt(0);
      }
      catch (ArgumentOutOfRangeException ex)
      {
      }
      if (unit004813Menu.selectedMaterialPlayerUnits.Count <= 0)
        Singleton<NGSceneManager>.GetInstance().changeScene("unit004_LumpTouta", false);
      else
        Singleton<NGSceneManager>.GetInstance().changeScene("unit004_8_12", false, (object) unit004813Menu.selectedMaterialPlayerUnits, (object) unit004813Menu.resultPlayerUnits, (object) unit004813Menu.otherInfos, (object) unit004813Menu.showPopupDatas);
    }
    else
      unit004813Menu.backScene();
    return false;
  }

  private delegate IEnumerator Runner();
}
