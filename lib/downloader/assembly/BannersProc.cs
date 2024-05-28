// Decompiled with JetBrains decompiler
// Type: BannersProc
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
public class BannersProc : MonoBehaviour
{
  [SerializeField]
  private NGWrapScrollParts Scroll;
  [SerializeField]
  private TweenAlpha StartAlpha;
  [SerializeField]
  private int margin;
  private List<BannerSetting> BannerButtons = new List<BannerSetting>();
  private int index;
  private int loadIndex;
  private DateTime serverTime;
  private List<Banner> banners;
  private GameObject prefab;
  private bool autoScrollFlag;
  private UIScrollView ScrollView;
  private int indexCount;
  private int count;
  private float delay = 3f;
  private bool isArrowBtn = true;
  private bool prevDragging;
  private bool isScrolling;
  private Action pushCallback;
  private bool isStack = true;
  private bool isEffectStop;
  private bool isLoadStarted;

  private void StopEffectAnimeAll()
  {
    foreach (GameObject contentChild in this.Scroll.GetContentChildren())
    {
      if (contentChild.activeInHierarchy)
        contentChild.SendMessage("StopAnime");
    }
  }

  public void IbtnLeftArrow()
  {
    if (!this.isArrowBtn)
      return;
    this.isArrowBtn = false;
    this.isScrolling = true;
    this.StopEffectAnimeAll();
    this.count = this.Scroll.selected - 1;
    if (this.count < 0)
      this.count = this.Scroll.GetContentChildren().Count<GameObject>() - 1;
    // ISSUE: method pointer
    this.Scroll.centerOnChild.onFinished = new SpringPanel.OnFinished((object) this, __methodptr(\u003CIbtnLeftArrow\u003Eb__21_0));
    this.Scroll.setItemPosition(this.count);
    this.delay = (float) this.banners[this.count].duration_seconds;
  }

  public void IbtnRightArrow()
  {
    if (!this.isArrowBtn)
      return;
    this.isArrowBtn = false;
    this.isScrolling = true;
    this.StopEffectAnimeAll();
    this.count = this.Scroll.selected + 1;
    if (this.count >= this.Scroll.GetContentChildren().Count<GameObject>())
      this.count = 0;
    // ISSUE: method pointer
    this.Scroll.centerOnChild.onFinished = new SpringPanel.OnFinished((object) this, __methodptr(\u003CIbtnRightArrow\u003Eb__22_0));
    this.Scroll.setItemPosition(this.count);
    this.delay = (float) this.banners[this.count].duration_seconds;
  }

  private void Awake()
  {
    this.Scroll.content.itemSize += this.margin;
    this.ScrollView = this.Scroll.scrollView.GetComponent<UIScrollView>();
  }

  private void Update()
  {
    if (!this.autoScrollFlag && !this.isScrolling)
      return;
    if (this.isEffectStop)
    {
      if (!Singleton<PopupManager>.GetInstance().isOpen && !Singleton<CommonRoot>.GetInstance().isSeaGlobalMenuOpen)
      {
        this.Scroll.GetContentChild(this.count).SendMessage("StartAnime");
        this.isEffectStop = false;
      }
    }
    else if (Singleton<PopupManager>.GetInstance().isOpen || Singleton<CommonRoot>.GetInstance().isSeaGlobalMenuOpen)
    {
      this.StopEffectAnimeAll();
      this.isEffectStop = true;
    }
    this.delay -= Time.deltaTime;
    if (this.prevDragging != this.ScrollView.isDragging)
    {
      if (this.ScrollView.isDragging)
      {
        this.StopEffectAnimeAll();
      }
      else
      {
        this.isArrowBtn = false;
        this.isScrolling = true;
        // ISSUE: method pointer
        this.Scroll.centerOnChild.onFinished = new SpringPanel.OnFinished((object) this, __methodptr(\u003CUpdate\u003Eb__24_0));
      }
      this.prevDragging = this.ScrollView.isDragging;
    }
    if (this.count != this.Scroll.selected && this.Scroll.selected > -1)
    {
      this.count = this.Scroll.selected >= this.banners.Count<Banner>() ? 0 : this.Scroll.selected;
      this.delay = (float) this.banners[this.count].duration_seconds;
    }
    else
    {
      if ((double) this.delay >= 0.0)
        return;
      this.count = this.Scroll.selected + 1;
      if (this.count >= this.Scroll.GetContentChildren().Count<GameObject>() || this.count < 0)
        this.count = 0;
      this.isArrowBtn = false;
      this.isScrolling = true;
      this.StopEffectAnimeAll();
      // ISSUE: method pointer
      this.Scroll.centerOnChild.onFinished = new SpringPanel.OnFinished((object) this, __methodptr(\u003CUpdate\u003Eb__24_1));
      this.Scroll.setItemPosition(this.count);
      this.delay = (float) this.banners[this.count].duration_seconds;
    }
  }

  private void CreateBannerList()
  {
    this.indexCount = 0;
    this.autoScrollFlag = false;
    this.banners = new List<Banner>();
    Banner[] source1 = SMManager.Get<Banner[]>();
    if (source1 == null)
      return;
    List<Banner> list1 = ((IEnumerable<Banner>) source1).ToList<Banner>();
    List<Banner> list2 = list1.Where<Banner>((Func<Banner, bool>) (x => x.priority != 0)).OrderBy<Banner, int>((Func<Banner, int>) (y => y.priority)).ToList<Banner>();
    List<Banner> list3 = list1.Where<Banner>((Func<Banner, bool>) (x => x.priority == 0)).ToList<Banner>();
    List<Banner> source2 = list2;
    source2.AddRange((IEnumerable<Banner>) list3);
    try
    {
      this.banners = source2.Where<Banner>((Func<Banner, bool>) (x => BannerSetting.judgeTime(x, this.serverTime) && BannerSetting.IsExistSpritePath(x))).ToList<Banner>();
    }
    catch (Exception ex)
    {
      this.banners.Clear();
    }
    UIScrollView component = this.Scroll.scrollView.GetComponent<UIScrollView>();
    if (Object.op_Inequality((Object) component, (Object) null))
      ((Behaviour) component).enabled = false;
    source2.Clear();
    list1.Clear();
    list2.Clear();
    list3.Clear();
  }

  private IEnumerator LoadBanner(int index)
  {
    BannersProc parent = this;
    GameObject button = parent.Scroll.instantiateParts(parent.prefab);
    button.transform.localPosition = new Vector3((float) (button.GetComponent<UIWidget>().width * parent.indexCount + parent.margin * parent.indexCount), 0.0f, 0.0f);
    button.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
    BannerSetting setting = button.GetComponent<BannerSetting>();
    IEnumerator e = setting.Init(parent.banners[index], parent, parent.serverTime, parent.pushCallback, parent.isStack);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (setting.DestroyButton)
      Object.Destroy((Object) button);
    else
      ++parent.indexCount;
    parent.BannerButtons.Add(setting);
  }

  private IEnumerator LoadBannerAll()
  {
    yield return (object) null;
    for (int idx = this.loadIndex; idx < this.banners.Count<Banner>(); ++idx)
    {
      IEnumerator e = this.LoadBanner(idx);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      yield return (object) new WaitForEndOfFrame();
    }
    yield return (object) null;
    this.Scroll.content.SortBasedOnScrollMovement();
    this.Scroll.content.WrapContent();
    this.Scroll.ResetPosition();
    if (this.banners.Count<Banner>() > 1)
    {
      this.Scroll.ForceArrowDisplay(true);
      UIScrollView component = this.Scroll.scrollView.GetComponent<UIScrollView>();
      if (Object.op_Inequality((Object) component, (Object) null))
        ((Behaviour) component).enabled = true;
      this.autoScrollFlag = true;
    }
  }

  public IEnumerator BannerCreate(bool isCloudAnim, Action callback = null, bool stack = true)
  {
    BannersProc bannersProc = this;
    bannersProc.index = 0;
    bannersProc.loadIndex = 0;
    bannersProc.count = 0;
    bannersProc.autoScrollFlag = false;
    bannersProc.isArrowBtn = true;
    bannersProc.pushCallback = callback;
    bannersProc.isStack = stack;
    bannersProc.BannerButtons.Clear();
    ((Component) bannersProc).GetComponent<NGWrapScrollParts>().destroyParts();
    bannersProc.Scroll.content.SortBasedOnScrollMovement();
    bannersProc.Scroll.content.WrapContent();
    IEnumerator e;
    if (Object.op_Equality((Object) bannersProc.prefab, (Object) null))
    {
      Future<GameObject> prefabf = Res.Prefabs.mypage.Banner.Load<GameObject>();
      e = prefabf.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      bannersProc.prefab = prefabf.Result;
      prefabf = (Future<GameObject>) null;
    }
    e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    bannersProc.serverTime = ServerTime.NowAppTime();
    bannersProc.Scroll.centerOnChild.onFinished = (SpringPanel.OnFinished) null;
    bannersProc.CreateBannerList();
    if (bannersProc.banners.Count > 0)
    {
      if (Persist.integralNoaTutorial.Data.beginnersQuest)
      {
        bannersProc.StopScroll();
        int index = bannersProc.banners.FindIndex((Predicate<Banner>) (x => x.id == 887));
        if (index == -1)
          index = 0;
        e = bannersProc.LoadBanner(index);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        bannersProc.isLoadStarted = true;
      }
      else
      {
        e = bannersProc.LoadBanner(0);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        ++bannersProc.loadIndex;
        bannersProc.delay = (float) bannersProc.banners[bannersProc.count].duration_seconds;
        bannersProc.isLoadStarted = false;
        if (!isCloudAnim)
        {
          bannersProc.isLoadStarted = true;
          bannersProc.StartCoroutine(bannersProc.LoadBannerAll());
        }
      }
    }
  }

  public void StartLoadBannerAll()
  {
    if (this.isLoadStarted)
      return;
    this.isLoadStarted = true;
    this.StartCoroutine(this.LoadBannerAll());
  }

  public void LoopBannerNext()
  {
    this.BannerButtons[this.index].StartTween();
    this.index = this.BannerButtons.Count - 1 <= this.index ? 0 : this.index + 1;
  }

  public void StopScroll()
  {
    this.autoScrollFlag = false;
    this.StopEffectAnimeAll();
  }

  public void StartScroll()
  {
    this.autoScrollFlag = false;
    this.Scroll.setItemPosition(0);
    this.count = 0;
    this.delay = (float) this.banners[this.count].duration_seconds;
    this.autoScrollFlag = true;
    this.isScrolling = false;
    this.Scroll.GetContentChild(this.count).SendMessage("StartAnime");
  }

  public void StopEffect()
  {
    if (this.BannerButtons == null || this.BannerButtons.Count <= this.count || Object.op_Equality((Object) this.BannerButtons[this.count], (Object) null))
      return;
    this.BannerButtons[this.count].setEmphasisEffectVisibility(false);
  }

  public void Clear() => this.Scroll.destroyParts();

  public BannerSetting GetBeginnersBanner()
  {
    int? nullable = this.BannerButtons.FirstIndexOrNull<BannerSetting>((Func<BannerSetting, bool>) (x => x.ID == 887));
    return !nullable.HasValue ? this.BannerButtons[0] : this.BannerButtons[nullable.Value];
  }
}
