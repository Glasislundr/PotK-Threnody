// Decompiled with JetBrains decompiler
// Type: Versus02614ScrollParts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Versus02614ScrollParts : MonoBehaviour
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
  [SerializeField]
  protected GameObject[] slcTotalWin;
  [SerializeField]
  protected GameObject[] slcWin;
  [SerializeField]
  private UIButton button;
  private int index;
  private readonly int TOP3 = 2;
  private readonly int ONE_COLUMN_LIMIT = 9;
  private readonly int DOUBLE_DIGIT_LIMIT = 99;

  public GameObject GetTextDir() => this.DirObject[this.index];

  public void Init(PvPRankingPlayer data)
  {
    if (data != null)
    {
      this.index = data.ranking - 1 <= this.TOP3 ? data.ranking - 1 : this.TOP3 + 1;
      ((IEnumerable<GameObject>) this.DirObject).ToggleOnce(this.index);
      ((IEnumerable<GameObject>) this.slcTotalWin).ToggleOnce(this.index);
      ((IEnumerable<GameObject>) this.slcWin).ToggleOnce(-1);
      if (this.index > this.TOP3)
      {
        this.DirRankNumSingle.SetActive(false);
        this.DirRankNumDouble.SetActive(false);
        if (data.ranking <= this.DOUBLE_DIGIT_LIMIT)
        {
          if (data.ranking > this.ONE_COLUMN_LIMIT)
          {
            int ranking = data.ranking;
            ((IEnumerable<GameObject>) this.SlcRankNum1).ToggleOnce(ranking % 10);
            ((IEnumerable<GameObject>) this.SlcRankNum10).ToggleOnce(ranking / 10);
            this.DirRankNumDouble.SetActive(true);
          }
          else
          {
            ((IEnumerable<GameObject>) this.SlcRankNum).ToggleOnce(data.ranking);
            this.DirRankNumSingle.SetActive(true);
          }
        }
      }
      if (!Object.op_Inequality((Object) this.button, (Object) null))
        return;
      this.button.onClick.Clear();
      this.button.onClick.Add(new EventDelegate((EventDelegate.Callback) (() => this.onClickBar(data))));
    }
    else
      ((IEnumerable<GameObject>) this.DirObject).ToggleOnce(4);
  }

  public void onClickBar(PvPRankingPlayer data)
  {
    Debug.Log((object) "===ChangeScene 008_6");
    Singleton<NGSceneManager>.GetInstance().changeScene("friend008_6", true, (object) data.player_id, (object) Friend0086Scene.Mode.Friend, (object) Res.Prefabs.BackGround.MultiBackground);
  }
}
