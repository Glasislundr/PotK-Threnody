// Decompiled with JetBrains decompiler
// Type: NGxFaceSprite
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class NGxFaceSprite : MonoBehaviour
{
  public string faceName = "normal";
  public UI2DSprite FaceUI2DSprite;
  public NGxMaskSpriteWithScale mask;
  private Dictionary<string, Sprite> faceSpriteDict = new Dictionary<string, Sprite>();
  [SerializeField]
  private int unitID;
  private int resourceUnitID;
  private int? extId_;
  private UnitUnit unitUnit_;
  private UnitExtensionStory unitExtStory_;
  private UnitUnitStory unitUnitStory_;
  private bool isEnable;

  private void Start()
  {
    this.resetResourceUnit(this.unitID);
    if (this.unitUnit_ != null)
      this.resetExt(this.extId_);
    else if (this.extId_.HasValue)
      this.resetExt(new int?());
    this.StartCoroutine(this.ChangeFace(this.faceName));
  }

  public int UnitID
  {
    get => this.unitID;
    set
    {
      if (this.unitID == value)
        return;
      this.unitID = value;
      int resourceUnitId1 = this.resourceUnitID;
      this.resetResourceUnit(value);
      int resourceUnitId2 = this.resourceUnitID;
      if (resourceUnitId1 != resourceUnitId2)
      {
        if (this.unitUnit_ != null)
          this.resetExt(this.extId_);
        else if (this.extId_.HasValue)
          this.resetExt(new int?());
      }
      this.clearSpriteCache();
    }
  }

  public int? ExtID
  {
    get => this.extId_;
    set
    {
      int? extId = this.extId_;
      int? nullable = value;
      if (extId.GetValueOrDefault() == nullable.GetValueOrDefault() & extId.HasValue == nullable.HasValue)
        return;
      if (this.unitUnit_ != null)
        this.resetExt(value);
      else if (this.extId_.HasValue)
        this.resetExt(new int?());
      this.clearSpriteCache();
    }
  }

  private void resetResourceUnit(int unitId)
  {
    if (MasterData.UnitUnit.TryGetValue(unitId, out this.unitUnit_))
    {
      this.resourceUnitID = this.unitUnit_.resource_reference_unit_id_UnitUnit;
    }
    else
    {
      this.resourceUnitID = unitId;
      this.unitUnit_ = (UnitUnit) null;
    }
    if (MasterData.UnitUnitStory.TryGetValue(this.resourceUnitID, out this.unitUnitStory_))
      return;
    this.unitUnitStory_ = (UnitUnitStory) null;
  }

  private bool resetExt(int? extId)
  {
    UnitExtensionStory unitExtStory1 = this.unitExtStory_;
    if (!extId.HasValue || extId.Value == 0)
    {
      this.extId_ = new int?();
      this.unitExtStory_ = (UnitExtensionStory) null;
    }
    else
    {
      this.extId_ = extId;
      this.unitExtStory_ = Array.Find<UnitExtensionStory>(MasterData.UnitExtensionStoryList, (Predicate<UnitExtensionStory>) (x => x.unit == this.resourceUnitID && x.job_metamor_id == extId.Value));
    }
    UnitExtensionStory unitExtStory2 = this.unitExtStory_;
    return unitExtStory1 != unitExtStory2;
  }

  private void clearSpriteCache()
  {
    this.faceSpriteDict.Clear();
    this.mask.MainUI2DSprite.sprite2D = (Sprite) null;
  }

  public IEnumerator ChangeFace(string faceName_)
  {
    this.faceName = faceName_;
    Future<Sprite> mainFuture;
    IEnumerator e;
    if (Object.op_Equality((Object) this.mask.MainUI2DSprite.sprite2D, (Object) null))
    {
      if (this.unitID <= 999)
      {
        mainFuture = MobUnits.LoadSpriteLarge(this.unitID);
        e = mainFuture.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.mask.MainUI2DSprite.sprite2D = mainFuture.Result;
        this.mask.FitMask();
        mainFuture = (Future<Sprite>) null;
      }
      else if (this.unitUnit_ != null)
      {
        mainFuture = this.extId_.HasValue ? this.unitUnit_.LoadSpriteLarge(this.extId_.Value, 1f) : this.unitUnit_.LoadSpriteLarge();
        e = mainFuture.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.mask.MainUI2DSprite.sprite2D = mainFuture.Result;
        this.mask.FitMask();
        mainFuture = (Future<Sprite>) null;
      }
      else
      {
        Debug.LogWarning((object) ("unit id not found on master data. id=" + (object) this.unitID));
        yield break;
      }
    }
    if (this.faceName == "normal")
      this.updateFaceSprite(this.faceName, (Sprite) null);
    Sprite sprite1;
    if (this.faceSpriteDict.TryGetValue(this.faceName, out sprite1))
      this.updateFaceSprite(this.faceName, sprite1);
    else if (this.unitID <= 999)
    {
      if (MobUnits.HasSpriteFace(this.unitID, this.faceName))
      {
        mainFuture = MobUnits.LoadSpriteFace(this.unitID, this.faceName);
        e = mainFuture.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        Sprite result = mainFuture.Result;
        Sprite sprite2 = Sprite.Create(result.texture, result.rect, Vector2.zero, 1f, 0U, (SpriteMeshType) 0);
        ((Object) sprite2).name = this.faceName;
        this.updateFaceSprite(this.faceName, sprite2);
        mainFuture = (Future<Sprite>) null;
      }
      else if (this.faceName != "normal")
        Debug.LogError((object) string.Format("MobUnit:{0} face sprite not found. faceName={1}", (object) this.unitID, (object) this.faceName));
    }
    else if ((this.unitUnit_ != null ? (this.extId_.HasValue ? (this.unitUnit_.HasSpriteFace(this.extId_.Value, this.faceName) ? 1 : 0) : (this.unitUnit_.HasSpriteFace(this.faceName) ? 1 : 0)) : 0) != 0)
    {
      mainFuture = this.extId_.HasValue ? this.unitUnit_.LoadSpriteFace(this.extId_.Value, this.faceName) : this.unitUnit_.LoadSpriteFace(this.faceName);
      e = mainFuture.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Sprite result = mainFuture.Result;
      Sprite sprite3 = Sprite.Create(result.texture, result.rect, Vector2.zero, 1f, 0U, (SpriteMeshType) 0);
      ((Object) sprite3).name = this.faceName;
      this.updateFaceSprite(this.faceName, sprite3);
      mainFuture = (Future<Sprite>) null;
    }
    else if (this.faceName != "normal")
    {
      if (this.extId_.HasValue)
        Debug.LogError((object) string.Format("Unit:{0} Job:{1} face sprite not found. faceName={2}", (object) this.unitID, (object) this.extId_.Value, (object) this.faceName));
      else
        Debug.LogError((object) string.Format("Unit:{0} face sprite not found. faceName={1}", (object) this.unitID, (object) this.faceName));
    }
  }

  public void updateFaceSprite(string faceName, Sprite sprite)
  {
    this.faceSpriteDict[faceName] = sprite;
    this.FaceUI2DSprite.sprite2D = sprite;
    if (Object.op_Equality((Object) sprite, (Object) null))
      return;
    ((UIWidget) this.FaceUI2DSprite).SetDimensions(((Texture) sprite.texture).width, ((Texture) sprite.texture).height);
    Vector3 localPosition = ((Component) this.FaceUI2DSprite).gameObject.transform.localPosition;
    float num1 = (float) this.mask.xOffsetPixel * this.mask.scale;
    float num2 = ((float) this.mask.yOffsetPixel + this.mask.topOffset) * this.mask.scale;
    UnitUnitStory unitUnitStory1;
    if (this.unitExtStory_ == null)
    {
      unitUnitStory1 = this.unitUnitStory_;
    }
    else
    {
      unitUnitStory1 = new UnitUnitStory();
      unitUnitStory1.face_x = this.unitExtStory_.face_x;
      unitUnitStory1.face_y = this.unitExtStory_.face_y;
    }
    UnitUnitStory unitUnitStory2 = unitUnitStory1;
    if (unitUnitStory2 != null)
    {
      UI2DSprite component = ((Component) ((Component) this.FaceUI2DSprite).transform.parent).gameObject.GetComponent<UI2DSprite>();
      num1 += (float) (((Texture) component.sprite2D.texture).width / 2 - (unitUnitStory2.face_x + ((Texture) sprite.texture).width / 2)) * this.mask.scale;
      num2 += (float) (((Texture) component.sprite2D.texture).height / 2 - (unitUnitStory2.face_y + ((Texture) sprite.texture).height / 2)) * this.mask.scale;
    }
    ((Component) this.FaceUI2DSprite).gameObject.transform.localScale = new Vector3(this.mask.scale, this.mask.scale, 1f);
    ((Component) this.FaceUI2DSprite).gameObject.transform.localPosition = new Vector3(-num1, -num2, localPosition.z);
    ((UIWidget) this.FaceUI2DSprite).depth = ((Component) ((Component) this.FaceUI2DSprite).transform.parent).gameObject.GetComponent<UIWidget>().depth + 1;
  }

  private void Update()
  {
    if (this.isEnable)
    {
      ((Behaviour) this.FaceUI2DSprite).enabled = true;
      this.isEnable = false;
    }
    if ((double) this.mask.spriteAlpha < 1.0)
      ((Behaviour) this.FaceUI2DSprite).enabled = false;
    else
      this.isEnable = true;
  }
}
