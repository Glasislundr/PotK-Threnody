// Decompiled with JetBrains decompiler
// Type: PopupNoticeContentWebImage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class PopupNoticeContentWebImage : PopupNoticeContentBase
{
  [SerializeField]
  private UI2DSprite mWebImage;
  [SerializeField]
  private UIScrollView mScrollView;
  [SerializeField]
  private int mViewMaxWidth = 560;
  [SerializeField]
  private int mViewMaxHeight = 690;

  public override IEnumerator LoadContent(OfficialInfoPopupSchema contentData)
  {
    string url = contentData.popup_img_url;
    NGGameDataManager dataMgr = Singleton<NGGameDataManager>.GetInstance();
    if (string.IsNullOrEmpty(url))
    {
      ((Component) this.mWebImage).gameObject.SetActive(false);
    }
    else
    {
      IEnumerator e = dataMgr.LoadWebImage(url);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Texture2D texture2D = (Texture2D) null;
      dataMgr.webImageCache.TryGetValue(url, out texture2D);
      if (Object.op_Equality((Object) texture2D, (Object) null))
      {
        ((Component) this.mWebImage).gameObject.SetActive(false);
      }
      else
      {
        ((Texture) texture2D).wrapMode = (TextureWrapMode) 1;
        float width = (float) ((Texture) texture2D).width;
        float height = (float) ((Texture) texture2D).height;
        float num = Mathf.Min((float) this.mViewMaxWidth / width, 1f);
        this.mWebImage.sprite2D = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, width, height), new Vector2(0.0f, 0.0f), 100f, 0U, (SpriteMeshType) 0);
        ((UIWidget) this.mWebImage).SetDimensions((int) width, (int) height);
        ((Component) this.mWebImage).transform.localScale = new Vector3(num, num);
        ((Component) this.mWebImage).gameObject.SetActive(true);
        this.AdjustCollider();
        this.mScrollView.contentPivot = (double) this.mViewMaxHeight <= (double) height ? (UIWidget.Pivot) 1 : (UIWidget.Pivot) 4;
        this.mScrollView.ResetPosition();
      }
    }
  }

  public override void Refresh() => this.mScrollView.ResetPosition();

  private void AdjustCollider()
  {
    int num1 = Mathf.Max(((UIWidget) this.mWebImage).width, this.mViewMaxWidth);
    int num2 = Mathf.Max(((UIWidget) this.mWebImage).height, this.mViewMaxHeight);
    ((Component) this.mWebImage).GetComponent<BoxCollider>().size = new Vector3((float) num1, (float) num2, 0.0f);
  }
}
