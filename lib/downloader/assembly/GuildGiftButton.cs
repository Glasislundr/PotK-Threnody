// Decompiled with JetBrains decompiler
// Type: GuildGiftButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using LocaleTimeZone;
using MasterDataTable;
using SM;
using System;
using UnityEngine;

#nullable disable
public class GuildGiftButton : MypageEventButton
{
  [SerializeField]
  private UIButton GuildGiftBtn;
  [SerializeField]
  private GameObject KeyObj;

  public override bool IsActive()
  {
    PlayerAffiliation current = PlayerAffiliation.Current;
    return current != null && current.isGuildMember();
  }

  public override bool IsBadge()
  {
    PlayerAffiliation current = PlayerAffiliation.Current;
    if ((current != null ? (current.isGuildMember() ? 1 : 0) : 0) != 0)
    {
      TimeZoneInfo timeZone = Japan.CreateTimeZone();
      DateTime dateTime = ServerTime.NowAppTimeAddDelta();
      foreach (GuildGiftEvent guildGiftEvent in MasterData.GuildGiftEventList)
      {
        if (guildGiftEvent.badge_flag && dateTime <= TimeZoneInfo.ConvertTime(guildGiftEvent.end_at.Value, timeZone, TimeZoneInfo.Local) && TimeZoneInfo.ConvertTime(guildGiftEvent.start_at.Value, timeZone, TimeZoneInfo.Local) <= dateTime)
          return true;
      }
    }
    return false;
  }

  public override void SetActive(bool value)
  {
    ((UIButtonColor) this.GuildGiftBtn).isEnabled = value;
    this.KeyObj.SetActive(!value);
    this.SetBadgeActive(false);
  }
}
