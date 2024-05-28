// Decompiled with JetBrains decompiler
// Type: PopupAppReview
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class PopupAppReview : BackButtonMenuBase
{
  public static bool Show(MonoBehaviour m)
  {
    if (!PopupAppReview.IsShow())
      return false;
    m.StartCoroutine(PopupAppReview.ShowReviewPopup());
    Persist.appReview.Data.isShow = true;
    Persist.appReview.Flush();
    return true;
  }

  private static bool IsShow() => !Persist.appReview.Data.isShow && PopupAppReview.IsOSEnable();

  private static bool IsOSEnable() => false;

  private static IEnumerator ShowReviewPopup()
  {
    Future<GameObject> f = new ResourceObject("Prefabs/popup/popup_Review__anim_popup01").Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(f.Result);
  }

  public void OnReview()
  {
    if (this.IsPushAndSet())
      return;
    Debug.LogWarningFormat("This OS→{0} is not app review supported", (object) SystemInfo.operatingSystem);
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public void OnContact()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
    Help0154Scene.ChangeScene(true);
  }

  public override void onBackButton() => this.IbtnRetrun();

  private void IbtnRetrun()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }
}
