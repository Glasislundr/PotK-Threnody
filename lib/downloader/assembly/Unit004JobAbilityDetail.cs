// Decompiled with JetBrains decompiler
// Type: Unit004JobAbilityDetail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Text;
using UnitStatusInformation;
using UnityEngine;

#nullable disable
public class Unit004JobAbilityDetail : BackButtonMenuBase
{
  [SerializeField]
  private UILabel mAbilityNameLbl;
  [SerializeField]
  private UILabel[] mSubAbilityNameLbls;
  [SerializeField]
  private UILabel mLevelLbl;
  [SerializeField]
  private GameObject[] mCategoryAnchor;
  [SerializeField]
  private UILabel mDescriptionNonMaxLbl;
  [SerializeField]
  private UILabel mDescriptionLvMaxLbl;
  [SerializeField]
  private UILabel mMasterBonusEffectsLbl;
  [SerializeField]
  private GameObject mXLvUpObj;
  [SerializeField]
  private UILabel mXLvUpEffectLbl;
  [SerializeField]
  private UIButton mLvUpButton;
  private PlayerUnit mUnit;
  private PlayerUnitJob_abilities mJobAbility;
  private Action<Action> mOnPreUpdateJobAbility;
  private Action mOnUpdatedJobAbility;
  private bool mIsClassChangeScene;
  private JobCharacteristics mJobChar;
  private BattleskillSkill mSkill1;
  private BattleskillSkill mSkill2;
  private GameObject mLvUpJobAbilityPrefab;
  private GameObject mSkillDetailPrefab;

  public IEnumerator Init(
    PlayerUnit unit,
    PlayerUnitJob_abilities jobAbility,
    bool enableLvUp,
    Action eventUpdatedJobAbility,
    bool isClassChangeScene = false,
    Action<Action> eventPreUpdateJobAbility = null)
  {
    this.mUnit = unit;
    this.mJobAbility = jobAbility;
    this.mJobChar = this.mJobAbility.master;
    this.mSkill1 = this.mJobChar.skill;
    this.mSkill2 = this.mJobChar.skill2;
    this.mOnPreUpdateJobAbility = eventPreUpdateJobAbility;
    this.mOnUpdatedJobAbility = eventUpdatedJobAbility;
    this.mIsClassChangeScene = isClassChangeScene;
    JobCharacteristicsLevelupPattern currentLevelupPattern = jobAbility.current_levelup_pattern;
    Future<GameObject> loader = new ResourceObject("Prefabs/unit004_Job/" + (currentLevelupPattern == null || !currentLevelupPattern.proficiency.HasValue ? "Unit_JobCharacteristic_UP_Dialog" : "Unit_X_JobCharacteristic_UP_Dialog")).Load<GameObject>();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mLvUpJobAbilityPrefab = loader.Result;
    loader = PopupSkillDetails.createPrefabLoader(Singleton<NGGameDataManager>.GetInstance().IsSea);
    yield return (object) loader.Wait();
    this.mSkillDetailPrefab = loader.Result;
    e = this.SetupInfomation();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((UIButtonColor) this.mLvUpButton).isEnabled = enableLvUp;
  }

  private IEnumerator SetupInfomation()
  {
    Future<GameObject> prefabF = Res.Icons.SkillGenreIcon.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    this.mAbilityNameLbl.SetTextLocalize(this.mSkill1.name);
    foreach (UILabel subAbilityNameLbl in this.mSubAbilityNameLbls)
      subAbilityNameLbl.SetTextLocalize(this.mSkill2.name);
    this.mLevelLbl.SetTextLocalize(this.mJobAbility.level);
    GameObject gameObject1 = result.Clone(this.mCategoryAnchor[0].transform);
    gameObject1.GetComponent<SkillGenreIcon>().Init(this.mSkill1.genre1);
    ((UIWidget) gameObject1.GetComponent<UI2DSprite>()).depth = 9;
    GameObject gameObject2 = result.Clone(this.mCategoryAnchor[1].transform);
    gameObject2.GetComponent<SkillGenreIcon>().Init(this.mSkill1.genre2);
    ((UIWidget) gameObject2.GetComponent<UI2DSprite>()).depth = 9;
    this.mDescriptionNonMaxLbl.SetTextLocalize(this.mJobChar.normal_description);
    this.mDescriptionLvMaxLbl.SetTextLocalize(this.mJobChar.lvmax_description);
    this.mMasterBonusEffectsLbl.SetTextLocalize(string.Format("{0} {1} {2}", (object) this.GenBonusText(this.mJobChar.levelmax_bonus, this.mJobChar.levelmax_bonus_value), (object) this.GenBonusText(this.mJobChar.levelmax_bonus2, this.mJobChar.levelmax_bonus_value2), (object) this.GenBonusText(this.mJobChar.levelmax_bonus3, this.mJobChar.levelmax_bonus_value3)));
    if (this.mJobChar.xlevel_limits != null)
    {
      this.mXLvUpEffectLbl.SetTextLocalize(string.Format("Lv上限+{0}", (object) this.mJobChar.xlevel_limits.getLimit(this.mJobAbility.level)));
      this.mXLvUpObj.SetActive(true);
    }
    else
      this.mXLvUpObj.SetActive(false);
  }

  private string GenBonusText(JobCharacteristicsLevelmaxBonus bonus, int value)
  {
    if (value < 1)
      return string.Empty;
    StringBuilder stringBuilder = new StringBuilder();
    switch (bonus)
    {
      case JobCharacteristicsLevelmaxBonus.hp_add:
        stringBuilder.Append("HP");
        break;
      case JobCharacteristicsLevelmaxBonus.strength_add:
        stringBuilder.Append("力");
        break;
      case JobCharacteristicsLevelmaxBonus.intelligence_add:
        stringBuilder.Append("魔");
        break;
      case JobCharacteristicsLevelmaxBonus.vitality_add:
        stringBuilder.Append("守");
        break;
      case JobCharacteristicsLevelmaxBonus.mind_add:
        stringBuilder.Append("精");
        break;
      case JobCharacteristicsLevelmaxBonus.agility_add:
        stringBuilder.Append("速");
        break;
      case JobCharacteristicsLevelmaxBonus.dexterity_add:
        stringBuilder.Append("技");
        break;
      case JobCharacteristicsLevelmaxBonus.lucky_add:
        stringBuilder.Append("運");
        break;
      case JobCharacteristicsLevelmaxBonus.movement_add:
        stringBuilder.Append("移動");
        break;
    }
    stringBuilder.Append(string.Format("+{0}", (object) value));
    return stringBuilder.ToString();
  }

  private IEnumerator OpenJobDialogUp()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit004JobAbilityDetail jobAbilityDetail = this;
    GameObject go;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      Singleton<PopupManager>.GetInstance().startOpenAnime(go);
      jobAbilityDetail.StartCoroutine(jobAbilityDetail.IsPushOff());
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    Singleton<PopupManager>.GetInstance().dismiss();
    go = Singleton<PopupManager>.GetInstance().open(jobAbilityDetail.mLvUpJobAbilityPrefab, isNonSe: true, isNonOpenAnime: true);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) go.GetComponent<Unit004JobDialogUp>().Init(jobAbilityDetail.mUnit, jobAbilityDetail.mJobAbility, jobAbilityDetail.mOnUpdatedJobAbility, jobAbilityDetail.mIsClassChangeScene, jobAbilityDetail.mOnPreUpdateJobAbility);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void onSkillDetailButton()
  {
    if (this.IsPushAndSet())
      return;
    PopupSkillDetails.show(this.mSkillDetailPrefab, new PopupSkillDetails.Param(this.mSkill1, UnitParameter.SkillGroup.JobAbility, new int?(this.mJobAbility.level)));
    this.StartCoroutine(this.IsPushOff());
  }

  public void onLevelupButton()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.OpenJobDialogUp());
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }
}
