using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;       
    public Rigidbody m_Shell;            
    public Transform m_FireTransform;    
    public Slider m_AimSlider;
    public Slider m_ReloadSlider;
    public AudioSource m_ShootingAudio;  
    public AudioClip m_ChargingClip;     
    public AudioClip m_FireClip;         
    public float m_MinLaunchForce = 15f; 
    public float m_MaxLaunchForce = 30f; 
    public float m_MaxChargeTime = 1f;
    public float m_ReloadTime = 2f;

    
    private string m_FireButton;         
    private float m_CurrentLaunchForce;  
    private float m_ChargeSpeed;         
    private bool m_Fired;
    private bool m_Charging;
    private float m_TimeStamp = 0f;


    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start()
    {
        m_FireButton = "Fire" + m_PlayerNumber;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }
    

    private void Update()
    {
        // Track the current state of the fire button and make decisions based on the current launch force.
        m_AimSlider.value = m_MinLaunchForce;

        if(m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
        {
            // holding fire button beyond max charge
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire();
        }
        else if(Input.GetButtonDown(m_FireButton))
        {
            // just started holding fire button
            m_Fired = false;

            if (Time.time < m_TimeStamp + m_ReloadTime)
            {
                m_Charging = true;
                return;
            }
            
            m_CurrentLaunchForce = m_MinLaunchForce;

            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play();
        }
        else if(Input.GetButton(m_FireButton) && !m_Fired && (Time.time > m_TimeStamp + m_ReloadTime))
        {
            // holding fire button, not yet fired

            if (m_Charging)
            {
                m_CurrentLaunchForce = m_MinLaunchForce;
                m_ShootingAudio.clip = m_ChargingClip;
                m_ShootingAudio.Play();
                m_Charging = false;
            }

            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;

            m_AimSlider.value = m_CurrentLaunchForce;
        }
        else if(Input.GetButtonUp(m_FireButton) && !m_Fired && (Time.time > m_TimeStamp + m_ReloadTime))
        {
            // button released, FIRE!
            Fire();
        }

    }


    private void Fire()
    {
        // Instantiate and launch the shell.
        m_TimeStamp = Time.time;
        m_Fired = true;
        //TankReload tankReload = GetComponentInChildren<TankReload>();
        //tankReload.Reload(m_TimeStamp, m_ReloadTime);

        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        m_CurrentLaunchForce = m_MinLaunchForce;
    }
}