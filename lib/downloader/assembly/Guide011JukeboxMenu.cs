// Decompiled with JetBrains decompiler
// Type: Guide011JukeboxMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Guide011JukeboxMenu : BackButtonMenuBase
{
  [Header("上部 タブ周り")]
  [SerializeField]
  private UIButton buySiteButton;
  [SerializeField]
  private UIScrollView musicTabScroll;
  [SerializeField]
  private UICenterOnChild musicTabCenterOnChild;
  private SpringPanel musicTabScrollSpringPanel;
  [SerializeField]
  private List<GameObject> musicTabs;
  [SerializeField]
  private List<GameObject> musicTabsNew;
  [SerializeField]
  private UIButton leftArrow;
  [SerializeField]
  private UIButton rightArrow;
  [Header("中段 楽曲スクロール周り")]
  private const int MUSIC_SCROLL_ITEM_COUNT = 13;
  private const int MUSIC_SCROLL_ITEM_WIGHT = 576;
  private const int MUSIC_SCROLL_ITEM_HEIGHT = 70;
  private GameObject baseMusicScrollItem;
  [SerializeField]
  private NGxScroll2 musicScrollContainer;
  public UIScrollView musicListScroll;
  private List<MusicItemInfo> musicItemAllInfos = new List<MusicItemInfo>();
  private Music[] musicList;
  private Music[] plaingMusicList;
  private List<MusicScrollItem> musicScrollItems = new List<MusicScrollItem>();
  [Header("下段 楽曲説明周り")]
  [SerializeField]
  private GameObject arrangeOff;
  [SerializeField]
  private GameObject arrangeOn;
  [SerializeField]
  private UILabel playingMusicName;
  [SerializeField]
  private GameObject composition;
  [SerializeField]
  private UILabel playingComposition;
  [SerializeField]
  private GameObject arrangement;
  [SerializeField]
  private UILabel playingArrangement;
  [SerializeField]
  private GameObject compositionAndArrangement;
  [SerializeField]
  private UILabel playingCompositionAndArrangement;
  [SerializeField]
  private GameObject btnPlay;
  [SerializeField]
  private GameObject btnStop;
  private Dictionary<string, int> tabDict;
  private Dictionary<int, int> tabIdToIndex;
  private Dictionary<int, int> tabIndexToId;
  [Header("再生中回転ディスク関連")]
  [SerializeField]
  private GameObject slc_Record;
  [SerializeField]
  private float add_r_z = 0.03f;
  [SerializeField]
  private float sub_r_z = 0.01f;
  [SerializeField]
  private float max_r_z = 0.5f;
  [HideInInspector]
  public int currentTabId;
  private Music currentMusic;
  private Music currentPlayMusic;
  private bool isPause = true;
  private HashSet<int> unlockMusics;
  private bool isPlayFirst;
  private float scrollStartY;
  private int allScrollHeight;
  private int addScrollHeight;
  private int checkHeight = 140;
  private Transform transformMusicListScroll;
  private List<Transform> transformScrollItems = new List<Transform>();
  private int tabIdLog;
  private float before_diff_y;
  private float beforeUpdateTime;
  private float updateTime;
  private float spped_r_z;

  public IEnumerator onInitSceneAsync()
  {
    this.transformMusicListScroll = ((Component) this.musicListScroll).transform;
    this.addScrollHeight = 910;
    this.checkHeight = 140;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) this.SetBaseMusicItem();
    this.tabDict = new Dictionary<string, int>();
    this.tabIdToIndex = new Dictionary<int, int>();
    this.tabIndexToId = new Dictionary<int, int>();
    int num = 0;
    foreach (GameObject musicTab in this.musicTabs)
    {
      int musicTabId = musicTab.GetComponent<Guide011JukeboxMusicTabId>().MusicTabId;
      this.tabDict.Add(((Object) musicTab).name, musicTabId);
      this.musicTabsNew[num].SetActive(this.isNewTab(musicTabId));
      this.tabIdToIndex[musicTabId] = num;
      this.tabIndexToId[num] = musicTabId;
      if (num == 0)
        this.tabIndexToId[this.musicTabs.Count] = musicTabId;
      else if (num == this.musicTabs.Count - 1)
        this.tabIndexToId[-1] = musicTabId;
      ++num;
    }
  }

  private int MoveTabLeft(int tabId) => this.tabIndexToId[this.tabIdToIndex[tabId] - 1];

  private int MoveTabRight(int tabId) => this.tabIndexToId[this.tabIdToIndex[tabId] + 1];

  public IEnumerator onStartSceneAsync()
  {
    Guide011JukeboxMenu guide011JukeboxMenu = this;
    // ISSUE: method pointer
    guide011JukeboxMenu.musicTabCenterOnChild.onFinished = new SpringPanel.OnFinished((object) guide011JukeboxMenu, __methodptr(\u003ConStartSceneAsync\u003Eb__52_0));
    int tabId = 1;
    if (Persist.jukeBox.Data.LatestSelectTabId != 0)
      tabId = Persist.jukeBox.Data.LatestSelectTabId;
    guide011JukeboxMenu.SetTab(tabId);
    guide011JukeboxMenu.tabIdLog = tabId;
    yield return (object) new WaitForSeconds(0.3f);
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  private IEnumerator SetBaseMusicItem()
  {
    Future<GameObject> l = new ResourceObject("Prefabs/guide011_Jukebox/dir_Jukebox_list").Load<GameObject>();
    yield return (object) l.Wait();
    this.baseMusicScrollItem = l.Result;
  }

  private bool isNewTab(int tabId)
  {
    Music[] array = ((IEnumerable<Music>) MasterData.MusicList).Where<Music>((Func<Music, bool>) (x => x.music_tab_id == tabId)).ToArray<Music>();
    bool flag = false;
    foreach (Music music in array)
    {
      if ((Persist.jukeBox.Data.PlayedMusicId.Contains(music.ID) ? 0 : 1) != 0)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  private void SetTab(int tabId)
  {
    this.musicTabCenterOnChild.CenterOn(((Component) this.musicTabCenterOnChild).transform.GetChild(this.tabIdToIndex[tabId]));
  }

  private void SetMusicScroll(int tabId)
  {
    if (tabId == this.currentTabId)
      return;
    this.currentTabId = tabId;
    this.musicList = ((IEnumerable<Music>) MasterData.MusicList).Where<Music>((Func<Music, bool>) (x => x.music_tab_id == tabId)).ToArray<Music>();
    if (this.plaingMusicList == null)
      this.plaingMusicList = this.musicList;
    this.musicItemAllInfos.Clear();
    foreach (Music music in this.musicList)
    {
      MusicItemInfo musicItemInfo = new MusicItemInfo();
      bool isNew = !Persist.jukeBox.Data.PlayedMusicId.Contains(music.ID);
      musicItemInfo.Set(music, isNew, MusitStatus.Normal);
      this.musicItemAllInfos.Add(musicItemInfo);
    }
    this.musicScrollContainer.Clear();
    this.musicScrollContainer.Reset();
    this.musicScrollItems.Clear();
    this.transformScrollItems.Clear();
    for (int index = 0; index < Mathf.Min(this.musicList.Length, 13); ++index)
    {
      GameObject gameObject = Object.Instantiate<GameObject>(this.baseMusicScrollItem);
      this.musicScrollContainer.AddColumn1(gameObject, 576, 70, true);
      this.transformScrollItems.Add(gameObject.transform);
      this.musicScrollItems.Add(gameObject.GetComponent<MusicScrollItem>());
      this.ResetScroll(index);
      this.CreateScroll(index, index);
    }
    this.musicScrollContainer.CreateScrollPointHeight(70, this.musicItemAllInfos.Count);
    this.musicScrollContainer.ResolvePosition();
    this.scrollStartY = this.transformMusicListScroll.localPosition.y;
    this.allScrollHeight = (this.musicItemAllInfos.Count - 1) * 70;
  }

  public void SetBottom(Music music, bool isTapPlay = false)
  {
    if (this.currentMusic != null && music.ID == this.currentMusic.ID)
    {
      if (!isTapPlay || !this.isPause)
        return;
      this.onMusicPlay();
      this.plaingMusicList = this.musicList;
    }
    else
    {
      if (this.currentMusic != null && music.ID != this.currentMusic.ID)
      {
        int? nullable = this.musicItemAllInfos.FirstIndexOrNull<MusicItemInfo>((Func<MusicItemInfo, bool>) (x => x.Music.ID == this.currentMusic.ID));
        if (nullable.HasValue)
        {
          this.musicItemAllInfos[nullable.Value].MusitStatus = MusitStatus.Normal;
          if (Object.op_Inequality((Object) this.musicItemAllInfos[nullable.Value].MusicScrollItem, (Object) null))
            this.musicItemAllInfos[nullable.Value].MusicScrollItem.Set(this, this.musicItemAllInfos[nullable.Value]);
        }
      }
      this.currentMusic = music;
      this.arrangeOff.SetActive(true);
      this.arrangeOn.SetActive(false);
      SpreadColorButton component = this.arrangeOff.GetComponent<SpreadColorButton>();
      if (Object.op_Inequality((Object) component, (Object) null))
      {
        if (this.currentMusic.is_arrange)
        {
          ((UIButtonColor) component).isEnabled = true;
          component.SetColor(Color.white);
        }
        else
        {
          ((UIButtonColor) component).isEnabled = false;
          component.SetColor(((UIButtonColor) component).disabledColor);
        }
      }
      if (this.currentPlayMusic != null && this.currentPlayMusic == this.currentMusic)
      {
        if (this.isPause)
        {
          this.btnPlay.SetActive(true);
          this.btnStop.SetActive(false);
        }
        else
        {
          this.btnPlay.SetActive(false);
          this.btnStop.SetActive(true);
        }
      }
      else if (isTapPlay)
      {
        this.onMusicPlay();
        this.plaingMusicList = this.musicList;
      }
      else
      {
        this.btnPlay.SetActive(true);
        this.btnStop.SetActive(false);
      }
      if (this.currentMusic.purchase_site_link == "")
        ((UIButtonColor) this.buySiteButton).isEnabled = false;
      else
        ((UIButtonColor) this.buySiteButton).isEnabled = true;
      this.playingMusicName.text = music.music_name;
      if (music.composer == music.arranger)
      {
        this.composition.SetActive(false);
        this.arrangement.SetActive(false);
        this.compositionAndArrangement.SetActive(true);
        this.playingCompositionAndArrangement.text = music.composer;
      }
      else
      {
        this.compositionAndArrangement.SetActive(false);
        this.composition.SetActive(true);
        this.arrangement.SetActive(true);
        this.playingComposition.text = music.composer;
        this.playingArrangement.text = music.arranger;
      }
    }
  }

  protected override void Update()
  {
    base.Update();
    this.UpdateDisc();
    if (Object.op_Equality((Object) this.transformMusicListScroll, (Object) null))
      return;
    float num1 = this.transformMusicListScroll.localPosition.y - this.scrollStartY;
    if ((double) num1 == (double) this.before_diff_y)
      return;
    this.before_diff_y = num1;
    if ((double) num1 < 0.0)
      num1 = 0.0f;
    if ((double) num1 > (double) this.allScrollHeight)
      num1 = (float) this.allScrollHeight;
    bool flag;
    do
    {
      flag = false;
      int num2 = 0;
      foreach (Transform transformScrollItem in this.transformScrollItems)
      {
        GameObject go = ((Component) transformScrollItem).gameObject;
        float num3 = transformScrollItem.localPosition.y + num1;
        if ((double) num3 > (double) this.checkHeight)
        {
          int? nullable = this.musicItemAllInfos.FirstIndexOrNull<MusicItemInfo>((Func<MusicItemInfo, bool>) (v => Object.op_Inequality((Object) v.MusicScrollItem, (Object) null) && Object.op_Equality((Object) ((Component) v.MusicScrollItem).gameObject, (Object) go)));
          int info_index = nullable.HasValue ? nullable.Value + 13 : this.musicItemAllInfos.Count;
          if (nullable.HasValue && info_index < this.musicItemAllInfos.Count)
          {
            transformScrollItem.localPosition = new Vector3(transformScrollItem.localPosition.x, transformScrollItem.localPosition.y - (float) this.addScrollHeight, 0.0f);
            if (info_index >= this.musicItemAllInfos.Count)
            {
              go.SetActive(false);
            }
            else
            {
              this.ResetScroll(num2);
              this.CreateScroll(info_index, num2);
            }
            flag = true;
          }
        }
        else if ((double) num3 < (double) -(this.addScrollHeight - this.checkHeight))
        {
          int num4 = 13;
          if (!go.activeSelf)
          {
            go.SetActive(true);
            num4 = 0;
          }
          int? nullable = this.musicItemAllInfos.FirstIndexOrNull<MusicItemInfo>((Func<MusicItemInfo, bool>) (v => Object.op_Inequality((Object) v.MusicScrollItem, (Object) null) && Object.op_Equality((Object) ((Component) v.MusicScrollItem).gameObject, (Object) go)));
          int info_index = nullable.HasValue ? nullable.Value - num4 : -1;
          if (nullable.HasValue && info_index >= 0)
          {
            transformScrollItem.localPosition = new Vector3(transformScrollItem.localPosition.x, transformScrollItem.localPosition.y + (float) this.addScrollHeight, 0.0f);
            this.ResetScroll(num2);
            this.CreateScroll(info_index, num2);
            flag = true;
          }
        }
        ++num2;
      }
    }
    while (flag);
  }

  private void UpdateDisc()
  {
    if (Object.op_Equality((Object) this.slc_Record, (Object) null))
      return;
    if (this.isPause)
    {
      this.spped_r_z -= this.sub_r_z;
      if ((double) this.spped_r_z < 0.0)
        this.spped_r_z = 0.0f;
    }
    else
    {
      this.spped_r_z += this.add_r_z;
      if ((double) this.spped_r_z > (double) this.max_r_z)
        this.spped_r_z = this.max_r_z;
    }
    this.beforeUpdateTime = this.updateTime;
    this.updateTime = Time.realtimeSinceStartup;
    if ((double) this.beforeUpdateTime == 0.0)
      this.beforeUpdateTime = this.updateTime;
    this.slc_Record.transform.Rotate(0.0f, 0.0f, (float) (-(double) this.spped_r_z * 60.0) * (this.updateTime - this.beforeUpdateTime));
  }

  private void ResetScroll(int index)
  {
    MusicScrollItem musicScrollItem = this.musicScrollItems[index];
    foreach (MusicItemInfo musicItemAllInfo in this.musicItemAllInfos)
    {
      if (Object.op_Equality((Object) musicItemAllInfo.MusicScrollItem, (Object) musicScrollItem))
        musicItemAllInfo.MusicScrollItem = (MusicScrollItem) null;
    }
    ((Component) musicScrollItem).gameObject.SetActive(false);
  }

  private void CreateScroll(int info_index, int unit_index)
  {
    MusicItemInfo musicItemAllInfo1 = this.musicItemAllInfos[info_index];
    MusicScrollItem musicScrollItem = this.musicScrollItems[unit_index];
    foreach (MusicItemInfo musicItemAllInfo2 in this.musicItemAllInfos)
    {
      if (Object.op_Equality((Object) musicItemAllInfo2.MusicScrollItem, (Object) musicScrollItem))
        musicItemAllInfo2.MusicScrollItem = (MusicScrollItem) null;
    }
    musicItemAllInfo1.MusicScrollItem = musicScrollItem;
    musicScrollItem.Set(this, musicItemAllInfo1);
    ((Component) musicScrollItem).gameObject.SetActive(true);
    if (this.currentMusic == null && musicItemAllInfo1.MusitStatus == MusitStatus.Normal)
      musicScrollItem.OnSelectFirst();
    if (this.currentMusic == null || this.currentMusic.ID != musicItemAllInfo1.Music.ID)
      return;
    musicScrollItem.OnSelectFirst();
  }

  public void onBuySiteButton()
  {
    if (this.IsPushAndSet())
      return;
    Application.OpenURL(this.currentMusic.purchase_site_link);
    this.StartCoroutine(this.IsPushOff());
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public void onMusicTabLefftArrow()
  {
    if (((Behaviour) this.musicTabScrollSpringPanel).enabled)
      return;
    this.SetTab(this.MoveTabLeft(this.currentTabId));
  }

  public void onMusicTabRightArrow()
  {
    if (((Behaviour) this.musicTabScrollSpringPanel).enabled)
      return;
    this.SetTab(this.MoveTabRight(this.currentTabId));
  }

  public void onArrangeOff()
  {
    if (this.IsPushAndSet())
      return;
    this.arrangeOff.SetActive(false);
    this.arrangeOn.SetActive(true);
    this.SetNormalOrArrange();
    this.StartCoroutine(this.IsPushOff());
  }

  public void onArrangeOn()
  {
    if (this.IsPushAndSet())
      return;
    this.arrangeOff.SetActive(true);
    this.arrangeOn.SetActive(false);
    this.SetNormalOrArrange();
    this.StartCoroutine(this.IsPushOff());
  }

  public void onMusicPlay()
  {
    if (this.isPause && this.currentMusic == this.currentPlayMusic)
    {
      Singleton<NGSoundManager>.GetInstance().ResumeBgm();
      this.SetNormalOrArrange();
      this.btnPlay.SetActive(false);
      this.btnStop.SetActive(true);
      this.isPause = false;
    }
    else
    {
      if (this.isPause)
      {
        Singleton<NGSoundManager>.GetInstance().StopBgm(time: 0.1f);
        Singleton<NGSoundManager>.GetInstance().PlayBgmFile(this.currentMusic.bgm_file, this.currentMusic.bgm_name, delay: 0.1f);
      }
      else
        Singleton<NGSoundManager>.GetInstance().PlayBgmFile(this.currentMusic.bgm_file, this.currentMusic.bgm_name);
      this.SetNormalOrArrange();
      this.btnPlay.SetActive(false);
      this.btnStop.SetActive(true);
      MusicItemInfo musicItemInfo = this.musicItemAllInfos.FirstOrDefault<MusicItemInfo>((Func<MusicItemInfo, bool>) (x => x.Music.ID == this.currentMusic.ID));
      if (musicItemInfo != null)
      {
        musicItemInfo.IsNew = false;
        if (Object.op_Inequality((Object) musicItemInfo.MusicScrollItem, (Object) null))
          musicItemInfo.MusicScrollItem.Set(this, musicItemInfo);
      }
      Persist.jukeBox.Data.PlayedMusicId.Add(this.currentMusic.ID);
      this.musicTabsNew[this.tabIdToIndex[this.currentMusic.music_tab_id]].SetActive(this.isNewTab(this.currentMusic.music_tab_id));
      this.isPause = false;
    }
    this.currentPlayMusic = this.currentMusic;
    Persist.jukeBox.Data.LatestSelectTabId = this.currentTabId;
    Persist.jukeBox.Data.LatestScrollVarValue = this.musicListScroll.verticalScrollBar.value;
    Persist.jukeBox.Data.LatestMusicId = this.currentMusic.ID;
    this.isPlayFirst = true;
  }

  private void SetNormalOrArrange()
  {
    this.StopCoroutine("SetNormalOrArrangeAsync");
    this.StartCoroutine("SetNormalOrArrangeAsync");
  }

  private IEnumerator SetNormalOrArrangeAsync()
  {
    if (this.IsArrange())
      yield return (object) Singleton<NGSoundManager>.GetInstance().crossFadeCurrentBGMAsync(2f, 1f);
    else
      yield return (object) Singleton<NGSoundManager>.GetInstance().crossFadeCurrentBGMAsync(2.5f, 0.0f);
  }

  private bool IsArrange() => this.currentMusic.is_arrange && this.arrangeOn.activeSelf;

  public void onStopMusic()
  {
    Singleton<NGSoundManager>.GetInstance().PauseBgm();
    this.btnPlay.SetActive(true);
    this.btnStop.SetActive(false);
    this.isPause = true;
  }

  public void onPrevMusic()
  {
    if (this.plaingMusicList == null)
      return;
    int index = this.getCurrentPlayMusicIndex() - 1;
    if (index < 0)
      index = this.plaingMusicList.Length - 1;
    this.SetPlaingMusic(this.plaingMusicList[index]);
  }

  public void onNextMusic()
  {
    if (this.plaingMusicList == null)
      return;
    int index = this.getCurrentPlayMusicIndex() + 1;
    if (this.plaingMusicList.Length <= index)
      index = 0;
    this.SetPlaingMusic(this.plaingMusicList[index]);
  }

  private int getCurrentPlayMusicIndex()
  {
    int id;
    if (this.currentPlayMusic != null)
    {
      id = this.currentPlayMusic.ID;
    }
    else
    {
      if (this.currentMusic == null)
        return -1;
      id = this.currentMusic.ID;
    }
    for (int currentPlayMusicIndex = 0; currentPlayMusicIndex < this.plaingMusicList.Length; ++currentPlayMusicIndex)
    {
      if (this.plaingMusicList[currentPlayMusicIndex].ID == id)
        return currentPlayMusicIndex;
    }
    return -1;
  }

  private MusicScrollItem getCurrentPlayScrollItem(Music music)
  {
    for (int index = 0; index < this.musicItemAllInfos.Count; ++index)
    {
      if (this.musicItemAllInfos[index].Music.ID == music.ID && this.musicItemAllInfos[index].Music.music_tab_id == music.music_tab_id)
        return this.musicItemAllInfos[index].MusicScrollItem;
    }
    return (MusicScrollItem) null;
  }

  private void SetPlaingMusic(Music music)
  {
    MusicScrollItem currentPlayScrollItem = this.getCurrentPlayScrollItem(music);
    if (Object.op_Inequality((Object) currentPlayScrollItem, (Object) null))
    {
      currentPlayScrollItem.OnTapNormal();
    }
    else
    {
      this.SetBottom(music);
      this.onMusicPlay();
    }
  }
}
