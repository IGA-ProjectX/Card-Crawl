using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IGDF
{
    public class O_FlowerBud : MonoBehaviour
    {
        public AnimationClip blossimAnim;
        private float animTime;
        private bool isBlooming = false;
        private bool isBlossomy = false;
        private NodeInfo thisNode;
        private CharacterType treeType;

        void Update()
        {
            if (!isBlossomy)
                if (isBlooming) TimeForward();
                else TimeRewind();
        }

        public void InitializeBud(NodeInfo infoToSet,CharacterType whichTree)
        {
            thisNode = infoToSet;
            treeType = whichTree;
            CheckIsFlowerBlossomy();

            void CheckIsFlowerBlossomy()
            {
                foreach (var unlockedNode in M_Global.instance.mainData.unlockedSkillNodes)
                    if (unlockedNode.characterType == treeType && unlockedNode.thisNodeIndex == thisNode.thisNodeIndex)
                        FlowerTurnIntoBlossomy();
            }
        }

        private void OnMouseDown()
        {
            isBlooming = true;
        }

        private void OnMouseUp()
        {
            isBlooming = false;
        }

        private void TimeForward()
        {
            if (blossimAnim!=null)
            {
                animTime += Time.deltaTime;
                if (animTime > blossimAnim.length) FlowerTurnIntoBlossomy();
                blossimAnim.SampleAnimation(gameObject, animTime);
            }
        }

        private void TimeRewind()
        {
            if (blossimAnim != null)
            {
                if (animTime>0)
                {
                    animTime -= Time.deltaTime;
                    blossimAnim.SampleAnimation(gameObject, animTime);
                }
                else animTime = 0;
            }
        }

        private void FlowerTurnIntoBlossomy()
        {
            isBlossomy = true;
            blossimAnim.SampleAnimation(gameObject, blossimAnim.length);
            FluctifySkills();
            transform.GetComponent<BoxCollider2D>().enabled = false;
        }

        public void FluctifySkills()
        {
            if (!CheckIsSkillLoaded())
            {
                Transform newSkill = Instantiate(M_SkillTree.instance.pre_FruitSkill, transform).transform;
                float targetScale = newSkill.localScale.x;
                newSkill.transform.localPosition = Vector3.zero;
                newSkill.localScale = Vector3.zero;
                newSkill.GetComponent<O_V_SkillFruit>().InitializeSkillFruit(thisNode.childSkills[0]);
                newSkill.DOScale(targetScale, 0.7f);
            }

            bool CheckIsSkillLoaded()
            {
                List<SO_Skill> tempList = new List<SO_Skill>();
                foreach (SO_Skill skill in M_Global.instance.skillList) tempList.Add(skill);

                if (tempList.Contains(thisNode.childSkills[0])) return true;
                else return false;
            }
        }
    }
}