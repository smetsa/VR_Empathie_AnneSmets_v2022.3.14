using UnityEngine;

public class RespawnOnExit : MonoBehaviour
{
    private Vector3 originalPosition; // Urspr�ngliche Position des Objekts
    public Collider respawnArea; // Der Trigger-Collider f�r den Bereich
    public float checkInterval = 0.5f; // Zeitintervall f�r die �berpr�fung in Sekunden

    void Start()
    {
        // Speichere die urspr�ngliche Position des Objekts beim Start
        originalPosition = transform.position;

        // Starte die regelm��ige �berpr�fung
        InvokeRepeating("CheckIfInArea", 0f, checkInterval);
    }

    private void CheckIfInArea()
    {
        // �berpr�fe, ob das Objekt nicht mehr innerhalb des Bereichs ist
        if (!respawnArea.bounds.Contains(transform.position))
        {
            // Respawn das Objekt
            Respawn();
        }
    }

    private void Respawn()
    {
        // Setze die Position des Objekts auf seine urspr�ngliche Position
        transform.position = originalPosition;
        // F�ge hier zus�tzliche Aktionen hinzu, die nach dem Respawn ausgef�hrt werden sollen
    }
}