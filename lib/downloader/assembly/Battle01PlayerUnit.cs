// Decompiled with JetBrains decompiler
// Type: Battle01PlayerUnit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Battle01PlayerUnit : BattleMonoBehaviour
{
  [SerializeField]
  private NGTweenGaugeScale hpGauge;
  [SerializeField]
  private UI2DSprite character;
  [SerializeField]
  private Battle01UnitCounter counter;
  [SerializeField]
  private GameObject dropout;
  [SerializeField]
  private Battle01PVPRespawnCount pvpRespawnCount;
  private BL.BattleModified<BL.PhaseState> phaseModified;
  private BL.BattleModified<BL.Skill> ougiModified;
  private BL.BattleModified<BL.UnitPosition> positionModified;
  private BL.BattleModified<BL.Unit> modified;
  private UnitIcon unitIcon;
  [SerializeField]
  private UIWidget rootWidget;
  public bool isViewCounter;
  private SkillMetamorphosis metamorphosis;

  public bool isViewDropout
  {
    get => this.dropout.activeSelf;
    set
    {
      if (this.dropout.activeSelf == value)
        return;
      this.dropout.SetActive(value);
    }
  }

  private void Awake() => ((Behaviour) this.character).enabled = false;

  protected override IEnumerator Start_Battle()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Battle01PlayerUnit battle01PlayerUnit = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    battle01PlayerUnit.phaseModified = BL.Observe<BL.PhaseState>(battle01PlayerUnit.env.core.phaseState);
    return false;
  }

  public override IEnumerator onInitAsync()
  {
    Battle01PlayerUnit battle01PlayerUnit = this;
    Future<GameObject> f = !battle01PlayerUnit.battleManager.isSea ? Res.Prefabs.UnitIcon.normal.Load<GameObject>() : Res.Prefabs.Sea.UnitIcon.normal_sea.Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = f.Result;
    battle01PlayerUnit.unitIcon = result.CloneAndGetComponent<UnitIcon>(((Component) battle01PlayerUnit.character).gameObject.transform);
    UIWidget component = ((Component) battle01PlayerUnit).GetComponent<UIWidget>();
    if (Object.op_Inequality((Object) component, (Object) null))
      NGUITools.AdjustDepth(((Component) battle01PlayerUnit.unitIcon).gameObject, component.depth);
    battle01PlayerUnit.unitIcon.SetBasedOnHeight(((UIWidget) battle01PlayerUnit.character).height);
    NGUITools.MarkParentAsChanged(((Component) battle01PlayerUnit.unitIcon).gameObject);
  }

  private IEnumerator doSetIcon(BL.Unit unit, bool isGray)
  {
    SkillMetamorphosis metamorphosis = unit?.metamorphosis;
    if (this.unitIcon.unit == null || this.unitIcon.unit.ID != unit.unit.ID || this.metamorphosis != metamorphosis)
    {
      this.metamorphosis = metamorphosis;
      if (metamorphosis != null)
      {
        int metamorphosisId = metamorphosis.metamorphosis_id;
      }
      IEnumerator e = this.unitIcon.SetUnit(unit.playerUnit, metamorphosis, isGray);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.unitIcon.BottomModeValue = UnitIconBase.BottomMode.Nothing;
    }
    this.unitIcon.Gray = isGray;
  }

  protected override void LateUpdate_Battle()
  {
    if (this.modified == null || !Object.op_Inequality((Object) this.unitIcon, (Object) null))
      return;
    bool flag1 = this.modified.isChangedOnce();
    BL.Unit unit = this.modified.value;
    bool flag2 = this.isViewCounter && unit.hasOugi && (!this.battleManager.isOvo || !unit.isDead) && BattleFuncs.checkUseSkillInvokeGameMode((BL.ISkillEffectListUnit) unit, this.ougiModified.value, false) && BattleFuncs.checkUseOugiSkillMaxCountInDeck((BL.ISkillEffectListUnit) unit, this.ougiModified.value);
    if (flag1)
    {
      this.StartCoroutine(this.doSetIcon(unit, this.positionModified.value.isCompleted));
      this.hpGauge.setValue(unit.isDead ? 0 : unit.hp, unit.parameter.Hp);
      this.counter.isActive = flag2;
      this.isViewDropout = unit.isDead;
    }
    if (flag2 && (flag1 || this.phaseModified.isChangedOnce() || this.ougiModified.isChangedOnce()))
      this.counter.setCount(this.ougiModified.value.useTurn - this.phaseModified.value.absoluteTurnCount);
    if (this.battleManager.isOvo & flag1)
    {
      if (unit.isDead)
      {
        ((Component) this.pvpRespawnCount).gameObject.SetActive(true);
        this.pvpRespawnCount.setCount(unit.pvpRespawnCount);
      }
      else
        ((Component) this.pvpRespawnCount).gameObject.SetActive(false);
    }
    if (!this.positionModified.isChangedOnce())
      return;
    this.unitIcon.Gray = this.positionModified.value.isCompleted || unit.isDead;
  }

  public void setUnit(BL.Unit unit)
  {
    this.hpGauge.setValue(unit.isDead ? 0 : unit.hp, unit.parameter.Hp, false);
    this.modified = BL.Observe<BL.Unit>(unit);
    this.ougiModified = BL.Observe<BL.Skill>(unit.ougi);
    this.positionModified = BL.Observe<BL.UnitPosition>(Singleton<NGBattleManager>.GetInstance().environment.core.getUnitPosition(unit));
  }

  public BL.Unit getUnit() => this.modified == null ? (BL.Unit) null : this.modified.value;
}
