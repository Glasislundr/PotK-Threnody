// Decompiled with JetBrains decompiler
// Type: Quest00230NextRewardObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Quest00230NextRewardObject : MonoBehaviour
{
  [SerializeField]
  private CreateIconObject[] nextRewardThums;
  [SerializeField]
  private UILabel txtNextRewardPointLabel;
  private const int TWEEN_GROUPID_START = 0;
  private const int TWEEN_GROUPID_END = 1;
  private TweenAlpha[] tweenAlphas;
  private Action displayEndAction;
  private Action hiddenEndAction;
  private bool isEnable;

  public bool Enable => this.isEnable;

  public IEnumerator Init(
    WebAPI.Response.EventTop eventTopInfo,
    List<IGrouping<Tuple<EventPointType, int, int>, PunitiveExpeditionEventReward>> rewardList,
    EventPointType rewardType,
    Action displayAction,
    Action hiddenAction)
  {
    Quest00230NextRewardObject nextRewardObject = this;
    string text = string.Empty;
    List<PunitiveExpeditionEventReward> list = (List<PunitiveExpeditionEventReward>) null;
    nextRewardObject.isEnable = false;
    nextRewardObject.displayEndAction = displayAction;
    nextRewardObject.hiddenEndAction = hiddenAction;
    switch (rewardType)
    {
      case EventPointType.personal:
        IOrderedEnumerable<IGrouping<Tuple<EventPointType, int, int>, PunitiveExpeditionEventReward>> source1 = rewardList.Where<IGrouping<Tuple<EventPointType, int, int>, PunitiveExpeditionEventReward>>((Func<IGrouping<Tuple<EventPointType, int, int>, PunitiveExpeditionEventReward>, bool>) (x => x.Key.Item1 == EventPointType.personal && x.Key.Item2 > eventTopInfo.player_point)).OrderBy<IGrouping<Tuple<EventPointType, int, int>, PunitiveExpeditionEventReward>, int>((Func<IGrouping<Tuple<EventPointType, int, int>, PunitiveExpeditionEventReward>, int>) (x => x.Key.Item2));
        if (source1 != null && source1.Count<IGrouping<Tuple<EventPointType, int, int>, PunitiveExpeditionEventReward>>() > 0)
        {
          list = source1.FirstOrDefault<IGrouping<Tuple<EventPointType, int, int>, PunitiveExpeditionEventReward>>().ToList<PunitiveExpeditionEventReward>();
          if (list != null && list.Count<PunitiveExpeditionEventReward>() > 0)
          {
            nextRewardObject.isEnable = true;
            text = Consts.Format(Consts.GetInstance().QUEST_00230_EVENT_NEXT_REWARD_PERSONAL_POINT, (IDictionary) new Hashtable()
            {
              {
                (object) "point",
                (object) list[0].point
              }
            });
            break;
          }
          break;
        }
        break;
      case EventPointType.all:
        IOrderedEnumerable<IGrouping<Tuple<EventPointType, int, int>, PunitiveExpeditionEventReward>> source2 = rewardList.Where<IGrouping<Tuple<EventPointType, int, int>, PunitiveExpeditionEventReward>>((Func<IGrouping<Tuple<EventPointType, int, int>, PunitiveExpeditionEventReward>, bool>) (x =>
        {
          if (x.Key.Item1 != EventPointType.all)
            return false;
          return x.Key.Item2 > eventTopInfo.all_player_point || x.Key.Item3 > eventTopInfo.player_point;
        })).OrderBy<IGrouping<Tuple<EventPointType, int, int>, PunitiveExpeditionEventReward>, int>((Func<IGrouping<Tuple<EventPointType, int, int>, PunitiveExpeditionEventReward>, int>) (x => x.Key.Item2));
        if (source2 != null && source2.Count<IGrouping<Tuple<EventPointType, int, int>, PunitiveExpeditionEventReward>>() > 0)
        {
          list = source2.FirstOrDefault<IGrouping<Tuple<EventPointType, int, int>, PunitiveExpeditionEventReward>>().ToList<PunitiveExpeditionEventReward>();
          if (list != null && list.Count<PunitiveExpeditionEventReward>() > 0)
          {
            nextRewardObject.isEnable = true;
            text = Consts.Format(Consts.GetInstance().QUEST_00230_EVENT_NEXT_REWARD_ALL_POINT, (IDictionary) new Hashtable()
            {
              {
                (object) "point",
                (object) list[0].point
              }
            });
            break;
          }
          break;
        }
        break;
      case EventPointType.guild:
        IOrderedEnumerable<IGrouping<Tuple<EventPointType, int, int>, PunitiveExpeditionEventReward>> source3 = rewardList.Where<IGrouping<Tuple<EventPointType, int, int>, PunitiveExpeditionEventReward>>((Func<IGrouping<Tuple<EventPointType, int, int>, PunitiveExpeditionEventReward>, bool>) (x =>
        {
          if (x.Key.Item1 != EventPointType.guild)
            return false;
          return x.Key.Item2 > eventTopInfo.guild_point || x.Key.Item3 > eventTopInfo.player_point;
        })).OrderBy<IGrouping<Tuple<EventPointType, int, int>, PunitiveExpeditionEventReward>, int>((Func<IGrouping<Tuple<EventPointType, int, int>, PunitiveExpeditionEventReward>, int>) (x => x.Key.Item2));
        if (source3 != null && source3.Count<IGrouping<Tuple<EventPointType, int, int>, PunitiveExpeditionEventReward>>() > 0)
        {
          list = source3.FirstOrDefault<IGrouping<Tuple<EventPointType, int, int>, PunitiveExpeditionEventReward>>().ToList<PunitiveExpeditionEventReward>();
          if (list != null && list.Count<PunitiveExpeditionEventReward>() > 0)
          {
            nextRewardObject.isEnable = true;
            text = Consts.Format(Consts.GetInstance().QUEST_00230_EVENT_NEXT_REWARD_GUILD_POINT, (IDictionary) new Hashtable()
            {
              {
                (object) "point",
                (object) list[0].point
              }
            });
            break;
          }
          break;
        }
        break;
    }
    if (nextRewardObject.isEnable)
    {
      nextRewardObject.tweenAlphas = ((Component) nextRewardObject).GetComponents<TweenAlpha>();
      nextRewardObject.txtNextRewardPointLabel.SetTextLocalize(text);
      int rewartThumLength = nextRewardObject.nextRewardThums.Length < list.Count<PunitiveExpeditionEventReward>() ? nextRewardObject.nextRewardThums.Length : list.Count<PunitiveExpeditionEventReward>();
      for (int i = 0; i < rewartThumLength; ++i)
      {
        ((Component) nextRewardObject.nextRewardThums[i]).transform.DetachChildren();
        IEnumerator e = nextRewardObject.nextRewardThums[i].CreateThumbnail(list[i].reward_type_id, list[i].reward_id, list[i].reward_quantity);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    ((Component) nextRewardObject).gameObject.SetActive(false);
  }

  private void StartTweenAlpha(int groupID)
  {
    TweenAlpha tweenAlpha = ((IEnumerable<TweenAlpha>) this.tweenAlphas).FirstOrDefault<TweenAlpha>((Func<TweenAlpha, bool>) (x => ((UITweener) x).tweenGroup == groupID));
    if (!Object.op_Inequality((Object) tweenAlpha, (Object) null))
      return;
    ((UITweener) tweenAlpha).ResetToBeginning();
    ((UITweener) tweenAlpha).PlayForward();
  }

  public void DisplayEnd()
  {
    if (this.displayEndAction == null)
      return;
    this.displayEndAction();
  }

  public void HiddenEnd()
  {
    ((Component) this).gameObject.SetActive(false);
    if (this.hiddenEndAction == null)
      return;
    this.hiddenEndAction();
  }

  public void Display()
  {
    if (!this.isEnable)
      return;
    ((Component) this).gameObject.SetActive(true);
    this.StartTweenAlpha(0);
  }

  public void Hidden()
  {
    if (!this.isEnable)
      return;
    this.StartTweenAlpha(1);
  }
}
