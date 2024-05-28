// Decompiled with JetBrains decompiler
// Type: OPMoviePlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
public class OPMoviePlayer
{
  private const string movieAsset = "op_fix_800x450.mp4";
  public const string toNextScene = "startup000_6";

  public IEnumerator Attach()
  {
    string str = Application.streamingAssetsPath + "/op_fix_800x450.mp4";
    string absoluteUri = new Uri(str).AbsoluteUri;
    StatusBarHelper.SetVisibilityForIPhoneX(false);
    WindowsMovieController wmc = (Object.Instantiate(Resources.Load("Prefabs/WindowsMovie")) as GameObject).GetComponent<WindowsMovieController>();
    wmc.ShowMovie(str);
    yield return (object) new WaitForEndOfFrame();
    while (((CriManaMovieMaterial) wmc.movieScreen).player.status != null)
      yield return (object) new WaitForEndOfFrame();
    StatusBarHelper.SetVisibilityForIPhoneX(true);
    SceneManager.LoadScene("startup000_6");
  }
}
