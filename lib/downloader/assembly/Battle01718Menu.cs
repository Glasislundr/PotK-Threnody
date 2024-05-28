// Decompiled with JetBrains decompiler
// Type: Battle01718Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Battle01718Menu : BattleBackButtonMenuBase
{
  private const int DirConditionSingle = 0;
  private const int DirConditionMulti = 1;
  [SerializeField]
  private GameObject[] dirConditionsVictory;
  [SerializeField]
  private UILabel lose_condition_txt;
  [SerializeField]
  private UILabel victory_condition_txt;
  [SerializeField]
  private UILabel[] victory_condition_txt_multi;
  [SerializeField]
  private UILabel player_nb_unit_txt;
  [SerializeField]
  private UILabel enemy_nb_unit_txt;
  [SerializeField]
  private UILabel passed_turn_txt;
  [SerializeField]
  private GameObject auto_button;
  [SerializeField]
  private GameObject retreat_button;
  [SerializeField]
  private GameObject mission_button;
  [SerializeField]
  private GameObject map_grid;
  [SerializeField]
  private GameObject waveDir;
  [SerializeField]
  private UILabel waveNum;
  [SerializeField]
  private List<GameObject> duel_touch_wait_onoroff;
  [SerializeField]
  private List<GameObject> duel_skill_confirmation_onoroff;
  [SerializeField]
  private SpreadColorButton btnSimpleStandby;
  [SerializeField]
  private SpreadColorButton btnSkillConfirmation;
  private bool is_push;
  private static string chipExt = ".png__GUI__battle_mapchip__battle_mapchip_prefab";

  protected override IEnumerator Start_Battle()
  {
    Battle01718Menu battle01718Menu = this;
    ((IEnumerable<GameObject>) battle01718Menu.dirConditionsVictory).ToggleOnce(-1);
    if (battle01718Menu.env.core.condition.condition.victory_text.Count<char>((Func<char, bool>) (x => x.Equals('\n'))) == 0)
    {
      ((IEnumerable<GameObject>) battle01718Menu.dirConditionsVictory).ToggleOnce(0);
      battle01718Menu.victory_condition_txt.SetTextLocalize(battle01718Menu.env.core.condition.condition.victory_text);
    }
    else
    {
      ((IEnumerable<GameObject>) battle01718Menu.dirConditionsVictory).ToggleOnce(1);
      string[] strArray = battle01718Menu.env.core.condition.condition.victory_text.Split('\n');
      battle01718Menu.victory_condition_txt_multi[0].SetTextLocalize(strArray[0]);
      battle01718Menu.victory_condition_txt_multi[1].SetTextLocalize(strArray[1]);
    }
    battle01718Menu.lose_condition_txt.SetTextLocalize(battle01718Menu.env.core.condition.condition.lose_text);
    battle01718Menu.setText(battle01718Menu.player_nb_unit_txt, battle01718Menu.countActiveUnits(battle01718Menu.env.core.playerUnits.value));
    battle01718Menu.setText(battle01718Menu.enemy_nb_unit_txt, battle01718Menu.countActiveUnits(battle01718Menu.env.core.enemyUnits.value));
    battle01718Menu.setText(battle01718Menu.passed_turn_txt, battle01718Menu.env.core.phaseState.absoluteTurnCount);
    if (Object.op_Inequality((Object) battle01718Menu.waveDir, (Object) null))
    {
      if (battle01718Menu.env.core.battleInfo.isWave)
      {
        battle01718Menu.waveDir.SetActive(true);
        battle01718Menu.setText(battle01718Menu.waveNum, string.Format("{0}/{1}", (object) (battle01718Menu.env.core.currentWave + 1), (object) battle01718Menu.env.core.battleInfo.waveInfos.Length));
      }
      else
        battle01718Menu.waveDir.SetActive(false);
    }
    if (battle01718Menu.duel_touch_wait_onoroff != null && battle01718Menu.duel_touch_wait_onoroff.Count > 0)
      battle01718Menu.duel_touch_wait_onoroff.ToggleOnce(battle01718Menu.env.core.isTouchWait.value ? 1 : 0);
    if (battle01718Menu.duel_skill_confirmation_onoroff != null && battle01718Menu.duel_skill_confirmation_onoroff.Count > 0)
      battle01718Menu.duel_skill_confirmation_onoroff.ToggleOnce(battle01718Menu.env.core.isSkillUseConfirmation.value ? 1 : 0);
    if (Object.op_Inequality((Object) battle01718Menu.auto_button, (Object) null))
      battle01718Menu.setButtonEnabled(battle01718Menu.auto_button.GetComponent<UIButton>(), battle01718Menu.env.core.battleInfo.isAutoBattleEnable && !battle01718Menu.env.core.isAutoBattle.value);
    battle01718Menu.setButtonEnabled(battle01718Menu.retreat_button.GetComponent<UIButton>(), battle01718Menu.env.core.battleInfo.isRetreatEnable);
    if (Object.op_Inequality((Object) battle01718Menu.mission_button, (Object) null))
      battle01718Menu.setButtonEnabled(battle01718Menu.mission_button.GetComponent<UIButton>(), battle01718Menu.env.core.battleInfo.hasMission);
    IEnumerator e = battle01718Menu.setupMapChips();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
    {
      if (Object.op_Inequality((Object) battle01718Menu.btnSimpleStandby, (Object) null))
        ((UIButtonColor) battle01718Menu.btnSimpleStandby).isEnabled = false;
      if (Object.op_Inequality((Object) battle01718Menu.btnSkillConfirmation, (Object) null))
        ((UIButtonColor) battle01718Menu.btnSkillConfirmation).isEnabled = false;
    }
  }

  private int countActiveUnits(List<BL.Unit> units)
  {
    int num = 0;
    foreach (BL.Unit unit in units)
    {
      if (unit.isEnable && !unit.isDead)
        ++num;
    }
    return num;
  }

  private void setButtonEnabled(UIButton button, bool v)
  {
    if (Object.op_Equality((Object) button, (Object) null))
      return;
    ((UIButtonColor) button).isEnabled = v;
  }

  private IEnumerator AutoBattleStartPop()
  {
    Battle01718Menu battle01718Menu = this;
    Future<GameObject> prefab = Res.Prefabs.popup.popup_017_18_16__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battle01718Menu.battleManager.popupOpen(prefab.Result);
  }

  private IEnumerator TurnEndPop()
  {
    Battle01718Menu battle01718Menu = this;
    Future<GameObject> prefab = Res.Prefabs.popup.popup_017_18_18__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battle01718Menu.battleManager.popupOpen(prefab.Result);
  }

  public void IbtnAutoBattle()
  {
    if (this.IsPushCheck())
      return;
    this.StartCoroutine(this.AutoBattleStartPop());
  }

  public void IbtnTurnEnd()
  {
    if (this.IsPushCheck())
      return;
    this.StartCoroutine(this.TurnEndPop());
  }

  public void IbtnClose()
  {
    if (this.IsPushCheck())
      return;
    this.battleManager.popupDismiss();
    this.StartCoroutine(this.SavePersist());
  }

  public override void onBackButton() => this.IbtnClose();

  public void IbtnMission()
  {
    if (this.IsPushCheck())
      return;
    this.StartCoroutine(this.missionPop());
  }

  public void IbtnForceList()
  {
    if (this.IsPushCheck())
      return;
    this.StartCoroutine(this.ForcePop());
  }

  public void IbtnRetreat()
  {
    if (this.IsPushCheck())
      return;
    this.StartCoroutine(this.RetreatPop());
  }

  public void IbtnSuspend()
  {
    if (this.IsPushCheck())
      return;
    this.StartCoroutine(this.SuspendPop());
  }

  public void IbtnTouchWait()
  {
    this.env.core.isTouchWait.value = !this.env.core.isTouchWait.value;
    if (this.duel_touch_wait_onoroff == null || this.duel_touch_wait_onoroff.Count <= 0)
      return;
    this.duel_touch_wait_onoroff.ToggleOnce(this.env.core.isTouchWait.value ? 1 : 0);
  }

  public void IbtnSkillConfirmation()
  {
    this.env.core.isSkillUseConfirmation.value = !this.env.core.isSkillUseConfirmation.value;
    if (this.duel_skill_confirmation_onoroff == null || this.duel_skill_confirmation_onoroff.Count <= 0)
      return;
    this.duel_skill_confirmation_onoroff.ToggleOnce(this.env.core.isSkillUseConfirmation.value ? 1 : 0);
  }

  private IEnumerator SavePersist()
  {
    Battle01718Menu battle01718Menu = this;
    if (Persist.battleTouchWait.Data.isTouchWait != battle01718Menu.env.core.isTouchWait.value)
    {
      Persist.battleTouchWait.Data.isTouchWait = battle01718Menu.env.core.isTouchWait.value;
      Persist.battleTouchWait.Flush();
      yield return (object) null;
    }
    if (Persist.battleSkillUseConfirmation.Data.isSkillUseConfirmation != battle01718Menu.env.core.isSkillUseConfirmation.value)
    {
      Persist.battleSkillUseConfirmation.Data.isSkillUseConfirmation = battle01718Menu.env.core.isSkillUseConfirmation.value;
      Persist.battleSkillUseConfirmation.Flush();
      yield return (object) null;
    }
  }

  private IEnumerator RetreatPop()
  {
    Battle01718Menu battle01718Menu = this;
    Future<GameObject> prefab = Res.Prefabs.popup.popup_017_18_1__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battle01718Menu.battleManager.popupOpen(prefab.Result);
  }

  private IEnumerator ForcePop()
  {
    Battle01718Menu battle01718Menu = this;
    Future<GameObject> prefab = Res.Prefabs.popup.BattleUI_02.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battle01718Menu.battleManager.popupOpen(prefab.Result);
  }

  private IEnumerator missionPop()
  {
    Battle01718Menu battle01718Menu = this;
    Future<GameObject> prefab = !battle01718Menu.battleManager.isSea ? new ResourceObject("Prefabs/quest002_2/dir_Mission_List_Battle").Load<GameObject>() : new ResourceObject("Prefabs/quest002_2_sea/dir_Mission_List_Battle_sea").Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battle01718Menu.battleManager.popupOpen(prefab.Result);
  }

  private IEnumerator SuspendPop()
  {
    Battle01718Menu battle01718Menu = this;
    Future<GameObject> prefab = Res.Prefabs.popup.popup_017_18_2__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battle01718Menu.battleManager.popupOpen(prefab.Result);
  }

  private IEnumerator setupMapChips()
  {
    Battle01718Menu battle01718Menu = this;
    Future<GameObject> prefabF = Res.Battle_Mapchip.BattleMapChipSprite.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    int fieldHeight = battle01718Menu.env.core.getFieldHeight();
    int fieldWidth = battle01718Menu.env.core.getFieldWidth();
    UIWidget inParents = NGUITools.FindInParents<UIWidget>(battle01718Menu.map_grid);
    float num = (float) inParents.height / (float) fieldHeight;
    int size = (int) Mathf.Min((float) inParents.width / (float) fieldWidth, num);
    for (int row = fieldHeight - 1; row >= 0; --row)
    {
      for (int column = 0; column < fieldWidth; ++column)
      {
        BL.UnitPosition unitPosition = (BL.UnitPosition) null;
        BL.Panel fieldPanel = battle01718Menu.env.core.getFieldPanel(row, column);
        BL.UnitPosition[] fieldUnits = battle01718Menu.env.core.getFieldUnits(row, column);
        if (fieldUnits != null)
          unitPosition = ((IEnumerable<BL.UnitPosition>) fieldUnits).FirstOrDefault<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => !x.unit.isFacility || x.unit.facility.isView)) ?? ((IEnumerable<BL.UnitPosition>) fieldUnits).FirstOrDefault<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => x.unit.isFacility && !x.unit.facility.isView));
        string name = unitPosition == null ? "slc_mapchip_" + (object) fieldPanel.landform.baseID : (battle01718Menu.env.core.getForceID(unitPosition.unit) != BL.ForceID.player || unitPosition.unit.isFacility ? (unitPosition.unit.isFacility ? (!unitPosition.unit.facility.isView ? "slc_mapchip_" + (object) fieldPanel.landform.baseID : "slc_mapchip_238") : "slc_mapchip_51") : "slc_mapchip_50");
        battle01718Menu.cloneMapChip(name, size, result);
      }
    }
    UIGrid component = battle01718Menu.map_grid.GetComponent<UIGrid>();
    component.arrangement = (UIGrid.Arrangement) 0;
    component.maxPerLine = fieldWidth;
    component.cellHeight = (float) size;
    component.cellWidth = (float) size;
    inParents.width = size * fieldWidth;
    inParents.height = size * fieldHeight;
    component.repositionNow = true;
  }

  private void cloneMapChip(string name, int size, GameObject prefab)
  {
    UISprite component = prefab.CloneAndGetComponent<UISprite>(this.map_grid);
    component.spriteName = name + Battle01718Menu.chipExt;
    ((UIWidget) component).width = size;
    ((UIWidget) component).height = size;
  }

  private bool IsPushCheck()
  {
    if (this.battleManager.environment.core.phaseState.state == BL.Phase.gameover || this.battleManager.environment.core.phaseState.state == BL.Phase.surrender || this.is_push)
      return true;
    this.is_push = true;
    this.StartCoroutine(this.pushCancel());
    return false;
  }

  private IEnumerator pushCancel()
  {
    yield return (object) new WaitForSeconds(0.5f);
    this.is_push = false;
  }
}
