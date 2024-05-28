// Decompiled with JetBrains decompiler
// Type: Unit004ReincarnationTypeCompleteMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using UnitStatusInformation;
using UnityEngine;

#nullable disable
public class Unit004ReincarnationTypeCompleteMenu : BackButtonMenuBase
{
  public GameObject unitTexture;
  [SerializeField]
  private UILabel TxtCharaName;
  [SerializeField]
  private UILabel TxtHimeTypeLeft;
  [SerializeField]
  private UILabel TxtHimeTypeRight;
  public UILabel jobNameLabel;
  public UILabel maxLevelLabel;
  public GameObject growthParameter;
  [SerializeField]
  private GameObject DirAnimRevival;
  [SerializeField]
  private GameObject DirMove;
  [SerializeField]
  private UILabel TxtMoveBefore;
  [SerializeField]
  private UILabel TxtMoveAfter;
  [SerializeField]
  private UILabel TxtNoChange;
  [SerializeField]
  private GameObject MoveBefore;
  [SerializeField]
  private GameObject MoveAfter;
  [SerializeField]
  private GameObject SlcArrow;
  [SerializeField]
  private GameObject NoChange;

  public void onTouchPanel()
  {
    if (this.IsPushAndSet() || Singleton<NGSceneManager>.GetInstance().backScene("unit004_Reincarnation_Type_TicketSelection"))
      return;
    this.backScene();
  }

  public override void onBackButton() => this.onTouchPanel();

  private void SetMaxLv(int beforeLv, int afterLv)
  {
    this.maxLevelLabel.SetTextLocalize(afterLv);
    ((UIWidget) this.maxLevelLabel).color = beforeLv < afterLv ? Color.yellow : Color.white;
  }

  private void SetMovement(int beforeMovement, int afterMovement)
  {
    if (beforeMovement != afterMovement)
    {
      this.MoveBefore.SetActive(true);
      this.MoveAfter.SetActive(true);
      this.SlcArrow.SetActive(true);
      this.NoChange.SetActive(false);
      this.TxtMoveBefore.SetTextLocalize(beforeMovement);
      this.TxtMoveAfter.SetTextLocalize(afterMovement);
    }
    else
    {
      this.MoveBefore.SetActive(false);
      this.MoveAfter.SetActive(false);
      this.SlcArrow.SetActive(false);
      this.NoChange.SetActive(true);
      this.TxtNoChange.SetTextLocalize(afterMovement);
    }
  }

  private IEnumerator SetUnitTexture(UnitUnit unit, int jobId)
  {
    Future<GameObject> future = unit.LoadMypage();
    yield return (object) future.Wait();
    GameObject texObj = future.Result.Clone(this.unitTexture.transform);
    yield return (object) unit.SetLargeSpriteSnap(jobId, texObj, 4);
    yield return (object) unit.SetLargeSpriteWithMask(jobId, texObj, Res.GUI._004_9_8_sozai.mask_chara.Load<Texture2D>(), 5, -146, 36);
  }

  public IEnumerator setCharacter(PlayerUnit beforeUnit, PlayerUnit afterUnit)
  {
    Future<GameObject> loader = GrowthParameter.LoadPrefab();
    yield return (object) loader.Wait();
    GrowthParameter instance = GrowthParameter.GetInstance(loader.Result, this.growthParameter.transform);
    UnitParameter unitParameter1 = new UnitParameter(beforeUnit);
    UnitParameter unitParameter2 = new UnitParameter(afterUnit);
    this.TxtCharaName.SetTextLocalize(unitParameter2.unit.name);
    this.TxtHimeTypeLeft.SetTextLocalize(beforeUnit.unit_type.name);
    this.TxtHimeTypeRight.SetTextLocalize(afterUnit.unit_type.name);
    this.jobNameLabel.SetTextLocalize(unitParameter2.job.name);
    this.SetMaxLv(beforeUnit.max_level, afterUnit.max_level);
    this.SetMovement(unitParameter1.nonBattleParam.Move, unitParameter2.nonBattleParam.Move);
    instance.SetParameter(GrowthParameter.ParameterType.HP, afterUnit.hp.transmigrate - beforeUnit.hp.transmigrate, afterUnit.self_hp_without_x - afterUnit.hp.transmigrate, afterUnit.self_hp_without_x, false);
    instance.SetParameter(GrowthParameter.ParameterType.STR, afterUnit.strength.transmigrate - beforeUnit.strength.transmigrate, afterUnit.self_strength_without_x - afterUnit.strength.transmigrate, afterUnit.self_strength_without_x, false);
    instance.SetParameter(GrowthParameter.ParameterType.INT, afterUnit.intelligence.transmigrate - beforeUnit.intelligence.transmigrate, afterUnit.self_intelligence_without_x - afterUnit.intelligence.transmigrate, afterUnit.self_intelligence_without_x, false);
    instance.SetParameter(GrowthParameter.ParameterType.VIT, afterUnit.vitality.transmigrate - beforeUnit.vitality.transmigrate, afterUnit.self_vitality_without_x - afterUnit.vitality.transmigrate, afterUnit.self_vitality_without_x, false);
    instance.SetParameter(GrowthParameter.ParameterType.MND, afterUnit.mind.transmigrate - beforeUnit.mind.transmigrate, afterUnit.self_mind_without_x - afterUnit.mind.transmigrate, afterUnit.self_mind_without_x, false);
    instance.SetParameter(GrowthParameter.ParameterType.AGI, afterUnit.agility.transmigrate - beforeUnit.agility.transmigrate, afterUnit.self_agility_without_x - afterUnit.agility.transmigrate, afterUnit.self_agility_without_x, false);
    instance.SetParameter(GrowthParameter.ParameterType.DEX, afterUnit.dexterity.transmigrate - beforeUnit.dexterity.transmigrate, afterUnit.self_dexterity_without_x - afterUnit.dexterity.transmigrate, afterUnit.self_dexterity_without_x, false);
    instance.SetParameter(GrowthParameter.ParameterType.LUK, afterUnit.lucky.transmigrate - beforeUnit.lucky.transmigrate, afterUnit.self_lucky_without_x - afterUnit.lucky.transmigrate, afterUnit.self_lucky_without_x, false);
    instance.SetParameterEffects();
    instance.SetParameterMaxStar(GrowthParameter.ParameterType.HP, afterUnit.hp.is_max);
    instance.SetParameterMaxStar(GrowthParameter.ParameterType.STR, afterUnit.strength.is_max);
    instance.SetParameterMaxStar(GrowthParameter.ParameterType.INT, afterUnit.intelligence.is_max);
    instance.SetParameterMaxStar(GrowthParameter.ParameterType.VIT, afterUnit.vitality.is_max);
    instance.SetParameterMaxStar(GrowthParameter.ParameterType.MND, afterUnit.mind.is_max);
    instance.SetParameterMaxStar(GrowthParameter.ParameterType.AGI, afterUnit.agility.is_max);
    instance.SetParameterMaxStar(GrowthParameter.ParameterType.DEX, afterUnit.dexterity.is_max);
    instance.SetParameterMaxStar(GrowthParameter.ParameterType.LUK, afterUnit.lucky.is_max);
    IEnumerator e = this.SetUnitTexture(afterUnit.unit, afterUnit.job_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.DirAnimRevival.SetActive(true);
  }
}
