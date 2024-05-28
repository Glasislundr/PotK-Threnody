// Decompiled with JetBrains decompiler
// Type: Versus026DirWinLossRecordsScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Versus026DirWinLossRecordsScene : NGSceneBase
{
  [SerializeField]
  private Versus026DirWinLossRecordsMenu menu;

  public static void ChangeScene(bool stack, Versus02613Scene.BootParam param)
  {
    param.push("versus026_win_loss_records");
    Singleton<NGSceneManager>.GetInstance().changeScene(param.current.scene, (stack ? 1 : 0) != 0, (object) param);
  }

  public IEnumerator onStartSceneAsync(Versus02613Scene.BootParam param)
  {
    IEnumerator e = this.menu.Init(param);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
