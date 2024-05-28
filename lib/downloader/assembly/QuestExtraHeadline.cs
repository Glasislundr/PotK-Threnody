// Decompiled with JetBrains decompiler
// Type: QuestExtraHeadline
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/QuestExtra/Headline")]
public class QuestExtraHeadline : MonoBehaviour
{
  [SerializeField]
  private UI2DSprite eventSprite_;
  [Header("AnchorTopControl")]
  [Tooltip("EventSpriteの有無に合わせてScrollViewのTop位置を調整")]
  [SerializeField]
  private UIWidget widgetScrollViewTop_;
  [SerializeField]
  private int whenOnEventSprite_;
  [SerializeField]
  private int whenOffEventSprige_;
  private static readonly string prefix = "Prefabs/Banners/ExtraQuest/";
  private static readonly string suffix = "/Specialquest_Story";

  public IEnumerator doInitialize(QuestExtraLL info)
  {
    if (info.enabled_header)
    {
      ((UIRect) this.widgetScrollViewTop_).topAnchor.absolute = this.whenOnEventSprite_;
      yield return (object) this.doSetSprite(this.doLoadSprite(info));
    }
    else
    {
      ((UIRect) this.widgetScrollViewTop_).topAnchor.absolute = this.whenOffEventSprige_;
      ((Component) this.eventSprite_).gameObject.SetActive(false);
    }
  }

  public IEnumerator doInitialize(QuestExtraL info)
  {
    if (info.enabled_header)
    {
      ((UIRect) this.widgetScrollViewTop_).topAnchor.absolute = this.whenOnEventSprite_;
      yield return (object) this.doSetSprite(this.doLoadSprite(info));
    }
    else
    {
      ((UIRect) this.widgetScrollViewTop_).topAnchor.absolute = this.whenOffEventSprige_;
      ((Component) this.eventSprite_).gameObject.SetActive(false);
    }
  }

  private IEnumerator doSetSprite(Future<Texture2D> loader)
  {
    yield return (object) loader.Wait();
    Texture2D result = loader.Result;
    Sprite sprite = Sprite.Create(result, new Rect(0.0f, 0.0f, (float) ((Texture) result).width, (float) ((Texture) result).height), new Vector2(0.5f, 0.5f), 1f, 100U, (SpriteMeshType) 0);
    ((Object) sprite).name = ((Object) result).name;
    this.eventSprite_.sprite2D = sprite;
    ((Component) this.eventSprite_).gameObject.SetActive(true);
  }

  private static string defaultPath
  {
    get => QuestExtraHeadline.prefix + "L/4" + QuestExtraHeadline.suffix;
  }

  private Future<Texture2D> doLoadSprite(QuestExtraLL info)
  {
    string path = QuestExtraHeadline.prefix + string.Format("LL/{0}", (object) info.banner_image_id.GetValueOrDefault(info.ID)) + QuestExtraHeadline.suffix;
    if (!Singleton<ResourceManager>.GetInstance().Contains(path))
      path = QuestExtraHeadline.defaultPath;
    return Singleton<ResourceManager>.GetInstance().Load<Texture2D>(path);
  }

  private Future<Texture2D> doLoadSprite(QuestExtraL info)
  {
    string path = QuestExtraHeadline.prefix + string.Format("L/{0}", (object) info.banner_image_id.GetValueOrDefault(info.ID)) + QuestExtraHeadline.suffix;
    if (!Singleton<ResourceManager>.GetInstance().Contains(path))
      path = QuestExtraHeadline.defaultPath;
    return Singleton<ResourceManager>.GetInstance().Load<Texture2D>(path);
  }
}
