using UnityEngine;

public class BearTrapHit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Animator animator;
    GameObject healthBar;
    public GameObject parentTrap;
    public AudioClip snapShutSound;
    private AudioSource audioSource;
    private GameObject playerHealth;
    Item item;
    
    void Start()
    {
        item = parentTrap.GetComponent<Item>();
        animator = parentTrap.GetComponent<Animator>();
        playerHealth = GameObject.FindWithTag("PlayerHealthBar");
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(item.itemType == ItemType.Trap)
        {
            if(animator.GetBool("isClosed") == false)
            {
                audioSource.PlayOneShot(snapShutSound);
                Debug.Log("Col: " + collision);
                Debug.Log("GO: " + collision.gameObject);
                Debug.Log("TAG: " + collision.gameObject.tag);
            }
            
            if (collision.gameObject.CompareTag("Player") && animator.GetBool("isClosed") == false)
            {
                Debug.Log("Player hit the trap!");
                animator.SetBool("isClosed", true);

                if(parentTrap != null)
                {
                    Debug.Log("Parent trap does " + item.healthValue + " damage");
                    playerHealth.GetComponent<HealthBar>().TakeDamage(item.healthValue);
                }
            }
            else if (collision.gameObject.CompareTag("Enemy") && animator.GetBool("isClosed") == false)
            {
                Debug.Log("Enemy hit the trap!");
                animator.SetBool("isClosed", true);
                EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
                enemyHealth.DecreaseHealth(item.healthValue);

                //This is where you would damage the enemy (once that system is added)
            }
        }
    }
    void Update()
    {

    }
}
