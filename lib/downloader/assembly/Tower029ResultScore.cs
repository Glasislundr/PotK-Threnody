// Decompiled with JetBrains decompiler
// Type: Tower029ResultScore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Tower029ResultScore : ResultMenuBase
{
  [SerializeField]
  private GameObject objResultScore;
  [SerializeField]
  private GameObject dir_Action_num;
  [SerializeField]
  private UILabel lblActionTitle;
  [SerializeField]
  private UILabel lblActionValue;
  [SerializeField]
  private UILabel lblActionPoint;
  [SerializeField]
  private GameObject dir_Unable;
  [SerializeField]
  private UILabel lblUnableTitle;
  [SerializeField]
  private UILabel lblUnableValue;
  [SerializeField]
  private UILabel lblUnablePoint;
  [SerializeField]
  private GameObject dir_Damage;
  [SerializeField]
  private UILabel lblDamageTitle;
  [SerializeField]
  private UILabel lblDamageValue;
  [SerializeField]
  private UILabel lblDamagePoint;
  [SerializeField]
  private GameObject dir_Title;
  [SerializeField]
  private UILabel lblTitle;
  private NGSoundManager SM;
  private WebAPI.Response.TowerBattleFinish result;
  private bool isShowDamage;

  public bool IsShowDamage
  {
    set => this.isShowDamage = value;
    get => this.isShowDamage;
  }

  private void Awake() => this.SM = Singleton<NGSoundManager>.GetInstance();

  private IEnumerator ShowActionObj()
  {
    if (this.result != null)
    {
      this.lblActionValue.SetTextLocalize(this.result.finish_turn_count);
      this.lblActionPoint.SetTextLocalize(Consts.Format(Consts.GetInstance().TOWER_RESULT_POINT, (IDictionary) new Hashtable()
      {
        {
          (object) "point",
          (object) this.result.finish_turn_score
        }
      }));
      this.dir_Action_num.SetActive(true);
      yield return (object) new WaitForSeconds(1.5f);
    }
  }

  private IEnumerator ShowUnableObj()
  {
    if (this.result != null)
    {
      this.lblUnableValue.SetTextLocalize(this.result.finish_unit_death_count);
      this.lblUnablePoint.SetTextLocalize(Consts.Format(Consts.GetInstance().TOWER_RESULT_POINT, (IDictionary) new Hashtable()
      {
        {
          (object) "point",
          (object) this.result.finish_unit_death_score
        }
      }));
      this.dir_Unable.SetActive(true);
      yield return (object) new WaitForSeconds(1.5f);
    }
  }

  private IEnumerator ShowDamageObj()
  {
    if (this.result != null && this.isShowDamage)
    {
      this.lblDamageValue.SetTextLocalize(this.result.finish_overkill_damage);
      this.lblDamagePoint.SetTextLocalize(Consts.Format(Consts.GetInstance().TOWER_RESULT_POINT, (IDictionary) new Hashtable()
      {
        {
          (object) "point",
          (object) this.result.finish_overkill_score
        }
      }));
      this.dir_Damage.SetActive(true);
      yield return (object) new WaitForSeconds(1.5f);
    }
  }

  private void SetTitleLabel(WebAPI.Response.TowerBattleFinish result)
  {
    string text = string.Empty;
    TowerPeriod towerPeriod = ((IEnumerable<TowerPeriod>) MasterData.TowerPeriodList).FirstOrDefault<TowerPeriod>((Func<TowerPeriod, bool>) (x => x.ID == result.period_id));
    if (towerPeriod != null && towerPeriod.floor_id != null)
    {
      string str = string.Format("{0}{1}{2}", towerPeriod.floor_id.prefix == null ? (object) string.Empty : (object) towerPeriod.floor_id.prefix, (object) (result.floor * towerPeriod.floor_id.interval), towerPeriod.floor_id.suffix == null ? (object) string.Empty : (object) towerPeriod.floor_id.suffix);
      text = string.Format("{0} {1}", (object) towerPeriod.tower_name, (object) str);
    }
    this.lblTitle.SetTextLocalize(text);
  }

  public override IEnumerator Init(BattleInfo info, WebAPI.Response.TowerBattleFinish result)
  {
    this.lblActionTitle.SetTextLocalize(Consts.GetInstance().TOWER_RESULT_ACTION_COUNT_TITLE);
    this.lblUnableTitle.SetTextLocalize(Consts.GetInstance().TOWER_RESULT_RESURRECTION_TITLE);
    this.lblDamageTitle.SetTextLocalize(Consts.GetInstance().TOWER_RESULT_DAMAGE_TITLE);
    this.SetTitleLabel(result);
    this.dir_Action_num.SetActive(false);
    this.dir_Unable.SetActive(false);
    this.dir_Damage.SetActive(false);
    this.objResultScore.SetActive(false);
    this.result = result;
    yield break;
  }

  public override IEnumerator Run()
  {
    Tower029ResultScore tower029ResultScore = this;
    tower029ResultScore.dir_Title.SetActive(true);
    tower029ResultScore.objResultScore.SetActive(true);
    Tower029ResultScore.Runner[] runnerArray = new Tower029ResultScore.Runner[3]
    {
      new Tower029ResultScore.Runner(tower029ResultScore.ShowActionObj),
      new Tower029ResultScore.Runner(tower029ResultScore.ShowUnableObj),
      new Tower029ResultScore.Runner(tower029ResultScore.ShowDamageObj)
    };
    for (int index = 0; index < runnerArray.Length; ++index)
    {
      IEnumerator e = runnerArray[index]();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    runnerArray = (Tower029ResultScore.Runner[]) null;
  }

  public override IEnumerator OnFinish()
  {
    this.dir_Title.SetActive(false);
    this.objResultScore.SetActive(false);
    yield break;
  }

  private delegate IEnumerator Runner();
}
