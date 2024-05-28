// Decompiled with JetBrains decompiler
// Type: Versus0268Scene
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
public class Versus0268Scene : NGSceneBase
{
  [SerializeField]
  private Versus0268Menu menu;
  [SerializeField]
  private GameObject touchToNext;
  [SerializeField]
  private UIButton scrollPanelButton;
  private List<ResultMenuBase> sequences;
  private bool toNextSequence;
  private bool isStarted;
  private WebAPI.Response.PvpPlayerFinish pvpInfo;
  private ResultMenuBase nowPlayBase;

  public static void ChangeScene()
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("versus026_8", false);
  }

  public static void ChangeScene(bool isDebug)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("versus026_8", false, (object) isDebug);
  }

  public static void ChangeScene(WebAPI.Response.PvpPlayerNpcFinish resNpcFinish)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("versus026_8", false, (object) resNpcFinish);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Versus0268Scene versus0268Scene = this;
    Future<GameObject> bgF = Res.Prefabs.BackGround.MultiBackground.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    versus0268Scene.backgroundPrefab = bgF.Result;
  }

  public IEnumerator onStartSceneAsync()
  {
    Future<WebAPI.Response.PvpPlayerFinish> futureF = WebAPI.PvpPlayerFinish((Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      WebAPI.DefaultUserErrorCallback(e);
    }));
    IEnumerator e1 = futureF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    this.pvpInfo = futureF.Result;
    e1 = this.onStartSceneAsync(this.pvpInfo);
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(bool isDebug)
  {
    IEnumerator e;
    if (!isDebug)
    {
      e = this.onStartSceneAsync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      this.pvpInfo = this.ForDebugData(new WebAPI.Response.PvpPlayerFinish());
      e = this.onStartSceneAsync(this.pvpInfo);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public IEnumerator onStartSceneAsync(WebAPI.Response.PvpPlayerNpcFinish resNpcFinish)
  {
    this.pvpInfo = resNpcFinish.ConvertToPlayerFinish();
    IEnumerator e = this.onStartSceneAsync(this.pvpInfo);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(WebAPI.Response.PvpPlayerFinish result)
  {
    Versus0268Scene versus0268Scene = this;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    versus0268Scene.sequences = new List<ResultMenuBase>()
    {
      (ResultMenuBase) ((Component) versus0268Scene).GetComponent<Versus0268Menu>(),
      (ResultMenuBase) null
    };
    if (!result.target_player_is_friend)
      versus0268Scene.sequences.Add((ResultMenuBase) ((Component) versus0268Scene).GetComponent<FriendMenu>());
    Persist.pvpSuspend.Delete();
    IEnumerator e = versus0268Scene.InitMenus(result);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
  }

  public void onStartScene()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    if (this.isStarted)
      return;
    this.isStarted = true;
    this.StartCoroutine(this.RunMenus());
  }

  public void onStartScene(bool isDebug)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    if (this.isStarted)
      return;
    this.isStarted = true;
    this.StartCoroutine(this.RunMenus());
  }

  public void onStartScene(WebAPI.Response.PvpPlayerNpcFinish resNpcFinish)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    if (this.isStarted)
      return;
    this.isStarted = true;
    this.StartCoroutine(this.RunMenus());
  }

  private IEnumerator InitMenus(WebAPI.Response.PvpPlayerFinish result)
  {
    this.touchToNext.SetActive(false);
    ((UIButtonColor) this.scrollPanelButton).isEnabled = false;
    foreach (ResultMenuBase sequence in this.sequences)
    {
      if (Object.op_Inequality((Object) sequence, (Object) null))
      {
        IEnumerator e = sequence.Init(result);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  private IEnumerator RunMenus()
  {
    Versus0268Scene versus0268Scene = this;
    List<ResultMenuBase>.Enumerator seqe = versus0268Scene.sequences.GetEnumerator();
    IEnumerator e;
    while (seqe.MoveNext())
    {
      versus0268Scene.nowPlayBase = seqe.Current;
      if (!Object.op_Equality((Object) versus0268Scene.nowPlayBase, (Object) null))
      {
        versus0268Scene.touchToNext.SetActive(true);
        e = versus0268Scene.nowPlayBase.Run();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        versus0268Scene.toNextSequence = false;
        versus0268Scene.touchToNext.SetActive(true);
        ((UIButtonColor) versus0268Scene.scrollPanelButton).isEnabled = true;
        while (!versus0268Scene.toNextSequence)
          yield return (object) null;
        versus0268Scene.toNextSequence = false;
        versus0268Scene.touchToNext.SetActive(false);
        ((UIButtonColor) versus0268Scene.scrollPanelButton).isEnabled = false;
        e = versus0268Scene.nowPlayBase.OnFinish();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
        break;
    }
    versus0268Scene.nowPlayBase = (ResultMenuBase) null;
    versus0268Scene.touchToNext.SetActive(true);
    ((Collider) versus0268Scene.touchToNext.GetComponent<BoxCollider>()).enabled = false;
    while (seqe.MoveNext())
    {
      e = seqe.Current.Run();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    if (versus0268Scene.pvpInfo.is_tutorial && Persist.pvpInfo.Data.currentPage == 2)
      versus0268Scene.StartTutorial(new Action(versus0268Scene.FinishScene));
    else
      versus0268Scene.FinishScene();
  }

  private void StartTutorial(Action finishScene)
  {
    Singleton<TutorialRoot>.GetInstance().ForceShowAdvice("pvp3", (Action) (() =>
    {
      Persist.pvpInfo.Data.currentPage = 3;
      Persist.pvpInfo.Flush();
      finishScene();
    }));
  }

  private void FinishScene()
  {
    if (this.pvpInfo.pvp_maintenance)
      this.StartCoroutine(PopupCommon.Show(this.pvpInfo.pvp_maintenance_title, this.pvpInfo.pvp_maintenance_message, (Action) (() => MypageScene.ChangeScene())));
    else if (this.pvpInfo.matching_type == 6)
    {
      Singleton<NGSceneManager>.GetInstance().clearStack();
      Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
      if (this.pvpInfo.rank_aggregate)
        Versus0261Scene.ChangeScene0261(false);
      else
        Versus02610Scene.ChangeScene(false, true);
    }
    else
    {
      Singleton<NGSceneManager>.GetInstance().clearStack();
      Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
      Versus0262Scene.ChangeScene0262(false, Persist.pvpInfo.Data.lastMatchingType, true);
    }
  }

  private IEnumerator RunPopupMaintenance(WebAPI.Response.PvpPlayerFinish pvpInfo)
  {
    IEnumerator e = PopupCommon.Show(pvpInfo.pvp_maintenance_title, pvpInfo.pvp_maintenance_message, (Action) (() =>
    {
      NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
      instance.clearStack();
      instance.destroyCurrentScene();
      instance.changeScene(Singleton<CommonRoot>.GetInstance().startScene, false);
    }));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void IbtnTouchToNext()
  {
    this.toNextSequence = true;
    if (!Object.op_Inequality((Object) this.nowPlayBase, (Object) null))
      return;
    this.nowPlayBase.isSkip = true;
  }

  private WebAPI.Response.PvpPlayerFinish ForDebugData(WebAPI.Response.PvpPlayerFinish data)
  {
    bool flag1 = false;
    bool flag2 = false;
    bool UnitProficienciesEffectOn = false;
    bool GearBrokenEffectOn = true;
    bool flag3 = true;
    bool flag4 = true;
    bool flag5 = true;
    bool flag6 = true;
    bool flag7 = false;
    bool flag8 = false;
    Player player1 = SMManager.Get<Player>();
    PlayerUnit[] array1 = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Skip<PlayerUnit>(0).Take<PlayerUnit>(5).ToArray<PlayerUnit>();
    PlayerItem[] playerGears = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.entity_type == MasterDataTable.CommonRewardType.gear)).ToArray<PlayerItem>();
    ((IEnumerable<PlayerUnit>) array1).ForEach<PlayerUnit>((Action<PlayerUnit>) (p =>
    {
      PlayerItem[] array2 = ((IEnumerable<PlayerItem>) playerGears).Where<PlayerItem>((Func<PlayerItem, bool>) (x => MasterData.GearGear[x.entity_id].kind_GearKind == p.initial_gear.kind_GearKind)).ToArray<PlayerItem>();
      try
      {
        PlayerItem playerItem = array2[(int) ((double) Random.value * (double) (array2.Length - 1))];
        p.equip_gear_ids = new int?[1]
        {
          new int?(playerItem.id)
        };
      }
      catch (IndexOutOfRangeException ex)
      {
      }
    }));
    playerGears = ((IEnumerable<PlayerUnit>) array1).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.equippedGear != (PlayerItem) null)).Select<PlayerUnit, PlayerItem>((Func<PlayerUnit, PlayerItem>) (x => ((IEnumerable<PlayerItem>) playerGears).First<PlayerItem>((Func<PlayerItem, bool>) (y => y.id == x.equippedGear.id)))).Distinct<PlayerItem>().ToArray<PlayerItem>();
    Player player2 = player1.Clone();
    PlayerUnit[] array3 = ((IEnumerable<PlayerUnit>) array1).Select<PlayerUnit, PlayerUnit>((Func<PlayerUnit, PlayerUnit>) (x => x.Clone())).ToArray<PlayerUnit>();
    PlayerItem[] array4 = ((IEnumerable<PlayerItem>) playerGears).ToArray<PlayerItem>();
    array1[0].skills = new PlayerUnitSkills[0];
    player2.total_exp += 2000;
    ++player2.level;
    foreach (PlayerUnit playerUnit in array3)
    {
      if (flag1)
      {
        playerUnit.level += 3;
        playerUnit.hp = new PlayerUnitHp()
        {
          level = playerUnit.hp.level + 13,
          compose = playerUnit.hp.compose,
          inheritance = playerUnit.hp.inheritance,
          initial = playerUnit.hp.initial
        };
        playerUnit.strength = new PlayerUnitStrength()
        {
          level = playerUnit.strength.level + 3,
          compose = playerUnit.strength.compose,
          inheritance = playerUnit.strength.inheritance,
          initial = playerUnit.strength.initial
        };
        playerUnit.agility = new PlayerUnitAgility()
        {
          level = playerUnit.agility.level + 3,
          compose = playerUnit.agility.compose,
          inheritance = playerUnit.agility.inheritance,
          initial = playerUnit.agility.initial
        };
        playerUnit.dexterity = new PlayerUnitDexterity()
        {
          level = playerUnit.dexterity.level + 3,
          compose = playerUnit.dexterity.compose,
          inheritance = playerUnit.dexterity.inheritance,
          initial = playerUnit.dexterity.initial
        };
        playerUnit.mind = new PlayerUnitMind()
        {
          level = playerUnit.mind.level + 3,
          compose = playerUnit.mind.compose,
          inheritance = playerUnit.mind.inheritance,
          initial = playerUnit.mind.initial
        };
        playerUnit.lucky = new PlayerUnitLucky()
        {
          level = playerUnit.lucky.level + 3,
          compose = playerUnit.lucky.compose,
          inheritance = playerUnit.lucky.inheritance,
          initial = playerUnit.lucky.initial
        };
      }
    }
    ((IEnumerable<PlayerUnit>) array3).Skip<PlayerUnit>(2).ForEach<PlayerUnit>((Action<PlayerUnit>) (p =>
    {
      PlayerUnit playerUnit = p;
      PlayerUnitGearProficiency[] unitGearProficiencyArray;
      if (!UnitProficienciesEffectOn)
        unitGearProficiencyArray = new PlayerUnitGearProficiency[1]
        {
          new PlayerUnitGearProficiency()
          {
            gear_kind_id = p.gear_proficiencies[0].gear_kind_id,
            level = p.gear_proficiencies[0].level
          }
        };
      else
        unitGearProficiencyArray = new PlayerUnitGearProficiency[1]
        {
          new PlayerUnitGearProficiency()
          {
            gear_kind_id = p.gear_proficiencies[0].gear_kind_id,
            level = p.gear_proficiencies[0].level + 1
          }
        };
      playerUnit.gear_proficiencies = unitGearProficiencyArray;
    }));
    foreach (PlayerItem playerItem in array4)
    {
      if (flag2)
        playerItem.gear_level += 3;
    }
    ((IEnumerable<PlayerItem>) array4).ForEach<PlayerItem>((Action<PlayerItem>) (x => x.broken = GearBrokenEffectOn));
    PvPEndPlayer_character_intimates_in_battle[] intimatesInBattleArray1;
    if (!flag8)
      intimatesInBattleArray1 = new PvPEndPlayer_character_intimates_in_battle[0];
    else
      intimatesInBattleArray1 = new PvPEndPlayer_character_intimates_in_battle[2]
      {
        new PvPEndPlayer_character_intimates_in_battle()
        {
          character_id = array1[0].unit.character.ID,
          target_character_id = array1[1].unit.character.ID,
          before_level = 1,
          after_level = 2
        },
        new PvPEndPlayer_character_intimates_in_battle()
        {
          character_id = array1[2].unit.character.ID,
          target_character_id = array1[3].unit.character.ID,
          before_level = 3,
          after_level = 4
        }
      };
    PvPEndPlayer_character_intimates_in_battle[] intimatesInBattleArray2 = intimatesInBattleArray1;
    data.gladiators = new PlayerHelper[1]
    {
      new PlayerHelper()
      {
        target_player_id = "6f8fc498-3b1a-41c8-a4db-8291faee22d5",
        target_player_name = "知り合いじゃないですか？",
        leader_unit = array1[0],
        is_friend = false
      }
    };
    UnlockQuest[] array5 = ((IEnumerable<QuestCharacterS>) MasterData.QuestCharacterSList).Take<QuestCharacterS>(2).Select<QuestCharacterS, UnlockQuest>((Func<QuestCharacterS, UnlockQuest>) (x => new UnlockQuest()
    {
      quest_s_id = x.ID,
      quest_type = 2
    })).ToArray<UnlockQuest>();
    UnlockQuest[] unlockQuestArray = flag3 ? array5 : new UnlockQuest[0];
    WebAPI.Response.PvpPlayerFinish pvpPlayerFinish1 = data;
    WebAPI.Response.PvpPlayerFinishBonus_rewards[] finishBonusRewardsArray;
    if (!flag4)
      finishBonusRewardsArray = new WebAPI.Response.PvpPlayerFinishBonus_rewards[0];
    else
      finishBonusRewardsArray = new WebAPI.Response.PvpPlayerFinishBonus_rewards[1]
      {
        new WebAPI.Response.PvpPlayerFinishBonus_rewards()
        {
          reward_quantity = 4,
          reward_id = 2,
          reward_type_id = 19
        }
      };
    pvpPlayerFinish1.bonus_rewards = finishBonusRewardsArray;
    WebAPI.Response.PvpPlayerFinish pvpPlayerFinish2 = data;
    WebAPI.Response.PvpPlayerFinishCampaign_rewards[] finishCampaignRewardsArray;
    if (!flag5)
      finishCampaignRewardsArray = new WebAPI.Response.PvpPlayerFinishCampaign_rewards[0];
    else
      finishCampaignRewardsArray = new WebAPI.Response.PvpPlayerFinishCampaign_rewards[1]
      {
        new WebAPI.Response.PvpPlayerFinishCampaign_rewards()
        {
          reward_quantity = 2,
          show_text2 = "debug desuyo-2",
          reward_type_id = 10,
          campaign_id = 1,
          show_title = "debug desuyo-title",
          show_text = "debug desuyo-1",
          reward_id = 0
        }
      };
    pvpPlayerFinish2.campaign_rewards = finishCampaignRewardsArray;
    WebAPI.Response.PvpPlayerFinish pvpPlayerFinish3 = data;
    WebAPI.Response.PvpPlayerFinishFirst_battle_rewards[] firstBattleRewardsArray;
    if (!flag6)
      firstBattleRewardsArray = new WebAPI.Response.PvpPlayerFinishFirst_battle_rewards[0];
    else
      firstBattleRewardsArray = new WebAPI.Response.PvpPlayerFinishFirst_battle_rewards[1]
      {
        new WebAPI.Response.PvpPlayerFinishFirst_battle_rewards()
        {
          reward_id = 0,
          reward_type_id = 10,
          show_text = "姫石　かける　3個だよー"
        }
      };
    pvpPlayerFinish3.first_battle_rewards = firstBattleRewardsArray;
    data.campaign_next_rewards = new WebAPI.Response.PvpPlayerFinishCampaign_next_rewards[0];
    data.pvp_class_record = new PvPClassRecord()
    {
      current_season_draw_count = 5,
      current_season_loss_count = 5,
      current_season_win_count = 0,
      pvp_record = new PvPRecord()
      {
        win = 5,
        loss = 4,
        draw = 1
      }
    };
    data.pvp_record = new PvPRecord();
    data.pvp_record_by_friend = new PvPRecord();
    data.player = player2;
    data.pvp_finish = new PvPEnd();
    data.pvp_finish.battle_result = 1;
    data.pvp_finish.after_player_units = array3;
    data.pvp_finish.after_player_gears = array4;
    data.pvp_finish.before_player_units = array1;
    data.pvp_finish.before_player_gears = playerGears;
    data.ranking = 1000;
    data.ranking_pt = 1000000;
    data.pvp_finish.unlock_quests = unlockQuestArray;
    data.is_tutorial = flag7;
    data.matching_type = 6;
    data.current_class = 1;
    data.reward_money = 1111111111;
    data.pvp_finish.player_character_intimates_in_battle = intimatesInBattleArray2;
    data.campaigns = new Campaign[1]
    {
      new Campaign() { campaign_type_id = 1 }
    };
    return data;
  }
}
