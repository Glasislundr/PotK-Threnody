// Decompiled with JetBrains decompiler
// Type: Colosseum02371MenuParts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Colosseum02371MenuParts : MonoBehaviour
{
  [SerializeField]
  protected GameObject[] DirObject;
  [SerializeField]
  protected GameObject DirRankNumSingle;
  [SerializeField]
  protected GameObject[] SlcRankNum;
  [SerializeField]
  protected GameObject DirRankNumDouble;
  [SerializeField]
  protected GameObject[] SlcRankNum1;
  [SerializeField]
  protected GameObject[] SlcRankNum10;
  private int index;

  public GameObject GetTextDir() => this.DirObject[this.index];

  public void Init(RankingPlayer data)
  {
    if (data != null)
    {
      this.index = data.ranking - 1 < 3 ? data.ranking - 1 : 3;
      ((IEnumerable<GameObject>) this.DirObject).ToggleOnce(this.index);
      if (this.index <= 2)
        return;
      this.DirRankNumSingle.SetActive(false);
      this.DirRankNumDouble.SetActive(false);
      if (data.ranking > 99)
        return;
      if (data.ranking > 9)
      {
        int num = data.ranking > 99 ? 99 : data.ranking;
        ((IEnumerable<GameObject>) this.SlcRankNum1).ToggleOnce(num % 10);
        ((IEnumerable<GameObject>) this.SlcRankNum10).ToggleOnce(num / 10);
        this.DirRankNumDouble.SetActive(true);
      }
      else
      {
        ((IEnumerable<GameObject>) this.SlcRankNum).ToggleOnce(data.ranking);
        this.DirRankNumSingle.SetActive(true);
      }
    }
    else
      ((IEnumerable<GameObject>) this.DirObject).ToggleOnce(4);
  }
}
