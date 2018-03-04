using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    /* 
     Welcome to Mama's little Witch [working title]

     This game is a mutation of the classic "Memory" card game
     In memory, many pairs of cards are placed face-down and two players take 
     turns matching pairs by  flipping 2 cards per turn and memorizing the 
     positions of cards as they play.

     This game is a single player version with one significant change:
     Instead of matching pairs, we're matching a list of values to a number of
     containers that hold one of the values on the list.

     While the list is visible, the contents of the containers is unknown until
     the player first checks the container.

     The player will need to remember which item is in which container, as the
     Stages will be driven by time limits or "move" limits

     Features:

     Stage: Each stage will have a set of containers. 

     Container: More containers are unlocked as the player progresses through the stage

     Ingredient: Ingredients are hidden throughout the stage containers.
    
     Familiar: As the little witch is busy stirring the cauldron and reading the spellbook, 
               her familiar must find and gather the ingredients for her. They have different
               attributes, such as gathering speed, multiple-item carrying, and more.

     Spells: As recipes are completed, some nice particle effects will happen, such as rainbow-dust
             bursts, the score meter showing a rainbow bar if no mistakes are made for a while, and
             other confidence-boosting rewards.
    
             If mistakes are made, the same will happen; slime-burst, raincloud, sour meter, and more.

     Studying: The little witch can study to do stuff
    */

    /* Activities
     * 
     * Start off gathering and mixing ingredients to make the spells you want or need
     * The need comes from:
     *  -animal friends who want help or amusement
     *  -animal enemies who disturb and bother you, like flies and aggressive birds and stuff
     *  - 
     * 
     * 
     * 
     */
    public static UIScript ui;
    public static int NumRecipes;
    public static int NumIngredients;
    public static int RecipeSize;
    public static int NumContainers;

    public static int current_recipe_index = 0;
    public static int current_ingredient_index = 0;
    public static int turns;
    public static int streak;
    public static float elapsedTime;

    public static Familiar familiar;
    public static GameObject cauldron;
    public static List<Recipe> recipes;
    public static List<Ingredient> ingredients;
    public static Container[] containers;

    // Sort of isometric camera, with perspective
    //private Vector3 camera_rotation = new Vector3(-17.99f, 45f, -6.323f);
    //private Vector3 camera_position = new Vector3(-8.323f, 5.777f, -6.323f);




    private void Awake()
    {
        
        //We need this forever
        DontDestroyOnLoad(gameObject);
        GameObject duplicate = GameObject.FindObjectOfType<UIScript>().gameObject;
        if (duplicate != null) Destroy(this);
        //How hard? how many recipes, how many ingredients, how many containers, how many turns, etc
        StartCoroutine(GameTimer());
        
        
    }

    void SetStageVariables()
    {
        NumRecipes = 2;
        RecipeSize = 3;
        NumIngredients = 3;
        NumContainers = NumIngredients;

    }

    //Call all methods needed to set the stage
    private void BuildStage()
    {
        cauldron = GameObject.FindGameObjectWithTag("Cauldron");
        SetStageVariables();
        LoadIngredients();
        MakeRecipes();
        SetFamiliar();

    }

    //Get some nice ingredient assets loaded and save in array
    void LoadIngredients()
    {
        //Limit loading of ingredients to num_ingredients
        // later, limit containers to that amount
        //GameObject[] loadIngredients = (GameObject[]) Resources.LoadAll("Ingredients", typeof (GameObject));
        ingredients = new List<Ingredient>();
        Object[] loadIngredients = Resources.LoadAll("Ingredients", typeof(GameObject));
        foreach (Object o in loadIngredients) { Debug.Log(o.name); }
        for (int i = 0; i < NumIngredients; i++)
        {
            GameObject g = (GameObject)loadIngredients[i] as GameObject;
            ingredients.Add(g.GetComponent<Ingredient>());
            //Debug.Log("Added " + g.name + " [" + i + "]");

        }
    }

    //Pull some nice ingredients from the array and build random recipes, repeats allowed    
    void MakeRecipes()
    {
        recipes = new List<Recipe>();
        for (int i = 0; i < NumRecipes; i++)
        {
            recipes.Add(new Recipe(ingredients.ToArray()));
        }


    }

    /* Shuffle 
     * 
     * Takes an array of ints, uses pitiful Random.range, and reogranizes them
     * Needs to be re-coded with a more organic shuffling
     * 
     * Primarily used to place ingredients randomly in the containers
     * without 
     */



    /* GetContainers
     * 
     * Our scene will start with many containers
     * We will find them, count them, and store them in an array
     * Later, we will add ingredients uniquely to each container
     */
    
    /* Place Ingredients
     * 
     * Our game controller will load some ingredients
     * We will store each one, uniquely, in a random container
     * Later, we will use these ingredients to create recipes
     */

    
    /* SetFamiliar
     * 
     * Finds the familiar in the scene, by tag
     * 
     */

    void SetFamiliar()
    {
        GameObject f = GameObject.FindGameObjectWithTag("Familiar");
        //Debug.Log("F: " + f.name);
        familiar = f.GetComponent<Familiar>();
        //Debug.Log("familiar: " + familiar.name);
    }

    //public static GameObject GetCurrentRecipe() { return recipes[current_recipe_index].gameObject; }
    public static Recipe GetCurrentRecipe() { return recipes[current_recipe_index]; }
    //Orchestration

    public static bool CheckRecipe(GameObject g)
    {
        Recipe r = recipes[current_recipe_index];
        Ingredient ii = r.ingredients[current_ingredient_index];
        if (ii.name == g.name)
        {
            Debug.Log("Ingredient Matched");
            current_ingredient_index++;
            Debug.Log("Current ingredient index: " + current_ingredient_index + " ingredients count: " + r.ingredients.Count);
            if (current_ingredient_index >= r.ingredients.Count)
            {
                Debug.Log("Recipe Complete, " + NumRecipes + " total, " + current_recipe_index + " complete");
                current_recipe_index++;
                current_ingredient_index = 0;
                if (current_recipe_index >= NumRecipes)
                {
                    // Winner!
                    // Do animation, calculate score
                    // Open menu
                }
                else
                {
                    //ui.UpdateRecipe();// update UI to animate the next recipe page and hide the last one
                }
            }
            else
            {
                // update UI to X the last ingredient and highlight the next one
                //ui.UpdateIngredients(g);
            }
            return true;
        }
        Debug.Log("Wrong Ingredient Delivered (brought " + g.name + ", needed " + ii.name);
        return false;
    }
    public static void UpdateUIIngredient() { }
    public static void YouWin() { }
    public IEnumerator GameTimer()
    {
        while (true)
        {
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(1);
            ui.UpdateTime();
        }

    }
    public static void AddTurn()
    {
        turns++;
        ui.UpdateTurns();
    }
}
