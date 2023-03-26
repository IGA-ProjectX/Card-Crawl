using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF
{
    
    public class M_ChatBubble : MonoBehaviour
    {
        public GameObject pre_ChatBubble;
        public Dictionary<TalkConditionType, bool> isToldStates = new Dictionary<TalkConditionType, bool>();
        public Dictionary<TalkConditionType, TalkContent> talkContentPool = new Dictionary<TalkConditionType, TalkContent>();

        public void InstantiateChatBubble(TalkConditionType talkConditionType)
        {
            CharacterType toTalkCha = talkContentPool[talkConditionType].talkCharacter;
            GameObject go = Instantiate(pre_ChatBubble, Vector3.zero, Quaternion.identity, M_Global.instance.chatBubbleParent);
            go.transform.localScale = Vector3.zero;
            go.GetComponent<O_ChatBubble>().PopUpChatBubble(toTalkCha, talkContentPool[talkConditionType]);
            isToldStates[talkConditionType] = true;
        }

        public void PrepareTalkList(TalkContent[] talkArray)
        {
            foreach (TalkContent talkContent in talkArray)
            {
                isToldStates.Add(talkContent.conditionType, false);
                talkContentPool.Add(talkContent.conditionType, talkContent);
            }
        }

        public void TryTriggerTalkStaffValueChange(CharacterType staffType)
        {
            foreach (TalkConditionType conditionType in talkContentPool.Keys)
            {
                if (!isToldStates[conditionType])
                {
                    switch (conditionType)
                    {
                        case TalkConditionType.ExpSVLargerThan100:
                            if (staffType == CharacterType.Producer)
                                if (M_Main.instance.m_Staff.GetStaffValue(0)>=100)
                                    InstantiateChatBubble(TalkConditionType.ExpSVLargerThan100);
                            break;
                        case TalkConditionType.DesSVLargerThan10:
                            if (staffType == CharacterType.Designer)
                                if (M_Main.instance.m_Staff.GetStaffValue(1) > 10)
                                    InstantiateChatBubble(TalkConditionType.DesSVLargerThan10);
                            break;
                        case TalkConditionType.ArtSVLargerThan10:
                            if (staffType == CharacterType.Artist)
                                if (M_Main.instance.m_Staff.GetStaffValue(2) > 10)
                                    InstantiateChatBubble(TalkConditionType.ArtSVLargerThan10);
                            break;
                        case TalkConditionType.ProSVLargerThan10:
                            if (staffType == CharacterType.Programmer)
                                if (M_Main.instance.m_Staff.GetStaffValue(3) > 10)
                                    InstantiateChatBubble(TalkConditionType.ProSVLargerThan10);
                            break;
                    }
                }
            }
        }

        public void TryTriggerTalkCardUse(O_Card usedCard)
        {

        }

        public void TryTriggerTalkSkillUse(O_Skill usedSkill)
        {

        }

        public void TryTriggerTalkSpecialCondition(TalkConditionType talkConditionType)
        {

        }
    }

}
