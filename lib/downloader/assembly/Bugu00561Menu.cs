// Decompiled with JetBrains decompiler
// Type: Bugu00561Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Bugu00561Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtDescription;
  [SerializeField]
  protected UILabel SupplyIntroduction;
  [SerializeField]
  protected UILabel GearIntroduction;
  [SerializeField]
  protected UILabel SupplyOwnednumber;
  [SerializeField]
  protected UILabel GearOwnednumber;
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  protected UI2DSprite LinkItem;
  [SerializeField]
  protected GameObject SupplyDetail;
  [SerializeField]
  protected GameObject GearDetail;
  [SerializeField]
  protected GameObject TouchBack;
  [SerializeField]
  protected BoxCollider ibtnBackCollider;
  [SerializeField]
  protected TweenAlpha slc_NewIcon;
  private GameCore.ItemInfo info;
  public NGxScroll scroll;
  [SerializeField]
  private GameObject rightArrows;
  [SerializeField]
  private GameObject leftArrows;
  private GameObject[] detailObject;
  [SerializeField]
  private UICenterOnChild centerOnChild;
  private readonly int DISPLAY_OBJECT_MAX = 4;
  private int objectCnt;
  private int currentInfoPageIndex;
  private int chacheCount;
  private int currentIndex;
  private GameObject buguDetail;
  private List<GameObject> objectList;
  private GearGear[] gearList;
  private Dictionary<GameObject, Unit00443BuguList> detailPrefabDict;
  private Unit00443BuguList currentBuguDatail;
  private bool isScrollViewDragStart;
  private int scrollStartCurrent;
  private GameCore.ItemInfo[] itemInfos;
  private bool isArrowBtns = true;
  public GameCore.ItemInfo RetentionGear;

  public IEnumerator onStartSceneAsync(GameCore.ItemInfo item)
  {
    this.leftArrows.SetActive(false);
    this.rightArrows.SetActive(false);
    ((Behaviour) this.scroll.scrollView).enabled = false;
    IEnumerator e = this.InitDetailedScreen(item);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(GearGear gear)
  {
    this.leftArrows.SetActive(false);
    this.rightArrows.SetActive(false);
    ((Behaviour) this.scroll.scrollView).enabled = false;
    IEnumerator e = this.InitDetailedScreen(gear);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    GameCore.ItemInfo[] items,
    bool isNew,
    bool isScreenTouch,
    int counter = 0)
  {
    this.leftArrows.SetActive(false);
    this.rightArrows.SetActive(false);
    yield return (object) null;
  }

  public IEnumerator InitDetailedScreen(GearGear gear)
  {
    if (Object.op_Inequality((Object) this.SupplyDetail, (Object) null))
      this.SupplyDetail.SetActive(false);
    if (Object.op_Inequality((Object) this.GearDetail, (Object) null))
      this.GearDetail.SetActive(true);
    Future<Sprite> spriteF = gear.LoadSpriteBasic();
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.LinkItem.sprite2D = spriteF.Result;
    UI2DSprite linkItem1 = this.LinkItem;
    Rect textureRect1 = spriteF.Result.textureRect;
    int num1 = Mathf.FloorToInt(((Rect) ref textureRect1).width);
    ((UIWidget) linkItem1).width = num1;
    UI2DSprite linkItem2 = this.LinkItem;
    Rect textureRect2 = spriteF.Result.textureRect;
    int num2 = Mathf.FloorToInt(((Rect) ref textureRect2).height);
    ((UIWidget) linkItem2).height = num2;
    if (Object.op_Inequality((Object) this.TxtTitle, (Object) null))
      this.TxtTitle.SetTextLocalize(gear.name);
    this.GearIntroduction.SetTextLocalize(gear.description);
  }

  public IEnumerator InitDetailedScreen(GameCore.ItemInfo item)
  {
    if (Object.op_Inequality((Object) this.SupplyDetail, (Object) null))
      this.SupplyDetail.SetActive(false);
    if (Object.op_Inequality((Object) this.GearDetail, (Object) null))
      this.GearDetail.SetActive(true);
    Future<Sprite> spriteF = item.gear.LoadSpriteBasic();
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.LinkItem.sprite2D = spriteF.Result;
    UI2DSprite linkItem1 = this.LinkItem;
    Rect textureRect1 = spriteF.Result.textureRect;
    int num1 = Mathf.FloorToInt(((Rect) ref textureRect1).width);
    ((UIWidget) linkItem1).width = num1;
    UI2DSprite linkItem2 = this.LinkItem;
    Rect textureRect2 = spriteF.Result.textureRect;
    int num2 = Mathf.FloorToInt(((Rect) ref textureRect2).height);
    ((UIWidget) linkItem2).height = num2;
    if (Object.op_Inequality((Object) this.TxtTitle, (Object) null))
      this.TxtTitle.SetTextLocalize(item.gear.name);
    this.GearIntroduction.SetTextLocalize(item.gear.description);
  }

  public IEnumerator InitDetailedScreen(
    GameCore.ItemInfo targetGear,
    GameCore.ItemInfo[] items,
    bool isNew,
    bool isScreenTouch,
    int counter = 0,
    int index = 0)
  {
    Bugu00561Menu bugu00561Menu = this;
    bugu00561Menu.TouchBack.SetActive(false);
    bugu00561Menu.RetentionGear = targetGear;
    if (items != null)
    {
      bugu00561Menu.itemInfos = items;
      bugu00561Menu.gearList = new GearGear[items.Length];
      for (int index1 = 0; index1 < items.Length; ++index1)
        bugu00561Menu.gearList[index1] = items[index1].gear;
    }
    else
    {
      bugu00561Menu.gearList = new GearGear[1]
      {
        targetGear.gear
      };
      bugu00561Menu.itemInfos = new GameCore.ItemInfo[1];
      bugu00561Menu.itemInfos[0] = bugu00561Menu.RetentionGear;
    }
    if (isScreenTouch)
      bugu00561Menu.ibtnBackCollider.size = new Vector3(2000f, 2000f, 0.0f);
    ((Component) bugu00561Menu.slc_NewIcon).gameObject.SetActive(isNew);
    if (isNew)
      ((UITweener) bugu00561Menu.slc_NewIcon).PlayForward();
    if (Object.op_Inequality((Object) bugu00561Menu.scroll, (Object) null))
      ((Behaviour) bugu00561Menu.scroll.scrollView).enabled = bugu00561Menu.gearList.Length > 1;
    Future<GameObject> prefabF = Res.Prefabs.unit004_4_3.bugu_detail_list.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    bugu00561Menu.buguDetail = prefabF.Result;
    bugu00561Menu.objectCnt = bugu00561Menu.gearList.Length;
    bugu00561Menu.currentIndex = index;
    bugu00561Menu.currentInfoPageIndex = 0;
    bugu00561Menu.chacheCount = 0;
    if (bugu00561Menu.objectCnt > bugu00561Menu.DISPLAY_OBJECT_MAX)
      bugu00561Menu.objectCnt = bugu00561Menu.DISPLAY_OBJECT_MAX;
    bugu00561Menu.objectList = new List<GameObject>();
    bugu00561Menu.detailObject = new GameObject[bugu00561Menu.objectCnt];
    bugu00561Menu.detailPrefabDict = new Dictionary<GameObject, Unit00443BuguList>();
    for (int index2 = 0; index2 < bugu00561Menu.objectCnt; ++index2)
    {
      bugu00561Menu.detailObject[index2] = Object.Instantiate<GameObject>(bugu00561Menu.buguDetail);
      bugu00561Menu.objectList.Add(bugu00561Menu.detailObject[index2]);
      bugu00561Menu.scroll.Add(bugu00561Menu.detailObject[index2]);
      Unit00443BuguList component = bugu00561Menu.detailObject[index2].GetComponent<Unit00443BuguList>();
      bugu00561Menu.detailPrefabDict.Add(bugu00561Menu.detailObject[index2], component);
    }
    yield return (object) null;
    bugu00561Menu.scroll.ResolvePosition();
    ((Component) bugu00561Menu.scroll.scrollView).transform.localPosition = new Vector3(-bugu00561Menu.scroll.grid.cellWidth * (float) bugu00561Menu.currentIndex, 0.0f, 0.0f);
    foreach (GameObject key in bugu00561Menu.detailObject)
      bugu00561Menu.detailPrefabDict[key].SetContainerPosition(bugu00561Menu.scroll.scrollView.panel.GetViewSize().y);
    yield return (object) null;
    e = bugu00561Menu.CreatePages(bugu00561Menu.currentIndex);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) null;
    int start = bugu00561Menu.currentIndex - 1 < 0 ? 0 : bugu00561Menu.currentIndex - 1;
    int end = bugu00561Menu.currentIndex + 1 >= bugu00561Menu.gearList.Length ? bugu00561Menu.gearList.Length - 1 : bugu00561Menu.currentIndex + 1;
    for (int i = start; i <= end; ++i)
    {
      if (i != bugu00561Menu.currentIndex)
      {
        e = bugu00561Menu.CreatePages(i);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    bugu00561Menu.SetMenuInformation(bugu00561Menu.currentIndex);
    bugu00561Menu.CenterOnChild(bugu00561Menu.currentIndex);
    yield return (object) null;
    if (items != null)
      bugu00561Menu.SetInfo(items[index]);
    else
      bugu00561Menu.SetInfo(bugu00561Menu.itemInfos[0]);
    if (counter > 1)
      ModalWindow.Show(Consts.Format(Consts.GetInstance().GACHA_0061MULTIPLE_WEAPONS_TITLE), Consts.Format(Consts.GetInstance().GACHA_0061MULTIPLE_WEAPONS_NUM, (IDictionary) new Hashtable()
      {
        {
          (object) "weapon",
          items != null ? (object) items[index].name.ToString() : (object) bugu00561Menu.itemInfos[0].name.ToString()
        },
        {
          (object) "num",
          (object) counter.ToString()
        }
      }), (Action) (() => { }));
    for (int index3 = 0; index3 < bugu00561Menu.objectList.Count; ++index3)
      bugu00561Menu.objectList[index3].transform.localPosition = end < bugu00561Menu.gearList.Length - 1 ? new Vector3(bugu00561Menu.scroll.grid.cellWidth * (float) (end + index3 + 1), 0.0f, 0.0f) : new Vector3(bugu00561Menu.scroll.grid.cellWidth * (float) (start - (index3 + 1)), 0.0f, 0.0f);
  }

  private void SetInfo(GameCore.ItemInfo item)
  {
    if (Object.op_Inequality((Object) this.SupplyDetail, (Object) null))
      this.SupplyDetail.SetActive(item.isSupply);
    if (Object.op_Inequality((Object) this.GearDetail, (Object) null))
      this.GearDetail.SetActive(!item.isSupply);
    ((Component) this.LinkItem).gameObject.SetActive(false);
    if (!item.isSupply)
    {
      if (Object.op_Inequality((Object) this.TxtTitle, (Object) null))
        this.TxtTitle.SetTextLocalize(item.gear.name);
      this.GearIntroduction.SetTextLocalize(item.gear.description);
      this.GearOwnednumber.SetTextLocalize(item.quantity.ToLocalizeNumberText());
    }
    else
    {
      if (Object.op_Inequality((Object) this.TxtTitle, (Object) null))
        this.TxtTitle.SetTextLocalize(item.supply.name);
      this.SupplyOwnednumber.SetTextLocalize(item.quantity.ToLocalizeNumberText());
      this.TxtDescription.SetTextLocalize(item.supply.description);
      this.SupplyIntroduction.SetTextLocalize(item.supply.flavor);
    }
  }

  public IEnumerator CreatePages(int gearIndex, bool isChangePage = false)
  {
    GameObject go = this.objectList[0];
    Unit00443BuguList d = this.detailPrefabDict[go];
    this.objectList.RemoveAt(0);
    Vector3 gridPos = ((Component) this.scroll.grid).transform.localPosition;
    go.transform.localPosition = new Vector3(this.scroll.grid.cellWidth * (float) gearIndex, 0.0f, 0.0f);
    yield return (object) null;
    IEnumerator e = d.Init(this.itemInfos[gearIndex]);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    d.index = gearIndex;
    go.transform.localPosition = new Vector3(this.scroll.grid.cellWidth * (float) gearIndex, 0.0f, 0.0f);
    ((Component) this.scroll.grid).transform.localPosition = gridPos;
    if (PerformanceConfig.GetInstance().IsLowMemory && this.chacheCount > 0)
    {
      GC.Collect();
      Singleton<ResourceManager>.GetInstance().ClearCache();
      Resources.UnloadUnusedAssets();
      this.chacheCount = 0;
    }
    if (isChangePage)
      ++this.chacheCount;
  }

  private void SetMenuInformation(int idx)
  {
    if (idx < 0 || idx > this.gearList.Length - 1)
      return;
    foreach (GameObject key in this.detailObject)
    {
      Unit00443BuguList unit00443BuguList = this.detailPrefabDict[key];
      if (unit00443BuguList.index == idx)
      {
        this.currentBuguDatail = unit00443BuguList;
        break;
      }
    }
    this.RetentionGear = this.itemInfos[idx];
    this.currentBuguDatail.SetGearInformation();
    if (Object.op_Inequality((Object) this.rightArrows, (Object) null))
      this.rightArrows.SetActive(true);
    if (Object.op_Inequality((Object) this.leftArrows, (Object) null))
      this.leftArrows.SetActive(true);
    if (idx == 0 && Object.op_Inequality((Object) this.leftArrows, (Object) null))
      this.leftArrows.SetActive(false);
    if (idx == this.gearList.Length - 1 && Object.op_Inequality((Object) this.rightArrows, (Object) null))
      this.rightArrows.SetActive(false);
    this.SetInfo(this.RetentionGear);
  }

  private void CenterOnChild(int num)
  {
    if (num < 0)
      return;
    foreach (GameObject key in this.detailObject)
    {
      if (this.detailPrefabDict[key].index == num)
      {
        this.centerOnChild.CenterOn(key.transform);
        break;
      }
    }
  }

  private void UpdateCurrentItem()
  {
    int num1 = this.currentIndex;
    if (Object.op_Equality((Object) this.scroll, (Object) null))
      return;
    if ((double) ((Component) this.scroll.scrollView).transform.localPosition.x < 0.0)
    {
      int num2 = (int) Mathf.Abs((((Component) this.scroll.scrollView).transform.localPosition.x - this.scroll.grid.cellWidth / 2f) / this.scroll.grid.cellWidth);
      num1 = num2 <= this.gearList.Length ? num2 : this.gearList.Length - 1;
    }
    if (this.currentIndex != num1)
    {
      int num3 = this.currentIndex < num1 ? 1 : 0;
      bool flag = true;
      this.currentIndex = num1;
      if (this.currentIndex < 0)
      {
        this.currentIndex = 0;
        flag = false;
      }
      if (this.currentIndex >= this.gearList.Length)
      {
        this.currentIndex = this.gearList.Length - 1;
        flag = false;
      }
      this.SetMenuInformation(this.currentIndex);
      this.UpdateObjectList();
      if (num3 != 0)
      {
        if (this.currentIndex < this.gearList.Length - 1)
          this.StartCoroutine(this.CreatePages(this.currentIndex + 1, true));
      }
      else if (this.currentIndex > 0)
        this.StartCoroutine(this.CreatePages(this.currentIndex - 1, true));
      if (flag)
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1005");
    }
    if (this.scroll.scrollView.isDragging)
    {
      if (this.isScrollViewDragStart)
        return;
      this.isScrollViewDragStart = true;
      this.scrollStartCurrent = this.currentIndex;
    }
    else
    {
      if (this.isScrollViewDragStart && this.scrollStartCurrent == this.currentIndex)
      {
        int currentIndex = this.currentIndex;
        double num4 = -(double) this.scroll.grid.cellWidth * (double) this.currentIndex;
        float num5 = this.scroll.grid.cellWidth * 0.25f;
        float num6 = (float) num4 - num5;
        float num7 = (float) num4 + num5;
        if ((double) ((Component) this.scroll.scrollView).transform.localPosition.x <= (double) num6)
          ++currentIndex;
        else if ((double) ((Component) this.scroll.scrollView).transform.localPosition.x >= (double) num7)
          --currentIndex;
        this.CenterOnChild(currentIndex <= this.gearList.Length ? currentIndex : this.gearList.Length - 1);
      }
      this.isScrollViewDragStart = false;
    }
  }

  private void UpdateObjectList()
  {
    foreach (GameObject key in this.detailObject)
    {
      int index = this.detailPrefabDict[key].index;
      if ((index < this.currentIndex - 1 || index > this.currentIndex + 1) && !this.objectList.Contains(key))
        this.objectList.Add(key);
    }
  }

  public void IbtnLeftArrows()
  {
    if (!this.isArrowBtns)
      return;
    this.isArrowBtns = false;
    this.StartCoroutine(this.IsArrowBtnOn());
    int num = this.currentIndex - 1;
    if (num < 0)
      return;
    this.CenterOnChild(num);
  }

  public void IbtnRightArrows()
  {
    if (!this.isArrowBtns)
      return;
    this.isArrowBtns = false;
    this.StartCoroutine(this.IsArrowBtnOn());
    int num = this.currentIndex + 1;
    if (num > this.gearList.Length - 1)
      return;
    this.CenterOnChild(num);
  }

  protected IEnumerator IsArrowBtnOn()
  {
    yield return (object) new WaitForSeconds(0.2f);
    this.isArrowBtns = true;
  }

  protected override void Update()
  {
    base.Update();
    this.UpdateCurrentItem();
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    if (this.RetentionGear != null)
      Singleton<NGGameDataManager>.GetInstance().lastReferenceItemID = this.RetentionGear.itemID;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();
}
