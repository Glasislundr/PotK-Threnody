// Decompiled with JetBrains decompiler
// Type: MapEditFacilityInformation
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
public class MapEditFacilityInformation : MonoBehaviour
{
  [SerializeField]
  private UIButton btnRemove_;
  [SerializeField]
  private UIButton btnToLayout_;
  [SerializeField]
  private UILabel txtName_;
  [SerializeField]
  private GameObject lnkIcon_;
  [SerializeField]
  private int depthProperty_ = 11;
  [SerializeField]
  private GameObject lnkProperty0_;
  [SerializeField]
  private GameObject lnkProperty1_;
  [SerializeField]
  private UILabel txtDescription_;
  [SerializeField]
  private UILabel txtHP_;
  [SerializeField]
  private UILabel txtDEF_;
  [SerializeField]
  private UILabel txtINT_;
  [SerializeField]
  [Tooltip("機能[OFF, ON]の順にセット")]
  private GameObject[] iconPassage_;
  [SerializeField]
  [Tooltip("機能[OFF, ON]の順にセット")]
  private GameObject[] iconDestruction_;
  [SerializeField]
  [Tooltip("機能[OFF, ON]の順にセット")]
  private GameObject[] iconVisibility_;
  [SerializeField]
  private UILabel txtCost_;
  [SerializeField]
  private UILabel txtArea_;
  private MapEdit031TopMenu menu_;
  private NGTweenParts tween_;
  private bool isWait_ = true;
  private bool isReboot_;
  private WeakReference wCurrent_;
  private GameObject prefabIcon_;
  private GameObject prefabGenre_;
  private GameObject iconGenre0_;
  private GameObject iconGenre1_;
  private MapEditFacilityInformation.TrackEffectArea trackEffectArea_;
  private const int INDEX_OFF = 0;
  private const int INDEX_ON = 1;
  private const int DEPTH_INTERVAL = 1;

  private bool isActiveInformation_
  {
    get
    {
      return !Object.op_Inequality((Object) this.tween_, (Object) null) ? ((Component) this).gameObject.activeSelf : this.tween_.isActive;
    }
  }

  private void setActiveInformation(bool bActive, bool bInit = false)
  {
    if (Object.op_Inequality((Object) this.tween_, (Object) null))
    {
      if (bInit)
        this.tween_.forceActive(bActive);
      else
        this.tween_.isActive = bActive;
    }
    else
      ((Component) this).gameObject.SetActive(bActive);
  }

  private bool waitAndSet()
  {
    if (this.isWait_)
      return true;
    this.isWait_ = true;
    return false;
  }

  private MapEditOrnament current_
  {
    get
    {
      return this.wCurrent_ == null || !this.wCurrent_.IsAlive ? (MapEditOrnament) null : this.wCurrent_.Target as MapEditOrnament;
    }
    set
    {
      if (Object.op_Equality((Object) this.current_, (Object) value))
        return;
      this.wCurrent_ = Object.op_Inequality((Object) value, (Object) null) ? new WeakReference((object) value) : (WeakReference) null;
    }
  }

  public void initialize(MapEdit031TopMenu menu)
  {
    this.menu_ = menu;
    if (Object.op_Equality((Object) menu, (Object) null))
      return;
    this.tween_ = ((Component) this).GetComponent<NGTweenParts>();
    this.setActiveInformation(false, true);
    this.menu_.isActiveButtonStorage_ = true;
    this.isWait_ = false;
    if (Object.op_Inequality((Object) this.btnRemove_, (Object) null))
      EventDelegate.Set(this.btnRemove_.onClick, (EventDelegate.Callback) (() =>
      {
        if (Object.op_Equality((Object) this.menu_.currentOrnament_, (Object) null) || this.waitAndSet())
          return;
        this.menu_.returnStorageWithEffect();
      }));
    if (!Object.op_Inequality((Object) this.btnToLayout_, (Object) null))
      return;
    EventDelegate.Set(this.btnToLayout_.onClick, (EventDelegate.Callback) (() =>
    {
      MapEditOrnament currentOrnament = this.menu_.currentOrnament_;
      if (Object.op_Equality((Object) currentOrnament, (Object) null) || this.waitAndSet())
        return;
      this.menu_.startLayout(currentOrnament);
    }));
  }

  private IEnumerator Start()
  {
    Future<GameObject> ldPrefab = MapEdit.Prefabs.facility_thumb.Load<GameObject>();
    IEnumerator e = ldPrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.prefabIcon_ = ldPrefab.Result;
    ldPrefab = (Future<GameObject>) null;
    ldPrefab = Res.Icons.SkillGenreIcon.Load<GameObject>();
    e = ldPrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.prefabGenre_ = ldPrefab.Result;
    ldPrefab = (Future<GameObject>) null;
  }

  private void OnEnable()
  {
    this.isWait_ = false;
    this.isReboot_ = true;
  }

  private void Update()
  {
    if (Object.op_Equality((Object) this.menu_, (Object) null))
      return;
    MapEditOrnament currentOrnament = this.menu_.currentOrnament_;
    bool flag = this.menu_.enabledInformation_ && Object.op_Inequality((Object) currentOrnament, (Object) null);
    if (this.isActiveInformation_ != flag && !flag)
    {
      this.setActiveInformation(false);
      this.menu_.isActiveButtonStorage_ = true;
      this.menu_.resetDrawEffectArea();
    }
    if (!this.isActiveInformation_)
      return;
    if (!this.isWait_ && Object.op_Inequality((Object) this.current_, (Object) currentOrnament))
    {
      if (Object.op_Equality((Object) this.current_, (Object) null) || this.current_.facility_ != currentOrnament.facility_)
      {
        this.updateInformation(currentOrnament);
      }
      else
      {
        this.current_ = currentOrnament;
        if (this.trackEffectArea_ != null)
        {
          this.trackEffectArea_ = new MapEditFacilityInformation.TrackEffectArea(this.trackEffectArea_.target_, currentOrnament.row_, currentOrnament.column_, this.trackEffectArea_.minRange_, this.trackEffectArea_.maxRange_);
          this.isReboot_ = true;
        }
      }
    }
    if (!this.isReboot_)
      return;
    this.drawEffectArea();
    this.isReboot_ = true;
  }

  public void updateInformation(MapEditOrnament ornament)
  {
    this.current_ = ornament;
    if (Object.op_Equality((Object) ornament, (Object) null) || ornament.facility_ == null)
    {
      this.menu_.isActiveButtonStorage_ = true;
      this.resetDrawEffectArea();
    }
    else
    {
      this.menu_.isActiveButtonStorage_ = false;
      UnitUnit unit = ornament.facility_.unit;
      MapFacility master = ornament.facility_.master;
      PlayerUnit facilityUnit = PlayerUnit.FromFacility(unit, ornament.facility_.id);
      Judgement.NonBattleParameter nonbattleParameter = facilityUnit.nonbattleParameter;
      this.txtName_.SetTextLocalize(unit.name);
      this.txtDescription_.SetTextLocalize(unit.description);
      this.txtHP_.SetTextLocalize(nonbattleParameter.Hp);
      this.txtDEF_.SetTextLocalize(nonbattleParameter.PhysicalDefense);
      this.txtINT_.SetTextLocalize(nonbattleParameter.MagicDefense);
      ((IEnumerable<GameObject>) this.iconPassage_).ToggleOnceEx(master.is_puton ? 1 : 0);
      ((IEnumerable<GameObject>) this.iconDestruction_).ToggleOnceEx(master.is_target ? 1 : 0);
      ((IEnumerable<GameObject>) this.iconVisibility_).ToggleOnceEx(master.is_view ? 1 : 0);
      this.txtCost_.SetTextLocalize(unit.cost);
      if (Object.op_Inequality((Object) this.iconGenre0_, (Object) null))
      {
        Object.Destroy((Object) this.iconGenre0_);
        this.iconGenre0_ = (GameObject) null;
      }
      if (Object.op_Inequality((Object) this.iconGenre1_, (Object) null))
      {
        Object.Destroy((Object) this.iconGenre1_);
        this.iconGenre1_ = (GameObject) null;
      }
      BattleskillSkill skill = ((IEnumerable<PlayerUnitSkills>) unit.facilitySkills).FirstOrDefault<PlayerUnitSkills>()?.skill;
      if (Object.op_Inequality((Object) this.prefabGenre_, (Object) null) && skill != null)
      {
        BattleskillGenre? genre1 = skill.genre1;
        if (genre1.HasValue)
        {
          this.iconGenre0_ = this.prefabGenre_.Clone(this.lnkProperty0_.transform);
          UIWidget component1 = this.lnkProperty0_.GetComponent<UIWidget>();
          SkillGenreIcon component2 = this.iconGenre0_.GetComponent<SkillGenreIcon>();
          component2.Init(genre1);
          if (Object.op_Inequality((Object) component1, (Object) null))
            component2.SetSize(Mathf.RoundToInt(component1.localSize.x), Mathf.RoundToInt(component1.localSize.y));
          NGUITools.AdjustDepth(this.iconGenre0_, this.depthProperty_);
        }
        BattleskillGenre? genre2 = skill.genre2;
        if (genre2.HasValue)
        {
          this.iconGenre1_ = this.prefabGenre_.Clone(this.lnkProperty1_.transform);
          UIWidget component3 = this.lnkProperty1_.GetComponent<UIWidget>();
          SkillGenreIcon component4 = this.iconGenre1_.GetComponent<SkillGenreIcon>();
          component4.Init(genre2);
          if (Object.op_Inequality((Object) component3, (Object) null))
            component4.SetSize(Mathf.RoundToInt(component3.localSize.x), Mathf.RoundToInt(component3.localSize.y));
          NGUITools.AdjustDepth(this.iconGenre1_, this.depthProperty_);
        }
      }
      int minRange = 0;
      int maxRange = 0;
      BattleskillTargetType target = BattleskillTargetType.myself;
      bool flag = false;
      if (skill != null)
      {
        target = skill.GetRangeEffect(out minRange, out maxRange);
        switch (target)
        {
          case BattleskillTargetType.player_range:
            flag = true;
            break;
          case BattleskillTargetType.enemy_range:
            flag = true;
            break;
          case BattleskillTargetType.complex_range:
            flag = true;
            break;
        }
      }
      if (flag)
      {
        this.trackEffectArea_ = new MapEditFacilityInformation.TrackEffectArea(target, ornament.row_, ornament.column_, minRange, maxRange);
        this.drawEffectArea();
        this.txtArea_.SetTextLocalize(Consts.Format(Consts.GetInstance().MAPEDIT_031_VALUE_RANGE_EFFECT, (IDictionary) new Hashtable()
        {
          {
            (object) "min",
            (object) minRange
          },
          {
            (object) "max",
            (object) maxRange
          }
        }));
      }
      else
      {
        this.resetDrawEffectArea();
        this.txtArea_.SetTextLocalize(Consts.GetInstance().COMMON_NOVALUE);
      }
      if (((Component) this).gameObject.activeInHierarchy)
        this.StartCoroutine(this.doUpdateInformation(facilityUnit));
      else
        Singleton<NGSceneManager>.GetInstance().StartCoroutine(this.doUpdateInformation(facilityUnit));
    }
  }

  private void drawEffectArea()
  {
    if (this.trackEffectArea_ == null)
      return;
    this.menu_.setDrawEffectArea(this.trackEffectArea_.row_, this.trackEffectArea_.column_, this.trackEffectArea_.minRange_, this.trackEffectArea_.maxRange_, this.trackEffectArea_.target_);
  }

  private void resetDrawEffectArea()
  {
    this.menu_.resetDrawEffectArea();
    this.trackEffectArea_ = (MapEditFacilityInformation.TrackEffectArea) null;
  }

  private IEnumerator doUpdateInformation(PlayerUnit facilityUnit)
  {
    this.lnkIcon_.transform.Clear();
    this.lnkIcon_.SetActive(false);
    GameObject go = this.prefabIcon_.Clone(this.lnkIcon_.transform);
    FacilityIcon icon = go.GetComponent<FacilityIcon>();
    IEnumerator e = icon.SetUnit(facilityUnit, true, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UIWidget component = this.lnkIcon_.GetComponent<UIWidget>();
    icon.SetSize(Mathf.RoundToInt(component.localSize.x), Mathf.RoundToInt(component.localSize.y));
    this.lnkIcon_.SetActive(true);
    NGUITools.AdjustDepth(go, component.depth + 1);
  }

  private class TrackEffectArea
  {
    public BattleskillTargetType target_ { get; private set; }

    public int row_ { get; private set; }

    public int column_ { get; private set; }

    public int minRange_ { get; private set; }

    public int maxRange_ { get; private set; }

    public TrackEffectArea(
      BattleskillTargetType target,
      int row,
      int column,
      int minRange,
      int maxRange)
    {
      this.target_ = target;
      this.row_ = row;
      this.column_ = column;
      this.minRange_ = minRange;
      this.maxRange_ = maxRange;
    }
  }
}
