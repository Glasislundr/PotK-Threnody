// Decompiled with JetBrains decompiler
// Type: Menu0591Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Menu0591Menu : BackButtonMenuBase
{
  [SerializeField]
  private NGxScroll scrollContainer;

  public IEnumerator InitSceneAsync()
  {
    this.scrollContainer.ResolvePosition();
    yield break;
  }

  public IEnumerator StartSceneAsync()
  {
    yield break;
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().clearStack();
    Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
    Mypage051Scene.ChangeScene(false);
  }

  public void onHelpClick()
  {
    if (this.IsPushAndSet())
      return;
    Help0151Scene.ChangeScene(true);
  }

  public void onDetaResetClick()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.ShowDetaResetPopup());
  }

  public void onStoryClick()
  {
    if (this.IsPushAndSet())
      return;
    Story0592Scene.ChangeScene(true);
  }

  private IEnumerator ShowDetaResetPopup()
  {
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_051_10__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefab = prefabF.Result.Clone();
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }
}
