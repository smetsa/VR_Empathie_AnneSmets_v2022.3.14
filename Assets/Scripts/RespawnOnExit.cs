using UnityEngine;

public class RespawnOnExit : MonoBehaviour
{
    private Vector3 originalPosition; // Ursprüngliche Position des Objekts
    public Collider respawnArea; // Der Trigger-Collider für den Bereich
    public float checkInterval = 0.5f; // Zeitintervall für die Überprüfung in Sekunden

    void Start()
    {
        // Speichere die ursprüngliche Position des Objekts beim Start
        originalPosition = transform.position;

        // Starte die regelmäßige Überprüfung
        InvokeRepeating("CheckIfInArea", 0f, checkInterval);
    }

    private void CheckIfInArea()
    {
        // Überprüfe, ob das Objekt nicht mehr innerhalb des Bereichs ist
        if (!respawnArea.bounds.Contains(transform.position))
        {
            // Respawn das Objekt
            Respawn();
        }
    }

    private void Respawn()
    {
        // Setze die Position des Objekts auf seine ursprüngliche Position
        transform.position = originalPosition;
        // Füge hier zusätzliche Aktionen hinzu, die nach dem Respawn ausgeführt werden sollen
    }
}