// Decompiled with JetBrains decompiler
// Type: BattleHealCharacterInfoHealer
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
public class BattleHealCharacterInfoHealer : BattleHealCharacterInfoBase
{
  [SerializeField]
  private GameObject selectCommand;
  [SerializeField]
  protected UI2DSprite iconGear;
  [SerializeField]
  protected UI2DSprite iconSecondGear;
  [SerializeField]
  private NGHorizontalScrollParts indicator;
  [SerializeField]
  protected UILabel TxtWeaponName;
  [SerializeField]
  protected UILabel TxtSecondWeaponName;
  [SerializeField]
  protected UILabel TxtHealAmount;
  [SerializeField]
  private BattleHealCharacterInfoInjured mInjured;
  [SerializeField]
  private GameObject mDotContainer;
  private GameObject commandPrefab;
  private GameObject gearIconPrefab;
  protected AttackStatus[] magicBullets;
  [SerializeField]
  private Battle019_6_1_RecoveryButton recoveryButton;
  [SerializeField]
  protected TweenAlpha tweenAlpha_first_weapon;
  [SerializeField]
  protected TweenAlpha tweenAlpha_second_weapon;

  public override IEnumerator Init(BL.UnitPosition up, AttackStatus[] attacks)
  {
    BattleHealCharacterInfoHealer characterInfoHealer = this;
    Future<GameObject> commandPrefabF = !Singleton<NGBattleManager>.GetInstance().isSea ? Res.Prefabs.battleUI_04.command.Load<GameObject>() : Res.Prefabs.battleUI_04_sea.command_sea.Load<GameObject>();
    IEnumerator e = commandPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    characterInfoHealer.commandPrefab = commandPrefabF.Result;
    if (up == null)
    {
      Debug.LogWarning((object) "unit is null");
    }
    else
    {
      characterInfoHealer.magicBullets = ((IEnumerable<AttackStatus>) attacks).Where<AttackStatus>((Func<AttackStatus, bool>) (x => x.magicBullet != null && x.magicBullet.isHeal)).ToArray<AttackStatus>();
      // ISSUE: reference to a compiler-generated method
      e = characterInfoHealer.\u003C\u003En__0(up, attacks);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      characterInfoHealer.indicator.destroyParts();
      characterInfoHealer.indicator.resetScrollView();
      characterInfoHealer.setCurrentAttack(((IEnumerable<AttackStatus>) attacks).Any<AttackStatus>() ? attacks[0] : (AttackStatus) null);
      characterInfoHealer.setHPNumbers((characterInfoHealer.currentUnit.unit.hp - characterInfoHealer.getCurrentAttack().magicBullet.cost).ToString());
      Future<GameObject> loader = Res.Icons.GearKindIcon.Load<GameObject>();
      e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      characterInfoHealer.gearIconPrefab = loader.Result;
      if (Object.op_Inequality((Object) null, (Object) characterInfoHealer.gearIconPrefab))
      {
        GameObject gameObject1 = characterInfoHealer.gearIconPrefab.Clone(((Component) characterInfoHealer.iconGear).transform);
        ((UIWidget) gameObject1.GetComponent<UI2DSprite>()).depth = ((UIWidget) characterInfoHealer.iconGear).depth + 1;
        gameObject1.GetComponent<GearKindIcon>().Init(characterInfoHealer.currentUnit.unit.unit.kind);
        if (((Component) characterInfoHealer.tweenAlpha_second_weapon).gameObject.activeSelf)
        {
          GameObject gameObject2 = characterInfoHealer.gearIconPrefab.Clone(((Component) characterInfoHealer.iconSecondGear).transform);
          ((UIWidget) gameObject2.GetComponent<UI2DSprite>()).depth = ((UIWidget) characterInfoHealer.iconSecondGear).depth + 1;
          GearKindIcon component = gameObject2.GetComponent<GearKindIcon>();
          CommonElement commonElement = characterInfoHealer.currentUnit.unit.playerUnit.equippedGear2 != (PlayerItem) null ? characterInfoHealer.currentUnit.unit.playerUnit.equippedGear2.GetElement() : characterInfoHealer.currentUnit.unit.playerUnit.equippedGearOrInitial.GetElement();
          GearKind kind = characterInfoHealer.currentUnit.unit.playerUnit.equippedGear2OrInitial.kind;
          int element = (int) commonElement;
          component.Init(kind, (CommonElement) element);
        }
      }
      if (((IEnumerable<AttackStatus>) characterInfoHealer.magicBullets).Any<AttackStatus>())
      {
        characterInfoHealer.setCurrentAttack(characterInfoHealer.magicBullets[0]);
        characterInfoHealer.selectCommand.SetActive(true);
        AttackStatus[] attackStatusArray = characterInfoHealer.magicBullets;
        for (int index = 0; index < attackStatusArray.Length; ++index)
        {
          AttackStatus attack = attackStatusArray[index];
          BattleskillGenre? nullable = attack.magicBullet.skill.genre1;
          BattleskillGenre battleskillGenre1 = BattleskillGenre.heal;
          if (!(nullable.GetValueOrDefault() == battleskillGenre1 & nullable.HasValue))
          {
            nullable = attack.magicBullet.skill.genre2;
            BattleskillGenre battleskillGenre2 = BattleskillGenre.heal;
            if (!(nullable.GetValueOrDefault() == battleskillGenre2 & nullable.HasValue))
              continue;
          }
          yield return (object) characterInfoHealer.indicator.instantiateParts(characterInfoHealer.commandPrefab).GetComponent<BattleUI04CommandPrefab>().Init(attack, up.unit);
        }
        attackStatusArray = (AttackStatus[]) null;
        if (((IEnumerable<AttackStatus>) characterInfoHealer.magicBullets).Where<AttackStatus>((Func<AttackStatus, bool>) (x => x.isHeal)).Count<AttackStatus>() <= 1)
        {
          if (Object.op_Inequality((Object) null, (Object) characterInfoHealer.mDotContainer))
            characterInfoHealer.mDotContainer.SetActive(false);
        }
        else if (Object.op_Inequality((Object) null, (Object) characterInfoHealer.mDotContainer))
        {
          characterInfoHealer.mDotContainer.SetActive(true);
          Vector3 localPosition = characterInfoHealer.mDotContainer.transform.localPosition;
          localPosition.y = -32f;
          characterInfoHealer.mDotContainer.transform.localPosition = localPosition;
        }
      }
      else
        characterInfoHealer.selectCommand.SetActive(false);
      characterInfoHealer.indicator.resetScrollView();
    }
  }

  public void onItemChanged()
  {
    if (this.magicBullets == null || this.magicBullets.Length == 0)
      return;
    int selected = this.indicator.selected;
    if (selected < 0 || selected >= this.magicBullets.Length)
      Debug.LogError((object) ("bug, illegal indicator index=" + (object) selected));
    else
      this.setCurrentAttack(this.magicBullets[selected]);
  }

  protected void setCurrentAttack(AttackStatus attackStatus)
  {
    this.currentAttack = attackStatus;
    if (this.currentAttack == null)
    {
      ((Component) this.iconGear).gameObject.SetActive(false);
      ((Component) this.iconSecondGear).gameObject.SetActive(false);
      this.TxtWeaponName.SetText("-");
      this.TxtSecondWeaponName.SetText("-");
      this.TxtHealAmount.SetText("-");
    }
    else
    {
      GearGear gear = this.currentUnit.unit.weapon.gear;
      ((Component) this.iconGear).gameObject.SetActive(true);
      this.TxtWeaponName.SetTextLocalize(this.currentUnit.unit.playerUnit.equippedGearName);
      int num = !Object.op_Inequality((Object) null, (Object) this.mInjured) || this.mInjured.getCurrent().unit.CanHeal(this.currentAttack.magicBullet.skill.skill_type) ? this.currentAttack.healAttack((BL.ISkillEffectListUnit) this.currentUnit.unit, Object.op_Inequality((Object) this.mInjured, (Object) null) ? (BL.ISkillEffectListUnit) this.mInjured.getCurrent().unit : (BL.ISkillEffectListUnit) null) : 0;
      this.TxtHealAmount.SetTextLocalize(num);
      if (Object.op_Inequality((Object) null, (Object) this.mInjured))
        this.mInjured.setCurrentHP(num);
      bool flag = this.currentUnit.unit.unit.awake_unit_flag && this.currentUnit.unit.playerUnit.equippedGear2 != (PlayerItem) null;
      ((Component) this.tweenAlpha_second_weapon).gameObject.SetActive(flag);
      ((Behaviour) this.tweenAlpha_first_weapon).enabled = false;
      ((Behaviour) this.tweenAlpha_second_weapon).enabled = false;
      if (flag)
      {
        this.TxtSecondWeaponName.SetTextLocalize(this.currentUnit.unit.playerUnit.equippedGearName2);
        ((UITweener) this.tweenAlpha_first_weapon).ResetToBeginning();
        ((UITweener) this.tweenAlpha_second_weapon).ResetToBeginning();
        this.StartCoroutine(this.StartWeaponNameAnim());
      }
      else
      {
        ((UIRect) ((Component) this.tweenAlpha_first_weapon).GetComponent<UIWidget>()).alpha = 1f;
        ((UIRect) ((Component) this.tweenAlpha_second_weapon).GetComponent<UIWidget>()).alpha = 0.0f;
      }
    }
    this.recoveryButton.setAttackStatus(this.currentAttack, (BattleHealCharacterInfoBase) this);
    BL.Unit unit = this.currentUnit.unit;
    this.hpBar.setValue(unit.hp - this.currentAttack.magicBullet.cost, unit.parameter.Hp, false);
    this.setHPNumbers((unit.hp - this.currentAttack.magicBullet.cost).ToString());
    this.consumeBar.setValue(unit.hp, unit.parameter.Hp, false);
  }

  protected override ResourceObject maskResource() => Res.GUI._009_3_sozai.mask_Chara_L;

  private IEnumerator StartWeaponNameAnim()
  {
    yield return (object) new WaitForSeconds(0.5f);
    ((UITweener) this.tweenAlpha_first_weapon).PlayForward();
    ((UITweener) this.tweenAlpha_second_weapon).PlayForward();
  }
}
