// Decompiled with JetBrains decompiler
// Type: GuildRaidTopLapInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using UnityEngine;

#nullable disable
public class GuildRaidTopLapInfo : MonoBehaviour
{
  [SerializeField]
  private UILabel txtLapCount;
  [SerializeField]
  private UILabel txtLapMaxCount;
  [SerializeField]
  private GameObject txtParent;
  [SerializeField]
  private UILabel txtEndlessLapCount;
  [SerializeField]
  private UILabel txtEndlessLapMaxCount;
  [SerializeField]
  private GameObject txtEndlessParent;

  public void setLapNum(int lapNow, int lapMax)
  {
    if (lapNow <= lapMax)
    {
      this.txtParent.gameObject.SetActive(true);
      this.txtEndlessParent.gameObject.SetActive(false);
      this.txtLapCount.SetTextLocalize(lapNow);
      this.txtLapMaxCount.SetTextLocalize("/{0}".F((object) lapMax));
    }
    else
    {
      this.txtParent.gameObject.SetActive(false);
      this.txtEndlessParent.gameObject.SetActive(true);
      this.txtEndlessLapMaxCount.SetTextLocalize(lapNow - lapMax);
    }
  }
}
