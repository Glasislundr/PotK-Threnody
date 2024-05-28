// Decompiled with JetBrains decompiler
// Type: CommonGuildButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class CommonGuildButton : MypageEventButton
{
  public UISprite guildBtnSprite;
  [SerializeField]
  private GameObject mNormalObj;
  [SerializeField]
  private GameObject mRaidObj;
  [SerializeField]
  private UI2DSprite mRaidImage;
  [SerializeField]
  private GameObject guildBadgeLabelEntry;
  [SerializeField]
  private GameObject guildBadgeLabelPrepare;
  [SerializeField]
  private GameObject guildBadgeLabelBattle;
  [SerializeField]
  private GameObject guildBadgeLabelMatch;
  private WebAPI.Response.GuildTop mGuildTopResponse;
  private const string guildBtnSpriteName = "ibtn_guild_Change.png__GUI__mypage_sozai__mypage_sozai_prefab";
  private const string guildBattleBtnSpriteName = "ibtn_GuildBattle_myapage.png__GUI__mypage_sozai__mypage_sozai_prefab";

  public IEnumerator UpdateButtonState(WebAPI.Response.GuildTop guildTopResponse)
  {
    CommonGuildButton commonGuildButton = this;
    commonGuildButton.mGuildTopResponse = guildTopResponse;
    GuildRaidPeriod guildRaidPeriod;
    if (commonGuildButton.mGuildTopResponse != null && commonGuildButton.mGuildTopResponse.raid_period != null && MasterData.GuildRaidPeriod.TryGetValue(commonGuildButton.mGuildTopResponse.raid_period.id, out guildRaidPeriod))
    {
      Future<Sprite> ft = new ResourceObject(string.Format("Prefabs/Banners/RaidButton/{0}/ibtn_Raid", (object) guildRaidPeriod.button_id)).Load<Sprite>();
      IEnumerator e = ft.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (Object.op_Inequality((Object) ft.Result, (Object) null))
        commonGuildButton.mRaidImage.sprite2D = ft.Result;
      ft = (Future<Sprite>) null;
    }
    commonGuildButton.UpdateButtonState();
    commonGuildButton.SetGuildBtnState();
  }

  public override bool IsActive()
  {
    return this.mGuildTopResponse == null || this.mGuildTopResponse.raid_period == null;
  }

  public override bool IsBadge() => false;

  public override void SetActive(bool value)
  {
    if (value)
    {
      this.mNormalObj.SetActive(true);
      this.mRaidObj.SetActive(false);
    }
    else
    {
      this.mNormalObj.SetActive(false);
      this.mRaidObj.SetActive(true);
    }
  }

  public void SetGuildBtnState()
  {
    GuildUtil.GuildBadgeLabelType labelType = GuildUtil.GuildBadgeLabelType.none;
    if (PlayerAffiliation.Current != null && PlayerAffiliation.Current.guild != null)
    {
      switch (PlayerAffiliation.Current.guild.gvg_status)
      {
        case GvgStatus.can_entry:
          labelType = GuildUtil.GuildBadgeLabelType.entry;
          break;
        case GvgStatus.matching:
          labelType = GuildUtil.GuildBadgeLabelType.matching;
          break;
        case GvgStatus.preparing:
          labelType = GuildUtil.GuildBadgeLabelType.prepare;
          break;
        case GvgStatus.fighting:
          labelType = GuildUtil.GuildBadgeLabelType.battle;
          break;
      }
    }
    this.SetGuildFooterBadgeLabel(labelType);
  }

  public void SetGuildFooterBadgeLabel(GuildUtil.GuildBadgeLabelType labelType)
  {
    if (Singleton<NGGameDataManager>.GetInstance().IsEarth)
      return;
    this.guildBadgeLabelEntry.SetActive(false);
    this.guildBadgeLabelPrepare.SetActive(false);
    this.guildBadgeLabelBattle.SetActive(false);
    this.guildBadgeLabelMatch.SetActive(false);
    ((Component) this.guildBtnSprite).gameObject.GetComponent<UIButton>().normalSprite = "ibtn_guild_Change.png__GUI__mypage_sozai__mypage_sozai_prefab";
    ((Component) this.guildBtnSprite).gameObject.GetComponent<UIButton>().pressedSprite = "ibtn_guild_Change.png__GUI__mypage_sozai__mypage_sozai_prefab";
    ((UIWidget) this.guildBtnSprite).MakePixelPerfect();
    if (labelType == GuildUtil.GuildBadgeLabelType.none)
      return;
    this.guildBadgeLabelEntry.SetActive(labelType == GuildUtil.GuildBadgeLabelType.entry);
    this.guildBadgeLabelPrepare.SetActive(labelType == GuildUtil.GuildBadgeLabelType.prepare);
    this.guildBadgeLabelBattle.SetActive(labelType == GuildUtil.GuildBadgeLabelType.battle);
    this.guildBadgeLabelMatch.SetActive(labelType == GuildUtil.GuildBadgeLabelType.matching);
    if (labelType != GuildUtil.GuildBadgeLabelType.prepare && labelType != GuildUtil.GuildBadgeLabelType.battle)
      return;
    ((Component) this.guildBtnSprite).gameObject.GetComponent<UIButton>().normalSprite = "ibtn_GuildBattle_myapage.png__GUI__mypage_sozai__mypage_sozai_prefab";
    ((Component) this.guildBtnSprite).gameObject.GetComponent<UIButton>().pressedSprite = "ibtn_GuildBattle_myapage.png__GUI__mypage_sozai__mypage_sozai_prefab";
    ((UIWidget) this.guildBtnSprite).SetDimensions(110, 142);
    this.mNormalObj.SetActive(true);
    this.mRaidObj.SetActive(false);
  }
}
