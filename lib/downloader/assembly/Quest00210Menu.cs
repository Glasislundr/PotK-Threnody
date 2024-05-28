// Decompiled with JetBrains decompiler
// Type: Quest00210Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Quest00210Menu : BackButtonMenuBase
{
  [SerializeField]
  private GameObject[] linkItem;
  [SerializeField]
  private GameObject[] selectButton;
  [SerializeField]
  private GameObject[] changeButton;
  [SerializeField]
  private List<SupplyItem> SupplyItems = new List<SupplyItem>();
  [SerializeField]
  private List<SupplyItem> SaveDeck = new List<SupplyItem>();
  [SerializeField]
  private GameObject FullButtonsAnchor;
  [SerializeField]
  private GameObject LimitedButtonAnchor;
  private ItemIcon[] icons = new ItemIcon[Consts.GetInstance().DECK_SUPPLY_MAX];
  private Quest00210Scene.Mode mode;
  private GameObject iconPrefab;

  public IEnumerator Init(List<SupplyItem> SupplyItems, Quest00210Scene.Mode mode)
  {
    this.mode = mode;
    if (mode == Quest00210Scene.Mode.Raid)
      this.FullButtonsAnchor.SetActive(false);
    else
      this.LimitedButtonAnchor.SetActive(false);
    IEnumerator e;
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

  private IEnumerator updateSupplyDeckAsync()
  {
    Quest00210Menu quest00210Menu = this;
    List<SupplyItem> source = quest00210Menu.SupplyItems.DeckList();
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
    if (quest00210Menu.mode == Quest00210Scene.Mode.Raid)
    {
      e = WebAPI.GuildraidBattleEditSupplyDeck(deck_quantities, deck_supply_ids).Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = WebAPI.ItemSupplyDeckEdit(deck_quantities, deck_supply_ids).Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    quest00210Menu.backScene();
  }

  private void updateEarthSupplyDeck()
  {
    Singleton<EarthDataManager>.GetInstance().SetSupplyBox(this.SupplyItems.DeckList().Select<SupplyItem, Tuple<int, int>>((Func<SupplyItem, Tuple<int, int>>) (x => new Tuple<int, int>(x.Supply.ID, x.SelectCount))).ToArray<Tuple<int, int>>());
    Singleton<EarthDataManager>.GetInstance().Save();
    this.backScene();
  }

  public void IbtnDecide()
  {
    if (this.IsPushAndSet())
      return;
    if (this.mode == Quest00210Scene.Mode.Earth)
      this.updateEarthSupplyDeck();
    else
      this.StartCoroutine(this.updateSupplyDeckAsync());
  }

  public void IbtnChange(int idx)
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
      limitedOnly = this.mode == Quest00210Scene.Mode.Raid,
      mode = this.mode
    });
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnChange() => this.IbtnChange(1);

  public virtual void IbtnChange2() => this.IbtnChange(2);

  public virtual void IbtnChange3() => this.IbtnChange(3);

  public virtual void IbtnChange4() => this.IbtnChange(4);

  public virtual void IbtnChange5() => this.IbtnChange(5);

  public virtual void IbtnPopupResign()
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

  public virtual void IbtnPopupRevival()
  {
    if (this.IsPush)
      return;
    this.SupplyItems.Fill();
    this.InitSaveSpace();
    this.StartCoroutine(this.RenderIcons());
  }

  private IEnumerator RenderIcons()
  {
    foreach (SupplyItem s in this.SupplyItems.DeckList())
    {
      ((Component) this.icons[s.DeckIndex - 1]).gameObject.SetActive(true);
      IEnumerator e = this.icons[s.DeckIndex - 1].InitBySupplyItem(s);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.icons[s.DeckIndex - 1].QuantitySupply = true;
      this.icons[s.DeckIndex - 1].EnableQuantity(s.SelectCount);
    }
    for (int i = 0; i < Consts.GetInstance().DECK_SUPPLY_MAX; i++)
    {
      if (this.SupplyItems.Where<SupplyItem>((Func<SupplyItem, bool>) (x => x.DeckIndex == i + 1)).Count<SupplyItem>() == 0)
      {
        this.icons[i].SetEmpty(true);
        this.icons[i].QuantitySupply = false;
      }
    }
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

  public class Param
  {
    public List<SupplyItem> SupplyItems;
    public List<SupplyItem> SaveDeck;
    public int select;
    public bool removeButton;
    public bool limitedOnly;
    public Quest00210Scene.Mode mode;
    public object[] args;
  }
}
