using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EightDirectionSprites", menuName = "EightDirectionSprites")]
public class EightDirectionSprites : ScriptableObject
{
    public List<Sprite> SpritesN;
    public List<Sprite> SpritesNE;
    public List<Sprite> SpritesE;
    public List<Sprite> SpritesSE;
    public List<Sprite> SpritesS;
    public List<Sprite> SpritesSW;
    public List<Sprite> SpritesW;
    public List<Sprite> SpritesNW;
}
