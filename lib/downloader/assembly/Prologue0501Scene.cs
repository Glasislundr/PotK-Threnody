// Decompiled with JetBrains decompiler
// Type: Prologue0501Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Prologue0501Scene : NGSceneBase
{
  private string moviePath;
  private bool isFinish;
  private bool callChangeScene;

  public static void ChangeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("prologue050_1", stack);
  }

  public void onStartScene()
  {
    CommonEarthHeader earthHeaderComponent = Singleton<CommonRoot>.GetInstance().GetEarthHeaderComponent();
    if (Object.op_Inequality((Object) earthHeaderComponent, (Object) null))
      earthHeaderComponent.isActive = false;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    this.moviePath = Singleton<EarthDataManager>.GetInstance().questProgress.GetPrologueData().arg1;
    CommonRoot instance = Singleton<CommonRoot>.GetInstance();
    if (instance.getCloudAnimEnabled())
      instance.StartCloudAnimEnd((Action) (() => this.StartCoroutine(this.PlayMovie())));
    else
      this.StartCoroutine(this.PlayMovie());
  }

  private void Update()
  {
    if (!this.isFinish || this.callChangeScene)
      return;
    Singleton<EarthDataManager>.GetInstance().NextPrologueScene();
    this.callChangeScene = true;
  }

  private string GetMoviePath() => "windows/" + this.moviePath;

  private IEnumerator PlayMovie()
  {
    Singleton<NGSoundManager>.GetInstance().StopAll();
    string moviepath = this.GetMoviePath();
    if (moviepath == "")
    {
      yield return (object) new WaitForEndOfFrame();
      Debug.LogError((object) "Movie path is empty");
      this.isFinish = true;
    }
    else
    {
      IEnumerator e = OnDemandDownload.waitLoadMovieResource((IEnumerable<string>) Singleton<ResourceManager>.GetInstance().PathsFromMovie(moviepath), false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      string str = Singleton<ResourceManager>.GetInstance().ResolveStreamingAssetsPathForMovie(moviepath);
      string absoluteUri = new Uri(str).AbsoluteUri;
      WindowsMovieController wmc = (Object.Instantiate(Resources.Load("Prefabs/WindowsMovie")) as GameObject).GetComponent<WindowsMovieController>();
      wmc.ShowMovie(str);
      yield return (object) new WaitForEndOfFrame();
      while (((CriManaMovieMaterial) wmc.movieScreen).player.status != null)
        yield return (object) new WaitForEndOfFrame();
      this.isFinish = true;
    }
  }
}
