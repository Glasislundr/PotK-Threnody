// Decompiled with JetBrains decompiler
// Type: DailyMission0272CollectivePopupList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class DailyMission0272CollectivePopupList : MonoBehaviour
{
  [SerializeField]
  private GameObject dirThumbImage;
  [SerializeField]
  private UILabel rewardText;
  [SerializeField]
  private GameObject lineObj;
  private DailyMission0272Panel.RewardViewModel playerPresent;

  public void Initialize(DailyMission0272Panel.RewardViewModel data, int count)
  {
    this.playerPresent = data;
    this.rewardText.SetTextLocalize(this.playerPresent.Name);
    if (count == 0)
      this.lineObj.SetActive(false);
    this.StartCoroutine(this.LoadThumb());
  }

  private IEnumerator LoadThumb()
  {
    IEnumerator e = this.playerPresent.LoadThumb(this.dirThumbImage);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
