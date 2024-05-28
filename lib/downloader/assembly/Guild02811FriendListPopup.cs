// Decompiled with JetBrains decompiler
// Type: Guild02811FriendListPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Guild02811FriendListPopup : BackButtonMenuBase
{
  [SerializeField]
  private NGxScroll scroll;
  [SerializeField]
  private GameObject nonFriend;
  private Guild02811Menu menu2811;
  private Guild02812Menu menu2812;
  private GuildUtil.MenuType aMenu;

  public void Initialize(
    Guild02811Menu menu,
    GameObject parts,
    WebAPI.Response.GuildFriendAffiliations friends,
    DateTime now)
  {
    this.aMenu = GuildUtil.MenuType.menu2811;
    this.menu2811 = menu;
    this.Init(parts, friends, now);
  }

  public void Initialize(
    Guild02812Menu menu,
    GameObject parts,
    WebAPI.Response.GuildFriendAffiliations friends,
    DateTime now)
  {
    this.aMenu = GuildUtil.MenuType.menu2812;
    this.menu2812 = menu;
    this.Init(parts, friends, now);
  }

  private void Init(
    GameObject parts,
    WebAPI.Response.GuildFriendAffiliations friends,
    DateTime now)
  {
    this.scroll.Clear();
    this.scroll.Reset();
    if (friends == null || friends.friend_affiliations.Length == 0)
    {
      this.nonFriend.SetActive(true);
    }
    else
    {
      bool flag = true;
      PlayerFriend[] source = SMManager.Get<PlayerFriend[]>();
      foreach (FriendAffiliation friendAffiliation in friends.friend_affiliations)
      {
        FriendAffiliation friend = friendAffiliation;
        if (friend.guild != null)
        {
          PlayerFriend friend1 = ((IEnumerable<PlayerFriend>) source).FirstOrDefault<PlayerFriend>((Func<PlayerFriend, bool>) (z => z.target_player_id == friend.player_id));
          if (friend1 != null)
          {
            GameObject gameObject = Object.Instantiate<GameObject>(parts);
            Guild02811FriendScrollBar component = gameObject.GetComponent<Guild02811FriendScrollBar>();
            if (Object.op_Inequality((Object) component, (Object) null))
            {
              component.Set(friend1, now);
              this.StartCoroutine(component.SetUnitIcon());
              switch (this.aMenu)
              {
                case GuildUtil.MenuType.menu2811:
                  component.SetMenu(this.menu2811);
                  break;
                case GuildUtil.MenuType.menu2812:
                  component.SetMenu(this.menu2812);
                  break;
              }
              component.Guild = friend.guild;
            }
            this.scroll.Add(gameObject);
            flag = false;
          }
        }
      }
      this.nonFriend.SetActive(flag);
    }
    this.scroll.ResolvePosition();
  }

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().dismiss();
}
