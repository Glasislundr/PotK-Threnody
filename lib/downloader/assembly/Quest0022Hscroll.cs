// Decompiled with JetBrains decompiler
// Type: Quest0022Hscroll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Quest0022Hscroll : QuestHScrollButton
{
  private int missionNum;
  private int missionClearNum;
  private bool storyOnly;
  private QuestStageMenuBase stageMenu;
  private CommonQuestType questType;

  public int MissionNum
  {
    get => this.missionNum;
    set => this.missionNum = value;
  }

  public int MissionClearNum
  {
    get => this.missionClearNum;
    set => this.missionClearNum = value;
  }

  public bool StoryOnly
  {
    set => this.storyOnly = value;
    get => this.storyOnly;
  }

  public bool CanPlay => this.canPlay;

  public float defaultPosition() => ((Component) this).transform.localPosition.x;

  public float spaceValue() => this.SpaceValue;

  public int id() => this.ID;

  public CommonQuestType QuestType
  {
    get => this.questType;
    set => this.questType = value;
  }

  public int stageNumber() => this.StageNumber;

  public IEnumerator Init(
    QuestStageMenuBase menu,
    QuestConverterData StageData,
    float gridWidth,
    int center,
    Action act,
    bool eventquest,
    bool storyOnly,
    EventDelegate.Callback onLongPressed)
  {
    Quest0022Hscroll quest0022Hscroll = this;
    quest0022Hscroll.stageMenu = menu;
    quest0022Hscroll.storyOnly = storyOnly;
    int numSInThisM = StageData.numS_in_thisM;
    int? buttonFolderId = StageData.button_folder_ID;
    quest0022Hscroll.canPlay = StageData.canPlay;
    string NumberSpritePath = quest0022Hscroll.SetStageNumSpritePath(buttonFolderId, numSInThisM, eventquest);
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = quest0022Hscroll.\u003C\u003En__0(numSInThisM, gridWidth, center, NumberSpritePath, StageData.hscroll_bg_name, eventquest, storyOnly);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    quest0022Hscroll.InitValue(StageData.lost_ap, StageData.is_new, StageData.is_clear, StageData.has_reward, gridWidth, center, StageData.clear_rewards);
    quest0022Hscroll.ID = StageData.id_S;
    quest0022Hscroll.QuestType = StageData.type;
    StageAvailibilityCheckHelper stageCheckHelper = ((Component) quest0022Hscroll).gameObject.GetOrAddComponent<StageAvailibilityCheckHelper>();
    EventDelegate.Set(quest0022Hscroll.StageButton.onClick, (EventDelegate.Callback) (() =>
    {
      NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
      if (Object.op_Inequality((Object) instance, (Object) null))
        instance.playSE(Singleton<NGGameDataManager>.GetInstance().IsSea ? "SE_1040" : "SE_1002");
      this.StartCoroutine(stageCheckHelper.PopupJudge(StageData, (Action<Action>) (endWait =>
      {
        act();
        if (endWait == null)
          return;
        endWait();
      }), (Action<PopupUtility.SceneTo>) null, eventquest, storyOnly));
    }));
    quest0022Hscroll.StageButton.onLongPress_.Clear();
    if (onLongPressed != null && !quest0022Hscroll.StoryOnly)
      EventDelegate.Set(quest0022Hscroll.StageButton.onLongPress_, onLongPressed);
  }

  public string SetStageNumSpritePath(int? folderid, int num, bool Event)
  {
    string str1 = folderid.HasValue ? folderid.Value.ToString() : (Singleton<NGGameDataManager>.GetInstance().IsSea ? "sea" : "0");
    string str2 = !Event ? "Prefabs/Quest/Story/DifficultImage/" : "Prefabs/Quest/Extra/DifficultImage/";
    string path = str2 + string.Format("{0}/{1}a", (object) str1, (object) num);
    string str3;
    if (!Singleton<ResourceManager>.GetInstance().Contains(path))
    {
      Debug.LogError((object) ("NotFoundResourcesPath : " + path));
      str3 = str2 + "0/1a";
    }
    else
      str3 = path;
    return str3;
  }
}
