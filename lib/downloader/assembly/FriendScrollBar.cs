// Decompiled with JetBrains decompiler
// Type: FriendScrollBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class FriendScrollBar : MonoBehaviour
{
  [SerializeField]
  protected UILabel friendName;
  [SerializeField]
  protected UILabel lastPlay;
  [SerializeField]
  protected UILabel level;
  [SerializeField]
  protected GameObject LinkCharacter;
  [SerializeField]
  protected UI2DSprite Emblem;
  private PlayerFriend friend;
  protected UnitIcon unitIcon;
  protected GameObject unitIconPrefab;
  protected DateTime nowTime;
  protected Friend0081Menu menu;
  protected int index;

  public UnitIcon UnicIcon => this.unitIcon;

  public PlayerFriend Friend => this.friend;

  public void Set(PlayerFriend friend, DateTime now)
  {
    this.nowTime = now;
    this.friendName.SetTextLocalize(friend.target_player_name);
    this.level.SetTextLocalize(friend.level.ToString());
    TimeSpan self = this.nowTime - friend.target_player_last_signed_in_at;
    this.lastPlay.SetTextLocalize(Consts.Format(Consts.GetInstance().LAST_PLAY, (IDictionary) new Hashtable()
    {
      {
        (object) "time",
        (object) self.DisplayStringForFriendsGuildMember()
      }
    }));
    this.friend = friend;
  }

  public virtual IEnumerator SetUnitIcon()
  {
    FriendScrollBar friendScrollBar = this;
    IEnumerator e;
    if (Object.op_Equality((Object) friendScrollBar.unitIconPrefab, (Object) null))
    {
      Future<GameObject> unitIconPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = unitIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      friendScrollBar.unitIconPrefab = unitIconPrefabF.Result;
      unitIconPrefabF = (Future<GameObject>) null;
    }
    if (friendScrollBar.friend != null)
    {
      if (Object.op_Equality((Object) friendScrollBar.unitIcon, (Object) null))
        friendScrollBar.unitIcon = friendScrollBar.unitIconPrefab.CloneAndGetComponent<UnitIcon>(friendScrollBar.LinkCharacter);
      // ISSUE: reference to a compiler-generated method
      friendScrollBar.unitIcon.onClick = new Action<UnitIconBase>(friendScrollBar.\u003CSetUnitIcon\u003Eb__16_0);
      e = friendScrollBar.unitIcon.setSimpleUnit(friendScrollBar.friend.leader_unit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      friendScrollBar.unitIcon.Favorite = friendScrollBar.friend.is_favorite;
      friendScrollBar.unitIcon.setLevelText(friendScrollBar.friend.leader_unit);
      friendScrollBar.unitIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    }
    Future<Sprite> sprF = EmblemUtility.LoadEmblemSprite(friendScrollBar.friend.current_emblem_id);
    e = sprF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    friendScrollBar.Emblem.sprite2D = sprF.Result;
  }

  protected void SetApplication(UILabel label)
  {
    DateTime dateTime1;
    ref DateTime local = ref dateTime1;
    DateTime dateTime2 = this.friend.applied_at.Value;
    int year = dateTime2.Year;
    dateTime2 = this.friend.applied_at.Value;
    int month = dateTime2.Month;
    dateTime2 = this.friend.applied_at.Value;
    int day = dateTime2.Day;
    local = new DateTime(year, month, day);
    TimeSpan self = this.nowTime - dateTime1;
    label.SetTextLocalize(Consts.Format(Consts.GetInstance().FRIEND_0085SCROLLPARTS_DESCRIPTION01, (IDictionary) new Hashtable()
    {
      {
        (object) "dsfapplied",
        (object) self.DisplayStringForFriendsApplied().ToConverter()
      }
    }));
  }

  protected void onDetails()
  {
    if (Object.op_Inequality((Object) this.menu, (Object) null))
      this.menu.LastIndex = this.index;
    if (this.friend.application)
      Singleton<NGSceneManager>.GetInstance().changeScene("friend008_6", true, (object) this.friend.target_player_id, (object) Friend0086Scene.Mode.Accept, null);
    else
      Singleton<NGSceneManager>.GetInstance().changeScene("friend008_6", true, (object) this.friend.target_player_id, (object) Friend0086Scene.Mode.Friend, null);
  }
}
