// Decompiled with JetBrains decompiler
// Type: Guild0288GuildInBattleResultMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Guild0288GuildInBattleResultMenu : ResultMenuBase
{
  [SerializeField]
  private GameObject battleResultMenuContainer;
  [SerializeField]
  private GameObject battleResultMenuTitle;
  [SerializeField]
  private GameObject battleResultIconContainer;
  [SerializeField]
  private GameObject winIcon;
  [SerializeField]
  private GameObject drawIcon;
  [SerializeField]
  private GameObject loseIcon;
  [SerializeField]
  private GameObject excellentIcon;
  [SerializeField]
  private GameObject greatIcon;
  [SerializeField]
  private UI2DSprite manaPointSprite;
  [SerializeField]
  private UI2DSprite phankillMedalSprite;
  [SerializeField]
  private UILabel manaPointCountLabel;
  [SerializeField]
  private UILabel phankillMedalCountLabel;
  [SerializeField]
  private GameObject dir_Mana;
  [SerializeField]
  private GameObject dir_Phankill_Medal;
  [SerializeField]
  private UILabel addedExperienceLabel;
  [SerializeField]
  private UILabel experienceToNextLevelLabel;
  [SerializeField]
  private GameObject dir_Exp;
  [SerializeField]
  private GameObject experienceProgressBar;
  private NGSoundManager soundManager;
  private GameObject playerLevelupPrefab;
  private WebAPI.Response.GvgBattleFinish guildBattleResultData;

  private void Awake() => this.soundManager = Singleton<NGSoundManager>.GetInstance();

  private void Update()
  {
  }

  public override IEnumerator Init(
    WebAPI.Response.GvgBattleFinish guildBattleResultData)
  {
    Debug.Log((object) "Guild0288GuildInBattleResultMenu.Init is invoked!");
    this.guildBattleResultData = guildBattleResultData;
    IEnumerator e = this.LoadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    switch (guildBattleResultData.battle_status)
    {
      case GvgBattleStatus.win:
        this.winIcon.SetActive(true);
        break;
      case GvgBattleStatus.lose:
        this.loseIcon.SetActive(true);
        break;
      case GvgBattleStatus.draw:
        this.drawIcon.SetActive(true);
        break;
    }
    switch (guildBattleResultData.star_status)
    {
      case GvgStarStatus.excellent:
        this.excellentIcon.SetActive(true);
        break;
      case GvgStarStatus.great:
        this.greatIcon.SetActive(true);
        break;
    }
    this.manaPointCountLabel.SetTextLocalize(guildBattleResultData.gain_friend_point);
    this.phankillMedalCountLabel.SetTextLocalize(guildBattleResultData.gain_battle_medal);
    this.addedExperienceLabel.SetTextLocalize("+" + (object) guildBattleResultData.gain_player_experience);
    this.experienceToNextLevelLabel.SetTextLocalize(guildBattleResultData.player.exp_next <= 0 ? "MAX" : guildBattleResultData.player.exp_next.ToString());
  }

  private IEnumerator LoadResources()
  {
    Future<GameObject> h = Res.Prefabs.battle.PlayerLevelUpPrefab.Load<GameObject>();
    IEnumerator e = h.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.playerLevelupPrefab = h.Result;
    Future<Sprite> manaPointTextureF = Singleton<ResourceManager>.GetInstance().Load<Sprite>("Icons/ManaPoint_Icon");
    e = manaPointTextureF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.manaPointSprite.sprite2D = manaPointTextureF.Result;
    Future<Sprite> phankillMedalTextureF = Singleton<ResourceManager>.GetInstance().Load<Sprite>("Icons/BattleMedal_Icon");
    e = phankillMedalTextureF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.phankillMedalSprite.sprite2D = phankillMedalTextureF.Result;
  }

  public override IEnumerator Run()
  {
    Guild0288GuildInBattleResultMenu battleResultMenu = this;
    battleResultMenu.battleResultMenuContainer.SetActive(true);
    battleResultMenu.battleResultMenuTitle.SetActive(true);
    battleResultMenu.battleResultIconContainer.SetActive(true);
    Guild0288GuildInBattleResultMenu.Runner[] runnerArray = new Guild0288GuildInBattleResultMenu.Runner[4]
    {
      new Guild0288GuildInBattleResultMenu.Runner(battleResultMenu.InitObjects),
      new Guild0288GuildInBattleResultMenu.Runner(battleResultMenu.ShowManaPoint),
      new Guild0288GuildInBattleResultMenu.Runner(battleResultMenu.ShowPhankillMedal),
      new Guild0288GuildInBattleResultMenu.Runner(battleResultMenu.ShowPlayerExperience)
    };
    for (int index = 0; index < runnerArray.Length; ++index)
    {
      IEnumerator e = runnerArray[index]();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    runnerArray = (Guild0288GuildInBattleResultMenu.Runner[]) null;
  }

  public override IEnumerator OnFinish()
  {
    this.battleResultMenuContainer.SetActive(false);
    this.battleResultMenuTitle.SetActive(false);
    this.battleResultIconContainer.SetActive(false);
    yield break;
  }

  private IEnumerator InitObjects()
  {
    this.dir_Exp.SetActive(false);
    this.dir_Mana.SetActive(false);
    this.dir_Phankill_Medal.SetActive(false);
    yield return (object) null;
  }

  private IEnumerator ShowManaPoint()
  {
    this.soundManager.playSE("SE_2514");
    this.dir_Mana.SetActive(true);
    yield return (object) new WaitForSeconds(1.5f);
  }

  private IEnumerator ShowPhankillMedal()
  {
    this.soundManager.playSE("SE_1011");
    this.soundManager.playSE("SE_1012");
    this.soundManager.playSE("SE_1013", delay: 1.28f);
    this.dir_Phankill_Medal.SetActive(true);
    yield return (object) new WaitForSeconds(1.5f);
  }

  private IEnumerator ShowPlayerExperience()
  {
    Guild0288GuildInBattleResultMenu battleResultMenu = this;
    battleResultMenu.soundManager.playSE("SE_1012");
    GaugeRunner r = new GaugeRunner(battleResultMenu.experienceProgressBar, (float) battleResultMenu.guildBattleResultData.before_player.exp / (float) (battleResultMenu.guildBattleResultData.before_player.exp + battleResultMenu.guildBattleResultData.before_player.exp_next), (float) battleResultMenu.guildBattleResultData.player.exp / (float) (battleResultMenu.guildBattleResultData.player.exp + battleResultMenu.guildBattleResultData.player.exp_next), battleResultMenu.guildBattleResultData.player.level - battleResultMenu.guildBattleResultData.before_player.level, new Func<GameObject, int, IEnumerator>(battleResultMenu.OnPlayerLevelup));
    battleResultMenu.dir_Exp.SetActive(true);
    yield return (object) new WaitForSeconds(0.9f);
    IEnumerator e = GaugeRunner.Run(r);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator OnPlayerLevelup(GameObject obj, int count)
  {
    Guild0288GuildInBattleResultMenu battleResultMenu = this;
    yield return (object) new WaitForSeconds(0.1f);
    GaugeRunner.PauseSE();
    Battle020171Menu component = battleResultMenu.OpenPopup(battleResultMenu.playerLevelupPrefab).GetComponent<Battle020171Menu>();
    component.SetLv(battleResultMenu.guildBattleResultData.before_player.level + count, battleResultMenu.guildBattleResultData.before_player.level + count + 1);
    component.SetName(battleResultMenu.guildBattleResultData.player.name);
    List<string> self = new List<string>();
    self.Add(Consts.GetInstance().BATTLE_RESULT_RECOVERY_AP);
    int num1;
    if ((num1 = battleResultMenu.guildBattleResultData.player.ap_max - battleResultMenu.guildBattleResultData.before_player.ap_max) > 0)
      self.Add(Consts.Format(Consts.GetInstance().BATTLE_RESULT_INCR_AP, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) num1
        }
      }));
    int num2;
    if ((num2 = battleResultMenu.guildBattleResultData.player.max_cost - battleResultMenu.guildBattleResultData.before_player.max_cost) > 0)
      self.Add(Consts.Format(Consts.GetInstance().BATTLE_RESULT_INCR_DECK_COST, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) num2
        }
      }));
    int num3;
    if ((num3 = battleResultMenu.guildBattleResultData.player.max_friends - battleResultMenu.guildBattleResultData.before_player.max_friends) > 0)
      self.Add(Consts.Format(Consts.GetInstance().BATTLE_RESULT_INCR_FRIEND_COUNT, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) num3
        }
      }));
    component.SetExplanetion(self.Join("\n"));
    bool onFinished = false;
    component.SetCallback((Action) (() => onFinished = true));
    while (!onFinished)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.1f);
    GaugeRunner.ResumeSE();
  }

  private delegate IEnumerator Runner();
}
