// Decompiled with JetBrains decompiler
// Type: GuildBankDonateListBase
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
public class GuildBankDonateListBase : BackButtonMenuBase
{
  [SerializeField]
  protected NGxScroll2 scroll;
  protected List<DonateBarInfo> allDonateInfo = new List<DonateBarInfo>();
  protected List<GuildBankDonateListParts> allDonateBar = new List<GuildBankDonateListParts>();
  protected bool isUpdate;
  private int barWidth;
  private int barHeight;
  private int barColumnValue = 1;
  private int barRowValue;
  private int screenValue;
  private int MaxValue;
  private bool isInitialize;
  private DateTime now;
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
    this.screenValue = screenValue;
    this.MaxValue = barRowValue;
    this.now = nowTime;
    this.scroll.Clear();
  }

  public void InitializeEnd()
  {
    this.scrool_start_y = ((Component) this.scroll.scrollView).transform.localPosition.y;
    this.isInitialize = true;
  }

  public void CreateDonateInfo(GuildMoneyRate[] donateInfoList)
  {
    foreach (GuildMoneyRate donateInfo in donateInfoList)
      this.allDonateInfo.Add(new DonateBarInfo()
      {
        moneyRate = donateInfo
      });
    int count = this.allDonateInfo.Count;
    for (int index = 0; index < count; ++index)
      this.allDonateInfo[index].index = index;
  }

  protected void ScrollUpdate()
  {
    if ((!this.isInitialize || this.allDonateInfo.Count <= this.screenValue) && !this.isUpdate)
      return;
    int num1 = this.barHeight * 2;
    float num2 = ((Component) this.scroll.scrollView).transform.localPosition.y - this.scrool_start_y;
    float num3 = (float) (Mathf.Max(0, this.allDonateInfo.Count - this.screenValue - 1) / this.barColumnValue * this.barHeight);
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
        int? nullable = this.allDonateInfo.FirstIndexOrNull<DonateBarInfo>((Func<DonateBarInfo, bool>) (v => Object.op_Inequality((Object) v.scrollParts, (Object) null) && Object.op_Equality((Object) ((Component) v.scrollParts).gameObject, (Object) unit)));
        if ((double) num6 > (double) num1)
        {
          int info_index = nullable.HasValue ? nullable.Value + this.MaxValue : this.allDonateInfo.Count;
          if (nullable.HasValue && info_index < this.allDonateInfo.Count)
          {
            unit.transform.localPosition = new Vector3(unit.transform.localPosition.x, unit.transform.localPosition.y - num4, 0.0f);
            if (info_index >= this.allDonateInfo.Count)
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
    this.allDonateBar.Clear();
    for (int index = 0; index < Mathf.Min(this.MaxValue, this.allDonateInfo.Count); ++index)
      this.allDonateBar.Add(Object.Instantiate<GameObject>(prefab).GetComponent<GuildBankDonateListParts>());
    this.scroll.Reset();
    for (int index = 0; index < Mathf.Min(this.MaxValue, this.allDonateBar.Count); ++index)
      this.scroll.AddColumn1(((Component) this.allDonateBar[index]).gameObject, this.barWidth, this.barHeight);
    this.scroll.CreateScrollPointHeight(this.barHeight, this.allDonateInfo.Count);
    this.scroll.ResolvePosition();
    for (int index = 0; index < Mathf.Min(this.MaxValue, this.allDonateInfo.Count); ++index)
      this.ResetScroll(index);
    for (int i = 0; i < Mathf.Min(this.MaxValue, this.allDonateInfo.Count); ++i)
    {
      IEnumerator e = this.CreateScroll(i, i);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  protected void ResetScroll(int index)
  {
    GuildBankDonateListParts scrollParts = this.allDonateBar[index];
    ((Component) scrollParts).gameObject.SetActive(false);
    this.allDonateInfo.Where<DonateBarInfo>((Func<DonateBarInfo, bool>) (a => Object.op_Equality((Object) a.scrollParts, (Object) scrollParts))).ForEach<DonateBarInfo>((Action<DonateBarInfo>) (b => b.scrollParts = (GuildBankDonateListParts) null));
  }

  protected virtual IEnumerator CreateScroll(int info_index, int bar_index)
  {
    yield break;
  }

  public override void onBackButton()
  {
  }
}
