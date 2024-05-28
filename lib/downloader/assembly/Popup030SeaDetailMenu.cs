// Decompiled with JetBrains decompiler
// Type: Popup030SeaDetailMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class Popup030SeaDetailMenu : BackButtonMenuBase
{
  [SerializeField]
  private UIScrollView scrollview;
  [SerializeField]
  private NGxScroll2 scroll;
  [SerializeField]
  private GameObject lineObj;
  [SerializeField]
  private GameObject titleAcquired;
  [SerializeField]
  private int cellWidth = 500;
  [SerializeField]
  private int cellHeight = 50;
  private GameObject listPrefab;
  private Action callback;

  public IEnumerator Init(PlayerAlbum playerAlbum, List<SeaAlbumPiece> pieceList, Action callback = null)
  {
    IEnumerator e = this.LoadResource();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.callback = callback;
    List<SeaAlbumPiece> list1 = pieceList.Where<SeaAlbumPiece>((Func<SeaAlbumPiece, bool>) (x => !x.is_released)).ToList<SeaAlbumPiece>();
    List<SeaAlbumPiece> list2 = list1.Where<SeaAlbumPiece>((Func<SeaAlbumPiece, bool>) (x => !this.IsOpenPanel(playerAlbum, x))).ToList<SeaAlbumPiece>();
    List<SeaAlbumPiece> list3 = list1.Where<SeaAlbumPiece>((Func<SeaAlbumPiece, bool>) (x => this.IsOpenPanel(playerAlbum, x))).ToList<SeaAlbumPiece>();
    foreach (SeaAlbumPiece seaAlbumPiece in list2)
    {
      SeaAlbumPiece piece = seaAlbumPiece;
      PlayerAlbumPiece playerPiece = ((IEnumerable<PlayerAlbumPiece>) playerAlbum.player_album_piece).SingleOrDefault<PlayerAlbumPiece>((Func<PlayerAlbumPiece, bool>) (x => x.piece_id == piece.piece_id));
      GameObject gameObject = this.listPrefab.Clone();
      if (playerPiece != null && piece != null)
      {
        gameObject.GetComponent<SeaAlbumPieceList>().Init(playerPiece, piece);
        this.scroll.AddColumn1(gameObject, this.cellWidth, this.cellHeight);
      }
    }
    if (list1.Count == list3.Count || list1.Count == list2.Count)
    {
      this.lineObj.SetActive(false);
      this.titleAcquired.SetActive(false);
    }
    else
    {
      this.scroll.AddColumn1(this.lineObj, this.cellWidth, this.cellHeight);
      this.scroll.AddColumn1(this.titleAcquired, this.cellWidth, this.cellHeight);
    }
    foreach (SeaAlbumPiece seaAlbumPiece in list3)
    {
      SeaAlbumPiece piece = seaAlbumPiece;
      PlayerAlbumPiece playerPiece = ((IEnumerable<PlayerAlbumPiece>) playerAlbum.player_album_piece).SingleOrDefault<PlayerAlbumPiece>((Func<PlayerAlbumPiece, bool>) (x => x.piece_id == piece.piece_id));
      GameObject gameObject = this.listPrefab.Clone();
      if (playerPiece != null && piece != null)
      {
        gameObject.GetComponent<SeaAlbumPieceList>().Init(playerPiece, piece);
        this.scroll.AddColumn1(gameObject, this.cellWidth, this.cellHeight);
      }
    }
    this.scrollview.ResetPosition();
    this.scrollview.UpdateScrollbars(true);
  }

  private IEnumerator LoadResource()
  {
    Future<GameObject> prefabF = Res.Prefabs.sea030_album.dir_sea_album_piece_list.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.listPrefab = prefabF.Result;
  }

  private bool IsOpenPanel(PlayerAlbum playerAlbum, SeaAlbumPiece piece)
  {
    PlayerAlbumPiece playerAlbumPiece = ((IEnumerable<PlayerAlbumPiece>) playerAlbum.player_album_piece).SingleOrDefault<PlayerAlbumPiece>((Func<PlayerAlbumPiece, bool>) (x => x.piece_id == piece.piece_id));
    return playerAlbumPiece != null && playerAlbumPiece.is_open;
  }

  public void IbtnPopupOk()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    this.callback();
  }

  public override void onBackButton() => this.IbtnPopupOk();
}
