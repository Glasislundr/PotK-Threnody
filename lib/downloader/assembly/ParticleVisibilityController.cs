// Decompiled with JetBrains decompiler
// Type: ParticleVisibilityController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ParticleVisibilityController : MonoBehaviour
{
  private ParticleSystemRenderer psRenderer;
  private UIRect parentRect;
  private ParticleClippingController pcController;
  private bool currentVisibility;
  private bool previousVisibility;
  private bool isLocaleScaleInitialized;
  private bool isClipRegionInitialized;

  private void Start()
  {
    this.parentRect = ((Component) this).GetComponentInParent<UIRect>();
    this.psRenderer = ((Component) this).GetComponentInChildren<ParticleSystemRenderer>(true);
    this.pcController = ((Component) this).GetComponentInParent<ParticleClippingController>();
  }

  private void Update()
  {
    this.UpdateLocalScale();
    this.UpdateClipRegion();
    this.UpdateVisibility();
  }

  private void UpdateLocalScale()
  {
    if (this.isLocaleScaleInitialized)
      return;
    if (Object.op_Equality((Object) this.pcController, (Object) null) || !this.pcController.isInitialized)
    {
      this.pcController = ((Component) this).GetComponentInParent<ParticleClippingController>();
    }
    else
    {
      Vector3 vector3 = Vector3.op_Multiply(Vector3.op_Multiply(((Component) this.psRenderer).transform.parent.lossyScale, this.pcController.UIRootHeight), 0.5f);
      ((Component) this.psRenderer).transform.localScale = new Vector3(this.pcController.resolutionFactor * vector3.x, this.pcController.resolutionFactor * vector3.y, this.pcController.resolutionFactor * vector3.z);
      this.isLocaleScaleInitialized = true;
    }
  }

  private void UpdateClipRegion()
  {
    if (Object.op_Equality((Object) this.pcController, (Object) null) || !this.pcController.isInitialized)
    {
      this.pcController = ((Component) this).GetComponentInParent<ParticleClippingController>();
    }
    else
    {
      ((Renderer) this.psRenderer).material.SetVector("_WorldClipRegion", this.pcController.worldClipRegion);
      this.isClipRegionInitialized = true;
    }
  }

  private void UpdateVisibility()
  {
    if (!this.isLocaleScaleInitialized || !this.isClipRegionInitialized)
      return;
    if (Object.op_Equality((Object) this.parentRect, (Object) null))
    {
      this.parentRect = ((Component) this).GetComponentInParent<UIRect>();
    }
    else
    {
      this.currentVisibility = (double) this.parentRect.finalAlpha == 1.0;
      if (this.currentVisibility == this.previousVisibility)
        return;
      ((Component) this.psRenderer).gameObject.SetActive(this.currentVisibility);
      this.previousVisibility = this.currentVisibility;
    }
  }

  private void OnDisable()
  {
    this.isLocaleScaleInitialized = false;
    this.isClipRegionInitialized = false;
    if (!Object.op_Implicit((Object) this.psRenderer) || !Object.op_Implicit((Object) ((Component) this.psRenderer).gameObject))
      return;
    this.previousVisibility = false;
    ((Component) this.psRenderer).gameObject.SetActive(false);
  }
}
