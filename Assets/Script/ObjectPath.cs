
using UnityEngine;
using TMPro;

public class ObjectPath : MonoBehaviour
{
    public TMP_Text scoreText;

    private static int score = 0;
    private const int MAX_SCORE = 10;

    private void OnTriggerEnter(Collider other)
    {
        // 최대 점수에 도달하면 아무 동작도 하지 않음
        if (score >= MAX_SCORE)
        {
            return;
        }

        // PASS 태그를 가진 벽에 닿았을 때
        if (other.CompareTag("PASS"))
        {
            score++;

            // 점수 UI 갱신
            if (scoreText != null)
            {
                scoreText.text = "OO : " + score;
            }

            // 점수가 10점 미만일 때만 파괴
            Destroy(gameObject);
        }
    }
}
