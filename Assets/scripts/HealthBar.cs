using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private HealthSystem healthSystem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;
        this.healthSystem.onHealthChanged += HealthSystem_OnHealthChanged;
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e) 
    {
        transform.Find("Bar").localScale = new Vector3(this.healthSystem.GetHealthPercent(),1);
    }

    public void destroyThis()
    {
        Destroy(this);
    }
}
