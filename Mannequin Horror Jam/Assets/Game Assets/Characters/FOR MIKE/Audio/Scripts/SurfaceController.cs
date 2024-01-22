using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SurfaceController : MonoBehaviour
{

    enum Current_Terrain
    { Wood, Stone, Grit, Concrete, Carpet };

    [SerializeField] private Current_Terrain currentTerrain;
    FMOD.Studio.EventInstance Footsteps;


    private void DetermineTerrain()
    {
        RaycastHit[] hit;

        hit = Physics.RaycastAll(transform.position, Vector3.down, 10.0f);

        foreach (RaycastHit rayhit in hit)
        {
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

    void PlayFootstep(int terrain)
    {
        Footsteps = FMODUnity.RuntimeManager.CreateInstance("event:/Footsteps");
        Footsteps.setParameterByName("Terrain", terrain);
        Footsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        Footsteps.start();
        Footsteps.release();

    }

    public void SelectAndPlayFootstep()
    {
        switch (currentTerrain)
        {
            case Current_Terrain.Wood:
                PlayFootstep(1);
                break;

            case Current_Terrain.Stone:
                PlayFootstep(0);
                break;

            case Current_Terrain.Grit:
                PlayFootstep(2);
                break;

            case Current_Terrain.Concrete:
                PlayFootstep(3);
                break;

            case Current_Terrain.Carpet:
                PlayFootstep(3);
                break;

            default:
                PlayFootstep(0);
                break;
        }
    }

}
