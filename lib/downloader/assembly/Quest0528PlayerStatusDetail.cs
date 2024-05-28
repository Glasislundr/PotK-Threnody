// Decompiled with JetBrains decompiler
// Type: Quest0528PlayerStatusDetail
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
public class Quest0528PlayerStatusDetail : BackButtonMenuBase
{
  private const int WEAPON_SKILL_ICON_ID = 10;
  private const int WEAPON_ELEMENT_ICON_ID = 0;
  private const string MAGIC_BULLET_NONE_NAME = "-";
  private static Dictionary<PlayerUnit, Sprite> spriteCache = new Dictionary<PlayerUnit, Sprite>();
  [SerializeField]
  protected NGTweenGaugeScale hpGauge;
  [SerializeField]
  protected GameObject[] dir_ForceHeader;
  [SerializeField]
  protected NGxMaskSprite link_CharacterMask;
  [SerializeField]
  protected Transform characterTransform;
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
  protected UILabel txt_Fighting;
  [SerializeField]
  protected UILabel txt_Matk;
  [SerializeField]
  protected UILabel txt_Mdef;
  [SerializeField]
  protected UILabel txt_Movement;
  [SerializeField]
  protected UILabel txt_Agility;
  [SerializeField]
  protected UILabel txt_Luck;
  [SerializeField]
  protected UILabel txt_Magic;
  [SerializeField]
  protected UILabel txt_Power;
  [SerializeField]
  protected UILabel txt_Stability;
  [SerializeField]
  protected UILabel txt_Spirit;
  [SerializeField]
  protected UILabel txt_Technique;
  [SerializeField]
  protected UILabel txt_CharacterName;
  [SerializeField]
  protected UILabel txt_Lv;
  [SerializeField]
  protected UILabel txt_Hp;
  [SerializeField]
  protected UILabel txt_Hpmax;
  [SerializeField]
  protected UILabel txt_Jobname;
  [SerializeField]
  protected Transform weaponGearKindIconParent;
  [SerializeField]
  protected Transform shieldGearKindIconParent;
  [SerializeField]
  protected Transform weaponEquipKindIconParent;
  [SerializeField]
  protected Transform[] skillTypeIconParent;
  [SerializeField]
  protected Transform[] elementTypeIconParent;
  [SerializeField]
  protected UILabel[] txt_Magic_name;
  [SerializeField]
  protected UILabel[] txt_Magic_range;
  [SerializeField]
  protected Transform[] spaTypeIconParent1;
  [SerializeField]
  protected Transform[] spaTypeIconParent2;
  [SerializeField]
  protected Transform gearProfiencyIconParentW;
  [SerializeField]
  protected Transform gearProfiencyIconParentS;
  [SerializeField]
  protected GameObject backGround;
  [SerializeField]
  protected GameObject LeaderSkillBtn;
  [SerializeField]
  protected GameObject SkillDialogRoot;
  [SerializeField]
  protected UISprite[] SkillIconBase;
  private GameObject gearKindIcon01;
  private GameObject gearKindIcon02;
  private GameObject weaponEquipIcon;
  private GameObject weaponSkillIcon;
  private GameObject skillDialog;
  private GameObject gearProfiencyIconW;
  private GameObject gearProfiencyIconS;
  private GameObject[] elementTypeIcon = new GameObject[5];
  private GameObject[] spaTypeIcon1 = new GameObject[5];
  private GameObject[] spaTypeIcon2 = new GameObject[5];
  private GameObject[] skillTypeIcon = new GameObject[12];
  private GameObject skillTypeIconPrefab;
  private GameObject skillDialogPrefab;
  private GameObject gearProfiencyIconPrefab;
  private GameObject elementTypeIconPrefab;
  private GameObject spaTypeIconPrefab;
  private GameObject gearKindIconPrefab;
  private List<PlayerUnitSkills> dispSkills;
  private List<PlayerUnitSkills> dispMagicBullets;
  private PlayerItem dispWeapon;
  private PlayerUnitLeader_skills dispLeaderSkill;

  public static void ClearCache() => Quest0528PlayerStatusDetail.spriteCache.Clear();

  public static IEnumerator LoadIcon(PlayerUnit v)
  {
    if (!Quest0528PlayerStatusDetail.spriteCache.ContainsKey(v))
    {
      Future<Sprite> fs = v.unit.LoadSpriteThumbnail();
      IEnumerator e = fs.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Quest0528PlayerStatusDetail.spriteCache.Add(v, fs.Result);
    }
  }

  public IEnumerator ResourcePreLoad()
  {
    Future<GameObject> gearKindIconPrefabF;
    IEnumerator e;
    if (Object.op_Equality((Object) this.gearKindIconPrefab, (Object) null))
    {
      gearKindIconPrefabF = Res.Icons.GearKindIcon.Load<GameObject>();
      e = gearKindIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.gearKindIconPrefab = gearKindIconPrefabF.Result;
      gearKindIconPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.skillTypeIconPrefab, (Object) null))
    {
      gearKindIconPrefabF = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
      e = gearKindIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.skillTypeIconPrefab = gearKindIconPrefabF.Result;
      gearKindIconPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.elementTypeIconPrefab, (Object) null))
    {
      gearKindIconPrefabF = Res.Icons.CommonElementIcon.Load<GameObject>();
      e = gearKindIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.elementTypeIconPrefab = gearKindIconPrefabF.Result;
      gearKindIconPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.spaTypeIconPrefab, (Object) null))
    {
      gearKindIconPrefabF = Res.Icons.SPAtkTypeIcon.Load<GameObject>();
      e = gearKindIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.spaTypeIconPrefab = gearKindIconPrefabF.Result;
      gearKindIconPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.gearProfiencyIconPrefab, (Object) null))
    {
      gearKindIconPrefabF = Res.Icons.GearProfiencyIcon.Load<GameObject>();
      e = gearKindIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.gearProfiencyIconPrefab = gearKindIconPrefabF.Result;
      gearKindIconPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.skillDialogPrefab, (Object) null))
    {
      gearKindIconPrefabF = Res.Prefabs.battle017_11_1_1.SkillDetailDialog.Load<GameObject>();
      e = gearKindIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.skillDialogPrefab = gearKindIconPrefabF.Result;
      gearKindIconPrefabF = (Future<GameObject>) null;
    }
    this.skillDialog = this.skillDialogPrefab.Clone(this.SkillDialogRoot.transform);
    this.skillDialog.GetComponentInChildren<UIPanel>().depth += 30;
    this.skillDialog.SetActive(false);
    this.gearKindIcon01 = this.CreateIcon(this.gearKindIconPrefab, this.weaponGearKindIconParent);
    this.gearKindIcon02 = this.CreateIcon(this.gearKindIconPrefab, this.shieldGearKindIconParent);
    this.weaponEquipIcon = this.CreateIcon(this.gearKindIconPrefab, this.weaponEquipKindIconParent);
    this.dispSkills = new List<PlayerUnitSkills>();
    this.dispWeapon = (PlayerItem) null;
    this.dispLeaderSkill = (PlayerUnitLeader_skills) null;
  }

  private GameObject CreateIcon(GameObject prefab, Transform trans)
  {
    trans.Clear();
    GameObject icon = prefab.Clone(trans);
    UI2DSprite componentInChildren1 = icon.GetComponentInChildren<UI2DSprite>();
    UI2DSprite componentInChildren2 = ((Component) trans).GetComponentInChildren<UI2DSprite>();
    ((UIWidget) componentInChildren1).SetDimensions(((UIWidget) componentInChildren2).width, ((UIWidget) componentInChildren2).height);
    ((UIWidget) componentInChildren1).depth = ((UIWidget) this.backGround.GetComponentInChildren<UISprite>()).depth + 100;
    return icon;
  }

  private IEnumerator CreateMaskdCharacter(PlayerUnit v)
  {
    Future<GameObject> future = v.unit.LoadMypage();
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.characterTransform.Clear();
    GameObject result = future.Result;
    if (!Object.op_Equality((Object) result, (Object) null))
    {
      int nextDepth = NGUITools.CalculateNextDepth(((Component) this.characterTransform).gameObject);
      GameObject go = result.Clone(this.characterTransform);
      go.GetComponent<NGxMaskSpriteWithScale>().scale = 0.6f;
      e = v.unit.SetLargeSpriteWithMask(go, Res.GUI.battleUI_03.mask_Character.Load<Texture2D>(), nextDepth, 190, 25);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private void SetSkillIconBaseSprite()
  {
    foreach (UISprite uiSprite in this.SkillIconBase)
      uiSprite.spriteName = "slc_skill_icon_base_unit_zero_60_62.png__GUI__common__common_prefab";
  }

  public IEnumerator Init(PlayerUnit unit, BL.ForceID forceID)
  {
    Quest0528PlayerStatusDetail playerStatusDetail = this;
    PlayerUnit playerUnit = unit;
    Judgement.NonBattleParameter nonBattleParameter = Judgement.NonBattleParameter.FromPlayerUnit(playerUnit);
    playerStatusDetail.SetSkillIconBaseSprite();
    ((IEnumerable<GameObject>) playerStatusDetail.dir_ForceHeader).ToggleOnce((int) forceID);
    playerStatusDetail.txt_Agility.SetTextLocalize(nonBattleParameter.Agility);
    playerStatusDetail.txt_Luck.SetTextLocalize(nonBattleParameter.Luck);
    playerStatusDetail.txt_Magic.SetTextLocalize(nonBattleParameter.Intelligence);
    playerStatusDetail.txt_Power.SetTextLocalize(nonBattleParameter.Strength);
    playerStatusDetail.txt_Stability.SetTextLocalize(nonBattleParameter.Vitality);
    playerStatusDetail.txt_Spirit.SetTextLocalize(nonBattleParameter.Mind);
    playerStatusDetail.txt_Technique.SetTextLocalize(nonBattleParameter.Dexterity);
    playerStatusDetail.weaponEquipIcon.GetComponentInChildren<GearKindIcon>().Init(playerUnit.equippedGearOrInitial.kind);
    playerStatusDetail.hpGauge.setValue(nonBattleParameter.Hp, nonBattleParameter.Hp, false);
    playerStatusDetail.txt_CharacterName.SetTextLocalize(playerUnit.unit.name);
    playerStatusDetail.txt_Lv.SetTextLocalize(playerUnit.level);
    playerStatusDetail.txt_Fighting.SetTextLocalize(nonBattleParameter.Combat);
    playerStatusDetail.txt_Hp.SetTextLocalize(nonBattleParameter.Hp);
    playerStatusDetail.txt_Hpmax.SetTextLocalize("/" + (object) nonBattleParameter.Hp);
    playerStatusDetail.txt_Jobname.SetTextLocalize(playerUnit.unit.job.name);
    playerStatusDetail.txt_Movement.SetTextLocalize(nonBattleParameter.Move);
    playerStatusDetail.txt_Attack.SetTextLocalize(nonBattleParameter.PhysicalAttack);
    playerStatusDetail.txt_Critical.SetTextLocalize(nonBattleParameter.Critical);
    playerStatusDetail.txt_Defense.SetTextLocalize(nonBattleParameter.PhysicalDefense);
    playerStatusDetail.txt_Dexterity.SetTextLocalize(nonBattleParameter.Hit);
    playerStatusDetail.txt_Evasion.SetTextLocalize(nonBattleParameter.Evasion);
    playerStatusDetail.txt_Matk.SetTextLocalize(nonBattleParameter.MagicAttack);
    playerStatusDetail.txt_Mdef.SetTextLocalize(nonBattleParameter.MagicDefense);
    if (playerUnit.skills != null)
    {
      PlayerUnitSkills[] array = ((IEnumerable<PlayerUnitSkills>) playerUnit.skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.skill.skill_type != BattleskillSkillType.magic & x.skill.skill_type != BattleskillSkillType.leader)).ToArray<PlayerUnitSkills>();
      int num1 = playerStatusDetail.skillTypeIcon.Length - 1;
      int num2 = array.Length < num1 ? array.Length : num1;
      for (int index = 0; index < num2; ++index)
      {
        PlayerUnitSkills playerUnitSkills = array[index];
        playerStatusDetail.dispSkills.Add(playerUnitSkills);
        playerStatusDetail.skillTypeIcon[index] = playerStatusDetail.CreateIcon(playerStatusDetail.skillTypeIconPrefab, playerStatusDetail.skillTypeIconParent[index]);
        playerStatusDetail.StartCoroutine(playerStatusDetail.skillTypeIcon[index].GetComponentInChildren<BattleSkillIcon>().Init(playerUnitSkills.skill));
      }
    }
    if (playerUnit.equippedGear != (PlayerItem) null)
    {
      for (int index = 0; index < playerUnit.equippedGear.skills.Length; ++index)
      {
        playerStatusDetail.skillTypeIcon[10 + index] = playerStatusDetail.CreateIcon(playerStatusDetail.skillTypeIconPrefab, playerStatusDetail.skillTypeIconParent[10 + index]);
        playerStatusDetail.StartCoroutine(playerStatusDetail.skillTypeIcon[10 + index].GetComponentInChildren<BattleSkillIcon>().Init(playerUnit.equippedGear.skills[index].skill));
        playerStatusDetail.dispWeapon = playerUnit.equippedGear;
      }
    }
    if (playerUnit.equippedGearOrInitial != null)
    {
      playerStatusDetail.elementTypeIcon[0] = playerStatusDetail.CreateIcon(playerStatusDetail.elementTypeIconPrefab, playerStatusDetail.elementTypeIconParent[0]);
      if (playerUnit.equippedGearOrInitial.elements.Length != 0)
        playerStatusDetail.elementTypeIcon[0].GetComponentInChildren<CommonElementIcon>().Init(playerUnit.equippedGearOrInitial.elements[0].element);
      else
        playerStatusDetail.elementTypeIcon[0].GetComponentInChildren<CommonElementIcon>().Init(CommonElement.none);
      playerStatusDetail.txt_Magic_name[0].SetTextLocalize(playerUnit.equippedGearName);
      GearGear equippedGearOrInitial = playerUnit.equippedGearOrInitial;
      playerStatusDetail.txt_Magic_range[0].SetTextLocalize(string.Format("{0} - {1}", (object) equippedGearOrInitial.min_range, (object) equippedGearOrInitial.max_range));
      UnitFamily[] specialAttackTargets = playerUnit.equippedWeaponGearOrInitial.SpecialAttackTargets;
      Transform[] transformArray = new Transform[2]
      {
        playerStatusDetail.spaTypeIconParent1[0],
        playerStatusDetail.spaTypeIconParent2[0]
      };
      GameObject[] gameObjectArray = new GameObject[2]
      {
        playerStatusDetail.spaTypeIcon1[0],
        playerStatusDetail.spaTypeIcon2[0]
      };
      for (int index = 0; index < specialAttackTargets.Length && transformArray.Length > index; ++index)
      {
        gameObjectArray[index] = playerStatusDetail.CreateIcon(playerStatusDetail.spaTypeIconPrefab, transformArray[index]);
        gameObjectArray[index].GetComponentInChildren<SPAtkTypeIcon>().InitKindId(specialAttackTargets[index]);
      }
    }
    if (playerUnit.skills != null)
    {
      playerStatusDetail.dispMagicBullets = new List<PlayerUnitSkills>();
      for (int index = 0; index < 4; ++index)
      {
        playerStatusDetail.txt_Magic_name[index + 1].SetTextLocalize("-");
        playerStatusDetail.txt_Magic_range[index + 1].SetTextLocalize("-");
      }
      int num = 0;
      foreach (PlayerUnitSkills skill in playerUnit.skills)
      {
        if (skill.skill.skill_type == BattleskillSkillType.magic)
        {
          playerStatusDetail.dispMagicBullets.Add(skill);
          playerStatusDetail.elementTypeIcon[num + 1] = playerStatusDetail.CreateIcon(playerStatusDetail.elementTypeIconPrefab, playerStatusDetail.elementTypeIconParent[num + 1]);
          playerStatusDetail.elementTypeIcon[num + 1].GetComponentInChildren<CommonElementIcon>().Init(skill.skill.element);
          playerStatusDetail.txt_Magic_name[num + 1].SetTextLocalize(skill.skill.name);
          playerStatusDetail.txt_Magic_range[num + 1].SetTextLocalize(string.Format("{0} - {1}", (object) skill.skill.min_range, (object) skill.skill.max_range));
          playerStatusDetail.spaTypeIcon1[num + 1] = playerStatusDetail.CreateIcon(playerStatusDetail.spaTypeIconPrefab, playerStatusDetail.spaTypeIconParent1[num + 1]);
          ++num;
        }
      }
    }
    bool isAllEquipUnit = unit.unit.IsAllEquipUnit;
    if (playerUnit.gear_proficiencies != null && playerUnit.gear_proficiencies.Length >= 1)
    {
      playerStatusDetail.gearKindIcon01.GetComponent<GearKindIcon>().Init(playerUnit.gear_proficiencies[0].gear_kind_id);
      playerStatusDetail.gearProfiencyIconW = playerStatusDetail.CreateIcon(playerStatusDetail.gearProfiencyIconPrefab, playerStatusDetail.gearProfiencyIconParentW);
      playerStatusDetail.gearProfiencyIconW.GetComponentInChildren<GearProfiencyIcon>().Init(playerUnit.gear_proficiencies[0].level, isAllEquipUnit);
    }
    if (playerUnit.gear_proficiencies != null && playerUnit.gear_proficiencies.Length >= 2)
    {
      playerStatusDetail.gearKindIcon02.GetComponent<GearKindIcon>().Init(playerUnit.gear_proficiencies[1].gear_kind_id);
      playerStatusDetail.gearProfiencyIconS = playerStatusDetail.CreateIcon(playerStatusDetail.gearProfiencyIconPrefab, playerStatusDetail.gearProfiencyIconParentS);
      playerStatusDetail.gearProfiencyIconS.GetComponentInChildren<GearProfiencyIcon>().Init(playerUnit.gear_proficiencies[1].level, isAllEquipUnit);
    }
    if (playerUnit.is_enemy && playerUnit.is_enemy_leader && playerUnit.leader_skill != null)
    {
      playerStatusDetail.dispLeaderSkill = playerUnit.leader_skill;
      playerStatusDetail.LeaderSkillBtn.SetActive(true);
    }
    else
      playerStatusDetail.LeaderSkillBtn.SetActive(false);
    IEnumerator e = playerStatusDetail.CreateMaskdCharacter(playerUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void OnButtonSkillProc(int idx)
  {
    if (Object.op_Equality((Object) this.skillTypeIcon[idx], (Object) null) || Object.op_Equality((Object) this.skillDialog, (Object) null))
      return;
    this.skillDialog.SetActive(true);
    Battle0171111Event componentInChildren = this.skillDialog.GetComponentInChildren<Battle0171111Event>();
    if (!Object.op_Inequality((Object) null, (Object) componentInChildren))
      return;
    componentInChildren.setSkillProperty(true);
    componentInChildren.setData(this.dispSkills[idx].skill);
    componentInChildren.setSkillLv(this.dispSkills[idx].level, this.dispSkills[idx].skill.upper_level);
    componentInChildren.Show();
  }

  public void onButtonSkill1() => this.OnButtonSkillProc(0);

  public void onButtonSkill2() => this.OnButtonSkillProc(1);

  public void onButtonSkill3() => this.OnButtonSkillProc(2);

  public void onButtonSkill4() => this.OnButtonSkillProc(3);

  public void onButtonSkill5() => this.OnButtonSkillProc(4);

  public void onButtonSkill6() => this.OnButtonSkillProc(5);

  public void onButtonSkill7() => this.OnButtonSkillProc(6);

  public void onButtonSkill8() => this.OnButtonSkillProc(7);

  public void onButtonSkillLeader()
  {
    if (this.dispLeaderSkill == null || Object.op_Equality((Object) this.skillDialog, (Object) null))
      return;
    this.skillDialog.SetActive(true);
    Battle0171111Event componentInChildren = this.skillDialog.GetComponentInChildren<Battle0171111Event>();
    if (!Object.op_Inequality((Object) null, (Object) componentInChildren))
      return;
    componentInChildren.setSkillProperty(true);
    componentInChildren.setData(this.dispLeaderSkill.skill);
    componentInChildren.setSkillLv(this.dispLeaderSkill.level, this.dispLeaderSkill.skill.upper_level);
    componentInChildren.Show();
  }

  public void onButtonSkillw(int id)
  {
    if (Object.op_Equality((Object) this.skillTypeIcon[10 + id], (Object) null) || Object.op_Equality((Object) this.skillDialog, (Object) null))
      return;
    this.skillDialog.SetActive(true);
    Battle0171111Event componentInChildren = this.skillDialog.GetComponentInChildren<Battle0171111Event>();
    if (!Object.op_Inequality((Object) null, (Object) componentInChildren))
      return;
    componentInChildren.setSkillProperty(true);
    componentInChildren.setData(this.dispWeapon.skills[id].skill);
    componentInChildren.setSkillLv(this.dispWeapon.skills[id].skill_level, this.dispWeapon.skills[id].skill.upper_level);
    componentInChildren.Show();
  }

  public void onButtonSkillw1() => this.onButtonSkillw(0);

  public void onButtonSkillw2() => this.onButtonSkillw(1);

  private void DispMagicBulletDetailDialog(int index)
  {
    if (index - 1 < 0 || this.dispMagicBullets.Count < index || Object.op_Equality((Object) this.skillDialog, (Object) null))
      return;
    this.skillDialog.SetActive(true);
    Battle0171111Event componentInChildren = this.skillDialog.GetComponentInChildren<Battle0171111Event>();
    if (!Object.op_Inequality((Object) null, (Object) componentInChildren))
      return;
    componentInChildren.setData(this.dispMagicBullets[index - 1].skill);
    componentInChildren.setSkillLv(this.dispMagicBullets[index - 1].level, this.dispMagicBullets[index - 1].skill.upper_level);
    componentInChildren.Show();
  }

  public void onButtonMB1() => this.DispMagicBulletDetailDialog(1);

  public void onButtonMB2() => this.DispMagicBulletDetailDialog(2);

  public void onButtonMB3() => this.DispMagicBulletDetailDialog(3);

  public void onButtonMB4() => this.DispMagicBulletDetailDialog(4);

  public void onClose() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.onClose();
}
