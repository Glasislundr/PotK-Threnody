// Decompiled with JetBrains decompiler
// Type: TutorialProgress
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
[Serializable]
public class TutorialProgress
{
  public Action OnNextPageCallback = (Action) (() => { });
  public Action OnFinishCallback = (Action) (() => { });
  public int CurrentPageIndex;
  public GameObject root;
  [SerializeField]
  private TutorialPageBase[] allPages = new TutorialPageBase[0];

  public TutorialProgress(TutorialPageBase[] allPages_, TutorialAdvice advice, GameObject root_)
  {
    TutorialProgress progress_ = this;
    this.root = root_;
    this.allPages = ((IEnumerable<TutorialPageBase>) allPages_).OrderBy<TutorialPageBase, string>((Func<TutorialPageBase, string>) (x => ((Object) x).name)).ToArray<TutorialPageBase>();
    ((IEnumerable<TutorialPageBase>) this.allPages).ForEach<TutorialPageBase>((Action<TutorialPageBase>) (x => x.Init(progress_, advice, progress_.root)));
  }

  public TutorialPageBase currentOrNull()
  {
    return !this.IsFinish() ? this.allPages[this.CurrentPageIndex] : (TutorialPageBase) null;
  }

  public string CurrentName()
  {
    TutorialPageBase tutorialPageBase = this.currentOrNull();
    return Object.op_Equality((Object) tutorialPageBase, (Object) null) ? "finish(" + (object) this.CurrentPageIndex + ")" : ((Object) tutorialPageBase).name;
  }

  public TutorialGachaPage GachaPage() => this.currentOrNull() as TutorialGachaPage;

  public TutorialBattlePage BattlePage() => this.currentOrNull() as TutorialBattlePage;

  public TutorialHomePage HomePage() => this.currentOrNull() as TutorialHomePage;

  public int GetTutoarialGachaPage()
  {
    for (int tutoarialGachaPage = 0; tutoarialGachaPage < this.allPages.Length; ++tutoarialGachaPage)
    {
      if (Object.op_Inequality((Object) this.allPages[tutoarialGachaPage], (Object) null) && this.allPages[tutoarialGachaPage] is TutorialGachaPage)
        return tutoarialGachaPage;
    }
    return 15;
  }

  public void ReleaseResources()
  {
    ((IEnumerable<TutorialPageBase>) this.allPages).ForEach<TutorialPageBase>((Action<TutorialPageBase>) (x => x.ReleaseResources()));
  }

  public IEnumerator Render()
  {
    if (this.allPages.Length <= this.CurrentPageIndex)
    {
      Debug.LogError((object) string.Format("allPages.Length({0}) <= CurrentPageIndex({1}). so restart tutorial.", (object) this.allPages.Length, (object) this.CurrentPageIndex));
      this.CurrentPageIndex = 0;
      Persist.tutorial.Data.SetPageIndex(0);
    }
    ((IEnumerable<TutorialPageBase>) this.allPages).ForEach<TutorialPageBase>((Action<TutorialPageBase>) (x => x.Hide()));
    Singleton<TutorialRoot>.GetInstance().StartCoroutine(this.allPages[this.CurrentPageIndex].Show());
    yield break;
  }

  public void OnNextPage()
  {
    if (this.IsFinish())
    {
      Debug.LogWarning((object) "call OnNextPage() but tutorial is finished");
    }
    else
    {
      this.allPages[this.CurrentPageIndex].Hide();
      ++this.CurrentPageIndex;
      if (this.IsFinish())
      {
        Debug.Log((object) "tutorial OnFinishPageCallback");
        this.OnFinishCallback();
      }
      else
      {
        Debug.Log((object) "tutorial OnNextPageCallback");
        this.OnNextPageCallback();
        Singleton<TutorialRoot>.GetInstance().StartCoroutine(this.allPages[this.CurrentPageIndex].Show());
      }
    }
  }

  public bool IsFinish() => this.CurrentPageIndex >= this.allPages.Length;

  public void DebugTutorialFinish()
  {
    this.CurrentPageIndex = this.allPages.Length - 1;
    Singleton<TutorialRoot>.GetInstance().StartCoroutine(this.allPages[this.CurrentPageIndex].Show());
    Persist.tutorial.Data.SetPageIndex(this.CurrentPageIndex);
    Persist.tutorial.Flush();
  }
}
