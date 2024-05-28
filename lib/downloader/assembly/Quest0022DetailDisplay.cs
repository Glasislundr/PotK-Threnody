// Decompiled with JetBrains decompiler
// Type: Quest0022DetailDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Quest0022DetailDisplay : MonoBehaviour
{
  [SerializeField]
  private UILabel Txt_Stagename;
  [SerializeField]
  private UILabel Txt_SubCondition;
  [SerializeField]
  private UILabel Txt_Condition;
  [SerializeField]
  private UILabel Txt_Wave_Condition;
  [SerializeField]
  private UILabel Txt_Wave_Number;
  [SerializeField]
  private UI2DSprite StageNumber;
  [SerializeField]
  protected GameObject SlcHiScroeBase;
  [SerializeField]
  protected UILabel TxtHiScroe;
  [SerializeField]
  protected UILabel TxtRest;
  private static readonly Color SEA_TEXT_COLOR = new Color(0.305882365f, 0.305882365f, 0.305882365f, 1f);

  public void InitDetailDisplay(
    QuestConverterData stageData,
    int num,
    QuestScoreCampaignProgress qscp)
  {
    this.Txt_Stagename.SetTextLocalize(stageData.title_S);
    if (stageData.wave != null)
    {
      ((Component) this.Txt_Wave_Condition).gameObject.SetActive(true);
      ((Component) this.Txt_Wave_Number).gameObject.SetActive(true);
      this.Txt_Wave_Condition.SetTextLocalize(stageData.wave.victory_description);
      this.Txt_Wave_Number.SetTextLocalize(Consts.Format(Consts.GetInstance().QUEST_0022_WAVE_COUNT, (IDictionary) new Hashtable()
      {
        {
          (object) "number",
          (object) stageData.wave.wave_count
        }
      }));
      ((Component) this.Txt_Condition).gameObject.SetActive(false);
      ((Component) this.Txt_SubCondition).gameObject.SetActive(false);
    }
    else
    {
      ((Component) this.Txt_Condition).gameObject.SetActive(true);
      ((Component) this.Txt_SubCondition).gameObject.SetActive(true);
      this.Txt_Condition.SetTextLocalize(stageData.victory_name);
      this.Txt_SubCondition.SetTextLocalize(stageData.victory_sub_name);
      ((Component) this.Txt_Wave_Condition).gameObject.SetActive(false);
      ((Component) this.Txt_Wave_Number).gameObject.SetActive(false);
    }
    if (Object.op_Inequality((Object) this.TxtRest, (Object) null))
    {
      ((Component) this.TxtRest).GetComponent<UIWidget>().color = Singleton<NGGameDataManager>.GetInstance().IsSea ? Quest0022DetailDisplay.SEA_TEXT_COLOR : Color.white;
      if (stageData.remain_battle_count.HasValue)
      {
        int num1 = stageData.daily_limit > 0 ? stageData.daily_limit : stageData.max_battle_count_limit;
        int num2 = stageData.remain_battle_count.Value;
        this.TxtRest.SetTextLocalize(Consts.Format(Consts.GetInstance().QUEST_0022_LIMIT, (IDictionary) new Hashtable()
        {
          {
            (object) "remains",
            (object) num2
          },
          {
            (object) "max",
            (object) num1
          }
        }));
        if (num2 == 0)
          ((Component) this.TxtRest).GetComponent<UIWidget>().color = Color.red;
      }
      else
        this.TxtRest.SetTextLocalize(Consts.GetInstance().QUEST_0022_LIMIT_NONE);
    }
    if (Object.op_Inequality((Object) this.SlcHiScroeBase, (Object) null))
      this.SlcHiScroeBase.SetActive(false);
    if (qscp != null)
      this.SetRankingEventHiScore(qscp.GetQuestSScore(stageData.id_S));
    int num3 = stageData.button_folder_ID ?? 0;
    string str1;
    if (stageData.type == CommonQuestType.Extra)
      str1 = "Prefabs/Quest/Extra/DifficultImage/";
    else if (stageData.type == CommonQuestType.Story)
    {
      str1 = "Prefabs/Quest/Story/DifficultImage/";
    }
    else
    {
      if (stageData.type != CommonQuestType.Sea)
        return;
      str1 = "Prefabs/Quest/Story/DifficultImage/";
    }
    string str2 = str1;
    string path1 = stageData.type != CommonQuestType.Sea ? str2 + string.Format("{0}/{1}a", (object) num3, (object) num) : str2 + string.Format("sea/{0}a", (object) num);
    string path2;
    if (!Singleton<ResourceManager>.GetInstance().Contains(path1))
    {
      Debug.LogError((object) ("NotFoundResourcesPath : " + path1));
      path2 = str1 + "0/1a";
    }
    else
      path2 = path1;
    this.StartCoroutine(this.SpriteCreate(path2));
  }

  protected void SetRankingEventHiScore(int score)
  {
    this.SlcHiScroeBase.SetActive(true);
    this.TxtHiScroe.SetTextLocalize(Consts.Format(Consts.GetInstance().QUEST_00220_HISCORE_TEXT, (IDictionary) new Hashtable()
    {
      {
        (object) nameof (score),
        (object) score
      }
    }));
  }

  private IEnumerator SpriteCreate(string path)
  {
    Future<Sprite> spriteF = Singleton<ResourceManager>.GetInstance().Load<Sprite>(path);
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.StageNumber.sprite2D = spriteF.Result;
    UI2DSprite stageNumber1 = this.StageNumber;
    Rect textureRect1 = spriteF.Result.textureRect;
    int num1 = Mathf.FloorToInt(((Rect) ref textureRect1).width);
    ((UIWidget) stageNumber1).width = num1;
    UI2DSprite stageNumber2 = this.StageNumber;
    Rect textureRect2 = spriteF.Result.textureRect;
    int num2 = Mathf.FloorToInt(((Rect) ref textureRect2).height);
    ((UIWidget) stageNumber2).height = num2;
  }

  public void StartTween(bool order)
  {
    foreach (UITweener component in ((Component) this).GetComponents<UITweener>())
    {
      if (component.tweenGroup == 1)
        component.Play(order);
    }
  }
}
