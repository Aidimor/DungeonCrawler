using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    public Vector2 _startPos;
    private Coroutine _movementCoroutine;

    [Header("Zonas de Destino")]
    public float _dropRadius = 150f;     // Distancia máxima para considerar que está "cerca" o arriba

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        _startPos = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        if (_movementCoroutine != null)
        {
            StopCoroutine(_movementCoroutine);
            _movementCoroutine = null;
        }

        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }

        if (rectTransform != null && canvas != null)
        {
            float scaleFactor = canvas.scaleFactor > 0f ? canvas.scaleFactor : 1f;
            rectTransform.anchoredPosition += eventData.delta / scaleFactor;
        }

        MainController.Instance._scriptFusion._cardObjectSelected = this.gameObject;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }

        bool droppedOnTarget = false;
        RectTransform targetObject = null;

        // Comprobamos si está cerca del Objeto 1
        var fusionScript = MainController.Instance._scriptFusion;
        if (fusionScript._targetsInfo[0]._targetObject != null && Vector2.Distance(rectTransform.position, fusionScript._targetsInfo[0]._targetObject.position) <= _dropRadius && !fusionScript._targetsInfo[0]._cardSet)
        {
            Debug.Log("¡Carta soltada en / cerca del Objeto 1!");
            targetObject = fusionScript._targetsInfo[0]._targetObject;
            fusionScript._targetsInfo[0]._cardObject = this.gameObject;
            fusionScript._targetsInfo[0]._attack = GetComponent<CardInfoScript>()._attack;
            fusionScript._targetsInfo[0]._defense = GetComponent<CardInfoScript>()._defense;
            fusionScript._targetsInfo[0]._cardSet = true;
            fusionScript._targetsInfo[0]._cardId = GetComponent<CardInfoScript>()._id;
            droppedOnTarget = true;
        }
        // Comprobamos si está cerca del Objeto 2
        else if (fusionScript._targetsInfo[1]._targetObject != null && Vector2.Distance(rectTransform.position, fusionScript._targetsInfo[1]._targetObject.position) <= _dropRadius && !fusionScript._targetsInfo[1]._cardSet)
        {
            Debug.Log("¡Carta soltada en / cerca del Objeto 2!");
            targetObject = fusionScript._targetsInfo[1]._targetObject;
            fusionScript._targetsInfo[1]._cardObject = this.gameObject;
            fusionScript._targetsInfo[1]._attack = GetComponent<CardInfoScript>()._attack;
            fusionScript._targetsInfo[1]._defense = GetComponent<CardInfoScript>()._defense;
            fusionScript._targetsInfo[1]._cardSet = true;
            fusionScript._targetsInfo[1]._cardId = GetComponent<CardInfoScript>()._id;
            droppedOnTarget = true;
        }

        // Si cayó en un blanco, hacemos lerp hacia él; si no, regresa a la posición inicial
        if (droppedOnTarget && targetObject != null)
        {
            _movementCoroutine = StartCoroutine(MoveToTargetRoutine(targetObject));
        }
        else
        {
            _movementCoroutine = StartCoroutine(ReturnToStartPosRoutine());
        }
    }

    private IEnumerator MoveToTargetRoutine(RectTransform target)
    {
        float elapsedTime = 0f;
        float duration = 0.15f;
        Vector2 startPos = rectTransform.anchoredPosition;

        // Convertimos la posición mundial del target al espacio local del Canvas padre de nuestra carta
        // Para asegurar que coincida perfectamente con el anchoredPosition
        Vector2 targetLocalPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            target.position,
            canvas.worldCamera,
            out targetLocalPos
        );

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetLocalPos, t);
            yield return null;
        }

        rectTransform.anchoredPosition = targetLocalPos;
        _movementCoroutine = null;
    }

    private IEnumerator ReturnToStartPosRoutine()
    {
        var fusionScript = MainController.Instance._scriptFusion;

        // Validamos si esta carta ya estaba fija en el Objeto 1 o 2 para limpiarla
        if (fusionScript._targetsInfo[0]._cardObject == this.gameObject)
        {
            Debug.Log("La carta estaba fijada en el Objetivo 1 y ha sido removida de su posición.");
            fusionScript._targetsInfo[0]._cardSet = false;
            fusionScript._targetsInfo[0]._cardObject = null;
            fusionScript._targetsInfo[0]._attack = 0;
            fusionScript._targetsInfo[0]._defense = 0;
            fusionScript._targetsInfo[0]._cardId = 0;
        }
        else if (fusionScript._targetsInfo[1]._cardObject == this.gameObject)
        {
            Debug.Log("La carta estaba fijada en el Objetivo 2 y ha sido removida de su posición.");
            fusionScript._targetsInfo[1]._cardSet = false;
            fusionScript._targetsInfo[1]._cardObject = null;
            fusionScript._targetsInfo[1]._attack = 0;
            fusionScript._targetsInfo[1]._defense = 0;
            fusionScript._targetsInfo[1]._cardId = 0;
        }

        float elapsedTime = 0f;
        float duration = 0.15f;
        Vector2 currentPos = rectTransform.anchoredPosition;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            rectTransform.anchoredPosition = Vector2.Lerp(currentPos, _startPos, t);
            yield return null;
        }

        rectTransform.anchoredPosition = _startPos;
        _movementCoroutine = null;
    }
}