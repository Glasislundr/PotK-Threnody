// Decompiled with JetBrains decompiler
// Type: DeckOrganization.Unit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace DeckOrganization
{
  public class Unit
  {
    private List<int> trackIndex_ = new List<int>();

    public PlayerUnit unit_ { get; private set; }

    public bool isRegular_ { get; private set; }

    public Judgement.NonBattleParameter param_ { get; private set; }

    public UnitGroup unitGroup_ { get; private set; }

    public int index_ { get; private set; }

    public List<int> trackIndices_ => this.trackIndex_;

    public bool hasTrackIndex => this.trackIndex_.Any<int>();

    public Unit(PlayerUnit u, bool bregular = false, int index = -1)
    {
      this.unit_ = u;
      this.isRegular_ = bregular;
      this.param_ = u.nonbattleParameter;
      this.index_ = index;
      this.unitGroup_ = ((IEnumerable<UnitGroup>) MasterData.UnitGroupList).FirstOrDefault<UnitGroup>((Func<UnitGroup, bool>) (ug => ug.unit_id == u.unit.ID));
    }

    public void setIndex(int index = -1)
    {
      if (this.index_ >= 0)
        this.trackIndex_.Add(this.index_);
      this.index_ = index;
    }

    public bool hasIndex => this.index_ >= 0;
  }
}
