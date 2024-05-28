// Decompiled with JetBrains decompiler
// Type: PurchaseBehaviorLoadingLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
internal class PurchaseBehaviorLoadingLayer
{
  private static bool isActive;
  private static GameObject layer;

  public static IEnumerator Enable()
  {
    if (!PurchaseBehaviorLoadingLayer.isActive && Object.op_Equality((Object) PurchaseBehaviorLoadingLayer.layer, (Object) null))
    {
      PurchaseBehaviorLoadingLayer.isActive = true;
      IEnumerator e = PurchaseBehaviorLoadingLayer.ShowLoading();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public static void Disable()
  {
    PurchaseBehaviorLoadingLayer.isActive = false;
    if (!Object.op_Inequality((Object) PurchaseBehaviorLoadingLayer.layer, (Object) null))
      return;
    PurchaseBehavior.PopupDismiss();
    PurchaseBehaviorLoadingLayer.layer = (GameObject) null;
  }

  private static IEnumerator ShowLoading()
  {
    Future<GameObject> loadingLoader = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/common/Loading/LoadingSimplePrefab");
    IEnumerator e = loadingLoader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (PurchaseBehaviorLoadingLayer.isActive)
    {
      UIPanel component = loadingLoader.Result.GetComponent<UIPanel>();
      GameObject gameObject = PurchaseBehavior.PopupOpen(loadingLoader.Result);
      Transform transform = gameObject.transform;
      ((UIRect) component).SetAnchor(transform);
      gameObject.GetComponent<UIPanel>().depth = ((Component) gameObject.transform.parent).GetComponent<UIPanel>().depth;
      PurchaseBehaviorLoadingLayer.layer = gameObject;
    }
  }
}
