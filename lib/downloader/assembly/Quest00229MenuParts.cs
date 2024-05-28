// Decompiled with JetBrains decompiler
// Type: Quest00229MenuParts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Quest00229MenuParts : MonoBehaviour
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
  private const int RANK_3 = 2;
  private const int RANK_4 = 3;
  private const int RANK_OTHER = 4;
  private const int RANK_9 = 9;
  private const int RANK_99 = 99;

  public GameObject GetTextDir() => this.DirObject[this.index];

  public void Init(QuestScoreRankingPlayer data)
  {
    if (data != null)
    {
      this.index = data.rank - 1 < 3 ? data.rank - 1 : 3;
      ((IEnumerable<GameObject>) this.DirObject).ToggleOnce(this.index);
      if (this.index <= 2)
        return;
      this.DirRankNumSingle.SetActive(false);
      this.DirRankNumDouble.SetActive(false);
      if (data.rank > 99)
        return;
      if (data.rank > 9)
      {
        int num = data.rank > 99 ? 99 : data.rank;
        ((IEnumerable<GameObject>) this.SlcRankNum1).ToggleOnce(num % 10);
        ((IEnumerable<GameObject>) this.SlcRankNum10).ToggleOnce(num / 10);
        this.DirRankNumDouble.SetActive(true);
      }
      else
      {
        ((IEnumerable<GameObject>) this.SlcRankNum).ToggleOnce(data.rank);
        this.DirRankNumSingle.SetActive(true);
      }
    }
    else
      ((IEnumerable<GameObject>) this.DirObject).ToggleOnce(4);
  }
}
