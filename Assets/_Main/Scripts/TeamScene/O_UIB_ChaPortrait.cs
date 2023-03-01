using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF
{
    public class O_UIB_ChaPortrait : MonoBehaviour
    {
        public CharacterType characterType;

        private void Awake()
        {
            gameObject.AddComponent<BoxCollider2D>();
        }

        private void OnMouseEnter()
        {
            
        }

        private void OnMouseDown()
        {
            switch (characterType)
            {
                case CharacterType.Producer:
                    M_SkillTree.instance.OpenSkillTreePanel(0);
                    break;
                case CharacterType.Designer:
                    M_SkillTree.instance.OpenSkillTreePanel(1);
                    break;
                case CharacterType.Artist:
                    M_SkillTree.instance.OpenSkillTreePanel(2);
                    break;
                case CharacterType.Programmer:
                    M_SkillTree.instance.OpenSkillTreePanel(3);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
                    break;
            }

        }
    }
}