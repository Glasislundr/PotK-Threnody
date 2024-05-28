// Decompiled with JetBrains decompiler
// Type: GuildBattleRecordListBase
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
public class GuildBattleRecordListBase : BackButtonMonoBehaiviour
{
  [SerializeField]
  protected NGxScroll2 scroll;
  protected List<GuildBattleRecordBarInfo> allRecordInfo = new List<GuildBattleRecordBarInfo>();
  protected List<GuildBattleRecordScroll> allRecordBar = new List<GuildBattleRecordScroll>();
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

  public void CreateRecordInfo(GvgHistory[] records)
  {
    foreach (GvgHistory record in records)
      this.allRecordInfo.Add(new GuildBattleRecordBarInfo()
      {
        record = record
      });
    int count = this.allRecordInfo.Count;
    for (int index = 0; index < count; ++index)
      this.allRecordInfo[index].index = index;
  }

  protected void ScrollUpdate()
  {
    if ((!this.isInitialize || this.allRecordInfo.Count <= this.screenValue) && !this.isUpdate)
      return;
    int num1 = this.barHeight * 2;
    float num2 = ((Component) this.scroll.scrollView).transform.localPosition.y - this.scrool_start_y;
    float num3 = (float) (Mathf.Max(0, this.allRecordInfo.Count - this.screenValue - 1) / this.barColumnValue * this.barHeight);
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
        int? nullable = this.allRecordInfo.FirstIndexOrNull<GuildBattleRecordBarInfo>((Func<GuildBattleRecordBarInfo, bool>) (v => Object.op_Inequality((Object) v.scrollParts, (Object) null) && Object.op_Equality((Object) ((Component) v.scrollParts).gameObject, (Object) unit)));
        if ((double) num6 > (double) num1)
        {
          int info_index = nullable.HasValue ? nullable.Value + this.MaxValue : this.allRecordInfo.Count;
          if (nullable.HasValue && info_index < this.allRecordInfo.Count)
          {
            unit.transform.localPosition = new Vector3(unit.transform.localPosition.x, unit.transform.localPosition.y - num4, 0.0f);
            if (info_index >= this.allRecordInfo.Count)
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
    this.allRecordBar.Clear();
    for (int index = 0; index < Mathf.Min(this.MaxValue, this.allRecordInfo.Count); ++index)
      this.allRecordBar.Add(Object.Instantiate<GameObject>(prefab).GetComponent<GuildBattleRecordScroll>());
    this.scroll.Reset();
    for (int index = 0; index < Mathf.Min(this.MaxValue, this.allRecordBar.Count); ++index)
      this.scroll.AddColumn1(((Component) this.allRecordBar[index]).gameObject, this.barWidth, this.barHeight);
    this.scroll.CreateScrollPointHeight(this.barHeight, this.allRecordInfo.Count);
    this.scroll.ResolvePosition();
    for (int index = 0; index < Mathf.Min(this.MaxValue, this.allRecordInfo.Count); ++index)
      this.ResetScroll(index);
    for (int i = 0; i < Mathf.Min(this.MaxValue, this.allRecordInfo.Count); ++i)
    {
      IEnumerator e = this.CreateScroll(i, i);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  protected void ResetScroll(int index)
  {
    GuildBattleRecordScroll scrollParts = this.allRecordBar[index];
    ((Component) scrollParts).gameObject.SetActive(false);
    this.allRecordInfo.Where<GuildBattleRecordBarInfo>((Func<GuildBattleRecordBarInfo, bool>) (a => Object.op_Equality((Object) a.scrollParts, (Object) scrollParts))).ForEach<GuildBattleRecordBarInfo>((Action<GuildBattleRecordBarInfo>) (b => b.scrollParts = (GuildBattleRecordScroll) null));
  }

  protected virtual IEnumerator CreateScroll(int info_index, int bar_index)
  {
    yield break;
  }

  public override void onBackButton()
  {
  }
}
