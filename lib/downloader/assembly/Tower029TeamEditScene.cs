// Decompiled with JetBrains decompiler
// Type: Tower029TeamEditScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Tower029TeamEditScene : NGSceneBase
{
  [SerializeField]
  private Tower029TeamEditMenu menu;

  public static void ChangeScene(TowerProgress progress)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("tower029_team_edit", true, (object) progress);
  }

  public IEnumerator onStartSceneAsync(TowerProgress progress)
  {
    Tower029TeamEditScene tower029TeamEditScene = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) null;
    IEnumerator e = tower029TeamEditScene.menu.InitializeAsync(progress);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    tower029TeamEditScene.bgmFile = TowerUtil.BgmFile;
    tower029TeamEditScene.bgmName = TowerUtil.BgmName;
  }

  public void onStartScene(TowerProgress progress)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public override void onEndScene() => Singleton<CommonRoot>.GetInstance().isLoading = true;
}
