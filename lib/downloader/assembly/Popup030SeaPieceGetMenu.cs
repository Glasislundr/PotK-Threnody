// Decompiled with JetBrains decompiler
// Type: Popup030SeaPieceGetMenu
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
public class Popup030SeaPieceGetMenu : ResultMenuBase
{
  private GameObject popupObject;
  private List<PieceGetResult> pieceGetResultList = new List<PieceGetResult>();

  public override IEnumerator Init(BattleInfo info, BattleEnd result)
  {
    BattleEndGet_sea_album_piece_counts[] albumPieceCounts1 = result.get_sea_album_piece_counts;
    MasterDataTable.SeaAlbum[] seaAlbumList = MasterData.SeaAlbumList;
    SeaAlbumPiece[] seaAlbumPieceList = MasterData.SeaAlbumPieceList;
    foreach (BattleEndGet_sea_album_piece_counts albumPieceCounts2 in albumPieceCounts1)
    {
      BattleEndGet_sea_album_piece_counts targetPiece = albumPieceCounts2;
      SeaAlbumPiece piece = ((IEnumerable<SeaAlbumPiece>) seaAlbumPieceList).FirstOrDefault<SeaAlbumPiece>((Func<SeaAlbumPiece, bool>) (x => x.ID == targetPiece.album_piece_id));
      if (piece != null)
      {
        MasterDataTable.SeaAlbum seaAlbum = ((IEnumerable<MasterDataTable.SeaAlbum>) seaAlbumList).FirstOrDefault<MasterDataTable.SeaAlbum>((Func<MasterDataTable.SeaAlbum, bool>) (x => x.ID == piece.album_id));
        if (seaAlbum != null)
          this.pieceGetResultList.Add(new PieceGetResult(seaAlbum.name, piece.name, targetPiece.count, piece.same_character_id));
      }
    }
    IEnumerator e = this.LoadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator LoadResources()
  {
    Future<GameObject> handler = Res.Prefabs.popup.popup_030_sea_piece_get__anim_fade.Load<GameObject>();
    IEnumerator e = handler.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.popupObject = handler.Result.Clone(Singleton<CommonRoot>.GetInstance().LoadTmpObj.transform);
    this.popupObject.SetActive(false);
  }

  public override IEnumerator Run()
  {
    Popup030SeaPieceGetMenu popup030SeaPieceGetMenu = this;
    popup030SeaPieceGetMenu.popupObject.SetParent(((Component) popup030SeaPieceGetMenu).gameObject);
    popup030SeaPieceGetMenu.popupObject.SetActive(true);
    Popup030SeaPieceGetSetting script = popup030SeaPieceGetMenu.popupObject.GetComponent<Popup030SeaPieceGetSetting>();
    bool toNext = false;
    IEnumerator e = script.Init(popup030SeaPieceGetMenu.pieceGetResultList, (Action) (() => toNext = true));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1034");
    while (!toNext)
    {
      if (Singleton<NGGameDataManager>.GetInstance().questAutoLap)
      {
        yield return (object) new WaitForSeconds(3f);
        script.OnButtonOK();
      }
      yield return (object) null;
    }
    Object.DestroyObject((Object) popup030SeaPieceGetMenu.popupObject);
  }
}
