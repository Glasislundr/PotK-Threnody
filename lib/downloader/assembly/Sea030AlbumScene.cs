// Decompiled with JetBrains decompiler
// Type: Sea030AlbumScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Sea030AlbumScene : NGSceneBase
{
  [SerializeField]
  private Sea030AlbumMenu menu;

  public static void ChangeScene(bool isStack)
  {
    Singleton<NGGameDataManager>.GetInstance().IsSea = true;
    Singleton<NGSceneManager>.GetInstance().changeScene("sea030_album", isStack);
  }

  public IEnumerator onStartSceneAsync()
  {
    Sea030AlbumScene sea030AlbumScene = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e;
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      e = sea030AlbumScene.SetSeaBackground();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    SeaHomeMap seaHomeMap = ((IEnumerable<SeaHomeMap>) MasterData.SeaHomeMapList).ActiveSeaHomeMap(ServerTime.NowAppTimeAddDelta());
    if (seaHomeMap != null && !string.IsNullOrEmpty(seaHomeMap.bgm_cuesheet_name) && !string.IsNullOrEmpty(seaHomeMap.bgm_cue_name))
    {
      sea030AlbumScene.bgmFile = seaHomeMap.bgm_cuesheet_name;
      sea030AlbumScene.bgmName = seaHomeMap.bgm_cue_name;
    }
    e = sea030AlbumScene.menu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene()
  {
    if (!this.menu.IsSuccess)
      return;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  private IEnumerator SetSeaBackground()
  {
    Sea030AlbumScene sea030AlbumScene = this;
    Future<GameObject> bgF = new ResourceObject("Prefabs/BackGround/DefaultBackground_sea").Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) bgF.Result, (Object) null))
      sea030AlbumScene.backgroundPrefab = bgF.Result;
  }

  public override IEnumerator onEndSceneAsync()
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) base.onEndSceneAsync();
  }
}
