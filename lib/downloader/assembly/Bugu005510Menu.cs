// Decompiled with JetBrains decompiler
// Type: Bugu005510Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Bugu005510Menu : BackButtonMenuBase
{
  [SerializeField]
  public GameObject SetBuguForm1;
  [SerializeField]
  public GameObject SetBuguForm2;
  [SerializeField]
  public GameObject LinkBugu0101;
  [SerializeField]
  public GameObject LinkBugu0102;
  [SerializeField]
  public GameObject LinkBugu0103;
  [SerializeField]
  public GameObject LinkBugu0104;
  [SerializeField]
  public GameObject LinkBugu0105;
  [SerializeField]
  public GameObject LinkBugu0201;
  [SerializeField]
  public GameObject LinkBugu0202;
  [SerializeField]
  public GameObject LinkBugu0203;
  [SerializeField]
  public GameObject LinkBugu0204;
  [SerializeField]
  private UILabel TxtDescription;
  [SerializeField]
  private UILabel TxtPopuptitle;
  [SerializeField]
  public UIButton yesButton;

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  public void Show(PlayerItem[] playerItems)
  {
    this.StartCoroutine(this.SetSelectedItemIcons(playerItems));
  }

  private GameObject SelectPosition(int cnt, int num)
  {
    GameObject gameObject = this.LinkBugu0103;
    switch (num)
    {
      case 1:
        if (cnt == 0)
        {
          gameObject = this.LinkBugu0103;
          break;
        }
        break;
      case 2:
        if (cnt == 0)
          gameObject = this.LinkBugu0202;
        if (cnt == 1)
        {
          gameObject = this.LinkBugu0203;
          break;
        }
        break;
      case 3:
        if (cnt == 0)
          gameObject = this.LinkBugu0102;
        if (cnt == 1)
          gameObject = this.LinkBugu0103;
        if (cnt == 2)
        {
          gameObject = this.LinkBugu0104;
          break;
        }
        break;
      case 4:
        if (cnt == 0)
          gameObject = this.LinkBugu0201;
        if (cnt == 1)
          gameObject = this.LinkBugu0202;
        if (cnt == 2)
          gameObject = this.LinkBugu0203;
        if (cnt == 3)
        {
          gameObject = this.LinkBugu0204;
          break;
        }
        break;
      case 5:
        if (cnt == 0)
          gameObject = this.LinkBugu0101;
        if (cnt == 1)
          gameObject = this.LinkBugu0102;
        if (cnt == 2)
          gameObject = this.LinkBugu0103;
        if (cnt == 3)
          gameObject = this.LinkBugu0104;
        if (cnt == 4)
        {
          gameObject = this.LinkBugu0105;
          break;
        }
        break;
    }
    return gameObject;
  }

  private IEnumerator SetSelectedItemIcons(PlayerItem[] playerItems)
  {
    List<PlayerItem> rarePlayerItems = new List<PlayerItem>();
    Future<GameObject> prefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefab = prefabF.Result;
    foreach (PlayerItem playerItem in playerItems)
    {
      if (playerItem.gear.rarity.index >= 2)
        rarePlayerItems.Add(playerItem);
    }
    this.SetBuguForm1.SetActive(rarePlayerItems.Count % 2 != 0);
    this.SetBuguForm2.SetActive(rarePlayerItems.Count % 2 == 0);
    int i = 0;
    foreach (PlayerItem playerItem in rarePlayerItems)
    {
      GameObject itemIconGo = Object.Instantiate<GameObject>(prefab);
      e = itemIconGo.GetComponent<ItemIcon>().InitByPlayerItem(playerItem);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject parent = this.SelectPosition(i, rarePlayerItems.Count);
      parent.SetActive(false);
      itemIconGo.gameObject.SetParent(parent, 0.85f);
      parent.SetActive(true);
      itemIconGo.SetActive(true);
      ++i;
      itemIconGo = (GameObject) null;
    }
  }
}
