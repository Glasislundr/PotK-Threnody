// Decompiled with JetBrains decompiler
// Type: GuildRaidButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class GuildRaidButton : MypageEventButton
{
  [SerializeField]
  private GameObject mOnTimeObj;
  [SerializeField]
  private GameObject mOffTimeObj;
  [SerializeField]
  private GameObject mAggregatingTimeObj;
  [SerializeField]
  private UILabel mOnTimeRestLbl;
  [SerializeField]
  private UILabel mOnTimeRestRightLbl;
  [SerializeField]
  private UILabel mRestRPLbl;
  [SerializeField]
  private UI2DSprite mOnTimeImage;
  private WebAPI.Response.GuildTop mGuildTopResponse;
  private DateTime mTime;

  public IEnumerator UpdateButtonState(WebAPI.Response.GuildTop guildTopResponse)
  {
    GuildRaidButton guildRaidButton = this;
    guildRaidButton.mGuildTopResponse = guildTopResponse;
    guildRaidButton.mTime = ServerTime.NowAppTimeAddDelta();
    GuildRaidPeriod guildRaidPeriod;
    if (guildRaidButton.mGuildTopResponse != null && guildRaidButton.mGuildTopResponse.raid_period != null && MasterData.GuildRaidPeriod.TryGetValue(guildRaidButton.mGuildTopResponse.raid_period.id, out guildRaidPeriod))
    {
      Future<Sprite> ft = new ResourceObject(string.Format("Prefabs/Banners/RaidButton/{0}/ibtn_Raid", (object) guildRaidPeriod.button_id)).Load<Sprite>();
      IEnumerator e = ft.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (Object.op_Inequality((Object) ft.Result, (Object) null))
        guildRaidButton.mOnTimeImage.sprite2D = ft.Result;
      ft = (Future<Sprite>) null;
    }
    guildRaidButton.UpdateButtonState();
  }

  public override bool IsActive()
  {
    return this.mGuildTopResponse != null && (this.mGuildTopResponse.raid_period != null || this.mGuildTopResponse.raid_aggregating);
  }

  public override bool IsBadge() => false;

  public override void SetActive(bool value)
  {
    if (value)
    {
      this.mOffTimeObj.SetActive(false);
      RaidPeriod raidPeriod = this.mGuildTopResponse.raid_period;
      if (raidPeriod != null)
      {
        this.mOnTimeObj.SetActive(true);
        DateTime? endAt = raidPeriod.end_at;
        DateTime mTime = this.mTime;
        (endAt.HasValue ? new TimeSpan?(endAt.GetValueOrDefault() - mTime) : new TimeSpan?()).Value.SetDisplayStringForGuildRaid(ref this.mOnTimeRestLbl, ref this.mOnTimeRestRightLbl);
        this.mRestRPLbl.SetTextLocalize(GuildUtil.rp);
        this.mAggregatingTimeObj.SetActive(false);
      }
      else
      {
        if (!this.mGuildTopResponse.raid_aggregating)
          return;
        this.mAggregatingTimeObj.SetActive(true);
        this.mOnTimeObj.SetActive(false);
      }
    }
    else
    {
      this.mOffTimeObj.SetActive(true);
      this.mOnTimeObj.SetActive(false);
      this.mAggregatingTimeObj.SetActive(false);
    }
  }
}
