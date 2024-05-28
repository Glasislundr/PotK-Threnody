// Decompiled with JetBrains decompiler
// Type: Bugu005510aMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Bugu005510aMenu : BackButtonMenuBase
{
  [SerializeField]
  private GameObject[] LinkBugu;
  [SerializeField]
  public UIButton yesButton;
  [SerializeField]
  private UILabel TxtDescription;
  [SerializeField]
  private UILabel TxtPopuptitle;

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

  private IEnumerator SetSelectedItemIcons(PlayerItem[] playerItems)
  {
    Future<GameObject> prefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefab = prefabF.Result;
    int i = 0;
    PlayerItem[] playerItemArray = playerItems;
    for (int index = 0; index < playerItemArray.Length; ++index)
    {
      PlayerItem playerItem = playerItemArray[index];
      if (playerItem.gear.rarity.index >= 2)
      {
        GameObject itemIconGo = Object.Instantiate<GameObject>(prefab);
        e = itemIconGo.GetComponent<ItemIcon>().InitByPlayerItem(playerItem);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.LinkBugu[i].gameObject.SetActive(false);
        itemIconGo.gameObject.SetParent(this.LinkBugu[i].gameObject, 0.85f);
        this.LinkBugu[i].gameObject.SetActive(true);
        itemIconGo.SetActive(true);
        ++i;
        itemIconGo = (GameObject) null;
      }
    }
    playerItemArray = (PlayerItem[]) null;
  }
}
