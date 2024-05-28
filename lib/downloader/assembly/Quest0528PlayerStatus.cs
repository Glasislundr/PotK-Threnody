// Decompiled with JetBrains decompiler
// Type: Quest0528PlayerStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Quest0528PlayerStatus : MonoBehaviour
{
  public NGTweenGaugeScale hpGauge;
  [SerializeField]
  protected SelectParts statusBase;
  [SerializeField]
  protected UIWidget character;
  [SerializeField]
  protected UIWidget weapon;
  [SerializeField]
  protected UILabel txt_CharacterName;
  [SerializeField]
  protected UILabel txt_Lv;
  [SerializeField]
  protected UILabel txt_Fighting;
  [SerializeField]
  protected UILabel txt_Hp;
  [SerializeField]
  protected UILabel txt_Hpmax;
  [SerializeField]
  protected UILabel txt_Jobname;
  [SerializeField]
  protected UILabel txt_Movement;
  [SerializeField]
  protected UILabel txt_Attack;
  [SerializeField]
  protected UILabel txt_Critical;
  [SerializeField]
  protected UILabel txt_Defense;
  [SerializeField]
  protected UILabel txt_Dexterity;
  [SerializeField]
  protected UILabel txt_Evasion;
  [SerializeField]
  protected UILabel txt_Matk;
  [SerializeField]
  protected UILabel txt_Mdef;
  [SerializeField]
  protected UIWidget[] dyn_Ailments;
  [SerializeField]
  protected UIButton[] btn_Ailments;
  private PlayerUnit mUnit;
  private BL.ForceID forceID;
  private UnitIcon unitIcon;
  private GearKindIcon gearIcon;
  private List<BattleSkillIcon> skillIcons;
  private GameObject popupInfoPrefab;
  private Color mGreen = new Color(0.0f, 0.863f, 0.118f);
  private Color mRed = new Color(0.98f, 0.0f, 0.0f);
  private Color mOrigin = new Color(1f, 1f, 1f);

  public IEnumerator Init(
    GameObject normalPrefab,
    GameObject gearKindPrefab,
    GameObject statusDetail,
    GameObject battleSkillIconPrefab,
    GameObject skillDetailDialogPrefab,
    Quest0528Menu.FieldUnitInfo unitInfo)
  {
    this.popupInfoPrefab = statusDetail;
    this.CreateUnitIcon(normalPrefab);
    this.CreateGearKindIcon(gearKindPrefab);
    this.CreateBattleSkillIcon(battleSkillIconPrefab);
    this.statusBase.setValue((int) unitInfo.unitType);
    IEnumerator e = this.SetUnitInfo(unitInfo.unit, (int) unitInfo.unitType);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void CreateUnitIcon(GameObject normalPrefab)
  {
    ((Component) this.character).gameObject.transform.Clear();
    this.unitIcon = normalPrefab.CloneAndGetComponent<UnitIcon>(((Component) this.character).gameObject.transform);
    NGUITools.AdjustDepth(((Component) this.unitIcon).gameObject, this.character.depth);
    this.unitIcon.isViewBackObject = false;
  }

  private void CreateGearKindIcon(GameObject gearKindPrefab)
  {
    ((Component) this.weapon).gameObject.transform.Clear();
    this.gearIcon = gearKindPrefab.CloneAndGetComponent<GearKindIcon>(((Component) this.weapon).gameObject.transform);
    NGUITools.AdjustDepth(((Component) this.gearIcon).gameObject, this.weapon.depth);
    this.gearIcon.SetBasedOnHeight(this.weapon.height);
  }

  private void CreateBattleSkillIcon(GameObject battleSkillIconPrefab)
  {
    this.skillIcons = new List<BattleSkillIcon>();
    foreach (UIWidget dynAilment in this.dyn_Ailments)
    {
      ((Component) dynAilment).gameObject.transform.Clear();
      BattleSkillIcon component = battleSkillIconPrefab.CloneAndGetComponent<BattleSkillIcon>(((Component) dynAilment).gameObject.transform);
      component.SetDepth(dynAilment.depth);
      this.skillIcons.Add(component);
      ((Component) component).gameObject.SetActive(false);
    }
  }

  private IEnumerator doSetIcon(PlayerUnit unit)
  {
    IEnumerator e = this.unitIcon.SetUnit(unit.unit, unit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unitIcon.BottomModeValue = UnitIconBase.BottomMode.Nothing;
  }

  private IEnumerator doSetAilmentIcon(int index, BattleskillSkill skill, int? remainTurn)
  {
    if (skill.ID == this.skillIcons[index].skillID)
    {
      ((Component) this.skillIcons[index]).gameObject.SetActive(true);
      EventDelegate.Set(this.btn_Ailments[index].onClick, (EventDelegate.Callback) (() => { }));
    }
    else if (this.skillIcons.Count > index)
    {
      IEnumerator e = this.skillIcons[index].Init(skill);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ((Component) this.skillIcons[index]).gameObject.SetActive(true);
      EventDelegate.Set(this.btn_Ailments[index].onClick, (EventDelegate.Callback) (() => { }));
    }
  }

  private IEnumerator SetUnitInfo(PlayerUnit unit, int unitType)
  {
    this.mUnit = unit;
    this.forceID = (BL.ForceID) unitType;
    Judgement.NonBattleParameter u = Judgement.NonBattleParameter.FromPlayerUnit(unit);
    IEnumerator e = this.doSetIcon(unit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.gearIcon.Init(unit.unit.kind_GearKind);
    this.txt_CharacterName.SetTextLocalize(unit.unit.name);
    this.txt_Lv.SetTextLocalize(unit.level);
    this.setColordText(this.txt_Fighting, u.Combat, 0);
    this.setColordText_BeforeStringNoColorChange(this.txt_Hpmax, u.Hp, 0, "/");
    this.txt_Jobname.SetTextLocalize(unit.unit.job.name);
    this.setColordText(this.txt_Movement, u.Move, 0);
    this.setColordText(this.txt_Attack, u.PhysicalAttack, 0);
    this.setColordText(this.txt_Defense, u.PhysicalDefense, 0);
    this.setColordText(this.txt_Matk, u.MagicAttack, 0);
    this.setColordText(this.txt_Mdef, u.MagicDefense, 0);
    this.setColordText(this.txt_Dexterity, u.Hit, 0);
    this.setColordText(this.txt_Evasion, u.Evasion, 0);
    this.setColordText(this.txt_Critical, u.Critical, 0);
    this.txt_Hp.SetTextLocalize(u.Hp);
    this.hpGauge.setValue(u.Hp, u.Hp, false);
  }

  private IEnumerator ShowDetailStatusPopup()
  {
    GameObject popup = this.popupInfoPrefab.Clone();
    Quest0528PlayerStatusDetail menu = popup.GetComponent<Quest0528PlayerStatusDetail>();
    IEnumerator e = menu.ResourcePreLoad();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((UIRect) popup.gameObject.GetComponent<UIWidget>()).alpha = 0.0f;
    e = menu.Init(this.mUnit, this.forceID);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(false);
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    ((UIRect) popup.gameObject.GetComponent<UIWidget>()).alpha = 1f;
    popup.SetActive(true);
  }

  public void onButtonInfo() => this.StartCoroutine(this.ShowDetailStatusPopup());

  private void setColordText(UILabel label, int v, int bd, string before_string = "")
  {
    label.SetTextLocalize(before_string + (object) v);
    if (bd > 0)
      ((UIWidget) label).color = this.mGreen;
    else if (bd < 0)
      ((UIWidget) label).color = this.mRed;
    else
      ((UIWidget) label).color = this.mOrigin;
  }

  private void setColordText_BeforeStringNoColorChange(
    UILabel label,
    int v,
    int bd,
    string before_string = "")
  {
    ((UIWidget) label).color = this.mOrigin;
    string str = v.ToString();
    string text = bd <= 0 ? (bd >= 0 ? before_string + str : before_string + "[fa0000]" + str + "[-]") : before_string + "[00dc1e]" + str + "[-]";
    label.SetTextLocalize(text);
  }
}
