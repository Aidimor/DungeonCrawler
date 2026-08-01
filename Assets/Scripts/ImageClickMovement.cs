using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageClickMovement : MonoBehaviour, IPointerClickHandler
{
    
    public void OnPointerClick(PointerEventData eventData)
    {
        var ScriptMovement = MainController.Instance._scriptMovement;
        if (!ScriptMovement.Moving && MainController.Instance._onStation == 1)
        {
            ScriptMovement.Moving = true;
            //StartCoroutine(ScriptMovement.MoveCharNumerator());         
        }      
        
    }
}