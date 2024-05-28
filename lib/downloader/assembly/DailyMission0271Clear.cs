// Decompiled with JetBrains decompiler
// Type: DailyMission0271Clear
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DailyMission0271Clear : BackButtonMonoBehaiviour
{
  [SerializeField]
  private List<GameObject> linkObjects;
  [SerializeField]
  private UISprite slcTxtBox;
  [SerializeField]
  private UILabel txtRwardMessage;
  [SerializeField]
  private GameObject[] objTragerBox;
  [SerializeField]
  private GameObject objHimeishi;
  [SerializeField]
  private GameObject dirEffect;
  private const int FONT_PIXEL_HIGHT = 38;
  private const int FRAME_HEIGHT_3_4 = 260;
  private const int LABEL_HEIGHT_3_4 = 250;
  private const int FRAME_HEIGHT_9_16 = 350;
  private const int LABEL_HEIGHT_9_16 = 340;
  private const float RATIO_9_16 = 0.5625f;
  private const float RATIO_3_4 = 0.75f;

  public void IbtnNo() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.IbtnNo();

  private void SetClearBonusHeight()
  {
    int num1 = ((UIWidget) this.slcTxtBox).height + ((UIWidget) this.txtRwardMessage).height - 38;
    float num2 = (float) (((double) Screen.width / (double) Screen.height - 9.0 / 16.0) / (3.0 / 16.0));
    int num3 = (int) Mathf.Lerp(350f, 260f, num2);
    int num4 = (int) Mathf.Lerp(340f, 250f, num2);
    if (num1 <= num3)
    {
      ((UIWidget) this.slcTxtBox).height = num1;
      this.txtRwardMessage.overflowMethod = (UILabel.Overflow) 3;
    }
    else
    {
      ((UIWidget) this.slcTxtBox).height = num3;
      this.txtRwardMessage.overflowMethod = (UILabel.Overflow) 0;
      ((UIWidget) this.txtRwardMessage).height = num4;
    }
  }

  public IEnumerator SetClearBonus(Hashtable[] Rewards)
  {
    int count = 0;
    string message = "";
    if (Rewards.Length > 1 || (int) Rewards[0][(object) "reward_type_id"] != 10)
    {
      this.objHimeishi.SetActive(false);
      ((IEnumerable<GameObject>) this.objTragerBox).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(true)));
      this.dirEffect.SetActive(true);
      this.linkObjects.ForEachIndex<GameObject>((Action<GameObject, int>) ((x, i) => x.SetActive(Rewards.Length - 1 == i)));
      foreach (Transform transform in this.linkObjects[Rewards.Length - 1].transform)
      {
        Hashtable reward = Rewards[count];
        IEnumerator e = ((Component) transform).gameObject.GetOrAddComponent<CreateIconObject>().CreateThumbnail((MasterDataTable.CommonRewardType) reward[(object) "reward_type_id"], (int) reward[(object) "reward_id"], (int) reward[(object) "reward_quantity"]);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        message = message + CommonRewardType.GetRewardName((MasterDataTable.CommonRewardType) reward[(object) "reward_type_id"], (int) reward[(object) "reward_id"], (int) reward[(object) "reward_quantity"]) + "\n";
        ++count;
        reward = (Hashtable) null;
      }
    }
    else
    {
      Hashtable reward = Rewards[0];
      this.objHimeishi.SetActive(true);
      ((IEnumerable<GameObject>) this.objTragerBox).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(false)));
      this.dirEffect.SetActive(false);
      message = CommonRewardType.GetRewardName((MasterDataTable.CommonRewardType) reward[(object) "reward_type_id"], (int) reward[(object) "reward_id"], (int) reward[(object) "reward_quantity"]);
    }
    this.txtRwardMessage.text = message;
    this.SetClearBonusHeight();
  }

  public IEnumerator SetClearBonus(BingoRewardGroup[] Rewards)
  {
    int count = 0;
    string message = "";
    if (Rewards.Length > 1 || Rewards[0].reward_type_id != MasterDataTable.CommonRewardType.coin)
    {
      this.objHimeishi.SetActive(false);
      ((IEnumerable<GameObject>) this.objTragerBox).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(true)));
      this.dirEffect.SetActive(true);
      this.linkObjects.ForEachIndex<GameObject>((Action<GameObject, int>) ((x, i) => x.SetActive(Rewards.Length - 1 == i)));
      foreach (Transform transform in this.linkObjects[Rewards.Length - 1].transform)
      {
        BingoRewardGroup reward = Rewards[count];
        IEnumerator e = ((Component) transform).gameObject.GetOrAddComponent<CreateIconObject>().CreateThumbnail(reward.reward_type_id, reward.reward_id, reward.reward_quantity);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        message = message + reward.reward_message + "\n";
        ++count;
        reward = (BingoRewardGroup) null;
      }
    }
    else
    {
      this.objHimeishi.SetActive(true);
      ((IEnumerable<GameObject>) this.objTragerBox).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(false)));
      this.dirEffect.SetActive(false);
      message = Rewards[0].reward_message;
    }
    this.txtRwardMessage.text = message;
    this.SetClearBonusHeight();
  }

  public IEnumerator SetClearBonus(DailyMission0272Panel.RewardViewModel[] Rewards)
  {
    int count = 0;
    string message = "";
    if (Rewards.Length > 1 || !Rewards[0].IsCoin)
    {
      this.objHimeishi.SetActive(false);
      ((IEnumerable<GameObject>) this.objTragerBox).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(true)));
      this.dirEffect.SetActive(true);
      this.linkObjects.ForEachIndex<GameObject>((Action<GameObject, int>) ((x, i) => x.SetActive(Rewards.Length - 1 == i)));
      foreach (Transform transform in this.linkObjects[Rewards.Length - 1].transform)
      {
        DailyMission0272Panel.RewardViewModel reward = Rewards[count];
        IEnumerator e = ((Component) transform).gameObject.GetOrAddComponent<CreateIconObject>().CreateThumbnail((MasterDataTable.CommonRewardType) reward.typeId, reward.id, reward.quantity);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        message = message + reward.RewardMessage + "\n";
        ++count;
        reward = (DailyMission0272Panel.RewardViewModel) null;
      }
    }
    else
    {
      this.objHimeishi.SetActive(true);
      ((IEnumerable<GameObject>) this.objTragerBox).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(false)));
      this.dirEffect.SetActive(false);
      message = Rewards[0].RewardMessage;
    }
    this.txtRwardMessage.text = message;
    this.SetClearBonusHeight();
  }
}
