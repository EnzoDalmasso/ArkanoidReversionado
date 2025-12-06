using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour
{
    [SerializeField] private BrickData data;
    private int currentHits = 0;

    private SpriteRenderer srBrick;
    private JuegoMenuManager gameManager;

    private bool isBreaking = false;

    private BrickManager brickManager;
    private void Start()
    {
        gameManager = FindFirstObjectByType<JuegoMenuManager>();
        srBrick = GetComponent<SpriteRenderer>();
        brickManager = GetComponentInParent<BrickManager>();

        if (data.damageSprites.Length > 0)
        {
            srBrick.sprite = data.damageSprites[0];
        }
            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;
        if (isBreaking) return;

        currentHits++;

        if (currentHits >= data.hitsToBreak)
        {
            StartCoroutine(BreakSequence());
        }
        else
        {
            UpdateVisualDamage();
        }
    }

    private void UpdateVisualDamage()
    {
        int index = currentHits;

        if (data.damageSprites.Length > index)
            srBrick.sprite = data.damageSprites[index];
    }

    private IEnumerator BreakSequence()
    {
        isBreaking = true;

        //Mostrar sprites de destrucción
        for (int i = 0; i < data.breakSprites.Length; i++)
        {
            srBrick.sprite = data.breakSprites[i];
            yield return new WaitForSeconds(0.05f); // velocidad de la animación
        }
        gameManager.SumarPuntos(data.score);

        brickManager.BrickDestroyed();
        Destroy(gameObject);
    }


}
