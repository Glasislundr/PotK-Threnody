// Decompiled with JetBrains decompiler
// Type: Friend0085Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Friend0085Scene : NGSceneBase
{
  [SerializeField]
  private Friend0085Menu menu;
  private PlayerFriend[] recivedFriend;

  public override IEnumerator onInitSceneAsync()
  {
    this.recivedFriend = ((IEnumerable<PlayerFriend>) SMManager.Get<PlayerFriend[]>()).ReceivedFriendApplications();
    IEnumerator e = this.menu.InitFriendScroll();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync()
  {
    bool flag = false;
    foreach (PlayerFriend playerFriend in this.recivedFriend)
    {
      PlayerFriend f = playerFriend;
      if (((IEnumerable<PlayerFriend>) ((IEnumerable<PlayerFriend>) SMManager.Get<PlayerFriend[]>()).ReceivedFriendApplications()).Where<PlayerFriend>((Func<PlayerFriend, bool>) (x => x.sent_player_id == f.sent_player_id)).FirstOrDefault<PlayerFriend>() == null)
      {
        flag = true;
        this.recivedFriend = ((IEnumerable<PlayerFriend>) SMManager.Get<PlayerFriend[]>()).ReceivedFriendApplications();
        break;
      }
    }
    if (flag)
    {
      IEnumerator e = this.menu.InitFriendScroll();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }
}
