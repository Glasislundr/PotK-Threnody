// Decompiled with JetBrains decompiler
// Type: DailyMission0271Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class DailyMission0271Menu : BackButtonMenuBase
{
  [SerializeField]
  private NGHorizontalScrollParts scrollParts;
  [SerializeField]
  private DailyMission0271MissionRoot missonRoot;
  private PlayerBingo[] enablePlayerBingos;
  private PlayerBingo selectedPlayerBingo;
  private GameObject rewardPopup;
  private UIScrollView bannerScrollView;
  private bool isArrowMove_;

  public bool isLockItemChange { get; set; }

  public bool isPlayingEffect { get; set; }

  public IEnumerator Init()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.BingoIndex> future = WebAPI.BingoIndex((Action<WebAPI.Response.UserError>) (error =>
    {
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
      Singleton<CommonRoot>.GetInstance().isLoading = false;
    }));
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (future.Result != null)
    {
      PlayerBingo[] playerBingo = future.Result.player_bingo;
      if (playerBingo == null)
      {
        MypageScene.ChangeScene();
        Singleton<CommonRoot>.GetInstance().isLoading = false;
      }
      else
      {
        this.enablePlayerBingos = ((IEnumerable<PlayerBingo>) playerBingo).Where<PlayerBingo>((Func<PlayerBingo, bool>) (x =>
        {
          if (x.is_end || x.bingo == null)
            return false;
          if (!x.bingo.end_at.HasValue)
            return true;
          return x.bingo.end_at.HasValue && x.bingo.end_at.Value > ServerTime.NowAppTimeAddDelta();
        })).OrderBy<PlayerBingo, int>((Func<PlayerBingo, int>) (x => x.bingo.priority)).ThenBy<PlayerBingo, int>((Func<PlayerBingo, int>) (x => x.bingo_id)).ToArray<PlayerBingo>();
        if (((IEnumerable<PlayerBingo>) this.enablePlayerBingos).Count<PlayerBingo>() <= 0)
        {
          MypageScene.ChangeScene();
          Singleton<CommonRoot>.GetInstance().isLoading = false;
        }
        else
        {
          ResourceManager resourceManager = Singleton<ResourceManager>.GetInstance();
          Future<GameObject> rewardPopupF = resourceManager.Load<GameObject>("Prefabs/popup/popup_027_1__anim_popup01");
          e = rewardPopupF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          this.rewardPopup = rewardPopupF.Result;
          Future<GameObject> headerPrefabF = resourceManager.Load<GameObject>("Prefabs/dailymission027_1/dir_panel_mission_header");
          e = headerPrefabF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          e = this.missonRoot.InitPrefabs();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          this.scrollParts.destroyParts(false);
          GameObject headerPrefab = headerPrefabF.Result;
          PlayerBingo[] playerBingoArray = this.enablePlayerBingos;
          for (int index = 0; index < playerBingoArray.Length; ++index)
          {
            PlayerBingo bingoData = playerBingoArray[index];
            e = this.scrollParts.instantiateParts(headerPrefab).GetComponent<DailyMission0271PanelMissionHeader>().InitHeader(bingoData);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
          playerBingoArray = (PlayerBingo[]) null;
          this.isLockItemChange = false;
          this.scrollParts.resetScrollView();
          this.scrollParts.setItemPositionQuick(0);
          e = this.SetBingoPanel(this.enablePlayerBingos[0]);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          yield return (object) new WaitForEndOfFrame();
          yield return (object) new WaitForEndOfFrame();
          Singleton<CommonRoot>.GetInstance().isLoading = false;
        }
      }
    }
  }

  private void onItemChanged(int selected)
  {
    if (this.isLockItemChange)
    {
      this.scrollParts.setItemPosition(((IEnumerable<PlayerBingo>) this.enablePlayerBingos).FirstIndexOrNull<PlayerBingo>((Func<PlayerBingo, bool>) (x => x == this.selectedPlayerBingo)) ?? 0);
    }
    else
    {
      if (this.isArrowMove)
        this.isArrowMove = false;
      this.IsPush = true;
      this.StartCoroutine(this.SetBingoPanel(this.enablePlayerBingos[selected], true));
    }
  }

  private IEnumerator SetBingoPanel(PlayerBingo playerBingoData, bool isCheckObject = false)
  {
    DailyMission0271Menu dailyMission0271Menu = this;
    if (dailyMission0271Menu.selectedPlayerBingo != playerBingoData)
    {
      dailyMission0271Menu.selectedPlayerBingo = playerBingoData;
      IEnumerator e = dailyMission0271Menu.missonRoot.SetMissionData(dailyMission0271Menu.selectedPlayerBingo);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    dailyMission0271Menu.IsPush = false;
  }

  public void SetPlayerBingoData(PlayerBingo playerBingo)
  {
    for (int index = 0; index < this.enablePlayerBingos.Length; ++index)
    {
      if (this.enablePlayerBingos[index] == this.selectedPlayerBingo)
        this.enablePlayerBingos[index] = playerBingo;
    }
    this.selectedPlayerBingo = playerBingo;
  }

  public void IbtnBack()
  {
    if (this.CheckIsPush() || this.IsPushAndSet())
      return;
    MypageScene.ChangeScene();
  }

  public override void onBackButton()
  {
    if (Singleton<PopupManager>.GetInstance().isOpen)
      return;
    this.IbtnBack();
  }

  private IEnumerator selectCompRewardAsync(int groupId)
  {
    CommonRoot common = Singleton<CommonRoot>.GetInstance();
    common.loadingMode = 2;
    common.isLoading = true;
    Future<WebAPI.Response.BingoSelectComplete> future = WebAPI.BingoSelectComplete(this.selectedPlayerBingo.bingo_id, groupId, (Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = future.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (future.Result != null)
    {
      common.isLoading = false;
      common.loadingMode = 0;
    }
  }

  public void IbtnLeftArrow()
  {
    if (this.CheckIsPush() || this.scrollParts.selected <= 0)
      return;
    this.scrollParts.setItemPosition(this.scrollParts.selected - 1);
    this.isArrowMove = true;
  }

  public void IbtnRightArrow()
  {
    if (this.CheckIsPush() || this.scrollParts.selected >= this.scrollParts.PartsCnt - 1)
      return;
    this.scrollParts.setItemPosition(this.scrollParts.selected + 1);
    this.isArrowMove = true;
  }

  public void IbtnCompleteReward()
  {
    if (this.CheckIsPush() || this.IsPushAndSet())
      return;
    this.StartCoroutine(this.ShowCompleteReward());
  }

  private bool isArrowMove
  {
    get => this.isArrowMove_;
    set
    {
      this.isArrowMove_ = value;
      this.IsPush = value;
    }
  }

  public bool CheckIsPush()
  {
    return this.isPlayingEffect || this.bannerScrollView.isDragging || this.IsPush;
  }

  private IEnumerator ShowCompleteReward()
  {
    DailyMission0271Menu dailyMission0271Menu = this;
    // ISSUE: reference to a compiler-generated method
    MasterDataTable.BingoRewardGroup completeReward = ((IEnumerable<MasterDataTable.BingoRewardGroup>) MasterData.BingoRewardGroupList).Where<MasterDataTable.BingoRewardGroup>(new Func<MasterDataTable.BingoRewardGroup, bool>(dailyMission0271Menu.\u003CShowCompleteReward\u003Eb__29_0)).OrderBy<MasterDataTable.BingoRewardGroup, int>((Func<MasterDataTable.BingoRewardGroup, int>) (x => x.ID)).FirstOrDefault<MasterDataTable.BingoRewardGroup>();
    IEnumerator e = Singleton<PopupManager>.GetInstance().open(dailyMission0271Menu.rewardPopup).GetComponent<DailyMission0271ConfirmationCompRewardPopup>().Init(completeReward);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void Start()
  {
    this.bannerScrollView = this.scrollParts.scrollView.GetComponent<UIScrollView>();
  }
}
