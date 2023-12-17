using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float smoothness;
    public float speed;
    public float jumpForce;
    public Transform feet;
    public AudioSource steps;

    [Header("Holding")]
    public float maxDistance;
	public float attractionForce;
	public float holdDistance;
    public float throwForce;
    public GameObject hoverIcon;
    public GameObject grabHint;
    public GameObject grabIcon;
    public GameObject throwHint;
    public AudioSource pickUpSound;
    public AudioSource dropSound;
    public AudioSource throwSound;
    public bool[] hovers;
    public bool[] grabs;

    [Header("Inventory")]
    public Inventory inventory;
    public GameObject stashHint;
    public Transform trashParent;
    public float dropDistance;

	private Rigidbody rb;
    Vector3 smoothedVel;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        int amountoftrash = FindObjectOfType<DistributeTrash>().amount;
        hovers = new bool[amountoftrash];
        grabs = new bool[amountoftrash];
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = new(Input.GetAxisRaw("Horizontal") * speed, rb.velocity.y, Input.GetAxisRaw("Vertical") * speed);
        smoothedVel = Vector3.Lerp(smoothedVel, input, smoothness);

        rb.velocity = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0) * smoothedVel;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.Raycast(feet.position, Vector3.down, .2f))
                rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
        }

        hoverIcon.SetActive(hovers.Any(n => n));
        grabHint.SetActive(hoverIcon.activeSelf);

        grabIcon.SetActive(grabs.Any(n => n));
        throwHint.SetActive(grabIcon.activeSelf);

        if (stashHint) stashHint.SetActive(hoverIcon.activeSelf && inventory.inventory.Count < inventory.numSlots);

        steps.volume = Mathf.Clamp01(input.sqrMagnitude);

        if (Input.GetKeyDown(KeyCode.Q))
            inventory.DropAll(Camera.main.transform.position + Camera.main.transform.rotation * Vector3.forward * dropDistance, trashParent); ;
    }
}
