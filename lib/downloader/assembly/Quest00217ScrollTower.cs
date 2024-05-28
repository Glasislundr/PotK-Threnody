// Decompiled with JetBrains decompiler
// Type: Quest00217ScrollTower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Quest00217ScrollTower : Quest00217Scroll
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
    this.SetEndTime(param.towerInfo.end_at);
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
    if (this.initData.entryConditionID == 0)
      EventDelegate.Set(this.BtnFormation.onClick, (EventDelegate.Callback) (() => Tower029TopScene.ChangeScene(false)));
    ((Component) this.New).gameObject.SetActive(flag1);
    ((Component) this.Clear).gameObject.SetActive(flag2);
    if (Object.op_Inequality((Object) this.Highlighting, (Object) null))
    {
      this.EffectRenderer.SetTexture("_MaskTex", ((UIWidget) this.IdleSprite).mainTexture);
      this.Highlighting.SetActive(flag3);
    }
    if (serverTime < param.towerInfo.end_at)
    {
      this.EndTime = param.towerInfo.end_at;
      this.rankingEventTerm = CampaignQuest.RankingEventTerm.normal;
    }
    else if (serverTime < param.towerInfo.final_at)
    {
      this.EndTime = param.towerInfo.final_at;
      this.rankingEventTerm = CampaignQuest.RankingEventTerm.receive;
    }
    this.SetTime(serverTime, this.rankingEventTerm);
    this.setQuestLock();
    this.initialized = true;
  }

  public new IEnumerator SetAndCreate_BannerSprite()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Quest00217ScrollTower quest00217ScrollTower = this;
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
    this.\u003C\u003E2__current = (object) quest00217ScrollTower.SetAndCreate_BannerSprite(quest00217ScrollTower.initData);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator SetAndCreate_BannerSprite(Quest00217Scroll.Parameter param)
  {
    Quest00217ScrollTower quest00217ScrollTower = this;
    TowerPeriod towerPeriod = ((IEnumerable<TowerPeriod>) MasterData.TowerPeriodList).FirstOrDefault<TowerPeriod>((Func<TowerPeriod, bool>) (x => x.ID == param.towerInfo.id));
    if (towerPeriod != null)
    {
      string path = quest00217ScrollTower.SetSpritePath(towerPeriod.banner_id);
      IEnumerator e = quest00217ScrollTower.SetAndCreate_BannerSprite(path, quest00217ScrollTower.IdleSprite);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  protected string SetSpritePath(int towerID)
  {
    return BannerBase.GetSpriteIdlePath(towerID, BannerBase.Type.tower);
  }

  protected IEnumerator SetAndCreate_BannerSprite(string path, UI2DSprite obj)
  {
    if (!Singleton<ResourceManager>.GetInstance().Contains(path))
      path = string.Format("Prefabs/Banners/ExtraQuest/M/1/Specialquest_idle");
    Future<Texture2D> future = Singleton<ResourceManager>.GetInstance().Load<Texture2D>(path);
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Texture2D result = future.Result;
    if (!Object.op_Equality((Object) result, (Object) null))
    {
      Sprite sprite = Sprite.Create(result, new Rect(0.0f, 0.0f, (float) ((Texture) result).width, (float) ((Texture) result).height), new Vector2(0.5f, 0.5f), 1f, 100U, (SpriteMeshType) 0);
      ((Object) sprite).name = ((Object) result).name;
      obj.sprite2D = sprite;
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

  protected override void setQuestLockText()
  {
    this.txtLock.SetTextLocalize(MasterData.TowerEntryConditions[this.initData.entryConditionID].text);
  }
}
