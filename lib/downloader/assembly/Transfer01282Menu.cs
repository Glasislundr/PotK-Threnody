// Decompiled with JetBrains decompiler
// Type: Transfer01282Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Transfer01282Menu : NGMenuBase
{
  [SerializeField]
  private GameObject Migrate;
  [SerializeField]
  private GameObject FgGIDMigrate;

  public void IbtnMigtate()
  {
    this.Migrate.SetActive(true);
    this.Migrate.GetComponent<Startup00017Menu>().InitDataCode();
    ((Component) this).gameObject.SetActive(false);
  }

  public void IbtnFgGIDMigtate()
  {
    this.FgGIDMigrate.SetActive(true);
    this.FgGIDMigrate.GetComponent<Startup00018Menu>().InitDataCode();
    ((Component) this).gameObject.SetActive(false);
  }

  public virtual void IbtnPopupBack() => ((Component) this).gameObject.SetActive(false);
}
