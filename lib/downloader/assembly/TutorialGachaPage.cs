// Decompiled with JetBrains decompiler
// Type: TutorialGachaPage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;

#nullable disable
public class TutorialGachaPage : TutorialPageBase
{
  private const int REWARD_TYPE_UNIT = 1;

  public override IEnumerator Show()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    TutorialGachaPage tutorialGachaPage = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    tutorialGachaPage.StartCoroutine(Singleton<TutorialRoot>.GetInstance().signUpLoop(false, (Action) (() =>
    {
      Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
      Singleton<NGGameDataManager>.GetInstance().StartSceneAsyncProxy((Action<NGGameDataManager.StartSceneProxyResult>) (_ => Singleton<TutorialRoot>.GetInstance().TutorialGachaAdvice()));
    })));
    return false;
  }
}
