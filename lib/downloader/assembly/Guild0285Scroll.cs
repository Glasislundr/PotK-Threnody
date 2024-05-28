// Decompiled with JetBrains decompiler
// Type: Guild0285Scroll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Guild0285Scroll : MonoBehaviour
{
  private const int MAX_DISPLAY_DAY_NUM = 3;
  private Guild0285Menu menu;
  private GuildApplicant applicantInfo;
  private UnitIcon unitIcon;
  private GameObject unitIconPrefab;
  private DateTime now;
  private List<string> player_ids;
  [SerializeField]
  private UILabel playerName;
  [SerializeField]
  private UILabel playerLevel;
  [SerializeField]
  private UILabel requestedDate;
  [SerializeField]
  private UILabel txtLastPlay;
  [SerializeField]
  private UILabel txtLastPlayDate;
  [SerializeField]
  private UI2DSprite playerImage;
  [SerializeField]
  private UI2DSprite playerTitleImage;

  public IEnumerator Initialize(Guild0285Menu menu, GuildApplicant applicant, DateTime now)
  {
    this.menu = menu;
    this.applicantInfo = applicant;
    this.now = now;
    this.player_ids = new List<string>();
    this.SetData();
    IEnumerator e = this.SetUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.SetApplication(this.requestedDate);
  }

  public IEnumerator SetUnitIcon()
  {
    Guild0285Scroll guild0285Scroll = this;
    IEnumerator e;
    if (Object.op_Equality((Object) guild0285Scroll.unitIconPrefab, (Object) null))
    {
      Future<GameObject> unitIconPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = unitIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      guild0285Scroll.unitIconPrefab = unitIconPrefabF.Result;
      unitIconPrefabF = (Future<GameObject>) null;
    }
    if (guild0285Scroll.applicantInfo != null)
    {
      PlayerUnit player = PlayerUnit.create_by_unitunit(guild0285Scroll.applicantInfo.player.leader_unit_unit);
      player.level = guild0285Scroll.applicantInfo.player.leader_unit_level;
      player.job_id = guild0285Scroll.applicantInfo.player.leader_unit_job_id;
      if (Object.op_Equality((Object) guild0285Scroll.unitIcon, (Object) null))
        guild0285Scroll.unitIcon = guild0285Scroll.unitIconPrefab.CloneAndGetComponent<UnitIcon>(((Component) guild0285Scroll.playerImage).gameObject);
      // ISSUE: reference to a compiler-generated method
      guild0285Scroll.unitIcon.onClick = new Action<UnitIconBase>(guild0285Scroll.\u003CSetUnitIcon\u003Eb__15_0);
      guild0285Scroll.unitIcon.Button.onLongPress.Clear();
      // ISSUE: reference to a compiler-generated method
      guild0285Scroll.unitIcon.Button.onLongPress.Add(new EventDelegate(new EventDelegate.Callback(guild0285Scroll.\u003CSetUnitIcon\u003Eb__15_1)));
      e = guild0285Scroll.unitIcon.setSimpleUnit(player);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      guild0285Scroll.unitIcon.setLevelText(player);
      guild0285Scroll.unitIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
      player = (PlayerUnit) null;
    }
    Future<Sprite> sprF = EmblemUtility.LoadEmblemSprite(guild0285Scroll.applicantInfo.player.player_emblem_id);
    e = sprF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    guild0285Scroll.playerTitleImage.sprite2D = sprF.Result;
  }

  public void onDetails(GuildPlayerInfo playerInfo, int playerUnitID)
  {
    Unit0042Scene.changeSceneFriendUnit(true, playerInfo.player_id, playerUnitID);
  }

  protected void SetApplication(UILabel label)
  {
    DateTime dateTime = new DateTime(this.applicantInfo.applied_at.Year, this.applicantInfo.applied_at.Month, this.applicantInfo.applied_at.Day);
    TimeSpan self = ServerTime.NowAppTime() - dateTime;
    label.SetTextLocalize(Consts.Format(Consts.GetInstance().FRIEND_0085SCROLLPARTS_DESCRIPTION01, (IDictionary) new Hashtable()
    {
      {
        (object) "dsfapplied",
        (object) self.DisplayStringForFriendsApplied().ToConverter()
      }
    }));
  }

  private void SetData()
  {
    this.txtLastPlay.SetTextLocalize(Consts.Format(Consts.GetInstance().GUILD_28_5_LAST_LOGIN));
    this.playerName.SetTextLocalize(this.applicantInfo.player.player_name);
    this.playerLevel.SetTextLocalize(this.applicantInfo.player.player_level);
    TimeSpan self = this.now - this.applicantInfo.player.last_signed_in_at;
    if (self.Days < 3)
      this.txtLastPlayDate.SetTextLocalize(Consts.Format(Consts.GetInstance().GUILD_28_5_LAST_LOGIN_DATE, (IDictionary) new Hashtable()
      {
        {
          (object) "time",
          (object) self.DisplayString()
        }
      }));
    else
      this.txtLastPlayDate.SetTextLocalize(Consts.GetInstance().GUILD_28_5_LAST_LOGIN_DATE2);
  }

  private IEnumerator Accept()
  {
    Guild0285Scroll guild0285Scroll = this;
    guild0285Scroll.player_ids.Clear();
    guild0285Scroll.player_ids.Add(guild0285Scroll.applicantInfo.player.player_id);
    // ISSUE: reference to a compiler-generated method
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = guild0285Scroll.menu.Accept(guild0285Scroll.player_ids.ToArray(), new Action(guild0285Scroll.\u003CAccept\u003Eb__19_0), new Action<WebAPI.Response.UserError>(guild0285Scroll.\u003CAccept\u003Eb__19_1));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onAcceptButton()
  {
    if (PlayerAffiliation.Current.onGvgOperation)
      this.menu.ShowOkPopup(Consts.GetInstance().GUILD_28_5_ACCEPT_REQUEST_TITLE, PlayerAffiliation.Current.guild.gvg_status == GvgStatus.preparing ? Consts.GetInstance().GUILD_28_5_ACCEPT_ERROR_GVG_PREPARE : Consts.GetInstance().GUILD_28_5_ACCEPT_ERROR_GVG);
    else if (PlayerAffiliation.Current.guild.appearance.membership_num < PlayerAffiliation.Current.guild.appearance.membership_capacity)
      this.StartCoroutine(this.Accept());
    else
      this.menu.ShowOkPopup(Consts.Format(Consts.GetInstance().GUILD_28_5_ACCEPT_REQUEST_TITLE), Consts.Format(Consts.GetInstance().GUILD_28_5_ACCEAT_FAILED_DESC));
  }

  private IEnumerator Refuse()
  {
    Guild0285Scroll guild0285Scroll = this;
    guild0285Scroll.player_ids.Clear();
    guild0285Scroll.player_ids.Add(guild0285Scroll.applicantInfo.player.player_id);
    // ISSUE: reference to a compiler-generated method
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = guild0285Scroll.menu.Refuse(guild0285Scroll.player_ids.ToArray(), new Action(guild0285Scroll.\u003CRefuse\u003Eb__21_0), new Action<WebAPI.Response.UserError>(guild0285Scroll.\u003CRefuse\u003Eb__21_1));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onRefuseButton()
  {
    if (PlayerAffiliation.Current.onGvgOperation)
      this.menu.ShowOkPopup(Consts.GetInstance().GUILD_28_5_REJECT_REQUEST_TITLE, PlayerAffiliation.Current.guild.gvg_status == GvgStatus.preparing ? Consts.GetInstance().GUILD_28_5_REJECT_ERROR_GVG_PREPARE : Consts.GetInstance().GUILD_28_5_REJECT_ERROR_GVG);
    else
      this.StartCoroutine(this.Refuse());
  }
}
