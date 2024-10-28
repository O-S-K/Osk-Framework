using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace OSK
{
public class UIMoveEffect : OSK.SingletonMono<UIMoveEffect>
{
    public Canvas canvas;
    public GameObject[] iconClones;

    public int indexIcon = 0;
    public Vector2 randomX = new Vector2(-30f, 30f);
    public Vector2 randomY = new Vector2(-20f, -30f);

    public float timeDrop = 0.15f;
    public float speedDrop = 1.5f;

    public float timeFly = 1;
    public float speedFly = 3;

    public int numberOfCoins = 10;
    public float delayMove = 0.15f;

    public System.Action onCompleted;
    private GameObject coinParent;

    public Transform pointSpawn;
    public Vector3 startPosition;
    public Transform target;


    private void Start()
    {
        if (canvas == null)
        {
            canvas = Main.UI.GetCanvas;
        }

        for (int i = 0; i < iconClones.Length; i++)
        {
            iconClones[i].SetActive(false);
        }

        coinParent = new GameObject("iconMoveParent");
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         var pC = World.UI.GetScreen<GameplayUI>().GetCoinUI().GetRootCoin();
    //         SpawnImageWithRectTransform(0, transform, pC, () =>
    //         {
    //             Debug.Log("Completed");
    //         });
    //     }
    // }

    public void SpawnImageWithRectTransform(int idx, Transform posInit, Transform posIcon,
        System.Action onCompleted = null)
    {
        new SpawnImageBuilder()
            .SetIndexIcon(idx)
            .SetStartTransform(posInit)
            .SetEndPoint(posIcon)
            .BuildWithTransform();
    }

    public void SpawnImageWithPosition(int idx, Transform posInit, Transform posIcon, System.Action onCompleted = null)
    {
        new SpawnImageBuilder()
            .SetIndexIcon(idx)
            .SetStartPosition(posInit, canvas.GetComponent<RectTransform>())
            .SetEndPoint(posIcon)
            .BuildWithPosition();
    }

    public void SpawnImageWithPosition()
    {
        StartCoroutine(DelaySpawnCoins(startPosition));
    }


    public void SpawnImagesTransform()
    {
        StartCoroutine(DelaySpawnCoins(pointSpawn.position));
    }

    private IEnumerator DelaySpawnCoins(Vector3 startPoint)
    {
        var cp = Main.Pool.Spawn("CoinParent", coinParent);
        cp.transform.parent = canvas.transform;
        cp.transform.localPosition = Vector3.zero;
        cp.transform.localScale = Vector3.one;
        this.DoDelay(delayMove + numberOfCoins / 10, () => { Main.Pool.Despawn(cp); });

        float timeDlaySpawn = 0;

        for (int i = 0; i < numberOfCoins; i++)
        {
            var coinClone = Main.Pool.Spawn("icon", iconClones[indexIcon]);
            coinClone.transform.position = startPoint;
            coinClone.transform.localScale = Vector3.one;
            coinClone.transform.SetParent(cp.transform, false);
            coinClone.transform.localPosition.WithZ(0);
            coinClone.gameObject.SetActive(true);

            if (gameObject.activeInHierarchy)
                StartCoroutine(DropCoins(coinClone, startPoint));

            float randomTime = Random.Range(0.01f, 0.03f);
            timeDlaySpawn += randomTime;
            yield return new WaitForSeconds(randomTime);

            if (gameObject.activeInHierarchy)
                StartCoroutine(MoveCoinToTarget(coinClone.transform));
        }

        onCompleted?.Invoke();
    }

    public bool isDrop = true;

    private IEnumerator DropCoins(GameObject image, Vector3 startPoint)
    {
        float timer = 0f;
        Vector3 randomOffset = new Vector2(Random.Range(randomX.x, randomX.y), Random.Range(randomY.x, randomY.y));
        Vector3 spawnPos = startPoint + randomOffset;

        if (isDrop)
        {
            while (timer < timeDrop)
            {
                float t = timer / speedDrop;
                image.transform.position =
                    Vector3.MoveTowards(image.transform.position, new Vector3(spawnPos.x, spawnPos.y, 0), t);
                timer += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            image.transform.position = spawnPos;
        }

        // AudioManager.Instance.PlayOneShot("coin_collect_appear", 1, 1);   
    }

    public bool isUseJump = true;
    public float jumpPower = 3;
    public int numjump = 1;
    public Ease ease = Ease.Linear;

    private IEnumerator MoveCoinToTarget(Transform imageTransform)
    {
        float timer = 0f;
        yield return new WaitForSeconds(delayMove + Random.Range(0.01f, 0.02f));

        if (isUseJump)
        {
            imageTransform.transform.DOJump(target.position, jumpPower, numjump, timeFly)
                .SetEase(ease).OnComplete(() => { OnCompletedMove(imageTransform.gameObject); });
        }
        else
        {
            imageTransform.DOMove(target.position, timeFly)
                .SetEase(ease).OnComplete(() => { OnCompletedMove(imageTransform.gameObject); });
        }

        yield return null;
    }

    private void OnCompletedMove(GameObject go)
    {
        Main.Pool.Despawn(go);
        Main.Sound.Play("coin_add", false);
        Main.Native.Vibrate(EffectHaptic.Heavy);
    }
}


public class SpawnImageBuilder
{
    private UIMoveEffect _img;

    public SpawnImageBuilder()
    {
        _img = UIMoveEffect.Instance;
    }

    public SpawnImageBuilder SetIndexIcon(int indexIcon)
    {
        _img.indexIcon = indexIcon;
        return this;
    }


    public SpawnImageBuilder SetStartTransform(Transform startPoint)
    {
        _img.pointSpawn = startPoint;
        return this;
    }


    public SpawnImageBuilder SetStartPosition(Transform transform, RectTransform rectTransformCanvas)
    {
        var convertPos = OSK.CanvasUtils.WorldToCanvasPosition(rectTransformCanvas, Camera.main, transform);
        _img.startPosition = convertPos;
        return this;
    }

    public SpawnImageBuilder SetEndPoint(Transform endPoint)
    {
        _img.target = endPoint;
        return this;
    }

    public SpawnImageBuilder SetOnCompleted(System.Action onCompleted)
    {
        _img.onCompleted = onCompleted;
        return this;
    }

    public SpawnImageBuilder SetRandomX(Vector2 randomX)
    {
        _img.randomX = randomX;
        return this;
    }

    public SpawnImageBuilder SetRandomY(Vector2 randomY)
    {
        _img.randomY = randomY;
        return this;
    }

    public SpawnImageBuilder SetTimeDrop(float timeDrop)
    {
        _img.timeDrop = timeDrop;
        return this;
    }

    public SpawnImageBuilder SetSpeedDrop(float speedDrop)
    {
        _img.speedDrop = speedDrop;
        return this;
    }

    public SpawnImageBuilder SetTimeFly(float timeFly)
    {
        _img.timeFly = timeFly;
        return this;
    }

    public SpawnImageBuilder SetSpeedFly(float speedFly)
    {
        _img.speedFly = speedFly;
        return this;
    }

    public SpawnImageBuilder SetNumberOfCoins(int numberOfCoins)
    {
        _img.numberOfCoins = numberOfCoins;
        return this;
    }

    public SpawnImageBuilder SetDelayMove(float delayMove)
    {
        _img.delayMove = delayMove;
        return this;
    }

    public SpawnImageBuilder SetUseJump(bool isUseJump)
    {
        _img.isUseJump = isUseJump;
        return this;
    }


    public void BuildWithTransform()
    {
        _img.SpawnImagesTransform();
    }

    public void BuildWithPosition()
    {
        _img.SpawnImageWithPosition();
    }
}
}