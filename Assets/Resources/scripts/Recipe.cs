using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour {
    public List<Ingredient> ingredients;
    public ParticleSystem recipeResult;
    // Use this for initialization

    public Recipe(Ingredient[] newIngredients) {                
        SetIngredients(newIngredients);
    }
    void SetIngredients(Ingredient[] newIngredients)
    {
        
        ingredients = new List<Ingredient>();
        for (int i = 0; i < GameController.RecipeSize; i++) { 
            ingredients.Add(newIngredients[i]);
            Debug.Log("New recipe, adding ingredient... " + ingredients[i]);
        }        
    }
    public bool Match(Ingredient i)
    {
        return i.name.ToLower() == ingredients[0].name.ToLower();
    }

    public Ingredient[] GetIngredients()
    {
        return ingredients.ToArray();
    }
}