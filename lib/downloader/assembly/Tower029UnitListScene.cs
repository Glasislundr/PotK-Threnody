// Decompiled with JetBrains decompiler
// Type: Tower029UnitListScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Tower029UnitListScene : NGSceneBase
{
  [SerializeField]
  private Tower029UnitListMenu menu;

  public static void ChangeScene()
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("tower029_unit_list", true);
  }

  public IEnumerator onStartSceneAsync()
  {
    Tower029UnitListScene tower029UnitListScene = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) null;
    IEnumerator e = tower029UnitListScene.menu.InitializeAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    tower029UnitListScene.bgmFile = TowerUtil.BgmFile;
    tower029UnitListScene.bgmName = TowerUtil.BgmName;
  }

  public void onStartScene() => Singleton<CommonRoot>.GetInstance().isLoading = false;

  public override void onEndScene() => Singleton<CommonRoot>.GetInstance().isLoading = true;
}
