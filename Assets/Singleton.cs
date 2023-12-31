using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance;

    public bool DoPersist = true;
    

    // Start is called before the first frame update
    protected virtual void Awake() {
        if (instance == null) {
            instance = this as T;
            if (DoPersist) {
                DontDestroyOnLoad(gameObject);
            }
        } else {
            Destroy(this.gameObject);
        }
    }
}
