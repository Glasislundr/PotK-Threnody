// Decompiled with JetBrains decompiler
// Type: Persist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using CustomDeck;
using Earth;
using GameCore;
using GameCore.Serialization;
using LocaleTimeZone;
using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using System.IO;
using UniLinq;
using UnityEngine;

#nullable disable
public static class Persist
{
  public static Persist<Persist.Auth> auth = new Persist<Persist.Auth>("auth.dat");
  public static Persist<Persist.SortOrder> sortOrder = new Persist<Persist.SortOrder>("order.dat");
  public static Persist<Persist.GuideUnitSortAndFilter> guidUnitSortAndFilter = new Persist<Persist.GuideUnitSortAndFilter>("guidUnitSortAndFilter.dat");
  public static Persist<Persist.GuideEnemySortAndFilter> guidEnemySortAndFilter = new Persist<Persist.GuideEnemySortAndFilter>("guidEnemySortAndFilter.dat");
  public static Persist<Persist.GuideGearSortAndFilter> guidGearSortAndFilter = new Persist<Persist.GuideGearSortAndFilter>("guidGearSortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> unit00410SortAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("unit00410SortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> unit00411SortAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("unit00411SortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> unit00412SortAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("unit00412SortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> unit00468SortAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("unit00468SortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> unit0048SortAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("unit0048SortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> unit00481SortAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("unit00481SortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> unit00491SortAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("unit00491SortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> unit004912SortAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("unit004912SortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> unit004ReincarnationTypeAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("unit004ReincarnationTypeSortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> unit004431SortAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("unit004431SortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> unit00486SortAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("unit00486SortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> unit00487SortAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("unit00487SortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> unit005411SortAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("unit005411SortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> unit005468SortAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("unit005468SortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> tower029UnitListSortAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("tower029UnitListSortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> unit004ExtraSkillEquipUnitListSortAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("unit004ExtraSkillEquipUnitListSortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> unit004StorageSortAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("unit004StorageSortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> unit004JobChangeUnitSelectSortAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("unit004JobChangeUnitSelectSortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> mypageEditorSortAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("mypageEditorSortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> friendSupportSortAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("friendSupportSortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> unit004OverkillersSlotUnitSortAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("unit004OverkillersSlotUnitSortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> unit004RegressionSortAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("unit004RegressionSortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> unit004UnitTrainingListSortAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("unit004UnitTrainingListSortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> quest002201SortAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("quest002201SortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> corpsUnitListSortAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("corpsUnitListSortAndFilter.dat");
  public static Persist<Persist.UnitSortAndFilterInfo> unitLumpToutaSortAndFilter = new Persist<Persist.UnitSortAndFilterInfo>("unitLumpToutaSortAndFilter.dat");
  public static Persist<Persist.EmblemSortAndFilterInfo> emblemSortAndFilter = new Persist<Persist.EmblemSortAndFilterInfo>("emblemSortAndFilter.dat");
  public static Persist<Persist.RecipeSortAndFilterInfo> bugu00510SortAndFilter = new Persist<Persist.RecipeSortAndFilterInfo>("bugu00510SortAndFilter.dat");
  public static Persist<Persist.ItemSortAndFilterInfo> bugu0052SortAndFilter = new Persist<Persist.ItemSortAndFilterInfo>("bugu0052SortAndFilter.dat");
  public static Persist<Persist.ItemSortAndFilterInfo> bugu005SupplyListSortAndFilter = new Persist<Persist.ItemSortAndFilterInfo>("bugu005SupplyListSortAndFilter.dat");
  public static Persist<Persist.ItemSortAndFilterInfo> bugu005MaterialListSortAndFilter = new Persist<Persist.ItemSortAndFilterInfo>("bugu005MaterialListSortAndFilter.dat");
  public static Persist<Persist.ItemSortAndFilterInfo> bugu005WeaponMaterialListSortAndFilter = new Persist<Persist.ItemSortAndFilterInfo>("bugu005WeaponMaterialListSortAndFilter.dat");
  public static Persist<Persist.ItemSortAndFilterInfo> bugu0052CompositeSortAndFilter = new Persist<Persist.ItemSortAndFilterInfo>("bugu0052CompositeSortAndFilter.dat");
  public static Persist<Persist.ItemSortAndFilterInfo> bugu0052RepairSortAndFilter = new Persist<Persist.ItemSortAndFilterInfo>("bugu0052RepairSortAndFilter.dat");
  public static Persist<Persist.ItemSortAndFilterInfo> bugu0052SellSortAndFilter = new Persist<Persist.ItemSortAndFilterInfo>("bugu0052RepairSortAndFilter.dat");
  public static Persist<Persist.ItemSortAndFilterInfo> bugu0052DrillingBaseSortAndFilter = new Persist<Persist.ItemSortAndFilterInfo>("bugu0052DrillingBaseSortAndFilter.dat");
  public static Persist<Persist.ItemSortAndFilterInfo> bugu0052DrillingMaterialSortAndFilter = new Persist<Persist.ItemSortAndFilterInfo>("bugu0052DrillingMaterialSortAndFilter.dat");
  public static Persist<Persist.ItemSortAndFilterInfo> bugu0052BuildupBaseSortAndFilter = new Persist<Persist.ItemSortAndFilterInfo>("bugu0052BuildupBaseSortAndFilter.dat");
  public static Persist<Persist.ItemSortAndFilterInfo> bugu0052BuildupMaterialSortAndFilter = new Persist<Persist.ItemSortAndFilterInfo>("bugu0052BuildupMaterialSortAndFilter.dat");
  public static Persist<Persist.ItemSortAndFilterInfo> unit0044SortAndFilter = new Persist<Persist.ItemSortAndFilterInfo>("unit0044SortAndFilter.dat");
  public static Persist<Persist.ItemSortAndFilterInfo> bugu0552SortAndFilter = new Persist<Persist.ItemSortAndFilterInfo>("bugu0552SortAndFilter.dat");
  public static Persist<Persist.ItemSortAndFilterInfo> bugu055SellSortAndFilter = new Persist<Persist.ItemSortAndFilterInfo>("bugu055SellSortAndFilter.dat");
  public static Persist<Persist.ItemSortAndFilterInfo> bugu0544SortAndFilter = new Persist<Persist.ItemSortAndFilterInfo>("bugu0544SortAndFilter.dat");
  public static Persist<Persist.ItemSortAndFilterInfo> bugu005ReisouMixerSortAndFilter = new Persist<Persist.ItemSortAndFilterInfo>("bugu005ReisouMixerSortAndFilter.dat");
  public static Persist<Persist.ReisouSortAndFilterInfo> bugu005ReisouListSortAndFilter = new Persist<Persist.ReisouSortAndFilterInfo>("bugu005ReisouListSortAndFilter.dat");
  public static Persist<Persist.ExtraSkillSortAndFilterInfo> unit004ExtraSkillSortAndFilter = new Persist<Persist.ExtraSkillSortAndFilterInfo>("unit004ExtraSkillSortAndFilter.dat");
  public static Persist<Persist.ExtraSkillSortAndFilterInfo> unit004ExtraSkillEquipListSortAndFilter = new Persist<Persist.ExtraSkillSortAndFilterInfo>("unit004ExtraSkillEquipListSortAndFilter.dat");
  public static Persist<Persist.GuildMemberSortInfo> guildMemberListSort = new Persist<Persist.GuildMemberSortInfo>("guildMemberListSort.dat");
  public static Persist<Persist.EmblemSortCategory> emblemSortCategory = new Persist<Persist.EmblemSortCategory>("emblemSortCategory.dat");
  public static Persist<Persist.CharacterQuestFilterInfo> characterQuestFilterInfo = new Persist<Persist.CharacterQuestFilterInfo>("characterQuestFilterInfo.dat");
  public static Persist<Persist.InfoUnRead> infoUnRead = new Persist<Persist.InfoUnRead>("infoUnRead.dat");
  public static Persist<Persist.LastInfoTime> lastInfoTime = new Persist<Persist.LastInfoTime>("lastInfoTime.dat");
  public static Persist<Persist.LastAccessTime> lastAccessTime = new Persist<Persist.LastAccessTime>("lastAccessTime.dat");
  public static Persist<Persist.NoticeReadCheck> noticeReadCheck = new Persist<Persist.NoticeReadCheck>("noticeReadCheck.dat");
  public static Persist<Persist.UserPolicy> userPolicy = new Persist<Persist.UserPolicy>("userPolicy.dat");
  public static Persist<Persist.GuildHeaderChat> guildHeaderChat = new Persist<Persist.GuildHeaderChat>("guildHeaderChat.dat");
  public static Persist<Persist.GuildEventCheck> guildEventCheck = new Persist<Persist.GuildEventCheck>("guildEventCheck.dat");
  public static Persist<Persist.Volume> volume = new Persist<Persist.Volume>("volume.dat");
  public static Persist<Persist.AppFPS> appFPS = new Persist<Persist.AppFPS>("app_fps.dat");
  public static Persist<Persist.Notification> notification = new Persist<Persist.Notification>("notification.dat");
  public static Persist<Persist.PushNotification> pushnotification = new Persist<Persist.PushNotification>("pushnotification.dat");
  public static Persist<BE> battleEnvironment = new Persist<BE>("be.dat");
  public static Persist<Persist.Tutorial> tutorial = new Persist<Persist.Tutorial>("tutorial.dat");
  public static Persist<Persist.NewTutorial> newTutorial = new Persist<Persist.NewTutorial>("newtutorial.dat");
  public static Persist<Persist.IntegralNoaTutorial> integralNoaTutorial = new Persist<Persist.IntegralNoaTutorial>("integralNoaTutorial.dat");
  public static Persist<Persist.NewTutorialGacha> newTutorialGacha = new Persist<Persist.NewTutorialGacha>("newtutorialgacha.dat");
  public static Persist<Persist.QuestLastSortie> lastsortie = new Persist<Persist.QuestLastSortie>("lastsortie.dat");
  public static Persist<Persist.SeaQuestLastSortie> seaLastSortie = new Persist<Persist.SeaQuestLastSortie>("seaLastSortie.dat");
  public static Persist<Persist.EventQuestExplanation> explanation = new Persist<Persist.EventQuestExplanation>("explanation.dat");
  public static Persist<Persist.Duel> duel = new Persist<Persist.Duel>("duel.dat");
  public static Persist<Persist.Battle> battle = new Persist<Persist.Battle>("battle.dat");
  public static Persist<Persist.BattleIcon> battleIcon = new Persist<Persist.BattleIcon>("battleIcon.dat");
  public static Persist<Persist.dangerAreaIcon> dangerousAreaIcon = new Persist<Persist.dangerAreaIcon>("dangerAreaIcon.dat");
  public static Persist<Persist.BattleNoDuel> battleNoDuel = new Persist<Persist.BattleNoDuel>("battleNoDuel.dat");
  public static Persist<Persist.OpeningMovie> opmovie = new Persist<Persist.OpeningMovie>("opmovie.dat");
  public static Persist<Persist.CustomDeckTutorial> customDeckTutorial = new Persist<Persist.CustomDeckTutorial>("customDeckTutorial.dat");
  public static Persist<Persist.DeckOrganized> deckOrganized = new Persist<Persist.DeckOrganized>("deckOrganized.dat");
  public static Persist<Persist.DeckOrganized> seaDeckOrganized = new Persist<Persist.DeckOrganized>("seaDeckOrganized.dat");
  public static Persist<Persist.ColosseumDeckOrganized> colosseumDeckOrganized = new Persist<Persist.ColosseumDeckOrganized>("colosseimDeckOrganized.dat");
  public static Persist<Persist.ColosseumTransactionID> colosseumEnv = new Persist<Persist.ColosseumTransactionID>("colosseum.dat");
  public static Persist<Persist.ColosseumOpen> colosseumOpen = new Persist<Persist.ColosseumOpen>("colosseumOpen.dat");
  public static Persist<Persist.ColosseumTutorial> colosseumTutorial = new Persist<Persist.ColosseumTutorial>("colosseumTutorial.dat");
  public static Persist<Persist.VersusDeckOrganized> versusDeckOrganized = new Persist<Persist.VersusDeckOrganized>("versusDeckOrganized.dat");
  public static Persist<Persist.PvPInfo> pvpInfo = new Persist<Persist.PvPInfo>("pvpInfo.dat");
  public static Persist<Persist.PvPSuspend> pvpSuspend = new Persist<Persist.PvPSuspend>("pvpSuspend.dat");
  public static Persist<Persist.CacheInfo> cacheInfo = new Persist<Persist.CacheInfo>("cacheInfo.dat");
  public static Persist<Persist.PvpUnitPositions> pvpUnitPositions_order1 = new Persist<Persist.PvpUnitPositions>("pvpUnitPositions_order1.dat");
  public static Persist<Persist.PvpUnitPositions> pvpUnitPositions_order2 = new Persist<Persist.PvpUnitPositions>("pvpUnitPositions_order2.dat");
  public static Persist<EarthDataManager.EarthData> oldEarthData = new Persist<EarthDataManager.EarthData>("earth.dat");
  public static Persist<EarthDataManager.EarthDataNew> earthData = new Persist<EarthDataManager.EarthDataNew>("earth_new.dat");
  public static Persist<BE> earthBattleEnvironment = new Persist<BE>("earthbe.dat");
  public static Persist<Persist.IntegralNoahProcess> integralNoahProcess = new Persist<Persist.IntegralNoahProcess>("integralNoahProcess.dat");
  public static Persist<Persist.EverAfterProcess> everAfterProcess = new Persist<Persist.EverAfterProcess>("everAfterProcess.dat");
  public static Persist<Persist.PvPRankMatch> pvpRankMatch = new Persist<Persist.PvPRankMatch>("pvpRankMatch.dat");
  public static Persist<Persist.MissionHistory> missionHistory = new Persist<Persist.MissionHistory>("missionHistory.dat");
  public static Persist<Persist.EventStoryPlay> eventStoryPlay = new Persist<Persist.EventStoryPlay>("eventStoryPlay.dat");
  public static Persist<Persist.GuildSettingInfo> guildSetting = new Persist<Persist.GuildSettingInfo>("guildSettingInfo.dat");
  public static Persist<Persist.GuildBankSettingInfo> guildBankSetting = new Persist<Persist.GuildBankSettingInfo>("guildBankSettingInfo.dat");
  public static Persist<Persist.TowerSettingInfo> towerSetting = new Persist<Persist.TowerSettingInfo>("towerSettingInfo.dat");
  public static Persist<Persist.CorpsSettingInfo> corpsSetting = new Persist<Persist.CorpsSettingInfo>("corpsSettingInfo.dat");
  public static Persist<BE> gvgBattleEnvironment = new Persist<BE>("gvgbe.dat");
  public static Persist<Persist.GuildTopLevel> guildTopLevel = new Persist<Persist.GuildTopLevel>("guildTopLevel.dat");
  public static Persist<Persist.GuildBattleUser> guildBattleUser = new Persist<Persist.GuildBattleUser>("guildBattleUser.dat");
  public static Persist<Persist.GuildOverkillersAlertLog> guildOverkillersAlertLog = new Persist<Persist.GuildOverkillersAlertLog>("guildOverkillersAlertLog.dat");
  public static Persist<Persist.GuildRaidLastSortie> guildRaidLastSortie = new Persist<Persist.GuildRaidLastSortie>("guildRaidLastSortie.dat");
  public static Persist<Persist.GuildRaidProgress> guildRaidProgress = new Persist<Persist.GuildRaidProgress>("guildRaidProgress.dat");
  public static Persist<Persist.ExploreRankingInfo> exploreRankingInfo = new Persist<Persist.ExploreRankingInfo>("exploreRankingInfo.dat");
  public static Persist<Persist.UserInfo> userInfo = new Persist<Persist.UserInfo>("userInfo.dat");
  public static Persist<Persist.AutoBattleSetting> autoBattleSetting = new Persist<Persist.AutoBattleSetting>("autobattlesetting.dat");
  public static Persist<Persist.BattleTimeSetting> battleTimeSetting = new Persist<Persist.BattleTimeSetting>("battletimesetting.dat");
  public static Persist<Persist.BattleTouchWait> battleTouchWait = new Persist<Persist.BattleTouchWait>("battletouchwait.dat");
  public static Persist<Persist.BattleSkillUseConfirmation> battleSkillUseConfirmation = new Persist<Persist.BattleSkillUseConfirmation>("battleskilluseconfirmation.dat");
  public static Persist<Persist.SeaHomeUnitDate> seaHomeUnitDate = new Persist<Persist.SeaHomeUnitDate>("seahomeunitdata.dat");
  public static Persist<Persist.SeaTutorialData> seaTutorialData = new Persist<Persist.SeaTutorialData>("seatutorialdata.dat");
  public static Persist<Persist.StoryModePopupInfo> storyModePopupInfo = new Persist<Persist.StoryModePopupInfo>("storymodepopupinfo.dat");
  public static Persist<Persist.TitlePermissionAsk> titlePermissionAsk = new Persist<Persist.TitlePermissionAsk>("titlepermissionask.dat");
  public static Persist<Persist.AppReview> appReview = new Persist<Persist.AppReview>("appReview.dat");
  public static Persist<Persist.NormalDLC> normalDLC = new Persist<Persist.NormalDLC>("normalDLC.dat");
  public static Persist<Persist.SpeedPriority> speedPriority = new Persist<Persist.SpeedPriority>("selectGraphicMode.dat");
  public static Persist<Persist.MypageUnitId> mypageUnitId = new Persist<Persist.MypageUnitId>("mypage_unit_id.dat");
  public static Persist<Persist.RaidStoryAlreadyRead> raidStoryAlreadyRead = new Persist<Persist.RaidStoryAlreadyRead>("raidStoryAlreadyRead.dat");
  public static Persist<Persist.StoryOptions> storyOptions = new Persist<Persist.StoryOptions>("storyOptions.dat");
  public static Persist<Persist.JobXInfo> jobXInfo = new Persist<Persist.JobXInfo>("jobXInfo.dat");
  public static Persist<Persist.FileMoved> fileMoved = new Persist<Persist.FileMoved>("fileMoved.dat");
  public static Persist<Persist.JukeBox> jukeBox = new Persist<Persist.JukeBox>("jukeBox.dat");

  public static List<Persist.ISaveUnit> saves { get; private set; } = (List<Persist.ISaveUnit>) null;

  public static void DeleteAll(IEnumerable<Persist.ISaveUnit> lstDelete = null)
  {
    if (lstDelete == null)
      lstDelete = (IEnumerable<Persist.ISaveUnit>) Persist.saves;
    foreach (Persist.ISaveUnit isaveUnit in lstDelete)
      isaveUnit.DeleteAndClear();
  }

  public static void EndTutorial()
  {
    Persist.tutorial.Data.SetTutorialFinish();
    Persist.tutorial.Flush();
    Persist.integralNoaTutorial.Data.SetTutorialFinish();
    Persist.integralNoaTutorial.Data.beginnersQuest = false;
    Persist.integralNoaTutorial.Flush();
    Persist.newTutorialGacha.Data.clearGachaResult();
    Persist.newTutorialGacha.Flush();
  }

  public abstract class ISaveUnit
  {
    public string fileName;
    private static HashSet<char> permitChars = new HashSet<char>((IEnumerable<char>) "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_-.");

    public ISaveUnit(string fileName)
    {
      this.fileName = fileName;
      if (Persist.saves == null)
        Persist.saves = new List<Persist.ISaveUnit>(130);
      Persist.saves.Add(this);
    }

    private static string Sanitize(string str)
    {
      return str.Select<char, char>((Func<char, char>) (c => !Persist.ISaveUnit.permitChars.Contains(c) ? '_' : c)).ToStringForChars();
    }

    public string FilePath
    {
      get => Path.Combine(PersistentPath.Value, Persist.ISaveUnit.Sanitize(this.fileName));
    }

    public string FileFallbackPath
    {
      get => Path.Combine(PersistentPath.Fallback, Persist.ISaveUnit.Sanitize(this.fileName));
    }

    public bool Exists => File.Exists(this.FilePath);

    public void Delete()
    {
      if (!File.Exists(this.FilePath))
        return;
      File.Delete(this.FilePath);
    }

    public void DeleteAndClear()
    {
      this.Delete();
      this.Clear();
    }

    protected object DeserializeObjectFromFile(string filePath)
    {
      object obj = (object) null;
      try
      {
        obj = EasySerializer.DeserializeObjectFromFile(filePath);
      }
      catch (Exception ex)
      {
        if (filePath.Contains(Persist.ISaveUnit.Sanitize(Persist.auth.fileName)))
          throw ex;
      }
      return obj;
    }

    public abstract void NewData();

    public abstract void Clear();

    public abstract void Flush();
  }

  [Serializable]
  public class Auth
  {
    public string UUID;
    public string SecretKey;
    public string DeviceID;

    public Auth() => this.setDefault();

    public void ResetAllAuthInfo()
    {
      this.UUID = (string) null;
      this.SecretKey = (string) null;
      this.DeviceID = (string) null;
      this.setDefault();
    }

    public bool IsNeedAuthRegister() => this.DeviceID == "";

    private void setDefault()
    {
      if (string.IsNullOrEmpty(this.UUID))
      {
        this.UUID = SystemInfo.deviceUniqueIdentifier;
        Debug.Log((object) ("config default set UUID = " + this.UUID));
      }
      if (string.IsNullOrEmpty(this.SecretKey))
      {
        this.SecretKey = Guid.NewGuid().ToString();
        Debug.Log((object) ("config default set SecretKey = " + this.SecretKey));
      }
      if (!string.IsNullOrEmpty(this.DeviceID))
        return;
      this.DeviceID = "";
    }
  }

  [Serializable]
  public class SortOrder
  {
    public int Weapon = 2;
    public int Unit = 4;
    public int Friend;
    public int GuildMember;
  }

  [Serializable]
  public class GuideUnitSortAndFilter
  {
    public List<GearKindEnum> gearKindEnumList = new List<GearKindEnum>();
    public List<int> unitFamilyOrNullList = new List<int>();
    public List<GuideSortAndFilter.GUIDE_CATEGORY_TYPE> unitCategoryList = new List<GuideSortAndFilter.GUIDE_CATEGORY_TYPE>();
    public GuideSortAndFilter.GUIDE_SORT_TYPE sortType = GuideSortAndFilter.GUIDE_SORT_TYPE.NUMBER;
    public GuideSortAndFilter.GUIDE_ORDER_BUY_SORT_TYPE orderBuySort = GuideSortAndFilter.GUIDE_ORDER_BUY_SORT_TYPE.BACK;
  }

  [Serializable]
  public class GuideEnemySortAndFilter
  {
    public List<GearKindEnum> gearKindEnumList = new List<GearKindEnum>();
    public List<int> unitFamilyOrNullList = new List<int>();
    public GuideSortAndFilter.GUIDE_SORT_TYPE sortType = GuideSortAndFilter.GUIDE_SORT_TYPE.NUMBER;
    public GuideSortAndFilter.GUIDE_ORDER_BUY_SORT_TYPE orderBuySort = GuideSortAndFilter.GUIDE_ORDER_BUY_SORT_TYPE.BACK;
  }

  [Serializable]
  public class GuideGearSortAndFilter
  {
    public List<GearKindEnum> gearKindEnumList = new List<GearKindEnum>();
    public List<GuideSortAndFilter.GUIDE_GEAR_CATEGORY_TYPE> unitCategoryList = new List<GuideSortAndFilter.GUIDE_GEAR_CATEGORY_TYPE>();
    public GuideSortAndFilter.GUIDE_SORT_TYPE sortType = GuideSortAndFilter.GUIDE_SORT_TYPE.NUMBER;
    public GuideSortAndFilter.GUIDE_ORDER_BUY_SORT_TYPE orderBuySort = GuideSortAndFilter.GUIDE_ORDER_BUY_SORT_TYPE.BACK;
  }

  [Serializable]
  public class UnitSortAndFilterInfo
  {
    public SortAndFilter.SORT_TYPE_ORDER_BUY order;
    public UnitSortAndFilter.SORT_TYPES sortType = UnitSortAndFilter.SORT_TYPES.BranchOfAnArmy;
    public UnitSortAndFilter.ModeTypes modeType;
    public bool isBattleFirst;
    public bool isTowerEntry = true;
    public List<bool> filter = new List<bool>();
    public Dictionary<UnitGroupHead, List<int>> groupIDs;
    private bool isInit;
    private bool isInitSortType;

    public UnitSortAndFilterInfo() => this.setDefault();

    private void setDefault()
    {
      if (this.isInit)
        return;
      this.isInit = true;
      this.isInitSortType = false;
      for (int index = 0; index < 60; ++index)
        this.filter.Add(false);
      switch (Persist.sortOrder.Data.Unit)
      {
        case 0:
          this.sortType = UnitSortAndFilter.SORT_TYPES.Level;
          break;
        case 1:
          this.sortType = UnitSortAndFilter.SORT_TYPES.Rarity;
          break;
        case 3:
          this.sortType = UnitSortAndFilter.SORT_TYPES.FightingPower;
          break;
        case 5:
          this.sortType = UnitSortAndFilter.SORT_TYPES.Cost;
          break;
        default:
          this.sortType = UnitSortAndFilter.SORT_TYPES.BranchOfAnArmy;
          break;
      }
      this.groupIDs = new Dictionary<UnitGroupHead, List<int>>()
      {
        [UnitGroupHead.group_all] = new List<int>() { 2 },
        [UnitGroupHead.group_large] = new List<int>(),
        [UnitGroupHead.group_small] = new List<int>(),
        [UnitGroupHead.group_clothing] = new List<int>(),
        [UnitGroupHead.group_generation] = new List<int>()
      };
      this.isBattleFirst = true;
      this.isTowerEntry = true;
    }

    public void setInitSortType(UnitSortAndFilter.SORT_TYPES type)
    {
      if (this.isInitSortType)
        return;
      this.sortType = type;
      this.isInitSortType = true;
    }
  }

  [Serializable]
  public class EmblemSortAndFilterInfo
  {
    public SortAndFilter.SORT_TYPE_ORDER_BUY order;
    public EmblemSortAndFilter.SORT_TYPES sortType = EmblemSortAndFilter.SORT_TYPES.GetOrder;
    public EmblemSortAndFilter.ModeTypes modeType;
    public List<bool> filter = new List<bool>();
    private bool isInit;

    public EmblemSortAndFilterInfo() => this.setDefault();

    private void setDefault()
    {
      if (this.isInit)
        return;
      this.isInit = true;
      for (int index = 0; index < Enum.GetNames(typeof (EmblemSortAndFilter.FILTER_TYPES)).Length; ++index)
        this.filter.Add(true);
      this.sortType = EmblemSortAndFilter.SORT_TYPES.GetOrder;
    }
  }

  [Serializable]
  public class RecipeSortAndFilterInfo
  {
    public SortAndFilter.SORT_TYPE_ORDER_BUY order;
    public RecipeSortAndFilter.SORT_TYPES sortType = RecipeSortAndFilter.SORT_TYPES.Recommended;
    public RecipeSortAndFilter.MODE_TYPES modeType;
    public List<bool> filter = new List<bool>();
    public bool isRecipeExist = true;
    private bool isInit;

    public RecipeSortAndFilterInfo() => this.setDefault();

    private void setDefault()
    {
      if (this.isInit)
        return;
      this.isInit = true;
      for (int index = 0; index < 24; ++index)
        this.filter.Add(false);
      this.sortType = RecipeSortAndFilter.SORT_TYPES.Recommended;
      this.isRecipeExist = true;
    }
  }

  [Serializable]
  public class ItemSortAndFilterInfo
  {
    public SortAndFilter.SORT_TYPE_ORDER_BUY order;
    public ItemSortAndFilter.SORT_TYPES sortType = ItemSortAndFilter.SORT_TYPES.BranchOfWeapon;
    public ItemSortAndFilter.ModeTypes modeType;
    public List<bool> filter = new List<bool>();
    public bool isEquipFirst = true;
    private bool isInit;

    public ItemSortAndFilterInfo() => this.setDefault();

    private void setDefault()
    {
      if (this.isInit)
        return;
      this.isInit = true;
      for (int index = 0; index < 45; ++index)
        this.filter.Add(false);
      this.sortType = ItemSortAndFilter.SORT_TYPES.BranchOfWeapon;
      this.isEquipFirst = true;
    }

    public void setData(Persist.ItemSortAndFilterInfo info)
    {
      this.isInit = true;
      this.order = info.order;
      this.sortType = info.sortType;
      this.modeType = info.modeType;
      this.filter.Clear();
      foreach (bool flag in info.filter)
        this.filter.Add(flag);
      this.isEquipFirst = info.isEquipFirst;
    }

    public bool isDifference(Persist.ItemSortAndFilterInfo info)
    {
      if (this.order != info.order || this.sortType != info.sortType || this.filter.Count != info.filter.Count)
        return true;
      for (int index = 0; index < this.filter.Count; ++index)
      {
        if (this.filter[index] != info.filter[index])
          return true;
      }
      return this.isInit != info.isInit;
    }
  }

  [Serializable]
  public class ReisouSortAndFilterInfo
  {
    public SortAndFilter.SORT_TYPE_ORDER_BUY order;
    public ReisouSortAndFilter.SORT_TYPES sortType = ReisouSortAndFilter.SORT_TYPES.BranchOfWeapon;
    public ReisouSortAndFilter.ModeTypes modeType;
    public List<bool> filter = new List<bool>();
    public bool isEquipFirst = true;
    private bool isInit;

    public ReisouSortAndFilterInfo() => this.setDefault();

    private void setDefault()
    {
      if (this.isInit)
        return;
      this.isInit = true;
      for (int index = 0; index < 23; ++index)
        this.filter.Add(false);
      this.sortType = ReisouSortAndFilter.SORT_TYPES.BranchOfWeapon;
      this.isEquipFirst = true;
    }
  }

  [Serializable]
  public class ExtraSkillSortAndFilterInfo
  {
    public SortAndFilter.SORT_TYPE_ORDER_BUY order;
    public ExtraSkillSortAndFilter.SORT_TYPES sortType = ExtraSkillSortAndFilter.SORT_TYPES.Level;
    public ExtraSkillSortAndFilter.ModeTypes modeType;
    public List<bool> filter = new List<bool>();
    private bool isInit;

    public ExtraSkillSortAndFilterInfo() => this.setDefault();

    private void setDefault()
    {
      if (this.isInit)
        return;
      this.isInit = true;
      for (int index = 0; index < 25; ++index)
        this.filter.Add(false);
      this.sortType = ExtraSkillSortAndFilter.SORT_TYPES.Level;
    }
  }

  [Serializable]
  public class GuildMemberSortInfo
  {
    public GuildMemberSort.SORT_TYPES sortType = GuildMemberSort.SORT_TYPES.Contribution;
    public SortAndFilter.SORT_TYPE_ORDER_BUY order;
    private bool isInit;

    public GuildMemberSortInfo() => this.setDefault();

    private void setDefault()
    {
      if (this.isInit)
        return;
      this.isInit = true;
      switch (Persist.sortOrder.Data.GuildMember)
      {
        case 0:
          this.sortType = GuildMemberSort.SORT_TYPES.Contribution;
          break;
        case 1:
          this.sortType = GuildMemberSort.SORT_TYPES.JoinAt;
          break;
        case 2:
          this.sortType = GuildMemberSort.SORT_TYPES.LastLoginAt;
          break;
        case 3:
          this.sortType = GuildMemberSort.SORT_TYPES.Level;
          break;
      }
    }
  }

  [Serializable]
  public class EmblemSortCategory
  {
    public int category;
  }

  [Serializable]
  public class CharacterQuestFilterInfo
  {
    public Dictionary<UnitGroupHead, List<int>> groupIDs;

    public CharacterQuestFilterInfo() => this.setDefault();

    private void setDefault()
    {
      this.groupIDs = Persist.CharacterQuestFilterInfo.createMapUnitGroupHead();
    }

    public static Dictionary<UnitGroupHead, List<int>> createMapUnitGroupHead()
    {
      return new Dictionary<UnitGroupHead, List<int>>()
      {
        [UnitGroupHead.group_all] = new List<int>() { 2 },
        [UnitGroupHead.group_large] = new List<int>(),
        [UnitGroupHead.group_small] = new List<int>(),
        [UnitGroupHead.group_clothing] = new List<int>(),
        [UnitGroupHead.group_generation] = new List<int>()
      };
    }
  }

  [Serializable]
  public class InfoUnRead
  {
    private Dictionary<int, long> unRead;

    public bool GetUnRead(OfficialInformationArticle info)
    {
      if (this.unRead == null)
        this.unRead = new Dictionary<int, long>();
      long dateData = 0;
      this.unRead.TryGetValue(info.id, out dateData);
      return info.IsPast(DateTime.FromBinary(dateData));
    }

    public bool GetUnReadPostScript(OfficialInformationArticle info)
    {
      if (this.unRead == null)
        this.unRead = new Dictionary<int, long>();
      long dateData = 0;
      this.unRead.TryGetValue(info.id, out dateData);
      return info.IsPastPostscript(DateTime.FromBinary(dateData));
    }

    public void SetUnRead(OfficialInformationArticle info, DateTime accessTime)
    {
      if (this.unRead == null)
        this.unRead = new Dictionary<int, long>();
      this.unRead[info.id] = accessTime.Ticks;
    }
  }

  [Serializable]
  public class LastInfoTime
  {
    private DateTime lastInfoTime;

    public DateTime GetLastInfoTime() => this.lastInfoTime;

    public void SetLastInfoTime(DateTime infoTime) => this.lastInfoTime = infoTime;
  }

  [Serializable]
  public class LastAccessTime
  {
    public DateTime gachaRootLastAccessTime;
    public DateTime shopRootLastAccessTime;
    public DateTime zeniShopLastAccessTime;
    public DateTime rareMedalSlotLastAccessTime;
    public DateTime battleMedalShopLastAccessTime;
    private Dictionary<int, DateTime> _limitedShopLatestLoginTimes = new Dictionary<int, DateTime>();

    public Dictionary<int, DateTime> limitedShopLatestLoginTimes
    {
      get
      {
        if (this._limitedShopLatestLoginTimes == null)
          this._limitedShopLatestLoginTimes = new Dictionary<int, DateTime>();
        return this._limitedShopLatestLoginTimes;
      }
    }

    public void UpdateLimitedShopLatestLoginTimes(int bannerId)
    {
      if (this.limitedShopLatestLoginTimes.ContainsKey(bannerId))
        this.limitedShopLatestLoginTimes[bannerId] = DateTime.Now;
      else
        this.limitedShopLatestLoginTimes.Add(bannerId, DateTime.Now);
      Persist.lastAccessTime.Flush();
    }
  }

  [Serializable]
  public class NoticeReadCheck
  {
    private Dictionary<int, int> PickUpNotice = new Dictionary<int, int>();
    private DateTime PickUpNoticeTime;
    private DateTime PickUpUnitTime;

    public bool CheckPickUpNotice()
    {
      foreach (OfficialInfoPopupSchema popupPickup in Singleton<NGGameDataManager>.GetInstance().officialInfoPopup.popup_pickups)
      {
        int officialinfoId = popupPickup.officialinfo_id;
        if (popupPickup.popup_version > 99 && !this.PickUpNotice.ContainsKey(officialinfoId))
          return true;
      }
      return false;
    }

    public void UpdatePickUpNotice()
    {
      this.PickUpNotice.Clear();
      foreach (OfficialInfoPopupSchema popupPickup in Singleton<NGGameDataManager>.GetInstance().officialInfoPopup.popup_pickups)
        this.PickUpNotice[popupPickup.officialinfo_id] = popupPickup.popup_version;
    }

    public bool CheckPickUpNoticeTime() => ServerTime.NowAppTime().Day != this.PickUpNoticeTime.Day;

    public void UpdatePickUpNoticeTime() => this.PickUpNoticeTime = ServerTime.NowAppTime();

    public bool CheckPickUpUnitTime() => ServerTime.NowAppTime().Day != this.PickUpUnitTime.Day;

    public void UpdatePickUpUnitTime() => this.PickUpUnitTime = ServerTime.NowAppTime();
  }

  [Serializable]
  public class UserPolicy
  {
    private bool isUserPolicy;
    private int yyyymmdd_;

    public bool GetUserPolicy(int yyyymmdd = 0)
    {
      if (!this.isUserPolicy)
      {
        this.SetUserPolicy(true, yyyymmdd);
        Persist.userPolicy.Flush();
      }
      return this.isUserPolicy;
    }

    public bool SetUserPolicy(bool flag, int yyyymmdd = 0)
    {
      bool flag1 = this.isUserPolicy != flag;
      this.isUserPolicy = flag;
      if (yyyymmdd == 0)
      {
        flag1 |= this.yyyymmdd_ != 0;
        this.yyyymmdd_ = 0;
      }
      else if (this.yyyymmdd_ != yyyymmdd && Persist.UserPolicy.IsReleased(yyyymmdd))
      {
        this.yyyymmdd_ = yyyymmdd;
        flag1 = true;
      }
      return flag1;
    }

    public static bool IsReleased(int yyyymmdd)
    {
      string str = yyyymmdd.ToString("x8");
      TimeZoneInfo timeZone = Japan.CreateTimeZone();
      DateTime universalTime = TimeZoneInfo.ConvertTime(new DateTime(int.Parse(str.Substring(0, 4)), int.Parse(str.Substring(4, 2)), int.Parse(str.Substring(6))), timeZone).ToUniversalTime();
      return ServerTime.NowAppTimeAddDelta() >= universalTime;
    }
  }

  [Serializable]
  public class GuildHeaderChat
  {
    public bool isChatNew;
    public string chatId;
    public string latestLogId;

    public GuildHeaderChat() => this.setDefault();

    private void setDefault()
    {
      this.isChatNew = false;
      this.chatId = "0";
      this.latestLogId = "0";
    }

    public void reset() => this.setDefault();
  }

  [Serializable]
  public class GuildEventCheck
  {
    public bool isGuildRaidTransition;
    public bool isGuildBattleTransition;

    public GuildEventCheck() => this.setDefault();

    private void setDefault()
    {
      this.isGuildRaidTransition = false;
      this.isGuildBattleTransition = false;
    }

    public void reset() => this.setDefault();
  }

  [Serializable]
  public class Volume
  {
    private bool isInit;
    public float Bgm;
    public float Se;
    public float Voice;

    public Volume() => this.setDefault();

    private void setDefault()
    {
      this.isInit = false;
      if (this.isInit)
        return;
      this.Bgm = 0.9f;
      this.Se = 0.75f;
      this.Voice = 0.85f;
      this.isInit = true;
    }
  }

  [Serializable]
  public class AppFPS
  {
    public bool IsSetup;
    public int MaxFPS;

    public AppFPS() => this.setDefault();

    private void setDefault()
    {
      if (this.IsSetup)
        return;
      this.MaxFPS = 60;
      this.IsSetup = true;
    }
  }

  [Serializable]
  public class Notification
  {
    private bool isInit;
    public bool Ap;
    public bool Bp = true;
    public bool Explore = true;
    public int ExploreBoxSpan;
    public int ExploreProgSpan;

    public Notification() => this.setDefault();

    private void setDefault()
    {
      this.isInit = false;
      if (this.isInit)
        return;
      this.Ap = true;
      this.Bp = true;
      this.Explore = true;
      this.ExploreBoxSpan = 0;
      this.ExploreProgSpan = 0;
      this.isInit = true;
    }
  }

  [Serializable]
  public class PushNotification
  {
    private bool isInit;
    public bool enablePush;

    public PushNotification() => this.setDefault();

    private void setDefault()
    {
      this.isInit = false;
      if (this.isInit)
        return;
      this.enablePush = true;
      this.isInit = true;
    }
  }

  [Serializable]
  public class Tutorial
  {
    public int CurrentPage;
    public int MiniGameScore;
    public int GachaUnitId;
    public string PlayerName;
    public Dictionary<string, bool> Hints;
    public bool signupCalled;
    public DateTime ChangeNextPageAt;
    public int LastPageIndex;

    public Tutorial() => this.setDefault();

    public int DuringSeconds()
    {
      return (int) (DateTime.Now - Persist.tutorial.Data.ChangeNextPageAt).TotalSeconds;
    }

    public bool HasMiniGameScore() => this.MiniGameScore >= 0;

    public bool IsFinishTutorial() => this.CurrentPage == this.LastPageIndex;

    public bool IsNotStartTutorial() => this.CurrentPage == 0;

    public void SetPageIndex(int page)
    {
      this.CurrentPage = page;
      this.ChangeNextPageAt = DateTime.Now;
    }

    public void SetTutorialFinish()
    {
      this.CurrentPage = this.LastPageIndex;
      this.ChangeNextPageAt = DateTime.Now;
    }

    private void setDefault()
    {
      this.CurrentPage = 0;
      this.GachaUnitId = 0;
      this.MiniGameScore = -1;
      this.PlayerName = string.Empty;
      this.Hints = new Dictionary<string, bool>();
      this.signupCalled = false;
      this.LastPageIndex = int.MaxValue;
      this.ChangeNextPageAt = DateTime.Now;
    }
  }

  [Serializable]
  public class NewTutorial
  {
    public bool tutoialGacha;
    public bool beginnersQuest;
    public bool startRagnarokTutorial;

    public void SetTutorialFinish() => this.beginnersQuest = true;

    public void setDefault()
    {
      this.tutoialGacha = false;
      this.beginnersQuest = false;
      this.startRagnarokTutorial = false;
    }
  }

  [Serializable]
  public class IntegralNoaTutorial
  {
    public bool beginnersQuest;
    public bool startIntegralNoaTutorial;

    public void SetTutorialFinish() => this.beginnersQuest = true;

    public void setDefault()
    {
      this.beginnersQuest = false;
      this.startIntegralNoaTutorial = false;
    }
  }

  [Serializable]
  public class NewTutorialGacha
  {
    public string strUnitIDs;
    public string strUnitTypes;
    public string strDirectionTypes;
    public bool tutorialGacha;

    public void setDefault()
    {
      this.strUnitIDs = string.Empty;
      this.strUnitTypes = string.Empty;
      this.strDirectionTypes = string.Empty;
      this.tutorialGacha = false;
    }

    public void clearGachaResult()
    {
      this.strUnitIDs = string.Empty;
      this.strUnitTypes = string.Empty;
      this.strDirectionTypes = string.Empty;
    }
  }

  [Serializable]
  public class QuestLastSortie
  {
    public int s_id;
    public int m_id;
    public int l_id;

    public QuestLastSortie() => this.setDefault();

    public void SaveLastSortie(int s_id, int m_id, int l_id)
    {
      this.s_id = s_id;
      this.m_id = m_id;
      this.l_id = l_id;
    }

    private void setDefault()
    {
      this.s_id = 0;
      this.m_id = 0;
      this.l_id = 0;
    }
  }

  [Serializable]
  public class SeaQuestLastSortie
  {
    public Dictionary<int, int> dicXL_S;
    public int clearedS;

    public bool saveLastSortie(int xl_id, int s_id)
    {
      int num;
      if (this.dicXL_S.TryGetValue(xl_id, out num) && num == s_id)
        return false;
      this.dicXL_S[xl_id] = s_id;
      return true;
    }

    public bool saveClearedS(int s_id)
    {
      if (this.clearedS == s_id)
        return false;
      this.clearedS = s_id;
      return true;
    }

    public SeaQuestLastSortie()
    {
      this.dicXL_S = new Dictionary<int, int>();
      this.clearedS = 0;
    }
  }

  [Serializable]
  public class EventQuestExplanation
  {
    public Dictionary<int, bool> Explanation;

    public EventQuestExplanation() => this.setDefault();

    public bool IsOpen(int sceneID)
    {
      bool flag = false;
      if (this.Explanation.ContainsKey(sceneID))
        flag = this.Explanation[sceneID];
      return flag;
    }

    private void setDefault() => this.Explanation = new Dictionary<int, bool>();
  }

  [Serializable]
  public class Duel
  {
    public int speed;

    public Duel() => this.setDefault();

    private void setDefault() => this.speed = 1;
  }

  [Serializable]
  public class Battle
  {
    public int sight;

    public Battle() => this.setDefault();

    private void setDefault() => this.sight = 1;
  }

  [Serializable]
  public class BattleIcon
  {
    public bool canDisp;

    public BattleIcon() => this.setDefault();

    private void setDefault() => this.canDisp = false;
  }

  [Serializable]
  public class dangerAreaIcon
  {
    public bool canDisp;

    public dangerAreaIcon() => this.setDefault();

    private void setDefault() => this.canDisp = false;
  }

  [Serializable]
  public class BattleNoDuel
  {
    public bool noDuelScene;

    public BattleNoDuel() => this.setDefault();

    private void setDefault() => this.noDuelScene = false;
  }

  [Serializable]
  public class OpeningMovie
  {
    public bool isPlayMovie;

    public OpeningMovie() => this.setDefault();

    private void setDefault() => this.isPlayMovie = false;
  }

  [Serializable]
  public class CustomDeckTutorial
  {
    public bool isUnlocked;

    public CustomDeckTutorial() => this.setDefault();

    private void setDefault() => this.isUnlocked = false;
  }

  [Serializable]
  public class DeckOrganized
  {
    public int number;
    public int customNumber;
    public bool isCustom;

    public DeckOrganized() => this.setDefault();

    public void reset() => this.setDefault();

    private void setDefault()
    {
      this.number = 0;
      this.customNumber = 0;
      this.isCustom = false;
    }
  }

  [Serializable]
  public class SeaDeckOrganized
  {
    public int number;

    public SeaDeckOrganized() => this.setDefault();

    private void setDefault() => this.number = 0;
  }

  private static class DeckUtility
  {
    public static DeckInfo getSelectedDeck(
      bool bSelectedCustom,
      int normalDeckIndex,
      int customDeckIndex)
    {
      if (bSelectedCustom && Util.checkUnlockedPlayerLevel(Player.Current.level))
      {
        PlayerCustomDeck deck = SMManager.Get<PlayerCustomDeck[]>()[customDeckIndex];
        if (deck.player_unit_ids.Length != 0 && deck.player_unit_ids[0].IsValid())
          return PlayerCustomDeck.createDeckInfo(deck);
      }
      PlayerDeck[] playerDeckArray = SMManager.Get<PlayerDeck[]>();
      PlayerDeck playerDeck = playerDeckArray[normalDeckIndex >= 0 ? normalDeckIndex : 0];
      return PlayerDeck.createDeckInfo(playerDeck.player_unit_ids.Length == 0 || !playerDeck.player_unit_ids[0].IsValid() ? playerDeckArray[0] : playerDeck);
    }
  }

  [Serializable]
  public class ColosseumDeckOrganized
  {
    public int number = -1;
    public int customNumber;
    public bool isCustom;

    public ColosseumDeckOrganized() => this.setDefault();

    private void setDefault()
    {
      this.number = -1;
      this.customNumber = 0;
      this.isCustom = false;
    }

    public DeckInfo getSelectedDeck()
    {
      return Persist.DeckUtility.getSelectedDeck(this.isCustom, this.number, this.customNumber);
    }
  }

  [Serializable]
  public class ColosseumTransactionID
  {
    public string id;

    public ColosseumTransactionID() => this.setDefault();

    private void setDefault() => this.id = "";
  }

  [Serializable]
  public class ColosseumOpen
  {
    public bool isOpen;

    public ColosseumOpen() => this.setDefault();

    private void setDefault() => this.isOpen = false;
  }

  [Serializable]
  public class ColosseumTutorial
  {
    public int CurrentPage;

    public ColosseumTutorial() => this.setDefault();

    private void setDefault() => this.CurrentPage = 0;
  }

  [Serializable]
  public class VersusDeckOrganized
  {
    public int number;
    public int customNumber;
    public bool isCustom;

    public VersusDeckOrganized() => this.setDefault();

    private void setDefault()
    {
      this.number = 0;
      this.customNumber = 0;
      this.isCustom = false;
    }

    public DeckInfo getSelectedDeck()
    {
      return Persist.DeckUtility.getSelectedDeck(this.isCustom, this.number, this.customNumber);
    }
  }

  [Serializable]
  public class PvPInfo
  {
    public int currentPage;
    public PvpMatchingTypeEnum lastMatchingType;

    public PvPInfo() => this.setDefault();

    public void setDefault()
    {
      this.currentPage = 0;
      this.lastMatchingType = PvpMatchingTypeEnum.normal;
    }
  }

  [Serializable]
  public class PvPSuspend
  {
    public string host = "";
    public int port;
    public string token = "";
    public MpStage stage;
    public Player player;
    public Player enemy;
    public PvpMatchingTypeEnum matchingType;
    public int order;

    public PvPSuspend() => this.setDefault();

    private void setDefault()
    {
      this.order = 0;
      this.host = "";
      this.port = 0;
      this.token = "";
      this.stage = (MpStage) null;
      this.player = this.enemy = (Player) null;
      this.matchingType = PvpMatchingTypeEnum.normal;
    }
  }

  [Serializable]
  public class CacheInfo
  {
    public bool hasDeleted;

    public CacheInfo() => this.setDefault();

    private void setDefault() => this.hasDeleted = false;
  }

  [Serializable]
  public class PvpUnitPositions
  {
    public int stageId;
    public Tuple<int, int>[] positions;
    public int order;

    public PvpUnitPositions() => this.setDefault();

    private void setDefault()
    {
      this.stageId = -1;
      this.positions = new Tuple<int, int>[0];
    }

    public bool check(int stageId, int nbPositions, int order)
    {
      return this.stageId == stageId && this.positions.Length == nbPositions && this.order == order;
    }

    public bool check(MpStage stage, int nbPositions, int order)
    {
      return this.check(stage.stage_id, nbPositions, order);
    }

    public static void save(int stageId, List<BL.UnitPosition> upl, int order)
    {
      Persist.PvpUnitPositions pvpUnitPositions = order != 1 ? Persist.pvpUnitPositions_order2.Data : Persist.pvpUnitPositions_order1.Data;
      pvpUnitPositions.stageId = stageId;
      pvpUnitPositions.positions = upl.Select<BL.UnitPosition, Tuple<int, int>>((Func<BL.UnitPosition, Tuple<int, int>>) (up => new Tuple<int, int>(up.row, up.column))).ToArray<Tuple<int, int>>();
      if (order == 1)
        Persist.pvpUnitPositions_order1.Flush();
      else
        Persist.pvpUnitPositions_order2.Flush();
    }

    public static Persist.PvpUnitPositions getData(MpStage stage, int nbPositions, int order)
    {
      if (Persist.pvpUnitPositions_order1.Data.check(stage, nbPositions, order))
        return Persist.pvpUnitPositions_order1.Data;
      return Persist.pvpUnitPositions_order2.Data.check(stage, nbPositions, order) ? Persist.pvpUnitPositions_order2.Data : (Persist.PvpUnitPositions) null;
    }
  }

  [Serializable]
  public class IntegralNoahProcess
  {
    public int lastIntegralNoahSId;

    public IntegralNoahProcess() => this.setDefault();

    private void setDefault() => this.lastIntegralNoahSId = 0;
  }

  [Serializable]
  public class EverAfterProcess
  {
    public int lastEverAfterSId;

    public EverAfterProcess() => this.setDefault();

    private void setDefault() => this.lastEverAfterSId = 0;
  }

  [Serializable]
  public class PvPRankMatch
  {
    public int lastRankMatchPeriodId;

    public PvPRankMatch() => this.setDefault();

    private void setDefault() => this.lastRankMatchPeriodId = 0;
  }

  [Serializable]
  public class MissionHistory
  {
    public Persist.MissionHistory.IDList daily;
    public Persist.MissionHistory.IDList mission;

    public MissionHistory() => this.setDefault();

    public void setDefault()
    {
      this.daily = new Persist.MissionHistory.IDList();
      this.mission = new Persist.MissionHistory.IDList();
    }

    [Serializable]
    public class IDList
    {
      public DateTime? date;
      public List<int> ids = new List<int>();
    }
  }

  [Serializable]
  public class EventStoryPlay
  {
    public List<int> reserveIDList;
    private Dictionary<Persist.EventStoryPlay.ControlType, List<int>> playedIDdict;

    public EventStoryPlay() => this.setDefault();

    private void setDefault()
    {
      this.reserveIDList = new List<int>();
      this.setPlayedIDdict();
    }

    private void setPlayedIDdict()
    {
      this.playedIDdict = new Dictionary<Persist.EventStoryPlay.ControlType, List<int>>()
      {
        {
          Persist.EventStoryPlay.ControlType.Server,
          new List<int>()
        },
        {
          Persist.EventStoryPlay.ControlType.Client,
          new List<int>()
        }
      };
    }

    private bool checkServerControlScene(string sceneName)
    {
      return sceneName == "mypage" || sceneName == "quest002_17" || sceneName == "quest002_SideStory_List";
    }

    public void SetReserveList(int[] eventIds, string sceneName)
    {
      if (eventIds == null)
        return;
      bool flag = false;
      Persist.EventStoryPlay.ControlType key1 = this.checkServerControlScene(sceneName) ? Persist.EventStoryPlay.ControlType.Server : Persist.EventStoryPlay.ControlType.Client;
      if (this.playedIDdict == null)
      {
        this.setPlayedIDdict();
        flag = true;
      }
      List<int> intList1 = this.playedIDdict[key1];
      foreach (int eventId in eventIds)
      {
        if (!intList1.Contains(eventId) && !this.reserveIDList.Contains(eventId))
        {
          this.reserveIDList.Add(eventId);
          flag = true;
        }
      }
      List<int> intList2 = new List<int>();
      foreach (int reserveId in this.reserveIDList)
      {
        if (((IEnumerable<int>) eventIds).Contains<int>(reserveId) && MasterData.StoryPlaybackEventPlay.ContainsKey(reserveId))
          intList2.Add(reserveId);
      }
      if (intList2.Count != this.reserveIDList.Count)
      {
        this.reserveIDList = intList2;
        flag = true;
      }
      List<int> intList3 = new List<int>();
      foreach (int key2 in intList1)
      {
        if (((IEnumerable<int>) eventIds).Contains<int>(key2) && MasterData.StoryPlaybackEventPlay.ContainsKey(key2))
          intList3.Add(key2);
      }
      if (intList3.Count != intList1.Count)
      {
        this.playedIDdict[key1] = intList3;
        flag = true;
      }
      if (!flag)
        return;
      Persist.eventStoryPlay.Flush();
    }

    public bool isExistEventScript(string sceneName, int arg1)
    {
      return this.reserveIDList.Select<int, StoryPlaybackEventPlay>((Func<int, StoryPlaybackEventPlay>) (x => MasterData.StoryPlaybackEventPlay[x])).Any<StoryPlaybackEventPlay>((Func<StoryPlaybackEventPlay, bool>) (x =>
      {
        if (!string.IsNullOrEmpty(x.scene_name) && !sceneName.Contains(x.scene_name))
          return false;
        return !x.arg1.HasValue || x.arg1.Value == arg1;
      }));
    }

    public bool PlayEventScript(string sceneName, int arg1, bool needEndLoadingLayer = false)
    {
      StoryPlaybackEventPlay playbackEventPlay = this.reserveIDList.Select<int, StoryPlaybackEventPlay>((Func<int, StoryPlaybackEventPlay>) (x => MasterData.StoryPlaybackEventPlay[x])).FirstOrDefault<StoryPlaybackEventPlay>((Func<StoryPlaybackEventPlay, bool>) (x =>
      {
        if (!string.IsNullOrEmpty(x.scene_name) && !sceneName.Contains(x.scene_name))
          return false;
        return !x.arg1.HasValue || x.arg1.Value == arg1;
      }));
      if (playbackEventPlay != null)
      {
        this.playedIDdict[this.checkServerControlScene(sceneName) ? Persist.EventStoryPlay.ControlType.Server : Persist.EventStoryPlay.ControlType.Client].Add(playbackEventPlay.ID);
        this.reserveIDList.Remove(playbackEventPlay.ID);
        Persist.eventStoryPlay.Flush();
        Action action = (Action) (() =>
        {
          Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
          Singleton<NGSceneManager>.GetInstance().backScene();
        });
        Story0093Scene.changeScene(true, playbackEventPlay.script_id, new bool?(Singleton<NGGameDataManager>.GetInstance().IsSea), needEndLoadingLayer ? action : (Action) null);
      }
      return playbackEventPlay != null;
    }

    private enum ControlType
    {
      Server = 1,
      Client = 2,
    }
  }

  [Serializable]
  public class GuildSettingInfo
  {
    private Dictionary<GuildUtil.GuildBadgeInfoType, bool> badgeState;
    public int titleSortCategory;
    public int memberNum = -1;
    public int sightUseNumber;
    public DateTime timeTitleAppear;

    public GuildSettingInfo() => this.setDefault();

    private void setDefault()
    {
      this.resetBadge();
      this.titleSortCategory = 0;
      this.memberNum = -1;
      this.sightUseNumber = 0;
      this.timeTitleAppear = new DateTime(2000, 1, 1);
    }

    private void resetBadge()
    {
      this.badgeState = new Dictionary<GuildUtil.GuildBadgeInfoType, bool>();
      for (int key = 0; key < Enum.GetValues(typeof (GuildUtil.GuildBadgeInfoType)).Length; ++key)
        this.setBadgeState((GuildUtil.GuildBadgeInfoType) key, false);
    }

    public void setBadgeState(GuildUtil.GuildBadgeInfoType key, bool state)
    {
      if (this.badgeState == null)
        this.resetBadge();
      if (!this.badgeState.ContainsKey(key))
        this.badgeState.Add(key, state);
      else
        this.badgeState[key] = state;
    }

    public bool getBadgeState(GuildUtil.GuildBadgeInfoType key)
    {
      if (this.badgeState == null)
        this.resetBadge();
      if (!this.badgeState.ContainsKey(key))
        this.badgeState.Add(key, false);
      return this.badgeState[key];
    }

    public void reset() => this.setDefault();
  }

  [Serializable]
  public class GuildBankSettingInfo
  {
    public bool guildBankFirstTime = true;

    public void setDefault() => this.guildBankFirstTime = true;

    public void reset() => this.setDefault();
  }

  [Serializable]
  public class TowerSettingInfo
  {
    public bool isFirstTime = true;

    public void setDefault() => this.isFirstTime = true;

    public void reset() => this.setDefault();
  }

  [Serializable]
  public class CorpsSettingInfo
  {
    public bool isFirstTime = true;

    public void setDefault() => this.isFirstTime = true;

    public void reset() => this.setDefault();
  }

  [Serializable]
  public class GuildTopLevel
  {
    public string guildID = string.Empty;
    public int level = 1;

    public void setDefault()
    {
      this.guildID = string.Empty;
      this.level = 1;
    }

    public void reset() => this.setDefault();
  }

  [Serializable]
  public class GuildBattleUser
  {
    public string guildID;
    public int roleNo;
    public int gvgID;
    public int countTopIN;
    public int gvgCount;

    public GuildBattleUser() => this.setDefault();

    public void setDefault()
    {
      this.guildID = string.Empty;
      this.roleNo = 0;
      this.gvgID = 0;
      this.countTopIN = 0;
      this.gvgCount = -1;
    }

    public void reset(string id, int role, int gvgId, int count)
    {
      this.guildID = id;
      this.roleNo = role;
      this.gvgID = gvgId;
      this.countTopIN = 0;
      this.gvgCount = count;
    }
  }

  [Serializable]
  public class GuildOverkillersAlertLog
  {
    public string guildID;
    public int gvgID;
    public bool isAlertOverkillersUnits;

    public GuildOverkillersAlertLog() => this.reset();

    public void reset(string id = null, int gvgId = 0)
    {
      this.guildID = id != null ? id : string.Empty;
      this.gvgID = gvgId;
      this.isAlertOverkillersUnits = true;
    }
  }

  [Serializable]
  public class GuildRaidLastSortie
  {
    public int deckNumber;
    public int customDeckNumber;
    public bool isCustom;

    public GuildRaidLastSortie() => this.setDefault();

    private void setDefault()
    {
      this.deckNumber = 0;
      this.customDeckNumber = 0;
      this.isCustom = false;
    }
  }

  [Serializable]
  public class GuildRaidProgress
  {
    public int lastPeriodId;
    public int lastLap;
    public int lastOrder;
    public bool isLastPeriodComplete;
    public bool isStartedEndless;
    public int orderNow;
    public bool startOrderMaxAnime;
    public Dictionary<int, bool> endlessBossAnime;

    public GuildRaidProgress() => this.setDefault();

    public void setDefault()
    {
      this.lastPeriodId = 0;
      this.lastLap = 0;
      this.lastOrder = 0;
      this.orderNow = 0;
      this.isLastPeriodComplete = false;
      this.isStartedEndless = false;
      if (this.endlessBossAnime == null)
        this.endlessBossAnime = new Dictionary<int, bool>();
      this.endlessBossAnime.Clear();
    }
  }

  [Serializable]
  public class ExploreRankingInfo
  {
    public int lastPeriodId;
    public bool isResultView;

    public ExploreRankingInfo() => this.setDefault();

    public void setDefault()
    {
      this.lastPeriodId = 0;
      this.isResultView = false;
    }
  }

  [Serializable]
  public class UserInfo
  {
    public string userId = string.Empty;

    public UserInfo() => this.setDefault();

    private void setDefault() => this.userId = string.Empty;
  }

  [Serializable]
  public class AutoBattleSetting
  {
    public bool isAutoBattle;
    public bool isItemMove;
    public bool isCallSkill;

    public AutoBattleSetting() => this.setDefault();

    private void setDefault()
    {
      this.isAutoBattle = false;
      this.isItemMove = false;
      this.isCallSkill = false;
    }
  }

  [Serializable]
  public class BattleTimeSetting
  {
    public int speed;

    public BattleTimeSetting() => this.setDefault();

    private void setDefault() => this.speed = 3;
  }

  [Serializable]
  public class BattleTouchWait
  {
    public bool isTouchWait;

    public BattleTouchWait() => this.setDefault();

    private void setDefault() => this.isTouchWait = true;
  }

  [Serializable]
  public class BattleSkillUseConfirmation
  {
    public bool isSkillUseConfirmation;

    public BattleSkillUseConfirmation() => this.setDefault();

    private void setDefault() => this.isSkillUseConfirmation = true;
  }

  [Serializable]
  public class SeaHomeUnitDate
  {
    public DateTime saveTime;
    public List<int> DisplaySameUnitIDs;
    public List<int> TrustMaxSameUnitIDs;
    public int messageID;
    public int messageUnitID;
    public long timeStamp;

    public SeaHomeUnitDate() => this.setDefault();

    private void setDefault()
    {
      this.saveTime = DateTime.Now;
      this.DisplaySameUnitIDs = new List<int>();
      this.TrustMaxSameUnitIDs = new List<int>();
      this.messageID = 0;
      this.messageUnitID = 0;
      this.timeStamp = 0L;
    }
  }

  [Serializable]
  public class SeaTutorialData
  {
    public bool seaHomeTutorial;
    public bool seaTalkTutorial;

    public SeaTutorialData() => this.setDefault();

    private void setDefault()
    {
      this.seaHomeTutorial = false;
      this.seaTalkTutorial = false;
    }
  }

  [Serializable]
  public class StoryModePopupInfo
  {
    public bool alreadyShow;

    public StoryModePopupInfo() => this.setDefault();

    private void setDefault() => this.alreadyShow = false;

    public void reset() => this.setDefault();
  }

  [Serializable]
  public class TitlePermissionAsk
  {
    public bool alreadyShow;

    public TitlePermissionAsk() => this.setDefault();

    private void setDefault() => this.alreadyShow = false;

    public void reset() => this.setDefault();
  }

  [Serializable]
  public class AppReview
  {
    public bool isShow;
  }

  [Serializable]
  public class NormalDLC
  {
    public bool IsSoundSetup;
    public bool IsSound;
  }

  [Serializable]
  public class SpeedPriority
  {
    public bool IsSpeedPrioritySetup;
    private bool isSpeedPriority;

    public bool IsSpeedPriority
    {
      get => this.IsSpeedPrioritySetup && this.isSpeedPriority;
      set => this.isSpeedPriority = value;
    }
  }

  [Serializable]
  public class MypageUnitId
  {
    public int _unit_id;

    public MypageUnitId() => this.setDefault();

    private void setDefault() => this._unit_id = 0;
  }

  [Serializable]
  public class RaidStoryAlreadyRead
  {
    private Dictionary<int, HashSet<int>> readStoryIds;

    public void addReadStoryId(int stage_id, int story_id)
    {
      if (this.readStoryIds == null)
        this.reset();
      if (!this.readStoryIds.ContainsKey(stage_id))
        this.readStoryIds[stage_id] = new HashSet<int>();
      this.readStoryIds[stage_id].Add(story_id);
    }

    public bool isAlreadyRead(int stage_id, int story_id)
    {
      if (this.readStoryIds == null)
        this.reset();
      return this.readStoryIds.ContainsKey(stage_id) && this.readStoryIds[stage_id].Contains(story_id);
    }

    private void setDefault() => this.readStoryIds = new Dictionary<int, HashSet<int>>();

    public void reset() => this.setDefault();
  }

  [Serializable]
  public class StoryOptions
  {
    public bool autoPlayEnable;

    public StoryOptions() => this.setDefault();

    private void setDefault() => this.autoPlayEnable = false;
  }

  [Serializable]
  public class JobXInfo
  {
    public bool tutorialShow;

    public JobXInfo() => this.tutorialShow = false;
  }

  [Serializable]
  public class FileMoved
  {
    public bool isAskMoved;
    private bool _isAllMoved;
    public bool isIncomplete;

    public bool isAllMoved
    {
      get => this._isAllMoved;
      set
      {
        if (value && Directory.Exists(DLC.ResourceTempDirectory))
          Directory.Delete(DLC.ResourceTempDirectory, true);
        this._isAllMoved = value;
      }
    }
  }

  [Serializable]
  public class JukeBox
  {
    public int LatestSelectTabId;
    public float LatestScrollVarValue;
    public int LatestMusicId;
    public HashSet<int> PlayedMusicId = new HashSet<int>();
  }
}
