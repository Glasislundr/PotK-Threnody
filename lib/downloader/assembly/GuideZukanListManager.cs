// Decompiled with JetBrains decompiler
// Type: GuideZukanListManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class GuideZukanListManager : BackButtonMenuBase
{
  private const int GETA = 10000;
  public NGxScroll2 scroll;
  public List<GameObject> sortText = new List<GameObject>();
  [SerializeField]
  public List<SpriteCash> spriteCashList = new List<SpriteCash>();
  public List<GuideUnitUnit> unitList = new List<GuideUnitUnit>();
  public List<WithNumberInfo> withNumberInfoList = new List<WithNumberInfo>();
  public List<WithNumber> withNumberList = new List<WithNumber>();
  public GameObject sortAndFilterPrefab;
  public GuideSortAndFilter sortAndFilter;
  public GameObject numberPrefab;
  protected bool isInitialize;
  protected float scrool_start_y;
  protected int iconWidth = UnitIcon.Width;
  protected int iconHeight = UnitIcon.Height;
  protected int iconColumnValue = UnitIcon.ColumnValue;
  protected int iconRowValue = UnitIcon.RowValue;
  protected int iconScreenValue = UnitIcon.ScreenValue;
  protected int iconMaxValue = UnitIcon.MaxValue;

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Debug.Log((object) "click default event IbtnBack");
    this.backScene();
  }

  public void StopCreateUnitIconImage()
  {
    foreach (WithNumber withNumber in this.withNumberList)
      this.StopCoroutine(withNumber.CreateIcon());
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void SortAction()
  {
    this.SortAndFilter();
    this.StartCoroutine(this.CreateSpriteCashList());
    this.InitializeInfo();
    this.SetIcon();
  }

  public virtual void IbtnSortAndFilter()
  {
    if (this.IsPushAndSet())
      return;
    this.sortAndFilter = Singleton<PopupManager>.GetInstance().open(this.sortAndFilterPrefab).GetComponent<GuideSortAndFilter>();
    this.sortAndFilter.Initialize(new Action(this.SortAction));
  }

  public virtual void Initialize()
  {
    this.isInitialize = false;
    this.scroll.Clear();
    this.spriteCashList.Clear();
    this.unitList.Clear();
  }

  protected void InitializeEnd()
  {
    this.scrool_start_y = ((Component) this.scroll.scrollView).transform.localPosition.y;
    this.isInitialize = true;
  }

  private void FixedUpdate() => this.ScrollUpdate();

  protected void ScrollUpdate()
  {
    if (!this.isInitialize || this.withNumberInfoList.Count <= this.iconScreenValue)
      return;
    int num1 = this.iconHeight * 2;
    float num2 = ((Component) this.scroll.scrollView).transform.localPosition.y - this.scrool_start_y;
    float num3 = (float) (Mathf.Max(0, this.withNumberInfoList.Count - this.iconScreenValue - 1) / this.iconColumnValue * this.iconHeight);
    float num4 = (float) (this.iconHeight * this.iconRowValue);
    if ((double) num2 < 0.0)
      num2 = 0.0f;
    if ((double) num2 > (double) num3)
      num2 = num3;
    bool flag;
    do
    {
      flag = false;
      int num5 = 0;
      foreach (GameObject gameObject in this.scroll)
      {
        GameObject unit = gameObject;
        float num6 = unit.transform.localPosition.y + num2;
        if ((double) num6 > (double) num1)
        {
          int? nullable = this.withNumberInfoList.FirstIndexOrNull<WithNumberInfo>((Func<WithNumberInfo, bool>) (v => Object.op_Inequality((Object) v.icon, (Object) null) && Object.op_Equality((Object) ((Component) v.icon).gameObject, (Object) unit)));
          int info_index = nullable.HasValue ? nullable.Value + this.iconMaxValue : (this.withNumberInfoList.Count + 4) / 5 * 5;
          if (nullable.HasValue && info_index < (this.withNumberInfoList.Count + 4) / 5 * 5)
          {
            unit.transform.localPosition = new Vector3(unit.transform.localPosition.x, unit.transform.localPosition.y - num4, 0.0f);
            if (info_index >= this.withNumberInfoList.Count)
            {
              unit.SetActive(false);
            }
            else
            {
              this.ResetUnitIcon(num5);
              this.CreateUnitIcon(info_index, num5);
            }
            flag = true;
          }
        }
        else if ((double) num6 < -((double) num4 - (double) num1))
        {
          int num7 = this.iconMaxValue;
          if (!unit.activeSelf)
          {
            unit.SetActive(true);
            num7 = 0;
          }
          int? nullable = this.withNumberInfoList.FirstIndexOrNull<WithNumberInfo>((Func<WithNumberInfo, bool>) (v => Object.op_Inequality((Object) v.icon, (Object) null) && Object.op_Equality((Object) ((Component) v.icon).gameObject, (Object) unit)));
          int info_index = nullable.HasValue ? nullable.Value - num7 : -1;
          if (nullable.HasValue && info_index >= 0)
          {
            unit.transform.localPosition = new Vector3(unit.transform.localPosition.x, unit.transform.localPosition.y + num4, 0.0f);
            this.ResetUnitIcon(num5);
            this.CreateUnitIcon(info_index, num5);
            flag = true;
          }
        }
        ++num5;
      }
    }
    while (flag);
  }

  protected virtual IEnumerator CreateZukanList(Future<GameObject> popupSortAndFilter)
  {
    GuideZukanListManager zukanListManager = this;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    IEnumerator e = zukanListManager.CreateSortAndFilter(popupSortAndFilter);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    zukanListManager.Initialize();
    zukanListManager.SortAndFilter();
    zukanListManager.StartCoroutine(zukanListManager.CreateSpriteCashList());
    zukanListManager.InitializeInfo();
    zukanListManager.SetIcon();
    zukanListManager.InitializeEnd();
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
  }

  public virtual int ZukanID(GuideUnitUnit unit) => unit.Unit.history_group_number;

  public virtual int ResourceID(GuideUnitUnit unit)
  {
    return unit.Unit.resource_reference_unit_id_UnitUnit;
  }

  public virtual Future<Sprite> ResourceSprite(GuideUnitUnit unit)
  {
    return unit.Unit.LoadSpriteThumbnail();
  }

  public virtual int HistroyNumber(GuideUnitUnit unit) => unit.Unit.history_group_number;

  public virtual DateTime? PublishedTime(GuideUnitUnit unit) => unit.Unit.published_at;

  public virtual IEnumerator CreateSpriteCashList()
  {
    int count = 0;
    List<GuideUnitUnit> list = this.unitList.Where<GuideUnitUnit>((Func<GuideUnitUnit, bool>) (x => x.History != new DateTime())).ToList<GuideUnitUnit>();
    foreach (GuideUnitUnit unit in list)
      this.AddSpriteCashList(this.ZukanID(unit), this.ResourceSprite(unit));
    foreach (GuideUnitUnit unit in list)
    {
      while (this.iconMaxValue < count)
      {
        if (!this.LoadCheck())
        {
          yield return (object) null;
          break;
        }
        yield return (object) null;
      }
      this.LoadSpriteCash(this.ZukanID(unit));
      ++count;
    }
  }

  public virtual void SortAndFilter()
  {
  }

  public virtual void InitializeInfo()
  {
  }

  public virtual void SetIcon()
  {
    this.scroll.Clear();
    this.CreateUnitIconBase();
    this.CreateUnitIconImage();
    this.ScrollReset(this.unitList.Count);
  }

  protected void CreateUnitIconBase()
  {
    this.withNumberList.Clear();
    for (int index = 0; index < Mathf.Min(this.iconMaxValue, this.withNumberInfoList.Count); ++index)
    {
      GameObject gameObject = Object.Instantiate<GameObject>(this.numberPrefab);
      WithNumber component = gameObject.GetComponent<WithNumber>();
      component.withNumberInfo = this.withNumberInfoList[index];
      component.withNumberInfo.icon = component;
      component.withNumberInfo.icon.withNumberInfo = this.withNumberInfoList[index];
      this.withNumberList.Add(component);
      this.scroll.Add(gameObject, this.iconWidth, this.iconHeight);
      gameObject.SetActive(false);
      gameObject.SetActive(true);
    }
    for (int index = 0; index < Mathf.Min(this.iconMaxValue, this.withNumberInfoList.Count); ++index)
      this.ResetUnitIcon(index);
    for (int index = 0; index < Mathf.Min(this.iconMaxValue, this.withNumberInfoList.Count); ++index)
      this.CreateUnitIcon(index, index);
  }

  public void CreateUnitIconImage()
  {
    foreach (WithNumber withNumber in this.withNumberList)
      this.StartCoroutine(withNumber.CreateIcon());
  }

  protected void CreateUnitIcon(int info_index, int unit_index)
  {
    WithNumber unitIcon = this.withNumberList[unit_index];
    this.withNumberInfoList.Where<WithNumberInfo>((Func<WithNumberInfo, bool>) (a => Object.op_Equality((Object) a.icon, (Object) unitIcon))).ForEach<WithNumberInfo>((Action<WithNumberInfo>) (b => b.icon = (WithNumber) null));
    this.withNumberInfoList[info_index].icon = unitIcon;
    unitIcon.withNumberInfo = this.withNumberInfoList[info_index];
    unitIcon.withNumberInfo.icon = unitIcon;
    unitIcon.withNumberInfo.icon.withNumberInfo = this.withNumberInfoList[info_index];
    unitIcon.pressEvent = new Action(this.IbtnFreeze);
    this.StartCoroutine(unitIcon.CreateIcon());
    ((Component) unitIcon).gameObject.SetActive(true);
  }

  protected void ResetUnitIcon(int index)
  {
    WithNumber unitIcon = this.withNumberList[index];
    unitIcon.Reset();
    ((Component) unitIcon).gameObject.SetActive(false);
    this.withNumberInfoList.Where<WithNumberInfo>((Func<WithNumberInfo, bool>) (a => Object.op_Equality((Object) a.icon, (Object) unitIcon))).ForEach<WithNumberInfo>((Action<WithNumberInfo>) (b => b.icon = (WithNumber) null));
  }

  public void ScrollReset(int maxCount)
  {
    this.scroll.CreateScrollPoint(UnitIcon.Height, maxCount);
    this.scroll.ResolvePosition();
    this.scrool_start_y = ((Component) this.scroll.scrollView).transform.localPosition.y;
  }

  public void AddSpriteCashList(int id, Future<Sprite> fSprite)
  {
    if (this.spriteCashList.Exists((Predicate<SpriteCash>) (x => x.id == id)))
      return;
    this.spriteCashList.Add(new SpriteCash()
    {
      id = id,
      fSprite = fSprite
    });
  }

  public bool LoadCheck()
  {
    foreach (SpriteCash spriteCash in this.spriteCashList)
    {
      if (spriteCash.isLoading)
        return true;
    }
    return false;
  }

  public void LoadSpriteCash(int id)
  {
    SpriteCash spriteCash = this.spriteCashList.Find((Predicate<SpriteCash>) (x => x.id == id));
    if (spriteCash == null)
      return;
    this.StartCoroutine(spriteCash.LoadSprite());
  }

  public IEnumerator CreateIconWithNumber(Future<GameObject> fPrefab)
  {
    IEnumerator e = fPrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.numberPrefab = fPrefab.Result;
  }

  public IEnumerator CreateSortAndFilter(Future<GameObject> fPrefab)
  {
    IEnumerator e = fPrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.sortAndFilterPrefab = fPrefab.Result;
  }

  public void IbtnUse()
  {
    foreach (WithNumber withNumber in this.withNumberList)
    {
      ((Behaviour) withNumber.button).enabled = true;
      withNumber.boxCollider.enabled = true;
    }
  }

  public void IbtnFreeze()
  {
    foreach (WithNumber withNumber in this.withNumberList)
    {
      ((Behaviour) withNumber.button).enabled = false;
      withNumber.boxCollider.enabled = false;
    }
  }

  protected string Order(GuideUnitUnit unit, bool reverse)
  {
    bool flag = unit.History == new DateTime();
    return (reverse == flag ? "1" : "0") + unit.History.ToString();
  }

  protected int RarityOrder(GuideUnitUnit unit)
  {
    return unit.Unit.history_group_number + unit.Unit.rarity.index * 10000;
  }

  protected int RarityOrderGear(GuideUnitUnit gear)
  {
    return gear.Gear.history_group_number + gear.Gear.rarity.index * 10000;
  }
}
