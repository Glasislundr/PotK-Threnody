// Decompiled with JetBrains decompiler
// Type: PopupRecommendPartEquipments
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Popup/Recommend/Equipments")]
public class PopupRecommendPartEquipments : PopupRecommendPart
{
  [SerializeField]
  private UILabel txtTitle_;
  [SerializeField]
  private UILabel txtName_;
  [SerializeField]
  private int depth_;
  [SerializeField]
  private Transform lnkGear_;
  [SerializeField]
  private Transform[] lnkSkills_;
  [SerializeField]
  private GameObject notPossessed_;
  [SerializeField]
  [Tooltip("0:\"装備\"/1:\"入手\"")]
  private GameObject[] backGrounds_;
  private GearGear targetGear_;
  private PlayerItem playerGear_;
  private PopupSkillDetails.Param[] skills_;
  private bool isExistQuests_;
  private GameObject prefabQuest_;
  private const int ENABLED_RARITY = 5;

  public override IEnumerator doInitialize(PlayerUnit playerUnit, UnitUnit target)
  {
    PopupRecommendPartEquipments recommendPartEquipments = this;
    recommendPartEquipments.playerUnit_ = playerUnit;
    recommendPartEquipments.target_ = target;
    recommendPartEquipments.playerGear_ = (PlayerItem) null;
    recommendPartEquipments.targetGear_ = (GearGear) null;
    recommendPartEquipments.isExistQuests_ = false;
    UnitRecommend unitRecommend;
    if (MasterData.UnitRecommend.TryGetValue(playerUnit.unit.same_character_id, out unitRecommend))
    {
      recommendPartEquipments.targetGear_ = unitRecommend.gear;
      recommendPartEquipments.isExistQuests_ = unitRecommend.story_quests != null && unitRecommend.story_quests.Length != 0 || unitRecommend.sea_quests != null && unitRecommend.sea_quests.Length != 0 || unitRecommend.extra_quests != null && unitRecommend.extra_quests.Length != 0;
    }
    bool flag1 = recommendPartEquipments.targetGear_ != null;
    if (!flag1 && target.rarity.index < 5)
    {
      ((Component) recommendPartEquipments).gameObject.SetActive(false);
    }
    else
    {
      if (recommendPartEquipments.targetGear_ == null)
        recommendPartEquipments.targetGear_ = recommendPartEquipments.getSpecialGear();
      if (recommendPartEquipments.targetGear_ == null)
      {
        ((Component) recommendPartEquipments).gameObject.SetActive(false);
      }
      else
      {
        recommendPartEquipments.txtTitle_.SetTextLocalize(flag1 ? Consts.GetInstance().POPUPUNITRECOMMEND_TITLE_EQUIPMENTS : Consts.GetInstance().POPUPUNITRECOMMEND_TITLE_SPECIALEQUIP);
        recommendPartEquipments.txtName_.SetTextLocalize(recommendPartEquipments.targetGear_.name);
        int bgIndex;
        bool flag2;
        if (recommendPartEquipments.menu.isDisabledAccountStatus)
        {
          recommendPartEquipments.playerGear_ = (PlayerItem) null;
          recommendPartEquipments.notPossessed_.SetActive(false);
          // ISSUE: reference to a compiler-generated method
          flag2 = Array.Find<PlayerItem>(SMManager.Get<PlayerItem[]>(), new Predicate<PlayerItem>(recommendPartEquipments.\u003CdoInitialize\u003Eb__14_0)) != (PlayerItem) null;
          bgIndex = -1;
        }
        else
        {
          PlayerItem equippedGear = recommendPartEquipments.playerUnit_.equippedGear;
          if (equippedGear != (PlayerItem) null && equippedGear.gear == recommendPartEquipments.targetGear_)
          {
            recommendPartEquipments.playerGear_ = equippedGear;
          }
          else
          {
            PlayerItem equippedGear3;
            if ((equippedGear3 = recommendPartEquipments.playerUnit_.equippedGear3) != (PlayerItem) null && equippedGear3.gear == recommendPartEquipments.targetGear_)
            {
              recommendPartEquipments.playerGear_ = equippedGear3;
            }
            else
            {
              PlayerItem equippedGear2;
              if ((equippedGear2 = recommendPartEquipments.playerUnit_.equippedGear2) != (PlayerItem) null && equippedGear2.gear == recommendPartEquipments.targetGear_)
                recommendPartEquipments.playerGear_ = equippedGear2;
            }
          }
          if (recommendPartEquipments.playerGear_ == (PlayerItem) null)
          {
            // ISSUE: reference to a compiler-generated method
            recommendPartEquipments.playerGear_ = Array.Find<PlayerItem>(SMManager.Get<PlayerItem[]>(), new Predicate<PlayerItem>(recommendPartEquipments.\u003CdoInitialize\u003Eb__14_2));
          }
          recommendPartEquipments.notPossessed_.SetActive(recommendPartEquipments.playerGear_ == (PlayerItem) null);
          flag2 = recommendPartEquipments.playerGear_ != (PlayerItem) null;
          bgIndex = flag2 ? 0 : 1;
        }
        ((IEnumerable<GameObject>) recommendPartEquipments.backGrounds_).ToggleOnce(bgIndex);
        if (!flag2)
          yield return (object) OnDemandDownload.waitLoadSomethingResource((IEnumerable<string>) recommendPartEquipments.targetGear_.ResourcePaths(), false);
        Future<GameObject> ldIcon = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
        yield return (object) ldIcon.Wait();
        ItemIcon icon1 = ldIcon.Result.Clone(recommendPartEquipments.lnkGear_).GetComponent<ItemIcon>();
        yield return (object) icon1.InitByGear(recommendPartEquipments.targetGear_);
        icon1.isButtonActive = false;
        if (recommendPartEquipments.playerGear_ != (PlayerItem) null)
        {
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          EventDelegate.Set(recommendPartEquipments.backGrounds_[bgIndex].GetComponent<UIButton>().onClick, recommendPartEquipments.canEquipped ? new EventDelegate.Callback(recommendPartEquipments.\u003CdoInitialize\u003Eb__14_3) : new EventDelegate.Callback(recommendPartEquipments.\u003CdoInitialize\u003Eb__14_4));
        }
        else if (recommendPartEquipments.menu.onChangedQuest != null)
        {
          // ISSUE: reference to a compiler-generated method
          EventDelegate.Set(recommendPartEquipments.backGrounds_[bgIndex].GetComponent<UIButton>().onClick, new EventDelegate.Callback(recommendPartEquipments.\u003CdoInitialize\u003Eb__14_5));
        }
        icon1 = (ItemIcon) null;
        List<PopupSkillDetails.Param> skillParams = new List<PopupSkillDetails.Param>(recommendPartEquipments.lnkSkills_.Length);
        List<List<GearGearSkill>> lstSkills = recommendPartEquipments.targetGear_.rememberSkills;
        if (lstSkills.Count > 0)
        {
          Future<GameObject> ldSkill = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
          yield return (object) ldSkill.Wait();
          int maxSet = Mathf.Min(lstSkills.Count, recommendPartEquipments.lnkSkills_.Length);
          int index;
          for (index = 0; index < maxSet; ++index)
          {
            GearGearSkill ggs = lstSkills[index][0];
            BattleSkillIcon icon2 = ldSkill.Result.Clone(recommendPartEquipments.lnkSkills_[index]).GetComponent<BattleSkillIcon>();
            yield return (object) icon2.Init(ggs.skill);
            icon2.SetDepth(recommendPartEquipments.depth_);
            skillParams.Add(new PopupSkillDetails.Param(ggs));
            ggs = (GearGearSkill) null;
            icon2 = (BattleSkillIcon) null;
          }
          for (; index < recommendPartEquipments.lnkSkills_.Length; ++index)
            ((Component) recommendPartEquipments.lnkSkills_[index]).gameObject.SetActive(false);
          recommendPartEquipments.skills_ = skillParams.ToArray();
          ldSkill = (Future<GameObject>) null;
        }
        else
          ((IEnumerable<Transform>) recommendPartEquipments.lnkSkills_).Select<Transform, GameObject>((Func<Transform, GameObject>) (x => ((Component) x).gameObject)).SetActives(false);
        Future<GameObject> ldQuest = PopupGearQuestList.createLoader();
        IEnumerator e = ldQuest.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        recommendPartEquipments.prefabQuest_ = ldQuest.Result;
        if (recommendPartEquipments.menu.isAutoBootQuestList)
          recommendPartEquipments.StartCoroutine("doWaitAutoBootQuestList");
      }
    }
  }

  private IEnumerator doWaitAutoBootQuestList()
  {
    PopupRecommendPartEquipments recommendPartEquipments = this;
    while (!recommendPartEquipments.menu.isInitialized)
      yield return (object) null;
    recommendPartEquipments.onPopupQuestList();
  }

  private bool canEquipped
  {
    get
    {
      if (this.playerUnit_ == (PlayerUnit) null || this.playerGear_ == (PlayerItem) null)
        return true;
      MasterDataTable.UnitJob jobData = this.playerUnit_.getJobData();
      if (jobData.classification_GearClassificationPattern.HasValue)
      {
        int? classificationPattern1 = jobData.classification_GearClassificationPattern;
        int num = 0;
        if (!(classificationPattern1.GetValueOrDefault() == num & classificationPattern1.HasValue))
        {
          classificationPattern1 = jobData.classification_GearClassificationPattern;
          int? classificationPattern2 = this.targetGear_.classification_GearClassificationPattern;
          return classificationPattern1.GetValueOrDefault() == classificationPattern2.GetValueOrDefault() & classificationPattern1.HasValue == classificationPattern2.HasValue;
        }
      }
      return true;
    }
  }

  public void onClickedSkill_1() => this.onClickedSkill(0);

  public void onClickedSkill_2() => this.onClickedSkill(1);

  private void onClickedSkill(int index)
  {
    if (Object.op_Equality((Object) this.menu.skillDetailPrefab, (Object) null) || this.skills_ == null || this.skills_.Length <= index)
      return;
    PopupSkillDetails.show(this.menu.skillDetailPrefab, this.skills_, index);
  }

  private void onPopupQuestList()
  {
    PopupGearQuestList.open(this.prefabQuest_, this.playerUnit_, this.menu.onChangedQuest, (Action<PopupUtility.SceneTo>) null, this.menu.isDisabledChangeQuest);
  }

  private GearGear getSpecialGear()
  {
    int[] includeGears = new int[1]
    {
      this.target_.kind_GearKind
    };
    GearSpecificationOfEquipmentUnit[] gearSpecials = ((IEnumerable<GearSpecificationOfEquipmentUnit>) MasterData.GearSpecificationOfEquipmentUnitList).Where<GearSpecificationOfEquipmentUnit>((Func<GearSpecificationOfEquipmentUnit, bool>) (x => x.unit_id == this.target_.ID)).ToArray<GearSpecificationOfEquipmentUnit>();
    HashSet<int> groups = new HashSet<int>(((IEnumerable<GearSpecificationOfEquipmentUnit>) gearSpecials).Select<GearSpecificationOfEquipmentUnit, int>((Func<GearSpecificationOfEquipmentUnit, int>) (x => x.group_id)).Distinct<int>());
    Dictionary<int, HashSet<int>> source = new Dictionary<int, HashSet<int>>();
    foreach (GearSpecificationOfEquipmentUnit specificationOfEquipmentUnit in ((IEnumerable<GearSpecificationOfEquipmentUnit>) MasterData.GearSpecificationOfEquipmentUnitList).Where<GearSpecificationOfEquipmentUnit>((Func<GearSpecificationOfEquipmentUnit, bool>) (y => groups.Contains(y.group_id) && y.unit_id != 0)))
    {
      UnitUnit unitUnit;
      if (MasterData.UnitUnit.TryGetValue(specificationOfEquipmentUnit.unit_id, out unitUnit) && ((IEnumerable<int>) includeGears).Contains<int>(unitUnit.kind_GearKind))
      {
        if (!source.ContainsKey(specificationOfEquipmentUnit.group_id))
          source[specificationOfEquipmentUnit.group_id] = new HashSet<int>();
        source[specificationOfEquipmentUnit.group_id].Add(unitUnit.same_character_id);
      }
    }
    Dictionary<int, int> dicGroupPoints = source.ToDictionary<KeyValuePair<int, HashSet<int>>, int, int>((Func<KeyValuePair<int, HashSet<int>>, int>) (k => k.Key), (Func<KeyValuePair<int, HashSet<int>>, int>) (v => v.Value.Count));
    return !dicGroupPoints.Any<KeyValuePair<int, int>>() ? (GearGear) null : ((IEnumerable<GearGear>) MasterData.GearGearList).Where<GearGear>((Func<GearGear, bool>) (x => x.hasSpecificationOfEquipmentUnits && ((IEnumerable<int>) includeGears).Contains<int>(x.kind_GearKind) && dicGroupPoints.ContainsKey(x.specification_of_equipment_unit_group_id.Value))).OrderByDescending<GearGear, int>((Func<GearGear, int>) (y => y.rarity.index)).ThenByDescending<GearGear, bool>((Func<GearGear, bool>) (yy => !Array.Find<GearSpecificationOfEquipmentUnit>(gearSpecials, (Predicate<GearSpecificationOfEquipmentUnit>) (gg =>
    {
      int groupId = gg.group_id;
      int? equipmentUnitGroupId = yy.specification_of_equipment_unit_group_id;
      int valueOrDefault = equipmentUnitGroupId.GetValueOrDefault();
      return groupId == valueOrDefault & equipmentUnitGroupId.HasValue;
    })).job_id.HasValue)).ThenBy<GearGear, int>((Func<GearGear, int>) (a => dicGroupPoints[a.specification_of_equipment_unit_group_id.Value])).ThenByDescending<GearGear, int>((Func<GearGear, int>) (b => b.ID)).FirstOrDefault<GearGear>();
  }

  private enum BackGround
  {
    Off = -1, // 0xFFFFFFFF
    Equip = 0,
    ToQuest = 1,
  }
}
