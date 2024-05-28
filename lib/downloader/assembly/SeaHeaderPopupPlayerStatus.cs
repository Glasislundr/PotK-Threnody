// Decompiled with JetBrains decompiler
// Type: SeaHeaderPopupPlayerStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class SeaHeaderPopupPlayerStatus : MonoBehaviour
{
  [SerializeField]
  private UILabel lvLabel;
  [SerializeField]
  private UILabel nextExpLabel;
  [SerializeField]
  private CommonHeaderAP apHeader;
  [SerializeField]
  private CommonHeaderBP dpHeader;
  [SerializeField]
  private UILabel princessNumberLabel;
  [SerializeField]
  private UILabel itemNumberLabel;
  [SerializeField]
  private UILabel guildNameLabel;
  [SerializeField]
  private UILabel guildRoleLabel;

  public void SetPlayerStatus(Player player)
  {
    this.lvLabel.SetTextLocalize(player.level);
    this.nextExpLabel.SetTextLocalize(player.exp_next);
  }

  public CommonHeaderAP GetCommonAP() => this.apHeader;

  public CommonHeaderBP GetCommonDP() => this.dpHeader;

  public void SetUnitCount(int now, int max)
  {
    this.princessNumberLabel.SetTextLocalize(Consts.Format(Consts.GetInstance().HEADER_POPUP_UNIT_COUNT, (IDictionary) new Hashtable()
    {
      {
        (object) nameof (now),
        (object) now
      },
      {
        (object) nameof (max),
        (object) max
      }
    }));
  }

  public void SetItemCount(int now, int max)
  {
    this.itemNumberLabel.SetTextLocalize(Consts.Format(Consts.GetInstance().HEADER_POPUP_ITEM_COUNT, (IDictionary) new Hashtable()
    {
      {
        (object) nameof (now),
        (object) now
      },
      {
        (object) nameof (max),
        (object) max
      }
    }));
  }

  public void SetGuildStatus(PlayerAffiliation affiliation)
  {
    if (affiliation != null && affiliation.status == GuildMembershipStatus.membership)
    {
      this.guildNameLabel.SetTextLocalize(PlayerAffiliation.Current.guild.guild_name);
      this.guildRoleLabel.SetTextLocalize(PlayerAffiliation.Current.role_name.name);
    }
    else
    {
      this.guildNameLabel.SetTextLocalize(Consts.GetInstance().GUILD_HEADER_GUILD_NAME_NONE);
      this.guildRoleLabel.SetTextLocalize(Consts.GetInstance().COMMON_NOVALUE);
    }
  }
}
