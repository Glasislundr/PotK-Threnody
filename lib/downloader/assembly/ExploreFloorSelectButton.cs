// Decompiled with JetBrains decompiler
// Type: ExploreFloorSelectButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using UnityEngine;

#nullable disable
public class ExploreFloorSelectButton : MonoBehaviour
{
  [SerializeField]
  private UILabel mFloorNameLbl;
  [SerializeField]
  private UIButton mFloorImgButton;
  [SerializeField]
  private GameObject mNowFloorBadge;
  private int mFloorId;
  private ExploreFloorSelectPopup mBasePopup;

  public void Initialize(ExploreFloorSelectPopup basePopup, int floorId)
  {
    this.mBasePopup = basePopup;
    this.mFloorId = floorId;
    ExploreFloor exploreFloor = MasterData.ExploreFloor[this.mFloorId];
    this.mFloorImgButton.normalSprite = string.Format("ibtn_HierarchySelection_{0}_{1:000}.png__GUI__explore_HierarchyBtn__explore_HierarchyBtn_prefab", (object) exploreFloor.folder_path, (object) exploreFloor.map_near_id);
    this.mFloorNameLbl.SetTextLocalize(exploreFloor.name);
    if (this.mFloorId == Singleton<ExploreDataManager>.GetInstance().FloorData.ID)
      this.mNowFloorBadge.SetActive(true);
    else
      this.mNowFloorBadge.SetActive(false);
  }

  public void OnFloorMoveButton() => this.mBasePopup.OnFloorMoveButton(this.mFloorId);

  public void OnFloorInfoButton() => this.mBasePopup.OnFloorInfoButton(this.mFloorId);
}
