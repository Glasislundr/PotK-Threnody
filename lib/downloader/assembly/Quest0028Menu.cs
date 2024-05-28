// Decompiled with JetBrains decompiler
// Type: Quest0028Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using CustomDeck;
using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Quest0028Menu : BackButtonMenuBase
{
  [SerializeField]
  private GameObject topDeckModeButtons;
  [SerializeField]
  [Tooltip("0:通常/1:カスタム でボタン配置")]
  private GameObject[] objDeckModeButtons;
  [SerializeField]
  private GameObject objEditDeckName;
  private bool battleSettingInitialized;
  private int currCost;
  public NGHorizontalScrollParts indicator;
  private Quest0028Indicator selectedDeck;
  private GameObject detailPopup;
  private List<ItemIcon> itemIconList = new List<ItemIcon>();
  [SerializeField]
  private List<SupplyItem> SupplyItems = new List<SupplyItem>();
  [SerializeField]
  private List<SupplyItem> SaveDeck = new List<SupplyItem>();
  [SerializeField]
  private GameObject _mainPanel;
  [SerializeField]
  [Tooltip("0:出撃/1:武器破損/2:編成 でボタンを配置")]
  private GameObject[] Btns;
  public static bool isGoingToBattle;
  [SerializeField]
  private ToggleTweenPositionControl toggleAuto_;
  [SerializeField]
  private ToggleTweenPositionControl toggleItemAutoLap_;
  [SerializeField]
  protected GameObject slcAutoBattleToggle;
  [SerializeField]
  protected GameObject slcAutoLapToggle;
  [SerializeField]
  private UIButton btnSelectFriend;
  [SerializeField]
  protected GameObject linkFriendCharacter;
  [SerializeField]
  protected GameObject[] linkFriendGear;
  [SerializeField]
  protected GameObject slcFriendTextGuest;
  private UnitIcon friendUnitIcon;
  private ItemIcon[] friendItemIcon;
  [SerializeField]
  private UIButton btnBuguChange;
  private PlayerUnit friendUnit;
  private const float gear_scale = 1f;
  private const float gear_scale_sea = 0.65f;
  private const float supply_scale = 0.7f;
  private const float supply_scale_sea = 1f;
  private BattleStagePlayer[] unitPositions;
  private BattleStageGuest[] guest;
  private bool battleSettingBack_;
  private bool isLimitation;
  private bool isUserDeckStage;
  private bool friendPositionEnable;
  private DeckInfo[] regulationDeck;
  private Func<DeckInfo[]> funcGetNormalDecks;
  private DeckInfo[] playerDecks;
  private Dictionary<int, Quest0028Indicator> indicators = new Dictionary<int, Quest0028Indicator>();
  private Dictionary<int, int> deckIndexs = new Dictionary<int, int>();
  private Dictionary<int, int> deckIndexsBack = new Dictionary<int, int>();
  private bool isBreakGearUnits;
  private Dictionary<Quest0028Menu.DeckMode, int> dicStackSelected = new Dictionary<Quest0028Menu.DeckMode, int>();
  private QuestScoreBonusTimetable[] bonusTimeTables;
  private UnitBonus[] unitBonus;
  private PlayerHelper friend;
  private PlayerStoryQuestS story_quest;
  private PlayerExtraQuestS extra_quest;
  private PlayerCharacterQuestS char_quest;
  private PlayerQuestSConverter convert_quest;
  private PlayerSeaQuestS sea_quest;
  private GameObject apPopup;
  private QuestDetailManager detailManager_;
  private int selectDeck;
  private int[] DeckNums;
  private const int SUPPLY_DECK_MAX = 5;
  private bool isPlayingScript;
  private bool isPlayingMovie;
  private bool friendWpChange;
  private Quest0028Menu.LimitedQuestData regulation = new Quest0028Menu.LimitedQuestData();
  private string limitationLabel;
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  protected UILabel TxtRecommendCombat;
  [SerializeField]
  protected UILabel TxtTeamName;
  [SerializeField]
  protected UILabel TxtCombatValue;
  [SerializeField]
  private UILabel TxtCost;
  [SerializeField]
  protected GameObject[] dir_Items;
  private int recommenCombat;
  protected const float scale = 1f;
  private bool isSea_;
  private Dictionary<Quest0028Menu.DeckMode, GameObject> dicPrefabIndicator = new Dictionary<Quest0028Menu.DeckMode, GameObject>(Enum.GetValues(typeof (Quest0028Menu.DeckMode)).Length);
  private GameObject prefabIndicator;
  private GameObject prefabUnitIcon;
  private GameObject prefabItemIcon;
  private BattleskillSkill friendLeaderSkill;
  private Action<PlayerHelper> onSetHelper_;
  private Dictionary<string, Dictionary<int, Quest0028Menu.FriendInfo>> dicFriendInfo_ = new Dictionary<string, Dictionary<int, Quest0028Menu.FriendInfo>>();
  private Dictionary<string, Dictionary<int, Quest0028Menu.FriendInfo>> dicSeaFriendInfo_ = new Dictionary<string, Dictionary<int, Quest0028Menu.FriendInfo>>();
  [SerializeField]
  private QuestMoviePlayer movieObj;
  public BattleInfo CharacterQuestAfterBattleInfo;
  public int CharacterQuestAfterBattleScriptId;
  private GameObject prefabEditDeckName_;

  public Quest0028Menu.DeckMode deckMode { get; private set; }

  public GameObject mainPanel => this._mainPanel;

  protected QuestDetailManager detailManager
  {
    get
    {
      if (this.detailManager_ == null)
        this.detailManager_ = new QuestDetailManager();
      return this.detailManager_;
    }
  }

  public bool story_only { get; private set; }

  public BattleInfo battleInfo { get; private set; }

  public bool IsPlayingStory
  {
    set => this.isPlayingScript = value;
    get => this.isPlayingScript;
  }

  private string quest_name
  {
    get
    {
      if (this.story_quest != null)
        return this.story_quest.quest_story_s.name;
      if (this.extra_quest != null)
        return this.extra_quest.quest_extra_s.name;
      if (this.char_quest != null)
        return this.char_quest.quest_character_s.name;
      if (this.convert_quest != null)
        return this.convert_quest.questS.name;
      return this.sea_quest != null ? this.sea_quest.quest_sea_s.name : "";
    }
  }

  private int questType
  {
    get
    {
      if (this.story_quest != null)
        return 1;
      if (this.extra_quest != null)
        return 3;
      if (this.char_quest != null)
        return 2;
      return this.convert_quest != null ? (this.convert_quest.questS.data_type == QuestSConverter.DataType.Character ? 2 : 4) : (this.sea_quest != null ? 9 : 0);
    }
  }

  private int questID
  {
    get
    {
      if (this.story_quest != null)
        return this.story_quest._quest_story_s;
      if (this.extra_quest != null)
        return this.extra_quest._quest_extra_s;
      if (this.char_quest != null)
        return this.char_quest._quest_character_s;
      if (this.convert_quest != null)
        return this.convert_quest._quest_s_id;
      return this.sea_quest != null ? this.sea_quest._quest_sea_s : 0;
    }
  }

  private int questRecommendCombat
  {
    get
    {
      if (this.story_quest != null)
        return this.story_quest.quest_story_s.stage.recommend_strength;
      if (this.extra_quest != null)
        return this.extra_quest.quest_extra_s.stage.recommend_strength;
      if (this.char_quest != null)
        return this.char_quest.quest_character_s.stage.recommend_strength;
      if (this.convert_quest != null)
        return this.convert_quest.questS.stage.recommend_strength;
      return this.sea_quest != null ? this.sea_quest.quest_sea_s.stage.recommend_strength : 0;
    }
  }

  private int lost_ap
  {
    get
    {
      if (this.story_quest != null)
        return this.story_quest.consumed_ap;
      if (this.extra_quest != null)
        return this.extra_quest.consumed_ap;
      if (this.char_quest != null)
        return this.char_quest.consumed_ap;
      if (this.convert_quest != null)
        return this.convert_quest.consumed_ap;
      return this.sea_quest != null ? this.sea_quest.consumed_ap : -1;
    }
  }

  private UnitGender gender_restriction
  {
    get
    {
      if (this.story_quest != null)
        return this.story_quest.quest_story_s.gender_restriction;
      if (this.extra_quest != null)
        return this.extra_quest.quest_extra_s.gender_restriction;
      if (this.char_quest != null)
        return this.char_quest.quest_character_s.gender_restriction;
      if (this.convert_quest != null)
        return this.convert_quest.questS.gender_restriction;
      return this.sea_quest != null ? this.sea_quest.quest_sea_s.gender_restriction : UnitGender.none;
    }
  }

  protected Quest0028Menu.LimitedQuestData Regulation => this.regulation;

  protected IEnumerator CallExtarDeckData(int id)
  {
    Future<WebAPI.Response.QuestLimitationExtra> apiF = WebAPI.QuestLimitationExtra(id);
    IEnumerator e = apiF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    PlayerDeck[] tmp = (PlayerDeck[]) null;
    PlayerCustomDeck[] tmp2 = (PlayerCustomDeck[]) null;
    QuestLimitationBase[] limits = (QuestLimitationBase[]) null;
    if (apiF.Result != null)
    {
      tmp = apiF.Result.limitation_player_decks;
      tmp2 = apiF.Result.limitation_player_custom_decks;
      limits = apiF.Result.limitations;
    }
    this.regulation.SetInfo(tmp, tmp2, limits);
    this.limitationLabel = ((IEnumerable<QuestExtraLimitationLabel>) MasterData.QuestExtraLimitationLabelList).FirstOrDefault<QuestExtraLimitationLabel>((Func<QuestExtraLimitationLabel, bool>) (q => q.quest_s_id_QuestExtraS == id))?.label;
  }

  protected IEnumerator CallStoryDeckData(int id)
  {
    Future<WebAPI.Response.QuestLimitationStory> apiF = WebAPI.QuestLimitationStory(id);
    IEnumerator e = apiF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    PlayerDeck[] tmp = (PlayerDeck[]) null;
    PlayerCustomDeck[] tmp2 = (PlayerCustomDeck[]) null;
    QuestLimitationBase[] limits = (QuestLimitationBase[]) null;
    if (apiF.Result != null)
    {
      tmp = apiF.Result.limitation_player_decks;
      tmp2 = apiF.Result.limitation_player_custom_decks;
      limits = apiF.Result.limitations;
    }
    this.regulation.SetInfo(tmp, tmp2, limits);
    this.limitationLabel = ((IEnumerable<QuestStoryLimitationLabel>) MasterData.QuestStoryLimitationLabelList).FirstOrDefault<QuestStoryLimitationLabel>((Func<QuestStoryLimitationLabel, bool>) (q => q.quest_s_id_QuestStoryS == id))?.label;
  }

  protected IEnumerator CallCharaDeckData(int id)
  {
    Future<WebAPI.Response.QuestLimitationCharacter> apiF = WebAPI.QuestLimitationCharacter(id);
    IEnumerator e = apiF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    PlayerDeck[] tmp = (PlayerDeck[]) null;
    PlayerCustomDeck[] tmp2 = (PlayerCustomDeck[]) null;
    QuestLimitationBase[] limits = (QuestLimitationBase[]) null;
    if (apiF.Result != null)
    {
      tmp = apiF.Result.limitation_player_decks;
      tmp2 = apiF.Result.limitation_player_custom_decks;
      limits = apiF.Result.limitations;
    }
    this.regulation.SetInfo(tmp, tmp2, limits);
    this.limitationLabel = ((IEnumerable<QuestCharacterLimitationLabel>) MasterData.QuestCharacterLimitationLabelList).FirstOrDefault<QuestCharacterLimitationLabel>((Func<QuestCharacterLimitationLabel, bool>) (q => q.quest_s_id_QuestCharacterS == id))?.label;
  }

  protected IEnumerator CallConvertDeckData(PlayerQuestSConverter data)
  {
    PlayerDeck[] d = (PlayerDeck[]) null;
    PlayerCustomDeck[] d2 = (PlayerCustomDeck[]) null;
    QuestLimitationBase[] lb = (QuestLimitationBase[]) null;
    IEnumerator e;
    if (data.questS.data_type == QuestSConverter.DataType.Character)
    {
      Future<WebAPI.Response.QuestLimitationCharacter> apiF = WebAPI.QuestLimitationCharacter(data.questS.ID);
      e = apiF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (apiF.Result != null)
      {
        d = apiF.Result.limitation_player_decks;
        d2 = apiF.Result.limitation_player_custom_decks;
        lb = apiF.Result.limitations;
      }
      this.limitationLabel = ((IEnumerable<QuestCharacterLimitationLabel>) MasterData.QuestCharacterLimitationLabelList).FirstOrDefault<QuestCharacterLimitationLabel>((Func<QuestCharacterLimitationLabel, bool>) (q => q.quest_s_id_QuestCharacterS == data._quest_s_id))?.label;
      apiF = (Future<WebAPI.Response.QuestLimitationCharacter>) null;
    }
    else if (data.questS.data_type == QuestSConverter.DataType.Harmony)
    {
      Future<WebAPI.Response.QuestLimitationHarmony> apiF = WebAPI.QuestLimitationHarmony(data.questS.ID);
      e = apiF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (apiF.Result != null)
      {
        d = apiF.Result.limitation_player_decks;
        d2 = apiF.Result.limitation_player_custom_decks;
        lb = apiF.Result.limitations;
      }
      this.limitationLabel = ((IEnumerable<QuestHarmonyLimitationLabel>) MasterData.QuestHarmonyLimitationLabelList).FirstOrDefault<QuestHarmonyLimitationLabel>((Func<QuestHarmonyLimitationLabel, bool>) (q => q.quest_s_id_QuestHarmonyS == data._quest_s_id))?.label;
      apiF = (Future<WebAPI.Response.QuestLimitationHarmony>) null;
    }
    this.regulation.SetInfo(d, d2, lb);
  }

  public void setEventSetHelper(Action<PlayerHelper> eventSetHelper)
  {
    this.onSetHelper_ = eventSetHelper;
  }

  private DeckInfo[] getDecks()
  {
    return this.deckMode == Quest0028Menu.DeckMode.Custom ? ((IEnumerable<PlayerCustomDeck>) SMManager.Get<PlayerCustomDeck[]>()).Select<PlayerCustomDeck, DeckInfo>((Func<PlayerCustomDeck, DeckInfo>) (x => PlayerCustomDeck.createDeckInfo(x))).ToArray<DeckInfo>() : this.funcGetNormalDecks();
  }

  public IEnumerator InitPlayerDecks(
    Func<DeckInfo[]> getNormalDecks,
    List<PlayerItem> SupplyList,
    PlayerHelper friend,
    PlayerStoryQuestS story_quest,
    PlayerExtraQuestS extra_quest,
    PlayerCharacterQuestS char_quest,
    PlayerQuestSConverter convert_quest,
    PlayerSeaQuestS sea_quest,
    bool story_only)
  {
    this.indicator.SeEnable = false;
    this.funcGetNormalDecks = getNormalDecks;
    this.deckMode = Quest0028Menu.DeckMode.Normal;
    this.battleSettingInitialized = false;
    bool flag1 = extra_quest != null && Quest0028Menu.IsExtraLimitation(extra_quest);
    bool flag2 = story_quest != null && Quest0028Menu.IsStoryLimitation(story_quest);
    bool flag3 = char_quest != null && Quest0028Menu.IsCharaLimitation(char_quest);
    bool flag4 = convert_quest != null && Quest0028Menu.IsConvertLimitation(convert_quest);
    bool flag5 = sea_quest != null && Quest0028Menu.IsSeaLimitation(sea_quest);
    this.isLimitation = flag1 | flag2 | flag3 | flag4 | flag5;
    this.isUserDeckStage = extra_quest != null && extra_quest.extra_quest_area == 3;
    this.friend = friend;
    this.story_quest = story_quest;
    this.extra_quest = extra_quest;
    this.char_quest = char_quest;
    this.convert_quest = convert_quest;
    this.sea_quest = sea_quest;
    this.story_only = story_only;
    this.isSea_ = Singleton<NGGameDataManager>.GetInstance().IsSea && sea_quest != null;
    if (this.story_only)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      this.playerDecks = this.getDecks();
      if (this.playerDecks != null)
      {
        this.selectDeck = this.isSea_ ? Persist.seaDeckOrganized.Data.number : Persist.deckOrganized.Data.number;
        this.QuestStart();
        yield break;
      }
    }
    bool flag6 = Util.checkUnlockedPlayerLevel(Player.Current.level);
    Persist<Persist.DeckOrganized> deckOrganized = Persist.deckOrganized;
    if (deckOrganized.Data.isCustom && !flag6)
    {
      deckOrganized.Data.reset();
      deckOrganized.Flush();
    }
    if (!this.isSea_ && deckOrganized.Data.isCustom)
      this.deckMode = Quest0028Menu.DeckMode.Custom;
    bool flag7 = !flag6 || !((IEnumerable<PlayerCustomDeck>) SMManager.Get<PlayerCustomDeck[]>()).Any<PlayerCustomDeck>((Func<PlayerCustomDeck, bool>) (x => ((IEnumerable<int>) x.player_unit_ids).FirstOrDefault<int>() != 0));
    if (flag7)
      this.deckMode = Quest0028Menu.DeckMode.Normal;
    this.setDeckModeButton(this.deckMode, !this.isSea_ && !flag7);
    IEnumerator e;
    if (flag1)
    {
      e = this.CallExtarDeckData(extra_quest.quest_extra_s.ID);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.regulationDeck = this.deckMode != Quest0028Menu.DeckMode.Custom ? this.Regulation.Deck : this.Regulation.CustomDeck;
    }
    else if (flag2)
    {
      e = this.CallStoryDeckData(story_quest.quest_story_s.ID);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.regulationDeck = this.deckMode != Quest0028Menu.DeckMode.Custom ? this.Regulation.Deck : this.Regulation.CustomDeck;
    }
    else if (flag3)
    {
      e = this.CallCharaDeckData(story_quest.quest_story_s.ID);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.regulationDeck = this.deckMode != Quest0028Menu.DeckMode.Custom ? this.Regulation.Deck : this.Regulation.CustomDeck;
    }
    else if (flag4)
    {
      e = this.CallConvertDeckData(convert_quest);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.regulationDeck = this.deckMode != Quest0028Menu.DeckMode.Custom ? this.Regulation.Deck : this.Regulation.CustomDeck;
    }
    else if (flag5)
    {
      e = this.CallStoryDeckData(sea_quest.quest_sea_s.ID);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.regulationDeck = this.deckMode != Quest0028Menu.DeckMode.Custom ? this.Regulation.Deck : this.Regulation.CustomDeck;
    }
    this.TxtTitle.SetText(this.quest_name);
    if (Object.op_Inequality((Object) this.toggleAuto_, (Object) null))
      this.toggleAuto_.resetSwitch(Persist.autoBattleSetting.Data.isAutoBattle);
    else if (Object.op_Inequality((Object) this.slcAutoBattleToggle, (Object) null))
      this.slcAutoBattleToggle.SetActive(Persist.autoBattleSetting.Data.isAutoBattle);
    if (Object.op_Inequality((Object) this.toggleItemAutoLap_, (Object) null))
      this.toggleItemAutoLap_.resetSwitch(Singleton<NGGameDataManager>.GetInstance().questAutoLap);
    else if (Object.op_Inequality((Object) this.slcAutoLapToggle, (Object) null))
      this.slcAutoLapToggle.SetActive(Singleton<NGGameDataManager>.GetInstance().questAutoLap);
    Future<GameObject> prefabF;
    if (Object.op_Equality((Object) this.prefabUnitIcon, (Object) null))
    {
      prefabF = this.isSea_ ? new ResourceObject("Prefabs/Sea/UnitIcon/normal_sea").Load<GameObject>() : Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.prefabUnitIcon = prefabF.Result;
    }
    if (Object.op_Equality((Object) this.prefabItemIcon, (Object) null))
    {
      prefabF = this.isSea_ ? new ResourceObject("Prefabs/Sea/ItemIcon/prefab_sea").Load<GameObject>() : Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.prefabItemIcon = prefabF.Result;
    }
    this.recommenCombat = this.questRecommendCombat;
    this.TxtRecommendCombat.SetTextRecommendCombat(this.recommenCombat);
    if (SupplyList.Count <= 5)
    {
      e = this.SetSupplyIcons(SupplyList);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
      Debug.LogWarning((object) "!!! BUG !!! SUPPLYDECK OUT OF RANGE");
    e = this.doInitDeckPanels();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator doInitDeckPanels()
  {
    Quest0028Menu quest0028Menu = this;
    quest0028Menu.battleSettingInitialized = false;
    quest0028Menu.indicator.destroyParts(false);
    quest0028Menu.playerDecks = quest0028Menu.getDecks();
    quest0028Menu.DeckNums = new int[quest0028Menu.playerDecks.Length];
    int cnt = 0;
    quest0028Menu.indicators.Clear();
    quest0028Menu.deckIndexs.Clear();
    quest0028Menu.deckIndexsBack.Clear();
    QuestScoreBonusTimetable[] source = SMManager.Get<QuestScoreBonusTimetable[]>();
    quest0028Menu.bonusTimeTables = ((IEnumerable<QuestScoreBonusTimetable>) source).Where<QuestScoreBonusTimetable>((Func<QuestScoreBonusTimetable, bool>) (x => x.start_at < ServerTime.NowAppTime() && x.end_at > ServerTime.NowAppTime() && x.quest_s_id == this.questID)).ToArray<QuestScoreBonusTimetable>();
    quest0028Menu.unitBonus = UnitBonus.getActiveUnitBonus(ServerTime.NowAppTime(), new int?(quest0028Menu.questType), new int?(quest0028Menu.questID));
    int battleStageID = 0;
    if (quest0028Menu.story_quest != null)
      battleStageID = quest0028Menu.story_quest.quest_story_s.stage_BattleStage;
    else if (quest0028Menu.extra_quest != null)
      battleStageID = quest0028Menu.extra_quest.quest_extra_s.wave == null ? quest0028Menu.extra_quest.quest_extra_s.stage_BattleStage : quest0028Menu.extra_quest.quest_extra_s.wave.first_quest_s_id;
    else if (quest0028Menu.char_quest != null)
      battleStageID = quest0028Menu.char_quest.quest_character_s.stage_BattleStage;
    else if (quest0028Menu.convert_quest != null)
      battleStageID = quest0028Menu.convert_quest.questS.stage_BattleStage;
    else if (quest0028Menu.sea_quest != null)
      battleStageID = quest0028Menu.sea_quest.quest_sea_s.stage_BattleStage;
    IEnumerator e = quest0028Menu.doLoadPrefabIndicator();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    quest0028Menu.unitPositions = ((IEnumerable<BattleStagePlayer>) MasterData.BattleStagePlayerList).Where<BattleStagePlayer>((Func<BattleStagePlayer, bool>) (x => x.stage_BattleStage == battleStageID)).ToArray<BattleStagePlayer>();
    quest0028Menu.guest = ((IEnumerable<BattleStageGuest>) MasterData.BattleStageGuestList).Where<BattleStageGuest>((Func<BattleStageGuest, bool>) (x => x.stage_BattleStage == battleStageID)).ToArray<BattleStageGuest>();
    for (int i = 0; i < quest0028Menu.playerDecks.Length; ++i)
    {
      quest0028Menu.deckIndexsBack.Add(i, 0);
      DeckInfo playerDeck = quest0028Menu.playerDecks[i];
      if (((IEnumerable<PlayerUnit>) playerDeck.player_units).FirstOrDefault<PlayerUnit>() != (PlayerUnit) null)
      {
        quest0028Menu.deckIndexs.Add(cnt, i);
        quest0028Menu.deckIndexsBack[i] = cnt;
        quest0028Menu.DeckNums[cnt] = playerDeck.deck_number;
        GameObject prefab = quest0028Menu.indicator.instantiateParts(quest0028Menu.prefabIndicator);
        e = quest0028Menu.AddDeck(playerDeck, prefab, quest0028Menu.bonusTimeTables, quest0028Menu.unitBonus, quest0028Menu.guest, battleStageID);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        ++cnt;
      }
    }
    ((Behaviour) quest0028Menu.btnSelectFriend).enabled = false;
    quest0028Menu.friendUnit = (PlayerUnit) null;
    quest0028Menu.friendLeaderSkill = (BattleskillSkill) null;
    if (quest0028Menu.isUserDeckStage || quest0028Menu.isLimitation || !Quest0028Menu.GetFriendPositionEnable(battleStageID))
    {
      bool isUsedGuestFriend = false;
      for (int index = 0; index < quest0028Menu.guest.Length; ++index)
      {
        if (quest0028Menu.guest[index].deck_position == Consts.GetInstance().DECK_POSITION_FRIEND)
        {
          isUsedGuestFriend = true;
          quest0028Menu.slcFriendTextGuest.SetActive(true);
          PlayerUnit playerUnit = PlayerUnit.FromGuest(quest0028Menu.guest[index]);
          yield return (object) quest0028Menu.SetFriendUnitIcon(playerUnit);
          if (playerUnit.leader_skill != null)
          {
            quest0028Menu.friendLeaderSkill = playerUnit.leader_skill.skill;
            break;
          }
          break;
        }
      }
      if (!isUsedGuestFriend)
      {
        quest0028Menu.slcFriendTextGuest.SetActive(false);
        yield return (object) quest0028Menu.SetFriendUnitIcon(isSortie: false);
      }
    }
    else
    {
      if (quest0028Menu.friend != null)
        quest0028Menu.friendLeaderSkill = quest0028Menu.friend.leader_skill_from_cache;
      yield return (object) quest0028Menu.setHelperUI();
    }
    quest0028Menu.selectDeck = -1;
    int firstDeckNumber = quest0028Menu.firstDeckNumber;
    quest0028Menu.indicator.resetScrollView();
    quest0028Menu.indicator.setItemPositionQuick(quest0028Menu.deckIndexsBack[firstDeckNumber]);
    quest0028Menu.updateDeck();
    quest0028Menu.battleSettingInitialized = true;
    quest0028Menu.StartCoroutine(quest0028Menu.WaitScrollSe());
  }

  private int firstDeckNumber
  {
    get
    {
      int firstDeckNumber;
      if (!this.dicStackSelected.TryGetValue(this.deckMode, out firstDeckNumber))
      {
        firstDeckNumber = this.deckMode != Quest0028Menu.DeckMode.Custom ? (this.isSea_ ? Persist.seaDeckOrganized.Data.number : Persist.deckOrganized.Data.number) : Persist.deckOrganized.Data.customNumber;
        this.dicStackSelected[this.deckMode] = firstDeckNumber;
      }
      return firstDeckNumber;
    }
  }

  private IEnumerator doLoadPrefabIndicator()
  {
    if (!this.dicPrefabIndicator.TryGetValue(this.deckMode, out this.prefabIndicator))
    {
      Future<GameObject> prefabF = new ResourceObject("Prefabs/quest002_8/" + (this.deckMode == Quest0028Menu.DeckMode.Custom ? "dir_party_2" : (this.isSea_ ? "dir_party_sea" : "dir_party"))).Load<GameObject>();
      IEnumerator e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.prefabIndicator = prefabF.Result;
      this.dicPrefabIndicator[this.deckMode] = this.prefabIndicator;
    }
  }

  protected override void Update()
  {
    if (!this.battleSettingInitialized || this.story_only)
      return;
    base.Update();
    if (this.battleSettingBack_)
    {
      if (Object.op_Inequality((Object) this.toggleAuto_, (Object) null))
        this.toggleAuto_.resetSwitch(Persist.autoBattleSetting.Data.isAutoBattle);
      else if (Object.op_Inequality((Object) this.slcAutoBattleToggle, (Object) null))
        this.slcAutoBattleToggle.SetActive(Persist.autoBattleSetting.Data.isAutoBattle);
      this.battleSettingBack_ = false;
    }
    if (Object.op_Inequality((Object) this.toggleItemAutoLap_, (Object) null))
      Singleton<NGGameDataManager>.GetInstance().questAutoLap = this.toggleItemAutoLap_.isSwitch;
    else if (Object.op_Inequality((Object) this.slcAutoLapToggle, (Object) null))
      Singleton<NGGameDataManager>.GetInstance().questAutoLap = this.slcAutoLapToggle.activeSelf;
    this.updateDeck();
  }

  private void updateDeck()
  {
    if (this.playerDecks == null || this.selectDeck == this.DeckNums[this.indicator.selected])
      return;
    this.selectDeck = this.DeckNums[this.indicator.selected];
    this.dicStackSelected[this.deckMode] = this.lastSelectedNumber;
    this.selectedDeck = this.indicators[this.selectDeck];
    PlayerUnit[] deckUnitData = this.selectedDeck.deckUnitData;
    this.setTeamInfo();
    this.isBreakGearUnits = false;
    for (int index = 0; index < this.selectedDeck.maxPlayer; ++index)
    {
      if (!(deckUnitData[index] == (PlayerUnit) null) && !deckUnitData[index].is_guest && (!(deckUnitData[index].equippedGear == (PlayerItem) null) || !(deckUnitData[index].equippedGear2 == (PlayerItem) null) || !(deckUnitData[index].equippedGear3 == (PlayerItem) null)) && (deckUnitData[index].equippedGear != (PlayerItem) null && deckUnitData[index].equippedGear.broken || deckUnitData[index].equippedGear2 != (PlayerItem) null && deckUnitData[index].equippedGear2.broken || deckUnitData[index].equippedGear3 != (PlayerItem) null && deckUnitData[index].equippedGear3.broken))
      {
        this.isBreakGearUnits = true;
        break;
      }
    }
    ((UIButtonColor) this.btnBuguChange).isEnabled = true;
    if (!this.indicators[this.selectDeck].ChangeWpSlotCheck() && !this.friendWpChange)
      ((UIButtonColor) this.btnBuguChange).isEnabled = false;
    if (this.isBreakGearUnits)
    {
      ((IEnumerable<GameObject>) this.Btns).ToggleOnceEx(1);
      Singleton<NGGameDataManager>.GetInstance().questAutoLap = false;
      if (Object.op_Inequality((Object) this.toggleItemAutoLap_, (Object) null))
      {
        this.toggleItemAutoLap_.setSwitch(false);
      }
      else
      {
        if (!Object.op_Inequality((Object) this.slcAutoLapToggle, (Object) null))
          return;
        this.slcAutoLapToggle.SetActive(false);
      }
    }
    else
    {
      ((IEnumerable<GameObject>) this.Btns).ToggleOnceEx(0);
      if (Object.op_Inequality((Object) this.selectedDeck, (Object) null) && !this.selectedDeck.isCompletedOverkillersDeck)
        this.setMainButtonEdit();
      else if (this.isLimitation && this.regulationDeck != null)
      {
        PlayerUnit[] source = new PlayerUnit[5];
        if (Object.op_Inequality((Object) this.selectedDeck, (Object) null))
        {
          for (int index = 0; index < this.selectedDeck.maxPlayer; ++index)
            source[index] = this.selectedDeck.deckUnitData[index];
        }
        int[] array = ((IEnumerable<PlayerUnit>) source).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null && !x.is_guest)).Select<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.id)).ToArray<int>();
        HashSet<int> checkedDeckIds = new HashSet<int>(((IEnumerable<int>) this.regulationDeck[this.selectDeck].player_unit_ids).Where<int>((Func<int, bool>) (n => n != 0)));
        Func<int, bool> predicate = (Func<int, bool>) (id => checkedDeckIds.Contains(id));
        if (((IEnumerable<int>) array).All<int>(predicate))
          return;
        this.setMainButtonEdit();
      }
      else if (this.isLimitation && this.regulationDeck == null)
      {
        this.setMainButtonEdit();
      }
      else
      {
        if (this.gender_restriction == UnitGender.none || ((IEnumerable<PlayerUnit>) this.playerDecks[this.selectDeck].player_units).All<PlayerUnit>((Func<PlayerUnit, bool>) (x => x == (PlayerUnit) null || x.unit.character.gender == this.gender_restriction)))
          return;
        this.setMainButtonEdit();
      }
    }
  }

  private void setMainButtonEdit() => ((IEnumerable<GameObject>) this.Btns).ToggleOnceEx(2);

  private IEnumerator AddDeck(
    DeckInfo playerDeck,
    GameObject prefab,
    QuestScoreBonusTimetable[] tables,
    UnitBonus[] unitBonus,
    BattleStageGuest[] guests,
    int battleStageID)
  {
    Quest0028Indicator component = prefab.GetComponent<Quest0028Indicator>();
    this.preInitializeIndicator(component);
    this.indicators.Add(playerDeck.deck_number, component);
    IEnumerator e = component.InitPlayerDeck(playerDeck, this.extra_quest, this.story_quest, this.char_quest, this.convert_quest, this.sea_quest, this.regulationDeck, tables, unitBonus, guests, battleStageID, this.prefabUnitIcon, this.prefabItemIcon);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private Quest0028Menu.FriendInfo getFriendInfoCache(string player_id, int player_unit_id)
  {
    Dictionary<int, Quest0028Menu.FriendInfo> dictionary;
    Quest0028Menu.FriendInfo friendInfo;
    return this.dicFriendInfo_.TryGetValue(player_id, out dictionary) && dictionary.TryGetValue(player_unit_id, out friendInfo) ? friendInfo : (Quest0028Menu.FriendInfo) null;
  }

  private void setFriendInfoCache(
    string player_id,
    int player_unit_id,
    Quest0028Menu.FriendInfo info)
  {
    Dictionary<int, Quest0028Menu.FriendInfo> dictionary1;
    if (!this.dicFriendInfo_.TryGetValue(player_id, out dictionary1))
    {
      Dictionary<int, Quest0028Menu.FriendInfo> dictionary2 = new Dictionary<int, Quest0028Menu.FriendInfo>()
      {
        {
          player_unit_id,
          info
        }
      };
      this.dicFriendInfo_.Add(player_id, dictionary2);
    }
    else
    {
      if (dictionary1.ContainsKey(player_unit_id))
        return;
      dictionary1.Add(player_unit_id, info);
    }
  }

  private Quest0028Menu.FriendInfo getSeaFriendInfoCache(string player_id, int player_unit_id)
  {
    Dictionary<int, Quest0028Menu.FriendInfo> dictionary;
    Quest0028Menu.FriendInfo friendInfo;
    return this.dicSeaFriendInfo_.TryGetValue(player_id, out dictionary) && dictionary.TryGetValue(player_unit_id, out friendInfo) ? friendInfo : (Quest0028Menu.FriendInfo) null;
  }

  private void setSeaFriendInfoCache(
    string player_id,
    int player_unit_id,
    Quest0028Menu.FriendInfo info)
  {
    Dictionary<int, Quest0028Menu.FriendInfo> dictionary1;
    if (!this.dicSeaFriendInfo_.TryGetValue(player_id, out dictionary1))
    {
      Dictionary<int, Quest0028Menu.FriendInfo> dictionary2 = new Dictionary<int, Quest0028Menu.FriendInfo>()
      {
        {
          player_unit_id,
          info
        }
      };
      this.dicSeaFriendInfo_.Add(player_id, dictionary2);
    }
    else
    {
      if (dictionary1.ContainsKey(player_unit_id))
        return;
      dictionary1.Add(player_unit_id, info);
    }
  }

  protected IEnumerator setHelperUI()
  {
    Quest0028Menu quest0028Menu = this;
    ((Behaviour) quest0028Menu.btnSelectFriend).enabled = true;
    quest0028Menu.slcFriendTextGuest.SetActive(false);
    PlayerUnit playerUnit = (PlayerUnit) null;
    if (quest0028Menu.friend != null)
    {
      IEnumerator e;
      Quest0028Menu.FriendInfo info;
      if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      {
        info = quest0028Menu.getSeaFriendInfoCache(quest0028Menu.friend.target_player_id, quest0028Menu.friend.leader_player_unit_id);
        if (info == null)
        {
          // ISSUE: reference to a compiler-generated method
          Future<WebAPI.Response.SeaFriendStatus> futureF = WebAPI.SeaFriendStatus(quest0028Menu.friend.target_player_id, quest0028Menu.friend.leader_player_unit_id, new Action<WebAPI.Response.UserError>(quest0028Menu.\u003CsetHelperUI\u003Eb__135_0));
          e = futureF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          WebAPI.Response.SeaFriendStatus result = futureF.Result;
          if (result == null)
          {
            yield break;
          }
          else
          {
            info = new Quest0028Menu.FriendInfo(result);
            if (info == null)
              Debug.LogError((object) "!!!フレンド情報が取得出来ない : player_id:{0}, unit_id:{1}".F((object) quest0028Menu.friend.target_player_id, (object) quest0028Menu.friend.leader_player_unit_id));
            else
              quest0028Menu.setSeaFriendInfoCache(quest0028Menu.friend.target_player_id, quest0028Menu.friend.leader_player_unit_id, info);
            futureF = (Future<WebAPI.Response.SeaFriendStatus>) null;
          }
        }
      }
      else
      {
        info = quest0028Menu.getFriendInfoCache(quest0028Menu.friend.target_player_id, quest0028Menu.friend.leader_player_unit_id);
        if (info == null)
        {
          // ISSUE: reference to a compiler-generated method
          Future<WebAPI.Response.FriendStatus> futureF = WebAPI.FriendStatus(quest0028Menu.friend.target_player_id, quest0028Menu.friend.leader_player_unit_id, new Action<WebAPI.Response.UserError>(quest0028Menu.\u003CsetHelperUI\u003Eb__135_1));
          e = futureF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          WebAPI.Response.FriendStatus result = futureF.Result;
          if (result == null)
          {
            yield break;
          }
          else
          {
            info = new Quest0028Menu.FriendInfo(result);
            if (info == null)
              Debug.LogError((object) "!!!フレンド情報が取得出来ない : player_id:{0}, unit_id:{1}".F((object) quest0028Menu.friend.target_player_id, (object) quest0028Menu.friend.leader_player_unit_id));
            else
              quest0028Menu.setFriendInfoCache(quest0028Menu.friend.target_player_id, quest0028Menu.friend.leader_player_unit_id, info);
            futureF = (Future<WebAPI.Response.FriendStatus>) null;
          }
        }
      }
      if (info != null)
      {
        playerUnit = info.target_leader_unit;
        playerUnit.primary_equipped_gear = playerUnit.FindEquippedGear(info.target_player_items);
        playerUnit.primary_equipped_gear2 = playerUnit.FindEquippedGear2(info.target_player_items);
        playerUnit.primary_equipped_gear3 = playerUnit.FindEquippedGear3(info.target_player_items);
        playerUnit.primary_equipped_reisou = playerUnit.FindEquippedReisou(info.target_player_items, info.target_player_reisou_items);
        playerUnit.primary_equipped_reisou2 = playerUnit.FindEquippedReisou2(info.target_player_items, info.target_player_reisou_items);
        playerUnit.primary_equipped_reisou3 = playerUnit.FindEquippedReisou3(info.target_player_items, info.target_player_reisou_items);
        playerUnit.resetUsedPrimary();
        if (playerUnit.primary_equipped_gear != (PlayerItem) null)
          playerUnit.primary_equipped_gear.broken = false;
        if (playerUnit.primary_equipped_gear2 != (PlayerItem) null)
          playerUnit.primary_equipped_gear2.broken = false;
        if (playerUnit.primary_equipped_gear3 != (PlayerItem) null)
          playerUnit.primary_equipped_gear3.broken = false;
      }
    }
    yield return (object) quest0028Menu.SetFriendUnitIcon(playerUnit);
  }

  public IEnumerator SetFriendUnitIcon(PlayerUnit unit = null, bool isSortie = true)
  {
    this.friendUnit = unit;
    this.friendWpChange = false;
    if (Object.op_Inequality((Object) this.friendUnitIcon, (Object) null))
    {
      this.friendUnitIcon = (UnitIcon) null;
      UnitIcon componentInChildren = ((Component) this.linkFriendCharacter.transform).GetComponentInChildren<UnitIcon>();
      if (Object.op_Inequality((Object) componentInChildren, (Object) null))
        Object.Destroy((Object) ((Component) componentInChildren).gameObject);
    }
    if (this.friendItemIcon != null)
    {
      for (int index = 0; index < this.friendItemIcon.Length; ++index)
      {
        this.friendItemIcon[index] = (ItemIcon) null;
        ItemIcon componentInChildren = ((Component) this.linkFriendGear[index].transform).GetComponentInChildren<ItemIcon>();
        if (Object.op_Inequality((Object) componentInChildren, (Object) null))
          Object.Destroy((Object) ((Component) componentInChildren).gameObject);
      }
    }
    GameObject gameObject = this.prefabUnitIcon.Clone(this.linkFriendCharacter.transform);
    gameObject.transform.localScale = new Vector3(1f, 1f);
    this.friendUnitIcon = gameObject.GetComponent<UnitIcon>();
    IEnumerator e = this.friendUnitIcon.setSimpleUnit(unit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.friendUnitIcon.setLevelText(unit);
    this.friendUnitIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    if (unit == (PlayerUnit) null)
    {
      this.friendUnitIcon.SetEmpty();
      if (isSortie)
      {
        this.friendUnitIcon.SelectUnit = true;
      }
      else
      {
        this.friendUnitIcon.SetRegulation(UnitIcon.Regulation.None);
        this.friendUnitIcon.BackgroundModeValue = UnitIcon.BackgroundMode.Empty;
        this.friendUnitIcon.SetIconBoxCollider(false);
      }
    }
    if (unit != (PlayerUnit) null & isSortie)
    {
      int slotNum = 1;
      List<PlayerItem> equippedGears = new List<PlayerItem>();
      equippedGears.Add(unit.equippedGear);
      List<PlayerItem> equippedReisous = new List<PlayerItem>();
      equippedReisous.Add(unit.equippedReisou);
      if (unit.equip_gear_ids != null)
      {
        if (unit.unit.awake_unit_flag)
        {
          if (unit.equip_gear_ids.Length >= 3)
          {
            equippedGears.Add(unit.equippedGear2);
            equippedReisous.Add(unit.equippedReisou2);
            equippedGears.Add(unit.equippedGear3);
            equippedReisous.Add(unit.equippedReisou3);
            this.friendWpChange = true;
          }
          else
          {
            equippedGears.Add(unit.equippedGear2);
            equippedReisous.Add(unit.equippedReisou2);
          }
        }
        else if (unit.equip_gear_ids.Length >= 2)
        {
          equippedGears.Add(unit.equippedGear3);
          equippedReisous.Add(unit.equippedReisou3);
        }
        slotNum = unit.equip_gear_ids.Length;
      }
      this.friendItemIcon = new ItemIcon[slotNum];
      for (int i = 0; i < slotNum; ++i)
      {
        ItemIcon component = this.prefabItemIcon.CloneAndGetComponent<ItemIcon>(this.linkFriendGear[i].transform);
        if (Singleton<NGGameDataManager>.GetInstance().IsSea)
          ((Component) component).transform.localScale = new Vector3(0.65f, 0.65f);
        else
          ((Component) component).transform.localScale = new Vector3(1f, 1f);
        this.friendItemIcon[i] = component;
        if (equippedGears[i] != (PlayerItem) null)
          yield return (object) component.InitByPlayerItem(equippedGears[i], equippedReisous[i]);
        else
          yield return (object) component.InitForEquipGear();
      }
      equippedGears = (List<PlayerItem>) null;
      equippedReisous = (List<PlayerItem>) null;
    }
    this.linkFriendGear[0].SetActive(true);
    this.linkFriendGear[1].SetActive(true);
    this.linkFriendGear[2].SetActive(false);
    this.friendUnitIcon.Favorite = false;
    this.friendUnitIcon.Gray = false;
    if (unit != (PlayerUnit) null && unit.is_guest)
    {
      this.friendUnitIcon.onClick = (Action<UnitIconBase>) (_ => Unit0042Scene.changeSceneGuestUnit(true, unit, new PlayerUnit[1]
      {
        unit
      }));
      EventDelegate.Set(this.friendUnitIcon.Button.onLongPress, (EventDelegate.Callback) (() => Unit0042Scene.changeSceneGuestUnit(true, unit, new PlayerUnit[1]
      {
        unit
      })));
    }
    else if (isSortie)
    {
      this.friendUnitIcon.onClick = (Action<UnitIconBase>) (_ => this.IbtnSelectFriend());
      if (this.friend != null)
        EventDelegate.Set(this.friendUnitIcon.Button.onLongPress, (EventDelegate.Callback) (() => Unit0042Scene.changeSceneFriendUnit(true, this.friend.target_player_id, this.friend.leader_player_unit_id)));
      else
        EventDelegate.Set(this.friendUnitIcon.Button.onLongPress, (EventDelegate.Callback) (() => this.IbtnSelectFriend()));
    }
  }

  protected IEnumerator resetHelper(PlayerHelper friend)
  {
    this.friend = friend;
    yield return (object) this.setHelperUI();
  }

  private void setTeamInfo()
  {
    this.TxtTeamName.SetTextLocalize(this.playerDecks[this.selectDeck].name);
    int num = 0;
    this.currCost = 0;
    if (Object.op_Inequality((Object) this.selectedDeck, (Object) null))
    {
      for (int index = 0; index < this.selectedDeck.maxPlayer; ++index)
      {
        if (this.selectedDeck.deckUnitData[index] != (PlayerUnit) null && !this.selectedDeck.deckUnitData[index].is_guest)
        {
          num += this.selectedDeck.deckUnitData[index].combat;
          this.currCost += this.selectedDeck.deckUnitData[index].cost;
        }
      }
    }
    int friendPos = Consts.GetInstance().DECK_POSITION_FRIEND;
    List<int> guestPos = new List<int>(((IEnumerable<BattleStageGuest>) this.guest).Select<BattleStageGuest, int>((Func<BattleStageGuest, int>) (x => x.deck_position)));
    if (((IEnumerable<BattleStagePlayer>) this.unitPositions).Count<BattleStagePlayer>((Func<BattleStagePlayer, bool>) (x => x.deck_position != friendPos && !guestPos.Contains(x.deck_position))) == 0)
    {
      this.TxtCombatValue.SetText("---");
      this.TxtCost.SetText("---");
    }
    else
    {
      Consts instance = Consts.GetInstance();
      if (this.recommenCombat == 0)
        this.TxtCombatValue.SetText(num.ToString());
      else
        this.TxtCombatValue.SetText(Consts.Format(this.recommenCombat <= num ? (this.isSea_ ? instance.QUEST_0028_DECK_ENOUGH_SEA : instance.QUEST_0028_DECK_ENOUGH) : (this.isSea_ ? instance.QUEST_0028_DECK_SHORT_SEA : instance.QUEST_0028_DECK_SHORT), (IDictionary) new Hashtable()
        {
          {
            (object) "combat",
            (object) num
          }
        }));
      int maxCost = Player.Current.max_cost;
      this.TxtCost.SetText(Consts.Format(maxCost >= this.currCost ? (this.isSea_ ? instance.QUEST_0028_COST_SAFE_SEA : instance.QUEST_0028_COST_SAFE) : (this.isSea_ ? instance.QUEST_0028_COST_OVER_SEA : instance.QUEST_0028_COST_OVER), (IDictionary) new Hashtable()
      {
        {
          (object) "total",
          (object) this.currCost
        },
        {
          (object) "max",
          (object) maxCost
        }
      }));
    }
  }

  protected virtual void preInitializeIndicator(Quest0028Indicator indicator)
  {
  }

  public virtual void IbtnItemedit()
  {
    if (this.IsPushAndSet())
      return;
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
    this.SupplyItems = ((IEnumerable<SupplyItem>) SupplyItem.Merge(((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>().AllSupplies()).ToList<PlayerItem>().ToArray(), ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>().AllBattleSupplies()).ToArray<PlayerItem>())).ToList<SupplyItem>();
    this.SaveDeck = this.SupplyItems.Copy();
    Quest00210aScene.ChangeScene(true, new Quest00210Menu.Param()
    {
      SupplyItems = this.SupplyItems,
      SaveDeck = this.SaveDeck,
      removeButton = false,
      limitedOnly = false,
      mode = Quest00210Scene.Mode.Quest
    });
  }

  private IEnumerator changeSceneSupplyEdit()
  {
    Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
    Quest00210Scene.changeScene(true);
    yield break;
  }

  protected void QuestStart()
  {
    if (this.isBreakGearUnits)
      return;
    Singleton<CommonRoot>.GetInstance().loadingMode = 4;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Player player = SMManager.Get<Player>();
    DeckInfo playerDeck = this.playerDecks[this.selectDeck];
    int ap = player.ap;
    int num = Math.Max(player.max_cost, playerDeck.cost_limit);
    if (Singleton<NGGameDataManager>.GetInstance().questAutoLap)
      this.friend = (PlayerHelper) null;
    if (this.battleSettingInitialized)
    {
      if (Object.op_Inequality((Object) this.toggleAuto_, (Object) null))
        Persist.autoBattleSetting.Data.isAutoBattle = this.toggleAuto_.isSwitch;
      else if (Object.op_Inequality((Object) this.slcAutoBattleToggle, (Object) null))
        Persist.autoBattleSetting.Data.isAutoBattle = this.slcAutoBattleToggle.activeSelf;
    }
    if (this.currCost <= num)
    {
      if (ap < this.lost_ap)
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        this.StartCoroutine(PopupUtility.RecoveryAP(true));
      }
      else
      {
        Quest0028Menu.isGoingToBattle = true;
        if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
        {
          Singleton<NGGameDataManager>.GetInstance().IsColosseum = false;
          Singleton<CommonRoot>.GetInstance().SetFooterEnable(true);
        }
        if (this.indicators.ContainsKey(this.selectDeck))
          Singleton<NGSoundManager>.GetInstance().playVoiceByID(this.indicators[this.selectDeck].deckUnitData[0].unit.unitVoicePattern, 70, 0);
        this.indicator.SeEnable = false;
        if (this.story_quest != null)
          this.StartCoroutine(this.StoryMovie(this.story_quest.quest_story_s.ID, this.story_quest.is_new, new Action(this.StoryStartApi)));
        else if (this.extra_quest != null)
          this.StartCoroutine(this.StoryMovie(this.extra_quest.quest_extra_s.ID, this.extra_quest.is_new, new Action(this.ExtraStartApi)));
        else if (this.char_quest != null)
          this.StartCoroutine(this.StoryMovie(this.char_quest.quest_character_s.ID, this.char_quest.is_new, new Action(this.CharacterStartApi)));
        else if (this.convert_quest != null)
        {
          Action act = (Action) null;
          if (this.convert_quest.questS.data_type == QuestSConverter.DataType.Character)
            act = new Action(this.CharacterStartApi2);
          else if (this.convert_quest.questS.data_type == QuestSConverter.DataType.Harmony)
            act = new Action(this.HarmonyStartApi);
          this.StartCoroutine(this.StoryMovie(this.convert_quest.questS.ID, this.convert_quest.is_new, act));
        }
        else if (this.sea_quest != null)
          this.StartCoroutine(this.StoryMovie(this.sea_quest.quest_sea_s.ID, this.sea_quest.is_new, new Action(this.SeaStartApi)));
        else
          Debug.LogError((object) "!!! BUG !!!");
      }
    }
    else
    {
      this.StartCoroutine(PopupCommon.Show(Consts.GetInstance().QUEST_0028_SORTIE_TITLE, Consts.GetInstance().QUEST_0028_COST_OVER_DESCRIPTION));
      Singleton<CommonRoot>.GetInstance().isLoading = false;
    }
  }

  public virtual void IbtnSortie()
  {
    if (this.IsPush)
      return;
    if (Singleton<NGGameDataManager>.GetInstance().questAutoLap && this.friend != null)
      PopupCommonNoYes2.Show("オート周回確認", "[ffff00]オート周回中はフレンドユニットを\n連れて行くことができません\n\n自分のユニットのみで出撃しますか？[-]", new Action(this.AutoLapFriendConfirmation), (Action) (() => { }));
    else
      this.QuestStart();
  }

  private void AutoLapFriendConfirmation() => this.QuestStart();

  private void StartBattle(BattleInfo info)
  {
    if (this.story_only)
    {
      int scriptId = this.getScriptID(info);
      this.battleInfo = info;
      this.IsPlayingStory = true;
      Story0093Scene.changeScene(true, scriptId, new bool?(Singleton<NGGameDataManager>.GetInstance().IsSea && info.seaQuest != null));
    }
    else
    {
      NGBattleManager instance = Singleton<NGBattleManager>.GetInstance();
      instance.deleteSavedEnvironment();
      instance.startBattle(info);
    }
  }

  private IEnumerator StoryMovie(int id, bool is_new, Action act)
  {
    if (!is_new || Object.op_Equality((Object) this.movieObj, (Object) null))
    {
      act();
    }
    else
    {
      yield return (object) null;
      if (this.movieObj.isPlayMovie(id))
      {
        while (Singleton<NGSoundManager>.GetInstance().IsVoicePlaying(0))
          yield return (object) null;
        this.isPlayingMovie = true;
        this.movieObj.Attach(id, act);
      }
      else
        act();
    }
  }

  private void StoryStartApi()
  {
    DeckInfo playerDeck = this.playerDecks[this.selectDeck];
    WebAPI.BattleStoryStart(playerDeck.deck_number, playerDeck.deck_type_id, 0, this.story_quest.quest_story_s.ID, this.friend == null ? "" : this.friend.target_player_id, this.friend == null ? 0 : this.friend.leader_player_unit_id, (Action<WebAPI.Response.UserError>) (error =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
    })).RunOn<WebAPI.Response.BattleStoryStart>((MonoBehaviour) Singleton<NGSceneManager>.GetInstance(), (Action<WebAPI.Response.BattleStoryStart>) (battle =>
    {
      if (battle == null)
        return;
      for (int index = 0; index < battle.helpers.Length; ++index)
      {
        battle.helpers[index].leader_unit = battle.helper_player_units[index];
        battle.helpers[index].leader_unit.importOverkillersUnits(battle.helper_player_unit_over_killers);
        battle.helpers[index].leader_unit.primary_equipped_gear = battle.helpers[index].leader_unit.FindEquippedGear(battle.helper_player_gears);
        battle.helpers[index].leader_unit.primary_equipped_gear2 = battle.helpers[index].leader_unit.FindEquippedGear2(battle.helper_player_gears);
        battle.helpers[index].leader_unit.primary_equipped_gear3 = battle.helpers[index].leader_unit.FindEquippedGear3(battle.helper_player_gears);
        battle.helpers[index].leader_unit.primary_equipped_reisou = battle.helpers[index].leader_unit.FindEquippedReisou(battle.helper_player_gears, battle.helper_player_reisou_gears);
        battle.helpers[index].leader_unit.primary_equipped_reisou2 = battle.helpers[index].leader_unit.FindEquippedReisou2(battle.helper_player_gears, battle.helper_player_reisou_gears);
        battle.helpers[index].leader_unit.primary_equipped_reisou3 = battle.helpers[index].leader_unit.FindEquippedReisou3(battle.helper_player_gears, battle.helper_player_reisou_gears);
        battle.helpers[index].leader_unit.primary_equipped_awake_skill = battle.helpers[index].leader_unit.FindEquippedExtraSkill(battle.helper_player_awake_skills);
      }
      int[] guestsId = GuestUnit.GetGuestsID(battle.quest_s_id);
      this.StartBattle(BattleInfo.MakeBattleInfo(battle.battle_uuid, (CommonQuestType) battle.quest_type, battle.quest_s_id, battle.deck_type_id, battle.quest_loop_count, battle.deck_number, ((IEnumerable<PlayerHelper>) battle.helpers).FirstOrDefault<PlayerHelper>(), battle.enemy, ((IEnumerable<WebAPI.Response.BattleStoryStartEnemy_item>) battle.enemy_item).Select<WebAPI.Response.BattleStoryStartEnemy_item, Tuple<int, int, int, int>>((Func<WebAPI.Response.BattleStoryStartEnemy_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(), battle.user_deck_units, battle.user_deck_gears, battle.user_deck_enemy, ((IEnumerable<WebAPI.Response.BattleStoryStartUser_deck_enemy_item>) battle.user_deck_enemy_item).Select<WebAPI.Response.BattleStoryStartUser_deck_enemy_item, Tuple<int, int, int, int>>((Func<WebAPI.Response.BattleStoryStartUser_deck_enemy_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(), battle.panel, ((IEnumerable<WebAPI.Response.BattleStoryStartPanel_item>) battle.panel_item).Select<WebAPI.Response.BattleStoryStartPanel_item, Tuple<int, int, int, int>>((Func<WebAPI.Response.BattleStoryStartPanel_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(), guestsId, (PlayerUnit[]) null, (Tuple<int, int>[]) null));
    }));
    Persist.lastsortie.Data.SaveLastSortie(this.story_quest.quest_story_s.ID, this.story_quest.quest_story_s.quest_m_QuestStoryM, this.story_quest.quest_story_s.quest_l_QuestStoryL);
    Persist.lastsortie.Flush();
    if (MasterData.QuestStoryS[this.story_quest.quest_story_s.ID].quest_xl_QuestStoryXL == 6)
    {
      Persist.integralNoahProcess.Data.lastIntegralNoahSId = this.story_quest.quest_story_s.ID;
      Persist.integralNoahProcess.Flush();
    }
    if (MasterData.QuestStoryS[this.story_quest.quest_story_s.ID].quest_xl_QuestStoryXL == 7)
    {
      Persist.everAfterProcess.Data.lastEverAfterSId = this.story_quest.quest_story_s.ID;
      Persist.everAfterProcess.Flush();
    }
    if (!Persist.storyModePopupInfo.Exists)
    {
      try
      {
        Persist.storyModePopupInfo.Data.reset();
        Persist.storyModePopupInfo.Flush();
      }
      catch
      {
        Persist.storyModePopupInfo.Delete();
        Persist.storyModePopupInfo.Data.reset();
      }
    }
    if (!Persist.storyModePopupInfo.Exists || Persist.storyModePopupInfo.Data.alreadyShow)
      return;
    PlayerStoryQuestS[] source = SMManager.Get<PlayerStoryQuestS[]>();
    if (source == null)
      return;
    if (!((IEnumerable<PlayerStoryQuestS>) source).Any<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x.quest_story_s.quest_l_QuestStoryL >= 19)))
      return;
    try
    {
      Persist.storyModePopupInfo.Data.alreadyShow = true;
      Persist.storyModePopupInfo.Flush();
    }
    catch
    {
    }
  }

  private void ExtraStartApi()
  {
    DeckInfo playerDeck = this.playerDecks[this.selectDeck];
    if (this.extra_quest.quest_extra_s.wave != null)
      WebAPI.BattleWaveStart(playerDeck.deck_number, playerDeck.deck_type_id, 0, this.extra_quest.quest_extra_s.ID, this.friend == null ? "" : this.friend.target_player_id, this.friend == null ? 0 : this.friend.leader_player_unit_id, (Action<WebAPI.Response.UserError>) (error =>
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        WebAPI.DefaultUserErrorCallback(error);
        MypageScene.ChangeSceneOnError();
      })).RunOn<WebAPI.Response.BattleWaveStart>((MonoBehaviour) Singleton<NGSceneManager>.GetInstance(), (Action<WebAPI.Response.BattleWaveStart>) (battle =>
      {
        if (battle == null)
          return;
        for (int index = 0; index < battle.helpers.Length; ++index)
        {
          battle.helpers[index].leader_unit = battle.helper_player_units[index];
          battle.helpers[index].leader_unit.primary_equipped_gear = battle.helpers[index].leader_unit.FindEquippedGear(battle.helper_player_gears);
          battle.helpers[index].leader_unit.primary_equipped_gear2 = battle.helpers[index].leader_unit.FindEquippedGear2(battle.helper_player_gears);
          battle.helpers[index].leader_unit.primary_equipped_gear3 = battle.helpers[index].leader_unit.FindEquippedGear3(battle.helper_player_gears);
          battle.helpers[index].leader_unit.primary_equipped_reisou = battle.helpers[index].leader_unit.FindEquippedReisou(battle.helper_player_gears, battle.helper_player_reisou_gears);
          battle.helpers[index].leader_unit.primary_equipped_reisou2 = battle.helpers[index].leader_unit.FindEquippedReisou2(battle.helper_player_gears, battle.helper_player_reisou_gears);
          battle.helpers[index].leader_unit.primary_equipped_reisou3 = battle.helpers[index].leader_unit.FindEquippedReisou3(battle.helper_player_gears, battle.helper_player_reisou_gears);
          battle.helpers[index].leader_unit.primary_equipped_awake_skill = battle.helpers[index].leader_unit.FindEquippedExtraSkill(battle.helper_player_awake_skills);
        }
        int[] guests = battle.wave_stage == null || battle.wave_stage.Length == 0 ? GuestUnit.GetGuestsID(battle.quest_s_id) : GuestUnit.GetGuestsID(battle.wave_stage[0].stage_id);
        List<BattleInfo.Wave> wave = new List<BattleInfo.Wave>();
        foreach (BattleWaveStageInfo battleWaveStageInfo in battle.wave_stage)
          wave.Add(new BattleInfo.Wave()
          {
            stage_id = battleWaveStageInfo.stage_id,
            enemies = battleWaveStageInfo.enemy,
            enemy_items = ((IEnumerable<BattleWaveStageInfoEnemy_item>) battleWaveStageInfo.enemy_item).Select<BattleWaveStageInfoEnemy_item, Tuple<int, int, int, int>>((Func<BattleWaveStageInfoEnemy_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(),
            user_enemies = battleWaveStageInfo.user_deck_enemy,
            user_enemy_items = ((IEnumerable<BattleWaveStageInfoUser_deck_enemy_item>) battleWaveStageInfo.user_deck_enemy_item).Select<BattleWaveStageInfoUser_deck_enemy_item, Tuple<int, int, int, int>>((Func<BattleWaveStageInfoUser_deck_enemy_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(),
            panels = battleWaveStageInfo.panel,
            panel_items = ((IEnumerable<BattleWaveStageInfoPanel_item>) battleWaveStageInfo.panel_item).Select<BattleWaveStageInfoPanel_item, Tuple<int, int, int, int>>((Func<BattleWaveStageInfoPanel_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(),
            user_units = battleWaveStageInfo.user_deck_units,
            user_items = battleWaveStageInfo.user_deck_gears
          });
        this.StartBattle(BattleInfo.MakeBattleInfo(battle.battle_uuid, (CommonQuestType) battle.quest_type, battle.quest_s_id, battle.deck_type_id, battle.quest_loop_count, battle.deck_number, ((IEnumerable<PlayerHelper>) battle.helpers).FirstOrDefault<PlayerHelper>(), guests, (IEnumerable<BattleInfo.Wave>) wave));
      }));
    else
      WebAPI.BattleExtraStart(playerDeck.deck_number, playerDeck.deck_type_id, 0, this.extra_quest.quest_extra_s.ID, this.friend == null ? "" : this.friend.target_player_id, this.friend == null ? 0 : this.friend.leader_player_unit_id, (Action<WebAPI.Response.UserError>) (error =>
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        WebAPI.DefaultUserErrorCallback(error);
        MypageScene.ChangeSceneOnError();
      })).RunOn<WebAPI.Response.BattleExtraStart>((MonoBehaviour) Singleton<NGSceneManager>.GetInstance(), (Action<WebAPI.Response.BattleExtraStart>) (battle =>
      {
        if (battle == null)
          return;
        for (int index = 0; index < battle.helpers.Length; ++index)
        {
          battle.helpers[index].leader_unit = battle.helper_player_units[index];
          battle.helpers[index].leader_unit.importOverkillersUnits(battle.helper_player_unit_over_killers);
          battle.helpers[index].leader_unit.primary_equipped_gear = battle.helpers[index].leader_unit.FindEquippedGear(battle.helper_player_gears);
          battle.helpers[index].leader_unit.primary_equipped_gear2 = battle.helpers[index].leader_unit.FindEquippedGear2(battle.helper_player_gears);
          battle.helpers[index].leader_unit.primary_equipped_gear3 = battle.helpers[index].leader_unit.FindEquippedGear3(battle.helper_player_gears);
          battle.helpers[index].leader_unit.primary_equipped_reisou = battle.helpers[index].leader_unit.FindEquippedReisou(battle.helper_player_gears, battle.helper_player_reisou_gears);
          battle.helpers[index].leader_unit.primary_equipped_reisou2 = battle.helpers[index].leader_unit.FindEquippedReisou2(battle.helper_player_gears, battle.helper_player_reisou_gears);
          battle.helpers[index].leader_unit.primary_equipped_reisou3 = battle.helpers[index].leader_unit.FindEquippedReisou3(battle.helper_player_gears, battle.helper_player_reisou_gears);
          battle.helpers[index].leader_unit.primary_equipped_awake_skill = battle.helpers[index].leader_unit.FindEquippedExtraSkill(battle.helper_player_awake_skills);
        }
        int[] guestsId = GuestUnit.GetGuestsID(battle.quest_s_id);
        this.StartBattle(BattleInfo.MakeBattleInfo(battle.battle_uuid, (CommonQuestType) battle.quest_type, battle.quest_s_id, battle.deck_type_id, battle.quest_loop_count, battle.deck_number, ((IEnumerable<PlayerHelper>) battle.helpers).FirstOrDefault<PlayerHelper>(), battle.enemy, ((IEnumerable<WebAPI.Response.BattleExtraStartEnemy_item>) battle.enemy_item).Select<WebAPI.Response.BattleExtraStartEnemy_item, Tuple<int, int, int, int>>((Func<WebAPI.Response.BattleExtraStartEnemy_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(), battle.user_deck_units, battle.user_deck_gears, battle.user_deck_enemy, ((IEnumerable<WebAPI.Response.BattleExtraStartUser_deck_enemy_item>) battle.user_deck_enemy_item).Select<WebAPI.Response.BattleExtraStartUser_deck_enemy_item, Tuple<int, int, int, int>>((Func<WebAPI.Response.BattleExtraStartUser_deck_enemy_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(), battle.panel, ((IEnumerable<WebAPI.Response.BattleExtraStartPanel_item>) battle.panel_item).Select<WebAPI.Response.BattleExtraStartPanel_item, Tuple<int, int, int, int>>((Func<WebAPI.Response.BattleExtraStartPanel_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(), guestsId, (PlayerUnit[]) null, (Tuple<int, int>[]) null));
      }));
  }

  private void CharacterStartApi()
  {
    DeckInfo playerDeck = this.playerDecks[this.selectDeck];
    WebAPI.BattleCharacterStart(playerDeck.deck_number, playerDeck.deck_type_id, 0, this.char_quest.quest_character_s.ID, this.friend == null ? "" : this.friend.target_player_id, this.friend == null ? 0 : this.friend.leader_player_unit_id, (Action<WebAPI.Response.UserError>) (error =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
    })).RunOn<WebAPI.Response.BattleCharacterStart>((MonoBehaviour) Singleton<NGSceneManager>.GetInstance(), (Action<WebAPI.Response.BattleCharacterStart>) (battle =>
    {
      if (battle == null)
        return;
      for (int index = 0; index < battle.helpers.Length; ++index)
      {
        battle.helpers[index].leader_unit = battle.helper_player_units[index];
        battle.helpers[index].leader_unit.importOverkillersUnits(battle.helper_player_unit_over_killers);
        battle.helpers[index].leader_unit.primary_equipped_gear = battle.helpers[index].leader_unit.FindEquippedGear(battle.helper_player_gears);
        battle.helpers[index].leader_unit.primary_equipped_gear2 = battle.helpers[index].leader_unit.FindEquippedGear2(battle.helper_player_gears);
        battle.helpers[index].leader_unit.primary_equipped_gear3 = battle.helpers[index].leader_unit.FindEquippedGear3(battle.helper_player_gears);
        battle.helpers[index].leader_unit.primary_equipped_reisou = battle.helpers[index].leader_unit.FindEquippedReisou(battle.helper_player_gears, battle.helper_player_reisou_gears);
        battle.helpers[index].leader_unit.primary_equipped_reisou2 = battle.helpers[index].leader_unit.FindEquippedReisou2(battle.helper_player_gears, battle.helper_player_reisou_gears);
        battle.helpers[index].leader_unit.primary_equipped_reisou3 = battle.helpers[index].leader_unit.FindEquippedReisou3(battle.helper_player_gears, battle.helper_player_reisou_gears);
        battle.helpers[index].leader_unit.primary_equipped_awake_skill = battle.helpers[index].leader_unit.FindEquippedExtraSkill(battle.helper_player_awake_skills);
      }
      int[] guestsId = GuestUnit.GetGuestsID(battle.quest_s_id);
      this.StartBattle(BattleInfo.MakeBattleInfo(battle.battle_uuid, (CommonQuestType) battle.quest_type, battle.quest_s_id, battle.deck_type_id, battle.quest_loop_count, battle.deck_number, ((IEnumerable<PlayerHelper>) battle.helpers).FirstOrDefault<PlayerHelper>(), battle.enemy, ((IEnumerable<WebAPI.Response.BattleCharacterStartEnemy_item>) battle.enemy_item).Select<WebAPI.Response.BattleCharacterStartEnemy_item, Tuple<int, int, int, int>>((Func<WebAPI.Response.BattleCharacterStartEnemy_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(), battle.user_deck_units, battle.user_deck_gears, battle.user_deck_enemy, ((IEnumerable<WebAPI.Response.BattleCharacterStartUser_deck_enemy_item>) battle.user_deck_enemy_item).Select<WebAPI.Response.BattleCharacterStartUser_deck_enemy_item, Tuple<int, int, int, int>>((Func<WebAPI.Response.BattleCharacterStartUser_deck_enemy_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(), battle.panel, ((IEnumerable<WebAPI.Response.BattleCharacterStartPanel_item>) battle.panel_item).Select<WebAPI.Response.BattleCharacterStartPanel_item, Tuple<int, int, int, int>>((Func<WebAPI.Response.BattleCharacterStartPanel_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(), guestsId, (PlayerUnit[]) null, (Tuple<int, int>[]) null));
    }));
  }

  private void CharacterStartApi2()
  {
    DeckInfo playerDeck = this.playerDecks[this.selectDeck];
    WebAPI.BattleCharacterStart(playerDeck.deck_number, playerDeck.deck_type_id, 0, this.convert_quest.questS.ID, this.friend == null ? "" : this.friend.target_player_id, this.friend == null ? 0 : this.friend.leader_player_unit_id, (Action<WebAPI.Response.UserError>) (error =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
    })).RunOn<WebAPI.Response.BattleCharacterStart>((MonoBehaviour) Singleton<NGSceneManager>.GetInstance(), (Action<WebAPI.Response.BattleCharacterStart>) (battle =>
    {
      if (battle == null)
        return;
      for (int index = 0; index < battle.helpers.Length; ++index)
      {
        battle.helpers[index].leader_unit = battle.helper_player_units[index];
        battle.helpers[index].leader_unit.importOverkillersUnits(battle.helper_player_unit_over_killers);
        battle.helpers[index].leader_unit.primary_equipped_gear = battle.helpers[index].leader_unit.FindEquippedGear(battle.helper_player_gears);
        battle.helpers[index].leader_unit.primary_equipped_gear2 = battle.helpers[index].leader_unit.FindEquippedGear2(battle.helper_player_gears);
        battle.helpers[index].leader_unit.primary_equipped_gear3 = battle.helpers[index].leader_unit.FindEquippedGear3(battle.helper_player_gears);
        battle.helpers[index].leader_unit.primary_equipped_reisou = battle.helpers[index].leader_unit.FindEquippedReisou(battle.helper_player_gears, battle.helper_player_reisou_gears);
        battle.helpers[index].leader_unit.primary_equipped_reisou2 = battle.helpers[index].leader_unit.FindEquippedReisou2(battle.helper_player_gears, battle.helper_player_reisou_gears);
        battle.helpers[index].leader_unit.primary_equipped_reisou3 = battle.helpers[index].leader_unit.FindEquippedReisou3(battle.helper_player_gears, battle.helper_player_reisou_gears);
        battle.helpers[index].leader_unit.primary_equipped_awake_skill = battle.helpers[index].leader_unit.FindEquippedExtraSkill(battle.helper_player_awake_skills);
      }
      int[] guestsId = GuestUnit.GetGuestsID(battle.quest_s_id);
      this.CharacterQuestAfterBattleInfo = BattleInfo.MakeBattleInfo(battle.battle_uuid, (CommonQuestType) battle.quest_type, battle.quest_s_id, battle.deck_type_id, battle.quest_loop_count, battle.deck_number, ((IEnumerable<PlayerHelper>) battle.helpers).FirstOrDefault<PlayerHelper>(), battle.enemy, ((IEnumerable<WebAPI.Response.BattleCharacterStartEnemy_item>) battle.enemy_item).Select<WebAPI.Response.BattleCharacterStartEnemy_item, Tuple<int, int, int, int>>((Func<WebAPI.Response.BattleCharacterStartEnemy_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(), battle.user_deck_units, battle.user_deck_gears, battle.user_deck_enemy, ((IEnumerable<WebAPI.Response.BattleCharacterStartUser_deck_enemy_item>) battle.user_deck_enemy_item).Select<WebAPI.Response.BattleCharacterStartUser_deck_enemy_item, Tuple<int, int, int, int>>((Func<WebAPI.Response.BattleCharacterStartUser_deck_enemy_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(), battle.panel, ((IEnumerable<WebAPI.Response.BattleCharacterStartPanel_item>) battle.panel_item).Select<WebAPI.Response.BattleCharacterStartPanel_item, Tuple<int, int, int, int>>((Func<WebAPI.Response.BattleCharacterStartPanel_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(), guestsId, (PlayerUnit[]) null, (Tuple<int, int>[]) null);
      this.StartBattle(this.CharacterQuestAfterBattleInfo);
    }));
  }

  private void HarmonyStartApi()
  {
    DeckInfo playerDeck = this.playerDecks[this.selectDeck];
    WebAPI.BattleHarmonyStart(playerDeck.deck_number, playerDeck.deck_type_id, 0, this.convert_quest.questS.ID, this.friend == null ? "" : this.friend.target_player_id, this.friend == null ? 0 : this.friend.leader_player_unit_id, (Action<WebAPI.Response.UserError>) (error =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
    })).RunOn<WebAPI.Response.BattleHarmonyStart>((MonoBehaviour) Singleton<NGSceneManager>.GetInstance(), (Action<WebAPI.Response.BattleHarmonyStart>) (battle =>
    {
      if (battle == null)
        return;
      for (int index = 0; index < battle.helpers.Length; ++index)
      {
        battle.helpers[index].leader_unit = battle.helper_player_units[index];
        battle.helpers[index].leader_unit.importOverkillersUnits(battle.helper_player_unit_over_killers);
        battle.helpers[index].leader_unit.primary_equipped_gear = battle.helpers[index].leader_unit.FindEquippedGear(battle.helper_player_gears);
        battle.helpers[index].leader_unit.primary_equipped_gear2 = battle.helpers[index].leader_unit.FindEquippedGear2(battle.helper_player_gears);
        battle.helpers[index].leader_unit.primary_equipped_gear3 = battle.helpers[index].leader_unit.FindEquippedGear3(battle.helper_player_gears);
        battle.helpers[index].leader_unit.primary_equipped_reisou = battle.helpers[index].leader_unit.FindEquippedReisou(battle.helper_player_gears, battle.helper_player_reisou_gears);
        battle.helpers[index].leader_unit.primary_equipped_reisou2 = battle.helpers[index].leader_unit.FindEquippedReisou2(battle.helper_player_gears, battle.helper_player_reisou_gears);
        battle.helpers[index].leader_unit.primary_equipped_reisou3 = battle.helpers[index].leader_unit.FindEquippedReisou3(battle.helper_player_gears, battle.helper_player_reisou_gears);
        battle.helpers[index].leader_unit.primary_equipped_awake_skill = battle.helpers[index].leader_unit.FindEquippedExtraSkill(battle.helper_player_awake_skills);
      }
      int[] guestsId = GuestUnit.GetGuestsID(battle.quest_s_id);
      this.StartBattle(BattleInfo.MakeBattleInfo(battle.battle_uuid, (CommonQuestType) battle.quest_type, battle.quest_s_id, battle.deck_type_id, battle.quest_loop_count, battle.deck_number, ((IEnumerable<PlayerHelper>) battle.helpers).FirstOrDefault<PlayerHelper>(), battle.enemy, ((IEnumerable<WebAPI.Response.BattleHarmonyStartEnemy_item>) battle.enemy_item).Select<WebAPI.Response.BattleHarmonyStartEnemy_item, Tuple<int, int, int, int>>((Func<WebAPI.Response.BattleHarmonyStartEnemy_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(), battle.user_deck_units, battle.user_deck_gears, battle.user_deck_enemy, ((IEnumerable<WebAPI.Response.BattleHarmonyStartUser_deck_enemy_item>) battle.user_deck_enemy_item).Select<WebAPI.Response.BattleHarmonyStartUser_deck_enemy_item, Tuple<int, int, int, int>>((Func<WebAPI.Response.BattleHarmonyStartUser_deck_enemy_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(), battle.panel, ((IEnumerable<WebAPI.Response.BattleHarmonyStartPanel_item>) battle.panel_item).Select<WebAPI.Response.BattleHarmonyStartPanel_item, Tuple<int, int, int, int>>((Func<WebAPI.Response.BattleHarmonyStartPanel_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(), guestsId, (PlayerUnit[]) null, (Tuple<int, int>[]) null));
    }));
  }

  private void SeaStartApi()
  {
    DeckInfo playerDeck = this.playerDecks[this.selectDeck];
    WebAPI.SeaBattleStart(playerDeck.deck_number, playerDeck.deck_type_id, 0, this.sea_quest.quest_sea_s.ID, this.friend == null ? "" : this.friend.target_player_id, this.friend == null ? 0 : this.friend.leader_player_unit_id, (Action<WebAPI.Response.UserError>) (error =>
    {
      if (string.Equals(error.Code, "SEA000"))
      {
        this.StartCoroutine(PopupUtility.SeaError(error));
      }
      else
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        WebAPI.DefaultUserErrorCallback(error);
        MypageScene.ChangeSceneOnError();
      }
    })).RunOn<WebAPI.Response.SeaBattleStart>((MonoBehaviour) Singleton<NGSceneManager>.GetInstance(), (Action<WebAPI.Response.SeaBattleStart>) (battle =>
    {
      if (battle == null)
        return;
      for (int index = 0; index < battle.helpers.Length; ++index)
      {
        battle.helpers[index].leader_unit = battle.helper_player_units[index];
        battle.helpers[index].leader_unit.importOverkillersUnits(battle.helper_player_unit_over_killers);
        battle.helpers[index].leader_unit.primary_equipped_gear = battle.helpers[index].leader_unit.FindEquippedGear(battle.helper_player_gears);
        battle.helpers[index].leader_unit.primary_equipped_gear2 = battle.helpers[index].leader_unit.FindEquippedGear2(battle.helper_player_gears);
        battle.helpers[index].leader_unit.primary_equipped_gear3 = battle.helpers[index].leader_unit.FindEquippedGear3(battle.helper_player_gears);
        battle.helpers[index].leader_unit.primary_equipped_reisou = battle.helpers[index].leader_unit.FindEquippedReisou(battle.helper_player_gears, battle.helper_player_reisou_gears);
        battle.helpers[index].leader_unit.primary_equipped_reisou2 = battle.helpers[index].leader_unit.FindEquippedReisou2(battle.helper_player_gears, battle.helper_player_reisou_gears);
        battle.helpers[index].leader_unit.primary_equipped_reisou3 = battle.helpers[index].leader_unit.FindEquippedReisou3(battle.helper_player_gears, battle.helper_player_reisou_gears);
        battle.helpers[index].leader_unit.primary_equipped_awake_skill = battle.helpers[index].leader_unit.FindEquippedExtraSkill(battle.helper_player_awake_skills);
      }
      PlayerHelper helper = (PlayerHelper) null;
      if (((IEnumerable<SeaPlayerHelper>) battle.helpers).FirstOrDefault<SeaPlayerHelper>() != null)
        helper = new Helper(((IEnumerable<SeaPlayerHelper>) battle.helpers).FirstOrDefault<SeaPlayerHelper>()).Clone();
      int[] guestsId = GuestUnit.GetGuestsID(battle.quest_s_id);
      this.StartBattle(BattleInfo.MakeBattleInfo(battle.battle_uuid, (CommonQuestType) battle.quest_type, battle.quest_s_id, battle.deck_type_id, battle.quest_loop_count, battle.deck_number, helper, battle.enemy, ((IEnumerable<WebAPI.Response.SeaBattleStartEnemy_item>) battle.enemy_item).Select<WebAPI.Response.SeaBattleStartEnemy_item, Tuple<int, int, int, int>>((Func<WebAPI.Response.SeaBattleStartEnemy_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(), battle.user_deck_units, battle.user_deck_gears, battle.user_deck_enemy, ((IEnumerable<WebAPI.Response.SeaBattleStartUser_deck_enemy_item>) battle.user_deck_enemy_item).Select<WebAPI.Response.SeaBattleStartUser_deck_enemy_item, Tuple<int, int, int, int>>((Func<WebAPI.Response.SeaBattleStartUser_deck_enemy_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(), battle.panel, ((IEnumerable<WebAPI.Response.SeaBattleStartPanel_item>) battle.panel_item).Select<WebAPI.Response.SeaBattleStartPanel_item, Tuple<int, int, int, int>>((Func<WebAPI.Response.SeaBattleStartPanel_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(), guestsId, (PlayerUnit[]) null, (Tuple<int, int>[]) null));
    }));
    QuestSeaS questSeaS = this.sea_quest.quest_sea_s;
    if (!Persist.seaLastSortie.Data.saveLastSortie(questSeaS.quest_xl_QuestSeaXL, questSeaS.ID))
      return;
    Persist.seaLastSortie.Flush();
  }

  private IEnumerator SetSupplyIcons(List<PlayerItem> SupplyList)
  {
    for (int i = 0; i < SupplyList.Count; ++i)
    {
      GameObject gameObject = this.prefabItemIcon.Clone(this.dir_Items[i].transform);
      gameObject.transform.localScale = new Vector3(1f, 1f);
      ItemIcon itemIconScript = gameObject.GetComponent<ItemIcon>();
      this.itemIconList.Add(itemIconScript);
      IEnumerator e = itemIconScript.InitByPlayerItem(SupplyList[i]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.setClickSupplyIconEvents(itemIconScript, SupplyList[i].supply.ID);
      if (Singleton<NGGameDataManager>.GetInstance().IsSea)
        ((Component) itemIconScript).transform.localScale = new Vector3(1f, 1f);
      else
        ((Component) itemIconScript).transform.localScale = new Vector3(0.7f, 0.7f);
      itemIconScript = (ItemIcon) null;
    }
    for (int count = SupplyList.Count; count < Consts.GetInstance().DECK_SUPPLY_MAX; ++count)
    {
      GameObject gameObject = this.prefabItemIcon.Clone(this.dir_Items[count].transform);
      gameObject.transform.localScale = new Vector3(1f, 1f);
      ItemIcon component = gameObject.GetComponent<ItemIcon>();
      component.SetModeSupply();
      component.SetEmpty(true);
      if (Singleton<NGGameDataManager>.GetInstance().IsSea)
        ((Component) component).transform.localScale = new Vector3(1f, 1f);
      else
        ((Component) component).transform.localScale = new Vector3(0.7f, 0.7f);
    }
  }

  private void setClickSupplyIconEvents(ItemIcon icon, int itemId)
  {
    icon.onClick = (Action<ItemIcon>) (_ => this.IbtnItemedit());
    icon.EnableLongPressEvent((Action<GameCore.ItemInfo>) (_ => this.StartCoroutine(this.setDetailPopup(itemId))));
  }

  public void EndScene()
  {
    for (int index = 0; index < Consts.GetInstance().DECK_SUPPLY_MAX; ++index)
    {
      foreach (Component componentsInChild in this.dir_Items[index].GetComponentsInChildren<ItemIcon>())
        Object.Destroy((Object) componentsInChild.gameObject);
    }
    foreach (Quest0028Indicator quest0028Indicator in this.indicators.Values)
      quest0028Indicator.DestroyObject();
    this.indicators.Clear();
    this.SaveSetting();
  }

  protected void SaveSetting()
  {
    if (this.battleSettingInitialized)
    {
      if (Object.op_Inequality((Object) this.toggleAuto_, (Object) null))
        Persist.autoBattleSetting.Data.isAutoBattle = this.toggleAuto_.isSwitch;
      else if (Object.op_Inequality((Object) this.slcAutoBattleToggle, (Object) null))
        Persist.autoBattleSetting.Data.isAutoBattle = this.slcAutoBattleToggle.activeSelf;
    }
    if (this.story_only)
      return;
    if (this.isSea_)
    {
      Persist.seaDeckOrganized.Data.number = this.lastSelectedNumber;
      Persist.seaDeckOrganized.Flush();
    }
    else
    {
      Persist.DeckOrganized data = Persist.deckOrganized.Data;
      if (this.deckMode == Quest0028Menu.DeckMode.Custom)
      {
        data.customNumber = this.lastSelectedNumber;
        data.isCustom = true;
      }
      else
      {
        data.number = this.lastSelectedNumber;
        data.isCustom = false;
      }
      Persist.deckOrganized.Flush();
    }
  }

  private int lastSelectedNumber
  {
    get => this.deckIndexs[Mathf.Clamp(this.indicator.selected, 0, this.playerDecks.Length - 1)];
  }

  private IEnumerator WaitScrollSe()
  {
    yield return (object) new WaitForSeconds(0.3f);
    this.indicator.SeEnable = true;
  }

  public void BtnRepair()
  {
    if (this.IsPushAndSet())
      return;
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      this.StartCoroutine(this.changeSceneRepair());
    }
    else
    {
      Bugu00524Scene.ChangeScene(true);
      this.onChangedSceneRepair(false);
    }
  }

  protected virtual void onChangedSceneRepair(bool isSea)
  {
  }

  private IEnumerator changeSceneRepair()
  {
    Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
    Bugu00524Scene.ChangeScene(true);
    this.onChangedSceneRepair(true);
    yield break;
  }

  public void BtnOrganization()
  {
    if (this.IsPushAndSet())
      return;
    if (this.deckMode == Quest0028Menu.DeckMode.Custom)
      EditCustomDeckScene.changeScene(true, this.lastSelectedNumber);
    else
      Unit0046Scene.changeScene(true, this.regulation.limitationParams, this.limitationLabel);
    this.dicStackSelected.Remove(this.deckMode);
  }

  public void BtnWeaponChange()
  {
    if (this.IsPushAndSet())
      return;
    Unit00468Scene.changeScene00412(true);
  }

  public void BtnWeaponChangeSlot()
  {
    Quest0028Indicator indicator = this.indicators[this.selectDeck];
    if (!indicator.ChangeWpSlotCheck() && !this.friendWpChange)
      return;
    indicator.ChangeWpSlot();
    if (!this.friendWpChange)
      return;
    for (int index = 0; index < this.linkFriendGear.Length; ++index)
      this.linkFriendGear[index].SetActive(!this.linkFriendGear[index].activeSelf);
  }

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    NGSceneManager instance1 = Singleton<NGSceneManager>.GetInstance();
    NGGameDataManager instance2 = Singleton<NGGameDataManager>.GetInstance();
    if (instance1.HasSavedChangeSceneParam() && instance2.IsFromPopupStageList)
    {
      NGSceneManager.ChangeSceneParam changeSceneParam = instance1.GetSavedChangeSceneParam();
      switch (changeSceneParam.sceneName)
      {
        case "unit004_JobChange":
          instance2.fromPopup = NGGameDataManager.FromPopup.Unit004JobChangeScene;
          break;
        case "unit004_2":
        case "unit004_2_sea":
          if (instance2.fromPopup != NGGameDataManager.FromPopup.Unit0042SceneCharacterQuest)
          {
            instance2.fromPopup = NGGameDataManager.FromPopup.Unit0042SceneUnity;
            break;
          }
          break;
        case "unit004_training":
          instance2.fromPopup = NGGameDataManager.FromPopup.Unit004Combine;
          break;
        default:
          instance2.fromPopup = NGGameDataManager.FromPopup.None;
          break;
      }
      instance1.ModifySceneStack(changeSceneParam);
      instance1.ClearSavedChangeSceneParam();
      instance2.IsFromPopupStageList = false;
      instance2.QuestType = new CommonQuestType?();
      instance2.returnSceneFromQuest = (Action) null;
      base.backScene();
    }
    else
      this.backScene();
  }

  protected override void backScene()
  {
    if (Singleton<NGSceneManager>.GetInstance().backScene())
      return;
    if (this.story_quest != null)
    {
      QuestStoryS questStoryS = this.story_quest.quest_story_s;
      Quest0022Scene.ChangeScene0022(false, questStoryS.quest_l_QuestStoryL, questStoryS.quest_m_QuestStoryM, questStoryS.ID);
    }
    else if (this.extra_quest != null)
      Quest00220Scene.ChangeScene00220(this.extra_quest._quest_extra_s);
    else if (this.sea_quest != null)
    {
      QuestSeaS questSeaS = this.sea_quest.quest_sea_s;
      Quest0022Scene.ChangeSceneSea(false, questSeaS.quest_xl_QuestSeaXL, questSeaS.quest_l_QuestSeaL, questSeaS.quest_m_QuestSeaM);
    }
    else
      base.backScene();
  }

  public override void onBackButton()
  {
    if (this.isPlayingMovie)
      return;
    this.IbtnBack();
  }

  public virtual void IbtnSelectFriend()
  {
    if (this.IsPushAndSet())
      return;
    if (this.story_quest != null)
      Quest00282Scene.changeScene(true, this.story_quest, this.onSetHelper_);
    else if (this.extra_quest != null)
      Quest00282Scene.changeScene(true, this.extra_quest, this.onSetHelper_);
    else if (this.char_quest != null)
      Quest00282Scene.changeScene(true, this.char_quest, this.onSetHelper_);
    else if (this.convert_quest != null)
    {
      Quest00282Scene.changeScene(true, this.convert_quest, this.onSetHelper_);
    }
    else
    {
      if (this.sea_quest == null)
        return;
      Quest00282Scene.changeScene(true, this.sea_quest, this.onSetHelper_);
    }
  }

  public void IbtnLeaderSkill()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().monitorCoroutine(this.doPopupLeaderSkill());
  }

  private IEnumerator doPopupLeaderSkill()
  {
    Quest0028Menu quest0028Menu = this;
    IEnumerator e = PopupLeaderFriendSkill.show(quest0028Menu.prefabUnitIcon, quest0028Menu.isSea_, quest0028Menu.indicators[quest0028Menu.selectDeck].deckUnitData[0], quest0028Menu.friendUnit, quest0028Menu.friendLeaderSkill);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    quest0028Menu.IsPush = false;
  }

  public void IbtnBattleSetting()
  {
    if (this.IsPushAndSet())
      return;
    if (Object.op_Inequality((Object) this.toggleAuto_, (Object) null))
      Persist.autoBattleSetting.Data.isAutoBattle = this.toggleAuto_.isSwitch;
    else if (Object.op_Inequality((Object) this.slcAutoBattleToggle, (Object) null))
      Persist.autoBattleSetting.Data.isAutoBattle = this.slcAutoBattleToggle.activeSelf;
    Singleton<PopupManager>.GetInstance().monitorCoroutine(this.doPopupBattleSetting());
  }

  private IEnumerator doPopupBattleSetting()
  {
    Quest0028Menu quest0028Menu = this;
    IEnumerator e = Quest0028PopupBattleSetting.show();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    quest0028Menu.battleSettingBack_ = true;
    quest0028Menu.IsPush = false;
  }

  public void IbtnAutoBattle()
  {
    this.slcAutoBattleToggle.SetActive(!this.slcAutoBattleToggle.activeSelf);
    Persist.autoBattleSetting.Data.isAutoBattle = this.slcAutoBattleToggle.activeSelf;
  }

  public void IbtnAutoLap()
  {
    this.slcAutoLapToggle.SetActive(!this.slcAutoLapToggle.activeSelf);
    Singleton<NGGameDataManager>.GetInstance().questAutoLap = this.slcAutoLapToggle.activeSelf;
  }

  private int getScriptID(BattleInfo info)
  {
    int scriptId = -1;
    switch (info.quest_type)
    {
      case CommonQuestType.Story:
        StoryPlaybackStoryDetail playbackStoryDetail = ((IEnumerable<StoryPlaybackStoryDetail>) MasterData.StoryPlaybackStoryDetailList).Where<StoryPlaybackStoryDetail>((Func<StoryPlaybackStoryDetail, bool>) (x => x.quest_s_id_QuestStoryS == info.quest_s_id && x.timing == StoryPlaybackTiming.select_stage)).FirstOrDefault<StoryPlaybackStoryDetail>();
        if (playbackStoryDetail != null)
        {
          scriptId = playbackStoryDetail.script_id;
          break;
        }
        break;
      case CommonQuestType.Character:
        IEnumerable<StoryPlaybackCharacterDetail> source = ((IEnumerable<StoryPlaybackCharacterDetail>) MasterData.StoryPlaybackCharacterDetailList).Where<StoryPlaybackCharacterDetail>((Func<StoryPlaybackCharacterDetail, bool>) (x => x.quest_QuestCharacterS == info.quest_s_id));
        if (source.Count<StoryPlaybackCharacterDetail>() > 0)
        {
          StoryPlaybackCharacterDetail playbackCharacterDetail1 = (source.Where<StoryPlaybackCharacterDetail>((Func<StoryPlaybackCharacterDetail, bool>) (x => x.timing == StoryPlaybackTiming.select_stage)).FirstOrDefault<StoryPlaybackCharacterDetail>() ?? source.Where<StoryPlaybackCharacterDetail>((Func<StoryPlaybackCharacterDetail, bool>) (x => x.timing == StoryPlaybackTiming.before_battle)).FirstOrDefault<StoryPlaybackCharacterDetail>()) ?? source.Where<StoryPlaybackCharacterDetail>((Func<StoryPlaybackCharacterDetail, bool>) (x => x.timing == StoryPlaybackTiming.after_battle)).FirstOrDefault<StoryPlaybackCharacterDetail>();
          if (playbackCharacterDetail1 != null)
          {
            scriptId = playbackCharacterDetail1.script_id;
            if (playbackCharacterDetail1.timing != StoryPlaybackTiming.after_battle)
            {
              StoryPlaybackCharacterDetail playbackCharacterDetail2 = source.Where<StoryPlaybackCharacterDetail>((Func<StoryPlaybackCharacterDetail, bool>) (x => x.timing == StoryPlaybackTiming.after_battle)).FirstOrDefault<StoryPlaybackCharacterDetail>();
              if (playbackCharacterDetail2 != null)
              {
                this.CharacterQuestAfterBattleScriptId = playbackCharacterDetail2.script_id;
                break;
              }
              break;
            }
            break;
          }
          break;
        }
        break;
      case CommonQuestType.Extra:
        StoryPlaybackExtraDetail playbackExtraDetail = ((IEnumerable<StoryPlaybackExtraDetail>) MasterData.StoryPlaybackExtraDetailList).Where<StoryPlaybackExtraDetail>((Func<StoryPlaybackExtraDetail, bool>) (x => x.quest_QuestExtraS == info.quest_s_id && x.timing == StoryPlaybackTiming.select_stage)).FirstOrDefault<StoryPlaybackExtraDetail>();
        if (playbackExtraDetail != null)
        {
          scriptId = playbackExtraDetail.script_id;
          break;
        }
        break;
      case CommonQuestType.Sea:
        StoryPlaybackSeaDetail playbackSeaDetail = ((IEnumerable<StoryPlaybackSeaDetail>) MasterData.StoryPlaybackSeaDetailList).Where<StoryPlaybackSeaDetail>((Func<StoryPlaybackSeaDetail, bool>) (x => x.quest_s_id_QuestSeaS == info.quest_s_id && x.timing == StoryPlaybackTiming.select_stage)).FirstOrDefault<StoryPlaybackSeaDetail>();
        if (playbackSeaDetail != null)
        {
          scriptId = playbackSeaDetail.script_id;
          break;
        }
        break;
    }
    return scriptId;
  }

  public static bool IsExtraLimitation(PlayerExtraQuestS extraQuest)
  {
    return ((IEnumerable<QuestExtraLimitation>) MasterData.QuestExtraLimitationList).Any<QuestExtraLimitation>((Func<QuestExtraLimitation, bool>) (n => n.quest_s_id_QuestExtraS == extraQuest.quest_extra_s.ID));
  }

  public static bool IsStoryLimitation(PlayerStoryQuestS storyQuest)
  {
    return ((IEnumerable<QuestStoryLimitation>) MasterData.QuestStoryLimitationList).Any<QuestStoryLimitation>((Func<QuestStoryLimitation, bool>) (n => n.quest_s_id_QuestStoryS == storyQuest.quest_story_s.ID));
  }

  public static bool IsCharaLimitation(PlayerCharacterQuestS charaQuest)
  {
    return ((IEnumerable<QuestCharacterLimitation>) MasterData.QuestCharacterLimitationList).Any<QuestCharacterLimitation>((Func<QuestCharacterLimitation, bool>) (n => n.quest_s_id_QuestCharacterS == charaQuest.quest_character_s.ID));
  }

  public static bool IsConvertLimitation(PlayerQuestSConverter convertQuest)
  {
    if (convertQuest.questS.data_type == QuestSConverter.DataType.Character)
      return ((IEnumerable<QuestCharacterLimitation>) MasterData.QuestCharacterLimitationList).Any<QuestCharacterLimitation>((Func<QuestCharacterLimitation, bool>) (n => n.quest_s_id_QuestCharacterS == convertQuest.questS.ID));
    return convertQuest.questS.data_type == QuestSConverter.DataType.Harmony && ((IEnumerable<QuestHarmonyLimitation>) MasterData.QuestHarmonyLimitationList).Any<QuestHarmonyLimitation>((Func<QuestHarmonyLimitation, bool>) (n => n.quest_s_id_QuestHarmonyS == convertQuest.questS.ID));
  }

  public static bool IsSeaLimitation(PlayerSeaQuestS seaQuest)
  {
    return ((IEnumerable<QuestStoryLimitation>) MasterData.QuestStoryLimitationList).Any<QuestStoryLimitation>((Func<QuestStoryLimitation, bool>) (n => n.quest_s_id_QuestStoryS == seaQuest.quest_sea_s.ID));
  }

  public static bool GetFriendPositionEnable(int stageID)
  {
    BattleStageGuest[] array = ((IEnumerable<BattleStageGuest>) MasterData.BattleStageGuestList).Where<BattleStageGuest>((Func<BattleStageGuest, bool>) (x => x.stage_BattleStage == stageID)).ToArray<BattleStageGuest>();
    return MasterData.BattleStagePlayer.Any<KeyValuePair<int, BattleStagePlayer>>((Func<KeyValuePair<int, BattleStagePlayer>, bool>) (x => x.Value.stage_BattleStage == stageID && x.Value.deck_position == Consts.GetInstance().DECK_POSITION_FRIEND)) && !((IEnumerable<BattleStageGuest>) array).Any<BattleStageGuest>((Func<BattleStageGuest, bool>) (x => x.deck_position == Consts.GetInstance().DECK_POSITION_FRIEND));
  }

  private IEnumerator setDetailPopup(int itemid)
  {
    IEnumerator e;
    if (Object.op_Equality((Object) this.detailPopup, (Object) null))
    {
      Future<GameObject> prefabF = Res.Prefabs.popup.popup_000_7_4_2__anim_popup01.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.detailPopup = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    GameObject popup = Singleton<PopupManager>.GetInstance().open(this.detailPopup);
    popup.SetActive(false);
    e = popup.GetComponent<Shop00742Menu>().Init(MasterDataTable.CommonRewardType.supply, itemid);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
  }

  public void FillItem()
  {
  }

  private bool setDeckModeButton(Quest0028Menu.DeckMode mode, bool bEnable)
  {
    if (!Object.op_Implicit((Object) this.topDeckModeButtons))
      return false;
    int num = this.deckMode != mode ? 1 : 0;
    this.deckMode = mode;
    ((IEnumerable<GameObject>) this.objDeckModeButtons).ToggleOnce((int) mode);
    ((UIButtonColor) this.objDeckModeButtons[(int) mode].GetComponent<UIButton>()).isEnabled = bEnable;
    this.objEditDeckName.SetActive(mode == Quest0028Menu.DeckMode.Custom);
    return num != 0;
  }

  public void onClickedChangeNormalDeck()
  {
    Quest0028Menu.DeckMode deckMode = this.deckMode;
    if (!this.setDeckModeButton(Quest0028Menu.DeckMode.Normal, true))
      return;
    this.dicStackSelected[deckMode] = this.lastSelectedNumber;
    this.StartCoroutine(this.doChangeDeckMode());
  }

  public void onClickedChangeCustomDeck()
  {
    Quest0028Menu.DeckMode deckMode = this.deckMode;
    if (!this.setDeckModeButton(Quest0028Menu.DeckMode.Custom, true))
      return;
    this.dicStackSelected[deckMode] = this.lastSelectedNumber;
    this.StartCoroutine(this.doChangeDeckMode());
  }

  private IEnumerator doChangeDeckMode()
  {
    DateTime wait = DateTime.Now.AddSeconds(0.3);
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    if (this.regulationDeck != null)
      this.regulationDeck = this.deckMode != Quest0028Menu.DeckMode.Custom ? this.Regulation.Deck : this.Regulation.CustomDeck;
    this.indicator.SeEnable = false;
    IEnumerator e = this.doInitDeckPanels();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    while (wait > DateTime.Now)
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public void onClickedEditDeckName() => this.StartCoroutine(this.doPopupEditDeckName());

  private IEnumerator doPopupEditDeckName()
  {
    DeckInfo deck = this.playerDecks[this.selectDeck];
    if (deck.isCustom)
    {
      if (!Object.op_Implicit((Object) this.prefabEditDeckName_))
      {
        Singleton<PopupManager>.GetInstance().open((GameObject) null, isViewBack: false, isNonSe: true);
        Future<GameObject> ld = PopupEditDeckName.createPrefabLoader();
        IEnumerator e = ld.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.prefabEditDeckName_ = ld.Result;
        Singleton<PopupManager>.GetInstance().dismiss();
        ld = (Future<GameObject>) null;
      }
      PopupEditDeckName.show(this.prefabEditDeckName_, this.TxtTeamName, deck.customDeck);
    }
  }

  public enum DeckMode
  {
    Normal,
    Custom,
  }

  private class FriendInfo
  {
    public PlayerUnit target_leader_unit;
    public PlayerItem[] target_player_items;
    public PlayerGearReisouSchema[] target_player_reisou_items;

    public FriendInfo(WebAPI.Response.SeaFriendStatus info)
    {
      this.target_leader_unit = info.target_leader_unit;
      this.target_player_items = info.target_player_items;
      this.target_player_reisou_items = info.target_player_reisou_items;
    }

    public FriendInfo(WebAPI.Response.FriendStatus info)
    {
      this.target_leader_unit = info.target_leader_unit;
      this.target_player_items = info.target_player_items;
      this.target_player_reisou_items = info.target_player_reisou_items;
    }
  }

  private enum BtnType
  {
    BATTLE,
    REPAIR,
    EDIT,
  }

  protected class LimitedQuestData
  {
    public DeckInfo[] Deck { get; private set; }

    public DeckInfo[] CustomDeck { get; private set; }

    public QuestLimitationBase[] limitationParams { get; private set; }

    public void SetInfo(PlayerDeck[] tmp, PlayerCustomDeck[] tmp2, QuestLimitationBase[] limits)
    {
      this.Deck = tmp != null ? ((IEnumerable<PlayerDeck>) tmp).Select<PlayerDeck, DeckInfo>((Func<PlayerDeck, DeckInfo>) (x => PlayerDeck.createDeckInfo(x))).ToArray<DeckInfo>() : (DeckInfo[]) null;
      this.CustomDeck = tmp2 != null ? ((IEnumerable<PlayerCustomDeck>) tmp2).Select<PlayerCustomDeck, DeckInfo>((Func<PlayerCustomDeck, DeckInfo>) (x => PlayerCustomDeck.createDeckInfo(x))).ToArray<DeckInfo>() : (DeckInfo[]) null;
      this.limitationParams = limits;
    }
  }
}
