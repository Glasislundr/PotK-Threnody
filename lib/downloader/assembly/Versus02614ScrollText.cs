// Decompiled with JetBrains decompiler
// Type: Versus02614ScrollText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Versus02614ScrollText : MonoBehaviour
{
  [SerializeField]
  protected GameObject LinkUnit;
  [SerializeField]
  protected UI2DSprite Emblem;
  [SerializeField]
  protected UILabel TxtPT;
  [SerializeField]
  protected UILabel TxtRank;
  [SerializeField]
  protected UILabel TxtLose;
  [SerializeField]
  protected UILabel TxtVictory;
  [SerializeField]
  protected UILabel TxtTotalVictory;
  [SerializeField]
  protected UILabel TxtPlayerName;
  [SerializeField]
  protected UILabel TxtPlayerPoint;
  [SerializeField]
  protected UILabel TxtPlayerRank;

  public IEnumerator Init(PvPRankingPlayer data, GameObject iconPrefab)
  {
    if (data != null)
    {
      UnitIcon ui = iconPrefab.CloneAndGetComponent<UnitIcon>(this.LinkUnit.transform);
      UnitUnit unit = MasterData.UnitUnit[data.leader_unit_id];
      PlayerUnit byUnitunit = PlayerUnit.create_by_unitunit(unit, data.leader_unit_level);
      byUnitunit.job_id = data.leader_unit_job_id;
      IEnumerator e = ui.SetUnit(byUnitunit, unit.GetElement(), false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ui.setLevelText(data.leader_unit_level.ToLocalizeNumberText());
      ui.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
      this.TxtPlayerName.SetTextLocalize(data.name);
      ((Component) this.TxtVictory).gameObject.SetActive(false);
      ((Component) this.TxtLose).gameObject.SetActive(false);
      ((Component) this.TxtTotalVictory).gameObject.SetActive(true);
      this.TxtTotalVictory.SetTextLocalize(Consts.Format(Consts.GetInstance().COLOSSEUM_002371_WIN, (IDictionary) new Hashtable()
      {
        {
          (object) "cnt",
          (object) data.total_win
        }
      }));
      this.TxtPlayerPoint.SetTextLocalize(data.ranking_pt);
      this.TxtPlayerRank.SetTextLocalize(MasterData.PvpClassKind[data.current_class_id].name);
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
