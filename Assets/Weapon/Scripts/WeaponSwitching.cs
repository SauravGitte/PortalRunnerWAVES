using UnityEngine;

public class WeaponSwitching : MonoBehaviour {
    public int selectedWeapon = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        selectWeapon();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
            if (selectedWeapon >= transform.childCount - 1) {
                selectedWeapon = 0;
            } else {
                selectedWeapon++;
            }
            selectWeapon();
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
            if (selectedWeapon <= 0) {
                selectedWeapon = transform.childCount - 1;
            } else {
                selectedWeapon--;
            }
            selectWeapon();
        }
    }

    void selectWeapon() {
        int i = 0;
        foreach (Transform weapon in transform) {
            if (i == selectedWeapon) {
                weapon.gameObject.SetActive(true);
            } else {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}