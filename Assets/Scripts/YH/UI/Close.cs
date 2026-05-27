using UnityEngine;

public class Close : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void clickClose()
    {
        if (gameObject.name == "rankPanel")
        {
            Transform Content = transform.GetChild(1).GetChild(0).GetChild(0);
            for (int i = 0; i < Content.childCount; i++)
            {
                Destroy(Content.GetChild(i).gameObject);    
            }
            
        }
        transform.gameObject.SetActive(false);
    }
}
