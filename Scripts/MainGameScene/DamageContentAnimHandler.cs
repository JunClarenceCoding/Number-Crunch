using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageContentAnimHandler : MonoBehaviour
{
    public Animator damageContentAnim;
    public Button backBtn, backBtn1, OpenBtn;
    
    void Start()
    {
        backBtn.gameObject.SetActive(false);
        backBtn1.gameObject.SetActive(false);
    }
    public void OpenContent()
    {
        OpenBtn.gameObject.SetActive(false);
        damageContentAnim.Play("OpenDamageContent");
        backBtn.gameObject.SetActive(true);
        backBtn1.gameObject.SetActive(true);
    }
    public void CloseContent()
    {
        StartCoroutine(CloseContentWithDelay());
    }
    private IEnumerator CloseContentWithDelay()
    {
        damageContentAnim.Play("CloseDamageContent");
        yield return new WaitForSeconds(0.4f);
        OpenBtn.gameObject.SetActive(true);
        backBtn.gameObject.SetActive(false);
        backBtn1.gameObject.SetActive(false);
    }
}