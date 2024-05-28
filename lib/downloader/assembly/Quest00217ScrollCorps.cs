// Decompiled with JetBrains decompiler
// Type: Quest00217ScrollCorps
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using LocaleTimeZone;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/quest002_17/ScrollCorps")]
public class Quest00217ScrollCorps : Quest00217Scroll
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
    DateTime serverTime1 = TimeZoneInfo.ConvertTime(serverTime, Japan.CreateTimeZone());
    this.initData = param;
    this.inside_ = ((Component) this).gameObject.activeSelf;
    this.rankingEventTerm = CampaignQuest.RankingEventTerm.normal;
    this.SetEndTime(param.corpsInfo.end_at);
    this.resetLocalTime(serverTime1);
    ((Behaviour) this.Button).enabled = true;
    if (this.initData.entryConditionID == 0)
      EventDelegate.Set(this.BtnFormation.onClick, (EventDelegate.Callback) (() => CorpsQuestTopScene.ChangeScene(this.initData.corpsInfo.ID)));
    ((Component) this.New).gameObject.SetActive(param.isNew);
    ((Component) this.Clear).gameObject.SetActive(param.isClear);
    if (Object.op_Inequality((Object) this.Highlighting, (Object) null))
    {
      this.EffectRenderer.SetTexture("_MaskTex", ((UIWidget) this.IdleSprite).mainTexture);
      this.Highlighting.SetActive(param.isHighlighting);
    }
    this.SetTime(serverTime1, this.rankingEventTerm);
    this.setQuestLock();
    this.initialized = true;
  }

  public new IEnumerator SetAndCreate_BannerSprite()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Quest00217ScrollCorps quest00217ScrollCorps = this;
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
    this.\u003C\u003E2__current = (object) quest00217ScrollCorps.SetAndCreate_BannerSprite(quest00217ScrollCorps.initData);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator SetAndCreate_BannerSprite(Quest00217Scroll.Parameter param)
  {
    Quest00217ScrollCorps quest00217ScrollCorps = this;
    string spriteIdlePath = BannerBase.GetSpriteIdlePath(param.corpsInfo.banner_id, BannerBase.Type.corps);
    IEnumerator e = quest00217ScrollCorps.SetAndCreate_BannerSprite(spriteIdlePath, quest00217ScrollCorps.IdleSprite);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
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

  protected override void setQuestLockText()
  {
    this.txtLock.SetTextLocalize(MasterData.CorpsEntryConditions[this.initData.entryConditionID].text);
  }
}
