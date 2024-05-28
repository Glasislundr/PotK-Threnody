// Decompiled with JetBrains decompiler
// Type: MusicScrollItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MusicScrollItem : MonoBehaviour
{
  [SerializeField]
  private GameObject btnNormal;
  [SerializeField]
  private UIDragScrollView btnNormalDragScrollView;
  [SerializeField]
  private GameObject btnPlay;
  [SerializeField]
  private UIDragScrollView btnPlayDragScrollView;
  [SerializeField]
  private GameObject btnLock;
  [SerializeField]
  private UIDragScrollView btnLockDragScrollView;
  [SerializeField]
  private UILabel musicName;
  public GameObject isNew;
  private Guide011JukeboxMenu menu;
  private MusicItemInfo musicItemInfo;

  public void Set(Guide011JukeboxMenu menu, MusicItemInfo musicItemInfo)
  {
    this.menu = menu;
    this.musicItemInfo = musicItemInfo;
    this.btnNormalDragScrollView.scrollView = menu.musicListScroll;
    this.btnPlayDragScrollView.scrollView = menu.musicListScroll;
    this.btnLockDragScrollView.scrollView = menu.musicListScroll;
    this.isNew.SetActive(musicItemInfo.IsNew);
    switch (musicItemInfo.MusitStatus)
    {
      case MusitStatus.Normal:
        this.SetNormal();
        break;
      case MusitStatus.Playing:
        this.SetPlaying();
        break;
      case MusitStatus.Lock:
        this.SetLock();
        break;
    }
    this.musicName.text = musicItemInfo.Music.music_name;
  }

  public void OnSelectFirst()
  {
    this.btnNormal.SetActive(false);
    this.btnPlay.SetActive(true);
    this.btnLock.SetActive(false);
    this.musicItemInfo.MusitStatus = MusitStatus.Playing;
    this.menu.SetBottom(this.musicItemInfo.Music);
  }

  public void OnTapNormal()
  {
    this.btnNormal.SetActive(false);
    this.btnPlay.SetActive(true);
    this.btnLock.SetActive(false);
    this.musicItemInfo.MusitStatus = MusitStatus.Playing;
    this.menu.SetBottom(this.musicItemInfo.Music, true);
  }

  public void SetNormal()
  {
    this.btnNormal.SetActive(true);
    this.btnPlay.SetActive(false);
    this.btnLock.SetActive(false);
    this.musicItemInfo.MusitStatus = MusitStatus.Normal;
  }

  public void SetPlaying()
  {
    this.btnNormal.SetActive(false);
    this.btnPlay.SetActive(true);
    this.btnLock.SetActive(false);
    this.musicItemInfo.MusitStatus = MusitStatus.Playing;
  }

  public void SetLock()
  {
    this.btnNormal.SetActive(false);
    this.btnPlay.SetActive(false);
    this.btnLock.SetActive(true);
    this.musicItemInfo.MusitStatus = MusitStatus.Lock;
  }

  public void OnTapLock()
  {
  }
}
