// Decompiled with JetBrains decompiler
// Type: Transfer01281Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Transfer01281Menu : NGMenuBase
{
  [SerializeField]
  private GameObject MigrateSelect;
  [SerializeField]
  private GameObject FgGIDMigrate;

  public virtual void IbtnPopupNext()
  {
    this.MigrateSelect.SetActive(true);
    ((Component) this).gameObject.SetActive(false);
  }

  public virtual void IbtnPopupBack() => ((Component) this).gameObject.SetActive(false);
}
