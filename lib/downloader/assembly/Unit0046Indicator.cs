// Decompiled with JetBrains decompiler
// Type: Unit0046Indicator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnitStatusInformation;
using UnityEngine;

#nullable disable
public class Unit0046Indicator : MonoBehaviour
{
  [SerializeField]
  private UILabel TxtLeaderskillDescription;
  [SerializeField]
  private UILabel TxtLeaderskillName;
  [SerializeField]
  private GameObject objLeaderSkillZoom;
  private Action popupLeaderSkillDetail_;

  public IEnumerator InitPlayerDeck(
    Player player,
    PlayerDeck playerDeck,
    GameObject leaderSkillPopupPrefab)
  {
    Unit0046Indicator unit0046Indicator = this;
    PlayerUnit playerUnit = ((IEnumerable<PlayerUnit>) playerDeck.player_units).FirstOrDefault<PlayerUnit>();
    if (playerUnit != (PlayerUnit) null)
    {
      if (playerUnit.leader_skill != null)
      {
        BattleskillSkill s = playerUnit.leader_skill.skill;
        unit0046Indicator.TxtLeaderskillName.SetTextLocalize(s.name);
        unit0046Indicator.TxtLeaderskillDescription.SetTextLocalize(s.description);
        unit0046Indicator.popupLeaderSkillDetail_ = (Action) (() => PopupSkillDetails.show(leaderSkillPopupPrefab, new PopupSkillDetails.Param(s, UnitParameter.SkillGroup.Leader)));
        unit0046Indicator.objLeaderSkillZoom.SetActive(true);
      }
      else
      {
        unit0046Indicator.TxtLeaderskillName.SetTextLocalize(Consts.GetInstance().UNIT_0046_INDICATOR_INIT_PLAYER_DECK_1);
        unit0046Indicator.TxtLeaderskillDescription.SetTextLocalize(Consts.GetInstance().UNIT_0046_INDICATOR_INIT_PLAYER_DECK_2);
        unit0046Indicator.objLeaderSkillZoom.SetActive(false);
      }
    }
    else
    {
      unit0046Indicator.TxtLeaderskillName.SetTextLocalize(Consts.GetInstance().UNIT_0046_INDICATOR_INIT_PLAYER_DECK_3);
      unit0046Indicator.TxtLeaderskillDescription.SetTextLocalize(Consts.GetInstance().UNIT_0046_INDICATOR_INIT_PLAYER_DECK_4);
      unit0046Indicator.objLeaderSkillZoom.SetActive(false);
    }
    IEnumerator e = ((Component) unit0046Indicator).GetComponent<Unit0046Indicator_menu>().SetStatus(playerDeck);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onClickedSkillZoom()
  {
    Action leaderSkillDetail = this.popupLeaderSkillDetail_;
    if (leaderSkillDetail == null)
      return;
    leaderSkillDetail();
  }
}
