// Decompiled with JetBrains decompiler
// Type: Versus026DirWinLossRecordsDetailItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Versus026DirWinLossRecordsDetailItem : MonoBehaviour
{
  [SerializeField]
  private Transform linkChara;
  [SerializeField]
  private GameObject icon;
  [SerializeField]
  private UILabel txtBattlePower;
  [SerializeField]
  private UILabel txtKill;
  [SerializeField]
  private UILabel txtDeath;
  [SerializeField]
  private UILabel txtDuel;
  private GameObject popupInfoPrefab;
  private PlayerUnit pUnit;
  private BL.Unit blUnit;
  private bool isPLayerUnit;
  [HideInInspector]
  public int battlePower;

  public IEnumerator InitDetailItems(
    GameObject infoPrefab,
    GameObject unitIconPrefab,
    WebAPI.Response.PvpClassMatchHistoryDetailPlayer_unit_pvp_results result,
    PlayerUnit playerUnit,
    PlayerAwakeSkill[] awakeSkills,
    PlayerItem[] gears,
    PlayerGearReisouSchema[] reisous)
  {
    this.isPLayerUnit = true;
    playerUnit.primary_equipped_awake_skill = playerUnit.FindEquippedExtraSkill(awakeSkills);
    playerUnit.primary_equipped_gear = playerUnit.FindEquippedGear(gears);
    playerUnit.primary_equipped_gear2 = playerUnit.FindEquippedGear2(gears);
    playerUnit.primary_equipped_gear3 = playerUnit.FindEquippedGear3(gears);
    playerUnit.primary_equipped_reisou = playerUnit.FindEquippedReisou(gears, reisous);
    playerUnit.primary_equipped_reisou2 = playerUnit.FindEquippedReisou2(gears, reisous);
    playerUnit.primary_equipped_reisou3 = playerUnit.FindEquippedReisou3(gears, reisous);
    IEnumerator e = this.SetInfoPrefab(infoPrefab, unitIconPrefab, playerUnit, result.defeat_count.ToString(), result.dead_count.ToString(), result.duel_count.ToString());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator InitDetailItems(
    GameObject infoPrefab,
    GameObject unitIconPrefab,
    WebAPI.Response.PvpClassMatchHistoryDetailTarget_player_unit_pvp_results result,
    PlayerUnit playerUnit,
    PlayerAwakeSkill[] awakeSkills,
    PlayerItem[] gears,
    PlayerGearReisouSchema[] reisous)
  {
    this.isPLayerUnit = false;
    playerUnit.primary_equipped_awake_skill = playerUnit.FindEquippedExtraSkill(awakeSkills);
    playerUnit.primary_equipped_gear = playerUnit.FindEquippedGear(gears);
    playerUnit.primary_equipped_gear2 = playerUnit.FindEquippedGear2(gears);
    playerUnit.primary_equipped_gear3 = playerUnit.FindEquippedGear3(gears);
    playerUnit.primary_equipped_reisou = playerUnit.FindEquippedReisou(gears, reisous);
    playerUnit.primary_equipped_reisou2 = playerUnit.FindEquippedReisou2(gears, reisous);
    playerUnit.primary_equipped_reisou3 = playerUnit.FindEquippedReisou3(gears, reisous);
    IEnumerator e = this.SetInfoPrefab(infoPrefab, unitIconPrefab, playerUnit, result.defeat_count.ToString(), result.dead_count.ToString(), result.duel_count.ToString());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator SetInfoPrefab(
    GameObject infoPrefab,
    GameObject unitIconPrefab,
    PlayerUnit playerUnit,
    string kill,
    string death,
    string duel)
  {
    Versus026DirWinLossRecordsDetailItem recordsDetailItem = this;
    recordsDetailItem.icon.SetActive(false);
    recordsDetailItem.pUnit = playerUnit;
    recordsDetailItem.blUnit = ColosseumEnvironmentInitializer.createUnitByPlayerUnit(recordsDetailItem.pUnit, 0, true);
    GameObject iconPrefab = unitIconPrefab.Clone(recordsDetailItem.linkChara);
    UnitIcon unitIconScript = ((Component) iconPrefab.GetComponent<UnitIconBase>()).GetComponent<UnitIcon>();
    unitIconScript.setBottom(recordsDetailItem.pUnit);
    unitIconScript.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    IEnumerator e = unitIconScript.SetUnit(recordsDetailItem.pUnit, recordsDetailItem.pUnit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitIconScript.BottomModeValue = UnitIconBase.GetBottomModeLevel(recordsDetailItem.pUnit.unit, recordsDetailItem.pUnit);
    unitIconScript.setLevelText(recordsDetailItem.pUnit);
    recordsDetailItem.popupInfoPrefab = infoPrefab;
    // ISSUE: reference to a compiler-generated method
    EventDelegate.Set(iconPrefab.GetComponentInChildren<LongPressButton>().onClick, new EventDelegate.Callback(recordsDetailItem.\u003CSetInfoPrefab\u003Eb__13_0));
    BattleFuncs.environment.Reset((BL) null);
    Judgement.BattleParameter parameter = Judgement.BattleParameter.FromBeUnit((BL.ISkillEffectListUnit) recordsDetailItem.blUnit, isUsePosition: false);
    recordsDetailItem.blUnit.setParameter(parameter);
    recordsDetailItem.blUnit.hp = parameter.Hp;
    recordsDetailItem.battlePower = parameter.Combat;
    recordsDetailItem.txtBattlePower.text = recordsDetailItem.battlePower.ToString();
    recordsDetailItem.txtKill.text = kill;
    recordsDetailItem.txtDeath.text = death;
    recordsDetailItem.txtDuel.text = duel;
  }

  public void OpenPopupInfo()
  {
    Singleton<PopupManager>.GetInstance().open(this.popupInfoPrefab).GetComponent<BattleUI01UnitInformation>().InitFromHistory(this.blUnit, this.isPLayerUnit);
  }
}
