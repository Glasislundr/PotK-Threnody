// Decompiled with JetBrains decompiler
// Type: GearQuestScrollItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("Popup/GearQuest/ScrollItem")]
public class GearQuestScrollItem : MonoBehaviour
{
  [SerializeField]
  private UILabel txtQuestName_;
  [SerializeField]
  private UILabel txtStageName_;
  [SerializeField]
  private UILabel txtAP_;
  [SerializeField]
  private GameObject objDisabled_;
  [SerializeField]
  private GameObject objNotSelected_;
  [SerializeField]
  private UIButton button_;
  private QuestConverterData questData_;

  public static Future<GameObject> createLoader()
  {
    return new ResourceObject("Prefabs/unit/dir_DropQuest_Menu").Load<GameObject>();
  }

  public void initialize(QuestStoryS questS)
  {
    PlayerStoryQuestS[] array = SMManager.Get<PlayerStoryQuestS[]>();
    PlayerStoryQuestS story = array != null ? Array.Find<PlayerStoryQuestS>(array, (Predicate<PlayerStoryQuestS>) (x => x._quest_story_s == questS.ID)) : (PlayerStoryQuestS) null;
    QuestConverterData questConverterData;
    if (story == null)
    {
      questConverterData = (QuestConverterData) null;
    }
    else
    {
      questConverterData = new QuestConverterData(story);
      questConverterData.no_return_scene = true;
    }
    this.questData_ = questConverterData;
    this.txtQuestName_.SetTextLocalize(questS.quest_m.name);
    this.txtStageName_.SetTextLocalize(questS.name);
    this.txtAP_.SetTextLocalize(story != null ? story.consumed_ap : questS.lost_ap);
    GameObject objDisabled = this.objDisabled_;
    QuestConverterData questData = this.questData_;
    int num = (questData != null ? (questData.canPlay ? 1 : 0) : 0) == 0 ? 1 : 0;
    objDisabled.SetActive(num != 0);
  }

  public void initialize(QuestSeaS questS)
  {
    PlayerSeaQuestS[] array = SMManager.Get<PlayerSeaQuestS[]>();
    PlayerSeaQuestS story = array != null ? Array.Find<PlayerSeaQuestS>(array, (Predicate<PlayerSeaQuestS>) (x => x._quest_sea_s == questS.ID)) : (PlayerSeaQuestS) null;
    QuestConverterData questConverterData;
    if (story == null)
    {
      questConverterData = (QuestConverterData) null;
    }
    else
    {
      questConverterData = new QuestConverterData(story);
      questConverterData.no_return_scene = true;
    }
    this.questData_ = questConverterData;
    this.txtQuestName_.SetTextLocalize(questS.quest_m.name);
    this.txtStageName_.SetTextLocalize(questS.name);
    this.txtAP_.SetTextLocalize(story != null ? story.consumed_ap : questS.lost_ap);
    GameObject objDisabled = this.objDisabled_;
    QuestConverterData questData = this.questData_;
    int num = (questData != null ? (questData.canPlay ? 1 : 0) : 0) == 0 ? 1 : 0;
    objDisabled.SetActive(num != 0);
  }

  public void initialize(QuestExtraS questS)
  {
    PlayerExtraQuestS[] array = SMManager.Get<PlayerExtraQuestS[]>();
    PlayerExtraQuestS extra = array != null ? Array.Find<PlayerExtraQuestS>(array, (Predicate<PlayerExtraQuestS>) (x => x._quest_extra_s == questS.ID)) : (PlayerExtraQuestS) null;
    QuestConverterData questConverterData;
    if (extra == null)
    {
      questConverterData = (QuestConverterData) null;
    }
    else
    {
      questConverterData = new QuestConverterData(extra);
      questConverterData.no_return_scene = true;
    }
    this.questData_ = questConverterData;
    this.txtQuestName_.SetTextLocalize(questS.quest_m.name);
    this.txtStageName_.SetTextLocalize(questS.name);
    this.txtAP_.SetTextLocalize(extra != null ? extra.consumed_ap : questS.lost_ap);
    GameObject objDisabled = this.objDisabled_;
    QuestConverterData questData = this.questData_;
    int num = (questData != null ? (questData.canPlay ? 1 : 0) : 0) == 0 ? 1 : 0;
    objDisabled.SetActive(num != 0);
  }

  public void setButton(Action<QuestConverterData> onSelected, bool bDisabledSelect)
  {
    if (!this.objDisabled_.activeSelf && onSelected != null)
    {
      this.objNotSelected_.SetActive(bDisabledSelect);
      if (!bDisabledSelect)
        EventDelegate.Set(this.button_.onClick, (EventDelegate.Callback) (() => onSelected(this.questData_)));
      else
        ((Collider) ((Component) this.button_).GetComponent<BoxCollider>()).enabled = false;
    }
    else
      ((Collider) ((Component) this.button_).GetComponent<BoxCollider>()).enabled = false;
  }
}
