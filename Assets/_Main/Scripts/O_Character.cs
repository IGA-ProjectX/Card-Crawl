using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF
{
    public class O_Character : MonoBehaviour
    {
        public CharacterType thisCharacter;

        private void Start()
        {
            if (thisCharacter!= CharacterType.Producer)
            {
                InitializeNameTemplate();
            }
        }

        private void InitializeNameTemplate()
        {
            switch (thisCharacter)
            {
                case CharacterType.Designer:
                    ChangeName("Design", "设计");
                    break;
                case CharacterType.Artist:
                    ChangeName("Art", "美术");
                    break;
                case CharacterType.Programmer:
                    ChangeName("Code", "程序");
                    break;
            }

            void ChangeName(string eng, string chi)
            {
                TMPro.TMP_Text targetText = transform.GetChild(0).Find("Icon").GetComponent<TMPro.TMP_Text>();
                if (M_Global.instance.GetLanguage() == SystemLanguage.English) targetText.text = eng;
                else targetText.text = chi;
            }
        }

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

