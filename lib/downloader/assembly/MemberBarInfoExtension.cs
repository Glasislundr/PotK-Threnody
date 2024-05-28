// Decompiled with JetBrains decompiler
// Type: MemberBarInfoExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public static class MemberBarInfoExtension
{
  public static IEnumerable<MemberBarInfo> SortBy(
    this IEnumerable<MemberBarInfo> self,
    GuildMemberSort.SORT_TYPES sortType,
    SortAndFilter.SORT_TYPE_ORDER_BUY order)
  {
    MemberBarInfo memberBarInfo1 = (MemberBarInfo) null;
    List<MemberBarInfo> source = new List<MemberBarInfo>();
    List<MemberBarInfo> memberBarInfoList = new List<MemberBarInfo>();
    foreach (MemberBarInfo memberBarInfo2 in self)
    {
      if (memberBarInfo2.member.role == GuildRole.master)
        memberBarInfo1 = memberBarInfo2;
      else
        source.Add(memberBarInfo2);
    }
    switch (sortType)
    {
      case GuildMemberSort.SORT_TYPES.Contribution:
        memberBarInfoList = order != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? source.OrderByDescending<MemberBarInfo, int>((Func<MemberBarInfo, int>) (x => x.member.contribution)).OrderByDescending<MemberBarInfo, bool>((Func<MemberBarInfo, bool>) (x => x.member.role == GuildRole.master)).ToList<MemberBarInfo>() : source.OrderBy<MemberBarInfo, int>((Func<MemberBarInfo, int>) (x => x.member.contribution)).ToList<MemberBarInfo>();
        break;
      case GuildMemberSort.SORT_TYPES.Level:
        memberBarInfoList = order != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? source.OrderByDescending<MemberBarInfo, int>((Func<MemberBarInfo, int>) (x => x.member.player.player_level)).ToList<MemberBarInfo>() : source.OrderBy<MemberBarInfo, int>((Func<MemberBarInfo, int>) (x => x.member.player.player_level)).ToList<MemberBarInfo>();
        break;
      case GuildMemberSort.SORT_TYPES.LastLoginAt:
        memberBarInfoList = order != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? source.OrderByDescending<MemberBarInfo, DateTime>((Func<MemberBarInfo, DateTime>) (x => x.member.player.last_signed_in_at)).ToList<MemberBarInfo>() : source.OrderBy<MemberBarInfo, DateTime>((Func<MemberBarInfo, DateTime>) (x => x.member.player.last_signed_in_at)).ToList<MemberBarInfo>();
        break;
      case GuildMemberSort.SORT_TYPES.JoinAt:
        memberBarInfoList = order != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? source.OrderByDescending<MemberBarInfo, DateTime>((Func<MemberBarInfo, DateTime>) (x => x.member.joined_at)).ToList<MemberBarInfo>() : source.OrderBy<MemberBarInfo, DateTime>((Func<MemberBarInfo, DateTime>) (x => x.member.joined_at)).ToList<MemberBarInfo>();
        break;
    }
    if (memberBarInfo1 == null)
      return (IEnumerable<MemberBarInfo>) memberBarInfoList;
    memberBarInfoList.Insert(0, memberBarInfo1);
    return (IEnumerable<MemberBarInfo>) memberBarInfoList;
  }
}
