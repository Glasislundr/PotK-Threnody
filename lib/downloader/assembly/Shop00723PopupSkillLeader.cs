// Decompiled with JetBrains decompiler
// Type: Shop00723PopupSkillLeader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Shop00723PopupSkillLeader : MonoBehaviour
{
  [SerializeField]
  private UILabel txtName_;
  [SerializeField]
  private UILabel txtDetail_;
  [SerializeField]
  private UILabel txtFactor_;

  public void initialize(UnitBattleSkillOrigin[] datas)
  {
    int? nullable = ((IEnumerable<UnitBattleSkillOrigin>) datas).FirstIndexOrNull<UnitBattleSkillOrigin>((Func<UnitBattleSkillOrigin, bool>) (d => d != null));
    UnitBattleSkillOrigin data = datas[nullable.Value];
    this.txtName_.SetTextLocalize(data.skill_.name);
    this.txtDetail_.SetTextLocalize(data.skill_.description);
    string text = "";
    if (data.IsOriginCharacterQuest)
    {
      Consts instance = Consts.GetInstance();
      text = string.Format(instance.SHOP_00723_SKILL_ORIGIN_QUEST, (object) instance.QUEST_TYPE_NAME_CHARACTER);
    }
    if (datas.Length > nullable.Value + 1 && datas[nullable.Value + 1].IsOriginCharacterQuest)
    {
      Consts instance = Consts.GetInstance();
      text += string.Format(instance.SHOP_00723_LEADERSKILL_NEXT_QUEST, (object) instance.QUEST_TYPE_NAME_CHARACTER);
    }
    this.txtFactor_.SetTextLocalize(text);
  }

  private void Awake()
  {
    ((Collider) ((Component) this).GetComponent<BoxCollider>()).enabled = false;
  }
}
