// Decompiled with JetBrains decompiler
// Type: Shop00723PopupSkillSkill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Shop00723PopupSkillSkill : MonoBehaviour
{
  [SerializeField]
  private UIWidget base_;
  [SerializeField]
  private GameObject labelSkill_;
  [SerializeField]
  private GameObject labelMagic_;
  [SerializeField]
  private GameObject labelDress_;
  [SerializeField]
  private UILabel txtName_;
  [SerializeField]
  private UILabel txtDetail_;
  [SerializeField]
  private UILabel txtMaxLv_;
  [SerializeField]
  private UILabel txtFactor_;
  [SerializeField]
  private GameObject topIcon_;
  [SerializeField]
  private GameObject[] topIconGenres_ = new GameObject[2];
  private GameObject objIcon_;
  private GameObject objFactor_;
  private GameObject objGenre1_;
  private GameObject objGenre2_;
  private UnitBattleSkillOrigin[] skillOrigins_;
  public int skillID;
  private const int DEFAULT_UNIT_LEVEL = 1;
  private static Color COLOR_GRAY = new Color(0.3f, 0.3f, 0.3f);
  private const int WIDTH_ICON_ELEMENT = 60;
  private const int HEIGHT_ICON_ELEMENT = 55;
  private const int DEPTH_INTERVAL = 1;

  public IEnumerator coInitialize(Shop00723Menu menu, UnitBattleSkillOrigin[] datas)
  {
    this.skillOrigins_ = datas;
    IEnumerator e = this.coInitialize(menu);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private Shop00723PopupSkillSkill.SkillMode GetSkilllabelMode(BattleskillSkill skill)
  {
    if (skill.skill_type == BattleskillSkillType.magic)
      return Shop00723PopupSkillSkill.SkillMode.Magic;
    return skill.awake_skill_category_id == 1 ? Shop00723PopupSkillSkill.SkillMode.Skill : Shop00723PopupSkillSkill.SkillMode.DressSkill;
  }

  private void SetSkillLabel(Shop00723PopupSkillSkill.SkillMode mode)
  {
    this.labelMagic_.SetActive(mode == Shop00723PopupSkillSkill.SkillMode.Magic);
    this.labelSkill_.SetActive(mode == Shop00723PopupSkillSkill.SkillMode.Skill);
    this.labelDress_.SetActive(mode == Shop00723PopupSkillSkill.SkillMode.DressSkill);
  }

  private IEnumerator coInitialize(Shop00723Menu menu)
  {
    UnitBattleSkillOrigin skillOrigin = ((IEnumerable<UnitBattleSkillOrigin>) this.skillOrigins_).First<UnitBattleSkillOrigin>();
    int num = skillOrigin.skill_.skill_type == BattleskillSkillType.magic ? 1 : 0;
    BattleskillSkill skill = skillOrigin.skill_;
    this.skillID = skill.ID;
    this.SetSkillLabel(this.GetSkilllabelMode(skill));
    this.txtName_.SetTextLocalize(skill.name);
    string equipableText = AwakeSkillCategory.GetEquipableText(skill.awake_skill_category_id);
    if (equipableText == string.Empty)
      this.txtDetail_.SetTextLocalize(skill.description);
    else
      this.txtDetail_.SetTextLocalize(skill.description + string.Format("\n[ff2200]{0}[-]", (object) equipableText));
    this.txtMaxLv_.SetTextLocalize(skill.upper_level > 0 ? string.Format(Consts.GetInstance().SHOP_00723_SKILL_LEVEL_MAX, (object) skill.upper_level) : "");
    UI2DSprite iconSprite;
    if (num != 0)
    {
      this.objIcon_ = menu.prefabIconElement.Clone(this.topIcon_.transform);
      CommonElementIcon component = this.objIcon_.GetComponent<CommonElementIcon>();
      component.Init(skillOrigin.skill_.element);
      NGUITools.AdjustDepth(((Component) component).gameObject, this.base_.depth + 1);
      iconSprite = component.iconSprite;
      ((UIWidget) iconSprite).SetDimensions(60, 55);
    }
    else
    {
      this.objIcon_ = menu.prefabIconSkill.Clone(this.topIcon_.transform);
      BattleSkillIcon scIcon = this.objIcon_.GetComponent<BattleSkillIcon>();
      IEnumerator e = scIcon.Init(skill);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      NGUITools.AdjustDepth(((Component) scIcon).gameObject, this.base_.depth + 1);
      iconSprite = scIcon.iconSprite;
      scIcon = (BattleSkillIcon) null;
    }
    string text = "";
    if (skillOrigin.IsOriginEvolution)
    {
      ((UIWidget) iconSprite).color = Shop00723PopupSkillSkill.COLOR_GRAY;
      UnitSkillEvolution evolution = skillOrigin.Evolution;
      text = string.Format(Consts.GetInstance().SHOP_00723_SKILL_ORIGIN_EVOLUTION, (object) evolution.level);
    }
    else if (skillOrigin.IsOriginCharacterQuest)
    {
      ((UIWidget) iconSprite).color = Shop00723PopupSkillSkill.COLOR_GRAY;
      Consts instance = Consts.GetInstance();
      text = string.Format(instance.SHOP_00723_SKILL_ORIGIN_QUEST, (object) instance.QUEST_TYPE_NAME_CHARACTER);
    }
    else if (skillOrigin.IsOriginHarmonyQuest)
    {
      ((UIWidget) iconSprite).color = Shop00723PopupSkillSkill.COLOR_GRAY;
      Consts instance = Consts.GetInstance();
      text = string.Format(instance.SHOP_00723_SKILL_ORIGIN_QUEST, (object) instance.QUEST_TYPE_NAME_HARMONY);
    }
    else if (skillOrigin.IsOriginAwake)
    {
      ((UIWidget) iconSprite).color = Shop00723PopupSkillSkill.COLOR_GRAY;
      text = string.Format(skillOrigin.skill_.awake_skill_category_id == 3 ? Consts.GetInstance().SHOP_00723_SKILL_ORIGIN_AWAKE : Consts.GetInstance().SHOP_00723_SKILL_ORIGIN_AWAKE_SECOND, (object) skillOrigin.Awake.need_affection);
    }
    else if (skillOrigin.IsOriginSEA)
    {
      this.txtMaxLv_.SetTextLocalize("");
      ((UIWidget) iconSprite).color = Shop00723PopupSkillSkill.COLOR_GRAY;
      text = Consts.GetInstance().SHOP_00723_SKILL_ORIGIN_SEA;
    }
    else if (skillOrigin.Basic.level > 1)
    {
      ((UIWidget) iconSprite).color = Shop00723PopupSkillSkill.COLOR_GRAY;
      text = string.Format(Consts.GetInstance().SHOP_00723_SKILL_ORIGIN_BASIC, (object) skillOrigin.Basic.level);
    }
    if (1 < this.skillOrigins_.Length)
    {
      UnitBattleSkillOrigin skillOrigin1 = this.skillOrigins_[1];
      if (skillOrigin1.IsOriginEvolution)
        text += string.Format(Consts.GetInstance().SHOP_00723_SKILL_NEXT_EVOLUTION, (object) skillOrigin1.Evolution.level);
      else if (skillOrigin1.IsOriginCharacterQuest)
      {
        Consts instance = Consts.GetInstance();
        text += string.Format(instance.SHOP_00723_SKILL_NEXT_QUEST, (object) instance.QUEST_TYPE_NAME_CHARACTER);
      }
      else if (skillOrigin1.IsOriginHarmonyQuest)
      {
        Consts instance = Consts.GetInstance();
        text += string.Format(instance.SHOP_00723_SKILL_NEXT_QUEST, (object) instance.QUEST_TYPE_NAME_HARMONY);
      }
    }
    this.txtFactor_.SetTextLocalize(text);
    BattleskillGenre? genre1 = skill.genre1;
    if (genre1.HasValue)
    {
      this.objGenre1_ = menu.prefabIconGenre.Clone(this.topIconGenres_[0].transform);
      SkillGenreIcon component = this.objGenre1_.GetComponent<SkillGenreIcon>();
      component.Init(genre1);
      NGUITools.AdjustDepth(((Component) component).gameObject, this.base_.depth + 1);
    }
    BattleskillGenre? genre2 = skill.genre2;
    if (genre2.HasValue)
    {
      this.objGenre2_ = menu.prefabIconGenre.Clone(this.topIconGenres_[1].transform);
      SkillGenreIcon component = this.objGenre2_.GetComponent<SkillGenreIcon>();
      component.Init(genre2);
      NGUITools.AdjustDepth(((Component) component).gameObject, this.base_.depth + 1);
    }
  }

  private void Awake()
  {
    ((Collider) ((Component) this).GetComponent<BoxCollider>()).enabled = false;
  }

  private enum SkillMode
  {
    Magic,
    Skill,
    DressSkill,
  }
}
