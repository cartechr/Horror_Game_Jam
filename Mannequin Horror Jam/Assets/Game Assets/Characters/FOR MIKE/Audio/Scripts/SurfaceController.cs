using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEditor;
using FMOD.Studio;

public class SurfaceController : MonoBehaviour
{

    enum Current_Terrain
    { Wood, Stone, Grit, Concrete, Carpet };

    [SerializeField] private Current_Terrain currentTerrain;

    private FMOD.Studio.EventInstance Footsteps;

    public float yOffset = 2.0f; // Adjust this value based on your desired offset
    public float raycastLength = 1.0f; // Adjust this value based on your desired ray length

    public FPSCONTROL playerController;

    private void Update()
    {
        DetermineTerrain();
        SwitchMoveType();
        

    }

    private void DetermineTerrain()
    {
        RaycastHit[] hit;

        hit = Physics.RaycastAll(transform.position + Vector3.up * yOffset, Vector3.down, raycastLength);

        foreach (RaycastHit rayhit in hit)
        {
            string layerName = LayerMask.LayerToName(rayhit.transform.gameObject.layer);
            Debug.Log("Hit layer: " + layerName);

            if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Wooden Floor"))
            {
                currentTerrain = Current_Terrain.Wood;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Stone Floor"))
            {
                currentTerrain = Current_Terrain.Stone;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Grit Floor"))
            {
                currentTerrain = Current_Terrain.Grit;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Concrete Floor"))
            {
                currentTerrain = Current_Terrain.Concrete;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Carpet Floor"))
            {
                currentTerrain = Current_Terrain.Carpet;
            }
        }
    }

    void SwitchMoveType()
    {
        if (playerController.isCrouching)
        {
            Footsteps.setParameterByNameWithLabel("MoveType", "Crouch");
        }
        else if (!playerController.isCrouching)
        {
            Footsteps.setParameterByNameWithLabel("MoveType", "Walk");
        }
        else if (playerController.isSprinting)
        {
            Footsteps.setParameterByNameWithLabel("MoveType", "Run");
        }


    }

    void PlayFootstep(int surfaceType)
    {
        Footsteps = FMODUnity.RuntimeManager.CreateInstance("event:/Character/Foley/Footsteps");
        Footsteps.setParameterByName("SurfaceType", surfaceType);
        Footsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        Footsteps.start();
        Footsteps.release();

    }

    public void SelectAndPlayFootstep()
    {
        switch (currentTerrain)
        {
            case Current_Terrain.Wood:
                PlayFootstep(4);
                break;

            case Current_Terrain.Stone:
                PlayFootstep(3);
                break;

            case Current_Terrain.Grit:
                PlayFootstep(2);
                break;

            case Current_Terrain.Concrete:
                PlayFootstep(1);
                break;

            case Current_Terrain.Carpet:
                PlayFootstep(0);
                break;

            default:
                PlayFootstep(0);
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; // Set the Gizmos color

       

        // Draw a ray in the Scene view to represent the downward raycast with Y-axis offset
        Vector3 rayStart = transform.position + Vector3.up * yOffset;
        Vector3 rayEnd = rayStart + Vector3.down * raycastLength;
        Gizmos.DrawRay(rayStart, Vector3.down * raycastLength);

        // Draw a sphere at the hit point of the raycast
        RaycastHit[] hits = Physics.RaycastAll(rayStart, Vector3.down, raycastLength);
        foreach (RaycastHit hit in hits)
        {
            Gizmos.DrawWireSphere(hit.point, 0.2f);
        }

        // Draw labels for each terrain type at the respective hit points
        foreach (RaycastHit hit in hits)
        {
            Vector3 labelPosition = hit.point + Vector3.up * 0.5f;

#if UNITY_EDITOR
            Handles.Label(labelPosition, hit.transform.gameObject.layer.ToString());
#endif
        }
    }

}
