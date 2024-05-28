// Decompiled with JetBrains decompiler
// Type: Quest00215DirEpisode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Quest00215DirEpisode : MonoBehaviour
{
  [SerializeField]
  private GameObject DirEpisode;
  [SerializeField]
  private GameObject DirEpisodeBlock;
  [SerializeField]
  private GameObject[] DirEpisodenum;
  [SerializeField]
  private UIButton IbtnEpisode;
  [SerializeField]
  private UILabel TxtEpisodetitle;
  [SerializeField]
  private GameObject SlcClear;
  [SerializeField]
  private GameObject SlcNew;
  [SerializeField]
  private GameObject[] DirEpisodenumBlock;
  [SerializeField]
  private UIButton IbtnEpisodeBlock;

  private IEnumerator openPopup002152()
  {
    Future<GameObject> prefab = Res.Prefabs.popup.popup_002_15_2__anim_popup02.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().openAlert(prefab.Result).GetComponent<Quest002152popup>().PopupSetiing();
  }

  public void setData(int id, bool isOpen)
  {
    this.DirEpisode.SetActive(isOpen);
    this.DirEpisodeBlock.SetActive(!isOpen);
    foreach (GameObject gameObject in this.DirEpisodenum)
      gameObject.SetActive(false);
    foreach (GameObject gameObject in this.DirEpisodenumBlock)
      gameObject.SetActive(false);
    if (isOpen)
    {
      this.DirEpisodenum[id].SetActive(true);
      EventDelegate.Set(this.IbtnEpisode.onClick, (EventDelegate.Callback) (() => Singleton<NGSceneManager>.GetInstance().changeScene("quest002_15_a", true)));
    }
    else
    {
      this.DirEpisodenumBlock[id].SetActive(true);
      EventDelegate.Set(this.IbtnEpisodeBlock.onClick, (EventDelegate.Callback) (() =>
      {
        Debug.Log((object) ("onClickDirEpisodenumBlock[" + (object) id + "]"));
        this.StartCoroutine(this.openPopup002152());
      }));
    }
    this.TxtEpisodetitle.SetTextLocalize("シナリオテストテスト");
    this.SlcClear.SetActive(false);
  }
}
