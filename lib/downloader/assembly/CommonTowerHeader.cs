// Decompiled with JetBrains decompiler
// Type: CommonTowerHeader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CommonTowerHeader : CommonHeaderBase
{
  [SerializeField]
  private UILabel lblPlayerName;
  [SerializeField]
  private UILabel lblStoneNum;
  [SerializeField]
  private GameObject kisekiBikkuriIcon;
  [SerializeField]
  private UILabel lblTowerMedalNum;
  [SerializeField]
  private UILabel lblFloorName;
  [SerializeField]
  private UI2DSprite slc_tower_medal_icon;
  [SerializeField]
  private UI2DSprite slc_kiseki_icon;

  public UI2DSprite Slc_tower_medal_icon
  {
    set => this.slc_tower_medal_icon = value;
    get => this.slc_tower_medal_icon;
  }

  public UI2DSprite Slc_kiseki_icon
  {
    set => this.slc_kiseki_icon = value;
    get => this.slc_kiseki_icon;
  }

  private void Start() => this.Init();

  protected override void Update()
  {
    if (this.player.Value == null)
      return;
    base.Update();
    if (!this.isChangedOncePlayer)
      return;
    this.SetHeaderStone(this.player.Value.coin);
  }

  public void SetHeaderInfo(int coin, int medal, string name, string floorName)
  {
    this.lblStoneNum.SetTextLocalize(coin);
    this.UpdateHeaderBikkuriIcon();
    this.lblPlayerName.SetTextLocalize(name);
    this.lblFloorName.SetTextLocalize(floorName);
    this.lblTowerMedalNum.SetTextLocalize(medal);
  }

  public void UpdateHeaderBikkuriIcon()
  {
    this.kisekiBikkuriIcon.SetActive(Singleton<NGGameDataManager>.GetInstance().receivableGift);
  }

  public void SetHederTowerName(string name) => this.lblFloorName.SetTextLocalize(name);

  public void SetHeaderTowerMedal(int val) => this.lblTowerMedalNum.SetTextLocalize(val);

  public void SetHeaderStone(int val) => this.lblStoneNum.SetTextLocalize(val);
}
