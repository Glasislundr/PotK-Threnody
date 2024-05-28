// Decompiled with JetBrains decompiler
// Type: NGxEyeSprite
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
public class NGxEyeSprite : MonoBehaviour
{
  public string EyeName = "normal";
  public UI2DSprite EyeUI2DSprite;
  public NGxMaskSpriteWithScale mask;
  private Dictionary<string, Sprite> eyeSpriteDict = new Dictionary<string, Sprite>();
  [SerializeField]
  private int unitID;
  private int resourceUnitID;
  private int? extId_;
  private UnitUnit unitUnit_;
  private UnitExtensionStory unitExtStory_;
  private UnitUnitStory unitUnitStory_;
  private float prevAlpha;

  private void Start()
  {
    this.resetResourceUnit(this.unitID);
    if (this.unitUnit_ != null)
      this.resetExt(this.extId_);
    else if (this.extId_.HasValue)
      this.resetExt(new int?());
    this.StartCoroutine(this.ChangeEye(this.EyeName));
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
    this.eyeSpriteDict.Clear();
    this.mask.MainUI2DSprite.sprite2D = (Sprite) null;
  }

  public IEnumerator ChangeEye(string eyeName)
  {
    this.EyeName = eyeName;
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
    if (this.EyeName == "normal")
      this.updateEyeSprite(this.EyeName, (Sprite) null);
    Sprite sprite1;
    if (this.eyeSpriteDict.TryGetValue(this.EyeName, out sprite1))
      this.updateEyeSprite(this.EyeName, sprite1);
    else if (this.unitID <= 999)
    {
      if (MobUnits.HasSpriteEye(this.unitID, this.EyeName))
      {
        mainFuture = MobUnits.LoadSpriteEye(this.unitID, this.EyeName);
        e = mainFuture.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        Sprite result = mainFuture.Result;
        Sprite sprite2 = Sprite.Create(result.texture, result.rect, Vector2.zero, 1f, 0U, (SpriteMeshType) 0);
        ((Object) sprite2).name = this.EyeName;
        this.updateEyeSprite(this.EyeName, sprite2);
        mainFuture = (Future<Sprite>) null;
      }
      else if (this.EyeName != "normal")
        Debug.LogError((object) string.Format("MobUnit:{0} eye sprite not found. eyeName={1}", (object) this.unitID, (object) this.EyeName));
    }
    else if ((this.unitUnit_ != null ? (this.extId_.HasValue ? (this.unitUnit_.HasSpriteEye(this.extId_.Value, this.EyeName) ? 1 : 0) : (this.unitUnit_.HasSpriteEye(this.EyeName) ? 1 : 0)) : 0) != 0)
    {
      mainFuture = this.extId_.HasValue ? this.unitUnit_.LoadSpriteEye(this.extId_.Value, this.EyeName) : this.unitUnit_.LoadSpriteEye(this.EyeName);
      e = mainFuture.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Sprite result = mainFuture.Result;
      Sprite sprite3 = Sprite.Create(result.texture, result.rect, Vector2.zero, 1f, 0U, (SpriteMeshType) 0);
      ((Object) sprite3).name = this.EyeName;
      this.updateEyeSprite(this.EyeName, sprite3);
      mainFuture = (Future<Sprite>) null;
    }
    else if (this.EyeName != "normal")
    {
      if (this.extId_.HasValue)
        Debug.LogError((object) string.Format("Unit:{0} Job:{1} eye sprite not found. eyeName={2}", (object) this.unitID, (object) this.extId_.Value, (object) this.EyeName));
      else
        Debug.LogError((object) string.Format("Unit:{0} eye sprite not found. eyeName={1}", (object) this.unitID, (object) this.EyeName));
    }
  }

  public void updateEyeSprite(string eyeName, Sprite sprite)
  {
    this.eyeSpriteDict[eyeName] = sprite;
    this.EyeUI2DSprite.sprite2D = sprite;
    if (Object.op_Equality((Object) sprite, (Object) null))
      return;
    ((UIWidget) this.EyeUI2DSprite).SetDimensions(((Texture) sprite.texture).width, ((Texture) sprite.texture).height);
    Vector3 localPosition = ((Component) this.EyeUI2DSprite).gameObject.transform.localPosition;
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
      UI2DSprite component = ((Component) ((Component) this.EyeUI2DSprite).transform.parent).gameObject.GetComponent<UI2DSprite>();
      num1 += (float) (((Texture) component.sprite2D.texture).width / 2 - (unitUnitStory2.face_x + ((Texture) sprite.texture).width / 2)) * this.mask.scale;
      num2 += (float) (((Texture) component.sprite2D.texture).height / 2 - (unitUnitStory2.face_y + ((Texture) sprite.texture).height / 2)) * this.mask.scale;
    }
    ((Component) this.EyeUI2DSprite).gameObject.transform.localScale = new Vector3(this.mask.scale, this.mask.scale, 1f);
    ((Component) this.EyeUI2DSprite).gameObject.transform.localPosition = new Vector3(-num1, -num2, localPosition.z);
    ((UIWidget) this.EyeUI2DSprite).depth = ((Component) ((Component) this.EyeUI2DSprite).transform.parent).gameObject.GetComponent<UIWidget>().depth + 2;
  }

  private void Update()
  {
    if ((double) this.mask.spriteAlpha < 1.0)
      ((Behaviour) this.EyeUI2DSprite).enabled = false;
    else
      ((Behaviour) this.EyeUI2DSprite).enabled = true;
  }
}
