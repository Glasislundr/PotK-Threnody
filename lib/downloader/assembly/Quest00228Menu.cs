// Decompiled with JetBrains decompiler
// Type: Quest00228Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Quest00228Menu : BackButtonMenuBase
{
  [SerializeField]
  private NGxScrollMasonry Scroll;
  private List<Texture2D[]> textures;

  public virtual void Foreground()
  {
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public virtual void VScrollBar()
  {
  }

  public IEnumerator Init(QuestScoreCampaignProgress qscp)
  {
    IEnumerator e = DetailController.Init(this.Scroll, qscp.description.title, qscp.description.bodies);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator Init(Description description)
  {
    IEnumerator e = DetailController.Init(this.Scroll, description.title, description.bodies);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator Init(QuestExtraDescription[] descriptions)
  {
    IEnumerator e = DetailController.Init(this.Scroll, descriptions);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator Init(int term_id)
  {
    ClassRankingHowto classRankingHowto = ((IEnumerable<ClassRankingHowto>) MasterData.ClassRankingHowtoList).FirstOrDefault<ClassRankingHowto>((Func<ClassRankingHowto, bool>) (x => x.term_id == term_id && x.kind == 1));
    ClassRankingHowto[] array = ((IEnumerable<ClassRankingHowto>) MasterData.ClassRankingHowtoList).Where<ClassRankingHowto>((Func<ClassRankingHowto, bool>) (x => x.term_id == term_id && x.kind >= 2)).ToArray<ClassRankingHowto>();
    yield return (object) DetailController.Init(this.Scroll, classRankingHowto == null ? string.Empty : classRankingHowto.body, array);
  }

  public IEnumerator Init(GuildRaidPeriod period)
  {
    GuildRaidHowto guildRaidHowto = ((IEnumerable<GuildRaidHowto>) MasterData.GuildRaidHowtoList).FirstOrDefault<GuildRaidHowto>((Func<GuildRaidHowto, bool>) (x => x.kind == 1));
    GuildRaidHowto[] array = ((IEnumerable<GuildRaidHowto>) MasterData.GuildRaidHowtoList).Where<GuildRaidHowto>((Func<GuildRaidHowto, bool>) (x => x.kind >= 2)).ToArray<GuildRaidHowto>();
    yield return (object) DetailController.Init(this.Scroll, guildRaidHowto == null ? string.Empty : guildRaidHowto.body, array);
  }
}
