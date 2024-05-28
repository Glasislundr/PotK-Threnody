// Decompiled with JetBrains decompiler
// Type: Explore033TopScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Explore033TopScene : NGSceneBase
{
  private Explore033TopMenu menu;

  public static void changeScene(bool stack = true, bool serverSync = true)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("explore033_Top", (stack ? 1 : 0) != 0, (object) serverSync);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Explore033TopScene explore033TopScene = this;
    ExploreDataManager dataManager = Singleton<ExploreDataManager>.GetInstance();
    yield return (object) dataManager.LoadSuspendData(false);
    if (dataManager.IsNotRegisteredDeck())
    {
      bool isError = false;
      yield return (object) dataManager.RegistrationInitialDeck((Action) (() => isError = true));
      if (isError)
      {
        Singleton<ExploreSceneManager>.GetInstance().SetReloadDirty();
        Singleton<NGSceneManager>.GetInstance().ChangeErrorPage();
        yield break;
      }
    }
    yield return (object) dataManager.LoadSuspendData(true);
    explore033TopScene.menu = explore033TopScene.menuBase as Explore033TopMenu;
    yield return (object) explore033TopScene.menu.InitAsync();
    Singleton<NGDuelDataManager>.GetInstance().Init();
    yield return (object) Singleton<NGDuelDataManager>.GetInstance().PreloadCommonDuelEffect();
  }

  public IEnumerator onStartSceneAsync(bool serverSync)
  {
    Explore033TopScene explore033TopScene = this;
    Singleton<CommonRoot>.GetInstance().SetFooterEnable(true);
    Singleton<CommonRoot>.GetInstance().GetNormalHeaderComponent().PlayEntryAnime();
    explore033TopScene.currentSceneGuildChatDisplayingStatus = NGSceneBase.GuildChatDisplayingStatus.Closed;
    ExploreSceneManager exploreMgr = Singleton<ExploreSceneManager>.GetInstance();
    ExploreDataManager data = Singleton<ExploreDataManager>.GetInstance();
    yield return (object) exploreMgr.OnStartExploreSceneAsync();
    yield return (object) data.ResumeExplore();
    if (serverSync && !exploreMgr.ReloadDirty)
    {
      bool saveFailed = false;
      yield return (object) data.SaveSuspendData((Action) (() => saveFailed = true));
      if (saveFailed)
      {
        data.InitReopenPopupState();
        exploreMgr.SetReloadDirty();
        Singleton<NGSceneManager>.GetInstance().ChangeErrorPage();
        yield break;
      }
    }
    Singleton<ExploreLotteryCore>.GetInstance().SetTaskReloadDirty();
    data.UpdateRankingViewInfo();
    yield return (object) explore033TopScene.menu.onStartSceneAsync();
  }

  public void onStartScene(bool serverSync)
  {
    this.setupExploreRenderSetting(true);
    Singleton<NGSceneManager>.GetInstance().waitSceneAction((Action) (() =>
    {
      ExploreDataManager instance = Singleton<ExploreDataManager>.GetInstance();
      if (instance.IsRankingPeriodFinished)
        Singleton<ExploreSceneManager>.GetInstanceOrNull()?.Pause(false);
      else if (instance.IsReopenPopup)
      {
        this.StartCoroutine(this.menu.ReopenPopup());
      }
      else
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<ExploreSceneManager>.GetInstanceOrNull()?.Pause(false);
      }
    }));
  }

  public IEnumerator onBackSceneAsync(bool serverSync)
  {
    Explore033TopScene explore033TopScene = this;
    CommonRoot instance = Singleton<CommonRoot>.GetInstance();
    instance.isActiveBackground = false;
    if (Singleton<ExploreSceneManager>.GetInstance().ReloadDirty)
      instance.isLoading = true;
    if (instance.guildChatManager.GetCurrentGuildChatStatus() == GuildChatManager.GuildChatStatus.DetailedView)
      explore033TopScene.currentSceneGuildChatDisplayingStatus = NGSceneBase.GuildChatDisplayingStatus.NotChanged;
    yield return (object) explore033TopScene.menu.onBackSceneAsync();
  }

  public void onBackScene(bool serverSync)
  {
    this.setupExploreRenderSetting(true);
    this.currentSceneGuildChatDisplayingStatus = NGSceneBase.GuildChatDisplayingStatus.Closed;
    Singleton<NGSceneManager>.GetInstance().waitSceneAction((Action) (() =>
    {
      Singleton<ExploreSceneManager>.GetInstance().OnBackExploreScene();
      ExploreDataManager instance = Singleton<ExploreDataManager>.GetInstance();
      if (instance.IsRankingPeriodFinished || Singleton<ExploreSceneManager>.GetInstance().ReloadDirty)
        Singleton<ExploreSceneManager>.GetInstance().Pause(false);
      else if (instance.IsReopenPopup)
      {
        this.StartCoroutine(this.menu.ReopenPopup());
      }
      else
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<ExploreSceneManager>.GetInstance().Pause(false);
      }
    }));
  }

  public override void onEndScene()
  {
    Singleton<ExploreSceneManager>.GetInstance().OnEndExploreScene();
  }

  public override IEnumerator onDestroySceneAsync()
  {
    this.setupExploreRenderSetting(false);
    ExploreSceneManager instance = Singleton<ExploreSceneManager>.GetInstance();
    ExploreDataManager dataMgr = Singleton<ExploreDataManager>.GetInstance();
    instance.OnDestoryExploreScene();
    bool saveFailed = false;
    bool flag = false;
    if (Singleton<NGGameDataManager>.GetInstance().IsEarth || Singleton<NGBattleManager>.GetInstance().isEarth)
      flag = true;
    else if (Object.op_Implicit((Object) Singleton<EarthDataManager>.GetInstance()) && Singleton<EarthDataManager>.GetInstance().isPrologue)
      flag = true;
    if (!instance.ReloadDirty && !dataMgr.IsRankingAcceptanceFinish && !flag)
      yield return (object) dataMgr.SaveSuspendData((Action) (() => saveFailed = true));
    Singleton<NGDuelDataManager>.GetInstance().Init();
    ExploreSceneManager.DestroyInstance();
    dataMgr.ClearCache();
    yield return (object) base.onDestroySceneAsync();
    if (saveFailed)
    {
      dataMgr.InitReopenPopupState();
      Singleton<NGSceneManager>.GetInstance().ChangeErrorPage();
    }
  }

  private void setupExploreRenderSetting(bool enable)
  {
    if (enable)
    {
      ExploreFloor floorData = Singleton<ExploreDataManager>.GetInstance().FloorData;
      RenderSettings.ambientLight = Consts.GetInstance().UI3DMODEL_AMBIENT_COLOR;
      RenderSettings.fog = true;
      RenderSettings.fogMode = (FogMode) 1;
      RenderSettings.fogStartDistance = 120f;
      RenderSettings.fogEndDistance = 180f;
      for (int index = 0; index < this.menu.mFogSetting.Length; ++index)
      {
        if (floorData.folder_path == this.menu.mFogSetting[index].folderPath)
        {
          RenderSettings.fogStartDistance = this.menu.mFogSetting[index].startDistance;
          RenderSettings.fogEndDistance = this.menu.mFogSetting[index].endDistance;
        }
      }
    }
    else
      RenderSettings.fog = false;
  }
}
