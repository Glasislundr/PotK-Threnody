// Decompiled with JetBrains decompiler
// Type: ColosseumResultUnit
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
public class ColosseumResultUnit : MonoBehaviour
{
  private const float LINK_WIDTH = 120f;
  private const float LINK_DEFWIDTH = 136f;
  private const float scale = 0.882352948f;
  [SerializeField]
  private UILabel TxtUnitEXP;
  [SerializeField]
  private GameObject LinkChar;
  [SerializeField]
  private GameObject DirUnitLevelUP;
  [SerializeField]
  private GameObject UnitExpGauge;
  [SerializeField]
  private GameObject MVPIcon;
  [SerializeField]
  private GameObject MVPIconWhite;
  [SerializeField]
  private GameObject DeathIcon;
  [SerializeField]
  private GameObject slc_DearDegree_Base;
  [SerializeField]
  private UISprite slcDearDegree;
  [SerializeField]
  private UILabel TxtDearDegree;
  [SerializeField]
  private UILabel TxtDearDegreeUpAmount;
  [SerializeField]
  private UILabel TxtDearDegreeNone;
  [SerializeField]
  private UILabel TxtUnityDegree;
  [SerializeField]
  private UILabel TxtUnityDegreeUpAmount;
  [SerializeField]
  private ColosseumResultUnit.GearInfo firstGearInfo;
  [SerializeField]
  private ColosseumResultUnit.GearInfo secondGearInfo;
  [SerializeField]
  private GameObject slc_Txt_ProLV_2nd;
  [SerializeField]
  private GameObject dir_Exp_Gauge_Bugu_2nd;
  [SerializeField]
  private NGxBlinkEx dir_DearDegree;
  [SerializeField]
  private NGxBlinkEx dir_UnityDegree;
  private int GetUnitEXP;
  private List<Tuple<bool, PlayerItem, GearGearSkill>> gearUpgradeSkills;
  private GearIconWithNumber gearIcon;
  private GearIconWithNumber secondGearIcon;
  private PlayerUnit beforeUnit;
  private PlayerUnit afterUnit;
  private int levelUpCount;

  public GameObject GetUnitExpGauge => this.UnitExpGauge;

  public int GetUnitExp => this.GetUnitEXP;

  public PlayerUnit BeforeUnit => this.beforeUnit;

  public PlayerUnit AfterUnit => this.afterUnit;

  public List<Tuple<bool, PlayerItem, GearGearSkill>> GearUpgradeSkills => this.gearUpgradeSkills;

  public IEnumerator OnUnitLevelup(GameObject obj, int count)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    ColosseumResultUnit colosseumResultUnit = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    colosseumResultUnit.levelUpCount = count;
    GaugeRunner.AddSEQueue("SE_1018");
    colosseumResultUnit.DirUnitLevelUP.SetActive(true);
    colosseumResultUnit.Invoke("OutUnitLevelup", 1f);
    UnitIcon component = ((Component) colosseumResultUnit.LinkChar.transform.GetChild(0)).GetComponent<UnitIcon>();
    component.setLevelText((component.PlayerUnit.level + count + 1).ToLocalizeNumberText());
    ((IEnumerable<UITweener>) colosseumResultUnit.DirUnitLevelUP.GetComponentsInChildren<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x =>
    {
      x.ResetToBeginning();
      x.PlayForward();
    }));
    return false;
  }

  public IEnumerator OnSkipUnitLevelup(GameObject obj, int count)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    ColosseumResultUnit colosseumResultUnit = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    GaugeRunner.AddSEQueue("SE_1018");
    colosseumResultUnit.DirUnitLevelUP.SetActive(true);
    colosseumResultUnit.Invoke("OutUnitLevelup", 1f);
    ((Component) colosseumResultUnit.LinkChar.transform.GetChild(0)).GetComponent<UnitIcon>().setLevelText(colosseumResultUnit.AfterUnit.level.ToLocalizeNumberText());
    ((IEnumerable<UITweener>) colosseumResultUnit.DirUnitLevelUP.GetComponentsInChildren<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x =>
    {
      x.ResetToBeginning();
      x.PlayForward();
    }));
    return false;
  }

  private void OutUnitLevelup() => this.DirUnitLevelUP.SetActive(false);

  public void SetDeath()
  {
    if (!Object.op_Inequality((Object) this.DeathIcon, (Object) null))
      return;
    this.DeathIcon.SetActive(true);
  }

  public IEnumerator Init(PlayerUnit beforeUnit, PlayerUnit afterUnit, BattleInfo info = null)
  {
    this.beforeUnit = beforeUnit;
    this.afterUnit = afterUnit;
    if (Object.op_Inequality((Object) this.secondGearInfo.RootObj, (Object) null))
      this.secondGearInfo.RootObj.SetActive(beforeUnit.unit.awake_unit_flag);
    this.DirUnitLevelUP.SetActive(false);
    ColosseumResultUnit.GearInfo[] gearInfoArray = new ColosseumResultUnit.GearInfo[2]
    {
      this.firstGearInfo,
      this.secondGearInfo
    };
    for (int index = 0; index < gearInfoArray.Length; ++index)
    {
      if (!Object.op_Equality((Object) gearInfoArray[index].RootObj, (Object) null) && gearInfoArray[index].RootObj.activeSelf)
      {
        gearInfoArray[index].DirBuguRankUP.SetActive(false);
        gearInfoArray[index].DirProficiency.SetActive(false);
        gearInfoArray[index].DirWeaponRankUP.SetActive(false);
        gearInfoArray[index].DirWeaponBroken.SetActive(false);
        gearInfoArray[index].ProficiencyBefore.SetActive(false);
        gearInfoArray[index].ProficiencyAfter.SetActive(false);
        gearInfoArray[index].WeaponBreakIcon.SetActive(false);
      }
    }
    ((Component) this.TxtUnitEXP).gameObject.SetActive(false);
    this.SetMVP(false);
    this.GetUnitEXP = afterUnit.total_exp - beforeUnit.total_exp;
    IEnumerator e = this.LoadUnitPrefab(beforeUnit, info);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.LoadGearPrefab(beforeUnit, info);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.SetProficiency(this.firstGearInfo, beforeUnit.equippedGearOrInitial.kind_GearKind, beforeUnit, afterUnit);
    if (Object.op_Inequality((Object) this.secondGearInfo.RootObj, (Object) null) && this.secondGearInfo.RootObj.activeSelf && beforeUnit.equippedGear2 != (PlayerItem) null)
      this.SetProficiency(this.secondGearInfo, beforeUnit.equippedGear2.gear.kind_GearKind, beforeUnit, afterUnit);
    this.InitDearDegree();
    this.InitUnityDegree();
  }

  private void InitDearDegree()
  {
    if (!Object.op_Inequality((Object) this.slc_DearDegree_Base, (Object) null))
      return;
    if (!this.afterUnit.is_trust)
    {
      ((Component) this.TxtDearDegreeNone).gameObject.SetActive(true);
      this.slc_DearDegree_Base.SetActive(false);
    }
    else
    {
      this.slc_DearDegree_Base.SetActive(true);
      this.SetDearDegreeSprite(this.afterUnit.unit);
      if (Object.op_Inequality((Object) this.TxtDearDegree, (Object) null))
        this.TxtDearDegree.SetTextLocalize(string.Format("{0}%", (object) (Math.Round((double) this.afterUnit.trust_rate * 100.0) / 100.0)));
      if (!Object.op_Inequality((Object) this.TxtDearDegreeUpAmount, (Object) null))
        return;
      float num = this.afterUnit.trust_rate - this.beforeUnit.trust_rate;
      if ((double) num > 0.0)
      {
        this.TxtDearDegreeUpAmount.SetTextLocalize(string.Format("+{0}%", (object) (Math.Round((double) num * 100.0) / 100.0)));
        if (!Object.op_Inequality((Object) this.dir_DearDegree, (Object) null))
          return;
        this.dir_DearDegree.SetChildren(((Component) this.dir_DearDegree).gameObject.transform.GetChildren().Select<Transform, GameObject>((Func<Transform, GameObject>) (x => ((Component) x).gameObject)).ToArray<GameObject>());
        ((Behaviour) this.dir_DearDegree).enabled = true;
      }
      else
        ((Component) this.TxtDearDegreeUpAmount).gameObject.SetActive(false);
    }
  }

  private void InitUnityDegree()
  {
    if (!Object.op_Inequality((Object) this.dir_UnityDegree, (Object) null))
      return;
    this.TxtUnityDegree.SetTextLocalize(this.afterUnit.unityInt);
    int num = this.afterUnit.unityInt - this.beforeUnit.unityInt;
    if (num > 0)
    {
      this.TxtUnityDegreeUpAmount.SetTextLocalize(string.Format("+{0}", (object) num));
      this.dir_UnityDegree.SetChildren(((Component) this.dir_UnityDegree).transform.GetChildren().Select<Transform, GameObject>((Func<Transform, GameObject>) (x => ((Component) x).gameObject)).ToArray<GameObject>());
      ((Behaviour) this.dir_UnityDegree).enabled = true;
    }
    else
      ((Component) ((Component) this.TxtUnityDegreeUpAmount).transform.parent).gameObject.SetActive(false);
  }

  private void SetDearDegreeSprite(UnitUnit unit)
  {
    if (Object.op_Equality((Object) this.slcDearDegree, (Object) null) || Object.op_Equality((Object) this.slcDearDegree.atlas, (Object) null))
      return;
    if (((Object) this.slcDearDegree.atlas).name == "023-4-6_sozai_prefab")
    {
      if (unit.IsSea)
      {
        this.slcDearDegree.spriteName = "slc_text_Favorability.png__GUI__023-4-6_sozai__023-4-6_sozai_prefab";
      }
      else
      {
        if (!unit.IsResonanceUnit)
          return;
        this.slcDearDegree.spriteName = "slc_txt_Relevance.png__GUI__023-4-6_sozai__023-4-6_sozai_prefab";
      }
    }
    else
    {
      if (unit.IsSea)
      {
        CommonQuestType? questType = Singleton<NGGameDataManager>.GetInstance().QuestType;
        CommonQuestType commonQuestType = CommonQuestType.Sea;
        string str = questType.GetValueOrDefault() == commonQuestType & questType.HasValue ? "_sea" : string.Empty;
        this.slcDearDegree.spriteName = "slc_text_dear_degree.png__GUI__battleUI_05" + str + "__battleUI_05" + str + "_prefab";
      }
      else if (unit.IsResonanceUnit)
        this.slcDearDegree.spriteName = "slc_txt_Relevance.png__GUI__battleUI_05__battleUI_05_prefab";
      UISpriteData atlasSprite = this.slcDearDegree.GetAtlasSprite();
      ((UIWidget) this.slcDearDegree).width = atlasSprite.width;
      ((UIWidget) this.slcDearDegree).height = atlasSprite.height;
    }
  }

  public IEnumerator LoadUnitPrefab(PlayerUnit unit, BattleInfo info)
  {
    ColosseumResultUnit colosseumResultUnit = this;
    Future<GameObject> prefabF = !Singleton<NGGameDataManager>.GetInstance().IsSea || info == null || info.seaQuest == null ? Res.Prefabs.UnitIcon.normal.Load<GameObject>() : Res.Prefabs.Sea.UnitIcon.normal_sea.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObject = prefabF.Result.Clone(colosseumResultUnit.LinkChar.transform);
    gameObject.transform.localScale = new Vector3(0.882352948f, 0.882352948f);
    UnitIcon unitScript = gameObject.GetComponent<UnitIcon>();
    e = unitScript.setSimpleUnit(unit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Behaviour) unitScript.Button).enabled = false;
    ((Collider) unitScript.buttonBoxCollider).enabled = false;
    List<SwitchUnitComponentBase> list = ((IEnumerable<SwitchUnitComponentBase>) ((Component) colosseumResultUnit).GetComponentsInChildren<SwitchUnitComponentBase>(true)).ToList<SwitchUnitComponentBase>();
    for (int index = 0; index < list.Count; ++index)
      list[index].SwitchMaterial(unit.unit.ID);
    unitScript.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    unitScript.setLevelText(unit.level.ToLocalizeNumberText());
  }

  public IEnumerator LoadGearPrefab(PlayerUnit unit, BattleInfo info)
  {
    PlayerItem gear1 = unit.equippedGear;
    PlayerItem gear2 = unit.equippedGear2;
    Future<GameObject> prefabF = Res.Prefabs.ItemIcon.numberGear.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefab = prefabF.Result;
    this.gearIcon = prefab.Clone(this.firstGearInfo.LinkBugu.transform).GetComponent<GearIconWithNumber>();
    if (gear1 == (PlayerItem) null)
    {
      e = this.gearIcon.SetGear(unit.equippedGearOrInitial, true, info);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = this.gearIcon.SetGear(gear1, true, info);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.gearIcon.ItemIcon.gear.button.onClick.Clear();
    ((Behaviour) this.gearIcon.ItemIcon.gear.button).enabled = false;
    ((Collider) ((Component) this.gearIcon.ItemIcon.gear.button).GetComponent<BoxCollider>()).enabled = false;
    ((Component) this.gearIcon.numberLabel).gameObject.SetActive(false);
    if (Object.op_Inequality((Object) this.secondGearInfo.RootObj, (Object) null) && this.secondGearInfo.RootObj.activeSelf)
    {
      this.secondGearIcon = prefab.Clone(this.secondGearInfo.LinkBugu.transform).GetComponent<GearIconWithNumber>();
      if (gear2 != (PlayerItem) null)
      {
        e = this.secondGearIcon.SetGear(gear2, true, info);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.secondGearIcon.ItemIcon.gear.button.onClick.Clear();
        ((Behaviour) this.secondGearIcon.ItemIcon.gear.button).enabled = false;
        ((Collider) ((Component) this.secondGearIcon.ItemIcon.gear.button).GetComponent<BoxCollider>()).enabled = false;
        ((Component) this.secondGearIcon.numberLabel).gameObject.SetActive(false);
      }
      else
      {
        this.secondGearIcon.numberLabel.SetTextLocalize(string.Empty);
        CreateIconObject icon = this.secondGearIcon.gearIconParent.AddComponent<CreateIconObject>();
        e = icon.CreateThumbnail(MasterDataTable.CommonRewardType.gear, 0, visibleBottom: false, isButton: false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        icon.GetIcon().GetComponent<ItemIcon>().gear.favorite.SetActive(false);
        this.slc_Txt_ProLV_2nd.SetActive(false);
        this.dir_Exp_Gauge_Bugu_2nd.SetActive(false);
        icon = (CreateIconObject) null;
      }
    }
  }

  private void SetProficiency(
    ColosseumResultUnit.GearInfo gearInfo,
    int gearKindId,
    PlayerUnit before,
    PlayerUnit after)
  {
    if (after.unit.IsAllEquipUnit)
    {
      gearInfo.DirProficiency.SetActive(true);
      gearInfo.ProficiencyBefore.SetActive(true);
      ((Component) gearInfo.ProficiencyBefore.transform.Find(this.toProficiencyObjectName(true, 0))).gameObject.SetActive(true);
    }
    else
    {
      PlayerUnitGearProficiency unitGearProficiency1 = ((IEnumerable<PlayerUnitGearProficiency>) before.gear_proficiencies).Where<PlayerUnitGearProficiency>((Func<PlayerUnitGearProficiency, bool>) (x => x.gear_kind_id == gearKindId)).ToArray<PlayerUnitGearProficiency>()[0];
      PlayerUnitGearProficiency unitGearProficiency2 = ((IEnumerable<PlayerUnitGearProficiency>) after.gear_proficiencies).Where<PlayerUnitGearProficiency>((Func<PlayerUnitGearProficiency, bool>) (x => x.gear_kind_id == gearKindId)).ToArray<PlayerUnitGearProficiency>()[0];
      gearInfo.IsHideProficiency = MasterData.GearKind[gearKindId].isHideProficiency;
      gearInfo.GearBeforeLv = unitGearProficiency1.level;
      gearInfo.GearAfterLv = unitGearProficiency2.level;
      ((Component) gearInfo.ProficiencyBefore.transform.Find(this.toProficiencyObjectName(gearInfo.IsHideProficiency, unitGearProficiency1.level))).gameObject.SetActive(true);
      gearInfo.DirProficiency.SetActive(true);
      gearInfo.ProficiencyBefore.SetActive(true);
      ((IEnumerable<UITweener>) gearInfo.ProficiencyBefore.GetComponentsInChildren<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x =>
      {
        if (x.tweenGroup != 1)
          return;
        x.ResetToBeginning();
        x.PlayForward();
      }));
    }
  }

  private string toProficiencyObjectName(bool bHide, int level)
  {
    string str = "dyn_IconRank_";
    return !bHide ? str + MasterData.UnitProficiency[level].proficiency : str;
  }

  public bool IsNotLastPlayGaugeRunner()
  {
    if (this.LinkChar.transform.childCount == 0 || this.AfterUnit == (PlayerUnit) null)
      return false;
    UnitIcon component = ((Component) this.LinkChar.transform.GetChild(0)).GetComponent<UnitIcon>();
    int level = component.PlayerUnit.level;
    return Object.op_Inequality((Object) component, (Object) null) && level + this.levelUpCount < this.AfterUnit.level - 1;
  }

  public GaugeRunner GetUnitExpLastGaugeRunner()
  {
    return new GaugeRunner(this.UnitExpGauge, (float) this.afterUnit.exp / (float) (this.afterUnit.exp + this.afterUnit.exp_next), 1, new Func<GameObject, int, IEnumerator>(this.OnSkipUnitLevelup));
  }

  public GaugeRunner GetUnitExpGaugeRunner(Func<GameObject, int, IEnumerator> levelupCallback)
  {
    float before = (float) this.beforeUnit.exp / (float) (this.beforeUnit.exp + this.beforeUnit.exp_next);
    float after = (float) this.afterUnit.exp / (float) (this.afterUnit.exp + this.afterUnit.exp_next);
    int loopNum = this.afterUnit.level - this.beforeUnit.level;
    float duration = 0.6f;
    Func<GameObject, int, IEnumerator> levelupCallback1 = new Func<GameObject, int, IEnumerator>(this.OnUnitLevelup);
    if (levelupCallback != null)
      levelupCallback1 = levelupCallback;
    return new GaugeRunner(this.UnitExpGauge, before, after, loopNum, levelupCallback1, duration: duration);
  }

  public List<GaugeRunner> GetGearExpGaugeRunner(
    Dictionary<int, PlayerItem> beforePlayerGears,
    Dictionary<int, PlayerItem> afterPlayerGears,
    Func<GameObject, int, IEnumerator> levelupCallback)
  {
    List<GaugeRunner> gearExpGaugeRunner = new List<GaugeRunner>();
    ColosseumResultUnit.GearInfo[] gearInfoArray = new ColosseumResultUnit.GearInfo[2]
    {
      this.firstGearInfo,
      this.secondGearInfo
    };
    int?[] equippedGearIds = new int?[2]
    {
      this.afterUnit.equippedGear?.id,
      this.afterUnit.equippedGear2?.id
    };
    PlayerItem[] source = SMManager.Get<PlayerItem[]>();
    for (int i = 0; i < equippedGearIds.Length; i++)
    {
      if (!equippedGearIds[i].HasValue || Object.op_Equality((Object) gearInfoArray[i].RootObj, (Object) null))
      {
        gearExpGaugeRunner.Add((GaugeRunner) null);
      }
      else
      {
        PlayerItem playerItem = after = (PlayerItem) null;
        if (!((IEnumerable<PlayerItem>) source).Any<PlayerItem>((Func<PlayerItem, bool>) (x =>
        {
          int id = x.id;
          int? nullable = equippedGearIds[i];
          int valueOrDefault = nullable.GetValueOrDefault();
          return id == valueOrDefault & nullable.HasValue;
        })))
        {
          Debug.LogError((object) ("ColosseumBattleResult: 装備武具が見つからない (id: " + (object) equippedGearIds[i].Value + ")"));
          gearExpGaugeRunner.Add((GaugeRunner) null);
        }
        else if (!beforePlayerGears.TryGetValue(equippedGearIds[i].Value, out playerItem))
          gearExpGaugeRunner.Add((GaugeRunner) null);
        else if (!afterPlayerGears.TryGetValue(equippedGearIds[i].Value, out after))
        {
          gearExpGaugeRunner.Add((GaugeRunner) null);
        }
        else
        {
          if (after.gear_level > playerItem.gear_level)
            gearInfoArray[i].IsGearRankUp = true;
          if (after.broken)
          {
            gearInfoArray[i].IsGearBreak = true;
            if (playerItem.broken)
            {
              gearInfoArray[i].IsGearBreakStopAnim = true;
              this.ShowWeaponBreak(gearInfoArray[i]);
              this.WeaponBreakStopAnimation(gearInfoArray[i]);
            }
          }
          if (gearInfoArray[i].IsGearRankUp && after.skills.Length != 0)
          {
            if (this.gearUpgradeSkills == null)
              this.gearUpgradeSkills = new List<Tuple<bool, PlayerItem, GearGearSkill>>();
            for (int j = 0; j < after.skills.Length; j++)
            {
              GearGearSkill gearGearSkill = ((IEnumerable<GearGearSkill>) playerItem.skills).FirstOrDefault<GearGearSkill>((Func<GearGearSkill, bool>) (x => x.skill_group == after.skills[j].skill_group));
              if (gearGearSkill == null)
                this.gearUpgradeSkills.Add(new Tuple<bool, PlayerItem, GearGearSkill>(true, after, after.skills[j]));
              else if (gearGearSkill.ID != after.skills[j].ID)
                this.gearUpgradeSkills.Add(new Tuple<bool, PlayerItem, GearGearSkill>(false, after, after.skills[j]));
            }
          }
          gearExpGaugeRunner.Add(new GaugeRunner(gearInfoArray[i].WeaponExpGauge, (float) playerItem.gear_exp / (float) (playerItem.gear_exp + playerItem.gear_exp_next), (float) after.gear_exp / (float) (after.gear_exp + after.gear_exp_next), after.gear_level - playerItem.gear_level, levelupCallback));
        }
      }
    }
    return gearExpGaugeRunner;
  }

  public void OpenAfterProficiency()
  {
    ColosseumResultUnit.GearInfo[] gearInfoArray = new ColosseumResultUnit.GearInfo[2]
    {
      this.firstGearInfo,
      this.secondGearInfo
    };
    for (int index = 0; index < gearInfoArray.Length; ++index)
    {
      if (!Object.op_Equality((Object) gearInfoArray[index].RootObj, (Object) null) && gearInfoArray[index].RootObj.activeSelf && gearInfoArray[index].GearBeforeLv != gearInfoArray[index].GearAfterLv)
      {
        ((Component) gearInfoArray[index].ProficiencyAfter.transform.Find(this.toProficiencyObjectName(gearInfoArray[index].IsHideProficiency, gearInfoArray[index].GearAfterLv))).gameObject.SetActive(true);
        gearInfoArray[index].ProficiencyAfter.SetActive(true);
        gearInfoArray[index].DirBuguRankUP.SetActive(true);
        this.Invoke("OutProficiencyRankUp", 1f);
        ((IEnumerable<UITweener>) gearInfoArray[index].DirBuguRankUP.GetComponentsInChildren<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x =>
        {
          x.ResetToBeginning();
          x.PlayForward();
        }));
        ((IEnumerable<UITweener>) gearInfoArray[index].ProficiencyAfter.GetComponentsInChildren<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x =>
        {
          x.ResetToBeginning();
          x.PlayForward();
        }));
        ((IEnumerable<UITweener>) gearInfoArray[index].ProficiencyBefore.GetComponentsInChildren<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x =>
        {
          if (x.tweenGroup != 0)
            return;
          x.ResetToBeginning();
          x.PlayForward();
        }));
      }
    }
  }

  public void SetMVP(bool active)
  {
    this.MVPIcon.SetActive(active);
    if (!Object.op_Inequality((Object) this.MVPIconWhite, (Object) null))
      return;
    this.MVPIconWhite.SetActive(active);
  }

  public void ShowUnitExp()
  {
    ((Component) this.TxtUnitEXP).gameObject.SetActive(false);
    if (this.GetUnitEXP <= 0)
      return;
    ((Component) this.TxtUnitEXP).gameObject.SetActive(true);
    this.TxtUnitEXP.SetTextLocalize("ＥＸＰ＋" + this.GetUnitEXP.ToString());
    ((IEnumerable<UITweener>) ((Component) this.TxtUnitEXP).GetComponentsInChildren<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x =>
    {
      x.ResetToBeginning();
      x.PlayForward();
    }));
  }

  public void ShowWeaponRankUp()
  {
    if (this.firstGearInfo.IsGearRankUp)
    {
      this.firstGearInfo.DirWeaponRankUP.SetActive(true);
      this.Invoke("OutWeaponRankUp", 1f);
    }
    if (Object.op_Equality((Object) this.secondGearInfo.RootObj, (Object) null) || !this.secondGearInfo.RootObj.activeSelf || !this.secondGearInfo.IsGearRankUp)
      return;
    this.secondGearInfo.DirWeaponRankUP.SetActive(true);
    this.Invoke("OutWeaponRankUp2", 1f);
  }

  private void OutWeaponRankUp() => this.firstGearInfo.DirWeaponRankUP.SetActive(false);

  private void OutWeaponRankUp2()
  {
    if (Object.op_Equality((Object) this.secondGearInfo.RootObj, (Object) null))
      return;
    this.secondGearInfo.DirWeaponRankUP.SetActive(false);
  }

  public bool ShowWeaponBreak()
  {
    if (this.firstGearInfo.IsGearBreak)
      this.firstGearInfo.DirWeaponBroken.SetActive(true);
    if (Object.op_Equality((Object) this.secondGearInfo.RootObj, (Object) null) || !this.secondGearInfo.RootObj.activeSelf)
      return this.firstGearInfo.IsGearBreak;
    if (this.secondGearInfo.IsGearBreak)
      this.secondGearInfo.DirWeaponBroken.SetActive(true);
    return this.firstGearInfo.IsGearBreak | this.secondGearInfo.IsGearBreak;
  }

  private bool ShowWeaponBreak(ColosseumResultUnit.GearInfo gearInfo)
  {
    if (gearInfo.IsGearBreak)
      gearInfo.DirWeaponBroken.SetActive(true);
    return gearInfo.IsGearBreak;
  }

  public bool IsWeaponBreakStopAnim()
  {
    return Object.op_Equality((Object) this.secondGearInfo.RootObj, (Object) null) || !this.secondGearInfo.RootObj.activeSelf ? this.firstGearInfo.IsGearBreakStopAnim : this.firstGearInfo.IsGearBreakStopAnim | this.secondGearInfo.IsGearBreakStopAnim;
  }

  private void WeaponBreakStopAnimation(ColosseumResultUnit.GearInfo gearInfo)
  {
    ((IEnumerable<UITweener>) gearInfo.DirWeaponBroken.GetComponentsInChildren<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x =>
    {
      x.delay = 0.0f;
      x.duration = 0.0f;
    }));
  }

  public bool ShowGearUpgradeSkill()
  {
    return this.gearUpgradeSkills != null && this.gearUpgradeSkills.Count > 0;
  }

  public bool IsLevelUP() => this.afterUnit.level > this.beforeUnit.level;

  public bool IsProficiencyLevelUp()
  {
    if (this.beforeUnit.unit.IsAllEquipUnit)
      return false;
    if (!Object.op_Equality((Object) this.secondGearInfo.RootObj, (Object) null) && this.secondGearInfo.RootObj.activeSelf)
      return ((this.firstGearInfo.IsHideProficiency ? 0 : (this.firstGearInfo.GearBeforeLv < this.firstGearInfo.GearAfterLv ? 1 : 0)) | (this.secondGearInfo.IsHideProficiency ? 0 : (this.secondGearInfo.GearBeforeLv < this.secondGearInfo.GearAfterLv ? 1 : 0))) != 0;
    return !this.firstGearInfo.IsHideProficiency && this.firstGearInfo.GearBeforeLv < this.firstGearInfo.GearAfterLv;
  }

  [Serializable]
  private class GearInfo
  {
    public GameObject RootObj;
    public GameObject LinkBugu;
    public GameObject DirBuguRankUP;
    public GameObject DirProficiency;
    public GameObject DirWeaponRankUP;
    public GameObject DirWeaponBroken;
    public GameObject ProficiencyBefore;
    public GameObject ProficiencyAfter;
    public GameObject DirWeaponExpGauge;
    public GameObject WeaponExpGauge;
    public GameObject WeaponBreakIcon;

    public bool IsGearBreak { get; set; }

    public bool IsGearBreakStopAnim { get; set; }

    public bool IsGearRankUp { get; set; }

    public bool IsHideProficiency { get; set; }

    public int GearBeforeLv { get; set; }

    public int GearAfterLv { get; set; }
  }
}
