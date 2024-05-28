// Decompiled with JetBrains decompiler
// Type: EditCustomDeckUnitPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using CustomDeck;
using EquipmentRules;
using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/CustomDeck/UnitPanel")]
public class EditCustomDeckUnitPanel : MonoBehaviour
{
  [SerializeField]
  private Transform lnkUnit_;
  [SerializeField]
  private Transform[] lnkGears_;
  [SerializeField]
  private GameObject[] lockGears_;
  [SerializeField]
  private UILabel txtVertex_;
  [SerializeField]
  private Transform[] lnkEquippedUnits_;
  [SerializeField]
  private GameObject[] objLockEquippedUnits_;
  [SerializeField]
  private Transform lnkAwakeSkill_;
  [SerializeField]
  private int depthAwakeSkill_;
  [SerializeField]
  [Tooltip("オーバーキラーズ枠、ディセーブル表示のコントロール")]
  private EditCustomDeckUnitPanel.DisabledParts[] dpEquippedUnits_;
  [SerializeField]
  private UIButton btnEdit_;
  [SerializeField]
  private GameObject[] topStatus_;
  [SerializeField]
  private GameObject[] disabledTopStatus_;
  private UnitIcon iconUnit_;
  private UnitIcon[] iconEquippedUnits_;
  private ItemIcon[] iconGears_;
  private BattleSkillIcon iconAwakeSkill_;
  private List<GameObject> objects_ = new List<GameObject>();

  public EditCustomDeckPanel panel { get; set; }

  public int index { get; set; }

  public PlayerUnit lastPlayerUnit { get; set; }

  public GameObject objIcon => ((Component) this.iconUnit_).gameObject;

  public void setBlank()
  {
    ((IEnumerable<GameObject>) this.topStatus_).SetActives(false);
    ((IEnumerable<GameObject>) this.disabledTopStatus_).Where<GameObject>((Func<GameObject, bool>) (go => Object.op_Inequality((Object) go, (Object) null))).SetActives(true);
    if (!Object.op_Implicit((Object) this.iconUnit_))
    {
      this.iconUnit_ = this.cloneObject<UnitIcon>(this.panel.menu.prefabUnitIcon, this.lnkUnit_);
      this.iconUnit_.SetIconBoxCollider(false);
    }
    this.setBlankUnit();
    if (this.iconGears_ == null)
    {
      this.iconGears_ = new ItemIcon[this.lnkGears_.Length];
      for (int index = 0; index < this.iconGears_.Length; ++index)
        this.iconGears_[index] = this.cloneObject<ItemIcon>(this.panel.menu.prefabItemIcon, this.lnkGears_[index]);
    }
    for (int no = 0; no < this.iconGears_.Length; ++no)
    {
      this.iconGears_[no].SetModeGear();
      this.setBlankGear(no, false);
    }
    ((IEnumerable<GameObject>) this.lockGears_).ToggleOnceEx(-1);
    if (this.iconEquippedUnits_ == null)
    {
      this.iconEquippedUnits_ = new UnitIcon[this.lnkEquippedUnits_.Length];
      for (int index = 0; index < this.iconEquippedUnits_.Length; ++index)
      {
        this.iconEquippedUnits_[index] = this.cloneObject<UnitIcon>(this.panel.menu.prefabUnitIcon, this.lnkEquippedUnits_[index]);
        this.iconEquippedUnits_[index].BottomModeValue = UnitIconBase.BottomMode.Level;
        this.iconEquippedUnits_[index].SetIconBoxCollider(false);
      }
    }
    for (int index = 0; index < this.dpEquippedUnits_.Length; ++index)
      this.dpEquippedUnits_[index].isDisabled = true;
    if (!Object.op_Implicit((Object) this.iconAwakeSkill_))
    {
      this.iconAwakeSkill_ = this.cloneObject<BattleSkillIcon>(this.panel.menu.prefabSkillIcon, this.lnkAwakeSkill_);
      foreach (UIWidget componentsInChild in ((Component) this.iconAwakeSkill_).GetComponentsInChildren<UIWidget>())
        componentsInChild.depth += this.depthAwakeSkill_;
    }
    this.lastPlayerUnit = (PlayerUnit) null;
    ((UIButtonColor) this.btnEdit_).isEnabled = false;
  }

  private void setBlankUnit()
  {
    this.iconUnit_.SetEmpty();
    this.iconUnit_.SelectUnit = true;
    this.iconUnit_.UnitUsed = false;
    this.txtVertex_.SetTextLocalize(this.panel.menu.jobRankLabels[0]);
  }

  private void setBlankGear(int no, bool bDisp, bool bJingi = false)
  {
    ItemIcon iconGear = this.iconGears_[no];
    ((Component) iconGear).gameObject.SetActive(bDisp);
    if (!bDisp)
      return;
    iconGear.SetEmpty(true, bJingi);
    iconGear.gear.type.SetActive(false);
    iconGear.isButtonActive = false;
  }

  private void setBlankEquippedUnit(int no)
  {
    this.dpEquippedUnits_[no].isDisabled = true;
    ((Component) this.iconEquippedUnits_[no]).gameObject.SetActive(false);
    this.objLockEquippedUnits_[no].gameObject.SetActive(false);
  }

  private void setBlankAwakeSkill(bool bDisp)
  {
    ((Component) this.iconAwakeSkill_).gameObject.SetActive(false);
  }

  public IEnumerator doInitialize(PlayerCustomDeckUnit_parameter_list param)
  {
    EditCustomDeckUnitPanel customDeckUnitPanel1 = this;
    if (param == null || param.checkBlank())
    {
      customDeckUnitPanel1.setBlank();
    }
    else
    {
      PlayerUnit[] units = customDeckUnitPanel1.panel.menu.playerUnits;
      PlayerAwakeSkill[] awakeSkills = customDeckUnitPanel1.panel.menu.awakeSkills;
      PlayerUnit pu = param.createPlayerUnit(units);
      ((UIButtonColor) customDeckUnitPanel1.btnEdit_).isEnabled = pu != (PlayerUnit) null;
      IEnumerator e = customDeckUnitPanel1.doUpdateUnit(pu);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (pu != (PlayerUnit) null)
        customDeckUnitPanel1.lastPlayerUnit = pu;
      EditCustomDeckUnitPanel customDeckUnitPanel2 = customDeckUnitPanel1;
      PlayerUnit playerUnit = pu;
      if ((object) playerUnit == null)
        playerUnit = customDeckUnitPanel1.lastPlayerUnit;
      PlayerCustomDeckUnit_parameter_list unitParameterList = param;
      e = customDeckUnitPanel2.doSetGears(playerUnit, unitParameterList);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      int oIndex;
      for (oIndex = 0; oIndex < customDeckUnitPanel1.iconEquippedUnits_.Length && oIndex < param.over_killers_ids.Length; ++oIndex)
      {
        customDeckUnitPanel1.dpEquippedUnits_[oIndex].isDisabled = false;
        bool flag = true;
        int oId = param.over_killers_ids[oIndex];
        switch (oId)
        {
          case -1:
            ((Component) customDeckUnitPanel1.iconEquippedUnits_[oIndex]).gameObject.SetActive(false);
            break;
          case 0:
            ((Component) customDeckUnitPanel1.iconEquippedUnits_[oIndex]).gameObject.SetActive(false);
            flag = false;
            break;
          default:
            ((Component) customDeckUnitPanel1.iconEquippedUnits_[oIndex]).gameObject.SetActive(true);
            e = customDeckUnitPanel1.iconEquippedUnits_[oIndex].SetPlayerUnit(Array.Find<PlayerUnit>(units, (Predicate<PlayerUnit>) (x => x.id == oId)), (PlayerUnit[]) null, (PlayerUnit) null, false, false);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            customDeckUnitPanel1.iconEquippedUnits_[oIndex].ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Rarity);
            flag = false;
            break;
        }
        customDeckUnitPanel1.objLockEquippedUnits_[oIndex].SetActive(flag);
      }
      for (; oIndex < customDeckUnitPanel1.iconEquippedUnits_.Length; ++oIndex)
        customDeckUnitPanel1.setBlankEquippedUnit(oIndex);
      if (param.awake_skill_id != 0)
      {
        ((Component) customDeckUnitPanel1.iconAwakeSkill_).gameObject.SetActive(true);
        e = customDeckUnitPanel1.iconAwakeSkill_.Init(Array.Find<PlayerAwakeSkill>(awakeSkills, (Predicate<PlayerAwakeSkill>) (x => x.id == param.awake_skill_id)).masterData);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
        customDeckUnitPanel1.setBlankAwakeSkill(pu == (PlayerUnit) null || pu.can_equip_awake_skill);
    }
  }

  private IEnumerator doSetGears(PlayerUnit playerUnit, PlayerCustomDeckUnit_parameter_list param)
  {
    EditCustomDeckUnitPanel customDeckUnitPanel1 = this;
    PlayerItem[] playerGears = customDeckUnitPanel1.panel.menu.playerGears;
    int[] imap = playerUnit?.equippedGearIndexMapByCustomDeck;
    PlayerUnit playerUnit1 = playerUnit;
    bool flag = (object) playerUnit1 != null && playerUnit1.isPossibleEquippedGear3;
    bool bOpenedEquippedGear3 = flag && playerUnit.isOpenedEquippedGear3;
    bool bAwakedUnit = bOpenedEquippedGear3 && playerUnit.unit.awake_unit_flag;
    bool bDisp = true;
    PlayerItem[] tmpGears = new PlayerItem[3]
    {
      param.getGear(playerGears, 0),
      param.getGear(playerGears, 1),
      param.getGear(playerGears, 2)
    };
    int? jingiIndex = customDeckUnitPanel1.getJingiIndex(tmpGears, bAwakedUnit, bOpenedEquippedGear3);
    int lockIndex = !flag || bOpenedEquippedGear3 ? -1 : imap.Length;
    int n = 0;
    while (n < customDeckUnitPanel1.iconGears_.Length)
    {
      if (bDisp)
      {
        int[] numArray = imap;
        int index = numArray != null ? numArray[n] : 0;
        PlayerItem gear = tmpGears[index];
        if (gear != (PlayerItem) null)
        {
          int reisouId = param.getReisouId(index);
          if (reisouId != 0)
          {
            gear = gear.Clone();
            gear.equipped_reisou_player_gear_id = reisouId;
          }
          IEnumerator e = customDeckUnitPanel1.doSetGear(n, gear);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        else
        {
          EditCustomDeckUnitPanel customDeckUnitPanel2 = customDeckUnitPanel1;
          int no = n;
          int num1 = index;
          int? nullable = jingiIndex;
          int valueOrDefault = nullable.GetValueOrDefault();
          int num2 = num1 == valueOrDefault & nullable.HasValue ? 1 : 0;
          customDeckUnitPanel2.setBlankGear(no, true, num2 != 0);
        }
        ++n;
        if (imap.IsNullOrLess<int>(n + 1))
          bDisp = false;
      }
      else if (lockIndex == n)
      {
        customDeckUnitPanel1.setBlankGear(n, true);
        ++n;
      }
      else
      {
        customDeckUnitPanel1.setBlankGear(n, false);
        ++n;
      }
    }
    ((IEnumerable<GameObject>) customDeckUnitPanel1.lockGears_).ToggleOnceEx(lockIndex);
  }

  private int? getJingiIndex(PlayerItem[] gears, bool bAwakedUnit, bool bOpenedEquippedGear3)
  {
    return !bOpenedEquippedGear3 || !((IEnumerable<PlayerItem>) gears).Any<PlayerItem>((Func<PlayerItem, bool>) (x => x != (PlayerItem) null)) ? new int?() : Gears.getIndexJingiSpace(((IEnumerable<PlayerItem>) gears).Select<PlayerItem, GearGear>((Func<PlayerItem, GearGear>) (x => x?.gear)).ToArray<GearGear>(), bAwakedUnit, true);
  }

  public IEnumerator doUpdateUnit(PlayerUnit target)
  {
    if (target != (PlayerUnit) null)
    {
      IEnumerator e = this.iconUnit_.SetPlayerUnit(target, (PlayerUnit[]) null, (PlayerUnit) null, false, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.iconUnit_.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Rarity);
      if (this.panel.menu.usedIds != null)
        this.iconUnit_.UnitUsed = ((IEnumerable<int>) this.panel.menu.usedIds).Contains<int>(target.id);
      this.txtVertex_.SetTextLocalize(this.panel.menu.jobRankLabels[(int) Util.getJobRank(target, target.job_id)]);
      UnitUnit unit = target.unit;
      this.topStatus_[0].SetActive(JobChangeUtil.getJobChangePatterns(unit.ID, unit.job_UnitJob) != null);
      this.topStatus_[1].SetActive(unit.exist_overkillers_slot);
      this.topStatus_[2].SetActive(target.can_equip_awake_skill);
    }
    else
    {
      this.setBlankUnit();
      ((IEnumerable<GameObject>) this.topStatus_).SetActives(true);
    }
    for (int index = 0; index < this.topStatus_.Length; ++index)
    {
      if (Object.op_Implicit((Object) this.disabledTopStatus_[index]))
        this.disabledTopStatus_[index].SetActive(!this.topStatus_[index].activeSelf);
    }
  }

  public IEnumerator doUpdateJob(PlayerUnit target)
  {
    EditCustomDeckUnitPanel customDeckUnitPanel1 = this;
    IEnumerator e = customDeckUnitPanel1.doUpdateUnit(target);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!(target == (PlayerUnit) null))
    {
      int[] indexMapByCustomDeck = target.equippedGearIndexMapByCustomDeck;
      PlayerItem[] gears = new PlayerItem[3]
      {
        target.equippedGear,
        target.equippedGear2,
        target.equippedGear3
      };
      int? jingiIndex = customDeckUnitPanel1.getJingiIndex(gears, target.unit.awake_unit_flag, target.isOpenedEquippedGear3);
      for (int index1 = 0; index1 < indexMapByCustomDeck.Length; ++index1)
      {
        int index2 = indexMapByCustomDeck[index1];
        if (gears[index2] == (PlayerItem) null)
        {
          EditCustomDeckUnitPanel customDeckUnitPanel2 = customDeckUnitPanel1;
          int no = index1;
          int num1 = index2;
          int? nullable = jingiIndex;
          int valueOrDefault = nullable.GetValueOrDefault();
          int num2 = num1 == valueOrDefault & nullable.HasValue ? 1 : 0;
          customDeckUnitPanel2.setBlankGear(no, true, num2 != 0);
        }
      }
    }
  }

  private IEnumerator doSetGear(int gearIndex, PlayerItem gear)
  {
    ItemIcon icon = this.iconGears_[gearIndex];
    ((Component) icon).gameObject.SetActive(true);
    icon.SetEmpty(false);
    icon.setEquipPlus(false);
    icon.isButtonActive = false;
    IEnumerator e = icon.InitByItemInfo(new GameCore.ItemInfo(gear, true));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    icon.resetExpireDate();
  }

  private T cloneObject<T>(GameObject prefab, Transform parent) where T : MonoBehaviour
  {
    GameObject gameObject = prefab.Clone(parent);
    this.objects_.Add(gameObject);
    return gameObject.GetComponent<T>();
  }

  public void onClickedEdit() => this.panel.menu.onClickedEdit(this.index);

  public void onClickedUnit() => this.panel.menu.onClickedSelectUnit(this.index);

  public void onLongPressedUnit() => this.panel.menu.onLongPressedUnit(this.index);

  [Serializable]
  private class DisabledParts
  {
    private bool initialized_;
    private bool disabled_;
    public UIWidget[] objects;
    public GameObject[] offObjects;

    public bool isDisabled
    {
      get => this.disabled_;
      set
      {
        if (this.initialized_ && this.disabled_ == value)
          return;
        this.initialized_ = true;
        this.disabled_ = value;
        if (value)
        {
          ((IEnumerable<UIWidget>) this.objects).SetActives<UIWidget>(true);
          ((IEnumerable<GameObject>) this.offObjects).SetActives(false);
        }
        else
        {
          ((IEnumerable<UIWidget>) this.objects).SetActives<UIWidget>(false);
          ((IEnumerable<GameObject>) this.offObjects).SetActives(true);
        }
      }
    }
  }

  private enum Top
  {
    Vertex,
    Overkillers,
    AwakeSkill,
  }
}
