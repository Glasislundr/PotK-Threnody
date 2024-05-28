// Decompiled with JetBrains decompiler
// Type: Help0156Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using System.Collections.Generic;

#nullable disable
public class Help0156Scene : NGSceneBase
{
  public Help0156Menu menu;

  public static void ChangeScene(bool stack, List<BeginnerNaviTitle> bnTitles)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("help_015_6", (stack ? 1 : 0) != 0, (object) bnTitles);
  }

  public static void ChangeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("help_015_6", stack);
  }

  public IEnumerator onStartSceneAsync(List<BeginnerNaviTitle> bnTitles)
  {
    IEnumerator e = this.menu.InitBeginnerNaviTitle(bnTitles);
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
