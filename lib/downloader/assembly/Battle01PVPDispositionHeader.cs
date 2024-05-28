// Decompiled with JetBrains decompiler
// Type: Battle01PVPDispositionHeader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Battle01PVPDispositionHeader : NGBattleMenuBase
{
  [SerializeField]
  private UILabel toVictory;
  [SerializeField]
  private UILabel toVictorySub;
  [SerializeField]
  private UILabel matching;

  protected override IEnumerator Start_Battle()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Battle01PVPDispositionHeader dispositionHeader = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    IGameEngine gameEngine = dispositionHeader.battleManager.gameEngine;
    string v1 = "";
    string v2 = "";
    switch (gameEngine)
    {
      case PVPManager _:
        PVPManager pvpManager = gameEngine as PVPManager;
        MpStage stage1 = pvpManager.stage;
        v1 = stage1.victory_condition;
        v2 = stage1.victory_sub_condition;
        dispositionHeader.setText(dispositionHeader.matching, dispositionHeader.matchingText(pvpManager.matchingType));
        break;
      case PVNpcManager _:
        PVNpcManager pvNpcManager = gameEngine as PVNpcManager;
        MpStage stage2 = pvNpcManager.stage;
        v1 = stage2.victory_condition;
        v2 = stage2.victory_sub_condition;
        dispositionHeader.setText(dispositionHeader.matching, dispositionHeader.matchingText(pvNpcManager.matchingType));
        break;
      case GVGManager _:
        GVGManager gvgManager = gameEngine as GVGManager;
        v1 = gvgManager.victory_condition;
        v2 = gvgManager.victory_sub_condition;
        break;
    }
    dispositionHeader.setText(dispositionHeader.toVictory, v1);
    dispositionHeader.setText(dispositionHeader.toVictorySub, v2);
    return false;
  }

  private string matchingText(PvpMatchingTypeEnum type)
  {
    Consts instance = Consts.GetInstance();
    switch (type)
    {
      case PvpMatchingTypeEnum.friend:
      case PvpMatchingTypeEnum.guest:
        return instance.PVP_MATCHING_TYPE_FREAND;
      case PvpMatchingTypeEnum.class_match:
        return instance.PVP_MATCHING_TYPE_CLASS;
      default:
        return instance.PVP_MATCHING_TYPE_RANDOM;
    }
  }
}
