// Decompiled with JetBrains decompiler
// Type: BattleUI05AlbumCompleteMenu
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
public class BattleUI05AlbumCompleteMenu : ResultMenuBase
{
  private GameObject popupObject;
  private GameObject prefab;
  private List<int> albumIds = new List<int>();

  public override IEnumerator Init(BattleInfo info, BattleEnd result)
  {
    this.albumIds = ((IEnumerable<int>) result.receive_sea_album_ids).ToList<int>();
    IEnumerator e = this.LoadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override IEnumerator Run()
  {
    BattleUI05AlbumCompleteMenu albumCompleteMenu = this;
    foreach (int albumId1 in albumCompleteMenu.albumIds)
    {
      int albumId = albumId1;
      albumCompleteMenu.popupObject.SetParent(((Component) albumCompleteMenu).gameObject);
      albumCompleteMenu.popupObject.SetActive(true);
      MasterDataTable.SeaAlbum album = ((IEnumerable<MasterDataTable.SeaAlbum>) MasterData.SeaAlbumList).FirstOrDefault<MasterDataTable.SeaAlbum>((Func<MasterDataTable.SeaAlbum, bool>) (x => x.ID == albumId));
      if (album != null)
      {
        List<SeaAlbumRewardGroup> list = ((IEnumerable<SeaAlbumRewardGroup>) MasterData.SeaAlbumRewardGroupList).Where<SeaAlbumRewardGroup>((Func<SeaAlbumRewardGroup, bool>) (x => x.reward_group_id == album.complete_reward_group_id)).ToList<SeaAlbumRewardGroup>();
        if (list != null)
        {
          IEnumerator e = albumCompleteMenu.popupObject.GetComponent<BattleUI05AlbumCompleteSetting>().Init(album, list);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          Singleton<NGSoundManager>.GetInstance().playSE("SE_1034");
          e = albumCompleteMenu.SetColorBackground();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          bool toNext = false;
          GameObject touchObj = albumCompleteMenu.CreateTouchObject((EventDelegate.Callback) (() => toNext = true));
          if (Singleton<NGGameDataManager>.GetInstance().questAutoLap)
          {
            yield return (object) new WaitForSeconds(3f);
            toNext = true;
          }
          while (!toNext)
            yield return (object) null;
          Object.DestroyObject((Object) albumCompleteMenu.popupObject);
          Object.DestroyObject((Object) touchObj);
          albumCompleteMenu.ClonePopup();
          touchObj = (GameObject) null;
        }
      }
    }
  }

  private IEnumerator LoadResources()
  {
    Future<GameObject> handler = Res.Animations.album.AlbumComplete.Load<GameObject>();
    IEnumerator e = handler.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.prefab = handler.Result;
    this.ClonePopup();
  }

  private void ClonePopup()
  {
    this.popupObject = this.prefab.Clone(Singleton<CommonRoot>.GetInstance().LoadTmpObj.transform);
    this.popupObject.SetActive(false);
  }

  private IEnumerator SetColorBackground()
  {
    BattleUI05AlbumCompleteMenu albumCompleteMenu = this;
    Future<Sprite> textureLoader = Res.Prefabs.BackGround.black.Load<Sprite>();
    IEnumerator e = textureLoader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Resolution currentResolution = Screen.currentResolution;
    GameObject gameObject = new GameObject("Color Layer");
    gameObject.transform.parent = albumCompleteMenu.popupObject.transform;
    gameObject.layer = ((Component) albumCompleteMenu).gameObject.layer;
    UIPanel uiPanel = gameObject.AddComponent<UIPanel>();
    UI2DSprite ui2Dsprite = gameObject.AddComponent<UI2DSprite>();
    uiPanel.depth = 20;
    ui2Dsprite.sprite2D = textureLoader.Result;
    ((UIRect) ui2Dsprite).alpha = 0.75f;
    ((UIWidget) ui2Dsprite).height = ((Resolution) ref currentResolution).height;
    ((UIWidget) ui2Dsprite).width = ((Resolution) ref currentResolution).width;
  }
}
