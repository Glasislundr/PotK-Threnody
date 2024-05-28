// Decompiled with JetBrains decompiler
// Type: Guild02861Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Guild02861Menu : Guild0286Scroll
{
  private const int iconWidth = 620;
  private const int iconHeight = 175;
  private const int iconColumnValue = 1;
  private const int iconRowValue = 12;
  private const int iconScreenValue = 8;
  private const int iconMaxValue = 12;
  private const int MAX_SEND = 60;
  [SerializeField]
  private UIButton ibtnSendAll;
  private ModalWindow popup;
  [SerializeField]
  private GameObject noGiftTxt;

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public void onButtonReceive()
  {
    if (this.IsPushAndSet())
      return;
    Guild0286Scene.ChangeScene(false);
  }

  public void onButtonSendAll()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.SendAll());
  }

  private void onButtonSend(GuildMemberGift gift)
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.SendConnection(gift));
  }

  private void CheckAllSend()
  {
    if (this.memberGifts.Length == 0)
    {
      ((UIButtonColor) this.ibtnSendAll).isEnabled = false;
      this.noGiftTxt.SetActive(true);
    }
    else
    {
      ((UIButtonColor) this.ibtnSendAll).isEnabled = true;
      this.noGiftTxt.SetActive(false);
    }
  }

  public IEnumerator Init(GuildMemberGift[] gifts)
  {
    Guild02861Menu guild02861Menu = this;
    Future<GameObject> prefabF = Res.Prefabs.guild028_6_1.guild_gift_send_list.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    guild02861Menu.Setting = new ScrollAreaSetting()
    {
      iconColumnValue = 1,
      iconHeight = 175,
      iconMaxValue = 12,
      iconRowValue = 12,
      iconScreenValue = 8,
      iconWidth = 620
    };
    guild02861Menu.SetPrefab(result);
    guild02861Menu.SetInitEndAction(new Action(guild02861Menu.CheckAllSend));
    e = guild02861Menu.Init(gifts, new Action<GuildMemberGift>(guild02861Menu.onButtonSend));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator SendConnection(GuildMemberGift gift)
  {
    Guild02861Menu guild02861Menu = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.GuildGiftSendExecute> send = WebAPI.GuildGiftSendExecute(false, new string[1]
    {
      gift.player_id
    }, (Action<WebAPI.Response.UserError>) (error =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e = send.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (send.Result != null)
    {
      e = OnDemandDownload.WaitLoadHasUnitResource(false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (send.Result != null)
      {
        // ISSUE: reference to a compiler-generated method
        guild02861Menu.popup = ModalWindow.Show(Consts.GetInstance().GUILD_GIFT_SEND_TITLE, Consts.GetInstance().GUILD_GIFT_SEND_MESSAGE, new Action(guild02861Menu.\u003CSendConnection\u003Eb__16_1));
        guild02861Menu.StartCoroutine(guild02861Menu.UpdateList(send.Result.player_send));
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      }
      else
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        MypageScene.ChangeScene();
      }
    }
  }

  private IEnumerator SendAll()
  {
    Guild02861Menu guild02861Menu = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.GuildGiftSendExecute> send = WebAPI.GuildGiftSendExecute(false, ((IEnumerable<GuildMemberGift>) guild02861Menu.memberGifts).Select<GuildMemberGift, string>((Func<GuildMemberGift, string>) (x => x.player_id)).ToArray<string>(), (Action<WebAPI.Response.UserError>) (error =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e = send.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (send.Result != null)
    {
      e = OnDemandDownload.WaitLoadHasUnitResource(false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (send.Result != null)
      {
        // ISSUE: reference to a compiler-generated method
        guild02861Menu.popup = ModalWindow.Show(Consts.GetInstance().GUILD_GIFT_SEND_ALL_TITLE, Consts.GetInstance().GUILD_GIFT_SEND_ALL_MESSAGE, new Action(guild02861Menu.\u003CSendAll\u003Eb__17_2));
        guild02861Menu.StartCoroutine(guild02861Menu.UpdateList(send.Result.player_send));
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      }
      else
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        MypageScene.ChangeScene();
      }
    }
  }
}
