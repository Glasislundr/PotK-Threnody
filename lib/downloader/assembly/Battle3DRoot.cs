// Decompiled with JetBrains decompiler
// Type: Battle3DRoot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Battle3DRoot : BattleMonoBehaviour
{
  public Transform mapPoint;
  public Transform panelPoint;
  private Transform panels;
  private Transform units;

  protected override IEnumerator Start_Battle()
  {
    this.initialize();
    yield break;
  }

  public void initialize()
  {
    if (!Object.op_Equality((Object) this.panels, (Object) null) && !Object.op_Equality((Object) this.units, (Object) null))
      return;
    Singleton<NGBattleManager>.GetInstance().battleCamera = ((Component) ((Component) this).gameObject.transform.GetChildInFind("CameraNode")).gameObject;
    this.panels = ((Component) this.panelPoint).transform.GetChildInFind("Panels");
    this.units = ((Component) this.panelPoint).transform.GetChildInFind("Units");
  }

  public void objectsAcitve(bool active)
  {
    foreach (Component componentsInChild in ((Component) this).GetComponentsInChildren<Camera>(true))
      componentsInChild.gameObject.SetActive(active);
    if (Object.op_Inequality((Object) this.panels, (Object) null))
      ((Component) this.panels).gameObject.SetActive(active);
    if (Object.op_Inequality((Object) this.units, (Object) null))
    {
      foreach (BattleUnitParts componentsInChild in ((Component) this.units).GetComponentsInChildren<BattleUnitParts>())
        componentsInChild.setActive(active);
    }
    ((Component) this.mapPoint).gameObject.SetActive(active);
  }
}
