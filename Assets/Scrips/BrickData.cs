using UnityEngine;

[CreateAssetMenu(fileName = "BrickData", menuName = "Scriptable Objects/BrickData")]
public class BrickData : ScriptableObject
{
    [Header("Vida")]
    public int hitsToBreak = 1; //golpes necesarios

    [Header("Feedback visual")]
    public Sprite[] damageSprites; //Array de imagenes
    //public Color brickColor = Color.white;

    [Header("Puntuación")]
    public int score = 50;

    [Header("Sprites de Destrucción")]
    public Sprite[] breakSprites;
}
