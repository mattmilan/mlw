using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    

    public GameObject myIngredient, ingredient;
    public bool showing { get; private set; }
    public Transform familiarPosition;
    public static int turns;
    //void Start() { RefreshIngredient(); familiarPosition = transform.GetChild(0); }
    public void SetIngredient(GameObject g)
    {
        myIngredient = g;
        myIngredient.SetActive(false);
    }
    void CallFamiliar()
    {
        //if (GameController.familiar.Busy()) return false;
        GameController.familiar.Come(gameObject, familiarPosition);
    }
    public void OpenContainer() { showing = true; }// GameController.
    public void CloseContainer() { showing = false; ContainerDim();}

    void ContainerGlow() {
        
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        float intensity = .5f;
        DynamicGI.SetEmissive(renderer, new Color(1f, 0.1f, 0.5f, 1.0f) * intensity); 
        
    }
    void ContainerDim() {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        float intensity = 0f;
        DynamicGI.SetEmissive(renderer, new Color(1f, 0.1f, 0.5f, 1.0f) * intensity);        
    }


    void RefreshIngredient()
    {
        //if (myIngredient == null) { gameObject.SetActive(false); return; }
        ingredient = Instantiate(myIngredient);
        ingredient.transform.SetParent(transform);
        ingredient.transform.localRotation = transform.rotation;
        ingredient.gameObject.SetActive(false);
    }
    void GiveIngredient()
    {
        if (GameController.familiar.TakeIngredient(ingredient))
        {
            CloseContainer();
            RefreshIngredient();

        }
        else Debug.Log("Give ingredient failed");
    }
    void OnMouseDown()
    {
        if (GameController.familiar.Busy()) { Debug.Log("Container Clicked, Familiar was busy"); return; }
        else if (showing) { GiveIngredient(); Debug.Log("Container Clicked, try Give Ingredient"); }
        else { CallFamiliar(); Debug.Log("Container Clicked, try call familiar"); }
        turns++;
    }
}
