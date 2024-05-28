// Decompiled with JetBrains decompiler
// Type: GuildMapStoragePopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class GuildMapStoragePopup : BackButtonMonoBehaiviour
{
  [SerializeField]
  private UIScrollView scrollView;
  [SerializeField]
  private UIGrid grid;
  [SerializeField]
  private GameObject slc_Listbase_None;
  [SerializeField]
  private UILabel lblPossession;
  private Guild0282Menu guildMenu;
  private GameObject mapScrollPrefab;
  private GameObject mapDetailPopupPrefab;
  private bool isPush;

  private IEnumerator ResourceLoad()
  {
    Future<GameObject> f;
    IEnumerator e;
    if (Object.op_Equality((Object) this.mapScrollPrefab, (Object) null))
    {
      f = new ResourceObject("Prefabs/map_edit031/dir_base_map_list").Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mapScrollPrefab = f.Result;
      f = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.mapDetailPopupPrefab, (Object) null))
    {
      f = new ResourceObject("Prefabs/popup/popup_031_map_detail__anim_popup01").Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mapDetailPopupPrefab = f.Result;
      f = (Future<GameObject>) null;
    }
  }

  private IEnumerator ShowMapDetailPopup(PlayerGuildTown guildTown)
  {
    GuildMapStoragePopup guildMapStoragePopup = this;
    GameObject popup = guildMapStoragePopup.mapDetailPopupPrefab.Clone();
    popup.SetActive(false);
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = popup.GetComponent<PopupMapDetailMenu>().InitializeAsync(guildTown._master, new Action(guildMapStoragePopup.\u003CShowMapDetailPopup\u003Eb__9_0));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
  }

  private bool isPushAndSet()
  {
    if (this.isPush)
      return true;
    this.isPush = true;
    return false;
  }

  public IEnumerator InitializeAsync(Guild0282Menu menu, PlayerGuildTown[] towns)
  {
    GuildMapStoragePopup guildMapStoragePopup = this;
    guildMapStoragePopup.guildMenu = menu;
    IEnumerator e = guildMapStoragePopup.ResourceLoad();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (towns == null || towns.Length == 0)
    {
      guildMapStoragePopup.slc_Listbase_None.SetActive(true);
    }
    else
    {
      guildMapStoragePopup.lblPossession.SetTextLocalize(towns.Length);
      guildMapStoragePopup.slc_Listbase_None.SetActive(false);
      for (int i = 0; i < towns.Length; ++i)
      {
        // ISSUE: reference to a compiler-generated method
        e = guildMapStoragePopup.mapScrollPrefab.Clone(((Component) guildMapStoragePopup.grid).transform).GetComponent<GuildMapStorageScroll>().InitializeAsync(towns[i], new Action<PlayerGuildTown>(guildMapStoragePopup.\u003CInitializeAsync\u003Eb__11_0));
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      guildMapStoragePopup.grid.Reposition();
    }
  }

  public override void onBackButton()
  {
    if (this.isPushAndSet())
      return;
    this.guildMenu.closePopup();
  }
}
