// Decompiled with JetBrains decompiler
// Type: GuildMemberListBase
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
public class GuildMemberListBase : BackButtonMenuBase
{
  [SerializeField]
  protected NGxScroll2 scroll;
  protected List<MemberBarInfo> allMemberInfo = new List<MemberBarInfo>();
  protected List<Guild0282MemberScroll> allMemberBar = new List<Guild0282MemberScroll>();
  protected bool isUpdate;
  protected int barWidth;
  protected int barHeight;
  private int barColumnValue = 1;
  private int barRowValue;
  private int screenValue;
  protected int MaxValue;
  private bool isInitialize;
  private DateTime now;
  [SerializeField]
  private GameObject SortRoot;
  [SerializeField]
  private UISprite SortTextSprite;
  private float scrool_start_y;
  private GuildMemberSort sortInfo;
  private GuildMemberSort.SORT_TYPES sortType;
  private SortAndFilter.SORT_TYPE_ORDER_BUY orderSortType;
  private int sortIndex;

  public Persist<Persist.GuildMemberSortInfo> GetPersist() => Persist.guildMemberListSort;

  public GuildMemberSort.SORT_TYPES SortType
  {
    get => this.sortType;
    set => this.sortType = value;
  }

  public SortAndFilter.SORT_TYPE_ORDER_BUY OrderSortType
  {
    get => this.orderSortType;
    set => this.orderSortType = value;
  }

  public bool IsOpenSortPopup
  {
    get
    {
      return !Object.op_Equality((Object) this.sortInfo, (Object) null) && ((Component) this.sortInfo).gameObject.GetComponent<NGTweenParts>().isActive;
    }
  }

  public void Initialize(
    DateTime nowTime,
    int barWidth,
    int barHeight,
    int barRowValue,
    int screenValue)
  {
    this.isInitialize = false;
    this.barWidth = barWidth;
    this.barHeight = barHeight;
    this.barRowValue = barRowValue;
    screenValue = screenValue;
    this.MaxValue = barRowValue;
    this.now = nowTime;
    this.scroll.Clear();
  }

  public void InitializeEnd()
  {
    this.scrool_start_y = ((Component) this.scroll.scrollView).transform.localPosition.y;
    this.isInitialize = true;
  }

  public void CreateMemberInfo(GuildMembership[] members)
  {
    foreach (GuildMembership member in members)
      this.allMemberInfo.Add(new MemberBarInfo()
      {
        member = member
      });
    int count = this.allMemberInfo.Count;
    for (int index = 0; index < count; ++index)
      this.allMemberInfo[index].index = index;
  }

  public void Sort(GuildMemberSort.SORT_TYPES sort, SortAndFilter.SORT_TYPE_ORDER_BUY order)
  {
    if (this.allMemberInfo.Count < 1)
      return;
    this.SortAndSetIcons(sort, order);
    for (int index = 0; index < this.allMemberBar.Count; ++index)
      this.ResetScroll(index);
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    this.StartCoroutine(this.CreateFriendBarRange(Mathf.Min(this.MaxValue, this.allMemberInfo.Count)));
    this.SortTextSprite = this.sortInfo.SortSpriteLabel(sort, this.SortTextSprite);
  }

  public void IbtnSort()
  {
    ((Component) this.sortInfo).gameObject.SetActive(true);
    ((Component) this.sortInfo).gameObject.GetComponent<NGTweenParts>().isActive = true;
  }

  protected IEnumerator CreateSortPopup()
  {
    GuildMemberListBase list = this;
    Future<GameObject> sortPopupPrefabF = Res.Prefabs.popup.popup_Guild_Member_Sort__anim_popup01.Load<GameObject>();
    IEnumerator e = sortPopupPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = sortPopupPrefabF.Result;
    list.sortInfo = result.CloneAndGetComponent<GuildMemberSort>(list.SortRoot);
    ((Component) list.sortInfo).gameObject.GetComponent<NGTweenParts>().isActive = false;
    ((Component) list.sortInfo).GetComponent<GuildMemberSort>().Initialize(list);
    list.Sort(list.sortType, list.orderSortType);
  }

  protected void SortAndSetIcons(
    GuildMemberSort.SORT_TYPES sort,
    SortAndFilter.SORT_TYPE_ORDER_BUY order)
  {
    this.allMemberInfo = this.allMemberInfo.SortBy(sort, order).ToList<MemberBarInfo>();
    this.scroll.Reset();
    for (int index = 0; index < Mathf.Min(this.MaxValue, this.allMemberBar.Count); ++index)
      this.scroll.AddColumn1(((Component) this.allMemberBar[index]).gameObject, this.barWidth, this.barHeight);
    this.scroll.CreateScrollPointHeight(this.barHeight, this.allMemberInfo.Count);
    this.scroll.ResolvePosition();
  }

  protected IEnumerator CreateFriendBarRange(int value)
  {
    for (int i = 0; i < value; ++i)
    {
      IEnumerator e = this.CreateScroll(i, i);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
    this.scroll.ResolvePosition();
  }

  protected void ScrollUpdate()
  {
    if ((!this.isInitialize || this.allMemberInfo.Count <= this.screenValue) && !this.isUpdate)
      return;
    int num1 = this.barHeight * 2;
    float num2 = ((Component) this.scroll.scrollView).transform.localPosition.y - this.scrool_start_y;
    float num3 = (float) (Mathf.Max(0, this.allMemberInfo.Count - this.screenValue - 1) / this.barColumnValue * this.barHeight);
    float num4 = (float) (this.barHeight * this.barRowValue);
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
        int? nullable = this.allMemberInfo.FirstIndexOrNull<MemberBarInfo>((Func<MemberBarInfo, bool>) (v => Object.op_Inequality((Object) v.scroll, (Object) null) && Object.op_Equality((Object) ((Component) v.scroll).gameObject, (Object) unit)));
        if ((double) num6 > (double) num1)
        {
          int info_index = nullable.HasValue ? nullable.Value + this.MaxValue : this.allMemberInfo.Count;
          if (nullable.HasValue && info_index < this.allMemberInfo.Count)
          {
            unit.transform.localPosition = new Vector3(unit.transform.localPosition.x, unit.transform.localPosition.y - num4, 0.0f);
            if (info_index >= this.allMemberInfo.Count)
            {
              unit.SetActive(false);
            }
            else
            {
              this.ResetScroll(num5);
              this.StartCoroutine(this.CreateScroll(info_index, num5));
            }
            flag = true;
          }
        }
        else if ((double) num6 < -((double) num4 - (double) num1))
        {
          int num7 = this.MaxValue;
          if (!unit.activeSelf)
          {
            unit.SetActive(true);
            num7 = 0;
          }
          int info_index = nullable.HasValue ? nullable.Value - num7 : -1;
          if (nullable.HasValue && info_index >= 0)
          {
            unit.transform.localPosition = new Vector3(unit.transform.localPosition.x, unit.transform.localPosition.y + num4, 0.0f);
            this.ResetScroll(num5);
            this.StartCoroutine(this.CreateScroll(info_index, num5));
            flag = true;
          }
        }
        else if (this.isUpdate)
          this.StartCoroutine(this.CreateScroll(nullable.Value, num5));
        ++num5;
      }
    }
    while (flag);
    if (!this.isUpdate)
      return;
    this.isUpdate = false;
  }

  protected IEnumerator CreateScrollBase(GameObject prefab)
  {
    this.allMemberBar.Clear();
    for (int index = 0; index < Mathf.Min(this.MaxValue, this.allMemberInfo.Count); ++index)
      this.allMemberBar.Add(Object.Instantiate<GameObject>(prefab).GetComponent<Guild0282MemberScroll>());
    this.scroll.Reset();
    for (int index = 0; index < Mathf.Min(this.MaxValue, this.allMemberBar.Count); ++index)
      this.scroll.AddColumn1(((Component) this.allMemberBar[index]).gameObject, this.barWidth, this.barHeight);
    this.scroll.CreateScrollPointHeight(this.barHeight, this.allMemberInfo.Count);
    this.scroll.ResolvePosition();
    for (int index = 0; index < Mathf.Min(this.MaxValue, this.allMemberInfo.Count); ++index)
      this.ResetScroll(index);
    for (int i = 0; i < Mathf.Min(this.MaxValue, this.allMemberInfo.Count); ++i)
    {
      IEnumerator e = this.CreateScroll(i, i);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  protected void ResetScroll(int index)
  {
    Guild0282MemberScroll scroll = this.allMemberBar[index];
    ((Component) scroll).gameObject.SetActive(false);
    this.allMemberInfo.Where<MemberBarInfo>((Func<MemberBarInfo, bool>) (a => Object.op_Equality((Object) a.scroll, (Object) scroll))).ForEach<MemberBarInfo>((Action<MemberBarInfo>) (b => b.scroll = (Guild0282MemberScroll) null));
  }

  protected virtual IEnumerator CreateScroll(int info_index, int bar_index)
  {
    yield break;
  }

  public override void onBackButton()
  {
    if (Object.op_Inequality((Object) this.sortInfo, (Object) null))
      return;
    int num = ((Component) this.sortInfo).gameObject.GetComponent<NGTweenParts>().isActive ? 1 : 0;
  }
}
