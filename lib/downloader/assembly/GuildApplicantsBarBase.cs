// Decompiled with JetBrains decompiler
// Type: GuildApplicantsBarBase
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
public class GuildApplicantsBarBase : BackButtonMenuBase
{
  [SerializeField]
  protected NGxScroll2 scroll;
  protected List<ApplicantBarInfo> allApplicantInfo = new List<ApplicantBarInfo>();
  protected List<Guild0285Scroll> allApplicantBar = new List<Guild0285Scroll>();
  protected bool isUpdate;
  private int barWidth;
  private int barHeight;
  private int barColumnValue = 1;
  private int barRowValue;
  private int screenValue;
  private int MaxValue;
  private bool isInitialize;
  protected DateTime now;
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

  public void CreateApplicantInfo(GuildApplicant[] applicants)
  {
    for (int index = 0; index < applicants.Length; ++index)
      this.allApplicantInfo.Add(new ApplicantBarInfo()
      {
        applicant = applicants[index]
      });
    int count = this.allApplicantInfo.Count;
    for (int index = 0; index < count; ++index)
      this.allApplicantInfo[index].index = index;
  }

  protected void ScrollUpdate()
  {
    if ((!this.isInitialize || this.allApplicantInfo.Count <= this.screenValue) && !this.isUpdate)
      return;
    int num1 = this.barHeight * 2;
    float num2 = ((Component) this.scroll.scrollView).transform.localPosition.y - this.scrool_start_y;
    float num3 = (float) (Mathf.Max(0, this.allApplicantInfo.Count - this.screenValue - 1) / this.barColumnValue * this.barHeight);
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
        int? nullable = this.allApplicantInfo.FirstIndexOrNull<ApplicantBarInfo>((Func<ApplicantBarInfo, bool>) (v => Object.op_Inequality((Object) v.scroll, (Object) null) && Object.op_Equality((Object) ((Component) v.scroll).gameObject, (Object) unit)));
        if ((double) num6 > (double) num1)
        {
          int info_index = nullable.HasValue ? nullable.Value + this.MaxValue : this.allApplicantInfo.Count;
          if (nullable.HasValue && info_index < this.allApplicantInfo.Count)
          {
            unit.transform.localPosition = new Vector3(unit.transform.localPosition.x, unit.transform.localPosition.y - num4, 0.0f);
            if (info_index >= this.allApplicantInfo.Count)
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
    this.allApplicantBar.Clear();
    for (int index = 0; index < Mathf.Min(this.MaxValue, this.allApplicantInfo.Count); ++index)
      this.allApplicantBar.Add(Object.Instantiate<GameObject>(prefab).GetComponent<Guild0285Scroll>());
    this.scroll.Reset();
    for (int index = 0; index < Mathf.Min(this.MaxValue, this.allApplicantBar.Count); ++index)
      this.scroll.AddColumn1(((Component) this.allApplicantBar[index]).gameObject, this.barWidth, this.barHeight);
    this.scroll.CreateScrollPointHeight(this.barHeight, this.allApplicantInfo.Count);
    this.scroll.ResolvePosition();
    for (int index = 0; index < Mathf.Min(this.MaxValue, this.allApplicantInfo.Count); ++index)
      this.ResetScroll(index);
    for (int i = 0; i < Mathf.Min(this.MaxValue, this.allApplicantInfo.Count); ++i)
    {
      IEnumerator e = this.CreateScroll(i, i);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  protected void ResetScroll(int index)
  {
    Guild0285Scroll scroll = this.allApplicantBar[index];
    ((Component) scroll).gameObject.SetActive(false);
    this.allApplicantInfo.Where<ApplicantBarInfo>((Func<ApplicantBarInfo, bool>) (a => Object.op_Equality((Object) a.scroll, (Object) scroll))).ForEach<ApplicantBarInfo>((Action<ApplicantBarInfo>) (b => b.scroll = (Guild0285Scroll) null));
  }

  protected virtual IEnumerator CreateScroll(int info_index, int bar_index)
  {
    yield break;
  }

  public override void onBackButton()
  {
  }
}
