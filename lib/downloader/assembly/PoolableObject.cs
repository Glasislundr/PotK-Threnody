// Decompiled with JetBrains decompiler
// Type: PoolableObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Reflection;
using UnityEngine;

#nullable disable
[AddComponentMenu("RecycleSystem/PoolableObject")]
public class PoolableObject : MonoBehaviour
{
  public int maxPoolSize = 10;
  public int preloadCount;
  public bool doNotDestroyOnLoad;
  public bool sendAwakeStartOnDestroyMessage = true;
  public bool sendPoolableActivateDeactivateMessages;
  internal bool _isAvailableForPooling;
  internal bool _createdWithPoolController;
  internal bool _destroyMessageFromPoolController;
  internal bool _wasPreloaded;
  internal bool _wasStartCalledByUnity;
  internal ObjectPoolController.ObjectPool _myPool;
  internal int _serialNumber;
  internal int _usageCount;

  protected void Start() => this._wasStartCalledByUnity = true;

  private static void _InvokeMethodByName(MonoBehaviour behaviour, string methodName)
  {
    if (!Object.op_Implicit((Object) behaviour))
      return;
    MethodInfo method = behaviour.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
    if (!(method != (MethodInfo) null))
      return;
    method.Invoke((object) behaviour, (object[]) null);
  }

  private static void _BroadcastMessageToGameObject(GameObject go, string message)
  {
    foreach (MonoBehaviour component in go.GetComponents(typeof (MonoBehaviour)))
      PoolableObject._InvokeMethodByName(component, message);
    if (go.transform.childCount <= 0)
      return;
    PoolableObject._BroadcastMessageToAllChildren(go, message);
  }

  private static void _BroadcastMessageToAllChildren(GameObject go, string message)
  {
    Transform[] transformArray = new Transform[go.transform.childCount];
    for (int index = 0; index < go.transform.childCount; ++index)
      transformArray[index] = go.transform.GetChild(index);
    for (int index = 0; index < transformArray.Length; ++index)
    {
      if (Object.op_Equality((Object) ((Component) transformArray[index]).GetComponent<PoolableObject>(), (Object) null))
        PoolableObject._BroadcastMessageToGameObject(((Component) transformArray[index]).gameObject, message);
    }
  }

  protected void OnDestroy()
  {
    if (!this._destroyMessageFromPoolController && this._myPool != null)
      this._myPool.Remove(this);
    if (!this._destroyMessageFromPoolController)
      PoolableObject._BroadcastMessageToGameObject(((Component) this).gameObject, "OnPoolableInstanceDestroy");
    this._destroyMessageFromPoolController = false;
  }

  public int GetSerialNumber() => this._serialNumber;

  public int GetUsageCount() => this._usageCount;

  public int DeactivateAllPoolableObjectsOfMyKind()
  {
    return this._myPool != null ? this._myPool._SetAllAvailable() : 0;
  }

  public bool IsDeactivated() => this._isAvailableForPooling;

  public PoolableObject[] GetAllPoolableObjectsOfMyKind(bool includeInactiveObjects)
  {
    return this._myPool != null ? this._myPool._GetAllObjects(includeInactiveObjects) : (PoolableObject[]) null;
  }
}
