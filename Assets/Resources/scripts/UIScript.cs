using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
public class UIScript : MonoBehaviour {


    /*UI OBJECT HIERARCHY VARS*/    
    public GameObject winnerPanel;
    //Recipes panel
    public GameObject current_recipe_title; // not used yet
    public GameObject[] current_recipe_ingredients; // four buttons
    public GameObject title_panel, gameroom_panel, bedroom_panel;
    //Reporting panel

    //Containers panel
    public List<GameObject> containers;
    public List<GameObject> ingredients;
    public List<GameObject> recipes;
    //Winner panel
    public Text highscores;
    
    /*END HIERARCHY*/
    /* GAME OBJECT SCRIPTS*/
    public List<Ingredient> _ingredients;
    public List<Container> _containers;
    public List<Recipe> _recipes;
    public List<Ingredient> cauldron;
    public Recipe current_recipe { get { return _recipes[current_recipe_index]; } }
    public List<Ingredient> current_ingredients;    
    public Text turn_text;
    public Text streak_text;
    public Slider streak_slider;
    public Text time_text;
    public Text recipes_completed_text;
    public Text requests_completed_text;
    public float time_counter;
    public int streak_counter, 
        turn_counter,
        requests_completed_counter, 
        max_recipes, 
        current_recipe_ingredient_index,
        current_recipe_index;
    Coroutine action_taken;
    public bool easymode = false;
    /* END GAME OBJECTS*/


    //NEW SCRIPTS
    private void Awake()
    {
        
        StartCoroutine(Test());
        if (GameObject.FindGameObjectsWithTag("GameController").Length > 1) Destroy(gameObject);            
        DontDestroyOnLoad(gameObject);       
        //OnSceneLoaded is a method defined later on in this file
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    IEnumerator Test()
    {
        string url = "http://mattmilan.com:3000/api/v1/spellbooks.json";
        using (WWW www = new WWW(url))
        {
            yield return www;

            Debug.Log("Test: " + www.text);
        }
    }

    void CreateStage()
    {
        LoadContainers();
        LoadRecipes();        
        GatherIngredientsFromRecipes();
        PlaceIngredientsInContainers(containers.Count);
        PlaceRecipeInPanel();
        LoadCauldron();
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Turn off all menu panels. We will turn on the proper one later
        // We subtract 1 because we childed the eventsystem into the UI
        // the eventsystem is the last child of the root ui, and we want it
        // to stay active; hence the -1.

        // of course there's a better way...
        for (int i = 0; i < gameObject.transform.childCount - 1; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }        
        switch (scene.buildIndex) {
            case 0: title_panel.SetActive(true); break;
            case 1: gameroom_panel.SetActive(true); CreateStage(); max_recipes = 3; break;
            case 2: bedroom_panel.SetActive(true); break;                
        }
    }
    void LoadContainers()
    {
        if (_containers.Count > 0) return;
        _containers = new List<Container>();
        foreach (GameObject g in containers)
        {
            _containers.Add(g.GetComponent<Container>());
        }
        /*GameObject[] find_containers = GameObject.FindGameObjectsWithTag("INGREDIENT_CONTAINER");
        foreach (GameObject g in find_containers)
        {
            containers.Add(g);            
        }*/
    }
    void LoadRecipes()
    {
        recipes = new List<GameObject>();
        
        Object[] newRecipes = Resources.LoadAll("prefabs/recipes");
        foreach (GameObject o in newRecipes)
        {
            recipes.Add(o);
        }
        _recipes = new List<Recipe>();
        
        foreach (GameObject g in recipes)
        {
            _recipes.Add(g.GetComponent<Recipe>());
        }
        //Initialize it here since there's nowwhere better
        current_ingredients = new List<Ingredient>();
    }
    void LoadCauldron() {
        // The cauldron is just a copy of the current recipe
        // the cauldron obeys the recipe rules
        // the cauldron keeps track of how many ingredients have been completed
        // the cauldron checks if the recipe is completed
        // the cauldron drops it's current recipe and grabs the next one from the book
        // the recipes themselves are never altered
        cauldron = new List<Ingredient>();

    }
    void GatherIngredientsFromRecipes()
    {
        
        _ingredients = new List<Ingredient>();
        foreach (Recipe r in _recipes)
        {
            foreach (Ingredient i in r.ingredients)
            {
                _ingredients.Add(i);
            }
        }
        _ingredients = _ingredients.Distinct().ToList();
    }
   
    void PlaceIngredientsInContainers(int howMany)
    {
        // Ings: A copy of the _ingredients list
        // Conts: A copy of the _containers list

        // We make copies because we're going to remove these
        // objects from the lists each time we iterate
        // This ensures unique ingredient placement among
        // the containers

        List<Ingredient> ings = new List<Ingredient>(_ingredients);
        List<Container> conts = new List<Container>(_containers);

        Debug.Log(ings.Count);
        Debug.Log(conts.Count);

        for (int i = 0; i < howMany; i++)
        {
            // Choose an ingredient randomly then remove it from the list            
            // Minus 1 to offset the list index
            int ingredientIndex = Random.Range(0, ings.Count - 1);
            Ingredient ingredient = Instantiate(ings[ingredientIndex]);//Instantiate(_ingredients[unique_ingredients[i]]);
            ings.RemoveAt(ingredientIndex);

            // Choose a container randomly and remove it from the list
            // Minus 1 to offset the list index
            int containerIndex = Random.Range(0, conts.Count - 1);
            Container container = conts[containerIndex];
            conts.RemoveAt(containerIndex);

            // Put the ingrdient in the container
            container.SetIngredient(ingredient);
            ingredient.transform.SetParent(container.transform);
            
            ingredient.transform.localPosition = Vector3.zero;
            ingredient.transform.localScale = Vector3.one * 75;

            // Add option for training/easy/debug mode
            //if (easymode == false) ingredient.GetComponent<RawImage>().color = Color.clear;
        }
    }
    void PlaceRecipeInPanel()
    {
        // Clean up panel of old recipe remnants
        if (current_ingredients.Count > 0)
        {
            foreach (Ingredient i in current_ingredients)
            {
                Destroy(i.gameObject);
            }
            current_ingredients.Clear();
        }
        
        for (int i = 0; i < current_recipe.ingredients.Count; i++)
        {
            Ingredient ing = Instantiate(current_recipe.ingredients[i]);

            ing.transform.SetParent(current_recipe_ingredients[i].transform);
            ing.transform.localPosition = Vector3.zero;
            ing.transform.localScale = Vector3.one * 75;

            current_ingredients.Add(ing);
            //Debug.Log("Ingredient " + i + " in recipe_ingredients: " + ing.name);
        }
    }
    void NextRecipe()
    {
        Debug.Log("Next Recipe!");
        current_recipe_ingredient_index = 0;
        current_recipe_index++;        
        recipes_completed_text.text = "Recipes Completed: " + current_recipe_index;
        if (current_recipe_index >= max_recipes) { Complete(); }
        else PlaceRecipeInPanel();
    }
    //Button Behaviour Scripts
    //1. change container button to show ingredient for 2 seconds
    //2. 
    //public void CheckContainer(Button b)
    //{
    //    //on click, fade in the ingredient, and lock out buttons for ~2 seconds     
    //    /if (action_taken != null) return;
    //    b.GetComponentInChildren<RawImage>().color = Color.white;
    //    action_taken = StartCoroutine(ButtonCheck(b));
        
        
    //}
    public void CheckContainer(Container c)
    {
        if (action_taken != null) return;
        action_taken = StartCoroutine(ButtonCheck(c));
    }
    void AddTurn()
    {
        turn_counter++;
        turn_text.text = turn_counter.ToString();
    }
    void AddStreak()
    {
        streak_counter++;
        streak_text.text = streak_counter.ToString();
        streak_slider.value = streak_counter;
    }
    void ReduceStreak()
    {
        streak_counter = 0;
        streak_text.text = streak_counter.ToString();
        streak_slider.value = streak_counter;
    }
    IEnumerator ButtonCheck(Container c)
    {
        AddTurn();
        //b.GetComponentInChildren<RawImage>().color = Color.white;
        Ingredient button_ingredient = c.ingredient;

        /* Fixed-Order recipe completion */
        if (Match(button_ingredient, CurrentRecipeIngredient()))
        {
            CompleteIngredient(CurrentRecipeIngredient());
            if (IsRecipeComplete()) NextRecipe();
        }
        else ReduceStreak();

        /* Any-Order recipe completion */
        /*foreach (Ingredient i in current_ingredients)
        {
            if (!i.gameObject.activeSelf) { continue; } // skip completed ingredients    
            if (Match(button_ingredient, i)) {
                CompleteIngredient(button_ingredient);
                if (IsRecipeComplete()) NextRecipe();
                return;
            }
            else ReduceStreak();
        }
        */


        float elapsed_time = 0f;
        float duration = (easymode ? .5f : 2f);
        while (elapsed_time < duration)
        {
            elapsed_time += Time.deltaTime;
            yield return null;
        }
        if (easymode == false) c.myButton.image.color = Color.clear;
        action_taken = null;
    }
    IEnumerator ButtonCheck(Button b)
    {
        AddTurn();
        //b.GetComponentInChildren<RawImage>().color = Color.white;
        Ingredient button_ingredient = b.GetComponentInChildren<Ingredient>();

        /* Fixed-Order recipe completion */
        if (Match(button_ingredient, CurrentRecipeIngredient()))
        {            
            CompleteIngredient(CurrentRecipeIngredient());
            if (IsRecipeComplete()) NextRecipe();
        }
        else ReduceStreak();

        /* Any-Order recipe completion */
        /*foreach (Ingredient i in current_ingredients)
        {
            if (!i.gameObject.activeSelf) { continue; } // skip completed ingredients    
            if (Match(button_ingredient, i)) {
                CompleteIngredient(button_ingredient);
                if (IsRecipeComplete()) NextRecipe();
                return;
            }
            else ReduceStreak();
        }
        */


        float elapsed_time = 0f;
        float duration = (easymode ? .5f : 2f);
        while (elapsed_time < duration)
        {
            elapsed_time += Time.deltaTime;
            yield return null;
        }
        if (easymode == false) b.GetComponentInChildren<RawImage>().color = Color.clear;
        action_taken = null;
    }
    Ingredient CurrentRecipeIngredient()
    {
        return current_ingredients[current_recipe_ingredient_index];
    }
    void CompleteIngredient(Ingredient i)
    {
        current_recipe_ingredient_index++;
        i.gameObject.SetActive(false);
        AddStreak();
        
    }
    bool IsRecipeComplete()
    {
        foreach (Ingredient i in current_ingredients)
        {
            if (i.gameObject.activeSelf) return false;
        }        
        return true;
    }
    bool Match(Ingredient a, Ingredient b)
    {
        return (a.name.ToLower() == b.name.ToLower());
    }
    //END NEW SCRIPTS







    private void Update()
    {
        time_counter += Time.deltaTime;
        UpdateTime();
    }


    public void UpdateTime() {
        int minutes = Mathf.RoundToInt(time_counter / 60), seconds = Mathf.RoundToInt(time_counter % 60);
        time_text.text = minutes + ":" + seconds;
    }
    private  void UpdateStreak() { streak_text.text = streak_counter.ToString(); }
    public  void UpdateTurns() {turn_text.text = turn_counter.ToString();

    }


    public void Complete()
    {
        StartCoroutine(DB_Controller.PostScores("TestPlayer", 12347));
        StartCoroutine(DB_Controller.GetScores(highscores));
        winnerPanel.SetActive(true);
    }


    public void Change(int scene)
    {
        SceneManager.LoadScene(scene);
    }
    int[] ShuffledUniqueIntegerArray(int size)
    {
        int[] shuffled = new int[size];
        for (int i = 0; i < size; i++)
        {
            shuffled[i] = i;
        }
        return Shuffle(shuffled);
    }
    int[] Shuffle(int[] input)
    {
        int[] returnValue = (int[])input.Clone();
        int swap = 0;
        int temp = 0;
        int r = 0;
        for (int i = 0; i < returnValue.Length; i++)
        {
            r = Random.Range(0, returnValue.Length);
            temp = returnValue[i];

            swap = returnValue[r];

            returnValue[i] = returnValue[r];

            returnValue[r] = temp;

            //input[i] = Random.Range(0, input.Length);
        }
        string int_out = "Post shuffle: "; foreach (int i in returnValue) { int_out += returnValue[i] + ", "; }
        Debug.Log(int_out);
        return returnValue;
    }
    public void ResetGameRoom()
    {
        current_recipe_index = 0;
        time_counter = 0;
        streak_counter = 0;
        turn_counter = 0;
        streak_slider.value = 0;
        for (int i = 0; i < containers.Count; i++)
        {
            Destroy(containers[i].GetComponentInChildren<Ingredient>().gameObject);
        }
    }
}

