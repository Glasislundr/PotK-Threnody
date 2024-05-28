// Decompiled with JetBrains decompiler
// Type: SwitchUnitComponentBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SwitchUnitComponentBase : MonoBehaviour
{
  protected SwitchUnitComponentBase.MATERIALTYPE materialType;
  protected SwitchUnitComponentBase.UnitType currUnitType;

  protected virtual void Init()
  {
  }

  public virtual void SwitchMaterial(int UnitID)
  {
    this.Init();
    this.currUnitType = SwitchUnitComponentBase.UnitType.DefaultUnit;
    foreach (KeyValuePair<int, UnitComponent> keyValuePair in MasterData.UnitComponent)
    {
      if (keyValuePair.Value.UnitID == UnitID)
      {
        this.currUnitType = SwitchUnitComponentBase.UnitType.MadokaMagica;
        break;
      }
    }
  }

  protected enum MATERIALTYPE
  {
    Sprite,
    Image,
    Label,
    Effect,
  }

  protected enum UnitType
  {
    DefaultUnit,
    MadokaMagica,
  }
}
