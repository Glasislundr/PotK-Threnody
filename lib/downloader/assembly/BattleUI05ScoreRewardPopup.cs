// Decompiled with JetBrains decompiler
// Type: BattleUI05ScoreRewardPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BattleUI05ScoreRewardPopup : MonoBehaviour
{
  [SerializeField]
  private UILabel point;
  [SerializeField]
  private UILabel description;
  public Action callback;
  private bool effectEnd;
  [SerializeField]
  private GameObject[] iconParentOne;
  [SerializeField]
  private GameObject[] iconParentTwo;
  [SerializeField]
  private GameObject[] iconParentThree;
  [SerializeField]
  private GameObject[] iconParentFour;
  [SerializeField]
  private GameObject[] iconParentFive;
  [SerializeField]
  private GameObject[] iconParentSix;
  [SerializeField]
  private GameObject[] iconParentSeven;
  private Dictionary<int, GameObject[]> iconParents;
  [SerializeField]
  private GameObject popupObj;

  public IEnumerator Init(
    QuestScoreBattleFinishContextScore_achivement_rewards rewardList)
  {
    BattleUI05ScoreRewardPopup scoreRewardPopup = this;
    if (rewardList.rewards.Length != 0)
    {
      scoreRewardPopup.iconParents = new Dictionary<int, GameObject[]>()
      {
        {
          1,
          scoreRewardPopup.iconParentOne
        },
        {
          2,
          scoreRewardPopup.iconParentTwo
        },
        {
          3,
          scoreRewardPopup.iconParentThree
        },
        {
          4,
          scoreRewardPopup.iconParentFour
        },
        {
          5,
          scoreRewardPopup.iconParentFive
        },
        {
          6,
          scoreRewardPopup.iconParentSix
        },
        {
          7,
          scoreRewardPopup.iconParentSeven
        }
      };
      scoreRewardPopup.popupObj.SetActive(true);
      scoreRewardPopup.point.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_POINT, (IDictionary) new Hashtable()
      {
        {
          (object) "point",
          (object) rewardList.score
        }
      }));
      scoreRewardPopup.description.SetTextLocalize(Consts.GetInstance().RESULT_HIGHSCORE_TOTAL_TITLE);
      int length = Mathf.Min(rewardList.rewards.Length, scoreRewardPopup.iconParents.Count);
      for (int i = 0; i < length; ++i)
      {
        IEnumerator e = scoreRewardPopup.CreateRewardIcon(scoreRewardPopup.iconParents[length][i], rewardList.rewards[i]);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      ((Component) scoreRewardPopup.iconParents[length][0].transform.parent).gameObject.SetActive(true);
      scoreRewardPopup.effectEnd = false;
    }
  }

  private IEnumerator CreateRewardIcon(GameObject parent, QuestScoreAchivementRewardReceived reward)
  {
    IEnumerator e = parent.GetOrAddComponent<CreateIconObject>().CreateThumbnail((MasterDataTable.CommonRewardType) reward.reward_type_id, reward.reward_id, reward.reward_quantity);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator Init(
    QuestScoreBattleFinishContextScore_total_rewards rewardList)
  {
    BattleUI05ScoreRewardPopup scoreRewardPopup = this;
    if (rewardList.rewards.Length != 0)
    {
      scoreRewardPopup.iconParents = new Dictionary<int, GameObject[]>()
      {
        {
          1,
          scoreRewardPopup.iconParentOne
        },
        {
          2,
          scoreRewardPopup.iconParentTwo
        },
        {
          3,
          scoreRewardPopup.iconParentThree
        },
        {
          4,
          scoreRewardPopup.iconParentFour
        },
        {
          5,
          scoreRewardPopup.iconParentFive
        },
        {
          6,
          scoreRewardPopup.iconParentSix
        },
        {
          7,
          scoreRewardPopup.iconParentSeven
        }
      };
      scoreRewardPopup.popupObj.SetActive(true);
      scoreRewardPopup.point.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_POINT, (IDictionary) new Hashtable()
      {
        {
          (object) "point",
          (object) rewardList.score
        }
      }));
      scoreRewardPopup.description.SetTextLocalize(Consts.GetInstance().RESULT_SCORETOTAL_TOTAL_TITLE);
      int length = Mathf.Min(rewardList.rewards.Length, scoreRewardPopup.iconParents.Count);
      for (int i = 0; i < length; ++i)
      {
        IEnumerator e = scoreRewardPopup.CreateRewardIcon(scoreRewardPopup.iconParents[length][i], rewardList.rewards[i]);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      ((Component) scoreRewardPopup.iconParents[length][0].transform.parent).gameObject.SetActive(true);
      scoreRewardPopup.effectEnd = false;
    }
  }

  private IEnumerator CreateRewardIcon(GameObject parent, QuestScoreTotalRewardReceived reward)
  {
    IEnumerator e = parent.GetOrAddComponent<CreateIconObject>().CreateThumbnail((MasterDataTable.CommonRewardType) reward.reward_type_id, reward.reward_id, reward.reward_quantity);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void SetTapCallBack(Action callback) => this.callback = callback;

  public void onTap()
  {
    if (this.callback == null || !this.effectEnd)
      return;
    TweenAlpha component = this.popupObj.GetComponent<TweenAlpha>();
    if (Object.op_Inequality((Object) component, (Object) null))
      ((UITweener) component).PlayForward();
    this.callback();
  }

  public void onFinish() => this.effectEnd = true;
}
