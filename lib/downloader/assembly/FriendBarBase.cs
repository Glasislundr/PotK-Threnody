// Decompiled with JetBrains decompiler
// Type: FriendBarBase
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
public class FriendBarBase : BackButtonMenuBase
{
  [SerializeField]
  protected NGxScroll2 scroll;
  protected List<FriendBarInfo> allFriendInfo = new List<FriendBarInfo>();
  protected List<FriendScrollBar> allFriendBar = new List<FriendScrollBar>();
  protected bool isUpdate;
  private int barWidth;
  private int barHeight;
  private int barColumnValue = 1;
  private int barRowValue;
  private int screenValue;
  private int MaxValue;
  private bool isInitialize;
  private DateTime now;
  [SerializeField]
  private UISprite[] sortSprites;
  private float scrool_start_y;

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
    this.MaxValue = barRowValue;
    this.now = nowTime;
    this.scroll.Clear();
  }

  public void InitializeEnd()
  {
    this.scrool_start_y = ((Component) this.scroll.scrollView).transform.localPosition.y;
    this.isInitialize = true;
  }

  public void CreateFriendInfo(PlayerFriend[] friends)
  {
    foreach (PlayerFriend friend in friends)
      this.allFriendInfo.Add(new FriendBarInfo()
      {
        friend = friend
      });
    this.allFriendInfo = this.allFriendInfo.SortBy().ToList<FriendBarInfo>();
    int count = this.allFriendInfo.Count;
    for (int index = 0; index < count; ++index)
      this.allFriendInfo[index].index = index;
  }

  protected void SetSortLabel()
  {
    for (int index = 0; index < this.sortSprites.Length; ++index)
      ((Component) this.sortSprites[index]).gameObject.SetActive(index == Persist.sortOrder.Data.Friend);
  }

  protected void SortFriendsData()
  {
    for (int index = 0; index < this.sortSprites.Length; ++index)
      ((Component) this.sortSprites[index]).gameObject.SetActive(index == Persist.sortOrder.Data.Friend);
    if (this.allFriendInfo.Count < 1)
      return;
    this.SortAndSetIcons();
    for (int index = 0; index < this.allFriendBar.Count; ++index)
      this.ResetScroll(index);
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    this.StartCoroutine(this.CreateFriendBarRange(Mathf.Min(this.MaxValue, this.allFriendInfo.Count)));
  }

  protected void SortAndSetIcons()
  {
    if (this.allFriendInfo.Count < 1)
      return;
    this.allFriendInfo = this.allFriendInfo.SortBy().ToList<FriendBarInfo>();
    this.scroll.Reset();
    for (int index = 0; index < Mathf.Min(this.MaxValue, this.allFriendBar.Count); ++index)
      this.scroll.AddColumn1(((Component) this.allFriendBar[index]).gameObject, this.barWidth, this.barHeight);
    this.scroll.CreateScrollPointHeight(this.barHeight, this.allFriendInfo.Count);
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
    if ((!this.isInitialize || this.allFriendInfo.Count <= this.screenValue) && !this.isUpdate)
      return;
    int num1 = this.barHeight * 2;
    float num2 = ((Component) this.scroll.scrollView).transform.localPosition.y - this.scrool_start_y;
    float num3 = (float) (Mathf.Max(0, this.allFriendInfo.Count - this.screenValue - 1) / this.barColumnValue * this.barHeight);
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
        int? nullable = this.allFriendInfo.FirstIndexOrNull<FriendBarInfo>((Func<FriendBarInfo, bool>) (v => Object.op_Inequality((Object) v.scroll, (Object) null) && Object.op_Equality((Object) ((Component) v.scroll).gameObject, (Object) unit)));
        if ((double) num6 > (double) num1)
        {
          int info_index = nullable.HasValue ? nullable.Value + this.MaxValue : this.allFriendInfo.Count;
          if (nullable.HasValue && info_index < this.allFriendInfo.Count)
          {
            unit.transform.localPosition = new Vector3(unit.transform.localPosition.x, unit.transform.localPosition.y - num4, 0.0f);
            if (info_index >= this.allFriendInfo.Count)
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

  public override void onBackButton()
  {
  }

  protected IEnumerator CreateScrollBase(GameObject prefab)
  {
    this.allFriendBar.Clear();
    for (int index = 0; index < Mathf.Min(this.MaxValue, this.allFriendInfo.Count); ++index)
      this.allFriendBar.Add(Object.Instantiate<GameObject>(prefab).GetComponent<FriendScrollBar>());
    this.scroll.Reset();
    for (int index = 0; index < Mathf.Min(this.MaxValue, this.allFriendBar.Count); ++index)
      this.scroll.AddColumn1(((Component) this.allFriendBar[index]).gameObject, this.barWidth, this.barHeight);
    this.scroll.CreateScrollPointHeight(this.barHeight, this.allFriendInfo.Count);
    this.scroll.ResolvePosition();
    for (int index = 0; index < Mathf.Min(this.MaxValue, this.allFriendInfo.Count); ++index)
      this.ResetScroll(index);
    for (int i = 0; i < Mathf.Min(this.MaxValue, this.allFriendInfo.Count); ++i)
    {
      IEnumerator e = this.CreateScroll(i, i);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  protected void ResetScroll(int index)
  {
    FriendScrollBar scroll = this.allFriendBar[index];
    ((Component) scroll).gameObject.SetActive(false);
    this.allFriendInfo.Where<FriendBarInfo>((Func<FriendBarInfo, bool>) (a => Object.op_Equality((Object) a.scroll, (Object) scroll))).ForEach<FriendBarInfo>((Action<FriendBarInfo>) (b => b.scroll = (FriendScrollBar) null));
  }

  protected virtual IEnumerator CreateScroll(int info_index, int bar_index)
  {
    FriendScrollBar scroll = this.allFriendBar[bar_index];
    FriendBarInfo friendBarInfo = this.allFriendInfo[info_index];
    friendBarInfo.scroll = scroll;
    scroll.Set(friendBarInfo.friend, this.now);
    ((Component) scroll).gameObject.SetActive(true);
    IEnumerator e = scroll.SetUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) scroll).gameObject.SetActive(true);
  }

  public enum FriendSort
  {
    Favorite,
    RegisterNew,
    Level,
    SORT_MAX,
  }
}
