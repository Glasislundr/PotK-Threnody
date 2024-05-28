// Decompiled with JetBrains decompiler
// Type: Tower029ManualScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Tower029ManualScene : NGSceneBase
{
  [SerializeField]
  private Tower029ManualMenu menu;

  public static void ChangeScene(bool isStack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("tower029_manual", isStack);
  }

  public IEnumerator onStartSceneAsync()
  {
    Tower029ManualScene tower029ManualScene = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) null;
    IEnumerator e = tower029ManualScene.menu.InitializeAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    tower029ManualScene.bgmFile = TowerUtil.BgmFile;
    tower029ManualScene.bgmName = TowerUtil.BgmName;
  }

  public void onStartScene() => Singleton<CommonRoot>.GetInstance().isLoading = false;

  public override void onEndScene()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    DetailController.Release();
    Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
  }
}
