using UnityEngine;

// 게임 오브젝트를 계속 왼쪽으로 움직이는 스크립트
public class PlayerScroll : MonoBehaviour
{
    public float speed = 8f; // 이동 속도
    private void Update()
    {
        if (!GameManager.instance.isGameover)
        {
            // 초당 speed의 속도로 왼쪽으로 평행 이동
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }
}