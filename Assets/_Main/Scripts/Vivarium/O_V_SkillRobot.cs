using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IGDF
{
    public class O_V_SkillRobot : MonoBehaviour
    {
        [HideInInspector]public bool isReadyForLoadNewSkill = true;
        public static O_V_SkillRobot selectedtSkillRobot;
        private SO_Skill currentSkill;

        private Vector2 upperLidOpenPos;
        private Vector2 bottomLidOpenPos;
        private Vector2 upperLidClosePos = new Vector2(0, 0.23f);
        private Vector2 bottomLidClosePos = new Vector2(0, -0.3f);
        private Vector2 eyeballMiddlePos;

        private Transform eyelidUpper;
        private Transform eyelidBottom;
        private Transform eyeball;

        private int skillIndex;

        public void InitializeSkillRobot(SO_Skill initialSkill,int toSetIndex)
        {
            currentSkill = initialSkill;
            skillIndex = toSetIndex;
            eyelidUpper = transform.Find("Eye White").Find("Eyelid Upper");
            eyelidBottom = transform.Find("Eye White").Find("Eyelid Bottom");
            eyeball = transform.Find("Eye White").Find("Eye Black");

            eyeball.Find("Eye Pupil").GetComponent<SpriteRenderer>().sprite = currentSkill.skillImage;

            upperLidOpenPos = eyelidUpper.position;
            bottomLidOpenPos = eyelidBottom.position;
            eyelidUpper.localPosition = upperLidClosePos;
            eyelidBottom.localPosition = bottomLidClosePos;
            upperLidClosePos = eyelidUpper.position;
            bottomLidClosePos = eyelidBottom.position;
            eyeballMiddlePos = eyeball.position;
        }

        void Update()
        {
            if (O_V_SkillFruit.selectedSkill!=null)
            {
                if (Vector2.Distance(O_V_SkillFruit.selectedSkill.transform.position, transform.position) < 1f)
                {
                    selectedtSkillRobot = this;
                }
            }
        }

        public void LoadNewSkillIntoThis(SO_Skill newSkill)
        {
            isReadyForLoadNewSkill = false;
            UnlockedSkillNode respawnBud = FindFlowerBudToRespawn(currentSkill);

            currentSkill = newSkill;
            //M_Global.instance.skillList[skillIndex] = currentSkill;
            //M_Global.instance.mainData.inUseSkills[skillIndex] = currentSkill;
            M_Global.instance.LoadSkillIntoInUse(skillIndex, currentSkill);

            Sequence s = DOTween.Sequence();
            s.AppendCallback(() => CloseEye());
            s.AppendInterval(0.7f);
            s.AppendCallback(() => eyeball.Find("Eye Pupil").GetComponent<SpriteRenderer>().sprite = currentSkill.skillImage);
            s.AppendCallback(() => OpenEye());
            s.AppendCallback(() => isReadyForLoadNewSkill = true);

            //Respawn Bud
            switch (respawnBud.characterType)
            {
                case CharacterType.Producer:
                    TargetBudRefructify(0, GetBudIndex(respawnBud));
                    break;
                case CharacterType.Designer:
                    TargetBudRefructify(1, GetBudIndex(respawnBud));
                    break;
                case CharacterType.Artist:
                    TargetBudRefructify(2, GetBudIndex(respawnBud));
                    break;
                case CharacterType.Programmer:
                    TargetBudRefructify(3, GetBudIndex(respawnBud));
                    break;
            }

            UnlockedSkillNode FindFlowerBudToRespawn(SO_Skill replacedSkill)
            {
                foreach (SO_SkillParent skillParent in M_SkillTree.instance.skillParents)
                    foreach (NodeInfo node in skillParent.nodeList)
                        if (node.childSkills[0] == replacedSkill)
                            return new UnlockedSkillNode(skillParent.characterType, node.thisNodeIndex);
                return null;
            }

            int GetBudIndex(UnlockedSkillNode toRespawnBud)
            {
                NodeInfo[] targetTree = M_SkillTree.instance.skillParents[(int)toRespawnBud.characterType].nodeList;
                for (int i = 0; i < targetTree.Length; i++)
                {
                    if (targetTree[i].thisNodeIndex == toRespawnBud.thisNodeIndex) return i;
                }
                return 0;
            }

            void TargetBudRefructify(int treeIndex, int flowerIndex)
            {
                M_SkillTree.instance.skillTrees[treeIndex].transform.Find("FlowerPivots").GetChild(flowerIndex).GetComponentInChildren<O_FlowerBud>().FluctifySkills();
            }
        }

        public void OpenEye()
        {
            float speed = 0.7f;
            eyeball.DOMove(eyeballMiddlePos, 0.3f);
            eyelidUpper.DOMoveY(upperLidOpenPos.y, speed);
            eyelidBottom.DOMoveY(bottomLidOpenPos.y, speed);
            M_Audio.PlaySound(SoundType.SkillRobotEyeOpen);
        }

        public void CloseEye()
        {
            float speed = 0.7f;
            eyelidUpper.DOMoveY(upperLidClosePos.y, speed);
            eyelidBottom.DOMoveY(bottomLidClosePos.y, speed);
        }

        public void RotateEye()
        {
            Vector2 direction = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)eyeball.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
         
            eyeball.DOMove(eyeballMiddlePos + new Vector2(direction.normalized.x / 5, direction.normalized.y / 5), 0.2f);
        }

        public SO_Skill CurrentSkillInfo()
        {
            return currentSkill;
        }
    }
}