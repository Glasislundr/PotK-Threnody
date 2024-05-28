// Decompiled with JetBrains decompiler
// Type: Sea030AlbumIllustrationScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Sea030AlbumIllustrationScene : NGSceneBase
{
  [SerializeField]
  private Sea030AlbumIllustrationMenu menu;

  public static void ChangeScene(float width, int album_id, bool isStack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("sea030_album_illustration", (isStack ? 1 : 0) != 0, (object) width, (object) album_id);
  }

  public IEnumerator onStartSceneAsync(float width, int album_id)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e;
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      e = this.SetSeaBackground();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    e = this.menu.Init(width, album_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene()
  {
  }

  public void onStartScene(float width, int album_id)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  private IEnumerator SetSeaBackground()
  {
    Sea030AlbumIllustrationScene illustrationScene = this;
    Future<GameObject> bgF = new ResourceObject("Prefabs/BackGround/DefaultBackground_sea").Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) bgF.Result, (Object) null))
      illustrationScene.backgroundPrefab = bgF.Result;
  }
}
