// Decompiled with JetBrains decompiler
// Type: DailyMission0271Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class DailyMission0271Scene : NGSceneBase
{
  [SerializeField]
  private DailyMission0271Menu menu;

  public static void ChangeScene0271(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("dailymission027_1", stack);
  }

  public IEnumerator onStartSceneAsync()
  {
    Debug.Log((object) "[daily mission] onStartSceneAsync");
    IEnumerator e = this.menu.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }
}
