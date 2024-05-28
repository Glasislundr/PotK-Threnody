// Decompiled with JetBrains decompiler
// Type: Guide011JukeboxScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Guide011JukeboxScene : NGSceneBase
{
  [SerializeField]
  private Guide011JukeboxMenu menu;
  private string oldBgm;
  private string oldCueName;

  public override IEnumerator onInitSceneAsync()
  {
    Guide011JukeboxScene guide011JukeboxScene = this;
    guide011JukeboxScene.oldBgm = Singleton<NGSoundManager>.GetInstance().GetBgmName();
    guide011JukeboxScene.oldCueName = Singleton<NGSoundManager>.GetInstance().GetCueName();
    Future<GameObject> bgF = new ResourceObject("Prefabs/BackGround/JukeboxBackground").Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    guide011JukeboxScene.backgroundPrefab = bgF.Result;
    yield return (object) guide011JukeboxScene.menu.onInitSceneAsync();
  }

  public IEnumerator onStartSceneAsync()
  {
    yield return (object) this.menu.onStartSceneAsync();
  }

  public override void onEndScene()
  {
    Persist.jukeBox.Flush();
    Singleton<NGSoundManager>.GetInstance().StopBgm(time: 0.1f);
    Singleton<NGSoundManager>.GetInstance().ResumeBgm(forceStop: true);
  }
}
