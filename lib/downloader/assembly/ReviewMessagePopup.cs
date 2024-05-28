// Decompiled with JetBrains decompiler
// Type: ReviewMessagePopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class ReviewMessagePopup : MonoBehaviour
{
  [SerializeField]
  private UILabel TxtTitle;
  [SerializeField]
  private UILabel TxtDescription;
  private string review_id;

  public void Init(string title, string message, string id)
  {
    this.review_id = id;
    ((UIRect) ((Component) this).GetComponent<UIWidget>()).alpha = 0.0f;
    this.TxtTitle.SetText(title);
    this.TxtDescription.SetText(message);
    ((UIRect) ((Component) this).GetComponent<UIWidget>()).alpha = 1f;
  }

  public void btnReview() => this.StartCoroutine(this.goReview());

  public void btnAfter() => Singleton<PopupManager>.GetInstance().onDismiss();

  public void btnRefuse() => this.StartCoroutine(this.goRefuse());

  private IEnumerator goReview()
  {
    Future<WebAPI.Response.ReviewContribute> res = WebAPI.ReviewContribute(this.review_id, (Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<PopupManager>.GetInstance().onDismiss();
      WebAPI.DefaultUserErrorCallback(e);
    }));
    IEnumerator e1 = res.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (res.Result != null)
      Singleton<PopupManager>.GetInstance().onDismiss();
  }

  private IEnumerator goRefuse()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    IEnumerator e1 = WebAPI.ReviewCancel(this.review_id, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e))).Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
  }
}
