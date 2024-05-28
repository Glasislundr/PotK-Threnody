// Decompiled with JetBrains decompiler
// Type: GuildMapStorageScroll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class GuildMapStorageScroll : MonoBehaviour
{
  [SerializeField]
  private UILabel lblMapName;
  [SerializeField]
  private UILabel lblCost;
  [SerializeField]
  private UILabel lblDescription;
  [SerializeField]
  private GameObject dyn_map_thumb_container;
  private Action<PlayerGuildTown> actionSelect;
  private PlayerGuildTown guildTown;
  private bool isPush;

  private bool isPushAndSet()
  {
    if (this.isPush)
      return true;
    this.isPush = true;
    return false;
  }

  public IEnumerator InitializeAsync(
    PlayerGuildTown guildTown,
    Action<PlayerGuildTown> actionSelect)
  {
    this.lblMapName.SetTextLocalize(guildTown.master.name);
    this.lblCost.SetTextLocalize(guildTown.master.cost_capacity);
    this.lblDescription.SetTextLocalize(guildTown.master.description);
    CreateIconObject orAddComponent = this.dyn_map_thumb_container.GetOrAddComponent<CreateIconObject>();
    if (Object.op_Inequality((Object) orAddComponent, (Object) null))
    {
      IEnumerator e = orAddComponent.CreateThumbnail(MasterDataTable.CommonRewardType.guild_town, guildTown.master.ID, visibleBottom: false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.actionSelect = actionSelect;
    this.guildTown = guildTown;
  }

  public void onThumbnailButton()
  {
    if (this.isPushAndSet() || this.actionSelect == null)
      return;
    this.actionSelect(this.guildTown);
    this.isPush = false;
  }
}
