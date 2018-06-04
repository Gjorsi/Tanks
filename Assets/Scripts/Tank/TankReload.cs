using UnityEngine;
using UnityEngine.UI;

public class TankReload : MonoBehaviour
{         
    public Slider m_Slider;                        
    public Image m_FillImage;
    public Color m_FullLoadColor = Color.black;  
    public Color m_ZeroLoadColor = Color.yellow;

    private float m_CurrentState;

    private void OnEnable()
    {
        SetLoadUI();
    }


    public void Reload(float timeStamp, float reloadTime)
    {
        while(Time.time < timeStamp + reloadTime)
        {
            m_CurrentState = (timeStamp + reloadTime - Time.time) / 2;
            SetLoadUI();
        }
            
    }


    private void SetLoadUI()
    {
        // Adjust the value and colour of the slider.
        m_Slider.value = m_CurrentState;

        m_FillImage.color = Color.Lerp(m_ZeroLoadColor, m_FullLoadColor, m_CurrentState);
    }
}