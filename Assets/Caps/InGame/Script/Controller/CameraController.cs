using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private TestPlayer player; // 추후 플레이어 로직 설정
    private Vector3 targetPos, refVel = Vector3.zero;
    private Camera cam;

    [SerializeField]
    private float zOffset; // 카메라 기본 고정값
    private Vector3 mousePoint;
    [SerializeField]
    private float camDist = 2.0f;
    [SerializeField]
    private float smoothTime = 0.2f;

    PlayerVetor playerVecStatus;

    private Vector2 pointer;
    public Vector2 Pointer => pointer;

    private float playerAngle;
    public float PlayerAngle => playerAngle;

    Vector2 mouseVecValue;

    private static CameraController instance;
    public static CameraController Instance => instance;

    bool isReverse = false;

    private void Awake()
    {
        Init();
        cam = GetComponent<Camera>();

    }

    void Start()
    {
        targetPos = player.transform.position;
        zOffset = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        mousePoint = CheckMousePointer();
        targetPos = UpdateTargetPos();
        UpdateCamPos();
        PlayerPos();
    }

    private void Init()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }

    private Vector3 CheckMousePointer()
    {
        // 마우스 포인트 찾기
        Vector2 refVec = cam.ScreenToViewportPoint(Input.mousePosition) * 2;
        refVec *= 2;
        refVec -= Vector2.one;
        float max = 0.9f;

        // 최대 벡터 길이 1로 설정
        if (Mathf.Abs(refVec.x) > max || Mathf.Abs(refVec.y) > max)
            refVec = refVec.normalized;

        return refVec;
    }

    private Vector3 UpdateTargetPos()
    {
        Vector3 mousePos = mousePoint * camDist;
        Vector3 refVec = player.transform.position + mousePos;
        refVec.z = zOffset;
        return refVec;
    }

    private void UpdateCamPos()
    {
        Vector3 tempPos;
        tempPos = Vector3.SmoothDamp(transform.position, targetPos,
                                    ref refVel, smoothTime);
        transform.position = tempPos;
    }

    // Player 방향 설정
    private void PlayerPos()
    {
        pointer = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseVecValue = pointer - (Vector2)GameManager.Instance.player.transform.localPosition;

        playerAngle = Mathf.Atan2(mouseVecValue.y, mouseVecValue.x) * Mathf.Rad2Deg;

        AngleCalculate(playerAngle);
    }
    /*
    // 방향 설정
    private void AngleCalculate(float angleValue)
    {
        bool isReverse = false;

        // 윗방향
        if (angleValue <= 120 && angleValue > 60)
            playerVecStatus = PlayerVetor.Up;
        // 오른 대각
        else if (angleValue <= 60 && angleValue > 0)
            playerVecStatus = PlayerVetor.UpRight;
        // 오른
        else if (angleValue <= 0 && angleValue > -60)
            playerVecStatus = PlayerVetor.Right;
        // 후면
        else if (angleValue <= -60 && angleValue > -120)
            playerVecStatus = PlayerVetor.Down;
        // 왼쪽(오른쪽에서 뒤집기)
        else if (angleValue <= -120 && angleValue > -180)
            playerVecStatus = PlayerVetor.Right;
        // 왼쪽 대각(오른쪽에서 뒤집기)
        else if (angleValue <= 180 || angleValue > 120)
            playerVecStatus = PlayerVetor.UpRight;


        if (angleValue > 90 || angleValue <= -90) isReverse = true;
        else isReverse = false;

        GameManager.Instance.player.VectorStatus(playerVecStatus);
        GameManager.Instance.player.isReverse = isReverse;
    }*/

    // 방향 설정
    private void AngleCalculate(float angleValue)
    {
        // 후면(윗 방향)
        if (angleValue < 120 && angleValue > 60)
            playerVecStatus = PlayerVetor.Up;
        // 오른 대각
        else if (angleValue <= 60 && angleValue >= 10)
            playerVecStatus = PlayerVetor.UpRight;
        // 오른
        else if (angleValue < 10 && angleValue >= -60)
            playerVecStatus = PlayerVetor.Right;
        // 정면(아랫 방향)
        else if (angleValue < -60 && angleValue > -120)
            playerVecStatus = PlayerVetor.Down;
        // 왼쪽(오른쪽에서 뒤집기)
        else if (angleValue <= -120 || angleValue > 170)
            playerVecStatus = PlayerVetor.Right;
        // 왼쪽 대각(오른쪽에서 뒤집기)
        else if (angleValue <= 170 || angleValue >= 120)
            playerVecStatus = PlayerVetor.UpRight;

        if (!isReverse) // 안 뒤집힌 상태
        {
            if (angleValue >= 105 || angleValue <= -105) isReverse = true;
        }
        else
        {
            if (angleValue >= -75 && angleValue <= 75) isReverse = false;
        }

        Debug.Log(angleValue);

        GameManager.Instance.player.ChancgVector(playerVecStatus, isReverse);
    }
}
