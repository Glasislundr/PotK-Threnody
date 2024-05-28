// Decompiled with JetBrains decompiler
// Type: CommonTips
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
public class CommonTips : MonoBehaviour
{
  [SerializeField]
  private GameObject gaugeObj;
  [SerializeField]
  private UI2DSprite bgSprite;
  private static List<TipsLoadingBackground> backgroundPaths = (List<TipsLoadingBackground>) null;
  private static int countBg = 0;
  private static readonly string basePath = "Prefabs/loading/{0}";
  private bool isBlack;

  public void SetBlackGround()
  {
    this.isBlack = true;
    this.bgSprite.sprite2D = Resources.Load<Sprite>("sprites/1x1_black");
  }

  protected virtual void Awake() => this.gaugeObj.SetActive(false);

  protected virtual IEnumerator Start()
  {
    if (ResourceDownloader.Completed)
    {
      NGGameDataManager gameDataManager = Singleton<NGGameDataManager>.GetInstance();
      if (Object.op_Equality((Object) gameDataManager.loadingBgSprite, (Object) null))
      {
        CommonTips.backgroundPaths = CommonTips.backgroundPaths ?? this.GetTipsLoadingBackgroundList();
        if (CommonTips.backgroundPaths.Count <= 0)
        {
          CommonTips.backgroundPaths = (List<TipsLoadingBackground>) null;
          CommonTips.countBg = 0;
          yield break;
        }
        else
        {
          CommonTips.countBg %= CommonTips.backgroundPaths.Count;
          string path = string.Format(CommonTips.basePath, (object) CommonTips.backgroundPaths[CommonTips.countBg++].image_name);
          Future<Sprite> spriteF = Singleton<ResourceManager>.GetInstance().LoadOrNull<Sprite>(path);
          IEnumerator e = spriteF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          if (Object.op_Inequality((Object) spriteF.Result, (Object) null))
            gameDataManager.loadingBgSprite = spriteF.Result;
          if (CommonTips.countBg >= CommonTips.backgroundPaths.Count)
          {
            CommonTips.backgroundPaths = this.GetTipsLoadingBackgroundList();
            CommonTips.countBg = 0;
          }
          spriteF = (Future<Sprite>) null;
        }
      }
      else if (!this.isBlack)
        this.bgSprite.sprite2D = gameDataManager.loadingBgSprite;
      gameDataManager = (NGGameDataManager) null;
    }
    else
    {
      CommonTips.backgroundPaths = (List<TipsLoadingBackground>) null;
      CommonTips.countBg = 0;
    }
  }

  private List<TipsLoadingBackground> GetTipsLoadingBackgroundList()
  {
    DateTime nowTime = ServerTime.NowAppTimeAddDelta();
    ResourceManager rm = Singleton<ResourceManager>.GetInstance();
    return ((IEnumerable<TipsLoadingBackground>) MasterData.TipsLoadingBackgroundList).Where<TipsLoadingBackground>((Func<TipsLoadingBackground, bool>) (x =>
    {
      if (x.start_at.HasValue && (!x.start_at.HasValue || !(x.start_at.Value <= nowTime)))
        return false;
      if (!x.end_at.HasValue)
        return true;
      return x.end_at.HasValue && x.end_at.Value >= nowTime;
    })).Where<TipsLoadingBackground>((Func<TipsLoadingBackground, bool>) (x => rm.Contains(string.Format(CommonTips.basePath, (object) x.image_name)))).Shuffle<TipsLoadingBackground>().ToList<TipsLoadingBackground>();
  }
}
