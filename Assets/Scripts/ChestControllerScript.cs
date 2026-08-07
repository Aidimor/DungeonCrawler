using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ChestControllerScript : MonoBehaviour
{
    public GameObject _chestPrefab;
    public GameObject _currentChest;

    [System.Serializable]
    public class Gifts
    {
        [System.Serializable]
        public enum Type
        {
            Gold,
            Card,
            Exp
        }
        public Type _type;
        [Range(0f, 100f)]
        public float _chance;
    }
    public Gifts[] _gifts;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator ChestFoundNumerator(GameObject Chest)
    {
        var Main = MainController.Instance;
        Main._scriptMovement.Moving = false;
        Main._scriptMovement._cantClick = true; // Bloquea clics por la batalla
        yield return new WaitForSeconds(1);
        Main._scriptBattle._chestPanel._panel.SetActive(true);
        Main._scriptBattle._chestPanel._giftText.text = Main._scriptChest.
            _gifts[Chest.GetComponent<ChestObjectController>()._giftID]._type.ToString();
        yield return new WaitForSeconds(3);
        Main._scriptBattle._chestPanel._panel.SetActive(false);
        Main._scriptMovement.Moving = true;
        Main._scriptMovement._cantClick = false; // Bloquea clics por la batalla

    }

    public int ChestGiftChoose()
    {
        // Validación de seguridad por si no hay regalos configurados
        if (_gifts == null || _gifts.Length == 0)
        {
            Debug.LogWarning("No hay regalos (_gifts) configurados en el cofre.");
            return -1; // Retorna -1 si hay un error o no hay elementos
        }

        // 1. Calcular el total de las probabilidades
        float totalChance = 0f;
        foreach (var gift in _gifts)
        {
            totalChance += gift._chance;
        }

        if (totalChance <= 0f)
        {
            Debug.LogWarning("El total de las probabilidades de los regalos es 0 o menor.");
            return -1;
        }

        // 2. Tirar el dado aleatorio entre 0 y el total de la probabilidad
        float roll = Random.Range(0f, totalChance);
        float cumulativeProbability = 0f;
        int selectedIndex = -1;

        for (int i = 0; i < _gifts.Length; i++)
        {
            cumulativeProbability += _gifts[i]._chance;
            if (roll <= cumulativeProbability)
            {
                selectedIndex = i;
                break;
            }
        }

        // Fallback por seguridad de redondeo flotante
        if (selectedIndex == -1)
        {
            selectedIndex = _gifts.Length - 1;
        }

        // 3. Regresa el índice numérico de la opción elegida
        return selectedIndex;
    }
}