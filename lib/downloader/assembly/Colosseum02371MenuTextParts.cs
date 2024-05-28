// Decompiled with JetBrains decompiler
// Type: Colosseum02371MenuTextParts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Colosseum02371MenuTextParts : MonoBehaviour
{
  [SerializeField]
  protected GameObject LinkUnit;
  [SerializeField]
  protected UI2DSprite Emblem;
  [SerializeField]
  protected UILabel TxtIssue;
  [SerializeField]
  protected UILabel TxtLose;
  [SerializeField]
  protected UILabel TxtPlayerName;
  [SerializeField]
  protected UILabel TxtPlayerPoint;
  [SerializeField]
  protected UILabel TxtPlayerRank;
  [SerializeField]
  protected UILabel TxtPT;
  [SerializeField]
  protected UILabel TxtVictory;
  [SerializeField]
  protected UILabel TxtRank;

  public IEnumerator Init(RankingPlayer data, GameObject iconPrefab)
  {
    if (data != null)
    {
      UnitIcon ui = iconPrefab.CloneAndGetComponent<UnitIcon>(this.LinkUnit.transform);
      PlayerUnit byUnitunit = PlayerUnit.create_by_unitunit(MasterData.UnitUnit[data.leader_unit_id], data.leader_unit_level);
      byUnitunit.job_id = data.leader_unit_job_id;
      IEnumerator e = ui.SetUnit(byUnitunit, byUnitunit.GetElement(), false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ui.setLevelText(data.leader_unit_level.ToLocalizeNumberText());
      ui.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
      this.TxtPlayerName.SetTextLocalize(data.name);
      this.TxtVictory.SetTextLocalize(Consts.Format(Consts.GetInstance().COLOSSEUM_002371_WIN, (IDictionary) new Hashtable()
      {
        {
          (object) "cnt",
          (object) data.win
        }
      }));
      this.TxtLose.SetTextLocalize(Consts.Format(Consts.GetInstance().COLOSSEUM_002371_LOSE, (IDictionary) new Hashtable()
      {
        {
          (object) "cnt",
          (object) data.lose
        }
      }));
      this.TxtPlayerPoint.SetTextLocalize(data.rank_pt);
      this.TxtPlayerRank.SetTextLocalize(ColosseumUtility.GetRankName(data.rank_pt));
      Future<Sprite> sprF = EmblemUtility.LoadEmblemSprite(data.current_emblem_id);
      e = sprF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.Emblem.sprite2D = sprF.Result;
      ((Component) this.TxtRank).gameObject.SetActive(false);
      if (data.ranking > 99)
      {
        ((Component) this.TxtRank).gameObject.SetActive(true);
        this.TxtRank.SetTextLocalize(Consts.Format(Consts.GetInstance().COLOSSEUM_002371_RANK, (IDictionary) new Hashtable()
        {
          {
            (object) "rank",
            (object) data.ranking
          }
        }));
      }
    }
  }
}
