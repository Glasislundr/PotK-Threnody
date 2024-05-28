// Decompiled with JetBrains decompiler
// Type: GuildTownTopEnemyPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GuildTownTopEnemyPopup : BackButtonMonoBehaiviour
{
  private Guild0282Menu guildMapMenu;
  private Guild0282MemberBaseMenu memberBaseMenu;
  [SerializeField]
  private GameObject MapContainer;
  [SerializeField]
  private UILabel lblMapName;
  private GameObject mapChipPrefab;
  private bool isPush;

  private IEnumerator ResourceLoad()
  {
    if (Object.op_Equality((Object) this.mapChipPrefab, (Object) null))
    {
      Future<GameObject> f = new ResourceObject("Prefabs/guild_town/MapChipContainer").Load<GameObject>();
      IEnumerator e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mapChipPrefab = f.Result;
      f = (Future<GameObject>) null;
    }
  }

  public IEnumerator InitializeAsync(
    Guild0282Menu menu,
    Guild0282MemberBaseMenu memberMenu,
    WebAPI.Response.GuildtownShow guildTownShow)
  {
    this.guildMapMenu = menu;
    this.memberBaseMenu = memberMenu;
    IEnumerator e = this.ResourceLoad();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GuildTownMapScroll component = this.mapChipPrefab.Clone(this.MapContainer.transform).GetComponent<GuildTownMapScroll>();
    List<Tuple<int, int>> positionList = new List<Tuple<int, int>>();
    for (int index = 0; index < guildTownShow.guild_town_slots[guildTownShow.default_town_slot_number].facilities_data.Length; ++index)
    {
      MapFacility mapFacility;
      if (MasterData.MapFacility.TryGetValue(guildTownShow.guild_town_slots[guildTownShow.default_town_slot_number].facilities_data[index].master_id, out mapFacility) && mapFacility.category_id != 4)
        positionList.Add(new Tuple<int, int>(guildTownShow.guild_town_slots[guildTownShow.default_town_slot_number].facilities_data[index].x, guildTownShow.guild_town_slots[guildTownShow.default_town_slot_number].facilities_data[index].y));
    }
    e = component.InitializeAsync(guildTownShow.guild_town_slots[guildTownShow.default_town_slot_number].master.stage_id, false, -1, positionList, this.memberBaseMenu);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.lblMapName.SetTextLocalize(guildTownShow.guild_town_slots[guildTownShow.default_town_slot_number].master.name);
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }

  public override void onBackButton()
  {
    if (this.isPush)
      return;
    this.isPush = true;
    this.guildMapMenu.closePopup();
  }
}
