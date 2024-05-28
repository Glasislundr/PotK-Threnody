// Decompiled with JetBrains decompiler
// Type: StoryPlaybackExtraExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public static class StoryPlaybackExtraExtension
{
  public static StoryPlaybackExtra[] DisplayList(
    this StoryPlaybackExtra[] self,
    DateTime serverTime)
  {
    return ((IEnumerable<StoryPlaybackExtra>) self).Where<StoryPlaybackExtra>((Func<StoryPlaybackExtra, bool>) (x =>
    {
      if (!x.display_expire_at.HasValue)
        return true;
      if (!x.display_expire_at.HasValue)
        return false;
      DateTime? displayExpireAt = x.display_expire_at;
      DateTime dateTime = serverTime;
      return displayExpireAt.HasValue && displayExpireAt.GetValueOrDefault() > dateTime;
    })).ToArray<StoryPlaybackExtra>();
  }
}
