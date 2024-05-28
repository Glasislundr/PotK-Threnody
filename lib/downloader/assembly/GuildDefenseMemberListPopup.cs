// Decompiled with JetBrains decompiler
// Type: GuildDefenseMemberListPopup
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
public class GuildDefenseMemberListPopup : GuildMemberListPopup
{
  private GuildRegistration GuildInfo;
  [SerializeField]
  private UIButton ibtn_Popup_Decision;
  private const int NonSelectedMemberPriority = 0;

  public Dictionary<int, string> SelectedKeyPriorityValuePlayerIDDic { get; set; }

  public Dictionary<string, int> LastSaveKeyPlayerIDValuePriorityDic { get; set; }

  public IEnumerator Initialize(GameObject memberPrefab, GuildRegistration guildInfo)
  {
    GuildDefenseMemberListPopup defenseMemberListPopup = this;
    defenseMemberListPopup.GuildInfo = guildInfo;
    defenseMemberListPopup.SelectedKeyPriorityValuePlayerIDDic = new Dictionary<int, string>();
    defenseMemberListPopup.LastSaveKeyPlayerIDValuePriorityDic = new Dictionary<string, int>();
    for (int i = 1; i <= guildInfo.memberships.Length; i++)
    {
      defenseMemberListPopup.LastSaveKeyPlayerIDValuePriorityDic.Add(guildInfo.memberships[i - 1].player.player_id, guildInfo.memberships[i - 1].defense_priority);
      GuildMembership guildMembership = ((IEnumerable<GuildMembership>) guildInfo.memberships).FirstOrDefault<GuildMembership>((Func<GuildMembership, bool>) (x => i == x.defense_priority));
      if (guildMembership != null)
        defenseMemberListPopup.SelectedKeyPriorityValuePlayerIDDic.Add(i, guildMembership.player.player_id);
      else
        defenseMemberListPopup.SelectedKeyPriorityValuePlayerIDDic.Add(i, (string) null);
    }
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) null;
    UIWidget component = ((Component) defenseMemberListPopup).GetComponent<UIWidget>();
    if (Object.op_Inequality((Object) component, (Object) null))
      ((UIRect) component).alpha = 0.0f;
    IEnumerator e = defenseMemberListPopup.InitMemberScroll(memberPrefab, guildInfo.memberships);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = defenseMemberListPopup.CreateSortPopup();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }

  protected virtual IEnumerator InitMemberScroll(GameObject memberPrefab, GuildMembership[] members)
  {
    GuildDefenseMemberListPopup defenseMemberListPopup = this;
    defenseMemberListPopup.allMemberInfo.Clear();
    defenseMemberListPopup.allMemberBar.Clear();
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    DateTime nowTime = ServerTime.NowAppTime();
    defenseMemberListPopup.Initialize(nowTime, 518, 157, 15, 8);
    defenseMemberListPopup.CreateMemberInfo(members);
    if (defenseMemberListPopup.allMemberInfo.Count > 0)
    {
      e = defenseMemberListPopup.CreateScrollBase(memberPrefab);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    defenseMemberListPopup.scroll.ResolvePosition();
    defenseMemberListPopup.scroll.scrollView.UpdatePosition();
    defenseMemberListPopup.guildMemberNum.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_MEMBER_NUMBER, (IDictionary) new Hashtable()
    {
      {
        (object) "now",
        (object) members.Length
      },
      {
        (object) "max",
        (object) PlayerAffiliation.Current.guild.appearance.membership_capacity
      }
    }));
    defenseMemberListPopup.InitializeEnd();
  }

  protected new virtual IEnumerator CreateScrollBase(GameObject prefab)
  {
    GuildDefenseMemberListPopup popup = this;
    popup.allMemberBar.Clear();
    for (int index = 0; index < Mathf.Min(popup.MaxValue, popup.allMemberInfo.Count); ++index)
    {
      Guild0282MemberScroll component = Object.Instantiate<GameObject>(prefab).GetComponent<Guild0282MemberScroll>();
      component.DefenseMemberSelectInitialize(popup);
      popup.allMemberBar.Add(component);
    }
    popup.scroll.Reset();
    for (int index = 0; index < Mathf.Min(popup.MaxValue, popup.allMemberBar.Count); ++index)
      popup.scroll.AddColumn1(((Component) popup.allMemberBar[index]).gameObject, popup.barWidth, popup.barHeight);
    popup.scroll.CreateScrollPointHeight(popup.barHeight, popup.allMemberInfo.Count);
    popup.scroll.ResolvePosition();
    for (int index = 0; index < Mathf.Min(popup.MaxValue, popup.allMemberInfo.Count); ++index)
      popup.ResetScroll(index);
    for (int i = 0; i < Mathf.Min(popup.MaxValue, popup.allMemberInfo.Count); ++i)
    {
      IEnumerator e = popup.CreateScroll(i, i);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public override void onBackButton()
  {
    if (this.IsOpenSortPopup)
      return;
    foreach (KeyValuePair<string, int> keyValuePair in this.LastSaveKeyPlayerIDValuePriorityDic)
    {
      KeyValuePair<string, int> dic = keyValuePair;
      GuildMembership guildMembership = ((IEnumerable<GuildMembership>) this.GuildInfo.memberships).FirstOrDefault<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id == dic.Key));
      if (guildMembership != null)
        guildMembership.defense_priority = dic.Value;
    }
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public void onClearButton()
  {
    if (this.IsOpenSortPopup)
      return;
    foreach (GuildMembership membership in this.GuildInfo.memberships)
      membership.defense_priority = 0;
    this.SelectedKeyPriorityValuePlayerIDDic.Clear();
    int key = 1;
    for (int index = 0; index < this.GuildInfo.memberships.Length; ++index)
    {
      this.SelectedKeyPriorityValuePlayerIDDic.Add(key, (string) null);
      ++key;
    }
    this.allMemberBar.ForEach((Action<Guild0282MemberScroll>) (x => this.StartCoroutine(x.SetData())));
  }

  public void onDecisionButton()
  {
    if (this.IsOpenSortPopup)
      return;
    this.StartCoroutine(this.SendDefenseMenber());
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  private IEnumerator SendDefenseMenber()
  {
    List<int> intList = new List<int>();
    List<string> stringList = new List<string>();
    bool flag = false;
    intList.Clear();
    stringList.Clear();
    foreach (KeyValuePair<string, int> keyValuePair1 in this.LastSaveKeyPlayerIDValuePriorityDic)
    {
      if (!flag)
      {
        foreach (KeyValuePair<int, string> keyValuePair2 in this.SelectedKeyPriorityValuePlayerIDDic)
        {
          if (keyValuePair1.Value == keyValuePair2.Key && keyValuePair1.Key != keyValuePair2.Value)
          {
            flag = true;
            break;
          }
          if (keyValuePair1.Value == 0 && keyValuePair2.Value == keyValuePair1.Key && keyValuePair2.Key != 0)
          {
            flag = true;
            break;
          }
        }
      }
      else
        break;
    }
    if (flag)
    {
      foreach (KeyValuePair<int, string> keyValuePair in this.SelectedKeyPriorityValuePlayerIDDic)
      {
        if (keyValuePair.Value != null)
        {
          intList.Add(keyValuePair.Key);
          stringList.Add(keyValuePair.Value);
        }
      }
      if (flag)
      {
        IEnumerator e = WebAPI.GvgDefenseMember(intList.ToArray(), stringList.ToArray()).Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }
}
