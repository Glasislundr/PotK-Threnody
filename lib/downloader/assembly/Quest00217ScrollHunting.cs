// Decompiled with JetBrains decompiler
// Type: Quest00217ScrollHunting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Quest00217ScrollHunting : Quest00217Scroll
{
  public override IEnumerator InitScroll(Quest00217Scroll.Parameter param, DateTime serverTime)
  {
    this.Setup(param, serverTime);
    IEnumerator e = this.SetAndCreate_BannerSprite(param);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public new void Setup(Quest00217Scroll.Parameter param, DateTime serverTime)
  {
    this.initialized = false;
    this.initData = param;
    this.inside_ = ((Component) this).gameObject.activeSelf;
    this.rankingEventTerm = CampaignQuest.RankingEventTerm.normal;
    this.SetEndTime(param.eventInfo.end_at);
    this.resetLocalTime(serverTime);
    ((Behaviour) this.Button).enabled = true;
    bool flag1 = param.isNew;
    bool flag2 = param.isClear;
    bool flag3 = param.isHighlighting;
    if (param.isNotice && param.startTime.HasValue && param.startTime.Value > serverTime)
    {
      flag2 = false;
      flag1 = false;
      flag3 = false;
      ((Behaviour) this.Button).enabled = false;
      this.SetEndTimeAsStart(param.startTime.Value);
      this.startCountdown(param.startTime.Value - serverTime);
    }
    this.loadEnabledHighlighting = flag3;
    EventDelegate.Set(this.BtnFormation.onClick, (EventDelegate.Callback) (() => Quest00230Scene.ChangeScene(true, param.eventInfo)));
    ((Component) this.New).gameObject.SetActive(flag1);
    ((Component) this.Clear).gameObject.SetActive(flag2);
    if (Object.op_Inequality((Object) this.Highlighting, (Object) null))
      this.Highlighting.SetActive(false);
    if (serverTime < param.eventInfo.end_at)
    {
      this.EndTime = param.eventInfo.end_at;
      this.rankingEventTerm = CampaignQuest.RankingEventTerm.normal;
    }
    else if (serverTime < param.eventInfo.final_at)
    {
      this.EndTime = param.eventInfo.final_at;
      this.rankingEventTerm = CampaignQuest.RankingEventTerm.receive;
    }
    this.SetTime(serverTime, this.rankingEventTerm);
    this.initialized = true;
  }

  public new IEnumerator SetAndCreate_BannerSprite()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Quest00217ScrollHunting quest00217ScrollHunting = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) quest00217ScrollHunting.SetAndCreate_BannerSprite(quest00217ScrollHunting.initData);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator SetAndCreate_BannerSprite(Quest00217Scroll.Parameter param)
  {
    Quest00217ScrollHunting quest00217ScrollHunting = this;
    IEnumerator e = Singleton<NGGameDataManager>.GetInstance().GetWebImage(param.eventInfo.banner_image_url, quest00217ScrollHunting.IdleSprite);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) quest00217ScrollHunting.Highlighting, (Object) null))
    {
      quest00217ScrollHunting.EffectRenderer.SetTexture("_MaskTex", ((UIWidget) quest00217ScrollHunting.IdleSprite).mainTexture);
      quest00217ScrollHunting.Highlighting.SetActive(false);
    }
  }

  protected override void updateCountdown(bool immediate)
  {
    if (!this.enabledCountdown)
      return;
    TimeSpan tspan = this.timeGoal - DateTime.Now;
    if (tspan.Ticks <= 0L)
    {
      this.enabledCountdown = false;
      if (!immediate)
        this.duplicateEffectFadeOut(((Component) this).gameObject, ((Component) this).gameObject, 0.5f);
      ((Component) this.Clear).gameObject.SetActive(this.initData.isClear);
      ((Component) this.New).gameObject.SetActive(this.initData.isNew);
      if (Object.op_Inequality((Object) this.Highlighting, (Object) null))
        this.Highlighting.SetActive(this.initData.isHighlighting);
      this.SetEndTime(this.initData.extra.today_day_end_at);
      this.SetTime(this.nowLocalTime, this.rankingEventTerm);
      if (immediate)
        ((Behaviour) this.Button).enabled = true;
      else
        this.fadeIn(0.5f);
    }
    else
    {
      if (!(this.lastSeconds != tspan.Seconds | immediate))
        return;
      this.lastSeconds = tspan.Seconds;
      this.updateTime(tspan, immediate ? 0.0f : 0.5f);
    }
  }

  protected override GameObject duplicateEffectFadeOut(
    GameObject parentObj,
    GameObject originalObj,
    float duration)
  {
    GameObject gameObject = NGUITools.AddChild(parentObj, originalObj);
    this.objEffect_ = gameObject;
    gameObject.SetActive(true);
    Object.Destroy((Object) gameObject.GetComponent<Quest002171Scroll>());
    gameObject.AddComponent<Quest00217ScrollFadeOut>().init(duration, 100, new EventDelegate((EventDelegate.Callback) (() =>
    {
      Object.Destroy((Object) this.objEffect_);
      this.objEffect_ = (GameObject) null;
    })));
    return gameObject;
  }

  protected override void fadeIn(float duration)
  {
    UIWidget uiWidget = Quest00217ScrollFadeOut.setWidget(((Component) this).gameObject, 0);
    if (!Object.op_Inequality((Object) uiWidget, (Object) null))
      return;
    ((UIRect) uiWidget).alpha = 0.0f;
    this.effect_ = true;
    TweenAlpha ta = TweenAlpha.Begin(((Component) this).gameObject, duration, 1f);
    ((UITweener) ta).SetOnFinished((EventDelegate.Callback) (() =>
    {
      Object.Destroy((Object) ta);
      Object.Destroy((Object) uiWidget);
      ((Behaviour) this.Button).enabled = true;
      this.effect_ = false;
    }));
  }
}
