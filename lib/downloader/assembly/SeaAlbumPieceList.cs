// Decompiled with JetBrains decompiler
// Type: SeaAlbumPieceList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using UnityEngine;

#nullable disable
public class SeaAlbumPieceList : MonoBehaviour
{
  [SerializeField]
  private UILabel txtPiece;
  [SerializeField]
  private UILabel txtProgress;
  [SerializeField]
  private GameObject acheiveObj;

  public void Init(PlayerAlbumPiece playerPiece, SeaAlbumPiece piece)
  {
    this.txtPiece.SetTextLocalize(piece.name);
    this.txtProgress.SetTextLocalize(playerPiece.count.ToString() + "/" + (object) piece.count);
    this.acheiveObj.SetActive(playerPiece.count == piece.count);
  }
}
