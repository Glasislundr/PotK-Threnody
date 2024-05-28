// Decompiled with JetBrains decompiler
// Type: Guild0286Scroll
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
public class Guild0286Scroll : BackButtonMenuBase
{
  public NGxScroll2 scroll;
  [SerializeField]
  protected GuildMemberGift[] memberGifts;
  [SerializeField]
  private List<GuildGiftScrollParts> allScroll = new List<GuildGiftScrollParts>();
  [SerializeField]
  private List<GuildGiftInfo> allGuildGiftInfos = new List<GuildGiftInfo>();
  private bool isInitialize;
  private float scrool_start_y;
  private ScrollAreaSetting setting;
  private Action initEndAction;
  private Action<GuildMemberGift> buttonAction;
  private GameObject prefab;

  public ScrollAreaSetting Setting
  {
    set => this.setting = value;
  }

  public override void onBackButton()
  {
  }

  public void SetInitEndAction(Action initEndAction) => this.initEndAction = initEndAction;

  public void SetPrefab(GameObject prefab) => this.prefab = prefab;

  public IEnumerator Init(GuildMemberGift[] gifts, Action<GuildMemberGift> action)
  {
    this.buttonAction = action;
    this.memberGifts = ((IEnumerable<GuildMemberGift>) gifts).OrderByDescending<GuildMemberGift, DateTime>((Func<GuildMemberGift, DateTime>) (x => x.send_at)).ToArray<GuildMemberGift>();
    this.Initialize();
    this.InitializeGuildGiftInfo(gifts);
    if (this.allGuildGiftInfos.Count > 0)
    {
      IEnumerator e = this.CreateScrollBase(this.prefab);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.scroll.ResolvePosition();
    this.InitializeEnd();
    if (this.initEndAction != null)
      this.initEndAction();
  }

  public IEnumerator UpdateList(GuildMemberGift[] gifts)
  {
    Guild0286Scroll guild0286Scroll = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    IEnumerator e = guild0286Scroll.Init(gifts, guild0286Scroll.buttonAction);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    guild0286Scroll.IsPush = false;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public void Initialize()
  {
    this.isInitialize = false;
    this.scroll.Clear();
  }

  private void InitializeEnd()
  {
    this.scrool_start_y = ((Component) this.scroll.scrollView).transform.localPosition.y;
    this.isInitialize = true;
  }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  protected void ScrollUpdate()
  {
    if (!this.isInitialize || this.allGuildGiftInfos.Count <= this.setting.iconScreenValue)
      return;
    int num1 = this.setting.iconHeight * 2;
    float num2 = ((Component) this.scroll.scrollView).transform.localPosition.y - this.scrool_start_y;
    float num3 = (float) (Mathf.Max(0, this.allGuildGiftInfos.Count - this.setting.iconScreenValue - 1) / this.setting.iconColumnValue * this.setting.iconHeight);
    float num4 = (float) (this.setting.iconHeight * this.setting.iconRowValue);
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
          int? nullable = this.allGuildGiftInfos.FirstIndexOrNull<GuildGiftInfo>((Func<GuildGiftInfo, bool>) (v => Object.op_Inequality((Object) v.scroll, (Object) null) && Object.op_Equality((Object) ((Component) v.scroll).gameObject, (Object) unit)));
          int info_index = nullable.HasValue ? nullable.Value + this.setting.iconMaxValue : this.allGuildGiftInfos.Count;
          if (nullable.HasValue && info_index < this.allGuildGiftInfos.Count)
          {
            unit.transform.localPosition = new Vector3(unit.transform.localPosition.x, unit.transform.localPosition.y - num4, 0.0f);
            if (info_index >= this.allGuildGiftInfos.Count)
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
          int num7 = this.setting.iconMaxValue;
          if (!unit.activeSelf)
          {
            unit.SetActive(true);
            num7 = 0;
          }
          int? nullable = this.allGuildGiftInfos.FirstIndexOrNull<GuildGiftInfo>((Func<GuildGiftInfo, bool>) (v => Object.op_Inequality((Object) v.scroll, (Object) null) && Object.op_Equality((Object) ((Component) v.scroll).gameObject, (Object) unit)));
          int info_index = nullable.HasValue ? nullable.Value - num7 : -1;
          if (nullable.HasValue && info_index >= 0)
          {
            unit.transform.localPosition = new Vector3(unit.transform.localPosition.x, unit.transform.localPosition.y + num4, 0.0f);
            this.ResetScroll(num5);
            this.StartCoroutine(this.CreateScroll(info_index, num5));
            flag = true;
          }
        }
        ++num5;
      }
    }
    while (flag);
  }

  private void InitializeGuildGiftInfo(GuildMemberGift[] gifts)
  {
    this.allGuildGiftInfos.Clear();
    foreach (GuildMemberGift gift in gifts)
      this.allGuildGiftInfos.Add(new GuildGiftInfo()
      {
        gift = gift
      });
  }

  private void ResetScroll(int index)
  {
    GuildGiftScrollParts scroll = this.allScroll[index];
    ((Component) scroll).gameObject.SetActive(false);
    this.allGuildGiftInfos.Where<GuildGiftInfo>((Func<GuildGiftInfo, bool>) (a => Object.op_Equality((Object) a.scroll, (Object) scroll))).ForEach<GuildGiftInfo>((Action<GuildGiftInfo>) (b => b.scroll = (GuildGiftScrollParts) null));
  }

  private IEnumerator CreateScrollBase(GameObject prefab)
  {
    this.allScroll.Clear();
    for (int index = 0; index < Mathf.Min(this.setting.iconMaxValue, this.allGuildGiftInfos.Count); ++index)
      this.allScroll.Add(Object.Instantiate<GameObject>(prefab).GetComponent<GuildGiftScrollParts>());
    this.scroll.Reset();
    for (int index = 0; index < Mathf.Min(this.setting.iconMaxValue, this.allScroll.Count); ++index)
      this.scroll.AddColumn1(((Component) this.allScroll[index]).gameObject, this.setting.iconWidth, this.setting.iconHeight);
    this.scroll.CreateScrollPointHeight(this.setting.iconHeight, this.allGuildGiftInfos.Count);
    this.scroll.ResolvePosition();
    for (int index = 0; index < Mathf.Min(this.setting.iconMaxValue, this.allGuildGiftInfos.Count); ++index)
      this.ResetScroll(index);
    for (int i = 0; i < Mathf.Min(this.setting.iconMaxValue, this.allGuildGiftInfos.Count); ++i)
    {
      IEnumerator e = this.CreateScroll(i, i);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private IEnumerator CreateScroll(int info_index, int unit_index)
  {
    GuildGiftScrollParts scroll = this.allScroll[unit_index];
    this.allGuildGiftInfos.Where<GuildGiftInfo>((Func<GuildGiftInfo, bool>) (a => Object.op_Equality((Object) a.scroll, (Object) scroll))).ForEach<GuildGiftInfo>((Action<GuildGiftInfo>) (b => b.scroll = (GuildGiftScrollParts) null));
    this.allGuildGiftInfos[info_index].scroll = scroll;
    scroll.Initialize(this.allGuildGiftInfos[info_index].gift);
    IEnumerator e = scroll.Initialize(this.allGuildGiftInfos[info_index].gift);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) scroll).gameObject.SetActive(true);
    EventDelegate.Set(scroll.GetButton().onClick, (EventDelegate.Callback) (() => this.buttonAction(this.allGuildGiftInfos[info_index].gift)));
    yield return (object) null;
  }
}
