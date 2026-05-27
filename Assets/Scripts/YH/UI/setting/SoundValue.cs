using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Sound : MonoBehaviour
{   
    void Update()
    {
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("{0:F0}",transform.GetComponent<Slider>().value *100);        
    }
}
