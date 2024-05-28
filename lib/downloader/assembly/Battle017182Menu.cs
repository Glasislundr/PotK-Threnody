// Decompiled with JetBrains decompiler
// Type: Battle017182Menu
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
public class Battle017182Menu : BattleBackButtonMenuBase
{
  [SerializeField]
  private UILabel victory_conditions_txt;
  [SerializeField]
  private UILabel player_nb_unit_txt;
  [SerializeField]
  private UILabel enemy_nb_unit_txt;
  [SerializeField]
  private UILabel passed_turn_txt;
  [SerializeField]
  private GameObject duel_skip_button;
  [SerializeField]
  private List<GameObject> duel_skip_onoroff;
  [SerializeField]
  private GameObject map_grid;
  private bool isPush;
  private static string chipExt = ".png__GUI__battle_mapchip__battle_mapchip_prefab";

  protected override IEnumerator Start_Battle()
  {
    Battle017182Menu battle017182Menu = this;
    battle017182Menu.isPush = false;
    ((Component) battle017182Menu.victory_conditions_txt).gameObject.SetActive(true);
    IGameEngine gameEngine = battle017182Menu.battleManager.gameEngine;
    int v = 0;
    string str1 = "";
    string str2 = "";
    switch (gameEngine)
    {
      case PVPManager _:
        MpStage stage1 = (gameEngine as PVPManager).stage;
        str1 = stage1.victory_condition;
        str2 = stage1.victory_sub_condition;
        v = stage1.turns - gameEngine.remainTurn.value;
        break;
      case PVNpcManager _:
        MpStage stage2 = (gameEngine as PVNpcManager).stage;
        str1 = stage2.victory_condition;
        str2 = stage2.victory_sub_condition;
        v = stage2.turns - gameEngine.remainTurn.value;
        break;
      case GVGManager _:
        GVGManager gvgManager = gameEngine as GVGManager;
        str1 = gvgManager.victory_condition;
        str2 = gvgManager.victory_sub_condition;
        v = gvgManager.endTurn - gameEngine.remainTurn.value;
        break;
    }
    battle017182Menu.setText(battle017182Menu.victory_conditions_txt, Consts.Format(Consts.GetInstance().VERSUS_017182MENU_VICTORY_CONDITION_FORMAT, (IDictionary) new Hashtable()
    {
      {
        (object) "a",
        (object) str1
      },
      {
        (object) "b",
        (object) str2
      }
    }));
    battle017182Menu.setText(battle017182Menu.player_nb_unit_txt, battle017182Menu.countActiveUnits(battle017182Menu.env.core.playerUnits.value));
    battle017182Menu.setText(battle017182Menu.enemy_nb_unit_txt, battle017182Menu.countActiveUnits(battle017182Menu.env.core.enemyUnits.value));
    battle017182Menu.setText(battle017182Menu.passed_turn_txt, v);
    if (battle017182Menu.duel_skip_onoroff != null && battle017182Menu.duel_skip_onoroff.Count > 0)
      battle017182Menu.duel_skip_onoroff.ToggleOnce(battle017182Menu.battleManager.noDuelScene ? 1 : 0);
    IEnumerator e = battle017182Menu.setupMapChips();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
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

  public void IbtnClose() => this.onBackButton();

  public override void onBackButton()
  {
    if (this.isPush)
      return;
    this.isPush = true;
    this.battleManager.popupDismiss();
  }

  public void IbtnRetreat()
  {
    if (this.isPush)
      return;
    this.isPush = true;
    this.StartCoroutine(this.RetreatPop());
  }

  public void IbtnDuelSkip()
  {
    this.battleManager.noDuelScene = !this.battleManager.noDuelScene;
    if (this.duel_skip_onoroff == null || this.duel_skip_onoroff.Count <= 0)
      return;
    this.duel_skip_onoroff.ToggleOnce(this.battleManager.noDuelScene ? 1 : 0);
  }

  private IEnumerator RetreatPop()
  {
    Battle017182Menu battle017182Menu = this;
    Future<GameObject> prefab = Res.Prefabs.popup.popup_017_18_1__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battle017182Menu.battleManager.popupOpen(prefab.Result);
    battle017182Menu.isPush = false;
  }

  private IEnumerator setupMapChips()
  {
    Battle017182Menu battle017182Menu = this;
    Future<GameObject> prefabF = Res.Battle_Mapchip.BattleMapChipSprite.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    int fieldHeight = battle017182Menu.env.core.getFieldHeight();
    int fieldWidth = battle017182Menu.env.core.getFieldWidth();
    UIWidget inParents = NGUITools.FindInParents<UIWidget>(battle017182Menu.map_grid);
    float num = (float) inParents.height / (float) fieldHeight;
    int cellSize = (int) Mathf.Min((float) inParents.width / (float) fieldWidth, num);
    // ISSUE: reference to a compiler-generated method
    List<PvpStageFormation> list = ((IEnumerable<PvpStageFormation>) MasterData.PvpStageFormationList).Where<PvpStageFormation>(new Func<PvpStageFormation, bool>(battle017182Menu.\u003CsetupMapChips\u003Eb__16_0)).ToList<PvpStageFormation>();
    if (battle017182Menu.battleManager.order == 0)
    {
      for (int r = fieldHeight - 1; r >= 0; --r)
      {
        for (int c = 0; c < fieldWidth; ++c)
          battle017182Menu.choiceMapChip(r, c, result, cellSize, list);
      }
    }
    else
    {
      for (int r = 0; r < fieldHeight; ++r)
      {
        for (int c = fieldWidth - 1; c >= 0; --c)
          battle017182Menu.choiceMapChip(r, c, result, cellSize, list);
      }
    }
    UIGrid component = battle017182Menu.map_grid.GetComponent<UIGrid>();
    component.arrangement = (UIGrid.Arrangement) 0;
    component.maxPerLine = fieldWidth;
    component.cellHeight = (float) cellSize;
    component.cellWidth = (float) cellSize;
    inParents.width = cellSize * fieldWidth;
    inParents.height = cellSize * fieldHeight;
    component.repositionNow = true;
  }

  public void choiceMapChip(
    int r,
    int c,
    GameObject prefab,
    int cellSize,
    List<PvpStageFormation> pvpFormation)
  {
    BL.UnitPosition unitPosition = (BL.UnitPosition) null;
    BL.Panel fieldPanel = this.env.core.getFieldPanel(r, c);
    BL.UnitPosition[] fieldUnits = this.env.core.getFieldUnits(r, c);
    if (fieldUnits != null)
      unitPosition = ((IEnumerable<BL.UnitPosition>) fieldUnits).FirstOrDefault<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => !x.unit.isFacility || x.unit.unit.facility.category_id != 4)) ?? ((IEnumerable<BL.UnitPosition>) fieldUnits).FirstOrDefault<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => x.unit.isFacility && x.unit.unit.facility.category_id == 4));
    UISprite targetMap = this.cloneMapChip(unitPosition == null ? "slc_mapchip_" + (object) fieldPanel.landform.baseID : (!unitPosition.unit.isFacility || unitPosition.unit.unit.facility.category_id != 4 ? (this.env.core.getForceID(unitPosition.unit) != BL.ForceID.player || unitPosition.unit.isFacility ? (!unitPosition.unit.isFacility ? "slc_mapchip_51" : "slc_mapchip_238") : "slc_mapchip_50") : "slc_mapchip_" + (object) fieldPanel.landform.baseID), cellSize, prefab);
    PvpStageFormation pvpStageFormation = pvpFormation.FirstOrDefault<PvpStageFormation>((Func<PvpStageFormation, bool>) (x => x.formation_x - 1 == c && x.formation_y - 1 == r));
    if (pvpFormation.Count <= 0 || pvpStageFormation == null)
      return;
    this.clonePvPFormation(targetMap, prefab, pvpStageFormation.player_order == this.battleManager.order);
  }

  private UISprite cloneMapChip(string name, int size, GameObject prefab)
  {
    UISprite component = prefab.CloneAndGetComponent<UISprite>(this.map_grid);
    component.spriteName = name + Battle017182Menu.chipExt;
    ((UIWidget) component).width = size;
    ((UIWidget) component).height = size;
    return component;
  }

  private void clonePvPFormation(UISprite targetMap, GameObject prefab, bool isMe)
  {
    UISprite component = prefab.CloneAndGetComponent<UISprite>(((Component) targetMap).transform);
    component.spriteName = this.GetFormationSpriteName(isMe ? 0 : 1) + Battle017182Menu.chipExt;
    ((UIWidget) component).depth = ((UIWidget) targetMap).depth + 1;
    ((Component) component).transform.Clear();
    ((UIWidget) component).width = ((UIWidget) targetMap).width;
    ((UIWidget) component).height = ((UIWidget) targetMap).height;
  }

  private string GetFormationSpriteName(int order)
  {
    string formationSpriteName = "";
    switch (order)
    {
      case 0:
        formationSpriteName = "slc_mapchip_ownarea";
        break;
      case 1:
        formationSpriteName = "slc_mapchip_enemyarea";
        break;
    }
    return formationSpriteName;
  }
}
