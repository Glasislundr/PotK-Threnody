// Decompiled with JetBrains decompiler
// Type: SM_PlayerItemHistoryExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public static class SM_PlayerItemHistoryExtension
{
  public static PlayerItemHistory[] AllGears(this PlayerItemHistory[] self)
  {
    return ((IEnumerable<PlayerItemHistory>) self).Where<PlayerItemHistory>((Func<PlayerItemHistory, bool>) (pi => pi.gear != null)).ToArray<PlayerItemHistory>();
  }

  public static PlayerItemHistory[] AllItems(this PlayerItemHistory[] self)
  {
    return ((IEnumerable<PlayerItemHistory>) self).Where<PlayerItemHistory>((Func<PlayerItemHistory, bool>) (pi => pi.item != null)).ToArray<PlayerItemHistory>();
  }
}
