// Decompiled with JetBrains decompiler
// Type: Sea030AlbumMenu
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
public class Sea030AlbumMenu : BackButtonMenuBase
{
  [SerializeField]
  private UIButton BtnReward;
  [SerializeField]
  private UIButton BtnZoom;
  [SerializeField]
  private UILabel TxtIllustName;
  [SerializeField]
  private UILabel TxtProgress;
  [SerializeField]
  private UIPanel mainPanel;
  [SerializeField]
  private NGHorizontalScrollParts scrollParts;
  [SerializeField]
  private UIScrollView scroll;
  [SerializeField]
  private UIGrid grid;
  [SerializeField]
  private GameObject btnRight;
  [SerializeField]
  private GameObject btnLeft;
  private bool isSuccess;
  private int currentAlbumId;
  private bool isInited;
  private PlayerAlbum[] playerAlbums;
  private PlayerAlbumPiece[] playerAlbumPeices;
  private List<MasterDataTable.SeaAlbum> albumList = new List<MasterDataTable.SeaAlbum>();
  private List<SeaAlbumPiece> albumPieceList = new List<SeaAlbumPiece>();
  private Dictionary<int, Sprite> illustDictionary = new Dictionary<int, Sprite>();
  private GameObject detailPopup;
  private GameObject rewardPopup;
  private GameObject AlbumPrefab;
  private List<SeaAlbum> seaAlbumList = new List<SeaAlbum>();

  public bool IsSuccess => this.isSuccess;

  public IEnumerator Initialize()
  {
    if (!this.isInited)
    {
      IEnumerator e = this.SetData();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (this.isSuccess)
      {
        e = this.LoadResources();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        PlayerAlbum[] playerAlbumArray = this.playerAlbums;
        for (int index = 0; index < playerAlbumArray.Length; ++index)
        {
          PlayerAlbum target = playerAlbumArray[index];
          MasterDataTable.SeaAlbum album = this.albumList.FirstOrDefault<MasterDataTable.SeaAlbum>((Func<MasterDataTable.SeaAlbum, bool>) (x => x.ID == target.album_id));
          if (album != null)
          {
            this.LoadIllustration(album);
            e = this.LoadIllustration(album);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
        }
        playerAlbumArray = (PlayerAlbum[]) null;
        this.SetAlbumList();
        this.UpdateInfo();
        this.isInited = true;
      }
    }
  }

  private void SetAlbumList()
  {
    for (int index = 0; index < this.playerAlbums.Length; ++index)
    {
      PlayerAlbum playerAlbum = this.playerAlbums[index];
      SeaAlbum component = ((Component) this.scrollParts.instantiateParts(this.AlbumPrefab, false).GetComponent<SeaAlbum>()).GetComponent<SeaAlbum>();
      int albumId = playerAlbum.album_id;
      component.Init(this.illustDictionary[albumId], playerAlbum, index);
      this.seaAlbumList.Add(component);
    }
    this.scroll.ResetPosition();
    this.scrollParts.resetScrollView();
  }

  private IEnumerator SetData()
  {
    Sea030AlbumMenu sea030AlbumMenu = this;
    sea030AlbumMenu.isSuccess = false;
    // ISSUE: reference to a compiler-generated method
    Future<WebAPI.Response.SeaAlbumIndex> f = WebAPI.SeaAlbumIndex(new Action<WebAPI.Response.UserError>(sea030AlbumMenu.\u003CSetData\u003Eb__26_0));
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (f.Result != null)
    {
      sea030AlbumMenu.isSuccess = true;
      sea030AlbumMenu.playerAlbums = f.Result.player_album;
      sea030AlbumMenu.albumList = ((IEnumerable<MasterDataTable.SeaAlbum>) MasterData.SeaAlbumList).ToList<MasterDataTable.SeaAlbum>();
      sea030AlbumMenu.albumPieceList = ((IEnumerable<SeaAlbumPiece>) MasterData.SeaAlbumPieceList).ToList<SeaAlbumPiece>();
    }
  }

  private IEnumerator LoadResources()
  {
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_030_sea_detail__anim_fade.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.detailPopup = prefabF.Result;
    prefabF = Res.Prefabs.popup.popup_030_sea_album_reward_confirmation__anim_fade.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.rewardPopup = prefabF.Result;
    prefabF = Res.Prefabs.sea030_album.dir_sea_album.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.AlbumPrefab = prefabF.Result;
  }

  private IEnumerator LoadIllustration(MasterDataTable.SeaAlbum album)
  {
    Future<Sprite> imageF = Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("Album/{0}/slc_album_s", (object) album.ID));
    IEnumerator e = imageF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = imageF.Result;
    if (!Object.op_Equality((Object) result, (Object) null))
      this.illustDictionary.Add(album.ID, result);
  }

  private void UpdateInfo(int index = 0)
  {
    this.currentAlbumId = index;
    PlayerAlbum playerAlbum = this.playerAlbums[index];
    MasterDataTable.SeaAlbum album = this.albumList.FirstOrDefault<MasterDataTable.SeaAlbum>((Func<MasterDataTable.SeaAlbum, bool>) (x => x.ID == playerAlbum.album_id));
    if (album == null)
      return;
    List<SeaAlbumPiece> list = this.albumPieceList.Where<SeaAlbumPiece>((Func<SeaAlbumPiece, bool>) (x => x.album_id == album.ID)).ToList<SeaAlbumPiece>();
    this.TxtIllustName.SetTextLocalize(album.name);
    this.TxtProgress.SetTextLocalize(((IEnumerable<PlayerAlbumPiece>) playerAlbum.player_album_piece).Where<PlayerAlbumPiece>((Func<PlayerAlbumPiece, bool>) (x => x.is_open)).Count<PlayerAlbumPiece>().ToString() + "/" + (object) list.Count<SeaAlbumPiece>());
    this.SetBtnZoom(playerAlbum);
    this.btnRight.SetActive(index != this.playerAlbums.Length - 1);
    this.btnLeft.SetActive(index != 0);
  }

  private void SetBtnZoom(PlayerAlbum playerAlbum)
  {
    ((UIButtonColor) this.BtnZoom).isEnabled = !((IEnumerable<PlayerAlbumPiece>) playerAlbum.player_album_piece).Any<PlayerAlbumPiece>((Func<PlayerAlbumPiece, bool>) (x => !x.is_open));
  }

  public void OnBtnChangeIllustration()
  {
    Sea030AlbumIllustrationScene.ChangeScene(this.mainPanel.width, this.playerAlbums[this.currentAlbumId].album_id, true);
  }

  public void OnBtnRewardPopup()
  {
    if (this.scroll.isDragging)
      return;
    PlayerAlbum playerAlbum = this.playerAlbums[this.currentAlbumId];
    MasterDataTable.SeaAlbum seaAlbum = this.albumList.FirstOrDefault<MasterDataTable.SeaAlbum>((Func<MasterDataTable.SeaAlbum, bool>) (x => x.ID == playerAlbum.album_id));
    if (seaAlbum == null || Object.op_Equality((Object) this.rewardPopup, (Object) null) || this.IsPushAndSet())
      return;
    this.StartCoroutine(this.OpenRewardPopup(seaAlbum.complete_reward_group_id));
  }

  private IEnumerator OpenRewardPopup(int reward_group_id)
  {
    Sea030AlbumMenu sea030AlbumMenu = this;
    GameObject popup = sea030AlbumMenu.rewardPopup.Clone();
    popup.SetActive(false);
    IEnumerator e = popup.GetComponent<Popup030SeaAlbumRewardConfirmationMenu>().Init(reward_group_id, new Action(sea030AlbumMenu.PushReset));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    yield return (object) null;
  }

  private void PushReset() => this.IsPush = false;

  public void OnBtnDetail()
  {
    if (this.scroll.isDragging)
      return;
    PlayerAlbum playerAlbum = this.playerAlbums[this.currentAlbumId];
    MasterDataTable.SeaAlbum album = this.albumList.FirstOrDefault<MasterDataTable.SeaAlbum>((Func<MasterDataTable.SeaAlbum, bool>) (x => x.ID == playerAlbum.album_id));
    if (album == null)
      return;
    List<SeaAlbumPiece> list = this.albumPieceList.Where<SeaAlbumPiece>((Func<SeaAlbumPiece, bool>) (x => x.album_id == album.ID)).ToList<SeaAlbumPiece>();
    if (Object.op_Equality((Object) this.detailPopup, (Object) null) || this.IsPushAndSet())
      return;
    this.StartCoroutine(this.OpenDetailPopup(playerAlbum, list));
  }

  private IEnumerator OpenDetailPopup(PlayerAlbum playerAlbum, List<SeaAlbumPiece> pieceList)
  {
    Sea030AlbumMenu sea030AlbumMenu = this;
    GameObject popup = sea030AlbumMenu.detailPopup.Clone();
    popup.SetActive(false);
    IEnumerator e = popup.GetComponent<Popup030SeaDetailMenu>().Init(playerAlbum, pieceList, new Action(sea030AlbumMenu.PushReset));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet() || !Singleton<NGSceneManager>.GetInstance().isSceneInitialized)
      return;
    if (SMManager.Get<PlayerSeaQuestS[]>() == null)
      Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
    Singleton<NGSceneManager>.GetInstance().sceneBase.IsPush = true;
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
    Singleton<NGSceneManager>.GetInstance().clearStack();
    Sea030HomeScene.ChangeScene(false);
  }

  public void OnBtnNextAlbum()
  {
    if (this.scrollParts.selected >= this.scrollParts.PartsCnt)
      return;
    this.scrollParts.setItemPosition(this.scrollParts.selected + 1);
  }

  public void OnBtnPrevAlbum()
  {
    if (this.scrollParts.selected <= 0)
      return;
    this.scrollParts.setItemPosition(this.scrollParts.selected - 1);
  }
}
