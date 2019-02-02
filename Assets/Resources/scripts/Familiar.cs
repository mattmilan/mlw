using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Familiar : MonoBehaviour
{
    //Coroutine travelling;
    //public GameObject ingredient;
    //public GameObject target;
    ////public Container container;

    //private void Awake(){    }

    ////public void Come(GameObject newTarget, Transform destination) { Come(newTarget, destination, null); }
 
    //public void Come(GameObject newTarget, Transform destination) {
    //    if (target != null) target.GetComponent<Container>().CloseContainer();
    //    target = newTarget;
    //    FaceTarget();
    //    travelling = StartCoroutine(Travel(transform, destination));
    //}
    //public void Deliver(Cauldron c)
    //{
    //    if (target != null) target.GetComponent<Container>().CloseContainer();

    //    //else return; //we dont want to travel from the cauldron to the cauldron
    //    target = c.gameObject;
    //    FaceTarget();
        
    //    target = null;
    //    travelling = StartCoroutine(Travel(transform, c.transform));
    //}
    //void FaceTarget()
    //{
    //    transform.LookAt(target.transform);
    //    //StartCoroutine(FaceDestination(transform, target.transform));
    //}
    ////public GameObject GetTarget() { return target; }
    //public bool TakeIngredient(GameObject i)
    //{
    //    if (ingredient == null)
    //    {
    //        target = null;
    //        ingredient = i;
    //        ingredient.transform.SetParent(transform);
    //        ingredient.SetActive(true);
    //        ingredient.transform.localPosition = new Vector3(0f, -.05f, -.55f);
    //        return true;
    //    }
    //    return false;
    //}
    //public bool Busy() { return travelling != null; }

    ///* <method>
    // *     <name> Travel </name>
    // *     <type> IEnumerator </type>
    // *     <parameters> 
    // *          <Transform> starting - the start position of the travel </Transform>
    // *          <Transform> ending - the end position of the travel</Transform>
    // *     </parameters>
    // *     <return> N/A </return>
    // *     <purpose> Animates the movement of the object to the parameter. Afterwards, triggers the "ShowIngredient" method of the target </purpose>
    // * </method>
    // */
    //IEnumerator Travel(Transform starting, Transform ending)
    //{
    //    Vector3 startPosition = starting.position;
    //    Vector3 endPosition = ending.position;
    //    //if (ingredient != null ) {            
    //        //endPosition = GameController.cauldron.transform.position;
    //    //}

    //    float elapsedTime = 0f;
    //    float duration = .5f;
    //    while (elapsedTime < duration)
    //    {
    //        transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
    //        elapsedTime += Time.deltaTime;

    //        yield return null;
    //    }
    //    travelling = null;
    //    //if (target == null) { GameController.cauldron.GetComponent<Cauldron>().Deliver(ingredient); ingredient = null; }
    //    //else target.GetComponent<Container>().OpenContainer();
    //    GameController.AddTurn();
    //}



    ///* <method>
    //*     <name> FaceDestination </name>
    //*     <type> IEnumerator </type>
    //*     <parameters> Transform t (A transform that we will rotate to face) </parameters>
    //*     <return> N/A </return>
    //*     <purpose> Animates the rotation of the object until it's facing it's target </purpose>
    //* </method>
    //*/
    //IEnumerator FaceDestination(Transform starting, Transform ending)
    //{
    //    Vector3 startRotation = starting.rotation.eulerAngles;
    //    Vector3 endRotation = ending.position - starting.position;
    //    float elapsedTime = 0f;
    //    float duration = .2f;
    //    while (elapsedTime < duration)
    //    {
    //        starting.rotation = Quaternion.Euler(Vector3.Lerp(startRotation, endRotation, elapsedTime / duration));
    //        elapsedTime += Time.deltaTime;
    //        //transform.rotation = Quaternion.LookRotation(newDir);
    //        //yield return new WaitForEndOfFrame();
    //        yield return null;
    //    }
    //}

}
