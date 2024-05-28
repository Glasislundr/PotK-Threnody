// Decompiled with JetBrains decompiler
// Type: GachaPickupSelectMenu
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
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Gacha/PickupSelect/Menu")]
public class GachaPickupSelectMenu : BackButtonMenuBase
{
  [Header("フィルター情報")]
  [SerializeField]
  private UISprite sprFilter_;
  [Header("選択対象一覧部")]
  [SerializeField]
  private ScrollViewSpecifyBounds scroll_;
  [SerializeField]
  private GridSetting gridSetting_;
  [Header("選択情報部")]
  [SerializeField]
  private UIGrid gridSelected_;
  [SerializeField]
  private GachaPickupSelectMenu.SelectFrame[] selectFrames_;
  [SerializeField]
  private UIButton btnClear_;
  [SerializeField]
  private UIButton btnOk_;
  private GachaModuleGacha module_;
  private Action onChanged_;
  private bool isReadonly_;
  private GachaPickupSelectMenu.Info[] allInfos_;
  private GachaPickupSelectMenu.Info[] selectedInfos_;
  private CharacterQuestFilter.Calculator filter_;
  private CharacterQuestFilter popupFilter_;
  private GachaPickupSelectMenu.Info[] displayInfos_;
  private GachaPickupSelectMenu.Icon[] icons_;
  private GachaPickupSelectMenu.Icon[] selectedIcons_;
  private Queue<int> quSelectNumbers_;
  private bool isInitialize_;
  private bool isWaitSort_;
  private float scrollBaseY_;
  private float? logScrollY_;
  private GameObject prefabIcon_;
  private GameObject prefabDetail_;
  private GameObject prefabFilter_;
  private GameObject prefabFilterParts_;

  private bool isMaxSelected => !this.quSelectNumbers_.Any<int>();

  public IEnumerator doLoadResources()
  {
    Future<GameObject> ld;
    IEnumerator e;
    if (!Object.op_Implicit((Object) this.prefabIcon_))
    {
      ld = PickupSelectIcon.createLoader();
      e = ld.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.prefabIcon_ = ld.Result;
      ld = (Future<GameObject>) null;
    }
    if (!Object.op_Implicit((Object) this.prefabDetail_))
    {
      ld = Res.Prefabs.popup.popup_000_7_4_2__anim_popup01.Load<GameObject>();
      e = ld.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.prefabDetail_ = ld.Result;
      ld = (Future<GameObject>) null;
    }
    if (!Object.op_Implicit((Object) this.prefabFilter_))
    {
      ld = new ResourceObject("Prefabs/popup/popup_Unit_Sort_CharaQue__anim_popup01").Load<GameObject>();
      e = ld.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.prefabFilter_ = ld.Result;
      ld = (Future<GameObject>) null;
    }
    if (!Object.op_Implicit((Object) this.prefabFilterParts_))
    {
      ld = new ResourceObject("Prefabs/unit004_6/ibtn_Popup_Group").Load<GameObject>();
      e = ld.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.prefabFilterParts_ = ld.Result;
      ld = (Future<GameObject>) null;
    }
  }

  public IEnumerator onStartSceneAsync(GachaModuleGacha module, Action callbackChanged)
  {
    GachaPickupSelectMenu pickupSelectMenu = this;
    pickupSelectMenu.isInitialize_ = false;
    pickupSelectMenu.module_ = module;
    pickupSelectMenu.onChanged_ = callbackChanged;
    pickupSelectMenu.filter_ = new CharacterQuestFilter.Calculator();
    pickupSelectMenu.setLabelFilter();
    bool bError = false;
    IEnumerator e = Singleton<NGGameDataManager>.GetInstance().downloadGachaPickupSelect(pickupSelectMenu.module_.id, new Action<WebAPI.Response.GachaG301PickupSelectPlayerPickup>(pickupSelectMenu.createInfos), (Action<WebAPI.Response.UserError>) (err =>
    {
      WebAPI.DefaultUserErrorCallback(err);
      MypageScene.ChangeSceneOnError();
      bError = true;
    }));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!bError)
    {
      ((IEnumerable<GameObject>) new GameObject[2]
      {
        ((Component) pickupSelectMenu.btnClear_).gameObject,
        ((Component) pickupSelectMenu.btnOk_).gameObject
      }).SetActives(!pickupSelectMenu.isReadonly_);
      e = pickupSelectMenu.onDemandDownloadIcons();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      pickupSelectMenu.setDisplayInfos(pickupSelectMenu.filterBy());
      pickupSelectMenu.createIcons();
      List<IEnumerator> batch = new List<IEnumerator>();
      pickupSelectMenu.resetSelectFrames(pickupSelectMenu.module_.pickup_select_count.Value);
      if (pickupSelectMenu.selectedInfos_.Length != 0)
      {
        for (int index = 0; index < pickupSelectMenu.selectedInfos_.Length; ++index)
          pickupSelectMenu.selected(pickupSelectMenu.selectedInfos_[index], batch);
        e = pickupSelectMenu.batchCoroutines(batch);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        batch.Clear();
      }
      pickupSelectMenu.resetIcons(pickupSelectMenu.isMaxSelected, batch);
      e = pickupSelectMenu.batchCoroutines(batch);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      pickupSelectMenu.updateSelectedInfo();
      pickupSelectMenu.isInitialize_ = true;
    }
  }

  private void createInfos(
    WebAPI.Response.GachaG301PickupSelectPlayerPickup data)
  {
    if (data == null)
      return;
    this.allInfos_ = ((IEnumerable<WebAPI.Response.GachaG301PickupSelectPlayerPickupPickup_list>) data.pickup_list).Select<WebAPI.Response.GachaG301PickupSelectPlayerPickupPickup_list, GachaPickupSelectMenu.Info>((Func<WebAPI.Response.GachaG301PickupSelectPlayerPickupPickup_list, GachaPickupSelectMenu.Info>) (x => new GachaPickupSelectMenu.Info(x))).OrderBy<GachaPickupSelectMenu.Info, int>((Func<GachaPickupSelectMenu.Info, int>) (y => y.historyNumber == 0 ? int.MaxValue : y.historyNumber)).ThenBy<GachaPickupSelectMenu.Info, int>((Func<GachaPickupSelectMenu.Info, int>) (z => z.ID)).ToArray<GachaPickupSelectMenu.Info>();
    for (int i = 0; i < this.allInfos_.Length; ++i)
      this.allInfos_[i].setIndex(i);
    this.selectedInfos_ = data.player_pickup_deck_entity_ids.Length == 0 ? new GachaPickupSelectMenu.Info[0] : ((IEnumerable<int>) data.player_pickup_deck_entity_ids).Select<int, GachaPickupSelectMenu.Info>((Func<int, GachaPickupSelectMenu.Info>) (n => Array.Find<GachaPickupSelectMenu.Info>(this.allInfos_, (Predicate<GachaPickupSelectMenu.Info>) (x => x.webEntity.deck_entity_id == n)))).Where<GachaPickupSelectMenu.Info>((Func<GachaPickupSelectMenu.Info, bool>) (x => x != null)).ToArray<GachaPickupSelectMenu.Info>();
    this.isReadonly_ = data.is_locked;
  }

  private IEnumerator onDemandDownloadIcons()
  {
    List<string> paths = new List<string>();
    foreach (GachaPickupSelectMenu.Info allInfo in this.allInfos_)
    {
      switch (allInfo.data)
      {
        case UnitUnit unitUnit:
          paths.Add(unitUnit.GetUIResourcePaths(false).First<string>());
          break;
        case GearGear gearGear:
          paths.Add(gearGear.ResourcePaths().First<string>());
          break;
      }
    }
    IEnumerator e = OnDemandDownload.waitLoadSomethingResource((IEnumerable<string>) paths, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void setDisplayInfos(GachaPickupSelectMenu.Info[] infos)
  {
    for (int index = 0; index < this.allInfos_.Length; ++index)
      this.allInfos_[index].displayIndex = -1;
    for (int index = 0; index < infos.Length; ++index)
      infos[index].displayIndex = index;
    this.displayInfos_ = infos;
  }

  private void resetSelectFrames(int nSelect)
  {
    if (this.selectedIcons_ != null)
    {
      for (int index = 0; index < this.selectedIcons_.Length; ++index)
        this.selectFrames_[index].lnk.Clear();
      this.selectedIcons_ = (GachaPickupSelectMenu.Icon[]) null;
    }
    this.selectedIcons_ = new GachaPickupSelectMenu.Icon[nSelect];
    for (int index = 0; index < this.selectedIcons_.Length; ++index)
    {
      this.selectFrames_[index].top.SetActive(true);
      this.selectedIcons_[index] = new GachaPickupSelectMenu.Icon(index);
      this.selectedIcons_[index].setIcon(this.prefabIcon_.Clone(this.selectFrames_[index].lnk).GetComponent<PickupSelectIcon>());
      this.selectedIcons_[index].icon.setPosition(index);
      this.selectedIcons_[index].icon.setBlank();
    }
    for (int length = this.selectedIcons_.Length; length < this.selectFrames_.Length; ++length)
      this.selectFrames_[length].top.SetActive(false);
    this.gridSelected_.Reposition();
    this.quSelectNumbers_ = new Queue<int>(((IEnumerable<GachaPickupSelectMenu.Icon>) this.selectedIcons_).Select<GachaPickupSelectMenu.Icon, int>((Func<GachaPickupSelectMenu.Icon, int>) (x => x.index)));
  }

  private void selected(GachaPickupSelectMenu.Info info, List<IEnumerator> loaders = null)
  {
    if (!this.quSelectNumbers_.Any<int>())
      return;
    int index = this.quSelectNumbers_.Dequeue();
    info.selected = index + 1;
    this.selectedIcons_[index].setInfo(info);
    this.updateSelectedIcon(this.selectedIcons_[index], loaders);
  }

  private void deselected(GachaPickupSelectMenu.Info info)
  {
    if (!info.isSelected)
      return;
    int element = info.selected - 1;
    info.selected = 0;
    this.quSelectNumbers_ = new Queue<int>((IEnumerable<int>) this.quSelectNumbers_.Select<int, int>((Func<int, int>) (i => i)).Append<int>(element).OrderBy<int, int>((Func<int, int>) (i => i)));
    this.selectedIcons_[element].setInfo((GachaPickupSelectMenu.Info) null);
    this.selectedIcons_[element].icon.setBlank();
  }

  private void updateSelectedIcon(GachaPickupSelectMenu.Icon icon, List<IEnumerator> loaders)
  {
    GachaPickupSelectMenu.Info info = icon.info;
    ((Component) icon.icon).gameObject.SetActive(true);
    switch (info.data)
    {
      case UnitUnit unit:
        icon.icon.initialize(unit, info.quantity);
        break;
      case GearGear gear:
        icon.icon.initialize(gear, info.isGearBody, info.quantity);
        break;
    }
    icon.icon.selected = false;
    icon.icon.notPossessed = info.isNotPossessed;
    this.setPressSelectedIconEvents(icon);
    if (loaders != null)
      loaders.Add(icon.icon.doLoadIcon());
    else
      icon.icon.loadIcon();
  }

  private void setPressSelectedIconEvents(GachaPickupSelectMenu.Icon icon)
  {
    LongPressButton button = icon.icon.button;
    if (!this.isReadonly_)
      EventDelegate.Set(button.onClick, (EventDelegate.Callback) (() => this.onDeselect(icon)));
    else
      button.onClick.Clear();
    switch (icon.icon.type)
    {
      case PickupSelectIcon.Type.Unit:
        EventDelegate.Set(button.onLongPress, (EventDelegate.Callback) (() => this.onLongPressedUnit(icon.info)));
        break;
      case PickupSelectIcon.Type.Material:
        EventDelegate.Set(button.onLongPress, (EventDelegate.Callback) (() => this.onLongPressedMaterial(icon.info)));
        break;
      case PickupSelectIcon.Type.Gear:
        EventDelegate.Set(button.onLongPress, (EventDelegate.Callback) (() => this.onLongPressedGear(icon.info)));
        break;
      case PickupSelectIcon.Type.Reisou:
        EventDelegate.Set(button.onLongPress, (EventDelegate.Callback) (() => this.onLongPressedReisou(icon.info)));
        break;
      case PickupSelectIcon.Type.GearMaterial:
        EventDelegate.Set(button.onLongPress, (EventDelegate.Callback) (() => this.onLongPressedGearMaterial(icon.info)));
        break;
      case PickupSelectIcon.Type.GearBody:
        EventDelegate.Set(button.onLongPress, (EventDelegate.Callback) (() => this.onLongPressedGearBody(icon.info)));
        break;
    }
  }

  private void onDeselect(GachaPickupSelectMenu.Icon icon, bool bCancelUpdateGrayStatus = false)
  {
    if (this.IsPush)
      return;
    bool isMaxSelected = this.isMaxSelected;
    GachaPickupSelectMenu.Info info = icon.info;
    GachaPickupSelectMenu.Icon icon1 = info.displayIndex != -1 ? ((IEnumerable<GachaPickupSelectMenu.Icon>) this.icons_).FirstOrDefault<GachaPickupSelectMenu.Icon>((Func<GachaPickupSelectMenu.Icon, bool>) (x => x.info == info)) : (GachaPickupSelectMenu.Icon) null;
    this.deselected(info);
    if (icon1 != null)
      icon1.icon.selected = false;
    if (bCancelUpdateGrayStatus)
      return;
    if (isMaxSelected != this.isMaxSelected)
      this.updateAllGrayStatus();
    else if (icon1 != null)
      this.setGrayStatus(icon1);
    this.updateSelectedInfo();
  }

  private void resetIcons(bool bLimit, List<IEnumerator> batchLst)
  {
    this.resetScrollRange();
    int num = Mathf.Min(this.icons_.Length, this.displayInfos_.Length);
    for (int index = 0; index < num; ++index)
    {
      this.updateIcon(index, index, bLimit, batchLst);
      ((Component) this.icons_[index].icon).transform.localPosition = this.gridSetting_.calcLocalPosition(index);
    }
    for (int index = num; index < this.icons_.Length; ++index)
    {
      PickupSelectIcon icon = this.icons_[index].icon;
      ((Component) icon).gameObject.SetActive(false);
      icon.setPosition();
    }
  }

  private void createIcons()
  {
    if (this.icons_ != null && this.icons_.Length != 0)
    {
      ((Component) this.scroll_).transform.Clear();
      this.icons_ = (GachaPickupSelectMenu.Icon[]) null;
    }
    this.icons_ = new GachaPickupSelectMenu.Icon[Mathf.Min(this.gridSetting_.quantityInstance, this.allInfos_.Length)];
    for (int index = 0; index < this.icons_.Length; ++index)
    {
      this.icons_[index] = new GachaPickupSelectMenu.Icon(index);
      GameObject gameObject = this.prefabIcon_.Clone(((Component) this.scroll_).transform);
      gameObject.transform.localPosition = this.gridSetting_.calcLocalPosition(index);
      this.icons_[index].setIcon(gameObject.GetComponent<PickupSelectIcon>());
      gameObject.SetActive(false);
    }
  }

  private void resetScrollRange()
  {
    Tuple<GameObject, GameObject> tuple = this.gridSetting_.setScrollRange(((Component) this.scroll_).transform, "top", "bottom", this.displayInfos_.Length);
    this.scroll_.ClearBounds();
    this.scroll_.AddBounds((IEnumerable<UIWidget>) new UIWidget[2]
    {
      tuple.Item1.GetComponent<UIWidget>(),
      tuple.Item2.GetComponent<UIWidget>()
    });
    this.scroll_.ResetPosition();
    this.scrollBaseY_ = ((Component) this.scroll_).transform.localPosition.y;
    this.logScrollY_ = new float?();
  }

  private IEnumerator batchCoroutines(List<IEnumerator> lst)
  {
    foreach (IEnumerator e in lst)
    {
      while (e.MoveNext())
        yield return e.Current;
    }
  }

  private void updateScroll()
  {
    if (this.isWaitSort_ || !this.isInitialize_ || this.displayInfos_.Length <= this.gridSetting_.quantityScreen)
      return;
    float y = ((Component) this.scroll_).transform.localPosition.y;
    if (this.logScrollY_.HasValue && (double) Mathf.Abs(y - this.logScrollY_.Value) < 1.0)
      return;
    this.logScrollY_ = new float?(y);
    int num1 = this.gridSetting_.height * 2;
    float num2 = (float) (Mathf.Max(0, this.displayInfos_.Length - this.gridSetting_.quantityScreen - 1) / this.gridSetting_.column * this.gridSetting_.height);
    float num3 = (float) (this.gridSetting_.height * this.gridSetting_.rowInstance);
    float num4 = Mathf.Clamp(y - this.scrollBaseY_, 0.0f, num2);
    int num5 = Mathf.Min(this.displayInfos_.Length, this.icons_.Length);
    bool isMaxSelected = this.isMaxSelected;
    bool flag;
    do
    {
      flag = false;
      for (int icon_index = 0; icon_index < num5; ++icon_index)
      {
        PickupSelectIcon icon = this.icons_[icon_index].icon;
        Transform transform = ((Component) icon).transform;
        GameObject gameObject = ((Component) icon).gameObject;
        float num6 = transform.localPosition.y + num4;
        if ((double) num6 > (double) num1)
        {
          int num7 = (this.displayInfos_.Length + this.gridSetting_.column - 1) / this.gridSetting_.column * this.gridSetting_.column;
          int num8 = icon.position >= 0 ? icon.position + this.gridSetting_.quantityInstance : num7;
          if (icon.position >= 0 && num8 < num7)
          {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - num3, 0.0f);
            if (num8 >= this.displayInfos_.Length)
            {
              gameObject.SetActive(false);
              icon.setPosition(num8);
            }
            else
              this.updateIcon(num8, icon_index, isMaxSelected);
            flag = true;
          }
        }
        else if ((double) num6 < -((double) num3 - (double) num1))
        {
          int quantityInstance = this.gridSetting_.quantityInstance;
          if (!gameObject.activeSelf)
            gameObject.SetActive(true);
          int info_index = icon.position >= 0 ? icon.position - quantityInstance : -1;
          if (icon.position >= 0 && info_index >= 0)
          {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + num3, 0.0f);
            this.updateIcon(info_index, icon_index, isMaxSelected);
            flag = true;
          }
        }
      }
    }
    while (flag);
  }

  private void updateIcon(int info_index, int icon_index, bool bLimit, List<IEnumerator> waitList = null)
  {
    GachaPickupSelectMenu.Info displayInfo = this.displayInfos_[info_index];
    GachaPickupSelectMenu.Icon icon = this.icons_[icon_index];
    ((Component) icon.icon).gameObject.SetActive(true);
    icon.icon.setPosition(info_index);
    if (!icon.setInfo(displayInfo))
    {
      icon.icon.selected = displayInfo.isSelected;
      icon.icon.notPossessed = displayInfo.isNotPossessed;
      this.setGrayStatus(icon, bLimit);
    }
    else
    {
      switch (displayInfo.data)
      {
        case UnitUnit unit:
          icon.icon.initialize(unit, displayInfo.quantity);
          break;
        case GearGear gear:
          icon.icon.initialize(gear, displayInfo.isGearBody, displayInfo.quantity);
          break;
      }
      icon.icon.selected = displayInfo.isSelected;
      icon.icon.notPossessed = displayInfo.isNotPossessed;
      this.setPressEvents(icon);
      if (waitList != null)
        waitList.Add(icon.icon.doLoadIcon());
      else
        icon.icon.loadIcon();
      this.setGrayStatus(icon, bLimit);
    }
  }

  private void setPressEvents(GachaPickupSelectMenu.Icon icon)
  {
    LongPressButton button = icon.icon.button;
    if (!this.isReadonly_)
      EventDelegate.Set(button.onClick, (EventDelegate.Callback) (() => this.onSelect(icon)));
    else
      button.onClick.Clear();
    switch (icon.icon.type)
    {
      case PickupSelectIcon.Type.Unit:
        EventDelegate.Set(button.onLongPress, (EventDelegate.Callback) (() => this.onLongPressedUnit(icon.info)));
        break;
      case PickupSelectIcon.Type.Material:
        EventDelegate.Set(button.onLongPress, (EventDelegate.Callback) (() => this.onLongPressedMaterial(icon.info)));
        break;
      case PickupSelectIcon.Type.Gear:
        EventDelegate.Set(button.onLongPress, (EventDelegate.Callback) (() => this.onLongPressedGear(icon.info)));
        break;
      case PickupSelectIcon.Type.Reisou:
        EventDelegate.Set(button.onLongPress, (EventDelegate.Callback) (() => this.onLongPressedReisou(icon.info)));
        break;
      case PickupSelectIcon.Type.GearMaterial:
        EventDelegate.Set(button.onLongPress, (EventDelegate.Callback) (() => this.onLongPressedGearMaterial(icon.info)));
        break;
      case PickupSelectIcon.Type.GearBody:
        EventDelegate.Set(button.onLongPress, (EventDelegate.Callback) (() => this.onLongPressedGearBody(icon.info)));
        break;
    }
  }

  private void onSelect(GachaPickupSelectMenu.Icon icon)
  {
    if (this.IsPush)
      return;
    bool isMaxSelected = this.isMaxSelected;
    GachaPickupSelectMenu.Info info = icon.info;
    if (icon.info.isSelected)
    {
      this.deselected(info);
    }
    else
    {
      if (isMaxSelected)
        return;
      this.selected(info);
    }
    icon.icon.selected = info.isSelected;
    if (isMaxSelected != this.isMaxSelected)
      this.updateAllGrayStatus();
    else
      this.setGrayStatus(icon);
    this.updateSelectedInfo();
  }

  private void updateAllGrayStatus()
  {
    bool isMaxSelected = this.isMaxSelected;
    for (int index = 0; index < this.icons_.Length; ++index)
      this.setGrayStatus(this.icons_[index], isMaxSelected);
  }

  private void onLongPressedUnit(GachaPickupSelectMenu.Info info)
  {
    if (this.IsPushAndSet())
      return;
    Unit0042PickupScene.changeScene(true, info.unit.ID);
  }

  private void onLongPressedMaterial(GachaPickupSelectMenu.Info info)
  {
    if (this.IsPushAndSet())
      return;
    GameObject go = Singleton<PopupManager>.GetInstance().open(this.prefabDetail_, isNonSe: true, isNonOpenAnime: true);
    this.waitStartPopup(go.GetComponent<ItemDetailPopupBase>().SetInfo(MasterDataTable.CommonRewardType.material_unit, info.unit.ID), go);
  }

  private void onLongPressedGear(GachaPickupSelectMenu.Info info)
  {
    if (this.IsPushAndSet())
      return;
    GameObject go = Singleton<PopupManager>.GetInstance().open(this.prefabDetail_, isNonSe: true, isNonOpenAnime: true);
    this.waitStartPopup(go.GetComponent<ItemDetailPopupBase>().SetInfo(MasterDataTable.CommonRewardType.gear, info.gear.ID), go);
  }

  private void onLongPressedReisou(GachaPickupSelectMenu.Info info)
  {
    if (this.IsPushAndSet())
      return;
    GameObject go = Singleton<PopupManager>.GetInstance().open(this.prefabDetail_, isNonSe: true, isNonOpenAnime: true);
    this.waitStartPopup(go.GetComponent<ItemDetailPopupBase>().SetInfo(MasterDataTable.CommonRewardType.gear, info.gear.ID), go);
  }

  private void onLongPressedGearMaterial(GachaPickupSelectMenu.Info info)
  {
    if (this.IsPushAndSet())
      return;
    GameObject go = Singleton<PopupManager>.GetInstance().open(this.prefabDetail_, isNonSe: true, isNonOpenAnime: true);
    this.waitStartPopup(go.GetComponent<ItemDetailPopupBase>().SetInfo(MasterDataTable.CommonRewardType.material_gear, info.gear.ID), go);
  }

  private void onLongPressedGearBody(GachaPickupSelectMenu.Info info)
  {
    if (this.IsPushAndSet())
      return;
    GameObject go = Singleton<PopupManager>.GetInstance().open(this.prefabDetail_, isNonSe: true, isNonOpenAnime: true);
    this.waitStartPopup(go.GetComponent<ItemDetailPopupBase>().SetInfo(MasterDataTable.CommonRewardType.gear_body, info.gear.ID), go);
  }

  private void waitStartPopup(IEnumerator e, GameObject go)
  {
    Singleton<PopupManager>.GetInstance().monitorCoroutine(this.doWaitStartPopup(e, go));
  }

  private IEnumerator doWaitStartPopup(IEnumerator e, GameObject go)
  {
    while (e.MoveNext())
      yield return e.Current;
    Singleton<PopupManager>.GetInstance().startOpenAnime(go);
  }

  private void updateSelectedInfo()
  {
    int count = this.quSelectNumbers_.Count;
    ((UIButtonColor) this.btnClear_).isEnabled = count != this.selectedIcons_.Length;
    ((UIButtonColor) this.btnOk_).isEnabled = count == 0;
  }

  private void setGrayStatus(GachaPickupSelectMenu.Icon icon, bool bLimit = false)
  {
    GachaPickupSelectMenu.Info info = icon.info;
    if (info == null)
      return;
    if (bLimit)
      icon.icon.gray = !info.isSelected;
    else
      icon.icon.gray = info.isSelected;
  }

  public void onClickedClear()
  {
    if (this.IsPush)
      return;
    for (int index = 0; index < this.selectedIcons_.Length; ++index)
    {
      if (this.selectedIcons_[index].info != null)
        this.onDeselect(this.selectedIcons_[index], true);
    }
    this.updateAllGrayStatus();
    this.updateSelectedInfo();
  }

  public void onClickedOk()
  {
    if (!this.isMaxSelected || this.IsPushAndSet())
      return;
    this.StartCoroutine(this.doUpload());
  }

  private IEnumerator doUpload()
  {
    GachaPickupSelectMenu pickupSelectMenu = this;
    // ISSUE: reference to a compiler-generated method
    bool flag = pickupSelectMenu.selectedInfos_.Length < pickupSelectMenu.selectedIcons_.Length || ((IEnumerable<GachaPickupSelectMenu.Icon>) pickupSelectMenu.selectedIcons_).Count<GachaPickupSelectMenu.Icon>(new Func<GachaPickupSelectMenu.Icon, bool>(pickupSelectMenu.\u003CdoUpload\u003Eb__63_0)) < pickupSelectMenu.selectedInfos_.Length;
    if (pickupSelectMenu.module_ != null & flag)
    {
      int nWait = 0;
      Consts instance = Consts.GetInstance();
      if (pickupSelectMenu.module_.is_change_pickup_select)
        PopupCommonNoYes.Show(instance.PICKUPSELECT_POPUP_SELECTED_CONFIRM_TITLE, instance.PICKUPSELECT_POPUP_SELECTED_CONFIRM_RESELECTABLE, (Action) (() => nWait = 1), (Action) (() => nWait = 2), (NGUIText.Alignment) 1);
      else
        PopupCommonNoYes.Show(instance.PICKUPSELECT_POPUP_SELECTED_CONFIRM_TITLE, instance.PICKUPSELECT_POPUP_SELECTED_CONFIRM_LOCK, (Action) (() => nWait = 1), (Action) (() => nWait = 2), (NGUIText.Alignment) 1);
      while (nWait == 0)
        yield return (object) null;
      if (nWait == 2)
      {
        pickupSelectMenu.IsPush = false;
        yield break;
      }
      else
      {
        Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
        int[] array = ((IEnumerable<GachaPickupSelectMenu.Icon>) pickupSelectMenu.selectedIcons_).SelectMany<GachaPickupSelectMenu.Icon, int>((Func<GachaPickupSelectMenu.Icon, IEnumerable<int>>) (x => (IEnumerable<int>) x.info.webEntity.send_deck_entity_ids)).ToArray<int>();
        IEnumerator e = Singleton<NGGameDataManager>.GetInstance().uploadGachaPickupSelect(pickupSelectMenu.module_.id, array);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        pickupSelectMenu.onChanged_();
        Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      }
    }
    pickupSelectMenu.backScene();
  }

  public void onClickedFilter()
  {
    if (this.IsPushAndSet())
      return;
    this.popupFilter_ = Singleton<PopupManager>.GetInstance().open(this.prefabFilter_, isNonSe: true, isNonOpenAnime: true).GetComponent<CharacterQuestFilter>();
    this.popupFilter_.Initialize(this.prefabFilterParts_, this.filter_, new Action(this.filtered), new Action(this.changeFilterSetting));
  }

  private void filtered()
  {
    this.setLabelFilter();
    Singleton<PopupManager>.GetInstance().dismiss();
    this.IsPush = true;
    Singleton<PopupManager>.GetInstance().open((GameObject) null);
    this.StartCoroutine(this.doFinishedFilter());
  }

  private IEnumerator doFinishedFilter()
  {
    GachaPickupSelectMenu pickupSelectMenu = this;
    pickupSelectMenu.isWaitSort_ = true;
    pickupSelectMenu.setDisplayInfos(pickupSelectMenu.filterBy());
    pickupSelectMenu.popupFilter_ = (CharacterQuestFilter) null;
    List<IEnumerator> enumeratorList = new List<IEnumerator>();
    pickupSelectMenu.resetIcons(pickupSelectMenu.isMaxSelected, enumeratorList);
    IEnumerator e = pickupSelectMenu.batchCoroutines(enumeratorList);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().dismiss();
    pickupSelectMenu.isWaitSort_ = false;
    pickupSelectMenu.IsPush = false;
  }

  private void changeFilterSetting()
  {
    this.popupFilter_.SetUnitNum(this.filterBy().Length, this.allInfos_.Length);
  }

  private GachaPickupSelectMenu.Info[] filterBy()
  {
    Dictionary<UnitGroupHead, List<int>> groupIDs = Object.op_Implicit((Object) this.popupFilter_) ? this.popupFilter_.selectedGroupIDs : this.filter_.selectedGroupIDs;
    if (groupIDs[UnitGroupHead.group_all].Any<int>())
      return ((IEnumerable<GachaPickupSelectMenu.Info>) this.allInfos_).ToArray<GachaPickupSelectMenu.Info>();
    List<GachaPickupSelectMenu.Info> infoList = new List<GachaPickupSelectMenu.Info>();
    Dictionary<int, UnitGroup> dictionary = ((IEnumerable<UnitGroup>) MasterData.UnitGroupList).ToDictionary<UnitGroup, int>((Func<UnitGroup, int>) (x => x.unit_id));
    for (int index = 0; index < this.allInfos_.Length; ++index)
    {
      GachaPickupSelectMenu.Info allInfo = this.allInfos_[index];
      object data = allInfo.data;
      if (data != null && data is UnitUnit unit && unit.IsNormalUnit && CharacterQuestFilter.Calculator.checkGroup(dictionary, unit, groupIDs, this.filter_.allGroupIDs))
        infoList.Add(allInfo);
    }
    return infoList.ToArray();
  }

  private void setLabelFilter()
  {
    string str1 = "filter_";
    List<int> selectedGroupId1 = this.filter_.selectedGroupIDs[UnitGroupHead.group_all];
    List<int> selectedGroupId2 = this.filter_.selectedGroupIDs[UnitGroupHead.group_large];
    List<int> selectedGroupId3 = this.filter_.selectedGroupIDs[UnitGroupHead.group_small];
    List<int> selectedGroupId4 = this.filter_.selectedGroupIDs[UnitGroupHead.group_clothing];
    List<int> selectedGroupId5 = this.filter_.selectedGroupIDs[UnitGroupHead.group_generation];
    string str2;
    if (selectedGroupId1.Contains(1))
      str2 = str1 + "group_all";
    else if (selectedGroupId1.Contains(2))
    {
      str2 = str1 + "group_none";
    }
    else
    {
      int count1 = selectedGroupId2.Count;
      int count2 = selectedGroupId3.Count;
      int count3 = selectedGroupId4.Count;
      int count4 = selectedGroupId5.Count;
      str2 = count1 != 1 || count2 != 0 || count3 != 0 || count4 != 0 ? (count1 != 0 || count2 != 1 || count3 != 0 || count4 != 0 ? (count1 != 0 || count2 != 0 || count3 != 1 || count4 != 0 ? (count1 != 0 || count2 != 0 || count3 != 0 || count4 != 1 ? str1 + "group_multiple" : string.Format("generation_{0}", (object) selectedGroupId5[0])) : string.Format("dresses_{0}", (object) selectedGroupId4[0])) : string.Format("s_classification_{0}", (object) selectedGroupId3[0])) : string.Format("l_classification_{0}", (object) selectedGroupId2[0]);
    }
    UISpriteData sprite = this.sprFilter_.atlas.GetSprite("slc_Label_" + str2 + "__GUI__unit_sort_label__unit_sort_label_prefab");
    if (sprite != null)
    {
      this.sprFilter_.spriteName = sprite.name;
      ((UIWidget) this.sprFilter_).width = sprite.width;
      ((UIWidget) this.sprFilter_).height = sprite.height;
      ((Component) this.sprFilter_).gameObject.SetActive(true);
    }
    else
      ((Component) this.sprFilter_).gameObject.SetActive(false);
  }

  protected override void Update()
  {
    base.Update();
    this.updateScroll();
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  [Serializable]
  private class SelectFrame
  {
    public GameObject top;
    public Transform lnk;
  }

  private class Info
  {
    public const int NONE_INDEX = -1;
    public int displayIndex = -1;
    public const int DESELECTED = 0;
    public int selected;

    public Info(
      WebAPI.Response.GachaG301PickupSelectPlayerPickupPickup_list entity)
    {
      this.webEntity = entity;
      this.rewardType = (MasterDataTable.CommonRewardType) entity.reward_type_id;
      int? q = entity.reward_quantity > 1 ? new int?(entity.reward_quantity) : new int?();
      bool np = !entity.is_possession;
      switch (this.rewardType)
      {
        case MasterDataTable.CommonRewardType.unit:
          UnitUnit targetUnit = MasterData.UnitUnit[entity.reward_id];
          if (targetUnit.IsNormalUnit)
          {
            UnitEvolutionPattern[] genealogy = UnitEvolutionPattern.getGenealogy(targetUnit.ID);
            if (genealogy.Length != 0)
              targetUnit = genealogy[genealogy.Length - 1].target_unit;
          }
          this.ID = targetUnit.ID;
          this.init(0, targetUnit, q, np);
          break;
        case MasterDataTable.CommonRewardType.gear:
          this.ID = entity.reward_id;
          this.init(0, MasterData.GearGear[entity.reward_id], false, q, np);
          break;
        case MasterDataTable.CommonRewardType.gear_body:
          this.init(0, MasterData.GearGear[entity.reward_id], true, q, np);
          break;
        default:
          Debug.LogError((object) string.Format("not support CommonRewardType.{0}", (object) this.rewardType));
          break;
      }
    }

    public Info(int i, UnitUnit u, int? q, bool np) => this.init(i, u, q, np);

    private void init(int i, UnitUnit u, int? q, bool np)
    {
      this.index = i;
      this.data = (object) u;
      this.quantity = q;
      this.isNotPossessed = np;
      this.historyNumber = u.history_group_number;
      this.isGearBody = false;
    }

    public Info(int i, GearGear g, int? q, bool np) => this.init(i, g, false, q, np);

    public Info(int i, GearGear g, bool bGearBody, bool np)
    {
      this.init(i, g, bGearBody, new int?(), np);
    }

    private void init(int i, GearGear g, bool bGearBody, int? q, bool np)
    {
      this.index = i;
      this.data = (object) g;
      this.quantity = q;
      this.isNotPossessed = np;
      this.historyNumber = g.history_group_number;
      this.isGearBody = bGearBody;
    }

    public int index { get; private set; }

    public void setIndex(int i) => this.index = i;

    public WebAPI.Response.GachaG301PickupSelectPlayerPickupPickup_list webEntity { get; private set; }

    public int ID { get; private set; }

    public MasterDataTable.CommonRewardType rewardType { get; private set; }

    public int historyNumber { get; private set; }

    public bool isGearBody { get; private set; }

    public object data { get; private set; }

    public UnitUnit unit => this.data as UnitUnit;

    public GearGear gear => this.data as GearGear;

    public bool hasQuantity => this.quantity.HasValue;

    public int? quantity { get; private set; }

    public bool isNotPossessed { get; private set; }

    public bool isSelected => this.selected > 0;
  }

  private class Icon
  {
    public Icon(int i) => this.index = i;

    public int index { get; private set; }

    public PickupSelectIcon icon { get; private set; }

    public void setIcon(PickupSelectIcon i) => this.icon = i;

    public GachaPickupSelectMenu.Info info { get; private set; }

    public bool setInfo(GachaPickupSelectMenu.Info v)
    {
      if (this.info == v)
        return false;
      this.info = v;
      return true;
    }
  }
}
