// Decompiled with JetBrains decompiler
// Type: GuildStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
[Serializable]
public class GuildStatus
{
  [SerializeField]
  private UI2DSprite guildTitleImage;
  private Future<Sprite> futureGuildTitleImage;
  public SpriteDecimalControl starNumber;
  public UILabel guildRank;
  public UILabel guildCurrentMember;
  public UILabel guildMaxMember;
  public UILabel guildName;
  public NGTweenGaugeScale starGauge;
  public NGTweenGaugeFillAmount starGaugeFillAmount;

  public IEnumerator ResourceLoad(GuildRegistration data)
  {
    if (data != null)
    {
      this.futureGuildTitleImage = EmblemUtility.LoadGuildEmblemSprite(data.appearance._current_emblem);
      IEnumerator e = this.futureGuildTitleImage.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public void SetStatus(GuildRegistration data)
  {
    if (Object.op_Inequality((Object) this.guildTitleImage, (Object) null))
      this.guildTitleImage.sprite2D = this.futureGuildTitleImage.Result;
    if (Object.op_Inequality((Object) this.guildRank, (Object) null))
      this.guildRank.SetTextLocalize(data.appearance.level.ToString());
    if (Object.op_Inequality((Object) this.guildCurrentMember, (Object) null))
      this.guildCurrentMember.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_1_1_4_CURRENT_MEMBER, (IDictionary) new Hashtable()
      {
        {
          (object) "currentMember",
          (object) data.appearance.membership_num
        }
      }));
    if (Object.op_Inequality((Object) this.guildMaxMember, (Object) null))
      this.guildMaxMember.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_1_1_4_MAX_MEMBER, (IDictionary) new Hashtable()
      {
        {
          (object) "maxMember",
          (object) data.appearance.membership_capacity
        }
      }));
    if (Object.op_Inequality((Object) this.guildName, (Object) null))
      this.guildName.SetTextLocalize(data.guild_name);
    int num1 = ((IEnumerable<GuildMembership>) data.memberships).Sum<GuildMembership>((Func<GuildMembership, int>) (x => x.capture_star));
    IEnumerable<GuildMembership> source = ((IEnumerable<GuildMembership>) data.memberships).Where<GuildMembership>((Func<GuildMembership, bool>) (x => x.is_defense_membership));
    int num2 = source.Count<GuildMembership>() * data.gvg_max_star_possession;
    int num3 = source.Sum<GuildMembership>((Func<GuildMembership, int>) (x => x.own_star));
    if (Object.op_Inequality((Object) this.starGauge, (Object) null))
      this.starGauge.setValue(num3, num2);
    if (Object.op_Inequality((Object) this.starGaugeFillAmount, (Object) null))
      this.starGaugeFillAmount.setValue(num3, num2, true, -1f, -1f);
    if (!Object.op_Inequality((Object) this.starNumber, (Object) null))
      return;
    this.starNumber.resetNumber(num1);
  }

  public void UpdateStatus(GuildRegistration data)
  {
    if (data == null)
      return;
    if (Object.op_Inequality((Object) this.guildTitleImage, (Object) null) && (Object.op_Equality((Object) this.guildTitleImage.sprite2D, (Object) null) || ((Object) this.guildTitleImage.sprite2D).name != ((Object) this.futureGuildTitleImage.Result).name))
      this.guildTitleImage.sprite2D = this.futureGuildTitleImage.Result;
    if (Object.op_Inequality((Object) this.guildRank, (Object) null))
      this.guildRank.SetTextLocalize(data.appearance.level.ToString());
    if (Object.op_Inequality((Object) this.guildCurrentMember, (Object) null))
      this.guildCurrentMember.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_1_1_4_CURRENT_MEMBER, (IDictionary) new Hashtable()
      {
        {
          (object) "currentMember",
          (object) data.appearance.membership_num
        }
      }));
    if (Object.op_Inequality((Object) this.guildMaxMember, (Object) null))
      this.guildMaxMember.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_1_1_4_MAX_MEMBER, (IDictionary) new Hashtable()
      {
        {
          (object) "maxMember",
          (object) data.appearance.membership_capacity
        }
      }));
    if (Object.op_Inequality((Object) this.guildName, (Object) null))
      this.guildName.SetTextLocalize(data.guild_name);
    int num1 = ((IEnumerable<GuildMembership>) data.memberships).Sum<GuildMembership>((Func<GuildMembership, int>) (x => x.capture_star));
    IEnumerable<GuildMembership> source = ((IEnumerable<GuildMembership>) data.memberships).Where<GuildMembership>((Func<GuildMembership, bool>) (x => x.is_defense_membership));
    int num2 = source.Count<GuildMembership>() * data.gvg_max_star_possession;
    int num3 = source.Sum<GuildMembership>((Func<GuildMembership, int>) (x => x.own_star));
    if (Object.op_Inequality((Object) this.starGauge, (Object) null))
      this.starGauge.setValue(num3, num2, duration: 1f);
    if (Object.op_Inequality((Object) this.starGaugeFillAmount, (Object) null))
      this.starGaugeFillAmount.setValue(num3, num2, true, -1f, 1f);
    if (!Object.op_Inequality((Object) this.starNumber, (Object) null))
      return;
    this.starNumber.setNumber(num1);
  }
}
