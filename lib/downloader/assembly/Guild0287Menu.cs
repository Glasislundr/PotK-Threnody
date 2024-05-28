// Decompiled with JetBrains decompiler
// Type: Guild0287Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Guild0287Menu : GuildBankDonateListBase
{
  private const int Width = 616;
  private const int Height = 156;
  private const int ColumnValue = 1;
  private const int RowValue = 8;
  private const int ScreenValue = 3;
  private GuildBank bank;
  private GuildBank bankBefore;
  private GuildBank bankAfter;
  private GameObject donateResultPopup;
  private GameObject donateListPrefab;
  private GameObject levelUpPrefab;
  private GuildBankDonationInfo donationInfo;
  [SerializeField]
  private UILabel sceneTitle;
  [SerializeField]
  private UISprite lvGauge;
  [SerializeField]
  private UISprite lv10;
  [SerializeField]
  private UISprite lv1;
  [SerializeField]
  private UILabel guild_zeny;
  [SerializeField]
  private UILabel exp_next_level;
  [SerializeField]
  private UILabel guildBankMessage;
  [SerializeField]
  private GameObject dir_donate_done;
  [SerializeField]
  private UILabel txt_donate_done;
  [SerializeField]
  private List<Guild0287Menu.RateButton> rateBtns;
  private int currentScale;
  private GuildImageCache guildImageCache;
  private int[] level_cmp;

  private IEnumerator ResourceLoad()
  {
    Future<GameObject> fgObj;
    IEnumerator e;
    if (Object.op_Equality((Object) this.donateResultPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_guild_donate_result__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.donateResultPopup = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.donateListPrefab, (Object) null))
    {
      fgObj = Res.Prefabs.guild028_7.dir_donate_list.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.donateListPrefab = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.levelUpPrefab, (Object) null))
    {
      fgObj = Res.Prefabs.popup.guild_base_levelup_anim.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.levelUpPrefab = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
  }

  private void SetAppearance()
  {
    this.sceneTitle.SetTextLocalize(Consts.GetInstance().GUILD_BANK_TOP_TITLE);
    this.guildBankMessage.SetTextLocalize(this.bank.message);
    this.guild_zeny.SetTextLocalize(this.bank.money);
    this.SetGuildLevelLabel(this.bank.level);
    this.SetExperienceNextLabel(this.bank.experience_next);
    float num = 0.0f;
    if (this.bank.experience_next > 0)
      num = (float) this.bank.experience / (float) (this.bank.experience + this.bank.experience_next);
    ((Component) this.lvGauge).transform.localScale = new Vector3(num, 1f, 1f);
    this.dir_donate_done.SetActive(!this.bank.available || !this.bank.released);
    if (!this.bank.released)
      this.txt_donate_done.SetTextLocalize(Consts.GetInstance().GUILD_BANK_DONATE_UNAVAILABLE_JOIN_GUILD);
    else
      this.txt_donate_done.SetTextLocalize(Consts.GetInstance().GUILD_BANK_DONATE_UNAVAILABLE);
    this.SetRateButton();
    this.SetRateButtonColor();
  }

  private void SetGuildLevelLabel(int level)
  {
    if (level < 10)
    {
      ((Component) this.lv1).gameObject.SetActive(false);
      ((Component) this.lv10).gameObject.SetActive(true);
      this.lv10.SetSprite(string.Format("slc_text_glv_number{0}.png__GUI__guild_common_other__guild_common_other_prefab", (object) level));
    }
    else
    {
      ((Component) this.lv1).gameObject.SetActive(true);
      ((Component) this.lv10).gameObject.SetActive(true);
      this.lv1.SetSprite(string.Format("slc_text_glv_number{0}.png__GUI__guild_common_other__guild_common_other_prefab", (object) (level % 10)));
      this.lv10.SetSprite(string.Format("slc_text_glv_number{0}.png__GUI__guild_common_other__guild_common_other_prefab", (object) (level / 10)));
    }
  }

  private void SetExperienceNextLabel(int expNext)
  {
    this.exp_next_level.SetTextLocalize(Consts.Format(Consts.GetInstance().GUILD_BANK_EXP_NEXT_LEVEL, (IDictionary) new Hashtable()
    {
      {
        (object) "exp",
        (object) expNext
      }
    }));
  }

  private void SetRateButton()
  {
    for (int index = 0; index < this.bank.scales.Length; ++index)
    {
      if (this.rateBtns.Count <= index)
        break;
      if (!this.bank.available || !this.bank.released)
      {
        ((Component) ((Component) this.rateBtns[index].btn).transform.parent).gameObject.SetActive(false);
      }
      else
      {
        ((Component) ((Component) this.rateBtns[index].btn).transform.parent).gameObject.SetActive(true);
        GuildInvestScale scale1 = this.bank.scales[index];
        int scale = scale1.scale;
        this.rateBtns[index].Scale = scale;
        ((UIButtonColor) this.rateBtns[index].btn).isEnabled = this.bank.level >= scale1.release_level;
        this.rateBtns[index].btn.onClick.Clear();
        this.rateBtns[index].btn.onClick.Add(new EventDelegate((EventDelegate.Callback) (() => this.onRateButton(scale))));
        if (((UIButtonColor) this.rateBtns[index].btn).isEnabled)
        {
          if (Object.op_Inequality((Object) this.rateBtns[index].dir_open_condition, (Object) null))
            this.rateBtns[index].dir_open_condition.SetActive(false);
        }
        else
        {
          if (Object.op_Inequality((Object) this.rateBtns[index].dir_open_condition, (Object) null))
            this.rateBtns[index].dir_open_condition.SetActive(true);
          if (Object.op_Inequality((Object) this.rateBtns[index].txt_crest_guild_condition, (Object) null))
            this.rateBtns[index].txt_crest_guild_condition.SetTextLocalize(Consts.Format(Consts.GetInstance().GUILD_BANK_RATEBUTTON_RELEASE, (IDictionary) new Hashtable()
            {
              {
                (object) "level",
                (object) scale1.release_level
              }
            }));
          if (Object.op_Inequality((Object) this.rateBtns[index].txt_crest_guild_condition_shadow, (Object) null))
            this.rateBtns[index].txt_crest_guild_condition_shadow.SetTextLocalize(Consts.Format(Consts.GetInstance().GUILD_BANK_RATEBUTTON_RELEASE, (IDictionary) new Hashtable()
            {
              {
                (object) "level",
                (object) scale1.release_level
              }
            }));
        }
      }
    }
  }

  public void SetRateButtonColor()
  {
    for (int index = 0; index < this.rateBtns.Count; ++index)
    {
      if (((UIButtonColor) this.rateBtns[index].btn).isEnabled)
      {
        Color color = this.currentScale == this.rateBtns[index].Scale ? Color.white : Color.gray;
        ((Component) this.rateBtns[index].btn).GetComponent<SpreadColorButton>().SetColor(color);
      }
    }
  }

  private IEnumerator ExecuteDonation(GuildMoneyRate moneyRate)
  {
    Guild0287Menu guild0287Menu = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
    GuildMembership guildMembership1 = ((IEnumerable<GuildMembership>) PlayerAffiliation.Current.guild.memberships).FirstOrDefault<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id.Equals(Player.Current.id)));
    int contribution = guildMembership1 == null ? moneyRate.gain_contribution * guild0287Menu.currentScale : guildMembership1.contribution;
    string errorCode = string.Empty;
    Future<WebAPI.Response.GuildBankInvest> fObj = WebAPI.GuildBankInvest(moneyRate.rate_id, guild0287Menu.currentScale, (Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      errorCode = e.Code;
    }));
    IEnumerator e1 = fObj.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (fObj.Result == null)
    {
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      if (errorCode.Equals("GLD014"))
      {
        Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
        MypageScene.ChangeSceneOnError();
      }
      else
        guild0287Menu.backScene();
    }
    else
    {
      Singleton<NGGameDataManager>.GetInstance().refreshGuildTop = true;
      GuildMembership guildMembership2 = ((IEnumerable<GuildMembership>) PlayerAffiliation.Current.guild.memberships).FirstOrDefault<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id.Equals(Player.Current.id)));
      if (guildMembership2 != null)
        contribution = guildMembership2.contribution - contribution;
      guild0287Menu.bankBefore = fObj.Result.from_bank;
      guild0287Menu.bankAfter = fObj.Result.bank;
      guild0287Menu.level_cmp = fObj.Result.level_cmp;
      guild0287Menu.bank = guild0287Menu.bankBefore;
      bool flag = guild0287Menu.bank.experience + guild0287Menu.bank.experience_next == 0;
      guild0287Menu.donationInfo.donateType = moneyRate.token_type;
      guild0287Menu.donationInfo.quantity = moneyRate.ask_token * guild0287Menu.currentScale;
      guild0287Menu.donationInfo.zeny_before = guild0287Menu.bankBefore.money;
      guild0287Menu.donationInfo.zeny_result = guild0287Menu.bankAfter.money;
      guild0287Menu.donationInfo.exp = flag ? 0 : guild0287Menu.bankAfter.money - guild0287Menu.bankBefore.money;
      guild0287Menu.donationInfo.contribution = contribution;
      guild0287Menu.bank.available = false;
      guild0287Menu.SetAppearance();
      e1 = guild0287Menu.InitDonateScroll(guild0287Menu.bankAfter.tokens);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      guild0287Menu.ShowResultPopup(guild0287Menu.donationInfo);
    }
  }

  private void ShowResultPopup(GuildBankDonationInfo info)
  {
    GameObject prefab = this.donateResultPopup.Clone();
    prefab.GetComponent<GuildBankDonateResultPopup>().Initialize(info, (Action) (() => this.StartCoroutine(this.StartAnimation())));
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }

  private IEnumerator StartAnimation()
  {
    Guild0287Menu guild0287Menu = this;
    while (Singleton<PopupManager>.GetInstance().isOpen)
      yield return (object) null;
    float duration = 1f;
    float targetScale = 1f;
    int currentLv = guild0287Menu.bankBefore.level;
    int levelupCnt = guild0287Menu.bankAfter.level - guild0287Menu.bankBefore.level;
    float currentMoney = (float) guild0287Menu.bankBefore.money;
    float addedMoney = (float) (guild0287Menu.bankAfter.money - guild0287Menu.bankBefore.money);
    float currentGaugeScaleX = ((Component) guild0287Menu.lvGauge).transform.localScale.x;
    float currentExp = (float) guild0287Menu.bankBefore.experience;
    float nextExp = (float) (guild0287Menu.bankBefore.experience + guild0287Menu.bankBefore.experience_next);
    bool maxLevel = (double) nextExp == 0.0;
    if (!maxLevel)
      GaugeRunner.PlaySE();
    Guild0282GuildBase guildBaseEffPrefab = (Guild0282GuildBase) null;
    Guild0282GuildBase guildBasePrefab = (Guild0282GuildBase) null;
    guild0287Menu.guildImageCache = (GuildImageCache) null;
    IEnumerator e;
    if (levelupCnt > 0)
    {
      Future<GameObject> fgObjEff = Res.Prefabs.guild028_2.GuildBase_for_levelup_anim.Load<GameObject>();
      e = fgObjEff.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      guildBaseEffPrefab = fgObjEff.Result.Clone().GetComponent<Guild0282GuildBase>();
      ((Component) guildBaseEffPrefab).gameObject.SetActive(false);
      Future<GameObject> fgObj = Res.Prefabs.guild028_2.GuildBase.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      guildBasePrefab = fgObj.Result.Clone().GetComponent<Guild0282GuildBase>();
      ((Component) guildBasePrefab).gameObject.SetActive(false);
      guild0287Menu.guildImageCache = new GuildImageCache();
      e = guild0287Menu.guildImageCache.GuildBankLevelUpResourceLoad(levelupCnt);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      fgObjEff = (Future<GameObject>) null;
      fgObj = (Future<GameObject>) null;
    }
    while (!maxLevel)
    {
      float num = targetScale / (duration / Time.deltaTime);
      if (!maxLevel)
      {
        currentGaugeScaleX += num;
        currentExp += nextExp * num;
      }
      guild0287Menu.SetExperienceNextLabel(Mathf.Max(0, (int) ((double) nextExp - (double) currentExp)));
      currentMoney += addedMoney * num / (float) (levelupCnt + 1);
      guild0287Menu.guild_zeny.SetTextLocalize((int) currentMoney);
      ((Component) guild0287Menu.lvGauge).transform.localScale = new Vector3(Mathf.Min(1f, currentGaugeScaleX), 1f, 1f);
      if (currentLv < guild0287Menu.bankAfter.level && (double) currentExp >= (double) nextExp)
      {
        GaugeRunner.StopSE();
        currentGaugeScaleX = 0.0f;
        ((Component) guild0287Menu.lvGauge).transform.localScale = new Vector3(0.0f, 1f, 1f);
        currentExp = 0.0f;
        nextExp = currentLv - guild0287Menu.bankBefore.level < guild0287Menu.level_cmp.Length ? (float) guild0287Menu.level_cmp[currentLv - guild0287Menu.bankBefore.level] : (float) (guild0287Menu.bankAfter.experience + guild0287Menu.bankAfter.experience_next);
        maxLevel = (double) nextExp == 0.0;
        ++currentLv;
        guild0287Menu.SetGuildLevelLabel(currentLv);
        e = guild0287Menu.showLevelupAnim(currentLv, ((Component) guildBasePrefab).gameObject, ((Component) guildBaseEffPrefab).gameObject);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (!maxLevel)
          GaugeRunner.PlaySE();
        else
          break;
      }
      if (maxLevel || currentLv >= guild0287Menu.bankAfter.level && (double) currentGaugeScaleX >= (double) guild0287Menu.bankAfter.experience / (double) (guild0287Menu.bankAfter.experience_next + guild0287Menu.bankAfter.experience))
      {
        GaugeRunner.StopSE();
        break;
      }
      yield return (object) null;
    }
    if (Object.op_Inequality((Object) guildBasePrefab, (Object) null))
    {
      Object.Destroy((Object) ((Component) guildBasePrefab).gameObject);
      guildBasePrefab = (Guild0282GuildBase) null;
    }
    if (Object.op_Inequality((Object) guildBaseEffPrefab, (Object) null))
    {
      Object.Destroy((Object) ((Component) guildBaseEffPrefab).gameObject);
      guildBaseEffPrefab = (Guild0282GuildBase) null;
    }
    guild0287Menu.bank = guild0287Menu.bankAfter;
    guild0287Menu.SetAppearance();
    guild0287Menu.IsPush = false;
    Persist.guildTopLevel.Data.level = PlayerAffiliation.Current.guild.appearance.level;
  }

  private IEnumerator showLevelupAnim(int level, GameObject guildBase, GameObject guildBaseEff)
  {
    GameObject prefab = this.levelUpPrefab.Clone();
    prefab.GetComponent<GuildLevelUpAnimPopup>().Initialize(level, guildBase, guildBaseEff, this.guildImageCache);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    while (Singleton<PopupManager>.GetInstance().isOpen)
      yield return (object) null;
  }

  protected override IEnumerator CreateScroll(int info_index, int bar_index)
  {
    Guild0287Menu guild0287Menu = this;
    if (bar_index < guild0287Menu.allDonateBar.Count && info_index < guild0287Menu.allDonateInfo.Count)
    {
      GuildBankDonateListParts scrollParts = guild0287Menu.allDonateBar[bar_index];
      DonateBarInfo donateBarInfo = guild0287Menu.allDonateInfo[info_index];
      donateBarInfo.scrollParts = scrollParts;
      // ISSUE: reference to a compiler-generated method
      IEnumerator e = scrollParts.Initialize(guild0287Menu.bank.available && guild0287Menu.bank.released, donateBarInfo.moneyRate, guild0287Menu.currentScale, new Action<GuildMoneyRate>(guild0287Menu.\u003CCreateScroll\u003Eb__36_0));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ((Component) scrollParts).gameObject.SetActive(true);
    }
  }

  public IEnumerator InitDonateScroll(GuildMoneyRate[] tokens)
  {
    Guild0287Menu guild0287Menu = this;
    guild0287Menu.allDonateInfo.Clear();
    guild0287Menu.allDonateBar.Clear();
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    DateTime nowTime = ServerTime.NowAppTime();
    guild0287Menu.Initialize(nowTime, 616, 156, 8, 3);
    guild0287Menu.CreateDonateInfo(tokens);
    if (guild0287Menu.allDonateInfo.Count > 0)
    {
      e = guild0287Menu.CreateScrollBase(guild0287Menu.donateListPrefab);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    guild0287Menu.scroll.ResolvePosition();
    guild0287Menu.scroll.scrollView.UpdatePosition();
    guild0287Menu.InitializeEnd();
  }

  public IEnumerator InitializeAsync(GuildBank bank)
  {
    this.bank = bank;
    IEnumerator e = this.ResourceLoad();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.currentScale = 1;
    this.SetAppearance();
    e = this.InitDonateScroll(this.bank.tokens);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) new WaitForSeconds(0.2f);
    this.donationInfo = new GuildBankDonationInfo();
  }

  private void onRateButton(int scale)
  {
    if (scale == this.currentScale)
      return;
    for (int index = 0; index < this.allDonateBar.Count; ++index)
      this.allDonateBar[index].SetAppearance(scale);
    this.currentScale = scale;
    this.SetRateButtonColor();
  }

  public void onHowtoButton()
  {
    if (this.IsPushAndSet())
      return;
    Guild02871Scene.ChangeScene();
  }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  [Serializable]
  public class RateButton
  {
    private int scale;
    public UIButton btn;
    public GameObject dir_open_condition;
    public UILabel txt_crest_guild_condition;
    public UILabel txt_crest_guild_condition_shadow;

    public int Scale
    {
      set => this.scale = value;
      get => this.scale;
    }
  }
}
