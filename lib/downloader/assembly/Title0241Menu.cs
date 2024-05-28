// Decompiled with JetBrains decompiler
// Type: Title0241Menu
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
public class Title0241Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  private UIGrid grid;
  [SerializeField]
  private UIScrollView scrollview;
  [SerializeField]
  private UI2DSprite CurrentTitle;
  [SerializeField]
  private GameObject[] DisplayOrderSprites;
  private GameObject ScrollPrefab;
  private PlayerEmblem[] emblems;
  private const int UNKNOWN_ID = 99999;
  private int curEmblemID;
  private int[] displayEmblemIds;
  private string target_player_id;
  protected Persist<Persist.UnitSortAndFilterInfo> persist;
  private bool[] filter = new bool[Enum.GetNames(typeof (EmblemSortAndFilter.FILTER_TYPES)).Length];
  private EmblemSortAndFilter.SORT_TYPES sortCategory = EmblemSortAndFilter.SORT_TYPES.GetOrder;
  private SortAndFilter.SORT_TYPE_ORDER_BUY orderBySort;
  protected GameObject SortPopupPrefab;
  [SerializeField]
  protected NGxScroll2 scroll;
  [SerializeField]
  protected EmblemSortAndFilter.SORT_TYPES CurrentSortType;
  [SerializeField]
  protected UISprite SortSprite;

  public bool[] Filter
  {
    get => this.filter;
    set => this.filter = value;
  }

  public EmblemSortAndFilter.SORT_TYPES SortCategory
  {
    get => this.sortCategory;
    set => this.sortCategory = value;
  }

  public SortAndFilter.SORT_TYPE_ORDER_BUY OrderBySort
  {
    get => this.orderBySort;
    set => this.orderBySort = value;
  }

  protected Future<GameObject> GetSortAndFilterPopupGameObject()
  {
    return Res.Prefabs.popup.popup_024_3__anim_popup01.Load<GameObject>();
  }

  public virtual Persist<Persist.EmblemSortAndFilterInfo> GetPersist()
  {
    return Persist.emblemSortAndFilter;
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void Foreground()
  {
  }

  public virtual void IbtnTitleList()
  {
  }

  public virtual void VScrollBar()
  {
  }

  public IEnumerator Init(string in_target_player_id)
  {
    this.target_player_id = in_target_player_id;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e = this.EmblemsUpdate();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<GameObject> ScrollPrefabF = Res.Prefabs.title0024_1.vscroll.Load<GameObject>();
    e = ScrollPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.ScrollPrefab = ScrollPrefabF.Result;
    e = this.ListSorting();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.CreateSprite();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    if (Object.op_Equality((Object) this.SortPopupPrefab, (Object) null))
    {
      Future<GameObject> sortPopupPrefabF = this.GetSortAndFilterPopupGameObject();
      if (sortPopupPrefabF != null)
      {
        e = sortPopupPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.SortPopupPrefab = sortPopupPrefabF.Result;
      }
      sortPopupPrefabF = (Future<GameObject>) null;
    }
    this.DisplayOrderSpriteSetting((int) this.GetPersist().Data.sortType);
  }

  private IEnumerator ScrollInit(bool hasEmblem, EmblemEmblem emblem)
  {
    Title0241Menu baseMenu = this;
    GameObject gameObject = baseMenu.ScrollPrefab.Clone(((Component) baseMenu.grid).transform);
    bool isNew = false;
    DateTime? time = new DateTime?();
    if (hasEmblem)
    {
      PlayerEmblem playerEmblem = ((IEnumerable<PlayerEmblem>) baseMenu.emblems).Where<PlayerEmblem>((Func<PlayerEmblem, bool>) (x => x.emblem_id == emblem.ID)).First<PlayerEmblem>();
      isNew = playerEmblem.is_new;
      time = new DateTime?(playerEmblem.created_at);
    }
    IEnumerator e = gameObject.GetComponent<Title0241Scroll>().Init(hasEmblem, emblem.ID, emblem.rarity_EmblemRarity, emblem.description, emblem.ID == baseMenu.curEmblemID, isNew, time, new Action(baseMenu.EmblemsScrollUpdate), baseMenu.target_player_id == Player.Current.id, (BackButtonMenuBase) baseMenu);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual void IbtnSort()
  {
    if (this.IsPush)
      return;
    this.SortPopup();
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  private void DisplayOrderSpriteSetting(int type)
  {
    ((IEnumerable<GameObject>) this.DisplayOrderSprites).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(false)));
    this.DisplayOrderSprites[type - 1].SetActive(true);
  }

  private void SortPopup()
  {
    if (!Singleton<PopupManager>.GetInstance().isOpen)
    {
      if (!Object.op_Inequality((Object) this.SortPopupPrefab, (Object) null))
        return;
      GameObject prefab = this.SortPopupPrefab.Clone();
      prefab.GetComponent<EmblemSortAndFilter>().Initialize(this, true);
      Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    }
    else
      this.IsPush = false;
  }

  private IEnumerator EmblemsUpdate()
  {
    IEnumerator e;
    if (this.target_player_id == SMManager.Get<Player>().id)
    {
      Future<WebAPI.Response.EmblemStatus> emblemData = WebAPI.EmblemStatus();
      e = emblemData.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.emblems = emblemData.Result.emblems;
      this.displayEmblemIds = emblemData.Result.display_emblem_ids;
      this.curEmblemID = emblemData.Result.current_emblem_id == 0 ? 99999 : emblemData.Result.current_emblem_id;
      emblemData = (Future<WebAPI.Response.EmblemStatus>) null;
    }
    else
    {
      Future<WebAPI.Response.EmblemOtherPlayerStatus> emblemData = WebAPI.EmblemOtherPlayerStatus(this.target_player_id);
      e = emblemData.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.emblems = emblemData.Result.emblems;
      this.displayEmblemIds = emblemData.Result.display_emblem_ids;
      this.curEmblemID = emblemData.Result.current_emblem_id == 0 ? 99999 : emblemData.Result.current_emblem_id;
      emblemData = (Future<WebAPI.Response.EmblemOtherPlayerStatus>) null;
    }
  }

  public void ListSort() => this.StartCoroutine(this.ListSorting());

  private IEnumerator ListSorting()
  {
    Title0241Menu title0241Menu1 = this;
    foreach (Component component in ((Component) title0241Menu1.grid).transform)
      Object.Destroy((Object) component.gameObject);
    EmblemSortAndFilter.SORT_TYPES sortType = title0241Menu1.GetPersist().Data.sortType;
    List<bool> filter = title0241Menu1.GetPersist().Data.filter;
    title0241Menu1.OrderBySort = title0241Menu1.GetPersist().Data.order;
    title0241Menu1.DisplayOrderSpriteSetting((int) sortType);
    switch (sortType)
    {
      case EmblemSortAndFilter.SORT_TYPES.GetOrder:
        title0241Menu1.displayEmblemIds = ((IEnumerable<int>) title0241Menu1.displayEmblemIds).OrderBy<int, int>((Func<int, int>) (x => x)).ToArray<int>();
        List<int> source = title0241Menu1.OrderBySort != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? ((IEnumerable<PlayerEmblem>) title0241Menu1.emblems).OrderByDescending<PlayerEmblem, DateTime>((Func<PlayerEmblem, DateTime>) (x => x.created_at)).ThenByDescending<PlayerEmblem, int>((Func<PlayerEmblem, int>) (x => x.emblem_id)).Select<PlayerEmblem, int>((Func<PlayerEmblem, int>) (x => x.emblem_id)).ToList<int>() : ((IEnumerable<PlayerEmblem>) title0241Menu1.emblems).OrderBy<PlayerEmblem, DateTime>((Func<PlayerEmblem, DateTime>) (x => x.created_at)).ThenBy<PlayerEmblem, int>((Func<PlayerEmblem, int>) (x => x.emblem_id)).Select<PlayerEmblem, int>((Func<PlayerEmblem, int>) (x => x.emblem_id)).ToList<int>();
        foreach (int displayEmblemId1 in title0241Menu1.displayEmblemIds)
        {
          int displayEmblemId = displayEmblemId1;
          if (!source.Any<int>((Func<int, bool>) (x => x == displayEmblemId)))
            source.Add(displayEmblemId);
        }
        title0241Menu1.displayEmblemIds = source.ToArray();
        break;
      case EmblemSortAndFilter.SORT_TYPES.Rarity:
        List<int> intList = new List<int>();
        foreach (EmblemEmblem emblemEmblem in title0241Menu1.OrderBySort != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? ((IEnumerable<EmblemEmblem>) MasterData.EmblemEmblemList).OrderByDescending<EmblemEmblem, int>((Func<EmblemEmblem, int>) (x => x.rarity_EmblemRarity)).ThenByDescending<EmblemEmblem, int>((Func<EmblemEmblem, int>) (x => x.ID)).ToArray<EmblemEmblem>() : ((IEnumerable<EmblemEmblem>) MasterData.EmblemEmblemList).OrderBy<EmblemEmblem, int>((Func<EmblemEmblem, int>) (x => x.rarity_EmblemRarity)).ThenBy<EmblemEmblem, int>((Func<EmblemEmblem, int>) (x => x.ID)).ToArray<EmblemEmblem>())
        {
          EmblemEmblem allEmblem = emblemEmblem;
          if (((IEnumerable<int>) title0241Menu1.displayEmblemIds).Any<int>((Func<int, bool>) (x => x == allEmblem.ID)))
            intList.Add(allEmblem.ID);
        }
        title0241Menu1.displayEmblemIds = intList.ToArray();
        break;
      case EmblemSortAndFilter.SORT_TYPES.NumOrder:
        title0241Menu1.displayEmblemIds = title0241Menu1.OrderBySort != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? ((IEnumerable<int>) title0241Menu1.displayEmblemIds).OrderByDescending<int, int>((Func<int, int>) (x => x)).ToArray<int>() : ((IEnumerable<int>) title0241Menu1.displayEmblemIds).OrderBy<int, int>((Func<int, int>) (x => x)).ToArray<int>();
        break;
    }
    List<Title0241Menu.DisplayEmblems> displayEmblemsList = new List<Title0241Menu.DisplayEmblems>();
    Title0241Menu title0241Menu = title0241Menu1;
    for (int i = 0; i < title0241Menu1.displayEmblemIds.Length; i++)
    {
      Title0241Menu.DisplayEmblems displayEmblems = new Title0241Menu.DisplayEmblems();
      displayEmblems.emblem = MasterData.EmblemEmblem[title0241Menu1.displayEmblemIds[i]];
      displayEmblems.hasTitle = ((IEnumerable<PlayerEmblem>) title0241Menu1.emblems).Any<PlayerEmblem>((Func<PlayerEmblem, bool>) (x => x.emblem_id == title0241Menu.displayEmblemIds[i]));
      if (displayEmblems.emblem.category_id != EmblemCategory.cpu && filter[displayEmblems.emblem.category_id_EmblemCategory])
        displayEmblemsList.Add(displayEmblems);
    }
    foreach (Title0241Menu.DisplayEmblems displayEmblems in displayEmblemsList)
    {
      IEnumerator e = title0241Menu1.ScrollInit(displayEmblems.hasTitle, displayEmblems.emblem);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    ((Component) title0241Menu1.grid).gameObject.SetActive(true);
    title0241Menu1.grid.Reposition();
    title0241Menu1.scrollview.ResetPosition();
  }

  private IEnumerator CreateSprite()
  {
    Future<Sprite> sprF = EmblemUtility.LoadEmblemSprite(this.curEmblemID);
    IEnumerator e = sprF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.CurrentTitle.sprite2D = sprF.Result;
  }

  private void EmblemsScrollUpdate() => this.StartCoroutine(this.Init(this.target_player_id));

  private enum SortFilterPopupMode
  {
    None,
    Full,
    Material,
    Bugu,
    AlchemistMaterial,
    CompseMaterial,
  }

  private class DisplayEmblems
  {
    public bool hasTitle;

    public EmblemEmblem emblem { get; set; }
  }
}
