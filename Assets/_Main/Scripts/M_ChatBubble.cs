using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF
{
    public class M_ChatBubble : MonoBehaviour
    {
        public GameObject pre_ChatBubble;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                InstantiateChatBubble(transform, CharacterType.Producer, "我希望将反抗组织刻画成一个\n神秘的存在，\n所以我得设计一些谜题。");
            }
        }

        public void InstantiateChatBubble(Transform instantiatePivot, CharacterType chaType, string sentence)
        {
            GameObject go = Instantiate(pre_ChatBubble, instantiatePivot.position, Quaternion.identity, GameObject.Find("Canvas").transform.Find("ChatBubbles"));
            go.transform.localScale = Vector3.zero;
            go.GetComponent<O_ChatBubble>().PopUpChatBubble(chaType, sentence);
        }
    }

}
