using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformHP : MonoBehaviour
{
    [Header("Stats")] public float MaxHP;
    private float HP;

    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
