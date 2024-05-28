// Decompiled with JetBrains decompiler
// Type: Unit004ExtraSkillListMenuBase
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
public class Unit004ExtraSkillListMenuBase : BackButtonMenuBase
{
  protected int iconWidth = ExtraSkillIcon.Width;
  protected int iconHeight = ExtraSkillIcon.Height;
  protected int iconColumnValue = ExtraSkillIcon.ColumnValue;
  protected int iconRowValue = ExtraSkillIcon.RowValue;
  protected int iconScreenValue = ExtraSkillIcon.ScreenValue;
  protected int iconMaxValue = ExtraSkillIcon.MaxValue;
  protected GameObject ItemIconPrefab;
  protected bool InitializeEnd;
  protected int itemCount = -1;
  protected int itemFavoriteCount = -1;
  protected float scroolStartY;
  protected bool isUpdateIcon;
  protected List<ExtraSkillIcon> AllExtraSkillIcon = new List<ExtraSkillIcon>();
  protected List<InventoryExtraSkill> InventoryExtraSkills = new List<InventoryExtraSkill>();
  protected List<InventoryExtraSkill> DisplaySkills = new List<InventoryExtraSkill>();
  private Dictionary<int, Sprite> spriteCache = new Dictionary<int, Sprite>();
  private bool[] filter = new bool[25];
  private ExtraSkillSortAndFilter.SORT_TYPES sortCategory = ExtraSkillSortAndFilter.SORT_TYPES.Level;
  private SortAndFilter.SORT_TYPE_ORDER_BUY orderBuySort;
  protected GameObject SortPopupPrefab;
  [SerializeField]
  protected NGxScroll2 scroll;
  [SerializeField]
  protected ExtraSkillSortAndFilter.SORT_TYPES CurrentSortType;
  [SerializeField]
  protected UISprite SortSprite;
  [SerializeField]
  private GameObject dir_noList;

  public bool[] Filter
  {
    get => this.filter;
    set => this.filter = value;
  }

  public ExtraSkillSortAndFilter.SORT_TYPES SortCategory
  {
    get => this.sortCategory;
    set => this.sortCategory = value;
  }

  public SortAndFilter.SORT_TYPE_ORDER_BUY OrderBuySort
  {
    get => this.orderBuySort;
    set => this.orderBuySort = value;
  }

  protected Future<GameObject> GetSortAndFilterPopupGameObject()
  {
    return new ResourceObject("Prefabs/popup/popup_Extraskill_Sort__anim_popup01").Load<GameObject>();
  }

  public virtual Persist<Persist.ExtraSkillSortAndFilterInfo> GetPersist()
  {
    return (Persist<Persist.ExtraSkillSortAndFilterInfo>) null;
  }

  protected virtual List<PlayerAwakeSkill> GetExtraSkills() => (List<PlayerAwakeSkill>) null;

  protected virtual void UpdateInventoryExtraSkillList(
    InventoryExtraSkill invItem,
    PlayerAwakeSkill skill,
    bool favorite)
  {
    invItem.Init(skill);
    invItem.favorite = favorite;
  }

  protected virtual void UpdateInventoryExtraSkillList()
  {
    List<InventoryExtraSkill> list = this.InventoryExtraSkills.Where<InventoryExtraSkill>((Func<InventoryExtraSkill, bool>) (x => !x.removeButton)).ToList<InventoryExtraSkill>();
    if (list != null && list.Count<InventoryExtraSkill>() > 0)
    {
      List<PlayerAwakeSkill> extraSkills = this.GetExtraSkills();
      foreach (InventoryExtraSkill inventoryExtraSkill in list)
      {
        InventoryExtraSkill invItem = inventoryExtraSkill;
        PlayerAwakeSkill skill = extraSkills.FirstOrDefault<PlayerAwakeSkill>((Func<PlayerAwakeSkill, bool>) (x => x.id == invItem.uniqueID));
        if (skill != null)
          this.UpdateInventoryExtraSkillList(invItem, skill, skill.favorite);
      }
    }
    this.DisplayIconAndBottomInfoUpdate();
    this.isUpdateIcon = true;
  }

  protected virtual void CreateItemIconAdvencedSetting(int inventoryIdx, int allItemIdx)
  {
    ExtraSkillIcon extraSkillIcon = this.AllExtraSkillIcon[allItemIdx];
    InventoryExtraSkill displaySkill = this.DisplaySkills[inventoryIdx];
    extraSkillIcon.ForBattle = displaySkill.forBattle;
    extraSkillIcon.Favorite = displaySkill.favorite;
    extraSkillIcon.Gray = false;
    extraSkillIcon.Deselect();
  }

  protected virtual IEnumerator InitExtension()
  {
    yield break;
  }

  protected virtual void BottomInfoUpdate()
  {
  }

  protected virtual void AllItemIconUpdate()
  {
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
    {
      Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(true);
    }
    this.backScene();
  }

  public virtual IEnumerator Init()
  {
    Unit004ExtraSkillListMenuBase menu = this;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    menu.InitializeEnd = false;
    IEnumerator e = menu.LoadItemIconPrefab();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Equality((Object) menu.SortPopupPrefab, (Object) null))
    {
      Future<GameObject> sortPopupPrefabF = menu.GetSortAndFilterPopupGameObject();
      if (sortPopupPrefabF != null)
      {
        e = sortPopupPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        menu.SortPopupPrefab = sortPopupPrefabF.Result;
        menu.SortPopupPrefab.GetComponent<ExtraSkillSortAndFilter>().Initialize(menu);
      }
      sortPopupPrefabF = (Future<GameObject>) null;
    }
    List<PlayerAwakeSkill> extraSkills = menu.GetExtraSkills();
    int itemListCnt = menu.GetItemListCnt(extraSkills);
    int itemListFavoriteCnt = menu.GetItemListFavoriteCnt(extraSkills);
    if (menu.itemCount != itemListCnt || menu.itemFavoriteCount != itemListFavoriteCnt && !menu.Filter[11])
    {
      menu.itemCount = itemListCnt;
      menu.itemFavoriteCount = itemListFavoriteCnt;
      menu.InventoryExtraSkills.Clear();
      menu.CreateInvetoryExtraSkill(extraSkills);
      e = menu.InitExtension();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      menu.CreateExtraSkillIcon();
      menu.BottomInfoUpdate();
    }
    else
      menu.UpdateInventoryExtraSkillList();
    menu.InitializeEnd = true;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
    if (Object.op_Inequality((Object) menu.dir_noList, (Object) null))
      menu.dir_noList.SetActive(menu.DisplaySkills.Count <= 0);
  }

  public virtual void onEndScene() => Singleton<PopupManager>.GetInstance().closeAll();

  protected IEnumerator LoadItemIconPrefab()
  {
    if (Object.op_Equality((Object) this.ItemIconPrefab, (Object) null))
    {
      Future<GameObject> prefabF = new ResourceObject("Prefabs/extraskill/ExtraSkillThumb").Load<GameObject>();
      IEnumerator e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.ItemIconPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
  }

  protected IEnumerator LoadSpriteCache()
  {
    if (this.InventoryExtraSkills.Count > this.iconMaxValue)
    {
      for (int i = this.iconMaxValue; i < this.InventoryExtraSkills.Count; ++i)
      {
        BattleskillSkill masterData = this.InventoryExtraSkills[i].skill.masterData;
        if (!this.IsCache(masterData))
        {
          Future<Sprite> spriteF = masterData.LoadBattleSkillIcon();
          IEnumerator e = spriteF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          this.spriteCache[masterData.ID] = spriteF.Result;
          spriteF = (Future<Sprite>) null;
        }
        masterData = (BattleskillSkill) null;
      }
    }
  }

  protected int GetItemListCnt(List<PlayerAwakeSkill> itemList)
  {
    return itemList.Count<PlayerAwakeSkill>();
  }

  protected int GetItemListFavoriteCnt(List<PlayerAwakeSkill> skillList)
  {
    int num = 0;
    return skillList != null && skillList.Count<PlayerAwakeSkill>() > 0 ? skillList.Count<PlayerAwakeSkill>((Func<PlayerAwakeSkill, bool>) (x => x.favorite)) : num;
  }

  protected void CreateInvetoryExtraSkill(List<PlayerAwakeSkill> skillList)
  {
    if (skillList == null || skillList.Count<PlayerAwakeSkill>() <= 0)
      return;
    foreach (PlayerAwakeSkill skill in skillList)
      this.InventoryExtraSkills.Add(this.CreateInventoryExtraSkill(skill));
  }

  protected virtual InventoryExtraSkill CreateInventoryExtraSkill(PlayerAwakeSkill skill)
  {
    return new InventoryExtraSkill(skill);
  }

  protected void DisplayIconAndBottomInfoUpdate()
  {
    this.AllItemIconUpdate();
    this.BottomInfoUpdate();
  }

  protected void CreateExtraSkillIcon()
  {
    this.scroll.Clear();
    this.AllExtraSkillIcon.Clear();
    for (int index = 0; index < Mathf.Min(ItemIcon.MaxValue, this.InventoryExtraSkills.Count); ++index)
      this.AllExtraSkillIcon.Add(Object.Instantiate<GameObject>(this.ItemIconPrefab).GetComponent<ExtraSkillIcon>());
    this.Sort(this.SortCategory, this.OrderBuySort);
    this.scroolStartY = ((Component) this.scroll.scrollView).transform.localPosition.y;
    this.StartCoroutine(this.LoadSpriteCache());
  }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  public override void onBackButton() => this.IbtnBack();

  protected virtual void ChangeDetailScene(GameCore.ItemInfo item)
  {
    if (item == null)
      return;
    if (item.isWeapon)
      Unit00443Scene.changeScene(true, item);
    else
      Bugu00561Scene.changeScene(true, item);
  }

  public virtual void IbtnSort()
  {
    if (this.IsPush)
      return;
    this.ShowSortAndFilterPopup();
  }

  private void ShowSortAndFilterPopup()
  {
    if (!Singleton<PopupManager>.GetInstance().isOpen)
    {
      if (!Object.op_Inequality((Object) this.SortPopupPrefab, (Object) null))
        return;
      GameObject prefab = this.SortPopupPrefab.Clone();
      ExtraSkillSortAndFilter sortAndFilter = prefab.GetComponent<ExtraSkillSortAndFilter>();
      sortAndFilter.Initialize(this, true);
      sortAndFilter.SetSkillNum(this.InventoryExtraSkills.FilterBy(this.filter).ToList<InventoryExtraSkill>(), this.InventoryExtraSkills);
      sortAndFilter.SortFilterSkillNum = (Action) (() => sortAndFilter.SetSkillNum(this.InventoryExtraSkills.FilterBy(this.filter).ToList<InventoryExtraSkill>(), this.InventoryExtraSkills));
      Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    }
    else
      this.IsPush = false;
  }

  public virtual void Sort(
    ExtraSkillSortAndFilter.SORT_TYPES type,
    SortAndFilter.SORT_TYPE_ORDER_BUY order)
  {
    this.CurrentSortType = type;
    if (Object.op_Inequality((Object) this.SortSprite, (Object) null))
      this.SortSprite = ExtraSkillSortAndFilter.SortSpriteLabel(type, this.SortSprite);
    this.DisplaySkills = this.InventoryExtraSkills.FilterBy(this.filter).ToList<InventoryExtraSkill>();
    this.DisplaySkills = this.DisplaySkills.SortBy(type, order).ToList<InventoryExtraSkill>();
    this.scroll.Reset();
    this.AllExtraSkillIcon.ForEach((Action<ExtraSkillIcon>) (x =>
    {
      ((Component) x).transform.parent = ((Component) this).transform;
      ((Component) x).gameObject.SetActive(false);
    }));
    for (int index = 0; index < Mathf.Min(this.iconMaxValue, this.DisplaySkills.Count); ++index)
    {
      this.scroll.Add(((Component) this.AllExtraSkillIcon[index]).gameObject, this.iconWidth, this.iconHeight);
      ((Component) this.AllExtraSkillIcon[index]).gameObject.SetActive(true);
    }
    this.InventoryExtraSkills.ForEach((Action<InventoryExtraSkill>) (v => v.icon = (ExtraSkillIcon) null));
    this.StartCoroutine(this.CreateItemIconRange(Mathf.Min(this.iconMaxValue, this.DisplaySkills.Count)));
    this.scroll.CreateScrollPoint(this.iconHeight, this.DisplaySkills.Count);
    this.scroll.ResolvePosition();
    if (!Object.op_Inequality((Object) this.dir_noList, (Object) null))
      return;
    this.dir_noList.SetActive(this.DisplaySkills.Count <= 0);
  }

  private void ScrollIconUpdate(int inventoryItemsIndex, int count)
  {
    if (this.DisplaySkills[inventoryItemsIndex].removeButton || this.IsCache(this.DisplaySkills[inventoryItemsIndex].skill.masterData))
      this.CreateItemIconCache(inventoryItemsIndex, count);
    else
      this.StartCoroutine(this.CreateItemIcon(inventoryItemsIndex, count));
  }

  private void ScrollUpdate()
  {
    if ((!this.InitializeEnd || this.DisplaySkills.Count <= this.iconScreenValue) && !this.isUpdateIcon)
      return;
    int num1 = this.iconHeight * 2;
    float num2 = ((Component) this.scroll.scrollView).transform.localPosition.y - this.scroolStartY;
    float num3 = (float) (Mathf.Max(0, this.DisplaySkills.Count - this.iconScreenValue - 1) / this.iconColumnValue * this.iconHeight);
    float num4 = (float) (this.iconHeight * this.iconRowValue);
    if ((double) num2 < 0.0)
      num2 = 0.0f;
    if ((double) num2 > (double) num3)
      num2 = num3;
    bool flag;
    do
    {
      flag = false;
      int count = 0;
      foreach (GameObject gameObject in this.scroll)
      {
        GameObject item = gameObject;
        float num5 = item.transform.localPosition.y + num2;
        int? nullable = this.DisplaySkills.FirstIndexOrNull<InventoryExtraSkill>((Func<InventoryExtraSkill, bool>) (v => Object.op_Inequality((Object) v.icon, (Object) null) && Object.op_Equality((Object) ((Component) v.icon).gameObject, (Object) item)));
        if ((double) num5 > (double) num1)
        {
          item.transform.localPosition = new Vector3(item.transform.localPosition.x, item.transform.localPosition.y - num4, 0.0f);
          if (nullable.HasValue && nullable.Value + this.iconMaxValue < (this.DisplaySkills.Count + 4) / 5 * 5)
          {
            if (nullable.Value + this.iconMaxValue >= this.DisplaySkills.Count)
              item.SetActive(false);
            else
              this.ScrollIconUpdate(nullable.Value + this.iconMaxValue, count);
            flag = true;
          }
        }
        else if ((double) num5 < -((double) num4 - (double) num1))
        {
          int num6 = this.iconMaxValue;
          if (!item.activeSelf)
          {
            item.SetActive(true);
            num6 = 0;
          }
          if (nullable.HasValue && nullable.Value - num6 >= 0)
          {
            item.transform.localPosition = new Vector3(item.transform.localPosition.x, item.transform.localPosition.y + num4, 0.0f);
            this.ScrollIconUpdate(nullable.Value - num6, count);
            flag = true;
          }
        }
        else if (this.isUpdateIcon)
          this.ScrollIconUpdate(nullable.Value, count);
        ++count;
      }
    }
    while (flag);
    if (!this.isUpdateIcon)
      return;
    this.isUpdateIcon = false;
  }

  private void ResetItemIcon(int allItemIdx)
  {
    ((Component) this.AllExtraSkillIcon[allItemIdx]).gameObject.SetActive(false);
  }

  private IEnumerator CreateItemIconRange(int max)
  {
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    for (int index = 0; index < this.AllExtraSkillIcon.Count; ++index)
      ((Component) this.AllExtraSkillIcon[index]).gameObject.SetActive(false);
    for (int i = 0; i < max; ++i)
    {
      if (this.DisplaySkills[i].skill != null && this.IsCache(this.DisplaySkills[i].skill.masterData))
      {
        this.CreateItemIconCache(i, i);
      }
      else
      {
        IEnumerator e = this.CreateItemIcon(i, i);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    for (int index = 0; index < max; ++index)
      ((Component) this.AllExtraSkillIcon[index]).gameObject.SetActive(true);
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
  }

  private IEnumerator CreateItemIcon(int inventoryItemIdx, int allItemIdx)
  {
    ExtraSkillIcon skillIcon = (ExtraSkillIcon) null;
    skillIcon = this.AllExtraSkillIcon[allItemIdx];
    this.DisplaySkills.Where<InventoryExtraSkill>((Func<InventoryExtraSkill, bool>) (a => Object.op_Equality((Object) a.icon, (Object) skillIcon))).ForEach<InventoryExtraSkill>((Action<InventoryExtraSkill>) (b => b.icon = (ExtraSkillIcon) null));
    this.DisplaySkills[inventoryItemIdx].icon = skillIcon;
    if (this.DisplaySkills[inventoryItemIdx].removeButton)
    {
      skillIcon.InitByRemoveButton();
    }
    else
    {
      IEnumerator e = skillIcon.Init(this.DisplaySkills[inventoryItemIdx], this.spriteCache);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.CreateItemIconAdvencedSetting(inventoryItemIdx, allItemIdx);
  }

  private void CreateItemIconCache(int inventoryItemIdx, int allItemIdx)
  {
    ExtraSkillIcon itemIcon = this.AllExtraSkillIcon[allItemIdx];
    this.DisplaySkills.Where<InventoryExtraSkill>((Func<InventoryExtraSkill, bool>) (a => Object.op_Equality((Object) a.icon, (Object) itemIcon))).ForEach<InventoryExtraSkill>((Action<InventoryExtraSkill>) (b => b.icon = (ExtraSkillIcon) null));
    this.DisplaySkills[inventoryItemIdx].icon = itemIcon;
    if (this.DisplaySkills[inventoryItemIdx].removeButton)
      itemIcon.InitByRemoveButton();
    else
      itemIcon.InitByCache(this.DisplaySkills[inventoryItemIdx], this.spriteCache);
    this.CreateItemIconAdvencedSetting(inventoryItemIdx, allItemIdx);
  }

  public bool IsCache(BattleskillSkill skill) => this.spriteCache.ContainsKey(skill.ID);

  public Sprite SpriteCache(BattleskillSkill skill)
  {
    return this.IsCache(skill) ? this.spriteCache[skill.ID] : (Sprite) null;
  }

  public void ClearCache() => this.spriteCache.Clear();

  public void AllDeselect()
  {
    this.InventoryExtraSkills.ForEach((Action<InventoryExtraSkill>) (x => x.select = false));
  }

  private enum SortFilterPopupMode
  {
    None,
    Full,
    Material,
    Bugu,
    AlchemistMaterial,
    CompseMaterial,
  }
}
