// Decompiled with JetBrains decompiler
// Type: PopupReisouChangeTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class PopupReisouChangeTarget : BackButtonPopupBase
{
  [SerializeField]
  protected GameObject dirItemIcon;
  private PlayerItem beforeWeapon;
  private GameCore.ItemInfo reisou;
  private GameCore.ItemInfo currWeapon;
  private Action removeCallback;

  public IEnumerator Init(
    PlayerItem beforeWeapon,
    GameCore.ItemInfo reisou,
    GameCore.ItemInfo currWeapon,
    Action removeCallback = null)
  {
    PopupReisouChangeTarget reisouChangeTarget = this;
    reisouChangeTarget.beforeWeapon = beforeWeapon;
    reisouChangeTarget.reisou = reisou;
    reisouChangeTarget.currWeapon = currWeapon;
    reisouChangeTarget.removeCallback = removeCallback;
    Future<GameObject> prefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ItemIcon itemIcon = prefabF.Result.Clone(reisouChangeTarget.dirItemIcon.transform).GetComponent<ItemIcon>();
    e = itemIcon.InitByPlayerItem(reisouChangeTarget.beforeWeapon);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    itemIcon.EnableLongPressEvent(new Action<GameCore.ItemInfo>(reisouChangeTarget.onLongPressItemIcon));
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
    if (this.removeCallback != null)
      this.removeCallback();
    else
      this.StartCoroutine(this.callEquipReisouAPI());
  }

  private IEnumerator callEquipReisouAPI()
  {
    PopupReisouChangeTarget reisouChangeTarget = this;
    Singleton<PopupManager>.GetInstance().closeAll(true);
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e = WebAPI.ItemGearReisouEquip(reisouChangeTarget.beforeWeapon.id, new int?(0), (Action<WebAPI.Response.UserError>) (error =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      if (error == null)
        return;
      WebAPI.DefaultUserErrorCallback(error);
    })).Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = WebAPI.ItemGearReisouEquip(reisouChangeTarget.currWeapon.itemID, new int?(reisouChangeTarget.reisou.itemID), (Action<WebAPI.Response.UserError>) (error =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      if (error == null)
        return;
      WebAPI.DefaultUserErrorCallback(error);
    })).Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    switch (GuildUtil.gvgPopupState)
    {
      case GuildUtil.GvGPopupState.None:
label_17:
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        reisouChangeTarget.backScene();
        yield break;
      case GuildUtil.GvGPopupState.AtkTeam:
        // ISSUE: reference to a compiler-generated method
        e = GuildUtil.UpdateGuildDeckAttack(PlayerAffiliation.Current.guild_id, Player.Current.id, new Action(reisouChangeTarget.\u003CcallEquipReisouAPI\u003Eb__10_2));
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      case GuildUtil.GvGPopupState.DefTeam:
        // ISSUE: reference to a compiler-generated method
        e = GuildUtil.UpdateGuildDeckDefanse(PlayerAffiliation.Current.guild_id, Player.Current.id, new Action(reisouChangeTarget.\u003CcallEquipReisouAPI\u003Eb__10_3));
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      default:
        reisouChangeTarget.backScene();
        break;
    }
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    goto label_17;
  }
}
