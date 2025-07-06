using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NPCScript : MonoBehaviour
{
    public string NpcName;
    public string ConvoContent;

    public TMP_Text txtNPCName, txtNPCContent;

    // Start is called before the first frame update
    void Start()
    {
        txtNPCContent.text = ConvoContent;
        txtNPCName.text = NpcName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
