using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Cursor : MonoBehaviour
{
    public enum CursorType { Arrow, Grabbing, Grabbed, Check, Poke,SkillTargeting }
    [SerializeField] private List<CursorAnimation> cursorAnimationList;

    private CursorAnimation cursorAnimation;
    private int currentFrame;
    private float frameTimer;
    private int frameCount;

    public static M_Cursor instance;

    private void Awake()
    {
        if (instance != null) Destroy(this);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        SetActiveCursorState(CursorType.Arrow);
    }

    private void Update()
    {
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f)
        {
            frameTimer += cursorAnimation.frameRate;
            currentFrame = (currentFrame + 1) % frameCount;
            Cursor.SetCursor(cursorAnimation.textureArray[currentFrame], cursorAnimation.offset, CursorMode.Auto);
        }
    }

    public void SetActiveCursorState(CursorType cursorType)
    {
        CursorAnimation animToSet = cursorAnimationList[0];
        switch (cursorType)
        {
            case CursorType.Arrow:
                animToSet = cursorAnimationList[0];
                break;
            case CursorType.Grabbing:
                animToSet = cursorAnimationList[1];
                break;
            case CursorType.Grabbed:
                animToSet = cursorAnimationList[2];
                break;
            case CursorType.Check:
                animToSet = cursorAnimationList[3];
                break;
            case CursorType.Poke:
                animToSet = cursorAnimationList[4];
                break;
            case CursorType.SkillTargeting:
                animToSet = cursorAnimationList[5];
                break;
        }
        cursorAnimation = animToSet;
        currentFrame = 0;
        frameTimer = cursorAnimation.frameRate;
        frameCount = cursorAnimation.textureArray.Length;
    }

    public void EnactiveTargetingLine()
    {

    }

    [System.Serializable]
    public class CursorAnimation
    {
        public CursorType cursorType;
        public Texture2D[] textureArray;
        public float frameRate;
        public Vector2 offset;
    }
}
