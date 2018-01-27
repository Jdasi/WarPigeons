using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PigeonAnimator : MonoBehaviour
{
    private Animator animator;
    private bool is_gliding = false;
    private float next_state_change = 0;

    [SerializeField] Vector2 flap_range = new Vector2(1, 3);
    [SerializeField] Vector2 glide_range = new Vector2(1, 3);


    // Use this for initialization
    void Start ()
	{
	    animator = GetComponent<Animator>();
	}
	

	// Update is called once per frame
	void Update ()
    {
        if (Time.time >= next_state_change)
        {
            is_gliding = !is_gliding;
            animator.SetBool("is_gliding", is_gliding);

            float random_delay = is_gliding ? Random.Range(glide_range.x, glide_range.y) : Random.Range(flap_range.x, flap_range.y);
            next_state_change = Time.time + random_delay;
        }
	}
}
