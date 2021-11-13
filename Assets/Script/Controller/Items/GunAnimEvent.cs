using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimEvent : MonoBehaviour
{
    ItemGun gun;
    protected AnimatorOverrideController overrideController;
    private void Awake()
    {
         gun = GetComponentInParent<ItemGun>();
        // overrideController = new AnimatorOverrideController(GetComponent<Animator>().runtimeAnimatorController);
        // GetComponent<Animator>().runtimeAnimatorController = overrideController;

        //  if (overrideController["_Shoot"].events.Length > 0)
        // {
        //     overrideController["_Shoot"].events[0].functionName = "CreateShell";
        // }
    }

    // Update is called once per frame
    void CreateShell()
    {
        gun.CreateShell();
    }
}
