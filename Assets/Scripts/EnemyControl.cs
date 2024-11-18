using UnityEngine;
public class EnemyControl : MonoBehaviour
{
    GameManager gameManager = GameManager.Instance;
    public float speed;
    private bool isExploding = false;


    private void Start()
    {
        speed = gameManager.GetSpeed();

    }


    void Update()
    {
        if (!isExploding)
        {
            transform.position += speed * Time.deltaTime * Vector3.left;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameManager gameManager = GameManager.Instance;

        if (other.CompareTag("Player"))
        {
            SetIsExploding();
            if (this.CompareTag("Asteroid"))
            {
                
                Animator animator = this.GetComponent<Animator>();
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                float animationLength = stateInfo.length;
                animator.SetTrigger("Explode");
                StartCoroutine(gameManager.DestroyAfterAnimation(this.gameObject, animationLength));
            }
            
            gameManager.GameOverScreen(other.gameObject);

        }
    }
    public void SetIsExploding()
    {
        isExploding = true;
    }
}

