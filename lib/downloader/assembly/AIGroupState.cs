// Decompiled with JetBrains decompiler
// Type: AIGroupState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public class AIGroupState
{
  private Dictionary<int, AIGroupState.AIGroup> groups = new Dictionary<int, AIGroupState.AIGroup>();

  public AIGroupState(IEnumerable<BL.AIUnit> units)
  {
    Dictionary<int, List<BL.AIUnit>> dictionary = new Dictionary<int, List<BL.AIUnit>>();
    foreach (BL.AIUnit aiUnit in (IEnumerable<BL.AIUnit>) units.OrderBy<BL.AIUnit, int>((Func<BL.AIUnit, int>) (x => x.unitPosition.unit.AIMoveGroupOrder)))
    {
      if (aiUnit.unitPosition.unit.IsAIMoveGroup)
      {
        int aiMoveGroup = aiUnit.unitPosition.unit.AIMoveGroup;
        List<BL.AIUnit> aiUnitList;
        if (dictionary.TryGetValue(aiMoveGroup, out aiUnitList))
          aiUnitList.Add(aiUnit);
        else
          dictionary[aiMoveGroup] = new List<BL.AIUnit>()
          {
            aiUnit
          };
      }
      foreach (KeyValuePair<int, List<BL.AIUnit>> keyValuePair in dictionary)
        this.groups[keyValuePair.Key] = new AIGroupState.AIGroup(keyValuePair.Value);
    }
  }

  public void NotifyAIUnit(BL env, BL.AIUnit aiUnit)
  {
    AIGroupState.AIGroup aiGroup;
    if ((!this.groups.TryGetValue(aiUnit.unitPosition.unit.AIMoveGroup, out aiGroup) ? 0 : (aiGroup.Leader == aiUnit ? 1 : 0)) == 0)
      return;
    aiGroup.TargetPanel = env.getFieldPanel((BL.UnitPosition) aiUnit);
    if (aiUnit.hp > 0 || aiGroup.Followers.Count <= 0)
      return;
    aiGroup.Leader = aiGroup.Followers.Pop();
    aiGroup.TargetPanel = (BL.Panel) null;
  }

  public BL.Panel GetMovePanel(BL.AIUnit aiUnit)
  {
    AIGroupState.AIGroup aiGroup;
    return this.groups.TryGetValue(aiUnit.unitPosition.unit.AIMoveGroup, out aiGroup) && aiGroup.Followers.Contains(aiUnit) ? aiGroup.TargetPanel : (BL.Panel) null;
  }

  private class AIGroup
  {
    public BL.AIUnit Leader;
    public Stack<BL.AIUnit> Followers;
    public BL.Panel TargetPanel;

    public AIGroup(List<BL.AIUnit> units)
    {
      this.Leader = units[0];
      this.Followers = new Stack<BL.AIUnit>(units.Skip<BL.AIUnit>(1));
    }
  }
}
