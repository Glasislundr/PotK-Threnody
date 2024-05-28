// Decompiled with JetBrains decompiler
// Type: PopupReisouRemove
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class PopupReisouRemove : BackButtonPopupBase
{
  [SerializeField]
  protected GameObject dirItemIcon;
  protected PlayerItem item;
  protected Action removeCallback;

  public IEnumerator Init(PlayerItem item, Action removeCallback = null)
  {
    PopupReisouRemove popupReisouRemove = this;
    popupReisouRemove.item = item;
    popupReisouRemove.removeCallback = removeCallback;
    Future<GameObject> prefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ItemIcon itemIcon = prefabF.Result.Clone(popupReisouRemove.dirItemIcon.transform).GetComponent<ItemIcon>();
    e = itemIcon.InitByPlayerItem(item);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    itemIcon.EnableLongPressEvent(new Action<GameCore.ItemInfo>(popupReisouRemove.onLongPressItemIcon));
  }

  public void onLongPressItemIcon(GameCore.ItemInfo item)
  {
    Unit00443Scene.changeSceneLimited(true, item);
    Singleton<PopupManager>.GetInstance().closeAll();
  }

  public override void onBackButton() => this.onClickedClose();

  public void onClickedClose() => Singleton<PopupManager>.GetInstance().dismiss();

  public void onBtnRemove()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.callEquipReisouAPI());
  }

  private IEnumerator callEquipReisouAPI()
  {
    PopupReisouRemove popupReisouRemove = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    int num = 0;
    IEnumerator e = WebAPI.ItemGearReisouEquip(popupReisouRemove.item.id, new int?(num), (Action<WebAPI.Response.UserError>) (error =>
    {
      if (error == null)
        return;
      WebAPI.DefaultUserErrorCallback(error);
    })).Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    switch (GuildUtil.gvgPopupState)
    {
      case GuildUtil.GvGPopupState.AtkTeam:
        // ISSUE: reference to a compiler-generated method
        e = GuildUtil.UpdateGuildDeckAttack(PlayerAffiliation.Current.guild_id, Player.Current.id, new Action(popupReisouRemove.\u003CcallEquipReisouAPI\u003Eb__8_1));
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      case GuildUtil.GvGPopupState.DefTeam:
        // ISSUE: reference to a compiler-generated method
        e = GuildUtil.UpdateGuildDeckDefanse(PlayerAffiliation.Current.guild_id, Player.Current.id, new Action(popupReisouRemove.\u003CcallEquipReisouAPI\u003Eb__8_2));
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
    }
    Action removeCallback = popupReisouRemove.removeCallback;
    if (removeCallback != null)
      removeCallback();
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    Singleton<PopupManager>.GetInstance().closeAll();
  }
}
