// Decompiled with JetBrains decompiler
// Type: Quest00282FriendManager
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
using UnitStatusInformation;
using UnityEngine;

#nullable disable
public class Quest00282FriendManager : MonoBehaviour
{
  [SerializeField]
  protected UILabel txt_Listdescription01;
  [SerializeField]
  protected UILabel leaderSkillName;
  [SerializeField]
  protected UILabel leaderSkillDescription;
  [SerializeField]
  protected GameObject Friend;
  [SerializeField]
  protected GameObject Master;
  [SerializeField]
  protected GameObject Guild;
  [SerializeField]
  protected Transform linkFriend;
  [SerializeField]
  protected GameObject FriendList;
  [SerializeField]
  protected GameObject FriendNoneList;
  [SerializeField]
  private UI2DSprite Emblem;
  [SerializeField]
  private UIButton Decision;
  [SerializeField]
  private UIButton skillAllBtn;
  [SerializeField]
  private UIButton DecisionNone;
  private Helper friend;
  private PlayerStoryQuestS story_quest;
  private PlayerExtraQuestS extra_quest;
  private PlayerCharacterQuestS char_quest;
  private PlayerQuestSConverter harmonyQuest;
  private PlayerSeaQuestS sea_quest;
  private Quest00282Menu menu;
  private bool favorite;
  private Action<PlayerHelper> onSetHelper_;
  private bool backSceneSelected_ = true;

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
      if (this.harmonyQuest != null)
        return this.harmonyQuest.questS.gender_restriction;
      return this.sea_quest != null ? this.sea_quest.quest_sea_s.gender_restriction : UnitGender.none;
    }
  }

  private IEnumerator SkillAllBtnOnClick()
  {
    if (this.friend.leader_skill_id.HasValue && MasterData.BattleskillSkill.ContainsKey(this.friend.leader_skill_id.Value))
    {
      Future<GameObject> loader = PopupSkillDetails.createPrefabLoader(Singleton<NGGameDataManager>.GetInstance().IsSea);
      yield return (object) loader.Wait();
      PopupSkillDetails.show(loader.Result, new PopupSkillDetails.Param(MasterData.BattleskillSkill[this.friend.leader_skill_id.Value], UnitParameter.SkillGroup.Leader));
      loader = (Future<GameObject>) null;
    }
  }

  public IEnumerator InitFriendList(
    Quest00282Menu menu,
    Helper friend,
    PlayerStoryQuestS story_quest,
    PlayerExtraQuestS extra_quest,
    PlayerCharacterQuestS char_quest,
    PlayerQuestSConverter harmony_quest,
    PlayerSeaQuestS sea_quest,
    DateTime now,
    Action<PlayerHelper> eventSetHelper,
    bool enabledDetail,
    bool backSceneSelected)
  {
    Quest00282FriendManager quest00282FriendManager = this;
    quest00282FriendManager.menu = menu;
    quest00282FriendManager.SetEnableButton(false);
    quest00282FriendManager.FriendNoneList.SetActive(false);
    quest00282FriendManager.FriendList.SetActive(true);
    quest00282FriendManager.favorite = false;
    quest00282FriendManager.onSetHelper_ = eventSetHelper;
    quest00282FriendManager.backSceneSelected_ = backSceneSelected;
    if (friend.is_friend)
      quest00282FriendManager.favorite = ((IEnumerable<string>) Singleton<NGGameDataManager>.GetInstance().favoriteFriends).Contains<string>(friend.target_player_id);
    quest00282FriendManager.friend = friend;
    quest00282FriendManager.story_quest = story_quest;
    quest00282FriendManager.extra_quest = extra_quest;
    quest00282FriendManager.char_quest = char_quest;
    quest00282FriendManager.harmonyQuest = harmony_quest;
    quest00282FriendManager.sea_quest = sea_quest;
    quest00282FriendManager.leaderSkillDescription.SetText(friend.leader_skill_id.HasValue ? (MasterData.BattleskillSkill.ContainsKey(friend.leader_skill_id.Value) ? MasterData.BattleskillSkill[friend.leader_skill_id.Value].description : string.Empty) : string.Empty);
    quest00282FriendManager.txt_Listdescription01.SetText(friend.target_player_name);
    quest00282FriendManager.leaderSkillName.SetText(friend.leader_skill_id.HasValue ? (MasterData.BattleskillSkill.ContainsKey(friend.leader_skill_id.Value) ? MasterData.BattleskillSkill[friend.leader_skill_id.Value].name : string.Empty) : string.Empty);
    quest00282FriendManager.skillAllBtn.onClick.Clear();
    // ISSUE: reference to a compiler-generated method
    quest00282FriendManager.skillAllBtn.onClick.Add(new EventDelegate(new EventDelegate.Callback(quest00282FriendManager.\u003CInitFriendList\u003Eb__26_0)));
    if (friend.is_guild_member)
    {
      quest00282FriendManager.Friend.SetActive(false);
      quest00282FriendManager.Master.SetActive(false);
      quest00282FriendManager.Guild.SetActive(true);
    }
    else if (friend.is_friend)
    {
      quest00282FriendManager.Friend.SetActive(true);
      quest00282FriendManager.Master.SetActive(false);
      quest00282FriendManager.Guild.SetActive(false);
    }
    else
    {
      quest00282FriendManager.Friend.SetActive(false);
      quest00282FriendManager.Guild.SetActive(false);
      quest00282FriendManager.Master.SetActive(true);
    }
    if (quest00282FriendManager.gender_restriction != UnitGender.none)
      ((UIButtonColor) quest00282FriendManager.Decision).isEnabled = friend.leader_unit.unit.character.gender == quest00282FriendManager.gender_restriction;
    Future<Sprite> sprF = EmblemUtility.LoadEmblemSprite(quest00282FriendManager.friend.current_emblem_id);
    IEnumerator e = sprF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    quest00282FriendManager.Emblem.sprite2D = sprF.Result;
    e = quest00282FriendManager.ChangeFriendIcon(friend, enabledDetail);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator ChangeFriendIcon(Helper friend, bool enabledDetail)
  {
    Quest00282FriendManager quest00282FriendManager = this;
    Future<GameObject> PrefabF = Singleton<NGGameDataManager>.GetInstance().IsSea ? Res.Prefabs.Sea.UnitIcon.normal_sea.Load<GameObject>() : Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = PrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UnitIcon unitIconScript = PrefabF.Result.Clone(quest00282FriendManager.linkFriend).GetComponent<UnitIcon>();
    e = unitIconScript.setSimpleUnit(friend.leader_unit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitIconScript.Favorite = quest00282FriendManager.favorite;
    friend.leader_unit.level = friend.leader_unit_level;
    unitIconScript.setLevelText(friend.leader_unit);
    unitIconScript.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    if (Singleton<NGGameDataManager>.GetInstance().IsSea && (friend.is_friend || friend.is_guild_member))
      unitIconScript.SetSeaPiece(friend.leader_unit.unit.GetPiece);
    unitIconScript.Button.onLongPress.Clear();
    if (enabledDetail)
    {
      // ISSUE: reference to a compiler-generated method
      EventDelegate.Add(unitIconScript.Button.onClick, new EventDelegate.Callback(quest00282FriendManager.\u003CChangeFriendIcon\u003Eb__27_0));
      // ISSUE: reference to a compiler-generated method
      EventDelegate.Add(unitIconScript.Button.onLongPress, new EventDelegate.Callback(quest00282FriendManager.\u003CChangeFriendIcon\u003Eb__27_1));
    }
    else
      unitIconScript.Button.onClick.Clear();
  }

  public void FriendDetailScene()
  {
    Unit0042Scene.changeSceneFriendUnit(true, this.friend.target_player_id, this.friend.rental_player_unit_id);
  }

  public void FriendNone(
    Quest00282Menu menu,
    PlayerStoryQuestS story_quest,
    PlayerExtraQuestS extra_quest,
    PlayerCharacterQuestS char_quest,
    PlayerQuestSConverter harmony_quest,
    PlayerSeaQuestS sea_quest,
    Action<PlayerHelper> eventSetHelper,
    bool backSceneSelected)
  {
    this.menu = menu;
    this.story_quest = story_quest;
    this.extra_quest = extra_quest;
    this.char_quest = char_quest;
    this.harmonyQuest = harmony_quest;
    this.sea_quest = sea_quest;
    this.friend = (Helper) null;
    this.backSceneSelected_ = backSceneSelected;
    this.onSetHelper_ = eventSetHelper;
    this.FriendList.SetActive(false);
    this.FriendNoneList.SetActive(true);
    this.SetEnableButton(false);
  }

  public void FriendSelect()
  {
    this.menu.SetEnableOtherElementBtn(false);
    this.menu.SetEnableFriendBtn(false);
    if (this.onSetHelper_ != null)
    {
      this.onSetHelper_(this.friend != null ? this.friend.Clone() : (PlayerHelper) null);
      if (!this.backSceneSelected_)
        return;
      Singleton<NGSceneManager>.GetInstance().backScene();
    }
    else if (this.story_quest != null)
      Quest0028Scene.changeScene(true, this.story_quest);
    else if (this.extra_quest != null)
      Quest0028Scene.changeScene(true, this.extra_quest);
    else if (this.char_quest != null)
      Quest0028Scene.changeScene(true, this.char_quest);
    else if (this.harmonyQuest != null)
    {
      Quest0028Scene.changeScene(true, this.harmonyQuest);
    }
    else
    {
      if (this.sea_quest == null)
        return;
      Quest0028Scene.changeScene(true, this.sea_quest);
    }
  }

  public void SetEnableButton(bool isEnabled)
  {
    ((UIButtonColor) this.Decision).isEnabled = isEnabled;
    ((UIButtonColor) this.skillAllBtn).isEnabled = isEnabled;
    ((UIButtonColor) this.DecisionNone).isEnabled = isEnabled;
  }
}
