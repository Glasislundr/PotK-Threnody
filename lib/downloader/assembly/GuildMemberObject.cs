// Decompiled with JetBrains decompiler
// Type: GuildMemberObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class GuildMemberObject
{
  private GameObject guildMemberListPopup;
  private GameObject guildMemberInfoPopup;
  private GameObject guildMemberListPrefab;
  private GameObject guildPositionManagementPopup;
  private GameObject guildPositionManagementPopupYesNo;
  private GameObject guildPositionManagementPopupOk;

  public GameObject GuildMemberListPopup => this.guildMemberListPopup;

  public GameObject GuildMemberInfoPopup => this.guildMemberInfoPopup;

  public GameObject GuildMemberListPrefab => this.guildMemberListPrefab;

  public GameObject GuildPositionManagementPopup => this.guildPositionManagementPopup;

  public GameObject GuildPositionManagementPopupYesNo => this.guildPositionManagementPopupYesNo;

  public GameObject GuildPositionManagementPopupOk => this.guildPositionManagementPopupOk;

  public IEnumerator ResourceLoad()
  {
    Future<GameObject> fgObj;
    IEnumerator e;
    if (Object.op_Equality((Object) this.guildMemberListPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_guild_member_list__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildMemberListPopup = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.guildMemberInfoPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_guild_member_info__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildMemberInfoPopup = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.guildMemberListPrefab, (Object) null))
    {
      fgObj = Res.Prefabs.guild.guild_member_list.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildMemberListPrefab = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.guildPositionManagementPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_guild_member_posiotion_management__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildPositionManagementPopup = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.guildPositionManagementPopupYesNo, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_guild_guildmaster_resign_confirmation__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildPositionManagementPopupYesNo = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.guildPositionManagementPopupOk, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_guild_guildmaster_resign_result__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildPositionManagementPopupOk = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
  }
}
