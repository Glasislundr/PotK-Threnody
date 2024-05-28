// Decompiled with JetBrains decompiler
// Type: Earth.EarthDataManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using MiniJSON;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
namespace Earth
{
  public class EarthDataManager : Singleton<EarthDataManager>
  {
    private bool mIsCreate;
    private bool mInit;
    private bool mIsStoryPlayBackMode;
    private EarthQuestEpisode mDisplayEpsodeData;
    private Dictionary<int, EarthCharacter> mCharacters = new Dictionary<int, EarthCharacter>();
    private Dictionary<int, EarthGear> mGears = new Dictionary<int, EarthGear>();
    private Dictionary<int, EarthItem> mItems = new Dictionary<int, EarthItem>();
    private Dictionary<Tuple<int, int>, EarthCharacterIntimate> mCharacterIntimates = new Dictionary<Tuple<int, int>, EarthCharacterIntimate>();
    private Dictionary<int, EarthQuestKey> mQuestKeys = new Dictionary<int, EarthQuestKey>();
    private Dictionary<int, int> mShopPurchaseHistory = new Dictionary<int, int>();
    private Dictionary<int, EarthQuestKeyPlayData> mKeyQuestOpen = new Dictionary<int, EarthQuestKeyPlayData>();
    private static Future<WebAPI.Response.ZeroLoad> zeroLoad = (Future<WebAPI.Response.ZeroLoad>) null;
    private EarthQuestProgress mQuestProgress;
    public BL.ClassValue<Dictionary<MasterDataTable.CommonRewardType, long>> mUserProperties;
    private static readonly string serverDataFormat = "{{\"characters\":[{0}],\"gears\":[{1}],\"supplies\":[{2}],\"character_intimates\":[{3}],\"quest_progress\":{4},\"user_properties\":[{5}],\"shop_purchase_history\":[{6}],\"quest_keys\":[{7}],\"key_quest_play_data\":[{8}]}}";
    private static readonly string serverUserPropertiseFormat = "{{\"type_id\":{0},\"quantity\":{1}}}";
    private static readonly string serverShopPurchaseHistoryFormat = "{{\"article_id\":{0},\"quantity\":{1}}}";
    private EarthExtraQuest extraQuestInfo;

    public static int GetAutoIndex()
    {
      byte[] byteArray = Guid.NewGuid().ToByteArray();
      return (int) byteArray[0] << 24 | (int) byteArray[1] << 16 | (int) byteArray[2] << 8 | (int) byteArray[3];
    }

    public int revision { get; private set; }

    public bool isCreate => this.mIsCreate;

    public EarthCharacter[] characters => this.mCharacters.Values.ToArray<EarthCharacter>();

    public bool isStoryPlayBackMode
    {
      get => this.mIsStoryPlayBackMode;
      set => this.mIsStoryPlayBackMode = value;
    }

    public EarthQuestEpisode displayEpsodeData
    {
      get => this.mDisplayEpsodeData;
      set => this.mDisplayEpsodeData = value;
    }

    public Dictionary<int, EarthCharacter> characterDict => this.mCharacters;

    public EarthGear[] gears => this.mGears.Values.ToArray<EarthGear>();

    public EarthItem[] items => this.mItems.Values.ToArray<EarthItem>();

    public bool isPrologue => this.questProgress.isPrologue;

    public EarthQuestProgress questProgress => this.mQuestProgress;

    public List<EarthExtraQuest> GetEnableEarthExtraQuestList()
    {
      int episodeID = this.questProgress.currentEpisode.ID;
      return ((IEnumerable<EarthExtraQuest>) MasterData.EarthExtraQuestList).Where<EarthExtraQuest>((Func<EarthExtraQuest, bool>) (x =>
      {
        int num1 = x.start_id.HasValue ? x.start_id.Value : 0;
        int num2 = x.end_id.HasValue ? x.end_id.Value : 10000;
        MasterDataTable.EarthQuestKey earthQuestKey = ((IEnumerable<MasterDataTable.EarthQuestKey>) MasterData.EarthQuestKeyList).FirstOrDefault<MasterDataTable.EarthQuestKey>((Func<MasterDataTable.EarthQuestKey, bool>) (y => y.quest_id == x.ID));
        int num3 = episodeID;
        return num1 <= num3 && episodeID <= num2 && earthQuestKey != null;
      })).ToList<EarthExtraQuest>();
    }

    public static void CreateInstance()
    {
      ((Component) Singleton<NGGameDataManager>.GetInstance()).gameObject.AddComponent<EarthDataManager>().mInit = false;
    }

    public static void DestoryInstance()
    {
      Object.Destroy((Object) Singleton<EarthDataManager>.GetInstanceOrNull());
    }

    protected override void Initialize()
    {
    }

    protected override void Finlaize() => base.Finlaize();

    public void EarthDataRevert()
    {
      if (!this.mInit)
        return;
      this.EarthDataReset();
      this.JsonLoad((Dictionary<string, object>) Json.Deserialize(Persist.earthData.Data.data));
      SMManager.UpdateList<PlayerUnit>(this.GetPlayerUnits(), true);
      SMManager.UpdateList<PlayerItem>(this.GetPlayerItems(), true);
      SMManager.UpdateList<PlayerCharacterIntimate>(this.GetPlayerCharacterIntimates(), true);
      ++this.revision;
      this.mInit = true;
    }

    public void EarthDataReset()
    {
      if (!this.mInit)
        return;
      this.mInit = false;
      this.mCharacters.Clear();
      this.mCharacterIntimates.Clear();
      this.mGears.Clear();
      this.mItems.Clear();
      this.mQuestProgress = (EarthQuestProgress) null;
      this.mUserProperties.value.Clear();
      this.mShopPurchaseHistory.Clear();
      this.mQuestKeys.Clear();
      this.mKeyQuestOpen.Clear();
      EarthDataManager.zeroLoad = (Future<WebAPI.Response.ZeroLoad>) null;
    }

    public IEnumerator EarthDataInit(
      Action<WebAPI.Response.UserError> userErrorCallback = null)
    {
      if (!this.mInit)
      {
        Player player = SMManager.Get<Player>();
        Dictionary<string, object> localJson = (Dictionary<string, object>) null;
        Dictionary<string, object> serverJson = (Dictionary<string, object>) null;
        DateTime? localSaveTime = new DateTime?();
        DateTime? serverUpdateTime = new DateTime?();
        Future<WebAPI.Response.ZeroLoad> rq = (Future<WebAPI.Response.ZeroLoad>) null;
        IEnumerator e;
        if (EarthDataManager.zeroLoad == null)
        {
          rq = WebAPI.ZeroLoad((Action<WebAPI.Response.UserError>) (error =>
          {
            if (userErrorCallback == null)
              WebAPI.DefaultUserErrorCallback(error);
            else
              userErrorCallback(error);
          }));
          e = rq.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        else
          rq = EarthDataManager.zeroLoad;
        if (rq.Result.has_data)
        {
          serverJson = Json.Deserialize(rq.Result.player_data) as Dictionary<string, object>;
          serverUpdateTime = rq.Result.player_data_updated_at;
        }
        if (Persist.oldEarthData.Exists && !Persist.earthData.Exists)
        {
          bool flag = true;
          if (serverJson != null && (int) (long) ((Dictionary<string, object>) serverJson["quest_progress"])["current_stage_index"] > (int) (long) ((Dictionary<string, object>) (Json.Deserialize(Persist.oldEarthData.Data.data) as Dictionary<string, object>)["quest_progress"])["current_stage_index"])
            flag = false;
          if (flag)
          {
            Persist.earthData.Data.data = Persist.oldEarthData.Data.data;
            Persist.earthData.Data.player_id = player.id;
            Persist.earthData.Data.severTime = ServerTime.NowAppTimeAddDelta();
            Persist.earthData.Flush();
            e = WebAPI.ZeroSave(Persist.earthData.Data.data).Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
          Persist.oldEarthData.Delete();
        }
        if (Persist.earthData.Exists && Persist.earthData.Data.player_id == player.id)
        {
          localJson = Json.Deserialize(Persist.earthData.Data.data) as Dictionary<string, object>;
          localSaveTime = new DateTime?(Persist.earthData.Data.severTime);
        }
        if (localJson == null && serverJson == null)
        {
          this.Create();
          e = this.SaveAndSendServer();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        else if (localJson == null && serverJson != null)
        {
          if (Persist.earthBattleEnvironment.Exists)
            Persist.earthBattleEnvironment.Delete();
          this.JsonLoad(serverJson);
          this.Save();
        }
        else if (localJson != null && serverJson == null)
        {
          this.Create();
          e = this.SaveAndSendServer();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        else if (localSaveTime.Value > serverUpdateTime.Value)
        {
          this.JsonLoad(localJson);
        }
        else
        {
          if (Persist.earthBattleEnvironment.Exists)
            Persist.earthBattleEnvironment.Delete();
          this.JsonLoad(serverJson);
          this.Save();
        }
        SMManager.UpdateList<PlayerUnit>(this.GetPlayerUnits(), true);
        SMManager.UpdateList<PlayerItem>(this.GetPlayerItems(), true);
        SMManager.UpdateList<PlayerCharacterIntimate>(this.GetPlayerCharacterIntimates(), true);
        EarthDataManager.zeroLoad = (Future<WebAPI.Response.ZeroLoad>) null;
        this.mInit = true;
      }
    }

    private IEnumerator SendSave()
    {
      IEnumerator e = WebAPI.ZeroSave(this.GetServerString()).Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }

    public static void SetZeroLoad(Future<WebAPI.Response.ZeroLoad> zero)
    {
      EarthDataManager.zeroLoad = zero;
    }

    public void Save()
    {
      Player player = SMManager.Get<Player>();
      Persist.earthData.Data.data = this.GetServerString();
      Persist.earthData.Data.player_id = player.id;
      Persist.earthData.Data.severTime = ServerTime.NowAppTimeAddDelta();
      Persist.earthData.Flush();
    }

    public IEnumerator SaveAndSendServer()
    {
      IEnumerator e = this.SendSave();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Player player = SMManager.Get<Player>();
      Persist.earthData.Data.data = this.GetServerString();
      Persist.earthData.Data.player_id = player.id;
      Persist.earthData.Data.severTime = ServerTime.NowAppTimeAddDelta();
      Persist.earthData.Flush();
    }

    private void Create()
    {
      this.mIsCreate = true;
      foreach (EarthJoinCharacter data in ((IEnumerable<EarthJoinCharacter>) MasterData.EarthJoinCharacterList).Where<EarthJoinCharacter>((Func<EarthJoinCharacter, bool>) (x => x.join_logic == EarthJoinLogicType.none)))
      {
        EarthCharacter earthCharacter = EarthCharacter.Create(data, ((IEnumerable<EarthCharacter>) this.characters).Where<EarthCharacter>((Func<EarthCharacter, bool>) (x => x.unit.IsNormalUnit)).Count<EarthCharacter>());
        this.mCharacters.Add(earthCharacter.ID, earthCharacter);
      }
      foreach (EarthQuestClearReward questClearReward in ((IEnumerable<EarthQuestClearReward>) MasterData.EarthQuestClearRewardList).Where<EarthQuestClearReward>((Func<EarthQuestClearReward, bool>) (x => x.episode == null)))
        this.EarthRewardAdd((int) questClearReward.reward_type, questClearReward.reward_id, questClearReward.quantity);
      this.mQuestProgress = EarthQuestProgress.Create();
      this.mUserProperties = new BL.ClassValue<Dictionary<MasterDataTable.CommonRewardType, long>>(new Dictionary<MasterDataTable.CommonRewardType, long>());
      this.ClearCharacterBattleIndex();
      this.ForceDeselectEventQuestBattleIndex();
      Persist.earthData.Data.data = this.GetServerString();
      Persist.earthData.Flush();
    }

    private void JsonLoadOnlyCharacters(Dictionary<string, object> json)
    {
      this.mCharacters.Clear();
      foreach (Dictionary<string, object> json1 in (List<object>) json["characters"])
      {
        EarthCharacter earthCharacter = EarthCharacter.JsonLoad(json1);
        this.mCharacters.Add(earthCharacter.ID, earthCharacter);
      }
    }

    private EarthQuestProgress GetQuestProgress(Dictionary<string, object> json)
    {
      return EarthQuestProgress.JsonLoad((Dictionary<string, object>) json["quest_progress"], (Action) (() => { }));
    }

    private int GetCharactorTotalExp(Dictionary<string, object> json)
    {
      List<object> objectList = (List<object>) json["characters"];
      int charactorTotalExp = 0;
      foreach (Dictionary<string, object> json1 in objectList)
      {
        EarthCharacter earthCharacter = EarthCharacter.JsonLoad(json1);
        charactorTotalExp += earthCharacter.experience;
      }
      return charactorTotalExp;
    }

    private void JsonLoad(Dictionary<string, object> json)
    {
      foreach (Dictionary<string, object> json1 in (List<object>) json["characters"])
      {
        EarthCharacter earthCharacter = EarthCharacter.JsonLoad(json1);
        this.mCharacters.Add(earthCharacter.ID, earthCharacter);
      }
      foreach (Dictionary<string, object> json2 in (List<object>) json["gears"])
      {
        EarthGear earthGear = EarthGear.JsonLoad(json2);
        this.mGears.Add(earthGear.ID, earthGear);
      }
      foreach (Dictionary<string, object> json3 in (List<object>) json["supplies"])
      {
        EarthItem earthItem = EarthItem.JsonLoad(json3);
        this.mItems.Add(earthItem.ID, earthItem);
      }
      foreach (Dictionary<string, object> json4 in (List<object>) json["character_intimates"])
      {
        EarthCharacterIntimate characterIntimate = EarthCharacterIntimate.JsonLoad(json4);
        this.mCharacterIntimates.Add(characterIntimate.key, characterIntimate);
      }
      if (json.ContainsKey("quest_keys"))
      {
        foreach (Dictionary<string, object> json5 in (List<object>) json["quest_keys"])
        {
          EarthQuestKey earthQuestKey = EarthQuestKey.JsonLoad(json5);
          this.mQuestKeys.Add(earthQuestKey.keyID, earthQuestKey);
        }
      }
      Dictionary<string, object> json6 = (Dictionary<string, object>) json["quest_progress"];
      bool isCallClearCharacterBattleIndex = false;
      this.mQuestProgress = EarthQuestProgress.JsonLoad(json6, (Action) (() => isCallClearCharacterBattleIndex = true));
      this.mUserProperties = new BL.ClassValue<Dictionary<MasterDataTable.CommonRewardType, long>>(new Dictionary<MasterDataTable.CommonRewardType, long>());
      foreach (Dictionary<string, object> dictionary in (List<object>) json["user_properties"])
        this.mUserProperties.value.Add((MasterDataTable.CommonRewardType) (long) dictionary["type_id"], (long) (int) (long) dictionary["quantity"]);
      this.mUserProperties.commit();
      this.mShopPurchaseHistory = new Dictionary<int, int>();
      if (json.ContainsKey("shop_purchase_history"))
      {
        foreach (Dictionary<string, object> dictionary in (List<object>) json["shop_purchase_history"])
          this.mShopPurchaseHistory.Add((int) (long) dictionary["article_id"], (int) (long) dictionary["quantity"]);
      }
      this.mKeyQuestOpen = new Dictionary<int, EarthQuestKeyPlayData>();
      if (json.ContainsKey("key_quest_play_data"))
      {
        foreach (Dictionary<string, object> json7 in (List<object>) json["key_quest_play_data"])
        {
          EarthQuestKeyPlayData questKeyPlayData = EarthQuestKeyPlayData.JsonLoad(json7);
          this.mKeyQuestOpen.Add(questKeyPlayData.ID, questKeyPlayData);
        }
      }
      if (!isCallClearCharacterBattleIndex)
        return;
      this.ClearCharacterBattleIndex();
      this.ForceDeselectEventQuestBattleIndex();
    }

    public void CreateIntimate(int characterID)
    {
      foreach (EarthCharacter earthCharacter in this.mCharacters.Values)
      {
        if (characterID != earthCharacter.character.ID)
        {
          Tuple<int, int> key = new Tuple<int, int>(characterID < earthCharacter.character.ID ? characterID : earthCharacter.character.ID, characterID > earthCharacter.character.ID ? characterID : earthCharacter.character.ID);
          if (!this.mCharacterIntimates.ContainsKey(key))
            this.mCharacterIntimates.Add(key, EarthCharacterIntimate.Create(key.Item1, key.Item2));
        }
      }
    }

    public void EarthCharacterAdd(EarthJoinCharacter joinData)
    {
      EarthCharacter earthCharacter = EarthCharacter.Create(joinData, ((IEnumerable<EarthCharacter>) this.characters).Where<EarthCharacter>((Func<EarthCharacter, bool>) (x => x.unit.IsNormalUnit)).Count<EarthCharacter>());
      this.mCharacters.Add(earthCharacter.ID, earthCharacter);
      SMManager.UpdateList<PlayerUnit>(new PlayerUnit[1]
      {
        earthCharacter.GetPlayerUnit()
      });
    }

    public void EarthCharacterAdd(int characterID, int unitID)
    {
      EarthCharacter earthCharacter = EarthCharacter.Create(characterID, unitID, -1);
      this.mCharacters.Add(earthCharacter.ID, earthCharacter);
      SMManager.UpdateList<PlayerUnit>(new PlayerUnit[1]
      {
        earthCharacter.GetPlayerUnit()
      });
    }

    public Tuple<bool, int> EarthRewardAdd(int item_type, int? item_id, int quantity)
    {
      int num = 0;
      bool flag = false;
      if (item_id.HasValue)
      {
        switch (item_type)
        {
          case 2:
            flag = !this.mItems.Values.Any<EarthItem>((Func<EarthItem, bool>) (x => x.itemID == item_id.Value));
            this.EarthItemAdd(item_id.Value, quantity);
            num = item_id.Value;
            break;
          case 3:
            flag = !this.mGears.Values.Any<EarthGear>((Func<EarthGear, bool>) (x => x.gearID == item_id.Value));
            for (int index = 0; index < quantity; ++index)
              num = this.EarthGearAdd(item_id.Value).ID;
            break;
          case 19:
            flag = !this.mQuestKeys.Values.Any<EarthQuestKey>((Func<EarthQuestKey, bool>) (x => x.keyID == item_id.Value));
            this.EarthQuestKeyAdd(item_id.Value, quantity);
            num = item_id.Value;
            break;
        }
      }
      else
      {
        try
        {
          MasterDataTable.CommonRewardType key = (MasterDataTable.CommonRewardType) item_type;
          if (this.mUserProperties.value.ContainsKey(key))
            this.mUserProperties.value[key] += (long) quantity;
          else
            this.mUserProperties.value.Add(key, (long) quantity);
          this.mUserProperties.commit();
        }
        catch (Exception ex)
        {
          Debug.LogException(ex);
        }
      }
      return new Tuple<bool, int>(flag, num);
    }

    public EarthGear EarthGearAdd(int gearID)
    {
      EarthGear earthGear = EarthGear.Create(gearID);
      this.mGears.Add(earthGear.ID, earthGear);
      SMManager.UpdateList<PlayerItem>(new PlayerItem[1]
      {
        earthGear.GetPlayerItem()
      });
      return earthGear;
    }

    public EarthItem EarthItemAdd(int supplyID, int quantity)
    {
      if (this.mItems.ContainsKey(supplyID))
        this.mItems[supplyID].quantity += quantity;
      else
        this.mItems.Add(supplyID, EarthItem.Create(supplyID, quantity));
      SMManager.UpdateList<PlayerItem>(this.mItems[supplyID].GetPlayerItemList().ToArray());
      return this.mItems[supplyID];
    }

    public EarthQuestKey EarthQuestKeyAdd(int keyID, int quantity)
    {
      if (this.mQuestKeys.ContainsKey(keyID))
        this.mQuestKeys[keyID].quantity += quantity;
      else
        this.mQuestKeys.Add(keyID, EarthQuestKey.Create(keyID, quantity));
      return this.mQuestKeys[keyID];
    }

    public void ItemSell(int[] gear_id, int[] item_id, int[] quantity, long addMoney)
    {
      int length1 = gear_id.Length;
      for (int index = 0; index < length1; ++index)
        this.EarthGearSub(gear_id[index]);
      int length2 = item_id.Length;
      for (int index = 0; index < length2; ++index)
        this.EarthItemSub(item_id[index], quantity[index]);
      SMManager.UpdateList<PlayerItem>(this.GetPlayerItems(), true);
      this.mUserProperties.value[MasterDataTable.CommonRewardType.money] += addMoney;
      this.mUserProperties.commit();
      this.Save();
    }

    private void EarthRewardSub(MasterDataTable.CommonRewardType item_type, int[] item_id, int[] quantity)
    {
      int length = item_id.Length;
      for (int index = 0; index < length; ++index)
      {
        switch (item_type)
        {
          case MasterDataTable.CommonRewardType.supply:
            this.EarthItemSub(item_id[index], quantity[index]);
            break;
          case MasterDataTable.CommonRewardType.gear:
            this.EarthGearSub(item_id[index]);
            break;
        }
      }
    }

    private void EarthGearSub(int gearID)
    {
      if (!this.mGears.ContainsKey(gearID))
        return;
      this.mGears[gearID].SetLost();
    }

    private void EarthItemSub(int supplyID, int quantity)
    {
      if (!this.mItems.ContainsKey(supplyID))
        return;
      this.mItems[supplyID].quantity -= quantity;
      if (this.mItems[supplyID].quantity >= 0)
        return;
      this.mItems[supplyID].quantity = 0;
    }

    public EarthQuestKey[] GetQuestKeys()
    {
      return this.mQuestKeys.Values.Where<EarthQuestKey>((Func<EarthQuestKey, bool>) (x => x != null)).OrderBy<EarthQuestKey, int>((Func<EarthQuestKey, int>) (x => x.ID)).ToArray<EarthQuestKey>();
    }

    public PlayerUnit[] GetPlayerUnits()
    {
      return this.mCharacters.Values.Where<EarthCharacter>((Func<EarthCharacter, bool>) (x => !x.isFall && !x.isDesert)).OrderBy<EarthCharacter, int>((Func<EarthCharacter, int>) (x => x.index)).Select<EarthCharacter, PlayerUnit>((Func<EarthCharacter, PlayerUnit>) (x => x.GetPlayerUnit())).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null)).ToArray<PlayerUnit>();
    }

    public PlayerUnit[] GetEnableSortiePlayerUnits()
    {
      return this.mCharacters.Values.Where<EarthCharacter>((Func<EarthCharacter, bool>) (x => !x.isFall && !x.isDesert && !this.questProgress.isImpossibleOfSortie(x.character.ID) && !x.unit.IsMaterialUnit)).OrderBy<EarthCharacter, int>((Func<EarthCharacter, int>) (x => x.index)).Select<EarthCharacter, PlayerUnit>((Func<EarthCharacter, PlayerUnit>) (x => x.GetPlayerUnit())).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null)).ToArray<PlayerUnit>();
    }

    public PlayerUnit GetPlayerUnit(int id)
    {
      return this.mCharacters.ContainsKey(id) ? this.mCharacters[id].GetPlayerUnit() : (PlayerUnit) null;
    }

    public int SetCharacterIndex(int playerunit_id, int index)
    {
      return index > 1 && this.mCharacters.ContainsKey(playerunit_id) ? this.mCharacters[playerunit_id].SetIndex(index) : -1;
    }

    public int GetEventQuestDeckCharacterCount()
    {
      return ((IEnumerable<EarthCharacter>) this.characters).Count<EarthCharacter>((Func<EarthCharacter, bool>) (x => x.eventQuestBattleIndex > 0 && !x.isFall));
    }

    public void ForceDeselectEventQuestBattleIndex()
    {
      ((IEnumerable<EarthCharacter>) this.characters).ForEach<EarthCharacter>((Action<EarthCharacter>) (x => x.SetEventQuestBattleIndex(0)));
    }

    public void ClearCharacterBattleIndex()
    {
      int num = 0;
      foreach (EarthCharacter earthCharacter in (IEnumerable<EarthCharacter>) ((IEnumerable<EarthCharacter>) this.characters).Where<EarthCharacter>((Func<EarthCharacter, bool>) (x => !x.isFall && !x.isDesert)).OrderBy<EarthCharacter, int>((Func<EarthCharacter, int>) (x => x.index)))
        earthCharacter.SetIndex(num++);
      ((IEnumerable<EarthCharacter>) this.characters).ForEach<EarthCharacter>((Action<EarthCharacter>) (x => x.SetBattleIndex(0)));
      int maxCount = this.questProgress.MaximumNumberOfSorties;
      int count = 1;
      foreach (EarthForcedSortieCharacter forcedSortieCharacter1 in this.questProgress.forcedSortieCharacters)
      {
        EarthForcedSortieCharacter forcedSortieCharacter = forcedSortieCharacter1;
        if (((IEnumerable<EarthCharacter>) this.characters).Any<EarthCharacter>((Func<EarthCharacter, bool>) (x => x.character.ID == forcedSortieCharacter.character_id)))
        {
          EarthCharacter earthCharacter = ((IEnumerable<EarthCharacter>) this.characters).Where<EarthCharacter>((Func<EarthCharacter, bool>) (x => x.character.ID == forcedSortieCharacter.character_id)).FirstOrDefault<EarthCharacter>();
          if (earthCharacter != null && !earthCharacter.isFall && !earthCharacter.isDesert)
            earthCharacter.SetBattleIndex(forcedSortieCharacter.sortie_position);
        }
      }
      ((IEnumerable<EarthCharacter>) this.characters).OrderBy<EarthCharacter, int>((Func<EarthCharacter, int>) (x => x.index)).ForEach<EarthCharacter>((Action<EarthCharacter>) (x =>
      {
        if (x.index < 0 || x.battleIndex != 0 || count > maxCount || this.questProgress.isImpossibleOfSortie(x.character.ID))
          return;
        x.SetBattleIndex(count);
        do
        {
          ++count;
        }
        while (this.questProgress.forcedSortieCharacters.Any<EarthForcedSortieCharacter>((Func<EarthForcedSortieCharacter, bool>) (condition => condition.sortie_position == count)));
      }));
    }

    public int SetCharacterBattleIndex(int playerunit_id, int index)
    {
      return index > 1 && this.mCharacters.ContainsKey(playerunit_id) ? this.mCharacters[playerunit_id].SetBattleIndex(index) : -1;
    }

    public bool EquipGear(int playerUnitID, int playerItemID)
    {
      if (!this.mCharacters.ContainsKey(playerUnitID))
        return false;
      List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
      EarthCharacter earthCharacter = this.mCharacters.Values.FirstOrDefault<EarthCharacter>((Func<EarthCharacter, bool>) (x => x.equipeGearID == playerItemID));
      if (earthCharacter != null)
      {
        earthCharacter.EquipGear(0);
        playerUnitList.Add(earthCharacter.GetPlayerUnit());
      }
      this.mCharacters[playerUnitID].EquipGear(playerItemID);
      playerUnitList.Add(this.mCharacters[playerUnitID].GetPlayerUnit());
      SMManager.UpdateList<PlayerUnit>(playerUnitList.ToArray());
      return true;
    }

    public PlayerUnit[] GetEvolutionPattern(int playerUnitID, int[] patternIDs)
    {
      return this.mCharacters.ContainsKey(playerUnitID) ? this.mCharacters[playerUnitID].GetEvolutionPlayerUnits(patternIDs).ToArray() : (PlayerUnit[]) null;
    }

    public Tuple<PlayerUnit, PlayerUnit> EvolutionUnit(
      int playerUnitID,
      int patternID,
      List<int> materailUnitIDs)
    {
      if (this.mCharacters.ContainsKey(playerUnitID))
      {
        if (!materailUnitIDs.All<int>((Func<int, bool>) (x => ((IEnumerable<EarthCharacter>) this.characters).Any<EarthCharacter>((Func<EarthCharacter, bool>) (y => !y.isFall && y.ID == x)))))
          return (Tuple<PlayerUnit, PlayerUnit>) null;
        materailUnitIDs.ForEach((Action<int>) (x => this.mCharacters[x].SetDeath()));
        PlayerUnit playerUnit = CopyUtil.DeepCopy<PlayerUnit>(this.mCharacters[playerUnitID].GetPlayerUnit());
        if (this.mCharacters[playerUnitID].Evolution(patternID))
        {
          SMManager.UpdateList<PlayerUnit>(new PlayerUnit[1]
          {
            this.mCharacters[playerUnitID].GetPlayerUnit()
          });
          SMManager.DeleteList<PlayerUnit>(materailUnitIDs.Select<int, object>((Func<int, object>) (x => (object) x)).ToArray<object>());
          return new Tuple<PlayerUnit, PlayerUnit>(playerUnit, this.mCharacters[playerUnitID].GetPlayerUnit());
        }
      }
      return (Tuple<PlayerUnit, PlayerUnit>) null;
    }

    public PlayerItem[] GetPlayerItems()
    {
      return this.mItems.Values.SelectMany<EarthItem, PlayerItem>((Func<EarthItem, IEnumerable<PlayerItem>>) (x => (IEnumerable<PlayerItem>) x.GetPlayerItemList())).Concat<PlayerItem>(this.mGears.Values.Where<EarthGear>((Func<EarthGear, bool>) (x => !x.isLost)).Select<EarthGear, PlayerItem>((Func<EarthGear, PlayerItem>) (x => x.GetPlayerItem()))).ToArray<PlayerItem>();
    }

    public PlayerCharacterIntimate[] GetPlayerCharacterIntimates()
    {
      return this.mCharacterIntimates.Values.Select<EarthCharacterIntimate, PlayerCharacterIntimate>((Func<EarthCharacterIntimate, PlayerCharacterIntimate>) (x => x.GetPlayerCharacterIntimate())).ToArray<PlayerCharacterIntimate>();
    }

    public PlayerCharacterIntimate GetPlayerCharacterIntimate(int characterID1, int characterID2)
    {
      if (characterID1 == characterID2)
        return (PlayerCharacterIntimate) null;
      Tuple<int, int> key = new Tuple<int, int>(characterID1 < characterID2 ? characterID1 : characterID2, characterID1 > characterID2 ? characterID1 : characterID2);
      return this.mCharacterIntimates.ContainsKey(key) ? this.mCharacterIntimates[key].GetPlayerCharacterIntimate() : (PlayerCharacterIntimate) null;
    }

    public void SetFavoriteGear(int playeritem_id, bool favorite)
    {
      if (!this.mGears.ContainsKey(playeritem_id))
        return;
      this.mGears[playeritem_id].favorite = favorite;
      SMManager.UpdateList<PlayerItem>(new PlayerItem[1]
      {
        this.mGears[playeritem_id].GetPlayerItem()
      });
    }

    public void SetLostGear(int playeritem_id)
    {
      if (!this.mGears.ContainsKey(playeritem_id))
        return;
      this.mGears[playeritem_id].SetLost();
      SMManager.DeleteList<PlayerItem>(new object[1]
      {
        (object) playeritem_id
      });
    }

    public void SetSupplyBox(Tuple<int, int>[] supplySetDataList)
    {
      IEnumerable<object> first = ((IEnumerable<PlayerItem>) this.GetPlayerItems()).Select<PlayerItem, object>((Func<PlayerItem, object>) (x => (object) x.id));
      IEnumerable<int> source = ((IEnumerable<Tuple<int, int>>) supplySetDataList).Select<Tuple<int, int>, int>((Func<Tuple<int, int>, int>) (x => x.Item1));
      foreach (EarthItem earthItem in this.mItems.Values.Where<EarthItem>((Func<EarthItem, bool>) (x => x.battleSetCount > 0)))
      {
        if (!source.Contains<int>(earthItem.ID))
          earthItem.battleSetCount = 0;
      }
      foreach (Tuple<int, int> supplySetData in supplySetDataList)
      {
        if (this.mItems.ContainsKey(supplySetData.Item1))
          this.mItems[supplySetData.Item1].battleSetCount = supplySetData.Item2;
      }
      PlayerItem[] playerItems = this.GetPlayerItems();
      IEnumerable<object> second = ((IEnumerable<PlayerItem>) playerItems).Select<PlayerItem, object>((Func<PlayerItem, object>) (x => (object) x.id));
      SMManager.UpdateList<PlayerItem>(((IEnumerable<PlayerItem>) playerItems).ToArray<PlayerItem>());
      SMManager.DeleteList<PlayerItem>(first.Except<object>(second).ToArray<object>());
    }

    public long GetProperty(MasterDataTable.CommonRewardType type)
    {
      return this.mUserProperties.value.ContainsKey(type) ? this.mUserProperties.value[type] : 0L;
    }

    private string GetServerString()
    {
      return string.Format(EarthDataManager.serverDataFormat, (object) string.Join(",", this.mCharacters.Values.Select<EarthCharacter, string>((Func<EarthCharacter, string>) (x => x.GetSeverString())).ToArray<string>()), (object) string.Join(",", this.mGears.Values.Select<EarthGear, string>((Func<EarthGear, string>) (x => x.GetSeverString())).ToArray<string>()), (object) string.Join(",", this.mItems.Values.Select<EarthItem, string>((Func<EarthItem, string>) (x => x.GetSeverString())).ToArray<string>()), (object) string.Join(",", this.mCharacterIntimates.Values.Select<EarthCharacterIntimate, string>((Func<EarthCharacterIntimate, string>) (x => x.GetSeverString())).ToArray<string>()), (object) this.mQuestProgress.GetSeverString(), (object) string.Join(",", this.mUserProperties.value.Select<KeyValuePair<MasterDataTable.CommonRewardType, long>, string>((Func<KeyValuePair<MasterDataTable.CommonRewardType, long>, string>) (x => string.Format(EarthDataManager.serverUserPropertiseFormat, (object) (int) x.Key, (object) x.Value))).ToArray<string>()), (object) string.Join(",", this.mShopPurchaseHistory.Select<KeyValuePair<int, int>, string>((Func<KeyValuePair<int, int>, string>) (x => string.Format(EarthDataManager.serverShopPurchaseHistoryFormat, (object) x.Key, (object) x.Value))).ToArray<string>()), (object) string.Join(",", this.mQuestKeys.Values.Select<EarthQuestKey, string>((Func<EarthQuestKey, string>) (x => x.GetSeverString())).ToArray<string>()), (object) string.Join(",", this.mKeyQuestOpen.Values.Select<EarthQuestKeyPlayData, string>((Func<EarthQuestKeyPlayData, string>) (x => x.GetSeverString())).ToArray<string>()));
    }

    public void GoPrologueScene() => this.questProgress.GoPrologueScene();

    public void PrevPrologueScene()
    {
      --this.questProgress.prologueIndex;
      this.Save();
      if (this.questProgress.isPrologue)
        this.questProgress.GoPrologueScene();
      else
        Mypage051Scene.ChangeScene(false);
    }

    public void NextPrologueScene()
    {
      ++this.questProgress.prologueIndex;
      this.Save();
      if (this.questProgress.isPrologue)
      {
        this.questProgress.GoPrologueScene();
      }
      else
      {
        Singleton<CommonRoot>.GetInstance().isLoading = true;
        this.StartCoroutine(this.SaveEndTutorial());
      }
    }

    private IEnumerator SaveEndTutorial()
    {
      IEnumerator e = this.SendSave();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<NGSceneManager>.GetInstance().sceneBase.IsPush = true;
      Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
      Mypage051Scene.ChangeScene(false);
    }

    public void ShopBuy(EarthShopArticle article, int buyNum)
    {
      if (buyNum < 1)
        return;
      this.mUserProperties.value[MasterDataTable.CommonRewardType.money] -= (long) (article.price * buyNum);
      this.mUserProperties.commit();
      this.EarthRewardAdd((int) article.ShopContents.entity_type, new int?(article.ShopContents.entity_id), article.ShopContents.quantity * buyNum);
      if (this.mShopPurchaseHistory.ContainsKey(article.ID))
        this.mShopPurchaseHistory[article.ID] += buyNum;
      else
        this.mShopPurchaseHistory[article.ID] = buyNum;
      this.Save();
    }

    public int GetShopPurchaseCount(int id)
    {
      return this.mShopPurchaseHistory.ContainsKey(id) ? this.mShopPurchaseHistory[id] : 0;
    }

    public bool IsKeyQuestOpen(int keyId)
    {
      return this.mKeyQuestOpen.ContainsKey(keyId) && this.mKeyQuestOpen[keyId].Open;
    }

    public int GetKeyQuestPlayCount(int keyId)
    {
      int keyQuestPlayCount = 0;
      if (this.mKeyQuestOpen.ContainsKey(keyId))
        keyQuestPlayCount = this.mKeyQuestOpen[keyId].PlayCount;
      return keyQuestPlayCount;
    }

    public void AddKeyQuestPlayCount(int keyId) => ++this.mKeyQuestOpen[keyId].PlayCount;

    public void KeyQuestOpen(int keyId, int num)
    {
      this.EarthQuestKeyAdd(keyId, -num);
      if (!this.mKeyQuestOpen.ContainsKey(keyId))
        this.mKeyQuestOpen.Add(keyId, new EarthQuestKeyPlayData()
        {
          ID = keyId,
          PlayCount = 0,
          Open = false
        });
      this.mKeyQuestOpen[keyId].Open = true;
      this.mKeyQuestOpen[keyId].PlayCount = 0;
      this.Save();
    }

    public void KeyQuestClose(int keyId)
    {
      this.mKeyQuestOpen[keyId].Open = false;
      this.Save();
    }

    public IEnumerator BattleInitExtra(EarthExtraQuest quest, bool isRetreatEnable = true)
    {
      this.extraQuestInfo = quest;
      IEnumerator e = MasterData.LoadBattleStageEnemy(quest.stage);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.BattleInit(CommonQuestType.EarthExtra, quest.stage.ID, quest.ID, quest.stage.Enemies, quest.stage.Guests, isRetreatEnable);
    }

    public IEnumerator BattleInitStory(bool isRetreatEnable = true)
    {
      this.extraQuestInfo = (EarthExtraQuest) null;
      IEnumerator e = MasterData.LoadBattleStageEnemy(this.questProgress.currentEpisode.stage);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.BattleInit(CommonQuestType.Earth, this.questProgress.currentEpisode.stage.ID, this.questProgress.currentEpisode.ID, this.questProgress.currentEpisode.stage.Enemies, this.questProgress.currentEpisode.stage.Guests, isRetreatEnable);
    }

    private void BattleInit(
      CommonQuestType questType,
      int stageID,
      int questSID,
      BattleStageEnemy[] enemies,
      BattleEarthStageGuest[] guests,
      bool isRetreatEnable)
    {
      this.Save();
      PlayerDeck playerDeck = new PlayerDeck();
      playerDeck.member_limit = this.extraQuestInfo != null ? this.extraQuestInfo.stage.Players.Length : this.questProgress.MaximumNumberOfSorties;
      playerDeck.cost_limit = 999;
      playerDeck.deck_number = 0;
      playerDeck.deck_type_id = 0;
      int?[] nullableArray = new int?[playerDeck.member_limit];
      for (int i = 0; i < playerDeck.member_limit; i++)
      {
        int num = questType != CommonQuestType.Earth ? ((IEnumerable<EarthCharacter>) this.characters).Where<EarthCharacter>((Func<EarthCharacter, bool>) (x => x.eventQuestBattleIndex == i + 1 && !x.isFall)).Select<EarthCharacter, int>((Func<EarthCharacter, int>) (x => x.ID)).FirstOrDefault<int>() : ((IEnumerable<EarthCharacter>) this.characters).Where<EarthCharacter>((Func<EarthCharacter, bool>) (x => x.battleIndex == i + 1 && !x.isFall)).Select<EarthCharacter, int>((Func<EarthCharacter, int>) (x => x.ID)).FirstOrDefault<int>();
        nullableArray[i] = num == 0 ? new int?() : new int?(num);
      }
      playerDeck.player_unit_ids = nullableArray;
      SMManager.UpdateList<PlayerDeck>(new PlayerDeck[1]
      {
        playerDeck
      });
      List<int> intList = new List<int>();
      foreach (IGrouping<int, EarthBattleStagePanelEvent> source in ((IEnumerable<EarthBattleStagePanelEvent>) MasterData.EarthBattleStagePanelEventList).Where<EarthBattleStagePanelEvent>((Func<EarthBattleStagePanelEvent, bool>) (x => x.stage_id == stageID)).GroupBy<EarthBattleStagePanelEvent, int>((Func<EarthBattleStagePanelEvent, int>) (x => x.set_group)))
      {
        if (source.Key == 0)
        {
          intList.AddRange(source.Select<EarthBattleStagePanelEvent, int>((Func<EarthBattleStagePanelEvent, int>) (x => x.ID)));
        }
        else
        {
          List<EarthBattleStagePanelEvent> list = source.ToList<EarthBattleStagePanelEvent>();
          int index = NC.Lot(list.Select<EarthBattleStagePanelEvent, int>((Func<EarthBattleStagePanelEvent, int>) (x => x.group_appearance)).ToArray<int>());
          intList.Add(list[index].ID);
        }
      }
      List<Tuple<int, int, int, int>> tupleList1 = new List<Tuple<int, int, int, int>>();
      foreach (int key in intList)
      {
        BattleEarthItemDropTable dropItem = BattleEarthItemDropTable.RandomGetDropItem(MasterData.EarthBattleStagePanelEvent[key].drop_table_id);
        if (dropItem != null)
          tupleList1.Add(new Tuple<int, int, int, int>(key, (int) dropItem.reward_type, dropItem.reward_id.HasValue ? dropItem.reward_id.Value : 0, dropItem.quantity));
      }
      List<Tuple<int, int, int, int>> tupleList2 = new List<Tuple<int, int, int, int>>();
      foreach (BattleStageEnemy enemy in enemies)
      {
        if (MasterData.BattleStageEnemyReward[enemy.ID].drop_id > 0)
        {
          BattleEarthItemDropTable dropItem = BattleEarthItemDropTable.RandomGetDropItem(MasterData.BattleStageEnemyReward[enemy.ID].drop_id);
          if (dropItem != null)
            tupleList2.Add(new Tuple<int, int, int, int>(enemy.ID, (int) dropItem.reward_type, dropItem.reward_id.HasValue ? dropItem.reward_id.Value : 0, dropItem.quantity));
        }
      }
      BattleInfo info = BattleInfo.MakeBattleInfo(Guid.NewGuid().ToString(), questType, questSID, 0, 0, 0, (PlayerHelper) null, ((IEnumerable<BattleStageEnemy>) enemies).Select<BattleStageEnemy, int>((Func<BattleStageEnemy, int>) (x => x.ID)).ToArray<int>(), tupleList2.ToArray(), new PlayerUnit[0], new PlayerItem[0], new int[0], new Tuple<int, int, int, int>[0], intList.ToArray(), tupleList1.ToArray(), ((IEnumerable<BattleEarthStageGuest>) guests).Select<BattleEarthStageGuest, int>((Func<BattleEarthStageGuest, int>) (x => x.ID)).ToArray<int>(), (PlayerUnit[]) null, (Tuple<int, int>[]) null);
      info.isRetreatEnable = isRetreatEnable;
      CommonRoot instance = Singleton<CommonRoot>.GetInstance();
      instance.loadingMode = 4;
      if (instance.getCloudAnimEnabled())
      {
        instance.isLoading = true;
        instance.StartCloudAnimEnd((Action) (() => Singleton<NGBattleManager>.GetInstance().startBattle(info)));
      }
      else
        Singleton<NGBattleManager>.GetInstance().startBattle(info);
    }

    public Future<BattleEnd> BattleFinish(WebAPI.Request.BattleFinish request, BE be)
    {
      BattleEnd battleEnd = new BattleEnd();
      if (!request.win)
        return Future.Single<BattleEnd>(battleEnd);
      battleEnd.battle_helpers = new PlayerHelper[0];
      battleEnd.boost_stage_clear_rewards = new BattleEndBoost_stage_clear_rewards[0];
      battleEnd.disappeared_player_gears = new int[0];
      battleEnd.drop_unit_entities = new BattleEndDrop_unit_entities[0];
      battleEnd.drop_gacha_ticket_entities = new BattleEndDrop_gacha_ticket_entities[0];
      battleEnd.incr_friend_point = 0;
      battleEnd.mission_complete_rewards = new BattleEndMission_complete_rewards[0];
      battleEnd.player_incr_exp = 0;
      battleEnd.player_mission_results = new PlayerMissionHistory[0];
      battleEnd.player_review = (BattleEndPlayer_review) null;
      battleEnd.score_campaigns = new QuestScoreBattleFinishContext[0];
      battleEnd.stage_clear_rewards = new BattleEndStage_clear_rewards[0];
      battleEnd.unlock_messages = new BattleEndUnlock_messages[0];
      battleEnd.unlock_quests = new UnlockQuest[0];
      battleEnd.before_player_gears = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerDeck[]>()[0].player_units).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null)).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.equip_gear_ids[0].HasValue)).Select<PlayerUnit, PlayerItem>((Func<PlayerUnit, PlayerItem>) (x => this.mGears[x.equip_gear_ids[0].Value].GetPlayerItem(true))).ToArray<PlayerItem>();
      battleEnd.before_player_units = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerDeck[]>()[0].player_units).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null)).Select<PlayerUnit, PlayerUnit>((Func<PlayerUnit, PlayerUnit>) (x => this.mCharacters[x.id].GetPlayerUnit(true))).ToArray<PlayerUnit>();
      Dictionary<int, int> dictionary = new Dictionary<int, int>();
      int num1 = 0;
      foreach (BL.Unit unit1 in be.core.playerUnits.value)
      {
        int num2 = 0;
        if (unit1.duelHistory != null)
        {
          foreach (BL.DuelHistory duelHistory in unit1.duelHistory)
          {
            if (duelHistory.inflictTotalDamage > 0)
            {
              int enemy_id = duelHistory.targetUnitID;
              BL.Unit unit2 = be.core.enemyUnits.value.FirstOrDefault<BL.Unit>((Func<BL.Unit, bool>) (x => x.playerUnit.id == enemy_id));
              if (!(unit2 == (BL.Unit) null))
              {
                BattleStageEnemyReward stageEnemyReward = MasterData.BattleStageEnemyReward[enemy_id];
                Decimal num3 = (Decimal) duelHistory.inflictTotalDamage / (Decimal) unit2.parameter.Hp;
                num2 += Mathf.Max(1, (int) Decimal.Round((Decimal) stageEnemyReward.exp * num3));
              }
            }
          }
        }
        dictionary.Add(unit1.playerUnit.id, num2);
      }
      foreach (WebAPI.Request.BattleFinish.EnemyResult enemy in request.enemies)
      {
        if (enemy.dead_count > 0)
        {
          BattleStageEnemyReward stageEnemyReward = MasterData.BattleStageEnemyReward[enemy.enemy_id];
          num1 += stageEnemyReward.money;
        }
      }
      if (!this.mUserProperties.value.ContainsKey(MasterDataTable.CommonRewardType.money))
        this.mUserProperties.value[MasterDataTable.CommonRewardType.money] = 0L;
      this.mUserProperties.value[MasterDataTable.CommonRewardType.money] += (long) num1;
      this.mUserProperties.commit();
      foreach (WebAPI.Request.BattleFinish.UnitResult unit3 in request.units)
      {
        WebAPI.Request.BattleFinish.UnitResult unit = unit3;
        if (this.mCharacters.ContainsKey(unit.player_unit_id))
        {
          if (unit.remaining_hp == 0)
          {
            if (this.mCharacters[unit.player_unit_id].equipeGearID != 0)
              this.mGears[this.mCharacters[unit.player_unit_id].equipeGearID].SetLost();
            this.mCharacters[unit.player_unit_id].SetDeath();
          }
          else
          {
            Dictionary<int, int> gearProficiencyExperiences = new Dictionary<int, int>();
            PlayerUnit playerUnit = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).FirstOrDefault<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.id == unit.player_unit_id));
            if (playerUnit.equippedGear != (PlayerItem) null)
            {
              gearProficiencyExperiences.Add(playerUnit.equippedGear.gear.kind.ID, unit.total_damage_count);
              this.mGears[playerUnit.equippedGear.id].AddExperience(unit.total_kill_count);
            }
            this.mCharacters[unit.player_unit_id].AddExperience(dictionary.ContainsKey(unit.player_unit_id) ? dictionary[unit.player_unit_id] : 0, gearProficiencyExperiences);
          }
        }
      }
      List<BattleEndPlayer_character_intimates_in_battle> intimatesInBattleList = new List<BattleEndPlayer_character_intimates_in_battle>();
      foreach (WebAPI.Request.BattleFinish.IntimateResult intimate in request.intimates)
      {
        WebAPI.Request.BattleFinish.IntimateResult intimateResult = intimate;
        Tuple<int, int> key = new Tuple<int, int>(intimateResult.character_id < intimateResult.target_character_id ? intimateResult.character_id : intimateResult.target_character_id, intimateResult.character_id > intimateResult.target_character_id ? intimateResult.character_id : intimateResult.target_character_id);
        EarthCharacter earthCharacter1 = this.mCharacters.Values.FirstOrDefault<EarthCharacter>((Func<EarthCharacter, bool>) (x => x.character.ID == intimateResult.character_id));
        EarthCharacter earthCharacter2 = this.mCharacters.Values.FirstOrDefault<EarthCharacter>((Func<EarthCharacter, bool>) (x => x.character.ID == intimateResult.target_character_id));
        if (earthCharacter1 != null && earthCharacter2 != null && this.mCharacterIntimates.ContainsKey(key) && !earthCharacter1.isFall && !earthCharacter2.isFall)
        {
          int experience = this.mCharacterIntimates[key].experience;
          Tuple<int, int> tuple = this.mCharacterIntimates[key].AddExperience(intimateResult.exp);
          intimatesInBattleList.Add(new BattleEndPlayer_character_intimates_in_battle()
          {
            target_character_id = intimateResult.target_character_id,
            character_id = intimateResult.character_id,
            before_total_exp = experience,
            after_total_exp = this.mCharacterIntimates[key].experience,
            after_level = tuple.Item2,
            before_level = tuple.Item1
          });
        }
      }
      battleEnd.player_character_intimates_in_battle = intimatesInBattleList.ToArray();
      foreach (WebAPI.Request.BattleFinish.SupplyResult supply in request.supplies)
      {
        if (this.mItems.ContainsKey(supply.supply_id))
          this.mItems[supply.supply_id].UseItem(supply.use_quantity);
      }
      List<BattleEndDrop_unit_entities> dropUnitEntitiesList = new List<BattleEndDrop_unit_entities>();
      List<BattleEndDrop_gear_entities> dropGearEntitiesList = new List<BattleEndDrop_gear_entities>();
      List<BattleEndDrop_supply_entities> dropSupplyEntitiesList = new List<BattleEndDrop_supply_entities>();
      foreach (GameCore.Reward reward in request.drop_reward.Concat<GameCore.Reward>((IEnumerable<GameCore.Reward>) request.panel_reward))
      {
        Tuple<bool, int> tuple = this.EarthRewardAdd((int) reward.Type, reward.Id > 0 ? new int?(reward.Id) : new int?(), reward.Quantity);
        switch (reward.Type)
        {
          case MasterDataTable.CommonRewardType.unit:
            dropUnitEntitiesList.Add(new BattleEndDrop_unit_entities()
            {
              reward_quantity = reward.Quantity,
              is_new = tuple.Item1,
              reward_id = tuple.Item2 != 0 ? new int?(tuple.Item2) : new int?(),
              reward_type_id = (int) reward.Type
            });
            continue;
          case MasterDataTable.CommonRewardType.supply:
            dropSupplyEntitiesList.Add(new BattleEndDrop_supply_entities()
            {
              reward_quantity = reward.Quantity,
              is_new = tuple.Item1,
              reward_id = tuple.Item2 != 0 ? new int?(tuple.Item2) : new int?(),
              reward_type_id = (int) reward.Type
            });
            continue;
          case MasterDataTable.CommonRewardType.gear:
            dropGearEntitiesList.Add(new BattleEndDrop_gear_entities()
            {
              reward_quantity = reward.Quantity,
              is_new = tuple.Item1,
              reward_id = tuple.Item2 != 0 ? new int?(tuple.Item2) : new int?(),
              reward_type_id = (int) reward.Type
            });
            continue;
          default:
            continue;
        }
      }
      battleEnd.after_player_gears = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerDeck[]>()[0].player_units).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null)).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.equip_gear_ids[0].HasValue)).Select<PlayerUnit, PlayerItem>((Func<PlayerUnit, PlayerItem>) (x => this.mGears[x.equip_gear_ids[0].Value].GetPlayerItem())).ToArray<PlayerItem>();
      battleEnd.after_player_units = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerDeck[]>()[0].player_units).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null)).Select<PlayerUnit, PlayerUnit>((Func<PlayerUnit, PlayerUnit>) (x => this.mCharacters[x.id].GetPlayerUnit())).ToArray<PlayerUnit>();
      battleEnd.drop_unit_entities = dropUnitEntitiesList.ToArray();
      battleEnd.drop_gear_entities = dropGearEntitiesList.ToArray();
      battleEnd.drop_supply_entities = dropSupplyEntitiesList.ToArray();
      foreach (EarthDesertCharacter earthDesertCharacter in ((IEnumerable<EarthDesertCharacter>) MasterData.EarthDesertCharacterList).Where<EarthDesertCharacter>((Func<EarthDesertCharacter, bool>) (x =>
      {
        if (x.desert_logic == EarthJoinLogicType.quest_clear)
        {
          double result = 0.0;
          if (double.TryParse(x.desert_logic_arg, out result))
          {
            int num4 = (int) result;
            return be.core.battleInfo.quest_type != CommonQuestType.Earth ? num4 == be.core.battleInfo.quest_s_id : num4 == this.questProgress.currentEpisode.ID;
          }
        }
        return false;
      })))
      {
        EarthDesertCharacter desertCharactor = earthDesertCharacter;
        if (((IEnumerable<EarthCharacter>) this.characters).Any<EarthCharacter>((Func<EarthCharacter, bool>) (x => x.character.ID == desertCharactor.character_id)))
          ((IEnumerable<EarthCharacter>) this.characters).Where<EarthCharacter>((Func<EarthCharacter, bool>) (x => x.character.ID == desertCharactor.character_id)).FirstOrDefault<EarthCharacter>()?.SetDesert(true);
      }
      foreach (EarthJoinCharacter earthJoinCharacter in ((IEnumerable<EarthJoinCharacter>) MasterData.EarthJoinCharacterList).Where<EarthJoinCharacter>((Func<EarthJoinCharacter, bool>) (x =>
      {
        if (x.join_logic == EarthJoinLogicType.quest_clear)
        {
          double result = 0.0;
          if (double.TryParse(x.join_logic_arg, out result))
          {
            int num5 = (int) result;
            return be.core.battleInfo.quest_type != CommonQuestType.Earth ? num5 == be.core.battleInfo.quest_s_id : num5 == this.questProgress.currentEpisode.ID;
          }
        }
        return false;
      })))
      {
        EarthJoinCharacter joinCharactor = earthJoinCharacter;
        if (joinCharactor.unit.IsNormalUnit && ((IEnumerable<EarthCharacter>) this.characters).Any<EarthCharacter>((Func<EarthCharacter, bool>) (x => x.character.ID == joinCharactor.charctor.ID)))
        {
          EarthCharacter earthCharacter = ((IEnumerable<EarthCharacter>) this.characters).Where<EarthCharacter>((Func<EarthCharacter, bool>) (x => x.character.ID == joinCharactor.charctor.ID)).FirstOrDefault<EarthCharacter>();
          if (earthCharacter != null)
          {
            earthCharacter.AddExperience(joinCharactor.experience, new Dictionary<int, int>());
            earthCharacter.SetDesert(false);
            earthCharacter.SetIndex(((IEnumerable<EarthCharacter>) this.characters).Where<EarthCharacter>((Func<EarthCharacter, bool>) (x => x.unit.IsNormalUnit && !x.isFall && !x.isDesert)).Count<EarthCharacter>());
          }
        }
        else
        {
          EarthCharacter earthCharacter = EarthCharacter.Create(joinCharactor, ((IEnumerable<EarthCharacter>) this.characters).Where<EarthCharacter>((Func<EarthCharacter, bool>) (x => x.unit.IsNormalUnit && !x.isFall && !x.isDesert)).Count<EarthCharacter>());
          this.mCharacters.Add(earthCharacter.ID, earthCharacter);
        }
      }
      foreach (EarthQuestClearReward questClearReward in ((IEnumerable<EarthQuestClearReward>) MasterData.EarthQuestClearRewardList).Where<EarthQuestClearReward>((Func<EarthQuestClearReward, bool>) (x => be.core.battleInfo.quest_type == CommonQuestType.Earth && x.episode != null && x.episode.ID == this.questProgress.currentEpisode.ID)))
        this.EarthRewardAdd((int) questClearReward.reward_type, questClearReward.reward_id, questClearReward.quantity);
      SMManager.UpdateList<PlayerUnit>(this.GetPlayerUnits(), true);
      SMManager.UpdateList<PlayerItem>(this.GetPlayerItems(), true);
      SMManager.UpdateList<PlayerCharacterIntimate>(this.GetPlayerCharacterIntimates(), true);
      if (be.core.battleInfo.quest_type == CommonQuestType.Earth)
      {
        this.questProgress.QuestClear();
        this.ClearCharacterBattleIndex();
      }
      else if (be.core.battleInfo.quest_type == CommonQuestType.EarthExtra)
      {
        EarthExtraQuest extraQuest = ((IEnumerable<EarthExtraQuest>) MasterData.EarthExtraQuestList).FirstOrDefault<EarthExtraQuest>((Func<EarthExtraQuest, bool>) (x => x.ID == be.core.battleInfo.quest_s_id));
        if (extraQuest != null)
        {
          MasterDataTable.EarthQuestKey earthQuestKey = ((IEnumerable<MasterDataTable.EarthQuestKey>) MasterData.EarthQuestKeyList).FirstOrDefault<MasterDataTable.EarthQuestKey>((Func<MasterDataTable.EarthQuestKey, bool>) (x => x.quest_id == extraQuest.ID));
          if (earthQuestKey != null)
          {
            Singleton<EarthDataManager>.GetInstance().AddKeyQuestPlayCount(earthQuestKey.ID);
            if (extraQuest.clear_limit <= Singleton<EarthDataManager>.GetInstance().GetKeyQuestPlayCount(earthQuestKey.ID))
              Singleton<EarthDataManager>.GetInstance().KeyQuestClose(earthQuestKey.ID);
          }
        }
      }
      return Future.Single<BattleEnd>(battleEnd);
    }

    public List<string> createResourceLoadList()
    {
      List<string> resourceLoadList = new List<string>();
      ResourceManager instance = Singleton<ResourceManager>.GetInstance();
      foreach (EarthCharacter earthCharacter in this.mCharacters.Values)
        resourceLoadList.AddRange((IEnumerable<string>) instance.PathsFromUnit(earthCharacter.unit));
      return resourceLoadList;
    }

    public void SuspendEarthMode()
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
      Singleton<NGSceneManager>.GetInstance().clearStack();
      MypageScene.ChangeScene(isEarthSuspend: true);
    }

    public IEnumerator DownloadEarthBGM()
    {
      HashSet<string> paths = new HashSet<string>();
      string[] strArray = new string[4]
      {
        "BgmPJZero001",
        "BgmPJZero003",
        "BgmPJZero004",
        "BgmPJZero_EV001"
      };
      foreach (string bgmPath in strArray)
      {
        foreach (string str in Singleton<ResourceManager>.GetInstance().PathsFromBgm(bgmPath))
          paths.Add(str);
      }
      IEnumerator e = OnDemandDownload.waitLoadSomethingResource((IEnumerable<string>) paths, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }

    public IEnumerator changeEarthScene()
    {
      IEnumerator e = this.DownloadEarthBGM();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = this.EarthDataInit();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().GetEarthHeaderComponent().Reset();
      if (this.isPrologue)
      {
        while (Singleton<PopupManager>.GetInstance().isOpenNoFinish)
          yield return (object) null;
        this.GoPrologueScene();
      }
      else
        Mypage051Scene.ChangeScene(false);
    }

    public static void startEarthScene(MonoBehaviour mb)
    {
      Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
      Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID = -1;
      Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitIndex = -1;
      Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
      Singleton<NGSceneManager>.GetInstance().clearStack();
      SMManager.Swap();
      MasterDataCache.SetGameMode(MasterDataCache.GameMode.EARTH);
      EarthDataManager.CreateInstance();
      mb.StartCoroutine(Singleton<EarthDataManager>.GetInstance().changeEarthScene());
    }

    [Serializable]
    public class EarthData
    {
      public string data;
    }

    [Serializable]
    public class EarthDataNew
    {
      public string player_id;
      public DateTime severTime;
      public string data;
    }
  }
}
