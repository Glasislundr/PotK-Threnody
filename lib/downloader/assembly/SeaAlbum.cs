// Decompiled with JetBrains decompiler
// Type: SeaAlbum
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using UnityEngine;

#nullable disable
public class SeaAlbum : MonoBehaviour
{
  [SerializeField]
  private UI2DSprite Illustration;
  [SerializeField]
  private GameObject[] CoverPanel;
  private int index;

  public int Index => this.index;

  public void Init(Sprite sprite, PlayerAlbum playerAlbum, int index)
  {
    this.index = index;
    for (int index1 = 0; index1 < this.CoverPanel.Length; ++index1)
    {
      bool flag = !playerAlbum.player_album_piece[index1].is_open;
      this.CoverPanel[index1].SetActive(flag);
    }
    this.Illustration.sprite2D = sprite;
  }
}
