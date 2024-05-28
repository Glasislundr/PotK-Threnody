// Decompiled with JetBrains decompiler
// Type: SeaTalkPartnerScene
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
public class SeaTalkPartnerScene : NGSceneBase
{
  [SerializeField]
  private SeaTalkPartnerMenu menu;
  private bool isInit;

  public bool IsInit
  {
    set => this.isInit = value;
  }

  public static void ChangeScene(Sea030HomeMenu seaHomeMenu)
  {
    Singleton<CommonRoot>.GetInstance().isActiveHeader = false;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = false;
    Singleton<NGSceneManager>.GetInstance().changeScene("sea030_talk_himeList", true, (object) seaHomeMenu);
  }

  public override IEnumerator onInitSceneAsync()
  {
    SeaTalkPartnerScene talkPartnerScene = this;
    SeaHomeMap seaHomeMap = ((IEnumerable<SeaHomeMap>) MasterData.SeaHomeMapList).ActiveSeaHomeMap(ServerTime.NowAppTimeAddDelta());
    if (seaHomeMap != null && !string.IsNullOrEmpty(seaHomeMap.bgm_cuesheet_name) && !string.IsNullOrEmpty(seaHomeMap.bgm_cue_name))
    {
      talkPartnerScene.bgmFile = seaHomeMap.bgm_cuesheet_name;
      talkPartnerScene.bgmName = seaHomeMap.bgm_cue_name;
    }
    yield return (object) talkPartnerScene.menu.onInitSceneAsync();
  }

  public IEnumerator onStartSceneAsync(Sea030HomeMenu seaHomeMenu)
  {
    SeaTalkPartnerScene talkPartnerScene = this;
    Singleton<CommonRoot>.GetInstance().isActiveHeader = false;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = false;
    IEnumerator e1 = talkPartnerScene.SetupMatchedBackground("HimeTalkBackground");
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (!talkPartnerScene.isInit)
    {
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
      Future<WebAPI.Response.SeaTalkPartner> api = WebAPI.SeaTalkPartner((Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      e1 = api.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      WebAPI.Response.SeaTalkPartner result = api.Result;
      yield return (object) talkPartnerScene.menu.onStartSceneAsync(result, seaHomeMenu);
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      talkPartnerScene.isInit = true;
    }
  }

  public override void onSceneInitialized()
  {
    Singleton<CommonRoot>.GetInstance().isActiveHeader = false;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = false;
  }
}
