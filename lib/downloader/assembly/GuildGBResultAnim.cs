// Decompiled with JetBrains decompiler
// Type: GuildGBResultAnim
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
public class GuildGBResultAnim : MonoBehaviour
{
  [SerializeField]
  private float sstaranimduration;
  [SerializeField]
  private float sstaranimendduration;
  [SerializeField]
  private float waitLStarAnim;
  [SerializeField]
  private float waitXLStarAnim;
  [SerializeField]
  private float waitXLStarAnimEnd;
  [SerializeField]
  private float waitSkipStarAnim;
  [SerializeField]
  private float waitSkipResetAnim;
  [SerializeField]
  private float waitSkipStarAnimEnd;
  [SerializeField]
  private float sstarSEFadeTime;
  [SerializeField]
  private float battleResultWait;
  [SerializeField]
  private GuildGBResultAnim.GuildData ownGuild;
  [SerializeField]
  private GuildGBResultAnim.GuildData enemyGuild;
  [SerializeField]
  private Animator dir_center;
  private GameObject resultObj;
  private bool skipFlag;
  public int ownStar;
  public int eneStar;

  public void Next() => this.skipFlag = true;

  public void Start()
  {
    this.ownGuild.Set(this);
    this.enemyGuild.Set(this);
    this.ownGuild.SetUseSStart(this.ownStar);
    this.enemyGuild.SetUseSStart(this.eneStar);
  }

  public IEnumerator InitializeAsync(WebAPI.Response.GvgResult result)
  {
    GuildAppearance ownData = new GuildAppearance();
    ownData.level = result.before_level;
    ownData.scaffold_rank = PlayerAffiliation.Current.guild.appearance.scaffold_rank;
    ownData.tower_rank = PlayerAffiliation.Current.guild.appearance.tower_rank;
    ownData.walls_rank = PlayerAffiliation.Current.guild.appearance.walls_rank;
    ownData._current_emblem = PlayerAffiliation.Current.guild.appearance._current_emblem;
    Future<GameObject> fgObj = Res.Prefabs.guild028_2.GuildBase.Load<GameObject>();
    IEnumerator e = fgObj.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.ownGuild.InitializeAsync(fgObj, ownData);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.enemyGuild.InitializeAsync(fgObj, result.opponent.appearance);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void Initialize(WebAPI.Response.GvgResult result, GameObject resultObj)
  {
    this.resultObj = resultObj;
    this.ownGuild.Initialize(this, PlayerAffiliation.Current.guild.guild_name, result.score.total_damage);
    this.enemyGuild.Initialize(this, result.opponent.guild_name, result.score.opponent_total_damage);
    this.ownStar = result.score.total_capture_star;
    this.eneStar = result.score.opponent_total_capture_star;
    this.ownGuild.SetUseSStart(this.ownStar);
    this.enemyGuild.SetUseSStart(this.eneStar);
  }

  public void StartAnimation() => this.StartCoroutine(this.Animation());

  private void OnDisable() => Object.Destroy((Object) this.resultObj);

  private IEnumerator Animation()
  {
    GuildGBResultAnim guildGbResultAnim = this;
    guildGbResultAnim.skipFlag = false;
    bool noneSe = Object.op_Equality((Object) Singleton<NGSoundManager>.GetInstanceOrNull(), (Object) null);
    int se = 0;
    if (!noneSe)
      se = Singleton<NGSoundManager>.GetInstance().PlaySe("SE_2503", true);
    guildGbResultAnim.StartCoroutine(guildGbResultAnim.ownGuild.StartAnimation(guildGbResultAnim.ownStar));
    guildGbResultAnim.StartCoroutine(guildGbResultAnim.enemyGuild.StartAnimation(guildGbResultAnim.eneStar));
    while (!guildGbResultAnim.ownGuild.animationEnd && !guildGbResultAnim.enemyGuild.animationEnd)
      yield return (object) null;
    if (guildGbResultAnim.eneStar > guildGbResultAnim.ownStar)
      guildGbResultAnim.dir_center.SetTrigger("count_result_enemy_win");
    if (guildGbResultAnim.eneStar < guildGbResultAnim.ownStar)
    {
      guildGbResultAnim.dir_center.SetTrigger("count_result_player_win");
    }
    else
    {
      guildGbResultAnim.ownGuild.DamageAnimation();
      guildGbResultAnim.enemyGuild.DamageAnimation();
    }
    while (!guildGbResultAnim.ownGuild.animationEnd || !guildGbResultAnim.enemyGuild.animationEnd)
      yield return (object) null;
    if (!noneSe)
      Singleton<NGSoundManager>.GetInstance().StopSe(se, guildGbResultAnim.sstarSEFadeTime);
    guildGbResultAnim.skipFlag = false;
    yield return (object) new WaitForSeconds(guildGbResultAnim.battleResultWait);
    if (Object.op_Inequality((Object) guildGbResultAnim.resultObj, (Object) null))
    {
      guildGbResultAnim.resultObj = guildGbResultAnim.resultObj.Clone();
      while (!guildGbResultAnim.skipFlag)
        yield return (object) null;
      guildGbResultAnim.resultObj.GetComponent<Animator>().SetTrigger("isOut");
      yield return (object) new WaitForSeconds(0.14f);
      Object.Destroy((Object) guildGbResultAnim.resultObj);
      guildGbResultAnim.skipFlag = false;
    }
    while (!guildGbResultAnim.skipFlag)
      yield return (object) null;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  [Serializable]
  private class StarDigitControl
  {
    public Animator animReposition_;
    public Animator[] indicates_;
    private bool[] wIsIndicates_;
    public Transform[] positions_;
    public Animator animCarry_;
    public TweenPosition effCarry_;
    public TweenPosition positionCarry_;

    public bool[] isIndicates_
    {
      get
      {
        if (this.wIsIndicates_ != null)
          return this.wIsIndicates_;
        this.wIsIndicates_ = new bool[this.indicates_.Length];
        return this.wIsIndicates_;
      }
    }
  }

  [Serializable]
  private class GuildData
  {
    [SerializeField]
    private SpriteDecimalControl controlNumber_;
    [SerializeField]
    private UILabel txt_guild_name;
    [SerializeField]
    private UILabel txt_guild_damage_num;
    [SerializeField]
    private GameObject dyn_GuildBase;
    [SerializeField]
    private UI2DSprite guildTitleImage;
    [SerializeField]
    private GuildGBResultAnim.StarDigitControl dc_s_ = new GuildGBResultAnim.StarDigitControl();
    [SerializeField]
    private GuildGBResultAnim.StarDigitControl dc_l_ = new GuildGBResultAnim.StarDigitControl();
    [SerializeField]
    private GuildGBResultAnim.StarDigitControl dc_xl_ = new GuildGBResultAnim.StarDigitControl();
    [SerializeField]
    private Animator[] dir_s_star;
    [SerializeField]
    private Animator[] dir_s_star_until_10;
    private int? wMaxStarValue_;
    private const string s_star_get = "s_star_get";
    private const string s_star_get_close = "s_star_get_close";
    private const string s_star_set_to_l_star = "s_star_set_to_l_star_{0:D2}";
    private const string l_star_get = "l_star_get";
    private const string l_star_get_close = "l_star_get_close";
    private const string l_star = "l_star_{0:D2}";
    private const string l_star_set_to_xl_star = "l_star_set_to_xl_star_{0:D2}";
    private const string xl_star_get = "l_star_get";
    private const string xl_star = "xl_star_{0:D2}";
    public bool animationEnd;
    private GuildGBResultAnim cntrl;

    public int maxStarValue_
    {
      get
      {
        if (this.wMaxStarValue_.HasValue)
          return this.wMaxStarValue_.Value;
        this.wMaxStarValue_ = new int?(this.dir_s_star.Length * this.dc_l_.indicates_.Length * (this.dc_xl_.indicates_.Length + 1) - 1);
        return this.wMaxStarValue_.Value;
      }
    }

    public void Set(GuildGBResultAnim _cntrl) => this.cntrl = _cntrl;

    public IEnumerator InitializeAsync(Future<GameObject> fgObj, GuildAppearance guildData)
    {
      Guild0282GuildBase guildBase = fgObj.Result.Clone(this.dyn_GuildBase.transform).GetComponent<Guild0282GuildBase>();
      GuildImageCache guildImageCache = new GuildImageCache();
      IEnumerator e = guildImageCache.ResourceLoad(guildData);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      guildBase.SetSprite(guildData, guildImageCache);
      Future<Sprite> futureGuildTitleImage = EmblemUtility.LoadGuildEmblemSprite(guildData._current_emblem);
      e = futureGuildTitleImage.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildTitleImage.sprite2D = futureGuildTitleImage.Result;
    }

    public void Initialize(GuildGBResultAnim _cntrl, string guildName, int damageVal)
    {
      this.cntrl = _cntrl;
      this.txt_guild_damage_num.SetTextLocalize(damageVal);
      this.txt_guild_name.SetTextLocalize(guildName);
    }

    public void Reset()
    {
      for (int index = 0; index < this.dir_s_star.Length; ++index)
        this.dir_s_star[index].SetTrigger("s_star_get_close");
      for (int index = 0; index < this.dir_s_star_until_10.Length; ++index)
        this.dir_s_star_until_10[index].SetTrigger("s_star_get_close");
    }

    public void ResetNumber(int num) => this.controlNumber_.resetNumber(num);

    public void SetUseSStart(int starValue)
    {
      if (starValue < 10)
      {
        ((Component) ((Component) this.dir_s_star[0]).transform.parent.parent).gameObject.SetActive(false);
        ((Component) ((Component) this.dir_s_star_until_10[0]).transform.parent.parent).gameObject.SetActive(true);
      }
      else
      {
        ((Component) ((Component) this.dir_s_star[0]).transform.parent.parent).gameObject.SetActive(true);
        ((Component) ((Component) this.dir_s_star_until_10[0]).transform.parent.parent).gameObject.SetActive(false);
      }
      this.Reset();
    }

    public IEnumerator StartAnimation(int starValue)
    {
      this.animationEnd = false;
      starValue = Mathf.Min(starValue, this.maxStarValue_);
      this.ResetNumber(0);
      this.dc_s_.indicates_ = starValue >= 10 ? this.dir_s_star : this.dir_s_star_until_10;
      bool firstCarryL = true;
      int maxSStar = this.dc_s_.indicates_.Length;
      int maxLStar = this.dc_l_.indicates_.Length * maxSStar;
      for (int i = 0; i < starValue; ++i)
      {
        this.ResetNumber(i + 1);
        int targetStar = i % maxSStar;
        this.dc_s_.indicates_[targetStar].SetTrigger("s_star_get");
        this.dc_s_.isIndicates_[targetStar] = true;
        if (!this.cntrl.skipFlag)
        {
          yield return (object) new WaitForSeconds(this.cntrl.sstaranimduration);
          if (targetStar == maxSStar - 1)
          {
            if (!this.cntrl.skipFlag)
            {
              yield return (object) new WaitForSeconds(this.cntrl.sstaranimendduration);
              if (!this.cntrl.skipFlag)
              {
                this.startEffectCarry(this.dc_s_, this.dc_l_, i % maxLStar / maxSStar, "s_star_get_close", "s_star_set_to_l_star_{0:D2}", "l_star_{0:D2}", "l_star_get");
                yield return (object) new WaitForSeconds(this.cntrl.waitLStarAnim);
              }
              else
                goto label_15;
            }
            else
              goto label_15;
          }
          if (i % maxLStar == maxLStar - 1)
          {
            if (!this.cntrl.skipFlag)
            {
              yield return (object) new WaitForSeconds(this.cntrl.waitXLStarAnim);
              if (!this.cntrl.skipFlag)
              {
                this.startEffectCarry(this.dc_l_, this.dc_xl_, i / maxLStar, "l_star_get_close", "l_star_set_to_xl_star_{0:D2}", "xl_star_{0:D2}", "l_star_get");
                if (firstCarryL && Object.op_Inequality((Object) this.dc_l_.positionCarry_, (Object) null))
                {
                  Vector3 vector3 = Vector3.op_Subtraction(this.dc_l_.positionCarry_.to, this.dc_l_.positionCarry_.from);
                  Transform parent1 = ((IEnumerable<Transform>) this.dc_l_.positions_).First<Transform>().parent;
                  parent1.localPosition = Vector3.op_Addition(parent1.localPosition, vector3);
                  Transform parent2 = ((Component) this.dc_l_.effCarry_).transform.parent;
                  parent2.localPosition = Vector3.op_Addition(parent2.localPosition, vector3);
                }
                firstCarryL = false;
                yield return (object) new WaitForSeconds(this.cntrl.waitXLStarAnimEnd);
                continue;
              }
            }
          }
          else
            continue;
        }
label_15:
        yield return (object) this.skipAnimation(starValue);
        yield break;
      }
      this.animationEnd = true;
    }

    private void startEffectCarry(
      GuildGBResultAnim.StarDigitControl cntlNow,
      GuildGBResultAnim.StarDigitControl cntlNext,
      int index,
      string nowHide,
      string nowCarry,
      string nextReposition,
      string nextIndicate)
    {
      int num = index + 1;
      for (int index1 = 0; index1 < cntlNow.indicates_.Length; ++index1)
      {
        cntlNow.indicates_[index1].SetTrigger(nowHide);
        cntlNow.isIndicates_[index1] = false;
      }
      cntlNow.animCarry_.SetTrigger(string.Format(nowCarry, (object) num));
      cntlNext.animReposition_.SetTrigger(string.Format(nextReposition, (object) num));
      cntlNext.indicates_[index].SetTrigger(nextIndicate);
      cntlNext.isIndicates_[index] = true;
      if (Object.op_Inequality((Object) cntlNow.positionCarry_, (Object) null) && index == 0)
        ((UITweener) cntlNow.positionCarry_).PlayForward();
      Transform position = cntlNext.positions_[index];
      Transform parent = position.parent;
      position.parent = ((Component) cntlNow.effCarry_).transform.parent;
      Vector3 localPosition = position.localPosition;
      position.parent = parent;
      cntlNow.effCarry_.to = localPosition;
      ((UITweener) cntlNow.effCarry_).ResetToBeginning();
      ((UITweener) cntlNow.effCarry_).PlayForward();
    }

    private IEnumerator skipAnimation(int starValue)
    {
      yield return (object) new WaitForSeconds(this.cntrl.waitSkipStarAnim);
      int length = this.dc_s_.indicates_.Length;
      int num = length * this.dc_l_.indicates_.Length;
      int nXLStar = starValue / num;
      int nLStar = starValue % num / length;
      yield return (object) this.skipDigitAnimation(this.dc_s_, starValue % length, "s_star_get", "s_star_get_close");
      yield return (object) this.skipDigitAnimation(this.dc_l_, nLStar, "l_star_get", "l_star_get_close", !((IEnumerable<bool>) this.dc_xl_.isIndicates_).First<bool>() && nXLStar > 0, "l_star_{0:D2}", this.cntrl.waitSkipResetAnim);
      yield return (object) this.skipDigitAnimation(this.dc_xl_, nXLStar, "l_star_get", "l_star_get", sReposition: "xl_star_{0:D2}", waitTime: this.cntrl.waitSkipResetAnim);
      yield return (object) new WaitForSeconds(this.cntrl.waitSkipStarAnimEnd);
      this.ResetNumber(starValue);
      this.animationEnd = true;
    }

    private IEnumerator skipDigitAnimation(
      GuildGBResultAnim.StarDigitControl dc,
      int nStar,
      string sIndicate,
      string sHide,
      bool moveCarry = false,
      string sReposition = null,
      float waitTime = 0.0f)
    {
      int num = ((IEnumerable<bool>) dc.isIndicates_).Count<bool>((Func<bool, bool>) (b => b));
      if (moveCarry && Object.op_Inequality((Object) dc.positionCarry_, (Object) null))
        ((UITweener) dc.positionCarry_).PlayForward();
      if (num != nStar)
      {
        bool bWaitReposition = !string.IsNullOrEmpty(sReposition);
        if (bWaitReposition && num > nStar)
        {
          for (int index = 0; index < num; ++index)
          {
            dc.indicates_[index].SetTrigger(sHide);
            dc.isIndicates_[index] = false;
          }
          yield return (object) new WaitForSeconds(waitTime);
        }
        for (int i = 0; i < dc.indicates_.Length; ++i)
        {
          bool flag = i < nStar;
          if (dc.isIndicates_[i] != flag)
          {
            dc.indicates_[i].SetTrigger(flag ? sIndicate : sHide);
            dc.isIndicates_[i] = flag;
            if (bWaitReposition)
            {
              dc.animReposition_.SetTrigger(string.Format(sReposition, (object) (i + 1)));
              yield return (object) new WaitForSeconds(waitTime);
            }
          }
        }
      }
    }

    public void DamageAnimation()
    {
      ((IEnumerable<UITweener>) ((Component) this.txt_guild_damage_num).GetComponents<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x => ((Behaviour) x).enabled = true));
    }
  }
}
