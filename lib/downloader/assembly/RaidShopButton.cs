// Decompiled with JetBrains decompiler
// Type: RaidShopButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;

#nullable disable
public class RaidShopButton : MypageEventButton
{
  public override bool IsActive()
  {
    DateTime dateTime1 = ServerTime.NowAppTimeAddDelta();
    ShopShop shopShop;
    if (MypageRootMenu.CurrentMode != MypageRootMenu.Mode.GUILD || !MasterData.ShopShop.TryGetValue(8000, out shopShop))
      return false;
    DateTime dateTime2 = dateTime1;
    DateTime? startAt = shopShop.start_at;
    return startAt.HasValue && dateTime2 >= startAt.GetValueOrDefault();
  }

  public override bool IsBadge()
  {
    NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
    if (!instance.raidMedalShopLatestStartTime.HasValue)
      return false;
    DateTime dateTime1 = ServerTime.NowAppTimeAddDelta();
    DateTime? shopLatestStartTime = instance.raidMedalShopLatestStartTime;
    DateTime dateTime2 = dateTime1;
    return shopLatestStartTime.HasValue && shopLatestStartTime.GetValueOrDefault() <= dateTime2;
  }
}
