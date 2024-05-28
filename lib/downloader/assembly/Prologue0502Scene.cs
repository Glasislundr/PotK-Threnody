// Decompiled with JetBrains decompiler
// Type: Prologue0502Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Prologue0502Scene : NGSceneBase
{
  [SerializeField]
  private string iosMoviePath;
  [SerializeField]
  private string androidMoviePath;
  [SerializeField]
  private string windowsMoviePath;
  private bool isFinish;
  private bool callChangeScene;

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = Singleton<EarthDataManager>.GetInstance().SaveAndSendServer();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene() => this.StartCoroutine(this.PlayMovie());

  private void Update()
  {
    if (!this.isFinish || this.callChangeScene)
      return;
    Singleton<NGSceneManager>.GetInstance().sceneBase.IsPush = true;
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
    Mypage051Scene.ChangeScene(false);
    this.callChangeScene = true;
  }

  private string moviePath() => this.windowsMoviePath;

  private IEnumerator PlayMovie()
  {
    Prologue0502Scene prologue0502Scene = this;
    Singleton<NGSoundManager>.GetInstance().StopAll();
    string moviepath = prologue0502Scene.moviePath();
    if (moviepath == "")
    {
      yield return (object) new WaitForEndOfFrame();
      Debug.LogError((object) "Movie path is empty");
      prologue0502Scene.isFinish = true;
    }
    else
    {
      IEnumerator e = OnDemandDownload.waitLoadMovieResource((IEnumerable<string>) Singleton<ResourceManager>.GetInstance().PathsFromMovie(moviepath), false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      string str = Singleton<ResourceManager>.GetInstance().ResolveStreamingAssetsPathForMovie(moviepath);
      string filePath = new Uri(str).AbsoluteUri;
      Debug.Log((object) ("play : " + filePath + " GameObj name : " + ((Object) ((Component) prologue0502Scene).gameObject).name));
      WindowsMovieController wmc = (Object.Instantiate(Resources.Load("Prefabs/WindowsMovie")) as GameObject).GetComponent<WindowsMovieController>();
      wmc.ShowMovie(str);
      yield return (object) new WaitForEndOfFrame();
      while (((CriManaMovieMaterial) wmc.movieScreen).player.status != null)
        yield return (object) new WaitForEndOfFrame();
      Debug.Log((object) ("play end : " + filePath));
      prologue0502Scene.isFinish = true;
    }
  }
}
