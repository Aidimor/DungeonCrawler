using System.Collections;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public Animator _battlePanelAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator BattleStarts()
    {
        MainController.Instance._onStation = 0;
        MainController.Instance._cinematicAnimator.SetBool("CinematicIn", true);

        yield return new WaitForSeconds(1);
        _battlePanelAnimator.SetBool("BattleIn", true);
    }
}
