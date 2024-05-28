// Decompiled with JetBrains decompiler
// Type: GuildGuestListBase
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
public class GuildGuestListBase : BackButtonMenuBase
{
  [SerializeField]
  protected NGxScroll2 scroll;
  protected List<GuestBarInfo> allGuestInfo = new List<GuestBarInfo>();
  protected List<GuildGuestSelectScroll> allGuestBar = new List<GuildGuestSelectScroll>();
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

  public bool IsInitialize => this.isInitialize;

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

  public void CreateFriendInfo(GvgCandidate[] friends)
  {
    foreach (GvgCandidate friend in friends)
      this.allGuestInfo.Add(new GuestBarInfo()
      {
        friend = friend
      });
    this.allGuestInfo.Add(new GuestBarInfo()
    {
      friend = (GvgCandidate) null
    });
    int count = this.allGuestInfo.Count;
    for (int index = 0; index < count; ++index)
      this.allGuestInfo[index].index = index;
  }

  public void CreateFriendInfo(GuildRaidPlayerHelpers[] friends)
  {
    foreach (GuildRaidPlayerHelpers friend in friends)
      this.allGuestInfo.Add(new GuestBarInfo()
      {
        friend = this.convertToGvgCandidate(friend)
      });
    this.allGuestInfo.Add(new GuestBarInfo()
    {
      friend = (GvgCandidate) null
    });
    int count = this.allGuestInfo.Count;
    for (int index = 0; index < count; ++index)
      this.allGuestInfo[index].index = index;
  }

  private GvgCandidate convertToGvgCandidate(GuildRaidPlayerHelpers helper)
  {
    GvgCandidate gvgCandidate = new GvgCandidate()
    {
      player_id = helper.target_player_id,
      player_name = helper.target_player_name,
      player_emblem_id = helper.current_emblem_id,
      player_unit = PlayerUnit.create_by_unitunit(MasterData.UnitUnit[helper.leader_unit_id])
    };
    gvgCandidate.player_unit.player_id = helper.target_player_id;
    gvgCandidate.player_unit.level = helper.leader_unit_level;
    gvgCandidate.player_unit.id = helper.leader_player_unit_id;
    gvgCandidate.player_unit.job_id = helper.leader_unit_job_id;
    if (!helper.leader_skill_id.HasValue)
      gvgCandidate.player_unit.leader_skills = new PlayerUnitLeader_skills[0];
    else
      gvgCandidate.player_unit.leader_skills = new PlayerUnitLeader_skills[1]
      {
        new PlayerUnitLeader_skills()
        {
          skill_id = helper.leader_skill_id.Value
        }
      };
    gvgCandidate.using_player_id = string.Empty;
    return gvgCandidate;
  }

  protected void ScrollUpdate()
  {
    if ((!this.isInitialize || this.allGuestInfo.Count <= this.screenValue) && !this.isUpdate)
      return;
    int num1 = this.barHeight * 2;
    float num2 = ((Component) this.scroll.scrollView).transform.localPosition.y - this.scrool_start_y;
    float num3 = (float) (Mathf.Max(0, this.allGuestInfo.Count - this.screenValue - 1) / this.barColumnValue * this.barHeight);
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
        int? nullable = this.allGuestInfo.FirstIndexOrNull<GuestBarInfo>((Func<GuestBarInfo, bool>) (v => Object.op_Inequality((Object) v.scrollParts, (Object) null) && Object.op_Equality((Object) ((Component) v.scrollParts).gameObject, (Object) unit)));
        if ((double) num6 > (double) num1)
        {
          int info_index = nullable.HasValue ? nullable.Value + this.MaxValue : this.allGuestInfo.Count;
          if (nullable.HasValue && info_index < this.allGuestInfo.Count)
          {
            unit.transform.localPosition = new Vector3(unit.transform.localPosition.x, unit.transform.localPosition.y - num4, 0.0f);
            if (info_index >= this.allGuestInfo.Count)
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
    this.allGuestBar.Clear();
    for (int index = 0; index < Mathf.Min(this.MaxValue, this.allGuestInfo.Count); ++index)
      this.allGuestBar.Add(Object.Instantiate<GameObject>(prefab).GetComponent<GuildGuestSelectScroll>());
    this.scroll.Reset();
    for (int index = 0; index < Mathf.Min(this.MaxValue, this.allGuestBar.Count); ++index)
      this.scroll.AddColumn1(((Component) this.allGuestBar[index]).gameObject, this.barWidth, this.barHeight);
    this.scroll.CreateScrollPointHeight(this.barHeight, this.allGuestInfo.Count);
    this.scroll.ResolvePosition();
    for (int index = 0; index < Mathf.Min(this.MaxValue, this.allGuestInfo.Count); ++index)
      this.ResetScroll(index);
    for (int i = 0; i < Mathf.Min(this.MaxValue, this.allGuestInfo.Count); ++i)
    {
      IEnumerator e = this.CreateScroll(i, i);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  protected void ResetScroll(int index)
  {
    GuildGuestSelectScroll scrollParts = this.allGuestBar[index];
    ((Component) scrollParts).gameObject.SetActive(false);
    this.allGuestInfo.Where<GuestBarInfo>((Func<GuestBarInfo, bool>) (a => Object.op_Equality((Object) a.scrollParts, (Object) scrollParts))).ForEach<GuestBarInfo>((Action<GuestBarInfo>) (b => b.scrollParts = (GuildGuestSelectScroll) null));
  }

  protected virtual IEnumerator CreateScroll(int info_index, int bar_index)
  {
    yield break;
  }

  public override void onBackButton()
  {
  }
}
