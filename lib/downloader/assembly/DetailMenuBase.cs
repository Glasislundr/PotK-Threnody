// Decompiled with JetBrains decompiler
// Type: DetailMenuBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;

#nullable disable
public class DetailMenuBase : NGMenuBase
{
  protected Unit0042Menu menu;
  protected int index;

  public int Index => this.index;

  public virtual IEnumerator Init(
    Unit0042Menu menu,
    int index,
    PlayerUnit playerUnit,
    int infoIndex,
    bool isLimit,
    bool isMaterial,
    QuestScoreBonusTimetable[] tables,
    UnitBonus[] unitBonus,
    bool isUpdate = true,
    PlayerUnit baseUnit = null)
  {
    yield break;
  }

  public virtual IEnumerator SetInformationPanelIndex(int index)
  {
    yield break;
  }
}
