// Decompiled with JetBrains decompiler
// Type: PopupReisouMixerResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class PopupReisouMixerResult : BackButtonMenuBase
{
  [SerializeField]
  protected UIScrollView scrollView;
  [SerializeField]
  protected UIGrid grid;
  [SerializeField]
  protected UILabel txtAcquisitionsValue;
  [SerializeField]
  protected GameObject dirNoItem;
  private const string seCountUp = "SE_1065";
  private int seChannel = -1;
  private int reisou_jewel;

  public IEnumerator Init(PlayerItem[] holy_reisou, int reisou_jewel)
  {
    PopupReisouMixerResult reisouMixerResult = this;
    reisouMixerResult.seChannel = -1;
    reisouMixerResult.reisou_jewel = reisou_jewel;
    reisouMixerResult.dirNoItem.SetActive(holy_reisou.Length == 0);
    Future<GameObject> prefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefab = prefabF.Result;
    PlayerItem[] playerItemArray = holy_reisou;
    for (int index = 0; index < playerItemArray.Length; ++index)
    {
      PlayerItem playerItem = playerItemArray[index];
      e = prefab.Clone(((Component) reisouMixerResult.grid).transform).GetComponent<ItemIcon>().InitByPlayerItem(playerItem);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    playerItemArray = (PlayerItem[]) null;
    reisouMixerResult.StartCoroutine(reisouMixerResult.PlayGetReisouJewel());
  }

  private IEnumerator PlayGetReisouJewel()
  {
    this.txtAcquisitionsValue.SetTextLocalize(0);
    yield return (object) new WaitForSeconds(0.25f);
    this.seChannel = Singleton<NGSoundManager>.GetInstance().playSE("SE_1065", true);
    float num1 = 40f;
    float num2 = 2f;
    float update_sec = num2 / 60f;
    float updateTime = 0.0f;
    if ((double) this.reisou_jewel > (double) num1 / (double) num2)
    {
      float t = 0.0f;
      float add_t = (float) (1.0 / ((double) num1 / (double) num2));
      while (true)
      {
        t += add_t;
        if ((double) t > 1.0)
          t = 1f;
        this.txtAcquisitionsValue.SetTextLocalize((int) Mathf.Lerp(0.0f, (float) this.reisou_jewel, t));
        if ((double) t < 1.0)
        {
          float num3 = updateTime;
          updateTime = Time.realtimeSinceStartup;
          if ((double) num3 == 0.0)
            num3 = updateTime;
          yield return (object) new WaitForSeconds(update_sec - (updateTime - num3 - update_sec));
        }
        else
          break;
      }
    }
    else
    {
      int disp_num = 0;
      while (true)
      {
        ++disp_num;
        if (disp_num > this.reisou_jewel)
          disp_num = this.reisou_jewel;
        this.txtAcquisitionsValue.SetTextLocalize(disp_num);
        if (disp_num < this.reisou_jewel)
        {
          float num4 = updateTime;
          updateTime = Time.realtimeSinceStartup;
          if ((double) num4 == 0.0)
            num4 = updateTime;
          yield return (object) new WaitForSeconds(update_sec - (updateTime - num4 - update_sec));
        }
        else
          break;
      }
    }
    Singleton<NGSoundManager>.GetInstance().stopSE(this.seChannel);
  }

  public void scrollResetPosition()
  {
    this.grid.Reposition();
    this.scrollView.ResetPosition();
  }

  public void IbtnClose()
  {
    if (this.IsPushAndSet())
      return;
    if (this.seChannel != -1)
      Singleton<NGSoundManager>.GetInstance().stopSE(this.seChannel);
    this.StopCoroutine(this.PlayGetReisouJewel());
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnClose();
}
