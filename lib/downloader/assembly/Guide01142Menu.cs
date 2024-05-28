// Decompiled with JetBrains decompiler
// Type: Guide01142Menu
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
public class Guide01142Menu : Unit00443Menu
{
  [SerializeField]
  private GearGear gear_;
  [SerializeField]
  protected UILabel TxtNumber;
  [SerializeField]
  protected GameObject dirNumber;
  public NGxScroll scroll;
  [SerializeField]
  private GameObject rightArrow;
  [SerializeField]
  private GameObject leftArrow;
  private int currentIndex;
  private int lastIndex;
  private GameObject[] detailObject;
  private Dictionary<GameObject, Unit00443BuguList> detailPrefabDict;
  private bool firstInit;
  private bool isArrowBtn = true;
  [SerializeField]
  private UICenterOnChild centerOnChild;
  [Header("Use by guide detail")]
  private GameObject buguDetail;
  public UILabel title;
  public UI2DSprite raritySIcons;
  private readonly int DISPLAY_OBJECT_MAX = 4;
  private int objectCnt;
  private List<GameObject> objectList;
  private GearGear[] gearList;
  private int[] quantityList;
  private bool isDispNumber;
  private bool isScrollViewDragStart;
  private int scrollStartCurrent;
  private Unit00443BuguList currentBuguDatail;
  private int chacheCount;
  [SerializeField]
  private GameObject NumPossession;
  [SerializeField]
  private UILabel txtUnit;
  private int[] itemIDs;

  public IEnumerator onStartSceneAsync(GearGear gear, bool isDispNumber, int index = 0)
  {
    this.leftArrow.SetActive(false);
    this.rightArrow.SetActive(false);
    ((Behaviour) this.scroll.scrollView).enabled = false;
    IEnumerator e = this.GearsInit(new GearGear[1]{ gear }, (int[]) null, (int[]) null, isDispNumber, index);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    GearGear[] gears,
    int[] quantitys,
    int[] itemIDs,
    bool isDispNumber,
    int index = 0)
  {
    this.leftArrow.SetActive(false);
    this.rightArrow.SetActive(false);
    IEnumerator e = this.GearsInit(gears, quantitys, itemIDs, isDispNumber, index);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(GearGear gear, int quantity, int index)
  {
    Guide01142Menu guide01142Menu = this;
    guide01142Menu.leftArrow.SetActive(false);
    guide01142Menu.rightArrow.SetActive(false);
    ((Behaviour) guide01142Menu.scroll.scrollView).enabled = false;
    IEnumerator e = guide01142Menu.GearsInit(new GearGear[1]
    {
      gear
    }, new int[1]{ quantity }, (int[]) null, false, index);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator GearsInit(
    GearGear[] gear,
    int[] quantity,
    int[] itemIDs,
    bool isDispNumber,
    int index)
  {
    Guide01142Menu guide01142Menu = this;
    if (guide01142Menu.firstInit)
    {
      guide01142Menu.SetChangeActiveComponent(true);
      guide01142Menu.SetMenuInformation(guide01142Menu.currentIndex);
    }
    else
    {
      guide01142Menu.gearList = gear;
      guide01142Menu.quantityList = quantity;
      guide01142Menu.itemIDs = itemIDs;
      guide01142Menu.isDispNumber = isDispNumber;
      IEnumerator e = guide01142Menu.indicatorPage1.LoadPrefabs();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Future<GameObject> prefabF = Res.Prefabs.unit004_4_3.bugu_detail_list.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      guide01142Menu.buguDetail = prefabF.Result;
      guide01142Menu.objectCnt = guide01142Menu.gearList.Length;
      guide01142Menu.currentIndex = index;
      guide01142Menu.chacheCount = 0;
      if (guide01142Menu.objectCnt > guide01142Menu.DISPLAY_OBJECT_MAX)
        guide01142Menu.objectCnt = guide01142Menu.DISPLAY_OBJECT_MAX;
      guide01142Menu.objectList = new List<GameObject>();
      guide01142Menu.detailObject = new GameObject[guide01142Menu.objectCnt];
      guide01142Menu.detailPrefabDict = new Dictionary<GameObject, Unit00443BuguList>();
      for (int index1 = 0; index1 < guide01142Menu.objectCnt; ++index1)
      {
        guide01142Menu.detailObject[index1] = Object.Instantiate<GameObject>(guide01142Menu.buguDetail);
        guide01142Menu.objectList.Add(guide01142Menu.detailObject[index1]);
        guide01142Menu.scroll.Add(guide01142Menu.detailObject[index1]);
        Unit00443BuguList component = guide01142Menu.detailObject[index1].GetComponent<Unit00443BuguList>();
        guide01142Menu.detailPrefabDict.Add(guide01142Menu.detailObject[index1], component);
      }
      yield return (object) null;
      guide01142Menu.scroll.ResolvePosition();
      ((Component) guide01142Menu.scroll.scrollView).transform.localPosition = new Vector3(-guide01142Menu.scroll.grid.cellWidth * (float) guide01142Menu.currentIndex, 0.0f, 0.0f);
      foreach (GameObject key in guide01142Menu.detailObject)
        guide01142Menu.detailPrefabDict[key].SetContainerPosition(guide01142Menu.scroll.scrollView.panel.GetViewSize().y);
      yield return (object) null;
      e = guide01142Menu.CreatePage(guide01142Menu.currentIndex);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      yield return (object) null;
      int start = guide01142Menu.currentIndex - 1 < 0 ? 0 : guide01142Menu.currentIndex - 1;
      int end = guide01142Menu.currentIndex + 1 >= guide01142Menu.gearList.Length ? guide01142Menu.gearList.Length - 1 : guide01142Menu.currentIndex + 1;
      for (int i = start; i <= end; ++i)
      {
        if (i != guide01142Menu.currentIndex)
        {
          e = guide01142Menu.CreatePage(i);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
      guide01142Menu.SetMenuInformation(guide01142Menu.currentIndex);
      guide01142Menu.CenterOnChild(guide01142Menu.currentIndex);
      yield return (object) null;
      for (int index2 = 0; index2 < guide01142Menu.objectList.Count; ++index2)
        guide01142Menu.objectList[index2].transform.localPosition = end < guide01142Menu.gearList.Length - 1 ? new Vector3(guide01142Menu.scroll.grid.cellWidth * (float) (end + index2 + 1), 0.0f, 0.0f) : new Vector3(guide01142Menu.scroll.grid.cellWidth * (float) (start - (index2 + 1)), 0.0f, 0.0f);
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      ItemIcon.IsPoolCache = true;
      guide01142Menu.firstInit = true;
    }
  }

  public virtual IEnumerator CreatePage(int gearIndex, bool isChangePage = false)
  {
    Guide01142Menu m = this;
    GameObject go = m.objectList[0];
    Unit00443BuguList d = m.detailPrefabDict[go];
    m.objectList.RemoveAt(0);
    Vector3 gridPos = ((Component) m.scroll.grid).transform.localPosition;
    go.transform.localPosition = new Vector3(m.scroll.grid.cellWidth * (float) gearIndex, 0.0f, 0.0f);
    yield return (object) null;
    IEnumerator e;
    if (m.quantityList != null && m.quantityList.Length >= gearIndex)
    {
      e = d.Init((Unit00443Menu) m, m.gearList[gearIndex], m.quantityList[gearIndex]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = d.Init((Unit00443Menu) m, m.gearList[gearIndex], m.isDispNumber);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    d.index = gearIndex;
    go.transform.localPosition = new Vector3(m.scroll.grid.cellWidth * (float) gearIndex, 0.0f, 0.0f);
    ((Component) m.scroll.grid).transform.localPosition = gridPos;
    if (PerformanceConfig.GetInstance().IsLowMemory && m.chacheCount > 0)
    {
      GC.Collect();
      Singleton<ResourceManager>.GetInstance().ClearCache();
      Resources.UnloadUnusedAssets();
      m.chacheCount = 0;
    }
    if (isChangePage)
      ++m.chacheCount;
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

  protected override void Update()
  {
    if (!this.firstInit)
      return;
    base.Update();
    this.UpdateCurrentItem();
  }

  private void UpdateCurrentItem()
  {
    int num1 = this.currentIndex;
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
          this.StartCoroutine(this.CreatePage(this.currentIndex + 1, true));
      }
      else if (this.currentIndex > 0)
        this.StartCoroutine(this.CreatePage(this.currentIndex - 1, true));
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
    this.currentBuguDatail.SetGearInformation();
    this.rightArrow.SetActive(true);
    this.leftArrow.SetActive(true);
    if (idx == 0)
      this.leftArrow.SetActive(false);
    if (idx == this.gearList.Length - 1)
      this.rightArrow.SetActive(false);
    this.StartCoroutine(this.RefreshInfo());
  }

  private IEnumerator RefreshInfo()
  {
    Guide01142Menu guide01142Menu = this;
    guide01142Menu.indicatorPage1.Init(guide01142Menu.gearList[guide01142Menu.currentIndex]);
    IEnumerator e = guide01142Menu.SetIncrementalParameter(guide01142Menu.gearList[guide01142Menu.currentIndex], guide01142Menu.DirAddStauts);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    guide01142Menu.indicatorPage2.Init(guide01142Menu.gearList[guide01142Menu.currentIndex]);
    ((Component) guide01142Menu.indicatorPage1).transform.localScale = Vector3.one;
    if (guide01142Menu.quantityList != null)
      guide01142Menu.SetQuanlity(guide01142Menu.quantityList[guide01142Menu.currentIndex]);
    else if (Object.op_Inequality((Object) guide01142Menu.NumPossession, (Object) null))
      guide01142Menu.NumPossession.SetActive(false);
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

  public void IbtnLeftArrow()
  {
    if (!this.isArrowBtn)
      return;
    this.isArrowBtn = false;
    this.StartCoroutine(this.IsArrowBtnOn());
    int num = this.currentIndex - 1;
    if (num < 0)
      return;
    this.CenterOnChild(num);
  }

  public void IbtnRightArrow()
  {
    if (!this.isArrowBtn)
      return;
    this.isArrowBtn = false;
    this.StartCoroutine(this.IsArrowBtnOn());
    int num = this.currentIndex + 1;
    if (num > this.gearList.Length - 1)
      return;
    this.CenterOnChild(num);
  }

  protected IEnumerator IsArrowBtnOn()
  {
    yield return (object) new WaitForSeconds(0.2f);
    this.isArrowBtn = true;
  }

  public override void EndScene() => this.SetChangeActiveComponent(false);

  private void SetChangeActiveComponent(bool isActive)
  {
    this.rightArrow.SetActive(isActive);
    this.leftArrow.SetActive(isActive);
  }

  public void SetQuanlity(int num)
  {
    if (!Object.op_Inequality((Object) this.NumPossession, (Object) null))
      return;
    this.txtUnit.SetTextLocalize(num);
    this.NumPossession.SetActive(true);
  }

  public override void IbtnZoom()
  {
    Unit00446Scene.changeScene(true, this.gearList[this.currentIndex]);
  }

  public override void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    if (this.itemIDs != null)
      Singleton<NGGameDataManager>.GetInstance().lastReferenceItemID = this.itemIDs[this.currentIndex];
    this.backScene();
  }
}
