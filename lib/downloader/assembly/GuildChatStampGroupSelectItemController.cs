// Decompiled with JetBrains decompiler
// Type: GuildChatStampGroupSelectItemController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class GuildChatStampGroupSelectItemController : MonoBehaviour
{
  public int iconStampID;
  public int stampGroupID;
  public bool isSelected;
  [SerializeField]
  private UISprite stampImage;
  [SerializeField]
  private UISprite backgroundImage;

  private void Start()
  {
  }

  private void Update()
  {
  }

  public void InitializeGuildChatStampGroupItem(int stampGroupID, int iconStampID)
  {
    this.stampGroupID = stampGroupID;
    this.iconStampID = iconStampID;
    Singleton<CommonRoot>.GetInstance().guildChatManager.SetStampSprite(this.stampImage, this.iconStampID);
  }

  public void SetSelected(bool isSelected)
  {
    this.isSelected = isSelected;
    if (isSelected)
    {
      ((UIWidget) this.stampImage).color = Color.white;
      ((Component) this.backgroundImage).gameObject.SetActive(true);
    }
    else
    {
      ((UIWidget) this.stampImage).color = new Color(0.5f, 0.5f, 0.5f);
      ((Component) this.backgroundImage).gameObject.SetActive(false);
    }
  }

  public void OnStampGroupItemClicked()
  {
    Singleton<CommonRoot>.GetInstance().guildChatManager.stampSelectViewController.SelectStampGroup(this.stampGroupID);
  }
}
