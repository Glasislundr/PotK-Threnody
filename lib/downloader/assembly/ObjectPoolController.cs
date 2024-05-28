// Decompiled with JetBrains decompiler
// Type: ObjectPoolController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class ObjectPoolController
{
  internal static int _globalSerialNumber = 0;
  internal static bool _isDuringInstantiate = false;
  private static Dictionary<GameObject, ObjectPoolController.ObjectPool> _pools = new Dictionary<GameObject, ObjectPoolController.ObjectPool>();

  public static bool isDuringPreload { get; private set; }

  public static GameObject Instantiate(GameObject prefab)
  {
    PoolableObject component = prefab.GetComponent<PoolableObject>();
    if (Object.op_Equality((Object) component, (Object) null))
      return Object.Instantiate<GameObject>(prefab);
    GameObject pooledInstance = ObjectPoolController._GetPool(component).GetPooledInstance(new Vector3?(), new Quaternion?());
    return Object.op_Implicit((Object) pooledInstance) ? pooledInstance : ObjectPoolController.InstantiateWithoutPool(prefab);
  }

  public static GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion quaternion)
  {
    PoolableObject component = prefab.GetComponent<PoolableObject>();
    if (Object.op_Equality((Object) component, (Object) null))
      return Object.Instantiate<GameObject>(prefab, position, quaternion);
    GameObject pooledInstance = ObjectPoolController._GetPool(component).GetPooledInstance(new Vector3?(position), new Quaternion?(quaternion));
    return Object.op_Implicit((Object) pooledInstance) ? pooledInstance : ObjectPoolController.InstantiateWithoutPool(prefab, position, quaternion);
  }

  public static GameObject InstantiateWithoutPool(GameObject prefab)
  {
    return ObjectPoolController.InstantiateWithoutPool(prefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
  }

  public static GameObject InstantiateWithoutPool(
    GameObject prefab,
    Vector3 position,
    Quaternion quaternion)
  {
    ObjectPoolController._isDuringInstantiate = true;
    GameObject gameObject = Object.Instantiate<GameObject>(prefab, position, quaternion);
    ObjectPoolController._isDuringInstantiate = false;
    PoolableObject component = gameObject.GetComponent<PoolableObject>();
    if (Object.op_Implicit((Object) component))
    {
      if (component.doNotDestroyOnLoad)
        Object.DontDestroyOnLoad((Object) gameObject);
      component._createdWithPoolController = true;
      Object.Destroy((Object) component);
    }
    return gameObject;
  }

  public static void Destroy(GameObject obj)
  {
    if (!Object.op_Implicit((Object) obj))
      return;
    PoolableObject component = obj.GetComponent<PoolableObject>();
    if (Object.op_Equality((Object) component, (Object) null))
    {
      ObjectPoolController._DetachChildrenAndDestroy(obj.transform);
      Object.Destroy((Object) obj);
    }
    else if (component._myPool != null)
    {
      component._myPool._SetAvailable(component, true);
    }
    else
    {
      if (!component._createdWithPoolController)
        Debug.LogWarning((object) ("Poolable object " + ((Object) obj).name + " not created with ObjectPoolController"));
      Object.Destroy((Object) obj);
    }
  }

  public static void Preload(GameObject prefab)
  {
    PoolableObject component = prefab.GetComponent<PoolableObject>();
    if (Object.op_Equality((Object) component, (Object) null))
    {
      Debug.LogWarning((object) ("Can not preload because prefab '" + ((Object) prefab).name + "' is not poolable"));
    }
    else
    {
      ObjectPoolController.ObjectPool pool = ObjectPoolController._GetPool(component);
      int num = component.preloadCount - pool.GetObjectCount();
      if (num <= 0)
        return;
      ObjectPoolController.isDuringPreload = true;
      try
      {
        for (int index = 0; index < num; ++index)
          pool.PreloadInstance();
      }
      finally
      {
        ObjectPoolController.isDuringPreload = false;
      }
    }
  }

  internal static ObjectPoolController.ObjectPool _GetPool(PoolableObject prefabPoolComponent)
  {
    GameObject gameObject = ((Component) prefabPoolComponent).gameObject;
    ObjectPoolController.ObjectPool pool;
    if (!ObjectPoolController._pools.TryGetValue(gameObject, out pool))
    {
      pool = new ObjectPoolController.ObjectPool(gameObject);
      ObjectPoolController._pools.Add(gameObject, pool);
    }
    return pool;
  }

  private static void _DetachChildrenAndDestroy(Transform transform)
  {
    int childCount = transform.childCount;
    Transform[] transformArray = new Transform[childCount];
    Transform transform1 = transform;
    for (int index = 0; index < childCount; ++index)
      transformArray[index] = transform1.GetChild(index);
    transform1.DetachChildren();
    for (int index = 0; index < childCount; ++index)
    {
      GameObject gameObject = ((Component) transformArray[index]).gameObject;
      if (Object.op_Implicit((Object) gameObject))
        ObjectPoolController.Destroy(gameObject);
    }
  }

  internal class ObjectPool
  {
    private HashSet_Object<PoolableObject> _pool;
    private PoolableObject _prefabPoolObj;
    private Transform _poolParentDummy;

    internal Transform poolParentDummy
    {
      get
      {
        this._ValidatePoolParentDummy();
        return this._poolParentDummy;
      }
    }

    private void _ValidatePoolParentDummy()
    {
      if (Object.op_Implicit((Object) this._poolParentDummy))
        return;
      GameObject gameObject = new GameObject("POOL:" + ((Object) this._prefabPoolObj).name);
      this._poolParentDummy = gameObject.transform;
      this._SetActive(gameObject, false);
      if (!this._prefabPoolObj.doNotDestroyOnLoad)
        return;
      Object.DontDestroyOnLoad((Object) gameObject);
    }

    public ObjectPool(GameObject prefab)
    {
      this._prefabPoolObj = prefab.GetComponent<PoolableObject>();
    }

    private void _ValidatePooledObjectDataContainer()
    {
      if (this._pool != null)
        return;
      this._pool = new HashSet_Object<PoolableObject>();
      this._ValidatePoolParentDummy();
    }

    internal void Remove(PoolableObject poolObj) => this._pool.Remove(poolObj);

    internal int GetObjectCount() => this._pool == null ? 0 : this._pool.Count;

    internal GameObject GetPooledInstance(Vector3? position, Quaternion? rotation)
    {
      this._ValidatePooledObjectDataContainer();
      Transform transform1 = ((Component) this._prefabPoolObj).transform;
      foreach (PoolableObject poolObj in (HashSet<PoolableObject>) this._pool)
      {
        if (Object.op_Inequality((Object) poolObj, (Object) null) && poolObj._isAvailableForPooling)
        {
          Transform transform2 = ((Component) poolObj).transform;
          transform2.position = position.HasValue ? position.Value : transform1.position;
          transform2.rotation = rotation.HasValue ? rotation.Value : transform1.rotation;
          transform2.localScale = transform1.localScale;
          ++poolObj._usageCount;
          this._SetAvailable(poolObj, false);
          return ((Component) poolObj).gameObject;
        }
      }
      return this._pool.Count < this._prefabPoolObj.maxPoolSize ? ((Component) this._NewPooledInstance(position, rotation)).gameObject : (GameObject) null;
    }

    internal PoolableObject PreloadInstance()
    {
      this._ValidatePooledObjectDataContainer();
      PoolableObject poolObj = this._NewPooledInstance(new Vector3?(), new Quaternion?());
      poolObj._wasPreloaded = true;
      this._SetAvailable(poolObj, true);
      return poolObj;
    }

    private PoolableObject _NewPooledInstance(Vector3? position, Quaternion? rotation)
    {
      ObjectPoolController._isDuringInstantiate = true;
      GameObject gameObject = !position.HasValue || !rotation.HasValue ? Object.Instantiate<GameObject>(((Component) this._prefabPoolObj).gameObject) : Object.Instantiate<GameObject>(((Component) this._prefabPoolObj).gameObject, position.Value, rotation.Value);
      ObjectPoolController._isDuringInstantiate = false;
      PoolableObject component = gameObject.GetComponent<PoolableObject>();
      component._createdWithPoolController = true;
      component._myPool = this;
      component._isAvailableForPooling = false;
      component._serialNumber = ++ObjectPoolController._globalSerialNumber;
      ++component._usageCount;
      if (component.doNotDestroyOnLoad)
        Object.DontDestroyOnLoad((Object) gameObject);
      this._pool.Add(component);
      gameObject.BroadcastMessage("OnPoolableInstanceAwake", (SendMessageOptions) 1);
      return component;
    }

    internal int _SetAllAvailable()
    {
      int num = 0;
      foreach (PoolableObject poolObj in (HashSet<PoolableObject>) this._pool)
      {
        if (Object.op_Inequality((Object) poolObj, (Object) null) && !poolObj._isAvailableForPooling)
        {
          this._SetAvailable(poolObj, true);
          ++num;
        }
      }
      return num;
    }

    internal PoolableObject[] _GetAllObjects(bool includeInactiveObjects)
    {
      List<PoolableObject> poolableObjectList = new List<PoolableObject>();
      foreach (PoolableObject poolableObject in (HashSet<PoolableObject>) this._pool)
      {
        if (Object.op_Inequality((Object) poolableObject, (Object) null) && (includeInactiveObjects || !poolableObject._isAvailableForPooling))
          poolableObjectList.Add(poolableObject);
      }
      return poolableObjectList.ToArray();
    }

    internal void _SetAvailable(PoolableObject poolObj, bool b)
    {
      poolObj._isAvailableForPooling = b;
      Transform transform = ((Component) poolObj).transform;
      if (b)
      {
        if (poolObj.sendAwakeStartOnDestroyMessage)
          poolObj._destroyMessageFromPoolController = true;
        transform.parent = (Transform) null;
        this._RecursivelySetInactiveAndSendMessages(((Component) poolObj).gameObject, poolObj, false);
        transform.parent = poolObj._myPool.poolParentDummy;
      }
      else
      {
        transform.parent = (Transform) null;
        this._SetActiveAndSendMessages(((Component) poolObj).gameObject, poolObj);
      }
    }

    private void _SetActive(GameObject obj, bool active) => obj.SetActive(active);

    private bool _GetActive(GameObject obj) => obj.activeInHierarchy;

    private void _SetActiveAndSendMessages(GameObject obj, PoolableObject parentPoolObj)
    {
      this._SetActive(obj, true);
      if (parentPoolObj.sendAwakeStartOnDestroyMessage)
      {
        obj.BroadcastMessage("Awake", (object) null, (SendMessageOptions) 1);
        if (this._GetActive(obj) && parentPoolObj._wasStartCalledByUnity)
          obj.BroadcastMessage("Start", (object) null, (SendMessageOptions) 1);
      }
      if (!parentPoolObj.sendPoolableActivateDeactivateMessages)
        return;
      obj.BroadcastMessage("OnPoolableObjectActivated", (object) null, (SendMessageOptions) 1);
    }

    private void _RecursivelySetInactiveAndSendMessages(
      GameObject obj,
      PoolableObject parentPoolObj,
      bool recursiveCall)
    {
      Transform transform1 = obj.transform;
      Transform[] transformArray = new Transform[transform1.childCount];
      for (int index = 0; index < transform1.childCount; ++index)
        transformArray[index] = transform1.GetChild(index);
      for (int index = 0; index < transformArray.Length; ++index)
      {
        Transform transform2 = transformArray[index];
        PoolableObject component = ((Component) transform2).gameObject.GetComponent<PoolableObject>();
        if (Object.op_Implicit((Object) component) && component._myPool != null)
          this._SetAvailable(component, true);
        else
          this._RecursivelySetInactiveAndSendMessages(((Component) transform2).gameObject, parentPoolObj, true);
      }
      if (parentPoolObj.sendAwakeStartOnDestroyMessage)
        obj.SendMessage("OnDestroy", (object) null, (SendMessageOptions) 1);
      if (parentPoolObj.sendPoolableActivateDeactivateMessages)
        obj.SendMessage("OnPoolableObjectDeactivated", (object) null, (SendMessageOptions) 1);
      if (recursiveCall)
        return;
      this._SetActive(obj, false);
    }
  }
}
