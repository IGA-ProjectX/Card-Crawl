using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class M_Button : MonoBehaviour
{
    public void ClickExitGame()
    {
        Application.Quit();
    }

    public void ClickEnterStaffPage()
    {

    }

    public void ClickEnterSettingPage()
    {

    }

    public void ClickEnterMainPage()
    {
        SceneManager.LoadScene(1);
    }

    public void ClickEnterLevelPage()
    {
        SceneManager.LoadScene(2);
    }

    public void ClickEnterTeamPage()
    {
        SceneManager.LoadScene(3);
    }

    public void ClickEnterWebsitePage()
    {
        SceneManager.LoadScene(4);
    }

    public void ClickEnterStudioPage()
    {
        SceneManager.LoadScene(5);
    }

    public void ClickEnterStartPage()
    {
        SceneManager.LoadScene(0);
    }
}
