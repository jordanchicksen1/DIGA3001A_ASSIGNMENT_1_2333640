using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class teleporter : MonoBehaviour
{
    public GameObject player;
    public CharacterController characterController;
    public GameObject teleportationScreen;
    public GameObject sitText;

    public fuelManager fuelManager;
    public playerControls playerControls;

    public GameObject tpOriginalFlame;
    public GameObject tpAbyss;
    public GameObject tpEasternCell;
    public GameObject tpNorthernCell;
    public GameObject tpSouthernCell;
    public void OriginalFlame()
    {
        if(fuelManager.fuel > 0.99)
        {
            StartCoroutine(OriginalFlameTeleport());
        }
        
    }

    public IEnumerator OriginalFlameTeleport()
    {
        yield return new WaitForSeconds(0f);
        tpOriginalFlame.SetActive(true);
        characterController.enabled = false;
        player.transform.position = new Vector3(2.9f, 11.89f, -41.95f);
        yield return new WaitForSeconds(3f);
        tpOriginalFlame.SetActive(false);
        characterController.enabled = true;
        teleportationScreen.SetActive(false);
        playerControls.isUsingTeleportationMenu = false;
        sitText.SetActive(false);
        fuelManager.subtractFuel();
        playerControls.isInCampfireRange1 = false;
        playerControls.isInCampfireRange2 = false;
        playerControls.isInCampfireRange3 = false;
        playerControls.isInCampfireRange4 = false;
        playerControls.isInCampfireRange5 = false;
        playerControls.isInOriginalFlameRange = false;

        playerControls.increaseHope = false;
    }

    public void TheAbyss()
    {
        if (fuelManager.fuel > 0.99) 
        { 
            StartCoroutine(TheAbyssTeleport());
        }
            
    }

    public IEnumerator TheAbyssTeleport()
    {
        yield return new WaitForSeconds(0f);
        tpAbyss.SetActive(true);
        characterController.enabled = false;
        player.transform.position = new Vector3(-71.25f, 10.48f, -163.55f);
        yield return new WaitForSeconds(3f);
        tpAbyss.SetActive(false);
        characterController.enabled = true;
        teleportationScreen.SetActive(false);
        playerControls.isUsingTeleportationMenu = false;
        sitText.SetActive(false);
        fuelManager.subtractFuel();
        playerControls.isInCampfireRange1 = false;
        playerControls.isInCampfireRange2 = false;
        playerControls.isInCampfireRange3 = false;
        playerControls.isInCampfireRange4 = false;
        playerControls.isInCampfireRange5 = false;
        playerControls.isInOriginalFlameRange = false;

        playerControls.increaseHope = false;
    }

    public void EasternCell()
    {
        if (fuelManager.fuel > 0.99)
        {
            StartCoroutine(EasternCellTeleport());
        }
            
    }

    public IEnumerator EasternCellTeleport()
    {
        
        yield return new WaitForSeconds(0f);
        tpEasternCell.SetActive(true);
        characterController.enabled = false;
        player.transform.position = new Vector3(-89f, 24.67f, -92f);
        yield return new WaitForSeconds(3f);
        tpEasternCell.SetActive(false);
        characterController.enabled = true;
        teleportationScreen.SetActive(false);
        playerControls.isUsingTeleportationMenu = false;
        sitText.SetActive(false);
        fuelManager.subtractFuel();
        playerControls.isInCampfireRange1 = false;
        playerControls.isInCampfireRange2 = false;
        playerControls.isInCampfireRange3 = false;
        playerControls.isInCampfireRange4 = false;
        playerControls.isInCampfireRange5 = false;
        playerControls.isInOriginalFlameRange = false;

        playerControls.increaseHope = false;
    }

    public void NorthernCell()
    {
        if (fuelManager.fuel > 0.99)
        {
            StartCoroutine(NorthernCellTeleport());
        }
            
    }
    
    public IEnumerator NorthernCellTeleport()
    {
        yield return new WaitForSeconds(0f);
        tpNorthernCell.SetActive(true);
        characterController.enabled = false;
        player.transform.position = new Vector3(7.1f, 17.5f, -140.2f);
        yield return new WaitForSeconds(3f);
        tpNorthernCell.SetActive(false) ;
        characterController.enabled = true;
        teleportationScreen.SetActive(false);
        playerControls.isUsingTeleportationMenu = false;
        sitText.SetActive(false);
        fuelManager.subtractFuel();
        playerControls.isInCampfireRange1 = false;
        playerControls.isInCampfireRange2 = false;
        playerControls.isInCampfireRange3 = false;
        playerControls.isInCampfireRange4 = false;
        playerControls.isInCampfireRange5 = false;
        playerControls.isInOriginalFlameRange = false;

        playerControls.increaseHope = false;
    }

    public void SouthernCell()
    {
        if (fuelManager.fuel > 0.99)
        {
            StartCoroutine(SouthernCellTeleport());
        }
            
    }

    public IEnumerator SouthernCellTeleport()
    {
        yield return new WaitForSeconds(0f);
        tpSouthernCell.SetActive(true);
        characterController.enabled = false;
        player.transform.position = new Vector3(19.85f, -12.1f, -88.69f);
        yield return new WaitForSeconds(3f);
        tpSouthernCell.SetActive(false);
        characterController.enabled = true;
        teleportationScreen.SetActive(false);
        playerControls.isUsingTeleportationMenu = false;
        sitText.SetActive(false);
        fuelManager.subtractFuel();
        playerControls.isInCampfireRange1 = false;
        playerControls.isInCampfireRange2 = false;
        playerControls.isInCampfireRange3 = false;
        playerControls.isInCampfireRange4 = false;
        playerControls.isInCampfireRange5 = false;
        playerControls.isInOriginalFlameRange = false;

        playerControls.increaseHope = false;

    }

    public void ExitButton()
    {
        characterController.enabled = true;
        teleportationScreen.SetActive(false);
        playerControls.isUsingTeleportationMenu = false;
    }
}
