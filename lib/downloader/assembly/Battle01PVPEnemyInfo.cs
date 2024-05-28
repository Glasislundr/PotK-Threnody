// Decompiled with JetBrains decompiler
// Type: Battle01PVPEnemyInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Battle01PVPEnemyInfo : NGBattleMenuBase
{
  [SerializeField]
  private UILabel playerName;
  [SerializeField]
  private UILabel className;
  [SerializeField]
  private UILabel rankName;
  [SerializeField]
  private UILabel ranking;
  [SerializeField]
  private UILabel point;
  [SerializeField]
  private UILabel power;
  [SerializeField]
  private UILabel contributionLabel;
  [SerializeField]
  private UILabel levelLabel;
  [SerializeField]
  private GameObject[] guildPositionNodes;
  [SerializeField]
  private GameObject memberBaseNode;
  [SerializeField]
  private Battle01PVPEnemyUnits enemyUnits;
  [SerializeField]
  private UI2DSprite emblem;

  protected override IEnumerator Start_Battle()
  {
    Battle01PVPEnemyInfo battle01PvpEnemyInfo = this;
    if (battle01PvpEnemyInfo.battleManager.gameEngine != null)
    {
      Future<GameObject> tF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      IEnumerator e = tF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = tF.Result;
      e = battle01PvpEnemyInfo.enemyUnits.setupUnits(battle01PvpEnemyInfo.env.core.enemyUnits.value, result);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Future<Sprite> f = EmblemUtility.LoadEmblemSprite(battle01PvpEnemyInfo.battleManager.gameEngine.enemyEmblem);
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      battle01PvpEnemyInfo.emblem.sprite2D = f.Result;
      e = battle01PvpEnemyInfo.setInfo(battle01PvpEnemyInfo.battleManager.gameEngine);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private IEnumerator setInfo(IGameEngine ge)
  {
    Battle01PVPEnemyInfo battle01PvpEnemyInfo = this;
    battle01PvpEnemyInfo.setText(battle01PvpEnemyInfo.playerName, ge.enemyName);
    int v1 = 0;
    foreach (BL.Unit unit in battle01PvpEnemyInfo.env.core.enemyUnits.value)
    {
      Judgement.NonBattleParameter nonBattleParameter = Judgement.NonBattleParameter.FromPlayerUnit(unit.playerUnit);
      v1 += nonBattleParameter.Combat;
    }
    battle01PvpEnemyInfo.setText(battle01PvpEnemyInfo.power, v1);
    switch (ge)
    {
      case PVPManager _:
        PVPManager pvpManager = ge as PVPManager;
        if (pvpManager.enemyInfo == null)
          break;
        battle01PvpEnemyInfo.setText(battle01PvpEnemyInfo.className, pvpManager.enemyInfo.getClassNameString());
        battle01PvpEnemyInfo.setText(battle01PvpEnemyInfo.rankName, pvpManager.enemyInfo.getRankNameString());
        string v2 = string.Format(Consts.GetInstance().BATTLE_PVP_ENEMYINFO_POINT, (object) pvpManager.enemyInfo.getPointString());
        battle01PvpEnemyInfo.setText(battle01PvpEnemyInfo.point, v2);
        string v3 = string.Format(Consts.GetInstance().BATTLE_PVP_ENEMYINFO_RANK, (object) pvpManager.enemyInfo.getRankingString());
        battle01PvpEnemyInfo.setText(battle01PvpEnemyInfo.ranking, v3);
        break;
      case PVNpcManager _:
        PVNpcManager pvNpcManager = ge as PVNpcManager;
        if (pvNpcManager.enemyInfo == null)
          break;
        battle01PvpEnemyInfo.setText(battle01PvpEnemyInfo.className, pvNpcManager.enemyInfo.getClassNameString());
        battle01PvpEnemyInfo.setText(battle01PvpEnemyInfo.rankName, pvNpcManager.enemyInfo.getRankNameString());
        string v4 = string.Format(Consts.GetInstance().BATTLE_PVP_ENEMYINFO_POINT, (object) pvNpcManager.enemyInfo.getPointString());
        battle01PvpEnemyInfo.setText(battle01PvpEnemyInfo.point, v4);
        string v5 = string.Format(Consts.GetInstance().BATTLE_PVP_ENEMYINFO_RANK, (object) pvNpcManager.enemyInfo.getRankingString());
        battle01PvpEnemyInfo.setText(battle01PvpEnemyInfo.ranking, v5);
        break;
      case GVGManager _:
        GVGManager gm = ge as GVGManager;
        battle01PvpEnemyInfo.setText(battle01PvpEnemyInfo.contributionLabel, gm.enemyContribution);
        battle01PvpEnemyInfo.setText(battle01PvpEnemyInfo.levelLabel, gm.enemyLevel);
        int num = gm.enemyGuildPosition - 2;
        for (int index = 0; index < battle01PvpEnemyInfo.guildPositionNodes.Length; ++index)
          battle01PvpEnemyInfo.guildPositionNodes[index].SetActive(num == index);
        Future<GameObject> f = Res.Prefabs.guild028_2.MemberBase.Load<GameObject>();
        IEnumerator e = f.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        GameObject guildMemberBasePrefab = f.Result;
        GuildImageCache memberImageCache = new GuildImageCache();
        e = memberImageCache.GuildFrameAnimResourceLoad();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        guildMemberBasePrefab.Clone(battle01PvpEnemyInfo.memberBaseNode.transform).GetComponent<Guild0282MemberBase>().InitializeGB(gm.enemyTownLevel, gm.enemyKeepStar, memberImageCache, battle01PvpEnemyInfo.env.core.battleInfo.gvgSetting.isTestBattle);
        gm = (GVGManager) null;
        f = (Future<GameObject>) null;
        guildMemberBasePrefab = (GameObject) null;
        memberImageCache = (GuildImageCache) null;
        break;
    }
  }
}
