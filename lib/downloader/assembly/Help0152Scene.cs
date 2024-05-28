// Decompiled with JetBrains decompiler
// Type: Help0152Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using System.Collections.Generic;

#nullable disable
public class Help0152Scene : NGSceneBase
{
  public Help0152Menu menu;
  private static readonly string DEFAULT_SCENE = "help015_2";

  public static void ChangeScene(bool stack, List<HelpHelp> helps)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(Help0152Scene.DEFAULT_SCENE, (stack ? 1 : 0) != 0, (object) helps);
  }

  public static void ChangeScene(bool stack, HelpCategory helpCategory)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(Help0152Scene.DEFAULT_SCENE, (stack ? 1 : 0) != 0, (object) helpCategory);
  }

  public override IEnumerator onInitSceneAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Help0152Scene help0152Scene = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (Singleton<NGGameDataManager>.GetInstance().IsEarth)
      help0152Scene.headerType = CommonRoot.HeaderType.Keep;
    return false;
  }

  public IEnumerator onStartSceneAsync(HelpCategory helpCategory)
  {
    yield return (object) this.onStartSceneAsync(Help0151Button.getHelpList(helpCategory));
  }

  public IEnumerator onStartSceneAsync(List<HelpHelp> helps)
  {
    Help0152Scene help0152Scene = this;
    if (Singleton<NGGameDataManager>.GetInstance().IsEarth)
      help0152Scene.bgmName = "bgm104";
    IEnumerator e = help0152Scene.menu.InitSubCatecory(helps);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Singleton<CommonRoot>.GetInstance().isLoading)
    {
      yield return (object) null;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
    }
  }
}
