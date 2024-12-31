using DG.Tweening;
using System.Collections;
using UnityEngine;
public class CheckPush : MonoBehaviour
{
    [Header("Move Item")]
    [SerializeField] GameObject Camera1;
    [SerializeField] GameObject PlayerArea;
    [SerializeField] GameObject TopWall;
    [SerializeField] GameObject BotWall;
    [SerializeField] Camera mainCamera;

    [Header("Direct Item")]
    [SerializeField] GameObject _startGameCameraLocation;
    [SerializeField] GameObject FirstLine;
    [SerializeField] float saveLastPosYLastBall;
    private bool checkUpdate = true;
    [SerializeField] GameObject endLocationPlayer;
    [SerializeField] Vector3 bottomCam;
    [SerializeField] Vector3 topCam;

    #region Dotween Parameter
    [Header("Move Settings")]
    [SerializeField] float MoveHeight;
    [SerializeField] float moveCameraDuration;
    [SerializeField] Ease moveCameraEase;
    [SerializeField] float movePlayerDuration;
    [SerializeField] Ease movePlayerEase;
    [SerializeField] float moveCamAfterShoot;
    [SerializeField] Ease moveCamAfterShootBall;
    [SerializeField] bool isGoThrought;
    [SerializeField] float rangePlayerAreaToCam;
    #endregion

    [Header("Check Push Settings")]
    public Vector2 boxSize;
    public LevelField levelField;
    public Vector2 saveLastPosY;
    public Vector2 saveTemp;
    public Trajectory trajectory;
    public Shooter shooter;
    public bool firstMoveDone;
    public bool isTutorialEnable = false;

    private void Awake()
    {
        GameplayEvent.OnAllFieldActionsEnd.AddListener(CallCorroutineMethod);
        BossEvent.OnPlayerShootBallEnd.AddListener(CallCorroutineMethod);
    }
    private void Start()
    {
        shooter = FindObjectOfType<Shooter>();
        isGoThrought = false;
        SetPushSettings();
        firstMoveDone = false;
    }

    private void OnDestroy()
    {
        GameplayEvent.OnAllFieldActionsEnd.RemoveListener(CallCorroutineMethod);
        BossEvent.OnPlayerShootBallEnd.RemoveListener(CallCorroutineMethod);
    }
    private void Update()
    {
        if (!checkUpdate) return;
        if (levelField.FirstBallPos != null)
        {
            GetBottomCameraAndSet();
            checkUpdate = false;

        }
    }
    public void GetBottomCameraAndSet()
    {
        float targetY = levelField.FirstBallPos.transform.position.y;
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraBottomOffset = cameraHeight / 2f;
        Vector3 newCameraPosition = mainCamera.transform.position;

        newCameraPosition.y = targetY + cameraBottomOffset;
        mainCamera.transform.position = newCameraPosition;
        StartCoroutine(MoveCam());
        mainCamera = Camera1.GetComponent<Camera>();
    }
    public bool GetTopCam()
    {
        topCam = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, mainCamera.nearClipPlane));
        float topY = topCam.y;
        if (topY >= TopWall.transform.position.y)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void SetPushSettings()
    {
        boxSize = transform.localScale;
        trajectory.canShowLine = true;
    }
    public string CheckDeviceType()
    {
        Camera mainCamera = Camera.main;

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float screenAspectRatio = screenWidth / screenHeight;
        if (screenAspectRatio > 0.7f)
        {
            //Ipad
            return "Ipad";
        }
        else if (screenAspectRatio > 0.5f && screenAspectRatio < 0.7f)
        {
            //iphone8
            return "Ip8";
        }
        else
        {
            //other iphone
            return "Ip";
        }
    }
    public IEnumerator MoveCam()
    {
        shooter.LockCatapult();
        yield return new WaitForSeconds(0.5f);
        Vector2 vector2 = levelField.LastBallPos.transform.position;
        saveLastPosY = levelField.LastBallPos.transform.position;
        vector2 = new Vector2(vector2.x, vector2.y - 3f);
        Vector2 moveTopWallPos = levelField.FirstBallPos.transform.position;
        moveTopWallPos = new Vector2(moveTopWallPos.x, moveTopWallPos.y + 2.3f);
        TopWall.transform.DOMoveY(moveTopWallPos.y, 0.5f).SetEase(Ease.Linear);
        Camera1.transform.DOMoveY(vector2.y, moveCameraDuration)
            .SetEase(moveCameraEase).OnComplete(() =>
            {
                GetTopCam();
                saveLastPosYLastBall = levelField.LastBallPos.transform.position.y - Camera1.transform.position.y;
                Vector3 viewportPosition = Camera1.transform.position;


                float newYPosition = 0f;
                switch (CheckDeviceType())
                {
                    case "Ipad":
                        newYPosition = viewportPosition.y - 8.58f;
                        break;
                    case "Ip8":
                        newYPosition = viewportPosition.y - 13.59f;
                        break;
                    case "Ip":
                        newYPosition = viewportPosition.y - 17.8f;
                        break;
                }
                PlayerArea.transform.DOMoveY(newYPosition, 0.8f)
                .SetEase(Ease.Linear).OnComplete(() =>
                {
                    if (!firstMoveDone)
                    {
                        firstMoveDone = true;
                        GameplayEvent.OnGameFieldSetupDone.Invoke();
                    } 

                    MoveBottomWall(); 
                    TryUnlockCatapult();
                    GameplayEvent.OnMoveGameObjectDone.Invoke();
                    rangePlayerAreaToCam = Mathf.Abs(PlayerArea.transform.position.y - Camera1.transform.position.y);
                });
            });

        saveTemp = saveLastPosY;
    }
    public void MoveBottomWall()
    {
        BotWall.transform.position = new Vector2(BotWall.transform.position.x, PlayerArea.transform.position.y - 8f);
    }
    public void CallCorroutineMethod()
    {
        StartCoroutine(MoveGameObject());
    }
    public IEnumerator MoveGameObject()
    {
        shooter.LockCatapult();
        yield return new WaitForSeconds(0.1f);
        MoveElementToLastBall();
    }
    public void MoveElementToLastBall()
    {
        saveLastPosY = levelField.LastBallPos.transform.position;
        MoveHeight = Mathf.Abs(saveTemp.y - saveLastPosY.y);
        shooter.LockCatapult();
        if (Mathf.Abs((levelField.FirstBallPos.transform.position.y + 5f) - topCam.y) - MoveHeight <= 0)
        {
            isGoThrought = true;
        }
        CalulatorTopCamPos(saveTemp, saveLastPosY);
        if (!GetTopCam())
        {
            TryUnlockCatapult();
        }
        if (saveTemp.y > saveLastPosY.y)
        {
            Vector2 moveTopWallPos = levelField.FirstBallPos.transform.position;
            moveTopWallPos = new Vector2(moveTopWallPos.x, moveTopWallPos.y + 4.6f);
            TopWall.transform.DOMoveY(moveTopWallPos.y, 0.5f).SetEase(Ease.Linear);

        }
        else
        {
            if (!isGoThrought)
            {
                Vector2 moveTopWallPos = levelField.FirstBallPos.transform.position;
                moveTopWallPos = new Vector2(moveTopWallPos.x, moveTopWallPos.y + 4.6f);
                TopWall.transform.DOMoveY(moveTopWallPos.y, 0.5f).SetEase(Ease.Linear);
            }
            else
            {
                Vector2 moveTopWallPos = levelField.FirstBallPos.transform.position;
                moveTopWallPos = new Vector2(moveTopWallPos.x, moveTopWallPos.y + 4.6f);
                TopWall.transform.DOMoveY(moveTopWallPos.y, 0.5f).SetEase(Ease.Linear);
            }

        }
        saveTemp = saveLastPosY;
    }
    public void CalulatorTopCamPos(Vector2 temp, Vector2 current)
    {
        if (temp.y >= current.y)
        {
            float NextCamMove = mainCamera.transform.position.y - MoveHeight;

            float currentVal = levelField.LastBallPos.transform.position.y - mainCamera.transform.position.y;
            if (currentVal < saveLastPosYLastBall)
            {
                mainCamera.transform.DOMoveY(mainCamera.transform.position.y - MoveHeight, moveCamAfterShoot)
                .SetEase(moveCamAfterShootBall).OnComplete(() =>
                {
                    GetTopCam();
                });
                PlayerArea.transform.DOMoveY(NextCamMove - rangePlayerAreaToCam, moveCamAfterShoot)
                .SetEase(moveCamAfterShootBall).OnComplete(() =>
                {
                    TryUnlockCatapult();
                    MoveBottomWall();
                });
            }
            else
            {
                TryUnlockCatapult();
            }
        }
        else
        {
            if (isGoThrought)
            {
                float cameraHeight = mainCamera.orthographicSize;

                Vector3 topCam = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 1f, mainCamera.nearClipPlane));

                topCam.y = levelField.FirstBallPos.transform.position.y;

                Vector3 newCameraPosition = mainCamera.transform.position;
                newCameraPosition.y = topCam.y - cameraHeight + 8f;

                if (mainCamera.transform.position.y != newCameraPosition.y)
                {
                    mainCamera.transform.DOMoveY(newCameraPosition.y, 0.5f).SetEase(moveCamAfterShootBall);
                    float NextCamMove = newCameraPosition.y;
                    PlayerArea.transform.DOMoveY(NextCamMove - rangePlayerAreaToCam, moveCamAfterShoot)
                                .SetEase(moveCamAfterShootBall).OnComplete(() =>
                                {
                                    TryUnlockCatapult();
                                    MoveBottomWall();
                                });
                }
            }
            else
            {
                float NextCamMove = Camera1.transform.position.y + MoveHeight;
                Camera1.transform.DOMoveY(Camera1.transform.position.y + MoveHeight, moveCamAfterShoot)
                    .SetEase(moveCamAfterShootBall).OnComplete(() =>
                    {
                        GetTopCam();

                    });
                PlayerArea.transform.DOMoveY(NextCamMove - rangePlayerAreaToCam, moveCamAfterShoot)
                    .SetEase(moveCamAfterShootBall).OnComplete(() =>
                    {
                        TryUnlockCatapult();
                        MoveBottomWall();
                    });
            }
        }
    }

    private void TryUnlockCatapult()
    {
        if (LevelDataHolder.LevelData.IsBossLevel)
        {
            GameStateManager gameStateManager = GameStateManager.Instance;

            if (gameStateManager.CurrentState != gameStateManager.playerShootState)
            {
                return;
            }
        }

        if (isTutorialEnable) return;

        shooter.UnlockCatapult();
    }
}
