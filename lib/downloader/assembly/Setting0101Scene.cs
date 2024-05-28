// Decompiled with JetBrains decompiler
// Type: Setting0101Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Setting0101Scene : NGSceneBase
{
  [SerializeField]
  private Setting0101Menu menu;

  public override IEnumerator onInitSceneAsync()
  {
    base.onInitSceneAsync();
    this.menu.Initialize();
    yield break;
  }

  public override IEnumerator onEndSceneAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Setting0101Scene setting0101Scene = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      // ISSUE: reference to a compiler-generated method
      setting0101Scene.\u003C\u003En__1();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    Singleton<PopupManager>.GetInstance().closeAll();
    Persist.volume.Data.Bgm = setting0101Scene.menu.bgmVolume;
    Persist.volume.Data.Se = setting0101Scene.menu.seVolume;
    Persist.volume.Data.Voice = setting0101Scene.menu.voiceVolume;
    Persist.volume.Flush();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) setting0101Scene.StartCoroutine(setting0101Scene.menu.onEndSceneAsync());
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }
}
