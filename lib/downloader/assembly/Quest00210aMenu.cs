// Decompiled with JetBrains decompiler
// Type: Quest00210aMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Quest00210aMenu : BackButtonMenuBase
{
  [SerializeField]
  private NGxScroll2 scroll;
  public UIButton decideBtn;
  public UIButton resetBtn;
  public Transform[] supportIconParent;
  private List<ItemIcon> currSupportItem;
  private int select;
  private List<SupplyItem> SupplyItems = new List<SupplyItem>();
  private List<SupplyItem> SaveDeck = new List<SupplyItem>();
  private int iconWidth = ItemIcon.Width;
  private int iconHeight = ItemIcon.Height;
  private int iconColumnValue = ItemIcon.ColumnValue;
  private int iconRowValue = ItemIcon.RowValue;
  private int iconScreenValue = ItemIcon.ScreenValue;
  private int iconMaxValue = ItemIcon.MaxValue;
  private bool isInitialize;
  private bool limitedOnly;
  private Quest00210Scene.Mode mode;
  private object[] args;
  private List<ItemIcon> allSupplyIcon = new List<ItemIcon>();
  private List<SupplyItem> showSupplyItems = new List<SupplyItem>();
  private List<SupplyItem> currSelectedItemList = new List<SupplyItem>();
  private float scrool_start_y;
  private GameObject SelectCountPopup;
  private GameObject confirmPopup;
  private GameObject warningPopup;
  private TowerProgress towerProgress;
  private TowerUtil.SequenceType towerSequenceType;
  private int[] towerSelectedUnitIDs;
  private CorpsUtil.SequenceType corpsSequenceType;
  private GameObject iconPrefab;
  private PlayerCorps playerCorps;
  private PlayerCorpsDeck playerCorpsDeck;
  private int[] corpsSelectedUnitIDs;
  protected const float LINK_HEIGHT = 100f;
  protected const float LINK_DEFHEIGHT = 136f;
  protected const float scale = 0.7352941f;

  public IEnumerator InitSupplyList(Quest00210Menu.Param param, bool fromTower)
  {
    Quest00210aMenu quest00210aMenu = this;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    quest00210aMenu.isInitialize = false;
    quest00210aMenu.limitedOnly = param.limitedOnly;
    quest00210aMenu.mode = param.mode;
    quest00210aMenu.args = param.args;
    quest00210aMenu.scroll.Clear();
    quest00210aMenu.select = quest00210aMenu.SupplyItems.DeckList().Count + 1;
    quest00210aMenu.SupplyItems = param.SupplyItems;
    quest00210aMenu.SupplyItems = quest00210aMenu.SupplyItems.OrderByDescending<SupplyItem, bool>((Func<SupplyItem, bool>) (x => x.SelectCount > 0)).ToList<SupplyItem>();
    quest00210aMenu.currSelectedItemList = quest00210aMenu.SupplyItems.DeckList();
    quest00210aMenu.SaveDeck = param.SaveDeck;
    quest00210aMenu.showSupplyItems = quest00210aMenu.SupplyItems;
    if (param.removeButton)
    {
      SupplyItem supplyItem = quest00210aMenu.SupplyItems[0].Copy();
      supplyItem.removeButton = true;
      quest00210aMenu.showSupplyItems.Insert(0, supplyItem);
    }
    Future<GameObject> countSelectF = Res.Prefabs.popup.popup_002_10_3.Load<GameObject>();
    IEnumerator e = countSelectF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    quest00210aMenu.SelectCountPopup = countSelectF.Result;
    Future<GameObject> prefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    quest00210aMenu.allSupplyIcon.Clear();
    for (int index = 0; index < Mathf.Min(ItemIcon.MaxValue, quest00210aMenu.showSupplyItems.Count); ++index)
    {
      ItemIcon component = Object.Instantiate<GameObject>(result).GetComponent<ItemIcon>();
      quest00210aMenu.allSupplyIcon.Add(component);
    }
    quest00210aMenu.scroll.Reset();
    for (int index = 0; index < Mathf.Min(quest00210aMenu.iconMaxValue, quest00210aMenu.allSupplyIcon.Count); ++index)
    {
      quest00210aMenu.scroll.Add(((Component) quest00210aMenu.allSupplyIcon[index]).gameObject, quest00210aMenu.iconWidth, quest00210aMenu.iconHeight);
      ((Component) quest00210aMenu.allSupplyIcon[index]).gameObject.SetActive(true);
    }
    quest00210aMenu.scroll.CreateScrollPoint(quest00210aMenu.iconHeight, quest00210aMenu.showSupplyItems.Count);
    quest00210aMenu.scroll.ResolvePosition();
    for (int index = 0; index < Mathf.Min(ItemIcon.MaxValue, quest00210aMenu.showSupplyItems.Count); ++index)
      quest00210aMenu.ResetItemIcon(index);
    for (int i = 0; i < Mathf.Min(ItemIcon.MaxValue, quest00210aMenu.showSupplyItems.Count); ++i)
    {
      e = quest00210aMenu.CreateItemIcon(i, i);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    for (int index = 0; index < quest00210aMenu.allSupplyIcon.Count; ++index)
      ((Component) quest00210aMenu.allSupplyIcon[index]).gameObject.SetActive(true);
    quest00210aMenu.scrool_start_y = ((Component) quest00210aMenu.scroll.scrollView).transform.localPosition.y;
    quest00210aMenu.decideBtn.onClick.Clear();
    // ISSUE: reference to a compiler-generated method
    quest00210aMenu.decideBtn.onClick.Add(new EventDelegate(new EventDelegate.Callback(quest00210aMenu.\u003CInitSupplyList\u003Eb__33_1)));
    quest00210aMenu.resetBtn.onClick.Clear();
    // ISSUE: reference to a compiler-generated method
    quest00210aMenu.resetBtn.onClick.Add(new EventDelegate(new EventDelegate.Callback(quest00210aMenu.\u003CInitSupplyList\u003Eb__33_2)));
    quest00210aMenu.StartCoroutine(quest00210aMenu.LoadObject());
    quest00210aMenu.StartCoroutine(quest00210aMenu.SetSupplyIcons());
    quest00210aMenu.RefreshSelectCount();
    quest00210aMenu.isInitialize = true;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
  }

  public void SelectRelease()
  {
    if (this.IsPushAndSet())
      return;
    this.scroll.Clear();
    this.backSceneByMode();
  }

  public void CountSelectPopUp(SupplyItem shotItem, Sprite icon)
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().open(this.SelectCountPopup).GetComponent<Quest002103Menu>().ChangePopUp(this, this.SupplyItems, shotItem, icon, this.SupplyItems.DeckList().Count + 1, this.SaveDeck);
  }

  private IEnumerator CreateItemIconRange(int value)
  {
    for (int index = 0; index < this.allSupplyIcon.Count; ++index)
      ((Component) this.allSupplyIcon[index]).gameObject.SetActive(false);
    for (int i = 0; i < value; ++i)
    {
      IEnumerator e = this.CreateItemIcon(i, i);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    for (int index = 0; index < this.allSupplyIcon.Count; ++index)
      ((Component) this.allSupplyIcon[index]).gameObject.SetActive(true);
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
  }

  private void ScrollUpdate()
  {
    if (!this.isInitialize || this.showSupplyItems.Count <= this.iconScreenValue)
      return;
    int num1 = this.iconHeight * 2;
    float num2 = ((Component) this.scroll.scrollView).transform.localPosition.y - this.scrool_start_y;
    float num3 = (float) (Mathf.Max(0, this.showSupplyItems.Count - this.iconScreenValue - 1) / this.iconColumnValue * this.iconHeight);
    float num4 = (float) (this.iconHeight * this.iconRowValue);
    if ((double) num2 < 0.0)
      num2 = 0.0f;
    if ((double) num2 > (double) num3)
      num2 = num3;
    bool flag;
    do
    {
      flag = false;
      int item_index = 0;
      foreach (GameObject gameObject in this.scroll)
      {
        GameObject item = gameObject;
        float num5 = item.transform.localPosition.y + num2;
        if ((double) num5 > (double) num1)
        {
          int? nullable = this.showSupplyItems.FirstIndexOrNull<SupplyItem>((Func<SupplyItem, bool>) (v => Object.op_Inequality((Object) v.icon, (Object) null) && Object.op_Equality((Object) ((Component) v.icon).gameObject, (Object) item)));
          int num6 = nullable.Value + this.iconMaxValue;
          if (nullable.HasValue && num6 < (this.showSupplyItems.Count + 4) / 5 * 5)
          {
            item.transform.localPosition = new Vector3(item.transform.localPosition.x, item.transform.localPosition.y - num4, 0.0f);
            if (num6 >= this.showSupplyItems.Count)
              item.SetActive(false);
            else if (this.showSupplyItems[num6].removeButton || ItemIcon.IsCache(this.showSupplyItems[num6]))
              this.CreateItemIconCache(num6, item_index);
            else
              this.StartCoroutine(this.CreateItemIcon(num6, item_index));
            flag = true;
          }
        }
        else if ((double) num5 < -((double) num4 - (double) num1))
        {
          int num7 = this.iconMaxValue;
          if (!item.activeSelf)
          {
            item.SetActive(true);
            num7 = 0;
          }
          int? nullable = this.showSupplyItems.FirstIndexOrNull<SupplyItem>((Func<SupplyItem, bool>) (v => Object.op_Inequality((Object) v.icon, (Object) null) && Object.op_Equality((Object) ((Component) v.icon).gameObject, (Object) item)));
          int num8 = nullable.Value - num7;
          if (nullable.HasValue && num8 >= 0)
          {
            item.transform.localPosition = new Vector3(item.transform.localPosition.x, item.transform.localPosition.y + num4, 0.0f);
            if (this.showSupplyItems[num8].removeButton || ItemIcon.IsCache(this.showSupplyItems[num8]))
              this.CreateItemIconCache(num8, item_index);
            else
              this.StartCoroutine(this.CreateItemIcon(num8, item_index));
            flag = true;
          }
        }
        ++item_index;
      }
    }
    while (flag);
  }

  private void ResetItemIcon(int index)
  {
    ((Component) this.allSupplyIcon[index]).gameObject.SetActive(false);
  }

  private IEnumerator CreateItemIcon(int info_index, int item_index)
  {
    ItemIcon itemIcon = this.allSupplyIcon[item_index];
    SupplyItem showSupplyItem = this.showSupplyItems[info_index];
    this.showSupplyItems.Where<SupplyItem>((Func<SupplyItem, bool>) (a => Object.op_Equality((Object) a.icon, (Object) itemIcon))).ForEach<SupplyItem>((Action<SupplyItem>) (b => b.icon = (ItemIcon) null));
    showSupplyItem.icon = itemIcon;
    if (showSupplyItem.removeButton)
    {
      itemIcon.InitByRemoveButton();
    }
    else
    {
      IEnumerator e = itemIcon.InitBySupplyItem(this.showSupplyItems[info_index]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.CreateItemIconSharing(info_index, item_index);
  }

  private void CreateItemIconCache(int info_index, int item_index)
  {
    ItemIcon itemIcon = this.allSupplyIcon[item_index];
    SupplyItem showSupplyItem = this.showSupplyItems[info_index];
    this.showSupplyItems.Where<SupplyItem>((Func<SupplyItem, bool>) (a => Object.op_Equality((Object) a.icon, (Object) itemIcon))).ForEach<SupplyItem>((Action<SupplyItem>) (b => b.icon = (ItemIcon) null));
    showSupplyItem.icon = itemIcon;
    if (showSupplyItem.removeButton)
      itemIcon.InitByRemoveButton();
    else
      itemIcon.InitBySupplyItemCache(showSupplyItem);
    this.CreateItemIconSharing(info_index, item_index);
  }

  private void CreateItemIconSharing(int info_index, int item_index)
  {
    ItemIcon itemIcon = this.allSupplyIcon[item_index];
    SupplyItem s_item = this.showSupplyItems[info_index];
    if (s_item.removeButton)
    {
      itemIcon.onClick = (Action<ItemIcon>) (supplyicon => this.SelectRelease());
      itemIcon.ForBattle = false;
      itemIcon.Gray = false;
    }
    else
    {
      itemIcon.onClick = !this.limitedOnly ? (Action<ItemIcon>) (supplyicon => this.CountSelectPopUp(s_item, supplyicon.IconSprite)) : (Action<ItemIcon>) (supplyicon =>
      {
        foreach (SupplyItem supplyItem in this.SupplyItems)
        {
          if (supplyItem.Supply.ID == s_item.Supply.ID)
          {
            supplyItem.SelectCount = this.currSelectedItemList.Contains(s_item) ? 0 : 1;
            supplyItem.DeckIndex = this.currSelectedItemList.Contains(s_item) ? -1 : this.select;
            this.InsertSelectItem(supplyItem);
            this.RefreshSelectCount();
            break;
          }
        }
      });
      itemIcon.ForBattle = false;
      itemIcon.Gray = false;
    }
    if (s_item.removeButton)
      EventDelegate.Set(itemIcon.supply.button.onLongPress, (EventDelegate.Callback) (() => { }));
    else
      EventDelegate.Set(itemIcon.supply.button.onLongPress, (EventDelegate.Callback) (() => Bugu00561Scene.changeScene(true, new GameCore.ItemInfo(((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.supply != null && x.supply.ID == s_item.Supply.ID)).FirstOrDefault<PlayerItem>()))));
  }

  private IEnumerator SetSupplyIcons()
  {
    Future<GameObject> prefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    this.currSupportItem = new List<ItemIcon>();
    for (int index = 0; index < this.supportIconParent.Length; ++index)
    {
      this.supportIconParent[index].Clear();
      GameObject gameObject = result.Clone(this.supportIconParent[index]);
      gameObject.transform.localScale = new Vector3(1f, 1f);
      ItemIcon component = gameObject.GetComponent<ItemIcon>();
      this.currSupportItem.Add(component);
      component.SetModeSupply();
      component.QuantitySupply = false;
      component.SetEmpty(true);
    }
  }

  public void RefreshSelectCount()
  {
    this.SetIconGray();
    List<SupplyItem> selectedItemList = this.currSelectedItemList;
    for (int index = 0; index < selectedItemList.Count; ++index)
    {
      selectedItemList[index].icon.Select(index);
      selectedItemList[index].Gray = false;
    }
    for (int index = 0; index < this.currSupportItem.Count; ++index)
    {
      this.currSupportItem[index].SetEmpty(true);
      if (selectedItemList.Count > index && selectedItemList[index] != null)
      {
        this.currSupportItem[index].SetEmpty(false);
        this.StartCoroutine(this.currSupportItem[index].InitBySupplyItem(selectedItemList[index]));
        this.currSupportItem[index].QuantitySupply = true;
        this.currSupportItem[index].SetModeSupply();
        this.currSupportItem[index].EnableQuantity(selectedItemList[index].SelectCount);
      }
    }
  }

  public void InsertSelectItem(SupplyItem item)
  {
    if (!this.currSelectedItemList.Contains(item))
    {
      if (item.SelectCount == 0)
        return;
      this.currSelectedItemList.Add(item);
    }
    else
    {
      for (int index = 0; index < this.currSelectedItemList.Count; ++index)
      {
        if (this.currSelectedItemList[index].Supply.ID == item.Supply.ID)
        {
          if (item.SelectCount != 0)
          {
            this.currSelectedItemList[index].SelectCount = item.SelectCount;
            break;
          }
          item.icon.ForBattle = false;
          item.icon.Deselect();
          this.currSelectedItemList.Remove(this.currSelectedItemList[index]);
          break;
        }
      }
    }
  }

  public List<SupplyItem> GetcurrSelectedItemList() => this.currSelectedItemList;

  private IEnumerator LoadObject()
  {
    if (this.showSupplyItems.Count > this.iconMaxValue)
    {
      for (int i = this.iconMaxValue; i < this.showSupplyItems.Count; ++i)
      {
        IEnumerator e = ItemIcon.LoadSprite(this.showSupplyItems[i]);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  public void onEndScene()
  {
  }

  public virtual void Foreground() => Debug.Log((object) "click default event Foreground");

  private void backSceneByMode()
  {
    switch (this.mode)
    {
      case Quest00210Scene.Mode.Tower:
        this.backScene();
        break;
      case Quest00210Scene.Mode.Raid:
        this.backScene();
        break;
      case Quest00210Scene.Mode.Corps:
        this.backScene();
        break;
      default:
        this.backScene();
        break;
    }
  }

  private IEnumerator updateSupplyDeckAsync()
  {
    Quest00210aMenu quest00210aMenu = this;
    List<SupplyItem> source = quest00210aMenu.SupplyItems.DeckList();
    int count = source.Count;
    int[] deck_quantities = new int[count];
    int[] deck_supply_ids = new int[count];
    for (int index = 0; index < count; ++index)
    {
      deck_quantities[index] = source.ElementAt<SupplyItem>(index).SelectCount;
      deck_supply_ids[index] = source.ElementAt<SupplyItem>(index).Supply.ID;
    }
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    IEnumerator e;
    switch (quest00210aMenu.mode)
    {
      case Quest00210Scene.Mode.Earth:
        quest00210aMenu.updateEarthSupplyDeck();
        break;
      case Quest00210Scene.Mode.Raid:
        e = WebAPI.GuildraidBattleEditSupplyDeck(deck_quantities, deck_supply_ids).Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      default:
        e = WebAPI.ItemSupplyDeckEdit(deck_quantities, deck_supply_ids).Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
    }
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    quest00210aMenu.backScene();
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.SupplyItems = this.SaveDeck.Copy();
    this.backSceneByMode();
  }

  public void OnBtnDecide()
  {
    switch (this.mode)
    {
      case Quest00210Scene.Mode.Tower:
        this.OnTowerDecideButton();
        break;
      case Quest00210Scene.Mode.Raid:
        this.StartCoroutine(this.updateSupplyDeckAsync());
        break;
      case Quest00210Scene.Mode.Corps:
        this.OnCorpsDecideButton();
        break;
      default:
        this.StartCoroutine(this.updateSupplyDeckAsync());
        break;
    }
  }

  public void OnBtnReset()
  {
    List<SupplyItem> supplyItemList = this.SupplyItems.DeckList();
    for (int index = 0; index < supplyItemList.Count; ++index)
    {
      supplyItemList[index].icon.ForBattle = false;
      supplyItemList[index].icon.Gray = false;
      supplyItemList[index].icon.Deselect();
    }
    this.SupplyItems.RemoveAll();
    this.currSelectedItemList.Clear();
    for (int index = 0; index < this.currSupportItem.Count; ++index)
    {
      this.currSupportItem[index].QuantitySupply = false;
      this.currSupportItem[index].SetEmpty(true);
    }
    this.SetIconGray();
  }

  private void OnTowerDecideButton() => this.StartCoroutine(this.OpenTowerPopup());

  private void OnCorpsDecideButton() => this.StartCoroutine(this.OpenCorpsPopup());

  private void SetIconGray()
  {
    List<SupplyItem> showSupplyItems = this.showSupplyItems;
    if (this.currSelectedItemList.Count >= Consts.GetInstance().DECK_SUPPLY_MAX)
    {
      for (int index = 0; index < showSupplyItems.Count; ++index)
      {
        if (showSupplyItems[index].SelectCount < 1)
        {
          showSupplyItems[index].icon.Gray = true;
          ((Behaviour) showSupplyItems[index].icon.supply.button).enabled = false;
        }
      }
    }
    else
    {
      for (int index = 0; index < showSupplyItems.Count; ++index)
      {
        showSupplyItems[index].icon.Gray = false;
        ((Behaviour) showSupplyItems[index].icon.supply.button).enabled = true;
      }
    }
  }

  private IEnumerator OpenTowerPopup()
  {
    Quest00210aMenu quest00210aMenu = this;
    Future<GameObject> f;
    IEnumerator e;
    if (Object.op_Equality((Object) quest00210aMenu.confirmPopup, (Object) null))
    {
      f = Res.Prefabs.popup.popup_029_tower_supplies_confirm__anim_popup01.Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      quest00210aMenu.confirmPopup = f.Result;
      f = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) quest00210aMenu.warningPopup, (Object) null))
    {
      f = Res.Prefabs.popup.popup_029_tower_supplies_warning_confirm__anim_popup01.Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      quest00210aMenu.warningPopup = f.Result;
      f = (Future<GameObject>) null;
    }
    GameObject prefab = quest00210aMenu.confirmPopup.Clone();
    prefab.SetActive(false);
    // ISSUE: reference to a compiler-generated method
    prefab.GetComponent<Tower029SupplyConfirmPopup>().Initialize(new Action(quest00210aMenu.\u003COpenTowerPopup\u003Eb__61_0));
    prefab.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }

  private IEnumerator OpenCorpsPopup()
  {
    Quest00210aMenu quest00210aMenu = this;
    Future<GameObject> f;
    IEnumerator e;
    if (Object.op_Equality((Object) quest00210aMenu.confirmPopup, (Object) null))
    {
      f = Res.Prefabs.popup.popup_029_tower_supplies_confirm__anim_popup01.Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      quest00210aMenu.confirmPopup = f.Result;
      f = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) quest00210aMenu.warningPopup, (Object) null))
    {
      f = Res.Prefabs.popup.popup_029_tower_supplies_warning_confirm__anim_popup01.Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      quest00210aMenu.warningPopup = f.Result;
      f = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) quest00210aMenu.iconPrefab, (Object) null))
    {
      f = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      quest00210aMenu.iconPrefab = f.Result;
      f = (Future<GameObject>) null;
    }
    GameObject prefab = quest00210aMenu.confirmPopup.Clone();
    prefab.SetActive(false);
    // ISSUE: reference to a compiler-generated method
    prefab.GetComponent<Tower029SupplyConfirmPopup>().Initialize(new Action(quest00210aMenu.\u003COpenCorpsPopup\u003Eb__62_0));
    prefab.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }

  private IEnumerator SetTowerSelectionUnitAndSupply()
  {
    Quest00210aMenu quest00210aMenu = this;
    quest00210aMenu.IsPush = true;
    Singleton<PopupManager>.GetInstance().dismiss(true);
    while (Singleton<PopupManager>.GetInstance().isOpen)
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    List<int> intList1 = new List<int>();
    List<int> intList2 = new List<int>();
    foreach (SupplyItem deck in quest00210aMenu.SupplyItems.DeckList())
    {
      intList1.Add(deck.SelectCount);
      intList2.Add(deck.Supply.ID);
    }
    IEnumerator e1;
    if (quest00210aMenu.towerSequenceType == TowerUtil.SequenceType.Restart)
    {
      Future<WebAPI.Response.TowerRestart> f = WebAPI.TowerRestart(intList1.ToArray(), intList2.ToArray(), quest00210aMenu.towerSelectedUnitIDs, quest00210aMenu.towerProgress.tower_id, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
      e1 = f.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (f.Result == null)
      {
        TowerUtil.GotoMypage();
        yield break;
      }
      else
        f = (Future<WebAPI.Response.TowerRestart>) null;
    }
    else if (quest00210aMenu.towerSequenceType == TowerUtil.SequenceType.Recovery)
    {
      Future<WebAPI.Response.TowerReassign> f = WebAPI.TowerReassign(intList1.ToArray(), intList2.ToArray(), TowerUtil.PayRecoveryCoinNum == 0, quest00210aMenu.towerSelectedUnitIDs, quest00210aMenu.towerProgress.tower_id, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
      e1 = f.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (f.Result == null)
      {
        TowerUtil.GotoMypage();
        yield break;
      }
      else
        f = (Future<WebAPI.Response.TowerReassign>) null;
    }
    else
    {
      Future<WebAPI.Response.TowerEntry> f = WebAPI.TowerEntry(intList1.ToArray(), intList2.ToArray(), quest00210aMenu.towerSelectedUnitIDs, quest00210aMenu.towerProgress.tower_id, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
      e1 = f.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (f.Result == null)
      {
        TowerUtil.GotoMypage();
        yield break;
      }
      else
        f = (Future<WebAPI.Response.TowerEntry>) null;
    }
    e1 = quest00210aMenu.UpdateTowerDeck((Action) (() =>
    {
      Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
      Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID = -1;
      Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitIndex = -1;
      Tower029TopScene.ChangeScene(false, true);
    }));
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
  }

  private IEnumerator SetCorpsSelectionUnitAndSupply()
  {
    Quest00210aMenu quest00210aMenu = this;
    quest00210aMenu.IsPush = true;
    Singleton<PopupManager>.GetInstance().dismiss(true);
    while (Singleton<PopupManager>.GetInstance().isOpen)
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    List<int> intList1 = new List<int>();
    List<int> intList2 = new List<int>();
    foreach (SupplyItem deck in quest00210aMenu.SupplyItems.DeckList())
    {
      intList1.Add(deck.SelectCount);
      intList2.Add(deck.Supply.ID);
    }
    IEnumerator e1;
    if (quest00210aMenu.corpsSequenceType == CorpsUtil.SequenceType.Reset)
    {
      Future<WebAPI.Response.QuestCorpsReset> f = WebAPI.QuestCorpsReset(intList1.ToArray(), intList2.ToArray(), quest00210aMenu.playerCorps.period_id, quest00210aMenu.corpsSelectedUnitIDs, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
      e1 = f.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (f.Result == null)
      {
        MypageScene.ChangeSceneOnError();
        yield break;
      }
      else
      {
        Singleton<NGGameDataManager>.GetInstance().corps_player_unit_ids = new HashSet<int>((IEnumerable<int>) f.Result.corps_player_unit_ids);
        f = (Future<WebAPI.Response.QuestCorpsReset>) null;
      }
    }
    else
    {
      Future<WebAPI.Response.QuestCorpsEntry> f = WebAPI.QuestCorpsEntry(intList1.ToArray(), intList2.ToArray(), quest00210aMenu.playerCorps.period_id, quest00210aMenu.corpsSelectedUnitIDs, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
      e1 = f.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (f.Result == null)
      {
        MypageScene.ChangeSceneOnError();
        yield break;
      }
      else
      {
        Singleton<NGGameDataManager>.GetInstance().corps_player_unit_ids = new HashSet<int>((IEnumerable<int>) f.Result.corps_player_unit_ids);
        f = (Future<WebAPI.Response.QuestCorpsEntry>) null;
      }
    }
    // ISSUE: reference to a compiler-generated method
    e1 = quest00210aMenu.UpdateCorpsDeck(new Action(quest00210aMenu.\u003CSetCorpsSelectionUnitAndSupply\u003Eb__64_2));
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
  }

  private IEnumerator UpdateTowerDeck(Action successAction)
  {
    if (TowerUtil.towerDeckUnits == null || TowerUtil.towerDeckUnits.Length == 0)
    {
      if (successAction != null)
        successAction();
    }
    else
    {
      List<int> intList = new List<int>();
      bool flag = false;
      for (int index = 0; index < TowerUtil.towerDeckUnits.Length; ++index)
      {
        TowerDeckUnit deckUnit = TowerUtil.towerDeckUnits[index];
        if (((IEnumerable<int>) this.towerSelectedUnitIDs).Any<int>((Func<int, bool>) (x => x == deckUnit.player_unit_id)))
          intList.Add(deckUnit.player_unit_id);
        else
          flag = true;
      }
      if (flag)
      {
        Future<WebAPI.Response.TowerDeckEdit> f = WebAPI.TowerDeckEdit(intList.ToArray(), this.towerProgress.tower_id, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
        IEnumerator e1 = f.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        if (f.Result == null)
        {
          TowerUtil.GotoMypage();
          yield break;
        }
        else
        {
          TowerUtil.towerDeckUnits = ((IEnumerable<TowerDeckUnit>) f.Result.tower_deck.tower_deck_units).OrderBy<TowerDeckUnit, int>((Func<TowerDeckUnit, int>) (u => u.position_id)).ToArray<TowerDeckUnit>();
          f = (Future<WebAPI.Response.TowerDeckEdit>) null;
        }
      }
      if (successAction != null)
        successAction();
    }
  }

  private IEnumerator UpdateCorpsDeck(Action successAction)
  {
    Quest00210aMenu quest00210aMenu = this;
    if (quest00210aMenu.playerCorpsDeck == null || quest00210aMenu.playerCorpsDeck.deck_player_unit_ids.Length == 0)
    {
      successAction();
    }
    else
    {
      List<int> intList = new List<int>(quest00210aMenu.playerCorpsDeck.deck_player_unit_ids.Length);
      bool flag = false;
      for (int index = 0; index < quest00210aMenu.playerCorpsDeck.deck_player_unit_ids.Length; ++index)
      {
        if (((IEnumerable<int>) quest00210aMenu.corpsSelectedUnitIDs).Contains<int>(quest00210aMenu.playerCorpsDeck.deck_player_unit_ids[index]))
          intList.Add(quest00210aMenu.playerCorpsDeck.deck_player_unit_ids[index]);
        else
          flag = true;
      }
      if (flag)
      {
        Future<WebAPI.Response.QuestCorpsDeckEdit> f = WebAPI.QuestCorpsDeckEdit(quest00210aMenu.playerCorps.period_id, intList.ToArray(), (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
        IEnumerator e1 = f.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        if (f.Result == null)
        {
          MypageScene.ChangeSceneOnError();
          yield break;
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          quest00210aMenu.playerCorpsDeck = Array.Find<PlayerCorpsDeck>(f.Result.player_corps_decks, new Predicate<PlayerCorpsDeck>(quest00210aMenu.\u003CUpdateCorpsDeck\u003Eb__66_1));
          f = (Future<WebAPI.Response.QuestCorpsDeckEdit>) null;
        }
      }
      successAction();
    }
  }

  public void SetTowerInfo(
    int[] selectedUnitIds,
    TowerProgress progress,
    TowerUtil.SequenceType type)
  {
    this.towerSelectedUnitIDs = selectedUnitIds;
    this.towerProgress = progress;
    this.towerSequenceType = type;
  }

  public void SetCorpsInfo(
    int[] selectedUnitIds,
    PlayerCorps progress,
    CorpsUtil.SequenceType type)
  {
    this.corpsSelectedUnitIDs = selectedUnitIds;
    this.playerCorps = progress;
    this.corpsSequenceType = type;
  }

  private void updateEarthSupplyDeck()
  {
    Singleton<EarthDataManager>.GetInstance().SetSupplyBox(this.SupplyItems.DeckList().Select<SupplyItem, Tuple<int, int>>((Func<SupplyItem, Tuple<int, int>>) (x => new Tuple<int, int>(x.Supply.ID, x.SelectCount))).ToArray<Tuple<int, int>>());
    Singleton<EarthDataManager>.GetInstance().Save();
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void VScrollBar() => Debug.Log((object) "click default event VScrollBar");
}
