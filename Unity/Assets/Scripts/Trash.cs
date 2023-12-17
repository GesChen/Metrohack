using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum TrashType
{
    Trash = 0,
    Recycle = 1,
    Glass = 2,
    Organic = 3
}
public class Trash : MonoBehaviour
{
    public TrashType type;
    public Sprite icon;
    public int id;
    bool beingHeld;
    Rigidbody rb;
    PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;
        bool didHit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, playerController.maxDistance);
        if (didHit)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform == transform)
                {
                    beingHeld = true;
                    playerController.pickUpSound.Play();
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && beingHeld)
        {
            beingHeld = false;
            playerController.dropSound.Play();
        }

        if (beingHeld)
        {
            Vector3 targetPoint = Camera.main.transform.position + Camera.main.transform.rotation * Vector3.forward * playerController.holdDistance;
            Vector3 difference = targetPoint - transform.position;
            rb.velocity = difference.normalized * playerController.attractionForce * difference.magnitude;

            if (Input.GetMouseButtonDown(1))
            {
                beingHeld = false;
                rb.velocity += Camera.main.transform.rotation * Vector3.forward * playerController.throwForce;
                playerController.throwSound.Play();
            }
        }

        playerController.hovers[id] = didHit && hit.transform == transform && !beingHeld;
        playerController.grabs[id] = beingHeld;

        if (transform.position.y < -1)
        {
            transform.position += Vector3.up * 3;
            rb.velocity = Vector3.zero;
        }
        if (Input.GetKeyDown(KeyCode.E) && hit.transform == transform && playerController.inventory.inventory.Count < playerController.inventory.numSlots)
        {
            playerController.hovers[id] = false;
            playerController.grabs[id] = false;

            playerController.inventory.Stash(gameObject);
            gameObject.SetActive(false);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (new string[] { "Trash Can", "Recycling Bin", "Glass Bin", "Composter" }.Contains(collision.transform.tag))
        {
			playerController.grabs[id] = false;
			playerController.hovers[id] = false;
			switch (collision.transform.tag)
			{
				case "Trash Can":
					FindObjectOfType<GameManager>().Score(this, TrashType.Trash);
					break;
				case "Recycling Bin":
					FindObjectOfType<GameManager>().Score(this, TrashType.Recycle);
					break;
				case "Glass Bin":
					FindObjectOfType<GameManager>().Score(this, TrashType.Glass);
					break;
				case "Composter":
					FindObjectOfType<GameManager>().Score(this, TrashType.Organic);
                    break;
			}

            Destroy(gameObject);
		}
        
	}
}
