// Decompiled with JetBrains decompiler
// Type: Versus026DirWinLossRecordsScrollItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Versus026DirWinLossRecordsScrollItem : MonoBehaviour
{
  [HideInInspector]
  public Versus026DirWinLossRecordsMenu menu;
  [SerializeField]
  private Transform playerIconPosition;
  [SerializeField]
  private UILabel txtBattledTime;
  [SerializeField]
  private UILabel txtPlayerName;
  [SerializeField]
  private UILabel txtPlayerClass;
  [SerializeField]
  private GameObject slcWin;
  [SerializeField]
  private GameObject slcGreat;
  [SerializeField]
  private GameObject slcExcellent;
  [SerializeField]
  private GameObject slcDraw;
  [SerializeField]
  private GameObject slcLose;
  [SerializeField]
  private UI2DSprite guildTitle;
  private string battleID;

  public IEnumerator CreateItem(
    Versus026DirWinLossRecordsMenu vMenu,
    GameObject unitPrefab,
    WebAPI.Response.PvpClassMatchHistoryClass_match_records info)
  {
    this.menu = vMenu;
    this.battleID = info.battle_uuid;
    UnitIcon unitIconScript = ((Component) unitPrefab.Clone(this.playerIconPosition).GetComponent<UnitIconBase>()).GetComponent<UnitIcon>();
    PlayerUnit pUnit = info.target_player_units[0];
    unitIconScript.setBottom(pUnit);
    unitIconScript.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    IEnumerator e = unitIconScript.SetUnit(pUnit, pUnit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitIconScript.BottomModeValue = UnitIconBase.GetBottomModeLevel(pUnit.unit, pUnit);
    unitIconScript.setLevelText(pUnit);
    this.txtBattledTime.text = string.Format("{0:yy/MM/dd HH:mm}", (object) info.start_at);
    this.txtPlayerName.text = info.target_player.name;
    this.txtPlayerClass.text = info.target_player_pvp_emblem;
    this.DeactiveAllResult();
    switch (info.battle_result)
    {
      case "Win":
        this.slcWin.SetActive(true);
        break;
      case "Draw":
        this.slcDraw.SetActive(true);
        break;
      case "Lose":
        this.slcLose.SetActive(true);
        break;
    }
    switch (info.battle_result_effect)
    {
      case "GreatEffect":
        this.slcGreat.SetActive(true);
        break;
      case "ExcellentEffect":
        this.slcExcellent.SetActive(true);
        break;
    }
    Future<Sprite> spriteF = EmblemUtility.LoadEmblemSprite(info.target_player.current_emblem_id);
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.guildTitle.sprite2D = spriteF.Result;
  }

  private void DeactiveAllResult()
  {
    this.slcWin.SetActive(false);
    this.slcGreat.SetActive(false);
    this.slcExcellent.SetActive(false);
    this.slcDraw.SetActive(false);
    this.slcLose.SetActive(false);
  }

  public void IbtnDetail() => this.menu.ChangeSceneToDetails(this.battleID);
}
