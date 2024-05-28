// Decompiled with JetBrains decompiler
// Type: Raid032BattleResultBossView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Raid032BattleResultBossView : MonoBehaviour
{
  [SerializeField]
  private Animator raidBossAnimator;
  [SerializeField]
  private Transform imageOffset;
  [SerializeField]
  private SpriteRenderer image;
  [SerializeField]
  private GameObject dir_camera;
  [SerializeField]
  private SpriteRenderer background;

  public IEnumerator Init(GuildRaid masterData, PlayerUnit bossUnit)
  {
    Future<Sprite> future = bossUnit.unit.LoadSpriteLarge();
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Vector3 localPosition = this.imageOffset.localPosition;
    this.imageOffset.localPosition = new Vector3(localPosition.x, localPosition.y + masterData.image_offset_y, localPosition.z);
    this.image.sprite = future.Result;
    GuildRaidPeriod guildRaidPeriod;
    if (MasterData.GuildRaidPeriod.TryGetValue(masterData.period_id, out guildRaidPeriod) && !string.IsNullOrEmpty(guildRaidPeriod.bg_path))
    {
      Future<Sprite> prefabBgF = new ResourceObject(guildRaidPeriod.bg_path).Load<Sprite>();
      e = prefabBgF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.background.sprite = prefabBgF.Result;
      ((Component) this.background).transform.localScale = new Vector3(1f, 1f);
      prefabBgF = (Future<Sprite>) null;
    }
    this.dir_camera.transform.position = ((Component) Singleton<CommonRoot>.GetInstance().getCamera()).transform.position;
  }

  public void PlayAnimation(bool isSurvive, bool isNoDamage)
  {
    if (isNoDamage)
      this.raidBossAnimator.SetTrigger("no_damage");
    else if (isSurvive)
      this.raidBossAnimator.SetTrigger("survive");
    else
      this.raidBossAnimator.SetTrigger("kill");
  }

  public void PlayAlreadyDefeatedAnimation() => this.raidBossAnimator.SetTrigger("kill_late");
}
