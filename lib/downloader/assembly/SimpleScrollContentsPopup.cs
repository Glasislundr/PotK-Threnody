// Decompiled with JetBrains decompiler
// Type: SimpleScrollContentsPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using UniLinq;
using UnityEngine;

#nullable disable
public class SimpleScrollContentsPopup : BackButtonMenuBase
{
  [SerializeField]
  private NGHorizontalScrollParts mScrollParts;
  [SerializeField]
  private UIButton mCloseButton;
  [SerializeField]
  private bool mAutoScrollEnabled;
  [SerializeField]
  private float mAutoScrollTime = 5f;
  private GameObject[] mContents;
  private int mCurrentPageIndex;
  private int mPgaeIndexMax;
  private float mAutoScrollDeltaTime;
  private Action mOnCloseCallback;

  public IEnumerator Initialize(Action onCloseCallback)
  {
    this.mOnCloseCallback = onCloseCallback;
    this.mContents = this.mScrollParts.GridChildren().ToArray<GameObject>();
    this.mPgaeIndexMax = this.mContents.Length - 1;
    this.mCurrentPageIndex = this.mScrollParts.selected;
    this.UpdateCloseButton();
    GameObject[] gameObjectArray = this.mContents;
    for (int index = 0; index < gameObjectArray.Length; ++index)
    {
      GameObject gameObject = gameObjectArray[index];
      ISimpleScrollContent componentInChildren = gameObject.GetComponentInChildren<ISimpleScrollContent>();
      if (componentInChildren == null)
      {
        Debug.LogWarning((object) string.Format("content {0} is not SimpleScrollContent", (object) ((Object) gameObject).name));
      }
      else
      {
        IEnumerator e = componentInChildren.Load();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    gameObjectArray = (GameObject[]) null;
    this.mAutoScrollEnabled &= this.mContents.Length > 1;
  }

  public void onItemChanged(int index)
  {
    this.mCurrentPageIndex = index;
    this.UpdateCloseButton();
    this.IsPush = false;
  }

  public void IncreasePageIndex()
  {
    this.mAutoScrollDeltaTime = 0.0f;
    if (this.IsPushAndSet())
      return;
    int currentPageIndex1 = this.mCurrentPageIndex;
    if (++this.mCurrentPageIndex > this.mPgaeIndexMax)
      this.mCurrentPageIndex = this.mPgaeIndexMax;
    int currentPageIndex2 = this.mCurrentPageIndex;
    if (currentPageIndex1 == currentPageIndex2)
      this.IsPush = false;
    else
      this.mScrollParts.setItemPosition(this.mCurrentPageIndex);
  }

  public void DecreasePageIndex()
  {
    this.mAutoScrollDeltaTime = 0.0f;
    if (this.IsPushAndSet())
      return;
    int currentPageIndex1 = this.mCurrentPageIndex;
    if (--this.mCurrentPageIndex < 0)
      this.mCurrentPageIndex = 0;
    int currentPageIndex2 = this.mCurrentPageIndex;
    if (currentPageIndex1 == currentPageIndex2)
      this.IsPush = false;
    else
      this.mScrollParts.setItemPosition(this.mCurrentPageIndex);
  }

  private void UpdateCloseButton()
  {
    ((UIButtonColor) this.mCloseButton).isEnabled = this.mCurrentPageIndex == this.mPgaeIndexMax;
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.mAutoScrollEnabled = false;
    if (this.mOnCloseCallback != null)
      this.mOnCloseCallback();
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  protected override void Update()
  {
    base.Update();
    if (!this.mAutoScrollEnabled)
      return;
    this.mAutoScrollDeltaTime += Time.deltaTime;
    if ((double) this.mAutoScrollDeltaTime <= (double) this.mAutoScrollTime)
      return;
    this.IncreasePageIndex();
  }
}
