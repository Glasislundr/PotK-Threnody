// Decompiled with JetBrains decompiler
// Type: SeaTalkMissionRewardReceive
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
public class SeaTalkMissionRewardReceive : BackButtonMenuBase
{
  [SerializeField]
  private List<GameObject> rewardParents = new List<GameObject>();
  [SerializeField]
  private UILabel rewardContentsText;
  [SerializeField]
  private GameObject slcFrame;
  [SerializeField]
  private GameObject objItemName;
  private List<Transform> iconParents = new List<Transform>();
  private WebAPI.Response.SeaTalkReceive response;
  private bool isOnBackButton;

  public IEnumerator Init(
    SeaTalkMissionItem seaTalkMissionItem,
    WebAPI.Response.SeaTalkReceive response)
  {
    this.response = response;
    int num = ((IEnumerable<PlayerCallMissionReward>) response.rewards).Count<PlayerCallMissionReward>((Func<PlayerCallMissionReward, bool>) (x => !x.is_already_received));
    for (int index = 0; index < this.rewardParents.Count; ++index)
    {
      if (index == num - 1)
      {
        IEnumerator enumerator = this.rewardParents[index].transform.GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
            this.iconParents.Add((Transform) enumerator.Current);
          break;
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
      }
    }
    string text = "";
    for (int i = 0; i < response.rewards.Length; ++i)
    {
      PlayerCallMissionReward playerCallMissionReward = response.rewards[i];
      if (playerCallMissionReward.reward_type_id.Value == 40)
      {
        yield return (object) seaTalkMissionItem.CreateRewardIconCommonTicket(this.iconParents[i], playerCallMissionReward);
        CommonTicket commonTicket;
        MasterData.CommonTicket.TryGetValue(playerCallMissionReward.reward_id.Value, out commonTicket);
        text += commonTicket.name;
        text += " × ";
        text += (string) (object) playerCallMissionReward.reward_quantity.Value;
      }
      else if (playerCallMissionReward.is_recipe)
      {
        if (!playerCallMissionReward.is_already_received)
          yield return (object) seaTalkMissionItem.CreateRewardIconRecipe(this.iconParents[i], playerCallMissionReward);
        CallGiftRecipe callGiftRecipe = ((IEnumerable<CallGiftRecipe>) MasterData.CallGiftRecipeList).First<CallGiftRecipe>((Func<CallGiftRecipe, bool>) (x => x.success_gear_id_GearGear == playerCallMissionReward.reward_id.Value));
        text = !playerCallMissionReward.is_already_received ? text + callGiftRecipe.success_gear_id.name + "のレシピ解放" : text + callGiftRecipe.success_gear_id.name + "\nのレシピは既に解放済みです";
      }
      else
        Debug.LogError((object) string.Format("想定していない報酬タイプです {0}", (object) (MasterDataTable.CommonRewardType) playerCallMissionReward.reward_type_id.Value));
      if (i != response.rewards.Length - 1)
        text += "\n";
    }
    this.rewardContentsText.text = text;
    this.objItemName.SetActive(true);
    this.slcFrame.SetActive(true);
  }

  public void IconDisPlayEvent() => this.StartCoroutine(this.IconDisPlayEventProcess());

  private IEnumerator IconDisPlayEventProcess()
  {
    yield return (object) null;
    int num = ((IEnumerable<PlayerCallMissionReward>) this.response.rewards).Count<PlayerCallMissionReward>((Func<PlayerCallMissionReward, bool>) (x => !x.is_already_received));
    for (int index = 0; index < this.rewardParents.Count; ++index)
    {
      if (index == num - 1)
        this.rewardParents[index].SetActive(true);
      else
        this.rewardParents[index].SetActive(false);
    }
  }

  public void AnimationEndEvent() => this.isOnBackButton = true;

  public override void onBackButton()
  {
    if (!this.isOnBackButton)
      return;
    SeaTalkMessageMenu objectOfType = Object.FindObjectOfType<SeaTalkMessageMenu>();
    objectOfType.SetMissionsAndBatch(this.response.missions);
    ((Component) this).gameObject.SetActive(false);
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    this.StartCoroutine(objectOfType.OpenMissionPopup());
  }
}
