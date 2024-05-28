// Decompiled with JetBrains decompiler
// Type: GuildGBResultDetail
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
public class GuildGBResultDetail : MonoBehaviour
{
  [SerializeField]
  private GuildGBResultDetail.GuildResult dir_own_guild_result;
  [SerializeField]
  private GuildGBResultDetail.GuildResult dir_enemy_guild_result;
  [SerializeField]
  private GuildGBResultDetail.GuildLevel dir_guild_level_info;
  [SerializeField]
  private UILabel txt_guildcoin_earned;
  [SerializeField]
  private UILabel txt_total_guildcoin;
  [SerializeField]
  private UILabel txt_contribution_earned;
  [SerializeField]
  private UILabel txt_total_contribution;
  private int totalGuildCoin;
  private int earnedGuildCoin;
  private int totalContribution;
  private int earnedContribution;
  [SerializeField]
  private UIButton nextButton;
  private Stack coutineList;
  [SerializeField]
  private GameObject winObj;
  [SerializeField]
  private GameObject loseObj;
  [SerializeField]
  private GameObject drawObj;
  [SerializeField]
  private float numberAnimDuration;

  public void Start()
  {
    this.coutineList = new Stack();
    ((Component) this.nextButton).gameObject.SetActive(false);
  }

  private IEnumerator InitiliazeAsync()
  {
    yield break;
  }

  public void Initialize()
  {
    GuildGBResultDetail.GuildResult dirOwnGuildResult = this.dir_own_guild_result;
    int num1 = Random.Range(0, 30);
    string startNum1 = num1.ToString();
    num1 = Random.Range(0, 1000);
    string totalDamage1 = num1.ToString();
    dirOwnGuildResult.Initialize("myguild", startNum1, totalDamage1);
    GuildGBResultDetail.GuildResult enemyGuildResult = this.dir_enemy_guild_result;
    int num2 = Random.Range(0, 30);
    string startNum2 = num2.ToString();
    num2 = Random.Range(0, 1000);
    string totalDamage2 = num2.ToString();
    enemyGuildResult.Initialize("myguild", startNum2, totalDamage2);
    this.dir_guild_level_info.Initialize(0, 10, 100, 1);
    this.totalGuildCoin = 100;
    this.earnedGuildCoin = 100;
    this.txt_total_guildcoin.SetTextLocalize(this.totalGuildCoin);
    this.txt_guildcoin_earned.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_BATTLE_RESULT_VALUE_COIN, (IDictionary) new Hashtable()
    {
      {
        (object) "value",
        (object) this.earnedGuildCoin
      }
    }));
    this.totalContribution = 10000;
    this.earnedContribution = 10000000;
    this.txt_contribution_earned.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_BATTLE_RESULT_VALUE_CONTRIBUTION, (IDictionary) new Hashtable()
    {
      {
        (object) "value",
        (object) this.earnedContribution
      }
    }));
    this.txt_total_contribution.SetTextLocalize(this.totalContribution);
  }

  public void Initialize(WebAPI.Response.GvgResult result)
  {
    switch (result.score.battle_status)
    {
      case GvgBattleStatus.win:
        this.winObj.SetActive(true);
        break;
      case GvgBattleStatus.lose:
        this.loseObj.SetActive(true);
        break;
      case GvgBattleStatus.draw:
        this.drawObj.SetActive(true);
        break;
    }
    this.dir_own_guild_result.Initialize(PlayerAffiliation.Current.guild.guild_name, result.score.total_capture_star.ToString(), result.score.total_damage.ToString());
    this.dir_enemy_guild_result.Initialize(result.opponent.guild_name, result.score.opponent_total_capture_star.ToString(), result.score.opponent_total_damage.ToString());
    this.dir_guild_level_info.Initialize(result);
    this.totalGuildCoin = PlayerAffiliation.Current.guild.money - result.score.gain_coin;
    this.earnedGuildCoin = result.score.gain_coin;
    this.txt_total_guildcoin.SetTextLocalize(this.totalGuildCoin);
    this.txt_guildcoin_earned.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_BATTLE_RESULT_VALUE_COIN, (IDictionary) new Hashtable()
    {
      {
        (object) "value",
        (object) this.earnedGuildCoin
      }
    }));
    this.totalContribution = ((IEnumerable<GuildMembership>) PlayerAffiliation.Current.guild.memberships).Single<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id == Player.Current.id)).contribution - result.score.gain_contribution;
    this.earnedContribution = result.score.gain_contribution;
    this.txt_contribution_earned.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_BATTLE_RESULT_VALUE_CONTRIBUTION, (IDictionary) new Hashtable()
    {
      {
        (object) "value",
        (object) this.earnedContribution
      }
    }));
    this.txt_total_contribution.SetTextLocalize(this.totalContribution);
  }

  public void CoinStartAnimation()
  {
    this.coutineList.Push((object) this.coutineList.Count);
    this.StartCoroutine(this.RealTimeAddCounter((Action) (() => this.coutineList.Pop()), this.txt_total_guildcoin, this.txt_guildcoin_earned, this.totalGuildCoin, this.earnedGuildCoin, _addString: Consts.GetInstance().POPUP_GUILD_BATTLE_RESULT_VALUE_COIN));
  }

  public void ExpStartAnimation()
  {
    this.coutineList.Push((object) this.coutineList.Count);
    this.StartCoroutine(this.dir_guild_level_info.StartAnimation((Action) (() => this.coutineList.Pop())));
  }

  public void ContributionStartAnimation()
  {
    this.coutineList.Push((object) this.coutineList.Count);
    this.StartCoroutine(this.RealTimeAddCounter((Action) (() => this.coutineList.Pop()), this.txt_total_contribution, this.txt_contribution_earned, this.totalContribution, this.earnedContribution, _addString: Consts.GetInstance().POPUP_GUILD_BATTLE_RESULT_VALUE_CONTRIBUTION));
    ((Component) this.nextButton).gameObject.SetActive(true);
  }

  public IEnumerator RealTimeAddCounter(
    Action callback,
    UILabel _baseLabel,
    UILabel _addLabel,
    int _baseValue,
    int _addValue,
    string _baseString = "%(value)s",
    string _addString = "+%(value)s")
  {
    float duration = this.numberAnimDuration;
    int sumValue = _baseValue;
    int addValue = _addValue;
    _addLabel.SetTextLocalize(Consts.Format(_addString, (IDictionary) new Hashtable()
    {
      {
        (object) "value",
        (object) addValue
      }
    }));
    float test = 0.0f;
    while (sumValue < _baseValue + _addValue)
    {
      _baseLabel.SetTextLocalize(Consts.Format(_baseString, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) sumValue
        }
      }));
      yield return (object) null;
      test += Time.deltaTime;
      float num = (float) _addValue / duration * Time.deltaTime;
      sumValue += Mathf.CeilToInt(num);
      addValue -= Mathf.CeilToInt(num);
    }
    _baseLabel.SetTextLocalize(Consts.Format(_baseString, (IDictionary) new Hashtable()
    {
      {
        (object) "value",
        (object) (_baseValue + _addValue)
      }
    }));
    callback();
  }

  public void Next()
  {
    if (this.coutineList.Count != 0)
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  [Serializable]
  private class GuildResult
  {
    [SerializeField]
    private UILabel txt_guild_name;
    [SerializeField]
    private UILabel txt_stars_earned;
    [SerializeField]
    private UILabel txt_total_damages;

    public void Initialize(string guildName = "", string startNum = "", string totalDamage = "")
    {
      if (Object.op_Inequality((Object) this.txt_guild_name, (Object) null) && guildName != "")
        this.txt_guild_name.SetTextLocalize(guildName);
      if (Object.op_Inequality((Object) this.txt_stars_earned, (Object) null) && startNum != "")
        this.txt_stars_earned.SetTextLocalize(startNum);
      if (!Object.op_Inequality((Object) this.txt_total_damages, (Object) null) || !(totalDamage != ""))
        return;
      this.txt_total_damages.SetTextLocalize(totalDamage);
    }
  }

  [Serializable]
  private class GuildLevel
  {
    [SerializeField]
    private NGTweenGaugeScale slc_gauge_exp;
    [SerializeField]
    private UILabel txt_guild_exp_earned;
    [SerializeField]
    private UILabel txt_guild_exp_for_next_level;
    [SerializeField]
    private UILabel txt_guild_level_num;
    private GameObject levelUpPrefab;
    private GuildImageCache guildImageCache;
    private Guild0282GuildBase guildBaseEffPrefab;
    private Guild0282GuildBase guildBasePrefab;
    private int currentLv;
    private int levelupCnt;
    private float beforeRate;
    private float afterRate;

    public void Initialize(int nowExp, int addExp, int maxExp, int guildLevel)
    {
      this.SetGuildLevelLabel(guildLevel);
      this.slc_gauge_exp.setValue(nowExp, maxExp, false);
      this.txt_guild_exp_earned.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_BATTLE_RESULT_VALUE_EXP, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) addExp
        }
      }));
      this.currentLv = PlayerAffiliation.Current.guild.appearance.level;
      this.levelupCnt = 1;
      this.beforeRate = 0.5f;
      this.afterRate = 0.5f;
    }

    public void Initialize(WebAPI.Response.GvgResult result)
    {
      this.SetGuildLevelLabel(result.before_level);
      if (result.before_experience + result.before_experience_next == 0)
        this.slc_gauge_exp.setValue(0, 1, false);
      else
        this.slc_gauge_exp.setValue(result.before_experience, result.before_experience + result.before_experience_next, false);
      string text = Consts.Format(Consts.GetInstance().POPUP_GUILD_BATTLE_RESULT_VALUE_EXP, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) result.score.gain_experience
        }
      });
      this.SetExperienceNextLabel(PlayerAffiliation.Current.guild.appearance.experience_next);
      this.txt_guild_exp_earned.SetTextLocalize(text);
      this.currentLv = result.before_level;
      this.levelupCnt = PlayerAffiliation.Current.guild.appearance.level - result.before_level;
      this.beforeRate = result.before_experience + result.before_experience_next != 0 ? (float) result.before_experience / (float) (result.before_experience + result.before_experience_next) : 0.0f;
      if (PlayerAffiliation.Current.guild.appearance.experience + PlayerAffiliation.Current.guild.appearance.experience_next == 0)
        this.afterRate = 0.0f;
      else
        this.afterRate = (float) PlayerAffiliation.Current.guild.appearance.experience / (float) (PlayerAffiliation.Current.guild.appearance.experience + PlayerAffiliation.Current.guild.appearance.experience_next);
    }

    private void SetExperienceNextLabel(int expNext)
    {
      this.txt_guild_exp_for_next_level.SetTextLocalize(Consts.Format(Consts.GetInstance().GUILD_BANK_EXP_NEXT_LEVEL, (IDictionary) new Hashtable()
      {
        {
          (object) "exp",
          (object) expNext
        }
      }));
    }

    private void SetGuildLevelLabel(int level)
    {
      this.txt_guild_level_num.SetTextLocalize(level.ToString());
    }

    public IEnumerator StartAnimation(Action callback)
    {
      GuildGBResultDetail.GuildLevel guildLevel = this;
      if (PlayerAffiliation.Current.guild.appearance.experience_next != 0)
        GaugeRunner.PlaySE();
      guildLevel.guildImageCache = (GuildImageCache) null;
      Func<GameObject, int, IEnumerator> levelupCallback = (Func<GameObject, int, IEnumerator>) null;
      IEnumerator e;
      if (guildLevel.levelupCnt > 0)
      {
        Future<GameObject> fgObjEff = Res.Prefabs.guild028_2.GuildBase_for_levelup_anim.Load<GameObject>();
        e = fgObjEff.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        guildLevel.guildBaseEffPrefab = fgObjEff.Result.Clone().GetComponent<Guild0282GuildBase>();
        ((Component) guildLevel.guildBaseEffPrefab).gameObject.SetActive(false);
        Future<GameObject> fgObj = Res.Prefabs.guild028_2.GuildBase.Load<GameObject>();
        e = fgObj.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        guildLevel.guildBasePrefab = fgObj.Result.Clone().GetComponent<Guild0282GuildBase>();
        ((Component) guildLevel.guildBasePrefab).gameObject.SetActive(false);
        guildLevel.guildImageCache = new GuildImageCache();
        e = guildLevel.guildImageCache.GuildBankLevelUpResourceLoad(guildLevel.levelupCnt);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (Object.op_Equality((Object) guildLevel.levelUpPrefab, (Object) null))
        {
          Future<GameObject> fObj = Res.Prefabs.popup.guild_base_levelup_anim.Load<GameObject>();
          e = fObj.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          guildLevel.levelUpPrefab = fObj.Result;
          fObj = (Future<GameObject>) null;
        }
        levelupCallback = new Func<GameObject, int, IEnumerator>(guildLevel.Levelup);
        fgObjEff = (Future<GameObject>) null;
        fgObj = (Future<GameObject>) null;
      }
      e = GaugeRunner.Run(new GaugeRunner(((Component) guildLevel.slc_gauge_exp).gameObject, guildLevel.beforeRate, guildLevel.afterRate, guildLevel.levelupCnt, levelupCallback));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (Object.op_Inequality((Object) guildLevel.guildBasePrefab, (Object) null))
      {
        Object.Destroy((Object) ((Component) guildLevel.guildBasePrefab).gameObject);
        guildLevel.guildBasePrefab = (Guild0282GuildBase) null;
      }
      if (Object.op_Inequality((Object) guildLevel.guildBaseEffPrefab, (Object) null))
      {
        Object.Destroy((Object) ((Component) guildLevel.guildBaseEffPrefab).gameObject);
        guildLevel.guildBaseEffPrefab = (Guild0282GuildBase) null;
      }
      Persist.guildTopLevel.Data.level = PlayerAffiliation.Current.guild.appearance.level;
      callback();
    }

    public IEnumerator TestStartAnimation(Action callback)
    {
      GuildGBResultDetail.GuildLevel guildLevel = this;
      if ((double) guildLevel.afterRate != 1.0)
        GaugeRunner.PlaySE();
      guildLevel.guildImageCache = (GuildImageCache) null;
      Func<GameObject, int, IEnumerator> levelupCallback = (Func<GameObject, int, IEnumerator>) null;
      IEnumerator e;
      if (guildLevel.levelupCnt > 0)
      {
        Future<GameObject> fgObjEff = Res.Prefabs.guild028_2.GuildBase_for_levelup_anim.Load<GameObject>();
        e = fgObjEff.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        guildLevel.guildBaseEffPrefab = fgObjEff.Result.Clone().GetComponent<Guild0282GuildBase>();
        ((Component) guildLevel.guildBaseEffPrefab).gameObject.SetActive(false);
        Future<GameObject> fgObj = Res.Prefabs.guild028_2.GuildBase.Load<GameObject>();
        e = fgObj.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        guildLevel.guildBasePrefab = fgObj.Result.Clone().GetComponent<Guild0282GuildBase>();
        ((Component) guildLevel.guildBasePrefab).gameObject.SetActive(false);
        guildLevel.guildImageCache = new GuildImageCache();
        e = guildLevel.guildImageCache.GuildBankLevelUpResourceLoad(guildLevel.levelupCnt);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (Object.op_Equality((Object) guildLevel.levelUpPrefab, (Object) null))
        {
          Future<GameObject> fObj = Res.Prefabs.popup.guild_base_levelup_anim.Load<GameObject>();
          e = fObj.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          guildLevel.levelUpPrefab = fObj.Result;
          fObj = (Future<GameObject>) null;
        }
        levelupCallback = new Func<GameObject, int, IEnumerator>(guildLevel.Levelup);
        fgObjEff = (Future<GameObject>) null;
        fgObj = (Future<GameObject>) null;
      }
      e = GaugeRunner.Run(new GaugeRunner(((Component) guildLevel.slc_gauge_exp).gameObject, guildLevel.beforeRate, guildLevel.afterRate, guildLevel.levelupCnt, levelupCallback));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (Object.op_Inequality((Object) guildLevel.guildBasePrefab, (Object) null))
      {
        Object.Destroy((Object) ((Component) guildLevel.guildBasePrefab).gameObject);
        guildLevel.guildBasePrefab = (Guild0282GuildBase) null;
      }
      if (Object.op_Inequality((Object) guildLevel.guildBaseEffPrefab, (Object) null))
      {
        Object.Destroy((Object) ((Component) guildLevel.guildBaseEffPrefab).gameObject);
        guildLevel.guildBaseEffPrefab = (Guild0282GuildBase) null;
      }
      Persist.guildTopLevel.Data.level = PlayerAffiliation.Current.guild.appearance.level;
      callback();
    }

    public IEnumerator Levelup(GameObject obj, int count)
    {
      GaugeRunner.PauseSE();
      GameObject prefab1 = this.levelUpPrefab.Clone();
      GuildLevelUpAnimPopup component = prefab1.GetComponent<GuildLevelUpAnimPopup>();
      ++this.currentLv;
      int currentLv = this.currentLv;
      GameObject gameObject1 = ((Component) this.guildBasePrefab).gameObject;
      GameObject gameObject2 = ((Component) this.guildBaseEffPrefab).gameObject;
      GuildImageCache guildImageCache = this.guildImageCache;
      component.Initialize(currentLv, gameObject1, gameObject2, guildImageCache);
      GameObject prefab = Singleton<PopupManager>.GetInstance().open(prefab1, isCloned: true);
      while (!Object.op_Equality((Object) prefab, (Object) null))
        yield return (object) null;
      this.SetGuildLevelLabel(this.currentLv);
      GaugeRunner.ResumeSE();
    }
  }
}
