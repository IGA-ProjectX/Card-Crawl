using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_SkillTree : MonoBehaviour
{
    private GameObject glassCover;

    void Start()
    {
    

    }

    public void UpdateGlassState(bool glassState)
    {
        glassCover = transform.Find("GlassCover").gameObject;
        glassCover.SetActive(!glassState);
    }
}
