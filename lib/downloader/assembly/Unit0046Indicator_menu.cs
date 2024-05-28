// Decompiled with JetBrains decompiler
// Type: Unit0046Indicator_menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit0046Indicator_menu : MonoBehaviour
{
  public Unit0046IndiStatus[] status = new Unit0046IndiStatus[5];
  public Unit0046IndiThumb[] Thumb = new Unit0046IndiThumb[5];

  public IEnumerator SetStatus(PlayerDeck pDeck)
  {
    int index1 = 0;
    PlayerUnit[] playerUnitArray = pDeck.player_units;
    for (int index2 = 0; index2 < playerUnitArray.Length; ++index2)
    {
      PlayerUnit pUnit = playerUnitArray[index2];
      if (index1 >= 5)
      {
        yield break;
      }
      else
      {
        if (pUnit == (PlayerUnit) null)
        {
          this.status[index1].RemoveText();
          ((Component) this.Thumb[index1]).gameObject.SetActive(false);
        }
        else
        {
          this.status[index1].SetText(pUnit);
          ((Component) this.Thumb[index1]).gameObject.SetActive(true);
          IEnumerator e = this.Thumb[index1].SetThumb(pUnit);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        ++index1;
      }
    }
    playerUnitArray = (PlayerUnit[]) null;
    for (int index = index1; index < 5; ++index)
      ((Component) this.Thumb[index1]).gameObject.SetActive(false);
  }
}
