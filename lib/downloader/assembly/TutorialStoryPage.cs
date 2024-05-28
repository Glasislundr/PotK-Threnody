// Decompiled with JetBrains decompiler
// Type: TutorialStoryPage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class TutorialStoryPage : TutorialPageBase
{
  [SerializeField]
  private int scriptId;

  public override IEnumerator Show()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    TutorialStoryPage tutorialStoryPage = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated method
    tutorialStoryPage.StartCoroutine(tutorialStoryPage.\u003C\u003En__0());
    Story0093Scene.changeScene009_3(false, tutorialStoryPage.scriptId);
    tutorialStoryPage.StartCoroutine(tutorialStoryPage.observeStoryEnd());
    return false;
  }

  private IEnumerator observeStoryEnd()
  {
    TutorialStoryPage tutorialStoryPage = this;
    while (Singleton<NGSceneManager>.GetInstance().changeSceneQueueCount > 0)
      yield return (object) null;
    while (!EmptyScene.IsActive)
      yield return (object) null;
    tutorialStoryPage.NextPage();
  }
}
