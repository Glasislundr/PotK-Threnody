// Decompiled with JetBrains decompiler
// Type: BattleUI04Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnitStatusInformation;
using UnityEngine;

#nullable disable
public class BattleUI04Menu : BattleBackButtonMenuBase
{
  [SerializeField]
  private BattlePrepareCharacterInfoPlayer player;
  [SerializeField]
  private BattlePrepareCharacterInfoEnemy enemy;
  [SerializeField]
  private NGHorizontalScrollParts indicator;
  [SerializeField]
  private UIButton attackButton;
  private bool isInitialized;
  private AttackStatus[] attackStatus;
  private int backSkillCursor;
  private BL.UnitPosition attack;
  private BL.UnitPosition defense;
  private GameObject skillDetailDialogPrefab;
  private GameObject ailmentsDetailPrefab;

  public IEnumerator LoadPrefab()
  {
    bool isSea = Singleton<NGBattleManager>.GetInstance().isSea;
    Future<GameObject> loader = PopupSkillDetails.createPrefabLoader(isSea);
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.skillDetailDialogPrefab = loader.Result;
    loader = Res.Prefabs.battle.popup_Ailments_detail__anim_popup01.Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.ailmentsDetailPrefab = loader.Result;
    e = this.player.LoadPrefab(isSea);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.enemy.LoadPrefab(isSea);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator Init(BL.UnitPosition attack, BL.UnitPosition defense)
  {
    BattleUI04Menu baseMenu = this;
    baseMenu.IsPush = true;
    baseMenu.isInitialized = false;
    baseMenu.indicator.SeEnable = false;
    baseMenu.backSkillCursor = -1;
    baseMenu.attack = attack;
    baseMenu.defense = defense;
    bool isFacility = attack.unit.isFacility || defense.unit.isFacility;
    baseMenu.attackStatus = BattleFuncs.getAttackStatusArray(attack, defense, true, false);
    IEnumerator e = baseMenu.player.Init(attack, baseMenu.attackStatus, isFacility, baseMenu);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    AttackStatus[] attackStatusArray = BattleFuncs.getAttackStatusArray(defense, attack, false, false);
    e = baseMenu.enemy.Init(defense, attackStatusArray, isFacility, baseMenu);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    baseMenu.player.SetCompatibility(defense.unit.playerUnit);
    baseMenu.enemy.SetCompatibility(attack.unit.playerUnit);
    baseMenu.player.SetDsInfo(baseMenu.enemy.getCurrent(), baseMenu.enemy.getCurrentAttack());
    baseMenu.enemy.SetDsInfo(baseMenu.player.getCurrent(), baseMenu.player.getCurrentAttack());
    ((UIButtonColor) baseMenu.attackButton).isEnabled = baseMenu.attackStatus.Length != 0;
    ((Component) baseMenu).GetComponent<UIPanel>().SetDirty();
  }

  public void onInitialized()
  {
    if (!this.battleManager.noDuelScene)
      Singleton<NGDuelDataManager>.GetInstance().StartBackGroundPreload(this.attack.unit, this.defense.unit);
    this.StartCoroutine(this.WaitScrollSe());
    this.isInitialized = true;
    this.IsPush = false;
  }

  protected override void Update_Battle()
  {
    if (!this.isInitialized || this.indicator.selected == -1 || this.backSkillCursor == this.indicator.selected || this.indicator.selected >= this.attackStatus.Length)
      return;
    this.player.setCurrentAttack(this.player.getAttackStatus(this.indicator.selected));
    this.player.SetDsInfo(this.enemy.getCurrent(), this.enemy.getCurrentAttack());
    this.enemy.SetDsInfo(this.player.getCurrent(), this.player.getCurrentAttack());
    this.backSkillCursor = this.indicator.selected;
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    if (!this.battleManager.noDuelScene)
    {
      Singleton<NGDuelDataManager>.GetInstance().StopBackGroundPreload();
      Singleton<NGDuelDataManager>.GetInstance().ClearOneTimeDuelCache();
    }
    this.backScene();
  }

  public void onAttackButton()
  {
    if (this.IsPushAndSet())
      return;
    AttackStatus currentAttack = this.player.getCurrentAttack();
    BL.UnitPosition current1 = this.player.getCurrent();
    BL.UnitPosition current2 = this.enemy.getCurrent();
    if (!this.battleManager.useGameEngine)
    {
      this.battleManager.startDuel(BattleFuncs.calcDuel(currentAttack, current1, current2));
    }
    else
    {
      int attackOriginIndex = this.player.getCurrentAttackOriginIndex();
      this.battleManager.gameEngine.moveUnitWithAttack(current1, current2, currentAttack.isHeal, attackOriginIndex);
    }
  }

  public void OpenSkillDetail(BL.Skill skill)
  {
    if (this.IsPushAndSet())
      return;
    int skillId = skill.skill.ID;
    int? nullable = MasterData.UnitSkillIntimate.FirstIndexOrNull<KeyValuePair<int, UnitSkillIntimate>>((Func<KeyValuePair<int, UnitSkillIntimate>, bool>) (x => x.Value.skill_BattleskillSkill == skillId));
    if (!nullable.HasValue)
    {
      nullable = MasterData.UnitSkillHarmonyQuest.FirstIndexOrNull<KeyValuePair<int, UnitSkillHarmonyQuest>>((Func<KeyValuePair<int, UnitSkillHarmonyQuest>, bool>) (x => x.Value.skill_BattleskillSkill == skillId));
      if (!nullable.HasValue)
      {
        PopupSkillDetails.show(this.skillDetailDialogPrefab, new PopupSkillDetails.Param(skill, UnitParameter.SkillGroup.Duel));
        goto label_5;
      }
    }
    PopupSkillDetails.show(this.skillDetailDialogPrefab, new PopupSkillDetails.Param(skill, UnitParameter.SkillGroup.Multi));
label_5:
    this.StartCoroutine(this.IsPushOff());
  }

  public void StartOpenAilmentsDetail(BL.UnitPosition unit)
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.OpenAilmentsDetail(unit));
  }

  private IEnumerator OpenAilmentsDetail(BL.UnitPosition unit)
  {
    BattleUI04Menu battleUi04Menu = this;
    IEnumerable<BattleFuncs.InvestSkill> investSkills = BattleFuncs.getInvestSkills(unit);
    GameObject popup = battleUi04Menu.ailmentsDetailPrefab.Clone();
    BattleUI01PopupAilmentsDetail menu = popup.GetComponent<BattleUI01PopupAilmentsDetail>();
    popup.SetActive(false);
    IEnumerator e = menu.Init(investSkills);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    menu.grid.Reposition();
    menu.scrollview.ResetPosition();
    popup.SetActive(true);
    e = battleUi04Menu.IsPushOff();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onEndScene()
  {
    this.isInitialized = false;
    this.indicator.SeEnable = false;
    this.eraseWithTween();
  }

  private IEnumerator WaitScrollSe()
  {
    yield return (object) new WaitForSeconds(0.3f);
    this.indicator.SeEnable = true;
  }

  private void eraseWithTween()
  {
    if (!Object.op_Inequality((Object) this.indicator, (Object) null))
      return;
    ((UITweener) ((Component) this.indicator).gameObject.GetComponent<TweenAlpha>()).PlayReverse();
  }
}
