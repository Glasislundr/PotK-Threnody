// Decompiled with JetBrains decompiler
// Type: Tower029SupplyEditMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Tower029SupplyEditMenu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel lblDesc;
  [SerializeField]
  private GameObject[] linkItem;
  [SerializeField]
  private GameObject[] selectButton;
  [SerializeField]
  private GameObject[] changeButton;
  private GameObject confirmPopup;
  private GameObject warningPopup;
  private int[] selectedUnitIDs;
  private List<SupplyItem> SupplyItems = new List<SupplyItem>();
  private List<SupplyItem> SaveDeck = new List<SupplyItem>();
  private ItemIcon[] icons = new ItemIcon[Consts.GetInstance().DECK_SUPPLY_MAX];
  private GameObject iconPrefab;
  private TowerProgress progress;
  private TowerUtil.SequenceType sequenceType;

  private IEnumerator ResourceLoad()
  {
    Future<GameObject> f;
    IEnumerator e;
    if (Object.op_Equality((Object) this.confirmPopup, (Object) null))
    {
      f = Res.Prefabs.popup.popup_029_tower_supplies_confirm__anim_popup01.Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.confirmPopup = f.Result;
      f = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.warningPopup, (Object) null))
    {
      f = Res.Prefabs.popup.popup_029_tower_supplies_warning_confirm__anim_popup01.Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.warningPopup = f.Result;
      f = (Future<GameObject>) null;
    }
  }

  private IEnumerator SetSelectionUnitAndSupply()
  {
    Tower029SupplyEditMenu tower029SupplyEditMenu = this;
    tower029SupplyEditMenu.IsPush = true;
    Singleton<PopupManager>.GetInstance().dismiss(true);
    while (Singleton<PopupManager>.GetInstance().isOpen)
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    List<int> intList1 = new List<int>();
    List<int> intList2 = new List<int>();
    foreach (SupplyItem deck in tower029SupplyEditMenu.SupplyItems.DeckList())
    {
      intList1.Add(deck.SelectCount);
      intList2.Add(deck.Supply.ID);
    }
    IEnumerator e1;
    if (tower029SupplyEditMenu.sequenceType == TowerUtil.SequenceType.Restart)
    {
      Future<WebAPI.Response.TowerRestart> f = WebAPI.TowerRestart(intList1.ToArray(), intList2.ToArray(), tower029SupplyEditMenu.selectedUnitIDs, tower029SupplyEditMenu.progress.tower_id, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
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
    else if (tower029SupplyEditMenu.sequenceType == TowerUtil.SequenceType.Recovery)
    {
      Future<WebAPI.Response.TowerReassign> f = WebAPI.TowerReassign(intList1.ToArray(), intList2.ToArray(), TowerUtil.PayRecoveryCoinNum == 0, tower029SupplyEditMenu.selectedUnitIDs, tower029SupplyEditMenu.progress.tower_id, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
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
      Future<WebAPI.Response.TowerEntry> f = WebAPI.TowerEntry(intList1.ToArray(), intList2.ToArray(), tower029SupplyEditMenu.selectedUnitIDs, tower029SupplyEditMenu.progress.tower_id, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
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
    e1 = tower029SupplyEditMenu.UpdateTowerDeck((Action) (() =>
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
        if (((IEnumerable<int>) this.selectedUnitIDs).Any<int>((Func<int, bool>) (x => x == deckUnit.player_unit_id)))
          intList.Add(deckUnit.player_unit_id);
        else
          flag = true;
      }
      if (flag)
      {
        Future<WebAPI.Response.TowerDeckEdit> f = WebAPI.TowerDeckEdit(intList.ToArray(), this.progress.tower_id, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
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

  private void IbtnChange(int idx)
  {
    if (this.IsPushAndSet())
      return;
    this.InitSaveSpace();
    bool flag = this.SupplyItems.RemoveDeck(idx);
    Quest00210aScene.ChangeScene(false, new Quest00210Menu.Param()
    {
      SupplyItems = this.SupplyItems,
      SaveDeck = this.SaveDeck,
      select = idx,
      removeButton = flag,
      limitedOnly = true,
      mode = Quest00210Scene.Mode.Tower
    }, true);
  }

  private void UpdateSelectButton()
  {
    for (int index = 0; index < Consts.GetInstance().DECK_SUPPLY_MAX; ++index)
      this.changeButton[index].SetActive(false);
    int num = 0;
    for (int index = 0; index < this.SupplyItems.Count; ++index)
    {
      if (this.SupplyItems[index].DeckIndex > 0)
      {
        this.changeButton[this.SupplyItems[index].DeckIndex - 1].SetActive(true);
        ++num;
      }
    }
  }

  private void InitSaveSpace() => this.SaveDeck = this.SupplyItems.Copy();

  public IEnumerator InitializeAsync(
    int[] unitIDs,
    List<SupplyItem> SupplyItems,
    TowerProgress progress,
    TowerUtil.SequenceType type)
  {
    this.lblDesc.SetTextLocalize(Consts.GetInstance().TOWER_SUPPLY_EDIT_DESCRIPTION);
    IEnumerator e = this.ResourceLoad();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (unitIDs != null)
      this.selectedUnitIDs = unitIDs;
    if (progress != null)
      this.progress = progress;
    if (type != TowerUtil.SequenceType.None)
      this.sequenceType = type;
    if (Object.op_Equality((Object) this.iconPrefab, (Object) null))
    {
      Future<GameObject> prefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.iconPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    for (int index = 0; index < Consts.GetInstance().DECK_SUPPLY_MAX; ++index)
    {
      GameObject self = Object.Instantiate<GameObject>(this.iconPrefab);
      if (Object.op_Inequality((Object) this.icons[index], (Object) null))
        Object.Destroy((Object) ((Component) this.icons[index]).gameObject);
      self.SetParent(this.linkItem[index]);
      ItemIcon component = self.GetComponent<ItemIcon>();
      component.SetModeSupply();
      this.icons[index] = component;
    }
    this.SupplyItems = SupplyItems;
    this.InitSaveSpace();
    int counter = 0;
    foreach (SupplyItem s in SupplyItems)
    {
      if (s.DeckIndex != 0)
      {
        this.icons[s.DeckIndex - 1].SetEmpty(false);
        e = this.icons[s.DeckIndex - 1].InitBySupplyItem(s);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.icons[s.DeckIndex - 1].QuantitySupply = true;
        this.icons[s.DeckIndex - 1].EnableQuantity(s.SelectCount);
        ++counter;
      }
    }
    for (int i = 0; i < Consts.GetInstance().DECK_SUPPLY_MAX; i++)
    {
      if (SupplyItems.Where<SupplyItem>((Func<SupplyItem, bool>) (x => x.DeckIndex == i + 1)).Count<SupplyItem>() == 0)
        this.icons[i].SetEmpty(true);
    }
    this.UpdateSelectButton();
  }

  public void onDecideButton()
  {
    if (this.IsPushAndSet())
      return;
    GameObject prefab = this.confirmPopup.Clone();
    prefab.SetActive(false);
    prefab.GetComponent<Tower029SupplyConfirmPopup>().Initialize((Action) (() => this.StartCoroutine(this.SetSelectionUnitAndSupply())));
    prefab.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }

  public void onSupplyButton1() => this.IbtnChange(1);

  public void onSupplyButton2() => this.IbtnChange(2);

  public void onSupplyButton3() => this.IbtnChange(3);

  public void onSupplyButton4() => this.IbtnChange(4);

  public void onSupplyButton5() => this.IbtnChange(5);

  public void onResetButton()
  {
    if (this.IsPush)
      return;
    this.SupplyItems.RemoveAll();
    for (int index = 0; index < Consts.GetInstance().DECK_SUPPLY_MAX; ++index)
    {
      this.icons[index].SetEmpty(true);
      this.icons[index].QuantitySupply = false;
    }
    this.UpdateSelectButton();
    this.InitSaveSpace();
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }
}
