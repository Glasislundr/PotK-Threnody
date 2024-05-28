// Decompiled with JetBrains decompiler
// Type: FriendBarInfoExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public static class FriendBarInfoExtension
{
  public static IEnumerable<FriendBarInfo> SortBy(this IEnumerable<FriendBarInfo> self)
  {
    int friend = Persist.sortOrder.Data.Friend;
    List<FriendBarInfo> friendBarInfoList = new List<FriendBarInfo>();
    switch (Persist.sortOrder.Data.Friend)
    {
      case 0:
        friendBarInfoList = self.OrderByDescending<FriendBarInfo, bool>((Func<FriendBarInfo, bool>) (fr => fr.friend.is_favorite)).ThenByDescending<FriendBarInfo, DateTime?>((Func<FriendBarInfo, DateTime?>) (fr => fr.friend.applied_at)).ToList<FriendBarInfo>();
        break;
      case 1:
        friendBarInfoList = self.OrderByDescending<FriendBarInfo, DateTime?>((Func<FriendBarInfo, DateTime?>) (fr => fr.friend.applied_at)).ThenByDescending<FriendBarInfo, int>((Func<FriendBarInfo, int>) (fr => fr.friend.level)).ToList<FriendBarInfo>();
        break;
      case 2:
        friendBarInfoList = self.OrderByDescending<FriendBarInfo, int>((Func<FriendBarInfo, int>) (fr => fr.friend.level)).ThenByDescending<FriendBarInfo, int>((Func<FriendBarInfo, int>) (fr => fr.friend.leader_unit.level)).ToList<FriendBarInfo>();
        break;
    }
    return (IEnumerable<FriendBarInfo>) friendBarInfoList;
  }
}
