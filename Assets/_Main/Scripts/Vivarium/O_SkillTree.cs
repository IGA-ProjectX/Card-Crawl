using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_SkillTree : MonoBehaviour
{
    private GameObject glassCover;

    void Start()
    {
        glassCover = transform.Find("GlassCover").gameObject;

    }

    public void UpdateGlassState(bool glassState)
    {
        glassCover.SetActive(!glassState);
    }
}
