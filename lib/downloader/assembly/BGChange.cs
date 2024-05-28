// Decompiled with JetBrains decompiler
// Type: BGChange
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
public class BGChange : MonoBehaviour
{
  private const int XL_BG_HEIGHT = 1136;
  private const int XL_BG_WIDTH = 720;
  private const int XL_BG_WIDTH_WIDE = 1044;
  [SerializeField]
  private GameObject current;
  public bool NotTween;

  public GameObject Current => this.current;

  public IEnumerator asyncBgAnim(QuestBG.AnimApply set, float duration = 0.0f)
  {
    Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>().Toggle(set, duration);
    yield return (object) null;
  }

  public IEnumerator QuestBGprefabCreate(int L, int M = 0, bool Hard = false, int XL = 1, bool mustCreate = false)
  {
    IEnumerator e = this.BGprefabCreate(mustCreate, L, M, XL);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Hard)
      this.BlackHangingBackGround(Hard, true);
    if (M != 0)
      this.ChangeCurrentBGprefab(M, XL, Hard);
  }

  public IEnumerator ExtraBGprefabCreate(string name)
  {
    Future<GameObject> prefabF = Res.Prefabs.Quest.Extra.BG.L.prefab.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().setBackground(prefabF.Result);
    string path = "Prefabs/BackGround/" + name;
    if (string.IsNullOrEmpty(name) || !Singleton<ResourceManager>.GetInstance().Contains(path))
      path = "Prefabs/BackGround/event_default";
    Future<Sprite> BGdata = Singleton<ResourceManager>.GetInstance().Load<Sprite>(path);
    e = BGdata.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>().current_xl.GetComponent<UI2DSprite>().sprite2D = BGdata.Result;
  }

  public void ChangeCurrentBGprefab(int M, int XL, bool Hard = false)
  {
    QuestBG backgroundComponent = Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>();
    backgroundComponent.Current = backgroundComponent.current_l;
    backgroundComponent.changeActive(false);
    this.M_BGAnimation(M, true, Hard);
  }

  public IEnumerator BGprefabCreate(bool mustCreate, int L, int M = 0, int XL = 1)
  {
    Future<GameObject> BGdata = Singleton<ResourceManager>.GetInstance().Load<GameObject>(string.Format("Prefabs/Quest/Story/BG/L/{0}/prefab", (object) L));
    IEnumerator e = BGdata.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject BGprefab = BGdata.Result;
    if (this.ComparisonBackground(BGprefab) || mustCreate)
    {
      e = this.SetXLBg(BGprefab.GetComponent<QuestBG>());
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().setBackground(BGprefab);
      QuestBG backgroundComponent = Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>();
      backgroundComponent.Current = backgroundComponent.current_xl;
    }
  }

  public IEnumerator MypageBGprefabCreate()
  {
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    DateTime server_time = ServerTime.NowAppTime();
    CommonMypageSetting commonMypageSetting = ((IEnumerable<CommonMypageSetting>) MasterData.CommonMypageSettingList).FirstOrDefault<CommonMypageSetting>((Func<CommonMypageSetting, bool>) (x =>
    {
      DateTime dateTime1 = server_time;
      DateTime? startAt = x.start_at;
      if ((startAt.HasValue ? (dateTime1 >= startAt.GetValueOrDefault() ? 1 : 0) : 0) == 0)
        return false;
      DateTime dateTime2 = server_time;
      DateTime? endAt = x.end_at;
      return endAt.HasValue && dateTime2 < endAt.GetValueOrDefault();
    }));
    Future<GameObject> BGdata = new ResourceObject(string.Format("Prefabs/BackGround_anim/{0}/background_anim", commonMypageSetting == null ? (object) "mypage_fairy" : (object) commonMypageSetting.background)).Load<GameObject>();
    e = BGdata.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = BGdata.Result;
    Singleton<CommonRoot>.GetInstance().setBackground(result);
  }

  public IEnumerator BGprefabCreateForQuest0025(int L, Quest00240723Menu.StoryMode storymode)
  {
    Future<GameObject> BGdata = Singleton<ResourceManager>.GetInstance().Load<GameObject>(string.Format("Prefabs/Quest/Story/BG/L/{0}/prefab", (object) L));
    IEnumerator e = BGdata.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = BGdata.Result;
    Singleton<CommonRoot>.GetInstance().setBackground(result);
    e = this.SetChapterBg(storymode);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    QuestBG backgroundComponent = Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>();
    backgroundComponent.Current = backgroundComponent.current_xl;
  }

  public IEnumerator SetXLBg(QuestBG _questBg = null)
  {
    int wid = 800;
    int hei = 1136;
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    CommonMypageSetting commonMypageSetting = ((IEnumerable<CommonMypageSetting>) MasterData.CommonMypageSettingList).FirstOrDefault<CommonMypageSetting>((Func<CommonMypageSetting, bool>) (x =>
    {
      DateTime dateTime1 = ServerTime.NowAppTime();
      DateTime? nullable = x.start_at;
      if ((nullable.HasValue ? (dateTime1 >= nullable.GetValueOrDefault() ? 1 : 0) : 0) == 0)
        return false;
      DateTime dateTime2 = ServerTime.NowAppTime();
      nullable = x.end_at;
      return nullable.HasValue && dateTime2 < nullable.GetValueOrDefault();
    }));
    QuestBG questBg = Object.op_Equality((Object) _questBg, (Object) null) ? Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>() : _questBg;
    questBg.currentPos = QuestBG.QuestPosition.Chapter;
    string path = string.Format("Prefabs/BackGround_anim/{0}/Material/bg_base", (object) commonMypageSetting.background);
    Future<Sprite> bgxlF = Singleton<ResourceManager>.GetInstance().Load<Sprite>(path);
    e = bgxlF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = bgxlF.Result;
    UI2DSprite component = questBg.current_xl.GetComponent<UI2DSprite>();
    component.sprite2D = result;
    ((UIWidget) component).SetDimensions(wid, hei);
  }

  public IEnumerator SetChapterBg(Quest00240723Menu.StoryMode storyMode)
  {
    IEnumerator e;
    if (MasterData.QuestCommonChapterBGList.Length == 0)
    {
      e = this.SetXLBg();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      QuestBG questBg = Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>();
      string path;
      switch (storyMode)
      {
        case Quest00240723Menu.StoryMode.LostRagnarok:
          path = Consts.GetInstance().BACKGROUND_BASE_PATH.F((object) MasterData.QuestCommonChapterBGList[0].bg_lost_ragnarok);
          break;
        case Quest00240723Menu.StoryMode.IntegralNoah:
          path = Consts.GetInstance().BACKGROUND_BASE_PATH.F((object) "IntegralNoah_ChapterSelect");
          break;
        default:
          path = Consts.GetInstance().BACKGROUND_BASE_PATH.F((object) MasterData.QuestCommonChapterBGList[0].bg_heaven);
          break;
      }
      Future<Sprite> bgxlF = Singleton<ResourceManager>.GetInstance().Load<Sprite>(path);
      e = bgxlF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Sprite result = bgxlF.Result;
      UI2DSprite component = questBg.current_xl.GetComponent<UI2DSprite>();
      component.sprite2D = result;
      ((UIWidget) component).SetDimensions(720, 1136);
    }
  }

  public IEnumerator EarthBGprefabCreate(string name)
  {
    Future<GameObject> prefabF = Res.Prefabs.Quest.Extra.BG.L.prefab.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().setBackground(prefabF.Result);
    string path = Consts.GetInstance().BACKGROUND_BASE_PATH.F((object) name);
    Future<Sprite> bgSpriteF = Singleton<ResourceManager>.GetInstance().LoadOrNull<Sprite>(path);
    e = bgSpriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) bgSpriteF.Result, (Object) null))
    {
      QuestBG backgroundComponent = Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>();
      Sprite result = bgSpriteF.Result;
      backgroundComponent.current_xl.GetComponent<UI2DSprite>().sprite2D = result;
      UIWidget component = backgroundComponent.current_xl.GetComponent<UIWidget>();
      Rect rect1 = result.rect;
      int width = (int) ((Rect) ref rect1).width;
      Rect rect2 = result.rect;
      int height = (int) ((Rect) ref rect2).height;
      component.SetDimensions(width, height);
    }
  }

  public void CrossToL(bool toHome = false, float fade_duration = 1f)
  {
    this.getCurrentBG();
    QuestBG backgroundComponent = Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>();
    if (!Object.op_Inequality((Object) backgroundComponent, (Object) null) || !Object.op_Equality((Object) this.current, (Object) backgroundComponent.current_xl))
      return;
    this.CrossFadeBG(toHome, fade_duration);
  }

  public void CrossToXL(bool toHome = false, float fade_duration = 1f)
  {
    this.getCurrentBG();
    QuestBG backgroundComponent = Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>();
    if (!Object.op_Inequality((Object) backgroundComponent, (Object) null) || !Object.op_Equality((Object) this.current, (Object) backgroundComponent.current_l))
      return;
    this.CrossFadeBG(toHome, fade_duration);
  }

  public void CrossFadeBG(bool toHome, float fade_duration = 1f)
  {
    this.getCurrentBG();
    QuestBG backgroundComponent = Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>();
    UI2DSprite component = this.current.GetComponent<UI2DSprite>();
    UI2DSprite currentNext = Object.op_Inequality((Object) this.current, (Object) backgroundComponent.current_xl) ? backgroundComponent.current_xl.GetComponent<UI2DSprite>() : backgroundComponent.current_l.GetComponent<UI2DSprite>();
    ((Component) component).gameObject.SetActive(true);
    ((Component) currentNext).gameObject.SetActive(true);
    ((UIWidget) component).depth = 10;
    ((UIWidget) currentNext).depth = 1;
    ((UIRect) ((Component) component).GetComponent<UI2DSprite>()).alpha = 1f;
    ((UIRect) ((Component) currentNext).GetComponent<UI2DSprite>()).alpha = 1f;
    backgroundComponent.Current = ((Component) currentNext).gameObject;
    if ((double) fade_duration == 0.0)
    {
      ((UIRect) ((Component) component).GetComponent<UI2DSprite>()).alpha = 0.0f;
      ((UIWidget) currentNext).depth = 10;
      if (!toHome)
        return;
      this.StartCoroutine(this.CheckStageBG());
    }
    else
    {
      TweenAlpha cross = ((Component) component).gameObject.GetOrAddComponent<TweenAlpha>();
      ((UITweener) cross).delay = 0.0f;
      ((UITweener) cross).duration = fade_duration;
      cross.to = 0.0f;
      cross.from = 1f;
      ((UITweener) cross).onFinished.Clear();
      EventDelegate.Set(((UITweener) cross).onFinished, (EventDelegate.Callback) (() =>
      {
        cross.value = 1f;
        ((UIWidget) currentNext).depth = 10;
        if (!toHome)
          return;
        this.StartCoroutine(this.CheckStageBG());
      }));
      ((UITweener) cross).ResetToBeginning();
      ((UITweener) cross).PlayForward();
    }
  }

  public void getCurrentBG()
  {
    if (Singleton<CommonRoot>.GetInstance().hasBackground())
    {
      this.current = (GameObject) null;
    }
    else
    {
      QuestBG backgroundComponent = Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>();
      this.current = Object.op_Equality((Object) backgroundComponent, (Object) null) ? (GameObject) null : backgroundComponent.Current;
    }
  }

  public bool ComparisonBackground(GameObject prefab)
  {
    this.getCurrentBG();
    return Object.op_Equality((Object) this.current, (Object) null) || Object.op_Equality((Object) ((Component) this.current.transform.parent).GetComponent<QuestBG>(), (Object) null) || !(((Component) this.current.transform.parent).GetComponent<QuestBG>().namePrefab == prefab.GetComponent<QuestBG>().namePrefab);
  }

  public void M_BGAnimation(int clickNum, bool quick = false, bool Hard = false)
  {
    bool TweenToS = true;
    if (quick)
      this.NotTween = true;
    QuestBG backgroundComponent = Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>();
    switch (clickNum)
    {
      case 1:
        if (quick)
        {
          backgroundComponent.Toggle(QuestBG.AnimApply.Button1, 0.0f);
          break;
        }
        backgroundComponent.Toggle(QuestBG.AnimApply.Button1);
        break;
      case 2:
        if (quick)
        {
          backgroundComponent.Toggle(QuestBG.AnimApply.Button2, 0.0f);
          break;
        }
        backgroundComponent.Toggle(QuestBG.AnimApply.Button2);
        break;
      case 3:
        if (quick)
        {
          backgroundComponent.Toggle(QuestBG.AnimApply.Button3, 0.0f);
          break;
        }
        backgroundComponent.Toggle(QuestBG.AnimApply.Button3);
        break;
      case 4:
        if (quick)
        {
          backgroundComponent.Toggle(QuestBG.AnimApply.Button4, 0.0f);
          break;
        }
        backgroundComponent.Toggle(QuestBG.AnimApply.Button4);
        break;
      case 5:
        if (quick)
        {
          backgroundComponent.Toggle(QuestBG.AnimApply.Button5, 0.0f);
          break;
        }
        backgroundComponent.Toggle(QuestBG.AnimApply.Button5);
        break;
      case 10:
        if (quick)
          backgroundComponent.Toggle(QuestBG.AnimApply.LMainPostion, 0.0f);
        else
          backgroundComponent.Toggle(QuestBG.AnimApply.LMainPostion);
        TweenToS = false;
        break;
    }
    if (Hard)
      return;
    backgroundComponent.ClickTweenV(TweenToS, quick);
  }

  public void BlackHangingBackGround(bool toHard, bool quick = false)
  {
    this.getCurrentBG();
    if (Object.op_Equality((Object) this.Current, (Object) null))
      Debug.LogWarning((object) "Current BackGround == null !!");
    else
      Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>().Darken(toHard, quick);
  }

  public IEnumerator CheckStageBG()
  {
    BGChange bgChange = this;
    int L = Persist.lastsortie.Data.l_id;
    QuestBG backgroundComponent = Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>();
    if (Object.op_Inequality((Object) backgroundComponent, (Object) null) && backgroundComponent.DiffStageLId)
    {
      if (L == 0)
        L = 1;
      IEnumerator e = ((Component) bgChange).GetComponent<BGChange>().BGprefabCreate(true, L);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }
}
