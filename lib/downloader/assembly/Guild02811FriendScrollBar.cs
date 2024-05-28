// Decompiled with JetBrains decompiler
// Type: Guild02811FriendScrollBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Guild02811FriendScrollBar : FriendScrollBar
{
  private GuildDirectory guild;
  private Guild02811Menu menu2811;
  private Guild02812Menu menu2812;
  private GuildUtil.MenuType aMenu;

  public GuildDirectory Guild
  {
    get => this.guild;
    set => this.guild = value;
  }

  public void SetMenu(Guild02811Menu menu)
  {
    this.aMenu = GuildUtil.MenuType.menu2811;
    this.menu2811 = menu;
  }

  public void SetMenu(Guild02812Menu menu)
  {
    this.aMenu = GuildUtil.MenuType.menu2812;
    this.menu2812 = menu;
  }

  public void onButtonGuildAbout()
  {
    switch (this.aMenu)
    {
      case GuildUtil.MenuType.menu2811:
        GameObject prefab1 = this.menu2811.GuildPopup.guildInfoPopup.Clone();
        prefab1.SetActive(false);
        prefab1.GetComponent<Guild028114Popup>().Initialize(this.Guild, this.menu2811.GuildPopup);
        prefab1.SetActive(true);
        Singleton<PopupManager>.GetInstance().open(prefab1, isCloned: true);
        break;
      case GuildUtil.MenuType.menu2812:
        GameObject prefab2 = this.menu2812.GuildPopup.guildInfoPopup.Clone();
        prefab2.SetActive(false);
        prefab2.GetComponent<Guild028114Popup>().Initialize(this.Guild, this.menu2812.GuildPopup, true);
        prefab2.SetActive(true);
        Singleton<PopupManager>.GetInstance().open(prefab2, isCloned: true);
        break;
    }
  }

  public override IEnumerator SetUnitIcon()
  {
    Guild02811FriendScrollBar guild02811FriendScrollBar = this;
    IEnumerator e;
    if (Object.op_Equality((Object) guild02811FriendScrollBar.unitIconPrefab, (Object) null))
    {
      Future<GameObject> unitIconPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = unitIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      guild02811FriendScrollBar.unitIconPrefab = unitIconPrefabF.Result;
      unitIconPrefabF = (Future<GameObject>) null;
    }
    if (guild02811FriendScrollBar.Friend != null)
    {
      if (Object.op_Equality((Object) guild02811FriendScrollBar.unitIcon, (Object) null))
        guild02811FriendScrollBar.unitIcon = guild02811FriendScrollBar.unitIconPrefab.CloneAndGetComponent<UnitIcon>(guild02811FriendScrollBar.LinkCharacter);
      e = guild02811FriendScrollBar.unitIcon.setSimpleUnit(guild02811FriendScrollBar.Friend.leader_unit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      guild02811FriendScrollBar.unitIcon.Favorite = guild02811FriendScrollBar.Friend.is_favorite;
      guild02811FriendScrollBar.unitIcon.setLevelText(guild02811FriendScrollBar.Friend.leader_unit);
      guild02811FriendScrollBar.unitIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    }
    Future<Sprite> sprF = EmblemUtility.LoadEmblemSprite(guild02811FriendScrollBar.Friend.current_emblem_id);
    e = sprF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    guild02811FriendScrollBar.Emblem.sprite2D = sprF.Result;
  }
}
