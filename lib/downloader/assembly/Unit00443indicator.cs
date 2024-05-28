// Decompiled with JetBrains decompiler
// Type: Unit00443indicator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using UniLinq;
using UnitStatusInformation;
using UnityEngine;

#nullable disable
public class Unit00443indicator : MonoBehaviour
{
  [SerializeField]
  protected UILabel TxtAttack;
  [SerializeField]
  protected UILabel TxtCritical;
  [SerializeField]
  protected UILabel TxtDefense;
  [SerializeField]
  protected UILabel TxtDexterity;
  [SerializeField]
  protected UILabel TxtEvasion;
  [SerializeField]
  protected UILabel TxtExp;
  [SerializeField]
  protected UILabel TxtMagicAttack;
  [SerializeField]
  protected UILabel TxtMagicDefense;
  [SerializeField]
  protected UILabel TxtRange;
  [SerializeField]
  protected UILabel TxtWait;
  [SerializeField]
  protected UILabel TxtWeapontype;
  [SerializeField]
  protected UISprite SlcGauge;
  [SerializeField]
  public GearKindIcon Weapon;
  [SerializeField]
  private SprGearAttack WeaponSpAttack;
  [SerializeField]
  private UILabel TxtWeaponAttack;
  [SerializeField]
  private SprGearElement WeaponSpElement;
  [SerializeField]
  private UILabel TxtWeaponElement;
  [SerializeField]
  protected GameObject SkillOne_Root;
  [SerializeField]
  protected UIWidget[] SkillOne_Object;
  [SerializeField]
  protected UIButton[] SkillOne_Buttons;
  [SerializeField]
  protected GameObject SkillTwo_Root;
  [SerializeField]
  protected UIWidget[] SkillTwo_Object;
  [SerializeField]
  protected UIButton[] SkillTwo_Buttons;
  [SerializeField]
  protected GameObject SkillArrow_Object;
  private GearGear current_;
  private GameObject WeaponSkillPrefab;
  private GameObject SkillDialog;
  private GameObject popupAttackPrefab;
  private GameObject popupElementPrefab;
  private GameObject commonElementIconPrefab;
  private const int maxExpGaugeWidth = 206;
  [SerializeField]
  private GameObject floatingSkillDialog;
  private Action<GearGearSkill, bool> showSkillDialog;
  private bool? isEarth_;

  private bool isEarth
  {
    get
    {
      return !this.isEarth_.HasValue ? (this.isEarth_ = new bool?(Singleton<NGGameDataManager>.GetInstance().IsEarth)).Value : this.isEarth_.Value;
    }
  }

  public IEnumerator LoadPrefabs()
  {
    Future<GameObject> loader;
    IEnumerator e;
    if (Object.op_Equality((Object) this.SkillDialog, (Object) null))
    {
      loader = this.isEarth ? Res.Prefabs.battle017_11_1_1.SkillDetailDialog.Load<GameObject>() : PopupSkillDetails.createPrefabLoader(false);
      e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.SkillDialog = loader.Result;
      loader = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.WeaponSkillPrefab, (Object) null))
    {
      loader = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
      e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.WeaponSkillPrefab = loader.Result;
      loader = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.popupAttackPrefab, (Object) null))
    {
      loader = PopupAttackClassDetail.createPrefabLoader();
      e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.popupAttackPrefab = loader.Result;
      loader = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.popupElementPrefab, (Object) null))
    {
      loader = PopupGearAttachedElementDetail.createPrefabLoader();
      e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.popupElementPrefab = loader.Result;
      loader = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.commonElementIconPrefab, (Object) null))
    {
      loader = Res.Icons.CommonElementIcon.Load<GameObject>();
      e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.commonElementIconPrefab = loader.Result;
      loader = (Future<GameObject>) null;
    }
  }

  public virtual void Init(GearGear gear)
  {
    this.SetParam(gear);
    this.SetSkillDeteilEvent(gear);
    this.SetWeaponAttack(gear);
    this.SetWeaponElement(gear);
    this.Weapon.Init(gear.kind_GearKind, gear.GetElement());
    if (!Object.op_Inequality((Object) this.SlcGauge, (Object) null))
      return;
    ((Component) this.SlcGauge).gameObject.SetActive(false);
  }

  public void Init(GameCore.ItemInfo targetGear, GameCore.ItemInfo targetReisou)
  {
    Judgement.GearParameter gearParameter = Judgement.GearParameter.FromPlayerGear(targetGear);
    if (targetReisou != null)
    {
      Judgement.GearParameter rhs = Judgement.GearParameter.FromPlayerGear(targetReisou);
      gearParameter = Judgement.GearParameter.AddReisou(gearParameter, rhs);
    }
    this.SetParam(targetGear.gear, gearParameter, targetGear.gearLevel, targetGear.gearLevelLimit);
    this.SetSkillDeteilEvent(targetGear);
    this.SetWeaponAttack(targetGear.gear);
    this.SetWeaponElement(targetGear.gear);
    this.Weapon.Init(targetGear.gear.kind_GearKind, targetGear.GetElement());
    float num = 206f * ((float) targetGear.gearExp / (float) (targetGear.gearExpNext + targetGear.gearExp));
    if ((double) num == 0.0 || targetGear.gearExpNext + targetGear.gearExp == 0)
    {
      ((Component) this.SlcGauge).gameObject.SetActive(false);
    }
    else
    {
      ((Component) this.SlcGauge).gameObject.SetActive(true);
      ((UIWidget) this.SlcGauge).width = (int) num;
    }
  }

  protected void SetParam(
    GearGear gear,
    Judgement.GearParameter gearParam = null,
    int gear_level = 1,
    int gear_level_limit = 5)
  {
    this.current_ = gear;
    if (gearParam == null)
      gearParam = new Judgement.GearParameter();
    if (gear.attack_type == GearAttackType.magic)
    {
      this.SetParam(this.TxtAttack, 0, gearParam.PhysicalPower);
      this.SetParam(this.TxtMagicAttack, gear.power, gearParam.MagicalPower);
    }
    else if (gear.attack_type == GearAttackType.physical)
    {
      this.SetParam(this.TxtAttack, gear.power, gearParam.PhysicalPower);
      this.SetParam(this.TxtMagicAttack, 0, gearParam.MagicalPower);
    }
    else
    {
      this.SetParam(this.TxtAttack, 0, gearParam.PhysicalPower);
      this.SetParam(this.TxtMagicAttack, 0, gearParam.MagicalPower);
    }
    this.SetParam(this.TxtCritical, gear.critical, gearParam.Critical);
    this.SetParam(this.TxtDexterity, gear.hit, gearParam.Hit);
    this.SetParam(this.TxtDefense, gear.physical_defense, gearParam.PhysicalDefense);
    this.SetParam(this.TxtMagicDefense, gear.magic_defense, gearParam.MagicDefense);
    this.SetParam(this.TxtEvasion, gear.evasion, gearParam.Evasion);
    this.TxtRange.SetTextLocalize(gear.min_range.ToString() + "-" + gear.max_range.ToString());
    this.TxtWait.SetTextLocalize(gear.weight);
    if (Object.op_Inequality((Object) this.TxtExp, (Object) null))
      this.TxtExp.SetTextLocalize(Consts.Format(Consts.GetInstance().BUGU_0059_RANK, (IDictionary) new Hashtable()
      {
        {
          (object) "now",
          (object) gear_level
        },
        {
          (object) "max",
          (object) gear_level_limit
        }
      }));
    this.TxtWeapontype.SetText(gear.gearClassification?.name ?? "-");
  }

  private void SetParam(UILabel label, int baseParam, int rankParam)
  {
    if (baseParam < rankParam)
    {
      label.SetTextLocalize(rankParam);
      ((UIWidget) label).color = Color.green;
    }
    else
    {
      label.SetTextLocalize(baseParam);
      ((UIWidget) label).color = Color.white;
    }
  }

  protected void SetSkillDialog(UIButton button, GearGearSkill skill_data, bool isRelease)
  {
    if (this.showSkillDialog == null)
    {
      if (this.isEarth)
      {
        Battle0171111Event dialog = this.SkillDialog.Clone(this.floatingSkillDialog.transform).GetComponentInChildren<Battle0171111Event>();
        ((Component) ((Component) dialog).transform.parent).gameObject.SetActive(false);
        this.showSkillDialog = (Action<GearGearSkill, bool>) ((skill, release) =>
        {
          dialog.setData(skill.skill);
          if (release)
            dialog.setSkillLv(skill.skill_level, skill.skill.upper_level);
          else
            dialog.setSkillLv(0, skill.skill.upper_level);
          dialog.Show();
        });
      }
      else
        this.showSkillDialog = (Action<GearGearSkill, bool>) ((skill, release) => PopupSkillDetails.show(this.SkillDialog, new PopupSkillDetails.Param(skill.skill, UnitParameter.SkillGroup.Equip, release ? new int?(skill.skill_level) : new int?())));
    }
    EventDelegate.Set(button.onClick, (EventDelegate.Callback) (() => this.showSkillDialog(skill_data, isRelease)));
  }

  protected void SetSkillDeteilEvent(GearGear gear)
  {
    UIWidget[] uiWidgetArray = this.SkillOne_Object;
    UIButton[] uiButtonArray = this.SkillOne_Buttons;
    this.SkillOne_Root.SetActive(true);
    this.SkillTwo_Root.SetActive(false);
    if (gear.rememberSkills.Count > 0)
    {
      int num = gear.rememberSkills.Count > 1 ? 1 : 0;
      bool flag = gear.rememberSkills[0].Count > 1;
      if ((num | (flag ? 1 : 0)) != 0)
      {
        uiWidgetArray = this.SkillTwo_Object;
        uiButtonArray = this.SkillTwo_Buttons;
        this.SkillOne_Root.SetActive(false);
        this.SkillTwo_Root.SetActive(true);
      }
      if (num != 0)
      {
        for (int index = 0; index < gear.rememberSkills.Count && uiWidgetArray.Length > index; ++index)
        {
          BattleSkillIcon battleSkillIcon = ((Component) uiWidgetArray[index]).GetComponentInChildren<BattleSkillIcon>();
          if (Object.op_Equality((Object) battleSkillIcon, (Object) null))
            battleSkillIcon = this.WeaponSkillPrefab.Clone(((Component) uiWidgetArray[index]).transform).GetComponent<BattleSkillIcon>();
          battleSkillIcon.SetDepth(uiWidgetArray[index].depth + 1);
          if (gear.rememberSkills[index][0].release_rank > 1)
            battleSkillIcon.EnableNeedRankIcon(gear.rememberSkills[index][0].release_rank);
          else
            battleSkillIcon.EnableNeedRankIcon(0);
          this.StartCoroutine(battleSkillIcon.Init(gear.rememberSkills[index][0].skill));
          this.SetSkillDialog(uiButtonArray[index], gear.rememberSkills[index][0], false);
        }
      }
      else
      {
        for (int index = 0; index < gear.rememberSkills[0].Count && uiWidgetArray.Length > index; ++index)
        {
          BattleSkillIcon battleSkillIcon = ((Component) uiWidgetArray[index]).GetComponentInChildren<BattleSkillIcon>();
          if (Object.op_Equality((Object) battleSkillIcon, (Object) null))
            battleSkillIcon = this.WeaponSkillPrefab.Clone(((Component) uiWidgetArray[index]).transform).GetComponent<BattleSkillIcon>();
          battleSkillIcon.SetDepth(uiWidgetArray[index].depth + 1);
          if (gear.rememberSkills[0][index].release_rank > 1)
            battleSkillIcon.EnableNeedRankIcon(gear.rememberSkills[0][index].release_rank);
          else
            battleSkillIcon.EnableNeedRankIcon(0);
          this.StartCoroutine(battleSkillIcon.Init(gear.rememberSkills[0][index].skill));
          this.SetSkillDialog(uiButtonArray[index], gear.rememberSkills[0][index], false);
        }
      }
      this.SkillArrow_Object.SetActive(flag);
    }
    else
    {
      for (int index = 0; index < uiButtonArray.Length; ++index)
        uiButtonArray[index].onClick.Clear();
      for (int index = 0; index < uiWidgetArray.Length; ++index)
      {
        foreach (Component componentsInChild in ((Component) uiWidgetArray[index]).GetComponentsInChildren<BattleSkillIcon>(true))
          Object.Destroy((Object) componentsInChild.gameObject);
      }
    }
  }

  protected void SetSkillDeteilEvent(GameCore.ItemInfo gear)
  {
    UIWidget[] uiWidgetArray = this.SkillOne_Object;
    UIButton[] uiButtonArray = this.SkillOne_Buttons;
    this.SkillOne_Root.SetActive(true);
    this.SkillTwo_Root.SetActive(false);
    GearGear gear1 = gear.gear;
    if (gear1.rememberSkills.Count > 0)
    {
      GearGearSkill[] skills = gear.skills;
      int num = gear1.rememberSkills.Count > skills.Length ? 1 : 0;
      bool flag = num == 0 && gear1.rememberSkills[0].Count > 0 && !gear1.rememberSkills[0].All<GearGearSkill>((Func<GearGearSkill, bool>) (x => x.isReleased(gear)));
      if (((num | (flag ? 1 : 0)) != 0 || gear.skills.Length > 1) && (gear1.rememberSkills.Count > 1 || gear1.rememberSkills.Count == 1 && gear1.rememberSkills[0].Count > 1))
      {
        uiWidgetArray = this.SkillTwo_Object;
        uiButtonArray = this.SkillTwo_Buttons;
        this.SkillOne_Root.SetActive(false);
        this.SkillTwo_Root.SetActive(true);
      }
      if (num != 0)
      {
        for (int index = 0; index < gear1.rememberSkills.Count && uiWidgetArray.Length > index; ++index)
        {
          BattleSkillIcon battleSkillIcon = ((Component) uiWidgetArray[index]).GetComponentInChildren<BattleSkillIcon>();
          if (Object.op_Equality((Object) battleSkillIcon, (Object) null))
            battleSkillIcon = this.WeaponSkillPrefab.Clone(((Component) uiWidgetArray[index]).transform).GetComponent<BattleSkillIcon>();
          battleSkillIcon.SetDepth(uiWidgetArray[index].depth + 1);
          if (gear1.rememberSkills[index][0].release_rank > gear.gearLevel)
            battleSkillIcon.EnableNeedRankIcon(gear1.rememberSkills[index][0].release_rank);
          else
            battleSkillIcon.EnableNeedRankIcon(0);
          this.StartCoroutine(battleSkillIcon.Init(gear1.rememberSkills[index][0].skill));
          this.SetSkillDialog(uiButtonArray[index], gear1.rememberSkills[index][0], gear1.rememberSkills[index][0].isReleased(gear));
        }
      }
      else if (flag)
      {
        for (int index = 0; index < gear1.rememberSkills[0].Count && uiWidgetArray.Length > index; ++index)
        {
          BattleSkillIcon component = this.WeaponSkillPrefab.Clone(((Component) uiWidgetArray[index]).transform).GetComponent<BattleSkillIcon>();
          if (gear1.rememberSkills[0][index].release_rank > gear.gearLevel)
            component.EnableNeedRankIcon(gear1.rememberSkills[0][index].release_rank);
          else
            component.EnableNeedRankIcon(0);
          component.SetDepth(uiWidgetArray[index].depth + 1);
          this.StartCoroutine(component.Init(gear1.rememberSkills[0][index].skill));
          this.SetSkillDialog(uiButtonArray[index], gear1.rememberSkills[0][index], gear1.rememberSkills[0][index].isReleased(gear));
        }
      }
      else
      {
        for (int index = 0; index < gear.skills.Length && uiWidgetArray.Length > index; ++index)
        {
          BattleSkillIcon battleSkillIcon = ((Component) uiWidgetArray[index]).GetComponentInChildren<BattleSkillIcon>();
          if (Object.op_Equality((Object) battleSkillIcon, (Object) null))
            battleSkillIcon = this.WeaponSkillPrefab.Clone(((Component) uiWidgetArray[index]).transform).GetComponent<BattleSkillIcon>();
          battleSkillIcon.SetDepth(uiWidgetArray[index].depth + 1);
          battleSkillIcon.DisableNeedDisp();
          this.StartCoroutine(battleSkillIcon.Init(gear.skills[index].skill));
          this.SetSkillDialog(uiButtonArray[index], gear.skills[index], true);
        }
      }
      this.SkillArrow_Object.SetActive(flag);
    }
    else
    {
      for (int index = 0; index < uiButtonArray.Length; ++index)
        uiButtonArray[index].onClick.Clear();
      for (int index = 0; index < uiWidgetArray.Length; ++index)
      {
        foreach (Component componentsInChild in ((Component) uiWidgetArray[index]).GetComponentsInChildren<BattleSkillIcon>(true))
          Object.Destroy((Object) componentsInChild.gameObject);
      }
    }
  }

  protected void SetWeaponAttack(GearGear gear)
  {
    if (Object.op_Equality((Object) this.WeaponSpAttack, (Object) null))
      return;
    ((Component) this.WeaponSpAttack).gameObject.SetActive(gear.kind.is_attack);
    ((Component) this.TxtWeaponAttack).gameObject.SetActive(gear.hasAttackClass);
    if (gear.kind.is_attack && gear.hasAttackClass)
    {
      this.WeaponSpAttack.InitGearAttackID((int) gear.gearClassification.attack_classification);
      this.TxtWeaponAttack.SetText(gear.attackClassificationName);
    }
    else
      this.TxtWeaponAttack.SetText("");
  }

  protected void SetWeaponElement(GearGear gear)
  {
    if (Object.op_Equality((Object) this.WeaponSpElement, (Object) null))
      return;
    ((Component) this.WeaponSpElement).gameObject.SetActive(gear.kind.is_attack);
    ((Component) this.TxtWeaponElement).gameObject.SetActive(gear.kind.is_attack);
    if (!gear.kind.is_attack && !gear.hasAttackClass)
      return;
    this.WeaponSpElement.Initialize(this.commonElementIconPrefab.GetComponent<CommonElementIcon>(), gear.attachedElement);
    this.TxtWeaponElement.SetText(CommonElementName.GetName((int) gear.attachedElement));
  }

  public void OpenPopupWeaponAttack()
  {
    this.StartCoroutine(PopupAttackClassDetail.show(this.popupAttackPrefab, this.current_));
  }

  public void OpenPopupWeaponElement()
  {
    this.StartCoroutine(PopupGearAttachedElementDetail.show(this.popupElementPrefab, this.current_));
  }

  public virtual void SetUnit(int num)
  {
  }
}
