using UnityEngine;

public class PickUpMechanic : MonoBehaviour
{
   
    public float pickupRange = 3f;
    public Transform holdPoint;
    public LayerMask pickupMask;

    private GameObject heldObject;
    private Rigidbody heldRb;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
                TryPickup();
            else
                DropObject();
        }

        if (heldObject != null)
        {
            MoveObject();
        }
    }

    void TryPickup()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, pickupMask))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                heldObject = rb.gameObject;
                heldRb = rb;

                heldRb.useGravity = false;
                heldRb.linearDamping = 10f;
                heldRb.constraints = RigidbodyConstraints.FreezeRotation;

                heldObject.transform.parent = holdPoint;

                Collider heldCol = heldObject.GetComponent<Collider>();
                Collider playerCol= GetComponentInParent<Collider>();
                if(heldCol != null && playerCol != null)
                { Physics.IgnoreCollision(heldCol,playerCol,true); }
            }
        }
    }

    void MoveObject()
    {
        Vector3 moveDir = (holdPoint.position - heldObject.transform.position);
        heldRb.linearVelocity = moveDir * 50f;
    }

    void DropObject()
    {
        
        Collider heldCol = heldObject.GetComponent<Collider>();
        Collider playerCol = GetComponentInParent<Collider>();
        if (heldCol != null && playerCol != null)
            Physics.IgnoreCollision(heldCol, playerCol, false);
 
        heldRb.useGravity = true;
        heldRb.linearDamping = 1f;
        heldRb.constraints = RigidbodyConstraints.None;
       
        heldObject.transform.rotation = Quaternion.identity; 
        heldRb.angularVelocity = Vector3.zero; 
        
        RaycastHit hit;
        if (Physics.Raycast(heldObject.transform.position, Vector3.down, out hit, 1f)) 
        {
            
            heldObject.transform.position = hit.point; 
        }

        heldObject.transform.parent = null;

        heldObject = null;
        heldRb = null;
    }
}

