// Decompiled with JetBrains decompiler
// Type: Explore033DeckEditScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;

#nullable disable
public class Explore033DeckEditScene : NGSceneBase
{
  private Explore033DeckEditMenu menu;

  public static void ChangeSceneExploreDeckEdit()
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("explore033_DeckEdit", true, (object) Explore033DeckEditScene.Mode.Explore);
  }

  public static void ChangeSceneChallengeDeckEdit()
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("explore033_DeckEdit", true, (object) Explore033DeckEditScene.Mode.Challenge);
  }

  public override IEnumerator onInitSceneAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Explore033DeckEditScene explore033DeckEditScene = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    explore033DeckEditScene.menu = explore033DeckEditScene.menuBase as Explore033DeckEditMenu;
    return false;
  }

  public IEnumerator onStartSceneAsync(Explore033DeckEditScene.Mode mode)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) this.menu.InitializeAsync(mode);
  }

  public void onStartScene(Explore033DeckEditScene.Mode mode)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public IEnumerator onBackSceneAsync(Explore033DeckEditScene.Mode mode)
  {
    CommonRoot instance = Singleton<CommonRoot>.GetInstance();
    instance.isLoading = true;
    instance.setStartScene("mypage");
    yield return (object) this.menu.onBackSceneAsync(mode);
  }

  public void onBackScene(Explore033DeckEditScene.Mode mode)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public override void onEndScene()
  {
  }

  public enum Mode
  {
    Explore,
    Challenge,
  }
}
