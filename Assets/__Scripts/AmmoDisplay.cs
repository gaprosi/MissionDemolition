
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class AmmoDisplay : MonoBehaviour
{
    public Slingshot slingshot;

    private Text ammoText;

    void Awake()
    {
        ammoText = GetComponent<Text>();
    }

    void Update()
    {
        if (slingshot == null) return;

        if (slingshot.useHeavyProjectile)
        {
            ammoText.text = "Ammo: HEAVY (2) | Normal (1)";
            ammoText.color = new Color(0.7f, 0.1f, 0.1f);
        }
        else
        {
            ammoText.text = "Ammo: NORMAL (1) | Heavy (2)";
            ammoText.color = Color.black;
        }
    }
}
