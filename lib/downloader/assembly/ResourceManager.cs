// Decompiled with JetBrains decompiler
// Type: ResourceManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using GameCore.IO;
using gu3.Device;
using gu3.Utils;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UniLinq;
using UnityEngine;

#nullable disable
public class ResourceManager : Singleton<ResourceManager>
{
  private Dictionary<ResourceInfo.PathType, IResourceLoader> loaderDic;
  private string resourceVersion;
  private ResourceInfo resourceInfo;
  private SimpleCache<string, Object> cache;
  private static Regex abUnit = new Regex("^Units/[0-9]+/.*$");
  private static Regex saUnit = new Regex("^AssetBundle/Resources/Units/[0-9]+/.*$");
  private static Regex saUnitExclude = new Regex("^AssetBundle/Resources/Units/(3103811|3201711|3300411|3810001|3820002|3830003)/2D/c_thum$");
  private static Regex abBattleMap = new Regex("^BattleMaps/[0-9]+/.*$");
  private static Regex eventImages = new Regex("^AssetBundle/Resources/EventImages/.*$");
  private static Regex duelAnimators = new Regex("Animators/duel/.*$");
  private static Regex duelWinAnimators = new Regex("Animators/duel_win/.*$");
  private static Regex animations = new Regex("Animations/.*/.*$");
  private static Regex partitioningMasterData = new Regex("MasterData/.*_part_.*$");
  private static Regex partitioningP0MasterData = new Regex("MasterData/p0/.*_part_.*$");
  private static Regex streamingAsset = new Regex("^(android|ios|windows)/[0-9a-zA-Z_]+.*$");
  private static Regex mobVoice = new Regex("^(android|ios|windows)/VO_99[0-9][0-9](_durin)?(_awb|_acb)$");
  private static Regex naviVoice = new Regex("^(android|ios|windows)/(VO_9967_awb|VO_9967_acb)*$");
  private static Regex opMovie = new Regex("^(android|ios|windows)/(opmovie_3)*$");
  private static Regex streamingAssetExclude = new Regex("^(android|ios|windows)/(BgmCueSheet_awb|BgmCueSheet_acb|BgmLostRagnarokBattle001_awb|BgmLostRagnarokBattle001_acb|VO_9000_awb|VO_9000_acb|SECueSheet_awb|SECueSheet_acb|SECueSheet_2_awb|SECueSheet_2_acb|BgmGacha_awb|BgmGacha_acb)*$");
  private static Regex battleCutIN = new Regex(" ^ AssetBundle/Resources/Characters/[0-9]+/.*battle_cutin.*$");
  private static Regex battleCutINExclude = new Regex("^AssetBundle/Resources/Characters/(1038|2017|3004|8053|8054|8055)/.*battle_cutin.*$");
  private static Regex abMobUnits = new Regex("^MobUnits/.*$");
  private static Regex saMobUnits = new Regex("^AssetBundle/Resources/MobUnits/.*$");
  private static Regex assetBundleGears = new Regex("^AssetBundle/Resources/Gears/[0-9]+/.*$");
  private static Regex assetBundleGearsExclude = new Regex("^AssetBundle/Resources/Gears/(10001|10002|20001|20002|30001|30002|40001|50001|60001|12001|22001|32001)/.*$");
  private static Regex assetBundleDifficultImage = new Regex("^AssetBundle/Resources/DifficultImage/.*$");
  private static Regex AnimatorsSpecial = new Regex("^Animators/special/[0-9a-zA-Z_]+.*$");
  private static Regex AnimatorsSpecialExclude = new Regex("^Animators/special/(3103811|3201711|3300411|3810001|3820002|3830003).*$");
  private static Regex BattleEffectsDuel = new Regex("^BattleEffects/duel/[0-9a-zA-Z_]+.*$");
  private static Regex BattleEffectsDuelExclude = new Regex("^BattleEffects/duel/(ef511_damage_number|ef518_ui_critica|ef519_ui_miss|ef510_bufu_number|ef522_common_critical_hit|ef524_ui_weakness|ef525_ui_resist|ef526_sword_holy_shake|ef526_sword_holy_hit|ef512_sword_locus_hit|ef516_cloud_dust_dual|ef513_hatchet_locus_hit|ef324_bow_bullet|ef529_bow_ice_bullet|ef325_axe_bullet|ef529_bow_ice_hit|ef200_fire_shooting_s|ef300_fire_bullet_s|ef400_fire_impact_s|ef509_wand_magic|ef218_lisire_shooting_s|ef318_lisire_bullet_s|ef321_lisire_bullet_2_s|ef418_lisire_impact_s|ef421_heal_target_s|ef031_heal_number_fe|unit_shadow_duel)$");
  private static Regex BattleEffectsDuelMagickBulletsExclude = new Regex("^BattleEffects/duel/MagicBullets/.*$");
  private static Regex BattleSkill = new Regex("^BattleSkills/[0-9a-zA-Z_]+/.*$");
  private static Regex GachaTicket = new Regex("^GachaTicket/[0-9]+/.*$");
  private static Regex Gears = new Regex("Gears/[0-9]+/.*$");
  private static Regex GearsExclude = new Regex("Gears/(10001|10002|11002|20001|20002|30001|30002|40001|50001|60001|12001|22001|32001)/.*$");
  private static Regex Album = new Regex("^Album/.*$");
  private static Regex SelectTicket = new Regex("^SelectTicket/[0-9]+/.*$");
  private static Regex BeginnerNavi = new Regex("^BeginnerNavi/[0-9a-zA-Z_]+/.*$");
  private static Regex Help = new Regex("^Help/[0-9a-zA-Z_]+/.*$");
  private static Regex PrefabsBanners = new Regex("^Prefabs/Banners/.*$");
  private static Regex PrefabsColosseumTitle = new Regex("^Prefabs/colosseum/colosseum_title/.*$");
  private static Regex PrefabsLoading = new Regex("^Prefabs/loading/.*$");
  private static Regex PrefabsQuestExtraDifficultImage = new Regex("^Prefabs/Quest/Extra/DifficultImage/.*$");
  private static Regex PrefabsQuestStoryDifficultImage = new Regex("^Prefabs/Quest/Story/DifficultImage/.*$");
  private static Regex PrefabsBackGroundImage = new Regex("^Prefabs/BackGround/.*$");
  private AssocList<int, string[]> unitToPaths;
  private AssocList<int, string[]> mobUnitToPaths;
  private AssocList<int, string[]> mapToPaths;
  private AssocList<int, string[]> movieToPaths;
  private AssocList<int, string[]> bgmToPaths;
  public static HashSet<string> alreadyDirPath = new HashSet<string>();

  public static string dlcVersionPath => StorageUtil.persistentDataPath + "/dlcversion.dat";

  public static string pathsJsonPath => StorageUtil.persistentDataPath + "/paths.json";

  protected override void Initialize()
  {
    this.loaderDic = new Dictionary<ResourceInfo.PathType, IResourceLoader>()
    {
      {
        ResourceInfo.PathType.Resource,
        (IResourceLoader) new ResourceLoader()
      },
      {
        ResourceInfo.PathType.StreamingAssets,
        (IResourceLoader) new StreamingAssetsLoader()
      },
      {
        ResourceInfo.PathType.AssetBundle,
        (IResourceLoader) new AssetBundleLoader((MonoBehaviour) this)
      }
    };
    Caching.maximumAvailableDiskSpace = 157286400L;
  }

  public IEnumerator CleanupCache()
  {
    DateTime now = DateTime.Now;
    string[] downloadedFilenames = this.GetHashedFilenameListFromFileSystem(DLC.ResourceDirectory);
    yield return (object) null;
    string[] downloadingFilenames = this.GetHashedFilenameListFromDatabase();
    yield return (object) null;
    string[] filenameListToBeDeleted = ((IEnumerable<string>) downloadedFilenames).Except<string>((IEnumerable<string>) downloadingFilenames).ToArray<string>();
    yield return (object) null;
    string[] strArray = filenameListToBeDeleted;
    for (int index = 0; index < strArray.Length; ++index)
    {
      string str = strArray[index];
      if (!str.Equals("paths.json"))
      {
        File.Delete(DLC.ResourceDirectory + str);
        yield return (object) null;
      }
    }
    strArray = (string[]) null;
    GC.Collect();
    GC.WaitForPendingFinalizers();
  }

  private string[] GetHashedFilenameListFromFileSystem(string path)
  {
    string[] files = Directory.GetFiles(path);
    for (int index = 0; index < files.Length; ++index)
      files[index] = Path.GetFileName(files[index]);
    return files;
  }

  private string[] GetHashedFilenameListFromDatabase()
  {
    List<string> stringList = new List<string>();
    foreach (ResourceInfo.Resource resource in this.resourceInfo)
    {
      switch (resource._value._path_type)
      {
        case ResourceInfo.PathType.AssetBundle:
        case ResourceInfo.PathType.StreamingAssets:
          if (!string.IsNullOrEmpty(resource._value._file_name))
          {
            stringList.Add(resource._value._file_name);
            continue;
          }
          continue;
        default:
          continue;
      }
    }
    return stringList.ToArray();
  }

  public string ResolveAssetPath(string hashedFilename)
  {
    foreach (ResourceInfo.Resource resource in this.resourceInfo)
    {
      if (!string.IsNullOrEmpty(resource._value._file_name) && resource._value._file_name.Equals(hashedFilename))
        return resource._key;
    }
    return "";
  }

  public void InitResourceInfoSync()
  {
    string pathsJsonPath = ResourceManager.pathsJsonPath;
    string dlcVersionPath = ResourceManager.dlcVersionPath;
    try
    {
      this.resourceInfo = ResourceInfo.Deserialize(pathsJsonPath);
      this.resourceVersion = File.ReadAllText(dlcVersionPath, Encoding.UTF8);
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ex.ToString());
      this.resourceInfo = (ResourceInfo) null;
      this.resourceVersion = (string) null;
    }
  }

  private bool ContainsInAssetDatabase(string hashedFilename)
  {
    foreach (ResourceInfo.Resource resource in this.resourceInfo)
    {
      if (!string.IsNullOrEmpty(resource._value._file_name) && resource._value._file_name.Equals(hashedFilename))
        return true;
    }
    return false;
  }

  public IEnumerator DownloadIfNotExists(string path)
  {
    DLC dlc = new DLC(this.resourceInfo, new string[1]
    {
      path
    });
    if (dlc.DownloadRequired)
    {
label_2:
      IEnumerator e = dlc.Start((MonoBehaviour) Singleton<ResourceManager>.GetInstance());
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (dlc.Error != null)
      {
        Debug.LogError((object) dlc.Error);
        bool waitRetry = true;
        ModalWindow.ShowRetryTitle(Consts.GetInstance().dlc_fail_download_title, dlc.Error, (Action) (() => waitRetry = false), (Action) (() => StartScript.Restart()));
        while (waitRetry)
          yield return (object) new WaitForEndOfFrame();
        goto label_2;
      }
    }
  }

  public bool Initialized => this.resourceInfo != null;

  public IEnumerator InitResourceInfo()
  {
    string pathsJsonPath = ResourceManager.pathsJsonPath;
    string dlcVersionPath = ResourceManager.dlcVersionPath;
    ResourceManager.CreateSaveDLCDir(DLC.ResourceDirectory);
    try
    {
      this.resourceInfo = ResourceInfo.Deserialize(pathsJsonPath);
      this.resourceVersion = File.ReadAllText(dlcVersionPath, Encoding.UTF8);
    }
    catch (Exception ex)
    {
      this.resourceVersion = (string) null;
      yield break;
    }
  }

  public static string DLCVersion
  {
    get
    {
      ResourceManager instance = Singleton<ResourceManager>.GetInstance();
      return !Object.op_Equality((Object) instance, (Object) null) ? instance.resourceVersion : "";
    }
    set => Singleton<ResourceManager>.GetInstance().resourceVersion = value;
  }

  public IEnumerator SaveResourceInfo(byte[] buf)
  {
    ResourceManager.CreateSaveDLCDir(DLC.ResourceDirectory);
    string pathsJsonPath = ResourceManager.pathsJsonPath;
    byte[] buffer = new byte[10240];
    try
    {
      using (FileStream fileStream = File.Create(pathsJsonPath))
      {
        using (ZlibUtilStream zlibUtilStream = ZlibUtilStream.DecompressBytes(buf, 0, buf.Length, (ZlibFormat) 2))
        {
          while (true)
          {
            int count = zlibUtilStream.Read(buffer, 0, buffer.Length);
            if (count != 0)
              fileStream.Write(buffer, 0, count);
            else
              break;
          }
          yield break;
        }
      }
    }
    catch (Exception ex1)
    {
      try
      {
        File.Delete(pathsJsonPath);
      }
      catch (Exception ex2)
      {
      }
      throw;
    }
  }

  public ResourceInfo ResourceInfo => this.resourceInfo;

  private SimpleCache<string, Object> Cache
  {
    get
    {
      if (this.cache == null)
        this.cache = new SimpleCache<string, Object>((Func<string, Promise<Object>, IEnumerator>) ((key, promise) => this.InternalLoad(key, promise)), (Func<Object, long>) (_ => 1L), 10L);
      return this.cache;
    }
  }

  private IEnumerator SetStep(Object res, ResourceInfo.Step step)
  {
    GameObject go = res as GameObject;
    if (Object.op_Inequality((Object) go, (Object) null))
    {
      ResourceInfo.StepsType type = step._type;
      IEnumerator e;
      if (type == ResourceInfo.StepsType.UISprite)
      {
        string path = step._path;
        string transform = step._transform;
        UISprite sprite = ((Component) go.transform.FindByFullPath(path)).GetComponent<UISprite>();
        Future<GameObject> future = Singleton<ResourceManager>.GetInstance().Load<GameObject>(transform);
        e = future.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        sprite.atlas = future.Result.GetComponent<UIAtlas>();
        sprite = (UISprite) null;
        future = (Future<GameObject>) null;
      }
      string transformPath;
      if (type == ResourceInfo.StepsType.UIAtlas)
      {
        transformPath = step._path;
        string transform = step._transform;
        Future<Texture2D> future = Singleton<ResourceManager>.GetInstance().Load<Texture2D>(transform);
        e = future.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        ((Component) go.transform.FindByFullPath(transformPath)).GetComponent<UIAtlas>().spriteMaterial.SetTexture("_MainTex", (Texture) future.Result);
        transformPath = (string) null;
        future = (Future<Texture2D>) null;
      }
      if (type == ResourceInfo.StepsType.UILable)
      {
        transformPath = step._path;
        string transform = step._transform;
        Future<Font> future = Singleton<ResourceManager>.GetInstance().Load<Font>(transform);
        e = future.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        ((Component) go.transform.FindByFullPath(transformPath)).GetComponent<UILabel>().trueTypeFont = future.Result;
        transformPath = (string) null;
        future = (Future<Font>) null;
      }
      if (type == ResourceInfo.StepsType.Renderer)
      {
        Renderer renderer = ((Component) go.transform.FindByFullPath(step._path)).GetComponent<Renderer>();
        if (step._sharedMaterials != null)
        {
          int num = step._sharedMaterials.Length;
          for (int i = 0; i < num; ++i)
          {
            foreach (string key in step._sharedMaterials[i].Keys)
            {
              transformPath = key;
              Future<Object> future = Singleton<ResourceManager>.GetInstance().Load<Object>(step.GetSharedMaterials(i, transformPath));
              e = future.Wait();
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              Texture2D result1 = future.Result as Texture2D;
              if (Object.op_Inequality((Object) result1, (Object) null))
              {
                ((Texture) result1).filterMode = step.GetSharedMaterialFilterMode(i, transformPath);
                ((Texture) result1).wrapMode = step.GetSharedMaterialTextureWrapMode(i, transformPath);
                renderer.sharedMaterials[i].SetTexture(transformPath, (Texture) result1);
              }
              else
              {
                Font result2 = future.Result as Font;
                if (Object.op_Inequality((Object) result2, (Object) null))
                {
                  ((Component) renderer).gameObject.GetComponent<TextMesh>().font = result2;
                  renderer.sharedMaterial = result2.material;
                  renderer.sharedMaterials[i] = result2.material;
                }
                else
                {
                  future = (Future<Object>) null;
                  transformPath = (string) null;
                }
              }
            }
          }
        }
        renderer = (Renderer) null;
      }
    }
  }

  public IEnumerator SetSteps(string path, Object obj)
  {
    ResourceInfo.Resource resource = this.resourceInfo.SearchResourceInfo(path);
    if (!string.IsNullOrEmpty(resource._key) && resource._value._steps != null)
    {
      ResourceInfo.Step[] stepArray = resource._value._steps;
      for (int index = 0; index < stepArray.Length; ++index)
      {
        ResourceInfo.Step step = stepArray[index];
        IEnumerator e = this.SetStep(obj, step);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      stepArray = (ResourceInfo.Step[]) null;
    }
  }

  private IEnumerator InternalLoad(string path, Promise<Object> promise)
  {
    ResourceInfo.Resource info = this.resourceInfo.SearchResourceInfo(path);
    if (string.IsNullOrEmpty(info._key))
    {
      Debug.LogError((object) string.Format("File not found: {0} in Local a", (object) path));
      promise.Result = (Object) null;
    }
    else
    {
      IEnumerator e;
      if (!info._value.Exists)
      {
        e = this.DownloadIfNotExists(path);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      Future<Object> future = this.loaderDic[info._value._path_type].Load(path, ref info);
      e = future.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (!future.HasResult || Object.op_Equality(future.Result, (Object) null))
      {
        Debug.LogError((object) string.Format("File not found: {0} in Local b", (object) path));
      }
      else
      {
        e = this.SetSteps(path, future.Result);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        promise.Result = future.Result;
      }
    }
  }

  public bool Contains(string path)
  {
    return !string.IsNullOrEmpty(this.resourceInfo.SearchResourceInfo(path)._key);
  }

  public Future<T> LoadOrNull<T>(string path) where T : Object
  {
    return !Singleton<ResourceManager>.GetInstance().Contains(path) ? Future.Single<T>(default (T)) : Singleton<ResourceManager>.GetInstance().Load<T>(path);
  }

  public Future<T> Load<T>(string path, float pixelsPerUnitForSprite = 1f) where T : Object
  {
    return typeof (T) != typeof (Sprite) ? this.Cache.Get(path).Then<T>((Func<Object, T>) (obj => (T) obj)) : this.Cache.Get(path).Then<T>((Func<Object, T>) (obj =>
    {
      SpriteMeshType spriteMeshType = (SpriteMeshType) 0;
      uint num = 100;
      if (Object.op_Equality(obj, (Object) null))
        obj = (Object) Resources.Load<Texture2D>("Sprites/1x1_alpha0");
      Texture2D texture2D = (Texture2D) obj;
      Sprite sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float) ((Texture) texture2D).width, (float) ((Texture) texture2D).height), new Vector2(0.5f, 0.5f), pixelsPerUnitForSprite, num, spriteMeshType);
      ((Object) sprite).name = ((Object) texture2D).name;
      return sprite as T;
    }));
  }

  public Future<T> Load<T>(string path, Action<T> callback) where T : Object
  {
    Future<T> future = this.Load<T>(path);
    future.RunOn<T>((MonoBehaviour) this, callback);
    return future;
  }

  public void ClearCache() => this.Cache.Clear();

  public void ClearPathCache()
  {
    this.unitToPaths = (AssocList<int, string[]>) null;
    this.mapToPaths = (AssocList<int, string[]>) null;
    this.movieToPaths = (AssocList<int, string[]>) null;
    this.mobUnitToPaths = (AssocList<int, string[]>) null;
    this.bgmToPaths = (AssocList<int, string[]>) null;
  }

  public T LoadImmediatelyForSmallObject<T>(string path) where T : Object
  {
    if (this.resourceInfo == null)
      return default (T);
    Object @object = this.Cache.TryGet(path);
    if (Object.op_Inequality(@object, (Object) null))
      return (T) @object;
    ResourceInfo.Resource context = this.resourceInfo.SearchResourceInfo(path);
    return string.IsNullOrEmpty(context._key) ? default (T) : (T) this.loaderDic[context._value._path_type].LoadImmediatelyForSmallObject(path, ref context);
  }

  public Future<T> LoadFromDownloadOrCache<T>(string path) where T : Object
  {
    if (this.resourceInfo == null)
      return (Future<T>) null;
    ResourceInfo.Resource context = this.resourceInfo.SearchResourceInfo(path);
    return string.IsNullOrEmpty(context._key) ? (Future<T>) null : this.loaderDic[context._value._path_type].DownloadOrCache(path, ref context).Then<T>((Func<Object, T>) (obj => (T) obj));
  }

  public IEnumerator LoadResource(GameObject gameObject)
  {
    UISprite[] componentsInChildren = gameObject.GetComponentsInChildren<UISprite>(true);
    AssocList<string, UIAtlas> loadUIAtlass = new AssocList<string, UIAtlas>();
    UISprite[] uiSpriteArray = componentsInChildren;
    for (int index = 0; index < uiSpriteArray.Length; ++index)
    {
      UISprite sprite = uiSpriteArray[index];
      int count = sprite.spriteName.IndexOf("__");
      if (count > 0)
      {
        string atlasname = sprite.spriteName.Remove(0, count);
        if (!loadUIAtlass.ContainsKey(atlasname))
        {
          string path = string.Join("/", ((IEnumerable<string>) atlasname.Split(new string[1]
          {
            "__"
          }, StringSplitOptions.None)).Skip<string>(1).ToArray<string>());
          Future<GameObject> future = Singleton<ResourceManager>.GetInstance().Load<GameObject>(path);
          IEnumerator e = future.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          loadUIAtlass.Add(atlasname, future.Result.GetComponent<UIAtlas>());
          future = (Future<GameObject>) null;
        }
        sprite.atlas = loadUIAtlass[atlasname];
        atlasname = (string) null;
      }
      sprite = (UISprite) null;
    }
    uiSpriteArray = (UISprite[]) null;
  }

  public string ResolveStreamingAssetsPath(string path)
  {
    ResourceInfo.Resource resource = this.resourceInfo != null ? this.resourceInfo.SearchResourceInfo(path) : throw new InvalidOperationException("resourceInfo is null");
    if (string.IsNullOrEmpty(resource._key))
      throw new ArgumentException(string.Format("resourceInfo.ContainsKey({0}) is false", (object) path));
    return Persist.fileMoved.Data.isAllMoved ? DLC.GetPath(resource._value._file_name) : DLC.ResourceDirectory + resource._value._file_name;
  }

  public string ResolveStreamingAssetsPathForMovie(string path)
  {
    string sourceFileName = this.ResolveStreamingAssetsPath(path);
    ResourceInfo.Resource resource = this.resourceInfo.SearchResourceInfo(path);
    if (string.IsNullOrEmpty(resource._key))
    {
      Debug.LogError((object) string.Format("resourceInfo.ContainsKey({0}) is false", (object) path));
      return "";
    }
    string str = sourceFileName + resource._value._ext;
    if (File.Exists(str))
      return str;
    try
    {
      File.Copy(sourceFileName, str);
    }
    catch (Exception ex)
    {
      return sourceFileName;
    }
    return str;
  }

  public static bool IsInitialDLCTarget(
    string path,
    bool isMobVoiceDowonload,
    bool isOpMovieDownload)
  {
    bool flag = !ResourceManager.abUnit.IsMatch(path) && (!ResourceManager.streamingAsset.IsMatch(path) || ResourceManager.streamingAssetExclude.IsMatch(path) || ResourceManager.naviVoice.IsMatch(path)) && (!ResourceManager.saUnit.IsMatch(path) || ResourceManager.saUnitExclude.IsMatch(path)) && !ResourceManager.duelAnimators.IsMatch(path) && !ResourceManager.duelWinAnimators.IsMatch(path) && !ResourceManager.abBattleMap.IsMatch(path) && !ResourceManager.eventImages.IsMatch(path) && !ResourceManager.partitioningMasterData.IsMatch(path) && !ResourceManager.partitioningP0MasterData.IsMatch(path) && (!ResourceManager.battleCutIN.IsMatch(path) || ResourceManager.battleCutINExclude.IsMatch(path)) && !ResourceManager.abMobUnits.IsMatch(path) && !ResourceManager.saMobUnits.IsMatch(path) && (!ResourceManager.assetBundleGears.IsMatch(path) || ResourceManager.assetBundleGearsExclude.IsMatch(path)) && (!ResourceManager.AnimatorsSpecial.IsMatch(path) || ResourceManager.AnimatorsSpecialExclude.IsMatch(path)) && (!ResourceManager.BattleEffectsDuel.IsMatch(path) || ResourceManager.BattleEffectsDuelExclude.IsMatch(path) || ResourceManager.BattleEffectsDuelMagickBulletsExclude.IsMatch(path)) && !ResourceManager.BattleSkill.IsMatch(path) && !ResourceManager.GachaTicket.IsMatch(path) && (!ResourceManager.Gears.IsMatch(path) || ResourceManager.GearsExclude.IsMatch(path)) && !ResourceManager.PrefabsBanners.IsMatch(path) && !ResourceManager.PrefabsColosseumTitle.IsMatch(path) && !ResourceManager.PrefabsLoading.IsMatch(path) && !ResourceManager.PrefabsQuestExtraDifficultImage.IsMatch(path) && !ResourceManager.PrefabsQuestStoryDifficultImage.IsMatch(path) && !ResourceManager.PrefabsBackGroundImage.IsMatch(path) && !ResourceManager.Album.IsMatch(path) && !ResourceManager.SelectTicket.IsMatch(path) && !ResourceManager.BeginnerNavi.IsMatch(path) && !ResourceManager.assetBundleDifficultImage.IsMatch(path);
    if (!flag & isMobVoiceDowonload)
      flag |= ResourceManager.IsMobVoice(path);
    if (!flag & isOpMovieDownload)
      flag |= ResourceManager.opMovie.IsMatch(path);
    return flag;
  }

  public static bool IsMobVoice(string path)
  {
    return !ResourceManager.naviVoice.IsMatch(path) && ResourceManager.mobVoice.IsMatch(path);
  }

  private void InitPaths()
  {
    if (this.resourceInfo == null)
      return;
    Regex regex1 = new Regex("^Units/(?<id>[0-9]+)/.*$");
    Regex regex2 = new Regex("^AssetBundle/Resources/Units/(?<id>[0-9]+)/.*$");
    Regex regex3 = new Regex("^windows/VO_(?<id>[0-9]+)_awb$");
    Regex regex4 = new Regex("^windows/VO_(?<id>[0-9]+)_acb$");
    AssocList<int, List<string>> assocList1 = new AssocList<int, List<string>>();
    AssocList<int, List<string>> assocList2 = new AssocList<int, List<string>>();
    Action<AssocList<int, List<string>>, int, string> action = (Action<AssocList<int, List<string>>, int, string>) ((dic, n, str) =>
    {
      if (!dic.ContainsKey(n))
        dic.Add(n, new List<string>());
      dic[n].Add(string.Intern(str));
    });
    Regex regex5 = new Regex("^MobUnits/(?<id>[0-9]+)/.*$");
    Regex regex6 = new Regex("^AssetBundle/Resources/MobUnits/(?<id>[0-9]+)/.*$");
    AssocList<int, List<string>> assocList3 = new AssocList<int, List<string>>();
    Regex regex7 = new Regex("^BattleMaps/(?<id>[0-9]+)/.*$");
    AssocList<int, List<string>> assocList4 = new AssocList<int, List<string>>();
    AssocList<int, List<string>> assocList5 = new AssocList<int, List<string>>();
    AssocList<int, List<string>> assocList6 = new AssocList<int, List<string>>();
    Regex regex8 = new Regex("^windows/(?<id>Bgm[0-9a-zA-Z_]+)_awb$");
    Regex regex9 = new Regex("^windows/(?<id>Bgm[0-9a-zA-Z_]+)_acb$");
    foreach (ResourceInfo.Resource resource in this.resourceInfo)
    {
      Match match1 = regex1.Match(resource._key);
      if (match1.Success)
      {
        int num = int.Parse(match1.Groups["id"].Value);
        action(assocList1, num, resource._key);
      }
      else
      {
        Match match2 = regex2.Match(resource._key);
        if (match2.Success)
        {
          int num = int.Parse(match2.Groups["id"].Value);
          action(assocList1, num, resource._key);
        }
        else
        {
          Match match3 = regex3.Match(resource._key);
          if (match3.Success)
          {
            int num = int.Parse(match3.Groups["id"].Value);
            action(assocList2, num, resource._key);
          }
          else
          {
            Match match4 = regex4.Match(resource._key);
            if (match4.Success)
            {
              int num = int.Parse(match4.Groups["id"].Value);
              action(assocList2, num, resource._key);
            }
            else
            {
              Match match5 = regex5.Match(resource._key);
              if (match5.Success)
              {
                int num = int.Parse(match5.Groups["id"].Value);
                action(assocList3, num, resource._key);
              }
              else
              {
                Match match6 = regex6.Match(resource._key);
                if (match6.Success)
                {
                  int num = int.Parse(match6.Groups["id"].Value);
                  action(assocList3, num, resource._key);
                }
                else
                {
                  Match match7 = regex7.Match(resource._key);
                  if (match7.Success)
                  {
                    int num = int.Parse(match7.Groups["id"].Value);
                    action(assocList4, num, resource._key);
                  }
                  else if (resource.IsMovie())
                  {
                    int hashCode = resource._key.GetHashCode();
                    action(assocList5, hashCode, resource._key);
                  }
                  else
                  {
                    Match match8 = regex8.Match(resource._key);
                    if (match8.Success)
                    {
                      int hashCode = match8.Groups["id"].Value.GetHashCode();
                      action(assocList6, hashCode, resource._key);
                    }
                    else
                    {
                      Match match9 = regex9.Match(resource._key);
                      if (match9.Success)
                      {
                        int hashCode = match9.Groups["id"].Value.GetHashCode();
                        action(assocList6, hashCode, resource._key);
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
    }
    this.unitToPaths = new AssocList<int, string[]>();
    Dictionary<int, UnitUnitGearModelKind> dictionary1 = new Dictionary<int, UnitUnitGearModelKind>();
    Dictionary<int, List<UnitUnitGearModelKind>> dictionary2 = new Dictionary<int, List<UnitUnitGearModelKind>>();
    foreach (UnitUnitGearModelKind unitGearModelKind in MasterData.UnitUnitGearModelKindList)
    {
      if (!unitGearModelKind.job_metamor_id.HasValue)
        dictionary1[unitGearModelKind.unit_UnitUnit] = unitGearModelKind;
      else if (dictionary2.ContainsKey(unitGearModelKind.unit_UnitUnit))
      {
        dictionary2[unitGearModelKind.unit_UnitUnit].Add(unitGearModelKind);
      }
      else
      {
        List<UnitUnitGearModelKind> unitGearModelKindList = new List<UnitUnitGearModelKind>()
        {
          unitGearModelKind
        };
        dictionary2.Add(unitGearModelKind.unit_UnitUnit, unitGearModelKindList);
      }
    }
    foreach (UnitUnit unitUnit in MasterData.UnitUnitList)
    {
      List<string> list = (assocList1.ContainsKey(unitUnit.resource_reference_unit_id.ID) ? (IEnumerable<string>) assocList1[unitUnit.resource_reference_unit_id.ID] : (IEnumerable<string>) new List<string>()).Concat<string>(assocList2.ContainsKey(unitUnit.character.ID) ? (IEnumerable<string>) assocList2[unitUnit.character.ID] : (IEnumerable<string>) new List<string>()).Concat<string>(assocList2.ContainsKey(unitUnit.same_character_id) ? (IEnumerable<string>) assocList2[unitUnit.same_character_id] : (IEnumerable<string>) new List<string>()).ToList<string>();
      if (dictionary1.ContainsKey(unitUnit.ID))
        list.AddRange((IEnumerable<string>) this.getAnimatorPaths(dictionary1[unitUnit.ID]));
      if (dictionary2.ContainsKey(unitUnit.ID))
        list.AddRange((IEnumerable<string>) this.getJobAnimatorPaths(dictionary2[unitUnit.ID]));
      if (unitUnit.unitVoicePattern != null)
      {
        int result = 0;
        if (int.TryParse(unitUnit.unitVoicePattern.file_name.Replace("VO_", ""), out result) && assocList2.ContainsKey(result))
          list.AddRange((IEnumerable<string>) assocList2[result]);
      }
      this.unitToPaths[unitUnit.ID] = list.ToArray();
    }
    this.mobUnitToPaths = new AssocList<int, string[]>();
    foreach (KeyValuePair<int, List<string>> keyValuePair in assocList3)
      this.mobUnitToPaths[keyValuePair.Key] = keyValuePair.Value.ToArray();
    this.mapToPaths = new AssocList<int, string[]>();
    foreach (KeyValuePair<int, List<string>> keyValuePair in assocList4)
      this.mapToPaths[keyValuePair.Key] = keyValuePair.Value.ToArray();
    this.movieToPaths = new AssocList<int, string[]>();
    foreach (KeyValuePair<int, List<string>> keyValuePair in assocList5)
      this.movieToPaths[keyValuePair.Key] = keyValuePair.Value.ToArray();
    this.bgmToPaths = new AssocList<int, string[]>();
    foreach (KeyValuePair<int, List<string>> keyValuePair in assocList6)
      this.bgmToPaths[keyValuePair.Key] = keyValuePair.Value.ToArray();
  }

  private List<string> getAnimatorPaths(UnitUnitGearModelKind gearModelKind)
  {
    List<string> animatorPaths = new List<string>();
    if (!string.IsNullOrEmpty(gearModelKind.duel_animator_controller_name))
    {
      string str = string.Intern(string.Format(UnitUnit.duelAnimatorRootPath, (object) gearModelKind.duel_animator_controller_name));
      animatorPaths.Add(str);
    }
    if (!string.IsNullOrEmpty(gearModelKind.winning_animator_controller_name))
    {
      string str = string.Intern(string.Format(UnitUnit.winAnimatorRootPath, (object) gearModelKind.winning_animator_controller_name));
      animatorPaths.Add(str);
    }
    return animatorPaths;
  }

  private List<string> getJobAnimatorPaths(
    List<UnitUnitGearModelKind> jobGearModelKindsByUnitID)
  {
    List<string> jobAnimatorPaths = new List<string>();
    foreach (UnitUnitGearModelKind unitGearModelKind in jobGearModelKindsByUnitID)
    {
      if (!string.IsNullOrEmpty(unitGearModelKind.duel_animator_controller_name))
      {
        string str = string.Intern(string.Format(UnitUnit.duelAnimatorRootPath, (object) unitGearModelKind.duel_animator_controller_name));
        jobAnimatorPaths.Add(str);
      }
    }
    foreach (UnitUnitGearModelKind unitGearModelKind in jobGearModelKindsByUnitID)
    {
      if (!string.IsNullOrEmpty(unitGearModelKind.winning_animator_controller_name))
      {
        string str = string.Intern(string.Format(UnitUnit.winAnimatorRootPath, (object) unitGearModelKind.winning_animator_controller_name));
        jobAnimatorPaths.Add(str);
      }
    }
    return jobAnimatorPaths;
  }

  public string[] PathsFromUnit(UnitUnit unit)
  {
    if (this.unitToPaths != null && this.unitToPaths.ContainsKey(unit.ID))
      return this.unitToPaths[unit.ID];
    this.InitPaths();
    return this.unitToPaths.ContainsKey(unit.ID) ? this.unitToPaths[unit.ID] : new string[0];
  }

  public string[] PathsFromMobUnit(int idx)
  {
    if (this.mobUnitToPaths == null)
      return new string[0];
    if (this.mobUnitToPaths != null && this.mobUnitToPaths.ContainsKey(idx))
      return this.mobUnitToPaths[idx];
    this.InitPaths();
    return this.mobUnitToPaths.ContainsKey(idx) ? this.mobUnitToPaths[idx] : new string[0];
  }

  public string[] PathsFromBattleMap(BattleMap map)
  {
    if (this.mapToPaths != null && this.mapToPaths.ContainsKey(map.ID))
      return this.mapToPaths[map.ID];
    this.InitPaths();
    return this.mapToPaths.ContainsKey(map.ID) ? this.mapToPaths[map.ID] : new string[0];
  }

  public string[] PathsFromMovie(string moviePath)
  {
    if (this.movieToPaths == null)
      return new string[0];
    int hashCode = moviePath.GetHashCode();
    if (this.movieToPaths != null && this.movieToPaths.ContainsKey(hashCode))
      return this.movieToPaths[hashCode];
    this.InitPaths();
    return this.movieToPaths.ContainsKey(hashCode) ? this.movieToPaths[hashCode] : new string[0];
  }

  public string[] PathsFromBgm(string bgmPath)
  {
    if (this.bgmToPaths == null)
      return new string[0];
    int hashCode = bgmPath.GetHashCode();
    if (this.bgmToPaths != null && this.bgmToPaths.ContainsKey(hashCode))
      return this.bgmToPaths[hashCode];
    this.InitPaths();
    return this.bgmToPaths.ContainsKey(hashCode) ? this.bgmToPaths[hashCode] : new string[0];
  }

  public DLC CreateDLC(string[] paths, bool fileCheckDisable = false, bool isStartupSequence = false)
  {
    if (!isStartupSequence)
      return new DLC(this.resourceInfo, paths, fileCheckDisable);
    List<ResourceInfo.Resource> resources = new List<ResourceInfo.Resource>();
    HashSet<string> stringSet1 = new HashSet<string>();
    HashSet<string> stringSet2 = !Persist.fileMoved.Data.isAllMoved ? new HashSet<string>((IEnumerable<string>) FileManager.GetEntries(DLC.ResourceDirectory)) : DLC.GetEntries();
    List<string> stringList = new List<string>();
    foreach (string path in paths)
    {
      ResourceInfo.Resource resource = this.resourceInfo.SearchResourceInfo(path);
      if (!stringSet2.Contains(resource._value._file_name))
        resources.Add(resource);
    }
    return new DLC(resources);
  }

  public static void CreateSaveDLCDir(string dirPath)
  {
    if (ResourceManager.alreadyDirPath.Contains(dirPath))
      return;
    try
    {
      if (!Directory.Exists(dirPath))
        Directory.CreateDirectory(dirPath);
      ResourceManager.alreadyDirPath.Add(dirPath);
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ex.ToString());
      if (!Directory.Exists(dirPath))
        return;
      Directory.Delete(dirPath);
    }
  }
}
