using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace IGDF
{
    public class M_Pause : MonoBehaviour
    {
        private bool isOpened = false;
        private HoverState beforeState;
        public Transform studioCarParent;

        [SerializeField] private TMPro.TMP_Text t_Restart;
        [SerializeField] private TMPro.TMP_Text t_Continue;
        [SerializeField] private TMPro.TMP_Text t_ReturnToOpenPage;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                if (isOpened) ClosePanel();
                else OpenPanel();
        }

        public void LanguageSet()
        {
            if (M_Global.instance.GetLanguage() == SystemLanguage.Chinese)
            {
                t_Restart.text = "重新开始";
                t_Continue.text = "继续游戏";
                t_ReturnToOpenPage.text = "返回外车厢";
            }
            else
            {
                t_Restart.text = "Restart";
                t_Continue.text = "Continue";
                t_ReturnToOpenPage.text = "Open Page";
            }
        }

        public void OpenPanel()
        {
            LanguageSet();
            transform.DOScale(1, 0.4f);
            isOpened = true;
            //Time.timeScale = 0;
            beforeState = M_Main.instance.m_HoverTip.GetCurrentState();
            M_Main.instance.m_HoverTip.EnterState(HoverState.AllDisactive);
            M_Main.instance.m_Skill.EnterCanNotUseState();
        }

        public void ClosePanel()
        {
            transform.DOScale(0, 0.4f);
            isOpened = false;
            //Time.timeScale = 1;
            M_Main.instance.m_HoverTip.EnterState(beforeState);
            M_Main.instance.m_Skill.EnterWaitForUseState();
        }

        public void B_ContinueGame()
        {
            ClosePanel();
        }

        public void B_RestartGame()
        {
            ClosePanel();
            foreach (O_ChatBubble bubble in transform.parent.Find("ChatBubbles").GetComponentsInChildren<O_ChatBubble>())
            {
                bubble.DestroyBubble();
            }
            Sequence s = DOTween.Sequence();
            s.Append(studioCarParent.Find("Car Door").DOMoveY(studioCarParent.Find("Car Door").position.y - 12, 1));
            s.AppendCallback(() => SceneManager.UnloadSceneAsync(1));
            s.AppendInterval(0.2f);
            s.AppendCallback(() => SceneManager.LoadScene(1, LoadSceneMode.Additive));
            s.AppendInterval(0.2f);
            s.AppendCallback(() => M_Main.instance.GameStart());
            s.Append(studioCarParent.Find("Car Door").DOMoveY(studioCarParent.Find("Car Door").position.y, 1));
        }

        public void B_ReturnToOpenPage()
        {
            ClosePanel();
            FindObjectOfType<M_SceneTransition>().ExitCurrentCabin();
        }
    }
}