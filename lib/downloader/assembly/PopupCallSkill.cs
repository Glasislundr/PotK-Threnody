// Decompiled with JetBrains decompiler
// Type: PopupCallSkill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class PopupCallSkill : BackButtonPopupBase
{
  [SerializeField]
  private PopupCallSkill.Layout layout;
  [SerializeField]
  private GameObject dirGauge;
  [SerializeField]
  private GameObject slcCallSkillGauge;
  [SerializeField]
  private GameObject slcCallSkillEffects;
  private GameObject prefabUnitIcon;
  private bool initializing_;
  private bool isAutoClose_;
  private GameObject genrePrefab;
  private NGBattleManager battleManager_;
  private BE env_;
  private BL.Phase battlePhase_;

  private NGBattleManager battleManager
  {
    get
    {
      return !Object.op_Inequality((Object) this.battleManager_, (Object) null) ? (this.battleManager_ = Singleton<NGBattleManager>.GetInstance()) : this.battleManager_;
    }
  }

  private BE env
  {
    get
    {
      return this.env_ == null ? (this.env_ = Object.op_Inequality((Object) this.battleManager, (Object) null) ? this.battleManager.environment : (BE) null) : this.env_;
    }
  }

  public IEnumerator initialize(
    UnitUnit unit,
    BattleskillSkill skill,
    bool isBattle,
    Decimal gauge_rate = 0M)
  {
    this.initializing_ = true;
    this.isAutoClose_ = isBattle;
    if (this.isAutoClose_)
      this.battlePhase_ = this.env.core.phaseState.state;
    yield return (object) this.setLayout(unit, skill, this.layout);
    this.dirGauge.SetActive(isBattle);
    if (isBattle)
    {
      Vector3 localScale = this.slcCallSkillGauge.transform.localScale;
      localScale.x = (float) gauge_rate;
      this.slcCallSkillGauge.transform.localScale = localScale;
      this.slcCallSkillEffects.SetActive(this.env.core.enemyCallSkillState.isCallGaugeMax);
    }
    this.initializing_ = false;
  }

  protected override void Update()
  {
    if (!this.isAutoClose_ || this.initializing_ || this.IsPush)
      return;
    base.Update();
    if (this.env.core.phaseState.state == this.battlePhase_)
      return;
    this.IsPush = true;
    Singleton<PopupManager>.GetInstance().closeAll();
  }

  private IEnumerator setLayout(
    UnitUnit unit,
    BattleskillSkill skill,
    PopupCallSkill.Layout layout)
  {
    bool isSea = Singleton<NGGameDataManager>.GetInstance().IsSea;
    Future<GameObject> prefabF;
    IEnumerator e;
    if (Object.op_Equality((Object) this.prefabUnitIcon, (Object) null))
    {
      prefabF = isSea ? new ResourceObject("Prefabs/Sea/UnitIcon/normal_sea").Load<GameObject>() : Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.prefabUnitIcon = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    UnitIcon unitScript = this.prefabUnitIcon.Clone(layout.dynUnitIcon.transform).GetComponent<UnitIcon>();
    e = unitScript.SetUnit(unit, unit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitScript.BottomBaseObject = false;
    ((Collider) unitScript.buttonBoxCollider).enabled = false;
    unitScript.BackgroundModeValue = UnitIcon.BackgroundMode.Call;
    unitScript = (UnitIcon) null;
    layout.TxtSkillName.SetTextLocalize(skill != null ? skill.name : "");
    if (skill != null)
    {
      if (Object.op_Equality((Object) this.genrePrefab, (Object) null))
      {
        prefabF = Res.Icons.SkillGenreIcon.Load<GameObject>();
        yield return (object) prefabF.Wait();
        this.genrePrefab = prefabF.Result;
        prefabF = (Future<GameObject>) null;
      }
      layout.spriteSkillProperty1.sprite2D = (Sprite) null;
      layout.spriteSkillProperty2.sprite2D = (Sprite) null;
      BattleskillGenre? genre1 = skill.genre1;
      BattleskillGenre? genre2 = skill.genre2;
      ((Component) layout.spriteSkillProperty1).gameObject.SetActive(genre1.HasValue);
      if (genre1.HasValue)
      {
        SkillGenreIcon component = this.genrePrefab.Clone(((Component) layout.spriteSkillProperty1).transform).GetComponent<SkillGenreIcon>();
        component.Init(genre1);
        ((UIWidget) component.iconSprite).depth = 3;
      }
      ((Component) layout.spriteSkillProperty2).gameObject.SetActive(genre2.HasValue);
      if (genre2.HasValue)
      {
        SkillGenreIcon component = this.genrePrefab.Clone(((Component) layout.spriteSkillProperty2).transform).GetComponent<SkillGenreIcon>();
        component.Init(genre2);
        ((UIWidget) component.iconSprite).depth = 3;
      }
    }
    else
    {
      ((Component) layout.spriteSkillIcon).gameObject.SetActive(false);
      ((Component) layout.spriteSkillProperty1).gameObject.SetActive(false);
      ((Component) layout.spriteSkillProperty2).gameObject.SetActive(false);
    }
    layout.TxtSkillDescription.SetTextLocalize(skill != null ? skill.description : "");
  }

  public override void onBackButton() => this.onClickedClose();

  public void onClickedClose()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  [Serializable]
  public class Layout
  {
    public GameObject dynUnitIcon;
    public UI2DSprite spriteSkillIcon;
    public UILabel TxtSkillName;
    public UILabel TxtSkillDescription;
    public UI2DSprite spriteSkillProperty1;
    public UI2DSprite spriteSkillProperty2;
  }
}
