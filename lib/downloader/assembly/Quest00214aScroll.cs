// Decompiled with JetBrains decompiler
// Type: Quest00214aScroll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Quest00214aScroll : MonoBehaviour
{
  public UILabel label;
  public GameObject charcter;

  public IEnumerator Init(GameObject iconPreafab, QuestDisplayConditionConverter data)
  {
    this.label.SetText(data.name);
    IEnumerator e = ColosseumUtility.CreateUnitIcon(iconPreafab, MasterData.UnitUnit[data.unit_UnitUnit], this.charcter.transform);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
