// Decompiled with JetBrains decompiler
// Type: Startup00012Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Startup00012Scene : NGSceneBase
{
  [SerializeField]
  private Startup00012Menu menu;

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = this.onStartSceneAsync(((IEnumerable<OfficialInformationArticle>) Singleton<NGGameDataManager>.GetInstance().officialInfos).FirstOrDefault<OfficialInformationArticle>());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(OfficialInformationArticle info)
  {
    IEnumerator e = this.menu.InitSceneAsync(info);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.menu.InitAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(OfficialInformationArticle info, bool isDirectBack)
  {
    Startup00012Scene startup00012Scene = this;
    startup00012Scene.isActiveFooter = false;
    IEnumerator e = startup00012Scene.onStartSceneAsync(info);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (isDirectBack)
      startup00012Scene.menu.ChangeDirectBackSceneMode();
  }

  public IEnumerator onStartSceneAsync(int info_id)
  {
    OfficialInformationArticle info = ((IEnumerable<OfficialInformationArticle>) Singleton<NGGameDataManager>.GetInstance().officialInfos).FirstOrDefault<OfficialInformationArticle>((Func<OfficialInformationArticle, bool>) (x => x.id == info_id));
    IEnumerator e;
    if (info != null)
    {
      e = this.onStartSceneAsync(info);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = this.onStartSceneAsync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public override void onSceneInitialized()
  {
    base.onSceneInitialized();
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public override void onEndScene() => DetailController.Release();
}
