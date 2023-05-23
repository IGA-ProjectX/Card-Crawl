using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace IGDF
{
    public class O_FlowerBud : MonoBehaviour
    {
        private AnimationClip blossomAnim;
        public AnimationClip[] animClips;
        private float animTime;
        private bool isBlooming = false;
        private bool isBlossomy = false;
        private NodeInfo thisNode;
        private CharacterType treeType;
        private bool isDataBlossomy = false;

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
            switch (treeType)
            {
                case CharacterType.Producer:
                    blossomAnim = animClips[0];
                    blossomAnim.SampleAnimation(gameObject, 0);
                    M_Vivarium.instance.waterAnim.SampleAnimation(M_Vivarium.instance.GetCharacterWaterAnim(CharacterType.Producer), 0);
                    break;
                case CharacterType.Designer:
                    break;
                case CharacterType.Artist:
                    break;
                case CharacterType.Programmer:
                    blossomAnim = animClips[1];
                    blossomAnim.SampleAnimation(gameObject, 0);
                    M_Vivarium.instance.waterAnim.SampleAnimation(M_Vivarium.instance.GetCharacterWaterAnim(CharacterType.Programmer), 0);
                    break;
            }
  
            isDataBlossomy = CheckIsFlowerBlossomy();
            if (isDataBlossomy) FlowerTurnIntoBlossomy();
        }

        private void OnMouseDown()
        {
            if (M_SkillTree.instance.GetTreeState(treeType))
            {
                if (CheckUnlockable())
                {
                    isBlooming = true;
                }
                else
                {
                    Sequence s = DOTween.Sequence();
                    s.Append(transform.DORotate(new Vector3(0, 0, -5), 0.1f));
                    s.Append(transform.DORotate(new Vector3(0, 0, 10), 0.2f));
                    s.Append(transform.DORotate(new Vector3(0, 0, 0), 0.1f));
                }
            } 
        }

        private void OnMouseUp()
        {
            if (M_SkillTree.instance.GetTreeState(treeType)) isBlooming = false;
        }

        private void TimeForward()
        {
            float stepPerAnim = animTime / thisNode.expToUnlock;
            if (blossomAnim!=null)
            {
                animTime += Time.deltaTime;
                if (animTime > blossomAnim.length)
                {
                    FlowerTurnIntoBlossomy();
                }
                blossomAnim.SampleAnimation(gameObject, animTime);
                M_Vivarium.instance.waterAnim.SampleAnimation(M_Vivarium.instance.GetCharacterWaterAnim(treeType), animTime);
            }
        }

        private void TimeRewind()
        {
            if (blossomAnim != null)
            {
                if (animTime>0)
                {
                    animTime -= Time.deltaTime;
                    blossomAnim.SampleAnimation(gameObject, animTime);
                    M_Vivarium.instance.waterAnim.SampleAnimation(M_Vivarium.instance.GetCharacterWaterAnim(treeType), animTime);
                }
                else animTime = 0;
            }
        }

        private void FlowerTurnIntoBlossomy()
        {
            isBlossomy = true;
            blossomAnim.SampleAnimation(gameObject, blossomAnim.length);
            M_Vivarium.instance.waterAnim.SampleAnimation(M_Vivarium.instance.GetCharacterWaterAnim(treeType), 0);
            FluctifySkills();
            transform.GetComponent<BoxCollider2D>().enabled = false;
            if (!isDataBlossomy) UpdateUnlockedSkillToData();
        }

        public void FluctifySkills()
        {
            if (!CheckIsSkillLoaded())
            {
                Transform newSkill = Instantiate(M_SkillTree.instance.pre_FruitSkill, transform).transform;
                float targetScale = newSkill.localScale.x;
                newSkill.transform.localPosition = Vector3.zero;
                newSkill.localScale = Vector3.zero;
                Sequence s = DOTween.Sequence();
                newSkill.GetComponent<O_V_SkillFruit>().InitializeSkillFruit(thisNode.childSkills[0],transform);
                newSkill.DOScale(targetScale, 0.7f);
            }

            bool CheckIsSkillLoaded()
            {
                List<SO_Skill> tempList = new List<SO_Skill>();
                foreach (int skillIndex in M_Global.instance.mainData.inUseSkills) tempList.Add(M_Global.instance.GetSingleSkillInUse(skillIndex));

                if (tempList.Contains(thisNode.childSkills[0])) return true;
                else return false;
            }
        }

        public SO_Skill GetBudSkillInfo()
        {
            return thisNode.childSkills[0];
        }

        public CharacterType GetBudParentTreeType()
        {
            return treeType;
        }

        public string GetBudPriceToUnlock()
        {
            if (!isBlossomy)
            {
                return thisNode.expToUnlock.ToString() + "Exp";
            }
            else return "";
        }

        bool CheckIsFlowerBlossomy()
        {
            foreach (var unlockedNode in M_Global.instance.mainData.unlockedSkillNodes)
                if (unlockedNode.characterType == treeType && unlockedNode.thisNodeIndex == thisNode.thisNodeIndex)
                   return isDataBlossomy = true;
            return false;
        }

        void UpdateUnlockedSkillToData()
        {
            M_Global.instance.mainData.unlockedSkillNodes.Add(new UnlockedSkillNode(treeType, thisNode.thisNodeIndex));
            M_Global.instance.PlayerExpUp(-thisNode.expToUnlock);
            O_UpperUIBar.instance.ChangeExp();
            isDataBlossomy = true;
        }

        bool CheckUnlockable()
        {
            if (M_Global.instance.mainData.playExp >= thisNode.expToUnlock) return true;
            else return false;
        }
    }

    [System.Serializable]
    public class ShakeData
    {
        public float duration;
        public float strength;
        public int vibrato;
        public float randomness;
        public bool fade;
        public ShakeRandomnessMode targetMode;
    }
}