using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IGDF
{
    public class M_DDL : MonoBehaviour
    {
        public GameObject pre_dot;
        public Transform ddlMachine;
        public int height;
        public int width;
        private DDLDot[,] ddlDots_Left;
        private DDLDot[,] ddlDots_Right;
        private Dictionary<int, List<int[]>> numberIndexList = new Dictionary<int, List<int[]>>();

        #region Window
        private Transform leftWindow;
        private Transform rightWindow;
        private float horiMoveDistance = 0.66f;
        private float expandTime = 0.3f;
        private float shrinkTime = 0.1f;
        private float expandedLeftX;
        private float shrinkedLeftX;
        private float expandedRightX;
        private float shrinkedRightX;

        private void Start()
        {
            leftWindow = ddlMachine.Find("Window Left");
            rightWindow = ddlMachine.Find("Window Right");
            expandedLeftX = leftWindow.position.x - horiMoveDistance;
            shrinkedLeftX = leftWindow.position.x;
            expandedRightX = rightWindow.position.x + horiMoveDistance;
            shrinkedRightX = rightWindow.position.x;
        }

        public void DDLSlotChangeTo(SlotCondition targetState)
        {
            switch (targetState)
            {
                case SlotCondition.Expanded:
                    leftWindow.DOMoveX(expandedLeftX, expandTime);
                    rightWindow.DOMoveX(expandedRightX, expandTime);
                    break;
                case SlotCondition.Shrinked:
                    leftWindow.DOMoveX(shrinkedLeftX, shrinkTime);
                    rightWindow.DOMoveX(shrinkedRightX, shrinkTime);
                    break;
            }
        }
        #endregion

        #region Dot
        public void CreateDots()
        {
            ddlDots_Left = new DDLDot[width, height];
            ddlDots_Right = new DDLDot[width, height];

            Transform LeftInitialDot = ddlMachine.Find("Window Left").Find("ddlDot UpperLeft");
            Transform rightInitialDot = ddlMachine.Find("Window Right").Find("ddlDot UpperRight");

            ddlDots_Left[0, 0] = new DDLDot(false, LeftInitialDot.position, LeftInitialDot, new int[] { 0, 0 });
            ddlDots_Right[0, 0] = new DDLDot(false, rightInitialDot.position, rightInitialDot, new int[] { 0, 0 });

            InstantiateDots("Left");
            InstantiateDots("Right");

            void InstantiateDots(string location)
            {
                Vector3 dot11Pos = Vector3.zero;
                if (location == "Left") dot11Pos = ddlDots_Left[0, 0].dotPos;
                else dot11Pos = ddlDots_Right[0, 0].dotPos;

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        if (i == 0 && j == 0) continue;
                        else
                        {
                            Vector3 newDotPos = dot11Pos + new Vector3(i * 0.22f, -j * 0.22f, 0);
                            Transform newDotTrans;
                            if (location == "Left")
                            {
                                newDotTrans = Instantiate(pre_dot, newDotPos, Quaternion.identity, LeftInitialDot.parent).transform;
                                newDotTrans.name = "Left" + i + "," + j;
                            }
                            else
                            {
                                newDotTrans = Instantiate(pre_dot, newDotPos, Quaternion.identity, rightInitialDot.parent).transform;
                                newDotTrans.name = "Right" + i + "," + j;
                            }
                            if (location == "Left") ddlDots_Left[i, j] = new DDLDot(false, newDotPos, newDotTrans, new int[] { i, j });
                            else ddlDots_Right[i, j] = new DDLDot(false, newDotPos, newDotTrans, new int[] { i, j });
                        }

                    }
                }
            }
        }

        public void InitializeNumberList()
        {
            for (int i = 0; i < 10; i++)
            {
                List<int[]> newlist = new List<int[]>();
                switch (i)
                {
                    case 0:
                        newlist.Add(new int[] { 0, 0 });
                        newlist.Add(new int[] { 1, 0 });
                        newlist.Add(new int[] { 2, 0 });
                        newlist.Add(new int[] { 0, 1 });
                        newlist.Add(new int[] { 2, 1 });
                        newlist.Add(new int[] { 2, 2 });
                        newlist.Add(new int[] { 0, 2 });
                        newlist.Add(new int[] { 0, 3 });
                        newlist.Add(new int[] { 2, 3 });
                        newlist.Add(new int[] { 0, 4 });
                        newlist.Add(new int[] { 1, 4 });
                        newlist.Add(new int[] { 2, 4 });
                        break;
                    case 1:
                        newlist.Add(new int[] { 1, 0 });
                        newlist.Add(new int[] { 1, 1 });
                        newlist.Add(new int[] { 1, 2 });
                        newlist.Add(new int[] { 1, 3 });
                        newlist.Add(new int[] { 1, 4 });
                        break;
                    case 2:
                        newlist.Add(new int[] { 0, 0 });
                        newlist.Add(new int[] { 1, 0 });
                        newlist.Add(new int[] { 2, 0 });
                        newlist.Add(new int[] { 2, 1 });
                        newlist.Add(new int[] { 2, 2 });
                        newlist.Add(new int[] { 1, 2 });
                        newlist.Add(new int[] { 0, 2 });
                        newlist.Add(new int[] { 0, 3 });
                        newlist.Add(new int[] { 0, 4 });
                        newlist.Add(new int[] { 1, 4 });
                        newlist.Add(new int[] { 2, 4 });
                        break;
                    case 3:
                        newlist.Add(new int[] { 0, 0 });
                        newlist.Add(new int[] { 1, 0 });
                        newlist.Add(new int[] { 2, 0 });
                        newlist.Add(new int[] { 2, 1 });
                        newlist.Add(new int[] { 2, 2 });
                        newlist.Add(new int[] { 1, 2 });
                        newlist.Add(new int[] { 0, 2 });
                        newlist.Add(new int[] { 2, 3 });
                        newlist.Add(new int[] { 0, 4 });
                        newlist.Add(new int[] { 1, 4 });
                        newlist.Add(new int[] { 2, 4 });
                        break;
                    case 4:
                        newlist.Add(new int[] { 0, 0 });
                        newlist.Add(new int[] { 0, 1 });
                        newlist.Add(new int[] { 0, 2 });
                        newlist.Add(new int[] { 1, 2 });
                        newlist.Add(new int[] { 2, 2 });
                        newlist.Add(new int[] { 2, 0 });
                        newlist.Add(new int[] { 2, 1 });
                        newlist.Add(new int[] { 2, 3 });
                        newlist.Add(new int[] { 2, 4 });
                        break;
                    case 5:
                        newlist.Add(new int[] { 0, 0 });
                        newlist.Add(new int[] { 1, 0 });
                        newlist.Add(new int[] { 2, 0 });
                        newlist.Add(new int[] { 0, 1 });
                        newlist.Add(new int[] { 2, 2 });
                        newlist.Add(new int[] { 1, 2 });
                        newlist.Add(new int[] { 0, 2 });
                        newlist.Add(new int[] { 2, 3 });
                        newlist.Add(new int[] { 0, 4 });
                        newlist.Add(new int[] { 1, 4 });
                        newlist.Add(new int[] { 2, 4 });
                        break;
                    case 6:
                        newlist.Add(new int[] { 0, 0 });
                        newlist.Add(new int[] { 1, 0 });
                        newlist.Add(new int[] { 2, 0 });
                        newlist.Add(new int[] { 0, 1 });
                        newlist.Add(new int[] { 2, 2 });
                        newlist.Add(new int[] { 1, 2 });
                        newlist.Add(new int[] { 0, 2 });
                        newlist.Add(new int[] { 0, 3 });
                        newlist.Add(new int[] { 2, 3 });
                        newlist.Add(new int[] { 0, 4 });
                        newlist.Add(new int[] { 1, 4 });
                        newlist.Add(new int[] { 2, 4 });
                        break;
                    case 7:
                        newlist.Add(new int[] { 0, 0 });
                        newlist.Add(new int[] { 1, 0 });
                        newlist.Add(new int[] { 2, 0 });
                        newlist.Add(new int[] { 2, 1 });
                        newlist.Add(new int[] { 2, 2 });
                        newlist.Add(new int[] { 2, 3 });
                        newlist.Add(new int[] { 2, 4 });
                        break;
                    case 8:
                        newlist.Add(new int[] { 0, 0 });
                        newlist.Add(new int[] { 1, 0 });
                        newlist.Add(new int[] { 2, 0 });
                        newlist.Add(new int[] { 0, 1 });
                        newlist.Add(new int[] { 2, 1 });
                        newlist.Add(new int[] { 2, 2 });
                        newlist.Add(new int[] { 1, 2 });
                        newlist.Add(new int[] { 0, 2 });
                        newlist.Add(new int[] { 0, 3 });
                        newlist.Add(new int[] { 2, 3 });
                        newlist.Add(new int[] { 0, 4 });
                        newlist.Add(new int[] { 1, 4 });
                        newlist.Add(new int[] { 2, 4 });
                        break;
                    case 9:
                        newlist.Add(new int[] { 0, 0 });
                        newlist.Add(new int[] { 1, 0 });
                        newlist.Add(new int[] { 2, 0 });
                        newlist.Add(new int[] { 2, 1 });
                        newlist.Add(new int[] { 2, 2 });
                        newlist.Add(new int[] { 2, 3 });
                        newlist.Add(new int[] { 2, 4 });
                        newlist.Add(new int[] { 0, 1 });
                        newlist.Add(new int[] { 0, 2 });
                        newlist.Add(new int[] { 1, 2 });
                        break;
                    default:
                        break;
                }
                numberIndexList.Add(i, newlist);
            }
        }

        public void FlipDot(DDLDot[,] targetDotArray, List<int[]> numberList)
        {
            List<DDLDot> lightList = new List<DDLDot>();
            foreach (var dot in targetDotArray)
            {
                for (int i = 0; i < numberList.Count; i++)
                {
                    if (numberList[i][0] == dot.index[0] && numberList[i][1] == dot.index[1])
                    {
                        lightList.Add(dot);
                        break;
                    }
                }
            }

            foreach (var dot in targetDotArray)
            {
                if (lightList.Contains(dot))
                {
                    dot.isLight = true;
                    dot.dotObj.DORotate(new Vector3(-180, 0, 0), 0.5f);
                }
                else
                {
                    dot.isLight = false;
                    dot.dotObj.DORotate(new Vector3(0, 0, 0), 0.5f);
                }
            }
        }

        public void GetValueChangeDot(int ddlValue)
        {
            if (ddlValue > 9)
            {
                int singleDigit = ddlValue % 10;
                int tensDigit = ddlValue / 10 % 10;
                FlipDot(ddlDots_Left, numberIndexList[tensDigit]);
                FlipDot(ddlDots_Right, numberIndexList[singleDigit]);
            }
            else
            {
                FlipDot(ddlDots_Left, numberIndexList[0]);
                FlipDot(ddlDots_Right, numberIndexList[ddlValue]);
            }
            M_Main.instance.CheckDevCircumstance();
        }

        private void MachineNumberTester()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                FlipDot(ddlDots_Left, numberIndexList[0]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                FlipDot(ddlDots_Left, numberIndexList[1]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                FlipDot(ddlDots_Left, numberIndexList[2]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                FlipDot(ddlDots_Left, numberIndexList[3]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                FlipDot(ddlDots_Left, numberIndexList[4]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                FlipDot(ddlDots_Left, numberIndexList[5]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                FlipDot(ddlDots_Left, numberIndexList[6]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                FlipDot(ddlDots_Left, numberIndexList[7]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                FlipDot(ddlDots_Left, numberIndexList[8]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                FlipDot(ddlDots_Left, numberIndexList[9]);
            }
        }
        #endregion

        public void UpdateName()
        {
            if (M_Global.instance.GetLanguage() == SystemLanguage.Chinese)
            {
                ddlMachine.Find("T_DDL").GetComponent<TMPro.TMP_Text>().text = "死线";
                ddlMachine.parent.Find("Skill Board").Find("T_Skill").GetComponent<TMPro.TMP_Text>().text = "技能";
                ddlMachine.parent.Find("Residue Card Counter").Find("T_ResidueTitle").GetComponent<TMPro.TMP_Text>().text = "剩余任务";
            }
            else 
            {
                ddlMachine.Find("T_DDL").GetComponent<TMPro.TMP_Text>().text = "DDL";
                ddlMachine.parent.Find("Skill Board").Find("T_Skill").GetComponent<TMPro.TMP_Text>().text = "Skill";
                ddlMachine.parent.Find("Residue Card Counter").Find("T_ResidueTitle").GetComponent<TMPro.TMP_Text>().text = "Residue Task Number";
            }
        }
    }

    public class DDLDot
    {
        public bool isLight;
        public Vector3 dotPos;
        public Transform dotObj;
        public int[] index;

        public DDLDot(bool isLight, Vector3 dotPos, Transform dotObj, int[] index)
        {
            this.isLight = isLight;
            this.dotPos = dotPos;
            this.dotObj = dotObj;
            this.index = index;
        }
    }
}
