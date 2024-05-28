// Decompiled with JetBrains decompiler
// Type: Sea030GiftListScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using System.Collections.Generic;

#nullable disable
public class Sea030GiftListScene : NGSceneBase
{
  public Sea030GiftListMenu menu;

  public override IEnumerator onInitSceneAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Sea030GiftListScene sea030GiftListScene = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) sea030GiftListScene.StartCoroutine(sea030GiftListScene.PlayerItemCallGift());
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public static void ChangeScene(bool stack, GearGear gear)
  {
    Sea030GiftListMenu.initPopupGear = gear;
    Singleton<NGSceneManager>.GetInstance().changeScene("sea030_giftList", stack);
  }

  public IEnumerator onStartSceneAsync()
  {
    Sea030GiftListScene sea030GiftListScene = this;
    sea030GiftListScene.headerType = CommonRoot.HeaderType.Sea;
    IEnumerator e = sea030GiftListScene.SetupMatchedBackground("HimeTalkBackground");
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    SeaHomeMap seaHomeMap = ((IEnumerable<SeaHomeMap>) MasterData.SeaHomeMapList).ActiveSeaHomeMap(ServerTime.NowAppTimeAddDelta());
    if (!string.IsNullOrEmpty(seaHomeMap.bgm_cuesheet_name) && !string.IsNullOrEmpty(seaHomeMap.bgm_cue_name))
    {
      sea030GiftListScene.bgmName = seaHomeMap.bgm_cue_name;
      sea030GiftListScene.bgmFile = seaHomeMap.bgm_cuesheet_name;
    }
    sea030GiftListScene.PlayBGM();
    e = sea030GiftListScene.menu.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Sea030GiftListMenu.initPopupGear != null)
      sea030GiftListScene.StartCoroutine(sea030GiftListScene.menu.OpenInitPopup());
  }

  private IEnumerator PlayerItemCallGift()
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    yield return (object) null;
    Future<WebAPI.Response.ItemGearCallGift> handler = WebAPI.ItemGearCallGift();
    IEnumerator e = handler.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    WebAPI.Response.ItemGearCallGift result = handler.Result;
    if (result == null)
    {
      Singleton<PopupManager>.GetInstance().closeAll();
      this.menu.IbtnBack();
      yield return (object) null;
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    }
    else
    {
      Sea030GiftListMenu.playerRecipeIDList = result.recipe_id_list;
      yield return (object) null;
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    }
  }

  public override void onSceneInitialized()
  {
    Singleton<CommonRoot>.GetInstance().isActiveHeader = true;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = false;
  }

  public virtual void onBackScene()
  {
    Singleton<CommonRoot>.GetInstance().isActiveHeader = false;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = false;
    this.menu.onBackScene();
  }

  public override void onEndScene()
  {
    Singleton<CommonRoot>.GetInstance().isActiveHeader = false;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = false;
    this.menu.onEndScene();
  }
}
