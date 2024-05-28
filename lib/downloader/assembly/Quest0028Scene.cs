// Decompiled with JetBrains decompiler
// Type: Quest0028Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class Quest0028Scene : NGSceneBase
{
  private bool story_only;
  public Quest0028Menu menu;
  [SerializeField]
  private GameObject bg;
  private List<PlayerItem> SupplyList = new List<PlayerItem>();
  private PlayerHelper friend_;

  public static void changeScene(bool stack, PlayerStoryQuestS story_quest, bool story_only = false)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_8", (stack ? 1 : 0) != 0, (object) story_quest, (object) story_only);
  }

  public static void changeScene(bool stack, PlayerExtraQuestS extra_quest, bool story_only = false)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_8", (stack ? 1 : 0) != 0, (object) extra_quest, (object) story_only);
  }

  public static void changeScene(bool stack, PlayerCharacterQuestS char_quest, bool story_only = false)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_8", (stack ? 1 : 0) != 0, (object) char_quest, (object) story_only);
  }

  public static void changeScene(bool stack, PlayerQuestSConverter char_quest, bool story_only = false)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_8", (stack ? 1 : 0) != 0, (object) char_quest, (object) story_only);
  }

  public static void changeScene(bool stack, PlayerSeaQuestS sea_quest, bool story_only = false)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_8_sea", (stack ? 1 : 0) != 0, (object) sea_quest, (object) story_only);
  }

  public IEnumerator onStartSceneAsync()
  {
    PlayerHelper playerHelper = SMManager.Get<PlayerHelper[]>()[1];
    playerHelper.leader_unit._unit = 500211;
    this.friend_ = playerHelper;
    IEnumerator e = this.onStartSceneAsync(SMManager.Get<PlayerStoryQuestS[]>()[0], (PlayerExtraQuestS) null, (PlayerCharacterQuestS) null, (PlayerQuestSConverter) null, (PlayerSeaQuestS) null, false, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(PlayerStoryQuestS story_quest, bool story_only = false)
  {
    IEnumerator e = this.onStartSceneAsync(story_quest, (PlayerExtraQuestS) null, (PlayerCharacterQuestS) null, (PlayerQuestSConverter) null, (PlayerSeaQuestS) null, story_only, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(PlayerExtraQuestS extra_quest, bool story_only = false)
  {
    IEnumerator e = this.onStartSceneAsync((PlayerStoryQuestS) null, extra_quest, (PlayerCharacterQuestS) null, (PlayerQuestSConverter) null, (PlayerSeaQuestS) null, story_only, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(PlayerCharacterQuestS char_quest, bool story_only = false)
  {
    IEnumerator e = this.onStartSceneAsync((PlayerStoryQuestS) null, (PlayerExtraQuestS) null, char_quest, (PlayerQuestSConverter) null, (PlayerSeaQuestS) null, story_only, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(PlayerQuestSConverter quest, bool story_only = false)
  {
    IEnumerator e = this.onStartSceneAsync((PlayerStoryQuestS) null, (PlayerExtraQuestS) null, (PlayerCharacterQuestS) null, quest, (PlayerSeaQuestS) null, story_only, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(PlayerSeaQuestS sea_quest, bool story_only = false)
  {
    IEnumerator e = this.onStartSceneAsync((PlayerStoryQuestS) null, (PlayerExtraQuestS) null, (PlayerCharacterQuestS) null, (PlayerQuestSConverter) null, sea_quest, story_only, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override void onSceneInitialized()
  {
    if (!this.story_only && Singleton<CommonRoot>.GetInstance().isLoading)
      Singleton<CommonRoot>.GetInstance().isLoading = false;
    base.onSceneInitialized();
  }

  public IEnumerator onBackSceneAsync(PlayerStoryQuestS story_quest, bool story_only = false)
  {
    IEnumerator e;
    if (!this.menu.IsPlayingStory)
    {
      e = this.onStartSceneAsync(story_quest, (PlayerExtraQuestS) null, (PlayerCharacterQuestS) null, (PlayerQuestSConverter) null, (PlayerSeaQuestS) null, story_only);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = this.onBackSceneAsync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public IEnumerator onBackSceneAsync(PlayerExtraQuestS extra_quest, bool story_only = false)
  {
    IEnumerator e;
    if (!this.menu.IsPlayingStory)
    {
      e = this.onStartSceneAsync((PlayerStoryQuestS) null, extra_quest, (PlayerCharacterQuestS) null, (PlayerQuestSConverter) null, (PlayerSeaQuestS) null, story_only);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = this.onBackSceneAsync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public IEnumerator onBackSceneAsync(PlayerCharacterQuestS char_quest, bool story_only = false)
  {
    IEnumerator e;
    if (!this.menu.IsPlayingStory)
    {
      e = this.onStartSceneAsync((PlayerStoryQuestS) null, (PlayerExtraQuestS) null, char_quest, (PlayerQuestSConverter) null, (PlayerSeaQuestS) null, story_only);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = this.onBackSceneAsync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public IEnumerator onBackSceneAsync(PlayerQuestSConverter quest, bool story_only = false)
  {
    IEnumerator e;
    if (!this.menu.IsPlayingStory)
    {
      e = this.onStartSceneAsync((PlayerStoryQuestS) null, (PlayerExtraQuestS) null, (PlayerCharacterQuestS) null, quest, (PlayerSeaQuestS) null, story_only);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = this.onBackSceneAsync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public IEnumerator onBackSceneAsync(PlayerSeaQuestS sea_quest, bool story_only = false)
  {
    IEnumerator e;
    if (!this.menu.IsPlayingStory)
    {
      e = this.onStartSceneAsync((PlayerStoryQuestS) null, (PlayerExtraQuestS) null, (PlayerCharacterQuestS) null, (PlayerQuestSConverter) null, sea_quest, story_only);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = this.onBackSceneAsync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public IEnumerator onBackSceneAsync()
  {
    if (this.menu.IsPlayingStory)
    {
      if (this.menu.CharacterQuestAfterBattleInfo != null && this.menu.CharacterQuestAfterBattleScriptId != 0)
      {
        Story0093Scene.changeScene(true, this.menu.CharacterQuestAfterBattleScriptId, new bool?(Singleton<NGGameDataManager>.GetInstance().IsSea && this.menu.CharacterQuestAfterBattleInfo.seaQuest != null));
        this.menu.CharacterQuestAfterBattleInfo = (BattleInfo) null;
        this.menu.CharacterQuestAfterBattleScriptId = 0;
      }
      else
      {
        Singleton<CommonRoot>.GetInstance().isLoading = true;
        Future<BattleEnd> f = WebAPI.BattleFinish(new WebAPI.Request.BattleFinish()
        {
          quest_type = this.menu.battleInfo.quest_type,
          win = true,
          is_game_over = false,
          battle_uuid = this.menu.battleInfo.battleId,
          player_money = 0,
          battle_turn = 0,
          continue_count = 0,
          week_element_attack_count = 0,
          week_kind_attack_count = 0
        }, (BE) null, (Action<WebAPI.Response.UserError>) (e =>
        {
          NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
          instance.clearStack();
          instance.destroyCurrentScene();
          instance.changeScene(Singleton<CommonRoot>.GetInstance().startScene, false);
        }));
        IEnumerator e1 = f.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        if (f.Result != null)
        {
          Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
          BattleUI05Scene.ChangeScene(this.menu.battleInfo, true, f.Result);
          this.menu.IsPlayingStory = false;
          this.menu.mainPanel.SetActive(true);
        }
      }
    }
  }

  public void setFriend(PlayerHelper friend) => this.friend_ = friend;

  public IEnumerator onStartSceneAsync(
    PlayerStoryQuestS story_quest,
    PlayerExtraQuestS extra_quest,
    PlayerCharacterQuestS char_quest,
    PlayerQuestSConverter quest,
    PlayerSeaQuestS sea_quest,
    bool story_only,
    bool isFirstInit = false)
  {
    Quest0028Scene quest0028Scene = this;
    quest0028Scene.story_only = story_only;
    NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
    instance.QuestType = story_quest == null ? (extra_quest == null ? (char_quest == null ? (quest == null ? (sea_quest == null ? new CommonQuestType?() : new CommonQuestType?(CommonQuestType.Sea)) : (quest.questS.data_type != QuestSConverter.DataType.Character ? new CommonQuestType?(CommonQuestType.Harmony) : new CommonQuestType?(CommonQuestType.Character))) : new CommonQuestType?(CommonQuestType.Character)) : new CommonQuestType?(CommonQuestType.Extra)) : new CommonQuestType?(CommonQuestType.Story);
    instance.IsConnectedResultQuestProgressExtra = false;
    Quest0028Menu.isGoingToBattle = false;
    if (Singleton<NGGameDataManager>.GetInstance().IsSea && sea_quest == null)
      quest0028Scene.headerType = CommonRoot.HeaderType.Normal;
    else if (!Singleton<NGGameDataManager>.GetInstance().IsSea && sea_quest != null)
      quest0028Scene.headerType = CommonRoot.HeaderType.Sea;
    PlayerItem[] source = SMManager.Get<PlayerItem[]>().AllBattleSupplies();
    quest0028Scene.SupplyList = ((IEnumerable<PlayerItem>) source).ToList<PlayerItem>();
    Func<DeckInfo[]> getNormalDecks = !Singleton<NGGameDataManager>.GetInstance().IsSea || sea_quest == null ? (Func<DeckInfo[]>) (() => ((IEnumerable<PlayerDeck>) SMManager.Get<PlayerDeck[]>()).Select<PlayerDeck, DeckInfo>((Func<PlayerDeck, DeckInfo>) (x => PlayerDeck.createDeckInfo(x))).ToArray<DeckInfo>()) : (Func<DeckInfo[]>) (() => ((IEnumerable<PlayerSeaDeck>) SMManager.Get<PlayerSeaDeck[]>()).Select<PlayerSeaDeck, DeckInfo>((Func<PlayerSeaDeck, DeckInfo>) (x => PlayerSeaDeck.createDeckInfo(x))).ToArray<DeckInfo>());
    string path = "";
    if (story_quest != null)
      path = quest0028Scene.setStoryPath(story_quest.quest_story_s);
    else if (extra_quest != null)
    {
      path = quest0028Scene.setExtraPath(extra_quest.quest_extra_s);
      Debug.LogWarning((object) path);
    }
    else if (char_quest != null)
      path = quest0028Scene.setCharaPath(char_quest.quest_character_s);
    else if (quest != null)
      path = quest0028Scene.setQuestConverterPath(quest.questS);
    else if (sea_quest != null)
      path = quest0028Scene.setSeaPath(sea_quest.quest_sea_s);
    else
      Debug.LogError((object) "!BUG! QUEST NOT FOUND");
    Future<GameObject> bgF = Res.Prefabs.BackGround.SortieBackGround.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    quest0028Scene.bg = bgF.Result;
    Future<Sprite> bgSpriteF = Singleton<ResourceManager>.GetInstance().LoadOrNull<Sprite>(path);
    e = bgSpriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) bgSpriteF.Result, (Object) null))
    {
      quest0028Scene.bg.GetComponent<UI2DSprite>().sprite2D = bgSpriteF.Result;
      UI2DSprite component = quest0028Scene.bg.GetComponent<UI2DSprite>();
      component.sprite2D = bgSpriteF.Result;
      Rect textureRect = bgSpriteF.Result.textureRect;
      ((UIWidget) component).width = Mathf.FloorToInt(((Rect) ref textureRect).width);
      textureRect = bgSpriteF.Result.textureRect;
      ((UIWidget) component).height = Mathf.FloorToInt(((Rect) ref textureRect).height);
      quest0028Scene.backgroundPrefab = quest0028Scene.bg;
    }
    quest0028Scene.menu.setEventSetHelper(new Action<PlayerHelper>(quest0028Scene.setFriend));
    e = quest0028Scene.menu.InitPlayerDecks(getNormalDecks, quest0028Scene.SupplyList, quest0028Scene.friend_, story_quest, extra_quest, char_quest, quest, sea_quest, story_only);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (isFirstInit)
      quest0028Scene.menu.FillItem();
    quest0028Scene.isActiveFooter = !Singleton<NGGameDataManager>.GetInstance().IsSea;
  }

  private string setStoryPath(QuestStoryS quest) => quest.GetBackgroundPath();

  private string setExtraPath(QuestExtraS quest) => quest.GetBackgroundPath();

  private string setCharaPath(QuestCharacterS quest) => quest.GetBackgroundPath();

  private string setQuestConverterPath(QuestSConverter quest)
  {
    string backgroundImageName = quest.quest_m.background_image_name;
    return backgroundImageName == null ? Consts.GetInstance().DEFULAT_BACKGROUND : string.Format(Consts.GetInstance().BACKGROUND_BASE_PATH, (object) backgroundImageName);
  }

  private string setSeaPath(QuestSeaS quest) => quest.GetBackgroundPath();

  public override void onEndScene()
  {
    this.menu.EndScene();
    Singleton<CommonRoot>.GetInstance().releaseBackground();
  }

  public override IEnumerator onDestroySceneAsync()
  {
    if (!Quest0028Menu.isGoingToBattle)
    {
      Singleton<NGGameDataManager>.GetInstance().IsFromPopupStageList = false;
      Singleton<NGSceneManager>.GetInstance().ClearSavedChangeSceneParam();
      Singleton<NGGameDataManager>.GetInstance().QuestType = new CommonQuestType?();
      yield break;
    }
  }
}
