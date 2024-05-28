// Decompiled with JetBrains decompiler
// Type: DetailMenuOverkillersSlots
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnitDetails;
using UnityEngine;

#nullable disable
public class DetailMenuOverkillersSlots : MonoBehaviour
{
  [SerializeField]
  [Tooltip("未設定|未開錠時表示")]
  private GameObject[] notsets_;
  [SerializeField]
  [Tooltip("空設定")]
  private GameObject[] spaces_;
  [SerializeField]
  private UIButton[] buttons_;
  [SerializeField]
  [Tooltip("設定したユニットアイコンの親")]
  private GameObject[] links_;
  [SerializeField]
  [Tooltip("未開錠表示オリジナル")]
  private EffectOverkillersSlotLock effectLock_;
  [SerializeField]
  [Tooltip("本表示を隠す際にOFFにする")]
  private GameObject[] objHide_;
  private GameObject popupReleasePrefab_;
  private PlayerUnit playerUnit_;
  private OverkillersSlotRelease.Conditions[] conditions_;
  private bool activeEdit_;
  private bool activeMove_;
  private EffectOverkillersSlotLock[] effects_;
  private bool[] effectRunnings_;
  private UnitIcon[] icons_;
  private bool getSEASkill_;

  public bool isHide
  {
    get => !this.objHide_[0].activeSelf;
    set
    {
      if (this.isHide == value)
        return;
      bool flag = !value;
      for (int index = 0; index < this.objHide_.Length; ++index)
        this.objHide_[index].SetActive(flag);
    }
  }

  private void Awake() => ((Component) this.effectLock_).gameObject.SetActive(false);

  public IEnumerator initialize(
    GameObject popupReleasePrefab,
    GameObject unitPrefab,
    PlayerUnit playerUnit,
    OverkillersSlotRelease.Conditions[] releaseConditions,
    Control controlFlags)
  {
    this.popupReleasePrefab_ = popupReleasePrefab;
    this.playerUnit_ = playerUnit;
    this.conditions_ = releaseConditions;
    this.activeEdit_ = (controlFlags & (Control.OverkillersEdit | Control.CustomDeck)) == Control.OverkillersEdit;
    this.activeMove_ = controlFlags.IsOn(Control.OverkillersMove);
    bool bInfoEquipStatus = controlFlags.IsOff(Control.SelfAbility);
    if (!bInfoEquipStatus)
    {
      this.activeEdit_ = false;
      this.activeMove_ = false;
    }
    if (this.effects_ == null)
    {
      this.effects_ = new EffectOverkillersSlotLock[this.links_.Length];
      this.effectRunnings_ = new bool[this.links_.Length];
      this.icons_ = new UnitIcon[this.links_.Length];
    }
    for (int n = 0; n < this.effects_.Length; ++n)
    {
      if (this.effectRunnings_[n])
      {
        Object.Destroy((Object) ((Component) this.effects_[n]).gameObject);
        this.effects_[n] = (EffectOverkillersSlotLock) null;
        this.effectRunnings_[n] = false;
      }
      if (releaseConditions == null || releaseConditions.Length <= n)
      {
        if (Object.op_Inequality((Object) this.effects_[n], (Object) null))
          ((Component) this.effects_[n]).gameObject.SetActive(false);
        this.notsets_[n].SetActive(false);
        this.spaces_[n].SetActive(true);
        ((UIButtonColor) this.buttons_[n]).isEnabled = false;
        if (Object.op_Inequality((Object) this.icons_[n], (Object) null))
          ((Component) this.icons_[n]).gameObject.SetActive(false);
      }
      else
      {
        this.spaces_[n].SetActive(false);
        bool flag = true;
        ((UIButtonColor) this.buttons_[n]).isEnabled = this.activeEdit_ || this.activeMove_;
        if (playerUnit.isReleasedOverkillersSlot(n))
        {
          if (Object.op_Inequality((Object) this.effects_[n], (Object) null))
            ((Component) this.effects_[n]).gameObject.SetActive(false);
          PlayerUnit[] overkillersUnits = playerUnit.cache_overkillers_units;
          int length = overkillersUnits != null ? overkillersUnits.Length : 0;
          PlayerUnit cacheOverkillersUnit = !bInfoEquipStatus || length <= n ? (PlayerUnit) null : playerUnit.cache_overkillers_units[n];
          if (cacheOverkillersUnit != (PlayerUnit) null)
          {
            if (Object.op_Equality((Object) this.icons_[n], (Object) null))
            {
              this.icons_[n] = unitPrefab.Clone(this.links_[n].transform).GetComponent<UnitIcon>();
              ((Collider) this.icons_[n].buttonBoxCollider).enabled = false;
            }
            ((Component) this.icons_[n]).gameObject.SetActive(true);
            UnitIcon icon = this.icons_[n];
            yield return (object) icon.SetUnit(cacheOverkillersUnit, cacheOverkillersUnit.GetElement(), false);
            icon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
            flag = false;
            icon = (UnitIcon) null;
          }
          else if (Object.op_Inequality((Object) this.icons_[n], (Object) null))
            ((Component) this.icons_[n]).gameObject.SetActive(false);
        }
        else
        {
          if (Object.op_Inequality((Object) this.icons_[n], (Object) null))
            ((Component) this.icons_[n]).gameObject.SetActive(false);
          if (Object.op_Equality((Object) this.effects_[n], (Object) null))
            this.effects_[n] = ((Component) this.effectLock_).gameObject.Clone(this.links_[n].transform.parent).GetComponent<EffectOverkillersSlotLock>();
          this.effects_[n].initialize(releaseConditions[n].unity_value, this.activeEdit_);
        }
        this.notsets_[n].SetActive(flag);
      }
    }
  }

  public void onClickedSlot0() => this.onClickedSlot(0);

  public void onLongPressedSlot0() => this.onLongPressedSlot(0);

  public void onClickedSlot1() => this.onClickedSlot(1);

  public void onLongPressedSlot1() => this.onLongPressedSlot(1);

  public void onClickedSlot2() => this.onClickedSlot(2);

  public void onLongPressedSlot2() => this.onLongPressedSlot(2);

  public void onClickedSlot3() => this.onClickedSlot(3);

  public void onLongPressedSlot3() => this.onLongPressedSlot(3);

  private void onClickedSlot(int no)
  {
    if (!this.activeEdit_ || this.getSEASkill_)
      return;
    if (this.playerUnit_.isReleasedOverkillersSlot(no))
    {
      Unit004OverkillersSlotUnitSelectScene.changeScene(this.playerUnit_, no, this.playerUnit_.cache_overkillers_units[no], new Action<PlayerUnit>(this.onSelectedUnit));
    }
    else
    {
      if (Object.op_Equality((Object) this.effects_[no], (Object) null) || this.effectRunnings_[no] || !Object.op_Inequality((Object) this.popupReleasePrefab_, (Object) null))
        return;
      bool[] slot_locks = new bool[this.conditions_.Length];
      for (int slot_no = 0; slot_no < slot_locks.Length; ++slot_no)
        slot_locks[slot_no] = !this.playerUnit_.isReleasedOverkillersSlot(slot_no);
      PopupOverkillersSlotRelease.show(this.popupReleasePrefab_, this.playerUnit_, no, slot_locks, this.conditions_[no], (Action) (() => this.StartCoroutine(this.doOverkillersSlotRelease(no))));
    }
  }

  private IEnumerator doOverkillersSlotRelease(int no)
  {
    DetailMenuOverkillersSlots overkillersSlots = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
    Unit0042Menu inParents = NGUITools.FindInParents<Unit0042Menu>(((Component) overkillersSlots).gameObject);
    if (Object.op_Inequality((Object) inParents, (Object) null))
    {
      bool bWait = true;
      inParents.UploadFavorites((Action) (() => bWait = false));
      while (bWait)
        yield return (object) null;
    }
    IEnumerator e = overkillersSlots.doUnlockSlot(no);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void onLongPressedSlot(int no)
  {
    if (!this.activeMove_ || !this.playerUnit_.isReleasedOverkillersSlot(no) || this.playerUnit_.cache_overkillers_units[no] == (PlayerUnit) null)
      return;
    Unit0042Menu inParents = NGUITools.FindInParents<Unit0042Menu>(((Component) this).gameObject);
    if (!Object.op_Inequality((Object) inParents, (Object) null))
      return;
    inParents.moveUnitPage(this.playerUnit_.cache_overkillers_units[no].id, ((Component) this.buttons_[no]).gameObject);
  }

  private void onSelectedUnit(PlayerUnit unit)
  {
  }

  private IEnumerator doUnlockSlot(int no)
  {
    this.getSEASkill_ = false;
    Future<WebAPI.Response.UnitReleaseFrameOverKillers> future = WebAPI.UnitReleaseFrameOverKillers(this.playerUnit_.id, this.getConditionsMaterialIds(no), no, (Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    yield return (object) future.Wait();
    if (future.Result != null)
    {
      if (GuildUtil.gvgPopupState != GuildUtil.GvGPopupState.None)
        yield return (object) GuildUtil.UpdateGuildDeck();
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      Singleton<PopupManager>.GetInstance().open((GameObject) null, isViewBack: false);
      yield return (object) new WaitForSeconds(0.5f);
      if (no == PlayerUnit.SEASkillUnlockConditions - 1)
      {
        UnitSEASkill unitSkill = ((IEnumerable<UnitSEASkill>) MasterData.UnitSEASkillList).FirstOrDefault<UnitSEASkill>((Func<UnitSEASkill, bool>) (x => x.ID == this.playerUnit_.unit.same_character_id));
        IEnumerator e = MasterData.LoadScriptScript(unitSkill.script_id);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (MasterData.ScriptScript != null && MasterData.ScriptScript.ContainsKey(unitSkill.script_id) || unitSkill.skill_1_BattleskillSkill.HasValue)
          this.getSEASkill_ = true;
        unitSkill = (UnitSEASkill) null;
      }
      this.effectRunnings_[no] = true;
      this.effects_[no].startUnlock((Action) (() => this.onFinishedEffectUnlock(no)));
    }
  }

  private int[] getConditionsMaterialIds(int no)
  {
    PlayerMaterialUnit[] playerMaterials = this.conditions_[no].getPlayerMaterials();
    int[] conditionsMaterialIds = new int[playerMaterials.Length];
    for (int index = 0; index < playerMaterials.Length; ++index)
      conditionsMaterialIds[index] = playerMaterials[index].id;
    return conditionsMaterialIds;
  }

  private void onFinishedEffectUnlock(int no)
  {
    Object.Destroy((Object) ((Component) this.effects_[no]).gameObject);
    this.effects_[no] = (EffectOverkillersSlotLock) null;
    this.effectRunnings_[no] = false;
    if (no == PlayerUnit.SEASkillUnlockConditions - 1 && this.getSEASkill_)
    {
      Unit0042SEASkillReleaseScene.changeScene(true, this.playerUnit_.unit.ID, this.playerUnit_.unit.same_character_id);
    }
    else
    {
      Singleton<PopupManager>.GetInstance().dismiss();
      Singleton<PopupManager>.GetInstance().monitorCoroutine(this.doUpdateUnits());
    }
  }

  private IEnumerator doUpdateUnits()
  {
    Unit0042Menu menu = NGUITools.FindInParents<Unit0042Menu>(((Component) this).gameObject);
    if (Object.op_Inequality((Object) menu, (Object) null))
    {
      Singleton<PopupManager>.GetInstance().open((GameObject) null, isViewBack: false);
      yield return (object) null;
      PlayerUnit[] array = SMManager.Get<PlayerUnit[]>();
      PlayerUnit[] playerUnits = new PlayerUnit[menu.UnitList.Length];
      for (int index = 0; index < menu.UnitList.Length; ++index)
      {
        int tid = menu.UnitList[index].id;
        playerUnits[index] = Array.Find<PlayerUnit>(array, (Predicate<PlayerUnit>) (x => x.id == tid));
      }
      yield return (object) menu.UpdateAllPage(playerUnits);
      yield return (object) null;
      Singleton<PopupManager>.GetInstance().dismiss();
    }
  }
}
