// Decompiled with JetBrains decompiler
// Type: TutorialPageBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class TutorialPageBase : MonoBehaviour
{
  public bool IsTouchBlock = true;
  private GameObject root;
  protected TutorialAdvice advice;
  protected TutorialProgress progress;

  public virtual void Init(TutorialProgress progress_, TutorialAdvice advice_, GameObject root_)
  {
    this.progress = progress_;
    this.advice = advice_;
    this.root = root_;
    ((Component) this).gameObject.SetActive(false);
  }

  public virtual void ReleaseResources()
  {
  }

  public virtual void NextPage() => this.progress.OnNextPage();

  public virtual IEnumerator Show()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    TutorialPageBase tutorialPageBase = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    tutorialPageBase.root.SetActive(true);
    tutorialPageBase.advice.Hide();
    ((Component) tutorialPageBase).gameObject.SetActive(true);
    if (tutorialPageBase.IsTouchBlock)
      tutorialPageBase.touchLock();
    return false;
  }

  public virtual void Advise() => this.NextPage();

  public virtual void Hide()
  {
    this.root.SetActive(false);
    ((Component) this).gameObject.SetActive(false);
    this.hideCommon();
    this.touchUnlock();
  }

  protected void showCommon()
  {
    Singleton<CommonRoot>.GetInstance().isActiveHeader = true;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = true;
    Singleton<CommonRoot>.GetInstance().setDisableFooterColor(true);
  }

  protected void hideCommon()
  {
    Singleton<CommonRoot>.GetInstance().isActiveHeader = false;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = false;
    Singleton<CommonRoot>.GetInstance().setDisableFooterColor(false);
  }

  private void touchLock()
  {
    CommonRoot instance = Singleton<CommonRoot>.GetInstance();
    if (!Object.op_Inequality((Object) instance, (Object) null))
      return;
    instance.isTouchBlock = true;
  }

  private void touchUnlock()
  {
    CommonRoot instance = Singleton<CommonRoot>.GetInstance();
    if (!Object.op_Inequality((Object) instance, (Object) null))
      return;
    instance.isTouchBlock = false;
  }

  public Coroutine StartCoroutine(IEnumerator e)
  {
    return Singleton<TutorialRoot>.GetInstance().StartCoroutine(e);
  }
}
