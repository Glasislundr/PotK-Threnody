// Decompiled with JetBrains decompiler
// Type: SeaHomeInputController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SeaHomeInputController : MonoBehaviour
{
  [SerializeField]
  private SeaHomeCameraController cameraController;
  [SerializeField]
  private SeaHomeManager sceneObject;

  private void OnEnable() => UICamera.fallThrough = ((Component) this).gameObject;

  private void OnDisable() => UICamera.fallThrough = (GameObject) null;

  private void OnClick()
  {
    if (UICamera.touchCount <= 0)
      return;
    this.sceneObject.SetTouchBipObject(this.hitObject("3DModels"));
  }

  private GameObject hitObject(string layer)
  {
    RaycastHit raycastHit = new RaycastHit();
    Ray ray = this.cameraController.mainCamera.ScreenPointToRay(Vector2.op_Implicit(UICamera.lastTouchPosition));
    int num1 = 1 << LayerMask.NameToLayer(layer);
    ref RaycastHit local = ref raycastHit;
    int num2 = num1;
    return Physics.Raycast(ray, ref local, 100f, num2) ? ((Component) ((RaycastHit) ref raycastHit).collider).gameObject : (GameObject) null;
  }
}
