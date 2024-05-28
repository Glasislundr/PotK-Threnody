// Decompiled with JetBrains decompiler
// Type: PopupNotificationGetItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

#nullable disable
public class PopupNotificationGetItem : MonoBehaviour
{
  [SerializeField]
  private UI2DSprite slcItemIcon;
  [SerializeField]
  private UILabel txtItemNumber;
  protected string seCountUp = "SE_1065";
  private int entity;

  public IEnumerator Init(MasterDataTable.CommonRewardType reward_type_id, int entity)
  {
    this.entity = entity;
    yield return (object) this.setRewerdIcon(reward_type_id);
    this.txtItemNumber.SetTextLocalize(0);
  }

  private IEnumerator setRewerdIcon(MasterDataTable.CommonRewardType reward_type_id)
  {
    Future<Sprite> spriteF = new ResourceObject(reward_type_id == MasterDataTable.CommonRewardType.reisou_jewel ? "Icons/ChaosJewel_Icon" : "Icons/Common_Icon").Load<Sprite>();
    yield return (object) spriteF.Wait();
    this.slcItemIcon.sprite2D = spriteF.Result;
  }

  public IEnumerator Play()
  {
    Stopwatch sw = new Stopwatch();
    sw.Start();
    yield return (object) new WaitForSeconds(0.25f);
    int seChannel = Singleton<NGSoundManager>.GetInstance().playSE(this.seCountUp, true);
    float num1 = 40f;
    float num2 = 2f;
    float update_sec = num2 / 60f;
    float updateTime = 0.0f;
    if ((double) this.entity > (double) num1 / (double) num2)
    {
      float t = 0.0f;
      float add_t = (float) (1.0 / ((double) num1 / (double) num2));
      while (true)
      {
        t += add_t;
        if ((double) t > 1.0)
          t = 1f;
        this.txtItemNumber.SetTextLocalize((int) Mathf.Lerp(0.0f, (float) this.entity, t));
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
        if (disp_num > this.entity)
          disp_num = this.entity;
        this.txtItemNumber.SetTextLocalize(disp_num);
        if (disp_num < this.entity)
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
    Singleton<NGSoundManager>.GetInstance().stopSE(seChannel);
    sw.Stop();
    yield return (object) new WaitForSeconds((float) (3000L - sw.ElapsedMilliseconds) / 1000f);
  }
}
