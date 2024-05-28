// Decompiled with JetBrains decompiler
// Type: DetailUnitParameter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnitStatusInformation;
using UnityEngine;

#nullable disable
public class DetailUnitParameter : MonoBehaviour
{
  [SerializeField]
  private DetailUnitParameter.UnitObjectData[] unitObjects;
  [SerializeField]
  private DetailUnitParameter.SkillObject[] skillObjects;
  [SerializeField]
  private UISprite bgColorSprite;
  [SerializeField]
  private UILabel unitName;
  [SerializeField]
  private UIWidget unitTypeIconRoot;
  [SerializeField]
  private UI2DSprite raritySprite;
  [SerializeField]
  private GameObject slcAwakening;
  [SerializeField]
  private UISprite slcCountry;
  private Color bgColor = Color.white;
  private GameObject battleSkillIconPrefab;
  private GameObject skillDetailPrefab;
  private List<PopupSkillDetails.Param> skillParams;
  [SerializeField]
  private UIWidget manageSizeWidget;
  [SerializeField]
  private int addedHeightWhenAddSkills;
  private UIButton oldUnitTypeButton;
  private readonly Dictionary<CommonElement, Color32> JobBgColors = new Dictionary<CommonElement, Color32>()
  {
    {
      CommonElement.none,
      new Color32((byte) 0, (byte) 0, (byte) 0, byte.MaxValue)
    },
    {
      CommonElement.fire,
      new Color32((byte) 186, (byte) 10, (byte) 10, byte.MaxValue)
    },
    {
      CommonElement.wind,
      new Color32((byte) 11, (byte) 152, (byte) 31, byte.MaxValue)
    },
    {
      CommonElement.ice,
      new Color32((byte) 0, (byte) 117, (byte) 178, byte.MaxValue)
    },
    {
      CommonElement.thunder,
      new Color32((byte) 247, (byte) 163, (byte) 0, byte.MaxValue)
    },
    {
      CommonElement.light,
      new Color32((byte) 190, (byte) 186, (byte) 153, byte.MaxValue)
    },
    {
      CommonElement.dark,
      new Color32((byte) 141, (byte) 2, (byte) 238, byte.MaxValue)
    }
  };
  private readonly Dictionary<CommonElement, Color32> CharactorSkillBgColors = new Dictionary<CommonElement, Color32>()
  {
    {
      CommonElement.none,
      new Color32((byte) 0, (byte) 0, (byte) 0, byte.MaxValue)
    },
    {
      CommonElement.fire,
      new Color32((byte) 157, (byte) 9, (byte) 9, byte.MaxValue)
    },
    {
      CommonElement.wind,
      new Color32((byte) 10, (byte) 141, (byte) 29, byte.MaxValue)
    },
    {
      CommonElement.ice,
      new Color32((byte) 0, (byte) 100, (byte) 152, byte.MaxValue)
    },
    {
      CommonElement.thunder,
      new Color32((byte) 203, (byte) 134, (byte) 0, byte.MaxValue)
    },
    {
      CommonElement.light,
      new Color32((byte) 144, (byte) 141, (byte) 112, byte.MaxValue)
    },
    {
      CommonElement.dark,
      new Color32((byte) 121, (byte) 0, (byte) 206, byte.MaxValue)
    }
  };
  private readonly Dictionary<CommonElement, Color32> BgColors = new Dictionary<CommonElement, Color32>()
  {
    {
      CommonElement.none,
      new Color32((byte) 0, (byte) 0, (byte) 0, byte.MaxValue)
    },
    {
      CommonElement.fire,
      new Color32((byte) 231, (byte) 66, (byte) 66, byte.MaxValue)
    },
    {
      CommonElement.wind,
      new Color32((byte) 73, (byte) 172, (byte) 87, byte.MaxValue)
    },
    {
      CommonElement.ice,
      new Color32((byte) 62, (byte) 140, (byte) 180, byte.MaxValue)
    },
    {
      CommonElement.thunder,
      new Color32((byte) 244, (byte) 194, (byte) 96, byte.MaxValue)
    },
    {
      CommonElement.light,
      new Color32((byte) 216, (byte) 212, (byte) 175, byte.MaxValue)
    },
    {
      CommonElement.dark,
      new Color32((byte) 182, (byte) 88, byte.MaxValue, byte.MaxValue)
    }
  };

  public IEnumerator Init(int unitID, int position)
  {
    if (MasterData.UnitUnit.ContainsKey(unitID))
    {
      UnitUnit unit = MasterData.UnitUnit[unitID];
      Future<GameObject> f = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
      IEnumerator e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.battleSkillIconPrefab = f.Result;
      this.skillParams = new List<PopupSkillDetails.Param>();
      if (Object.op_Equality((Object) this.skillDetailPrefab, (Object) null))
      {
        Future<GameObject> loader = PopupSkillDetails.createPrefabLoader(false);
        yield return (object) loader.Wait();
        this.skillDetailPrefab = loader.Result;
        loader = (Future<GameObject>) null;
      }
      if (Object.op_Inequality((Object) this.slcAwakening, (Object) null))
      {
        this.slcAwakening.SetActive(false);
        if (unit.awake_unit_flag)
          this.slcAwakening.SetActive(true);
      }
      if (Object.op_Inequality((Object) this.slcCountry, (Object) null))
      {
        ((Component) this.slcCountry).gameObject.SetActive(false);
        if (unit.country_attribute.HasValue)
        {
          ((Component) this.slcCountry).gameObject.SetActive(true);
          unit.SetCuntrySpriteName(ref this.slcCountry);
        }
      }
      this.unitName.SetTextLocalize(unit.name);
      GearKindIcon.GetPrefab().CloneAndGetComponent<GearKindIcon>(((Component) this.unitTypeIconRoot).gameObject).Init(unit.kind, unit.GetElement());
      ((UIWidget) this.bgColorSprite).color = Color32.op_Implicit(this.BgColors[unit.GetElement()]);
      this.raritySprite.sprite2D = RarityIcon.GetSprite(unit);
      UI2DSprite raritySprite1 = this.raritySprite;
      Rect textureRect = this.raritySprite.sprite2D.textureRect;
      int width = (int) ((Rect) ref textureRect).width;
      ((UIWidget) raritySprite1).width = width;
      UI2DSprite raritySprite2 = this.raritySprite;
      textureRect = this.raritySprite.sprite2D.textureRect;
      int height = (int) ((Rect) ref textureRect).height;
      ((UIWidget) raritySprite2).height = height;
      foreach (DetailUnitParameter.UnitObjectData unitObject in this.unitObjects)
        unitObject.rootObject.SetActive(false);
      int index1 = position == 9 ? 1 : 0;
      UnitLeaderSkill leaderSkill = ((IEnumerable<UnitLeaderSkill>) MasterData.UnitLeaderSkillList).FirstOrDefault<UnitLeaderSkill>((Func<UnitLeaderSkill, bool>) (x => x.unit.ID == unitID));
      List<UnitSkillCharacterQuest> charactorSkills = ((IEnumerable<UnitSkillCharacterQuest>) MasterData.UnitSkillCharacterQuestList).Where<UnitSkillCharacterQuest>((Func<UnitSkillCharacterQuest, bool>) (x => x.unit.ID == unitID && x.skill.skill_type != BattleskillSkillType.leader)).OrderBy<UnitSkillCharacterQuest, int>((Func<UnitSkillCharacterQuest, int>) (x => x.character_quest.priority)).ToList<UnitSkillCharacterQuest>();
      List<UnitSkill> unitTypeSkills = ((IEnumerable<UnitSkill>) MasterData.UnitSkillList).Where<UnitSkill>((Func<UnitSkill, bool>) (x =>
      {
        if (x.unit.ID != unitID || x.skill.skill_type == BattleskillSkillType.magic)
          return false;
        return x.unit_type == 1 || x.unit_type == 2 || x.unit_type == 3 || x.unit_type == 4 || x.unit_type == 5 || x.unit_type == 6;
      })).ToList<UnitSkill>();
      bool isDualTypeSkill = false;
      if (unitTypeSkills.Count<UnitSkill>((Func<UnitSkill, bool>) (x => x.unit_type == 1)) == 2 || unitTypeSkills.Count<UnitSkill>((Func<UnitSkill, bool>) (x => x.unit_type == 2)) == 2 || unitTypeSkills.Count<UnitSkill>((Func<UnitSkill, bool>) (x => x.unit_type == 3)) == 2 || unitTypeSkills.Count<UnitSkill>((Func<UnitSkill, bool>) (x => x.unit_type == 4)) == 2 || unitTypeSkills.Count<UnitSkill>((Func<UnitSkill, bool>) (x => x.unit_type == 5)) == 2 || unitTypeSkills.Count<UnitSkill>((Func<UnitSkill, bool>) (x => x.unit_type == 6)) == 2)
        isDualTypeSkill = true;
      foreach (DetailUnitParameter.SkillObject skillObject in this.skillObjects)
        skillObject.rootObject.SetActive(false);
      int activeIndex = -1;
      if (leaderSkill != null)
        activeIndex = !(unitTypeSkills.Count > 0 & isDualTypeSkill) || charactorSkills.Count != 2 && (charactorSkills.Count != 1 || charactorSkills[0].skillOfEvolution == null) ? (unitTypeSkills.Count <= 0 || charactorSkills.Count != 2 && (charactorSkills.Count != 1 || charactorSkills[0].skillOfEvolution == null) ? (charactorSkills.Count == 2 || charactorSkills.Count == 1 && charactorSkills[0].skillOfEvolution != null ? 1 : (charactorSkills.Count != 1 ? (!(unitTypeSkills.Count > 0 & isDualTypeSkill) ? (unitTypeSkills.Count <= 0 ? 6 : 5) : 4) : 0)) : 2) : 3;
      if (activeIndex < 0 || this.skillObjects.Length <= activeIndex)
      {
        Debug.LogError((object) "DetailUnitParameter can`t show this unit skills");
        activeIndex = 0;
      }
      this.manageSizeWidget.SetDimensions(((Vector2Int) ref this.skillObjects[activeIndex].UiWidgetSize).x, ((Vector2Int) ref this.skillObjects[activeIndex].UiWidgetSize).y);
      if (Object.op_Inequality((Object) this.skillObjects[activeIndex].unitExtraSetting.maskTexture, (Object) null))
      {
        this.unitObjects[index1].charaSprite.maskTexture = this.skillObjects[activeIndex].unitExtraSetting.maskTexture;
        ((Component) this.unitObjects[index1].charaSprite).transform.localPosition = this.skillObjects[activeIndex].unitExtraSetting.characterPosition;
        this.unitObjects[index1].charaSprite.yOffsetPixel = this.skillObjects[activeIndex].unitExtraSetting.characterOffsetY;
      }
      if (Object.op_Inequality((Object) this.skillObjects[activeIndex].unitShadowExtraSetting.maskTexture, (Object) null))
      {
        this.unitObjects[index1].charaShadowSprite.maskTexture = this.skillObjects[activeIndex].unitShadowExtraSetting.maskTexture;
        ((Component) this.unitObjects[index1].charaShadowSprite).transform.localPosition = this.skillObjects[activeIndex].unitShadowExtraSetting.characterPosition;
        this.unitObjects[index1].charaShadowSprite.yOffsetPixel = this.skillObjects[activeIndex].unitShadowExtraSetting.characterOffsetY;
      }
      e = this.InitUnitObject(unit, this.unitObjects[index1]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (activeIndex >= 0)
      {
        e = this.InitSkillObject(this.skillObjects[activeIndex], leaderSkill, charactorSkills, unit.GetElement());
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      List<UnitSkill> unitSkillList = unitTypeSkills;
      // ISSUE: explicit non-virtual call
      if ((unitSkillList != null ? (__nonvirtual (unitSkillList.Count) > 0 ? 1 : 0) : 0) != 0)
      {
        for (int index2 = 0; index2 < ((IEnumerable<DetailUnitParameter.UnitTypeSkillObject>) this.skillObjects[activeIndex].unitTypeSkillObjects).Count<DetailUnitParameter.UnitTypeSkillObject>(); ++index2)
        {
          this.skillObjects[activeIndex].unitTypeSkillObjects[index2].unitTypeSkillIcons = new List<DetailUnitParameter.UnitTypeSkillIcon>();
          foreach (DetailUnitParameter.UnitTypeButton unitTypeButton in this.skillObjects[activeIndex].unitTypeButtons)
            this.skillObjects[activeIndex].unitTypeSkillObjects[index2].unitTypeSkillIcons.Add(new DetailUnitParameter.UnitTypeSkillIcon(unitTypeButton.unitType));
        }
        e = this.InitUnitTypeSkillButton(activeIndex, isDualTypeSkill, unitTypeSkills, unit.GetElement());
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  private IEnumerator InitUnitObject(UnitUnit unit, DetailUnitParameter.UnitObjectData data)
  {
    data.rootObject.SetActive(true);
    Future<Sprite> f = unit.LoadSpriteLarge();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    data.charaSprite.MainUI2DSprite.sprite2D = f.Result;
    data.charaSprite.FitMask();
    data.charaShadowSprite.MainUI2DSprite.sprite2D = f.Result;
    data.charaShadowSprite.FitMask();
    ((UIWidget) data.jobBaseSprite).color = Color32.op_Implicit(this.JobBgColors[unit.GetElement()]);
    data.jobName.SetTextLocalize(unit.job.name);
    IOrderedEnumerable<UnitSkill> skills = ((IEnumerable<UnitSkill>) unit.RememberUnitSkills()).Where<UnitSkill>((Func<UnitSkill, bool>) (x => x.skill.skill_type != BattleskillSkillType.growth && x.skill.DispSkillList)).OrderBy<UnitSkill, int>((Func<UnitSkill, int>) (x => x.level));
    int skillsCount = skills.Count<UnitSkill>();
    if (skillsCount > 9)
    {
      for (int index = 9; index < data.linkSkillIcons.Length; ++index)
        ((Component) data.linkSkillIcons[index]).gameObject.SetActive(true);
      for (int index = 0; index < this.skillObjects.Length; ++index)
        this.skillObjects[index].rootObject.transform.localPosition = new Vector3(this.skillObjects[index].rootObject.transform.localPosition.x, this.skillObjects[index].rootObject.transform.localPosition.y - (float) this.addedHeightWhenAddSkills, this.skillObjects[index].rootObject.transform.localPosition.z);
      this.manageSizeWidget.height += this.addedHeightWhenAddSkills;
      UISprite jobBaseSprite = data.jobBaseSprite;
      ((UIWidget) jobBaseSprite).height = ((UIWidget) jobBaseSprite).height + this.addedHeightWhenAddSkills;
    }
    for (int i = 0; i < data.linkSkillIcons.Length && skillsCount > i; ++i)
    {
      BattleskillSkill skill = skills.ElementAt<UnitSkill>(i).skill;
      UIWidget rootObj = data.linkSkillIcons[i];
      BattleSkillIcon battleSkillIcon = this.battleSkillIconPrefab.CloneAndGetComponent<BattleSkillIcon>(((Component) rootObj).gameObject);
      e = battleSkillIcon.Init(skill);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      battleSkillIcon.SetDepth(rootObj.depth);
      this.skillParams.Add(new PopupSkillDetails.Param(skill, UnitParameter.SkillGroup.None));
      UIButton component = ((Component) rootObj).gameObject.GetComponent<UIButton>();
      if (Object.op_Inequality((Object) component, (Object) null))
        this.setEventPopupSkillDetail(component, skill);
      skill = (BattleskillSkill) null;
      rootObj = (UIWidget) null;
      battleSkillIcon = (BattleSkillIcon) null;
    }
  }

  private IEnumerator InitSkillObject(
    DetailUnitParameter.SkillObject skillObject,
    UnitLeaderSkill leaderSkill,
    List<UnitSkillCharacterQuest> charactorSkills,
    CommonElement element)
  {
    skillObject.rootObject.SetActive(true);
    BattleskillSkill s = leaderSkill.skill;
    skillObject.leaderSkillObject.leaderSkillDiscription.SetTextLocalize(s.description);
    this.skillParams.Add(new PopupSkillDetails.Param(s, UnitParameter.SkillGroup.Leader));
    this.setEventPopupSkillDetail(skillObject.leaderSkillObject.btnZoom, s);
    UnitSkillCharacterQuest charactorSkill;
    DetailUnitParameter.CharactorSkillObject cso;
    BattleSkillIcon battleSkillIcon;
    IEnumerator e;
    if (charactorSkills != null && charactorSkills.Count == 1)
    {
      charactorSkill = charactorSkills[0];
      if (skillObject.charactorSkillObjects.Length >= 1)
      {
        cso = skillObject.charactorSkillObjects[0];
        s = charactorSkill.skill;
        cso.skillName.SetTextLocalize(s.name);
        cso.skillDiscription.SetTextLocalize(s.description);
        battleSkillIcon = this.battleSkillIconPrefab.CloneAndGetComponent<BattleSkillIcon>(((Component) cso.skillIconRoot).gameObject);
        e = battleSkillIcon.Init(s);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        battleSkillIcon.SetDepth(cso.skillIconRoot.depth);
        cso.skillEntryPoint.SetTextLocalize(Consts.Format(Consts.GetInstance().DETAIL_UNIT_PARAMETER_GET_SKILL, (IDictionary) new Hashtable()
        {
          {
            (object) "episode",
            (object) charactorSkill.character_quest.priority
          }
        }));
        ((UIWidget) cso.skillBGColor).color = Color32.op_Implicit(this.CharactorSkillBgColors[element]);
        this.skillParams.Add(new PopupSkillDetails.Param(s, UnitParameter.SkillGroup.None));
        this.setEventPopupSkillDetail(cso.btnZoom, s);
        cso = new DetailUnitParameter.CharactorSkillObject();
        battleSkillIcon = (BattleSkillIcon) null;
      }
      if (skillObject.charactorSkillObjects.Length >= 2 && charactorSkill.skillOfEvolution != null)
      {
        cso = skillObject.charactorSkillObjects[1];
        s = charactorSkill.skillOfEvolution;
        cso.skillName.SetTextLocalize(s.name);
        cso.skillDiscription.SetTextLocalize(s.description);
        battleSkillIcon = this.battleSkillIconPrefab.CloneAndGetComponent<BattleSkillIcon>(((Component) cso.skillIconRoot).gameObject);
        e = battleSkillIcon.Init(s);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        battleSkillIcon.SetDepth(cso.skillIconRoot.depth);
        if (charactorSkill.evolution_character_quest != null)
          cso.skillEntryPoint.SetTextLocalize(Consts.Format(Consts.GetInstance().DETAIL_UNIT_PARAMETER_EVO_SKILL, (IDictionary) new Hashtable()
          {
            {
              (object) "episode",
              (object) charactorSkill.evolution_character_quest.priority
            }
          }));
        else
          ((Component) cso.skillEntryPoint).gameObject.SetActive(false);
        ((UIWidget) cso.skillBGColor).color = Color32.op_Implicit(this.CharactorSkillBgColors[element]);
        this.skillParams.Add(new PopupSkillDetails.Param(s, UnitParameter.SkillGroup.None));
        this.setEventPopupSkillDetail(cso.btnZoom, s);
        cso = new DetailUnitParameter.CharactorSkillObject();
        battleSkillIcon = (BattleSkillIcon) null;
      }
      charactorSkill = (UnitSkillCharacterQuest) null;
    }
    else if (charactorSkills != null && charactorSkills.Count == 2)
    {
      charactorSkill = charactorSkills[0];
      if (skillObject.charactorSkillObjects.Length >= 1)
      {
        cso = skillObject.charactorSkillObjects[0];
        s = charactorSkill.skill;
        cso.skillName.SetTextLocalize(s.name);
        cso.skillDiscription.SetTextLocalize(s.description);
        battleSkillIcon = this.battleSkillIconPrefab.CloneAndGetComponent<BattleSkillIcon>(((Component) cso.skillIconRoot).gameObject);
        e = battleSkillIcon.Init(s);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        battleSkillIcon.SetDepth(cso.skillIconRoot.depth);
        cso.skillEntryPoint.SetTextLocalize(Consts.Format(Consts.GetInstance().DETAIL_UNIT_PARAMETER_GET_SKILL, (IDictionary) new Hashtable()
        {
          {
            (object) "episode",
            (object) charactorSkill.character_quest.priority
          }
        }));
        ((UIWidget) cso.skillBGColor).color = Color32.op_Implicit(this.CharactorSkillBgColors[element]);
        this.skillParams.Add(new PopupSkillDetails.Param(s, UnitParameter.SkillGroup.None));
        this.setEventPopupSkillDetail(cso.btnZoom, s);
        cso = new DetailUnitParameter.CharactorSkillObject();
        battleSkillIcon = (BattleSkillIcon) null;
      }
      charactorSkill = charactorSkills[1];
      if (skillObject.charactorSkillObjects.Length >= 2 && charactorSkill != null)
      {
        cso = skillObject.charactorSkillObjects[1];
        s = charactorSkill.skill;
        cso.skillName.SetTextLocalize(s.name);
        cso.skillDiscription.SetTextLocalize(s.description);
        battleSkillIcon = this.battleSkillIconPrefab.CloneAndGetComponent<BattleSkillIcon>(((Component) cso.skillIconRoot).gameObject);
        e = battleSkillIcon.Init(s);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        battleSkillIcon.SetDepth(cso.skillIconRoot.depth);
        cso.skillEntryPoint.SetTextLocalize(Consts.Format(Consts.GetInstance().DETAIL_UNIT_PARAMETER_GET_SKILL, (IDictionary) new Hashtable()
        {
          {
            (object) "episode",
            (object) charactorSkill.character_quest.priority
          }
        }));
        ((UIWidget) cso.skillBGColor).color = Color32.op_Implicit(this.CharactorSkillBgColors[element]);
        this.skillParams.Add(new PopupSkillDetails.Param(s, UnitParameter.SkillGroup.None));
        this.setEventPopupSkillDetail(cso.btnZoom, s);
        cso = new DetailUnitParameter.CharactorSkillObject();
        battleSkillIcon = (BattleSkillIcon) null;
      }
      charactorSkill = (UnitSkillCharacterQuest) null;
    }
  }

  private IEnumerator InitUnitTypeSkillButton(
    int activeIndex,
    bool isDualTypeSkill,
    List<UnitSkill> unitTypeSkill,
    CommonElement element)
  {
    DetailUnitParameter detailUnitParameter1 = this;
    DetailUnitParameter.SkillObject skillObject;
    Future<Sprite> ft;
    IEnumerator e;
    if (isDualTypeSkill)
    {
      DetailUnitParameter detailUnitParameter = detailUnitParameter1;
      skillObject = detailUnitParameter1.skillObjects[activeIndex];
      List<DetailUnitParameter.UnitTypeButton> unitTypeButtons = skillObject.unitTypeButtons;
      // ISSUE: explicit non-virtual call
      if ((unitTypeButtons != null ? (__nonvirtual (unitTypeButtons.Count) > 0 ? 1 : 0) : 0) != 0)
      {
        DetailUnitParameter.UnitTypeSkillObject[] unitTypeSkillObjects = skillObject.unitTypeSkillObjects;
        foreach (DetailUnitParameter.UnitTypeSkillObject unitTypeSkillObject in unitTypeSkillObjects)
          ((UIWidget) unitTypeSkillObject.skillBGColor).color = Color32.op_Implicit(detailUnitParameter1.CharactorSkillBgColors[element]);
        foreach (DetailUnitParameter.UnitTypeButton unitTypeButton in skillObject.unitTypeButtons)
        {
          DetailUnitParameter.UnitTypeButton unitTypeSkillButton = unitTypeButton;
          List<UnitSkill> unitSkills = unitTypeSkill.Where<UnitSkill>((Func<UnitSkill, bool>) (x => (UnitTypeEnum) x.unit_type == unitTypeSkillButton.unitType)).ToList<UnitSkill>();
          List<UnitSkill> unitSkillList = unitSkills;
          // ISSUE: explicit non-virtual call
          if ((unitSkillList != null ? (__nonvirtual (unitSkillList.Count) > 0 ? 1 : 0) : 0) == 0)
          {
            ((Behaviour) unitTypeSkillButton.btnUnitType).enabled = false;
            ((Collider) ((Component) unitTypeSkillButton.btnUnitType).GetComponent<BoxCollider>()).enabled = false;
            ((IEnumerable<UISprite>) ((Component) unitTypeSkillButton.btnUnitType).GetComponentsInChildren<UISprite>()).ForEach<UISprite>((Action<UISprite>) (x => ((UIWidget) x).color = Color32.op_Implicit(new Color32((byte) 70, (byte) 70, (byte) 70, byte.MaxValue))));
          }
          else
          {
            foreach (UnitSkill unitSkill in unitSkills)
              detailUnitParameter1.skillParams.Add(new PopupSkillDetails.Param(unitSkill.skill, UnitParameter.SkillGroup.None));
            for (int i = 0; i < ((IEnumerable<DetailUnitParameter.UnitTypeSkillObject>) unitTypeSkillObjects).Count<DetailUnitParameter.UnitTypeSkillObject>(); ++i)
            {
              ft = (Future<Sprite>) null;
              ft = unitSkills.Count > i ? unitSkills[i].skill.LoadBattleSkillIcon() : new BattleskillSkill().LoadBattleSkillIcon();
              e = ft.Wait();
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              unitTypeSkillObjects[i].unitTypeSkillIcons.Find((Predicate<DetailUnitParameter.UnitTypeSkillIcon>) (x => x.unitType == unitTypeSkillButton.unitType)).icon = ft.Result;
              ft = (Future<Sprite>) null;
            }
            EventDelegate.Set(unitTypeSkillButton.btnUnitType.onClick, (EventDelegate.Callback) (() =>
            {
              if (Object.op_Inequality((Object) closure_5.oldUnitTypeButton, (Object) null))
                ((UIButtonColor) closure_5.oldUnitTypeButton).isEnabled = true;
              ((UIButtonColor) unitTypeSkillButton.btnUnitType).isEnabled = false;
              closure_5.oldUnitTypeButton = unitTypeSkillButton.btnUnitType;
              for (int index = 0; index < ((IEnumerable<DetailUnitParameter.UnitTypeSkillObject>) unitTypeSkillObjects).Count<DetailUnitParameter.UnitTypeSkillObject>(); ++index)
              {
                if (unitSkills.Count <= index)
                  closure_5.SetUnitTypeSkill(unitTypeSkillObjects[index], unitTypeSkillButton.unitType);
                else
                  closure_5.SetUnitTypeSkill(unitTypeSkillObjects[index], unitTypeSkillButton.unitType, unitSkills[index].skill);
              }
            }));
          }
        }
        for (int index = 0; index < ((IEnumerable<DetailUnitParameter.UnitTypeSkillObject>) unitTypeSkillObjects).Count<DetailUnitParameter.UnitTypeSkillObject>(); ++index)
        {
          unitTypeSkillObjects[index].skillIcon = detailUnitParameter1.battleSkillIconPrefab.CloneAndGetComponent<BattleSkillIcon>(((Component) unitTypeSkillObjects[index].skillIconRoot).gameObject);
          unitTypeSkillObjects[index].skillIcon.SetDepth(unitTypeSkillObjects[index].skillIconRoot.depth);
        }
        EventDelegate.Execute(skillObject.unitTypeButtons.Find((Predicate<DetailUnitParameter.UnitTypeButton>) (x => x.unitType == UnitTypeEnum.ouki)).btnUnitType.onClick);
        skillObject = new DetailUnitParameter.SkillObject();
      }
    }
    else
    {
      DetailUnitParameter detailUnitParameter = detailUnitParameter1;
      skillObject = detailUnitParameter1.skillObjects[activeIndex];
      List<DetailUnitParameter.UnitTypeButton> unitTypeButtons = skillObject.unitTypeButtons;
      // ISSUE: explicit non-virtual call
      if ((unitTypeButtons != null ? (__nonvirtual (unitTypeButtons.Count) > 0 ? 1 : 0) : 0) != 0)
      {
        DetailUnitParameter.UnitTypeSkillObject unitTypeSkillObject = skillObject.unitTypeSkillObjects[0];
        ((UIWidget) unitTypeSkillObject.skillBGColor).color = Color32.op_Implicit(detailUnitParameter1.CharactorSkillBgColors[element]);
        foreach (DetailUnitParameter.UnitTypeButton unitTypeButton in skillObject.unitTypeButtons)
        {
          DetailUnitParameter.UnitTypeButton unitTypeSkillButton = unitTypeButton;
          UnitSkill unitSkill = unitTypeSkill.Find((Predicate<UnitSkill>) (x => (UnitTypeEnum) x.unit_type == unitTypeSkillButton.unitType));
          if (unitSkill == null)
          {
            ((Behaviour) unitTypeSkillButton.btnUnitType).enabled = false;
            ((Collider) ((Component) unitTypeSkillButton.btnUnitType).GetComponent<BoxCollider>()).enabled = false;
            ((IEnumerable<UISprite>) ((Component) unitTypeSkillButton.btnUnitType).GetComponentsInChildren<UISprite>()).ForEach<UISprite>((Action<UISprite>) (x => ((UIWidget) x).color = Color32.op_Implicit(new Color32((byte) 70, (byte) 70, (byte) 70, byte.MaxValue))));
          }
          else
          {
            detailUnitParameter1.skillParams.Add(new PopupSkillDetails.Param(unitSkill.skill, UnitParameter.SkillGroup.None));
            ft = unitSkill.skill.LoadBattleSkillIcon();
            e = ft.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            unitTypeSkillObject.unitTypeSkillIcons.Find((Predicate<DetailUnitParameter.UnitTypeSkillIcon>) (x => x.unitType == unitTypeSkillButton.unitType)).icon = ft.Result;
            EventDelegate.Set(unitTypeSkillButton.btnUnitType.onClick, (EventDelegate.Callback) (() =>
            {
              if (Object.op_Inequality((Object) closure_7.oldUnitTypeButton, (Object) null))
                ((UIButtonColor) closure_7.oldUnitTypeButton).isEnabled = true;
              ((UIButtonColor) unitTypeSkillButton.btnUnitType).isEnabled = false;
              closure_7.oldUnitTypeButton = unitTypeSkillButton.btnUnitType;
              closure_7.SetUnitTypeSkill(unitTypeSkillObject, unitTypeSkillButton.unitType, unitSkill.skill);
            }));
            ft = (Future<Sprite>) null;
          }
        }
        unitTypeSkillObject.skillIcon = detailUnitParameter1.battleSkillIconPrefab.CloneAndGetComponent<BattleSkillIcon>(((Component) unitTypeSkillObject.skillIconRoot).gameObject);
        unitTypeSkillObject.skillIcon.SetDepth(unitTypeSkillObject.skillIconRoot.depth);
        EventDelegate.Execute(skillObject.unitTypeButtons.Find((Predicate<DetailUnitParameter.UnitTypeButton>) (x => x.unitType == UnitTypeEnum.ouki)).btnUnitType.onClick);
        skillObject = new DetailUnitParameter.SkillObject();
      }
    }
  }

  private void SetUnitTypeSkill(
    DetailUnitParameter.UnitTypeSkillObject unitTypeSkillObject,
    UnitTypeEnum unitType,
    BattleskillSkill skill = null)
  {
    unitTypeSkillObject.skillName.text = skill != null ? skill.name : "";
    unitTypeSkillObject.skillDiscription.text = skill != null ? skill.description : "";
    unitTypeSkillObject.skillIcon.iconSprite.sprite2D = unitTypeSkillObject.unitTypeSkillIcons.Find((Predicate<DetailUnitParameter.UnitTypeSkillIcon>) (x => x.unitType == unitType)).icon;
    if (skill == null)
      return;
    this.setEventPopupSkillDetail(unitTypeSkillObject.btnZoom, skill);
  }

  private void setEventPopupSkillDetail(UIButton btn, BattleskillSkill skill)
  {
    EventDelegate.Set(btn.onClick, (EventDelegate.Callback) (() => this.popupSkillDetail(skill)));
  }

  private void popupSkillDetail(BattleskillSkill skill)
  {
    int? nullable = this.skillParams.FirstIndexOrNull<PopupSkillDetails.Param>((Func<PopupSkillDetails.Param, bool>) (x => x.skill == skill));
    if (Object.op_Equality((Object) this.skillDetailPrefab, (Object) null) || !nullable.HasValue)
      return;
    PopupSkillDetails.show(this.skillDetailPrefab, this.skillParams[nullable.Value]);
  }

  [Serializable]
  private struct UnitObjectData
  {
    public GameObject rootObject;
    public NGxMaskSpriteWithScale charaSprite;
    public NGxMaskSpriteWithScale charaShadowSprite;
    public UISprite jobBaseSprite;
    public UILabel jobName;
    public UIWidget[] linkSkillIcons;
  }

  [Serializable]
  private struct CharactorSkillObject
  {
    public UISprite skillBGColor;
    public UILabel skillName;
    public UILabel skillDiscription;
    public UILabel skillEntryPoint;
    public UIWidget skillIconRoot;
    public UIButton btnZoom;
  }

  [Serializable]
  private struct UnitTypeButton
  {
    public UnitTypeEnum unitType;
    public UIButton btnUnitType;
  }

  [Serializable]
  private class UnitTypeSkillIcon
  {
    public UnitTypeEnum unitType;
    [NonSerialized]
    public Sprite icon;

    public UnitTypeSkillIcon(UnitTypeEnum type)
    {
      this.unitType = type;
      this.icon = (Sprite) null;
    }
  }

  [Serializable]
  private struct UnitTypeSkillObject
  {
    public UISprite skillBGColor;
    public UILabel skillName;
    public UILabel skillDiscription;
    public UIWidget skillIconRoot;
    public UIButton btnZoom;
    [NonSerialized]
    public BattleSkillIcon skillIcon;
    [NonSerialized]
    public List<DetailUnitParameter.UnitTypeSkillIcon> unitTypeSkillIcons;
  }

  [Serializable]
  private struct LeaderSkillObject
  {
    public UILabel leaderSkillDiscription;
    public UIButton btnZoom;
  }

  [Serializable]
  private struct UnitSpriteExtraSetting
  {
    public Vector3 characterPosition;
    public int characterOffsetY;
    public Texture2D maskTexture;
  }

  [Serializable]
  private struct SkillObject
  {
    public Vector2Int UiWidgetSize;
    public GameObject rootObject;
    public DetailUnitParameter.LeaderSkillObject leaderSkillObject;
    public DetailUnitParameter.CharactorSkillObject[] charactorSkillObjects;
    public List<DetailUnitParameter.UnitTypeButton> unitTypeButtons;
    public DetailUnitParameter.UnitTypeSkillObject[] unitTypeSkillObjects;
    public DetailUnitParameter.UnitSpriteExtraSetting unitExtraSetting;
    public DetailUnitParameter.UnitSpriteExtraSetting unitShadowExtraSetting;
  }
}
