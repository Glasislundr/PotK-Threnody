// Decompiled with JetBrains decompiler
// Type: MapEdit.MapEditData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
namespace MapEdit
{
  public class MapEditData
  {
    private int countGenerateId_;

    public PlayerGuildTownSlot saveSlot_ { get; private set; }

    public MapTown mapTown_ { get; private set; }

    public BattleMapFacilitySetting[] facilitySetting_ { get; private set; }

    public GvgStageFormation[] formation_ { get; private set; }

    public PlayerGuildTownSlotPosition[] originalPosition_ { get; private set; }

    public BattleStage stage_ { get; private set; }

    public BattleMap map_ { get; private set; }

    public int editId_ { get; private set; }

    public int defaultSlot_ { get; private set; }

    public void resetDefaultSlot()
    {
      this.defaultSlot_ = PlayerAffiliation.Current.default_town_slot_number;
    }

    public void setDefaultSlot(int slotId) => this.defaultSlot_ = slotId;

    public bool isError_ { get; private set; }

    public MapEditPanel[,] matrix_ { get; private set; }

    public int lengthRow_ { get; private set; }

    public int lengthColumn_ { get; private set; }

    public Dictionary<int, MapEditOrnament> dicOrnament_ { get; private set; }

    public Stack<TrackOrnament> stackTrackOrnament_ { get; private set; }

    public int generateId_ => ++this.countGenerateId_;

    public MapEditData(int slotId)
    {
      PlayerGuildTownSlot originalSlot = this.getOriginalSlot(slotId);
      if (originalSlot == null)
        return;
      this.initialize(originalSlot);
    }

    public MapEditData(int slotId, int townId)
    {
      PlayerGuildTownSlot originalSlot = this.getOriginalSlot(slotId);
      if (originalSlot == null)
        return;
      if (originalSlot._master == townId)
        this.initialize(originalSlot);
      else
        this.initialize(new PlayerGuildTownSlot()
        {
          id = originalSlot.id,
          facilities_data = new PlayerGuildTownSlotPosition[0],
          _master = townId,
          slot_number = slotId
        });
    }

    private PlayerGuildTownSlot getOriginalSlot(int slotId)
    {
      PlayerGuildTownSlot[] source = SMManager.Get<PlayerGuildTownSlot[]>();
      if (source != null && source.Length != 0)
        return ((IEnumerable<PlayerGuildTownSlot>) source).FirstOrDefault<PlayerGuildTownSlot>((Func<PlayerGuildTownSlot, bool>) (s => s.slot_number == slotId));
      this.isError_ = true;
      Debug.LogError((object) "\"SMManager.Get<SM.PlayerGuildTownSlot[]>()\"が空です");
      return (PlayerGuildTownSlot) null;
    }

    private void initialize(PlayerGuildTownSlot slot)
    {
      try
      {
        this.resetDefaultSlot();
        this.saveSlot_ = slot;
        this.originalPosition_ = this.finalizePositions(this.saveSlot_.facilities_data);
        this.mapTown_ = this.saveSlot_.master;
        int stageid = this.mapTown_.stage_id;
        this.stage_ = MasterData.BattleStage[stageid];
        this.map_ = this.stage_.map;
        this.facilitySetting_ = ((IEnumerable<BattleMapFacilitySetting>) MasterData.BattleMapFacilitySettingList).Where<BattleMapFacilitySetting>((Func<BattleMapFacilitySetting, bool>) (ms => ms.map_BattleStage == stageid)).ToArray<BattleMapFacilitySetting>();
        this.formation_ = ((IEnumerable<GvgStageFormation>) MasterData.GvgStageFormationList).Where<GvgStageFormation>((Func<GvgStageFormation, bool>) (x => x.stage_BattleStage == stageid)).ToArray<GvgStageFormation>();
        this.editId_ = slot.slot_number;
        this.matrix_ = new MapEditPanel[this.stage_.map_height, this.stage_.map_width];
        this.lengthRow_ = this.matrix_.GetLength(0);
        this.lengthColumn_ = this.matrix_.GetLength(1);
        this.dicOrnament_ = new Dictionary<int, MapEditOrnament>();
        this.stackTrackOrnament_ = new Stack<TrackOrnament>();
        this.isError_ = false;
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex.Message);
        this.isError_ = true;
      }
    }

    private PlayerGuildTownSlotPosition[] finalizePositions(PlayerGuildTownSlotPosition[] positions)
    {
      return positions == null ? new PlayerGuildTownSlotPosition[0] : ((IEnumerable<PlayerGuildTownSlotPosition>) positions).Distinct<PlayerGuildTownSlotPosition>((IEqualityComparer<PlayerGuildTownSlotPosition>) new LambdaEqualityComparer<PlayerGuildTownSlotPosition>((Func<PlayerGuildTownSlotPosition, PlayerGuildTownSlotPosition, bool>) ((a, b) => a.x == b.x && a.y == b.y))).OrderBy<PlayerGuildTownSlotPosition, int>((Func<PlayerGuildTownSlotPosition, int>) (f => f.y)).ThenBy<PlayerGuildTownSlotPosition, int>((Func<PlayerGuildTownSlotPosition, int>) (f => f.x)).ToArray<PlayerGuildTownSlotPosition>();
    }

    private PlayerGuildTownSlotPosition[] finalizePositions(IEnumerable<MapEditOrnament> ornaments)
    {
      return ornaments == null ? new PlayerGuildTownSlotPosition[0] : this.finalizePositions(ornaments.Where<MapEditOrnament>((Func<MapEditOrnament, bool>) (o => o.facility_ != null && o.hasLocation_)).Select<MapEditOrnament, PlayerGuildTownSlotPosition>((Func<MapEditOrnament, PlayerGuildTownSlotPosition>) (o => new PlayerGuildTownSlotPosition()
      {
        master_id = o.facility_._master,
        x = o.column_ + 1,
        y = o.row_ + 1
      })).ToArray<PlayerGuildTownSlotPosition>());
    }

    public void addOrnament(MapEditOrnament ornament)
    {
      if (Object.op_Equality((Object) ornament, (Object) null))
        return;
      this.dicOrnament_.Add(ornament.ID_, ornament);
    }

    public void removeOrnament(int id)
    {
      this.dicOrnament_.Remove(id);
      List<TrackOrnament> list = this.stackTrackOrnament_.ToList<TrackOrnament>();
      int index = 0;
      while (index < list.Count)
      {
        if (list[index].ID_ == id)
          list.RemoveAt(index);
        else
          ++index;
      }
      this.stackTrackOrnament_ = new Stack<TrackOrnament>((IEnumerable<TrackOrnament>) list);
    }

    public bool checkModified(int slotId)
    {
      bool flag;
      try
      {
        PlayerGuildTownSlot playerGuildTownSlot = ((IEnumerable<PlayerGuildTownSlot>) SMManager.Get<PlayerGuildTownSlot[]>()).First<PlayerGuildTownSlot>((Func<PlayerGuildTownSlot, bool>) (s => s.slot_number == slotId));
        flag = playerGuildTownSlot._master != this.saveSlot_._master;
        if (!flag)
        {
          PlayerGuildTownSlotPosition[] townSlotPositionArray1 = this.finalizePositions(this.dicOrnament_.Select<KeyValuePair<int, MapEditOrnament>, MapEditOrnament>((Func<KeyValuePair<int, MapEditOrnament>, MapEditOrnament>) (kv => kv.Value)));
          PlayerGuildTownSlotPosition[] townSlotPositionArray2 = this.finalizePositions(playerGuildTownSlot.facilities_data);
          if (townSlotPositionArray1.Length == townSlotPositionArray2.Length)
          {
            for (int index = 0; index < townSlotPositionArray1.Length; ++index)
            {
              if (townSlotPositionArray1[index].master_id != townSlotPositionArray2[index].master_id || townSlotPositionArray1[index].x != townSlotPositionArray2[index].x || townSlotPositionArray1[index].y != townSlotPositionArray2[index].y)
              {
                flag = true;
                break;
              }
            }
          }
          else
            flag = true;
        }
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex.Message);
        flag = false;
      }
      return flag;
    }

    public IEnumerator doSave(
      int slotId,
      Action<WebAPI.Response.UserError> userErrorCallback)
    {
      this.editId_ = slotId;
      IEnumerator e = this.doSave(userErrorCallback);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }

    public IEnumerator doSave(
      Action<WebAPI.Response.UserError> userErrorCallback)
    {
      bool bChangeDefaultSlot = this.editId_ != this.defaultSlot_;
      if (bChangeDefaultSlot)
      {
        bool bWait = true;
        Consts instance = Consts.GetInstance();
        ModalWindow.ShowYesNo(instance.MAPEDIT_031_TITLE_SETTING_DEFAULTSLOT, instance.MAPEDIT_031_MESSAGE_SETTING_DEFAULTSLOT, (Action) (() => bWait = false), (Action) (() =>
        {
          bChangeDefaultSlot = false;
          bWait = false;
        }));
        while (bWait)
          yield return (object) null;
      }
      if (bChangeDefaultSlot)
        this.setDefaultSlot(this.editId_);
      PlayerGuildTownSlotPosition[] townSlotPositionArray = this.finalizePositions(this.dicOrnament_.Select<KeyValuePair<int, MapEditOrnament>, MapEditOrnament>((Func<KeyValuePair<int, MapEditOrnament>, MapEditOrnament>) (kv => kv.Value)));
      int[] facilities_id = new int[townSlotPositionArray.Length];
      int[] facilities_position_x = new int[townSlotPositionArray.Length];
      int[] facilities_position_y = new int[townSlotPositionArray.Length];
      for (int index = 0; index < townSlotPositionArray.Length; ++index)
      {
        PlayerGuildTownSlotPosition townSlotPosition = townSlotPositionArray[index];
        facilities_id[index] = townSlotPosition.master_id;
        facilities_position_x[index] = townSlotPosition.x;
        facilities_position_y[index] = townSlotPosition.y;
      }
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      bool bError = false;
      IEnumerator e = WebAPI.GuildtownSave(facilities_id, facilities_position_x, facilities_position_y, this.editId_ == this.defaultSlot_, this.editId_, this.mapTown_.ID, (Action<WebAPI.Response.UserError>) (err =>
      {
        bError = true;
        if (userErrorCallback == null)
          return;
        userErrorCallback(err);
      })).Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      if (!bError && bChangeDefaultSlot)
      {
        bool bWait = true;
        Consts instance = Consts.GetInstance();
        ModalWindow.Show(instance.MAPEDIT_031_TITLE_SETTING_DEFAULTSLOT, instance.MAPEDIT_031_RESULT_SETTING_DEFAULTSLOT, (Action) (() => bWait = false));
        while (bWait)
          yield return (object) null;
      }
    }
  }
}
