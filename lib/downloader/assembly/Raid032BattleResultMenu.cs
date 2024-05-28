// Decompiled with JetBrains decompiler
// Type: Raid032BattleResultMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Raid032BattleResultMenu : MonoBehaviour, IRaidResultMenu
{
  [SerializeField]
  private Transform dir_RaidBoss_animParent;
  [SerializeField]
  private Raid032BattleResultBossStatus bossStatus;
  [SerializeField]
  private UILabel attackDamage;
  [SerializeField]
  private UILabel attackDamagePercentage;
  [SerializeField]
  private UILabel totalAttackDamage;
  [SerializeField]
  private UILabel totalAttackDamagePercentage;
  [SerializeField]
  private GameObject alreadyDefeatedMessage;
  private WebAPI.Response.GuildraidBattleFinish result;
  private Raid032BattleResultBossView bossView;
  private PlayerUnit bossUnit;
  private BattleInfo battleInfo;
  private float attackDamagePercentageTo;
  private float totalAttackDamagePercentageTo;

  public bool isSkip { get; set; }

  public IEnumerator Init(
    GuildRaid masterData,
    BattleInfo info,
    WebAPI.Response.GuildraidBattleFinish result,
    PlayerUnit bossUnit)
  {
    this.result = result;
    this.bossUnit = bossUnit;
    this.battleInfo = info;
    Future<GameObject> future = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/raid0032_result/dir_RaidBoss_anim");
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Equality((Object) future.Result, (Object) null))
    {
      Debug.LogError((object) "failed to load dir_RaidBoss_anim.prefab");
    }
    else
    {
      this.bossView = future.Result.Clone(this.dir_RaidBoss_animParent).GetComponent<Raid032BattleResultBossView>();
      yield return (object) this.bossView.Init(masterData, bossUnit);
      yield return (object) this.bossStatus.Init(masterData, info, result, bossUnit);
      this.attackDamage.SetTextLocalize(0);
      this.attackDamagePercentage.SetTextLocalize("0.0%");
      this.totalAttackDamage.SetTextLocalize(0);
      this.totalAttackDamagePercentage.SetTextLocalize("0.0%");
      this.attackDamagePercentageTo = (float) ((double) result.boss_damage / (double) bossUnit.total_hp * 100.0);
      this.totalAttackDamagePercentageTo = (float) ((double) result.boss_total_damage / (double) bossUnit.total_hp * 100.0);
      this.alreadyDefeatedMessage.SetActive(result.already_defeated);
    }
  }

  public IEnumerator Run()
  {
    Raid032BattleResultMenu battleResultMenu = this;
    if (battleResultMenu.result.already_defeated)
    {
      battleResultMenu.UpdateDamageNumbersImmediate();
      battleResultMenu.bossStatus.SetAfterHpAndGaugeImmediate();
      battleResultMenu.bossView.PlayAlreadyDefeatedAnimation();
    }
    else
    {
      battleResultMenu.bossView.PlayAnimation(battleResultMenu.bossStatus.isSurvive, battleResultMenu.result.boss_damage <= 0);
      battleResultMenu.StartCoroutine(battleResultMenu.bossStatus.AnimateHpGauge());
      yield return (object) battleResultMenu.CountUpDamageNumbers();
      yield return (object) battleResultMenu.CountUpTotalDamageNumbers();
      // ISSUE: explicit non-virtual call
      __nonvirtual (battleResultMenu.isSkip) = false;
      // ISSUE: explicit non-virtual call
      while (__nonvirtual (battleResultMenu.isSkip))
        yield return (object) null;
    }
  }

  private void UpdateDamageNumbersImmediate()
  {
    this.EndCountUpDamageNumbers();
    this.EndCountUpTotalDamageNumbers();
  }

  private IEnumerator CountUpDamageNumbers()
  {
    this.isSkip = false;
    float t = 0.0f;
    while ((double) t < 1.0)
    {
      t += Time.deltaTime;
      this.attackDamage.SetTextLocalize((int) Mathf.Lerp(0.0f, (float) this.result.boss_damage, t));
      this.attackDamagePercentage.SetTextLocalize(this.ToPercentageText(Mathf.Lerp(0.0f, this.attackDamagePercentageTo, t)));
      yield return (object) null;
      if (this.isSkip)
        break;
    }
    this.EndCountUpDamageNumbers();
  }

  private void EndCountUpDamageNumbers()
  {
    this.attackDamage.SetTextLocalize(this.result.boss_damage);
    this.attackDamagePercentage.SetTextLocalize(this.ToPercentageText(this.attackDamagePercentageTo));
  }

  private IEnumerator CountUpTotalDamageNumbers()
  {
    if (this.isSkip)
    {
      this.EndCountUpTotalDamageNumbers();
    }
    else
    {
      this.isSkip = false;
      float t = 0.0f;
      while ((double) t < 1.0)
      {
        t += Time.deltaTime;
        this.totalAttackDamage.SetTextLocalize((int) Mathf.Lerp(0.0f, (float) this.result.boss_total_damage, t));
        this.totalAttackDamagePercentage.SetTextLocalize(this.ToPercentageText(Mathf.Lerp(0.0f, this.totalAttackDamagePercentageTo, t)));
        yield return (object) null;
        if (this.isSkip)
          break;
      }
      this.EndCountUpTotalDamageNumbers();
    }
  }

  private void EndCountUpTotalDamageNumbers()
  {
    this.totalAttackDamage.SetTextLocalize(this.result.boss_total_damage);
    this.totalAttackDamagePercentage.SetTextLocalize(this.ToPercentageText(this.totalAttackDamagePercentageTo));
  }

  public IEnumerator OnFinish()
  {
    yield break;
  }

  private string ToPercentageText(float t)
  {
    float num = t - (float) (int) t;
    return string.Format("{0}.{1}%", (object) (int) t, (object) (int) ((double) num * 10.0));
  }
}
