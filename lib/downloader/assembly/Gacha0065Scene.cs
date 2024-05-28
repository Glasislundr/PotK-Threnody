// Decompiled with JetBrains decompiler
// Type: Gacha0065Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Gacha0065Scene : NGSceneBase
{
  private GameObject popUp;
  private Gacha0065Menu obj;

  public IEnumerator onStartSceneAsync()
  {
    GachaModule[] module = SMManager.Get<GachaModule[]>();
    if (Object.op_Equality((Object) this.popUp, (Object) null))
    {
      Future<GameObject> fPopUp = Res.Prefabs.gacha006_5.popup_006_5__anim_popup01.Load<GameObject>();
      IEnumerator e = fPopUp.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.popUp = fPopUp.Result;
      fPopUp = (Future<GameObject>) null;
    }
    this.obj = Singleton<PopupManager>.GetInstance().open(this.popUp).GetComponent<Gacha0065Menu>();
    this.obj.Init(module[0].name, module[0].gacha[0]);
  }
}
