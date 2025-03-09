using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class debugstamina : MonoBehaviour
{
    [SerializeField] private Gaman gaman;
    [SerializeField] private TextMeshProUGUI staminatext;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        staminatext.text = "dubug_stamina"+gaman.stamina.ToString();
    }
}
