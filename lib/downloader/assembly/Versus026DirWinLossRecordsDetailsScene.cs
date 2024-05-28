// Decompiled with JetBrains decompiler
// Type: Versus026DirWinLossRecordsDetailsScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Versus026DirWinLossRecordsDetailsScene : NGSceneBase
{
  [SerializeField]
  private Versus026DirWinLossRecordsDetailsMenu menu;

  public static void ChangeScene(
    bool stack,
    Versus02613Scene.BootParam param,
    string battleID,
    string playerID)
  {
    Versus02613Scene.BootArgument bootArgument = new Versus02613Scene.BootArgument("versus026_win_loss_records_details", param.current, battleID, playerID);
    param.push(bootArgument);
    Singleton<NGSceneManager>.GetInstance().changeScene(bootArgument.scene, (stack ? 1 : 0) != 0, (object) param);
  }

  public IEnumerator onStartSceneAsync(Versus02613Scene.BootParam param)
  {
    IEnumerator e = this.menu.Init(param);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
