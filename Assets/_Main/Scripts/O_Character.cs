using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF
{
    public class O_Character : MonoBehaviour
    {
        public CharacterType thisCharacter;

        public CharacterInfo GetCharacterInfo()
        {
            foreach (CharacterInfo chaInfo in M_Global.instance.repository.characterInfos)
            {
                if (chaInfo.type == thisCharacter)
                {
                    return chaInfo;
                }
            }
            Debug.LogError("No Character");
            return null;
        }
    }
}

