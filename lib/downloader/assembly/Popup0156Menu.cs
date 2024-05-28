// Decompiled with JetBrains decompiler
// Type: Popup0156Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Popup0156Menu : BackButtonMenuBase
{
  public GameObject dir_Button_Single;
  public GameObject dir_Button_Doubl;
  public NGxScrollMasonry ScrollContainer;
  private BeginnerNaviDetail detail;
  private GameObject question;
  private GameObject answer;
  [SerializeField]
  private UISprite ibtn_Popup_Ok_Label;

  public virtual void IbtnPopupOk()
  {
    if (this.detail.movePage.moveScene == "colosseum023_4" && !SMManager.Get<Player>().GetReleaseColosseum())
    {
      ModalWindow.Show(Consts.GetInstance().POPUP_0156_TITLE, Consts.GetInstance().POPUP_0156_DESCRIPT_TEXT, (Action) (() => { }));
    }
    else
    {
      switch (this.detail.movePage.ID)
      {
        case 1:
          this.DisableBody();
          Singleton<PopupManager>.GetInstance().onDismiss();
          break;
        case 2:
          Quest00240723Scene.ChangeScene0024(true, this.GetRecentStoryID(), true);
          goto case 1;
        case 6:
          Unit0046Scene.changeScene(true);
          goto case 1;
        case 7:
          Unit00468Scene.changeScene0048(true);
          goto case 1;
        case 8:
          Unit00468Scene.changeScene00491Evolution(true);
          goto case 1;
        case 9:
          Unit00468Scene.changeScene00412(true);
          goto case 1;
        default:
          Singleton<NGSceneManager>.GetInstance().changeScene(this.detail.movePage.moveScene, false);
          goto case 1;
      }
    }
  }

  public void IbtnNo()
  {
    this.DisableBody();
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  public void InitPopup(
    BeginnerNaviDetail detail,
    Sprite image,
    GameObject questionPrefab,
    GameObject answerPrefab)
  {
    this.detail = detail;
    this.SetButtonFrame();
    this.SetQuestionContainer(questionPrefab);
    this.SetAnswerContainer(answerPrefab, image);
    this.ScrollContainer.ResolvePosition();
  }

  private void SetButtonFrame()
  {
    this.dir_Button_Single.SetActive(false);
    this.dir_Button_Doubl.SetActive(false);
    if (this.detail.movePage.moveScene == "")
    {
      this.dir_Button_Single.SetActive(true);
    }
    else
    {
      this.dir_Button_Doubl.SetActive(true);
      this.ibtn_Popup_Ok_Label.SetSprite(this.detail.movePage.buttonImage);
    }
  }

  private void SetQuestionContainer(GameObject questionPrefab)
  {
    this.question = Object.Instantiate<GameObject>(questionPrefab);
    this.SetText(this.question, this.detail.questionText);
    this.ScrollContainer.Add(this.question);
  }

  private void SetAnswerContainer(GameObject answerPrefab, Sprite image)
  {
    this.answer = Object.Instantiate<GameObject>(answerPrefab);
    this.SetText(this.answer, this.detail.answerText);
    Transform transform = this.answer.transform.Find("dyn_image");
    if (Object.op_Inequality((Object) image, (Object) null) && Object.op_Inequality((Object) transform, (Object) null))
      ((Component) transform).gameObject.GetComponent<UI2DSprite>().sprite2D = image;
    this.ScrollContainer.Add(this.answer);
  }

  private void SetText(GameObject gameObject, string text)
  {
    gameObject.GetComponentInChildren<UILabel>().text = text;
  }

  private int GetRecentStoryID()
  {
    return ((IEnumerable<PlayerStoryQuestS>) SMManager.Get<PlayerStoryQuestS[]>()).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x.quest_story_s.quest_l.quest_mode == CommonQuestMode.normal)).Select<PlayerStoryQuestS, int>((Func<PlayerStoryQuestS, int>) (x => x.quest_story_s.quest_l_QuestStoryL)).Distinct<int>().Last<int>();
  }

  private void DisableBody()
  {
    this.question.SetActive(false);
    this.answer.SetActive(false);
  }
}
