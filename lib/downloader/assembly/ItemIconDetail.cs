// Decompiled with JetBrains decompiler
// Type: ItemIconDetail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class ItemIconDetail : MonoBehaviour
{
  [SerializeField]
  private CreateIconObject icon;
  [SerializeField]
  private GameObject markLoupe;
  [SerializeField]
  private BoxCollider markLoupeBoxColider;
  [SerializeField]
  private Transform quantity;
  [SerializeField]
  private UISprite quantitySprite;
  [SerializeField]
  private UILabel quantityLabel;
  [SerializeField]
  private GameObject stone;
  private MasterDataTable.CommonRewardType type;
  private int typeId;
  private int count;
  private Func<bool> OpenDetailValidate;

  public IEnumerator Init(
    MasterDataTable.CommonRewardType type,
    int typeId,
    int quantity,
    bool useDragScrollView = true,
    TransformSizeInfo iconInfo = null,
    TransformSizeInfo quantityInfo = null,
    SpriteTransformSizeInfo quantitySpriteInfo = null,
    TransformSizeInfo stoneInfo = null,
    Func<bool> openDetailValidate = null)
  {
    this.type = type;
    this.typeId = typeId;
    this.count = quantity;
    this.OpenDetailValidate = openDetailValidate;
    if (iconInfo != null)
    {
      Transform transform = ((Component) this.icon).gameObject.transform;
      Vector3 vector3;
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3).\u002Ector(transform.localPosition.x - iconInfo.PositionX, transform.localPosition.y - iconInfo.PositionY, 0.0f);
      transform.localScale = new Vector3(iconInfo.Scale, iconInfo.Scale, 1f);
      transform.localPosition = new Vector3(iconInfo.PositionX, iconInfo.PositionY, 0.0f);
      this.markLoupeBoxColider.size = new Vector3(this.markLoupeBoxColider.size.x * iconInfo.Scale, this.markLoupeBoxColider.size.y * iconInfo.Scale, 1f);
      this.markLoupeBoxColider.center = new Vector3(this.markLoupeBoxColider.center.x - vector3.x, this.markLoupeBoxColider.center.y - vector3.y, 0.0f);
    }
    if (quantityInfo != null)
    {
      this.quantity.localScale = new Vector3(quantityInfo.Scale, quantityInfo.Scale, 1f);
      this.quantity.localPosition = new Vector3(quantityInfo.PositionX, quantityInfo.PositionY, 0.0f);
    }
    if (quantitySpriteInfo != null)
    {
      ((Component) this.quantitySprite).gameObject.transform.localPosition = new Vector3(quantitySpriteInfo.PositionX, quantitySpriteInfo.PositionY, 0.0f);
      ((UIWidget) this.quantitySprite).SetDimensions(quantitySpriteInfo.SizeX, quantitySpriteInfo.SizeY);
    }
    if (useDragScrollView)
      this.markLoupe.GetOrAddComponent<UIDragScrollView>();
    if (type != MasterDataTable.CommonRewardType.coin)
    {
      this.stone.SetActive(false);
    }
    else
    {
      this.stone.SetActive(true);
      if (stoneInfo != null)
      {
        Transform transform = this.stone.transform;
        transform.localScale = new Vector3(stoneInfo.Scale, stoneInfo.Scale, 1f);
        transform.localPosition = new Vector3(stoneInfo.PositionX, stoneInfo.PositionY, 0.0f);
      }
    }
    IEnumerator e = this.icon.CreateThumbnail(type, typeId);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.quantityLabel.text = quantity.ToString();
    this.SetupDetailButton();
  }

  private void SetupDetailButton()
  {
    switch (this.type)
    {
      case MasterDataTable.CommonRewardType.emblem:
      case MasterDataTable.CommonRewardType.challenge_point:
        this.markLoupe.SetActive(false);
        break;
      default:
        this.markLoupe.SetActive(true);
        break;
    }
  }

  public void OnDetailButton()
  {
    if (this.OpenDetailValidate != null && !this.OpenDetailValidate())
      return;
    this.StartCoroutine(this.ShowDetail());
  }

  private IEnumerator ShowDetail()
  {
    Future<GameObject> prefab = Res.Prefabs.popup.popup_000_7_4_2__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject popup = prefab.Result.Clone();
    popup.SetActive(false);
    e = popup.GetComponent<ItemDetailPopupBase>().SetInfo(this.type, this.typeId, this.count);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    popup.SetActive(true);
  }
}
