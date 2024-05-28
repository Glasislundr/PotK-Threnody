// Decompiled with JetBrains decompiler
// Type: SeaTalkMessageScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class SeaTalkMessageScene : NGSceneBase
{
  [SerializeField]
  private SeaTalkMessageMenu menu;
  private bool isInit;

  public static void ChangeScene(TalkUnitInfo talkUnitInfo, Sea030HomeMenu homeMenu = null)
  {
    Singleton<CommonRoot>.GetInstance().isActiveHeader = false;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = false;
    Singleton<NGSceneManager>.GetInstance().changeScene("sea030_talk_himeTalk", true, (object) talkUnitInfo, (object) homeMenu);
  }

  public override IEnumerator onInitSceneAsync()
  {
    SeaTalkMessageScene talkMessageScene = this;
    SeaHomeMap seaHomeMap = ((IEnumerable<SeaHomeMap>) MasterData.SeaHomeMapList).ActiveSeaHomeMap(ServerTime.NowAppTimeAddDelta());
    if (seaHomeMap != null && !string.IsNullOrEmpty(seaHomeMap.bgm_cuesheet_name) && !string.IsNullOrEmpty(seaHomeMap.bgm_cue_name))
    {
      talkMessageScene.bgmFile = seaHomeMap.bgm_cuesheet_name;
      talkMessageScene.bgmName = seaHomeMap.bgm_cue_name;
    }
    yield return (object) talkMessageScene.menu.onInitSceneAsync();
  }

  public IEnumerator onStartSceneAsync(TalkUnitInfo talkUnitInfo, Sea030HomeMenu homeMenu)
  {
    SeaTalkMessageScene talkMessageScene = this;
    Singleton<CommonRoot>.GetInstance().isActiveHeader = false;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = false;
    IEnumerator e1 = talkMessageScene.SetupMatchedBackground("HimeTalkBackground");
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (talkMessageScene.isInit)
    {
      yield return (object) talkMessageScene.menu.onBackSceneUpdateGift();
      ((UIRect) ((Component) talkMessageScene).GetComponent<UIPanel>()).alpha = 1f;
      yield return (object) talkMessageScene.menu.AddMessageView();
    }
    else
    {
      if (Singleton<NGSceneManager>.GetInstance().isMatchSceneNameInStack("sea030_talk_himeList"))
      {
        Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
      }
      else
      {
        Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0, true);
        yield return (object) null;
        Singleton<CommonRoot>.GetInstance().isActiveBackground = true;
      }
      SeaTalkPartnerMenu[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll<SeaTalkPartnerMenu>();
      WebAPI.Response.SeaTalkPartner talkPartnerResponse;
      if (objectsOfTypeAll.Length == 0)
      {
        Future<WebAPI.Response.SeaTalkPartner> api = WebAPI.SeaTalkPartner((Action<WebAPI.Response.UserError>) (e =>
        {
          WebAPI.DefaultUserErrorCallback(e);
          MypageScene.ChangeSceneOnError();
        }));
        e1 = api.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        talkPartnerResponse = api.Result;
        api = (Future<WebAPI.Response.SeaTalkPartner>) null;
      }
      else
        talkPartnerResponse = objectsOfTypeAll[0].response;
      PlayerTalkPartner playerTalkPartner = ((IEnumerable<PlayerTalkPartner>) talkPartnerResponse.partners).First<PlayerTalkPartner>((Func<PlayerTalkPartner, bool>) (x => x.letter.same_character_id == talkUnitInfo.unit.same_character_id));
      if (playerTalkPartner.unread_count > 0)
      {
        playerTalkPartner.unread_count = 0;
        SeaTalkCommon.UpdateTalkBatch(talkPartnerResponse.partners);
        SeaTalkMessageMenu.SeaTalkPartnerRefresh();
      }
      Future<WebAPI.Response.SeaTalkMessage> seaTalkMessageApi = WebAPI.SeaTalkMessage(talkUnitInfo.unit.same_character_id, (Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      e1 = seaTalkMessageApi.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      WebAPI.Response.SeaTalkMessage result = seaTalkMessageApi.Result;
      result.messages = ((IEnumerable<PlayerTalkMessage>) result.messages).OrderBy<PlayerTalkMessage, int>((Func<PlayerTalkMessage, int>) (x => x.player_message_id)).ToArray<PlayerTalkMessage>();
      yield return (object) talkMessageScene.menu.onStartSceneAsync(talkUnitInfo, result, talkPartnerResponse, homeMenu);
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      yield return (object) talkMessageScene.menu.Conclusion();
      talkMessageScene.isInit = true;
    }
  }

  public override void onSceneInitialized()
  {
    Singleton<CommonRoot>.GetInstance().isActiveHeader = false;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = false;
  }
}
