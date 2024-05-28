// Decompiled with JetBrains decompiler
// Type: CorpsStageListItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/CorpsQuest/StageListItem")]
public class CorpsStageListItem : MonoBehaviour
{
  [SerializeField]
  private GameObject Slc_clear;
  [Space(8f)]
  [SerializeField]
  private UIButton SpriteButton;
  [Space(8f)]
  [SerializeField]
  private UILabel LblStageName;
  [SerializeField]
  private Color[] LblColor = new Color[4]
  {
    Color.white,
    Color.gray,
    Color.white,
    Color.gray
  };
  [SerializeField]
  private UILabel.Effect[] LblEffectStyle = new UILabel.Effect[4];
  [SerializeField]
  private Vector2[] LblEffectDistance = new Vector2[4];
  [SerializeField]
  private Color[] LblEffectColor = new Color[4]
  {
    Color.black,
    Color.black,
    Color.black,
    Color.black
  };
  private int BannerID;
  private bool IsBoss;

  public int Floor { get; private set; }

  public int StageID { get; private set; }

  public string Name { get; private set; }

  public bool IsCleared { get; private set; }

  public bool IsRewardGotten { get; private set; }

  public void Init(
    CorpsStage stage,
    bool isCleared,
    bool isRewardGotten,
    Action<GameObject> clickAction)
  {
    this.Floor = stage.number;
    this.StageID = stage.ID;
    this.BannerID = stage.banner_id;
    this.IsBoss = stage.isBoss;
    this.Name = stage.name;
    this.IsCleared = isCleared;
    this.IsRewardGotten = isRewardGotten;
    this.LblStageName.SetTextLocalize(this.Name);
    this.SpriteButton.onClick.Clear();
    this.SpriteButton.onClick.Add(new EventDelegate((EventDelegate.Callback) (() => clickAction(((Component) this).gameObject))));
    this.UpdateView();
  }

  private void UpdateView()
  {
    this.Slc_clear.SetActive(this.IsCleared);
    this.SpriteButton.normalSprite = string.Format("slc{0}_{1}{2}.png", this.IsBoss ? (object) "_Boss" : (object) string.Empty, (object) this.BannerID, this.IsCleared ? (object) "_Lock" : (object) string.Empty);
    int index = this.IsBoss ? (this.IsCleared ? 3 : 2) : (this.IsCleared ? 1 : 0);
    ((UIWidget) this.LblStageName).color = this.LblColor[index];
    this.LblStageName.effectStyle = (UILabel.Effect) (int) this.LblEffectStyle[index];
    this.LblStageName.effectDistance = this.LblEffectDistance[index];
    this.LblStageName.effectColor = this.LblEffectColor[index];
  }
}
