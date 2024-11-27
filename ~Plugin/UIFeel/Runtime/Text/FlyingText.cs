 
using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
public class FlyingText : MonoBehaviour
{
    private Text messess;
    private float alpha = 1f;
    private float speed = 1f;
    private float duration = 2f;
    private Vector2 startScale;
    private Vector2 randomPosX;
    
    private void Awake()
    {
        messess = GetComponent<Text>();
        startScale = messess.transform.localScale;
    }

    public void Init(string text,float spe, float dur)
    {
        this.speed = spe;
        duration = dur;
        messess.text = text;
        randomPosX.x = Random.Range(-1f, 1f);
    }

    private void Update()
    {
        if (alpha > 0)
        {
            // change the y position
            Vector3 pos = transform.position;
            pos.y += speed * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, 
                new Vector3(randomPosX.x, pos.y, pos.z), Random.Range(0.5f, 0.9f));

            // change alpha value
            alpha -= Time.deltaTime / duration;

            Color color = messess.color;
            color.a = alpha;
            messess.color = color;

            messess.transform.localScale = startScale * (0.5f + 0.5f * alpha);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
}