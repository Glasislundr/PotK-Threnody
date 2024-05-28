// Decompiled with JetBrains decompiler
// Type: Startup00014BonusIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Startup00014BonusIcon
{
  [SerializeField]
  private Startup00014Menu menu;
  [SerializeField]
  private int depth = 8;
  [SerializeField]
  private float scale = 0.7f;
  private Transform trans;
  private List<Transform> iconPositionList = new List<Transform>();
  [SerializeField]
  private List<GameObject> iconList = new List<GameObject>();

  public List<GameObject> IconList
  {
    get => this.iconList;
    set => this.iconList = value;
  }

  public void Clear()
  {
    foreach (Component iconPosition in this.iconPositionList)
      Object.Destroy((Object) iconPosition.gameObject);
    foreach (Object icon in this.IconList)
      Object.Destroy(icon);
    this.iconPositionList.Clear();
    this.IconList.Clear();
  }

  public IEnumerator Initialize(
    List<Transform> IconPositionList,
    List<LoginbonusReward> loginBonusRewardList,
    int s_index)
  {
    this.iconPositionList = IconPositionList;
    IEnumerator e = this.SetIcons(loginBonusRewardList, s_index);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator SetIcons(List<LoginbonusReward> loginBonusRewardList, int s_index)
  {
    loginBonusRewardList = loginBonusRewardList.Where<LoginbonusReward>((Func<LoginbonusReward, bool>) (x => x.number >= s_index)).OrderBy<LoginbonusReward, int>((Func<LoginbonusReward, int>) (x => x.number)).ToList<LoginbonusReward>();
    if (loginBonusRewardList.Count < this.iconPositionList.Count)
    {
      Debug.LogError((object) "表示する数に対して表示する報酬が足りていません");
    }
    else
    {
      for (int i = 0; i < this.iconPositionList.Count; ++i)
      {
        LoginbonusReward reward_data = loginBonusRewardList[i];
        CreateIconObject target = ((Component) this.iconPositionList[i]).gameObject.GetOrAddComponent<CreateIconObject>();
        IEnumerator e = target.CreateThumbnail(reward_data.reward_type, reward_data.reward_id, reward_data.reward_quantity);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        GameObject icon = target.GetIcon();
        this.AddObj(icon);
        if (reward_data.reward_type == MasterDataTable.CommonRewardType.unit)
          icon.transform.localPosition = new Vector3(0.0f, 3f, 0.0f);
        reward_data = (LoginbonusReward) null;
        target = (CreateIconObject) null;
      }
    }
  }

  private void AddObj(GameObject iconPrefab)
  {
    iconPrefab.GetComponent<UIWidget>().depth = this.depth;
    iconPrefab.transform.localScale = new Vector3(this.scale, this.scale, 1f);
    this.IconList.Add(iconPrefab);
  }

  public void ListEnable(bool flag)
  {
    this.IconList.ForEach((Action<GameObject>) (x => x.SetActive(flag)));
  }
}
