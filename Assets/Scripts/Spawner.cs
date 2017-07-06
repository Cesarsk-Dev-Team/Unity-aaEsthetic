using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Spawner : MonoBehaviour {

    public GameObject pinPrefab;
    public static bool isInputEnabled = true;

    private void Update()
    {
        if (isInputEnabled)
        {
            if(!GameManager.isPaused) OnMouseDown();
        }
    }

    void OnMouseDown()
    {
        // Detect mouse event
        if (IsPointerOverUIObject())
        {
            Debug.Log("return mouse");
            return;
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            SpawnPin();
            GetComponent<AudioSource>().Play();
        }
    }


    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }


    private void SpawnPin()
    {
        Instantiate(pinPrefab, transform.position, transform.rotation);
    }

}
