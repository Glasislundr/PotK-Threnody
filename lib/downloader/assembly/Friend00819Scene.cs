// Decompiled with JetBrains decompiler
// Type: Friend00819Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Friend00819Scene : NGSceneBase
{
  [SerializeField]
  private Friend00819Menu menu;
  private static bool mode;

  public static void ChangeSceneApproval(bool stack)
  {
    Friend00819Scene.mode = false;
    Singleton<NGSceneManager>.GetInstance().changeScene("friend008_19", (stack ? 1 : 0) != 0, (object) Friend00819Scene.mode);
  }

  public static void ChangeSceneDenial(bool stack)
  {
    Friend00819Scene.mode = true;
    Singleton<NGSceneManager>.GetInstance().changeScene("friend008_19", (stack ? 1 : 0) != 0, (object) Friend00819Scene.mode);
  }

  public IEnumerator onStartSceneAsync(bool mode)
  {
    IEnumerator e = this.menu.InitFriendScroll(((IEnumerable<PlayerFriend>) SMManager.Get<PlayerFriend[]>()).ReceivedFriendApplications(), mode);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
