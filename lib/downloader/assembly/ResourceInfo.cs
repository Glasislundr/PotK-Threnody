// Decompiled with JetBrains decompiler
// Type: ResourceInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using rapidjson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UniLinq;
using UnityEngine;

#nullable disable
public class ResourceInfo : IEnumerable<ResourceInfo.Resource>, IEnumerable
{
  private ResourceInfo.Resource[] resourceInfo;
  private Dictionary<Tuple<int, int>, ResourceInfo.Resource> resourceDict;

  public ResourceInfo.Resource this[int n] => this.resourceInfo[n];

  public IEnumerator<ResourceInfo.Resource> GetEnumerator()
  {
    ResourceInfo.Resource[] resourceArray = this.resourceInfo;
    for (int index = 0; index < resourceArray.Length; ++index)
      yield return resourceArray[index];
    resourceArray = (ResourceInfo.Resource[]) null;
  }

  IEnumerator IEnumerable.GetEnumerator() => this.resourceInfo.GetEnumerator();

  public static ResourceInfo.Resource SearchResourceInfo(string path, ResourceInfo resourceInfo)
  {
    ResourceInfo.Resource resource = new ResourceInfo.Resource();
    if (resourceInfo == null)
      return resource;
    resourceInfo.resourceDict.TryGetValue(new Tuple<int, int>(path.GetHashCode(), path.Length), out resource);
    return resource;
  }

  public static ResourceInfo Deserialize(string path)
  {
    GC.Collect();
    GC.WaitForPendingFinalizers();
    List<ResourceInfo.Resource> resourceList = new List<ResourceInfo.Resource>();
    using (Document document = Document.Parse(File.ReadAllBytes(path)))
    {
      rapidjson.Value root = document.Root;
      foreach (KeyValuePair<string, rapidjson.Value> keyValuePair in root.GetObject())
      {
        ResourceInfo.PathType path_type = (ResourceInfo.PathType) Enum.Parse(typeof (ResourceInfo.PathType), keyValuePair.Key);
        root = keyValuePair.Value;
        foreach (KeyValuePair<string, rapidjson.Value> info in root.GetObject())
        {
          ResourceInfo.Resource resource = new ResourceInfo.Resource();
          resource.Set(path_type, info);
          resourceList.Add(resource);
        }
      }
    }
    ResourceInfo resourceInfo = new ResourceInfo()
    {
      resourceInfo = resourceList.ToArray()
    };
    resourceInfo.resourceDict = ((IEnumerable<ResourceInfo.Resource>) resourceInfo.resourceInfo).ToDictionary<ResourceInfo.Resource, Tuple<int, int>>((Func<ResourceInfo.Resource, Tuple<int, int>>) (x => new Tuple<int, int>(x._key.GetHashCode(), x._key.Length)));
    return resourceInfo;
  }

  public ResourceInfo.Resource SearchResourceInfo(string path)
  {
    return ResourceInfo.SearchResourceInfo(path, this);
  }

  public enum AssetBundleField
  {
    FileName,
    ObjectType,
    FileSize,
    Steps,
  }

  public enum StreamingAssetsField
  {
    FileName,
    Extension,
    ObjectType,
    FileSize,
    Steps,
  }

  public enum ResourceField
  {
    ObjectType,
    Steps,
  }

  public enum PathType
  {
    None,
    AssetBundle,
    StreamingAssets,
    Resource,
  }

  public enum ObjectType
  {
    None,
    AnimationClip,
    AnimatorController,
    FontPhysicMaterial,
    GameObject,
    Material,
    MovieTexture,
    Object,
    Shader,
    TextAsset,
    Texture2D,
  }

  public enum StepsType
  {
    Renderer,
    UIAtlas,
    UILable,
    UISprite,
  }

  public struct Value
  {
    public string _ext;
    public string _file_name;
    public ResourceInfo.PathType _path_type;
    public ResourceInfo.ObjectType _object_type;
    public uint _file_size;
    public ResourceInfo.Step[] _steps;

    public bool Exists
    {
      get
      {
        if (this._path_type != ResourceInfo.PathType.AssetBundle && this._path_type != ResourceInfo.PathType.StreamingAssets)
          return true;
        return Persist.fileMoved.Data.isAllMoved ? CachedFile.Exists(DLC.GetPath(this._file_name)) : CachedFile.Exists(DLC.ResourceDirectory + this._file_name);
      }
    }
  }

  public struct Step
  {
    public ResourceInfo.StepsType _type;
    public string _path1;
    public string _path2;
    public string _transform;
    public Dictionary<string, ResourceInfo.Step._InternalTextureData>[] _sharedMaterials;

    public string _path => this._path2 == null ? this._path1 : this._path1 + this._path2;

    public string GetSharedMaterials(int num, string key)
    {
      return this._sharedMaterials[num][key].name[1] == null ? this._sharedMaterials[num][key].name[0] : this._sharedMaterials[num][key].name[0] + this._sharedMaterials[num][key].name[1];
    }

    public FilterMode GetSharedMaterialFilterMode(int num, string key)
    {
      return this._sharedMaterials[num][key].filterMode;
    }

    public TextureWrapMode GetSharedMaterialTextureWrapMode(int num, string key)
    {
      return this._sharedMaterials[num][key].wrapMode;
    }

    public struct _InternalTextureData
    {
      public string[] name;
      public FilterMode filterMode;
      public TextureWrapMode wrapMode;
    }
  }

  public struct Resource
  {
    private static string[] SoundExtension = new string[2]
    {
      ".awb",
      ".acb"
    };
    private static string[] MovieExtension = new string[2]
    {
      ".mp4",
      ".usm"
    };
    public string _key;
    public ResourceInfo.Value _value;

    public void Set(KeyValuePair<string, rapidjson.Value> info)
    {
      this._key = string.Intern(info.Key);
      this._value._path_type = (ResourceInfo.PathType) (int) info.Value["path_type"];
      ref ResourceInfo.Value local = ref this._value;
      rapidjson.Value obj = info.Value;
      obj = obj["ext"];
      string str = string.Intern(obj.ToString());
      local._ext = str;
    }

    public void Set(ResourceInfo.PathType path_type, KeyValuePair<string, rapidjson.Value> info)
    {
      this._key = string.Intern(info.Key);
      this._value._path_type = path_type;
      rapidjson.Array array = info.Value.GetArray();
      switch (this._value._path_type)
      {
        case ResourceInfo.PathType.AssetBundle:
          this._value._file_name = string.Copy(array[0].ToString());
          this._value._object_type = (ResourceInfo.ObjectType) array[1].ToInt();
          this._value._file_size = (uint) (long) array[2];
          this.SetSteps(array[3].GetArray());
          break;
        case ResourceInfo.PathType.StreamingAssets:
          this._value._file_name = string.Copy(array[0].ToString());
          this._value._ext = string.Copy(array[1].ToString());
          this._value._object_type = (ResourceInfo.ObjectType) array[2].ToInt();
          this._value._file_size = (uint) (long) array[3];
          this.SetSteps(array[4].GetArray());
          break;
        case ResourceInfo.PathType.Resource:
          this._value._object_type = (ResourceInfo.ObjectType) array[0].ToInt();
          this.SetSteps(array[1].GetArray());
          break;
      }
    }

    private void SetSteps(rapidjson.Array infos)
    {
      if (infos.Length == 0)
        return;
      ResourceInfo.Step[] stepArray = new ResourceInfo.Step[infos.Length];
      for (int index1 = 0; index1 < infos.Length; ++index1)
      {
        rapidjson.Value info = infos[index1];
        ref ResourceInfo.Step local1 = ref stepArray[index1];
        rapidjson.Value obj = info[0];
        int num = obj.ToInt();
        local1._type = (ResourceInfo.StepsType) num;
        obj = info[1];
        string[] stepPath1 = this.GetStepPath(obj.ToString());
        stepArray[index1]._path1 = stepPath1[0];
        stepArray[index1]._path2 = stepPath1[1];
        switch (stepArray[index1]._type)
        {
          case ResourceInfo.StepsType.Renderer:
            obj = info[2];
            rapidjson.Object @object = obj.GetObject();
            int memberCount = @object.MemberCount;
            if (memberCount > 0)
            {
              stepArray[index1]._sharedMaterials = new Dictionary<string, ResourceInfo.Step._InternalTextureData>[memberCount];
              int index2 = 0;
              using (IEnumerator<KeyValuePair<string, rapidjson.Value>> enumerator = @object.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  KeyValuePair<string, rapidjson.Value> current = enumerator.Current;
                  stepArray[index1]._sharedMaterials[index2] = new Dictionary<string, ResourceInfo.Step._InternalTextureData>();
                  obj = current.Value;
                  foreach (KeyValuePair<string, rapidjson.Value> keyValuePair in obj.GetObject())
                  {
                    obj = keyValuePair.Value;
                    rapidjson.Array array = obj.GetArray();
                    ResourceInfo.Step._InternalTextureData internalTextureData = new ResourceInfo.Step._InternalTextureData();
                    ref ResourceInfo.Step._InternalTextureData local2 = ref internalTextureData;
                    obj = array[0];
                    string[] stepPath2 = this.GetStepPath(obj.ToString());
                    local2.name = stepPath2;
                    internalTextureData.filterMode = (FilterMode) (int) array[1];
                    internalTextureData.wrapMode = (TextureWrapMode) (int) array[2];
                    stepArray[index1]._sharedMaterials[index2][keyValuePair.Key] = internalTextureData;
                  }
                  ++index2;
                }
                break;
              }
            }
            else
              break;
          case ResourceInfo.StepsType.UIAtlas:
          case ResourceInfo.StepsType.UILable:
          case ResourceInfo.StepsType.UISprite:
            ref ResourceInfo.Step local3 = ref stepArray[index1];
            obj = info[2];
            string str = string.Intern(obj.ToString());
            local3._transform = str;
            break;
        }
      }
      this._value._steps = stepArray;
    }

    private string[] GetStepPath(string path)
    {
      string[] stepPath = new string[2];
      int num1 = path.LastIndexOf('/');
      if (num1 <= 0)
      {
        stepPath[0] = string.Intern(path);
      }
      else
      {
        int num2 = num1 + 1;
        stepPath[0] = string.Intern(path.Substring(0, num2));
        stepPath[1] = string.Intern(path.Substring(num2));
      }
      return stepPath;
    }

    public bool IsSound()
    {
      return ((IEnumerable<string>) ResourceInfo.Resource.SoundExtension).Contains<string>(Path.GetExtension(this._value._ext));
    }

    public bool IsMovie()
    {
      return ((IEnumerable<string>) ResourceInfo.Resource.MovieExtension).Contains<string>(Path.GetExtension(this._value._ext));
    }
  }

  private class PathComparer : IComparer<ResourceInfo.Resource>
  {
    public static ResourceInfo.PathComparer Default = new ResourceInfo.PathComparer();

    public int Compare(ResourceInfo.Resource x, ResourceInfo.Resource y)
    {
      return Comparer<string>.Default.Compare(x._key, y._key);
    }
  }
}
