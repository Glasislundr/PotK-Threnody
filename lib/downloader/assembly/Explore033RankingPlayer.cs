// Decompiled with JetBrains decompiler
// Type: Explore033RankingPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Explore033RankingPlayer : MonoBehaviour
{
  private WebAPI.Response.ExploreRankingRankingRankings initData;
  [SerializeField]
  private UISprite[] baseBG;
  [SerializeField]
  private GameObject dirRank;
  [SerializeField]
  private UILabel labelRank;
  [SerializeField]
  private UILabel labelPlayerName;
  [SerializeField]
  private UILabel labelFloor;
  [SerializeField]
  private UILabel labelDefeat;
  private string rankingPlayerID;

  public void Initialize(WebAPI.Response.ExploreRankingRankingRankings data = null)
  {
    this.initData = data;
    if (data == null)
      return;
    this.labelPlayerName.SetTextLocalize(this.initData.name);
    this.labelFloor.SetTextLocalize(this.initData.current_floor);
    this.labelDefeat.SetTextLocalize(this.initData.defeat_count);
    for (int index = 0; index < this.baseBG.Length; ++index)
      ((Component) ((Component) this.baseBG[index]).transform.parent).gameObject.SetActive(false);
    this.SetRankBaseBG(this.initData.ranking);
    this.dirRank.SetActive(false);
    this.labelRank.text = "";
    if (this.initData.ranking <= 3 && this.initData.ranking != 0)
      return;
    this.SetRankNum(this.initData.ranking);
  }

  private void SetRankBaseBG(int rank)
  {
    if (rank > 3 || rank == 0)
      return;
    ((Component) ((Component) this.baseBG[rank - 1]).transform.parent).gameObject.SetActive(true);
  }

  private void SetRankNum(int rank)
  {
    this.dirRank.SetActive(true);
    if (rank == 0)
      this.labelRank.SetTextLocalize("--");
    else
      this.labelRank.SetTextLocalize(rank);
  }
}
