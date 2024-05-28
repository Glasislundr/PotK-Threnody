// Decompiled with JetBrains decompiler
// Type: MapEditFacilityList
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
public class MapEditFacilityList : MonoBehaviour
{
  [SerializeField]
  private UIButton btnSelect_;
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
  [Tooltip("機能[OFF, ON]順にセット")]
  private GameObject[] iconPassage_;
  [SerializeField]
  [Tooltip("機能[OFF, ON]順にセット")]
  private GameObject[] iconDestruction_;
  [SerializeField]
  [Tooltip("機能[OFF, ON]順にセット")]
  private GameObject[] iconVisibility_;
  [SerializeField]
  private GameObject[] topSwitch_;
  [SerializeField]
  [Tooltip("ボタンが押せない情報表示をセット0:全て設置済み/1:コストオーバー/2:設置可能な場所無し")]
  private GameObject[] toggleError_;
  [SerializeField]
  private MapEditFacilityList.DiffLayout layoutEdit_;
  [SerializeField]
  private MapEditFacilityList.DiffLayout layoutNormal_;
  private bool initailzed_;
  private bool wait_;
  private BL.StructValue<int> valueUsed_ = new BL.StructValue<int>(0);
  private BL.BattleModified<BL.StructValue<int>> modUsed_;
  private BL.BattleModified<BL.StructValue<int>> modRemainingCost_;
  private BL.BattleModified<BL.StructValue<bool>> modNotLocate_;
  private MapEditFacilityList.DiffLayout currentLayout_;
  private FacilityIcon facilityIcon_;
  private const int INDEX_OFF = 0;
  private const int INDEX_ON = 1;
  private const int DEPTH_INTERVAL = 1;

  public PlayerGuildFacility facility_ { get; private set; }

  public int ID_ => this.facility_ == null ? -1 : this.facility_._master;

  public int cost_ { get; private set; }

  public int used_
  {
    get => this.valueUsed_.value;
    set
    {
      if (this.valueUsed_.value == value)
        return;
      this.valueUsed_.value = value;
    }
  }

  private int remainingCost_
  {
    get => this.modRemainingCost_ == null ? int.MaxValue : this.modRemainingCost_.value.value;
  }

  private bool isChangedRemainingCost_
  {
    get => this.modRemainingCost_ != null && this.modRemainingCost_.isChanged;
  }

  private bool isNotLocate_ => this.modNotLocate_ != null && this.modNotLocate_.value.value;

  private bool isChangedNotLocate_ => this.modNotLocate_ != null && this.modNotLocate_.isChanged;

  public void preInitialize(bool isLayoutEdit)
  {
    if (isLayoutEdit)
    {
      this.layoutEdit_.enabled_ = true;
      this.layoutNormal_.enabled_ = false;
      this.currentLayout_ = this.layoutEdit_;
    }
    else
    {
      this.layoutEdit_.enabled_ = false;
      this.layoutNormal_.enabled_ = true;
      this.currentLayout_ = this.layoutNormal_;
    }
  }

  public IEnumerator initialize(
    GameObject prefabIcon,
    GameObject prefabGenre,
    PlayerGuildFacility facility,
    Action<PlayerGuildFacility> eventSelect,
    int used,
    BL.StructValue<int> remainingCost = null,
    BL.StructValue<bool> notLocate = null)
  {
    this.initailzed_ = false;
    if (this.currentLayout_ == null)
      this.preInitialize(true);
    this.wait_ = true;
    this.modUsed_ = BL.Observe<BL.StructValue<int>>(this.valueUsed_);
    this.facility_ = facility;
    this.cost_ = 0;
    ((IEnumerable<GameObject>) this.toggleError_).ToggleOnceEx(-1);
    EventDelegate.Set(this.btnSelect_.onClick, (EventDelegate.Callback) (() =>
    {
      if (this.wait_)
        return;
      this.wait_ = true;
      if (this.modUsed_.isChanged || this.isChangedRemainingCost_ || this.isChangedNotLocate_)
        return;
      eventSelect(this.facility_);
    }));
    if (facility != null)
    {
      UnitUnit u = facility.unit;
      MapFacility master = facility.master;
      if (u != null && master != null)
      {
        this.cost_ = u.cost;
        this.used_ = used;
        this.modRemainingCost_ = remainingCost != null ? BL.Observe<BL.StructValue<int>>(remainingCost) : (BL.BattleModified<BL.StructValue<int>>) null;
        this.modNotLocate_ = notLocate != null ? BL.Observe<BL.StructValue<bool>>(notLocate) : (BL.BattleModified<BL.StructValue<bool>>) null;
        this.txtName_.SetTextLocalize(u.name);
        this.txtDescription_.SetTextLocalize(u.description);
        PlayerUnit pu = PlayerUnit.FromFacility(u, facility.id);
        Judgement.NonBattleParameter nonbattleParameter = pu.nonbattleParameter;
        this.txtHP_.SetTextLocalize(nonbattleParameter.Hp);
        this.txtDEF_.SetTextLocalize(nonbattleParameter.PhysicalDefense);
        this.txtINT_.SetTextLocalize(nonbattleParameter.MagicDefense);
        ((IEnumerable<GameObject>) this.iconPassage_).ToggleOnceEx(master.is_puton ? 1 : 0);
        ((IEnumerable<GameObject>) this.iconDestruction_).ToggleOnceEx(master.is_target ? 1 : 0);
        ((IEnumerable<GameObject>) this.iconVisibility_).ToggleOnceEx(master.is_view ? 1 : 0);
        this.updateQuantity();
        this.modUsed_.isChangedOnce();
        if (this.modRemainingCost_ != null)
          this.modRemainingCost_.isChangedOnce();
        this.currentLayout_.setCost(this.cost_);
        GameObject goIcon = prefabIcon.Clone(this.lnkIcon_.transform);
        this.facilityIcon_ = goIcon.GetComponent<FacilityIcon>();
        IEnumerator e = this.facilityIcon_.SetUnit(pu, true, true);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        UIWidget component1 = this.lnkIcon_.GetComponent<UIWidget>();
        this.facilityIcon_.SetSize(Mathf.RoundToInt(component1.localSize.x), Mathf.RoundToInt(component1.localSize.y));
        NGUITools.AdjustDepth(goIcon, component1.depth + 1);
        PlayerUnitSkills playerUnitSkills = ((IEnumerable<PlayerUnitSkills>) u.facilitySkills).FirstOrDefault<PlayerUnitSkills>();
        if (playerUnitSkills != null && Object.op_Inequality((Object) prefabGenre, (Object) null))
        {
          BattleskillSkill skill = playerUnitSkills.skill;
          BattleskillGenre? genre1 = skill.genre1;
          if (genre1.HasValue)
          {
            GameObject gameObject = prefabGenre.Clone(this.lnkProperty0_.transform);
            UIWidget component2 = this.lnkProperty0_.GetComponent<UIWidget>();
            SkillGenreIcon component3 = gameObject.GetComponent<SkillGenreIcon>();
            component3.Init(genre1);
            if (Object.op_Inequality((Object) component2, (Object) null))
              component3.SetSize(Mathf.RoundToInt(component2.localSize.x), Mathf.RoundToInt(component2.localSize.y));
            NGUITools.AdjustDepth(gameObject, this.depthProperty_);
          }
          BattleskillGenre? genre2 = skill.genre2;
          if (genre2.HasValue)
          {
            GameObject gameObject = prefabGenre.Clone(this.lnkProperty1_.transform);
            UIWidget component4 = this.lnkProperty1_.GetComponent<UIWidget>();
            SkillGenreIcon component5 = gameObject.GetComponent<SkillGenreIcon>();
            component5.Init(genre2);
            if (Object.op_Inequality((Object) component4, (Object) null))
              component5.SetSize(Mathf.RoundToInt(component4.localSize.x), Mathf.RoundToInt(component4.localSize.y));
            NGUITools.AdjustDepth(gameObject, this.depthProperty_);
          }
        }
        this.updateButton();
        this.initailzed_ = true;
      }
    }
  }

  public void updateQuantity()
  {
    this.currentLayout_.setQuantity(this.facility_.hasnum, this.used_);
  }

  private void updateButton()
  {
    bool flag = !this.isNotLocate_ && this.facility_.hasnum > this.used_ && this.cost_ <= this.remainingCost_;
    ((UIButtonColor) this.btnSelect_).isEnabled = flag;
    if (Object.op_Inequality((Object) this.facilityIcon_, (Object) null))
      this.facilityIcon_.Gray = !flag;
    MapEditFacilityList.ErrorIndex index = MapEditFacilityList.ErrorIndex.None;
    if (this.facility_.hasnum <= this.used_)
      index = MapEditFacilityList.ErrorIndex.Soldout;
    else if (this.cost_ > this.remainingCost_)
      index = MapEditFacilityList.ErrorIndex.CostOver;
    else if (this.isNotLocate_)
      index = MapEditFacilityList.ErrorIndex.NoSpace;
    ((IEnumerable<GameObject>) this.toggleError_).ToggleOnceEx((int) index);
  }

  public void changeDraw(int index)
  {
    if (this.topSwitch_ == null)
      return;
    ((IEnumerable<GameObject>) this.topSwitch_).ToggleOnceEx(index);
  }

  private void Update()
  {
    if (!this.initailzed_)
      return;
    this.wait_ = false;
    int num = this.modUsed_.isChangedOnce() ? 1 : 0;
    bool flag1 = this.modRemainingCost_ != null && this.modRemainingCost_.isChangedOnce();
    bool flag2 = this.modNotLocate_ != null && this.modNotLocate_.isChangedOnce();
    if (num != 0)
      this.updateQuantity();
    if ((num | (flag1 ? 1 : 0) | (flag2 ? 1 : 0)) == 0)
      return;
    this.updateButton();
  }

  [Serializable]
  public class DiffLayout
  {
    [SerializeField]
    private GameObject top_;
    [SerializeField]
    private UILabel txtCost_;
    [SerializeField]
    private UILabel txtQuantity_;
    [SerializeField]
    private bool enabledUsed_ = true;

    public bool enabled_
    {
      get => this.top_.activeSelf;
      set => this.top_.SetActive(value);
    }

    public void setCost(int cost) => this.txtCost_.SetTextLocalize(cost);

    public void setQuantity(int quantity, int used = 0)
    {
      if (this.enabledUsed_)
        this.txtQuantity_.SetTextLocalize(Consts.Format(quantity > used ? Consts.GetInstance().MAPEDIT_031_FACILITY_QUANTITY : Consts.GetInstance().MAPEDIT_031_FACILITY_QUANTITY_RED_NORMAL, (IDictionary) new Hashtable()
        {
          {
            (object) nameof (used),
            (object) used
          },
          {
            (object) "hasnum",
            (object) quantity
          }
        }));
      else
        this.txtQuantity_.SetTextLocalize(quantity);
    }
  }

  private enum ErrorIndex
  {
    None = -1, // 0xFFFFFFFF
    Soldout = 0,
    CostOver = 1,
    NoSpace = 2,
  }
}
