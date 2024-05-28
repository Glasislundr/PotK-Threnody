// Decompiled with JetBrains decompiler
// Type: MessageButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using UnityEngine;

#nullable disable
public class MessageButton : MypageEventButton
{
  [SerializeField]
  private UILabel mNumberLabel;
  private int mMessageCount;

  public override bool IsActive() => true;

  public override bool IsBadge()
  {
    PlayerPresent[] playerPresentArray = SMManager.Get<PlayerPresent[]>();
    this.mMessageCount = 0;
    if (playerPresentArray != null)
    {
      foreach (PlayerPresent playerPresent in playerPresentArray)
      {
        if (!playerPresent.received_at.HasValue)
          ++this.mMessageCount;
      }
    }
    return this.mMessageCount > 0;
  }

  public override void SetBadgeActive(bool value)
  {
    base.SetBadgeActive(value);
    this.mNumberLabel.SetTextLocalize(string.Format("[b]{0}[-]", (object) Mathf.Min(this.mMessageCount, StaticConsts.PRESENT_COUNT_DISPLAY_MAX)));
  }
}
