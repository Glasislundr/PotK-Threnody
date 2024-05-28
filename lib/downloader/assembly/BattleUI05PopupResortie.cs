// Decompiled with JetBrains decompiler
// Type: BattleUI05PopupResortie
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class BattleUI05PopupResortie : BackButtonMenuBase
{
  [SerializeField]
  private GameObject topFriend_;
  [SerializeField]
  [Tooltip("助っ人に他人を選択していた時の表示制御")]
  private FriendRequestPopupBase cntlOthers_;
  [SerializeField]
  [Tooltip("助っ人にフレンドを選択していた時の表示制御")]
  private Battle0202502Menu cntlFriend_;
  [SerializeField]
  [Tooltip("助っ人のタイトル部の先頭0:フレンド/1:他人")]
  private GameObject[] topFriendTitles_;
  [SerializeField]
  [Tooltip("助っ人用のオブジェクトの中で初期OFFにしておきたいリスト")]
  private GameObject[] friendFirstDisables_;
  [SerializeField]
  private UIButton btnFriendRequest_;
  [SerializeField]
  private GameObject topSortie_;
  [SerializeField]
  private GameObject topAP_;
  [SerializeField]
  private UILabel txtRealAP_;
  [SerializeField]
  private UILabel txtRequiredAP_;
  [SerializeField]
  private UIButton btnResortie_;
  [SerializeField]
  private UILabel txtPlayerLevel_;
  [SerializeField]
  private GameObject autoLapTimer_;
  [SerializeField]
  private UILabel txtAutoLapTime_;
  private Action onRequestedFriend_;
  private object quest_;
  private Action onNext_;
  public static bool keyQuest_ = false;
  public static float autoLapTime = 5f;
  public static bool autoLapEscape = false;
  public static bool isQuestAutoLap = false;

  private int consumedAP_
  {
    get
    {
      if (this.quest_ is PlayerStoryQuestS)
        return this.storyQuest_.consumed_ap;
      if (this.quest_ is PlayerExtraQuestS)
        return this.extraQuest_.consumed_ap;
      return this.quest_ is PlayerQuestSConverter ? this.charaQuest_.consumed_ap : this.seaQuest_.consumed_ap;
    }
  }

  private int? remainBattleCount_
  {
    get
    {
      if (this.quest_ is PlayerStoryQuestS)
        return this.storyQuest_.remain_battle_count;
      if (this.quest_ is PlayerExtraQuestS)
        return this.extraQuest_.remain_battle_count;
      return this.quest_ is PlayerQuestSConverter ? this.charaQuest_.remain_battle_count : this.seaQuest_.remain_battle_count;
    }
  }

  private CommonQuestType questType_
  {
    get
    {
      if (this.quest_ is PlayerStoryQuestS)
        return CommonQuestType.Story;
      if (this.quest_ is PlayerExtraQuestS)
        return CommonQuestType.Extra;
      if (!(this.quest_ is PlayerQuestSConverter))
        return CommonQuestType.Sea;
      return this.charaQuest_.questS.data_type == QuestSConverter.DataType.Character ? CommonQuestType.Character : CommonQuestType.Harmony;
    }
  }

  private PlayerStoryQuestS storyQuest_ => this.quest_ as PlayerStoryQuestS;

  private PlayerExtraQuestS extraQuest_ => this.quest_ as PlayerExtraQuestS;

  private PlayerQuestSConverter charaQuest_ => this.quest_ as PlayerQuestSConverter;

  private PlayerSeaQuestS seaQuest_ => this.quest_ as PlayerSeaQuestS;

  private bool isWait_
  {
    get => this.IsPush || this.cntlOthers_.IsPush || this.cntlFriend_.IsPush;
    set
    {
      this.IsPush = value;
      this.cntlOthers_.IsPush = value;
      this.cntlFriend_.IsPush = value;
    }
  }

  private bool isWaitAndSet()
  {
    if (this.isWait_)
      return true;
    this.isWait_ = true;
    return false;
  }

  public static IEnumerator show(
    object questS,
    PlayerHelper helper,
    int friendPoint,
    bool isFriend,
    Action eventNext)
  {
    switch (questS)
    {
      case null:
        if (eventNext == null)
        {
          yield break;
        }
        else
        {
          eventNext();
          yield break;
        }
      case PlayerCharacterQuestS _:
        questS = (object) new PlayerQuestSConverter(questS as PlayerCharacterQuestS);
        break;
      case PlayerHarmonyQuestS _:
        questS = (object) new PlayerQuestSConverter(questS as PlayerHarmonyQuestS);
        break;
    }
    Future<GameObject> ldPrefab = new ResourceObject("Prefabs/battle/popup_Re_sortie__anim_popup01").Load<GameObject>();
    IEnumerator e = ldPrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Equality((Object) ldPrefab.Result, (Object) null))
    {
      if (eventNext != null)
        eventNext();
    }
    else
    {
      bool toNext = false;
      bool requestedFriend = false;
      int consumedAP = 0;
      string curScene = Singleton<NGSceneManager>.GetInstance().sceneName;
      BattleUI05PopupResortie.isQuestAutoLap = Singleton<NGGameDataManager>.GetInstance().questAutoLap;
      do
      {
        while (curScene != Singleton<NGSceneManager>.GetInstance().sceneName)
          yield return (object) null;
        GameObject go = Singleton<PopupManager>.GetInstance().open(ldPrefab.Result, isNonSe: true, isNonOpenAnime: true);
        BattleUI05PopupResortie cntl = go.GetComponent<BattleUI05PopupResortie>();
        e = cntl.initialize(questS, helper, friendPoint, isFriend, requestedFriend, (Action) (() => requestedFriend = true), (Action) (() =>
        {
          toNext = true;
          if (eventNext == null)
            return;
          eventNext();
        }));
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        consumedAP = cntl.consumedAP_;
        Singleton<PopupManager>.GetInstance().startOpenAnime(go);
        while (Singleton<PopupManager>.GetInstance().isOpen)
        {
          if (BattleUI05PopupResortie.isQuestAutoLap)
          {
            BattleUI05PopupResortie.autoLapEscape = false;
            while (BattleUI05PopupResortie.AutoLapTimeCheck() && !BattleUI05PopupResortie.autoLapEscape)
              yield return (object) null;
            cntl.AutoLapResortie();
          }
          yield return (object) null;
        }
        if (toNext)
        {
          yield break;
        }
        else
        {
          IEnumerator initPopup = (IEnumerator) null;
          if (BattleUI05PopupResortie.checkResortie(consumedAP, (Action<IEnumerator>) (ie => initPopup = ie)))
          {
            if (BattleUI05PopupResortie.isQuestAutoLap && !cntl.QuestStart())
            {
              Singleton<NGGameDataManager>.GetInstance().questAutoLap = BattleUI05PopupResortie.isQuestAutoLap = false;
              break;
            }
            break;
          }
          Singleton<NGGameDataManager>.GetInstance().questAutoLap = false;
          BattleUI05PopupResortie.isQuestAutoLap = Singleton<NGGameDataManager>.GetInstance().questAutoLap;
          while (initPopup.MoveNext())
            yield return initPopup.Current;
          while (Singleton<PopupManager>.GetInstance().isOpen)
            yield return (object) null;
          while (Singleton<PopupManager>.GetInstance().isRunningCoroutine)
            yield return (object) null;
          while (Singleton<PopupManager>.GetInstance().isOpen)
            yield return (object) null;
          go = (GameObject) null;
          cntl = (BattleUI05PopupResortie) null;
        }
      }
      while (!BattleUI05PopupResortie.checkResortie(consumedAP));
      if (!BattleUI05PopupResortie.isQuestAutoLap)
      {
        switch (questS)
        {
          case PlayerStoryQuestS _:
            Quest0028Scene.changeScene(true, (PlayerStoryQuestS) questS);
            break;
          case PlayerExtraQuestS _:
            Quest0028Scene.changeScene(true, (PlayerExtraQuestS) questS);
            break;
          case PlayerQuestSConverter _:
            if (questS is QuestSConverter.DataType.Character)
            {
              Quest0028Scene.changeScene(true, (PlayerExtraQuestS) questS);
              break;
            }
            Quest0028Scene.changeScene(true, (PlayerQuestSConverter) questS);
            break;
          default:
            Quest0028Scene.changeScene(true, (PlayerSeaQuestS) questS);
            break;
        }
      }
    }
  }

  private IEnumerator initialize(
    object questS,
    PlayerHelper helper,
    int friendPoint,
    bool isFriend,
    bool isReqestedFriend,
    Action eventRequestedFriend,
    Action eventNext)
  {
    BattleUI05PopupResortie ui05PopupResortie = this;
    ui05PopupResortie.quest_ = questS;
    ui05PopupResortie.onRequestedFriend_ = eventRequestedFriend;
    ui05PopupResortie.onNext_ = eventNext;
    BattleUI05PopupResortie.autoLapTime = 5f;
    if (helper != null)
    {
      ui05PopupResortie.topFriend_.SetActive(true);
      ((IEnumerable<GameObject>) ui05PopupResortie.topFriendTitles_).ToggleOnceEx(isFriend ? 0 : 1);
      int index = !helper.is_friend ? (!helper.is_guild_member ? 0 : 2) : 1;
      ((IEnumerable<GameObject>) ui05PopupResortie.friendFirstDisables_).ToggleOnceEx(index);
      ui05PopupResortie.txtPlayerLevel_.SetTextLocalize(helper.level);
      IEnumerator e;
      if (isFriend)
      {
        ((Component) ui05PopupResortie.btnFriendRequest_).gameObject.SetActive(false);
        ((Behaviour) ui05PopupResortie.cntlOthers_).enabled = false;
        e = ui05PopupResortie.cntlFriend_.Init(helper, friendPoint);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        ((Behaviour) ui05PopupResortie.cntlFriend_).enabled = false;
        ui05PopupResortie.cntlOthers_.SetCallback(new Action(ui05PopupResortie.onClickedFriendRequest));
        if (isReqestedFriend)
          ui05PopupResortie.setDisableFriendRequest();
        e = ui05PopupResortie.cntlOthers_.Init(helper, friendPoint);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    else
      ui05PopupResortie.topFriend_.SetActive(false);
    int? remainBattleCount = ui05PopupResortie.remainBattleCount_;
    bool flag = !remainBattleCount.HasValue || remainBattleCount.Value > 1;
    BattleUI05PopupResortie.keyQuest_ = false;
    if (flag)
    {
      switch (ui05PopupResortie.questType_)
      {
        case CommonQuestType.Story:
          PlayerStoryQuestS quest1 = ui05PopupResortie.storyQuest_;
          if (quest1.quest_story_s.story_only)
          {
            flag = false;
            break;
          }
          if (((IEnumerable<PlayerStoryQuestS>) SMManager.Get<PlayerStoryQuestS[]>()).FirstOrDefault<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x._quest_story_s == quest1._quest_story_s)) == null)
          {
            flag = false;
            break;
          }
          if (quest1.end_at.HasValue && ServerTime.NowAppTimeAddDelta() > quest1.end_at.Value)
          {
            flag = false;
            break;
          }
          break;
        case CommonQuestType.Character:
          PlayerQuestSConverter quest2 = ui05PopupResortie.charaQuest_;
          if (quest2.questS.story_only)
          {
            flag = false;
            break;
          }
          if (((IEnumerable<PlayerCharacterQuestS>) SMManager.Get<PlayerCharacterQuestS[]>()).FirstOrDefault<PlayerCharacterQuestS>((Func<PlayerCharacterQuestS, bool>) (x => x._quest_character_s == quest2._quest_s_id)) == null)
          {
            flag = false;
            break;
          }
          break;
        case CommonQuestType.Extra:
          PlayerExtraQuestS quest3 = ui05PopupResortie.extraQuest_;
          if (quest3.quest_extra_s.story_only)
          {
            flag = false;
            break;
          }
          if (((IEnumerable<PlayerExtraQuestS>) SMManager.Get<PlayerExtraQuestS[]>()).FirstOrDefault<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (x => x._quest_extra_s == quest3._quest_extra_s)) == null)
          {
            flag = false;
            break;
          }
          if (quest3.extra_quest_area == 2)
          {
            PlayerQuestGate playerQuestGate = quest3.GetPlayerQuestGate();
            BattleUI05PopupResortie.keyQuest_ = true;
            if (playerQuestGate != null)
            {
              DateTime dateTime = ServerTime.NowAppTimeAddDelta();
              DateTime? endAt = playerQuestGate.end_at;
              if ((endAt.HasValue ? (dateTime > endAt.GetValueOrDefault() ? 1 : 0) : 0) == 0 && playerQuestGate.in_progress)
                goto label_29;
            }
            flag = true;
            break;
          }
label_29:
          if (ServerTime.NowAppTimeAddDelta() > quest3.today_day_end_at)
          {
            flag = false;
            break;
          }
          break;
        case CommonQuestType.Harmony:
          PlayerQuestSConverter quest4 = ui05PopupResortie.charaQuest_;
          if (quest4.questS.story_only)
          {
            flag = false;
            break;
          }
          if (((IEnumerable<PlayerHarmonyQuestS>) SMManager.Get<PlayerHarmonyQuestS[]>()).FirstOrDefault<PlayerHarmonyQuestS>((Func<PlayerHarmonyQuestS, bool>) (x => x._quest_harmony_s == quest4._quest_s_id)) == null)
          {
            flag = false;
            break;
          }
          break;
        case CommonQuestType.Sea:
          PlayerSeaQuestS quest5 = ui05PopupResortie.seaQuest_;
          if (quest5.quest_sea_s.story_only)
          {
            flag = false;
            break;
          }
          if (((IEnumerable<PlayerSeaQuestS>) SMManager.Get<PlayerSeaQuestS[]>()).FirstOrDefault<PlayerSeaQuestS>((Func<PlayerSeaQuestS, bool>) (x => x._quest_sea_s == quest5._quest_sea_s)) == null)
          {
            flag = false;
            break;
          }
          if (quest5.end_at.HasValue && ServerTime.NowAppTimeAddDelta() > quest5.end_at.Value)
          {
            flag = false;
            break;
          }
          break;
      }
    }
    ui05PopupResortie.topAP_.SetActive(flag);
    Player player = SMManager.Get<Player>();
    int num = player.ap + player.ap_overflow;
    if (flag)
    {
      Hashtable args = new Hashtable()
      {
        {
          (object) "now",
          (object) num
        },
        {
          (object) "max",
          (object) player.ap_max
        }
      };
      Consts instance = Consts.GetInstance();
      ui05PopupResortie.txtRealAP_.SetTextLocalize(Consts.Format(num > player.ap_max ? instance.OVERFLOW_AP_NOW_MAX : instance.VALUE_AP_NOW_MAX, (IDictionary) args));
      ui05PopupResortie.txtRequiredAP_.SetTextLocalize(ui05PopupResortie.consumedAP_);
    }
    ((UIButtonColor) ui05PopupResortie.btnResortie_).isEnabled = flag;
    BattleUI05PopupResortie.isQuestAutoLap = Singleton<NGGameDataManager>.GetInstance().questAutoLap;
    if (BattleUI05PopupResortie.isQuestAutoLap)
    {
      ui05PopupResortie.autoLapTimer_.SetActive(true);
      ui05PopupResortie.txtAutoLapTime_.text = Mathf.FloorToInt(BattleUI05PopupResortie.autoLapTime).ToString();
    }
    else
      ui05PopupResortie.autoLapTimer_.SetActive(false);
  }

  private void onClickedFriendRequest()
  {
    this.setDisableFriendRequest();
    this.cntlOthers_.IsPush = false;
    if (this.onRequestedFriend_ == null)
      return;
    this.onRequestedFriend_();
  }

  private void setDisableFriendRequest()
  {
    ((Component) this.btnFriendRequest_).gameObject.SetActive(false);
    ((IEnumerable<GameObject>) this.topFriendTitles_).ToggleOnceEx(-1);
  }

  protected override void Update()
  {
    base.Update();
    if (!((Component) this.txtAutoLapTime_).gameObject.activeSelf)
      return;
    BattleUI05PopupResortie.autoLapTime -= Time.deltaTime;
    this.txtAutoLapTime_.text = Mathf.CeilToInt(BattleUI05PopupResortie.autoLapTime).ToString();
    if ((double) BattleUI05PopupResortie.autoLapTime >= 0.0)
      return;
    this.autoLapTimer_.SetActive(false);
  }

  public override void onBackButton() => this.showBackKeyToast();

  public void onClickedResortie()
  {
    Singleton<NGGameDataManager>.GetInstance().questAutoLap = false;
    BattleUI05PopupResortie.isQuestAutoLap = false;
    BattleUI05PopupResortie.autoLapEscape = true;
    if (BattleUI05PopupResortie.keyQuest_)
    {
      PlayerQuestGate playerQuestGate = this.extraQuest_.GetPlayerQuestGate();
      DateTime dateTime = ServerTime.NowAppTimeAddDelta();
      DateTime? endAt = playerQuestGate.end_at;
      if ((endAt.HasValue ? (dateTime > endAt.GetValueOrDefault() ? 1 : 0) : 0) != 0 || !playerQuestGate.in_progress)
      {
        Singleton<NGGameDataManager>.GetInstance().questAutoLap = false;
        BattleUI05PopupResortie.isQuestAutoLap = Singleton<NGGameDataManager>.GetInstance().questAutoLap;
        this.autoLapTimer_.SetActive(false);
        this.StartCoroutine(this.OpenQuestReleasePopup(new PlayerQuestGate[1]
        {
          playerQuestGate
        }));
      }
      else
      {
        if (this.isWaitAndSet())
          return;
        Singleton<PopupManager>.GetInstance().dismiss();
      }
    }
    else
    {
      if (this.isWaitAndSet())
        return;
      Singleton<PopupManager>.GetInstance().dismiss();
    }
  }

  private IEnumerator OpenQuestReleasePopup(PlayerQuestGate[] keyQuests)
  {
    BattleUI05PopupResortie battleResortie = this;
    Future<GameObject> popupF;
    switch (keyQuests.Length)
    {
      case 0:
        popupF = (Future<GameObject>) null;
        break;
      case 1:
        popupF = Res.Prefabs.Banners.KeyQuest.popup_prefab.temp_popup_1.Load<GameObject>();
        break;
      case 2:
        popupF = Res.Prefabs.Banners.KeyQuest.popup_prefab.temp_popup_2.Load<GameObject>();
        break;
      default:
        popupF = Res.Prefabs.Banners.KeyQuest.popup_prefab.temp_popup_over.Load<GameObject>();
        break;
    }
    if (popupF != null)
    {
      IEnumerator e = popupF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject popup = Singleton<PopupManager>.GetInstance().open(popupF.Result);
      popup.SetActive(false);
      e = popup.GetComponent<Quest002171QuestOpenPopup>().Init(keyQuests, battleResortie);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      yield return (object) null;
      popup.SetActive(true);
    }
  }

  public void AutoLapResortie()
  {
    if (!((UIButtonColor) this.btnResortie_).isEnabled || !BattleUI05PopupResortie.isQuestAutoLap)
    {
      Singleton<NGGameDataManager>.GetInstance().questAutoLap = false;
      BattleUI05PopupResortie.isQuestAutoLap = false;
    }
    else if (BattleUI05PopupResortie.keyQuest_)
    {
      PlayerQuestGate playerQuestGate = this.extraQuest_.GetPlayerQuestGate();
      DateTime dateTime = ServerTime.NowAppTimeAddDelta();
      DateTime? endAt = playerQuestGate.end_at;
      if ((endAt.HasValue ? (dateTime > endAt.GetValueOrDefault() ? 1 : 0) : 0) != 0 || !playerQuestGate.in_progress)
      {
        Singleton<NGGameDataManager>.GetInstance().questAutoLap = false;
        BattleUI05PopupResortie.isQuestAutoLap = Singleton<NGGameDataManager>.GetInstance().questAutoLap;
        this.StartCoroutine(this.OpenQuestReleasePopup(new PlayerQuestGate[1]
        {
          playerQuestGate
        }));
      }
      else
      {
        if (this.isWaitAndSet())
          return;
        Singleton<PopupManager>.GetInstance().dismiss();
      }
    }
    else
    {
      if (this.isWaitAndSet())
        return;
      Singleton<PopupManager>.GetInstance().dismiss();
    }
  }

  protected bool QuestStart()
  {
    DeckInfo selectDeck = this.getSelectDeck();
    if (selectDeck.isNeedsRepair)
      return false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 4;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Singleton<NGSoundManager>.GetInstance().playVoiceByID(selectDeck.player_units[0].unit.unitVoicePattern, 70, 0);
    switch (this.questType_)
    {
      case CommonQuestType.Story:
        this.StoryStartApi();
        break;
      case CommonQuestType.Character:
        this.CharacterStartApi();
        break;
      case CommonQuestType.Extra:
        this.ExtraStartApi();
        break;
      case CommonQuestType.Harmony:
        this.HarmonyStartApi();
        break;
      case CommonQuestType.Sea:
        this.SeaStartApi();
        break;
      default:
        Debug.LogError((object) "!!! BUG !!!");
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        MypageScene.ChangeSceneOnError();
        break;
    }
    return true;
  }

  public void onClickedNext()
  {
    if (this.isWaitAndSet())
      return;
    Singleton<NGGameDataManager>.GetInstance().questAutoLap = false;
    BattleUI05PopupResortie.isQuestAutoLap = false;
    BattleUI05PopupResortie.autoLapEscape = true;
    Singleton<PopupManager>.GetInstance().dismiss();
    if (this.onNext_ == null)
      return;
    this.onNext_();
  }

  private static bool checkResortie(
    int consumedAP,
    Action<IEnumerator> onStartPopup = null,
    Action onPopupAPCompleted = null)
  {
    Player player = SMManager.Get<Player>();
    if (player.CheckMaxHavingUnit())
    {
      if (onStartPopup != null)
        onStartPopup(BattleUI05PopupResortie.popupMaxUnit());
      return false;
    }
    if (player.CheckMaxHavingGear())
    {
      if (onStartPopup != null)
        onStartPopup(BattleUI05PopupResortie.popupMaxGear());
      return false;
    }
    if (player.CheckMaxHavingReisou())
    {
      if (onStartPopup != null)
        onStartPopup(BattleUI05PopupResortie.popupMaxReisou());
      return false;
    }
    if (player.ap >= consumedAP)
      return true;
    if (onStartPopup != null)
      onStartPopup(BattleUI05PopupResortie.popupShortageAP(onPopupAPCompleted));
    return false;
  }

  private static bool AutoLapTimeCheck() => (double) BattleUI05PopupResortie.autoLapTime > 0.0;

  private static IEnumerator popupMaxUnit()
  {
    IEnumerator e = PopupUtility._999_5_1();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) new WaitForSeconds(0.5f);
  }

  private static IEnumerator popupMaxGear()
  {
    IEnumerator e = PopupUtility._999_6_1(true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) new WaitForSeconds(0.5f);
  }

  private static IEnumerator popupMaxReisou()
  {
    IEnumerator e = PopupUtility.popupMaxReisou();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) new WaitForSeconds(0.5f);
  }

  private static IEnumerator popupShortageAP(Action onCompleted)
  {
    IEnumerator e = PopupUtility.RecoveryAP(true, onCompleted);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void StoryStartApi()
  {
    DeckInfo selectDeck = this.getSelectDeck();
    WebAPI.BattleStoryStart(selectDeck.deck_number, selectDeck.deck_type_id, 0, this.storyQuest_.quest_story_s.ID, "", 0, (Action<WebAPI.Response.UserError>) (error =>
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
    Persist.lastsortie.Data.SaveLastSortie(this.storyQuest_.quest_story_s.ID, this.storyQuest_.quest_story_s.quest_m_QuestStoryM, this.storyQuest_.quest_story_s.quest_l_QuestStoryL);
    Persist.lastsortie.Flush();
    if (MasterData.QuestStoryS[this.storyQuest_.quest_story_s.ID].quest_xl_QuestStoryXL == 6)
      Persist.integralNoahProcess.Data.lastIntegralNoahSId = this.storyQuest_.quest_story_s.ID;
    if (MasterData.QuestStoryS[this.storyQuest_.quest_story_s.ID].quest_xl_QuestStoryXL == 7)
      Persist.everAfterProcess.Data.lastEverAfterSId = this.storyQuest_.quest_story_s.ID;
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
    DeckInfo selectDeck = this.getSelectDeck();
    if (this.extraQuest_.quest_extra_s.wave != null)
      WebAPI.BattleWaveStart(selectDeck.deck_number, selectDeck.deck_type_id, 0, this.extraQuest_.quest_extra_s.ID, "", 0, (Action<WebAPI.Response.UserError>) (error =>
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
      WebAPI.BattleExtraStart(selectDeck.deck_number, selectDeck.deck_type_id, 0, this.extraQuest_.quest_extra_s.ID, "", 0, (Action<WebAPI.Response.UserError>) (error =>
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
    DeckInfo selectDeck = this.getSelectDeck();
    WebAPI.BattleCharacterStart(selectDeck.deck_number, selectDeck.deck_type_id, 0, this.charaQuest_._quest_s_id, "", 0, (Action<WebAPI.Response.UserError>) (error =>
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

  private void HarmonyStartApi()
  {
    DeckInfo selectDeck = this.getSelectDeck();
    WebAPI.BattleHarmonyStart(selectDeck.deck_number, selectDeck.deck_type_id, 0, this.charaQuest_._quest_s_id, "", 0, (Action<WebAPI.Response.UserError>) (error =>
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
    DeckInfo selectDeck = this.getSelectDeck();
    WebAPI.SeaBattleStart(selectDeck.deck_number, selectDeck.deck_type_id, 0, this.seaQuest_.quest_sea_s.ID, "", 0, (Action<WebAPI.Response.UserError>) (error =>
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
    Persist.lastsortie.Data.SaveLastSortie(this.seaQuest_.quest_sea_s.ID, this.seaQuest_.quest_sea_s.quest_m_QuestSeaM, this.seaQuest_.quest_sea_s.quest_l_QuestSeaL);
    Persist.lastsortie.Flush();
  }

  private DeckInfo getSelectDeck()
  {
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      return PlayerSeaDeck.createDeckInfo(SMManager.Get<PlayerSeaDeck[]>()[Persist.seaDeckOrganized.Data.number]);
    return Persist.deckOrganized.Data.isCustom ? PlayerCustomDeck.createDeckInfo(SMManager.Get<PlayerCustomDeck[]>()[Persist.deckOrganized.Data.customNumber]) : PlayerDeck.createDeckInfo(SMManager.Get<PlayerDeck[]>()[Persist.deckOrganized.Data.number]);
  }

  private void StartBattle(BattleInfo info)
  {
    NGBattleManager instance = Singleton<NGBattleManager>.GetInstance();
    instance.deleteSavedEnvironment();
    instance.startBattle(info);
  }

  private enum FriendState
  {
    Friend,
    Other,
  }
}
