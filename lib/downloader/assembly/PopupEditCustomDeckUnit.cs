// Decompiled with JetBrains decompiler
// Type: PopupEditCustomDeckUnit
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
[AddComponentMenu("Scenes/CustomDeck/Popup/EditCustomDeckUnit")]
public class PopupEditCustomDeckUnit : MonoBehaviour
{
  [SerializeField]
  private UI2DSprite sprUnit_;
  [SerializeField]
  private UISprite sprUnitType_;
  [SerializeField]
  private UI2DSprite sprRarity_;
  [SerializeField]
  private UI2DSprite sprGearKind_;
  [SerializeField]
  private UILabel txtLevel_;
  [SerializeField]
  private UILabel txtPosition_;
  [SerializeField]
  private UILabel txtDeckName_;
  [SerializeField]
  [Tooltip("項目先頭[0:クラスチェンジ/1:オーバーキラーズ/2:付け替えスキル]")]
  private GameObject[] topItems_;
  [SerializeField]
  private Transform[] lnkGears_;
  [SerializeField]
  private GameObject[] lockGears_;
  [SerializeField]
  private Transform[] lnkReisous_;
  [SerializeField]
  private GameObject[] lockReisous_;
  [SerializeField]
  private Sprite sprAddReisou;
  [SerializeField]
  private PopupEditCustomDeckUnit.JobChangeControl jobChange_;
  [SerializeField]
  private UILabel txtJobName_;
  [SerializeField]
  private PopupEditCustomDeckUnit.OverkillersSlot[] overkillersSlots_;
  [SerializeField]
  private Transform lnkAwakeSkill_;
  [SerializeField]
  private int depthAwakeSkill_;
  private EditCustomDeckMenu menu_;
  private PlayerCustomDeckUnit_parameter_list param_;
  private PlayerUnit playerUnit_;
  private UnitUnit unit_;
  private int jobId_;
  private int jobIndex_;
  private MasterDataTable.UnitJob[] jobs_;
  private bool isUpdating_;
  private int[] gearIndexMap_;
  private ItemIcon[] iconGears_;
  private ItemIcon[] iconReisous_;
  private int[] gearIds_;
  private int[] reisouIds_;
  private int? jingiIndex_;

  public IEnumerator doInitialize(
    EditCustomDeckMenu menu,
    PlayerCustomDeckUnit_parameter_list param)
  {
    PopupEditCustomDeckUnit editCustomDeckUnit = this;
    editCustomDeckUnit.menu_ = menu;
    editCustomDeckUnit.param_ = param;
    editCustomDeckUnit.playerUnit_ = param.createPlayerUnit(menu.playerUnits, menu.playerGears, menu.awakeSkills);
    editCustomDeckUnit.unit_ = editCustomDeckUnit.playerUnit_.unit;
    editCustomDeckUnit.jobId_ = -1;
    Consts instance = Consts.GetInstance();
    editCustomDeckUnit.txtPosition_.SetTextLocalize(Consts.Format(instance.CUSTOMDECK_POSITION, (IDictionary) new Hashtable()
    {
      {
        (object) "pos",
        (object) (param.index + 1)
      }
    }));
    editCustomDeckUnit.txtLevel_.SetTextLocalize(string.Format("{0}/{1}", (object) editCustomDeckUnit.playerUnit_.total_level, (object) editCustomDeckUnit.playerUnit_.total_max_level));
    editCustomDeckUnit.txtDeckName_.SetTextLocalize(menu.currentDeck.name);
    editCustomDeckUnit.sprGearKind_.sprite2D = GearKindIcon.LoadSprite((GearKindEnum) editCustomDeckUnit.unit_.kind_GearKind, editCustomDeckUnit.playerUnit_.GetElement());
    RarityIcon.SetRarity(editCustomDeckUnit.playerUnit_, editCustomDeckUnit.sprRarity_, false);
    UnitTypeIcon.SetAtlasSprite(editCustomDeckUnit.sprUnitType_, (UnitTypeEnum) editCustomDeckUnit.playerUnit_._unit_type);
    IEnumerator e = editCustomDeckUnit.doSetUnitSprite(editCustomDeckUnit.param_.job_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    editCustomDeckUnit.gearIndexMap_ = editCustomDeckUnit.playerUnit_.equippedGearIndexMapByCustomDeck;
    editCustomDeckUnit.iconGears_ = new ItemIcon[editCustomDeckUnit.gearIndexMap_.Length];
    editCustomDeckUnit.iconReisous_ = new ItemIcon[editCustomDeckUnit.iconGears_.Length];
    editCustomDeckUnit.gearIds_ = new int[editCustomDeckUnit.iconGears_.Length];
    editCustomDeckUnit.reisouIds_ = new int[editCustomDeckUnit.iconGears_.Length];
    editCustomDeckUnit.jingiIndex_ = new int?();
    for (int index = 0; index < editCustomDeckUnit.iconGears_.Length; ++index)
    {
      editCustomDeckUnit.iconGears_[index] = editCustomDeckUnit.createGearIcon(editCustomDeckUnit.lnkGears_[index]);
      editCustomDeckUnit.iconReisous_[index] = editCustomDeckUnit.createGearIcon(editCustomDeckUnit.lnkReisous_[index]);
    }
    e = editCustomDeckUnit.doSetGearIcons(editCustomDeckUnit.playerUnit_, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    int posActive = -1;
    if (!editCustomDeckUnit.playerUnit_.isOpenedEquippedGear3 && editCustomDeckUnit.playerUnit_.isPossibleEquippedGear3)
    {
      posActive = editCustomDeckUnit.gearIndexMap_.Length;
      ItemIcon lockGear = editCustomDeckUnit.createGearIcon(editCustomDeckUnit.lnkGears_[posActive]);
      ItemIcon lockReisou = editCustomDeckUnit.createGearIcon(editCustomDeckUnit.lnkReisous_[posActive]);
      e = editCustomDeckUnit.doSetGearIcon(lockGear, (PlayerItem) null, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      lockGear.setEquipPlus(false);
      lockGear.isButtonActive = false;
      e = editCustomDeckUnit.doSetReisouIcon(lockReisou, (PlayerItem) null, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      lockReisou.setEquipPlus(false);
      lockReisou.isButtonActive = false;
      lockGear = (ItemIcon) null;
      lockReisou = (ItemIcon) null;
    }
    ((IEnumerable<GameObject>) editCustomDeckUnit.lockGears_).ToggleOnceEx(posActive);
    ((IEnumerable<GameObject>) editCustomDeckUnit.lockReisous_).ToggleOnceEx(posActive);
    JobChangePatterns jobChangePatterns = JobChangeUtil.getJobChangePatterns(editCustomDeckUnit.playerUnit_);
    if (jobChangePatterns != null)
      jobChangePatterns = jobChangePatterns.GetEnablePatterns();
    if (jobChangePatterns != null)
      editCustomDeckUnit.jobs_ = jobChangePatterns.getJobs(editCustomDeckUnit.jobChange_.buttons.Length);
    else
      editCustomDeckUnit.jobs_ = new MasterDataTable.UnitJob[1]
      {
        editCustomDeckUnit.playerUnit_.getJobData()
      };
    if (editCustomDeckUnit.jobs_.Length > 1)
    {
      for (int index = 1; index < editCustomDeckUnit.jobs_.Length; ++index)
      {
        if (!((IEnumerable<int?>) editCustomDeckUnit.playerUnit_.changed_job_ids).Contains<int?>(new int?(editCustomDeckUnit.jobs_[index].ID)))
          editCustomDeckUnit.jobs_[index] = (MasterDataTable.UnitJob) null;
      }
      for (int no = 0; no < editCustomDeckUnit.jobs_.Length; ++no)
      {
        if (editCustomDeckUnit.jobs_[no] != null)
        {
          editCustomDeckUnit.setButtonColors(editCustomDeckUnit.jobChange_.buttons[no], editCustomDeckUnit.jobChange_.normalColors, false);
          editCustomDeckUnit.setJobChangeButtonEvent(editCustomDeckUnit.jobChange_.buttons[no], no);
        }
        else
        {
          ((UIButtonColor) editCustomDeckUnit.jobChange_.buttons[no]).isEnabled = false;
          Color disabledColor = ((UIButtonColor) editCustomDeckUnit.jobChange_.buttons[no]).disabledColor;
          foreach (UIWidget componentsInChild in ((Component) editCustomDeckUnit.jobChange_.buttons[no]).GetComponentsInChildren<UILabel>())
            componentsInChild.color = disabledColor;
        }
      }
      for (int length = editCustomDeckUnit.jobs_.Length; length < editCustomDeckUnit.jobChange_.buttons.Length; ++length)
        ((Component) editCustomDeckUnit.jobChange_.buttons[length]).gameObject.SetActive(false);
      editCustomDeckUnit.jobIndex_ = (int) Util.getJobRank(editCustomDeckUnit.playerUnit_, editCustomDeckUnit.jobId_);
      editCustomDeckUnit.setButtonColors(editCustomDeckUnit.jobChange_.buttons[editCustomDeckUnit.jobIndex_], editCustomDeckUnit.jobChange_.focusedColors, true);
      editCustomDeckUnit.txtJobName_.SetTextLocalize(editCustomDeckUnit.jobs_[editCustomDeckUnit.jobIndex_].name);
    }
    else
    {
      editCustomDeckUnit.topItems_[0].SetActive(false);
      editCustomDeckUnit.jobIndex_ = 0;
    }
    if (editCustomDeckUnit.unit_.exist_overkillers_slot)
    {
      posActive = Mathf.Min(editCustomDeckUnit.overkillersSlots_.Length, editCustomDeckUnit.unit_.numOverkillersSlot);
      for (int n = 0; n < posActive; ++n)
      {
        PopupEditCustomDeckUnit.OverkillersSlot overkillersSlot = editCustomDeckUnit.overkillersSlots_[n];
        int eId = editCustomDeckUnit.playerUnit_.over_killers_player_unit_ids[n];
        switch (eId)
        {
          case -1:
            continue;
          case 0:
            ((IEnumerable<GameObject>) overkillersSlot.objLocks).SetActives(false);
            UnitIcon component = editCustomDeckUnit.menu_.prefabUnitIcon.Clone(overkillersSlot.lnkIcon).GetComponent<UnitIcon>();
            component.SetEmpty();
            component.SelectUnit = true;
            editCustomDeckUnit.setButtonEventOverkillers(component.Button, n);
            continue;
          default:
            ((IEnumerable<GameObject>) overkillersSlot.objLocks).SetActives(false);
            PlayerUnit playerUnit = Array.Find<PlayerUnit>(editCustomDeckUnit.menu_.playerUnits, (Predicate<PlayerUnit>) (x => x.id == eId));
            UnitIcon icon = editCustomDeckUnit.menu_.prefabUnitIcon.Clone(overkillersSlot.lnkIcon).GetComponent<UnitIcon>();
            e = icon.SetPlayerUnit(playerUnit, (PlayerUnit[]) null, (PlayerUnit) null, false, false);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            icon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Rarity);
            editCustomDeckUnit.setButtonEventOverkillers(icon.Button, n);
            icon = (UnitIcon) null;
            continue;
        }
      }
      for (int index = posActive; index < editCustomDeckUnit.overkillersSlots_.Length; ++index)
        ((IEnumerable<GameObject>) editCustomDeckUnit.overkillersSlots_[index].tops).SetActives(false);
    }
    else
      editCustomDeckUnit.topItems_[1].SetActive(false);
    if (editCustomDeckUnit.playerUnit_.can_equip_awake_skill)
    {
      // ISSUE: reference to a compiler-generated method
      PlayerAwakeSkill playerAwakeSkill = editCustomDeckUnit.param_.awake_skill_id != 0 ? Array.Find<PlayerAwakeSkill>(editCustomDeckUnit.menu_.awakeSkills, new Predicate<PlayerAwakeSkill>(editCustomDeckUnit.\u003CdoInitialize\u003Eb__36_1)) : (PlayerAwakeSkill) null;
      if (playerAwakeSkill != null)
      {
        GameObject go = editCustomDeckUnit.menu_.prefabSkillIcon.Clone(editCustomDeckUnit.lnkAwakeSkill_);
        e = go.GetComponent<BattleSkillIcon>().Init(playerAwakeSkill.masterData);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        foreach (UIWidget componentsInChild in go.GetComponentsInChildren<UIWidget>())
          componentsInChild.depth += editCustomDeckUnit.depthAwakeSkill_;
        go = (GameObject) null;
      }
    }
    else
      editCustomDeckUnit.topItems_[2].SetActive(false);
    yield return (object) null;
    Vector3[] array = ((IEnumerable<GameObject>) editCustomDeckUnit.topItems_).Select<GameObject, Vector3>((Func<GameObject, Vector3>) (x => x.transform.localPosition)).ToArray<Vector3>();
    Vector3[] vector3Array = new Vector3[array.Length];
    vector3Array[0] = Vector3.zero;
    for (int index = 1; index < array.Length; ++index)
      vector3Array[index] = Vector3.op_Subtraction(array[index], array[index - 1]);
    Vector3 vector3 = array[0];
    int num = 0;
    for (int index = 0; index < editCustomDeckUnit.topItems_.Length; ++index)
    {
      if (editCustomDeckUnit.topItems_[index].activeSelf)
      {
        vector3 = Vector3.op_Addition(vector3, vector3Array[num++]);
        editCustomDeckUnit.topItems_[index].transform.localPosition = new Vector3(vector3.x, vector3.y, 0.0f);
      }
    }
  }

  private ItemIcon createGearIcon(Transform parent)
  {
    ItemIcon component = this.menu_.prefabItemIcon.Clone(parent).GetComponent<ItemIcon>();
    component.SetModeGear();
    return component;
  }

  private void setButtonColors(
    UIButton btn,
    PopupEditCustomDeckUnit.StateColors colors,
    bool bFocus)
  {
    if (bFocus)
    {
      ((Component) btn).GetComponent<UIWidget>().color = ((UIButtonColor) btn).defaultColor = ((UIButtonColor) btn).hover = ((UIButtonColor) btn).disabledColor = colors.normal;
      ((UIButtonColor) btn).pressed = colors.pressed;
      ((UIButtonColor) btn).isEnabled = false;
    }
    else
    {
      ((Component) btn).GetComponent<UIWidget>().color = ((UIButtonColor) btn).defaultColor = ((UIButtonColor) btn).hover = colors.normal;
      ((UIButtonColor) btn).pressed = colors.pressed;
      ((UIButtonColor) btn).isEnabled = true;
    }
  }

  private void setJobChangeButtonEvent(UIButton btn, int no)
  {
    EventDelegate.Set(btn.onClick, (EventDelegate.Callback) (() => this.changeJob(no)));
  }

  private void changeJob(int no)
  {
    if (this.isUpdating_ || this.jobIndex_ == no)
      return;
    this.setButtonColors(this.jobChange_.buttons[this.jobIndex_], this.jobChange_.normalColors, false);
    this.setButtonColors(this.jobChange_.buttons[no], this.jobChange_.focusedColors, true);
    this.jobIndex_ = no;
    this.txtJobName_.SetTextLocalize(this.jobs_[no].name);
    PlayerUnit playerUnit = JobChangeUtil.createPlayerUnit(this.playerUnit_, this.jobs_[no].ID);
    bool[] bClearGears = new bool[3]
    {
      this.playerUnit_.equippedGear != playerUnit.equippedGear,
      this.playerUnit_.equippedGear2 != playerUnit.equippedGear2,
      this.playerUnit_.equippedGear3 != playerUnit.equippedGear3
    };
    this.menu_.onSetJob(this.param_.index, this.jobs_[no].ID, bClearGears);
    this.StartCoroutine(this.doJobChange(playerUnit));
  }

  private IEnumerator doJobChange(PlayerUnit next)
  {
    this.isUpdating_ = true;
    RarityIcon.SetRarity(next, this.sprRarity_, false);
    IEnumerator e = this.doSetUnitSprite(next.job_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.doSetGearIcons(next, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.playerUnit_ = next;
    this.isUpdating_ = false;
  }

  private IEnumerator doSetUnitSprite(int jobId)
  {
    if (this.jobId_ != jobId)
    {
      Future<Sprite> ld = this.unit_.LoadSpriteLarge(jobId, 1f);
      IEnumerator e = ld.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.jobId_ = jobId;
      this.sprUnit_.sprite2D = ld.Result;
      ((Component) this.sprUnit_).GetComponent<NGxMaskSpriteWithScale>().FitMask();
    }
  }

  private IEnumerator doSetGearIcons(PlayerUnit target, bool bInit)
  {
    PopupEditCustomDeckUnit editCustomDeckUnit1 = this;
    PlayerItem[] equippedGears = new PlayerItem[3]
    {
      target.equippedGear,
      target.equippedGear2,
      target.equippedGear3
    };
    PlayerItem[] equippedReisous = new PlayerItem[3]
    {
      target.equippedReisou,
      target.equippedReisou2,
      target.equippedReisou3
    };
    int? jingiIndex = editCustomDeckUnit1.getJingiIndex(equippedGears, target.unit.awake_unit_flag, target.isOpenedEquippedGear3);
    int? nullable;
    int num1;
    if (!bInit)
    {
      int? jingiIndex1 = editCustomDeckUnit1.jingiIndex_;
      nullable = jingiIndex;
      num1 = !(jingiIndex1.GetValueOrDefault() == nullable.GetValueOrDefault() & jingiIndex1.HasValue == nullable.HasValue) ? 1 : 0;
    }
    else
      num1 = 0;
    bool bDiffJingi = num1 != 0;
    for (int n = 0; n < editCustomDeckUnit1.gearIndexMap_.Length; ++n)
    {
      int index = editCustomDeckUnit1.gearIndexMap_[n];
      PlayerItem iGear = equippedGears[index];
      PlayerItem iReisou = equippedReisous[index];
      PlayerItem playerItem1 = iGear;
      int gearId = (object) playerItem1 != null ? playerItem1.id : 0;
      bool flag = bInit;
      if (!bInit && editCustomDeckUnit1.gearIds_[n] == gearId)
      {
        if (bDiffJingi)
        {
          int num2 = index;
          nullable = editCustomDeckUnit1.jingiIndex_;
          int valueOrDefault1 = nullable.GetValueOrDefault();
          if (!(num2 == valueOrDefault1 & nullable.HasValue))
          {
            int num3 = index;
            nullable = jingiIndex;
            int valueOrDefault2 = nullable.GetValueOrDefault();
            if (!(num3 == valueOrDefault2 & nullable.HasValue))
              goto label_12;
          }
        }
        else
          goto label_12;
      }
      PopupEditCustomDeckUnit editCustomDeckUnit2 = editCustomDeckUnit1;
      ItemIcon iconGear = editCustomDeckUnit1.iconGears_[n];
      PlayerItem gear = iGear;
      int num4 = index;
      nullable = jingiIndex;
      int valueOrDefault = nullable.GetValueOrDefault();
      int num5 = num4 == valueOrDefault & nullable.HasValue ? 1 : 0;
      IEnumerator e = editCustomDeckUnit2.doSetGearIcon(iconGear, gear, num5 != 0);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      editCustomDeckUnit1.setButtonEventGear(editCustomDeckUnit1.iconGears_[n].gear.button, index);
      editCustomDeckUnit1.gearIds_[n] = gearId;
      flag = true;
label_12:
      PlayerItem playerItem2 = iReisou;
      gearId = (object) playerItem2 != null ? playerItem2.id : 0;
      if (flag || editCustomDeckUnit1.reisouIds_[n] != gearId)
      {
        e = editCustomDeckUnit1.doSetReisouIcon(editCustomDeckUnit1.iconReisous_[n], iReisou, iGear != (PlayerItem) null);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        editCustomDeckUnit1.setButtonEventReisou(editCustomDeckUnit1.iconReisous_[n].gear.button, index);
        editCustomDeckUnit1.reisouIds_[n] = gearId;
      }
      iGear = (PlayerItem) null;
      iReisou = (PlayerItem) null;
    }
    editCustomDeckUnit1.jingiIndex_ = jingiIndex;
  }

  private int? getJingiIndex(PlayerItem[] gears, bool bAwakedUnit, bool bOpendEquippedGear3)
  {
    return !bOpendEquippedGear3 ? new int?() : Gears.getIndexJingiSpace(((IEnumerable<PlayerItem>) gears).Select<PlayerItem, GearGear>((Func<PlayerItem, GearGear>) (x => x?.gear)).ToArray<GearGear>(), bAwakedUnit, true);
  }

  private IEnumerator doSetGearIcon(ItemIcon icon, PlayerItem gear, bool bJingiBack)
  {
    if (gear != (PlayerItem) null && gear.id != 0)
    {
      IEnumerator e = icon.InitByItemInfo(new GameCore.ItemInfo(gear, true));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      icon.resetExpireDate();
    }
    else
    {
      icon.SetEmpty(true, bJingiBack);
      icon.setEquipPlus(!bJingiBack);
      icon.setEquipJinkiPlus(bJingiBack);
      icon.gear.type.SetActive(false);
    }
  }

  private IEnumerator doSetReisouIcon(ItemIcon icon, PlayerItem gear, bool bExistGear)
  {
    if (gear != (PlayerItem) null && gear.id != 0)
    {
      IEnumerator e = icon.InitByItemInfo(new GameCore.ItemInfo(gear));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      icon.SetEmpty(true);
      icon.setEquipPlus(bExistGear);
      icon.gear.type.SetActive(false);
      icon.gear.equipPlus.GetComponent<UI2DSprite>().sprite2D = this.sprAddReisou;
      icon.gear.backGround.GetComponent<UI2DSprite>().sprite2D = icon.backSpriteReisou;
    }
    icon.Gray = !bExistGear;
  }

  private void setButtonEventGear(LongPressButton btn, int slotNo)
  {
    ((Component) btn).gameObject.SetActive(true);
    EventDelegate.Set(btn.onClick, (EventDelegate.Callback) (() => this.onClickedGear(slotNo)));
    EventDelegate.Set(btn.onLongPress, (EventDelegate.Callback) (() => this.onLongPressedGear(slotNo)));
  }

  private void onClickedGear(int slotNo) => this.menu_.onClickedGear(this.param_, slotNo);

  private void onLongPressedGear(int slotNo) => this.menu_.onLongPressedGear(this.param_, slotNo);

  private void setButtonEventReisou(LongPressButton btn, int slotNo)
  {
    ((Component) btn).gameObject.SetActive(true);
    EventDelegate.Set(btn.onClick, (EventDelegate.Callback) (() => this.onClickedReisou(slotNo)));
    EventDelegate.Set(btn.onLongPress, (EventDelegate.Callback) (() => this.onLongPressedReisou(slotNo)));
  }

  private void onClickedReisou(int slotNo) => this.menu_.onClickedReisou(this.param_, slotNo);

  private void onLongPressedReisou(int slotNo)
  {
    this.menu_.onLongPressedReisou(this.param_, slotNo);
  }

  private void setButtonEventOverkillers(LongPressButton btn, int slotNo)
  {
    EventDelegate.Set(btn.onClick, (EventDelegate.Callback) (() => this.onClickedOverkillers(slotNo)));
    EventDelegate.Set(btn.onLongPress, (EventDelegate.Callback) (() => this.onLongPressedOverkillers(slotNo)));
  }

  private void onClickedOverkillers(int slotNo)
  {
    this.menu_.onClickedOverkillers(this.param_, slotNo);
  }

  private void onLongPressedOverkillers(int slotNo)
  {
    this.menu_.onLongPressedOverkillers(this.param_, slotNo);
  }

  public void onClickedAwakeSkill() => this.menu_.onClickedAwakeSkill(this.param_);

  public void onLongPressedAwakeSkill() => this.menu_.onLongPressedAwakeSkill(this.param_);

  private enum Top
  {
    JobChange,
    Overkillers,
    AwakeSkill,
  }

  [Serializable]
  private class StateColors
  {
    public Color normal;
    public Color pressed;
  }

  [Serializable]
  private class JobChangeControl
  {
    public PopupEditCustomDeckUnit.StateColors normalColors;
    public PopupEditCustomDeckUnit.StateColors focusedColors;
    public UIButton[] buttons;
  }

  [Serializable]
  private class OverkillersSlot
  {
    public GameObject[] tops;
    public Transform lnkIcon;
    public GameObject[] objLocks;
  }
}
