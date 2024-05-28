// Decompiled with JetBrains decompiler
// Type: ResultMenuBase
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
public class ResultMenuBase : NGMenuBase
{
  protected GameObject DirUnitPrefab;
  protected int mvp_index = -1;
  protected int questSID = -1;
  protected CommonQuestType questType = CommonQuestType.Story;
  protected int deck_type_id;
  protected int deck_number;
  protected Dictionary<int, PlayerUnit> beforeUnits;
  protected Dictionary<int, PlayerUnit> afterUnits;
  protected Dictionary<int, PlayerItem> beforeGears;
  protected Dictionary<int, PlayerItem> afterGears;
  protected Dictionary<int, int> diffGearAccessoryRemainingAmounts;
  protected List<BattleEndPlayer_character_intimates_in_battle> characterIntimates = new List<BattleEndPlayer_character_intimates_in_battle>();
  protected List<UnlockIntimateSkill> unlockIntimateSkills;
  protected List<int> unlockCharacterQuestIDS = new List<int>();
  protected List<int> unlockHarmonyQuestIDS = new List<int>();
  protected List<PlayerItem> disappearedPlayerGears = new List<PlayerItem>();
  protected List<BattleEndTrust_upper_limit> trusutUpperLimits = new List<BattleEndTrust_upper_limit>();
  [SerializeField]
  protected GameObject DirUnitExp;
  [SerializeField]
  protected GameObject[] DirUnit;
  protected string CharacterIntimateUpPrefabName = "Prefabs/battle/popup_020_22_1__anim_popup01";
  protected string CharacterLoveLimitUpPrefabName = "Prefabs/popup/popup_004_limit_extended__popup01";
  public bool isSkip;
  protected List<IEnumerator> skipPopupList = new List<IEnumerator>();
  protected BattleInfo info;
  private Dictionary<string, GameObject> dicPrefab = new Dictionary<string, GameObject>();
  private bool isUnitResultEnd;
  private GameObject CharacterIntimateUpPrefab;
  private GameObject IntimateSkillGetPrefab;
  private GameObject CharacterLoveLimitUpPrefab;
  private GameObject CharacterStoryPrefab;
  private GameObject prefabCombi;
  private GameObject prefabTrio;

  public virtual void OnDestroy()
  {
    this.DirUnitPrefab = (GameObject) null;
    if (this.beforeUnits != null)
      this.beforeUnits.Clear();
    if (this.afterUnits != null)
      this.afterUnits.Clear();
    if (this.beforeGears != null)
      this.beforeGears.Clear();
    if (this.afterGears != null)
      this.afterGears.Clear();
    if (this.characterIntimates != null)
      this.characterIntimates.Clear();
    if (this.unlockCharacterQuestIDS != null)
      this.unlockCharacterQuestIDS.Clear();
    if (this.unlockHarmonyQuestIDS != null)
      this.unlockHarmonyQuestIDS.Clear();
    if (this.disappearedPlayerGears == null)
      return;
    this.disappearedPlayerGears.Clear();
  }

  private IEnumerator Init()
  {
    Future<GameObject> prefabF = !Singleton<NGGameDataManager>.GetInstance().IsSea || this.info == null || this.info.seaQuest == null ? Res.Prefabs.colosseum.colosseum023_4_6.dir_Unit.Load<GameObject>() : new ResourceObject("Prefabs/Sea/dir_Unit_sea").Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.DirUnitPrefab = prefabF.Result;
  }

  public virtual IEnumerator Init(BattleInfo info, BattleEnd result)
  {
    IEnumerator e = this.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual IEnumerator Init(BattleInfo info, BattleEnd result, int index)
  {
    IEnumerator e = this.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual IEnumerator Init(WebAPI.Response.GvgBattleFinish battleResultData)
  {
    IEnumerator e = this.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual IEnumerator Init(
    ColosseumUtility.Info info,
    ResultMenuBase.Param param,
    Gladiator gladiator)
  {
    IEnumerator e = this.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual IEnumerator Init(WebAPI.Response.PvpPlayerFinish info)
  {
    IEnumerator e = this.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual IEnumerator Init(WebAPI.Response.EventTop eventTopInfo)
  {
    IEnumerator e = this.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual IEnumerator Init(BattleInfo info, WebAPI.Response.TowerBattleFinish result)
  {
    IEnumerator e = this.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual IEnumerator Init(ResultMenuBase.Param param, Gladiator gladiator)
  {
    IEnumerator e = this.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual IEnumerator Init(BattleInfo info, WebAPI.Response.QuestCorpsBattleFinish result)
  {
    IEnumerator e = this.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual IEnumerator Init(int a)
  {
    IEnumerator e = this.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual IEnumerator Run()
  {
    yield break;
  }

  public virtual IEnumerator OnFinish()
  {
    yield break;
  }

  public virtual void OnRemove()
  {
  }

  public GameObject OpenPopup(GameObject original)
  {
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(original);
    ((Component) gameObject.transform.parent.Find("Popup Mask")).gameObject.GetComponent<TweenAlpha>().to = 0.75f;
    return gameObject;
  }

  public virtual GameObject CreateTouchObject(EventDelegate.Callback callback, Transform parent = null)
  {
    Resolution currentResolution = Screen.currentResolution;
    GameObject touchObject = new GameObject("touch object");
    touchObject.transform.parent = parent ?? ((Component) this).transform;
    UIWidget uiWidget = touchObject.AddComponent<UIWidget>();
    uiWidget.depth = 100000;
    uiWidget.width = ((Resolution) ref currentResolution).height;
    uiWidget.height = ((Resolution) ref currentResolution).width;
    BoxCollider boxCollider = touchObject.AddComponent<BoxCollider>();
    ((Collider) boxCollider).isTrigger = true;
    boxCollider.size = new Vector3()
    {
      x = (float) ((Resolution) ref currentResolution).height,
      y = (float) ((Resolution) ref currentResolution).width,
      z = 1f
    };
    UIButton uiButton = touchObject.AddComponent<UIButton>();
    ((UIButtonColor) uiButton).tweenTarget = (GameObject) null;
    EventDelegate.Add(uiButton.onClick, callback);
    return touchObject;
  }

  public void PlayTween(GameObject o)
  {
    o.SetActive(true);
    ((IEnumerable<UITweener>) o.GetComponentsInChildren<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x =>
    {
      if (x.tweenGroup != 0)
        return;
      x.ResetToBeginning();
      x.PlayForward();
    }));
  }

  protected IEnumerator ShowUnitEXP()
  {
    ResultMenuBase resultMenuBase = this;
    List<ColosseumResultUnit> unitList = new List<ColosseumResultUnit>();
    List<GaugeRunner> unitLevelUpGauge = new List<GaugeRunner>();
    List<GaugeRunner> gearLevelUpGauge = new List<GaugeRunner>();
    ((IEnumerable<GameObject>) resultMenuBase.DirUnit).ForEach<GameObject>((Action<GameObject>) (x => x.gameObject.SetActive(false)));
    IEnumerator e1;
    if (resultMenuBase.afterUnits == null || resultMenuBase.afterUnits.Count == 0)
    {
      if (resultMenuBase.checkForStoryOnly() && resultMenuBase.questType == CommonQuestType.Character)
      {
        e1 = resultMenuBase.CharacterSkillRelease();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
      }
    }
    else
    {
      int idx = 0;
      ColosseumResultUnit unit;
      foreach (KeyValuePair<int, PlayerUnit> afterUnit1 in resultMenuBase.afterUnits)
      {
        PlayerUnit afterUnit = afterUnit1.Value;
        PlayerUnit beforeUnit = resultMenuBase.beforeUnits.First<KeyValuePair<int, PlayerUnit>>((Func<KeyValuePair<int, PlayerUnit>, bool>) (x => x.Value.id == afterUnit.id)).Value;
        unit = resultMenuBase.DirUnitPrefab.Clone(resultMenuBase.DirUnit[idx].transform).GetComponent<ColosseumResultUnit>();
        unitList.Add(unit);
        e1 = unit.Init(beforeUnit, afterUnit, resultMenuBase.info);
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        unitLevelUpGauge.Add(unit.GetUnitExpGaugeRunner((Func<GameObject, int, IEnumerator>) null));
        gearLevelUpGauge.AddRange((IEnumerable<GaugeRunner>) unit.GetGearExpGaugeRunner(resultMenuBase.beforeGears, resultMenuBase.afterGears, (Func<GameObject, int, IEnumerator>) null));
        ++idx;
        unit = (ColosseumResultUnit) null;
      }
      resultMenuBase.DirUnitExp.SetActive(true);
      ((IEnumerable<GameObject>) resultMenuBase.DirUnit).ForEachIndex<GameObject>((Action<GameObject, int>) ((panel, index) =>
      {
        if (index >= this.afterUnits.Count<KeyValuePair<int, PlayerUnit>>())
          return;
        panel.SetActive(true);
        this.PlayTween(panel);
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1016", delay: (float) ((double) index / 10.0 + 0.89999997615814209));
      }));
      yield return (object) resultMenuBase.SkipWaitForSecond(1.5f);
      if (resultMenuBase.mvp_index != -1)
      {
        unitList[resultMenuBase.mvp_index].SetMVP(true);
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1017", delay: 0.05f);
        yield return (object) resultMenuBase.SkipWaitForSecond(0.4f);
      }
      if (Object.op_Inequality((Object) unitList.FirstOrDefault<ColosseumResultUnit>((Func<ColosseumResultUnit, bool>) (x => x.GetUnitExp != 0)), (Object) null))
      {
        foreach (ColosseumResultUnit colosseumResultUnit in unitList)
          colosseumResultUnit.ShowUnitExp();
        Dictionary<GaugeRunner, IEnumerator> e2 = new Dictionary<GaugeRunner, IEnumerator>();
        unitLevelUpGauge.ForEach((Action<GaugeRunner>) (x => e2.Add(x, GaugeRunner.Run(x))));
        while (true)
        {
          bool loop = false;
          e2.ForEach<KeyValuePair<GaugeRunner, IEnumerator>>((Action<KeyValuePair<GaugeRunner, IEnumerator>>) (x =>
          {
            if (!x.Value.MoveNext())
              return;
            loop = true;
          }));
          if (loop && !resultMenuBase.isSkip)
            yield return (object) null;
          else
            break;
        }
        if (resultMenuBase.isSkip)
        {
          List<GaugeRunner> removeKey = new List<GaugeRunner>();
          e2.ForEach<KeyValuePair<GaugeRunner, IEnumerator>>((Action<KeyValuePair<GaugeRunner, IEnumerator>>) (x =>
          {
            if (x.Key.IsLast())
              return;
            removeKey.Add(x.Key);
          }));
          removeKey.ForEach((Action<GaugeRunner>) (x => e2.Remove(x)));
          unitLevelUpGauge.Clear();
          unitList.ForEach((Action<ColosseumResultUnit>) (x =>
          {
            if (!x.IsNotLastPlayGaugeRunner())
              return;
            unitLevelUpGauge.Add(x.GetUnitExpLastGaugeRunner());
          }));
          e1 = GaugeRunner.Run(unitLevelUpGauge.ToArray());
          while (true)
          {
            bool loop = false;
            e2.ForEach<KeyValuePair<GaugeRunner, IEnumerator>>((Action<KeyValuePair<GaugeRunner, IEnumerator>>) (x =>
            {
              if (!x.Value.MoveNext())
                return;
              loop = true;
            }));
            if (e1.MoveNext())
              loop = true;
            if (loop)
              yield return (object) null;
            else
              break;
          }
          e1 = (IEnumerator) null;
        }
        yield return (object) resultMenuBase.SkipWaitForSecond(1.1f);
      }
      Future<GameObject> ldPrefab;
      int index1;
      foreach (ColosseumResultUnit colosseumResultUnit in unitList)
      {
        unit = colosseumResultUnit;
        List<Func<PopupResultUnitBase, IEnumerator>> sequencer = new List<Func<PopupResultUnitBase, IEnumerator>>(2);
        if (unit.IsLevelUP())
          sequencer.Add(new Func<PopupResultUnitBase, IEnumerator>(resultMenuBase.doCharacterLevelUp));
        OverkillersSkillRelease overkillersSkill;
        if ((overkillersSkill = unit.AfterUnit.overkillersSkill) != null && (double) unit.BeforeUnit.unityTotal < (double) overkillersSkill.unity_value && (double) overkillersSkill.unity_value <= (double) unit.AfterUnit.unityTotal)
          sequencer.Add(new Func<PopupResultUnitBase, IEnumerator>(resultMenuBase.doOverkillersSkillRelease));
        if (sequencer.Count > 0)
        {
          GameObject original = (GameObject) null;
          string prefabName = "UnitBasePrefab";
          if (!resultMenuBase.dicPrefab.TryGetValue(prefabName, out original))
          {
            ldPrefab = PopupResultUnitBase.createLoader();
            yield return (object) ldPrefab.Wait();
            resultMenuBase.dicPrefab[prefabName] = original = ldPrefab.Result;
            ldPrefab = (Future<GameObject>) null;
          }
          sequencer.Add(new Func<PopupResultUnitBase, IEnumerator>(resultMenuBase.doUnitResultEnd));
          resultMenuBase.isUnitResultEnd = false;
          PopupResultUnitBase UnitResult = resultMenuBase.OpenPopup(original).GetComponent<PopupResultUnitBase>();
          yield return (object) UnitResult.initialize(unit.BeforeUnit, unit.AfterUnit, (IEnumerable<Func<PopupResultUnitBase, IEnumerator>>) sequencer);
          while (!resultMenuBase.isUnitResultEnd)
          {
            if (Singleton<NGGameDataManager>.GetInstance().questAutoLap)
              UnitResult.onClickedNext();
            yield return (object) null;
          }
          prefabName = (string) null;
          UnitResult = (PopupResultUnitBase) null;
        }
        if (resultMenuBase.disappearedPlayerGears != null && resultMenuBase.disappearedPlayerGears.Count > 0)
        {
          PlayerItem[] array = resultMenuBase.beforeGears.Values.ToArray<PlayerItem>();
          PlayerUnit beforeUnit = unit.BeforeUnit;
          PlayerItem[] playerItemArray = new PlayerItem[2]
          {
            beforeUnit.FindEquippedGear(array),
            beforeUnit.FindEquippedGear2(array)
          };
          for (index1 = 0; index1 < playerItemArray.Length; ++index1)
          {
            PlayerItem beforeGear = playerItemArray[index1];
            if (beforeGear != (PlayerItem) null)
            {
              PlayerItem playerItem = resultMenuBase.disappearedPlayerGears.FirstOrDefault<PlayerItem>((Func<PlayerItem, bool>) (x => x.id == beforeGear.id));
              GearDisappearanceType[] disappearanceTypeList = MasterData.GearDisappearanceTypeList;
              bool isdisappearedPopupFinished = false;
              if (playerItem != (PlayerItem) null)
              {
                int disappearanceType = playerItem.gear.disappearance_type_GearDisappearanceType;
                GearDisappearanceType disappearanceType1 = ((IEnumerable<GearDisappearanceType>) disappearanceTypeList).FirstOrDefault<GearDisappearanceType>((Func<GearDisappearanceType, bool>) (x => x.ID == disappearanceType));
                string accessoryDisappearTitle = Consts.GetInstance().RESULT_MENU_BASE_ACCESSORY_DISAPPEAR_TITLE;
                resultMenuBase.StartCoroutine(PopupCommon.Show(accessoryDisappearTitle, disappearanceType1.discription, (Action) (() => isdisappearedPopupFinished = true)));
                while (!isdisappearedPopupFinished)
                {
                  if (Singleton<NGGameDataManager>.GetInstance().questAutoLap)
                    Singleton<NGGameDataManager>.GetInstance().questAutoLap = false;
                  yield return (object) null;
                }
              }
            }
          }
          playerItemArray = (PlayerItem[]) null;
        }
        sequencer = (List<Func<PopupResultUnitBase, IEnumerator>>) null;
        unit = (ColosseumResultUnit) null;
      }
      List<KeyValuePair<PlayerUnit, PlayerUnit>> results = new List<KeyValuePair<PlayerUnit, PlayerUnit>>();
      foreach (ColosseumResultUnit colosseumResultUnit in unitList)
      {
        if ((double) colosseumResultUnit.AfterUnit.unityTotal > (double) colosseumResultUnit.BeforeUnit.unityTotal)
          results.Add(new KeyValuePair<PlayerUnit, PlayerUnit>(colosseumResultUnit.BeforeUnit, colosseumResultUnit.AfterUnit));
      }
      if (results.Count > 0)
      {
        e1 = resultMenuBase.UnityGrowthEffect(results);
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
      }
      if (resultMenuBase.questType == CommonQuestType.Character)
      {
        e1 = resultMenuBase.CharacterSkillRelease();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
      }
      GameObject popup1;
      if (resultMenuBase.questType == CommonQuestType.Harmony)
      {
        UnitSkillHarmonyQuest[] harmonyquests = MasterData.UnitSkillHarmonyQuestList;
        ldPrefab = Res.Prefabs.battle.PartnerSkillGetPrefab.Load<GameObject>();
        e1 = ldPrefab.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        UnitSkillHarmonyQuest[] skillHarmonyQuestArray = harmonyquests;
        for (index1 = 0; index1 < skillHarmonyQuestArray.Length; ++index1)
        {
          UnitSkillHarmonyQuest harmonyquest = skillHarmonyQuestArray[index1];
          if (resultMenuBase.questSID == harmonyquest.character_quest_QuestHarmonyS)
          {
            yield return (object) new WaitForSeconds(1.1f);
            popup1 = resultMenuBase.OpenPopup(ldPrefab.Result);
            popup1.SetActive(false);
            Battle02029Menu o = popup1.GetComponent<Battle02029Menu>();
            e1 = o.Init(harmonyquest.character.GetDefaultUnitUnit().ID, harmonyquest.skill_BattleskillSkill);
            while (e1.MoveNext())
              yield return e1.Current;
            e1 = (IEnumerator) null;
            popup1.SetActive(true);
            bool isFinished = false;
            o.SetCallback((Action) (() => isFinished = true));
            while (!isFinished)
            {
              if (Singleton<NGGameDataManager>.GetInstance().questAutoLap)
                o.IbtnScreen();
              yield return (object) null;
            }
            yield return (object) new WaitForSeconds(0.6f);
            popup1 = (GameObject) null;
            o = (Battle02029Menu) null;
          }
          harmonyquest = (UnitSkillHarmonyQuest) null;
        }
        skillHarmonyQuestArray = (UnitSkillHarmonyQuest[]) null;
        harmonyquests = (UnitSkillHarmonyQuest[]) null;
        ldPrefab = (Future<GameObject>) null;
      }
      if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
      {
        unitList.Clear();
        unitLevelUpGauge.Clear();
        gearLevelUpGauge.Clear();
      }
      if (Object.op_Implicit((Object) unitList.FirstOrDefault<ColosseumResultUnit>((Func<ColosseumResultUnit, bool>) (x => x.IsProficiencyLevelUp()))))
      {
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1027", delay: 0.1f);
        foreach (ColosseumResultUnit colosseumResultUnit in unitList)
          colosseumResultUnit.OpenAfterProficiency();
        yield return (object) new WaitForSeconds(0.7f);
      }
      foreach (ColosseumResultUnit colosseumResultUnit in unitList)
      {
        if (colosseumResultUnit.ShowWeaponBreak() && !colosseumResultUnit.IsWeaponBreakStopAnim())
        {
          Singleton<NGSoundManager>.GetInstance().playSE("SE_1025", delay: 0.15f);
          yield return (object) new WaitForSeconds(0.1f);
        }
      }
      bool isGearLevelUp = false;
      foreach (GaugeRunner runner in gearLevelUpGauge)
      {
        if (runner != null)
        {
          isGearLevelUp = true;
          e1 = GaugeRunner.Run(runner);
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
        }
      }
      if (isGearLevelUp)
      {
        foreach (ColosseumResultUnit colosseumResultUnit in unitList)
          colosseumResultUnit.ShowWeaponRankUp();
        yield return (object) new WaitForSeconds(1f);
      }
      foreach (ColosseumResultUnit colosseumResultUnit in unitList)
      {
        unit = colosseumResultUnit;
        popup1 = (GameObject) null;
        if (unit.ShowGearUpgradeSkill())
        {
          if (Object.op_Equality((Object) popup1, (Object) null))
          {
            ldPrefab = Res.Prefabs.battle.BuguSkillGetPrefab.Load<GameObject>();
            e1 = ldPrefab.Wait();
            while (e1.MoveNext())
              yield return e1.Current;
            e1 = (IEnumerator) null;
            popup1 = ldPrefab.Result;
            ldPrefab = (Future<GameObject>) null;
          }
          foreach (Tuple<bool, PlayerItem, GearGearSkill> gearUpgradeSkill in unit.GearUpgradeSkills)
          {
            GameObject popup2 = resultMenuBase.OpenPopup(popup1);
            popup2.SetActive(false);
            BattleResultBuguSkillGet o = popup2.GetComponent<BattleResultBuguSkillGet>();
            e1 = o.Init(gearUpgradeSkill.Item1, new GameCore.ItemInfo(gearUpgradeSkill.Item2), gearUpgradeSkill.Item3);
            while (e1.MoveNext())
              yield return e1.Current;
            e1 = (IEnumerator) null;
            popup2.SetActive(true);
            o.doStart();
            bool isFinished = false;
            o.SetCallback((Action) (() => isFinished = true));
            while (!isFinished)
            {
              if (Singleton<NGGameDataManager>.GetInstance().questAutoLap)
              {
                yield return (object) new WaitForSeconds(3f);
                o.onClick();
              }
              yield return (object) null;
            }
            yield return (object) new WaitForSeconds(0.6f);
            popup2 = (GameObject) null;
            o = (BattleResultBuguSkillGet) null;
          }
        }
        popup1 = (GameObject) null;
        unit = (ColosseumResultUnit) null;
      }
      unitList.Clear();
      unitLevelUpGauge.Clear();
      gearLevelUpGauge.Clear();
    }
  }

  private IEnumerator doCharacterLevelUp(PopupResultUnitBase popupMain)
  {
    string prefabName = "UnitLevelupPrefab";
    GameObject result;
    if (!this.dicPrefab.TryGetValue(prefabName, out result))
    {
      Future<GameObject> UnitLevelUpPrefabF = Res.Prefabs.battle.UnitLevelUpPrefab.Load<GameObject>();
      yield return (object) UnitLevelUpPrefabF.Wait();
      this.dicPrefab[prefabName] = result = UnitLevelUpPrefabF.Result;
      UnitLevelUpPrefabF = (Future<GameObject>) null;
    }
    if (this.diffGearAccessoryRemainingAmounts == null)
      this.makeDiffGearAccessoryRemainingAmounts(this.beforeGears.Select<KeyValuePair<int, PlayerItem>, PlayerItem>((Func<KeyValuePair<int, PlayerItem>, PlayerItem>) (kv => kv.Value)).ToArray<PlayerItem>());
    GameObject popup = popupMain.attach(result);
    popup.SetActive(false);
    Battle020191Menu o = popup.GetComponent<Battle020191Menu>();
    IEnumerator e = o.Init(popupMain.beforeUnit, popupMain.afterUnit, this.beforeGears, this.diffGearAccessoryRemainingAmounts, fromUnitBase: true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
    popupMain.onNext = new Action(o.IbtnScreen);
    bool isFinished = false;
    o.SetCallback((Action) (() => isFinished = true));
    while (!isFinished)
    {
      if (Singleton<NGGameDataManager>.GetInstance().questAutoLap)
        o.IbtnScreen();
      yield return (object) null;
    }
    yield return (object) new WaitForSeconds(0.6f);
  }

  private IEnumerator doOverkillersSkillRelease(PopupResultUnitBase popupMain)
  {
    bool bWait = true;
    popupMain.onNext = (Action) (() => bWait = false);
    GameObject effectOverkillersSkillPrefab = (GameObject) null;
    string effectPrefabName = "effectOverkillersSkillPrefab";
    Future<GameObject> ldPrefab;
    if (!this.dicPrefab.TryGetValue(effectPrefabName, out effectOverkillersSkillPrefab))
    {
      ldPrefab = EffectOverkillersSkillRelease.createLoader();
      yield return (object) ldPrefab.Wait();
      this.dicPrefab[effectPrefabName] = effectOverkillersSkillPrefab = ldPrefab.Result;
      ldPrefab = (Future<GameObject>) null;
    }
    GameObject skillPrefab = (GameObject) null;
    string prefabName = "skillAcquisitionPrefab";
    if (!this.dicPrefab.TryGetValue(prefabName, out skillPrefab))
    {
      ldPrefab = EffectSkillAcquisition.createLoader();
      yield return (object) ldPrefab.Wait();
      this.dicPrefab[prefabName] = skillPrefab = ldPrefab.Result;
      ldPrefab = (Future<GameObject>) null;
    }
    popupMain.attach(effectOverkillersSkillPrefab).GetComponent<EffectOverkillersSkillRelease>().initialize(skillPrefab, popupMain.playerUnit, popupMain.beforeUnit.level < popupMain.afterUnit.level, new int?((int) popupMain.beforeUnit.unityTotal), new int?((int) popupMain.afterUnit.unityTotal));
    while (bWait)
    {
      if (Singleton<NGGameDataManager>.GetInstance().questAutoLap)
        popupMain.onNext();
      yield return (object) null;
    }
    yield return (object) new WaitForSeconds(0.6f);
  }

  private IEnumerator doUnitResultEnd(PopupResultUnitBase popupMain)
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    yield return (object) new WaitForSeconds(0.6f);
    this.isUnitResultEnd = true;
  }

  private IEnumerator CharacterIntimates(BattleEndPlayer_character_intimates_in_battle p, int i)
  {
    if (Object.op_Equality((Object) this.CharacterIntimateUpPrefab, (Object) null))
    {
      Future<GameObject> CharacterIntimateUpPrefabF = Singleton<ResourceManager>.GetInstance().Load<GameObject>(this.CharacterIntimateUpPrefabName);
      IEnumerator e = CharacterIntimateUpPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.CharacterIntimateUpPrefab = CharacterIntimateUpPrefabF.Result;
      CharacterIntimateUpPrefabF = (Future<GameObject>) null;
    }
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1020", delay: 0.5f, seChannel: i % 2 + 1);
    UnitCharacter unitCharacter1 = MasterData.UnitCharacter[p.character_id];
    UnitCharacter unitCharacter2 = MasterData.UnitCharacter[p.target_character_id];
    Battle020221Menu o = this.OpenPopup(this.CharacterIntimateUpPrefab).GetComponent<Battle020221Menu>();
    o.Init(p.character_id, p.target_character_id, unitCharacter1.name, unitCharacter2.name, p.before_level, p.after_level);
    bool isFinished = false;
    o.SetCallback((Action) (() => isFinished = true));
    while (!isFinished)
    {
      if (Singleton<NGGameDataManager>.GetInstance().questAutoLap)
      {
        yield return (object) new WaitForSeconds(3f);
        o.IbtnScreen();
      }
      yield return (object) null;
    }
    yield return (object) new WaitForSeconds(0.6f);
  }

  private IEnumerator IntimateSkillGet(UnlockIntimateSkill unlockIntimateSkill)
  {
    IEnumerator e;
    if (Object.op_Equality((Object) this.IntimateSkillGetPrefab, (Object) null))
    {
      Future<GameObject> IntimateSkillGetPrefabF = Res.Prefabs.battle.IntimateSkillGetPrefab.Load<GameObject>();
      e = IntimateSkillGetPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.IntimateSkillGetPrefab = IntimateSkillGetPrefabF.Result;
      IntimateSkillGetPrefabF = (Future<GameObject>) null;
    }
    yield return (object) new WaitForSeconds(1.1f);
    GameObject popup = this.OpenPopup(this.IntimateSkillGetPrefab);
    popup.SetActive(false);
    Battle02029Menu o = popup.GetComponent<Battle02029Menu>();
    e = o.Init(unlockIntimateSkill.unit_id, unlockIntimateSkill.skill_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
    bool isFinished = false;
    o.SetCallback((Action) (() => isFinished = true));
    while (!isFinished)
    {
      if (Singleton<NGGameDataManager>.GetInstance().questAutoLap)
        o.IbtnScreen();
      yield return (object) null;
    }
    yield return (object) new WaitForSeconds(0.6f);
  }

  protected IEnumerator CharacterIntimatesPopup()
  {
    IEnumerator e;
    for (int i = 0; i < this.characterIntimates.Count<BattleEndPlayer_character_intimates_in_battle>(); ++i)
    {
      BattleEndPlayer_character_intimates_in_battle characterIntimate = this.characterIntimates[i];
      if (characterIntimate.after_level > characterIntimate.before_level)
      {
        if (this.isSkip)
        {
          this.skipPopupList.Add(this.CharacterIntimates(characterIntimate, i));
        }
        else
        {
          e = this.CharacterIntimates(characterIntimate, i);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
    }
    if (this.unlockIntimateSkills != null && this.unlockIntimateSkills.Count > 0)
    {
      foreach (UnlockIntimateSkill unlockIntimateSkill in this.unlockIntimateSkills)
      {
        if (this.isSkip)
        {
          this.skipPopupList.Add(this.IntimateSkillGet(unlockIntimateSkill));
        }
        else
        {
          e = this.IntimateSkillGet(unlockIntimateSkill);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
    }
  }

  private IEnumerator CharacterLoveLimit(int i)
  {
    if (Object.op_Equality((Object) this.CharacterLoveLimitUpPrefab, (Object) null))
    {
      Future<GameObject> CharacterIntimateUpPrefabF = Singleton<ResourceManager>.GetInstance().Load<GameObject>(this.CharacterLoveLimitUpPrefabName);
      IEnumerator e = CharacterIntimateUpPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.CharacterLoveLimitUpPrefab = CharacterIntimateUpPrefabF.Result;
      CharacterIntimateUpPrefabF = (Future<GameObject>) null;
    }
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1020", delay: 0.5f, seChannel: i % 2 + 1);
    GameObject gameObject = this.OpenPopup(this.CharacterLoveLimitUpPrefab);
    UnitUnit unit = ((IEnumerable<UnitUnit>) MasterData.UnitUnitList).Where<UnitUnit>((Func<UnitUnit, bool>) (x => x.same_character_id == this.trusutUpperLimits[i].same_character_id)).FirstOrDefault<UnitUnit>();
    if (unit != null)
    {
      Popup004LoveLimitExtendedMenu o = gameObject.GetComponent<Popup004LoveLimitExtendedMenu>();
      o.Init(unit, unit.name, this.trusutUpperLimits[i].before_value, this.trusutUpperLimits[i].after_value);
      bool isFinished = false;
      o.SetCallback((Action) (() => isFinished = true));
      while (!isFinished)
      {
        if (Singleton<NGGameDataManager>.GetInstance().questAutoLap)
          o.PopUpTap();
        yield return (object) null;
      }
      yield return (object) new WaitForSeconds(0.6f);
    }
  }

  protected IEnumerator CharacterLoveLimitPopup()
  {
    for (int i = 0; i < this.trusutUpperLimits.Count; ++i)
    {
      if (this.isSkip)
      {
        this.skipPopupList.Add(this.CharacterLoveLimit(i));
      }
      else
      {
        IEnumerator e = this.CharacterLoveLimit(i);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  private IEnumerator CharacterStory(int i)
  {
    QuestCharacterS quest = MasterData.QuestCharacterS[this.unlockCharacterQuestIDS[i]];
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1028", delay: 0.8f, seChannel: i % 2 + 1);
    GameObject popup = this.OpenPopup(this.CharacterStoryPrefab);
    popup.SetActive(false);
    Battle020112Menu o = popup.GetComponent<Battle020112Menu>();
    IEnumerator e = o.Init(quest);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
    bool isFinished = false;
    o.SetCallback((Action) (() => isFinished = true));
    while (!isFinished)
    {
      if (Singleton<NGGameDataManager>.GetInstance().questAutoLap)
      {
        yield return (object) new WaitForSeconds(3f);
        o.PopUpTap();
      }
      yield return (object) null;
    }
    yield return (object) new WaitForSeconds(0.6f);
  }

  protected IEnumerator CharacterStoryPopup()
  {
    if (this.unlockCharacterQuestIDS.Count<int>() != 0)
    {
      IEnumerator e;
      if (Object.op_Equality((Object) this.CharacterStoryPrefab, (Object) null))
      {
        Future<GameObject> prefab = Res.Prefabs.battle.popup_020_11_2__anim_popup01.Load<GameObject>();
        e = prefab.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.CharacterStoryPrefab = prefab.Result;
        prefab = (Future<GameObject>) null;
      }
      for (int i = 0; i < this.unlockCharacterQuestIDS.Count<int>(); ++i)
      {
        if (this.isSkip)
        {
          this.skipPopupList.Add(this.CharacterStory(i));
        }
        else
        {
          e = this.CharacterStory(i);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
    }
  }

  private IEnumerator HarmonyStory(int i)
  {
    QuestHarmonyS quest = MasterData.QuestHarmonyS[this.unlockHarmonyQuestIDS[i]];
    Future<GameObject> prefab;
    IEnumerator e;
    if (!quest.target_unit2_UnitUnit.HasValue)
    {
      if (Object.op_Equality((Object) this.prefabCombi, (Object) null))
      {
        prefab = Res.Prefabs.battle.popup_020_11_3__anim_popup01.Load<GameObject>();
        e = prefab.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.prefabCombi = prefab.Result;
        prefab = (Future<GameObject>) null;
      }
      GameObject gameObject = this.OpenPopup(this.prefabCombi);
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1028", delay: 0.8f, seChannel: i % 2 + 1);
      Popup020113Menu o = gameObject.GetComponent<Popup020113Menu>();
      e = o.Init(quest);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      bool isFinished = false;
      o.SetCallback((Action) (() => isFinished = true));
      while (!isFinished)
      {
        if (Singleton<NGGameDataManager>.GetInstance().questAutoLap)
          o.PopUpTap();
        yield return (object) null;
      }
      yield return (object) new WaitForSeconds(0.6f);
      o = (Popup020113Menu) null;
    }
    else
    {
      if (Object.op_Equality((Object) this.prefabTrio, (Object) null))
      {
        prefab = Res.Prefabs.battle.popup_020_11_4__anim_popup01.Load<GameObject>();
        e = prefab.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.prefabTrio = prefab.Result;
        prefab = (Future<GameObject>) null;
      }
      GameObject gameObject = this.OpenPopup(this.prefabTrio);
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1028", delay: 0.8f, seChannel: i % 2 + 1);
      Popup020114Menu o = gameObject.GetComponent<Popup020114Menu>();
      e = o.Init(quest);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      bool isFinished = false;
      o.SetCallback((Action) (() => isFinished = true));
      while (!isFinished)
      {
        if (Singleton<NGGameDataManager>.GetInstance().questAutoLap)
          o.PopUpTap();
        yield return (object) null;
      }
      yield return (object) new WaitForSeconds(0.6f);
      o = (Popup020114Menu) null;
    }
  }

  protected IEnumerator HarmonyStoryPopup()
  {
    if (this.unlockHarmonyQuestIDS.Count<int>() != 0)
    {
      for (int i = 0; i < this.unlockHarmonyQuestIDS.Count<int>(); ++i)
      {
        if (this.isSkip)
        {
          this.skipPopupList.Add(this.HarmonyStory(i));
        }
        else
        {
          IEnumerator e = this.HarmonyStory(i);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
    }
  }

  protected IEnumerator SkipPopupPlay()
  {
    if (this.isSkip)
    {
      while (true)
      {
        this.isSkip = false;
        if (this.skipPopupList.Count != 0)
        {
          IEnumerator e = this.skipPopupList[0];
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          this.skipPopupList.Remove(this.skipPopupList[0]);
        }
        else
          break;
      }
      this.skipPopupList.Clear();
    }
  }

  protected IEnumerator SkipWaitForSecond(float waitTime)
  {
    while ((double) waitTime > 0.0 && !this.isSkip)
    {
      waitTime -= Time.deltaTime;
      yield return (object) null;
    }
  }

  private IEnumerator UnityGrowthEffect(List<KeyValuePair<PlayerUnit, PlayerUnit>> results)
  {
    Future<GameObject> ft = new ResourceObject("Prefabs/unit/Popup_UnitTouta_Result").Load<GameObject>();
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    PopupUnityGrowthResult popup = this.OpenPopup(ft.Result).GetComponent<PopupUnityGrowthResult>();
    e = popup.Initialize(results);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    bool isFinished = false;
    popup.SetOnFinishCallback((Action) (() => isFinished = true));
    yield return (object) new WaitForSeconds(0.33f);
    popup.StartGaugeAnime();
    while (!isFinished)
    {
      if (Singleton<NGGameDataManager>.GetInstance().questAutoLap)
      {
        popup.onBackButton();
        yield return (object) new WaitForSeconds(1f);
      }
      yield return (object) null;
    }
    yield return (object) new WaitForSeconds(0.3f);
  }

  protected IEnumerator CharacterSkillRelease()
  {
    Future<GameObject> charaSkillPrefabF = (Future<GameObject>) null;
    int skillId = 0;
    int skillIdBefore = 0;
    ResultMenuBase.EffectTYpe effectType = ResultMenuBase.EffectTYpe.NONE;
    foreach (IGrouping<Tuple<int, int>, UnitSkillCharacterQuest> source in ((IEnumerable<UnitSkillCharacterQuest>) MasterData.UnitSkillCharacterQuestList).GroupBy<UnitSkillCharacterQuest, Tuple<int, int>>((Func<UnitSkillCharacterQuest, Tuple<int, int>>) (x => new Tuple<int, int>(x.character_quest_QuestCharacterS, x.skill_after_evolution))))
    {
      skillId = 0;
      skillIdBefore = 0;
      effectType = ResultMenuBase.EffectTYpe.NONE;
      UnitSkillCharacterQuest charaquest = source.First<UnitSkillCharacterQuest>();
      if (this.questSID == charaquest.quest_id_for_evolution)
      {
        skillId = charaquest.skill_after_evolution;
        skillIdBefore = charaquest.skill_BattleskillSkill;
        if (MasterData.BattleskillSkill[skillId].skill_type == BattleskillSkillType.leader)
        {
          effectType = ResultMenuBase.EffectTYpe.LEADER_SKILL_EVOLUTION;
          charaSkillPrefabF = Res.Prefabs.battle.LeaderSkillUpPrefab.Load<GameObject>();
        }
        else
        {
          effectType = ResultMenuBase.EffectTYpe.SKILL_EVOLUTION;
          charaSkillPrefabF = Res.Prefabs.battle.CharaSkillRankUpPrefab.Load<GameObject>();
        }
      }
      else if (charaquest.quest_id_for_evolution == 0 && this.questSID == charaquest.character_quest_QuestCharacterS)
      {
        charaSkillPrefabF = Res.Prefabs.battle.CharaSkillGetPrefab.Load<GameObject>();
        skillId = charaquest.skill_BattleskillSkill;
        effectType = ResultMenuBase.EffectTYpe.NEW_SKILL;
      }
      if (charaSkillPrefabF != null && skillId > 0)
      {
        IEnumerator e = charaSkillPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        yield return (object) new WaitForSeconds(1.1f);
        GameObject popup = this.OpenPopup(charaSkillPrefabF.Result);
        popup.SetActive(false);
        Battle02029Menu o = popup.GetComponent<Battle02029Menu>();
        switch (effectType)
        {
          case ResultMenuBase.EffectTYpe.SKILL_EVOLUTION:
            e = o.InitForEvolution(charaquest.unit_UnitUnit, skillId, skillIdBefore);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case ResultMenuBase.EffectTYpe.NEW_SKILL:
            e = o.Init(charaquest.unit_UnitUnit, skillId);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case ResultMenuBase.EffectTYpe.LEADER_SKILL_EVOLUTION:
            e = o.InitLeaderSkill(charaquest.unit_UnitUnit, skillId);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
        }
        popup.SetActive(true);
        bool isFinished = false;
        o.SetCallback((Action) (() => isFinished = true));
        while (!isFinished)
        {
          if (Singleton<NGGameDataManager>.GetInstance().questAutoLap)
            o.IbtnScreen();
          yield return (object) null;
        }
        yield return (object) new WaitForSeconds(0.6f);
        popup = (GameObject) null;
        o = (Battle02029Menu) null;
      }
      charaquest = (UnitSkillCharacterQuest) null;
    }
  }

  private bool checkForStoryOnly()
  {
    bool flag = false;
    switch (this.questType)
    {
      case CommonQuestType.Story:
        QuestStoryS questStoryS;
        if (MasterData.QuestStoryS.TryGetValue(this.questSID, out questStoryS))
        {
          flag = questStoryS.story_only;
          break;
        }
        break;
      case CommonQuestType.Character:
        QuestCharacterS questCharacterS;
        if (MasterData.QuestCharacterS.TryGetValue(this.questSID, out questCharacterS))
        {
          flag = questCharacterS.story_only;
          break;
        }
        break;
      case CommonQuestType.Extra:
        QuestExtraS questExtraS;
        if (MasterData.QuestExtraS.TryGetValue(this.questSID, out questExtraS))
        {
          flag = questExtraS.story_only;
          break;
        }
        break;
      case CommonQuestType.Sea:
        QuestSeaS questSeaS;
        if (MasterData.QuestSeaS.TryGetValue(this.questSID, out questSeaS))
        {
          flag = questSeaS.story_only;
          break;
        }
        break;
    }
    return flag;
  }

  protected void patchPUNK_DEBUG_21142(PlayerItem[] beforeGears, PlayerItem[] afterGears)
  {
    PlayerItem[] array = SMManager.Get<PlayerItem[]>();
    foreach (PlayerItem beforeGear in beforeGears)
    {
      PlayerItem gear = beforeGear;
      PlayerItem playerItem1 = gear;
      PlayerItem playerItem2 = Array.Find<PlayerItem>(array, (Predicate<PlayerItem>) (x => x.id == gear.id));
      int accessoryRemainingAmount = (object) playerItem2 != null ? playerItem2.gear_accessory_remaining_amount : 0;
      playerItem1.gear_accessory_remaining_amount = accessoryRemainingAmount;
    }
    foreach (PlayerItem afterGear in afterGears)
    {
      PlayerItem gear = afterGear;
      PlayerItem playerItem3 = gear;
      PlayerItem playerItem4 = Array.Find<PlayerItem>(array, (Predicate<PlayerItem>) (x => x.id == gear.id));
      int accessoryRemainingAmount = (object) playerItem4 != null ? playerItem4.gear_accessory_remaining_amount : 0;
      playerItem3.gear_accessory_remaining_amount = accessoryRemainingAmount;
    }
  }

  protected PlayerUnit[] createCustomDeckUnits(
    int deck_number,
    PlayerUnit[] targets,
    PlayerItem[] gears)
  {
    PlayerCustomDeck playerCustomDeck = Array.Find<PlayerCustomDeck>(SMManager.Get<PlayerCustomDeck[]>(), (Predicate<PlayerCustomDeck>) (x => x.deck_number == deck_number));
    PlayerItem[] array1 = SMManager.Get<PlayerItem[]>();
    PlayerItem[] array2 = gears != null ? ((IEnumerable<PlayerItem>) gears).ToArray<PlayerItem>() : (PlayerItem[]) null;
    PlayerUnit[] customDeckUnits = new PlayerUnit[targets.Length];
    for (int n = 0; n < customDeckUnits.Length; ++n)
    {
      PlayerCustomDeckUnit_parameter_list unitParameterList = Array.Find<PlayerCustomDeckUnit_parameter_list>(playerCustomDeck.unit_parameter_list, (Predicate<PlayerCustomDeckUnit_parameter_list>) (x => x.player_unit_id == targets[n].id));
      int gear3Id = unitParameterList.getGearId(2);
      if (array2 != null && gear3Id.IsValid() && Array.Find<PlayerItem>(array2, (Predicate<PlayerItem>) (x => x.id == gear3Id)) == (PlayerItem) null)
      {
        PlayerItem playerItem = Array.Find<PlayerItem>(array1, (Predicate<PlayerItem>) (x => x.id == gear3Id));
        if (playerItem != (PlayerItem) null)
        {
          Array.Resize<PlayerItem>(ref array2, array2.Length + 1);
          array2[array2.Length - 1] = playerItem;
        }
      }
      customDeckUnits[n] = unitParameterList.createPlayerUnitByBattleResults(targets[n], array2);
    }
    return customDeckUnits;
  }

  protected void updateLocalCustomDecks(int[] disappearedGears)
  {
    if (disappearedGears == null || disappearedGears.Length == 0)
      return;
    foreach (PlayerCustomDeck playerCustomDeck in SMManager.Get<PlayerCustomDeck[]>())
      playerCustomDeck.cleanupGears(disappearedGears);
  }

  protected void makeDiffGearAccessoryRemainingAmounts(PlayerItem[] targets)
  {
    PlayerItem[] array = SMManager.Get<PlayerItem[]>();
    this.diffGearAccessoryRemainingAmounts = new Dictionary<int, int>(targets.Length);
    for (int index = 0; index < targets.Length; ++index)
    {
      PlayerItem a = targets[index];
      PlayerItem playerItem = Array.Find<PlayerItem>(array, (Predicate<PlayerItem>) (x => x.id == a.id));
      this.diffGearAccessoryRemainingAmounts[a.id] = a.gear_accessory_remaining_amount - ((object) playerItem != null ? playerItem.gear_accessory_remaining_amount : 0);
    }
  }

  private enum EffectTYpe
  {
    NONE,
    SKILL_EVOLUTION,
    NEW_SKILL,
    LEADER_SKILL_EVOLUTION,
  }

  public class BonusReward
  {
    public int reward_quantity;
    public int reward_type_id;
    public int reward_id;

    public void SetInfo(
      WebAPI.Response.ColosseumFinishBonus_rewards reward)
    {
      this.reward_quantity = reward.reward_quantity;
      this.reward_type_id = reward.reward_type_id;
      this.reward_id = reward.reward_id;
    }

    public void SetInfo(
      WebAPI.Response.ColosseumTutorialFinishBonus_rewards reward)
    {
      this.reward_quantity = reward.reward_quantity;
      this.reward_type_id = reward.reward_type_id;
      this.reward_id = reward.reward_id;
    }

    public BonusReward(
      WebAPI.Response.ColosseumFinishBonus_rewards reward)
    {
      this.SetInfo(reward);
    }

    public BonusReward(
      WebAPI.Response.ColosseumTutorialFinishBonus_rewards reward)
    {
      this.SetInfo(reward);
    }
  }

  public class CampaignReward
  {
    public int reward_quantity;
    public string show_text2;
    public int reward_type_id;
    public int campaign_id;
    public string show_title;
    public string show_text;
    public int reward_id;

    public void SetInfo(
      WebAPI.Response.ColosseumFinishCampaign_rewards reward)
    {
      this.reward_quantity = reward.reward_quantity;
      this.show_text2 = reward.show_text2;
      this.reward_type_id = reward.reward_type_id;
      this.campaign_id = reward.campaign_id;
      this.show_title = reward.show_title;
      this.show_text = reward.show_text;
      this.reward_id = reward.reward_id;
    }

    public void SetInfo(
      WebAPI.Response.ColosseumTutorialFinishCampaign_rewards reward)
    {
      this.reward_quantity = reward.reward_quantity;
      this.show_text2 = reward.show_text2;
      this.reward_type_id = reward.reward_type_id;
      this.campaign_id = reward.campaign_id;
      this.show_title = reward.show_title;
      this.show_text = reward.show_text;
      this.reward_id = reward.reward_id;
    }

    public CampaignReward()
    {
    }

    public CampaignReward(
      WebAPI.Response.ColosseumFinishCampaign_rewards reward)
    {
      this.SetInfo(reward);
    }

    public CampaignReward(
      WebAPI.Response.ColosseumTutorialFinishCampaign_rewards reward)
    {
      this.SetInfo(reward);
    }
  }

  public class CampaignNextReward
  {
    public string next_reward_title;
    public int campaign_id;
    public string next_reward_text;

    public void SetInfo(
      WebAPI.Response.ColosseumFinishCampaign_next_rewards reward)
    {
      this.next_reward_title = reward.next_reward_title;
      this.campaign_id = reward.campaign_id;
      this.next_reward_text = reward.next_reward_text;
    }

    public void SetInfo(
      WebAPI.Response.ColosseumTutorialFinishCampaign_next_rewards reward)
    {
      this.next_reward_title = reward.next_reward_title;
      this.campaign_id = reward.campaign_id;
      this.next_reward_text = reward.next_reward_text;
    }

    public CampaignNextReward(
      WebAPI.Response.ColosseumFinishCampaign_next_rewards reward)
    {
      this.SetInfo(reward);
    }

    public CampaignNextReward(
      WebAPI.Response.ColosseumTutorialFinishCampaign_next_rewards reward)
    {
      this.SetInfo(reward);
    }
  }

  public class Param
  {
    public ResultMenuBase.BonusReward[] bonus_rewards;
    public bool is_battle;
    public int next_battle_type;
    public Bonus[] bonus;
    public Gladiator[] gladiators;
    public ColosseumRecord colosseum_record;
    public PvPRecord pvp_record;
    public Player player;
    public bool target_player_is_friend;
    public bool is_tutorial;
    public int battle_type;
    public RankUpInfo colosseum_result_rank_up;
    public ColosseumEnd colosseum_finish;
    public PlayerPresent[] player_presents;
    public ResultMenuBase.CampaignReward[] campaign_rewards;
    public ResultMenuBase.CampaignNextReward[] campaign_next_rewards;
    public ChallengeEnd challenge_finish;

    public Param(WebAPI.Response.ColosseumFinish finish)
    {
      int length1 = finish.bonus_rewards.Length;
      this.bonus_rewards = new ResultMenuBase.BonusReward[length1];
      for (int index = 0; index < length1; ++index)
      {
        if (this.bonus_rewards[index] == null)
          this.bonus_rewards[index] = new ResultMenuBase.BonusReward(finish.bonus_rewards[index]);
        else
          this.bonus_rewards[index].SetInfo(finish.bonus_rewards[index]);
      }
      int length2 = finish.campaign_rewards.Length;
      this.campaign_rewards = new ResultMenuBase.CampaignReward[length2];
      for (int index = 0; index < length2; ++index)
        this.campaign_rewards[index] = new ResultMenuBase.CampaignReward(finish.campaign_rewards[index]);
      int length3 = finish.campaign_next_rewards.Length;
      this.campaign_next_rewards = new ResultMenuBase.CampaignNextReward[length3];
      for (int index = 0; index < length3; ++index)
        this.campaign_next_rewards[index] = new ResultMenuBase.CampaignNextReward(finish.campaign_next_rewards[index]);
      this.is_battle = finish.is_battle;
      this.next_battle_type = finish.next_battle_type;
      this.bonus = finish.bonus;
      this.gladiators = finish.gladiators;
      this.colosseum_record = finish.colosseum_record;
      this.player = finish.player;
      this.target_player_is_friend = finish.target_player_is_friend;
      this.is_tutorial = finish.is_tutorial;
      this.battle_type = finish.battle_type;
      this.colosseum_result_rank_up = finish.colosseum_result_rank_up;
      this.colosseum_finish = finish.colosseum_finish;
      this.player_presents = finish.player_presents;
    }

    public Param(WebAPI.Response.ColosseumTutorialFinish finish)
    {
      int length1 = finish.bonus_rewards.Length;
      this.bonus_rewards = new ResultMenuBase.BonusReward[length1];
      for (int index = 0; index < length1; ++index)
      {
        if (this.bonus_rewards[index] == null)
          this.bonus_rewards[index] = new ResultMenuBase.BonusReward(finish.bonus_rewards[index]);
        else
          this.bonus_rewards[index].SetInfo(finish.bonus_rewards[index]);
      }
      int length2 = finish.campaign_rewards.Length;
      this.campaign_rewards = new ResultMenuBase.CampaignReward[length2];
      for (int index = 0; index < length2; ++index)
        this.campaign_rewards[index] = new ResultMenuBase.CampaignReward(finish.campaign_rewards[index]);
      int length3 = finish.campaign_next_rewards.Length;
      this.campaign_next_rewards = new ResultMenuBase.CampaignNextReward[length3];
      for (int index = 0; index < length3; ++index)
        this.campaign_next_rewards[index] = new ResultMenuBase.CampaignNextReward(finish.campaign_next_rewards[index]);
      this.is_battle = finish.is_battle;
      this.next_battle_type = finish.next_battle_type;
      this.bonus = finish.bonus;
      this.gladiators = finish.gladiators;
      this.colosseum_record = finish.colosseum_record;
      this.player = finish.player;
      this.target_player_is_friend = finish.target_player_is_friend;
      this.is_tutorial = finish.is_tutorial;
      this.battle_type = finish.battle_type;
      this.colosseum_result_rank_up = finish.colosseum_result_rank_up;
      this.colosseum_finish = finish.colosseum_finish;
      this.player_presents = finish.player_presents;
    }

    public Param(WebAPI.Response.ExploreChallengeFinish finish)
    {
      this.is_battle = finish.is_battle;
      this.player = finish.player;
      this.target_player_is_friend = finish.target_player_is_friend;
      this.player_presents = finish.player_presents;
      this.challenge_finish = finish.challenge_finish;
    }
  }
}
