// Decompiled with JetBrains decompiler
// Type: Earth.EarthQuestProgress
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace Earth
{
  [Serializable]
  public class EarthQuestProgress : BL.ModelBase
  {
    public const int ForcedSortieCharacterMinIndex = 1;
    private int mCurrentStageIndex;
    private bool mIsCleared;
    private bool mIsRead;
    private int mPrologueIndex;
    private static readonly string serverDataFormat = "{{\"current_stage_index\":{0},\"is_clear\":{1},\"is_read\":{2},\"prolouge_index\":{3}}}";

    public int currentStageIndex => this.mCurrentStageIndex;

    public EarthQuestEpisode currentEpisode
    {
      get
      {
        return ((IEnumerable<EarthQuestEpisode>) MasterData.EarthQuestEpisodeList).Where<EarthQuestEpisode>((Func<EarthQuestEpisode, bool>) (x => x.stage_index == this.mCurrentStageIndex)).FirstOrDefault<EarthQuestEpisode>();
      }
    }

    public List<EarthForcedSortieCharacter> forcedSortieCharacters
    {
      get
      {
        return ((IEnumerable<EarthForcedSortieCharacter>) MasterData.EarthForcedSortieCharacterList).Where<EarthForcedSortieCharacter>((Func<EarthForcedSortieCharacter, bool>) (x => x.episode.ID == this.currentEpisode.ID)).ToList<EarthForcedSortieCharacter>();
      }
    }

    public int forcedSortieCharacterMaxPosition
    {
      get
      {
        return this.forcedSortieCharacters.Count > 0 ? this.forcedSortieCharacters.Max<EarthForcedSortieCharacter>((Func<EarthForcedSortieCharacter, int>) (x => x.sortie_position)) : 1;
      }
    }

    public List<EarthImpossibleOFSortieCharacter> impossibleOfSortieCharacter
    {
      get
      {
        return ((IEnumerable<EarthImpossibleOFSortieCharacter>) MasterData.EarthImpossibleOFSortieCharacterList).Where<EarthImpossibleOFSortieCharacter>((Func<EarthImpossibleOFSortieCharacter, bool>) (x => x.episode.ID == this.currentEpisode.ID)).ToList<EarthImpossibleOFSortieCharacter>();
      }
    }

    public int MaximumNumberOfSorties => this.currentEpisode.stage.Players.Length;

    public bool isRead
    {
      get => this.mIsRead;
      set => this.mIsRead = value;
    }

    public int prologueIndex
    {
      get => this.mPrologueIndex;
      set
      {
        if (value < 1)
          this.mPrologueIndex = 1;
        else
          this.mPrologueIndex = value;
      }
    }

    public void SetPrologue(bool enabled)
    {
      if (enabled)
        this.mPrologueIndex = 1;
      else
        this.mPrologueIndex = ((IEnumerable<EarthQuestPologue>) MasterData.EarthQuestPologueList).Max<EarthQuestPologue>((Func<EarthQuestPologue, int>) (x => x.number)) + 1;
    }

    public bool isPrologue
    {
      get
      {
        return this.mPrologueIndex <= ((IEnumerable<EarthQuestPologue>) MasterData.EarthQuestPologueList).Max<EarthQuestPologue>((Func<EarthQuestPologue, int>) (x => x.number));
      }
    }

    public bool isCleared => this.mIsCleared;

    public static EarthQuestProgress Create()
    {
      EarthQuestProgress earthQuestProgress = new EarthQuestProgress();
      earthQuestProgress.mCurrentStageIndex = ((IEnumerable<EarthQuestEpisode>) MasterData.EarthQuestEpisodeList).OrderBy<EarthQuestEpisode, int>((Func<EarthQuestEpisode, int>) (x => x.stage_index)).First<EarthQuestEpisode>().stage_index;
      earthQuestProgress.LoadMasterData();
      earthQuestProgress.mIsCleared = false;
      earthQuestProgress.mIsRead = false;
      earthQuestProgress.mPrologueIndex = 1;
      earthQuestProgress.commit();
      return earthQuestProgress;
    }

    public EarthQuestPologue GetPrologueData()
    {
      return ((IEnumerable<EarthQuestPologue>) MasterData.EarthQuestPologueList).FirstOrDefault<EarthQuestPologue>((Func<EarthQuestPologue, bool>) (x => x.number == this.mPrologueIndex));
    }

    public void GoPrologueScene(bool isCloudAnime = false)
    {
      EarthQuestPologue prologueData = this.GetPrologueData();
      CommonRoot instance = Singleton<CommonRoot>.GetInstance();
      switch (instance.loadingMode)
      {
        case 1:
        case 2:
          instance.isLoading = false;
          instance.loadingMode = 3;
          break;
      }
      instance.isLoading = true;
      switch (prologueData.type)
      {
        case "battle":
          instance.StartCoroutine(this.ChangeBattle());
          break;
        case "movie":
          Prologue0501Scene.ChangeScene(false);
          break;
      }
    }

    private IEnumerator ChangeBattle()
    {
      NGBattleManager bm = Singleton<NGBattleManager>.GetInstance();
      while (bm.initialized)
        yield return (object) null;
      IEnumerator e = Singleton<EarthDataManager>.GetInstance().BattleInitStory(false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }

    public void CheckNextStage()
    {
      int nextStageIndex = this.mCurrentStageIndex + 1;
      EarthQuestEpisode earthQuestEpisode = ((IEnumerable<EarthQuestEpisode>) MasterData.EarthQuestEpisodeList).Where<EarthQuestEpisode>((Func<EarthQuestEpisode, bool>) (x => nextStageIndex == x.stage_index)).FirstOrDefault<EarthQuestEpisode>();
      if (earthQuestEpisode != null)
      {
        this.mCurrentStageIndex = earthQuestEpisode.stage_index;
        this.LoadMasterData();
        this.mIsRead = false;
        this.mIsCleared = false;
        this.commit();
      }
      else
      {
        if (this.mIsCleared)
          return;
        this.mIsCleared = true;
        this.commit();
      }
    }

    public void QuestClear() => this.CheckNextStage();

    public bool isImpossibleOfSortie(int character_id)
    {
      return this.impossibleOfSortieCharacter.Any<EarthImpossibleOFSortieCharacter>((Func<EarthImpossibleOFSortieCharacter, bool>) (x => x.character_id == character_id));
    }

    public string GetSeverString()
    {
      return string.Format(EarthQuestProgress.serverDataFormat, (object) this.mCurrentStageIndex, (object) (this.mIsCleared ? 1 : 0), (object) (this.mIsRead ? 1 : 0), (object) this.mPrologueIndex);
    }

    public static EarthQuestProgress JsonLoad(
      Dictionary<string, object> json,
      Action nextStageOpenCallback)
    {
      EarthQuestProgress progress = new EarthQuestProgress();
      progress.mCurrentStageIndex = (int) (long) json["current_stage_index"];
      progress.mIsCleared = (int) (long) json["is_clear"] != 0;
      if (json.ContainsKey("is_read"))
        progress.mIsRead = (int) (long) json["is_read"] != 0;
      progress.mPrologueIndex = !json.ContainsKey("prolouge_index") ? 1 : (int) (long) json["prolouge_index"];
      ((IEnumerable<EarthQuestEpisode>) MasterData.EarthQuestEpisodeList).FirstOrDefault<EarthQuestEpisode>((Func<EarthQuestEpisode, bool>) (x => x.stage_index == progress.mCurrentStageIndex));
      progress.LoadMasterData();
      if (progress.isCleared)
      {
        progress.CheckNextStage();
        if (!progress.isCleared)
          nextStageOpenCallback();
      }
      return progress;
    }

    private IEnumerator InternalLoadMasterData()
    {
      IEnumerator e = MasterData.LoadBattleStageEnemy(this.currentEpisode.stage);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = MasterData.LoadBattleMapLandform(this.currentEpisode.stage.map);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }

    private void LoadMasterData()
    {
      if (this.currentEpisode == null)
        return;
      Singleton<EarthDataManager>.GetInstance().StartCoroutine(this.InternalLoadMasterData());
    }

    public void SetCurrentIndex(int index)
    {
      this.mCurrentStageIndex = index;
      this.LoadMasterData();
      this.mIsCleared = false;
      this.mIsRead = false;
      this.commit();
    }
  }
}
