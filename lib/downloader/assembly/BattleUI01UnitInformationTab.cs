// Decompiled with JetBrains decompiler
// Type: BattleUI01UnitInformationTab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public abstract class BattleUI01UnitInformationTab : MonoBehaviour
{
  protected BattleUI01UnitInformation main_ { get; private set; }

  protected BL.Unit unit_ { get; private set; }

  public abstract IEnumerator initialize();

  public abstract BattleUI01UnitInformationTab.Type type { get; }

  public void preInitialize(BattleUI01UnitInformation main, BL.Unit unit)
  {
    this.main_ = main;
    this.unit_ = unit;
  }

  public bool isEnabled
  {
    get => ((Component) this).gameObject.activeSelf;
    set => ((Component) this).gameObject.SetActive(value);
  }

  public enum Type
  {
    Skill,
    Weapon,
    Option,
  }
}
