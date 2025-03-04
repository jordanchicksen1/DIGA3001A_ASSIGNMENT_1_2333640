using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class teleporter : MonoBehaviour
{
    public GameObject player;
    public CharacterController characterController;
    public GameObject teleportationScreen;
    public GameObject sitText;


    public playerControls playerControls;
    public void OriginalFlame()
    {
        StartCoroutine(OriginalFlameTeleport());
    }

    public IEnumerator OriginalFlameTeleport()
    {
        yield return new WaitForSeconds(0f);
        characterController.enabled = false;
        player.transform.position = new Vector3(2.9f, 11.89f, -39.5f);
        yield return new WaitForSeconds(0.01f);
        characterController.enabled = true;
        teleportationScreen.SetActive(false);
        playerControls.isUsingTeleportationMenu = false;
        sitText.SetActive(false);

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
        StartCoroutine(TheAbyssTeleport());
    }

    public IEnumerator TheAbyssTeleport()
    {
       
        yield return new WaitForSeconds(0f);
        characterController.enabled = false;
        player.transform.position = new Vector3(-71.25f, 10.48f, -163.55f);
        yield return new WaitForSeconds(0.01f);
        characterController.enabled = true;
        teleportationScreen.SetActive(false);
        playerControls.isUsingTeleportationMenu = false;
        sitText.SetActive(false);

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
        StartCoroutine(EasternCellTeleport());
    }

    public IEnumerator EasternCellTeleport()
    {
        yield return new WaitForSeconds(0f);
        characterController.enabled = false;
        player.transform.position = new Vector3(-89f, 24.67f, -92f);
        yield return new WaitForSeconds(0.01f);
        characterController.enabled = true;
        teleportationScreen.SetActive(false);
        playerControls.isUsingTeleportationMenu = false;
        sitText.SetActive(false);

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
        StartCoroutine(NorthernCellTeleport());
    }
    
    public IEnumerator NorthernCellTeleport()
    {
        yield return new WaitForSeconds(0f);
        characterController.enabled = false;
        player.transform.position = new Vector3(7.1f, 17.5f, -140.2f);
        yield return new WaitForSeconds(0.01f);
        characterController.enabled = true;
        teleportationScreen.SetActive(false);
        playerControls.isUsingTeleportationMenu = false;
        sitText.SetActive(false);

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
        StartCoroutine(SouthernCellTeleport());
    }

    public IEnumerator SouthernCellTeleport()
    {
        yield return new WaitForSeconds(0f);
        characterController.enabled = false;
        player.transform.position = new Vector3(19.85f, -12.1f, -88.69f);
        yield return new WaitForSeconds(0.01f);
        characterController.enabled = true;
        teleportationScreen.SetActive(false);
        playerControls.isUsingTeleportationMenu = false;
        sitText.SetActive(false);

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
