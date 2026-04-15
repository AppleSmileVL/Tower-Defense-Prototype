using UnityEngine;

[DisallowMultipleComponent]
public abstract class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// Automatically mark object as persistent.
    /// </summary>
    [Header("Singleton")]
    [SerializeField] private bool m_DoNotDestroyOnLoad;

    public static T Instance { get; private set; }

    public void Init()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("MonoSingleton: object of type already exists, instance will be destroyed = " + typeof(T).Name);
            Destroy(this.gameObject);
            return;
        }

        Instance = this as T;

        if (m_DoNotDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
