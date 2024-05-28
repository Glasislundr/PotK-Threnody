// Decompiled with JetBrains decompiler
// Type: PopupNotice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class PopupNotice : BackButtonMenuBase
{
  [SerializeField]
  private GameObject mFrontObject;
  [SerializeField]
  private PopupNoticeContentBase mContent;
  [SerializeField]
  private UIGrid mDotIndexAnchor;
  [SerializeField]
  private GameObject mDotIndexPrefab;
  [SerializeField]
  private bool mAutoScrollEnabled;
  [SerializeField]
  private float mAutoScrollTime = 5f;
  private OfficialInfoPopupSchema[] mContentsData;
  private SelectParts[] mDotIndexParts;
  private int mCurrentPageIndex;
  private int mPgaeIndexMax;
  private IEnumerator mContentLoader;
  private float mAutoScrollDeltaTime;

  public bool IsFinish { get; private set; }

  public OfficialInformationArticle SelectedInfo { get; private set; }

  public IEnumerator Initialize(OfficialInfoPopupSchema[] contentsData, int index)
  {
    this.mContentsData = contentsData;
    this.mPgaeIndexMax = this.mContentsData.Length - 1;
    this.mCurrentPageIndex = index > this.mPgaeIndexMax ? 0 : index;
    for (int index1 = 0; index1 < this.mContentsData.Length; ++index1)
      this.mDotIndexPrefab.Clone(((Component) this.mDotIndexAnchor).transform);
    this.mDotIndexAnchor.Reposition();
    this.mDotIndexParts = ((Component) this.mDotIndexAnchor).GetComponentsInChildren<SelectParts>();
    this.mFrontObject.SetActive(this.mContentsData.Length > 1);
    this.mAutoScrollEnabled &= this.mContentsData.Length > 1;
    IEnumerator e = this.LoadContent(this.mCurrentPageIndex);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void Refresh()
  {
    if (this.mContentLoader != null)
      Debug.LogWarning((object) "Content is Loading Now. Cancel Refresh.");
    else
      this.mContent.Refresh();
  }

  public int GetCurrentPageIndex() => this.mCurrentPageIndex;

  private IEnumerator LoadContent(int index, Action callback = null)
  {
    for (int index1 = 0; index1 < this.mDotIndexParts.Length; ++index1)
      this.mDotIndexParts[index1].setValueNonTween(index1 == index ? 1 : 0);
    IEnumerator e = this.mContent.LoadContent(this.mContentsData[index]);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mContentLoader = (IEnumerator) null;
    if (callback != null)
      callback();
  }

  private void StartLoadCurrentContent(Action callback)
  {
    if (this.mContentLoader != null)
      this.StopCoroutine(this.mContentLoader);
    this.mContentLoader = this.LoadContent(this.mCurrentPageIndex, callback);
    this.StartCoroutine(this.mContentLoader);
  }

  public void IncreasePageIndex()
  {
    this.mAutoScrollDeltaTime = 0.0f;
    if (this.IsPushAndSet())
      return;
    int currentPageIndex1 = this.mCurrentPageIndex;
    if (++this.mCurrentPageIndex > this.mPgaeIndexMax)
      this.mCurrentPageIndex = 0;
    int currentPageIndex2 = this.mCurrentPageIndex;
    if (currentPageIndex1 == currentPageIndex2)
      this.IsPush = false;
    else
      this.StartLoadCurrentContent((Action) (() => this.IsPush = false));
  }

  public void DecreasePageIndex()
  {
    this.mAutoScrollDeltaTime = 0.0f;
    if (this.IsPushAndSet())
      return;
    int currentPageIndex1 = this.mCurrentPageIndex;
    if (--this.mCurrentPageIndex < 0)
      this.mCurrentPageIndex = this.mPgaeIndexMax;
    int currentPageIndex2 = this.mCurrentPageIndex;
    if (currentPageIndex1 == currentPageIndex2)
      this.IsPush = false;
    else
      this.StartLoadCurrentContent((Action) (() => this.IsPush = false));
  }

  public void OnClickContent()
  {
    if (this.IsPushAndSet())
      return;
    this.SelectedInfo = Array.Find<OfficialInformationArticle>(Singleton<NGGameDataManager>.GetInstance().officialInfos, (Predicate<OfficialInformationArticle>) (x => x.id == this.mContentsData[this.mCurrentPageIndex].officialinfo_id));
    if (this.SelectedInfo == null)
      this.IsPush = false;
    else
      this.IsFinish = true;
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.IsFinish = true;
  }

  protected override void Update()
  {
    base.Update();
    if (!this.mAutoScrollEnabled || this.mContentLoader != null)
      return;
    this.mAutoScrollDeltaTime += Time.deltaTime;
    if ((double) this.mAutoScrollDeltaTime <= (double) this.mAutoScrollTime)
      return;
    this.IncreasePageIndex();
  }
}
