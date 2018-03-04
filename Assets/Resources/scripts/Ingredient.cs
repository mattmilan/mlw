using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    // An ingredient has a number of attributes

    public enum CATEGORY { PLANT, ANIMAL, MINERAL, MAGICAL }
    public enum LOCATION { FOREST, RIVER, SHRINE, TOWN, MOUNTAIN, SKY }
    public enum ELEMENT { SUN, MOON, TIME, SPACE }
    public enum PROPERTY { GROW, SHRINK, HIDE, SHOW, FIND, CHANGE }

    public CATEGORY category { get; private set; }
    public LOCATION location { get; private set; }
    public ELEMENT element { get; private set; }
    public List<PROPERTY> properties { get; private set; }
    public Sprite mySprite { get; private set; }
    private void Awake()
    {
        TrimCloneFrom(name);
    }
    public static Ingredient Random()
    {
        return new Ingredient();
    }
    public Ingredient()
    {
        //int[] test = { 0, 1, 2, 3, 4, 5 };
        //new Ingredient("default", new Sprite(), test);
        Init();
    }
    public Ingredient(string name, Sprite newSprite, params int[] p)
    {
        Init();

    }
    //For now, a simple list of ingredients
    void Init()
    {
        properties = new List<PROPERTY>();
        
        Mushroom();
    }
    void TrimCloneFrom(string s)
    {
        name = s.Split('(')[0];
    }

    // The following presets should get stuffed in a data structure or something
    void Mushroom() { location = LOCATION.FOREST; }
    void Kelp() { location = LOCATION.RIVER; }
    void Moss() { location = LOCATION.SHRINE; }
    void Fern() { location = LOCATION.TOWN; }
    void Clover() { location = LOCATION.MOUNTAIN; }
    void Dandelion() { location = LOCATION.SKY; }

}
